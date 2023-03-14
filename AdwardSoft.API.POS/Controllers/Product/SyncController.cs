using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public SyncController(
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

        #region Product
        [HttpGet("ReadProduct")]
        public async Task<IActionResult> ReadProduct()
        {
            try
            {


                var result = await _generic.ReadAsync<ProductData>();
                if (result.Success)
                {
                    var products = result.Response;
                    foreach (var item in products)
                    {
                        if (item.Code == "3073781158229")
                        {
                            var code = "3073781158229";
                        }
                        var redis = new ProductRedis()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            CategoryId = item.CategoryId,
                            Code = item.Code,
                            Image = item.Image,
                            IsDefault = item.IsDefault,
                            StockId = item.StockId,
                            Status = item.Status,
                            MaxStock = item.MaxStock,
                            MinStock = item.MinStock,
                            RetailPrice = item.RetailPrice,
                            UnitId = item.UnitId,
                            WholesalePrice = item.WholesalePrice
                        };
                        //elastic
                        var els = _mapper.Map<ProductRedis, ProductElastic>(redis);

                        await _elasticClient
                                .IndexAsync(els, i => i.Index("product")
                                .Id(els.Id)
                                .Refresh(Elasticsearch.Net.Refresh.True));

                        //redis
                        await _redis.SetAsync<ProductRedis>(redis);
                    }
                }
            }
            catch
            {
                throw;
            }

            return Ok();
        }
        #endregion

    }
}
