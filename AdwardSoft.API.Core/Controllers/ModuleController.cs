using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ModuleController : Controller
    {
        private IGenericRepository _generic;
        private IAdapterPattern _adapter;
        private readonly IRedisRepositoty _redisRepositoty;

        public ModuleController(IGenericRepository generic, IAdapterPattern adapter, IRedisRepositoty redisRepositoty)
        {
            _generic = generic;
            _adapter = adapter; 
            _redisRepositoty = redisRepositoty;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<Module>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<Module>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadSelect")]
        public async Task<IActionResult> ReadBySelect()
        {
            var param = DataHelper.GenParams();
            var result = await _generic.ReadCustomAsync<SelectLevels>(param, "Module");

            if (!result.Success)
                return BadRequest(result.Response);

            return Ok(result.Response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Module obj)
        {
            var result = (await _adapter.QuerySingle<Module, Module>(obj, DataHelper.ApiCRUD.Create, ignorePara: "Types")).FlushData();
            if (result.Success)
            {
                if (!obj.IsPublic)
                {
                    List<ModuleType> moduleTypes = new List<ModuleType>();
                    obj.Types.Split(",").Where(x => x != string.Empty).Select(short.Parse).ToList().ForEach(x => moduleTypes.Add(new ModuleType() { ModuleId = result.Response.Id, Type = x }));
                    var resultModuleType = await _generic.CreateMultipleAsync(moduleTypes);
                    if (!resultModuleType.Success) return BadRequest(resultModuleType.Messages);
                    result.Response.Types = obj.Types;
                }
                else result.Response.Types = "";
                
                await _redisRepositoty.SetAsync<Module>(result.Response);
                return Ok(result.Response.Id);
            }
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Module obj)
        {
            var result = (await _adapter.QuerySingle<Module, Module>(obj, DataHelper.ApiCRUD.Update, ignorePara: "Types")).FlushData();
            if (result.Success)
            {
                if (!obj.IsPublic)
                {
                    List<ModuleType> moduleTypes = new List<ModuleType>();
                    obj.Types.Split(",").Where(x => x != string.Empty).Select(short.Parse).ToList().ForEach(x => moduleTypes.Add(new ModuleType() { ModuleId = result.Response.Id, Type = x }));
                    var resultModuleType = await _generic.CreateMultipleAsync(moduleTypes);
                    if (!resultModuleType.Success) return BadRequest(resultModuleType.Messages);
                    result.Response.Types = obj.Types;
                }
                else result.Response.Types = "";

                await _redisRepositoty.SetAsync<Module>(obj);
                return Ok(result.Response);
            }
            else return BadRequest(result.Messages);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var parms = DataHelper.GenParams("Id", id);
            var result = await _generic.DeteteAsync<Module, int>(parms);
            if (result.Success) 
            {
                var moduleRedis = await _redisRepositoty.SearchKeysAsync<Module>("ads:module:id:" + id);
                await _redisRepositoty.RemoveAsync<Module>(moduleRedis.ToList());
                return Ok(result.Response); 
            }
            else return BadRequest(result.Messages);
        }

        [HttpPost("Sort")]
        public async Task<IActionResult> Sort(ModuleSort json)
        {
            var result = (await _adapter.Query<ModuleSort, Module>(json, DataHelper.ApiCRUD.Update)).FlushData();
            //var result = await _generic.UpdateAsync<ModuleSort>(json);
            if (result.Success)
            {
                foreach (var item in result.Response)
                {
                    await _redisRepositoty.SetAsync<Module>(item);
                }
                
                return Ok(1);
            }
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadByUser")]
        public async Task<IActionResult> ReadByUser(long UserId)
        {
            var result = await _generic.ReadCustomAsync<Module>(DataHelper.GenParams("UserId", UserId), "ByUser");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);

        }
    }
}