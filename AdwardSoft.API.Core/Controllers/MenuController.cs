using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;

        public MenuController(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(string languageCode, int menuGroupId)
        {
            var param = DataHelper.GenParams("LanguageCode", languageCode, "MenuGroupId", menuGroupId);

            var result = await _genericRepository.ReadCustomAsync<Menu>(param, "");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response); 
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _genericRepository.ReadByIdAsync<Menu>(id);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadByLanguage")]
        public async Task<IActionResult> Read(int id, string languageCode)
        {
            var param = DataHelper.GenParams("Id", id, "LanguageCode", languageCode);

            var result = await _genericRepository.ReadByCustomAsync<Menu>(param, "ByLanguage");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpGet("ReadCustom")]
        public async Task<IActionResult> ReadCustoms(int menuGroupId)
        {
            var prms = DataHelper.GenParams("menuGroupId", menuGroupId);

            var result = await _genericRepository.ReadCustomAsync<Menu>(prms, "CustomByMenu");

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(result.Response);
        }

        [HttpGet("ReadPage")]
        public async Task<IActionResult> ReadPages(int menuGroupId)
        {
            var prms = DataHelper.GenParams("menuGroupId", menuGroupId);

            var result = await _genericRepository.ReadCustomAsync<Menu>(prms, "PageByMenu");

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(result.Response);
        }

        [HttpGet("ReadPost")]
        public async Task<IActionResult> ReadPosts(int menuGroupId)
        {
            var prms = DataHelper.GenParams("menuGroupId", menuGroupId);

            var result = await _genericRepository.ReadCustomAsync<Menu>(prms, "PostByMenu");

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(result.Response);
        }

        [HttpGet("ReadCategory")]
        public async Task<IActionResult> ReadCategories(int menuGroupId)
        {
            var prms = DataHelper.GenParams("menuGroupId", menuGroupId);

            var result = await _genericRepository.ReadCustomAsync<Menu>(prms, "CategoryByMenu");

            if (!result.Success)
                return BadRequest(result.Messages);
            return Ok(result.Response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] List<Menu> obj)
        {
            var result = await _genericRepository.CreateMultipleAsync(obj, "IsInMenu,MenuGroupName");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPost("CreateCustom")]
        public async Task<IActionResult> Create(Menu obj)
        {
            var result = await _genericRepository.CreateAsync(obj, "IsInMenu,MenuGroupName");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Menu obj)
        {
            var result = await _genericRepository.UpdateAsync(obj, "IsInMenu,MenuGroupName");

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPut("UpdateTranslation")]
        public async Task<IActionResult> UpdateTranslation(MenuTranslation obj)
        {
            var result = await _genericRepository.UpdateAsync(obj);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _genericRepository.DeteteAsync<Menu, int>(parms);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

        [HttpPost("Sort")]
        public async Task<IActionResult> Sort(MenuJson json)
        {
            var result = await _genericRepository.UpdateAsync(json);

            if (!result.Success)
                return BadRequest(result.Messages);

            return Ok(result.Response);
        }

    }
}