using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WaterBilling.Models.Billing;
using WaterBilling.Windows;
using WaterBillingProject.Models.Collection;
using WaterBillingProject.Repository;

namespace WaterBillingProject.Pages
{
    /// <summary>
    /// Interaction logic for CollectionPage.xaml
    /// </summary>
    public partial class CollectionPage : Page
    {
        CollectionDataContext dataCon;
        BackgroundWorker worker = new BackgroundWorker();
        CollectionRepository repo = new CollectionRepository();

        public CollectionPage()
        {
            InitializeComponent();
            InitializeWorkers();
            dataCon = new CollectionDataContext();
            this.DataContext = this.dataCon;
            this.btn_Find.Focus();
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
                this.dataCon.tempChargesList = this.repo.GetCharges(this.dataCon.ClientID);
                this.dataCon.BillingList = this.repo.GetBillingList(this.dataCon.ClientID);
                this.dataCon.TempdiscountList = this.repo.GetDiscount(this.dataCon.ClientID);

                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            populateCharges();
            populateDiscount();
            Compute();
            myCurrencyTextBox.Focus();
        }


        private void populateCharges()
        {
            var tempcharge = this.dataCon.tempChargesList.GroupBy(c => c.COAID)
                                                        .Select(cl => new
                                                        {
                                                            COAID = cl.Key,
                                                            Description = cl.First().Description,
                                                            Amount = cl.Sum(ca => ca.Amount),
                                                        }
                                                    ).ToList();

            List<CollectionChargesClass> tempBillingCharge = new List<CollectionChargesClass>();
            CollectionChargesClass temp;
            foreach (var item in tempcharge)
            {
                temp = new CollectionChargesClass();
                temp.Description = item.Description;
                temp.Amount = item.Amount;
                temp.COAID = item.COAID;

                tempBillingCharge.Add(temp);
            }

            this.dataCon.chargesList = tempBillingCharge;
        }

        private void populateDiscount()
        {
            var tempcharge = this.dataCon.TempdiscountList.GroupBy(c => c.COAID)
                                                        .Select(cl => new
                                                        {
                                                            COAID = cl.Key,
                                                            Description = cl.First().Description,
                                                            Amount = cl.Sum(ca => ca.Amount),
                                                        }
                                                    ).ToList();

            List<CollectionDiscountClass> tempBillingDiscount = new List<CollectionDiscountClass>();
            CollectionDiscountClass temp;
            foreach (var item in tempcharge)
            {
                temp = new CollectionDiscountClass();
                temp.Description = item.Description;
                temp.Amount = item.Amount;
                temp.COAID = item.COAID;

                tempBillingDiscount.Add(temp);
            }

            this.dataCon.discountList = tempBillingDiscount;
        }



        private void Compute()
        {
            Decimal Charges = 0;
            Decimal Bills = 0;
            Decimal Discount = 0;

            foreach (var item in this.dataCon.BillingList)
            {
                Bills += item.CurrentDue;
            }

            foreach (var item in this.dataCon.chargesList)
            {
                Charges += item.Amount;
            }

            foreach (var item in this.dataCon.discountList)
            {
                Discount += item.Amount;
            }


            this.dataCon.TotalDue = (Bills + Charges + this.dataCon.Interest) - Discount;

        }


        private void btn_Generate_Click(object sender, RoutedEventArgs e)
        {
            findClientFunction();
        }
        private void Executed_Find(object sender, ExecutedRoutedEventArgs e)
        {
            findClientFunction();
        }

        private void findClientFunction()
        {
            this.dataCon.Interest = 0;
            this.dataCon.TenderedAmount = 0;
            this.dataCon.Change = 0;
            var dialog = new ClientListWindow();
            if (dialog.ShowDialog() == true)
            {
                this.dataCon.ClientID = dialog.clientSelected.ClientID;
                this.dataCon.Fullname = dialog.clientSelected.FullName;
                this.dataCon.FullAddress = dialog.clientSelected.FullAddress;


                try
                {
                    worker.RunWorkerAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void myCurrencyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.dataCon.Change = this.dataCon.TenderedAmount - this.dataCon.TotalDue;
                if (this.dataCon.Change < 0)
                {
                    this.dataCon.Change = 0;
                }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Keyboard.IsKeyDown(Key.F))
                {
                    findClientFunction();
                }
            }
            else
            {
                this.dataCon.Change = 0;

            }
            
        }
    }//end of Collection Page


    public class CollectionDataContext : INotifyPropertyChanged
    {

        List<CollectionChargesClass> _chargesList;
        public List<CollectionChargesClass> chargesList
        {
            get { return _chargesList; }
            set
            {
                _chargesList = value;
                OnPropertyChanged("chargesList");
            }
        }


        List<CollectionChargesClass> _tempChargesList;
        public List<CollectionChargesClass> tempChargesList
        {
            get { return _tempChargesList; }
            set
            {
                _tempChargesList = value;
                OnPropertyChanged("tempChargesList");
            }
        }




        List<CollectionDiscountClass> _TempdiscountList;
        public List<CollectionDiscountClass> TempdiscountList
        {
            get { return _TempdiscountList; }
            set
            {
                _TempdiscountList = value;
                OnPropertyChanged("TempdiscountList");
            }
        }


        List<CollectionDiscountClass> _discountList;
        public List<CollectionDiscountClass> discountList
        {
            get { return _discountList; }
            set
            {
                _discountList = value;
                OnPropertyChanged("discountList");
            }
        }



        List<CollectionBillsClass> _BillingList;
        public List<CollectionBillsClass> BillingList
        {
            get { return _BillingList; }
            set
            {
                _BillingList = value;
                OnPropertyChanged("BillingList");
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


        private String _Fullname;
        public String Fullname
        {
            get
            {
                return _Fullname;
            }
            set
            {
                if (value != _Fullname)
                {
                    _Fullname = value;
                    OnPropertyChanged("Fullname");
                }
            }
        }



        private String _FullAddress;
        public String FullAddress
        {
            get
            {
                return _FullAddress;
            }
            set
            {
                if (value != _FullAddress)
                {
                    _FullAddress = value;
                    OnPropertyChanged("FullAddress");
                }
            }
        }


        private Decimal _TenderedAmount;
        public Decimal TenderedAmount
        {
            get
            {
                return _TenderedAmount;
            }
            set
            {
                if (value != _TenderedAmount)
                {
                    _TenderedAmount = value;
                    OnPropertyChanged("TenderedAmount");
                }
            }
        }



        private Decimal _TotalDue;
        public Decimal TotalDue
        {
            get
            {
                return _TotalDue;
            }
            set
            {
                if (value != _TotalDue)
                {
                    _TotalDue = value;
                    OnPropertyChanged("TotalDue");
                }
            }
        }


        private Decimal _Interest;
        public Decimal Interest
        {
            get
            {
                return _Interest;
            }
            set
            {
                if (value != _Interest)
                {
                    _Interest = value;
                    OnPropertyChanged("Interest");
                }
            }
        }


        private Decimal _Change;
        public Decimal Change
        {
            get
            {
                return _Change;
            }
            set
            {
                if (value != _Change)
                {
                    _Change = value;
                    OnPropertyChanged("Change");
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
