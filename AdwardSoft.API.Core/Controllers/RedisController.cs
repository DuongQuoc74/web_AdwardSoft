using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisRepositoty _redisRepositoty;
        public RedisController(IRedisRepositoty redisRepositoty)
        {
            _redisRepositoty = redisRepositoty;
        }

        //[HttpGet("Read")]
        //public async Task<IActionResult> Read()
        //{
        //    await _redisRepositoty.SetStringAsync("ADS", "AdwardSoft");
        //    var log = await _redisRepositoty.GetStringAsync("ADS");

        //    return Ok(log);
        //}
    }
}