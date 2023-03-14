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
    public class BankAccountController : ControllerBase
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;
        private readonly IRedisRepositoty _redis;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public BankAccountController(IGenericRepository genericRepository, IRedisRepositoty redis,
             IElasticClient elasticClient,
            IMapper mapper)
        {
            _genericRepository = genericRepository;
            _redis = redis;
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        #endregion

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _genericRepository.ReadAsync<BankAccount>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);

        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);

            var result = await _genericRepository.ReadByCustomAsync<BankAccount>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadByBankNo")]
        public async Task<IActionResult> ReadByBankNo(int id, string bankNo)
        {
            var param = DataHelper.GenParams("Id", id, "BankNo", bankNo);

            var result = await _genericRepository.ReadByCustomAsync<BankAccount>(param, "ByBankNo");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _genericRepository.ReadCustomAsync<Select>(null, "BankAccount");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);

        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> Create(BankAccount obj)
        {
            var result = await _genericRepository.CreateAsync<BankAccount, BankAccountRedis>(obj);

            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            //redis
            await _redis.SetAsync<BankAccountRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> Update(BankAccount obj)
        {
            var result = await _genericRepository.UpdateAsync<BankAccount, BankAccountRedis>(obj);
            if (!result.Success || result.Response == null)
                return BadRequest(result.Messages);

            //redis
            await _redis.SetAsync<BankAccountRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _genericRepository.DeteteAsync<BankAccount, int>(param);

            if (!result.Success || result.Response < 0)
                return BadRequest(result.Messages);

            // => Remove BankAccount
            var key = _redis.GenKey<BankAccountRedis>(new BankAccountRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);

        }

        #endregion
    }
}
