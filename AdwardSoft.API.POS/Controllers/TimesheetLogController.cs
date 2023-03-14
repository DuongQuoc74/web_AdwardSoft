using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.API.POS.Helper;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class TimesheetLogController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public TimesheetLogController(
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
        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(long userId)
        {
            var param = DataHelper.GenParams("UserId", userId);
            var result = await _generic.ReadCustomAsync<TimesheetLog>(param, "ByUserId");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody]TimesheetLog obj)
        {
            // => Create TimesheetLog
            if (obj.MapCoordinateLat < -90 || obj.MapCoordinateLat > 90 || obj.MapCoordinateLong < -180 || obj.MapCoordinateLong > 180)
                return BadRequest("Longitude and Latitude is invalid");

            var param = DataHelper.GenParams("Id", 0);
            var resultSetting = await _generic.ReadByCustomAsync<Setting>(param, "ById");
            if (!resultSetting.Success || resultSetting.Response.Id == 0)
                return BadRequest("Not find location in system to compare");

            var setting = resultSetting.Response;

            var distanceIsVAlid = LocationHepler.DistanceIsValid(lat1: decimal.ToDouble(obj.MapCoordinateLat), lon1: decimal.ToDouble(obj.MapCoordinateLong)
                , lat2: decimal.ToDouble(setting.MapCoordinateLat), lon2: decimal.ToDouble(setting.MapCoordinateLong)
                , distance: decimal.ToDouble(setting.ErrorRange));

            if (!distanceIsVAlid)
                return BadRequest("Invalid gap");

            var result = await _generic.CreateAsync<TimesheetLog, bool>(obj);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Function

        #endregion
    }
}
