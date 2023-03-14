using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Claims;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class ShiftController : Controller
    {
        #region Constructor

        private int _BranchId;
        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShiftController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;

            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);

            Int32.TryParse(branchId, out _BranchId);
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
            var model = new ShiftViewModel { Id = id };

            if (model.Id > 0)
                model = await _ReadByIdAsync(id);

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<ShiftDatatableViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<List<ShiftDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatable?branchId={_BranchId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(ShiftViewModel vm)
        {
            vm.BranchId = _BranchId;

            var result = await _apiFactory.PostAsync<ShiftViewModel, int>(
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
        public async Task<ResponseContainer> Update(ShiftViewModel vm)
        {
            vm.BranchId = _BranchId;

            var result = await _apiFactory.PutAsync<ShiftViewModel, int>(
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

        #region Methods

        private async Task<ShiftViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<ShiftViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion
    }
}
