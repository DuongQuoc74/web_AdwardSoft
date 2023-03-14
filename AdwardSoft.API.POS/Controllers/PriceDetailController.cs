using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceDetailController : ControllerBase
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redis;
        private readonly IElasticClient _elasticClient;
        private readonly IAdapterPattern _adapter;
        private readonly IMapper _mapper;

        public PriceDetailController(IGenericRepository genericRepository, IRedisRepositoty redis,
             IElasticClient elasticClient,
            IMapper mapper, IAdapterPattern adapter)
        {
            _genericRepository = genericRepository;
            _redis = redis;
            _elasticClient = elasticClient;
            _mapper = mapper;
            _adapter = adapter;
        }

        #endregion

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int productId = 0, int locationId = 0, int locationChildId = 0, int deliveryPointId = 0, short deliveryType = 1, DateTime date = new DateTime())
        {
            var param = DataHelper.GenParams("ProductId", productId, "LocationId", locationId, "LocationChildId", locationChildId, "DeliveryPointId", deliveryPointId, "DeliveryType", deliveryType, "Date", date);

            var result = await _genericRepository.ReadCustomAsync<PriceDetailDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int productId, int locationId, int deliveryPointId, short deliveryType, DateTime date)
        {
            var param = DataHelper.GenParams("ProductId", productId, "LocationId", locationId, "DeliveryPointId", deliveryPointId, "DeliveryType", deliveryType, "Date", date);
            var result = await _genericRepository.ReadByCustomAsync<PriceDetail>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            if (result.Response == null)
            {
                var initPriceDetail = new PriceDetail() { ProductId = productId, LocationId = locationId, DeliveryPointId = deliveryPointId, DeliveryType = deliveryType, Date = DateTime.Parse(_ConvertDateToShortDateStr(date)), Price = 0 };
                return Ok(initPriceDetail);
            }

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PriceDetail obj)
        {
            var result = await _genericRepository.CreateAsync<PriceDetail, PriceDetailRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            //redis
            await _redis.SetAsync<PriceDetailRedis>(result.Response);

            return Ok(1);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PriceDetail obj)
        {
            var result = await _genericRepository.UpdateAsync<PriceDetail, PriceDetailRedis>(obj);
            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            //redis
            await _redis.SetAsync<PriceDetailRedis>(result.Response);

            return Ok(1);
        }

        #endregion

        #region Delete



        #endregion

        #region Check
        [HttpGet("CheckIsExisting")]
        public async Task<IActionResult> CheckIsExisting(int productId, int locationId, int deliveryPointId, short deliveryType, DateTime date)
        {
            var isExist = await _CheckIsExisting(productId: productId, locationId: locationId, deliveryPointId: deliveryPointId, deliveryType: deliveryType, date: date);

            if (isExist.code == EResponse.Error)
                return BadRequest(isExist.message);

            return Ok((int)isExist.code);
        }
        #endregion

        #region Methods
        private async Task<(EResponse code, string message)> _CheckIsExisting(int productId, int locationId, int deliveryPointId, short deliveryType, DateTime date)
        {
            var param = DataHelper.GenParams("ProductId", productId, "LocationId", locationId, "DeliveryPointId", deliveryPointId, "DeliveryType", deliveryType, "Date", date);
            var result = await _genericRepository.ReadByCustomAsync<PriceDetail, int>(param, "IsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }
        private string _ConvertDateToShortDateStr(DateTime date)
        {
            string[] DateStr = date.ToShortDateString().Split("/");

            return DateStr[2] + "-" + DateStr[1] + "-" + DateStr[0];
        }


        #endregion
    }
}
