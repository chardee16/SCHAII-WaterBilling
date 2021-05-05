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
using WaterBilling.Repository;
using WaterBillingProject.Models.ClientMaster;

namespace WaterBillingProject.Windows
{
    /// <summary>
    /// Interaction logic for BillingHistoryWindow.xaml
    /// </summary>
    public partial class BillingHistoryWindow : Window
    {
        BillingHistoryDataContext dataCon = new BillingHistoryDataContext();
        ClientMasterRepository repo = new ClientMasterRepository();
        BackgroundWorker worker = new BackgroundWorker();

        public BillingHistoryWindow(Int64 ClientID)
        {
            InitializeComponent();
            InitializeWorkers();
            this.dataCon.ClientID = ClientID;
            this.DataContext = this.dataCon;

            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }

        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                this.dataCon.billingList = this.repo.GetBillingHistoryList(this.dataCon.ClientID);
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }









        public class BillingHistoryDataContext : INotifyPropertyChanged
        {
            List<BillingHistoryClass> _billingList;
            public List<BillingHistoryClass> billingList
            {
                get { return _billingList; }
                set
                {
                    _billingList = value;
                    OnPropertyChanged("billingList");
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

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string property)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }



    }
}
