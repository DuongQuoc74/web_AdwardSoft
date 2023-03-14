using AdwardSoft.Core.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Repositories.Pattern;
using AdwardSoft.Utilities.Extensions;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Nest;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerOrderController : ControllerBase
    {
        #region Constructor
        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redisRepository;
        private readonly IAdapterPattern _adapterPattern;
        private readonly IElasticClient _elasticClient;

        public CustomerOrderController(
            IGenericRepository genericRepository,
            IRedisRepositoty redisRepository,
            IAdapterPattern adapterPattern,
            IElasticClient elasticClient
            )
        {
            _redisRepository = redisRepository;
            _genericRepository = genericRepository;
            _adapterPattern = adapterPattern;
            _elasticClient = elasticClient;
        }
        #endregion

        #region Read
        [HttpPost("ReadOrderIds")]
        public async Task<IActionResult> ReadOrderIds(List<string> orderIds)
        {
            // orderIds Will have format like 
            // OrderID001|OrderID002 ...
            Dictionary<string, dynamic> param = DataHelper.GenParams("OrderIds", string.Join("|", orderIds));

            var result = await _genericRepository.ReadCustomAsync<CustomerOrder>(param, "ByOrderIds");

            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatablePagination")]
        public async Task<IActionResult> ReadDatatablePagination(string fromDate, string toDate, int pageSize = 10, int pageSkip = 0, short paymentStatus = 0)
        {
            fromDate = fromDate.IsNullOrEmpty() ? DateTime.Today.ToString("yyyy/MM/dd") : fromDate;
            toDate = toDate.IsNullOrEmpty() ? DateTime.Today.ToString("yyyy/MM/dd") : toDate;

            Dictionary<string, dynamic> param = DataHelper.GenParams(
                "PageSize", pageSize,
                "PageSkip", pageSkip,
                "PaymentStatus", paymentStatus,
                "FromDate", fromDate,
                "ToDate", toDate
                );

            var result = await _genericRepository.ReadCustomAsync<CustomerOrderDatatable>(param, "Pagination");

            if (!result.Success)
            {
                return BadRequest(result.Messages);
            }

            return Ok(result.Response);
        }
        

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _genericRepository.ReadAsync<CustomerOrder>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);

        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(string id)
        {
            var param = DataHelper.GenParams("Id", id);

            var result = await _genericRepository.ReadByCustomAsync<CustomerOrder>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int customerId, short status, short paymentStatus, int deliveryPointId, DateTime dateFrom, DateTime dateTo)
        {
            var param = DataHelper.GenParams("CustomerId", customerId, "Status", status, "PaymentStatus", paymentStatus, "DeliveryPointId", deliveryPointId, "DateFrom", dateFrom, "DateTo", dateTo);

            var result = await _genericRepository.ReadCustomAsync<CustomerOrderDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _genericRepository.ReadCustomAsync<Select>(null, "CustomerOrder");
    
            if (!result.Success)
                    return BadRequest(result.Messages);
    
            return Ok(result.Response);
        }
        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomerOrder obj)
        {
            var result = await _genericRepository.CreateAsync<CustomerOrder, string>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("CreateCustomerOrderDetail")]
        public async Task<IActionResult> CreateCustomerOrderDetail([FromBody] List<CustomerOrderDetail> obj)
        {
            var result = await _genericRepository.CreateMultipleAsync(obj);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(1);
        }


        #endregion

        #region Update
        [HttpPut("PayOrder")]
        public async Task<IActionResult> PayOrder(List<CustomerOrder> customerOrder)
        {
            for (int i = 0; i < customerOrder.Count; i++)
            {
                customerOrder[i].PaymentDate = DateTime.Now;
                customerOrder[i].PaymentStatus = (short) ECustomerOrderPaymentStatus.PAYMENT_STATUS_PAID;

                var result = (await _adapterPattern.QuerySingle<CustomerOrder, int>(
                DataHelper.GenParams(
                    "Id", customerOrder[i].Id, 
                    "PaymentDate", customerOrder[i].PaymentDate, 
                    "PaymentUser", customerOrder[i].PaymentUser,
                    "PaymentStatus", customerOrder[i].PaymentStatus
                    ),
                DataHelper.ApiCRUD.Update,
                ""
                )).FlushData();

                if (!result.Success)
                {
                    return BadRequest(result.Messages);
                }
            }

            return Ok(1);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(CustomerOrder obj)
        {
            var result = await _genericRepository.UpdateAsync<CustomerOrder, string>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _genericRepository.DeteteAsync<CustomerOrder, int>(param);

            if (!result.Success || result.Response< 0)
                return BadRequest(result.Messages);

            // => Remove CustomerOrder
            var key = _redisRepository.GenKey<CustomerOrderRedis>(new CustomerOrderRedis { Id = id });
            await _redisRepository.RemoveAsync(key);

            return Ok(result.Response);

        }

        #endregion
    }
}
