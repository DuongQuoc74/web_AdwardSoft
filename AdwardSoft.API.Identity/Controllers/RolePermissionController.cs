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
    public class RolePermissionController : Controller
    {
        #region Constructor
        private IGenericRepository _generic;

        public RolePermissionController(IGenericRepository generic)
        {
            _generic = generic;
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<RolePermission> obj)
        {
            var result = await _generic.CreateMultipleAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Read
        [HttpGet("ReadById")]
        public async Task<IActionResult> Read(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadCustomAsync<RolePermission>(parms, null);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Update
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<RolePermission, int>(parms);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion
    }
}