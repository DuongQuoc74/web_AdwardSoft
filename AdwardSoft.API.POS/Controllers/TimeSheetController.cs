using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TimeSheetController : Controller
    {
        private readonly IGenericRepository _generic;
        public TimeSheetController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(long userId, DateTime date)
        {
            var param = DataHelper.GenParams("UserId", userId, "Month", date.Month, "Year", date.Year);
            var result = await _generic.ReadByCustomAsync<Timesheet>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(Timesheet obj)
        {
            var result = await _generic.CreateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long userId, DateTime date)
        {
            var param = DataHelper.GenParams("UserId", userId, "Date", date);
            var result = await _generic.DeteteAsync<Timesheet, short>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
