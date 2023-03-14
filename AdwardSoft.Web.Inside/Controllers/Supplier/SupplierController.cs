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
    public class SupplierController : Controller
    {
        #region Constructor

        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public SupplierController(IUserSession userSession, IAPIFactory apiFactory)
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
            var model = new SupplierViewModel();

            ViewBag.Action = "Create";

            if (id != 0)
            {
                ViewBag.Action = "Update";
                model = await _apiFactory.GetAsync<SupplierViewModel>("Supplier/ReadById?id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
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

            var response = await _apiFactory.GetAsync<IEnumerable<SupplierDatatableViewModel>>(
                $"Supplier/ReadDatatable?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}", 
                Endpoints.ApiPOS, 
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

           return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }

        #endregion Read

        #region Search
        [HttpGet]
        public async Task<List<SupplierViewModel>> Search(string searchBy)
        {
            var response = await _apiFactory.GetAsync<List<SupplierViewModel>>($"Supplier/ReadDatatable?pageSize=5&skip=0&searchBy={searchBy}", Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }

        #endregion Search

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(SupplierViewModel model)
        {
            var result = await _apiFactory.PostAsync<SupplierViewModel, int>(
                data: model,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Create",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Update

        [HttpPut]
        public async Task<ResponseContainer> Update(SupplierViewModel model)
        {
            var result = await _apiFactory.PutAsync<SupplierViewModel, int>
            (
                 data: model,
                 apiUrl: this.ApiResources("Update"),
                 endpoint: Endpoints.ApiPOS,
                 token: _userSession.BearerToken
            );

            var response = new ResponseContainer
            {
                Activity = "Update",
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
           var result = await _apiFactory.DeleteAsync<int>(
                   apiUrl: this.ApiResources($"Delete?Id={id}"),
                   endpoint: Endpoints.ApiPOS,
                   token: _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Activity = "Delete";
            if (result == 1)
            {
                response.Succeeded = true;
            }
            else
            {
                response.Activity = "Supplier is using. Remove ";
                response.Succeeded = false;
            }

            return response;
        }

        #endregion

        #region Remote CheckPhone
        public async Task<JsonResult> CheckPhone(int id, string tel)
        {
            // => Check phone
            var isExisting = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckPhoneIsExisting?id={id}&tel={tel}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );
            if (isExisting > 0)
                return Json("The Tel is existing");

            return Json(true);
        }
        #endregion

        #region Remote CheckEmail
        public async Task<JsonResult> CheckEmail(int id, string email)
        {
            // => Check email
            var isExisting = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckEmailIsExisting?Id={id}&email={email}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isExisting > 0)
                return Json("The Email is existing");
            return Json(true);
        }
        #endregion

        #region Remote CheckSupplierIsUsing
        public async Task<JsonResult> CheckSupplierIsUsing(int id)
        {
           
            var isExisting = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckSupplierIsUsing?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isExisting > 0)
                return Json("The Supplier is using");

            return Json(true);
        }
        #endregion
    }
}
