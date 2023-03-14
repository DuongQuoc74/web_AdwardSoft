using System;
using System.Collections.Generic;
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
    public class ShiftController : Controller
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public ShiftController(
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
        public async Task<IActionResult> ReadDatatable(int branchId)
        {
            var param = DataHelper.GenParams("BranchId", branchId);

            var result = await _generic.ReadCustomAsync<ShiftDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);

            var result = await _generic.ReadByCustomAsync<Shift>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadBySelect(int userId, int branchId)
        {
            //usp_Select_ReadShift
            var param = DataHelper.GenParams("UserId", userId);

            var result = await _generic.ReadCustomAsync<Select>(param, "Shift");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }


        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Shift obj)
        {
            var result = await _generic.CreateAsync<Shift, ShiftRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            //redis
            await _redis.SetAsync<ShiftRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Shift obj)
        {
            var result = await _generic.UpdateAsync<Shift, ShiftRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            //redis
            await _redis.SetAsync<ShiftRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Shift, int>(parms);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            var redisResponse = await _redis.SearchKeysAsync<ShiftRedis>(_redis.GenKey(new ShiftRedis { Id = id }));
            await _redis.RemoveAsync(redisResponse.ToList());

            return Ok(result.Response);
        }

        #endregion

        #region Methods

        private async Task<(bool success, string message, int count)> _SyncRedis<T>()
        {
            // => Delete Customer in redis
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

        // Sync Redis
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _SyncRedis<ShiftRedis>();

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.count);
        }


        #endregion
    }
}
