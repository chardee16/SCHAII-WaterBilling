using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Services
{
    public static class LoginSession 
    {

        public static Int32 UserID;

        public static String UserName;

        public static Int32 TransYear;

        public static DateTime TransDate;

        public static Boolean IsAdmin;

        public static Decimal MinimumBill;

        public static Int32 MinimumConsumption;

        public static Int32 ExcessPerCubic;

    }
}
