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
    public class HandleErrorController : Controller
    {
        #region Structure
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        public HandleErrorController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region view
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Read
        [HttpGet]
        public async Task<List<HandleErrorViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<List<HandleErrorViewModel>>("HandleError/Read", HostConstants.ApiCore, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(HandleErrorViewModel HandleError)
        {
            var response = new ResponseContainer();
            var result = await _apiFactory.PostAsync<HandleErrorViewModel, int>(HandleError, this.ApiResources("Create"), HostConstants.ApiCore, _userSession.BearerToken);
            if (result < 0)
            {
                throw new Exception($"Thất bại khi tạo mới");
            }
            response.Activity = $"Tạo mới HandleError ";
            response.Action = "create";
            return response;
        }
        #endregion

        #region Update
        [HttpPost]
        public async Task<ResponseContainer> Update(HandleErrorViewModel HandleError)
        {
            var response = new ResponseContainer();
            var result = await _apiFactory.PutAsync<HandleErrorViewModel, int>(HandleError, this.ApiResources("Update"), HostConstants.ApiCore, _userSession.BearerToken);
            if (result < 0)
            {
                throw new Exception($"Thất bại khi Điều chỉnh");
            }
            response.Activity = $"Điều chỉnh thiết lập lỗi ";
            response.Action = "update";
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(string id)
        {
            var response = new ResponseContainer();          
            var result = await _apiFactory.DeleteAsync<int>(this.ApiResources("Delete?id=" + id), HostConstants.ApiCore, _userSession.BearerToken);
            if (result < 0)
                throw new Exception("Thất bại khi xóa HandleError");
            response.Action = "delete";
            response.Activity = $"Xóa HandleError ";
            return response;
        }
        #endregion

        #region Form
        [HttpGet]
        public async Task<IActionResult> _Form(int id, string languageCode)
        {
            var model = new HandleErrorViewModel();
            var languages = await _apiFactory.GetAsync<List<LanguageViewModel>>("Language/Read", HostConstants.ApiCore, _userSession.BearerToken);
            ViewBag.Action = "Create";
            if (String.IsNullOrEmpty(languageCode))
            {
                var langDefaut = languages.Where(x => x.IsDefault == true).First();
                ViewBag.LanguageName = langDefaut.Name;
                model.LanguageCode = langDefaut.Code;
            }
            if (id != 0)
            {
                ViewBag.Action = "Update";
                model = await _apiFactory.GetAsync<HandleErrorViewModel>("HandleError/ReadById?id=" + id + "&languageCode=" + model.LanguageCode, HostConstants.ApiCore, _userSession.BearerToken);
                ViewBag.Languages = languages.Select(x => new SelectListItem() { Value = x.Code, Text = x.Name, Selected = x.Code == (languageCode ?? "vi") });
            }
            return PartialView(model);

        }
        #endregion
    }
}