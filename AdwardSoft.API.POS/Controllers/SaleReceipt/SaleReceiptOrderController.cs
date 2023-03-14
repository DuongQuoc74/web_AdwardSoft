using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleReceiptOrderController : Controller
    {
        private readonly IGenericRepository _generic;
        public SaleReceiptOrderController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SaleReceiptOrder obj)
        {
            var result = await _generic.CreateAsync<SaleReceiptOrder, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string saleReceiptId, int customerOrderId)
        {
            var param = DataHelper.GenParams("SaleReceiptId", saleReceiptId, "CustomerOrderId", customerOrderId);
            var result = await _generic.DeteteAsync<SaleReceiptOrder, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
