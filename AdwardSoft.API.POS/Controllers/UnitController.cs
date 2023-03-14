using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UnitController : ControllerBase
    {
        #region Constructor

        private readonly IRedisRepositoty _redis;
        private readonly IGenericRepository _generic;
        private readonly IAdapterPattern _adapter;

        public UnitController(
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
        [AllowAnonymous]
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Unit>();

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadSelect()
        {
            var result = await _generic.ReadCustomAsync<Select>(null, "Unit");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelectByProduct")]
        public async Task<IActionResult> ReadSelectByProduct(int productId)
        {
            //usp_Select_ReadUnitByProduct
            var param = DataHelper.GenParams("ProductId", productId);
            var result = await _generic.ReadCustomAsync<Select>(param, "UnitByProduct");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadSelectByExcludeProduct")]
        public async Task<IActionResult> ReadSelectByExcludeProduct(int productId)
        {
            //usp_Select_ReadUnitByExcludeProduct
            var param = DataHelper.GenParams("ProductId", productId);
            var result = await _generic.ReadCustomAsync<Select>(param, "UnitByExcludeProduct");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _ReadById(id: id);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(result.response);
        }

        #endregion

        #region Create

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(Unit obj)
        {
            // => Check code is existing
            var isCodeExisting = await _CheckCodeIsExisting(code: obj.Code);

            if (isCodeExisting.code == EResponse.Error)
                return BadRequest(isCodeExisting.message);

            if (isCodeExisting.code == EResponse.IsExisting)
                return BadRequest("Mã số đã tồn tại !");

            // => Create unit
            var result = await _generic.CreateAsync<Unit, UnitRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<UnitRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Update

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(Unit obj)
        {
            // => Check unit is using
            // => If using, not allow update Code
            var isUsing = await _CheckUnitIsUsing(id: obj.Id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
            {
                var existingUnit = await _ReadById(id: obj.Id);

                if (!existingUnit.success)
                    return BadRequest(existingUnit.message);

                if (existingUnit.response == null)
                    return BadRequest("Unit is not found !");

                if (existingUnit.response.Code != obj.Code)
                    return BadRequest("Unit is using, not allow update code !");
            }


            // => Check code is existing
            var isCodeExisting = await _CheckCodeIsExisting(id: obj.Id, code: obj.Code);

            if (isCodeExisting.code == EResponse.Error)
                return BadRequest(isCodeExisting.message);

            if (isCodeExisting.code == EResponse.IsExisting)
                return BadRequest(new { Message = "Mã số đã tồn tại !" });

            // => Update unit
            var result = await _generic.UpdateAsync<Unit, UnitRedis>(obj);

            if (!result.Success && result.Response == null)
                return BadRequest(result.Messages);

            // => Set Redis
            await _redis.SetAsync<UnitRedis>(result.Response);

            return Ok(result.Response.Id);
        }

        #endregion

        #region Delete

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // => Check unit is using
            var isUsing = await _CheckUnitIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
                return BadRequest("Unit is using, not allow remove !");

            // => Delete unit
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Unit, int>(param);

            if (!result.Success || result.Response <= 0)
                return BadRequest(result.Messages);

            // => Remove Redis
            var key = _redis.GenKey<UnitRedis>(new UnitRedis { Id = id });
            await _redis.RemoveAsync(key);

            return Ok(result.Response);
        }

        #endregion

        #region Check

        [HttpGet("CheckCodeIsExisting")]
        public async Task<IActionResult> CheckCodeIsExisting(int id, string code)
        {
            var isExisting = await _CheckCodeIsExisting(id: id, code: code);

            if (isExisting.code == EResponse.Error)
                return BadRequest(isExisting.message);

            return Ok((int)isExisting.code);
        }

        [HttpGet("CheckUpdateCodeIsUsing")]
        public async Task<IActionResult> CheckUpdateCodeIsUsing(int id, string code)
        {
            var isUsing = await _CheckUnitIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            if (isUsing.code == EResponse.IsUsing)
            {
                var existingUnit = await _ReadById(id);

                if (existingUnit.response.Code.TrimEnd() != code)
                    return Ok(1);
            }

            return Ok(0);
        }

        [HttpGet("CheckIsUsing")]
        public async Task<IActionResult> CheckIsUsing(int id)
        {
            var isUsing = await _CheckUnitIsUsing(id: id);

            if (isUsing.code == EResponse.Error)
                return BadRequest(isUsing.message);

            return Ok((int)isUsing.code);
        }

        #endregion

        #region Methods

        private async Task<(EResponse code, string message)> _CheckCodeIsExisting(int id = 0, string code = "")
        {
            var param = DataHelper.GenParams("Id", id, "Code", code);
            var result = await _generic.ReadByCustomAsync<Unit>(param, "CheckCodeIsExisting");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if(result.Response == null)
                return (EResponse.NotExisting, "");

            return (EResponse.IsExisting, "");
        }

        private async Task<(EResponse code, string message)> _CheckUnitIsUsing(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Unit, int>(param, "CheckUnitIsUsing");

            if (!result.Success)
                return (EResponse.Error, result.Messages);

            if (result.Response == 0)
                return (EResponse.NotUsing, "");

            return (EResponse.IsUsing, "");
        }

        private async Task<(bool success, string message, Unit response)> _ReadById(int id)
        {
            var param = DataHelper.GenParams("Id", id);
            var result = await _generic.ReadByCustomAsync<Unit>(param, "ById");

            return (result.Success, result.Messages, result.Response);
        }

        #endregion

        #region Sync

        [HttpGet("SyncRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncRedisAsync()
        {
            // => Delete
            var keyToDelete = _redis.GenKey<UnitRedis>();
            var redisToDelete = await _redis.SearchKeysAsync<UnitRedis>(keyToDelete + ":*");

            await _redis.RemoveAsync<UnitRedis>(redisToDelete.ToList());

            // => Sync redis
            var resultToSync = await _generic.ReadAsync<UnitRedis>();

            if (!resultToSync.Success && !resultToSync.Response.Any())
                return BadRequest(resultToSync.Messages);

            await _redis.SetAsync<UnitRedis>(resultToSync.Response.ToList());

            return Ok(resultToSync.Response.Count());
        }

        #endregion

    }
}
