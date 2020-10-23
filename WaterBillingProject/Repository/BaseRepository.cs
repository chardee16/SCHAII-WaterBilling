using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using WaterBilling.Repository.Interface;
using WaterBilling.Services;

namespace WaterBilling.Repository
{
    public abstract class BaseRepository : IRepository
    {
        public SQLFile queryFile = new SQLFile();

        IDbConnection _connection;

        internal abstract string TableName { get; }

        internal IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var source = new Config();
                    var connectionString = source.SQLServerConnectionString;
                    _connection = new SqlConnection(connectionString);
                }

                return _connection;
            }
        }

        public bool AssociativeDelete<T>(T data)
        {
            var condition = string.Join(" AND ", data.GetType().GetProperties().Select(x => $"{x.Name} = @{x.Name}"));

            var sql = $@"DELETE FROM {TableName}
                        WHERE {condition}";

            return Connection.Execute(sql, data) > 0;
        }

        public bool AssociativeInsert<T>(T data)
        {
            var dynamicList = ((IEnumerable<Object>)data).ToList();

            System.Type myType = dynamicList[0].GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());


            var fields = string.Empty;
            List<string> fieldSet = new List<string>();
            foreach (PropertyInfo prop in props)
            {
                var propertyName = $"{prop.Name}".ToLower();
                string[] toBeExcludedProperties =
                {
                    "id"
                };

                if (!toBeExcludedProperties.Contains(propertyName))
                {
                    fieldSet.Add(prop.Name);
                }
            }

            fields = string.Join(",", fieldSet);

            var values = string.Empty;
            List<string> valueSet = new List<string>();
            foreach (PropertyInfo prop in props)
            {
                var propertyName = $"{prop.Name}".ToLower();
                string[] toBeExcludedProperties =
                {
                    "id"
                };

                if (!toBeExcludedProperties.Contains(propertyName))
                {
                    valueSet.Add($"@{prop.Name}");
                }
            }

            values = string.Join(",", valueSet);


            var sql = $@"INSERT INTO {TableName} ({fields})
                        VALUES ({values})";


            return Connection.Execute(sql, data) > 0;
        }

        //public bool BulkInsertData<T>(T data)
        //{
        //    Type typeParameterType = typeof(T);
        //}

        public bool Delete(int[] ids)
        {
            var sql = $@"DELETE FROM {TableName}
                        WHERE id IN @Ids";

            return Connection.Execute(sql, new { Ids = ids }) > 0;
        }



        public int Insert<T>(T data)
        {
            var fields = string.Join(",", data.GetType().GetProperties().Where(x =>
            {
                var propertyName = $"{x.Name}".ToLower();
                string[] toBeExcludedProperties =
                {
                    "id"
                };
                return !toBeExcludedProperties.Contains(propertyName);
            }).Select(x => $"{x.Name}"));

            var values = string.Join(",", data.GetType().GetProperties().Where(x =>
            {
                var propertyName = $"{x.Name}".ToLower();
                string[] toBeExcludedProperties =
                {
                    "id"
                };
                return !toBeExcludedProperties.Contains(propertyName);
            }).Select(x => $"@{x.Name}"));

            var sql = $@"DECLARE @SelectedId int;
                        INSERT INTO {TableName} ({fields})
                        VALUES ({values})
                        SET @SelectedId = SCOPE_IDENTITY();
                        SELECT @SelectedId";

            return Connection.Query<int>(sql, data).SingleOrDefault();
        }

        public bool InsertNonIdentity<T>(T data)
        {
            var fields = string.Join(",", data.GetType().GetProperties().Where(x =>
            {
                var propertyName = $"{x.Name}".ToLower();
                string[] toBeExcludedProperties =
                {

                };
                return !toBeExcludedProperties.Contains(propertyName);
            }).Select(x => $"{x.Name}"));

            var values = string.Join(",", data.GetType().GetProperties().Where(x =>
            {
                var propertyName = $"{x.Name}".ToLower();
                string[] toBeExcludedProperties =
                {

                };
                return !toBeExcludedProperties.Contains(propertyName);
            }).Select(x => $"@{x.Name}"));

            var sql = $@"INSERT INTO {TableName} ({fields})
                        VALUES ({values})";

            return Connection.Execute(sql, data) > 0;
        }

        public int RowCount(string condition)
        {
            throw new NotImplementedException();
        }

        public IList<T> SelectAll<T>()
        {
            var sql = $@"SELECT * FROM {TableName}";

            return Connection.Query<T>(sql).ToList();
        }

        public T SelectById<T>(int id)
        {
            var sql = $@"SELECT * FROM {TableName} WHERE id = @Id";

            return Connection.Query<T>(sql, new { Id = id }).FirstOrDefault();
        }

        public IList<T> SelectByIds<T>(int[] ids)
        {
            var sql = $@"SELECT * FROM {TableName} WHERE id in @ids";

            return Connection.Query<T>(sql, new { Ids = ids }).ToList();
        }

        public IList<T> SelectWithWhereCondition<T>(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                throw new MissingFieldException("Condition not found.");
            }

            var sql = $@"SELECT * FROM {TableName} WHERE {condition}";

            return Connection.Query<T>(sql).ToList();
        }

        public bool Update<T>(T data)
        {
            var fields = string.Join(",", data.GetType().GetProperties().Where(x =>
            {
                var propertyName = $"{x.Name}".ToLower();
                string[] toBeExcludedProperties =
                {
                    "id", "createdon", "createdby"
                };
                return !toBeExcludedProperties.Contains(propertyName);
            }).Select(x => $"{x.Name} = @{x.Name}"));
            var sql = $@"UPDATE {TableName} SET {fields} WHERE id = @Id";

            return Connection.Execute(sql, data) > 0;
        }

        public bool DeleteWithWhere(string condition)
        {
            if (string.IsNullOrEmpty(condition))
            {
                throw new MissingFieldException("Condition not found.");
            }

            var sql = $"DELETE FROM {TableName} WHERE {condition}";

            return Connection.Execute(sql) > 0;
        }
    

    }
}
