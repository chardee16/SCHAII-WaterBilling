using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Collection
{
    public class CollectionEntryClass
    {
        public String SL_Description { get; set; }
        public Int64 AccountCode { get; set; }
        public String BillMonth { get; set; }
        public String Debit { get; set; }
        public String Credit { get; set; }
    }
}
