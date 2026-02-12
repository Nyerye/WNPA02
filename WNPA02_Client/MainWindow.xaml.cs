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
        public static async Task<GameData>ConnectToServer(GameData gameData, TcpClient client)
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

        private async void NewGame_Click(object sender, RoutedEventArgs e)
        {
            //Populate fields with specific info
            gameData.command = "START";

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






        }

        private void ResumeGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About aboutPage = new About();
            aboutPage.ShowDialog();
        }

        private void SubmitGuess_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}