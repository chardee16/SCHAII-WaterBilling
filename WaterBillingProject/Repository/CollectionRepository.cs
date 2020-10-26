using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBilling.Repository;
using WaterBilling.Services;
using WaterBillingProject.Models.Collection;

namespace WaterBillingProject.Repository
{
    public class CollectionRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";

        public List<CollectionBillsClass> GetBillingList(Int64 ClientID)
        {
            List<CollectionBillsClass> toReturn = new List<CollectionBillsClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetCollectionBillingList.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
              

                return Connection.Query<CollectionBillsClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }



        public List<CollectionChargesClass> GetCharges(Int64 ClientID)
        {
            List<CollectionChargesClass> toReturn = new List<CollectionChargesClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetCollectionCharges.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
                return Connection.Query<CollectionChargesClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }


        public List<CollectionDiscountClass> GetDiscount(Int64 ClientID)
        {
            List<CollectionDiscountClass> toReturn = new List<CollectionDiscountClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Collection\\GetCollectionDiscount.sql";
                sqlFile.setParameter("_ClientID", ClientID.ToString());
                return Connection.Query<CollectionDiscountClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
        }




    }
}
