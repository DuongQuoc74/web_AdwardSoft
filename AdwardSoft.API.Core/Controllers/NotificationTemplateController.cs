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
    public class NotificationTemplateController : ControllerBase
    {
        private IGenericRepository _generic;
        public NotificationTemplateController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<NotificationTemplate>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByAction")]
        public async Task<IActionResult> ReadByAction(short action)
        {
            var param = DataHelper.GenParams("Action", action);
            var result = await _generic.ReadCustomAsync<NotificationTemplate>(param, "ByAction");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(string id)
        {
            var result = await _generic.ReadByIdAsync<NotificationTemplate>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(NotificationTemplate obj)
        {
            var result = await _generic.CreateAsync(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(NotificationTemplate obj)
        {
            var result = await _generic.UpdateAsync(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<NotificationTemplate, int>(param);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}