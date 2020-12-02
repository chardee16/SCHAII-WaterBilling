using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Login
{
    public class LoginClass
    {
        public Int32 UserID { get; set; }
        public String Username { get; set; }
        public Boolean IsAdmin { get; set; }
        public Decimal MinimumBill { get; set; }
        public  Int32 MinimumConsumption { get; set; }
        public  Int32 ExcessPerCubic { get; set; }

    }
}
