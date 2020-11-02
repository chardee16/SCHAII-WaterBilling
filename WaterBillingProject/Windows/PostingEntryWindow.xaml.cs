using System;
using System.Collections.Generic;
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
using WaterBillingProject.Models.Collection;
using WaterBillingProject.Repository;

namespace WaterBillingProject.Windows
{
    /// <summary>
    /// Interaction logic for PostingEntryWindow.xaml
    /// </summary>
    public partial class PostingEntryWindow : Window
    {
        CollectionRepository repo = new CollectionRepository();

        TransactionSummaryClass tranSummary = new TransactionSummaryClass();
        List<TransactionDetailClass> transactionDetail = new List<TransactionDetailClass>();
        List<TransactionCheckClass> transactionCheck = new List<TransactionCheckClass>();
        BillUpdateClass updateBill = new BillUpdateClass();
        List<CollectionEntryClass> collectionEntry = new List<CollectionEntryClass>();
 
        public PostingEntryWindow(TransactionSummaryClass summary, List<TransactionDetailClass> trdt, List<TransactionCheckClass> transcheck, BillUpdateClass updbill)
        {
            InitializeComponent();
            this.tranSummary = summary;
            this.transactionDetail = trdt;
            this.transactionCheck = transcheck;
            this.updateBill = updbill;

            foreach (var item in this.transactionDetail)
            {
                collectionEntry.Add(new CollectionEntryClass
                {
                    SL_Description = item.SL_Description,
                    AccountCode = item.AccountCode,
                    BillMonth = item.BillMonth,
                    Debit = item.Amt > 0 ? item.Amt.ToString() : "",
                    Credit = item.Amt < 0 ? (item.Amt * -1).ToString() : "",
                });
            }


            DG_BillList.ItemsSource = collectionEntry;

        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to post this transaction?", "CONFIRMATION", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (repo.PostPayment(this.tranSummary, this.transactionDetail, this.transactionCheck))
                    {
                        if (repo.UpdateBill(updateBill))
                        {
                            MessageBox.Show("Payment successfully posted.","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                            e.Handled = true;
                            this.Close();
                            
                        }
                    }
                }
                else
                {

                }
            }
        }
    }
}
