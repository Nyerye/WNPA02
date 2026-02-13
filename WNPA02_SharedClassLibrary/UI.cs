/// <file>
/// UI.cs
/// </file>
/// <project>
/// Windows Network Programming Assignment 2
/// </project>
/// <author>
/// Nicholas Reilly
/// </author>
/// <date>
/// February 13 2026
/// </date>
/// <description>
/// Class file that holds the UI methods.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
///

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_SharedClassLibrary
{
    /// <summary>
    /// UI Class that has the Print method
    /// </summary>
    public static class UI
    {
        /// <summary>
        /// Simply takes a string and displays it on the Console.
        /// </summary>
        /// <param name="message"></param>
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
