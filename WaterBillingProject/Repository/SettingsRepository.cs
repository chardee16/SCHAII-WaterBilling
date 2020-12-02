using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterBilling.Repository;
using WaterBilling.Services;
using WaterBillingProject.Models.Settings;
using WaterBillingProject.Services;

namespace WaterBillingProject.Repository
{
    public class SettingsRepository : BaseRepository
    {
        SQLFile sqlFile = new SQLFile();
        Config _config = new Config();
        internal override string TableName => "tblApplication";




        public Boolean InsertUser(UserClass user)
        {
            UserClass userClass = user;
            try
            {
                byte[] ba = Encoding.Default.GetBytes(Encryption.Encrypt(userClass.Password));
                var hexString = BitConverter.ToString(ba);

                this.sqlFile.sqlQuery = _config.SQLDirectory + "Settings\\InsertUser.sql";
                sqlFile.setParameter("_FirstName", userClass.FirstName);
                sqlFile.setParameter("_MiddleName", userClass.MiddleName);
                sqlFile.setParameter("_LastName", userClass.LastName);
                sqlFile.setParameter("_Username", userClass.Username);
                sqlFile.setParameter("_Password", hexString);
                sqlFile.setParameter("_IsAdmin", userClass.IsAdministrator.ToString());


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



        public Boolean UpdateUser(UserClass user)
        {
            UserClass userClass = user;
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Settings\\UpdateUser.sql";
                sqlFile.setParameter("_UserID", userClass.UserID.ToString());
                sqlFile.setParameter("_FirstName", userClass.FirstName);
                sqlFile.setParameter("_MiddleName", userClass.MiddleName);
                sqlFile.setParameter("_LastName", userClass.LastName);
                sqlFile.setParameter("_IsAdmin", userClass.IsAdministrator.ToString());


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



        public Boolean UpdatePassword(UserClass user)
        {
            UserClass userClass = user;
            try
            {
                byte[] ba = Encoding.Default.GetBytes(Encryption.Encrypt(userClass.Password));
                var hexString = BitConverter.ToString(ba);

                this.sqlFile.sqlQuery = _config.SQLDirectory + "Settings\\UpdatePassword.sql";
                sqlFile.setParameter("_UserID", userClass.UserID.ToString());
                sqlFile.setParameter("_Password", hexString);


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


        public List<UserClass> GetUserList()
        {
            List<UserClass> toReturn = new List<UserClass>();
            try
            {
                this.sqlFile.sqlQuery = _config.SQLDirectory + "Settings\\GetUserList.sql";
                return Connection.Query<UserClass>(this.sqlFile.sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                return toReturn;
            }

        }


    }
}
