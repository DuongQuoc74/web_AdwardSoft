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
    public class PromotionGiftController : Controller
    {
        private readonly IGenericRepository _generic;
        public PromotionGiftController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int promotionId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId);
            var result = await _generic.ReadCustomAsync<PromotionGiftDatatable>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int promotionId, int giftProductId, int purchaseProductId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "GiftProductId", giftProductId, "purchaseProductId", purchaseProductId);
            var result = await _generic.ReadByCustomAsync<PromotionGift>(param, "ById");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PromotionGift obj)
        {
            var result = await _generic.CreateAsync<PromotionGift, bool>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PromotionGift obj)
        {
            var result = await _generic.UpdateAsync<PromotionGift, bool>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int promotionId, int giftProductId, int purchaseProductId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "GiftProductId", giftProductId, "purchaseProductId", purchaseProductId);
            var result = await _generic.DeteteAsync<PromotionGift, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("CheckExist")]
        public async Task<IActionResult> CheckExist(int promotionId, int giftProductId, int purchaseProductId)
        {
            var param = DataHelper.GenParams("PromotionId", promotionId, "GiftProductId", giftProductId, "purchaseProductId", purchaseProductId);
            var result = await _generic.ReadByCustomAsync<PromotionGift, int>(param, "IsExist");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
