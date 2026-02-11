using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WNPA02_SharedClassLibrary;




namespace WNPA02_Server
{
    internal class GameMaster
    {
        private static readonly string configIp = ConfigurationManager.AppSettings["serverHost"];
        private static readonly string configPort = ConfigurationManager.AppSettings["serverPort"];

        internal static IPAddress ParseIP(string someIp)
        {
            IPAddress serverIp = IPAddress.Parse(someIp);
            return serverIp;
        }

        internal static int ParsePort(string somePort)
        {
            int serverPort = int.Parse(somePort);
            return serverPort;
        }
        internal static async Task Listen()
        {
            //Parse both the IP Address and the Port for the server from the application config file.
            IPAddress serverIP = ParseIP(configIp);
            int serverPort = ParsePort(configPort);

            //Build the TcpListener 
            TcpListener listener = new TcpListener(serverIP, serverPort);

            //Start the TcpListener
            listener.Start();

            //While loop to forever take client connections
            while (true)
            {
                //Stop and wait for one of the clients to connect and trigger using ConnectAsync
                TcpClient client = await listener.AcceptTcpClientAsync();

                //Once a client connects, start a new task to handle the client connection and pass the client object to the method that will handle the client connection.
                //Omg Norbert. What's this? Is that... a discard? Uh oh. I know you didnt teach me it. So why do I have it?
                //I do not want the task return value, so this lets me ignore it without a warning. I just want to fire and forget this task, so I dont care about the return value.
                //Here's your link so we dont get hit with academic integrity since I'm the last person at this institution who gives a damn.
                //https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/discards
                _ = Task.Run(() => HandleClient(client));
            }
        }

        internal static void HandleClient(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    //Receive the incoming message packet from the client
                    GameData incoming = GameLogic.ReceiveGameData(stream);

                    //Initlaize the outgoing response back to the client
                    GameData outgoing;

                    //Determine what to do absed on the packets command value
                    switch (incoming.command)
                    {
                        case "START":
                            outgoing = GameLogic.SendToClient(incoming);
                            //Method here to read in the data
                            //Method here to append some metrics to the outgoing
                            GameLogic.SaveGameData(outgoing);
                            outgoing.message = "Game Started!";
                            break;
                        case "GUESS":
                            outgoing = GameLogic.UpdateGame(incoming);
                            GameLogic.SaveGameData(outgoing);
                            break;
                        case "RESUME":
                            outgoing = GameLogic.LoadGameData(incoming.SessionID);
                            outgoing.message = "Game Resumed!";
                            break;
                        case "END":
                            outgoing = GameLogic.EndGame(incoming);
                            GameLogic.SaveGameData(outgoing);
                            break;
                        default:
                            UI.Log($"Unknown command received from client: {incoming.command}");
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                UI.Log($"Error handling client: {ex.Message}");

            }

        }

    }
}
