using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdwardSoft.Core.Pattern
{
    public interface IMongoDbRepository
    {
        #region Find
        public Task<List<T>> FindAsync<T>(FilterDefinition<T> filter, FindOptions<T> findOptions = null);

        public Task<T> FindOneAsync<T>(FilterDefinition<T> filter, FindOptions<T> findOptions = null);

        public IFindFluent<T, T> FindFluent<T>(FilterDefinition<T> filter, FindOptions findOptions = null, int numberOfItems = 0);
        #endregion

        #region Update
        public Task<UpdateResult> UpdateOneAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, UpdateOptions updateOpts = null);
        public Task<UpdateResult> UpdateManyAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, UpdateOptions updateOpts = null);
        #endregion

        #region Insert
        public Task InsertOneAsync<T>(T data, InsertOneOptions insertOneOpt = null);
        public Task InsertManyAsync<T>(IEnumerable<T> data, InsertManyOptions insertManyOpt = null);
        #endregion

        #region Delete  
        public Task<DeleteResult> DeleteOneAsync<T>(FilterDefinition<T> filter);

        public Task<DeleteResult> DeleteManyAsync<T>(FilterDefinition<T> filter);
        #endregion
    }
}
