using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleReceiptDetailController : Controller
    {
        #region Contractor
        private readonly IGenericRepository _generic;
        private readonly IMapper _mapper;
        public SaleReceiptDetailController(IGenericRepository genericRepository, IMapper mapper)
        {
            _generic = genericRepository;
            _mapper = mapper;
        }
        #endregion

        #region Web
        [HttpGet("Read")]
        public async Task<IActionResult> Read(string saleReceiptId)
        {
            var param = DataHelper.GenParams("SaleReceiptId", saleReceiptId);
            var result = await _generic.ReadCustomAsync<SaleReceiptDetailDataTable>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpGet("History/Read")]
        public async Task<IActionResult> HistoryRead(string searchBy,
            int shiftId, int checkoutCounterId, DateTime dateFrom, DateTime dateTo)
        {
            var param = DataHelper.GenParams("CheckoutCounterId", checkoutCounterId, "ShiftId", shiftId
                , "DateFrom", dateFrom, "DateTo", dateTo
                , "searchBy", searchBy);
            var result = await _generic.ReadCustomAsync<SaleReceiptDetailHistory>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpPost("CreateMultiple")]
        public async Task<IActionResult> Create([FromBody]List<SaleReceiptDetail> obj)
        {

            //var param = DataHelper.GenParams("SaleReceiptId", obj.FirstOrDefault().SaleReceiptId, "ProductId", obj.FirstOrDefault().ProductId, "IsAll", true);
            //var resultDelete = await _generic.DeteteAsync<SaleReceiptDetail, int>(param);

            var result = await _generic.CreateMultipleAsync<SaleReceiptDetail>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SaleReceiptDetail obj)
        {
            var result = await _generic.CreateAsync<SaleReceiptDetail, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(SaleReceiptDetail obj)
        {
            var result = await _generic.UpdateAsync<SaleReceiptDetail, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }



        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string saleReceiptId, string saleReceiptDetailId, bool isAll = false)
        {
            var param = DataHelper.GenParams("SaleReceiptId", saleReceiptId, "SaleReceiptDetailId", saleReceiptDetailId, "IsAll", isAll);
            var result = await _generic.DeteteAsync<SaleReceiptDetail, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Mobile
        [HttpGet]
        [Route("~/api/Mobile/[controller]/Read")]
        public async Task<IActionResult> ReadMobile(string saleReceiptId)
        {
            var param = DataHelper.GenParams("SaleReceiptId", saleReceiptId);
            var result = await _generic.ReadCustomAsync<SaleReceiptDetailDataTable>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion
    }
}
