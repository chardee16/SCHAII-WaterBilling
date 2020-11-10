using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBilling.Repository;
using WaterBilling.Services;
using WaterBillingProject.Models.Collection;

namespace WaterBillingProject.Repository
{
    public class CollectionRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";

        public List<CollectionBillsClass> GetBillingList(Int64 ClientID)
        {
            List<CollectionBillsClass> toReturn = new List<CollectionBillsClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetCollectionBillingList.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
              

                return Connection.Query<CollectionBillsClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        public List<CollectionChargesClass> GetCharges(Int64 ClientID)
        {
            List<CollectionChargesClass> toReturn = new List<CollectionChargesClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetCollectionCharges.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
                return Connection.Query<CollectionChargesClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        public List<CollectionDiscountClass> GetDiscount(Int64 ClientID)
        {
            List<CollectionDiscountClass> toReturn = new List<CollectionDiscountClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetCollectionDiscount.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
                return Connection.Query<CollectionDiscountClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        public Boolean PostPayment(TransactionSummaryClass summary, List<TransactionDetailClass> trdt, List<TransactionCheckClass> transcheck)
        {

            try
            {
                String TransactionDetailValue = "";
                Int32 SequenceNo = 0;
                int counter = 0;
                String Last = "";
                foreach (var item in trdt)
                {
                    counter++;
                    if (counter == trdt.Count)
                    {
                        Last = ";";
                    }
                    else
                    {
                        Last = ",\n";
                    }
                    TransactionDetailValue += "(" + item.TransactionCode + "," + item.TransYear + ",@ControlNo," + item.AccountCode
                                    + "," + item.ClientID + ",'" + item.BillMonth + "'," + item.SLC_CODE + "," + item.SLT_CODE
                                    + ",'" + item.ReferenceNo + "'," + item.SLE_CODE + "," + item.StatusID + ",'" + item.TransactionDate
                                    + "'," + item.Amt + "," + item.PostedBy + ",1," + SequenceNo + ",'" + item.ClientName + "')" + Last;

                    SequenceNo++;
                }


                String TransactionCheckValue = "";
                counter = 0;
                foreach (var item in transcheck)
                {
                    counter++;
                    if (counter == transcheck.Count)
                    {
                        Last = ";";
                    }
                    else
                    {
                        Last = ",\n";
                    }
                    TransactionCheckValue += "(" + item.TransactionCode + ",@ControlNo," + item.TransYear + "," + item.COCIType + "," + item.Amt + ",1)" + Last;
                }

                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\InsertCollection.sql";
                sqlFile.setParameter("_TransactionCode", summary.TransactionCode.ToString());
                sqlFile.setParameter("_TransYear", summary.TransYear.ToString());
                sqlFile.setParameter("_TransactionDate", summary.TransactionDate);
                sqlFile.setParameter("_ClientID", summary.ClientID.ToString());
                sqlFile.setParameter("_Explanation", summary.Explanation);
                sqlFile.setParameter("_PostedBy", summary.PostedBy.ToString());
                sqlFile.setParameter("_ORNumber", summary.ORNo);
                sqlFile.setParameter("_TransactionDetailValue", TransactionDetailValue);
                sqlFile.setParameter("_TransactionCheckValue", TransactionCheckValue);

                var affectedRow = Connection.Execute(sqlFile.sqlQuery);


                if (affectedRow > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                return false;
            }

          
        }



        public Boolean UpdateBill(BillUpdateClass updClass)
        {

            try
            {
                
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\UpdateBillStatus.sql";
                sqlFile.setParameter("_ClientID", updClass.ClientID.ToString());
                sqlFile.setParameter("_ReferenceNo", updClass.ReferenceNo);
                

                var affectedRow = Connection.Execute(sqlFile.sqlQuery);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }



        public List<TransactionListClass> GetTransactionList(String trDate)
        {
            List<TransactionListClass> toReturn = new List<TransactionListClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetTransactions.sql";
                sqlFile.setParameter("_trDate", trDate);
                return Connection.Query<TransactionListClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        public String GetORNo()
        {

            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetMaxORNo.sql";
                return Connection.Query<String>(this.sqlFile.sqlQuery).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return "0";
            }


        }





    }
}
