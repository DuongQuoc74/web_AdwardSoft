using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeUserController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public EmployeeUserController(
            IRedisRepositoty redis,
            IGenericRepository generic,
            IAdapterPattern adapter)
        {
            _redis = redis;
            _generic = generic;
            _adapter = adapter;
        }
        #endregion

        #region Read
        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int employeeId)
        {
            var param = DataHelper.GenParams("EmployeeId", employeeId);
            var result = await _generic.ReadByCustomAsync<EmployeeUser>(param, "ById");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }


        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(EmployeeUser obj)
        {
            // => Create EmployeeSalary
            if (obj.UserId == 0)
            {
                var param = DataHelper.GenParams("EmployeeId", obj.EmployeeId);
                var result = await _generic.DeteteAsync<EmployeeUser, int>(param);

                if (!result.Success || result.Response <= 0)
                    return BadRequest(result.Messages);

                return Ok(result.Response);
            }
            else
            {
                var result = await _generic.CreateAsync<EmployeeUser, int>(obj);

                if (!result.Success)
                    return BadRequest(result.Messages);

                return Ok(result.Response);
            }

        }
        #endregion

        #region CheckIsExists
        [HttpGet("CheckExist")]
        public async Task<IActionResult> CheckExist(int employeeId, long userId)
        {
            //usp_EmployeeUser_ReadIsExist
            var param = DataHelper.GenParams("EmployeeId", employeeId, "UserId", userId);
            var result = await _generic.ReadByCustomAsync<EmployeeUser, int>(param, "IsExist");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int employeeId)
        {
            // => Delete EmployeeSalary
            var param = DataHelper.GenParams("EmployeeId", employeeId);
            var result = await _generic.DeteteAsync<EmployeeSalary, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion
    }
}
