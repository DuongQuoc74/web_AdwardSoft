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
    public class PositionController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public PositionController(IUserSession userSession, IAPIFactory apiFactory)
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
            var model = new PositionViewModel { Id = id };

            if (model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
            }

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<PositionViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<IEnumerable<PositionViewModel>>(
                apiUrl: this.ApiResources($"Read"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<PositionViewModel> ReadById(int id) => await _ReadByIdAsync(id);
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PositionViewModel vm)
        {
            var result = await _apiFactory.PostAsync<PositionViewModel, int>(
                data: vm,
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
        public async Task<ResponseContainer> Update(PositionViewModel vm)
        {
            var result = await _apiFactory.PutAsync<PositionViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

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
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Xóa",
                Succeeded = result > 0 ? true : false
            };
            return response;
        }

        #endregion

        #region Methods
        private async Task<PositionViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<PositionViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }
        #endregion
    }
}
