using System.Collections.Generic;
using System.Linq;
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
    public class RoleController : Controller
    {
        #region Constructor
        private IGenericRepository _generic;

        private readonly IRedisRepositoty _redisRepositoty;
        public RoleController(IGenericRepository generic, IRedisRepositoty redisRepositoty)
        {
            _generic = generic;
            _redisRepositoty = redisRepositoty;
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Role obj)
        {
            var result = await _generic.CreateAsync<Role, int>(obj);
            if (result.Success)
            {
                if (result.Response > 0)
                {
                    obj.Id = result.Response;
                    await _redisRepositoty.SetAsync<Role>(obj);
                }
                return Ok(result.Response);
            }
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Read
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Role>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [AllowAnonymous]
        [HttpGet("ReadByUserId")]
        public async Task<IActionResult> ReadByUser(long id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadCustomAsync<Role>(parms, "ByUserId");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByUserType")]
        public async Task<IActionResult> ReadByUserType(int userType)
        {
            var param = DataHelper.GenParams("UserType", userType);
            var result = await _generic.ReadCustomAsync<Role>(param, "ByUserType");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<Role>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update(Role obj)
        {
            var result = await _generic.UpdateAsync(obj);
            if (result.Success)
            {
                await _redisRepositoty.SetAsync<Role>(obj);
                return Ok(result.Response);
            }
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Role, int>(parms);
            if (result.Success)
            {
                var roleRedis = await _redisRepositoty.SearchKeysAsync<Role>("ads:role:id:" + id);
                await _redisRepositoty.RemoveAsync<Role>(roleRedis.ToList());
                return Ok(result.Response);
            }
            else return BadRequest(result.Messages);
        }
        #endregion
    }
}