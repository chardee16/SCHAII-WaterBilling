using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaterBilling.Pages;

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

