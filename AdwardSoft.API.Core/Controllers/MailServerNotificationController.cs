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
    public class MailServerNotificationController : ControllerBase
    {
        private IGenericRepository _generic;

        public MailServerNotificationController(IGenericRepository generic)
        {
            _generic = generic;
        }

        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            var result = await _generic.ReadAsync<MailServerNotification>();
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> ReadById(int id)
        {
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>() { { "Id", id } };
            var result = await _generic.ReadCustomAsync<MailServerNotification>(param, "ById");
            if (result.Success) return Ok(result.Response);
            else return BadRequest(result.Messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MailServerNotification model)
        {
            List<MailServerNotification> mailServerNotifications = new List<MailServerNotification>();
            foreach (var item in model.NotificationTemplateIds)
            {
                mailServerNotifications.Add(new MailServerNotification() { NotificationTemplateId = item, MailServerId = model.MailServerId });
            }

            Dictionary<string, dynamic> parms = new Dictionary<string, dynamic>() { { "Id", model.MailServerId } };
            var result = await _generic.DeteteAsync<MailServerNotification, int>(parms);

            var result2 = await _generic.CreateMultipleAsync(mailServerNotifications);

            if (result2.Success) return Ok(result2.Response + result.Response);
            else return BadRequest(result2.Messages);
        }
    }
}