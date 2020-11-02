using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Collection
{
    public class TransactionDetailClass
    {
        public Int32 TransactionCode { get; set; }
        public Int32 TransYear { get; set; }
        public Int64 CTLNo { get; set; }
        public Int64 AccountCode { get; set; }
        public Int64 ClientID { get; set; }
        public String BillMonth { get; set; }
        public Int32 SLC_CODE { get; set; }
        public Int32 SLT_CODE { get; set; }
        public String ReferenceNo { get; set; }
        public Int32 SLE_CODE { get; set; }
        public Int32 StatusID { get; set; }
        public String TransactionDate { get; set; }
        public Decimal Amt { get; set; }
        public Int32 PostedBy { get; set; }
        public Int32 UPDTag { get; set; }
        public String ClientName { get; set; }
        public String SL_Description { get; set; }
    }
}
