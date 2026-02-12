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

namespace WNPA02_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method that will fetch a socket for the client to use to connect to the server
        /// </summary>
        /// <returns></returns>
        public static TcpClient FetchSocket()
        {
            //Fetch the player an empty socket that has an IPv4 address
            
            //Use the App.config file now that Norbert mentioned we can specify the client ip and port from the file.

            //Return it back. We will use it to connect to the server.
            //return client;


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
        public static async Task ConnectToServer(TcpClient client, string addressToParse, string portToParse)
        {
            //The server will display the ip and port it lives on to the user. Collect it from the passed in values.
            IPAddress placeholder = IPAddress.Parse(addressToParse);
            int port = int.Parse(portToParse);

            //Try to connect the server and determine the IP and Port Windows sets for the client
            try
            {

                //Wait to establish a connection
                await client.ConnectAsync(placeholder, port);

                //Send a message to the server with some sort of command to initate starting a game and the client id

            }

            catch (Exception ex)
            {
                //Take the exception and display it to the user to see. 
                MessageBox.Show(ex.ToString());
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResumeGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GuessInput_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void SubmitGuess_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}