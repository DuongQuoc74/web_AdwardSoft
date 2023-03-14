using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Generic;
using AdwardSoft.DTO.Presentation.POS;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Utilities.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace AdwardSoft.API.POS.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : Controller
    {
        #region Constructor

        private readonly IGenericRepository _genericRepository;


        public DashboardController(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        #endregion

        [HttpGet("Sumary")]
        public async Task<IActionResult> Sumary()
        {
            var result = await _genericRepository.ReadCustomAsync<DashboardSumary>(null,"");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response.FirstOrDefault());
        }
        [HttpGet("PieChart")]
        public async Task<IActionResult> PieChart(short type)
        {
            var param = DataHelper.GenParams("Type", type);
            var result = await _genericRepository.ReadCustomAsync<DashboardPie>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
        [HttpGet("BarChart")]
        public async Task<IActionResult> BarChart(short type)
        {
            var param = DataHelper.GenParams("Type", type);
            var result = await _genericRepository.ReadCustomAsync<DashboardBar>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }
    }
}
