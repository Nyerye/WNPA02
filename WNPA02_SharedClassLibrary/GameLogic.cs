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
            //Check to see of the guess is valid
            gameData.GuessCorrect = Puzzle.IsValidGuess(gameData.wordGuessed, gameData.puzzle);

            //Determine what to do based on a true/false guess and update the game data accordingly.
            if (gameData.GuessCorrect)
            {
                gameData.message = "Correct Guess!";
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

        public static void SaveGameData(GameData gameData)
        {
            //Method that will used with the SessionID to create a session and save it for the user reconnects.
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
            string jsonData = JsonConvert.SerializeObject(data);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonData);

            //Send it wherever its going.
            stream.Write(buffer, 0, buffer.Length);
        }
        public static GameData LoadGameData(Guid sessionID)
        {
            //Method that will be used to load the game data for a session when a user reconnects.

        }


    }
}

