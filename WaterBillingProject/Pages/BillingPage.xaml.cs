using CrystalDecisions.CrystalReports.Engine;
using SAPBusinessObjects.WPF.Viewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaterBilling.Models.Billing;
using WaterBilling.Models.ClientMaster;
using WaterBilling.Repository;
using WaterBilling.Windows;
using WaterBillingProject;
using WaterBillingProject.Reports;
using WaterBillingProject.Windows;

namespace WaterBilling.Pages
{
    /// <summary>
    /// Interaction logic for BillingPage.xaml
    /// </summary>
    public partial class BillingPage : Page
    {

        BillingDataContext dataCon;
        BillingRepository repo = new BillingRepository();

        BackgroundWorker worker = new BackgroundWorker();
        BackgroundWorker billworker = new BackgroundWorker();



        ReportDocument report = new ReportDocument();

        public BillingPage()
        {
            InitializeComponent();
            InitializeWorkers();
            this.dataCon = new BillingDataContext();
            this.DataContext = this.dataCon;
            this.btn_Find.Focus();

            this.dataCon.monthList = repo.GetMonthList();
            this.cmb_Months.ItemsSource = this.dataCon.monthList;


            this.dataCon.BillingMonthID = DateTime.Now.Month;
            this.dataCon.Year = DateTime.Now.Year;

            
        }

        private void chk_IsSenior_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chk_IsSenior_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Find_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataCon.ClientID == 0)
            {
                findClientFunction();
            }
            else
            {
                findBillFunction();
            }
        }


        private void findClientFunction()
        {
            var dialog = new ClientListWindow();
            if (dialog.ShowDialog() == true)
            {
                this.dataCon.ClientID = dialog.clientSelected.ClientID;
                this.dataCon.Fullname = dialog.clientSelected.FullName;
                this.dataCon.FullAddress = dialog.clientSelected.FullAddress;
                this.dataCon.Occupants = dialog.clientSelected.Occupants;
                this.dataCon.IsSenior = dialog.clientSelected.IsSenior;
                this.dataCon.SeniorCount = dialog.clientSelected.SeniorCount;


                try
                {
                    worker.RunWorkerAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }


        private void findBillFunction()
        {
            var dialog = new BillingListWindow(this.dataCon.ClientID);
            if (dialog.ShowDialog() == true)
            {
                billSelected();
                DateTime date = new DateTime();
                this.dataCon.PresentReading = dialog.billSelected.CurrentReading;
                this.dataCon.PreviousReading = dialog.billSelected.PreviousReading;
                date = DateTime.ParseExact(dialog.billSelected.BillMonth, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
                this.dataCon.BillingMonthID = date.Month;
                this.dataCon.Year = date.Year;
                this.dataCon.TotalConsumption = dialog.billSelected.Consumption;
                this.dataCon.CurrentDue = dialog.billSelected.CurrentDue;
                this.dataCon.DueWithoutCharges = dialog.billSelected.dueWithDiscount;
                this.dataCon.TR_Date = dialog.billSelected.TR_Date;
                this.dataCon.ReferenceNo = dialog.billSelected.ReferenceNo;
                try
                {
                    billworker.RunWorkerAsync();
                }
                catch (Exception ex)
                {

                }

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
                this.dataCon.tempChargesList = this.repo.GetCharges(this.dataCon.ClientID,true,"");
                this.dataCon.previousBillList = this.repo.GetPreviousBill(this.dataCon.ClientID,true,"");
                this.dataCon.TempdiscountList = this.repo.GetDiscount();
                this.dataCon.PreviousReading = this.repo.GetPreviousReading(this.dataCon.ClientID);

                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.dataCon.previousBillList.Count >= 1)
            {
                DG_PreviousBill.Visibility = Visibility.Visible;
            }
            else
            {
                DG_PreviousBill.Visibility = Visibility.Collapsed;
            }

            txt_PresentReading.Focus();
            txt_PresentReading.SelectAll();

        }


        private void billworker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.dataCon.tempChargesList = this.repo.GetCharges(this.dataCon.ClientID, false, this.dataCon.TR_Date);
                this.dataCon.previousBillList = this.repo.GetPreviousBill(this.dataCon.ClientID,false, this.dataCon.TR_Date);
                this.dataCon.discountList = this.repo.GetDiscount(this.dataCon.ClientID,this.dataCon.ReferenceNo);
                //this.dataCon.PreviousReading = this.repo.GetPreviousReading(this.dataCon.ClientID);

                break;
            }
        }

        private void billworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.dataCon.previousBillList.Count >= 1)
            {
                DG_PreviousBill.Visibility = Visibility.Visible;
            }
            else
            {
                DG_PreviousBill.Visibility = Visibility.Collapsed;
            }

            TotalDueFunction();

        }


        private void GetInterest()
        {
            ChargesClass chargeInterest;
            List<ChargesClass> charges = new List<ChargesClass>();


            if (this.dataCon.previousBillList.Count >= 2)
            {
                
                int days = 0;
                decimal TotalPrevious = 0;
                decimal TotalAmountDue = 0;


                DateTime date = new DateTime();

                foreach (var item in this.dataCon.previousBillList)
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
                    TotalPrevious += item.TotalDue;
                }
                days += DateTime.Now.Day;
                days -= 16;

                TotalAmountDue = TotalPrevious + this.dataCon.DueWithoutCharges;
                decimal divideDays = Math.Round(Convert.ToDecimal(days) / Convert.ToDecimal(360), 2, MidpointRounding.AwayFromZero);
                this.dataCon.InterestDue = Math.Round(TotalAmountDue * divideDays, 2, MidpointRounding.AwayFromZero); 

            }


            if (this.dataCon.InterestDue > 0)
            {
                chargeInterest = new ChargesClass
                {
                    SLC_CODE = 14,
                    SLT_CODE = 2,
                    SLE_CODE = 1,
                    StatusID = 15,
                    SL_Description = "Interest Due",
                    Formula = this.dataCon.InterestDue.ToString(),
                    AccountCode = 402104,
                    Amount = this.dataCon.InterestDue
                };

                charges.Add(chargeInterest);
            }


            foreach (var item in this.dataCon.tempChargesList)
            {
                chargeInterest = new ChargesClass
                {
                    SLC_CODE = item.SLC_CODE,
                    SLT_CODE = item.SLT_CODE,
                    SLE_CODE = item.SLE_CODE,
                    StatusID = item.StatusID,
                    SL_Description = item.SL_Description,
                    Formula = item.Formula,
                    AccountCode = item.AccountCode,
                    Amount = item.Amount
                };

                charges.Add(chargeInterest);
            }

            this.dataCon.chargesList = charges;

        }

        private void Executed_Find(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.dataCon.ClientID == 0)
            {
                findClientFunction();
            }
            else
            {
                findBillFunction();
            }
            
        }


        private void txt_PresentReading_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.dataCon.PresentReading < this.dataCon.PreviousReading)
                {
                    MessageBox.Show("Present Reading must be higher than previous.");
                    this.txt_PresentReading.Focus();
                    e.Handled = true;
                }
                else
                {
                    ComputeFunction();
                }
            }
        }



        private void ComputeFunction()
        {
            this.dataCon.TotalConsumption = this.dataCon.PresentReading - this.dataCon.PreviousReading;
            //Decimal currentDue = 0;
            if (this.dataCon.TotalConsumption <= 10)
            {
                this.dataCon.CurrentDue = Convert.ToDecimal(200.00);
            }
            else
            {
                this.dataCon.CurrentDue = Convert.ToDecimal(200.00 + ((this.dataCon.TotalConsumption - 10) * 22));
            }


            List<DiscountClass> tempDiscount = new List<DiscountClass>();
            foreach (var item in this.dataCon.TempdiscountList)
            {
                if (item.SLT_CODE == 1)
                {
                    decimal memberDiscount = Convert.ToDecimal(this.dataCon.TotalConsumption) * item.Amount;
                    tempDiscount.Add(new DiscountClass
                    {
                        SLC_CODE = item.SLC_CODE,
                        SLT_CODE = item.SLT_CODE,
                        SLE_CODE = item.SLE_CODE,
                        StatusID = item.StatusID,
                        SL_Description = item.SL_Description,
                        Formula = item.Formula,
                        AccountCode = item.AccountCode,
                        Amount = memberDiscount,
                    });
                }
                else if (item.SLT_CODE == 3)
                {
                    if (this.dataCon.IsSenior)
                    {
                        Decimal discountPercent = (item.Amount / Convert.ToDecimal(this.dataCon.Occupants)) * Convert.ToDecimal(this.dataCon.SeniorCount);
                        decimal discount = this.dataCon.CurrentDue * discountPercent;
                        tempDiscount.Add(new DiscountClass
                        {
                            SLC_CODE = item.SLC_CODE,
                            SLT_CODE = item.SLT_CODE,
                            SLE_CODE = item.SLE_CODE,
                            StatusID = item.StatusID,
                            SL_Description = item.SL_Description,
                            Formula = item.Formula,
                            AccountCode = item.AccountCode,
                            Amount = Math.Round(discount,2,MidpointRounding.AwayFromZero),
                        });
                    }
                }

            }


            this.dataCon.discountList = tempDiscount;
            TotalDueFunction();
        }


        private void TotalDueFunction()
        {

            decimal totalDiscount = 0;
            decimal totalCharges = 0;
            decimal totalPreviousBill = 0;

            foreach (var item in this.dataCon.discountList)
            {
                totalDiscount += item.Amount;
            }

            this.dataCon.DueWithoutCharges = this.dataCon.CurrentDue - totalDiscount;
            GetInterest();

            foreach (var item in this.dataCon.chargesList)
            {
                totalCharges += item.Amount;
            }

            foreach (var item in this.dataCon.previousBillList)
            {
                totalPreviousBill += item.TotalDue;
            }

            
            this.dataCon.TotalDue = this.dataCon.DueWithoutCharges + totalCharges + totalPreviousBill;
            

        }

        private void txt_PresentReading_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }


        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }


        private void Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to save the bill?", "CONFIRMATION", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SaveFunction();
            }
            else
            {

            }
        }





        private void SaveFunction()
        {
            try
            {
                CreateBillClass billing = new CreateBillClass();
                if (this.dataCon.ClientID != 0)
                {
                   
                    billing.SLC_CODE = 12;
                    billing.SLT_CODE = 1;
                    billing.ClientID = this.dataCon.ClientID;
                    billing.CurrentDue = this.dataCon.CurrentDue;
                    billing.BillMonth = string.Format("{0}{1}", string.Format("{0:D4}", DateTime.Now.Year), string.Format("{0:D2}", this.dataCon.BillingMonthID));
                    billing.TR_Date = string.Format("{0}-{1}-{2}", string.Format("{0:D4}", DateTime.Now.Year), string.Format("{0:D2}", this.dataCon.BillingMonthID), string.Format("{0:D2}",DateTime.Now.Day));
                    billing.BillStatus = 1;
                    billing.TR_CODE = 1;
                    billing.Consumption = this.dataCon.TotalConsumption;
                    billing.ExcessConsumption = this.dataCon.TotalConsumption > 10 ? this.dataCon.TotalConsumption - 10 : 0;
                    billing.dueWithDiscount = this.dataCon.DueWithoutCharges;
                    billing.CurrentReading = this.dataCon.PresentReading;
                    billing.PreviousReading = this.dataCon.PreviousReading;
                    billing.ChargesList = getCharges();
                    billing.DiscountList = getDiscount();

                    if (this.repo.GetExistingMonth(billing.ClientID, billing.BillMonth))
                    {
                        MessageBox.Show("Client has a bill already for this month.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (this.repo.SaveBill(billing))
                        {

                            MessageBoxResult messageBoxResult = MessageBox.Show("Bill Successfully saved.\n\nDo you want to generate Billing Statement?", "CONFIRMATION", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No);
                            if (messageBoxResult == MessageBoxResult.Yes)
                            {
                                GenerateBillingStatement();
                            }
                            else
                            {
                                Refresh();
                            }
                            Refresh();
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



        private List<ChargesClass> getCharges()
        {
            List<ChargesClass> toReturn = new List<ChargesClass>();
            try
            {
                foreach (var item in this.dataCon.chargesList)
                {
                    toReturn.Add(new ChargesClass 
                    {
                        SLC_CODE = item.SLC_CODE,
                        SLT_CODE = item.SLT_CODE,
                        AccountCode = item.AccountCode,
                        SL_Description = item.SL_Description,
                        Amount = Convert.ToDecimal(item.Formula),
                    });
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        private List<DiscountClass> getDiscount()
        {
            List<DiscountClass> toReturn = new List<DiscountClass>();
            try
            {
                foreach (var item in this.dataCon.discountList)
                {
                    toReturn.Add(new DiscountClass
                    {
                        SLC_CODE = item.SLC_CODE,
                        SLT_CODE = item.SLT_CODE,
                        AccountCode = item.AccountCode,
                        SL_Description = item.SL_Description,
                        Amount = item.Amount,
                    });
                }

                return toReturn;
            }
            catch (Exception ex)
            {
                return toReturn;
            }


        }


        private void Refresh()
        {
            List<ChargesClass> refreshCharges = new List<ChargesClass>();
            this.dataCon.chargesList = refreshCharges;

            List<DiscountClass> refreshDiscount = new List<DiscountClass>();
            this.dataCon.discountList = refreshDiscount;

            List<PreviousBillClass> refreshPreviousBill = new List<PreviousBillClass>();
            this.dataCon.previousBillList = refreshPreviousBill;


            this.dataCon.ClientID = 0;
            this.dataCon.Occupants = 0;
            this.dataCon.IsSenior = false;
            this.dataCon.SeniorCount = 0;
            this.dataCon.Fullname = "";
            this.dataCon.FullAddress = "";
            this.dataCon.PresentReading = 0;
            this.dataCon.PreviousReading = 0;
            this.dataCon.TotalConsumption = 0;
            this.dataCon.CurrentDue = 0;
            this.dataCon.TotalDue = 0;
            this.dataCon.DueWithoutCharges = 0;

            txt_PresentReading.IsReadOnly = false;
            btn_Save.IsEnabled = true;
            btn_Generate.IsEnabled = false;

            btn_Find.Focus();
        }


        private void billSelected()
        {
            btn_Generate.IsEnabled = true;
            txt_PresentReading.IsReadOnly = true;
            btn_Save.IsEnabled = false;
        }

        

        private void txt_PresentReading_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txt_PresentReading.SelectAll();
        }

        private void txt_PresentReading_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            txt_PresentReading.SelectAll();
        }


       

        private void btn_Generate_Click(object sender, RoutedEventArgs e)
        {
            GenerateBillingStatement();
        }


        private void GenerateBillingStatement()
        {
            try
            {
                CrystalReport crystalReport = new CrystalReport(this);
                this.report = new StatementOfAccount();
                List<PreviousBillClass> billing = new List<PreviousBillClass>();

                billing.Add(new PreviousBillClass
                {
                    BillMonth = string.Format("{0}{1}", string.Format("{0:D4}", DateTime.Now.Year), string.Format("{0:D2}", this.dataCon.BillingMonthID)),
                    ReferenceNo = "Current Due",
                    TotalDue = this.dataCon.DueWithoutCharges
                }); ;

                foreach (var item in dataCon.previousBillList)
                {
                    billing.Add(new PreviousBillClass
                    {
                        BillMonth = item.BillMonth,
                        ReferenceNo = item.ReferenceNo,
                        TotalDue = item.TotalDue
                    }); ;
                }



                this.report.Database.Tables[0].SetDataSource(billing);
                this.report.Database.Tables[1].SetDataSource(this.dataCon.chargesList);
                this.report.Database.Tables[2].SetDataSource(dataCon.discountList);


                this.report.SetParameterValue("Fullname", this.dataCon.Fullname);
                this.report.SetParameterValue("FullAddress", this.dataCon.FullAddress);
                this.report.SetParameterValue("AmountDue", string.Format("{0:n}", this.dataCon.CurrentDue));
                this.report.SetParameterValue("DeductDate", string.Format("{0}-{1}-{2}", string.Format("{0:D2}", this.dataCon.BillingMonthID), string.Format("{0:D2}", 5), string.Format("{0:D4}", DateTime.Now.Year)));
                this.report.SetParameterValue("DueDate", string.Format("{0}-{1}-{2}", string.Format("{0:D2}", this.dataCon.BillingMonthID), string.Format("{0:D2}", 16), string.Format("{0:D4}", DateTime.Now.Year)));
                this.report.SetParameterValue("TotalDue", string.Format("{0:n}", this.dataCon.TotalDue));
                this.report.SetParameterValue("DateFrom", string.Format("{0}-{1}-{2}", string.Format("{0:D2}", this.dataCon.BillingMonthID), string.Format("{0:D2}", 1), string.Format("{0:D4}", DateTime.Now.Year)));
                this.report.SetParameterValue("DateTo", string.Format("{0}-{1}-{2}", string.Format("{0:D2}", this.dataCon.BillingMonthID), string.Format("{0:D2}", DateTime.DaysInMonth(DateTime.Now.Year, this.dataCon.BillingMonthID)), string.Format("{0:D4}", DateTime.Now.Year)));
                this.report.SetParameterValue("PresentReading", this.dataCon.PresentReading.ToString());
                this.report.SetParameterValue("PreviousReading", this.dataCon.PreviousReading.ToString());
                this.report.SetParameterValue("TotalUsed", this.dataCon.TotalConsumption.ToString());

                crystalReport.cryRpt = this.report;
                crystalReport._CrystalReport.ViewerCore.ReportSource = this.report;
                //crystalReport.Owner = this;
                crystalReport.ShowInTaskbar = false;
                crystalReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(GetFastestNISTDate().ToString("yyyy/MM/dd"));
            Refresh();
        }


        public static DateTime GetFastestNISTDate()
        {
            var result = DateTime.MinValue;
            // Initialize the list of NIST time servers
            // http://tf.nist.gov/tf-cgi/servers.cgi
            string[] servers = new string[] {
                "time-a.nist.gov",
                "time-b.nist.gov",
                
                };

            // Try 5 servers in random order to spread the load
            Random rnd = new Random();
            foreach (string server in servers.OrderBy(s => rnd.NextDouble()).Take(5))
            {
                try
                {
                    // Connect to the server (at port 13) and get the response
                    string serverResponse = string.Empty;
                    using (var reader = new StreamReader(new System.Net.Sockets.TcpClient(server, 13).GetStream()))
                    {
                        serverResponse = reader.ReadToEnd();
                    }

                    // If a response was received
                    if (!string.IsNullOrEmpty(serverResponse))
                    {
                        // Split the response string ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC(NIST) *")
                        string[] tokens = serverResponse.Split(' ');

                        // Check the number of tokens
                        if (tokens.Length >= 6)
                        {
                            // Check the health status
                            string health = tokens[5];
                            if (health == "0")
                            {
                                // Get date and time parts from the server response
                                string[] dateParts = tokens[1].Split('-');
                                string[] timeParts = tokens[2].Split(':');

                                // Create a DateTime instance
                                DateTime utcDateTime = new DateTime(
                                    Convert.ToInt32(dateParts[0]) + 2000,
                                    Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]),
                                    Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]),
                                    Convert.ToInt32(timeParts[2]));

                                // Convert received (UTC) DateTime value to the local timezone
                                result = utcDateTime.ToLocalTime();

                                return result;
                                // Response successfully received; exit the loop

                            }
                        }

                    }

                }
                catch
                {
                    // Ignore exception and try the next server
                }
            }
            return result;
        }



        private void Executed_Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to cancel your current work?", "CONFIRMATION", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Refresh();
            }
            else
            {

            }
        }

        public class BillingDataContext : INotifyPropertyChanged
        {
            List<MonthsClass> _monthList;
            public List<MonthsClass> monthList
            {
                get { return _monthList; }
                set
                {
                    _monthList = value;
                    OnPropertyChanged("monthsList");
                }
            }


            List<ChargesClass> _chargesList;
            public List<ChargesClass> chargesList
            {
                get { return _chargesList; }
                set
                {
                    _chargesList = value;
                    OnPropertyChanged("chargesList");
                }
            }


            List<ChargesClass> _tempChargesList;
            public List<ChargesClass> tempChargesList
            {
                get { return _tempChargesList; }
                set
                {
                    _tempChargesList = value;
                    OnPropertyChanged("tempChargesList");
                }
            }




            List<DiscountClass> _TempdiscountList;
            public List<DiscountClass> TempdiscountList
            {
                get { return _TempdiscountList; }
                set
                {
                    _TempdiscountList = value;
                    OnPropertyChanged("TempdiscountList");
                }
            }


            List<DiscountClass> _discountList;
            public List<DiscountClass> discountList
            {
                get { return _discountList; }
                set
                {
                    _discountList = value;
                    OnPropertyChanged("discountList");
                }
            }



            List<PreviousBillClass> _previousBillList;
            public List<PreviousBillClass> previousBillList
            {
                get { return _previousBillList; }
                set
                {
                    _previousBillList = value;
                    OnPropertyChanged("previousBillList");
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



            private Int32 _Occupants;
            public Int32 Occupants
            {
                get
                {
                    return _Occupants;
                }
                set
                {
                    if (value != _Occupants)
                    {
                        _Occupants = value;
                        OnPropertyChanged("Occupants");
                    }
                }
            }


            private Boolean _IsSenior;
            public Boolean IsSenior
            {
                get
                {
                    return _IsSenior;
                }
                set
                {
                    if (value != _IsSenior)
                    {
                        _IsSenior = value;
                        OnPropertyChanged("IsSenior");
                    }
                }
            }



            private Int32 _SeniorCount;
            public Int32 SeniorCount
            {
                get
                {
                    return _SeniorCount;
                }
                set
                {
                    if (value != _SeniorCount)
                    {
                        _SeniorCount = value;
                        OnPropertyChanged("SeniorCount");
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



            private Int32 _BillingMonthID;
            public Int32 BillingMonthID
            {
                get
                {
                    return _BillingMonthID;
                }
                set
                {
                    if (value != _BillingMonthID)
                    {
                        _BillingMonthID = value;
                        OnPropertyChanged("BillingMonthID");
                    }
                }
            }



            private Int64 _Year;
            public Int64 Year
            {
                get
                {
                    return _Year;
                }
                set
                {
                    if (value != _Year)
                    {
                        _Year = value;
                        OnPropertyChanged("Year");
                    }
                }
            }


            private Int64 _PresentReading;
            public Int64 PresentReading
            {
                get
                {
                    return _PresentReading;
                }
                set
                {
                    if (value != _PresentReading)
                    {
                        _PresentReading = value;
                        OnPropertyChanged("PresentReading");
                    }
                }
            }



            private Int64 _PreviousReading;
            public Int64 PreviousReading
            {
                get
                {
                    return _PreviousReading;
                }
                set
                {
                    if (value != _PreviousReading)
                    {
                        _PreviousReading = value;
                        OnPropertyChanged("PreviousReading");
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


            private Decimal _CurrentDue;
            public Decimal CurrentDue
            {
                get
                {
                    return _CurrentDue;
                }
                set
                {
                    if (value != _CurrentDue)
                    {
                        _CurrentDue = value;
                        OnPropertyChanged("CurrentDue");
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


            private Decimal _DueWithoutCharges;
            public Decimal DueWithoutCharges
            {
                get
                {
                    return _DueWithoutCharges;
                }
                set
                {
                    if (value != _DueWithoutCharges)
                    {
                        _DueWithoutCharges = value;
                        OnPropertyChanged("DueWithoutCharges");
                    }
                }
            }


            private Decimal _InterestDue;
            public Decimal InterestDue
            {
                get
                {
                    return _InterestDue;
                }
                set
                {
                    if (value != _InterestDue)
                    {
                        _InterestDue = value;
                        OnPropertyChanged("InterestDue");
                    }
                }
            }



            private String _ReferenceNo;
            public String ReferenceNo
            {
                get
                {
                    return _ReferenceNo;
                }
                set
                {
                    if (value != _ReferenceNo)
                    {
                        _ReferenceNo = value;
                        OnPropertyChanged("ReferenceNo");
                    }
                }
            }

            private String _TR_Date;
            public String TR_Date
            {
                get
                {
                    return _TR_Date;
                }
                set
                {
                    if (value != _TR_Date)
                    {
                        _TR_Date = value;
                        OnPropertyChanged("TR_Date");
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
