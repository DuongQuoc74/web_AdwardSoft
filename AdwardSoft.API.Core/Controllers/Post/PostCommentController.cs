using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Core.Pattern;
using AdwardSoft.DTO.Presentation.Core;
using AdwardSoft.Utilities.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers.Post
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PostCommentController : Controller
    {
        private IGenericRepository _generic;

        public PostCommentController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read(int pageSize, int skip, string searchBy, int type)
        {
            var param = DataHelper.GenParams("pageSize", pageSize, "skip", skip, "searchBy", searchBy, "type", type);
            var result = await _generic.ReadCustomAsync<PostCommentDataTable>(param, "");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
        
        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int pageSize, int skip, string parentId, int status, long postId)
        {
            var param = DataHelper.GenParams("pageSize", pageSize, "skip", skip
                , "parentId", parentId, "status", status, "postId", postId);
            var result = await _generic.ReadCustomAsync<PostComment>(param, "");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PostComment obj)
        {
            var result = await _generic.CreateAsync<PostComment, string>(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PostComment obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _generic.DeteteAsync<PostComment, int>(parms);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> Approve(PostCommentStatus obj)
        {
            var result = await _generic.UpdateAsync(obj);

            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }
    }
}