using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;


namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierContactController : Controller
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redis;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public SupplierContactController(IGenericRepository genericRepository, IRedisRepositoty redis, IElasticClient elasticClient,
            IMapper mapper)
        {

            _genericRepository = genericRepository;
            _redis = redis;
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        #endregion

        #region Read
        [HttpGet("Read")]
        [AllowAnonymous]
        public async Task<IActionResult> Read(int supplierId)
        {
            var param = DataHelper.GenParams("supplierId", supplierId);
            var result = await _genericRepository.ReadCustomAsync<SupplierContact>(param, "");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int supplierId, int idx)
        {
            var param = DataHelper.GenParams("supplierId", supplierId, "Idx", idx);
            var result = await _genericRepository.ReadByCustomAsync<SupplierContact>(param, "ById");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SupplierContact obj)
        {
            var result = await _genericRepository.CreateAsync<SupplierContact, bool>(obj);

            if (!result.Success) return BadRequest(result.Messages);

            //Elastic
            await _SyncElasticById(obj.SupplierId);
            return Ok(result.Response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(SupplierContact obj)
        {
            var result = await _genericRepository.UpdateAsync<SupplierContact, bool>(obj);

            if (!result.Success) return BadRequest(result.Messages);

            //Elastic
            await _SyncElasticById(obj.SupplierId);
            return Ok(result.Response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int supplierId, int idx)
        {
            var param = DataHelper.GenParams("supplierId", supplierId, "Idx", idx);
            var result = await _genericRepository.DeteteAsync<SupplierContact, bool>(param);

            if (!result.Success) return BadRequest(result.Messages);

            //Elastic
            await _SyncElasticById(supplierId);
            return Ok(result.Response);
        }

        #region Methods elastic
        private async Task<bool> _SyncElasticById(int id)
        {

            // => Sync Elastic
            var param = DataHelper.GenParams("Id", id);
            var resultToSync = await _genericRepository.ReadByCustomAsync<SupplierElastic>(param, "ById");

            if (!resultToSync.Success || resultToSync.Response == null || resultToSync.Response.Id == 0)
                return false;

            await _elasticClient
                .IndexAsync(resultToSync.Response, i => i.Index("supplier")
                .Id(resultToSync.Response.Id)
                .Refresh(Elasticsearch.Net.Refresh.True));

            return true;
        }
        #endregion
    }
}
