using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeShiftController : Controller
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public EmployeeShiftController(
            IRedisRepositoty redis,
            IGenericRepository generic,
            IAdapterPattern adapter,
            IElasticClient elasticClient,
            IMapper mapper)
        {
            _redis = redis;
            _generic = generic;
            _adapter = adapter;
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        #endregion

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSize, int pageSkip, string searchBy, int employeeId, int branchId, int shiftId, DateTime date)
        {
            var param = DataHelper.GenParams("searchBy", searchBy, "pageSize", pageSize, "PageSkip", pageSkip
                , "EmployeeId", employeeId, "BranchId", branchId, "ShiftId", shiftId, "Date", date);
            var result = await _generic.ReadCustomAsync<EmployeeShiftDataTable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int employeeId, int shiftId, int year, int month)
        {
            var param = DataHelper.GenParams("EmployeeId", employeeId, "ShiftId", shiftId, "Year", year, "Month", month);
            var result = await _generic.ReadByCustomAsync<EmployeeShift>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadByUserId")]
        public async Task<IActionResult> ReadByUserId(int employeeId, int branchId, DateTime date)
        {
            //usp_EmployeeShift_ReadByUserId
            var param = DataHelper.GenParams("EmployeeId", employeeId, "BranchId", branchId, "Date", date);
            var result = await _generic.ReadCustomAsync<EmployeeShiftDataTable>(param, "ByUserId");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(EmployeeShift obj)
        {
            var result = await _generic.CreateAsync(obj);

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(result.Response);
        }
        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int employeeId, int shiftId, int year, int month)
        {
            var param = DataHelper.GenParams("EmployeeId", employeeId, "ShiftId", shiftId, "Year", year, "Month", month);
            var result = await _generic.DeteteAsync<EmployeeShift, int>(param);

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(result.Response);
        }

        #endregion

        [HttpGet("CheckExist")]
        public async Task<IActionResult> CheckExist(int employeeId, int shiftId, int year, int month)
        {
            //usp_EmployeeShift_ReadIsExist
            var param = DataHelper.GenParams("EmployeeId", employeeId, "ShiftId", shiftId, "Year", year, "Month", month);
            var result = await _generic.ReadByCustomAsync<EmployeeShift, int>(param, "IsExist");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
