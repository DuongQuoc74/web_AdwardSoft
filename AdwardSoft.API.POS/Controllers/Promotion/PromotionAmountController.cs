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
    public class PromotionAmountController : Controller
    {
        private readonly IGenericRepository _generic;
        public PromotionAmountController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int promotionId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId);
            var result = await _generic.ReadCustomAsync<PromotionAmount>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int promotionId, int idx)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "Idx", idx);
            var result = await _generic.ReadByCustomAsync<PromotionAmount>(param, "ById");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PromotionAmount obj)
        {
            var result = await _generic.CreateAsync<PromotionAmount, bool>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PromotionAmount obj)
        {
            var result = await _generic.UpdateAsync<PromotionAmount, bool>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int promotionId, int idx)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "Idx", idx);
            var result = await _generic.DeteteAsync<PromotionAmount, bool>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
