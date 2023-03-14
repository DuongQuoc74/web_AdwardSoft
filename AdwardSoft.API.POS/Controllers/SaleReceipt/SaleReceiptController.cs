using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleReceiptController : Controller
    {
        #region Contractor
        private readonly IGenericRepository _generic;
        public SaleReceiptController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }
        #endregion

        #region Web
        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSize, int skip, string searchBy,
            int shiftIdId, int checkoutCounterId, DateTime dateFrom, DateTime dateTo)
        {
            var param = DataHelper.GenParams("CheckoutCounterId", checkoutCounterId, "ShiftIdId", shiftIdId
                , "DateFrom", dateFrom, "DateTo", dateTo
                , "pageSize", pageSize, "skip", skip, "searchBy", searchBy);
            var result = await _generic.ReadCustomAsync<SaleReceiptDataTable>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpGet("History/Read")]
        public async Task<IActionResult> HistoryRead(string searchBy,
            int shiftId, int checkoutCounterId, DateTime dateFrom, DateTime dateTo)
        {
            var param = DataHelper.GenParams("CheckoutCounterId", checkoutCounterId, "ShiftId", shiftId
                , "DateFrom", dateFrom, "DateTo", dateTo
                ,"searchBy", searchBy);
            var result = await _generic.ReadCustomAsync<SaleReceiptHistory>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(string id)
        {
            var result = await _generic.ReadByIdAsync<SaleReceipt>(id);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("Display/Master")]
        public async Task<IActionResult> ReadMaster(string id)
        {
            var result = await _generic.ReadByIdAsync<SaleReceiptDisplay>(id);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SaleReceipt obj)
        {
            var result = await _generic.CreateAsync<SaleReceipt, string>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(SaleReceipt obj)
        {
            //Delete
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>() { { "Id", obj.Id } };
            await _generic.DeteteAsync<SaleReceipt, int>(param);
            //Create
            var result = await _generic.CreateAsync<SaleReceipt, string>(obj);
            //var result = await _generic.UpdateAsync<SaleReceipt, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _generic.DeteteAsync<SaleReceipt, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Mobile
        [HttpGet]
        [Route("~/api/Mobile/[controller]/Read")]
        public async Task<IActionResult> ReadMobile(int pageSize, int pageSkip, string searchBy,
            int userId, DateTime dateFrom, DateTime dateTo)
        {
            var skip = pageSkip * pageSize;
            var param = DataHelper.GenParams("UserId", userId
                , "DateFrom", dateFrom, "DateTo", dateTo
                , "pageSize", pageSize, "skip", skip, "searchBy", searchBy);

            var result = await _generic.ReadCustomAsync<SaleReceiptDataTable>(param, "Mobile");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion
    }
}
