using System;
using System.Collections.Generic;
using System.Text;

namespace WaterBilling.Services
{
    public class Config
    {
        private static string CurrentPath = Environment.CurrentDirectory;

        public static String IPAddress = "DESKTOP-QR1I6HF\\CHARDEE";
        public string SQLServerConnectionString = "Data Source=" + IPAddress + ";Database=WaterBillingDB;User ID=sa;Password=~dimasalanG";

        public string SQLDirectory = CurrentPath.Replace("\\bin\\Debug", "\\SQLScripts\\");


    }
}
