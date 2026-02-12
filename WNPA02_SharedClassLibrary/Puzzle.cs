using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_SharedClassLibrary
{
    /// <summary>
    /// Puzzle class. Object that will hold the read information from 1/4 random files and can be used by the program to reference client guesses
    /// </summary>
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
                words = value;
            }
        }

        /// <summary>
        /// Constructor for the Puzzle class.
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
            if (string.IsNullOrWhiteSpace(guess))
            {
                return false;
            }
            else
            {
                //Returns the lowercase word to our case insensitive hashset with any whitespasce trimmed off.
                return puzzle.Words.Contains(guess.Trim().ToLower());
            }
            
        }

    }
}
