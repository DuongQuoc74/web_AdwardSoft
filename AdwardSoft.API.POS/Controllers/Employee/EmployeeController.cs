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
    [Authorize]
    public class EmployeeController : Controller
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public EmployeeController(
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
        public async Task<IActionResult> Read(int pageSize, int pageSkip, string searchBy, int departmentId, int positionId)
        {
            var param = DataHelper.GenParams("searchBy", searchBy, "pageSize", pageSize, "PageSkip", pageSkip
                , "DepartmentId", departmentId, "PositionId", positionId);
            var result = await _generic.ReadCustomAsync<EmployeeDataTable>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<Employee>(id);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect(int branchId)
        {
            var result = await _generic.ReadCustomAsync<Select>(DataHelper.GenParams("BranchId", branchId), "Employee");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelectUser")]
        public async Task<IActionResult> ReadSelectUser(int branchId)
        {
            var result = await _generic.ReadCustomAsync<Select>(DataHelper.GenParams("BranchId", branchId), "EmployeeUser");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Employee obj)
        {
            var result = await _generic.CreateAsync<Employee, EmployeeRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            // => Set Elastic
            await _SetElastic(result.Response);

            //redis
            await _redis.SetAsync<EmployeeRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(Employee obj)
        {
            // => Update unit
            var result = await _generic.UpdateAsync<Employee, EmployeeRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            // => Set Elastic
            await _SetElastic(result.Response);

            // => Set Redis
            await _redis.SetAsync<EmployeeRedis>(result.Response);

            return Ok(result.Response.Id);
        }
        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Delete Employee
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Employee, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            // => Delete Elastic
            _elasticClient.Delete<EmployeeElastic>(id, del => del.Index("Employee"));

            // => Remove Redis
            var key = _redis.GenKey<EmployeeRedis>(new EmployeeRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }

        #endregion

        #region Methods
        private async Task<(bool success, string message, int count)> _SyncRedis()
        {
            // => Delete Customer in redis
            var keyToDelete = _redis.GenKey<EmployeeRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<EmployeeRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<EmployeeRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<EmployeeRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            await _redis.SetAsync<EmployeeRedis>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<(bool success, string message, int count)> _SyncElastic()
        {
            // => Delete Customer in elastic
            var result = await _elasticClient.DeleteByQueryAsync<EmployeeElastic>(del =>
                del.Index("employee").Query(q => q.QueryString(qs => qs.Query("*"))));

            // => Sync Elastic
            var resultToSync = await _generic.ReadAsync<EmployeeElastic>();

            if (!resultToSync.Success || !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);

            foreach (var els in resultToSync.Response)
            {
                await _elasticClient
                    .IndexAsync(els, i => i.Index("employee")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));
            }

            return (true, "", resultToSync.Response.Count());
        }

        private async Task _SetElastic(EmployeeRedis obj)
        {
            var els = _mapper.Map<EmployeeRedis, EmployeeElastic>(obj);

            await _elasticClient
                    .IndexAsync(els, i => i.Index("employee")
                    .Id(els.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));
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
            var result = await _elasticClient.Indices.DeleteAsync("employee");

            return Ok(result.IsValid);
        }

        // Sync Elastic
        [HttpPost("CreateElastic")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateElastic()
        {
            var success = false;

            // Customer
            if (!_elasticClient.Indices.Exists("employee").Exists)
            {
                var result = await _elasticClient.Indices.CreateAsync("employee", index => index.Map<EmployeeElastic>(x => x.AutoMap()));
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
        [HttpGet("Search")]
        public IActionResult Search(string keyword, int pageSize = 50, int pageNumber = 0)
        {
            var listResult = new EmployeeSearch();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
            }

            pageNumber = (pageNumber == 0 ? pageNumber : pageNumber * pageSize);
            var responsedata = _elasticClient.Search<EmployeeElastic>(s => s
                                                  .Index("employee")
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
