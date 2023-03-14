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
    public class PromotionComboController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public PromotionComboController(IUserSession userSession, IAPIFactory apiFactory)
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
        public async Task<List<PromotionComboDatatableViewModel>> Read(int promotionId)
        {
            var response = await _apiFactory.GetAsync<List<PromotionComboDatatableViewModel>>("PromotionCombo/Read?promotionId="
                + promotionId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int promotionId, int promoProductId, int purchaseProductId)
        {
            ViewBag.Product = await _apiFactory.GetAsync<List<Select>>("Product/ReadSelect", Endpoints.ApiPOS, _userSession.BearerToken);
            var model = new PromotionComboViewModel();
            model = await _apiFactory.GetAsync<PromotionComboViewModel>
                ($"PromotionCombo/ReadById?promotionId={promotionId}&promoProductId={promoProductId}&purchaseProductId={purchaseProductId}", 
                Endpoints.ApiPOS, _userSession.BearerToken);
            model.PromotionId = promotionId;
            model.PurchaseProductIdCurrnet = model.PurchaseProductId;
            model.PromoProductIdCurrent = model.PromoProductId;
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PromotionComboViewModel model)
        {
            var result = await _apiFactory.PostAsync<PromotionComboViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(PromotionComboViewModel model)
        {
            var result = await _apiFactory.PutAsync<PromotionComboViewModel, int>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int promotionId, int promoProductId, int purchaseProductId)
        {
            var result = await _apiFactory.DeleteAsync<bool>
                ($"PromotionCombo/Delete?promotionId={promotionId}&promoProductId={promoProductId}&purchaseProductId={purchaseProductId}", 
                Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Check
        public async Task<JsonResult> CheckExist(int promotionId, int promoProductId, int purchaseProductId, int purchaseProductIdCurrnet, int promoProductIdCurrent)
        {
            if (purchaseProductIdCurrnet == purchaseProductId && promoProductId == promoProductIdCurrent && purchaseProductIdCurrnet != 0 && promoProductIdCurrent != 0)
            {
                return Json(true);
            }

            var checkExist = await _apiFactory.GetAsync<int>
                ($"PromotionCombo/CheckExist?promotionId={promotionId}&promoProductId={promoProductId}&purchaseProductId={purchaseProductId}",
                Endpoints.ApiPOS, _userSession.BearerToken);

            if (checkExist == 1)
                return Json(false);
            return Json(true);
        }
        #endregion
    }
}
