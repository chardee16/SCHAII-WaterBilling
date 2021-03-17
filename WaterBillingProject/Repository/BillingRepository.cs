using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterBilling.Models.Billing;
using WaterBilling.Services;

namespace WaterBilling.Repository
{
    public class BillingRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";

        public List<MonthsClass> GetMonthList()
        {
            this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\GetMonthList.sql";
            return Connection.Query<MonthsClass>(this.sqlFile.sqlQuery).ToList();
        }



        public List<ChargesClass> GetCharges(Int64 ClientID,bool isNew,String TR_Date,String ReferenceNo)
        {
            List<ChargesClass> toReturn = new List<ChargesClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\GetBillCharges.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
                if (isNew)
                {
                    sqlFile.setParameter("_newCharges", "sl.Formula +");
                    sqlFile.setParameter("_Condition", "and Bill.BillStatus = 1");
                    sqlFile.setParameter("_withRef", " ");
                    sqlFile.setParameter("_ref", " ");
                    sqlFile.setParameter("_GroupBy", " ");
                }
                else
                {
                    sqlFile.setParameter("_newCharges", " ");
                    sqlFile.setParameter("_Condition", "and bill.TR_Date <= '" + TR_Date + "'" );
                    sqlFile.setParameter("_withRef", "and c.ReferenceNo = '" + ReferenceNo + "'");
                    sqlFile.setParameter("_ref", "and td.ReferenceNo = c.ReferenceNo");
                    sqlFile.setParameter("_GroupBy", ",c.ReferenceNo");
                }

                return Connection.Query<ChargesClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }





        public List<PreviousBillClass> GetPreviousBill(Int64 ClientID, bool isNew, String TR_Date)
        {
            List<PreviousBillClass> toReturn = new List<PreviousBillClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\GetPreviousBill.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
                if (isNew)
                {
                    sqlFile.setParameter("_Condition", "and BillStatus = 1");
                }
                else
                {
                    sqlFile.setParameter("_Condition", "and TR_Date < '" + TR_Date + "' and BillStatus != 3");
                }

                return Connection.Query<PreviousBillClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        public List<DiscountClass> GetDiscount()
        {
            List<DiscountClass> toReturn = new List<DiscountClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\GetDiscounts.sql";

                return Connection.Query<DiscountClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }



        public List<DiscountClass> GetDiscount(Int64 ClientID,String ReferenceNo)
        {
            List<DiscountClass> toReturn = new List<DiscountClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\GetBillingDiscounts.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
                sqlFile.setParameter("_ReferenceNo", ReferenceNo);


                return Connection.Query<DiscountClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }



        public Int64 GetPreviousReading(Int64 ClientID)
        {
            PreviousReadingClass toReturn = new PreviousReadingClass();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\GetPreviousReading.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());

                toReturn = Connection.Query<PreviousReadingClass>(this.sqlFile.sqlQuery).FirstOrDefault();

                return toReturn.PreviousReading;

            }
            catch (Exception ex)
            {
                return toReturn.PreviousReading;
            }
        }



        public Boolean SaveBill(CreateBillClass billData)
        {
            try
            {
                String discountValue = "";
                String ChargesValue = "";
                int counter = 0;
                String Last = "";

                foreach (var item in billData.DiscountList)
                {
                    counter++;
                    if (counter == billData.DiscountList.Count)
                    {
                        Last = ";";
                    }
                    else
                    {
                        Last = ",\n";
                    }
                    discountValue += "(" + item.SLC_CODE + "," + item.SLT_CODE + "," + item.AccountCode + ",'" + item.SL_Description + "'," + billData.ClientID + ", @ReferenceNo," + item.Amount + ")" + Last;
                }

                counter = 0;

                foreach (var item in billData.ChargesList)
                {
                    counter++;
                    if (counter == billData.ChargesList.Count)
                    {
                        Last = ";";
                    }
                    else
                    {
                        Last = ",\n";
                    }
                    ChargesValue += "(" + item.SLC_CODE + "," + item.SLT_CODE + "," + item.AccountCode + ",'" + item.SL_Description + "'," + billData.ClientID + ", @ReferenceNo," + item.Amount + ")" + Last;
                }


                String TransactionDetailValue = "";
                Int32 SequenceNo = 0;
                counter = 0;
                foreach (var item in billData.transdetail)
                {
                    counter++;
                    if (counter == billData.transdetail.Count)
                    {
                        Last = ";";
                    }
                    else
                    {
                        Last = ",\n";
                    }
                    TransactionDetailValue += "(" + item.TransactionCode + "," + item.TransYear + ",@ControlNo," + item.AccountCode
                                    + "," + item.ClientID + ",'" + billData.BillMonth + "'," + item.SLC_CODE + "," + item.SLT_CODE
                                    + ",@ReferenceNo," + item.SLE_CODE + "," + item.StatusID + ",'" + item.TransactionDate
                                    + "'," + item.Amt + "," + item.PostedBy + ",1," + SequenceNo + ",'" + item.ClientName + "')" + Last;

                    SequenceNo++;
                }





                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\InsertBill.sql";
                sqlFile.setParameter("_TransYear", billData.transummary.TransYear.ToString());
                sqlFile.setParameter("_Explanation", billData.transummary.Explanation);
                sqlFile.setParameter("_PostedBy", billData.transummary.PostedBy.ToString());
                sqlFile.setParameter("_TransactionDate", billData.transummary.TransactionDate);

                sqlFile.setParameter("_SLC_CODE", billData.SLC_CODE.ToString());
                sqlFile.setParameter("_SLT_CODE", billData.SLT_CODE.ToString());
                sqlFile.setParameter("_BillMonth", billData.BillMonth);
                sqlFile.setParameter("_ClientID", billData.ClientID.ToString());
                sqlFile.setParameter("_CurrentDue", billData.CurrentDue.ToString());
                sqlFile.setParameter("_BillStatus", billData.BillStatus.ToString());
                sqlFile.setParameter("_TR_CODE", billData.TR_CODE.ToString());
                sqlFile.setParameter("_TR_Date", billData.TR_Date);
                sqlFile.setParameter("_Consumption", billData.Consumption.ToString());
                sqlFile.setParameter("_ExcessConsumption", billData.ExcessConsumption.ToString());
                sqlFile.setParameter("_dueWithDiscount", billData.dueWithDiscount.ToString());
                sqlFile.setParameter("_CurrentReading", billData.CurrentReading.ToString());
                sqlFile.setParameter("_PreviousReading", billData.PreviousReading.ToString());
                sqlFile.setParameter("_discountValue", discountValue);
                sqlFile.setParameter("_ChargesValue", ChargesValue);
                sqlFile.setParameter("_TransactionDetailValue", TransactionDetailValue);


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


        public Boolean GetExistingMonth(Int64 ClientID,String BillMonth)
        {
            try
            {

                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\SelectCountBill.sql";
                sqlFile.setParameter("_BillMonth", BillMonth.ToString());
                sqlFile.setParameter("_ClientID", ClientID.ToString());

                var result = Connection.Query<CountExisting>(this.sqlFile.sqlQuery).FirstOrDefault();

                if (result.counting > 0)
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
                return true;
            }
        }


        public List<CreateBillClass> GetBillingList(Int64 ClientID)
        {
            List<CreateBillClass> toReturn = new List<CreateBillClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\GetBillingList.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());

                return Connection.Query<CreateBillClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        public Boolean CancelBill(CreateBillClass billData)
        {
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\CancelBill.sql";
                sqlFile.setParameter("_ClientID", billData.ClientID.ToString());
                sqlFile.setParameter("_TR_CODE", billData.TR_CODE.ToString());
                sqlFile.setParameter("_CTLNo", billData.CTLNo.ToString());
                sqlFile.setParameter("_BillMonth", billData.BillMonth.ToString());

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




        public Boolean SaveBillAdjustment(CreateBillClass billData)
        {
            try
            {
                
                int counter = 0;
                String Last = "";


                String TransactionDetailValue = "";
                Int32 SequenceNo = 0;
                foreach (var item in billData.transdetail)
                {
                    counter++;
                    if (counter == billData.transdetail.Count)
                    {
                        Last = ";";
                    }
                    else
                    {
                        Last = ",\n";
                    }
                    TransactionDetailValue += "(" + item.TransactionCode + "," + item.TransYear + ",@ControlNo," + item.AccountCode
                                    + "," + item.ClientID + ",'" + billData.BillMonth + "'," + item.SLC_CODE + "," + item.SLT_CODE
                                    + ",@ReferenceNo," + item.SLE_CODE + "," + item.StatusID + ",'" + item.TransactionDate
                                    + "'," + item.Amt + "," + item.PostedBy + ",1," + SequenceNo + ",'" + item.ClientName + "')" + Last;

                    SequenceNo++;
                }


                String ChargesValue = "";
                counter = 0;

                foreach (var item in billData.ChargesList)
                {
                    counter++;
                    if (counter == billData.ChargesList.Count)
                    {
                        Last = ";";
                    }
                    else
                    {
                        Last = ",\n";
                    }
                    ChargesValue += "(" + item.SLC_CODE + "," + item.SLT_CODE + "," + item.AccountCode + ",'" + item.SL_Description + "'," + billData.ClientID + ", @ReferenceNo," + item.Amount + ")" + Last;
                }





                this.sqlFile.sqlQuery = _config.SQLDirectory + "Billing\\InsertBillingAdjustment.sql";
                sqlFile.setParameter("_TransYear", billData.transummary.TransYear.ToString());
                sqlFile.setParameter("_Explanation", billData.transummary.Explanation);
                sqlFile.setParameter("_PostedBy", billData.transummary.PostedBy.ToString());
                sqlFile.setParameter("_TransactionDate", billData.transummary.TransactionDate);

                sqlFile.setParameter("_SLC_CODE", billData.SLC_CODE.ToString());
                sqlFile.setParameter("_SLT_CODE", billData.SLT_CODE.ToString());
                sqlFile.setParameter("_BillMonth", billData.BillMonth);
                sqlFile.setParameter("_ClientID", billData.ClientID.ToString());
                sqlFile.setParameter("_CurrentDue", billData.CurrentDue.ToString());
                sqlFile.setParameter("_BillStatus", billData.BillStatus.ToString());
                sqlFile.setParameter("_TR_CODE", billData.TR_CODE.ToString());
                sqlFile.setParameter("_TR_Date", billData.TR_Date);
                sqlFile.setParameter("_Consumption", billData.Consumption.ToString());
                sqlFile.setParameter("_ExcessConsumption", billData.ExcessConsumption.ToString());
                sqlFile.setParameter("_dueWithDiscount", billData.dueWithDiscount.ToString());
                sqlFile.setParameter("_CurrentReading", billData.CurrentReading.ToString());
                sqlFile.setParameter("_PreviousReading", billData.PreviousReading.ToString());
                sqlFile.setParameter("_TransactionDetailValue", TransactionDetailValue);
                sqlFile.setParameter("_ChargesValue", ChargesValue);

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


    }
}
