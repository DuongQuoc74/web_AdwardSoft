using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
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
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redis;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public CategoryController(IGenericRepository genericRepository, IRedisRepositoty redis,
             IElasticClient elasticClient,
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
        public async Task<IActionResult> Read()
        {
            var result = await _genericRepository.ReadAsync<Category>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
            
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);

            var result = await _genericRepository.ReadByCustomAsync<Category>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _genericRepository.ReadCustomAsync<Select>(null, "Category");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);

        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int pageSize, int pageSkip, string searchBy)
        {
            var param = DataHelper.GenParams("SearchBy", searchBy, "pageSize", pageSize, "pageSkip", pageSkip);
            var result = await _genericRepository.ReadCustomAsync<CategoryDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Category obj)
        {
            var result = await _genericRepository.CreateAsync<Category, CategoryRedis>(obj);

            if (!result.Success ||   result.Response == null)
                return BadRequest(result.Messages);

            //elastic
            var els = _mapper.Map<CategoryRedis, CategoryElastic>(result.Response);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("category")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));
            //redis
            await _redis.SetAsync<CategoryRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Category obj)
        {
            var result = await _genericRepository.UpdateAsync<Category, CategoryRedis>(obj);
            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);
            //elastic
            var els = _mapper.Map<CategoryRedis, CategoryElastic>(result.Response);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("supplier")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));
            //redis
            await _redis.SetAsync<CategoryRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            //Check supplier is using
            //var isUsing = await CheckSupplierIsUsing(id: id);
            //if (isUsing.code == EResponse.Error)
            //    return BadRequest(isUsing.massage);

            //if (isUsing.code == EResponse.IsUsing)
            //    return BadRequest("Supplier is using !");

            //Delete supplier sql
            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _genericRepository.DeteteAsync<Category, int>(parms);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);
            
            //elastic
            _elasticClient.Delete<CategoryElastic>(id, del => del.Index("category"));

            //redis
            var redisResponse = await _redis.SearchKeysAsync<CategoryRedis>(_redis.GenKey(new CategoryRedis { Id = id }));
            await _redis.RemoveAsync(redisResponse.ToList());
            
            return Ok(result.Response);

        }

        #endregion

        #region Check



        #endregion

        #region Methods

        private async Task<(EResponse code, string massage)> CheckCategoryIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _genericRepository.ReadByCustomAsync<Category, int>(param, "CategoryIsUsing");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response == 0)
                return (EResponse.NotUsing, "");
            else
                return (EResponse.IsUsing, "");
        }

        private async Task<(bool success, string message, int count)> _SyncRedis<T>()
        {
            // => Delete Customer in redis
            var keyToDelete = _redis.GenKey<T>();
            var redisToDelete = await _redis.SearchKeysAsync<T>(keyToDelete + ":*");

            await _redis.RemoveAsync<T>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _genericRepository.ReadAsync<T>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            await _redis.SetAsync<T>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<(bool success, string message, int count)> _SyncElastic()
        {
            // => Delete Customer in elastic
            var result = await _elasticClient.DeleteByQueryAsync<CategoryElastic>(del =>
                del.Index("category").Query(q => q.QueryString(qs => qs.Query("*"))));

            // => Sync Elastic
            var resultToSync = await _genericRepository.ReadAsync<CategoryElastic>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            foreach (var els in resultToSync.Response)
            {
                await _elasticClient
                    .IndexAsync(els, i => i.Index("category")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));
            }

            return (true, "", resultToSync.Response.Count());
        }

        #endregion

        #region Sync

        // Sync Redis
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _SyncRedis<CategoryRedis>();

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.count);
        }
        // Sync Elastic
        [HttpGet("SyncElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncElastic()
        {
            var result = await _SyncElastic();

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.count);
        }

        // Sync Elastic
        [HttpDelete("DeleteElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteElastic()
        {
            var result = await _elasticClient.Indices.DeleteAsync("category");

            return Ok(result.IsValid);
        }

        // Sync Elastic
        [HttpPost("CreateElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateElastic()
        {
            var success = false;

            // Customer
            if (!_elasticClient.Indices.Exists("category").Exists)
            {
                var result = await _elasticClient.Indices.CreateAsync("category", index => index.Map<CategoryElastic>(x => x.AutoMap()));
                success = result.IsValid;
            }

            return Ok(success);
        }

        // Sync
        [HttpGet("Sync")]
        [AllowAnonymous]
        public async Task<IActionResult> Sync()
        {
            var resultRedis = await _SyncRedis<CategoryRedis>();

            if (!resultRedis.success)
                return BadRequest(resultRedis.message);

            var resultElastic = await _SyncElastic();

            if (!resultElastic.success)
                return BadRequest(resultElastic.message);

            return Ok(new
            {
                Redis = resultRedis.count,
                Elastic = resultElastic.count
            }
            );
        }
        #endregion
    }
}
