

using System.Configuration;

namespace WNPA02_Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string configIp = ConfigurationManager.AppSettings["serverHost"];
            string configPort = ConfigurationManager.AppSettings["serverPort"];
            Console.WriteLine($"Server online at {configIp}:{configPort}");
            await GameMaster.Listen();
        }
    }
}