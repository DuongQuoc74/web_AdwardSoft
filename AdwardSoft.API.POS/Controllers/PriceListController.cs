using System;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis;
using AdwardSoft.DTO.Generic;
using Nest;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PriceListController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public PriceListController(
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
        [HttpGet("Read")]
        public async Task<IActionResult> Read(DateTime dateFrom, DateTime dateTo)
        {
            var param = DataHelper.GenParams("DateFrom", dateFrom, "DateTo", dateTo);

            var result = await _generic.ReadCustomAsync<PriceList>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "PriceList");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadByDate")]
        public async Task<IActionResult> ReadByDate(DateTime date)
        {
            var result = await _ReadByDate(date: date);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.response);
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(PriceList obj)
        {
            // => Check Date Existing
            var dateIsExisting = await _CheckDateIsExisting(date: obj.Date);

            if (dateIsExisting.code == EResponse.Error)
                return BadRequest(dateIsExisting.message);

            if (dateIsExisting.code == EResponse.IsExisting)
                return BadRequest("Existing date.");

            // => Create PriceList
            var result = await _generic.CreateAsync<PriceList, PriceListRedis>(obj);
            
            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<PriceListRedis>(result.Response);

            return Ok(result.Response.Date.CompareTo(default(DateTime)));
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(PriceList obj)
        {
            // => Update PriceList
            var result = await _generic.UpdateAsync<PriceList, PriceListRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<PriceListRedis>(result.Response);

            return Ok(result.Response.Date.CompareTo(default(DateTime)));
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(DateTime date)
        {
            // => Check PriceList is using by CustomerOrder
            var isUsing = await _CheckIsUsing(date: date);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
                return BadRequest("PriceList is using, not allow remove !");

            // => Delete sql
            var param = DataHelper.GenParams("Date", date);
            var result = await _generic.DeteteAsync<PriceList, int>(param);

            if (!result.Success && result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Redis
            var key = _redis.GenKey<PriceListRedis>(new PriceListRedis { Date = date });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, PriceList response)> _ReadByDate(DateTime date)
        {
            var param = DataHelper.GenParams("Date", date);
            var result = await _generic.ReadByCustomAsync<PriceList>(param, "ByDate");

            return (result.Success, result.Messages, result.Response);
        }
        private async Task<(EResponse code, string message)> _CheckIsUsing(DateTime date)
        {
            var param = DataHelper.GenParams("Date", date);
            var result = await _generic.ReadByCustomAsync<PriceList, int>(param, "CheckIsUsing");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response == 0)
                return (EResponse.NotUsing, "");

            return (EResponse.IsUsing, "");
        }

        private async Task<(EResponse code, string message)> _CheckDateIsExisting(DateTime date)
        {
            var param = DataHelper.GenParams("Date", date);

            var result = await _generic.ReadByCustomAsync<PriceList, int>(param, "DateIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }
        #endregion

        #region Sync
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<PriceListRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<PriceListRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<PriceListRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<PriceListRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<PriceListRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }
        #endregion
    }
}
