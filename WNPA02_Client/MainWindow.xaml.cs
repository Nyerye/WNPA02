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

        public static TcpClient FetchSocket()
        {
            //Fetch the player an empty socket that has an IPv4 address
            TcpClient client = new TcpClient(AddressFamily.InterNetwork);

            //Return it back. We will manipulate the data more in another method.
            return client;
            
            
        }
        public static async Task ConnectToServer(TcpClient client)
        {
            //Need a way to talk to the server before we can get Windows to tell us the true IP and Port.
            //However, everyones machine will be different if they are connecting to my server. 
            //We know where the server lives, that wont change.
            //Using localhost as the placeholder as it works on everyones machine.
            IPAddress placeholder = IPAddress.Parse("127.0.0.1");
            
            //Try to connect the server and determine the IP and Port Windows sets for the client
            try
            {
                
                await client.ConnectAsync(placeholder, 6000);

                //Break down the information we get from Windows.
                IPEndPoint windowsAssigned = (IPEndPoint)client.Client.LocalEndPoint;

                //Append the results for visibility
                string message = $"Your IP Address is {windowsAssigned.Address}:{windowsAssigned.Port}";

                //Maybe have some method that displays it on the client? Not there yet.

                //Log the message
                //Logger.Log(message);
            }

            catch (Exception ex)
            {
                //Take the exception and display it to the user to see. 
                MessageBox.Show(ex.ToString());
            }
        }

    }
}