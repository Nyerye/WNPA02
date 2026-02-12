/// <file>
/// GameLogic.cs
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
/// FIle that holds the methods that control game processes and the GameData struct.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
///
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WNPA02_SharedClassLibrary
{
   
    /// <summary>
    /// Declaration of the GameData struct.
    /// </summary>
    public struct GameData
    {
        public Guid SessionID;
        public string command;
        public bool GuessCorrect;
        public DateTime startTime;
        public int wordsLeft;
        public int gameVersion;
        public bool isGameOver;
        public string wordGuessed;
        public string message;
        public Puzzle puzzle;
    }

    
    /// <summary>
    /// Class that holds the methods to modify the GameData structs, read them and write them.
    /// </summary>
    public static class GameLogic
    {
        /// <summary>
        /// Method that updates a game by taking it the current struct with its corresponding values.
        /// Looks at whether a guess is valid, and then if valid, whether it is a match to the answer key.
        /// If it is, decrement the right words counter and remove the word from the guess pool.
        /// </summary>
        /// <param name="gameData"></param>
        /// <returns>
        /// The modified GameData struct.
        /// </returns>
        public static GameData UpdateGame(GameData gameData)
        {
            //In case somehow a transmission of data comes through after the game is already over, 
            //check to see if the game is already over and if it is, return the game data with a message saying so without making any changes to the game data.
            if (gameData.isGameOver)
            {
                gameData.message = "Game is already over.";
                return gameData;
            }

            //Have the control loop based on the guess being a valid string and not null or whitespace.
            gameData.wordGuessed = gameData.wordGuessed?.Trim();

            //Check to see if the guess is valid
            gameData.GuessCorrect = Puzzle.IsValidGuess(gameData.wordGuessed, gameData.puzzle);

            //If the guess is valid being a string without being null or having whitespace, and the guess exists as an answer, update fields.
            if (gameData.GuessCorrect && gameData.puzzle.Words.Contains(gameData.wordGuessed))
            {
                gameData.wordsLeft--;
                gameData.puzzle.Words.Remove(gameData.wordGuessed);

                //After removing an option, make sure that was not the last one needed to win. 
                if (gameData.wordsLeft <= 0)
                {
                    gameData.isGameOver = true;
                    gameData.message = "Congratulations! You've guessed all the words!";
                }
                else
                {
                    gameData.message = "Correct Guess!";
                }
            }
            else
            {
                gameData.message = "Incorrect Guess!";
            }

            //Return the updated struct with the new game data.
            return gameData;
        }

        /// <summary>
        /// Method that sets the true/false flag on whter the game is over.
        /// Will send a message back to the user to display on client side.
        /// </summary>
        /// <param name="gameData"></param>
        /// <returns>
        /// The modified GameData struct.
        /// </returns>
        public static GameData EndGame(GameData gameData)
        {
            gameData.isGameOver = true;
            gameData.message = "Game Over!";
            return gameData;
        }

       /// <summary>
       /// Method that receives data from a TcpCLient stream and transform the JSON Data back into a GameData struct.
       /// Reads up to 1024 bytes at a time and uses the Newtonsoft's JSONConvert class to deserialize the JSON into a GameData struct.
       /// </summary>
       /// <param name="stream"></param>
       /// <returns></returns>
        public static GameData ReceiveData(NetworkStream stream)
        {
            //Read up to 1024 bytes at a time from the stream.
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            //Convert into JSON and rebuild the struct on the other end.
            string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            GameData data = JsonConvert.DeserializeObject<GameData>(jsonData);

            return data;
        }

        /// <summary>
        /// Method that uses the TcpClient stream to send the fully modified GameData struct.
        /// Uses the Newtonsoft JSONConvert class to serialize it into a JSON string and send it across the netowrk to its recipiant.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        public static void SendData(NetworkStream stream, GameData data)
        {
            //Convert the struct into JSON and then into bytes to send over the stream. Go until we hit a newline character to signify the end of the message.
            string jsonData = JsonConvert.SerializeObject(data)+"\n";
            byte[] buffer = Encoding.UTF8.GetBytes(jsonData);

            //Send it wherever its going.
            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Method that will read in a game session file.
        /// Takes the GUID that represents the persons session id from the client and looks for a corresponding file.
        /// If its not found, you get nothing. If it is found, loads and transforms the JSON data into a GameData struct for processing and or sending.
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static GameData LoadGameData(Guid sessionID)
        {
            //Check to see if the SessionID is empty before trying to load. If it is, throw an exception.
            if (sessionID == Guid.Empty)
            {
                throw new ArgumentException("SessionID cannot be empty.");
            }

            //Look for a JSON file named after the SessionID in the sessions directory. 
            string sessionsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sessions");
            string filePath = Path.Combine(sessionsDir, sessionID.ToString() + ".json");

            //If it does not exist, make a barebones GameData struct with the SessionID and a message saying the session was not found, and return it.
            if (!File.Exists(filePath))
            {
                GameData notFound = new GameData
                {
                    SessionID = sessionID,
                    message = "Session not found."
                };
                return notFound;
            }

            //Read the JSON file and convert it back into a GameData struct, then return it.
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GameData>(json);

        }

        /// <summary>
        /// Method that saves a games session data.
        /// Takes the incoming GameData struct and looks at the SessionID value. If its not valid, it does not save and throws an error. Saves if valid.
        /// Creates a directory for the save files if once does not exist called sessions.
        /// Serializes the GameData struct as a JSON file and saves it to the file.
        /// </summary>
        /// <param name="gameData"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void SaveGameData(GameData gameData)
        {
            //Check to see if the SessionID is set before trying to save. If not, throw an exception.
            if (gameData.SessionID == Guid.Empty)
            {
                throw new ArgumentException("SessionID must be set before saving.");
            }
                
            //Create a directory for the sessions if it doesn't exist and then save the game data as a JSON file named after the SessionID.
            string sessionsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sessions");
            Directory.CreateDirectory(sessionsDir);

            //Save the game data as a JSON file named after the SessionID.
            string filePath = Path.Combine(sessionsDir, gameData.SessionID.ToString() + ".json");
            string json = JsonConvert.SerializeObject(gameData);
            File.WriteAllText(filePath, json);
        }




    }
}

