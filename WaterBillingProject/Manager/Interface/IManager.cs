using System;
using System.Collections.Generic;
using System.Text;

namespace WaterBilling.Manager.Interface
{
    public interface IManager
    {
        int Add<T>(T data);
        bool AddNonIdentity<T>(T data);
        bool Update<T>(T data);
        bool Delete(int[] ids);
        bool DeleteWithWhere(string condition);
        int GetCount(string condition);
        IList<T> GetAll<T>();
        T GetById<T>(int id);
        T GetById<T>(int? id);
        IList<T> GetByIds<T>(int[] ids);
        IList<T> GetWithWhereCondition<T>(string condition);

        string ToMSSqlDateFormat(DateTime dt);
    }
}
