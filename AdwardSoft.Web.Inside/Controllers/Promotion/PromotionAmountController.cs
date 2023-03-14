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
    public class PromotionAmountController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public PromotionAmountController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public IActionResult Index(int id)
        {
            ViewBag.PromotionId = id;
            return PartialView();
        }
        #endregion

        #region Read
        [HttpPost]
        public async Task<List<PromotionAmountViewModel>> Read(int promotionId)
        {
            var response = await _apiFactory.GetAsync<List<PromotionAmountViewModel>>("PromotionAmount/Read?promotionId="
                + promotionId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int promotionId, int idx)
        {
            var model = new PromotionAmountViewModel();
            model.PromotionId = promotionId;
            if (idx != 0)
            {
                model = await _apiFactory.GetAsync<PromotionAmountViewModel>($"PromotionAmount/ReadById?promotionId={promotionId}&idx={idx}", Endpoints.ApiPOS, _userSession.BearerToken);
            }
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PromotionAmountViewModel model)
        {
            var result = await _apiFactory.PostAsync<PromotionAmountViewModel, bool>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(PromotionAmountViewModel model)
        {
            var result = await _apiFactory.PutAsync<PromotionAmountViewModel, bool>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int promotionId, int idx)
        {
            var result = await _apiFactory.DeleteAsync<bool>($"Promotion/Delete?promotionId={promotionId}&idx={idx}", Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion
    }
}
