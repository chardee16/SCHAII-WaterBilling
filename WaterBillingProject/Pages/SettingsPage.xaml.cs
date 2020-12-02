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
using WaterBillingProject.Models.Settings;
using WaterBillingProject.Repository;
using WaterBillingProject.Services;

namespace WaterBillingProject.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        SettingsDataContext dataCon = new SettingsDataContext();
        SettingsRepository repo = new SettingsRepository();
        BackgroundWorker worker = new BackgroundWorker();

        public SettingsPage()
        {
            InitializeComponent();
            InitializeWorkers();
            this.DataContext = dataCon;

            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private void InitializeWorkers()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.dataCon.userList = repo.GetUserList();
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Refresh();

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

            if (this.dataCon.IsEdit)
            {
                if (Checker())
                {
                    Edit();
                }
                else
                {
                    MessageBox.Show("Some data must be filled out", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else if (this.dataCon.IsResetPassword)
            {
                if (PasswordChecker())
                {
                    ChangePassword();
                }
                else
                {
                    MessageBox.Show("Some data must be filled out", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (Checker())
                {
                    Save();
                }
                else
                {
                    MessageBox.Show("Some data must be filled out", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                
                
            
            
        }


        private void Save()
        {
            try
            {
                UserClass user = new UserClass();
                user.FirstName = this.dataCon.FirstName;
                user.MiddleName = this.dataCon.MiddleName;
                user.LastName = this.dataCon.LastName;
                user.Username = this.dataCon.Username;
                user.Password = this.Password.Password;
                user.IsAdministrator = this.dataCon.IsAdministrator;

                if (this.repo.InsertUser(user))
                {
                    MessageBox.Show("User successfully recored.","SUCCESS",MessageBoxButton.OK,MessageBoxImage.Information);
                    try
                    {
                        worker.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    MessageBox.Show("User failed to save.", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                }


            }
            catch
            {

            }
           
        }


        private void Edit()
        {
            try
            {
                if (this.dataCon.UserID > 0)
                {
                    UserClass user = new UserClass();
                    user.UserID = this.dataCon.UserID;
                    user.FirstName = this.dataCon.FirstName;
                    user.MiddleName = this.dataCon.MiddleName;
                    user.LastName = this.dataCon.LastName;
                    user.IsAdministrator = this.dataCon.IsAdministrator;

                    if (this.repo.UpdateUser(user))
                    {
                        MessageBox.Show("User successfully updated.", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                        try
                        {
                            worker.RunWorkerAsync();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("User failed to save.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No user selected", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                


            }
            catch
            {

            }

        }


        private void ChangePassword()
        {
            try
            {
                UserClass user = new UserClass();
                user.UserID = this.dataCon.UserID;
                user.Password = this.Password.Password;


                if (this.repo.UpdatePassword(user))
                {

                    if (this.dataCon.UserID == LoginSession.UserID)
                    {
                        MessageBox.Show("Password successfully changed.\nSystem will now close.", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        MessageBox.Show("Password successfully changed.", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                        try
                        {
                            worker.RunWorkerAsync();
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    
                }
                else
                {
                    MessageBox.Show("Password failed to change.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }
            catch
            {

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
            else if (String.IsNullOrEmpty(this.Password.Password))
            {
                return false;
            }
            else if (!this.Password.Password.Equals(this.VerifyPassword.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private Boolean PasswordChecker()
        {
            if (String.IsNullOrEmpty(this.Password.Password))
            {
                return false;
            }
            else if (!this.Password.Password.Equals(this.VerifyPassword.Password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Refresh()
        {
            this.dataCon.UserID = 0;
            this.dataCon.FirstName = "";
            this.dataCon.MiddleName = "";
            this.dataCon.LastName = "";
            this.dataCon.Username = "";
            this.dataCon.IsAdministrator = false;
            this.Password.Password = "jkhkhcx";
            this.VerifyPassword.Password = "sdadasd";
            this.dataCon.IsEdit = false;
            this.dataCon.IsResetPassword = false ;
        }

        private void btn_New_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            Disable(false);
            txt_FName.Focus();
        }

        private void DG_UserList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                selectedUser();
            }
            catch
            {

            }
        }

        private void selectedUser()
        {
            UserClass selected = (UserClass)DG_UserList.SelectedItem;
            this.dataCon.UserID = selected.UserID;
            this.dataCon.FirstName = selected.FirstName;
            this.dataCon.MiddleName = selected.MiddleName;
            this.dataCon.LastName = selected.LastName;
            this.dataCon.Username = selected.Username;
            this.Password.Password = selected.Password;
            this.VerifyPassword.Password = selected.Password;

            Disable(true);
        }

        private void Disable(Boolean IsActive)
        {
            txt_FName.IsReadOnly = IsActive;
            txt_MName.IsReadOnly = IsActive;
            txt_LName.IsReadOnly = IsActive;
            if (this.dataCon.IsEdit)
            {
                txt_Username.IsReadOnly = !IsActive;
                Password.IsEnabled = IsActive;
                VerifyPassword.IsEnabled = IsActive;
            }
            else
            {
                txt_Username.IsReadOnly = IsActive;
                Password.IsEnabled = !IsActive;
                VerifyPassword.IsEnabled = !IsActive;
            }
            
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            this.dataCon.IsEdit = true;
            Disable(false) ;
        }

        private void btn_ResetPass_Click(object sender, RoutedEventArgs e)
        {
            this.dataCon.IsResetPassword = true;
            Password.IsEnabled = true;
            VerifyPassword.IsEnabled = true;
        }
    }


    public class SettingsDataContext : INotifyPropertyChanged
    {

        List<UserClass> _userList;
        public List<UserClass> userList
        {
            get { return _userList; }
            set
            {
                _userList = value;
                OnPropertyChanged("userList");
            }
        }


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

        private Boolean _IsAdministrator;
        public Boolean IsAdministrator
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


        private Boolean _IsEdit;
        public Boolean IsEdit
        {
            get
            {
                return _IsEdit;
            }
            set
            {
                if (value != _IsEdit)
                {
                    _IsEdit = value;
                    OnPropertyChanged("IsEdit");
                }
            }
        }

        private Boolean _IsResetPassword;
        public Boolean IsResetPassword
        {
            get
            {
                return _IsResetPassword;
            }
            set
            {
                if (value != _IsResetPassword)
                {
                    _IsResetPassword = value;
                    OnPropertyChanged("IsResetPassword");
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
