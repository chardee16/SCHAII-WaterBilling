using System;
using System.Collections.Generic;
using System.Text;
using WaterBilling.Manager.Interface;
using WaterBilling.Repository.Interface;

namespace WaterBilling.Manager
{
    public abstract class BaseManager : IManager
    {
        internal abstract IRepository Repository { get; }

        public int Add<T>(T data)
        {
            return Repository.Insert(data);
        }

        public bool BulkAdd<T>(T data)
        {
            return Repository.AssociativeInsert(data);
        }

        public bool Delete(int[] ids)
        {
            return Repository.Delete(ids);
        }

        public bool Update<T>(T data)
        {
            return Repository.Update(data);
        }

        public IList<T> GetAll<T>()
        {
            return Repository.SelectAll<T>();
        }

        public T GetById<T>(int id)
        {
            return Repository.SelectById<T>(id);
        }

        public T GetById<T>(int? id)
        {
            if (id.HasValue)
            {
                return Repository.SelectById<T>(id.GetValueOrDefault());
            }

            return default(T);
        }

        public IList<T> GetByIds<T>(int[] ids)
        {
            return Repository.SelectByIds<T>(ids);
        }

        public IList<T> GetWithWhereCondition<T>(string condition)
        {
            return Repository.SelectWithWhereCondition<T>(condition);
        }

        public int GetCount(string condition)
        {
            return Repository.RowCount(condition);
        }

        public string ToMSSqlDateFormat(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public bool DeleteWithWhere(string condition)
        {
            return Repository.DeleteWithWhere(condition);
        }

        public bool AddNonIdentity<T>(T data)
        {
            return Repository.InsertNonIdentity<T>(data);
        }
    }
}
