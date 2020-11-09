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
using WaterBillingProject.Services;

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

        BackgroundWorker billworker = new BackgroundWorker();

        public ObservableCollection<ReportClientListClass> clientList;
        private ICollectionView MyData;
        string SearchText = string.Empty;
        int currentRow = 0, currentColumn = 1;

        public ReportPage()
        {
            InitializeComponent();

            this.dataCon.DateFrom = LoginSession.TransDate.ToString("MM/dd/yyyy");
            this.dataCon.DateTo = LoginSession.TransDate.ToString("MM/dd/yyyy");

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


            billworker.DoWork += billworker_DoWork;
            billworker.RunWorkerCompleted += billworker_RunWorkerCompleted;

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.dataCon.reportClientList = repo.GetClientList();
                this.dataCon.reportTransactionList = repo.GetTransactionList(this.dataCon.DateFrom,this.dataCon.DateTo);
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.dataCon.TotalClient = this.dataCon.reportClientList.Count;
            this.dataCon.TotalConsumption = this.dataCon.reportClientList.Sum(x => Convert.ToInt64(x.Consumption));
            this.dataCon.TotalDues = this.dataCon.reportClientList.Sum(x => Math.Round(x.TotalDue,2,MidpointRounding.AwayFromZero));

            this.dataCon.TotalWaterBill = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.WaterBill, 2, MidpointRounding.AwayFromZero));
            this.dataCon.TotalDiscount = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.Discount, 2, MidpointRounding.AwayFromZero));
            this.dataCon.TotalGarbage = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.GarbageCollection, 2, MidpointRounding.AwayFromZero));
            this.dataCon.TotalMonthlyDues = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.MonthlyDues, 2, MidpointRounding.AwayFromZero));


            this.DataContext = this.dataCon;
            clientList = new ObservableCollection<ReportClientListClass>(this.dataCon.reportClientList);
            DG_ClientList.ItemsSource = clientList;
            MyData = CollectionViewSource.GetDefaultView(clientList);
            DG_ClientList.Focus();
            DG_ClientList.SelectedIndex = 0;
            txt_Search.Focus();
            //this.cmb_ClientStatus.ItemsSource = this.dataCon.clientStatusList;

        }



        private void billworker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.dataCon.reportTransactionList = repo.GetTransactionList(this.dataCon.DateFrom, this.dataCon.DateTo);
                break;
            }
        }

        private void billworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.DataContext = this.dataCon;

            this.dataCon.TotalWaterBill = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.WaterBill, 2, MidpointRounding.AwayFromZero));
            this.dataCon.TotalDiscount = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.Discount, 2, MidpointRounding.AwayFromZero));
            this.dataCon.TotalGarbage = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.GarbageCollection, 2, MidpointRounding.AwayFromZero));
            this.dataCon.TotalMonthlyDues = this.dataCon.reportTransactionList.Sum(x => Math.Round(x.MonthlyDues, 2, MidpointRounding.AwayFromZero));

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
            try
            {
                billworker.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }
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


        private Decimal _TotalWaterBill;
        public Decimal TotalWaterBill
        {
            get
            {
                return _TotalWaterBill;
            }
            set
            {
                if (value != _TotalWaterBill)
                {
                    _TotalWaterBill = value;
                    OnPropertyChanged("TotalWaterBill");
                }
            }
        }

        private Decimal _TotalDiscount;
        public Decimal TotalDiscount
        {
            get
            {
                return _TotalDiscount;
            }
            set
            {
                if (value != _TotalDiscount)
                {
                    _TotalDiscount = value;
                    OnPropertyChanged("TotalDiscount");
                }
            }
        }


        private Decimal _TotalGarbage;
        public Decimal TotalGarbage
        {
            get
            {
                return _TotalGarbage;
            }
            set
            {
                if (value != _TotalGarbage)
                {
                    _TotalGarbage = value;
                    OnPropertyChanged("TotalGarbage");
                }
            }
        }


        private Decimal _TotalMonthlyDues;
        public Decimal TotalMonthlyDues
        {
            get
            {
                return _TotalMonthlyDues;
            }
            set
            {
                if (value != _TotalMonthlyDues)
                {
                    _TotalMonthlyDues = value;
                    OnPropertyChanged("TotalMonthlyDues");
                }
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
