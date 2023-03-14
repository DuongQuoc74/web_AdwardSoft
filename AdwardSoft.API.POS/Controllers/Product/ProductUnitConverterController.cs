using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductUnitConverterController : ControllerBase
    {
        #region Contructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public ProductUnitConverterController(
            IRedisRepositoty redis, 
            IGenericRepository generic, 
            IAdapterPattern adapter)
        {
            _redis = redis;
            _generic = generic;
            _adapter = adapter;
        }

        #endregion

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> ReadAsync(int productId)
        {
            var param = DataHelper.GenParams("ProductId", productId);

            var result = await _generic.ReadCustomAsync<ProductUnitConverterDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int productId, int unitId)
        {
            var param = DataHelper.GenParams("ProductId", productId, "UnitId", unitId);
            var result = await _generic.ReadByCustomAsync<ProductUnitConverter>(param, "ById");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(ProductUnitConverter obj)
        {
            var redisReturn = await _generic.CreateAsync<ProductUnitConverter, ProductUnitConverterRedis>(obj);

            if (!redisReturn.Success && redisReturn.Response == null)
                return BadRequest(redisReturn.Messages);

            // => Store redis
            await _redis.SetAsync<ProductUnitConverterRedis>(redisReturn.Response);

            return Ok(1);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(ProductUnitConverter obj)
        {
            var redisReturn = await _generic.UpdateAsync<ProductUnitConverter, ProductUnitConverterRedis>(obj);

            if (!redisReturn.Success && redisReturn.Response == null)
                return BadRequest(redisReturn.Messages);

            // => Store redis
            await _redis.SetAsync<ProductUnitConverterRedis>(redisReturn.Response);

            return Ok(1);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int productId, int unitId)
        {
            var param = DataHelper.GenParams("ProductId", productId, "UnitId", unitId);

            var result = await _generic.DeteteAsync<ProductUnitConverter, int>(param);

            if (!result.Success)
                return BadRequest(result.Messages);

            // => Delete redis
            await _redis.RemoveAsync<ProductUnitConverterRedis>(_redis.GenKey(new ProductUnitConverterRedis { ProductId = productId, UnitId = unitId }));

            return Ok(result.Response);
        }
        #endregion

        #region Sync

        [HttpGet("SyncRedis")]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _generic.ReadAsync<ProductUnitConverterRedis>();

            if (!result.Success)
                return BadRequest(result.Messages);

            result.Response.Select(async x =>
            {
                await _redis.SetAsync<ProductUnitConverterRedis>(x);
            });

            return Ok(result.Response.Count());
        }

        #endregion

        #region Methods

        [HttpGet("CheckExist")]
        public async Task<IActionResult> CheckExist(int unitId, int productId)
        {
            //usp_ProductUnitConverter_ReadIsExist
            var param = DataHelper.GenParams("UnitId", unitId, "ProductId", productId);
            var result = await _generic.ReadByCustomAsync<ProductUnitConverter, int>(param, "IsExist");

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        #endregion
    }
}
