using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PaymentMethodController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public PaymentMethodController(
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
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<PaymentMethod>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "PaymentMethod");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _ReadById(id: id);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.response);
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(PaymentMethod obj)
        {
            // => Create PaymentMethod
            var result = await _generic.CreateAsync<PaymentMethod, PaymentMethodRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<PaymentMethodRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(PaymentMethod obj)
        {
            // => Update PaymentMethod
            var result = await _generic.UpdateAsync<PaymentMethod, PaymentMethodRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<PaymentMethodRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Delete PaymentMethod
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<PaymentMethod, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Redis
            var key = _redis.GenKey<PaymentMethodRedis>(new PaymentMethodRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, PaymentMethod response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<PaymentMethod>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }
        #endregion

        #region Sync
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<PaymentMethodRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<PaymentMethodRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<PaymentMethodRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<PaymentMethodRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<PaymentMethodRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }
        #endregion
    }
}
