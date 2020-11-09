using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WaterBilling.Repository;
using WaterBilling.Services;
using WaterBillingProject.Models.Login;
using WaterBillingProject.Services;

namespace WaterBillingProject.Repository
{
    public class LoginRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";


        public LoginClass AuthenticateUser(String Username, String Password)
        {
            LoginClass toReturn = new LoginClass();
            try
            {
                byte[] ba = Encoding.Default.GetBytes(Encryption.Encrypt(Password));
                var hexString = BitConverter.ToString(ba);
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Login\\AuthenticateUser.sql";
                sqlFile.setParameter("_Username", Username);
                sqlFile.setParameter("_Password", hexString);
                return Connection.Query<LoginClass>(this.sqlFile.sqlQuery).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return toReturn;
            }

        }


    }
}
