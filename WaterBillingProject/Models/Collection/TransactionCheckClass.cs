using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Collection
{
    public class TransactionCheckClass
    {
        public Int32 TransactionCode { get; set; }
        public Int64 CTLNo { get; set; }
        public Int32 TransYear { get; set; }
        public Int32 COCIType { get; set; }
        public Decimal Amt { get; set; }
        public Int32 UPDTag { get; set; }
    }
}
