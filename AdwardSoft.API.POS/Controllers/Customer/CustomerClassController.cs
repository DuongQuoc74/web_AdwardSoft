using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerClassController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;

        public CustomerClassController(
            IRedisRepositoty redis,
            IGenericRepository generic)
        {
            _redis = redis;
            _generic = generic;
        }

        #endregion

        #region Read

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int pageSize, int pageSkip, string searchBy, DateTime dateFrom, DateTime dateTo)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy, "DateFrom", dateFrom, "DateTo", dateTo);

            var result = await _generic.ReadByCustomAsync<CustomerClassDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomerClass obj)
        {
            var result = await _generic.CreateAsync<CustomerClass, CustomerClassRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<CustomerClassRedis>(result.Response);

            return Ok(result.Response.CustomerId);
        }

        #endregion

        #region Methods

        private async Task<(bool success, string message, int count)> _SyncRedis()
        {
            // => Delete Customer in redis
            var keyToDelete = _redis.GenKey<CustomerClassRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<CustomerClassRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<CustomerClassRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<CustomerClassRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            await _redis.SetAsync<CustomerClassRedis>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        #endregion

        #region Sync

        // Sync Redis
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _SyncRedis();

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.count);
        }

        #endregion
    }
}
