using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdwardSoft.Core.Pattern
{
    public interface IRedisRepositoty
    {
        #region Features
        public string GenKey<T>(T data);

        public string GenKey<T>();

        public string GenKey<T>(object[] keys);

        #endregion

        #region Search
        public Task<string[]> SearchKeysAsync(string keyPattern);

        public Task<IEnumerable<T>> SearchKeysAsync<T>(string keyPattern = null);
        #endregion

        #region Get
        public Task<string> GetAsync(string key);

        public Task<T> GetAsync<T>(string key);

        public Task<T> GetAsync<T>(params object[] keys);

        public Task<T> GetAsync<T>(T data);

        public Task<string[]> GetAsync(List<string> keys);

        public Task<IEnumerable<T>> GetAsync<T>(List<string> keys);

        public Task<IEnumerable<T>> GetAsync<T>(List<T> data);
        #endregion

        #region Set
        public Task<bool> SetAsync(string key, string value, TimeSpan? slidingTime = null);

        public Task<bool> SetAsync<T>(T obj, string key = null, TimeSpan? slidingTime = null);

        public Task<bool[]> SetAsync<T>(List<T> data, TimeSpan? slidingTime = null);

        public Task<bool[]> SetAsync<T>(Dictionary<string, dynamic> data, TimeSpan? slidingTime = null);
        #endregion

        #region Remove  
        public Task<bool> RemoveAsync(string key);

        public Task<bool> RemoveAsync<T>(params object[] keys);

        public Task<bool[]> RemoveAsync(List<string> keys);

        public Task<bool[]> RemoveAsync<T>(List<T> data);

        public Task RemoveAllAsync();
        #endregion
    }
}
