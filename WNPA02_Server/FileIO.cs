using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WNPA02_SharedClassLibrary;

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

        public static Puzzle LoadRandomPuzzle()
        {
            //Pick a random index from the list.
            int randomIndex = random.Next(files.Count);
            string selectedFileName = files[randomIndex];

            //Build the full path.
            string filePath = Path.Combine(GameDirectory, selectedFileName);

            //If the file does not exist, throw an exception.
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Puzzle file not found: {filePath}");

            //Read the file.
            string[] lines = File.ReadAllLines(filePath);
            string puzzleString = lines[0];
            int wordCount = int.Parse(lines[1]);

            //Add it to the hashset ignoring case as it does not matter in the context of the game. Only matters if an actual word match is made.
            HashSet<string> validWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            //Add all words starting from line 3.
            for (int i = 2; i < lines.Length; i++)
            {
                validWords.Add(lines[i].Trim());
            }

            //Write to the log it was successful.
            UI.Log($"Loaded puzzle: {puzzleString} with {wordCount} words from {selectedFileName}");

            return new Puzzle(puzzleString, wordCount, validWords);
        }

    }
}
