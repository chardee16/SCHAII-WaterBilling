using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WaterBilling.Models.ClientMaster;
using WaterBilling.Repository;
using WaterBillingProject.Windows;

namespace WaterBilling.Pages
{
    /// <summary>
    /// Interaction logic for ClientMasterPage.xaml
    /// </summary>
    public partial class ClientMasterPage : Page
    {
        BackgroundWorker worker = new BackgroundWorker();
        ClientMasterRepository repo = new ClientMasterRepository();
        ClientMasterDataContext dataCon;



        public ObservableCollection<ClientClass> clientList;

        ClientClass toReturn = new ClientClass();
        private ICollectionView MyData;
        string SearchText = string.Empty;
        int currentRow = 0, currentColumn = 1;

        public ClientMasterPage()
        {
            InitializeComponent();
            InitializeWorkers();
            InitializeFields(true);
            this.PreviewKeyDown += new KeyEventHandler(HandleKeysEvent);
            this.dataCon = new ClientMasterDataContext();
            

            this.dataCon.clientStatusList = repo.GetClientStatus();
            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }

            this.cmb_ClientStatus.ItemsSource = this.dataCon.clientStatusList;

            txt_Search.Focus();
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
                this.dataCon.clientList = repo.GetClientList();
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DataContext = this.dataCon;

            

            //this.cmb_ClientStatus.ItemsSource = this.dataCon.clientStatusList;
            Refresh();

            clientList = new ObservableCollection<ClientClass>(this.dataCon.clientList);
            DG_ClientList.ItemsSource = clientList;
            MyData = CollectionViewSource.GetDefaultView(clientList);
            DG_ClientList.Focus();
            DG_ClientList.SelectedIndex = 0;

            this.Spinner.Visibility = Visibility.Hidden;
            this.Spinner.Spin = false;
        }


        private void chk_IsSenior_Checked(object sender, RoutedEventArgs e)
        {
            stk_Senior.Visibility = Visibility.Visible;
        }

        private void chk_IsSenior_Unchecked(object sender, RoutedEventArgs e)
        {
            stk_Senior.Visibility = Visibility.Hidden;
        }


        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataCon.IsEditted)
            {
                Edit();
            }
            else
            {
                Save();
            }
            
        }

        private void Save()
        {
            ClientClass clientCount = new ClientClass();
            clientCount.FirstName = this.dataCon.FirstName;
            clientCount.LastName = this.dataCon.LastName;
            clientCount.BlockNo = this.dataCon.BlockNo;
            clientCount.LotNo = this.dataCon.LotNo;

            if (repo.ClientCount(clientCount))
            {
                ClientClass client = new ClientClass();
                client.FirstName = this.dataCon.FirstName;
                client.MiddleName = this.dataCon.MiddleName;
                client.LastName = this.dataCon.LastName;
                client.ClientAccountStatusID = this.dataCon.ClientAccountStatusID;
                client.BlockNo = this.dataCon.BlockNo;
                client.LotNo = this.dataCon.LotNo;
                client.Occupants = this.dataCon.Occupants;
                client.IsSenior = this.dataCon.IsSenior;
                client.SeniorCount = this.dataCon.SeniorCount;
                client.PreviousReading = this.dataCon.PreviousReading;


                if (repo.InsertClient(client))
                {
                    MessageBox.Show("Client successfully saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Client failed to saved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                MessageBox.Show("Client already exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

        }

        private void Edit()
        {
            if (this.dataCon.ClientID > 0)
            {
                ClientClass client = new ClientClass();
                client.ClientID = this.dataCon.ClientID;
                client.FirstName = this.dataCon.FirstName;
                client.MiddleName = this.dataCon.MiddleName;
                client.LastName = this.dataCon.LastName;
                client.ClientAccountStatusID = this.dataCon.ClientAccountStatusID;
                client.BlockNo = this.dataCon.BlockNo;
                client.LotNo = this.dataCon.LotNo;
                client.Occupants = this.dataCon.Occupants;
                client.IsSenior = this.dataCon.IsSenior;
                client.SeniorCount = this.dataCon.SeniorCount;
                client.PreviousReading = this.dataCon.PreviousReading;


                if (repo.EditClient(client))
                {
                    MessageBox.Show("Client successfully updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Client failed to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                try
                {
                    worker.RunWorkerAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void InitializeFields(Boolean val)
        {
            txt_FirstName.IsReadOnly = val;
            txt_MiddleName.IsReadOnly = val;
            txt_LastName.IsReadOnly = val;
            cmb_ClientStatus.IsReadOnly = val;
            txt_BlockNo.IsReadOnly = val;
            txt_LotNo.IsReadOnly = val;
            txt_Occupants.IsReadOnly = val;
            chk_IsSenior.IsEnabled = !val;
            txt_NoOfSenior.IsReadOnly = val;
        }


        private void btn_New_Click(object sender, RoutedEventArgs e)
        {
            NewFunction();
        }


        private void Refresh()
        {
            this.dataCon.ClientID = 0;
            this.dataCon.FirstName = "";
            this.dataCon.MiddleName = "";
            this.dataCon.LastName = "";
            this.dataCon.ClientAccountStatusID = 1;
            this.dataCon.BlockNo = 0;
            this.dataCon.LotNo = 0;
            this.dataCon.Occupants = 0;
            this.dataCon.IsSenior = false;
            this.dataCon.SeniorCount = 0;
            this.dataCon.IsEditted = false;
            this.dataCon.PreviousReading = 0;
            InitializeFields(true);
        }

        private void NewFunction()
        {
            Refresh();
            InitializeFields(false);
        }


        private void DG_ClientList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ClientClass selected = (ClientClass)DG_ClientList.SelectedItem;
                this.dataCon.ClientID = selected.ClientID;
                this.dataCon.FirstName = selected.FirstName;
                this.dataCon.MiddleName = selected.MiddleName;
                this.dataCon.LastName = selected.LastName;
                this.dataCon.ClientAccountStatusID = selected.ClientAccountStatusID;
                this.dataCon.BlockNo = selected.BlockNo;
                this.dataCon.LotNo = selected.LotNo;
                this.dataCon.Occupants = selected.Occupants;
                this.dataCon.IsSenior = selected.IsSenior;
                this.dataCon.SeniorCount = selected.SeniorCount;
                this.dataCon.PreviousReading = selected.PreviousReading;
                InitializeFields(true);

            }
            catch (Exception ex)
            {

            }
        }


        private void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            this.dataCon.IsEditted = true;
            InitializeFields(false);
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            SearchText = t.Text.ToString();
            MyData.Filter = FilterData;

            DG_ClientList.SelectedIndex = 0;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    e.Handled = true;
                    break;
                case Key.Enter:
                    ClientClass selected = (ClientClass)DG_ClientList.SelectedItem;
                    this.dataCon.ClientID = selected.ClientID;
                    this.dataCon.FirstName = selected.FirstName;
                    this.dataCon.MiddleName = selected.MiddleName;
                    this.dataCon.LastName = selected.LastName;
                    this.dataCon.ClientAccountStatusID = selected.ClientAccountStatusID;
                    this.dataCon.BlockNo = selected.BlockNo;
                    this.dataCon.LotNo = selected.LotNo;
                    this.dataCon.Occupants = selected.Occupants;
                    this.dataCon.IsSenior = selected.IsSenior;
                    this.dataCon.SeniorCount = selected.SeniorCount;
                    this.dataCon.PreviousReading = selected.PreviousReading;
                    InitializeFields(true);
                    e.Handled = true;
                    break;
            }
        }


        private bool FilterData(object item)
        {
            var value = (ClientClass)item;
            if (value == null || value.FullName == null)
                return false;
            return Convert.ToString(value.ClientID).StartsWith(SearchText.ToLower()) || value.LastName.ToLower().StartsWith(SearchText.ToLower());
        }

        private void Executed_New(object sender, ExecutedRoutedEventArgs e)
        {
            NewFunction();
        }

        private void Executed_Edit(object sender, ExecutedRoutedEventArgs e)
        {
            this.dataCon.IsEditted = true;
            InitializeFields(false);
        }

        private void Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.dataCon.IsEditted)
            {
                Edit();
            }
            else
            {
                Save();
            }
        }

        private void SelectedCell_Click(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell.Column.DisplayIndex != this.currentColumn)
            {
                cell.IsSelected = false;
            }
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            DataGridRow row = dep as DataGridRow;
            this.currentRow = row.GetIndex();
        }


        private void HandleKeysEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (currentRow > 0)
                {
                    try
                    {
                        int previousIndex = DG_ClientList.SelectedIndex - 1;
                        if (previousIndex < 0)
                            return; DG_ClientList.SelectedIndex = previousIndex; DG_ClientList.ScrollIntoView(DG_ClientList.Items[currentRow]);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.Down)
            {
                if (currentRow < DG_ClientList.Items.Count - 1)
                {
                    try
                    {
                        int nextIndex = DG_ClientList.SelectedIndex + 1;

                        if (nextIndex > DG_ClientList.Items.Count - 1)
                            return;

                        DG_ClientList.SelectedIndex = nextIndex;
                        DG_ClientList.ScrollIntoView(DG_ClientList.Items[currentRow]);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                    }
                } // end if (this.SelectedOverride > 0)            
            } // end else if (e.Key == Key.Down)
           

        }

        private void btn_BillingHistory_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataCon.ClientID != 0)
            {
                BillingHistoryWindow billHistory = new BillingHistoryWindow(this.dataCon.ClientID);
                billHistory.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select client.","Error!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void btn_PaymentHistory_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataCon.ClientID != 0)
            {
                PaymentHistoryWindow billHistory = new PaymentHistoryWindow(this.dataCon.ClientID);
                billHistory.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select client.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Executed_BillingHistory(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.dataCon.ClientID != 0)
            {
                BillingHistoryWindow billHistory = new BillingHistoryWindow(this.dataCon.ClientID);
                billHistory.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select client.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Executed_PaymentHistory(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.dataCon.ClientID != 0)
            {
                PaymentHistoryWindow billHistory = new PaymentHistoryWindow(this.dataCon.ClientID);
                billHistory.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select client.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class ClientMasterDataContext : INotifyPropertyChanged
        {

            List<ClientStatusClass> _clientStatusList;
            public List<ClientStatusClass> clientStatusList
            {
                get { return _clientStatusList; }
                set
                {
                    _clientStatusList = value;
                    OnPropertyChanged("clientStatusList");
                }
            }

            List<ClientClass> _clientList;
            public List<ClientClass> clientList
            {
                get { return _clientList; }
                set
                {
                    _clientList = value;
                    OnPropertyChanged("clientList");
                }
            }



            private Int64 _ClientID;
            public Int64 ClientID
            {
                get
                {
                    return _ClientID;
                }
                set
                {
                    if (value != _ClientID)
                    {
                        _ClientID = value;
                        OnPropertyChanged("ClientID");
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



            private Int32 _ClientAccountStatusID;
            public Int32 ClientAccountStatusID
            {
                get
                {
                    return _ClientAccountStatusID;
                }
                set
                {
                    if (value != _ClientAccountStatusID)
                    {
                        _ClientAccountStatusID = value;
                        OnPropertyChanged("ClientAccountStatusID");
                    }
                }
            }



            private Int32 _BlockNo;
            public Int32 BlockNo
            {
                get
                {
                    return _BlockNo;
                }
                set
                {
                    if (value != _BlockNo)
                    {
                        _BlockNo = value;
                        OnPropertyChanged("BlockNo");
                    }
                }
            }


            private Int32 _LotNo;
            public Int32 LotNo
            {
                get
                {
                    return _LotNo;
                }
                set
                {
                    if (value != _LotNo)
                    {
                        _LotNo = value;
                        OnPropertyChanged("LotNo");
                    }
                }
            }



            private Int32 _Occupants;
            public Int32 Occupants
            {
                get
                {
                    return _Occupants;
                }
                set
                {
                    if (value != _Occupants)
                    {
                        _Occupants = value;
                        OnPropertyChanged("Occupants");
                    }
                }
            }


            private Boolean _IsSenior;
            public Boolean IsSenior
            {
                get
                {
                    return _IsSenior;
                }
                set
                {
                    if (value != _IsSenior)
                    {
                        _IsSenior = value;
                        OnPropertyChanged("IsSenior");
                    }
                }
            }



            private Int32 _SeniorCount;
            public Int32 SeniorCount
            {
                get
                {
                    return _SeniorCount;
                }
                set
                {
                    if (value != _SeniorCount)
                    {
                        _SeniorCount = value;
                        OnPropertyChanged("SeniorCount");
                    }
                }
            }


            private Boolean _IsEditted;
            public Boolean IsEditted
            {
                get
                {
                    return _IsEditted;
                }
                set
                {
                    if (value != _IsEditted)
                    {
                        _IsEditted = value;
                        OnPropertyChanged("IsEditted");
                    }
                }
            }



            private Int64 _PreviousReading;
            public Int64 PreviousReading
            {
                get
                {
                    return _PreviousReading;
                }
                set
                {
                    if (value != _PreviousReading)
                    {
                        _PreviousReading = value;
                        OnPropertyChanged("PreviousReading");
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
}
