using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBilling.Repository;
using WaterBilling.Services;
using WaterBillingProject.Models.Reports;

namespace WaterBillingProject.Repository
{
    public class ReportRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";



        public List<ReportClientListClass> GetClientList()
        {
            List<ReportClientListClass> toReturn = new List<ReportClientListClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Reports\\GetReportClientList.sql";
                return Connection.Query<ReportClientListClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }

        }



        public List<BillingReportClass> GetBillingReportList()
        {
            List<BillingReportClass> toReturn = new List<BillingReportClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Reports\\GetBillingReport.sql";

                return Connection.Query<BillingReportClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }

        }



        public List<ReportTransactionList> GetTransactionList(String DateFrom,String DateTo)
        {
            List<ReportTransactionList> toReturn = new List<ReportTransactionList>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Reports\\GetReportTransactionList.sql";
                sqlFile.setParameter("_DateFrom", DateFrom);
                sqlFile.setParameter("_DateTo", DateTo);
                return Connection.Query<ReportTransactionList>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }

        }




    }
}
