using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_Server
{
    public class FileIO
    {
        //Declare the directory where we store the games, create instance of random class, declare the static list of file names to randomly choose from.
        private static readonly string GameDirectory = "Games";
        private static readonly Random random = new Random();
        private static readonly List<string> files = new List<string>
        {
            "puzzle1.txt", "puzzle2.txt", "puzzle3.txt", "puzzle4.txt"
        };

    }
}
