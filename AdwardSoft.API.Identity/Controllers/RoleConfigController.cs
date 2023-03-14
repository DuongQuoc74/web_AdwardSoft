using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Identity;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Identity.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RoleConfigController : ControllerBase
    {
        #region Constructor
        private IGenericRepository _generic;

        public RoleConfigController(IGenericRepository generic)
        {
            _generic = generic;
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(RoleConfig obj)
        {
            var result = await _generic.CreateAsync<RoleConfig, int>(obj);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Read
        [HttpGet("ReadByDefaultType")]
        public async Task<IActionResult> ReadByDefaultType(int userType)
        {
            var param = DataHelper.GenParams("UserType", userType);
            var result = await _generic.ReadByCustomAsync<RoleConfig>(param, "ByDefaultType");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByUserType")]
        public async Task<IActionResult> ReadByUserType(int userType)
        {
            var param = DataHelper.GenParams("UserType", userType);
            var result = await _generic.ReadCustomAsync<RoleConfigDetail>(param, "ByUserType");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<RoleConfig>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update(RoleConfig obj)
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
            var result = await _generic.DeteteAsync<RoleConfig, int>(parms);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion
    }
}