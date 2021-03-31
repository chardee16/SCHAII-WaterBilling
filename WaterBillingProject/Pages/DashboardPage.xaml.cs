using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
using WaterBillingProject.Models.Dashboard;
using WaterBillingProject.Repository;

namespace WaterBilling.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {

        DashboardDataContext dataCon;
        BackgroundWorker worker = new BackgroundWorker();
        DashboardRepository repo = new DashboardRepository();

        public DashboardPage()
        {
            InitializeComponent();
            InitializeWorkers();
            dataCon = new DashboardDataContext();
            this.DataContext = this.dataCon;

            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }

            //this.dataCon.SeriesCollection = new SeriesCollection
            //{
            //    new ColumnSeries
            //    {
            //        Title = "Total Cu.M\nUsed",
            //        Values = new ChartValues<double> { 10, 50, 39, 50 },
            //        Fill = Brushes.DarkRed
            //    }
            //};

            ////adding series will update and animate the chart automatically


            ////also adding values updates and animates the chart automatically


            //this.dataCon.Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            //this.dataCon.Formatter = value => value.ToString("N");

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
                this.dataCon.MonthlyConsumptionList = repo.GetMonthlyConsumption();
                this.dataCon.dashboardValues = repo.GetDashboardValues();

                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            populateBarGraph();
            this.dataCon.Consumption = this.dataCon.dashboardValues.Consumption;
            this.dataCon.PaymentReceived = this.dataCon.dashboardValues.PaymentReceived;
            this.dataCon.ChargesReceived = this.dataCon.dashboardValues.ChargesReceived;
        }



        private void populateBarGraph()
        {
            try
            {

                long[] chartValues = new long[12];
                String[] labelValues = new string[12];
                int count = 0;
                foreach (var item in this.dataCon.MonthlyConsumptionList)
                {
                    chartValues[count] = Convert.ToInt64(item.Consumption);
                    labelValues[count] = item.MonthDescription;
                    count++;
                }


                dataCon.SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Total Cu.M\nUsed",
                        Values = chartValues.AsChartValues(),
                        Fill = Brushes.IndianRed,
                    }
                };

                this.dataCon.Labels = labelValues;
                this.dataCon.Formatter = value => value.ToString("N");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }



    }//end of dashboard page



    public class DashboardDataContext : INotifyPropertyChanged
    {

        private DashboardValues _dashboardValuest;
        public DashboardValues dashboardValues
        {
            get { return _dashboardValuest; }
            set
            {
                _dashboardValuest = value;
                OnPropertyChanged("dashboardValues");
            }
        }


        private Int64 _Consumption;
        public Int64 Consumption
        {
            get
            {
                return _Consumption;
            }
            set
            {
                if (value != _Consumption)
                {
                    _Consumption = value;
                    OnPropertyChanged("Consumption");
                }
            }
        }



        private Decimal _PaymentReceived;
        public Decimal PaymentReceived
        {
            get
            {
                return _PaymentReceived;
            }
            set
            {
                if (value != _PaymentReceived)
                {
                    _PaymentReceived = value;
                    OnPropertyChanged("PaymentReceived");
                }
            }
        }



        private Decimal _ChargesReceived;
        public Decimal ChargesReceived
        {
            get
            {
                return _ChargesReceived;
            }
            set
            {
                if (value != _ChargesReceived)
                {
                    _ChargesReceived = value;
                    OnPropertyChanged("ChargesReceived");
                }
            }
        }



        private string[] _Labels;
        public string[] Labels
        {
            get { return _Labels; }
            set
            {
                _Labels = value;
                OnPropertyChanged("Labels");
            }
        }

        private Func<Int32, string> _Formatter;
        public Func<Int32, string> Formatter
        {
            get { return _Formatter; }
            set
            {
                _Formatter = value;
                OnPropertyChanged("Formatter");
            }
        }


        private SeriesCollection _SeriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _SeriesCollection; }
            set
            {
                _SeriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }

        private List<DashboardMonthlyConsumption> _MonthlyConsumptionList;
        public List<DashboardMonthlyConsumption> MonthlyConsumptionList
        {
            get { return _MonthlyConsumptionList; }
            set
            {
                _MonthlyConsumptionList = value;
                OnPropertyChanged("MonthlyConsumptionList");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

    }






}
