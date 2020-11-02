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
using WaterBillingProject.Services;
using WaterBillingProject.Windows;

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
                this.dataCon.TempdiscountList = this.repo.GetDiscount();

                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            populateCharges();
            getDiscount();
            //DeductDiscount();
            populateDiscount();
            getInterest();
            Compute();
            myCurrencyTextBox.Focus();
        }



        private void getInterest()
        {
            if (this.dataCon.BillingList.Count >= 2)
            {
                int days = 0;
                decimal TotalAmountDue = 0;


                DateTime date = new DateTime();

                foreach (var item in this.dataCon.BillingList)
                {
                    try
                    {
                        date = DateTime.ParseExact(item.BillMonth, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
                        //MessageBox.Show(date.ToString("MM-dd-yyyy"));
                        days += DateTime.DaysInMonth(date.Year, date.Month);

                    }
                    catch (Exception ex)
                    {

                    }
                    TotalAmountDue += item.CurrentDue;
                }
                days += DateTime.Now.Day;
                Decimal multiple = Convert.ToDecimal(0.05);
                decimal divideDays = Math.Round(Convert.ToDecimal(days) / Convert.ToDecimal(360), 2, MidpointRounding.AwayFromZero);
                this.dataCon.Interest = Math.Round((TotalAmountDue * multiple) * divideDays, 2, MidpointRounding.AwayFromZero);

            }
            
        }

        private void getDiscount()
        {
            List<CollectionDiscountClass> temporary = new List<CollectionDiscountClass>();

            if (this.dataCon.BillingList.Count == 1)
            {
                if (LoginSession.TransDate.Day < 16)
                {
                    decimal totalBillDue = 0;
                    decimal discount = 0;
                    foreach (var item in this.dataCon.BillingList)
                    {
                        totalBillDue += item.CurrentDue;
                    }

                    foreach (var item in this.dataCon.TempdiscountList)
                    {
                        discount = Math.Round(totalBillDue * Convert.ToDecimal(item.Formula), 2, MidpointRounding.AwayFromZero);
                        if (discount > 0)
                        {
                            temporary.Add(new CollectionDiscountClass
                            {
                                SLC_CODE = item.SLC_CODE,
                                SLT_CODE = item.SLT_CODE,
                                SLE_CODE = item.SLE_CODE,
                                StatusID = item.StatusID,
                                Description = item.Description,
                                COAID = item.COAID,
                                ReferenceNo = this.dataCon.BillingList.Select(o => o.ReferenceNo).FirstOrDefault(),
                                Amount = discount,
                                BillMonth = this.dataCon.BillingList.Select(o => o.BillMonth).FirstOrDefault(),
                            });
                        }
                        
                    }

                }

                this.dataCon.TempdiscountList = temporary;


            }
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

        private void DeductDiscount()
        {
            if (this.dataCon.TempdiscountList.Count > 0)
            {
                Decimal discount = 0;
                foreach (var item in this.dataCon.TempdiscountList)
                {
                    discount += item.Amount;
                }
                List<CollectionBillsClass> bill = new List<CollectionBillsClass>();
                foreach (var item in this.dataCon.BillingList)
                {
                    bill.Add(new CollectionBillsClass 
                    {
                        SLC_CODE = item.SLC_CODE,
                        SLT_CODE = item.SLT_CODE,
                        BillMonth = item.BillMonth,
                        ReferenceNo = item.ReferenceNo,
                        Consumption = item.Consumption,
                        CurrentDue = item.CurrentDue - discount,
                        SL_Description = item.SL_Description,
                    });
                }

                this.dataCon.BillingList = bill;
            }
          
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

        private void ComputeChange()
        {
            this.dataCon.Change = this.dataCon.TenderedAmount - this.dataCon.TotalDue;
            if (this.dataCon.Change < 0)
            {
                this.dataCon.Change = 0;
            }
        }
        private void myCurrencyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ComputeChange();
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Keyboard.IsKeyDown(Key.F))
                {
                    findClientFunction();
                }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                if (Keyboard.IsKeyDown(Key.F))
                {
                    TransactionList billingList = new TransactionList();
                    billingList.ShowDialog();
                }
            }
            else if (e.Key == Key.F11)
            {
                SaveTransation();
            }
            else if(e.Key == Key.Escape)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to cancel your work?", "CONFIRMATION", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Refresh();
                }
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveTransation();
        }


        private void SaveTransation()
        {
            ComputeChange();
            TransactionSummaryClass tranSummary = SetTransactionSummary();
            List<TransactionDetailClass> transactionDetail = SetTransactionDetails();
            List<TransactionCheckClass> transactionCheck = SetTransactionCheck();
            BillUpdateClass billUpdate = SetBillingUpdate();

            PostingEntryWindow postingEntries = new PostingEntryWindow(tranSummary, transactionDetail, transactionCheck, billUpdate);
            postingEntries.ShowDialog();
            Refresh();

        }

        private TransactionSummaryClass SetTransactionSummary()
        {
            TransactionSummaryClass tranSummary = new TransactionSummaryClass();

            try
            {
                tranSummary.TransactionCode = 1;
                tranSummary.TransYear = LoginSession.TransYear;
                tranSummary.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                tranSummary.ClientID = this.dataCon.ClientID;
                tranSummary.Explanation = "Payment of : " + this.dataCon.Fullname;
                tranSummary.PostedBy = LoginSession.UserID;
                return tranSummary;

            }
            catch (Exception ex)
            {
                return tranSummary;
            }
        }

        private List<TransactionDetailClass> SetTransactionDetails()
        {
            List<TransactionDetailClass> transDT = new List<TransactionDetailClass>();
            try
            {
                Decimal TotalTendered = this.dataCon.TenderedAmount;
                Decimal TotalDiscount = 0;

                TransactionDetailClass TellerEntry;
                TellerEntry = new TransactionDetailClass();
                TellerEntry.TransactionCode = 1;
                TellerEntry.TransYear = LoginSession.TransYear;
                TellerEntry.AccountCode = 100101;
                TellerEntry.ClientID = this.dataCon.ClientID;
                TellerEntry.BillMonth = "";
                TellerEntry.SLC_CODE = 11;
                TellerEntry.SLT_CODE = 1;
                TellerEntry.ReferenceNo = "";
                TellerEntry.SLE_CODE = 11;
                TellerEntry.StatusID = 15;
                TellerEntry.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                TellerEntry.Amt = this.dataCon.TenderedAmount <= this.dataCon.TotalDue ? this.dataCon.TenderedAmount : this.dataCon.TotalDue;
                TellerEntry.PostedBy = LoginSession.UserID;
                TellerEntry.UPDTag = 1;
                TellerEntry.ClientName = this.dataCon.Fullname;
                TellerEntry.SL_Description = "Cash - on - hand";

                transDT.Add(TellerEntry);



                if (this.dataCon.Interest > 0)
                {
                    TransactionDetailClass interestEntry;
                    interestEntry = new TransactionDetailClass();
                    interestEntry.TransactionCode = 1;
                    interestEntry.TransYear = LoginSession.TransYear;
                    interestEntry.AccountCode = 402104;
                    interestEntry.ClientID = this.dataCon.ClientID;
                    interestEntry.BillMonth = "";
                    interestEntry.SLC_CODE = 14;
                    interestEntry.SLT_CODE = 2;
                    interestEntry.ReferenceNo = "";
                    interestEntry.SLE_CODE = 11;
                    interestEntry.StatusID = 15;
                    interestEntry.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                    interestEntry.Amt = TotalTendered >= this.dataCon.Interest ?  this.dataCon.Interest *-1 : TotalTendered * -1;
                    interestEntry.PostedBy = LoginSession.UserID;
                    interestEntry.UPDTag = 1;
                    interestEntry.ClientName = this.dataCon.Fullname;
                    interestEntry.SL_Description = "Water Bill Interest";

                    transDT.Add(interestEntry);

                    TotalTendered -= this.dataCon.Interest;
                }


                if (TotalTendered > 0)
                {
                    TransactionDetailClass billsCharges;
                    foreach (var item in this.dataCon.tempChargesList)
                    {
                        if (TotalTendered > 0 && item.Amount > 0)
                        {
                            billsCharges = new TransactionDetailClass();
                            billsCharges.TransactionCode = 1;
                            billsCharges.TransYear = LoginSession.TransYear;
                            billsCharges.AccountCode = item.COAID;
                            billsCharges.ClientID = this.dataCon.ClientID;
                            billsCharges.BillMonth = item.BillMonth;
                            billsCharges.SLC_CODE = item.SLC_CODE;
                            billsCharges.SLT_CODE = item.SLT_CODE;
                            billsCharges.ReferenceNo = item.ReferenceNo;
                            billsCharges.SLE_CODE = 11;
                            billsCharges.StatusID = 15;
                            billsCharges.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                            billsCharges.Amt = TotalTendered >= item.Amount ? item.Amount * -1 : TotalTendered * -1;
                            billsCharges.PostedBy = LoginSession.UserID;
                            billsCharges.UPDTag = 1;
                            billsCharges.ClientName = this.dataCon.Fullname;
                            billsCharges.SL_Description = item.Description;

                            transDT.Add(billsCharges);
                        }

                        TotalTendered -= item.Amount;
                    }
                }


                if (TotalTendered > 0)
                {
                    TransactionDetailClass billsDiscount;
                    foreach (var item in this.dataCon.TempdiscountList)
                    {
                        if (item.Amount > 0)
                        {
                            billsDiscount = new TransactionDetailClass();
                            billsDiscount.TransactionCode = 1;
                            billsDiscount.TransYear = LoginSession.TransYear;
                            billsDiscount.AccountCode = item.COAID;
                            billsDiscount.ClientID = this.dataCon.ClientID;
                            billsDiscount.BillMonth = item.BillMonth;
                            billsDiscount.SLC_CODE = item.SLC_CODE;
                            billsDiscount.SLT_CODE = item.SLT_CODE;
                            billsDiscount.ReferenceNo = item.ReferenceNo;
                            billsDiscount.SLE_CODE = 11;
                            billsDiscount.StatusID = 15;
                            billsDiscount.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                            billsDiscount.Amt = item.Amount * -1;
                            billsDiscount.PostedBy = LoginSession.UserID;
                            billsDiscount.UPDTag = 1;
                            billsDiscount.ClientName = this.dataCon.Fullname;
                            billsDiscount.SL_Description = item.Description;

                            transDT.Add(billsDiscount);
                            TotalDiscount += item.Amount;
                        }
                        
                    }

                    if (TotalDiscount > 0)
                    {
                        TransactionDetailClass totalDiscountClass;
                        totalDiscountClass = new TransactionDetailClass();
                        totalDiscountClass.TransactionCode = 3;
                        totalDiscountClass.TransYear = LoginSession.TransYear;
                        totalDiscountClass.AccountCode = 401;
                        totalDiscountClass.ClientID = 0;
                        totalDiscountClass.BillMonth = "";
                        totalDiscountClass.SLC_CODE = 0;
                        totalDiscountClass.SLT_CODE = 0;
                        totalDiscountClass.ReferenceNo = "";
                        totalDiscountClass.SLE_CODE = 0;
                        totalDiscountClass.StatusID = 0;
                        totalDiscountClass.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                        totalDiscountClass.Amt = TotalDiscount;
                        totalDiscountClass.PostedBy = LoginSession.UserID;
                        totalDiscountClass.UPDTag = 1;
                        totalDiscountClass.ClientName = this.dataCon.Fullname;
                        totalDiscountClass.SL_Description = "Discount";
                        transDT.Add(totalDiscountClass);
                    }
                   



                    TransactionDetailClass billsTrans;
                    foreach (var item in this.dataCon.BillingList)
                    {

                        if (TotalTendered > 0)
                        {
                            decimal amount = item.CurrentDue - TotalDiscount;

                            billsTrans = new TransactionDetailClass();

                            billsTrans.TransactionCode = 1;
                            billsTrans.TransYear = LoginSession.TransYear;
                            billsTrans.AccountCode = 402101;
                            billsTrans.ClientID = this.dataCon.ClientID;
                            billsTrans.BillMonth = item.BillMonth;
                            billsTrans.SLC_CODE = item.SLC_CODE;
                            billsTrans.SLT_CODE = item.SLT_CODE;
                            billsTrans.ReferenceNo = item.ReferenceNo;
                            billsTrans.SLE_CODE = 11;
                            billsTrans.StatusID = 15;
                            billsTrans.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                            billsTrans.Amt = TotalTendered >= amount  ? amount * -1 : TotalTendered * -1;
                            billsTrans.PostedBy = LoginSession.UserID;
                            billsTrans.UPDTag = 1;
                            billsTrans.ClientName = this.dataCon.Fullname;
                            billsTrans.SL_Description = item.SL_Description;

                            transDT.Add(billsTrans);

                            TotalTendered -= amount;
                        }


                    }
                }

                


                



                


                return transDT;
            }
            catch (Exception ex)
            {
                return transDT;
            }
        }

        private List<TransactionCheckClass> SetTransactionCheck()
        {
            List<TransactionCheckClass> tranCheck = new List<TransactionCheckClass>();

            try
            {
                tranCheck.Add(new TransactionCheckClass
                {
                    TransactionCode = 1,
                    TransYear = LoginSession.TransYear,
                    COCIType = 1,
                    Amt = this.dataCon.TenderedAmount,
                });

                if (this.dataCon.Change > 0)
                {
                    tranCheck.Add(new TransactionCheckClass
                    {
                        TransactionCode = 1,
                        TransYear = LoginSession.TransYear,
                        COCIType = 1,
                        Amt = this.dataCon.Change * -1,
                    });
                }

                return tranCheck;

            }
            catch (Exception ex)
            {
                return tranCheck;
            }
        }

        private BillUpdateClass SetBillingUpdate()
        {
            BillUpdateClass updClass = new BillUpdateClass();

            try
            {
                updClass.ClientID = this.dataCon.ClientID;
                int counter = 0;
                string Last = "";

                foreach (var item in this.dataCon.BillingList)
                {
                    counter++;
                    if (counter == this.dataCon.BillingList.Count)
                    {
                        Last = "";
                    }
                    else
                    {
                        Last = ",";
                    }


                    updClass.ReferenceNo += "'" + item.ReferenceNo + "'" + Last;
                }

                return updClass;

            }
            catch (Exception ex)
            {
                return updClass;
            }
        }

        private void Executed_Post(object sender, ExecutedRoutedEventArgs e)
        {
            SaveTransation();
        }




        private void Refresh()
        {
            List<CollectionChargesClass> _chargesList = new List<CollectionChargesClass>();
            this.dataCon.chargesList = _chargesList;
            List<CollectionChargesClass> _tempChargesList = new List<CollectionChargesClass>();
            this.dataCon.tempChargesList = _tempChargesList;


            List <CollectionDiscountClass> _TempdiscountList = new List<CollectionDiscountClass>();
            this.dataCon.TempdiscountList = _TempdiscountList;
            List<CollectionDiscountClass> _discountList = new List<CollectionDiscountClass>();
            this.dataCon.discountList = _discountList;


            List<CollectionBillsClass> _BillingList = new List<CollectionBillsClass>();
            this.dataCon.BillingList = _BillingList;

            this.dataCon.ClientID = 0;
            this.dataCon.Fullname = "";
            this.dataCon.FullAddress = "";
            this.dataCon.TenderedAmount = 0;
            this.dataCon.TotalDue = 0;
            this.dataCon.Interest = 0;
            this.dataCon.Change = 0;


        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to cancel your work?", "CONFIRMATION", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Refresh();
            }
                
        }

        private void Executed_FindTransaction(object sender, ExecutedRoutedEventArgs e)
        {
            TransactionList billingList = new TransactionList();
            billingList.ShowDialog();

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
