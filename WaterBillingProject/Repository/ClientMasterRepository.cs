using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using WaterBilling.Models.ClientMaster;
using WaterBilling.Services;

namespace WaterBilling.Repository
{
    public class ClientMasterRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";

        public List<ClientStatusClass> GetClientStatus()
        {
            this.sqlFile.sqlQuery = _config.SQLDirectory + "ClientMaster\\GetClientStatus.sql";
            return Connection.Query<ClientStatusClass>(this.sqlFile.sqlQuery).ToList();
        }

        public List<ClientClass> GetClientList()
        {
            List<ClientClass> toReturn = new List<ClientClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "ClientMaster\\GetClientList.sql";
                return Connection.Query<ClientClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }
            
        }


        public Boolean InsertClient(ClientClass client)
        {
            ClientClass clientClass = client;
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "ClientMaster\\InsertClient.sql";
                sqlFile.setParameter("_FirstName", clientClass.FirstName);
                sqlFile.setParameter("_MiddleName", clientClass.MiddleName);
                sqlFile.setParameter("_LastName", clientClass.LastName);
                sqlFile.setParameter("_BlockNo", clientClass.BlockNo.ToString());
                sqlFile.setParameter("_LotNo", clientClass.LotNo.ToString());
                sqlFile.setParameter("_ClientAccountStatusID", clientClass.ClientAccountStatusID.ToString());
                sqlFile.setParameter("_IsSenior", clientClass.IsSenior.ToString());
                sqlFile.setParameter("_Occupants", clientClass.Occupants.ToString());
                sqlFile.setParameter("_SeniorCount", clientClass.SeniorCount.ToString());

                var affectedRow = Connection.Execute(sqlFile.sqlQuery);
                if (affectedRow > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Boolean EditClient(ClientClass client)
        {
            ClientClass clientClass = client;
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "ClientMaster\\UpdateClient.sql";
                sqlFile.setParameter("_ClientID", clientClass.ClientID.ToString());
                sqlFile.setParameter("_FirstName", clientClass.FirstName);
                sqlFile.setParameter("_MiddleName", clientClass.MiddleName);
                sqlFile.setParameter("_LastName", clientClass.LastName);
                sqlFile.setParameter("_BlockNo", clientClass.BlockNo.ToString());
                sqlFile.setParameter("_LotNo", clientClass.LotNo.ToString());
                sqlFile.setParameter("_ClientAccountStatusID", clientClass.ClientAccountStatusID.ToString());
                sqlFile.setParameter("_IsSenior", clientClass.IsSenior.ToString());
                sqlFile.setParameter("_Occupants", clientClass.Occupants.ToString());
                sqlFile.setParameter("_SeniorCount", clientClass.SeniorCount.ToString());

                var affectedRow = Connection.Execute(sqlFile.sqlQuery);
                if (affectedRow > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }






    }
}
