using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PrivacyPolicyController : ControllerBase
    {
        private readonly IGenericRepository _generic;
        private readonly IRedisRepositoty _redisRepositoty;
        private readonly IAdapterPattern _adapter;

        public PrivacyPolicyController(IGenericRepository generic, IRedisRepositoty redisRepositoty, IAdapterPattern adapter)
        {
            _generic = generic;
            _redisRepositoty = redisRepositoty;
            _adapter = adapter;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<PrivacyPolicy>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadByIdLang")]
        public async Task<IActionResult> ReadByIdLang(int id, string languageCode)
        {
            var param = DataHelper.GenParams("Id", id, "LanguageCode", languageCode);

            var result = await _generic.ReadByCustomAsync<PrivacyPolicy>(param, "ByIdLang");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadTransByIdLang")]
        public async Task<IActionResult> ReadTransByIdLang(int id, string languageCode)
        {
            var param = DataHelper.GenParams("Id", id, "LanguageCode", languageCode);

            var result = await _generic.ReadByCustomAsync<PrivacyPolicyTrans>(param, "ByIdLang");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadTypeById")]
        public async Task<IActionResult> ReadTypeById(int privacyPolicyId)
        {
            var param = DataHelper.GenParams("PrivacyPolicyId", privacyPolicyId);

            var result = await _generic.ReadCustomAsync<PrivacyPolicyType>(param, "ById");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PrivacyPolicy obj)
        {
            var result = await _generic.CreateAsync<PrivacyPolicy, PrivacyPolicyRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // create type
            if (obj.UserTypes.Any())
            {
                foreach (var type in obj.UserTypes)
                {
                    var result2 = await _generic.CreateAsync<PrivacyPolicyType, PrivacyPolicyTypeRedis>(new PrivacyPolicyType { PrivacyPolicyId = result.Response.Id, UserType = type });

                    if (!result2.Success && result2.Response.PrivacyPolicyId == 0)
                        return BadRequest(result2.Messages);

                    // redis
                    await _redisRepositoty.SetAsync<PrivacyPolicyTypeRedis>(result2.Response);
                }
            }

            await _redisRepositoty.SetAsync<PrivacyPolicyRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        //[HttpPost("CreateType")]
        //public async Task<IActionResult> CreateType(PrivacyPolicyType obj)
        //{
        //    try
        //    {
        //        var result = (await _adapter.QuerySingle<PrivacyPolicy, PrivacyPolicyRedis>(obj, DataHelper.ApiCRUD.Create, ignorePara: "Count")).FlushData();
        //        if (result.Success && result.Response != null)
        //        {
        //            await _redisRepositoty.SetAsync<PrivacyPolicyRedis>(result.Response);
        //            return Ok(result.Response.Id);
        //        }
        //        else return BadRequest(result.Messages);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PrivacyPolicy obj)
        {
            var result = (await _adapter.Query<PrivacyPolicy, PrivacyPolicyRedis>(obj, DataHelper.ApiCRUD.Update, ignorePara: "Count")).FlushData();

            if(!result.Success)
                return BadRequest(result.Messages);

            if (obj.UserTypes.Any())
            {
                var resultType = await _generic.DeteteAsync<PrivacyPolicyType, int>(DataHelper.GenParams("PrivacyPolicyId", obj.Id));

                if (resultType.Response <= 0)
                    return BadRequest(resultType.Messages);

                // delete on redis
                var redisSearch = await _redisRepositoty.SearchKeysAsync<PrivacyPolicyTypeRedis>($"ads:privacypolicytyperedis:privacypolicyid:{obj.Id}:*");
                await _redisRepositoty.RemoveAsync<PrivacyPolicyTypeRedis>(redisSearch.ToList());

                foreach (var type in obj.UserTypes)
                {
                    var result2 = await _generic.CreateAsync<PrivacyPolicyType, PrivacyPolicyTypeRedis>(new PrivacyPolicyType { PrivacyPolicyId = obj.Id, UserType = type });

                    if (!result2.Success && result2.Response.PrivacyPolicyId == 0)
                        return BadRequest(result2.Messages);

                    // redis
                    await _redisRepositoty.SetAsync<PrivacyPolicyTypeRedis>(result2.Response);
                }
            }

            foreach (var item in result.Response)
            {
                await _redisRepositoty.SetAsync<PrivacyPolicyRedis>(item);
            }

            return Ok(result.Response.Count());

        }

        [HttpPut("UpdateTrans")]
        public async Task<IActionResult> UpdateTrans(PrivacyPolicyTrans obj)
        {
            var result = (await _adapter.QuerySingle<PrivacyPolicyTrans, PrivacyPolicyRedis>(obj, DataHelper.ApiCRUD.Update, ignorePara: "Count")).FlushData();
            
            if (!result.Success)
                return BadRequest(result.Messages);

            await _redisRepositoty.SetAsync<PrivacyPolicyRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _generic.DeteteAsync<PrivacyPolicy, int>(DataHelper.GenParams("Id", id));

            if (!result.Success && result.Response <= 0)
                return BadRequest(result.Messages);

            var redisResponse = await _redisRepositoty.SearchKeysAsync<PrivacyPolicyRedis>("ads:probenefitredis:id:" + id + "*");
            var redisTypeResponse = await _redisRepositoty.SearchKeysAsync<PrivacyPolicyTypeRedis>("ads:privacypolicytyperedis:privacypolicyid:{obj.Id}:*");
            await _redisRepositoty.RemoveAsync<PrivacyPolicyRedis>(redisResponse.ToList());
            await _redisRepositoty.RemoveAsync<PrivacyPolicyTypeRedis>(redisTypeResponse.ToList());

            return Ok(result.Response);
        }

        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _generic.ReadAsync<PrivacyPolicyRedis>();

            if (!result.Success)
                return BadRequest(result.Messages);

            foreach(var item in result.Response)
            {
                await _redisRepositoty.SetAsync<PrivacyPolicyRedis>(item);
            }

            var resultType = await _generic.ReadAsync<PrivacyPolicyTypeRedis>();

            if (!resultType.Success)
                return BadRequest(resultType.Messages);

            foreach (var item in resultType.Response)
            {
                await _redisRepositoty.SetAsync<PrivacyPolicyTypeRedis>(item);
            }

            return Ok(result.Response.Count());
        }

        //[HttpDelete("DeleteType")]
        //public async Task<IActionResult> DeleteType(int id)
        //{
        //    var param = DataHelper.GenParams("Id", id);
        //    var result = await _generic.DeteteAsync<PrivacyPolicy, int>(param);
        //    if (result.Success && result.Response > 0)
        //    {
        //        var PrivacyPolicyRedis = await _redisRepositoty.SearchKeysAsync<PrivacyPolicyRedis>("ads:probenefitredis:id:" + id + "*");
        //        await _redisRepositoty.RemoveAsync<PrivacyPolicyRedis>(PrivacyPolicyRedis.ToList());

        //        return Ok(result.Response);
        //    }
        //    else return BadRequest(result.Messages);
        //}
    }
}