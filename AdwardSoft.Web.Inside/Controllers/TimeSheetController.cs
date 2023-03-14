using System;
using System.Collections.Generic;
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
    public class TimeSheetController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public TimeSheetController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public async Task<IActionResult> Index() {
            var model = await _apiFactory.GetAsync<List<TimeSheetViewModel>>("TimeSheet/Read", Endpoints.ApiPOS, _userSession.BearerToken);
            return View(model);
        } 
        #endregion

        #region Form
        public IActionResult _Form(long userId)
        {
            var model = new TimeSheetViewModel();
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(TimeSheetViewModel model)
        {
            var result = await _apiFactory.PostAsync<TimeSheetViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(long userId, DateTime date)
        {
            var result = await _apiFactory.DeleteAsync<bool>("TimeSheet/Delete?userId=" + userId + "&date=" + date, Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Remove";
            response.Succeeded = result;
            return response;
        }
        #endregion
    }
}
