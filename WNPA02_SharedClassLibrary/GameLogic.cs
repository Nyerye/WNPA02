using System;
using System.Collections.Generic;
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
        
        public static GameData InitializeGame(GameData gameData)
        {

            //Method that will be used to initialize the game data for a new game session and then save the game data for the session.
        }

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

        public static GameData SendToClient(GameData gameData)
        {
            //Method that will be used to send the game data to the client.
        }

        public static GameData SendToServer(GameData gameData)
        {
            //Method that will be used to send the data to the server to process.
        }

        public static GameData LoadGameData(Guid sessionID)
        {
            //Method that will be used to load the game data for a session when a user reconnects.

        }

        public static GameData ReceiveGameData(NetworkStream stream)
        {
            
            //Method that will be used to receive the game data from the client.
            
        }

    }
}

