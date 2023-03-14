using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class NotificationTemplateController : Controller
    {
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public NotificationTemplateController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _Form(int id, short actions)
        {
            var model = new NotificationTemplateViewModel()
            {
                Action = actions,
                IsActive = true
            };
            var mailServers = await _apiFactory.GetAsync<List<MailServerViewModel>>("MailServer/Read", Endpoints.ApiCore, _userSession.BearerToken);
            ViewBag.MailServers = mailServers.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() });
            if (id > 0) model = await _apiFactory.GetAsync<NotificationTemplateViewModel>("NotificationTemplate/ReadById?id=" + id, HostConstants.ApiCore, _userSession.BearerToken);
            return PartialView(model);
        }

        [HttpGet]
        public async Task<List<NotificationTemplateViewModel>> Read(short actions)
        {
            var result = await _apiFactory.GetAsync<List<NotificationTemplateViewModel>>("NotificationTemplate/ReadByAction?action=" + actions, HostConstants.ApiCore, _userSession.BearerToken);
            return result;
        }

        [HttpPost]
        public async Task<ResponseContainer> Create(NotificationTemplateViewModel model)
        {
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Thêm mới mẫu thông báo";
            if (ModelState.IsValid)
            {
                var result = await _apiFactory.PostAsync<NotificationTemplateViewModel, int>(model, this.ApiResources("Create"), HostConstants.ApiCore, _userSession.BearerToken);
                if (result == -1)
                {
                    response.Activity = "Mẩu thông báo đã tồn tại. Thêm mới";
                    response.Succeeded = false;
                }
                else if (result < 0) throw new Exception($"Thêm mới mẫu thông báo thất bại");

            }
            else response.Succeeded = false;
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Update(NotificationTemplateViewModel model)
        {
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = $"Điều chỉnh mẫu thông báo";
            if (ModelState.IsValid)
            {
                var result = await _apiFactory.PutAsync<NotificationTemplateViewModel, int>(model, "NotificationTemplate/Update", HostConstants.ApiCore, _userSession.BearerToken);
                if (result < 0) throw new Exception($"Điều chỉnh mẫu thông báo thất bại");
            }
            else response.Succeeded = false;
            return response;
        }

        public async Task<ResponseContainer> Delete(int id)
        {
            var response = new ResponseContainer();
            response.Activity = $"Xóa mẫu thông báo";
            response.Action = "delete";
            var result = await _apiFactory.DeleteAsync<int>(this.ApiResources("Delete?id=" + id), HostConstants.ApiCore, _userSession.BearerToken);
            if (result < 0) response.Succeeded = false;
            return response;
        }
    }
}