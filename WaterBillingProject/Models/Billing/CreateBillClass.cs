using System;
using System.Collections.Generic;
using System.Text;
using WaterBillingProject.Models.Collection;

namespace WaterBilling.Models.Billing
{
    public class CreateBillClass
    {
        public Int32 SLC_CODE { get; set; }
        public Int32 SLT_CODE { get; set; }
        public String BillMonth { get; set; }
        public String ReferenceNo { get; set; }
        public Int64 ClientID { get; set; }
        public Decimal CurrentDue { get; set; }
        public Int32 BillStatus { get; set; }

        public String BillStatusDescription { get; set; }

        public Int32 TR_CODE { get; set; }
        public Int64 CTLNo { get; set; }
        public String TR_Date { get; set; }
        public String Remarks { get; set; }

        public Int64 CurrentReading { get; set; }
        public Int64 PreviousReading { get; set; }

        public Int64 Consumption { get; set; }
        public Int64 ExcessConsumption { get; set; }
        public Decimal dueWithDiscount { get; set; }
        public List<ChargesClass> ChargesList { get; set; }
        public List<DiscountClass> DiscountList { get; set; }
        public TransactionSummaryClass transummary { get; set; }
        public List<TransactionDetailClass> transdetail { get; set; }

    }

}
