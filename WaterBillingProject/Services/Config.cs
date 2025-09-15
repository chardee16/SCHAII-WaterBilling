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

        public static String IPAddress = "00ICT2019014\\DEVELOPER1";
        //public static String IPAddress = "10.18.10.43";
        public string SQLServerConnectionString = "Data Source=" + IPAddress + ";Database=WaterBillingDB;User ID=sa;Password=developer1@)@)";

        //public static String IPAddress = "DESKTOP-BJG7E88\\SCHHAI";
        //public static String IPAddress = "192.168.101.49";
        //public string SQLServerConnectionString = "Data Source=" + IPAddress + ";Database=WaterBillingDB;User ID=sa;Password=amac0386";

        public string SQLDirectory = CurrentPath.Replace("\\bin\\Debug", "\\SQLScripts\\");
        //public string SQLDirectory = CurrentPath + "\\SQLScripts\\";


    }
}
