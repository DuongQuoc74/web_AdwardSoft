using AdwardSoft.ValueObjects;
using AdwardSoft.ValueObjects.Generic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;


namespace AdwardSoft.Core.Pattern
{
    public interface IGenericRepository : IDisposable
    {
        #region Create
        Task<DataService<int>> CreateAsync<T>(T obj);
        Task<DataService<Q>> CreateAsync<T, Q>(T obj);
        Task<DataService<Q>> CreateAsync<T, Q>(T obj, string suffix);
        Task<DataService<int>> CreateAsync<T>(T obj, string ignorePara);
        Task<DataService<int>> CreateMultipleAsync<T>(List<T> objs);
        Task<DataService<int>> CreateMultipleAsync<T>(List<T> objs, string ignorePara);
        #endregion

        #region Read
        Task<DataService<IEnumerable<T>>> ReadAsync<T>();
        Task<DataService<IEnumerable<T>>> ReadByExpiryDateAsync<T>();
        Task<DataService<T>> ReadByIdAsync<T>(dynamic id);
        Task<DataService<IEnumerable<T>>> ReadCustomAsync<T>(Dictionary<string, dynamic> parms, string suffix);
        Task<DataService<T>> ReadByCustomAsync<T>(Dictionary<string, dynamic> parms, string suffix);
        Task<DataService<Q>> ReadByCustomAsync<T, Q>(Dictionary<string, dynamic> parms, string suffix);
        Task<DataService<ExpandoObject>> MultipleReadCustomAsync<T>(Dictionary<string, dynamic> parms, IEnumerable<MultipleDataEntry> mapItems, string suffix);

        #endregion

        #region Update
        Task<DataService<int>> UpdateAsync<T>(T obj);
        Task<DataService<Q>> UpdateAsync<T, Q>(T obj);
        Task<DataService<int>> UpdateMultipleAsync<T>(List<T> objs);
        Task<DataService<int>> UpdateAsync<T>(T obj, string ignorePara);
        #endregion

        #region Delete
        Task<DataService<Q>> DeteteAsync<T, Q>(Dictionary<string, dynamic> parms);
        Task<DataService<Q>> DeteteAsync<T, Q>(T obj, string suffix);
        #endregion
    }
}
