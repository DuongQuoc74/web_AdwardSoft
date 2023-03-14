using System;
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
    public class EmployeeSalaryController : Controller
    {
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public EmployeeSalaryController(
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
        [HttpGet("Read")]
        public async Task<IActionResult> Read(int employeeId)
        {
            var param = DataHelper.GenParams("EmployeeId", employeeId);
            var result = await _generic.ReadCustomAsync<EmployeeSalary>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int employeeId, DateTime effectiveDate)
        {
            var result = await _ReadById(employeeId: employeeId, effectiveDate: effectiveDate);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.response);
        }

        [HttpGet("ReadTop")]
        public async Task<IActionResult> ReadTop(int employeeId)
        {
            var param = DataHelper.GenParams("EmployeeId", employeeId);
            var result = await _generic.ReadByCustomAsync<EmployeeSalary>(param, "Top");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(EmployeeSalary obj)
        {
            // => Create EmployeeSalary
            var result = await _generic.CreateAsync<EmployeeSalary, int>(obj);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(EmployeeSalary obj)
        {
            // => Update EmployeeSalary
            var result = await _generic.UpdateAsync<EmployeeSalary, int>(obj);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int employeeId, DateTime effectiveDate)
        {
            // => Delete EmployeeSalary
            var param = DataHelper.GenParams("EmployeeId", employeeId, "EffectiveDate", effectiveDate);
            var result = await _generic.DeteteAsync<EmployeeSalary, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, EmployeeSalary response)> _ReadById(int employeeId, DateTime effectiveDate)
        {
            var param = DataHelper.GenParams("EmployeeId", employeeId, "EffectiveDate", effectiveDate);
            var result = await _generic.ReadByCustomAsync<EmployeeSalary>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }
        #endregion
    }
}
