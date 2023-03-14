using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public ProductController(
            IRedisRepositoty redis,
            IGenericRepository generic,
            IAdapterPattern adapter,
            IElasticClient elasticClient,
            IMapper mapper)
        {
            _redis = redis;
            _generic = generic;
            _adapter = adapter;
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        #endregion

        #region Mobile
        #endregion

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<ProductData>();

            if (result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "Product");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<ProductData>(id);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int pageSize, int pageSkip, string searchBy, string categoryIds)
        {
            var param = DataHelper.GenParams("searchBy", searchBy, "pageSize", pageSize, "PageSkip", pageSkip, "CategoryIds", categoryIds);
            var result = await _generic.ReadCustomAsync<ProductDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadByCategoryId")]
        public async Task<IActionResult> ReadByCategory(int categoryId = 0)
        {
            var param = DataHelper.GenParams("CategoryId", categoryId);
            var result = await _generic.ReadCustomAsync<ProductData>(param, "ByCategoryId");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadProductOrder")]
        public async Task<IActionResult> ReadProductOrder()
        {
            var result = await _generic.ReadCustomAsync<CustomerOrderDetailDatatable>(null, "ProductList");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProductData obj)
        {
            var result = await _generic.CreateAsync<ProductData, ProductRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);
            //elastic
            var els = _mapper.Map<ProductRedis, ProductElastic>(result.Response);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("product")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));

            //redis
            await _redis.SetAsync<ProductRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        [HttpPost("CreateSellingPriceUnit")]
        public async Task<IActionResult> CreateSellingPriceUnit([FromBody] List<SellingPriceUnit> obj)
        {
            //usp_SellingPriceUnitViewModel_Create
            var result = await _generic.CreateMultipleAsync(obj);

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(1);
        }

        [HttpPost("CreateMobile")]
        public async Task<IActionResult> CreateMobile([FromBody] ProductMoible obj)
        {
            var result = await _generic.CreateAsync<ProductData, ProductRedis>(obj.Product);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);
            //elastic
            var els = _mapper.Map<ProductRedis, ProductElastic>(result.Response);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("product")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));

            //redis
            await _redis.SetAsync<ProductRedis>(result.Response);

            //Converter
            obj.UnitConverters = obj.UnitConverters.Select(c => { c.ProductId = result.Response.Id; c.QuantityFrom = 1; return c; }).ToList();
            var resultConverter = await _generic.CreateMultipleAsync(obj.UnitConverters);

            if (!resultConverter.Success)
                return BadRequest(resultConverter.Messages);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(ProductData obj)
        {
            // => Update unit
            var result = await _generic.UpdateAsync<ProductData, ProductRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);
            // Update Elastic
            var els = _mapper.Map<ProductRedis, ProductElastic>(result.Response);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("product")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));

            // => Set Redis
            await _redis.SetAsync<ProductRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Delete product
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<ProductData, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            // => Delete Elastic
            _elasticClient.Delete<ProductElastic>(id, del => del.Index("product"));

            // => Remove Redis
            var key = _redis.GenKey<ProductRedis>(new ProductRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }

        #endregion

        #region Check

        [HttpGet("CheckCodeIsExisting")]
        public async Task<IActionResult> CheckCodeIsExisting(int id, string code)
        {
            var isExisting = await _CheckCodeIsExisting(id: id, code: code);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        #endregion

        #region Methods

        private async Task<(EResponse code, string message)> _CheckCodeIsExisting(int id, string code)
        {
            var param = DataHelper.GenParams("Id", id, "Code", code);

            var result = await _generic.ReadByCustomAsync<Product, int>(param, "CodeIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        private async Task<(EResponse code, string message)> _CheckCodeIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Product, int>(param, "CodeIsUsing");
            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        private async Task<(bool success, string message, int count)> _SyncRedis()
        {
            // => Delete Customer in redis
            var keyToDelete = _redis.GenKey<ProductRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<ProductRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<ProductRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<ProductRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            await _redis.SetAsync<ProductRedis>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<(bool success, string message, int count)> _SyncElastic()
        {
            // => Delete Customer in elastic
            var result = await _elasticClient.DeleteByQueryAsync<ProductElastic>(del =>
                del.Index("product").Query(q => q.QueryString(qs => qs.Query("*"))));

            // => Sync Elastic
            var resultToSync = await _generic.ReadAsync<ProductElastic>();

            if (!resultToSync.Success || !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            foreach (var els in resultToSync.Response)
            {
                await _elasticClient
                    .IndexAsync(els, i => i.Index("product")
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
            var result = await _SyncRedis();

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
            var result = await _elasticClient.Indices.DeleteAsync("product");

            return Ok(result.IsValid);
        }

        // Sync Elastic
        [HttpPost("CreateElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateElastic()
        {
            var success = false;

            // Customer
            if (!_elasticClient.Indices.Exists("product").Exists)
            {
                var result = await _elasticClient.Indices.CreateAsync("product", index => index.Map<ProductElastic>(x => x.AutoMap()));
                success = result.IsValid;
            }

            return Ok(success);
        }

        // Sync
        [HttpGet("Sync")]
        [AllowAnonymous]
        public async Task<IActionResult> Sync()
        {
            var resultRedis = await _SyncRedis();

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

        #region Search
        [AllowAnonymous]
        [HttpGet("SearchByCode")]
        public async Task<IActionResult> SearchByCode(string code)
        {
            var responsedata = _elasticClient.Search<ProductElastic>(s => s
                                                    .Index("product")
                                                    .Query(q =>
                                                             q.Match(f => f.Field(p => p.Code).Query(code))
                                                          )
                                                    );
            ProductVoucher productData = new ProductVoucher();
            if (responsedata.Total == 0)
            {
                //return Ok(new ProductData());
                var result = await _generic.ReadByCustomAsync<ProductVoucher>(DataHelper.GenParams("Code", code), "ByCode");
                productData = result.Response;
            }
            else
            {
                var result = await _generic.ReadByIdAsync<ProductVoucher>(responsedata.Documents.FirstOrDefault().Id);
                productData = result.Response;
            }
            if (productData != null)
            {
                var prices = await _generic.ReadCustomAsync<SellingPrice>(DataHelper.GenParams("ProductId", productData.Id), "ByProduct");
                productData.SellingPrices = prices.Response.ToList();

                var param = DataHelper.GenParams("ProductId", productData.Id);
                var unit = await _generic.ReadCustomAsync<Select>(param, "UnitByProduct");
                productData.Units = unit.Response.ToList();
                return Ok(productData);
            }
            else
            {
                return BadRequest();
            }

        }

        [AllowAnonymous]
        [HttpGet("SearchMobile")]
        public async Task<IActionResult> SearchMobile(string code)
        {
            var responsedata = _elasticClient.Search<ProductElastic>(s => s
                                                    .Index("product")
                                                    .Query(q =>
                                                             q.Match(f => f.Field(p => p.Code).Query(code))
                                                          )
                                                    );
            ProductUnitSearch productData = new ProductUnitSearch();
            if (responsedata.Total == 0)
            {

                var result = await _generic.ReadByCustomAsync<ProductVoucher>(DataHelper.GenParams("Code", code), "ByCode");
                if (result.Success && result.Response.Id != 0)
                {
                    productData.Id = result.Response.Id;
                    productData.Name = result.Response.Name;
                    productData.Code = result.Response.Code;
                }

            }
            else
            {
                productData.Id = responsedata.Documents.FirstOrDefault().Id;
                productData.Name = responsedata.Documents.FirstOrDefault().Name;
                productData.Code = responsedata.Documents.FirstOrDefault().Code;
            }

            if (productData != null)
            {
                var param = DataHelper.GenParams("ProductId", productData.Id);
                var unit = await _generic.ReadCustomAsync<Select>(param, "UnitByProduct");
                productData.Units = unit.Response.ToList();

                return Ok(productData);
            }
            else
            {
                return BadRequest();
            }

        }
        #endregion

        #region Print
        [HttpGet("Print")]
        public async Task<IActionResult> Print(int type, int pageSize, int pageSkip, string searchBy, string categoryIds)
        {
            var param = DataHelper.GenParams("searchBy", searchBy, "pageSize", pageSize, "PageSkip", pageSkip, "CategoryIds", categoryIds, "Type", type);
            var result = await _generic.ReadCustomAsync<ProductPrint>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion
    }
}
