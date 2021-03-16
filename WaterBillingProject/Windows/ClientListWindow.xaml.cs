using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WaterBilling.Models.ClientMaster;
using WaterBilling.Repository;

namespace WaterBilling.Windows
{
    /// <summary>
    /// Interaction logic for ClientListWindow.xaml
    /// </summary>
    public partial class ClientListWindow : Window
    {


        BackgroundWorker worker = new BackgroundWorker();
        ClientMasterRepository repo = new ClientMasterRepository();
        ClientListDataContext dataCon;

        public ObservableCollection<ClientClass> clientList;
        ClientClass toReturn = new ClientClass();
        private ICollectionView MyData;
        string SearchText = string.Empty;
        int currentRow = 0, currentColumn = 1;

        public ClientListWindow()
        {
            InitializeComponent();
            InitializeWorkers();
            this.PreviewKeyDown += new KeyEventHandler(HandleKeysEvent);
            this.dataCon = new ClientListDataContext();


            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }
        }


        public ClientClass clientSelected
        {
            get { return this.toReturn; }
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
            clientList = new ObservableCollection<ClientClass>(this.dataCon.clientList);
            DG_ClientList.ItemsSource = clientList;
            MyData = CollectionViewSource.GetDefaultView(clientList);
            DG_ClientList.Focus();
            DG_ClientList.SelectedIndex = 0;
            txt_Search.Focus();
            //this.cmb_ClientStatus.ItemsSource = this.dataCon.clientStatusList;

        }


        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DG_ClientList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            clientSelectedFunction();
        }


        private void clientSelectedFunction()
        {
            if (DG_ClientList.SelectedItem == null) return;
            var selectedPerson = DG_ClientList.SelectedItem as ClientClass;
            if (selectedPerson != null)
            {
                this.toReturn.ClientID = selectedPerson.ClientID;
                this.toReturn.FirstName = selectedPerson.FirstName;
                this.toReturn.MiddleName = selectedPerson.MiddleName;
                this.toReturn.LastName = selectedPerson.LastName;
                this.toReturn.ClientAccountStatusID = selectedPerson.ClientAccountStatusID;
                this.toReturn.ClientStatusDesc = selectedPerson.ClientStatusDesc;
                this.toReturn.BlockNo = selectedPerson.BlockNo;
                this.toReturn.LotNo = selectedPerson.LotNo;
                this.toReturn.Occupants = selectedPerson.Occupants;
                this.toReturn.IsSenior = selectedPerson.IsSenior;
                this.toReturn.SeniorCount = selectedPerson.SeniorCount;
                this.toReturn.FullName = selectedPerson.FullName;
                this.toReturn.FullAddress = selectedPerson.FullAddress;

                DialogResult = true;
            }
        }


        private void DG_ClientList_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            dataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }


        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            SearchText = t.Text.ToString();
            MyData.Filter = FilterData;

            DG_ClientList.SelectedIndex = 0;
        }


        private void DG_ClientList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    e.Handled = true;
                    this.Close();
                    break;
                case Key.Enter:
                    clientSelectedFunction();
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


        public class ClientListDataContext : INotifyPropertyChanged
        {



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






            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string property)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_Search.Focus();
        }

        private void txt_Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                clientSelectedFunction();
                e.Handled = true;
            }
        }

        private void HandleKeys(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    e.Handled = true;
                    this.Close();
                    break;
                case Key.Enter:
                    clientSelectedFunction();
                    e.Handled = true;
                    break;
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
            else if (e.Key == Key.End && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                DG_ClientList.SelectedItem = clientList.Last();
                DG_ClientList.ScrollIntoView(DG_ClientList.Items[DG_ClientList.SelectedIndex]);
            }
        }





    }
}
