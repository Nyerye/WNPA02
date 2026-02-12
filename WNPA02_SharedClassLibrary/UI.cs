using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_SharedClassLibrary
{
    public static class UI
    {
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(string message)
        {
            StreamWriter logWritter = new StreamWriter("C:\\Logs",true);
            logWritter.WriteLine(message);
        }
    }
}
