using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class BankAccountController : Controller
    {

        #region Constructor

        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public BankAccountController(IUserSession userSession, IAPIFactory apiFactory)
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
            var model = new BankAccountViewModel { Id = id };

            if (model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
            }

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<BankAccountViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<IEnumerable<BankAccountViewModel>>(
                apiUrl: this.ApiResources($"Read"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<BankAccountViewModel> ReadById(int id) => await _ReadByIdAsync(id);

        #endregion 

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(BankAccountViewModel model)
        {
            var result = await _apiFactory.PostAsync<BankAccountViewModel, int>(
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
        public async Task<ResponseContainer> Update(BankAccountViewModel model)
        {
            var result = await _apiFactory.PutAsync<BankAccountViewModel, int>
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

            var response = new ResponseContainer {
                Activity = result == 1 ? "Xóa" : "Tài khoản ngân hàng đang được sử dụng. Xóa ",
                Succeeded = result == 1 ? true : false,
                Action = "Delete"
            };

            return response;
        }

        #endregion

        #region Check

        public async Task<JsonResult> CheckDuplicateBankNo(int id, string bankNo)
        {
            var duplicateBankAccount = await _ReadByBankNoAsync(id, bankNo);

            if (string.IsNullOrWhiteSpace(duplicateBankAccount.BankNo))
                return Json(true);
            return Json(false);
        }

        #endregion

        #region Methods

        private async Task<BankAccountViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<BankAccountViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<BankAccountViewModel> _ReadByBankNoAsync(int id, string bankNo)
        {
            var response = await _apiFactory.GetAsync<BankAccountViewModel>(
                apiUrl: this.ApiResources($"ReadByBankNo?id={id}&bankNo={bankNo}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion
    }
}
