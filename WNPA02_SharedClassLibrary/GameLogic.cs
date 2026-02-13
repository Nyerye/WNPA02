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
/// File that holds the methods that control game processes and the GameData struct.
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
///
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        public int wordsLeft;
        public bool isGameOver;
        public string wordGuessed;
        public string message;
        public string puzzleString;
        public int puzzleWordCount;
        public Puzzle puzzle;
    }


    /// <summary>
    /// Class that holds the methods to modify the GameData structs, read them and write them.
    /// </summary>
    public static class GameLogic
    {
        //A custom JSON serializer settings object that uses a custom contract resolver to ignore the Puzzle object when sending data over the network.
        //I found a way to do this without destorying how I update things through Newtonsofts documentation: https://www.newtonsoft.com/json/help/html/customjsonconverter.htm
        private static readonly JsonSerializerSettings NetworkJson = new JsonSerializerSettings
        {
            ContractResolver = new NetworkContractResolver(),
            Formatting = Formatting.None
        };


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
            //Make sure a stale transmission has not gotten thorugh before traversing. If so, return the message back.
            if (gameData.isGameOver)
            {
                gameData.GuessCorrect = false;
                gameData.message = "Game is already over.";
                return gameData;
            }

            //Set the flag to be false. If it is a match, it will go through.
            gameData.GuessCorrect = false;

            //Trim the whitespace off the guess string and reapply it back to the struct.
            string guess = (gameData.wordGuessed).Trim();
            gameData.wordGuessed = guess;

            //Using this bool value to determine whats next
            bool ok = Puzzle.IsValidGuess(guess, gameData.puzzle);
            gameData.GuessCorrect = ok;

            //If its not a valid guess, send the message back to the user and return the struct without modifying anything else.
            if (!ok)
            {
                gameData.message = "Incorrect guess.";
                return gameData;
            }

            //Have a barrier to prevent the user from guessing the same word twice and getting free points.
            gameData.puzzle.Words.Remove(guess);

            //Decrement the wordsLeft count.
            gameData.wordsLeft--;

            //If its less or equal to zero, se tthe game over flag and send a congratulatory message back to the user. If not, send a correct message back.
            if (gameData.wordsLeft <= 0)
            {
                gameData.isGameOver = true;
                gameData.message = "Congratulations! You've guessed all the words!";
            }
            else
            {
                gameData.message = "Correct!";
            }

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
            //Read up to 2048 bytes at a time from the stream.
            byte[] buffer = new byte[2048];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            //Guard 1 that detects if the other side closes prematurly.
            if (bytesRead == 0)
            {
                throw new IOException("Remote closed the connection (no response).");
            }
            //Make the JSON string from the GameData struct
            string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            //Make sure its not blank in case it was captured from a disconnect.
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                throw new IOException("Received empty/whitespace response.");
            }

            //Convert into JSON and rebuild the struct on the other end.
            GameData data = JsonConvert.DeserializeObject<GameData>(jsonData, NetworkJson);

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
            //Stop JSON string once we hit the Puzzle class.
            string jsonData = JsonConvert.SerializeObject(data, NetworkJson) + "\n";
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
        /// Serializes the GameData struct as a JSON String to send over network.
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


    internal sealed class NetworkContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
            System.Reflection.MemberInfo member,
            MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            // Do not send the Puzzle object over the network
            if (prop.PropertyName == nameof(GameData.puzzle))
                prop.Ignored = true;

            return prop;
        }
    }

}

