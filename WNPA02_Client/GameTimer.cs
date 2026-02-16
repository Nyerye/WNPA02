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
using System.Windows;
using System.Windows.Threading;

namespace WNPA02_SharedClassLibrary
{
    /// <summary>
    /// GameTimer class that has the methods for starting a stopwatch, checking if the time has ran out, and handling appropriatly if it has ran out.
    /// </summary>
    public static class GameTimer
    {
        //Read the time from the App.config file and initialize the Stopwatch variable.
        private static readonly TimeSpan GameDuration = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["gameTimer"]));
        private static TimeSpan timeRemaining;
        public static DispatcherTimer clientTimer; //Reference for where I learned about these: https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatchertimer?view=windowsdesktop-10.0


        /// <summary>
        /// Starts a timer when called on
        /// </summary>
        public static void StartTimer()
        {
            //Initialize the remaining time from the config value.
            timeRemaining = GameDuration;
            
            //Create a new DispatcherTimer. Set intervals for firing to be every second. Subscribe to the event and start it.
            clientTimer = new DispatcherTimer();
            clientTimer.Interval = TimeSpan.FromSeconds(1);
            clientTimer.Tick += ClientTimer_Tick;
            clientTimer.Start();
        }

        /// <summary>
        /// Stops timer when called on.
        /// </summary>
        public static void StopTimer()
        {
            clientTimer.Stop();
        }

        /// <summary>
        /// Reduces the timer by 1 every second (decreases time by one second live on client side)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ClientTimer_Tick(object sender, EventArgs e)
        {
            //Subtract one second from the remaining time on each tick.
            timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));

            //If the timer has reached zero, stop it.
            if (timeRemaining <= TimeSpan.Zero)
            {
                clientTimer.Stop();
                timeRemaining = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Returns the remaining time in mm:ss format for UI updates.
        /// </summary>
        public static string GetTimeRemaining()
        {
            return timeRemaining.ToString(@"mm\:ss");
        }

        /// <summary>
        /// Checks to see if the timer has ran out.
        /// </summary>
        public static bool IsTimeUp()
        {
            return timeRemaining <= TimeSpan.Zero;
        }
    }
}
