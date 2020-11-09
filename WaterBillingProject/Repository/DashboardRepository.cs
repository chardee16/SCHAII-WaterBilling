using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBilling.Repository;
using WaterBilling.Services;
using WaterBillingProject.Models.Dashboard;
using WaterBillingProject.Services;

namespace WaterBillingProject.Repository
{
    public class DashboardRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";


        public List<DashboardMonthlyConsumption> GetMonthlyConsumption()
        {
            this.sqlFile.sqlQuery = _config.SQLDirectory + "Dashboard\\GetMonthlyUsage.sql";
            sqlFile.setParameter("_Year", LoginSession.TransYear.ToString());
            return Connection.Query<DashboardMonthlyConsumption>(this.sqlFile.sqlQuery).ToList();
        }



    }
}
