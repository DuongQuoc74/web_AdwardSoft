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
    public class MembershipClassController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public MembershipClassController(IUserSession userSession, IAPIFactory apiFactory)
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
            var model = new MembershipClassViewModel { Id = id };

            if (model.Id > 0)
                model = await _ReadByIdAsync(id);

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<MembershipClassViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<List<MembershipClassViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatable"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<MembershipClassViewModel> ReadById(int id) => await _ReadByIdAsync(id);

        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(MembershipClassViewModel vm)
        {
            var result = await _apiFactory.PostAsync<MembershipClassViewModel, int>(
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
        public async Task<ResponseContainer> Update(MembershipClassViewModel vm)
        {
            var result = await _apiFactory.PutAsync<MembershipClassViewModel, int>(
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

        [HttpPut]
        public async Task<ResponseContainer> UpdateLevel(int selectId, bool isUp)
        {
            var vm = new MembershipClassSortViewModel
            {
                SelectId = selectId,
                IsUp = isUp
            };

            var result = await _apiFactory.PutAsync<MembershipClassSortViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Update/Level"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            var response = new ResponseContainer
            {
                Action = "Update",
                Activity = "Update level",
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

        public async Task<JsonResult> CheckName(int id, string name)
        {
            // => Check is using
            var isUsing = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckUpdateNameIsUsing?id={id}&name={name}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (isUsing > 0)
                return Json("Membership class is using, not allow update name !");

            return Json(true);
        }

        public async Task<JsonResult> CheckRange(int id, decimal lowestValue, decimal highestValue)
        {
            // => Check range
            var rangeStr = await _apiFactory.GetAsync<string>
            (
                apiUrl: this.ApiResources($"CheckRange?lowestValue={lowestValue}&highestValue={highestValue}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (!string.IsNullOrEmpty(rangeStr))
                return Json(rangeStr);

            // => Check in range
            var inRange = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckInRange?id={id}&lowestValue={lowestValue}&highestValue={highestValue}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            if (inRange > 0)
                return Json("The value must out range !");

            return Json(true);
        }

        #endregion

        #region Methods

        private async Task<MembershipClassViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<MembershipClassViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion
    }
}
