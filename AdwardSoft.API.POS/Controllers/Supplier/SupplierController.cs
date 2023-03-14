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
    [Authorize]
    public class SupplierController : ControllerBase
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redis;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public SupplierController(IGenericRepository genericRepository, IRedisRepositoty redis, IElasticClient elasticClient,
            IMapper mapper)
        {

            _genericRepository = genericRepository;
            _redis = redis;
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        #endregion

        #region Read
        [AllowAnonymous]
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _genericRepository.ReadAsync<Supplier>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [AllowAnonymous]
        [HttpGet("ReadPOS")]
        public async Task<IActionResult> ReadPOS()
        {
            var result = await _genericRepository.ReadAsync<SupplierPOS>();

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            if(result.Response != null)
            {
                var suppliers = new List<SupplierInfo>();
                suppliers = result.Response.Where(x => x.Id > 0).GroupBy(x => x.Id)
                            .Select(c => c.FirstOrDefault()).ToList()
                            .Select(x => new SupplierInfo() {
                                Id = x.Id, Name = x.Name, Address = x.Address, Email = x.Email,
                                IsDefault = x.IsDefault, Contacts = 
                                (result.Response.Where(s => s.SupplierId == x.Id).Select(c => new SupplierContact() { 
                                    SupplierId = c.SupplierId, Idx = c.Idx, Name = c.Name, Note = c.Note, Phone = c.Phone, Position = c.Position
                                })).ToList()
                            }).ToList();
                return Ok(suppliers);
            }
            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);

            var result = await _genericRepository.ReadByCustomAsync<Supplier>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _genericRepository.ReadCustomAsync<Supplier>(null, "Select");
            var select = new List<Select>();

            if (!result.Success)
                return BadRequest(result.Messages);
            
            if (result.Response.Any())
                select = result.Response.Select(x => new Select { Id = x.Id, Text = x.Name }).ToList();

            return Ok(select);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable(int pageSize, int pageSkip, string searchBy)
        {
            var param = DataHelper.GenParams("searchBy", searchBy,"pageSize",pageSize, "pageSkip", pageSkip);
            var result = await _genericRepository.ReadCustomAsync<SupplierDatatable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Supplier obj)
        {
            var result = await _genericRepository.CreateAsync<Supplier, SupplierRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            //elastic
            await _SyncElasticById(result.Response.Id);

            //redis
            await _redis.SetAsync<SupplierRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Supplier obj)
        {
            var result = await _genericRepository.UpdateAsync<Supplier, SupplierRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // Update Elastic
            await _SyncElasticById(result.Response.Id);

            // Update redis
            await _redis.SetAsync<SupplierRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {         
            //Delete supplier sql
            var parms = DataHelper.GenParams("Id", id);
            var result = await _genericRepository.DeteteAsync<Supplier, int>(parms);

            if (result.Success && result.Response > 0)
            //    return BadRequest(result.Messages);
            {
               // Delete Elastic
                _elasticClient.Delete<SupplierElastic>(id, del => del.Index("supplier"));

                //Delete supplier redis
                var key = _redis.GenKey<SupplierRedis>(new SupplierRedis { Id = id });
                await _redis.RemoveAsync(key);
            }

            return Ok(result.Response);

        }

        #endregion

        #region Check

        [HttpGet("CheckPhoneIsExisting")]
        public async Task<IActionResult> CheckPhoneIsExisting(int id, string tel)
        {
            var isExisting = await _CheckPhoneIsExisting(id: id, tel: tel);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        [HttpGet("CheckSupplierIsUsing")]
        public async Task<IActionResult> CheckIdentityIsUsing(int id)
        {
            var isExisting = await _CheckSupplierIsUsing(id: id);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        [HttpGet("CheckEmailIsExisting")]
        public async Task<IActionResult> CheckEmailIsExisting(int id, string email)
        {
            var isExisting = await _CheckEmailIsExisting(id: id, email: email);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        #endregion

        #region Methods

        private async Task<(EResponse code, string massage)> CheckSupplierIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _genericRepository.ReadByCustomAsync<Supplier, int>(param, "SupplierIsUsing");

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

        private async Task<(EResponse code, string message)> _CheckPhoneIsExisting(int id, string tel)
        {
            var param = DataHelper.GenParams("Id", id, "Tel", tel);

            var result = await _genericRepository.ReadByCustomAsync<Supplier, int>(param, "PhoneIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        private async Task<(EResponse code, string message)> _CheckSupplierIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);

            var result = await _genericRepository.ReadByCustomAsync<Supplier, int>(param, "CheckSupplierIsUsing");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        private async Task<(EResponse code, string message)> _CheckEmailIsExisting(int id, string email)
        {
            var param = DataHelper.GenParams("Id", id, "Email", email);

            var result = await _genericRepository.ReadByCustomAsync<Supplier, int>(param, "EmailIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response > 0)
                return (EResponse.IsExisting, "");

            return (EResponse.NotExisting, "");
        }

        #endregion

        #region Methods elastic

        private async Task<(bool success, string message, int count)> _SyncRedis()
        {
            // => Delete Customer in redis
            var keyToDelete = _redis.GenKey<SupplierRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<SupplierRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<SupplierRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _genericRepository.ReadAsync<SupplierRedis>();

            if (!resultToSync.Success || !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            await _redis.SetAsync<SupplierRedis>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<bool> _SyncElasticById(int id)
        {

            // => Sync Elastic
            var param = DataHelper.GenParams("Id", id);
            var resultToSync = await _genericRepository.ReadByCustomAsync<SupplierElastic>(param, "ById");

            if (!resultToSync.Success || resultToSync.Response == null || resultToSync.Response.Id == 0)
                return false;

            await _elasticClient
                .IndexAsync(resultToSync.Response, i => i.Index("supplier")
                .Id(resultToSync.Response.Id)
                .Refresh(Elasticsearch.Net.Refresh.True));

            return true;
        }

        private async Task<(bool success, string message, int count)> _SyncElastic()
        {
            // => Delete Customer in elastic
            var result = await _elasticClient.DeleteByQueryAsync<SupplierElastic>(del =>
                del.Index("supplier").Query(q => q.QueryString(qs => qs.Query("*"))));

            // => Sync Elastic
            var resultToSync = await _genericRepository.ReadAsync<SupplierElastic>();

            if (!resultToSync.Success || !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            foreach (var els in resultToSync.Response)
            {
                await _elasticClient
                    .IndexAsync(els, i => i.Index("supplier")
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
            var result = await _elasticClient.Indices.DeleteAsync("supplier");

            return Ok(result.IsValid);
        }

        // Sync Elastic
        [HttpPost("CreateElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateElastic()
        {
            var success = false;

            // Customer
            if (!_elasticClient.Indices.Exists("supplier").Exists)
            {
                var result = await _elasticClient.Indices.CreateAsync("supplier", index => index.Map<SupplierElastic>(x => x.AutoMap()));
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
    }
}
