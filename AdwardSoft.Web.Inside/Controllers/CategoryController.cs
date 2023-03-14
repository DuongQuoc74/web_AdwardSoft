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
    public class CategoryController : Controller
    {

        #region Constructor

        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public CategoryController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion

        #region View

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(int id)
        {
            var model = new CategoryViewModel();
            ViewBag.Action = "Create";

            if (id != 0)
            {
                ViewBag.Action = "Update";
                model = await _apiFactory.GetAsync<CategoryViewModel>("Category/ReadById?Id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
            }

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var response = await _apiFactory.GetAsync<List<CategoryDatatableViewModel>>(
                $"Category/ReadDatatable?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }

        #endregion 

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(CategoryViewModel model)
        {
            var result = await _apiFactory.PostAsync<CategoryViewModel, int>(
                data: model,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Thêm mới",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Update

        [HttpPut]
        public async Task<ResponseContainer> Update(CategoryViewModel model)
        {
            var result = await _apiFactory.PutAsync<CategoryViewModel, int>
            (
                 data: model,
                 apiUrl: this.ApiResources("Update"),
                 endpoint: Endpoints.ApiPOS,
                 token: _userSession.BearerToken
            );

            var response = new ResponseContainer
            {
                Activity = "Điều chỉnh",
                Action = "Update",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<int>
            (
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            var response = new ResponseContainer();

            response.Activity = "Xóa";

            if (result == 1)
            {
                response.Succeeded = true;
            }
            else
            {
                response.Activity = "Loại sản phẩm đang được sử dụng. Xóa ";
                response.Succeeded = false;
            }

            return response;
        }

        #endregion
    }
}
