using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.DTO.Presentation.CORE.Post;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PostController : Controller
    {
        private IGenericRepository _generic;
        public PostController(IGenericRepository generic)
        {
            _generic = generic;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<PostInfor>();

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpGet("ReadInfoBasic")]
        [AllowAnonymous]
        public async Task<IActionResult> ReadInfoBasic()
        {
            var param = DataHelper.GenParams("pageSize", 5, "skip", 0);
            var result = await _generic.ReadCustomAsync<DataTablePostInfor>(param, "InfoBasic");
            if (!result.Success)
                return BadRequest(result.Messages);

            var response = result.Response.Select(x => new { Title = x.Title, Description = x.Description, FeaturedImage = x.FeaturedImage });

            return Ok(response);

        }
        [HttpGet("ReadDetails")]
        [AllowAnonymous]
        public async Task<IActionResult> ReadDetails(int id)
        {
            var param = DataHelper.GenParams("id", id);
            var result = await _generic.ReadCustomAsync<DataTablePostInfor>(param, "Details");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);

        }
        [HttpGet("ReadPagination")]
        public async Task<IActionResult> ReadPagination(int pageSize, int skip, string searchBy, int type)
        {
            var param = DataHelper.GenParams("pageSize", pageSize, "skip", skip, "searchBy", searchBy, "type", type);
            var result = await _generic.ReadCustomAsync<DataTablePostInfor>(param, "ReadPagination");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        
        [HttpGet("Read/{id}")]
        public async Task<IActionResult> ReadById(int id)
        {
            var result = await _generic.ReadByIdAsync<PostInfor>(id);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpGet("ReadByPostId/{id}")]
        public async Task<IActionResult> ReadByPostId(int id)
        {
            var param = DataHelper.GenParams("PostId", id);
            var result = await _generic.ReadCustomAsync<PostGallery>( param, "ReadByPostId");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpPost("GalleryCreate")]
        public async Task<IActionResult> GalleryCreate([FromBody] PostGallery model)
        {
            var result = await _generic.CreateAsync<PostGallery, string>(model);
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
       

        [HttpDelete("GalleryDelete/{id}")]
        public async Task<IActionResult> GalleryDelete(string id)
        {
            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _generic.DeteteAsync<PostGallery, int>(parms);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PostInfor obj)
        {
            var result = await _generic.CreateAsync<PostInfor, int>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        [HttpPut("UpdatePost")]
        public async Task<IActionResult> UpdatePost(PostInfor obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok();
            else return BadRequest(result.Messages);
        }

        [HttpPut("UpdatePostSEO")]
        public async Task<IActionResult> UpdatePostSEO(PostSEO obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok();
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _generic.DeteteAsync<PostInfor, int>(parms);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

    }
}