
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WaterBilling.Services
{
    public class SQLFile
    {
        private String _FilePath = "";
        private String _Query = "";

        public String sqlQuery
        {
            get
            {
                return this._Query;
            }
            set
            {
                this._FilePath = value;


                String FilePath = this._FilePath;


                if (File.Exists(FilePath))
                {
                    this._Query = File.ReadAllText(FilePath);
                }
            }
        }

        public void setParameter(String parameter, String value)
        {
            this._Query = this._Query.Replace("@" + parameter, value);
        }

    }
}
