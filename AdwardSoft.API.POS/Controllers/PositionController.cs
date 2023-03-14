using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PositionController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public PositionController(
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
            var result = await _generic.ReadAsync<Position>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "Position");

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
        public async Task<IActionResult> CreateAsync(Position obj)
        {
            // => Create Position
            var result = await _generic.CreateAsync<Position, PositionRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<PositionRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(Position obj)
        {
            // => Update Position
            var result = await _generic.UpdateAsync<Position, PositionRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<PositionRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Delete Position
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Position, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Redis
            var key = _redis.GenKey<PositionRedis>(new PositionRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, Position response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Position>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }
        #endregion

        #region Sync
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<PositionRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<PositionRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<PositionRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<PositionRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<PositionRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }
        #endregion
    }
}
