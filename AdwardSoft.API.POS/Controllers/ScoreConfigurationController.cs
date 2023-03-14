using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreConfigurationController : ControllerBase
    {
        #region constructer
        private readonly IGenericRepository _generic;
        private readonly IRedisRepositoty _redis;
        public ScoreConfigurationController(IGenericRepository genericRepository, IRedisRepositoty redis)
        {
            _generic = genericRepository;
            _redis = redis;
        }
        #endregion

        #region read
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<ScoreConfiguration>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
      
        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int pageSize, int pageSkip, string searchBy,int status)
        {
            var param = DataHelper.GenParams("searchBy", searchBy, "pageSize", pageSize, "pageSkip", pageSkip,"status",status);
            var result = await _generic.ReadCustomAsync<ScoreConfigurationDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(result.Response);
        }
        #endregion

        #region create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ScoreConfiguration obj)
        {
            var result = await _generic.CreateAsync<ScoreConfiguration, ScoreConfigurationRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            await _redis.SetAsync<ScoreConfigurationRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region ReadById
        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<ScoreConfiguration>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region update
        [HttpPut("Update")]
        public async Task<IActionResult> Update(ScoreConfiguration obj)
        {
            var result = await _generic.UpdateAsync<ScoreConfiguration, ScoreConfigurationRedis>(obj);
            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            await _redis.SetAsync<ScoreConfigurationRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _generic.DeteteAsync<ScoreConfiguration, int>(parms);
            if (!result.Success && result.Response <= 0)
                 return BadRequest(result.Messages);
                //Delete supplier redis
            var key = _redis.GenKey<ScoreConfigurationRedis>(new ScoreConfigurationRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);

        }
        #endregion

        #region checkDate

        [HttpGet("CheckDateIsExisting")]
        public async Task<IActionResult> CheckDateIsExisting(int id, string date)
        {
            var isExisting = await _CheckDateIsExisting(id: id, date: date);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        private async Task<(EResponse code, string message)> _CheckDateIsExisting(int id, string date)
        {
            // => Convert str to date
            string[] str = date.Split("/");
            var Edate = DateTime.Parse(str[2] + "-" + str[1] + "-" + str[0]);

            var param = DataHelper.GenParams("Id", id, "EffectiveDate", Edate);


            var result = await _generic.ReadByCustomAsync<ScoreConfiguration, int>(param, "DateIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        #endregion
    }
}
