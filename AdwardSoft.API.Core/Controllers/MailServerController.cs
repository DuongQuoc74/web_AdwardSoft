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
    public class MailServerController : ControllerBase
    {
        private IGenericRepository _generic;
        private readonly IRedisRepositoty _redisRepositoty;

        public MailServerController(IGenericRepository generic, IRedisRepositoty redisRepositoty)
        {
            _generic = generic;
            _redisRepositoty = redisRepositoty;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<MailServer>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<MailServer>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByAction")]
        public async Task<IActionResult> ReadByAction(int action, short type)
        {
            var param = DataHelper.GenParams("Action", action, "Type", type);
            var result = await _generic.ReadByCustomAsync<MailServerDetail>(param,"ByAction");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MailServer obj)
        {
            var result = await _generic.CreateAsync<MailServer, int>(obj);
            if (result.Success && result.Response > 0) {
                obj.Id = result.Response;
                await _redisRepositoty.SetAsync<MailServer>(obj);
                return Ok(result.Response);
            } 
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(MailServer obj)
        {
            var result = await _generic.UpdateAsync<MailServer, int>(obj);
            if (result.Success)
            {
                await _redisRepositoty.SetAsync<MailServer>(obj);
                return Ok(result.Response);
            }
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<MailServer, int>(parms);
            if (result.Success && result.Response > 0) {
                await _redisRepositoty.RemoveAsync<MailServer>("id", id);
                return Ok(result.Response);
            } 
            else return BadRequest(result.Messages);
        }

        [HttpGet("IsExistEmail")]
        public async Task<IActionResult> IsExistEmail(string email, int id)
        {
            var param = DataHelper.GenParams("Email", email, "Id", id);
            var result = await _generic.ReadByCustomAsync<MailServer, int>(param, "EmailIsExist");
            if (!result.Success) return BadRequest(result.Messages);
            return Ok(result.Response);
        }
    }
}