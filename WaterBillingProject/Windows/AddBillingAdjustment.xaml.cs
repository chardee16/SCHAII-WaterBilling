using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WaterBilling.Models.Billing;
using WaterBilling.Repository;
using WaterBillingProject.Models.Collection;
using WaterBillingProject.Services;

namespace WaterBillingProject.Windows
{
    /// <summary>
    /// Interaction logic for AddBillingAdjustment.xaml
    /// </summary>
    public partial class AddBillingAdjustment : Window
    {
        BillingRepository repo = new BillingRepository();
        addBillingDataContext dataCon = new addBillingDataContext();
        public AddBillingAdjustment(Int64 ClientID,String Fullname)
        {
            InitializeComponent();
            this.DataContext = this.dataCon;

            this.dataCon.ClientID = ClientID;
            this.dataCon.Fullname = Fullname;

            txt_Billmonth.Focus();

        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleKeys(object sender, KeyEventArgs e)
        {

        }

        private void btn_AddBill_Click(object sender, RoutedEventArgs e)
        {
            if (Checker())
            {
                SaveFunction();
            }
        }




        private void SaveFunction()
        {
            try
            {
                CreateBillClass billing = new CreateBillClass();
                if (this.dataCon.ClientID != 0)
                {

                    billing.SLC_CODE = 14;
                    billing.SLT_CODE = 1;
                    billing.ClientID = this.dataCon.ClientID;
                    billing.CurrentDue = this.dataCon.TotalDue;
                    billing.BillMonth = this.dataCon.BillMonth;
                    billing.TR_Date = string.Format("{0}-{1}-{2}", string.Format("{0:D4}", string.Format("{0:D4}", this.dataCon.BillMonth.Substring(0,4))), string.Format("{0:D2}", this.dataCon.BillMonth.Substring(4)), string.Format("{0:D2}", DateTime.Now.Day));
                    billing.BillStatus = 1;
                    billing.TR_CODE = 4;
                    billing.Consumption = 0;
                    billing.ExcessConsumption = 0;
                    billing.dueWithDiscount = 0;
                    billing.CurrentReading = 0;
                    billing.PreviousReading = 0;
                    billing.ChargesList = getCharges();
                    //billing.DiscountList = getDiscount();
                    billing.transummary = SetTransactionSummary();
                    billing.transdetail = SetTransactionDetails();

                    if (this.repo.GetExistingMonth(billing.ClientID, billing.BillMonth))
                    {
                        MessageBox.Show("Client has a bill already for this month.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (this.repo.SaveBillAdjustment(billing))
                        {

                            MessageBox.Show("Bill Successfully saved.", "Information", System.Windows.MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Something went wrong. \nPlease contact administrator.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Must select client!.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong. \nPlease contact administrator.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }





        private TransactionSummaryClass SetTransactionSummary()
        {
            TransactionSummaryClass tranSummary = new TransactionSummaryClass();

            try
            {
                tranSummary.TransactionCode = 4;
                tranSummary.TransYear = LoginSession.TransYear;
                tranSummary.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                tranSummary.ClientID = this.dataCon.ClientID;
                tranSummary.Explanation = "Billing Setup of : " + this.dataCon.Fullname;
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
                Decimal balancingEntry = 0;

                TransactionDetailClass billsTrans;

                billsTrans = new TransactionDetailClass();
                billsTrans.TransactionCode = 4;
                billsTrans.TransYear = LoginSession.TransYear;
                billsTrans.AccountCode = 402101;
                billsTrans.ClientID = this.dataCon.ClientID;
                billsTrans.BillMonth = "";
                billsTrans.SLC_CODE = 14;
                billsTrans.SLT_CODE = 1;
                billsTrans.ReferenceNo = "";
                billsTrans.SLE_CODE = 11;
                billsTrans.StatusID = 15;
                billsTrans.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                billsTrans.Amt = this.dataCon.TotalDue;
                billsTrans.PostedBy = LoginSession.UserID;
                billsTrans.UPDTag = 1;
                billsTrans.ClientName = this.dataCon.Fullname;
                billsTrans.SL_Description = "";

                transDT.Add(billsTrans);



                TransactionDetailClass billMonthlyDue;

                billMonthlyDue = new TransactionDetailClass();
                billMonthlyDue.TransactionCode = 4;
                billMonthlyDue.TransYear = LoginSession.TransYear;
                billMonthlyDue.AccountCode = 402102;
                billMonthlyDue.ClientID = this.dataCon.ClientID;
                billMonthlyDue.BillMonth = "";
                billMonthlyDue.SLC_CODE = 15;
                billMonthlyDue.SLT_CODE = 1;
                billMonthlyDue.ReferenceNo = "";
                billMonthlyDue.SLE_CODE = 11;
                billMonthlyDue.StatusID = 15;
                billMonthlyDue.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                billMonthlyDue.Amt = this.dataCon.MonthlyDue;
                billMonthlyDue.PostedBy = LoginSession.UserID;
                billMonthlyDue.UPDTag = 1;
                billMonthlyDue.ClientName = this.dataCon.Fullname;
                billMonthlyDue.SL_Description = "";

                transDT.Add(billMonthlyDue);


                TransactionDetailClass GarbageDue;

                GarbageDue = new TransactionDetailClass();
                GarbageDue.TransactionCode = 4;
                GarbageDue.TransYear = LoginSession.TransYear;
                GarbageDue.AccountCode = 402103;
                GarbageDue.ClientID = this.dataCon.ClientID;
                GarbageDue.BillMonth = "";
                GarbageDue.SLC_CODE = 15;
                GarbageDue.SLT_CODE = 2;
                GarbageDue.ReferenceNo = "";
                GarbageDue.SLE_CODE = 11;
                GarbageDue.StatusID = 15;
                GarbageDue.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                GarbageDue.Amt = this.dataCon.MonthlyDue;
                GarbageDue.PostedBy = LoginSession.UserID;
                GarbageDue.UPDTag = 1;
                GarbageDue.ClientName = this.dataCon.Fullname;
                GarbageDue.SL_Description = "";

                transDT.Add(GarbageDue);




                TransactionDetailClass TellerEntry;
                TellerEntry = new TransactionDetailClass();
                TellerEntry.TransactionCode = 4;
                TellerEntry.TransYear = LoginSession.TransYear;
                TellerEntry.AccountCode = 100201;
                TellerEntry.ClientID = this.dataCon.ClientID;
                TellerEntry.BillMonth = "";
                TellerEntry.SLC_CODE = 12;
                TellerEntry.SLT_CODE = 1;
                TellerEntry.ReferenceNo = "";
                TellerEntry.SLE_CODE = 11;
                TellerEntry.StatusID = 15;
                TellerEntry.TransactionDate = LoginSession.TransDate.ToString("yyyy-MM-dd");
                TellerEntry.Amt = (this.dataCon.TotalDue + this.dataCon.MonthlyDue + this.dataCon.GarbageDue) * -1;
                TellerEntry.PostedBy = LoginSession.UserID;
                TellerEntry.UPDTag = 1;
                TellerEntry.ClientName = this.dataCon.Fullname;
                TellerEntry.SL_Description = "Balancing Entry";

                transDT.Add(TellerEntry);


                return transDT;
            }
            catch (Exception ex)
            {
                return transDT;
            }
        }




        private List<ChargesClass> getCharges()
        {
            List<ChargesClass> toReturn = new List<ChargesClass>();
            try
            {
                //monthly due
                toReturn.Add(new ChargesClass
                {
                    SLC_CODE = 15,
                    SLT_CODE = 1,
                    AccountCode = 402102,
                    SL_Description = "Monthly Dues",
                    Amount = this.dataCon.MonthlyDue,
                }) ;

                //Garbage
                toReturn.Add(new ChargesClass
                {
                    SLC_CODE = 15,
                    SLT_CODE = 2,
                    AccountCode = 402103,
                    SL_Description = "Garbage Collection",
                    Amount = this.dataCon.GarbageDue,
                });


                return toReturn;
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }




        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private Boolean Checker()
        {
            if (String.IsNullOrEmpty(this.dataCon.BillMonth))
            {
                MessageBox.Show("Bill Month is in incorrect format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                if (this.dataCon.BillMonth.Length != 6)
                {
                    MessageBox.Show("Bill Month is in incorrect format.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    int str = Convert.ToInt32(this.dataCon.BillMonth.Substring(4));
                    if (str > 12)
                    {
                        MessageBox.Show("Bill Month is in incorrect format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        if (this.dataCon.TotalDue <= 0)
                        {
                            MessageBox.Show("Total due must have value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        else if (this.dataCon.MonthlyDue > 50)
                        {
                            MessageBox.Show("Monthly Due must not exceed P50.00.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        else if (this.dataCon.GarbageDue > 30)
                        {
                            MessageBox.Show("Garbage Due must not exceed P30.00.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }




    }


    public class addBillingDataContext : INotifyPropertyChanged
    {
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


        private String _BillMonth;
        public String BillMonth
        {
            get
            {
                return _BillMonth;
            }
            set
            {
                if (value != _BillMonth)
                {
                    _BillMonth = value;
                    OnPropertyChanged("BillMonth");
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


        private Decimal _MonthlyDue;
        public Decimal MonthlyDue
        {
            get
            {
                return _MonthlyDue;
            }
            set
            {
                if (value != _MonthlyDue)
                {
                    _MonthlyDue = value;
                    OnPropertyChanged("MonthlyDue");
                }
            }
        }


        private Decimal _GarbageDue;
        public Decimal GarbageDue
        {
            get
            {
                return _GarbageDue;
            }
            set
            {
                if (value != _GarbageDue)
                {
                    _GarbageDue = value;
                    OnPropertyChanged("GarbageDue");
                }
            }
        }



        private String _Remarks;
        public String Remarks
        {
            get
            {
                return _Remarks;
            }
            set
            {
                if (value != _Remarks)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
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
