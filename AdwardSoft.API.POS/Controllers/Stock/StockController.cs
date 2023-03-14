using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public StockController(
            IRedisRepositoty redis,
            IGenericRepository generic,
            IAdapterPattern adapter)
        {
            _redis = redis;
            _generic = generic;
            _adapter = adapter;
        }

        #endregion

        #region Read
        [AllowAnonymous]
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Stock>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "Stock");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelectByBranch")]
        public async Task<IActionResult> ReadSelectByBranch(int branchId)
        {
            var result = await _generic.ReadCustomAsync<Select>(DataHelper.GenParams("BranchId", branchId), "StockByBranch");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _ReadById(id);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.response);
        }

        [HttpGet("ReadByBranchId")]
        public async Task<IActionResult> ReadByBranchId(int branchId)
        {
            //usp_Stock_ReadByBranchId
            var result = await _generic.ReadCustomAsync<Stock>(DataHelper.GenParams("BranchId", branchId), "ByBranchId");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatableAsync(int branchId)
        {
            var param = DataHelper.GenParams("BranchId", branchId );

            var result = await _generic.ReadCustomAsync<StockDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(Stock obj)
        {
            var redisReturn = await _generic.CreateAsync<Stock, StockRedis>(obj);

            if (!redisReturn.Success && redisReturn.Response == null)
                return BadRequest(redisReturn.Messages);

            // 
            // Redis
            //

            // => Check is default
            if (obj.IsDefault)
            {
                // => change default
                var replacedDefault = _redis.GenKey<StockRedis>(new StockRedis { BranchId = redisReturn.Response.BranchId });

                var redisResponse = await _redis.SearchKeysAsync<StockRedis>(replacedDefault);

                var defauting = redisResponse.FirstOrDefault(x => x.IsDefault);

                if (defauting != null)
                {
                    defauting.IsDefault = false;
                    await _redis.SetAsync<StockRedis>(defauting);
                }
            }

            // => Store redis
            await _redis.SetAsync<StockRedis>(redisReturn.Response);

            return Ok(redisReturn.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(Stock obj)
        {

            var redisReturn = await _generic.UpdateAsync<Stock, StockRedis>(obj);

            if (!redisReturn.Success && redisReturn.Response == null)
                return BadRequest(redisReturn.Messages);

            //
            // Redis
            //

            // => get current stock
            var keyCurrentStock = _redis.GenKey<StockRedis>(new StockRedis { Id = obj.Id, BranchId = obj.BranchId });
            var currentStock = await _redis.GetAsync<Stock>(keyCurrentStock);

            if (currentStock != null)
            {
                // => if current is not defaulting
                // => want to set default 
                if (!currentStock.IsDefault && obj.IsDefault)
                {
                    // => find defaulting in redis and set not default
                    var keys = _redis.GenKey<StockRedis>(new StockRedis { BranchId = obj.BranchId });
                    var stockRedis = await _redis.SearchKeysAsync<StockRedis>(keys);
                    var defaulting = stockRedis.FirstOrDefault(x => x.IsDefault);

                    if (defaulting != null)
                    {
                        // => Set defaulting is not default
                        defaulting.IsDefault = false;
                        await _redis.SetAsync<StockRedis>(defaulting);
                    }
                }


                // => if current is defaulting
                // => want to set not default
                if (currentStock.IsDefault && !obj.IsDefault)
                {
                    // => Find new defaulting in sql
                    var newDefaulting = await _ReadDefaulting(branchId: obj.BranchId);

                    if (newDefaulting.success && newDefaulting.response != null && newDefaulting.response.Id > 0)
                    {
                        // => Find in redis
                        var keyDefaulting = _redis.GenKey<StockRedis>(new StockRedis { Id = newDefaulting.response.Id });
                        var defaultingRedis = await _redis.SearchKeysAsync<StockRedis>(keyDefaulting);

                        if (defaultingRedis.Any())
                        {
                            var defaulting = defaultingRedis.FirstOrDefault();
                            if (defaulting != null)
                            {
                                // => change new defaulting redis
                                defaulting.IsDefault = true;
                                await _redis.SetAsync<StockRedis>(defaulting);
                            }
                            else
                            {
                                // => add new defaulting redis
                                await _redis.SetAsync<StockRedis>(newDefaulting.response);
                            }
                        }
                    }
                    await _redis.SetAsync<StockRedis>(newDefaulting.response);
                }
            }

            // => Store redis
            await _redis.SetAsync<StockRedis>(redisReturn.Response);

            return Ok(redisReturn.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Get BranchId
            var stock = await _ReadById(id);

            if (!stock.success && stock.response == null)
                return BadRequest(stock.message);

            var param = DataHelper.GenParams("Id", id);

            var result = await _generic.DeteteAsync<Stock, int>(param);

            if (!result.Success && result.Response > 0)
                return BadRequest(result.Messages);

            //
            // Redis
            //
            var key = _redis.GenKey<StockRedis>(new StockRedis { Id = id });
            var redisResponse = await _redis.SearchKeysAsync<StockRedis>(key);

            if (redisResponse.Any())
            {
                var stockRedis = redisResponse.FirstOrDefault();

                if (stockRedis != null && stockRedis.IsDefault)
                {
                    // => Find new defaulting in sql
                    var newDefaulting = await _ReadDefaulting(stockRedis.BranchId);

                    if (newDefaulting.success && newDefaulting.response != null && newDefaulting.response.Id > 0)
                    {
                        // => Find in redis
                        var keyDefaulting = _redis.GenKey<StockRedis>(new StockRedis { Id = newDefaulting.response.Id });
                        var defaultingRedis = await _redis.SearchKeysAsync<StockRedis>(keyDefaulting);

                        if (defaultingRedis.Any())
                        {
                            var defaulting = defaultingRedis.FirstOrDefault();
                            if (defaulting != null)
                            {
                                // => change new defaulting redis
                                defaulting.IsDefault = true;
                                await _redis.SetAsync<StockRedis>(defaulting);
                            }
                            else
                            {
                                // => add new defaulting redis
                                await _redis.SetAsync<StockRedis>(newDefaulting.response);
                            }
                        }
                    }
                }
            }

            // => Remove Redis
            await _redis.RemoveAsync<StockRedis>(redisResponse.ToList());

            return Ok(result.Response);
        }

        #endregion

        #region Methods

        private async Task<(bool success, string message, StockRedis response)> _ReadDefaulting(int branchId)
        {
            var param = DataHelper.GenParams("BranchId", branchId);
            var result = await _generic.ReadByCustomAsync<StockRedis>(param, "Default");

            return (result.Success, result.Messages, result.Response);
        }

        private async Task<(bool success, string message, Stock response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Stock>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }

        private async Task<(bool success, string message, int count)> _SyncRedis<T>()
        {
            // => Delete Membership Class in redis
            var keyToDelete = _redis.GenKey<T>();
            var redisToDelete = await _redis.SearchKeysAsync<T>(keyToDelete + ":*");

            await _redis.RemoveAsync<T>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<T>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);


            await _redis.SetAsync<T>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        #endregion

        #region Sync

        [HttpGet("SyncRedis")]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _SyncRedis<StockRedis>();

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.count);
        }

        #endregion
    }
}
