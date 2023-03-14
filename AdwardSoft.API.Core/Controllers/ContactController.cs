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
    public class ContactController : Controller
    {
        private readonly IGenericRepository _genericRepository;

        public ContactController(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int status)
        {
            var param = DataHelper.GenParams("Status", status);
            var result = await _genericRepository.ReadCustomAsync<Contact>(param, "");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _genericRepository.ReadByIdAsync<Contact>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Contact obj)
        {
            var result = await _genericRepository.CreateAsync<Contact, int>(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Contact obj)
        {
            var result = await _genericRepository.UpdateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _genericRepository.DeteteAsync<Contact, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}