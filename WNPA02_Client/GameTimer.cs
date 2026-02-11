using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_SharedClassLibrary
{
    public static class GameTimer
    {
        private static readonly TimeSpan GameDuration = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["gameTimer"]));
        public static Stopwatch clientWatch;

        private static void StartStopWatch()
        {
            //Start the stopwatch.
            clientWatch = Stopwatch.StartNew();
        }

        public static string GetTimeRemaining()
        {
            //If its null, return it to have the game duration value in the config file formatted to mm:hh
            if (clientWatch == null)
                return GameDuration.ToString(@"mm\:ss");

            //To find the time remaining, subtract the ceiling from the time elapsed in the stopwatch
            TimeSpan remaining = GameDuration - clientWatch.Elapsed;

            //If user runs out of time, return 0
            if (remaining <= TimeSpan.Zero)
                return "00:00";

            //Return the current time left
            return remaining.ToString(@"mm\:ss");
        }

    }




}
