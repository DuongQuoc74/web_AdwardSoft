using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdwardSoft.API.POS.Helper;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembershipScoreController : ControllerBase
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;

        public MembershipScoreController(
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
            var result = await _generic.ReadAsync<MembershipScore>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int customerId)
        {
            var key = _redis.GenKey<MembershipScore>();
            decimal totalPoint = 0;
            var redisData = await _redis.SearchKeysAsync<MembershipScore>(key+ ":" + customerId.ToString()+ ":"+ DateTime.Now.Year.ToString() + ":*");
            if (redisData == null || redisData.Count() < 1)
            {
                var result = await _ReadById(customerId);

                if (!result.success)
                    return BadRequest(result.message);
                totalPoint = result.response;
            }
            else
            {
                totalPoint = (redisData.Where(x => x.Type == 1).Select(x => x.Point).Sum() - redisData.Where(x => x.Type == 0).Select(x => x.Point).Sum());
            }

            return Ok(totalPoint);
        }
        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(MembershipScore obj)
        {
            var result = await _generic.CreateAsync<MembershipScore, MembershipScore>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<MembershipScore>(result.Response);

            return Ok(result.Response);
        }

        #endregion

        #region Methods
        private async Task<(bool success, string message, int count)> _SyncRedis()
        {
            // => Delete Membership Class in redis
            var keyToDelete = _redis.GenKey<MembershipScore>();
            var redisToDelete = await _redis.SearchKeysAsync<MembershipScore>(keyToDelete + ":*");

            await _redis.RemoveAsync<MembershipScore>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<MembershipScore>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);


            await _redis.SetAsync<MembershipScore>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<(bool success, string message, int response)> _ReadById(int id)
        {
            var solarTo = DateTime.Now.Date;
            var solarFrom = new DateTime(solarTo.Year, 1, 1);
            var lunar = LunarHelper.ConvertSolar2Lunar(solarTo.Day, solarTo.Month, solarTo.Year, 7.0);

            //Set solar from date by by lunar from date
            var solarData = LunarHelper.ConvertLunar2Solar(1, 1, lunar[2], 0, 7.0);
            solarFrom = new DateTime(solarData[2], solarData[1], solarData[0]);

            var param = DataHelper.GenParams("Id", id, "DateFrom", solarFrom , "DateTo", solarTo);
            var result = await _generic.ReadByCustomAsync<MembershipScore, int>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }

        #endregion

        #region Sync

        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _SyncRedis();

            if(!result.success)
                return BadRequest(result.message);

            return Ok(result.count);
        }

        #endregion
    }
}
