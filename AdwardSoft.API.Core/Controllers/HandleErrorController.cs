using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class HandleErrorController : Controller
    {
        private IGenericRepository _generic;
        public HandleErrorController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<HandleError>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id, string languageCode)
        {
            var parms = DataHelper.GenParams("Id", id, "LanguageCode", languageCode);
            var result = await _generic.ReadByCustomAsync<HandleError>(parms, "ById");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(HandleError obj)
        {
            var result = await _generic.CreateAsync(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(HandleError obj)
        {
            var result = await _generic.UpdateAsync(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<HandleError, int>(parms);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByCode")]
        public async Task<IActionResult> ReadByUser(string code, string languageCode)
        {
            var parms = DataHelper.GenParams("Code", code, "LanguageCode", languageCode);
            var result = await _generic.ReadByCustomAsync<HandleError>(parms, "ByCode");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);

        }
    }
}