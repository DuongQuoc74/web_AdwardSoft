using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MenuGroupController : ControllerBase
    {
        private readonly IGenericRepository _generic;

        public MenuGroupController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSize, int skip, string searchBy)
        {
            var param = DataHelper.GenParams("pageSize", pageSize, "skip", skip, "searchBy", searchBy);

            var result = await _generic.ReadCustomAsync<MenuGroup>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(string id)
        {

            var result = await _generic.ReadByIdAsync<MenuGroup>(id);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MenuGroup obj)
        {
            var result = await _generic.CreateAsync(obj, "Count");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(MenuGroup obj)
        {
            var result = await _generic.UpdateAsync(obj, "Count");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<MenuGroup, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Sort")]
        public async Task<IActionResult> Sort(MenuGroupSort obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}