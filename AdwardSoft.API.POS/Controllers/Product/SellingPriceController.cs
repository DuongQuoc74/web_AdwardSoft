using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellingPriceController : Controller
    {
        private readonly IGenericRepository _generic;
        public SellingPriceController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }


        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSkip, int pageSize, int productId, int unitId)
        {
            var param = DataHelper.GenParams("ProductId", productId, "UnitId", unitId, "pageSkip", pageSkip, "pageSize", pageSize);
            var result = await _generic.ReadCustomAsync<SellingPriceDatatable>(param, "ByProduct");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int productId, int unitId, DateTime date)
        {
            var param = DataHelper.GenParams("ProductId", productId, "UnitId", unitId, "Date", date);
            var result = await _generic.ReadByCustomAsync<SellingPrice>(param, "ById");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByProduct")]
        public async Task<IActionResult> ReadByProduct(int productId)
        {
            var param = DataHelper.GenParams("ProductId", productId);
            var result = await _generic.ReadByCustomAsync<List<SellingPrice>>(param, "ByProduct");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(SellingPrice obj)
        {
            var result = await _generic.CreateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(SellingPrice obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int productId, int unitId, DateTime date)
        {
            var param = DataHelper.GenParams("ProductId", productId, "UnitId", unitId, "Date", date);
            var result = await _generic.DeteteAsync<SellingPrice, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
