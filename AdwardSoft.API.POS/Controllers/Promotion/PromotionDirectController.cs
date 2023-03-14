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
    public class PromotionDirectController : Controller
    {
        private readonly IGenericRepository _generic;
        public PromotionDirectController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int promotionId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId);
            var result = await _generic.ReadCustomAsync<PromotionDirectDatatable>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int promotionId, int purchaseProductId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "purchaseProductId", purchaseProductId);
            var result = await _generic.ReadByCustomAsync<PromotionDirect>(param, "ById");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PromotionDirect obj)
        {
            var result = await _generic.CreateAsync<PromotionDirect, bool>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PromotionDirect obj)
        {
            var result = await _generic.UpdateAsync<PromotionDirect, bool>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int promotionId, int purchaseProductId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "purchaseProductId", purchaseProductId);
            var result = await _generic.DeteteAsync<PromotionDirect, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("CheckExist")]
        public async Task<IActionResult> CheckExist(int promotionId, int purchaseProductId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "purchaseProductId", purchaseProductId);
            var result = await _generic.ReadByCustomAsync<PromotionDirect, int>(param, "IsExist");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
