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
    public class PromotionController : Controller
    {
        private readonly IGenericRepository _generic;
        public PromotionController(IGenericRepository genericRepository)
        {
            _generic = genericRepository;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(short type, short status, int year)
        {
            var param = DataHelper.GenParams("Type", type, "Status", status, "Year", year);
            var result = await _generic.ReadCustomAsync<Promotion>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<Promotion>(id);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Promotion obj)
        {
            var result = await _generic.CreateAsync<Promotion, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Promotion obj)
        {
            var result = await _generic.UpdateAsync<Promotion,int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _generic.DeteteAsync<Promotion, int>(param);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }


        [HttpPost("CheckDate")]
        public async Task<IActionResult> CheckDate(Promotion obj)
        {
            var param = DataHelper.GenParams("Id", obj.Id, "Type", obj.Type, "EffectiveDate", obj.EffectiveDate, "ExpiryDate", obj.ExpiryDate);
            var result = await _generic.ReadByCustomAsync<Promotion, bool>(param, "CheckDate");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}
