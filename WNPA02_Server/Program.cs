/// <file>
/// Program.cs
/// </file>
/// <project>
/// Windows Network Programming Assignment 2
/// </project>
/// <author>
/// Nicholas Reilly
/// </author>
/// <date>
/// February 12 2026
/// </date>
/// <description>
/// Class that holds Main, which starts the Listener and waits for it to end (as of now, it never does. Server should always allow new connections).
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
/// 
using System.Configuration;

namespace WNPA02_Server
{
    internal class Program
    {
        /// <summary>
        /// Main method that calls the Listen method.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            string configIp = ConfigurationManager.AppSettings["serverHost"];
            string configPort = ConfigurationManager.AppSettings["serverPort"];
            Console.WriteLine($"Server online at {configIp}:{configPort}");
            await GameMaster.Listen();
        }
    }
}