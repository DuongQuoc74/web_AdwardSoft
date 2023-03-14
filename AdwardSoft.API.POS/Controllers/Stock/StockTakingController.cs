using System;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StockTakingController : ControllerBase
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redis;
        private readonly IElasticClient _elasticClient;
        private readonly IAdapterPattern _adapter;
        private readonly IMapper _mapper;

        public StockTakingController(IGenericRepository genericRepository, IRedisRepositoty redis,
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

        [HttpGet("ReadByDate")]
        public async Task<IActionResult> ReadByDate(int productId, int stockId, DateTime date)
        {
            var param = DataHelper.GenParams("ProductId", productId, "StockId", stockId, "Date", date);
            var result = await _genericRepository.ReadByCustomAsync<StockTakingDatatable>(param, "ByDate");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSize, int pageSkip, string searchBy
            , DateTime dateFrom, DateTime dateTo, int branchId, int stockId = 0, int productId = 0)
        {
            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy
                , "DateFrom", dateFrom, "DateTo", dateTo
                , "StockId", stockId, "ProductId", productId, "BranchId", branchId);
            
            var result = await _genericRepository.ReadCustomAsync<StockTakingDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(StockTaking obj)
        {
            var result = await _genericRepository.CreateAsync<StockTaking>(obj);

            if (!result.Success || result.Response <= 0 )
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPost]
        [Route("~/api/Mobile/[controller]/Create")]
        public async Task<IActionResult> CreateMultiMobile([FromBody] List<StockTaking> obj)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(obj.First().StockId, obj.First().Date);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Group by
            var stockGrouped = obj.GroupBy(x => new { x.ProductId, x.UnitId })
                                .Select(x => new StockTaking
                                {
                                    StockId = x.First().StockId,
                                    ProductId = x.First().ProductId,
                                    Quantity = x.Sum(y => y.Quantity),
                                    Date = x.First().Date,
                                    UnitId = x.First().UnitId,
                                    IsLock = true
                                }).Where(x => x.ProductId != 0).ToList();

            var result = await _genericRepository.CreateMultipleAsync(stockGrouped);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPost("CreateMulti")]
        public async Task<IActionResult> CreateMulti([FromBody] List<StockTaking> obj)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(obj.First().StockId, obj.First().Date);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Group by
            var stockGrouped = obj.GroupBy(x => new { x.ProductId, x.UnitId })
                                .Select(x => new StockTaking
                                {
                                    StockId = x.First().StockId,
                                    ProductId = x.First().ProductId,
                                    Quantity = x.Sum(y => y.Quantity),
                                    Date = x.First().Date,
                                    UnitId = x.First().UnitId,
                                    IsLock = true
                                }).Where(x => x.ProductId != 0).ToList();

            // => Delete for sync
            //var resultDelete = (await _adapter.QuerySingle<StockTaking, int>(
            //    DataHelper.GenParams("StockId", stockGrouped.First().StockId, "Date", stockGrouped.First().Date),
            //    DataHelper.ApiCRUD.Delete,
            //    "ByDate"
            //)).FlushData();

            //if (!resultDelete.Success)
            //    return BadRequest(resultDelete.Messages);

            var result = await _genericRepository.CreateMultipleAsync(stockGrouped);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Upsert

        [HttpPut("Upsert")]
        public async Task<IActionResult> Upsert([FromBody] List<StockTakingData> obj)
        {
            var productNotCreated = new List<string>();

            // => Check Locking
            var isLocking = await _CheckIsLock(obj.First().StockId, obj.First().Date);

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
                                .Select(x => new StockTaking
                                {
                                    StockId = x.First().StockId,
                                    ProductId = x.First().ProductId,
                                    Quantity = x.Sum(y => y.Quantity),
                                    Date = x.First().Date,
                                    UnitId = x.First().UnitId,
                                    IsLock = true
                                }).Where(x => x.ProductId != 0).ToList();

            // => Delete for sync
            //var resultDelete = (await _adapter.QuerySingle<StockTaking, int>(
            //    DataHelper.GenParams("StockId", stockGrouped.First().StockId, "Date", stockGrouped.First().Date), 
            //    DataHelper.ApiCRUD.Delete, 
            //    "ByDate"
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
        public async Task<IActionResult> UpdateQuantity(StockTaking obj)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(obj.StockId, obj.Date);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            var result = (await _adapter.QuerySingle<StockTaking, int>(obj, DataHelper.ApiCRUD.Update, "Quantity")).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Lock/Unlock

        [HttpPut("Lock")]
        public async Task<IActionResult> Lock(DateTime date, int stockId)
        {
            var result = (await _adapter.QuerySingle<StockTaking, int>(
                DataHelper.GenParams("StockId", stockId, "Date", date),
                DataHelper.ApiCRUD.Update,
                "Lock"
            )).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPut("Unlock")]
        public async Task<IActionResult> Unlock(DateTime date, int stockId)
        {
            // => Check is Forward
            var isForward = await _CheckIsForward(stockId, date);

            if (isForward.code == EResponse.Error)
                return BadRequest(isForward.message);

            if (isForward.code == EResponse.IsForward)
                return BadRequest("This content is forward cannot unlock!");

                var result = (await _adapter.QuerySingle<StockTaking, int>(
                DataHelper.GenParams("StockId", stockId, "Date", date),
                DataHelper.ApiCRUD.Update,
                "Unlock"
            )).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Delete

        [HttpDelete("DeleteByDate")]
        public async Task<IActionResult> DeleteByDate(int stockId, DateTime date)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(stockId, date);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Delete all
            var param = DataHelper.GenParams("StockId", stockId, "Date", date);
            var result = (await _adapter.QuerySingle<StockTaking, int>(param, DataHelper.ApiCRUD.Delete, "ByDate")).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int productId, int stockId, DateTime date, int unitId)
        {
            // => Check Locking
            var isLocking = await _CheckIsLock(stockId, date);

            if (isLocking.code == EResponse.Error)
                return BadRequest(isLocking.message);

            if (isLocking.code == EResponse.Lock)
                return BadRequest("Locking !");

            // => Delete all
            var param = DataHelper.GenParams("ProductId", productId, "StockId", stockId, "Date", date, "UnitId", unitId);
            var result = (await _adapter.QuerySingle<StockTaking, int>(param, DataHelper.ApiCRUD.Delete, "")).FlushData();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Check

        [HttpGet("CheckIsLock")]
        public async Task<IActionResult> CheckIsLock(int stockId, DateTime date)
        {
            var isLock = await _CheckIsLock(stockId: stockId, date: date);

            if (isLock.code == EResponse.Error)
                return BadRequest(isLock.message);

            return Ok((int)isLock.code);
        }

        [HttpGet("CheckIsForward")]
        public async Task<IActionResult> CheckIsForward(int stockId, DateTime date)
        {
            var isForward = await _CheckIsForward(stockId: stockId, date: date);

            if (isForward.code == EResponse.Error)
                return BadRequest(isForward.message);

            return Ok((int)isForward.code);
        }

        [HttpGet("CheckHaveData")]
        public async Task<IActionResult> CheckHaveData(int stockId, DateTime date)
        {
            var haveData = await _CheckHaveData(stockId: stockId, date: date);

            if (haveData.code == EResponse.Error)
                return BadRequest(haveData.message);

            return Ok((int)haveData.code);
        }


        #endregion

        #region Methods

        private async Task<(EResponse code, string message)> _CheckIsLock(int stockId, DateTime date)
        {
            var param = DataHelper.GenParams("StockId", stockId, "Date", date);

            var result = await _genericRepository.ReadByCustomAsync<StockTaking, int>(param, "IsLock");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.Lock, "");

            return (EResponse.UnLock, "");
        }

        private async Task<(EResponse code, string message)> _CheckIsForward(int stockId, DateTime date)
        {
            var param = DataHelper.GenParams("StockId", stockId, "Date", date);

            var result = await _genericRepository.ReadByCustomAsync<StockTaking, int>(param, "IsForward");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsForward, "");

            return (EResponse.NotForward, "");
        }

        private async Task<(EResponse code, string message)> _CheckHaveData(int stockId, DateTime date)
        {
            var param = DataHelper.GenParams("StockId", stockId, "Date", date);

            var result = await _genericRepository.ReadByCustomAsync<StockTaking, int>(param, "HaveData");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.HaveData, "");

            return (EResponse.NoData, "");
        }

        private async Task<(bool success, string message, int productId)> _ReadProductIdByCode(string code)
        {
            var result = await _genericRepository.ReadByCustomAsync<StockTaking, int>(DataHelper.GenParams("Code", code), "ProductIdByCode");

            return (result.Success, result.Messages, result.Response);
        }

        #endregion
    }
}
