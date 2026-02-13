/// <file>
/// Puzzle.cs
/// </file>
/// <project>
/// Windows Desktop Programming Assignment 2
/// </project>
/// <author>
/// Nicholas Reilly
/// </author>
/// <date>
/// Februrary 13 2026
/// </date>
/// <description>
/// Class file that holds the Puzzle constructors, data members and methods.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
///
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_SharedClassLibrary
{
    /// <summary>
    /// Puzzle class. Object that will hold the read information from 1/4 random files and can be used by the program to reference client guesses
    /// </summary>
    /// 
    public class Puzzle
    {
        //Set the private versions of the data members and then create their public versions.
        private string puzzlestring;
        private int wordcount;
        private HashSet<string> words;



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

        public int WordCount
        {
            get
            {
                return wordcount;
            }

            set
            {
                wordcount = value;
            }
        }

        public HashSet<string> Words
        {
            get
            {
                return words;
            }

            set
            {
                //Had to add this. When the JSON rebuilds, we lose the case insentivity from the initial load in FileIO. So this allows any, ANY and Any to work.
                words = new HashSet<string>(value, StringComparer.OrdinalIgnoreCase);
            }
        }
        
        /// <summary>
        /// This is an empty constructor for when the JSON needs to deserialize the GameData struct. This allows for proper transformation.
        /// </summary>
        public Puzzle()
        {

        }

        /// <summary>
        /// Constructor for the Puzzle class used in making it from the FileIO class' LoadPuzzle method
        /// </summary>
        /// <param name="puzzleString"></param>
        /// <param name="wordCount"></param>
        /// <param name="words"></param>
        public Puzzle(string puzzleString, int wordCount, HashSet<string> words)
        {
            this.puzzlestring = puzzleString;
            this.wordcount = wordCount;
            this.words = words;
        }

        /// <summary>
        /// Method that determines whether a guess is valid or not.
        /// </summary>
        /// <param name="guess"></param>
        /// <param name="puzzle"></param>
        /// <returns>
        /// The guess in its lower case form if its valid, false if its empty or whitespace.
        /// </returns>
        public static bool IsValidGuess(string guess, Puzzle puzzle)
        {
            //Check to see if its null or whitespace
            if (string.IsNullOrWhiteSpace(guess))
                return false;

            //Check to see if the puzzle's word list is null. If it is, we can't check the guess against it, so return false.
            if (puzzle.Words == null)
                return false;

            //Since its past the two checks, trim it of whitespace and get the true/false back from the Contains check.
            guess = guess.Trim();
            return puzzle.Words.Contains(guess);
        }
    }
}
