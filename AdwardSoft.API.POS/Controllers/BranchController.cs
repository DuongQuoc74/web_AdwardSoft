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
    public class BranchController : Controller
    {
        private readonly IGenericRepository _generic;
        public BranchController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Branch>();

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<Branch>(id);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByUserId")]
        public async Task<IActionResult> ReadByUserId(int userId)
        {
            //usp_Branch_ReadByUserId
            var param = DataHelper.GenParams("UserId", userId);
            var result = await _generic.ReadCustomAsync<BranchStock>(param, "ByUserId");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(Branch obj)
        {
            var result = await _generic.CreateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Branch obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Branch, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Sort")]
        public async Task<IActionResult> Sort(BranchJson json)
        {
            var result = await _generic.UpdateAsync(json);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<SelectLevels>(null, "Branch");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [AllowAnonymous]
        [HttpGet("ReadSelectUser")]
        public async Task<IActionResult> ReadSelectUser(long userId)
        {
            //usp_SelectLevels_ReadBranchByUser
            var param = DataHelper.GenParams("UserId", userId);
            var result = await _generic.ReadCustomAsync<SelectLevels>(param, "BranchByUser");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

    }
}
