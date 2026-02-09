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
        public int timeLeft;
        public int wordsLeft;
        public int gameVersion;
        public bool isGameOver;
        public string wordGuessed;
        public string message;


    }

    public static class GameLogic
    {

        public static GameData InitializeGame(GameData gameData)
        {
            //Method that will be used to initialize the game data for a new game session and then save the game data for the session.
        }

        public static GameData UpdateGame(GameData gameData, bool guessCorrect)
        {
            //Method that will be used to update the game data based on the guess made by the user and then save the game data for the session.
        }

        public static GameData EndGame(GameData gameData)
        {
            //Method that will be used to end the game and set the isGameOver variable to true and then save the game data for the session.
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

