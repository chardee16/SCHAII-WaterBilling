using System;
using System.Collections.Generic;
using System.Text;

namespace WaterBilling.Repository.Interface
{
    public interface IRepository
    {
        int Insert<T>(T data);
        bool InsertNonIdentity<T>(T data);
        bool AssociativeInsert<T>(T data);
        bool Update<T>(T data);
        bool AssociativeDelete<T>(T data);
        bool Delete(int[] ids);
        bool DeleteWithWhere(string condition);

        int RowCount(string condition);

        IList<T> SelectAll<T>();
        T SelectById<T>(int id);
        IList<T> SelectByIds<T>(int[] ids);
        IList<T> SelectWithWhereCondition<T>(string condition);
    }
}
