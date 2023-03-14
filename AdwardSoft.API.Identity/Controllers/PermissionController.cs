using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Identity;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Identity.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PermissionController : Controller
    {
        #region Constructor
        private IGenericRepository _generic;

        public PermissionController(IGenericRepository generic)
        {
            _generic = generic;
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Permission obj)
        {
            var result = await _generic.CreateAsync<Permission>(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Read
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Permission>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {

            var result = await _generic.ReadByIdAsync<Permission>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update(Permission obj)
        {
            var result = await _generic.UpdateAsync(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Role, int>(parms);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Check
        [HttpGet("CheckExsit")]
        public async Task<IActionResult> CheckExsit(string controller, string action, int id)
        {
            var param = DataHelper.GenParams("Id", id, "Controller", controller, "action", action);
            var result = await _generic.ReadByCustomAsync<Permission>(param, "CheckExist");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("CheckUse")]
        public async Task<IActionResult> CheckUse(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Permission>(param, "CheckUse");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion
    }
}