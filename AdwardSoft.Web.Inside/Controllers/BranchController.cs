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
    public class BranchController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public BranchController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public async Task<IActionResult> Index() {
            var model = await _apiFactory.GetAsync<List<BranchViewModel>>("Branch/Read", Endpoints.ApiPOS, _userSession.BearerToken);
            return View(model);
        } 
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int id)
        {
            var model = new BranchViewModel();
            if (id != 0)
            {
                 model = await _apiFactory.GetAsync<BranchViewModel>($"Branch/ReadById?id={id}", Endpoints.ApiPOS, _userSession.BearerToken);
            }
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(BranchViewModel model)
        {
            var result = await _apiFactory.PostAsync<BranchViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Update
        [HttpPost]
        public async Task<ResponseContainer> Update(BranchViewModel model)
        {
            var result = await _apiFactory.PutAsync<BranchViewModel, int>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<bool>("Branch/Delete?id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Remove";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Sort
        [HttpPost]
        public async Task<ResponseContainer> Sort(string json)
        {
            var Json = new JsonData();
            Json.Data = json;
            var result = await _apiFactory.PostAsync<JsonData, int>(Json, "Branch/Sort", Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "Update";
            response.Activity = "Sort";
            response.Succeeded = result > 0 ? true : false;
            return response;

        }
        #endregion
    }
}
