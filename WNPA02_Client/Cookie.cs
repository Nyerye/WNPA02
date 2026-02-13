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
    public class Cookie
    {

        private Guid sessionid;
        private string puzzlestring;
        private string wordsguessed;
        private string wordsleft;
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cookie.json");

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

        public static void WriteCookieToFile(Cookie cookie)
        {
            string json = JsonConvert.SerializeObject(cookie, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static Cookie ReadCookieFromFile()
        {
            if (!File.Exists(FilePath))
                return null;

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Cookie>(json);
        }
    }
}
