using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class LanguageController : Controller
    {
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public LanguageController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _Form(string code)
        {
            var model = new LanguageViewModel();
            if (!string.IsNullOrWhiteSpace(code))
            {
                model = await ReadByCode(code);
            }

            ViewBag.ActionMethod = string.IsNullOrWhiteSpace(code) ? "POST" : "PUT";
            return PartialView(model);
        }

        public async Task<LanguageViewModel> ReadByCode(string code)
        {
            var language = await _apiFactory.GetAsync<LanguageViewModel>($"Language/ReadById?id={code}", Endpoints.ApiCore, _userSession.BearerToken);
            return language;
        }

        [HttpGet]
        public async Task<List<LanguageViewModel>> Read()
        {
            var languages = await _apiFactory.GetAsync<List<LanguageViewModel>>("Language/Read", Endpoints.ApiCore, _userSession.BearerToken);
            return languages;
        }

        [HttpPost]
        public async Task<ResponseContainer> Create(LanguageViewModel vm)
        {
            vm.Code = vm.Code.ToUpper();
            var result = await _apiFactory.PostAsync<LanguageViewModel, int>(vm, "Language/Create", Endpoints.ApiCore, _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Thêm mới",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        [HttpPut]
        public async Task<ResponseContainer> Update(LanguageViewModel vm)
        {
            var result = await _apiFactory.PutAsync<LanguageViewModel, int>(vm, "Language/Update", Endpoints.ApiCore, _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Điều chỉnh",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Delete(string code)
        {
            var result = await _apiFactory.DeleteAsync<int>($"Language/Delete?Id={code}", HostConstants.ApiCore, _userSession.BearerToken);

            var response = new ResponseContainer();
            response.Activity = "Xóa";

 
            if (result == 1)
            {
                response.Succeeded = true;
            }
            else
            {
                response.Activity = "Dữ liệu đang được sử dụng. Xóa ";
                response.Succeeded = false;
            }

            return response;
        } 

        public async Task<JsonResult> CheckDuplicateCode(string code)
        {
            var duplicateLanguage = await ReadByCode(code);

            if (string.IsNullOrWhiteSpace(duplicateLanguage.Code))
                return Json(true);
            return Json(false);
        }
    }
}