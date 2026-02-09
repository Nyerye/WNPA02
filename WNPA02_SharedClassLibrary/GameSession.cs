using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WNPA02_SharedClassLibrary.GameSession;

namespace WNPA02_SharedClassLibrary
{
    public class GameSession
    {
        public struct GameData
        {
            Guid SessionID;
            bool GuessCorrect;
            int timeLeft;
            int wordsLeft;
            int gameVersion;
            bool isGameOver;
            string wordGuessed;


        }
    }
}

namespace WNPA02_SharedClassLibrary
{
    public class GameLogic
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

    }
}

    

