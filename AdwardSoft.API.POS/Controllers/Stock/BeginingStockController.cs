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
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeginingStockController : ControllerBase
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redis;
        private readonly IElasticClient _elasticClient;
        private readonly IAdapterPattern _adapter;
        private readonly IMapper _mapper;

        public BeginingStockController(IGenericRepository genericRepository, IRedisRepositoty redis,
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
        [HttpGet("ReadByYear")]
        public async Task<IActionResult> ReadByDate(int productId, int stockId, string year, int unitId)
        {
            var param = DataHelper.GenParams("ProductId", productId, "StockId", stockId, "Year", year, "UnitId", unitId);
            var result = await _genericRepository.ReadByCustomAsync<BeginingStockDataTable>(param, "ByYear");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }


        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSize, int pageSkip, string searchBy
            , string year, int branchId, int stockId = 0, int productId = 0)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "Year", year, "StockId", stockId, "ProductId", productId, "BranchId", branchId);

            var result = await _genericRepository.ReadCustomAsync<BeginingStockDataTable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _genericRepository.ReadCustomAsync<Select>(null, "BeginingStock");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int locationId)
        {
            var param = DataHelper.GenParams("LocationId", locationId);
            var result = await _genericRepository.ReadCustomAsync<DeliveryPointDatatable>(param, "ByLocationId");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("CreateMulti")]
        public async Task<IActionResult> CreateMulti([FromBody] List<BeginingStock> obj)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(obj.First().StockId, obj.First().Year);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Group by
            var stockGrouped = obj.GroupBy(x => new { x.ProductId, x.UnitId })
                                .Select(x => new BeginingStock
                                {
                                    StockId = x.First().StockId,
                                    ProductId = x.First().ProductId,
                                    Quantity = x.Sum(y => y.Quantity),
                                    Year = x.First().Year,
                                    UnitId = x.First().UnitId,
                                    IsLock = true
                                }).Where(x => x.ProductId != 0).ToList();

            //// => Delete for sync
            //var resultDelete = (await _adapter.QuerySingle<BeginingStock, int>(
            //    DataHelper.GenParams("StockId", stockGrouped.First().StockId, "Year", stockGrouped.First().Year),
            //    DataHelper.ApiCRUD.Delete,
            //    "ByYear"
            //)).FlushData();

            //if (!resultDelete.Success)
            //    return BadRequest(resultDelete.Messages);

            // => Get Product Id
            var result = await _genericRepository.CreateMultipleAsync(stockGrouped);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Upsert

        [HttpPut("Upsert")]
        public async Task<IActionResult> Upsert([FromBody] List<BeginingStockData> obj)
        {
            var productNotCreated = new List<string>();

            // => Check Locking
            var isLocking = await _CheckIsLock(obj.First().StockId, obj.First().Year);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Get Product Id By Code
            foreach (var stk in obj)
            {
                var res = await _ReadProductIdByCode(stk.ProductCode);

                if (!res.success)
                    return BadRequest(res.message);

                stk.ProductId = res.productId;
            }

            // => Get Product not in db
            productNotCreated = obj.Where(x => x.ProductId == default(int))
                                   .Select(x => x.ProductCode).ToList();

            // => Group by
            var stockGrouped = obj.GroupBy(x => new { x.ProductCode, x.UnitId })
                                .Select(x => new BeginingStock
                                {
                                    StockId = x.First().StockId,
                                    ProductId = x.First().ProductId,
                                    Quantity = x.Sum(y => y.Quantity),
                                    Year = x.First().Year,
                                    UnitId = x.First().UnitId,
                                    IsLock = true
                                }).Where(x => x.ProductId != 0).ToList();

            // => Delete for sync
            //var resultDelete = (await _adapter.QuerySingle<BeginingStock, int>(
            //    DataHelper.GenParams("StockId", stockGrouped.First().StockId, "Year", stockGrouped.First().Year),
            //    DataHelper.ApiCRUD.Delete,
            //    "ByYear"
            //)).FlushData();

            //if (!resultDelete.Success)
            //    return BadRequest(resultDelete.Messages);

            // => Get Product Id
            var result = await _genericRepository.CreateMultipleAsync(stockGrouped);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Update

        [HttpPut("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(BeginingStock obj)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(obj.StockId, obj.Year);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            var result = (await _adapter.QuerySingle<BeginingStock, int>(obj, DataHelper.ApiCRUD.Update, "Quantity")).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Lock/Unlock

        [HttpPut("Lock")]
        public async Task<IActionResult> Lock(string year, int stockId)
        {
            var result = (await _adapter.QuerySingle<BeginingStock, int>(
                DataHelper.GenParams("StockId", stockId, "Year", year),
                DataHelper.ApiCRUD.Update,
                "Lock"
            )).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPut("Unlock")]
        public async Task<IActionResult> Unlock(string year, int stockId)
        {
            var result = (await _adapter.QuerySingle<BeginingStock, int>(
            DataHelper.GenParams("StockId", stockId, "Year", year),
            DataHelper.ApiCRUD.Update,
            "Unlock"
        )).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Delete

        [HttpDelete("DeleteByYear")]
        public async Task<IActionResult> DeleteByYear(int stockId, string year)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(stockId, year);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Delete all
            var param = DataHelper.GenParams("StockId", stockId, "Year", year);
            var result = (await _adapter.QuerySingle<BeginingStock, int>(param, DataHelper.ApiCRUD.Delete, "ByYear")).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int productId, int stockId, string year, int unitId)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(stockId, year);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Delete all
            var param = DataHelper.GenParams("ProductId", productId, "StockId", stockId, "Year", year, "UnitId", unitId);
            var result = (await _adapter.QuerySingle<BeginingStock, int>(param, DataHelper.ApiCRUD.Delete, "")).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Check

        [HttpGet("CheckIsLock")]
        public async Task<IActionResult> CheckIsLock(int stockId, string year)
        {
            var isLock = await _CheckIsLock(stockId: stockId, year: year);

            if (isLock.code == EResponse.Error)
                return BadRequest(isLock.message);

            return Ok((int)isLock.code);
        }

        [HttpGet("CheckHaveData")]
        public async Task<IActionResult> CheckHaveData(int stockId, string year)
        {
            var haveData = await _CheckHaveData(stockId: stockId, year: year);

            if (haveData.code == EResponse.Error)
                return BadRequest(haveData.message);

            return Ok((int)haveData.code);
        }


        #endregion

        #region Methods

        private async Task<(EResponse code, string message)> _CheckIsLock(int stockId, string year)
        {
            var param = DataHelper.GenParams("StockId", stockId, "Year", year);

            var result = await _genericRepository.ReadByCustomAsync<BeginingStock, int>(param, "IsLock");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.Lock, "");

            return (EResponse.UnLock, "");
        }

        private async Task<(EResponse code, string message)> _CheckHaveData(int stockId, string year)
        {
            var param = DataHelper.GenParams("StockId", stockId, "Year", year);

            var result = await _genericRepository.ReadByCustomAsync<BeginingStock, int>(param, "HaveData");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.HaveData, "");

            return (EResponse.NoData, "");
        }

        private async Task<(bool success, string message, int productId)> _ReadProductIdByCode(string code)
        {
            var result = await _genericRepository.ReadByCustomAsync<Product, int>(DataHelper.GenParams("Code", code), "ProductIdByCode");

            return (result.Success, result.Messages, result.Response);
        }

        #endregion
    }
}
