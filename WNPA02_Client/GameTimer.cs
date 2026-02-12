
/// <file>
/// GameTimer.cs
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
/// File that holds the client side class that counts down a game session until timeout.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
/// 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_SharedClassLibrary
{
    /// <summary>
    /// GameTimer class that has the methods for starting a stopwatch, checking if the time has ran out, and handling appropriatly if it has ran out.
    /// </summary>
    public static class GameTimer
    {
        //Read the time from the App.config file and initialize the Stopwatch variable.
        private static readonly TimeSpan GameDuration = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["gameTimer"]));
        public static Stopwatch clientWatch;

        /// <summary>
        /// Starts a stopwatch when called on. 
        /// </summary>
        private static void StartStopWatch()
        {
            //Start the stopwatch.
            clientWatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Gets the time remaining live by subtracting the stopwatchs time from the GameDuration value taken from the App.config file.
        /// </summary>
        /// <returns></returns>
        public static string GetTimeRemaining()
        {
            //If its null, return it to have the game duration value in the config file formatted to mm:hh.
            if (clientWatch == null)
                return GameDuration.ToString(@"mm\:ss");

            //To find the time remaining, subtract the ceiling from the time elapsed in the stopwatch.
            TimeSpan remaining = GameDuration - clientWatch.Elapsed;

            //If user runs out of time, return 0.
            if (remaining <= TimeSpan.Zero)
                return "00:00";

            //Return the current time left.
            return remaining.ToString(@"mm\:ss");
        }

    }




}
