using System;
using System.Collections.Generic;
using System.Text;

namespace WaterBilling.Services
{
    public class Config
    {
        private static string CurrentPath = Environment.CurrentDirectory;

        //public static String IPAddress = "DESKTOP-QR1I6HF\\CHARDEE";
        //public string SQLServerConnectionString = "Data Source=" + IPAddress + ";Database=WaterBillingDB;User ID=sa;Password=~dimasalanG";

        public static String IPAddress = "DEVELOPER1\\DEVELOPER";
        public string SQLServerConnectionString = "Data Source=" + IPAddress + ";Database=WaterBillingDB;User ID=sa;Password=developer1@)@)";

        //public static String IPAddress = "DESKTOP-9JD2NFI\\SCHHAI";
        //public string SQLServerConnectionString = "Data Source=" + IPAddress + ";Database=WaterBillingDB;User ID=sa;Password=amac0386";

        public string SQLDirectory = CurrentPath.Replace("\\bin\\Debug", "\\SQLScripts\\");
        //public string SQLDirectory = CurrentPath + "\\SQLScripts\\";


    }
}
