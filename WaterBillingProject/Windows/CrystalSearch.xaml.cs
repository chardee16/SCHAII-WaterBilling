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
using System.Windows.Shapes;
using WaterBillingProject;

namespace WaterBilling.Windows
{
    /// <summary>
    /// Interaction logic for CrystalSearch.xaml
    /// </summary>
    public partial class CrystalSearch : Window
    {
        private CrystalReport _parent;
        public CrystalSearch(CrystalReport cv, String search = "")
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleKeys);
            this._parent = cv;
            searchText.Text = search;
            searchText.Focus();
            Keyboard.Focus(searchText);
            searchText.SelectAll();

        }

        private void HandleKeys(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            if (e.Key == Key.Enter)
            {
                search();
            }
        }

        private void search()
        {

            if (this._parent._CrystalReport.ViewerCore.SearchForText(searchText.Text, false, false))
            {
                this._parent.searchText = searchText.Text;
            }
            else
            {
                if (this._parent._CrystalReport.ViewerCore.CurrentPageNumber > 1)
                {
                    if (MessageBox.Show("Search could not find any more instance of specified text on this last page.\n\nDo you wish to search from the first page?", "Could Not find text", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        this.Hide();
                        this._parent._CrystalReport.ViewerCore.ShowFirstPage();
                        if (!this._parent._CrystalReport.ViewerCore.SearchForText(searchText.Text, false, false))
                        {
                            MessageBox.Show("Search could not find any instance of the specified text in this document.", "Text not found", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Search could not find any instance of the specified text in this document.", "Text not found", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            this.Close();
        }
        private void searchText1_Click(object sender, RoutedEventArgs e)
        {
            search();
        }

        private void cancelSearch_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
