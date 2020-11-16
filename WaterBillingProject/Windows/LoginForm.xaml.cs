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
using System.Windows.Shapes;
using WaterBillingProject.Models.Login;
using WaterBillingProject.Repository;

namespace WaterBillingProject.Windows
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        LoginRepository repo = new LoginRepository();
        LoginDataContext dataCon;
        public LoginForm()
        {
            InitializeComponent();
            this.dataCon = new LoginDataContext();
            this.DataContext = this.dataCon;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.UsernameField.Focus();
        }


        private void HandleEsc(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                {
                    this.Close();
                }
                else if (e.Key == Key.Enter)
                {
                    LogInInit();
                }
            }
            catch (Exception ex)
            {
                e.Handled = true;
            }
        }


        private void LogInInit()
        {
            try
            {
                LoginClass login = new LoginClass();
                login = repo.AuthenticateUser(this.dataCon.UserName, PasswordField.Password);
                if (login.UserID == 0)
                {
                    MessageBox.Show(this, "Incorrect Username or Password", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MainWindow main = new MainWindow(login);
                    this.Hide();
                    main.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Incorrect Username or Password", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LogInInit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }



    public class LoginDataContext : INotifyPropertyChanged
    {

        private String _UserName;
        public String UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                if (value != _UserName)
                {
                    _UserName = value;
                    OnPropertyChanged("UserName");
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
