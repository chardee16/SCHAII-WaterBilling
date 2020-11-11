using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WaterBillingProject.Models.Collection;
using WaterBillingProject.Repository;
using WaterBillingProject.Services;

namespace WaterBillingProject.Windows
{
    /// <summary>
    /// Interaction logic for TransactionList.xaml
    /// </summary>
    public partial class TransactionList : Window
    {

        BackgroundWorker worker = new BackgroundWorker();
        TransactionListDataContext dataCon = new TransactionListDataContext();
        CollectionRepository repo = new CollectionRepository();
        public ObservableCollection<TransactionListClass> transactionList;
        private ICollectionView MyData;
        string SearchText = string.Empty;
        int currentRow = 0, currentColumn = 1;
        public TransactionList()
        {
            InitializeComponent();
            InitializeWorkers();
            this.PreviewKeyDown += new KeyEventHandler(HandleKeysEvent);
            this.dataCon = new TransactionListDataContext();

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
                this.dataCon.transactionList = repo.GetTransactionList(LoginSession.TransDate.ToString("yyyy-MM-dd"));
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DataContext = this.dataCon;
            transactionList = new ObservableCollection<TransactionListClass>(this.dataCon.transactionList);
            DG_TransactionList.ItemsSource = transactionList;
            MyData = CollectionViewSource.GetDefaultView(transactionList);
            DG_TransactionList.Focus();
            DG_TransactionList.SelectedIndex = 0;
            txt_Search.Focus();
            //this.cmb_ClientStatus.ItemsSource = this.dataCon.clientStatusList;

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
                    selectedTransaction();
                    e.Handled = true;
                    break;
            }
        }


        private void selectedTransaction()
        {
            try
            {
                TransactionListClass selected = (TransactionListClass)DG_TransactionList.SelectedItem;
                PostingEntryWindow entryWindow = new PostingEntryWindow(selected);
                entryWindow.ShowDialog();
                this.Close();

            }
            catch (Exception ex)
            {

            }
        }


        private void SelectedCell_Click(object sender, RoutedEventArgs e)
        {
            //selectedTransaction();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txt_Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                selectedTransaction();
                e.Handled = true;
            }
        }

        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DG_ClientList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DG_ClientList_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DG_ClientList_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }




        private void HandleKeysEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (currentRow > 0)
                {
                    try
                    {
                        int previousIndex = DG_TransactionList.SelectedIndex - 1;
                        if (previousIndex < 0)
                            return; DG_TransactionList.SelectedIndex = previousIndex; DG_TransactionList.ScrollIntoView(DG_TransactionList.Items[currentRow]);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.Down)
            {
                if (currentRow < DG_TransactionList.Items.Count - 1)
                {
                    try
                    {
                        int nextIndex = DG_TransactionList.SelectedIndex + 1;

                        if (nextIndex > DG_TransactionList.Items.Count - 1)
                            return;

                        DG_TransactionList.SelectedIndex = nextIndex;
                        DG_TransactionList.ScrollIntoView(DG_TransactionList.Items[currentRow]);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                    }
                } // end if (this.SelectedOverride > 0)            
            } // end else if (e.Key == Key.Down)
            else if (e.Key == Key.End && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                DG_TransactionList.SelectedItem = transactionList.Last();
                DG_TransactionList.ScrollIntoView(DG_TransactionList.Items[DG_TransactionList.SelectedIndex]);
            }
        }





    }




    public class TransactionListDataContext : INotifyPropertyChanged
    {



        List<TransactionListClass> _transactionList;
        public List<TransactionListClass> transactionList
        {
            get { return _transactionList; }
            set
            {
                _transactionList = value;
                OnPropertyChanged("transactionList");
            }
        }






        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }




}
