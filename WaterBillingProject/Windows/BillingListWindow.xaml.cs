using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WaterBilling.Models.Billing;
using WaterBilling.Repository;

namespace WaterBillingProject.Windows
{
    /// <summary>
    /// Interaction logic for BillingListWindow.xaml
    /// </summary>
    public partial class BillingListWindow : Window
    {

        BackgroundWorker worker = new BackgroundWorker();
        BillingRepository repo = new BillingRepository();
        BillingListDataContext dataCon;
        public ObservableCollection<CreateBillClass> billingList;
        CreateBillClass toReturn = new CreateBillClass();
        private ICollectionView MyData;
        string SearchText = string.Empty;
        int currentRow = 0, currentColumn = 1;



        public BillingListWindow(Int64 ClientID)
        {
            InitializeComponent();
            InitializeWorkers();
            this.PreviewKeyDown += new KeyEventHandler(HandleKeysEvent);
            dataCon = new BillingListDataContext();

            this.dataCon.ClientID = ClientID;

            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }


        }

        public CreateBillClass billSelected
        {
            get { return this.toReturn; }
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
                this.dataCon.billingList = repo.GetBillingList(this.dataCon.ClientID);
                break;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DataContext = this.dataCon;
            this.billingList = new ObservableCollection<CreateBillClass>(this.dataCon.billingList);
            DG_BillList.ItemsSource = this.billingList;
            MyData = CollectionViewSource.GetDefaultView(this.billingList);
            DG_BillList.Focus();
            DG_BillList.SelectedIndex = 0;
            txt_Search.Focus();
            //this.cmb_ClientStatus.ItemsSource = this.dataCon.clientStatusList;

        }



        private void DG_BillList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BillSelectedFunction();
        }


        private void BillSelectedFunction()
        {
            if (DG_BillList.SelectedItem == null) return;
            var selectedBill = DG_BillList.SelectedItem as CreateBillClass;
            if (selectedBill != null)
            {
                this.toReturn.ClientID = selectedBill.ClientID;
                this.toReturn.BillMonth = selectedBill.BillMonth;
                this.toReturn.ReferenceNo = selectedBill.ReferenceNo;
                this.toReturn.CurrentDue = selectedBill.CurrentDue;
                this.toReturn.BillStatus = selectedBill.BillStatus;
                this.toReturn.Consumption = selectedBill.Consumption;
                this.toReturn.dueWithDiscount = selectedBill.dueWithDiscount;
                this.toReturn.CurrentReading = selectedBill.CurrentReading;
                this.toReturn.PreviousReading = selectedBill.PreviousReading;
                this.toReturn.TR_Date = Convert.ToDateTime(selectedBill.TR_Date).ToString("yyyy-MM-dd") ;
                DialogResult = true;
            }
        }


        private void DG_BillList_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            dataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void DG_BillList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Enter:
                    BillSelectedFunction();
                    e.Handled = true;
                    break;
            }
        }

        private void txt_Search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Enter:
                    BillSelectedFunction();
                    e.Handled = true;
                    break;
            }
        }

        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            SearchText = t.Text.ToString();
            MyData.Filter = FilterData;

            DG_BillList.SelectedIndex = 0;
        }


        private bool FilterData(object item)
        {
            var value = (CreateBillClass)item;
            if (value == null || value.ReferenceNo == null)
                return false;
            return Convert.ToString(value.ClientID).StartsWith(SearchText.ToLower()) || value.BillMonth.ToLower().StartsWith(SearchText.ToLower()) || value.BillStatusDescription.ToLower().StartsWith(SearchText.ToLower());
        }

        private void SelectedCell_Click(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell.Column.DisplayIndex != this.currentColumn)
            {
                cell.IsSelected = false;
            }
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            while ((dep != null) && !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            DataGridRow row = dep as DataGridRow;
            this.currentRow = row.GetIndex();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void HandleKeys(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Enter:
                    BillSelectedFunction();
                    e.Handled = true;
                    break;
            }
        }


        private void HandleKeysEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (currentRow > 0)
                {
                    try
                    {
                        int previousIndex = DG_BillList.SelectedIndex - 1;
                        if (previousIndex < 0)
                            return; DG_BillList.SelectedIndex = previousIndex; DG_BillList.ScrollIntoView(DG_BillList.Items[currentRow]);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (e.Key == Key.Down)
            {
                if (currentRow < DG_BillList.Items.Count - 1)
                {
                    try
                    {
                        int nextIndex = DG_BillList.SelectedIndex + 1;

                        if (nextIndex > DG_BillList.Items.Count - 1)
                            return;

                        DG_BillList.SelectedIndex = nextIndex;
                        DG_BillList.ScrollIntoView(DG_BillList.Items[currentRow]);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                    }
                } // end if (this.SelectedOverride > 0)            
            } // end else if (e.Key == Key.Down)
            else if (e.Key == Key.End && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                DG_BillList.SelectedItem = billingList.Last();
                DG_BillList.ScrollIntoView(DG_BillList.Items[DG_BillList.SelectedIndex]);
            }
        }



        public class BillingListDataContext : INotifyPropertyChanged
        {



            List<CreateBillClass> _billingList;
            public List<CreateBillClass> billingList
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
