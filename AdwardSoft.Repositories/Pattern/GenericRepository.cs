using AdwardSoft.ValueObjects.Generic;
using AdwardSoft.Utilities.Helper;
using System.Collections.Generic;
using AdwardSoft.ValueObjects;
using AdwardSoft.Core.Pattern;
using System.Threading.Tasks;
using AdwardSoft.ORM.Dapper;
using System.Dynamic;
using System;

namespace AdwardSoft.Repositories.Pattern
{
    public class GenericRepository : IGenericRepository
    {
        #region Constructor
        private IAdapterPattern _adapter;

        public GenericRepository(IAdapterPattern adapter)
        {
            _adapter = adapter;
        }
        #endregion

        #region Read
        public async Task<DataService<IEnumerable<T>>> ReadAsync<T>()
        {
            try
            {
                var ret = await _adapter.Query<T>(null);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<IEnumerable<T>>();
            }
        }

        public async Task<DataService<T>> ReadByIdAsync<T>(dynamic id)
        {
            try
            {
                T ret = await _adapter.QuerySingle<T>(DataHelper.GenParams("Id", id));
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<T>();
            }
        }

        public virtual async Task<DataService<IEnumerable<T>>> ReadCustomAsync<T>(Dictionary<string, dynamic> parms, string suffix)
        {
            try
            {
                IEnumerable<T> ret = await _adapter.Query<T>(parms, DataHelper.ApiCRUD.Read, suffix);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<IEnumerable<T>>();
            }
        }
        public virtual async Task<DataService<T>> ReadByCustomAsync<T>(Dictionary<string, dynamic> parms, string suffix)
        {
            try
            {
                T ret = await _adapter.QuerySingle<T>(parms, DataHelper.ApiCRUD.Read, suffix);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<T>();
            }
        }

        public virtual async Task<DataService<Q>> ReadByCustomAsync<T, Q>(Dictionary<string, dynamic> parms, string suffix)
        {
            try
            {
                Q ret = await _adapter.QuerySingle<T, Q>(parms, DataHelper.ApiCRUD.Read, suffix);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<Q>();
            }
        }

        public virtual async Task<DataService<ExpandoObject>> MultipleReadCustomAsync<T>(Dictionary<string, dynamic> parms, IEnumerable<MultipleDataEntry> mapItems, string suffix)
        {
            try
            {
                ExpandoObject ret = await _adapter.QueryMultiple<T>(parms, mapItems, DataHelper.ApiCRUD.Read, suffix);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<ExpandoObject>();
            }
        }

        public async Task<DataService<IEnumerable<T>>> ReadByExpiryDateAsync<T>()
        {
            try
            {
                var ret = await _adapter.Query<T>(null, DataHelper.ApiCRUD.Read, "ByExpiryDate");
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<IEnumerable<T>>();
            }
        }
        #endregion

        #region Create
        public async Task<DataService<int>> CreateAsync<T>(T obj)
        {
            try
            {
                var ret = await _adapter.ExecuteSingle(obj);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<int>();
            }
        }

        public async Task<DataService<Q>> CreateAsync<T, Q>(T obj)
        {
            try
            {
                var ret = await _adapter.QuerySingle<T, Q>(obj, DataHelper.ApiCRUD.Create);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<Q>();
            }
        }
        
        public async Task<DataService<Q>> CreateAsync<T, Q>(T obj, string suffix)
        {
            try
            {
                var ret = await _adapter.QuerySingle<T, Q>(obj, DataHelper.ApiCRUD.Create, suffix);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<Q>();
            }
        }

        public async Task<DataService<int>> CreateAsync<T>(T obj, string ignorePara)
        {
            try
            {
                var ret = await _adapter.ExecuteSingle(obj, ignorePara: ignorePara);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<int>();
            }
        }

        public async Task<DataService<int>> CreateMultipleAsync<T>(List<T> objs)
        {
            try
            {
                var ret = await _adapter.ExecuteMultiple<T>(objs);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<int>();
            }
        }

        public async Task<DataService<int>> CreateMultipleAsync<T>(List<T> objs, string ignorePara)
        {
            try
            {
                var ret = await _adapter.ExecuteMultiple<T>(objs, ignorePara: ignorePara);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<int>();
            }
        }
        #endregion

        #region Update
        public async Task<DataService<int>> UpdateAsync<T>(T obj)
        {
            try
            {
                var ret = await _adapter.ExecuteSingle(obj, DataHelper.ApiCRUD.Update);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<int>();
            }
        }

        public async Task<DataService<Q>> UpdateAsync<T, Q>(T obj)
        {
            try
            {
                var ret = await _adapter.QuerySingle<T, Q>(obj, DataHelper.ApiCRUD.Update);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<Q>();
            }
        }

        public async Task<DataService<int>> UpdateMultipleAsync<T>(List<T> objs)
        {
            try
            {
                var ret = await _adapter.ExecuteMultiple<T>(objs, DataHelper.ApiCRUD.Update);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<int>();
            }
        }

        public async Task<DataService<int>> UpdateAsync<T>(T obj, string ignorePara)
        {
            try
            {
                var ret = await _adapter.ExecuteSingle(obj, DataHelper.ApiCRUD.Update, ignorePara: ignorePara);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<int>();
            }
        }

        #endregion

        #region Delete
        public async Task<DataService<Q>> DeteteAsync<T, Q>(Dictionary<string, dynamic> parms)
        {
            try
            {
                Q ret = await _adapter.ExecuteSingle<T, Q>(parms, DataHelper.ApiCRUD.Delete);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<Q>();
            }
        }

        public async Task<DataService<Q>> DeteteAsync<T, Q>(T obj, string suffix)
        {
            try
            {
                Q ret = await _adapter.ExecuteSingle<T, Q>(obj, DataHelper.ApiCRUD.Delete, suffix);
                return ret.FlushData();
            }
            catch (Exception ex)
            {
                return ex.FatalException<Q>();
            }
        }
        #endregion

        #region Disposed
        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        #endregion
    }
}
