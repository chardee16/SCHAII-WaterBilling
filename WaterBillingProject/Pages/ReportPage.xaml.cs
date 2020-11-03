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

namespace WaterBillingProject.Pages
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(10 + (150 * index), 30, 0, 0);

            switch (index)
            {
                case 0:
                    this.BillingGrid.Visibility = Visibility.Collapsed;
                    this.ClientGrid.Visibility = Visibility.Visible;
                    break;
                case 1:
                    this.ClientGrid.Visibility = Visibility.Collapsed;
                    this.BillingGrid.Visibility = Visibility.Visible;
                    break;
                case 2:
                    //GridMain.Background = Brushes.CadetBlue;
                    break;
                case 3:
                    //GridMain.Background = Brushes.DarkBlue;
                    break;
            }
        }
    }
}
