using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehicleTypeController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public VehicleTypeController(
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
            var result = await _generic.ReadAsync<VehicleType>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "VehicleType");

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
        public async Task<IActionResult> CreateAsync(VehicleType obj)
        {
            // => Create VehicleType
            var result = await _generic.CreateAsync<VehicleType, VehicleTypeRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<VehicleTypeRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(VehicleType obj)
        {
            // => Update Location
            var result = await _generic.UpdateAsync<VehicleType, VehicleTypeRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<VehicleTypeRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Delete DeliveryPoint
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<VehicleType, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove DeliveryPoint
            var key = _redis.GenKey<VehicleTypeRedis>(new VehicleTypeRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, VehicleType response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<VehicleType>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }
      
        #endregion

        #region Sync
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<VehicleTypeRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<VehicleTypeRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<VehicleTypeRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<VehicleTypeRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<VehicleTypeRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }
        #endregion
    }
}
