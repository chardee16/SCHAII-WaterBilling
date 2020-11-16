using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        SettingsDataContext dataCon = new SettingsDataContext();
        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = dataCon;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(10 + (150 * index), 30, 0, 0);

            switch (index)
            {
                case 0:
                    this.UserGrid.Visibility = Visibility.Collapsed;
                    this.UserGrid.Visibility = Visibility.Visible;
                    break;
                case 1:
                    this.UserGrid.Visibility = Visibility.Collapsed;
                    this.EnvironmentGrid.Visibility = Visibility.Visible;
                    break;
                case 2:
                    //GridMain.Background = Brushes.CadetBlue;
                    break;
                case 3:
                    //GridMain.Background = Brushes.DarkBlue;
                    break;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Checker())
            {
                MessageBox.Show("proceed");
            }
            else
            {
                MessageBox.Show("Some data is missing!");
            }
        }


        private Boolean Checker()
        {
            if (String.IsNullOrEmpty(this.dataCon.FirstName))
            {
                return false;
            }
            else if (String.IsNullOrEmpty(this.dataCon.LastName))
            {
                return false;
            }
            else if (String.IsNullOrEmpty(this.dataCon.Username))
            {
                return false;
            }
            else if (String.IsNullOrEmpty(this.dataCon.Password))
            {
                return false;
            }
            else if (!this.dataCon.Password.Equals(this.dataCon.VerifyPassword))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }




    public class SettingsDataContext : INotifyPropertyChanged
    {
        private Int32 _UserID;
        public Int32 UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                if (value != _UserID)
                {
                    _UserID = value;
                    OnPropertyChanged("UserID");
                }
            }
        }


        private String _Username;
        public String Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (value != _Username)
                {
                    _Username = value;
                    OnPropertyChanged("Username");
                }
            }
        }


        private String _Password;
        public String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (value != _Password)
                {
                    _Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }


        private String _VerifyPassword;
        public String VerifyPassword
        {
            get
            {
                return _VerifyPassword;
            }
            set
            {
                if (value != _VerifyPassword)
                {
                    _VerifyPassword = value;
                    OnPropertyChanged("VerifyPassword");
                }
            }
        }


        private String _FirstName;
        public String FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (value != _FirstName)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }



        private String _MiddleName;
        public String MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (value != _MiddleName)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }


        private String _LastName;
        public String LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                if (value != _LastName)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private String _IsAdministrator;
        public String IsAdministrator
        {
            get
            {
                return _IsAdministrator;
            }
            set
            {
                if (value != _IsAdministrator)
                {
                    _IsAdministrator = value;
                    OnPropertyChanged("IsAdministrator");
                }
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }



}
