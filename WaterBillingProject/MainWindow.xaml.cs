using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WaterBilling.Pages;
using WaterBillingProject.Models.Login;
using WaterBillingProject.Pages;
using WaterBillingProject.Services;

namespace WaterBillingProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(LoginClass login)
        {
            InitializeComponent();

            if (GetFastestNISTDate().ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy - MM - dd"))
            {
                LoginSession.UserID = login.UserID;
                LoginSession.TransYear = DateTime.Now.Year;
                LoginSession.UserName = login.Username;
                LoginSession.TransDate = DateTime.Now;


                txt_Title.Text = "DASHBOARD";
                MainContent.Content = new DashboardPage();
            }
            else
            {
                MessageBox.Show("Computer date is not correct \nThe program will close.","ERROR",MessageBoxButton.OK,MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            
        }

        public MainWindow()
        {
            InitializeComponent();

            if (GetFastestNISTDate().ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy - MM - dd"))
            {
                LoginSession.UserID = 1;
                LoginSession.TransYear = 2020;
                LoginSession.UserName = "User";
                LoginSession.TransDate = DateTime.Now;

                txt_Title.Text = "DASHBOARD";
                MainContent.Content = new DashboardPage();
            }
            else
            {
                MessageBox.Show("Computer date is not correct");
            }


        }



        public static DateTime GetFastestNISTDate()
        {
            var result = DateTime.MinValue;
            // Initialize the list of NIST time servers
            // http://tf.nist.gov/tf-cgi/servers.cgi
            string[] servers = new string[] {
                "time-a.nist.gov",
                "time-b.nist.gov",

                };

            // Try 5 servers in random order to spread the load
            Random rnd = new Random();
            foreach (string server in servers.OrderBy(s => rnd.NextDouble()).Take(5))
            {
                try
                {
                    // Connect to the server (at port 13) and get the response
                    string serverResponse = string.Empty;
                    using (var reader = new StreamReader(new System.Net.Sockets.TcpClient(server, 13).GetStream()))
                    {
                        serverResponse = reader.ReadToEnd();
                    }

                    // If a response was received
                    if (!string.IsNullOrEmpty(serverResponse))
                    {
                        // Split the response string ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC(NIST) *")
                        string[] tokens = serverResponse.Split(' ');

                        // Check the number of tokens
                        if (tokens.Length >= 6)
                        {
                            // Check the health status
                            string health = tokens[5];
                            if (health == "0")
                            {
                                // Get date and time parts from the server response
                                string[] dateParts = tokens[1].Split('-');
                                string[] timeParts = tokens[2].Split(':');

                                // Create a DateTime instance
                                DateTime utcDateTime = new DateTime(
                                    Convert.ToInt32(dateParts[0]) + 2000,
                                    Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]),
                                    Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]),
                                    Convert.ToInt32(timeParts[2]));

                                // Convert received (UTC) DateTime value to the local timezone
                                result = utcDateTime.ToLocalTime();

                                return result;
                                // Response successfully received; exit the loop

                            }
                        }

                    }

                }
                catch
                {
                    // Ignore exception and try the next server
                }
            }
            return result;
        }

        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            txt_Title.Text = "DASHBOARD";
            MainContent.Content = new DashboardPage();
        }

        private void btn_Clientmaster_Click(object sender, RoutedEventArgs e)
        {
            txt_Title.Text = "CLIENT MASTER";
            MainContent.Content = new ClientMasterPage();
        }

        private void btn_Billing_Click(object sender, RoutedEventArgs e)
        {
            txt_Title.Text = "BILLING";
            MainContent.Content = new BillingPage();
        }

        private void btn_Collection_Click(object sender, RoutedEventArgs e)
        {
            txt_Title.Text = "COLLECTION";
            MainContent.Content = new CollectionPage();
        } 

        private void btn_Report_Click(object sender, RoutedEventArgs e)
        {
            txt_Title.Text = "REPORT";
            MainContent.Content = new ReportPage();
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            txt_Title.Text = "SETTINGS";
            MainContent.Content = new SettingsPage();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                txt_Title.Text = "DASHBOARD";
                MainContent.Content = new DashboardPage();
                e.Handled = true;
            }
            else if (e.Key == Key.F2)
            {
                txt_Title.Text = "CLIENT MASTER";
                MainContent.Content = new ClientMasterPage();
                e.Handled = true;
            }
            else if (e.Key == Key.F3)
            {
                txt_Title.Text = "BILLING";
                MainContent.Content = new BillingPage();
                e.Handled = true;
            }
            else if (e.Key == Key.F4)
            {
                txt_Title.Text = "COLLECTION";
                MainContent.Content = new CollectionPage();
                e.Handled = true;
            }
            else if (e.Key == Key.F5)
            {
                txt_Title.Text = "REPORT";
                MainContent.Content = new ReportPage();
                e.Handled = true;
            }
            else if (e.Key == Key.F6)
            {
                txt_Title.Text = "SETTINGS";
                MainContent.Content = new SettingsPage();
                e.Handled = true;
            }
        }
    }
}

