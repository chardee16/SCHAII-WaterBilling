using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using WaterBillingProject.Models.Reports;
using WaterBillingProject.Repository;

namespace WaterBillingProject.Pages
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        ReportDataContext dataCon = new ReportDataContext();
        BackgroundWorker worker = new BackgroundWorker();
        ReportRepository repo = new ReportRepository();

        public ObservableCollection<ReportClientListClass> clientList;
        private ICollectionView MyData;
        string SearchText = string.Empty;
        int currentRow = 0, currentColumn = 1;

        public ReportPage()
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

        private void InitializeWorkers()
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.dataCon.reportClientList = repo.GetClientList();
                this.dataCon.reportTransactionList = repo.GetTransactionList("2020-11-01","2020-11-30");
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.dataCon.TotalClient = this.dataCon.reportClientList.Count;
            this.dataCon.TotalConsumption = this.dataCon.reportClientList.Sum(x => Convert.ToInt64(x.Consumption));
            this.dataCon.TotalDues = this.dataCon.reportClientList.Sum(x => Math.Round(x.TotalDue,2,MidpointRounding.AwayFromZero));


            this.DataContext = this.dataCon;
            clientList = new ObservableCollection<ReportClientListClass>(this.dataCon.reportClientList);
            DG_ClientList.ItemsSource = clientList;
            MyData = CollectionViewSource.GetDefaultView(clientList);
            DG_ClientList.Focus();
            DG_ClientList.SelectedIndex = 0;
            txt_Search.Focus();
            //this.cmb_ClientStatus.ItemsSource = this.dataCon.clientStatusList;

        }

        private void txt_Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            SearchText = t.Text.ToString();
            MyData.Filter = FilterData;

            DG_ClientList.SelectedIndex = 0;
        }

        private void btn_Find_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.dataCon.DateTo);
        }

        private bool FilterData(object item)
        {
            var value = (ReportClientListClass)item;
            if (value == null || value.FullName == null)
                return false;
            return Convert.ToString(value.ClientID).StartsWith(SearchText.ToLower()) || value.FullName.ToLower().StartsWith(SearchText.ToLower()) || value.FullAddress.ToLower().StartsWith(SearchText.ToLower());
        }
        





    }//end of report page class



    public class ReportDataContext : INotifyPropertyChanged
    {

        private Int32 _TotalClient;
        public Int32 TotalClient
        {
            get
            {
                return _TotalClient;
            }
            set
            {
                if (value != _TotalClient)
                {
                    _TotalClient = value;
                    OnPropertyChanged("TotalClient");
                }
            }
        }


        private Int64 _TotalConsumption;
        public Int64 TotalConsumption
        {
            get
            {
                return _TotalConsumption;
            }
            set
            {
                if (value != _TotalConsumption)
                {
                    _TotalConsumption = value;
                    OnPropertyChanged("TotalConsumption");
                }
            }
        }


        private Decimal _TotalDues;
        public Decimal TotalDues
        {
            get
            {
                return _TotalDues;
            }
            set
            {
                if (value != _TotalDues)
                {
                    _TotalDues = value;
                    OnPropertyChanged("TotalDues");
                }
            }
        }


        List<ReportClientListClass> _reportClientList;
        public List<ReportClientListClass> reportClientList
        {
            get { return _reportClientList; }
            set
            {
                _reportClientList = value;
                OnPropertyChanged("reportClientList");
            }
        }



        private String _DateFrom;
        public String DateFrom
        {
            get
            {
                return _DateFrom;
            }
            set
            {
                if (value != _DateFrom)
                {
                    _DateFrom = value;
                    OnPropertyChanged("DateFrom");
                }
            }
        }


        private String _DateTo;
        public String DateTo
        {
            get
            {
                return _DateTo;
            }
            set
            {
                if (value != _DateTo)
                {
                    _DateTo = value;
                    OnPropertyChanged("DateTo");
                }
            }
        }


        List<ReportTransactionList> _reportTransactionList;
        public List<ReportTransactionList> reportTransactionList
        {
            get { return _reportTransactionList; }
            set
            {
                _reportTransactionList = value;
                OnPropertyChanged("reportTransactionList");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }




}
