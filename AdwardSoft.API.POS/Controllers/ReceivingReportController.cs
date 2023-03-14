using System;
using System.Linq;
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
    public class ReceivingReportController : Controller
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;

        public ReceivingReportController(
            IRedisRepositoty redis,
            IGenericRepository generic)
        {
            _redis = redis;
            _generic = generic;
        }

        #endregion

        #region Read
        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSize, int pageSkip, string searchBy
        , DateTime dateFrom, DateTime dateTo, int branchId, long createUser = 0, int supplierId = 0)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "DateFrom", dateFrom, "DateTo", dateTo, "CreateUser", createUser, "SupplierId ", supplierId, "BranchId ", branchId);

            //usp_ReceivingReportView_Read
            var result = await _generic.ReadCustomAsync<ReceivingReportView>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(string Id)
        {


            //usp_ReceivingReport_ReadById
            var result = await _generic.ReadByCustomAsync<ReceivingReport>(DataHelper.GenParams("Id", Id), "ById");

            if (!result.Success || result.Response == null || String.IsNullOrEmpty(result.Response.Id))
                return BadRequest(result.Messages);
            //usp_ReceivingReportDetail_ReadByReceivingReportId
            var resultDetail = await _generic.ReadCustomAsync<ReceivingReportDetail>(DataHelper.GenParams("ReceivingReportId", Id), "ByReceivingReportId");
            if (!resultDetail.Success)
                return BadRequest(resultDetail.Messages);

            foreach (var item in resultDetail.Response)
            {
                var param = DataHelper.GenParams("ProductId", item.ProductId);
                var resultUnit = await _generic.ReadCustomAsync<Select>(param, "UnitByProduct");
                item.Units = resultUnit.Response.ToList();
            }

            var receivingReportTmp = new ReceivingReportTmp();
            receivingReportTmp.ReceivingReport = result.Response;
            receivingReportTmp.ReceivingReportDetail = resultDetail.Response.ToList();

            return Ok(receivingReportTmp);
        }
        #endregion

        #region Create
        [HttpPost("CreateMobile")]
        public async Task<IActionResult> CreateMobile([FromBody] ReceivingReportTmp obj)
        {
            //usp_ //usp_ReceivingReport_Create
            var result = await _generic.CreateAsync<ReceivingReport, string>(obj.ReceivingReport);

            if (!result.Success || String.IsNullOrEmpty(result.Response))
                return BadRequest(result.Messages);
         

            //Converter
            obj.ReceivingReportDetail = obj.ReceivingReportDetail.Select(c => { c.ReceivingReportId = result.Response; return c; }).ToList();
            //usp_ReceivingReportDetail_Create
            var resultConverter = await _generic.CreateMultipleAsync(obj.ReceivingReportDetail);

            if (!resultConverter.Success)
                return BadRequest(resultConverter.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update(ReceivingReportTmp obj)
        {
            //usp_ReceivingReport_Update
            var result = await _generic.UpdateAsync(obj.ReceivingReport);

            if (!result.Success)
                return BadRequest(result.Messages);


            //Converter  

            //usp_ReceivingReportDetail_Delete
            await _generic.DeteteAsync<ReceivingReportDetail, int>(DataHelper.GenParams("ReceivingReportId", obj.ReceivingReport.Id));

            //usp_ReceivingReportDetail_Create
            var resultConverter = await _generic.CreateMultipleAsync(obj.ReceivingReportDetail);

            if (!resultConverter.Success)
                return BadRequest(resultConverter.Messages);

            return Ok(result.Response);
        }    
        
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(ReceivingReportStatus obj)
        {
            //usp_ReceivingReportStatus_Update
            var result = await _generic.UpdateAsync<ReceivingReportStatus,bool>(obj);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            //usp_ReceivingReport_Delete
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<ReceivingReport, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion
    }
}
