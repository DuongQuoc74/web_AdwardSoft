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
    public class CustomerGroupController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public CustomerGroupController(IUserSession userSession, IAPIFactory apiFactory)
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
            var model = new CustomerGroupViewModel { Id = id };

            if (model.Id > 0)
                model = await _ReadByIdAsync(id);

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<CustomerGroupDatatableViewModel>> Read(int pricingPolicy, int status)
        {
            var response = await _apiFactory.GetAsync<IEnumerable<CustomerGroupDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatable?pricingPolicy={pricingPolicy}&status={status}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<CustomerGroupViewModel> ReadById(int id) => await _ReadByIdAsync(id);

        [HttpGet]
        public async Task<JsonResult> CheckIsUsing(int id)
        {
            // Check is using
            var isUsing = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckIsUsing?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isUsing > 0)
                return Json(false);

            return Json(true);
        }

        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(CustomerGroupViewModel vm)
        {
            var result = await _apiFactory.PostAsync<CustomerGroupViewModel, int>(
                data: vm,
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
        public async Task<ResponseContainer> Update(CustomerGroupViewModel vm)
        {
            var result = await _apiFactory.PutAsync<CustomerGroupViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

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

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Remote

        public async Task<JsonResult> Check(int id, short status)
        {
            // Check is using
            var isUsing = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckUpdateStatusIsUsing?id={id}&status={status}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isUsing > 0)
                return Json("Nhóm khách hàng đã bị trùng!");

            return Json(true);
        }

        #endregion

        #region Methods

        private async Task<CustomerGroupViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<CustomerGroupViewModel>
            (
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion
    }
}
