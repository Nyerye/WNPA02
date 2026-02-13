/// <file>
/// GameMaster.cs
/// </file>
/// <project>
/// Windows Network Programming Assingment 2
/// </project>
/// <author>
/// Nicholas Reilly
/// </author>
/// <date>
/// Febraury 12 2026
/// </date>
/// <description>
/// File that holds the Server side code for the project.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
/// 

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
        //Create the configIp and configPort strings the server will use from the values from the App.config file.
        private static readonly string configIp = ConfigurationManager.AppSettings["serverHost"];
        private static readonly string configPort = ConfigurationManager.AppSettings["serverPort"];

        /// <summary>
        /// Method that parses the string for an IP Address the server will use to create a listener.
        /// </summary>
        /// <param name="someIp"></param>
        /// <returns>
        /// An IPAddress data type from the transformed string.
        /// </returns>
        internal static IPAddress ParseIP(string someIp)
        {
            IPAddress serverIp = IPAddress.Parse(someIp);
            return serverIp;
        }

        /// <summary>
        /// Method that takes the port string from the App.config file and transforms it.
        /// </summary>
        /// <param name="somePort"></param>
        /// <returns>
        /// an integer data type with the transformed port number.
        /// </returns>
        internal static int ParsePort(string somePort)
        {
            int serverPort = int.Parse(somePort);
            return serverPort;
        }

        /// <summary>
        /// Method that starts the Server's listener to accept client connections and fire HandleClients task.
        /// This allows the Listener to really focus on just accepting connections and sneidng them off to be processed.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Method that determines what to do with the incoming client data based on the command argument it gives.
        /// Uses a switch case that determines what to do. 
        /// </summary>
        /// <param name="client"></param>
        internal static void HandleClient(TcpClient client)
        {
            try
            {
                //Release the resource and use the network stream client used.
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    //Receive the incoming message packet from the client
                    GameData incoming = GameLogic.ReceiveData(stream);

                    //Initlaize the outgoing response back to the client
                    GameData outgoing;

                    //Determine what to do based on the packets command value
                    switch (incoming.command)
                    {
                        case "START":
                            outgoing = incoming;
                            outgoing.puzzle = FileIO.LoadRandomPuzzle();
                            outgoing.SessionID = Guid.NewGuid();
                            outgoing.isGameOver = false;
                            outgoing.wordsLeft = outgoing.puzzle.Words.Count;
                            outgoing.message = $"Game started. String is {outgoing.puzzle.PuzzleString}";
                            GameLogic.SaveGameData(outgoing);
                            GameLogic.SendData(stream, outgoing);
                            break;
                        case "GUESS":
                            outgoing = GameLogic.LoadGameData(incoming.SessionID);
                            outgoing.wordGuessed = (incoming.wordGuessed.Trim());
                            outgoing = GameLogic.UpdateGame(outgoing);
                            GameLogic.SaveGameData(outgoing);
                            GameLogic.SendData(stream, outgoing);
                            break;
                        case "RESUME":
                            outgoing = GameLogic.LoadGameData(incoming.SessionID);
                            outgoing.message = "Game Resumed!";
                            GameLogic.SendData(stream, outgoing);
                            break;
                        case "END":
                            outgoing = GameLogic.LoadGameData(incoming.SessionID);
                            outgoing = GameLogic.EndGame(incoming);
                            GameLogic.SaveGameData(outgoing);
                            GameLogic.SendData(stream, outgoing);
                            break;
                        default:
                            outgoing = incoming;
                            outgoing.message = "Unknown command received. No action taken.";
                            Logger.Log($"Unknown command received from client: {incoming.command}");
                            GameLogic.SendData(stream, outgoing);
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error handling client: {ex.Message}");

            }

        }

    }
}
