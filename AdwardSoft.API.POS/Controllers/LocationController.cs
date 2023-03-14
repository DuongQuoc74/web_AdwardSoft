using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Repositories.Pattern;
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
    public class LocationController : Controller
    {   
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public LocationController(
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
            var result = await _generic.ReadAsync<LocationDatatable>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "Location");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        [HttpGet("ReadChildSelect")]
        public async Task<IActionResult> ReadChildSelect(int parentId)
        {
            var param = DataHelper.GenParams("ParentId", parentId);
            var result = await _generic.ReadCustomAsync<Select>(param, "ChildLocation");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int parentId)
        {
            var param = DataHelper.GenParams("ParentId", parentId);
            var result = await _generic.ReadCustomAsync<LocationDatatable>(param, "ByParentId");

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
        public async Task<IActionResult> CreateAsync(Location obj)
        {
            // => Create Location
            var result = await _generic.CreateAsync<Location, LocationRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<LocationRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(Location obj)
        {
            // => Update Location
            var result = await _generic.UpdateAsync<Location, LocationRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<LocationRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Check Location is using by DeliveryPoint
            var isUsing = await _CheckIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
                return BadRequest("Location is using, not allow remove !");

            // => Delete sql
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Location, int>(param);

            if (!result.Success && result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Redis
            var key = _redis.GenKey<LocationRedis>(new LocationRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, Location response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Location>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }
        private async Task<(EResponse code, string message)> _CheckIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Location, int>(param, "CheckIsUsing");

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
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<LocationRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<LocationRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<LocationRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<LocationRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<LocationRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }
        #endregion
    }
}
