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

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class MenuGroupController : Controller
    {
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public MenuGroupController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _Form(int id)
        {
            MenuGroupViewModel model = new MenuGroupViewModel();
            if (id > 0)
            {
                model = await _apiFactory.GetAsync<MenuGroupViewModel>("MenuGroup/ReadById?Id=" + id, Endpoints.ApiCore, _userSession.BearerToken);
            }

            ViewBag.ActionMethod = model.IsNew ? "POST" : "PUT";
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model)
        {
            if(String.IsNullOrWhiteSpace(model.Search.Value))
            {
                model.Search.Value = "NULL";
            }

            var result = await _apiFactory.GetAsync<List<MenuGroupViewModel>>(
                this.ApiResources($"Read?pageSize={model.Length}&skip={model.Start}&searchBy={model.Search.Value}"), 
                Endpoints.ApiCore, 
                _userSession.BearerToken
            );

            int recordsTotal = result.Any() ? result.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = result });
        }

        [HttpGet]
        public async Task<MenuGroupViewModel> ReadById(int id)
        {
            return await _apiFactory.GetAsync<MenuGroupViewModel>(this.ApiResources($"ReadById?id={id}"), Endpoints.ApiCore, _userSession.BearerToken);
        }

        [HttpPost]
        public async Task<ResponseContainer> Create(MenuGroupViewModel vm)
        {
            var result = await _apiFactory.PostAsync<MenuGroupViewModel, int>(vm, "MenuGroup/Create", Endpoints.ApiCore, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Thêm mới";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }

        [HttpPut]
        public async Task<ResponseContainer> Update(MenuGroupViewModel vm)
        {
            var result = await _apiFactory.PutAsync<MenuGroupViewModel, int>(vm, "MenuGroup/Update", Endpoints.ApiCore, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Điều chỉnh";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<bool>("MenuGroup/Delete?Id=" + id, Endpoints.ApiCore, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Xóa";
            response.Succeeded = result;
            return response;
        }

        public async Task<ResponseContainer> Sort(int selectedId, bool isUp)
        {
            var response = new ResponseContainer() { Activity = "Điều chỉnh nhóm menu" };

            MenuGroupSortViewModel vm = new MenuGroupSortViewModel
            {
                SelectedId = selectedId,
                IsUp = isUp
            };

            var result = await _apiFactory.PutAsync<MenuGroupSortViewModel, int>(vm, "MenuGroup/Sort", Endpoints.ApiCore, _userSession.BearerToken);
            if(result < 0)
            {
                response.Succeeded = false;
            }

            return response;
        }
    }
}