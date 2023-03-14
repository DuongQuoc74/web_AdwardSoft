using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.API.POS.Helper;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace AdwardSoft.API.POS.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : Controller
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;


        public ReportController(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        #endregion

        [HttpGet("ReadStock")]
        public async Task<IActionResult> ReadStock(int pageSize, int pageSkip, string searchBy
            ,DateTime dateFrom, DateTime dateTo, int branchId)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "DateFrom", dateFrom, "DateTo", dateTo, "BranchId ", branchId);

            //usp_ReportStock_Read
            var result = await _genericRepository.ReadCustomAsync<ReportStock>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadReceiving")]
        public async Task<IActionResult> ReadReceiving(int pageSize, int pageSkip, string searchBy
            , DateTime dateFrom, DateTime dateTo, int branchId, int supplierId = 0)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "DateFrom", dateFrom, "DateTo", dateTo, "SupplierId ", supplierId, "BranchId ", branchId);

            //usp_ReportReceiving_Read
            var result = await _genericRepository.ReadCustomAsync<ReportReceiving>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSellingPrice")]
        public async Task<IActionResult> ReadSellingPrice(int pageSize, int pageSkip, string searchBy
            , DateTime dateFrom, DateTime dateTo, string receivingReportId, int branchId, int supplierId = 0)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "DateFrom", dateFrom, "DateTo", dateTo, "ReceivingReportId", receivingReportId
                , "SupplierId ", supplierId, "BranchId ", branchId);

            //usp_ReportSellingPrice_Read
            var result = await _genericRepository.ReadCustomAsync<ReportSellingPrice>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadServiceDisplay")]
        public async Task<IActionResult> ReadServiceDisplay(int pageSize, int pageSkip, string searchBy
            , DateTime dateFrom, DateTime dateTo, int supplierId = 0)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "DateFrom", dateFrom, "DateTo", dateTo, "SupplierId ", supplierId);

            //usp_ReportReceiving_Read
            var result = await _genericRepository.ReadCustomAsync<ReportServiceDisplay>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadCustomerReport")]
        public async Task<IActionResult> ReadCustomerReport(int pageSize, int pageSkip, string searchBy)
        {
            var solarTo = DateTime.Now.Date;
            var solarFrom = new DateTime(solarTo.Year, 1, 1);
            var lunar = LunarHelper.ConvertSolar2Lunar(solarTo.Day, solarTo.Month, solarTo.Year, 7.0);

            //Set solar from date by by lunar from date
            var solarData = LunarHelper.ConvertLunar2Solar(1, 1, lunar[2], 0, 7.0);
            solarFrom = new DateTime(solarData[2], solarData[1], solarData[0]);

            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "DateFrom ", solarFrom, "DateTo", solarTo);

            var result = await _genericRepository.ReadCustomAsync<CustomerReport>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
    }
}
