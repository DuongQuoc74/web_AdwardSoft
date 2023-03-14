using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.ORM.MongoDb;
using MongoDB.Driver;

namespace AdwardSoft.Repositories.Pattern
{
    public class MongoDbRepository: IMongoDbRepository
    {
        #region Constructor
        private readonly IMongodbClient _mongodbClient;

        public MongoDbRepository(IMongodbClient mongodbClient)
        {
            _mongodbClient = mongodbClient;
        }
        #endregion

        #region Find
        public async Task<List<T>> FindAsync<T>(FilterDefinition<T> filter, FindOptions<T> findOptions)
        {
            return await _mongodbClient.FindAsync(filter, findOptions);
        }

        public async Task<T> FindOneAsync<T>(FilterDefinition<T> filter, FindOptions<T> findOptions)
        {
            return await _mongodbClient.FindOneAsync(filter, findOptions);
        }
        public IFindFluent<T, T> FindFluent<T>(FilterDefinition<T> filter, FindOptions findOptions, int numberOfItems)
        {
            return _mongodbClient.FindFluent(filter, findOptions, numberOfItems);
        }
        #endregion

        #region Update
        public async Task<UpdateResult> UpdateOneAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, UpdateOptions updateOpts)
        {
            return await _mongodbClient.UpdateOneAsync<T>(filter, updateDefinition, updateOpts);
        }
        public async Task<UpdateResult> UpdateManyAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> updateDefinition, UpdateOptions updateOpts)
        {
            return await _mongodbClient.UpdateManyAsync<T>(filter, updateDefinition, updateOpts);
        }
        #endregion

        #region Insert
        public async Task InsertOneAsync<T>(T data, InsertOneOptions insertOneOpt)
        {
            await _mongodbClient.InsertOneAsync<T>(data, insertOneOpt);
        }
        public async Task InsertManyAsync<T>(IEnumerable<T> data, InsertManyOptions insertManyOpt)
        {
            await _mongodbClient.InsertManyAsync<T>(data, insertManyOpt);
        }
        #endregion

        #region Delete  
        public async Task<DeleteResult> DeleteOneAsync<T>(FilterDefinition<T> filter)
        {
            return await _mongodbClient.DeleteOneAsync<T>(filter);
        }

        public async Task<DeleteResult> DeleteManyAsync<T>(FilterDefinition<T> filter)
        {
            return await _mongodbClient.DeleteManyAsync<T>(filter);
        }
        #endregion
    }
}
