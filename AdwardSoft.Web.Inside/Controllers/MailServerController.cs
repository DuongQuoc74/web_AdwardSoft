using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class MailServerController : Controller
    {
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public MailServerController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        public IActionResult Index()
        {
            return View(new MailServerViewModel());
        }

        public async Task<IActionResult> _Form(int id)
        {
            MailServerViewModel model = new MailServerViewModel();
            if (id > 0)
            {
                model = await _apiFactory.GetAsync<MailServerViewModel>("MailServer/ReadById?Id=" + id, Endpoints.ApiCore, _userSession.BearerToken);
            }
            return PartialView(model);
        }

        [HttpGet]
        public async Task<List<MailServerViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<List<MailServerViewModel>>("MailServer/Read", Endpoints.ApiCore, _userSession.BearerToken);
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Create(MailServerViewModel vm)
        {
            var result = await _apiFactory.PostAsync<MailServerViewModel, int>(vm, "MailServer/Create", Endpoints.ApiCore, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Thêm mới";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Update(MailServerViewModel vm)
        {
            var result = await _apiFactory.PutAsync<MailServerViewModel, int>(vm, "MailServer/Update", Endpoints.ApiCore, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Điều chỉnh";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<bool>("MailServer/Delete?Id=" + id, Endpoints.ApiCore, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Xóa";
            response.Succeeded = result;
            return response;
        }

        public async Task<JsonResult> IsExistEmail(string email, int id)
        {
            var existingEmail = await _apiFactory.GetAsync<int>($"MailServer/IsExistEmail?email={email}&id={id}", Endpoints.ApiCore, _userSession.BearerToken);

            if (existingEmail == 1)
                return Json(false);
            return Json(true);
        }
    }
}