using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Repositories.Pattern;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerWalletController : ControllerBase
    {   
        #region Constructor
        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public CustomerWalletController(
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
        [HttpGet("ReadDatatableRecharge")]
        public async Task<IActionResult> ReadDatatableRecharge(DateTime dateFrom, DateTime dateTo, decimal amountFrom, decimal amountTo, int customerId)
        {
            var param = DataHelper.GenParams("DateFrom", dateFrom, "DateTo", dateTo, "AmountFrom", amountFrom, "AmountTo", amountTo, "CustomerId", customerId);
            var result = await _generic.ReadCustomAsync<CustomerWalletDatatable>(param, "Recharge");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatableTopupCredit")]
        public async Task<IActionResult> ReadDatatableTopupCredit(DateTime dateFrom, DateTime dateTo, decimal amountFrom, decimal amountTo, int customerId)
        {
            var param = DataHelper.GenParams("DateFrom", dateFrom, "DateTo", dateTo, "AmountFrom", amountFrom, "AmountTo", amountTo, "CustomerId", customerId);
            var result = await _generic.ReadCustomAsync<CustomerWalletDatatable>(param, "TopupCredit");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatableInternalRecharge")]
        public async Task<IActionResult> ReadDatatableInternalRecharge(DateTime dateFrom, DateTime dateTo, decimal amountFrom, decimal amountTo, int customerId)
        {
            var param = DataHelper.GenParams("DateFrom", dateFrom, "DateTo", dateTo, "AmountFrom", amountFrom, "AmountTo", amountTo, "CustomerId", customerId);
            var result = await _generic.ReadCustomAsync<CustomerWalletDatatable>(param, "InternalRecharge");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        [HttpGet("ReadDatatableCreditApproval")]
        public async Task<IActionResult> ReadDatatableCreditApproval()
        {
            var result = await _generic.ReadCustomAsync<CustomerWalletDatatable>(null, "CreditApproval");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        [HttpGet("ReadDatatableRechargeApproval")]
        public async Task<IActionResult> ReadDatatableRechargeApproval()
        {
            var result = await _generic.ReadCustomAsync<CustomerWalletDatatable>(null, "RechargeApproval");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(string id)
        {
            var result = await _ReadById(id: id);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.response);
        }

        [HttpGet("ReadAccountBalance")]
        public async Task<IActionResult> ReadAccountBalance(string customerId)
        {
            Dictionary<string, dynamic> param = DataHelper.GenParams("CustomerId", customerId);
            var result = await _generic.ReadByCustomAsync<CustomerWallet, long>(param, "AccountBalanceByCustomerId");

            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }
            return Ok(result.Response);
        }
        #endregion

        #region Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CustomerWallet obj)
        {
            // => Create CustomerWallet
            var result = await _generic.CreateAsync<CustomerWallet, string>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("CreateMulti")]
        public async Task<IActionResult> CreateMultiAsync(List<CustomerWallet> customerWallets)
        {
            var result = await _generic.CreateMultipleAsync<CustomerWallet>(customerWallets);

            if (result.Success)
            {
                return Ok(true);
            }

            return BadRequest(result.Messages);
        }
        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(CustomerWallet obj)
        {
            // => Update CustomerWallet
            var result = await _generic.UpdateAsync<CustomerWallet, CustomerWalletRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<CustomerWalletRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        
        [HttpPut("Approval")]
        public async Task<IActionResult> ApprovalAsync(CustomerWallet obj)
        {
            var result = (await _adapter.QuerySingle<CustomerWallet, CustomerWalletRedis>(obj, DataHelper.ApiCRUD.Update, "Status")).FlushData();

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<CustomerWalletRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            // => Delete sql
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<CustomerWallet, int>(param);

            if (!result.Success && result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Redis
            var key = _redis.GenKey<CustomerWalletRedis>(new CustomerWalletRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }
        #endregion

        #region Methods
        private async Task<(bool success, string message, CustomerWallet response)> _ReadById(string id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<CustomerWallet>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }
        #endregion

        #region Sync
        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<CustomerWalletRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<CustomerWalletRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<CustomerWalletRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<CustomerWalletRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<CustomerWalletRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }
        #endregion
    }
}
