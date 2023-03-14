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
    public class UnitController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public UnitController(IUserSession userSession, IAPIFactory apiFactory)
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
            var model = new UnitViewModel { Id = id };

            if(model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
                model.Code = model.Code.TrimEnd();
            }
                
            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<UnitViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<IEnumerable<UnitViewModel>>(
                apiUrl: this.ApiResources($"Read"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<UnitViewModel> ReadById(int id) => await _ReadByIdAsync(id);

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
        public async Task<ResponseContainer> Create(UnitViewModel vm)
        {
            var result = await _apiFactory.PostAsync<UnitViewModel, int>(
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
        public async Task<ResponseContainer> Update(UnitViewModel vm)
        {
            var result = await _apiFactory.PutAsync<UnitViewModel, int>(
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

            var response = new ResponseContainer {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Remote

        public async Task<JsonResult> CheckCode(string code, int id)
        {
            // Check Duplicate
            var isDuplicate = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckCodeIsExisting?id={id}&code={code}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isDuplicate > 0)
                return Json("đã tồn tại");

            // Check is using
            var isUsing = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckUpdateCodeIsUsing?id={id}&code={code}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isUsing > 0)
                return Json("Unit is using, not allow update code !");

            return Json(true);
        }

        #endregion

        #region Methods

        private async Task<UnitViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<UnitViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion
    }
}
