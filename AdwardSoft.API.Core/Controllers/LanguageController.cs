using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LanguageController : Controller
    {
        private IGenericRepository _generic;
        public LanguageController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Language>();

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(string id)
        {

            var result = await _generic.ReadByIdAsync<Language>(id);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Language obj)
        {
            var result = await _generic.CreateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Language obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Code", id } };

            var result = await _generic.DeteteAsync<Language, int>(parms);

            //if (result.Success) return Ok(result.Response);
            //else return BadRequest(result.Messages);
            return Ok(result.Response);
        }

        [HttpGet("ReadDefault")]
        public async Task<IActionResult> ReadDefault()
        {
            var result = await _generic.ReadByCustomAsync<Language>(null, "ByDefault");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}