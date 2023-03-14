using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
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
    [Authorize]
    public class CustomerGroupController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;

        public CustomerGroupController(
            IRedisRepositoty redis,
            IGenericRepository generic)
        {
            _redis = redis;
            _generic = generic;
        }

        #endregion

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<CustomerGroup>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "CustomerGroup");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);

            var result = await _generic.ReadByCustomAsync<CustomerGroup>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(short pricingPolicy = -1, short status = -1)
        {
            var param = DataHelper.GenParams("PricingPolicy", pricingPolicy, "Status", status);

            var result = await _generic.ReadCustomAsync<CustomerGroupDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CustomerGroup obj)
        {
            // => Create Sql
            var result = await _generic.CreateAsync<CustomerGroup, CustomerGroupRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<CustomerGroupRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CustomerGroup obj)
        {
            // => Check CustomerGroup is using by Product
            // => If using, not allow update Status
            var isUsing = await _CheckIsUsing(id: obj.Id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing && obj.Status == 0)
                return BadRequest("Customer group is using, not allow update status !");

            // => Update sql
            var result = await _generic.UpdateAsync<CustomerGroup, CustomerGroupRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<CustomerGroupRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            // => Check CustomerGroup is using by Product
            var isUsing = await _CheckIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
                return BadRequest("Customer group is using, not allow remove !");
            
            // => Delete sql
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<CustomerGroup, int>(param);

            if (!result.Success && result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Redis
            var key = _redis.GenKey<CustomerGroupRedis>(new CustomerGroupRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }

        #endregion

        #region Check

        [HttpGet("CheckUpdateStatusIsUsing")]
        public async Task<IActionResult> CheckUpdateStatusIsUsing(int id, short status)
        {
            var isUsing = await _CheckIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing && status == 0)
                return Ok(1);
            
            return Ok(0);
        }

        [HttpGet("CheckIsUsing")]
        public async Task<IActionResult> CheckIsUsing(int id)
        {
            var isUsing = await _CheckIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            return Ok((int)isUsing.code);
        }

        #endregion

        #region Methods

        private async Task<(EResponse code, string message)> _CheckIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<CustomerGroup, int>(param, "CheckIsUsing");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response == 0)
                return (EResponse.NotUsing, "");

            return (EResponse.IsUsing, "");
        }

        #endregion

        #region Sync

        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedis()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<CustomerGroupRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<CustomerGroupRedis>(keyToDelete + "*");

            await _redis.RemoveAsync<CustomerGroupRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<CustomerGroupRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<CustomerGroupRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }

        #endregion
    }
}
