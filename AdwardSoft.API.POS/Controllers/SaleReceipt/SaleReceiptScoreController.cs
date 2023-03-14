using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleReceiptScoreController : Controller
    {
        private readonly IGenericRepository _generic;
        public SaleReceiptScoreController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(string saleReceiptId, int scoreConfigurationId)
        {
            var param = DataHelper.GenParams("SaleReceiptId", saleReceiptId, "ScoreConfigurationId", scoreConfigurationId);
            var result = await _generic.ReadCustomAsync<SaleReceiptDetailDataTable>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SaleReceiptScore obj)
        {
            var result = await _generic.CreateAsync<SaleReceiptScore, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(SaleReceiptScore obj)
        {
            var result = await _generic.UpdateAsync<SaleReceiptScore, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string saleReceiptId, int scoreConfigurationId)
        {
            var param = DataHelper.GenParams("SaleReceiptId", saleReceiptId, "ScoreConfigurationId", scoreConfigurationId);
            var result = await _generic.DeteteAsync<SaleReceiptScore, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

    }
}
