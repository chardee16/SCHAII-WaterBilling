using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using WaterBilling.Repository;
using WaterBillingProject.Models.ClientMaster;

namespace WaterBillingProject.Windows
{
    /// <summary>
    /// Interaction logic for PaymentHistoryWindow.xaml
    /// </summary>
    public partial class PaymentHistoryWindow : Window
    {

        PaymentHistoryDataContext dataCon = new PaymentHistoryDataContext();
        ClientMasterRepository repo = new ClientMasterRepository();
        BackgroundWorker worker = new BackgroundWorker();

        public PaymentHistoryWindow(Int64 ClientID)
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
                this.dataCon.reportTransactionList = this.repo.GetPaymentHistoryList(this.dataCon.ClientID);
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }




        public class PaymentHistoryDataContext : INotifyPropertyChanged
        {
            List<PaymentHistoryClass> _reportTransactionList;
            public List<PaymentHistoryClass> reportTransactionList
            {
                get { return _reportTransactionList; }
                set
                {
                    _reportTransactionList = value;
                    OnPropertyChanged("reportTransactionList");
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
