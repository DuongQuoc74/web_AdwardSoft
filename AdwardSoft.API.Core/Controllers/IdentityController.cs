using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.API.Core.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet("Test")]
        [Authorize]
        //[Route("api/ResourceWithoutPolicy")
        public IActionResult ResourceWithoutPolicy()
        {
            return new JsonResult(new { ApiName = "API.Core", AuthorizationType = "Without Policy" });
        }
    }
}