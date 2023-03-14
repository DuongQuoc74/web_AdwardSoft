using AdwardSoft.ORM.Caching.Distributed;
using System.Collections.Generic;
using AdwardSoft.Core.Pattern;
using System.Threading.Tasks;
using System;

namespace AdwardSoft.Repositories.Pattern
{
    public class RedisRepository : IRedisRepositoty
    {
        #region Constructor
        private readonly IRedisCache _redisCache;

        public RedisRepository(IRedisCache redisCache)
        {
            _redisCache = redisCache;
        }
        #endregion

        #region Features
        public string GenKey<T>(T data)
        {
            return _redisCache.GenKey<T>(data);
        }

        public string GenKey<T>()
        {
            return _redisCache.GenKey<T>();
        }

        public string GenKey<T>(object[] keys)
        {
            return _redisCache.GenKey<T>(keys);
        }

        #endregion

        #region Search
        /// <summary>
        /// Search keys
        /// </summary>
        /// <param name="keyPattern">*key | key* | *key*</param>
        /// <returns></returns>
        public async Task<string[]> SearchKeysAsync(string keyPattern)
        {
            return await _redisCache.SearchKeysAsync(keyPattern);
        }

        public async Task<IEnumerable<T>> SearchKeysAsync<T>(string keyPattern)
        {
            return await _redisCache.SearchKeysAsync<T>(keyPattern);
        }
        #endregion

        #region Get
        public async Task<string> GetAsync(string key)
        {
            return await _redisCache.GetAsync(key);
        }
        public async Task<T> GetAsync<T>(string key)
        {
            return await _redisCache.GetAsync<T>(key);
        }

        public async Task<T> GetAsync<T>(params object[] keys)
        {
            return await _redisCache.GetAsync<T>(keys);
        }
        public async Task<T> GetAsync<T>(T data)
        {
            return await _redisCache.GetAsync<T>(data);
        }
        public async Task<string[]> GetAsync(List<string> keys)
        {
            return await _redisCache.GetAsync(keys);
        }
        public async Task<IEnumerable<T>> GetAsync<T>(List<string> keys)
        {
            return await _redisCache.GetAsync<T>(keys);
        }
        public async Task<IEnumerable<T>> GetAsync<T>(List<T> data)
        {
            return await _redisCache.GetAsync<T>(data);
        }
        #endregion

        #region Set
        public async Task<bool> SetAsync(string key, string value, TimeSpan? slidingTime)
        {
            return await _redisCache.SetAsync(key, value, slidingTime);
        }

        public async Task<bool> SetAsync<T>(T obj, string key, TimeSpan? slidingTime)
        {
            return await _redisCache.SetAsync<T>(obj, key, slidingTime);
        }
        public async Task<bool[]> SetAsync<T>(Dictionary<string, dynamic> data, TimeSpan? slidingTime)
        {
            return await _redisCache.SetAsync<T>(data, slidingTime);
        }
        public async Task<bool[]> SetAsync<T>(List<T> data, TimeSpan? slidingTime)
        {
            return await _redisCache.SetAsync<T>(data, slidingTime);
        }
        #endregion

        #region Remove
        public async Task<bool> RemoveAsync(string key)
        {
            return await _redisCache.RemoveAsync(key);
        }
        public async Task<bool> RemoveAsync<T>(params object[] keys)
        {
            return await _redisCache.RemoveAsync<T>(keys);
        }
        public async Task<bool[]> RemoveAsync<T>(List<T> data)
        {
            return await _redisCache.RemoveAsync<T>(data);
        }
        public async Task<bool[]> RemoveAsync(List<string> keys)
        {
            return await _redisCache.RemoveAsync(keys);
        }
        public async Task RemoveAllAsync()
        {
            await _redisCache.RemoveAllAsync();
        }
        #endregion
    }
}
