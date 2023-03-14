using System.Collections.Generic;
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
    public class UserRoleController : Controller
    {
        #region Constructor
        private IGenericRepository _generic;

        public UserRoleController(IGenericRepository generic)
        {
            _generic = generic;
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateMultiRole([FromBody] List<UserRole> obj)
        {
            var result = await _generic.CreateMultipleAsync(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Read
        [HttpGet("ReadRoles")]
        public async Task<IActionResult> ReadRoles()
        {
            var result = await _generic.ReadAsync<UserRoles>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(long id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadCustomAsync<UserRole>(parms, null);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Update
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<UserRole, int>(parms);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion
    }
}