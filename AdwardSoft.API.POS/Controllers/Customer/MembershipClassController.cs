using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembershipClassController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;

        public MembershipClassController(
            IRedisRepositoty redis,
            IGenericRepository generic)
        {
            _redis = redis;
            _generic = generic;
        }

        #endregion

        #region Read

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<MembershipClass>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadAsync<MembershipClass>();
            var select = new List<Select>();

            if (!result.Success)
                return BadRequest(result.Messages);

            if (result.Response.Any())
                select = result.Response.Select(x => new Select { Id = x.Id, Text = x.Name }).ToList();

            return Ok(select);
        }

        [HttpGet("ReadSelectDefault")]
        public async Task<IActionResult> ReadSelectDefault()
        {
            var result = await _generic.ReadAsync<MembershipClass>();
            var select = new List<SelectDefault>();

            if (!result.Success)
                return BadRequest(result.Messages);

            if (result.Response.Any())
                select = result.Response.Select(x => new SelectDefault { Id = x.Id, Text = x.Name, IsDefault = x.IsDefault }).ToList();

            return Ok(select);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _ReadById(id: id);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.response);
        }

        [HttpGet("ReadDatatable")]
        public async Task<IActionResult> ReadDatatable()
        {
            var result = await _generic.ReadAsync<MembershipClassDatatable>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(MembershipClass obj)
        {
            // => Check range
            var checkValue = _ValidateValue(lowestValue: obj.LowestValue, highestValue: obj.HighestValue);

            if(checkValue.code == EResponse.InRange)
                return BadRequest(checkValue.message);

            // => Check highest price and lower price in range
            // => If in range, not allow create
            var isInRange = await _CheckInRange
            (
                id: obj.Id,
                lowestValue: obj.LowestValue, 
                highestValue: obj.HighestValue
            );

            if (isInRange.code == EResponse.Error)
                return BadRequest(isInRange.message);

            if (isInRange.code == EResponse.InRange)
                return BadRequest("The value must out range !");


            // => Create Sql
            var result = await _generic.CreateAsync<MembershipClass, MembershipClassRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<MembershipClassRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(MembershipClass obj)
        {
            // => Check Membership class is using by Product
            // => If using, not allow update name
            var isUsing = await _CheckIsUsing(id: obj.Id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
            {
                var existingMembershipClass = await _generic.ReadByCustomAsync<MembershipClass>(DataHelper.GenParams("Id", obj.Id), "ById");

                if (!existingMembershipClass.Success)
                    return BadRequest(existingMembershipClass.Messages);

                if (existingMembershipClass.Response == null)
                    return BadRequest("Membership class is not found !");

                if (existingMembershipClass.Response.Name != obj.Name)
                    return BadRequest("Membership class is using, not allow update name !");
            }

            // => Check range
            var checkValue = _ValidateValue(lowestValue: obj.LowestValue, highestValue: obj.HighestValue);

            if (checkValue.code == EResponse.InRange)
                return BadRequest(checkValue.message);

            // => Check highest price and lower price in range
            // => If in range, not allow create
            var isInRange = await _CheckInRange
            (
                id: obj.Id,
                lowestValue: obj.LowestValue, 
                highestValue: obj.HighestValue
            );

            if (isInRange.code == EResponse.Error)
                return BadRequest(isInRange.message);
                
            if (isInRange.code == EResponse.InRange)
                return BadRequest(isInRange.message);

            // => Update sql
            var result = await _generic.UpdateAsync<MembershipClass, MembershipClassRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);


            // => Set Redis
            await _redis.SetAsync<MembershipClassRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        [HttpPut("Update/Level")]
        public async Task<IActionResult> UpdateLevel(MembershipClassSort obj)
        {
            // => Update Level
            var result = await _generic.UpdateAsync(obj);

            if (!result.Success)
                return BadRequest(result.Messages);

            // => Sync Redis
            var redisResult = await _SyncRedis();

            if (!redisResult.success)
                return BadRequest(redisResult.message);

            return Ok(result.Response);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Check is using 
            // => If using, not allow remove
            var isUsing = await _CheckIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
                return BadRequest(isUsing.message);

            // => Delete sql
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<MembershipClass, int>(param);

            if (!result.Success && result.Response <= 0)
                return BadRequest(result.Messages);

            //
            // Redis
            //

            // => Check is default
            // => If defaulting, set new default in sql
            var key = _redis.GenKey<MembershipClassRedis>(new MembershipClassRedis { Id = id });
            var redisResponse = await _redis.SearchKeysAsync<MembershipClassRedis>(key);

            if (redisResponse.Any())
            {
                var membershipRedis = redisResponse.FirstOrDefault();
                if(membershipRedis != null && membershipRedis.IsDefault)
                {
                    // => Find new defaulting in sql
                    var newDefaulting = await _generic.ReadByCustomAsync<MembershipClassRedis>(
                        null,
                        "Default"
                    );

                    if (newDefaulting.Success && newDefaulting.Response != null && newDefaulting.Response.Id > 0)
                    {
                        // => Find in redis
                        var keyDefaulting = _redis.GenKey<MembershipClassRedis>(new MembershipClassRedis { Id = newDefaulting.Response.Id });
                        var newDefaultingRedis = await _redis.SearchKeysAsync<MembershipClassRedis>(keyDefaulting);

                        if (newDefaultingRedis.Any())
                        {
                            var defaulting = newDefaultingRedis.FirstOrDefault();
                            if (defaulting != null)
                            {
                                // => change new defaulting redis
                                defaulting.IsDefault = true;
                                await _redis.SetAsync<MembershipClassRedis>(defaulting);
                            }
                            else
                            {
                                // => add new defaulting redis
                                await _redis.SetAsync<MembershipClassRedis>(newDefaulting.Response);
                            }
                        }
                    }
                }
            }

            // => Remove Redis
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }

        #endregion

        #region Check

        [HttpGet("CheckIsUsing")]
        public async Task<IActionResult> CheckUsing(int id, string code)
        {
            var isUsing = await _CheckIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            return Ok((int)isUsing.code);
        }

        [HttpGet("CheckUpdateNameIsUsing")]
        public async Task<IActionResult> CheckUpdateNameIsUsing(int id , string name)
        {
            // => Check Membership class is using by Product
            // => If using, not allow update name
            var isUsing = await _CheckIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.NotUsing)
                return Ok(0);

            var existingMembershipClass = await _ReadById(id: id);

            if (!existingMembershipClass.success)
                return BadRequest(existingMembershipClass.message);

            if (existingMembershipClass.response == null)
                return BadRequest("Membership class is not found !");

            if (existingMembershipClass.response.Name != name)
                return Ok(1);

            return Ok(0);
        }

        [HttpGet("CheckInRange")]
        public async Task<IActionResult> CheckInRange(int id, decimal lowestValue, decimal highestValue)
        {
            // => Check in range
            var inRange = await _CheckInRange(id, lowestValue, highestValue);

            if (inRange.code == EResponse.Error)
                return BadRequest(inRange.message);

            return Ok((int)inRange.code);
        }

        [HttpGet("CheckRange")]
        public IActionResult CheckRange(decimal lowestValue, decimal highestValue)
        {
            var checkValue = _ValidateValue(lowestValue, highestValue);

            return Ok(checkValue.message);
        }

        #endregion

        #region Methods

        private async Task<(EResponse code, string message)> _CheckIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<MembershipClass, int>(param, "CheckIsUsing");

            if (!result.Success)
                return (EResponse.Error, result.Messages);
                
            if (result.Response == 1)
                return (EResponse.IsUsing, "Membership class is using, not allow remove !");

            return (EResponse.NotUsing, "");
        }

        private async Task<(EResponse code, string message)> _CheckInRange(int id, decimal lowestValue, decimal highestValue)
        {
            var param = DataHelper.GenParams("Id", id, "LowestValue", lowestValue, "HighestValue", highestValue);
            var result = await _generic.ReadByCustomAsync<MembershipClass, int>(param, "CheckInRange");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response == 1)
                return (EResponse.InRange, "The value must out range !");


            return (EResponse.NotInRange, "");
        }

        private (EResponse code, string message) _ValidateValue(decimal lowestValue, decimal highestValue)
        {
            if (highestValue == 0)
                return (EResponse.NotInRange, "");
            // => Range
            if (lowestValue > highestValue)
                return (EResponse.InRange, "The lowest value can't greater than the highest value !!");

            if (highestValue < lowestValue)
                return (EResponse.InRange, "The highest value can't less than the lowest value !!");

            return (EResponse.NotInRange, "");
        }

        private async Task<(bool success, string message, int count)> _SyncRedis()
        {
            // => Delete Membership Class in redis
            var keyToDelete = _redis.GenKey<MembershipClassRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<MembershipClassRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<MembershipClassRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<MembershipClassRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return (false, resultToSync.Messages, 0);


            await _redis.SetAsync<MembershipClassRedis>(resultToSync.Response.ToList());

            return (true, "", resultToSync.Response.Count());
        }

        private async Task<(bool success, string message, MembershipClass response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<MembershipClass>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }

        #endregion

        #region Sync

        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedis()
        {
            var result = await _SyncRedis();

            if(!result.success)
                return BadRequest(result.message);

            return Ok(result.count);
        }

        #endregion
    }
}
