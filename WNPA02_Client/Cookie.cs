
/// <file>
/// Cookie.cs
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
/// File that contains the Cookie class, constructors, data members and methods.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;

namespace WNPA02_Client
{
    /// <summary>
    /// Cookie class. Helps restore sessions when users disconnect part way.
    /// </summary>
    public class Cookie
    {
        //Private data members.
        private Guid sessionid;
        private string puzzlestring;
        private string wordsguessed;
        private string wordsleft;
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cookie.json");

        /// <summary>
        /// Empty constructor. Required for proper deserialization for JSON.
        /// </summary>
        public Cookie()
        {

        }

        /// <summary>
        /// COnstructor used to create an instance of a Cookie in the MainWindow.xaml.cs
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="puzzlestring"></param>
        /// <param name="wordsguessed"></param>
        /// <param name="wordsleft"></param>
        public Cookie(Guid sessionid, string puzzlestring, string wordsguessed, string wordsleft)
        {
            this.sessionid = sessionid;
            this.puzzlestring = puzzlestring;
            this.wordsguessed = wordsguessed;
            this.wordsleft = wordsleft;
        }

        public Guid SessionID
        {
            get
            {
                return sessionid;
            }
            set
            {
                sessionid = value;
            }
        }

        public string WordsGuessed
        {
            get
            {
                return wordsguessed;
            }
            set
            {
                wordsguessed = value;
            }
        }

        public string WordsLeft
        {
            get
            {
                return wordsleft;
            }

            set
            {
                wordsleft = value;
            }
        }

        public string PuzzleString
        {
            get
            {
                return puzzlestring;
            }
            set
            {
                puzzlestring = value;
            }
        }

        /// <summary>
        /// Method that is used to write a cookie to a file. Happens after the user makes their first guess.
        /// </summary>
        /// <param name="cookie"></param>
        public static void WriteCookieToFile(Cookie cookie)
        {
            string json = JsonConvert.SerializeObject(cookie, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Method that reads a cookie from the client side.
        /// </summary>
        /// <returns></returns>
        public static Cookie ReadCookieFromFile()
        {
            //If its not there, return null.
            if (!File.Exists(FilePath))
                return null;

            //Return the cookie made from the JSON file.
            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Cookie>(json);
        }
    }
}
