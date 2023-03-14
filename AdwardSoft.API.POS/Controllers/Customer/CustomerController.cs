using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Repositories.Pattern;
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
    [Authorize]
    public class CustomerController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public CustomerController(
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

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Customer>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "Customer");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);

        }

        [HttpGet("ReadSelectByCreditLimit")]
        public async Task<IActionResult> ReadSelectByCreditLimit()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "CustomerByCreditLimit");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);

        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByCustomAsync<CustomerDatatable>(DataHelper.GenParams("Id", id), "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadByUserId")]
        [AllowAnonymous]
        public async Task<IActionResult> ReadByUserId(long userId)
        {
            var result = await _generic.ReadByCustomAsync<Customer>(DataHelper.GenParams("UserId", userId), "ByUserId");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int pageSize = 0, int pageSkip = 0, string searchBy = "", int customerGroupId = 0, short type = -1)
        {

            var param = DataHelper.GenParams("PageSize", pageSize, "PageSkip", pageSkip, "SearchBy", searchBy, "CustomerGroupId", customerGroupId, "Type", type);

            var result = await _generic.ReadCustomAsync<CustomerDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("FrmReadById")]
        public async Task<IActionResult> FrmReadById(int id)
        {

            var param = DataHelper.GenParams("Id", id);

            var result = await _generic.ReadCustomAsync<CustomerDatatable>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadDefault")]
        public async Task<IActionResult> ReadDefault()
        {
            var result = await _generic.ReadByCustomAsync<Customer>(null, "Default");
            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Customer obj)
        {
            // => Check phone is existing ?
            // => If existing, not allow create
            {
                var phoneIsExisting = await _CheckPhoneIsExisting(id: obj.Id, phone: obj.Phone);

                if (phoneIsExisting.code == EResponse.Error)
                    return BadRequest(phoneIsExisting.message);

                if (phoneIsExisting.code == EResponse.IsExisting)
                    return BadRequest("Existing phone number!");
            }

            // => Check identity is existing ?
            // => If existing, not allow create
            {
                var identityIsExisting = await _CheckIdentityIsExisting(id: obj.Id, identity: obj.IdentityCode);

                if (identityIsExisting.code == EResponse.Error)
                    return BadRequest(identityIsExisting.message);

                if (identityIsExisting.code == EResponse.IsExisting)
                    return BadRequest("Existing identity!");
            }

            // => Check customer group is available ?
            // => If Unavailable, not allow create
            {
                var customerGroupAvailable = await _CheckCustomerGroupAvailable(customerGroupId: obj.CustomerGroupId);

                if (customerGroupAvailable.code == EResponse.Error)
                    return BadRequest(customerGroupAvailable.message);

                if (customerGroupAvailable.code == EResponse.Unavailable)
                    return BadRequest("Customer group is unavailable!");
            }



            // => Create Sql
            // obj.Tag = obj.Tag.NonUnicode(true);
            var result = await _generic.CreateAsync<Customer, CustomerRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            // => Update Redis default
            {
                if (obj.IsDefault)
                {
                    // => Get defaulting on sql
                    var sqlDefaulting = await _ReadDefaulting();
                    if (sqlDefaulting.response != null)
                    {
                        // => Get defaulting redis
                        var redisDefaulting = await _redis.GetAsync(sqlDefaulting.response);

                        if (redisDefaulting != null && redisDefaulting != null)
                        {
                            redisDefaulting.IsDefault = false;
                            await _redis.SetAsync(redisDefaulting);
                        }
                    }
                }
            }

            // => Set Elastic
            var els = _mapper.Map<CustomerRedis, CustomerElastic>(result.Response);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("customer")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));

            // => Set Redis
            await _redis.SetAsync<CustomerRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Customer obj)
        {
            // => Check Phone update is existing
            // => If existing, not allow update Phone
            {
                var phoneIsExisting = await _CheckPhoneIsExisting(id: obj.Id, phone: obj.Phone);

                if (phoneIsExisting.code == EResponse.Error)
                    return BadRequest(phoneIsExisting.message);

                if (phoneIsExisting.code == EResponse.IsExisting)
                    return BadRequest("Existing phone number.");
            }


            // => Check Identity update is existing
            // => If existing, not allow update Phone
            {
                var identityIsExisting = await _CheckIdentityIsExisting(id: obj.Id, identity: obj.IdentityCode);

                if (identityIsExisting.code == EResponse.Error)
                    return BadRequest(identityIsExisting.message);

                if (identityIsExisting.code == EResponse.IsExisting)
                    return BadRequest("Existing personal identity.");
            }

            // => Check customer group is available ?
            // => If Unavailable, not allow update
            {
                var customerGroupAvailable = await _CheckCustomerGroupAvailable(customerGroupId: obj.CustomerGroupId);

                if (customerGroupAvailable.code == EResponse.Error)
                    return BadRequest(customerGroupAvailable.message);

                if (customerGroupAvailable.code == EResponse.Unavailable)
                    return BadRequest("Customer group is unavailable !");
            }

            // => Update sql
            var result = await _generic.UpdateAsync<Customer, CustomerRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            // => Check defaulting status
            {
                // => get current customer redis
                var keyCurrentCustomer = _redis.GenKey<CustomerRedis>(
                    new CustomerRedis
                    {
                        Id = result.Response.Id,
                        PaymentMethodId = result.Response.PaymentMethodId,
                        CustomerGroupId = result.Response.CustomerGroupId
                    });

                var currentCustomer = await _redis.GetAsync<CustomerRedis>(keyCurrentCustomer);

                // => if current is not defaulting
                // => want to set default 
                if (currentCustomer != null)
                {
                    if (!currentCustomer.IsDefault && obj.IsDefault)
                    {
                        // => find defaulting in redis and set not default
                        var keys = _redis.GenKey<CustomerRedis>(new CustomerRedis());
                        var redisResponse = await _redis.SearchKeysAsync<CustomerRedis>(keys);
                        var defaulting = redisResponse.FirstOrDefault(x => x.IsDefault);

                        if (defaulting != null)
                        {
                            // => Set defaulting is not default
                            defaulting.IsDefault = false;
                            await _redis.SetAsync<CustomerRedis>(defaulting);
                        }
                    }

                    // => if current is defaulting
                    // => want to set not default
                    if (currentCustomer.IsDefault && !obj.IsDefault)
                    {
                        // => Find new defaulting in sql
                        var newDefaulting = await _ReadDefaulting();

                        if (newDefaulting.success && newDefaulting.response != null && newDefaulting.response.Id > 0)
                        {
                            // => Find in redis
                            var keyDefaulting = _redis.GenKey<CustomerRedis>(new CustomerRedis { Id = newDefaulting.response.Id });
                            var defaultingRedis = await _redis.SearchKeysAsync<CustomerRedis>(keyDefaulting);

                            if (defaultingRedis.Any())
                            {
                                var defaulting = defaultingRedis.FirstOrDefault();
                                if (defaulting != null)
                                {
                                    // => change new defaulting redis
                                    defaulting.IsDefault = true;
                                    await _redis.SetAsync<CustomerRedis>(defaulting);
                                }
                            }
                        }
                    }
                }
            }

            // => Set Elastic
            var els = _mapper.Map<CustomerRedis, CustomerElastic>(result.Response);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("customer")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));

            // => Set Redis
            await _redis.SetAsync<CustomerRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            // => Check customer already purchasing
            // => If purchasing, not allow delete
            {
                var isPurchasing = await _CheckIsPurchasing(id: id);

                if (isPurchasing.code == EResponse.Error)
                    return BadRequest(isPurchasing.message);

                if (isPurchasing.code == EResponse.AlreadyPurchasing)
                    return BadRequest("Customer is purchasing, not allow delete !");
            }

            // => Delete sql
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Customer, int>(param);

            if (!result.Success && result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Elastic 
            _elasticClient.Delete<CustomerElastic>(id, del => del.Index("customer"));

            // => Set default in redis
            var newDefaulting = await _ReadDefaulting();
            await _redis.SetAsync<CustomerRedis>(newDefaulting.response);

            // => Remove Redis
            var key = _redis.GenKey<CustomerRedis>(new CustomerRedis { Id = id });
            var redisResponse = await _redis.SearchKeysAsync<CustomerRedis>(key);

            await _redis.RemoveAsync(redisResponse.ToList());

            return Ok(result.Response);
        }

        #endregion

        #region Check

        [HttpGet("CheckPhoneIsExisting")]
        public async Task<IActionResult> CheckPhoneIsExisting(int id, string phone)
        {
            var isExisting = await _CheckPhoneIsExisting(id: id, phone: phone);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        [HttpGet("CheckIdentityIsExisting")]
        public async Task<IActionResult> CheckIdentityIsUsing(int id, string identityCode)
        {
            var isExisting = await _CheckIdentityIsExisting(id: id, identity: identityCode);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        #endregion

        #region Methods

        private async Task<(EResponse code, string message)> _CheckIsPurchasing(int id)
        {
            return (EResponse.NotPurchasing, "");
        }

        private async Task<(EResponse code, string message)> _CheckPhoneIsExisting(int id, string phone)
        {
            var param = DataHelper.GenParams("Id", id, "Phone", phone);

            var result = await _generic.ReadByCustomAsync<Customer, int>(param, "PhoneIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        private async Task<(EResponse code, string message)> _CheckIdentityIsExisting(int id, string identity)
        {
            var param = DataHelper.GenParams("Id", id, "Identity", identity);

            var result = await _generic.ReadByCustomAsync<Customer, int>(param, "IdentityIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        private async Task<(EResponse code, string message)> _CheckCustomerGroupAvailable(int customerGroupId)
        {
            var param = DataHelper.GenParams("Id", customerGroupId);

            var result = await _generic.ReadByCustomAsync<CustomerGroup>(param, "ById");

            if (!result.Success && result.Response == null)
                return (EResponse.Error, result.Messages);

            if (result.Response.Status == 0)
                return (EResponse.Unavailable, "");

            return (EResponse.Available, "");
        }

        private async Task<(bool success, string message, int count)> _SyncRedis()
        {
            // => Delete Customer in redis
            var keyToDelete = _redis.GenKey<CustomerRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<CustomerRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<CustomerRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<CustomerRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            await _redis.SetAsync<CustomerRedis>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<(bool success, string message, int count)> _SyncElastic()
        {
            // => Delete Customer in elastic
            var result = await _elasticClient.DeleteByQueryAsync<CustomerElastic>(del =>
                del.Index("customer").Query(q => q.QueryString(qs => qs.Query("*"))));

            // => Sync Elastic
            var resultToSync = await _generic.ReadAsync<CustomerElastic>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            foreach (var els in resultToSync.Response)
            {
                await _elasticClient
                    .IndexAsync(els, i => i.Index("customer")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));
            }

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<(bool success, string message, CustomerRedis response)> _ReadDefaulting()
        {
            var result = await _generic.ReadByCustomAsync<CustomerRedis>(null, "Defaulting");

            return (result.Success, result.Messages, result.Response);
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

        #region Elastic

        // Delete Elastic
        [HttpDelete("DeleteElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteElastic()
        {
            var result = await _elasticClient.Indices.DeleteAsync("customer");

            return Ok(result.IsValid);
        }

        // Create Elastic
        [HttpPost("CreateElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateElastic()
        {
            var success = false;

            // Customer
            if (!_elasticClient.Indices.Exists("customer").Exists)
            {
                var result = await _elasticClient.Indices.CreateAsync("customer", index => index.Map<CustomerElastic>(x => x.AutoMap()));
                success = result.IsValid;
            }

            return Ok(success);
        }

        #endregion

        #region Search
        [HttpGet("Search")]
        public IActionResult Search(string keyword, int pageSize = 50, int pageNumber = 0)
        {
            var listResult = new CustomerSearch();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
            }

            pageNumber = (pageNumber == 0 ? pageNumber : pageNumber * pageSize);
            var responsedata = _elasticClient.Search<CustomerElastic>(s => s
                                                  .Index("customer")
                                                  .Query(q =>
                                                          (
                                                              q.QueryString(qs => qs.Fields("tag,tag.keyword").Query("*\\ " + keyword.Replace(" ", "\\ ") + "*"))
                                                          )
                                                       )
                                                  .From(pageNumber)
                                                  .Size(pageSize)
                                                  .Sort(ss => ss.Ascending(p => p.Name.Suffix("keyword")))
                                              );

            listResult.Items = responsedata.Documents.ToList();
            listResult.Total = responsedata.Total;
            return Ok(listResult);
        }
        #endregion
    }
}
