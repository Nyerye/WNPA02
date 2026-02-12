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

    

    public static class GameLogic
    {

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


        public static GameData EndGame(GameData gameData)
        {
            gameData.isGameOver = true;
            gameData.message = "Game Over!";
            return gameData;
        }

       
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

        public static void SendData(NetworkStream stream, GameData data)
        {
            //Convert the struct into JSON and then into bytes to send over the stream. Go until we hit a newline character to signify the end of the message.
            string jsonData = JsonConvert.SerializeObject(data)+"\n";
            byte[] buffer = Encoding.UTF8.GetBytes(jsonData);

            //Send it wherever its going.
            stream.Write(buffer, 0, buffer.Length);
        }
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

