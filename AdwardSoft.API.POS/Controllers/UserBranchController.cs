using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBranchController : Controller
    {
        private readonly IGenericRepository _generic;

        public UserBranchController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }

        [HttpGet("ReadByUserId")]
        public async Task<IActionResult> ReadByUserId(int id)
        {
            var param = DataHelper.GenParams("UserId", id);
            var result = await _generic.ReadCustomAsync<UserBranch>(param, "ByUserId");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<UserBranch> obj)
        {
            var param = DataHelper.GenParams("UserId", obj.FirstOrDefault().UserId);
            var deleteRS = await _generic.DeteteAsync<UserBranch, int>(param);
            var result = await _generic.CreateMultipleAsync(obj);
            if (result.Success)
            {
                return Ok(result.Response);
            }
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var param = DataHelper.GenParams("UserId", id);
            var result = await _generic.DeteteAsync<UserBranch, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
