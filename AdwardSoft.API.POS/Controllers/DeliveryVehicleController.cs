using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Repositories.Pattern;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryVehicleController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public DeliveryVehicleController(
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

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> Read(string licensePlate, int customerId, int vehicleTypeId)
        {
            var param = DataHelper.GenParams("LicensePlate", licensePlate, "CustomerId", customerId, "VehicleTypeId", vehicleTypeId);

            var result = await _generic.ReadCustomAsync<DeliveryVehicleDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect(int customerId)
        {
            var param = DataHelper.GenParams("CustomerId", customerId);

            var result = await _generic.ReadCustomAsync<Select>(param, "DeliveryVehicle");

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

        [HttpGet("ReadByLicensePlate")]
        public async Task<IActionResult> ReadByLicensePlate(int id, string licensePlate)
        {
            var param = DataHelper.GenParams("Id", id, "LicensePlate", licensePlate);

            var result = await _generic.ReadByCustomAsync<DeliveryVehicle>(param, "ByLicensePlate");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(DeliveryVehicle obj)
        {
            // => Create DeliveryVehicle
            var result = await _generic.CreateAsync<DeliveryVehicle, DeliveryVehicleRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<DeliveryVehicleRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(DeliveryVehicle obj)
        {
            // => Update Location
            var result = await _generic.UpdateAsync<DeliveryVehicle, DeliveryVehicleRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<DeliveryVehicleRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Delete DeliveryPoint
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<DeliveryVehicle, int>(param);

            if (!result.Success || result.Response < 0)
                return BadRequest(result.Messages);

            // => Remove DeliveryPoint
            var key = _redis.GenKey<DeliveryVehicleRedis>(new DeliveryVehicleRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, DeliveryVehicle response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<DeliveryVehicle>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }
      
        #endregion

        #region Sync
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<DeliveryVehicleRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<DeliveryVehicleRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<DeliveryVehicleRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<DeliveryVehicleRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<DeliveryVehicleRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }
        #endregion
    }
}
