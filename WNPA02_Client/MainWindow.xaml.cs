/// <file>
/// MainWindow.xaml.cs
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
/// Class file that holds the backend functioanlity to the frontend XAML the client uses when playing the game. 
/// </description>
/// <references>
/// Deitel, P., & Deitel, H. (2017). *C# 6 for Programmers Sixth Edition* 
/// (Sixth, Ser. Deitel Development Series). Pearson Education.
/// </references>
/// 
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WNPA02_SharedClassLibrary;

namespace WNPA02_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static GameData gameData = new GameData();
        public static int wordsFound;
        public static int totalWords;

        /// <summary>
        /// Constructor for the page
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Method that will allows the client to connect to the server. 
        /// Waits to establish a connection as the server could be busy.
        /// Once a connection is established, it will send over data. 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="addressToParse"></param>
        /// <param name="portToParse"></param>
        /// <returns></returns>
        public static async Task<GameData> ConnectToServer(GameData gameData, TcpClient client)
        {
            //Get the IP and the Port from the App.config file.
            string serverIP = ConfigurationManager.AppSettings["serverIp"];
            int port = int.Parse(ConfigurationManager.AppSettings["serverPort"]);

            //Try to connect the server and determine the IP and Port Windows sets for the client
            try
            {

                //Wait to establish a connection
                await client.ConnectAsync(serverIP, port);

                //Once connected, get the stream from it.
                NetworkStream stream = client.GetStream();

                //Send a message to the server with some sort of command to initate starting a game and the client id
                GameLogic.SendData(stream, gameData);

                //Receive the data back.
                gameData = GameLogic.ReceiveData(stream);

                return gameData;
            }

            catch (Exception ex)
            {
                //Take the exception and display it to the user to see. 
                MessageBox.Show(ex.ToString());

                //Return the gameData struct to show SessionID and string error
                return gameData;
            }
        }

        /// <summary>
        /// Event handler for when the user initiates a new game
        /// Loads the Start Command and updates the client UI with the results back from the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NewGame_Click(object sender, RoutedEventArgs e)
        {
            //Populate fields with specific info
            gameData.command = "START";

            //Start the timer.
            GameTimer.StartTimer();
            GameTimer.clientTimer.Tick += UpdateTimerUI;


            //Make the TcpClient and get the stream
            TcpClient client = new TcpClient();

            try
            {
                //Wait for the connection to the server to be accepted, data to be sent and then received back after processing
                gameData = await ConnectToServer(gameData, client);

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //Update fields in the UI.
            PuzzleStringTextBox.Text = gameData.puzzle.PuzzleString;
            SessionIDTextBox.Text = gameData.SessionID.ToString();
            TimeRemainingTextBox.Text = GameTimer.GetTimeRemaining();

            //Update the words to find value. This will determine whether the user wins
            totalWords = gameData.puzzle.WordCount;
        }

        /// <summary>
        /// Event handler that fires when the user clicks the "Submit Guess" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitGuess_Click(object sender, RoutedEventArgs e)
        {
            //Trim the whitespace off the guess string in case there is any.
            string guess = GuessInputTextBox.Text.Trim();

            //Check to see if its whitespace. If so, do not allow a guess to go through.
            if (string.IsNullOrWhiteSpace(guess))
            {
                MessageBox.Show("Guesses can not be blank. You must enter a valid string.");
                return;
            }

            //Check to see if the user has already exceeded the allowed playtime.
            if (GameTimer.GetTimeRemaining() == "00:00")
            {
                MessageBox.Show("You have run out of time. Please play again");
                Application.Current.Shutdown();
                return;
            }

<<<<<<< Updated upstream
            
=======
            //Check to see if the user has won by checking the flag. If so, ask if they want to go again.
            if (gameData.isGameOver)
            {
                string message = gameData.message;
                string title = "Information";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show(message, title, buttons);
                if (result == MessageBoxResult.Yes)
                {
                    NewGame_Click(sender, e);
                }
                else
                {
                    Application.Current.Shutdown();
                    return;
                }

            }
>>>>>>> Stashed changes

            //Load the command and append the trimmed guess to the GameData struct.
            gameData.command = "GUESS";
            gameData.wordGuessed = guess;

            using TcpClient client = new TcpClient();

            //Try to connect to the server and get the modified struct back
            try
            {
                gameData = await ConnectToServer(gameData, client);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            //Check to see if the guess was correct. If not, show why. Messages can have many meanings.
            if (!gameData.GuessCorrect)
            {
                MessageBox.Show(gameData.message);
                return;
            }

            //Increment the wordsfound, udpate the UI with the value. 
            //Add the guessed word that was correct to the found words box and update the time remaining.
            wordsFound++;
            WordsFoundTextBox.Text = wordsFound.ToString();
            FoundWordsTextBox.AppendText(guess + ",");
           

            //Check to see if the user has won.
            if (gameData.isGameOver)
            {
                MessageBox.Show(gameData.message);
                Application.Current.Shutdown();
                return;
            }

            //Write a Cookie in case there are disconnects
            Cookie clientCookie = new Cookie(
                gameData.SessionID,
                PuzzleStringTextBox.Text,
                FoundWordsTextBox.Text,
                WordsFoundTextBox.Text
            );
            Cookie.WriteCookieToFile(clientCookie);
        }

        /// <summary>
        /// Method that handles resuming a game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ResumeGame_Click(object sender, RoutedEventArgs e)
        {
            //Set the command to RESUME
            gameData.command = "RESUME";

            //Load the client cookie
            Cookie clientCookie;
            try
            {
                clientCookie = Cookie.ReadCookieFromFile();
                if (clientCookie != null)
                {
                    //Append the saved sessionID to the GameData struct.
                    gameData.SessionID = clientCookie.SessionID;

                    //Update the UI fields with the cookie data.
                    PuzzleStringTextBox.Text = clientCookie.PuzzleString;
                    FoundWordsTextBox.Text = clientCookie.WordsGuessed;

                    //Repopulate the string we were guessing words in and the sessionid
                    SessionIDTextBox.Text = clientCookie.SessionID.ToString();

                    //Restore total words found directly
                    wordsFound = int.Parse(clientCookie.WordsLeft);
                    WordsFoundTextBox.Text = wordsFound.ToString();
                }
                else
                {
                    MessageBox.Show("No saved game found. Please start a new game.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            //Start the timer again and resubscribe to the UpdateTimerUI
            GameTimer.StartTimer();
            GameTimer.clientTimer.Tick += UpdateTimerUI;

            //Make the TcpClient and get the stream
            using TcpClient client = new TcpClient();

            try
            {
                //Wait for the connection to the server to be accepted, data to be sent and then received back after processing.
                gameData = await ConnectToServer(gameData, client);
            }
            catch (Exception ex)
            {
                //Display a message box error to the user.
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Event handler exits the application when the user clicks exit from the File menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event handler that exits the application when the user clicks the X in the top right of the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XBtn_Click(object sender, CancelEventArgs e)
        {
            string message = "Are you sure you want to exit the game?";
            string title = "Warning";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons);
            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Event handler that opens the About page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            About aboutPage = new About();
            aboutPage.ShowDialog();
        }

        /// <summary>
        /// Tick event handler that live updates the time after a New Game
        /// sets its up. If the time is reached, it ends the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimerUI(object sender, EventArgs e)
        {
            //Call the method in GameTimer that returns our time as a string and append.
            TimeRemainingTextBox.Text = GameTimer.GetTimeRemaining();

            //End the game if time is up.
            if (GameTimer.IsTimeUp())
            {
                MessageBox.Show("You have run out of time.");
                Application.Current.Shutdown();
            }
        }



    }
}