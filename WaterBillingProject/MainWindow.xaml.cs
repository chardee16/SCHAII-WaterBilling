using System.Windows;
using System.Windows.Input;
using WaterBilling.Pages;
using WaterBillingProject.Pages;

namespace WaterBillingProject
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
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            txt_Title.Text = "SETTINGS";
        }
    }
}

