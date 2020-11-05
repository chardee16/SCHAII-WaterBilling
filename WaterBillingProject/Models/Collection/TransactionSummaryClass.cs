using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Collection
{
    public class TransactionSummaryClass
    {
        public Int32 TransactionCode { get; set; }
        public Int32 TransYear { get; set; }
        public Int64 CTLNo { get; set; }
        public String TransactionDate { get; set; }
        public Int64 ClientID { get; set; }
        public String Explanation { get; set; }
        public Int32 PostedBy { get; set; }
        public String ORNo { get; set; }
    }
}
