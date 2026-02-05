using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;




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

                //Code that will spin up a task.
                //Calls the HandleClient method maybe in a task
            }
        }

        internal static void HandleClient(TcpClient client)
        {
            //Figure out where am I being tried to reached from.
            IPEndPoint remote = (IPEndPoint)client.Client.RemoteEndPoint;

            //Something with the protocol here. Build on it so its got the send and receive info. 

            //Check the incoming message to see what the user wants based on the header

            //Act accordingly

            //Send payload back over here or another method

        }

    }
}
