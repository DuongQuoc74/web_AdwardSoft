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
    public class PromotionGiftController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public PromotionGiftController(IUserSession userSession, IAPIFactory apiFactory)
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
        public async Task<List<PromotionGiftDatatableViewModel>> Read(int promotionId)
        {
            var response = await _apiFactory.GetAsync<List<PromotionGiftDatatableViewModel>>("PromotionGift/Read?promotionId="
                + promotionId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int promotionId, int giftProductId, int purchaseProductId)
        {
            ViewBag.Product = await _apiFactory.GetAsync<List<Select>>("Product/ReadSelect", Endpoints.ApiPOS, _userSession.BearerToken);
            var model = new PromotionGiftViewModel();
            model = await _apiFactory.GetAsync<PromotionGiftViewModel>
                ($"PromotionGift/ReadById?promotionId={promotionId}&giftProductId={giftProductId}&purchaseProductId={purchaseProductId}",
                Endpoints.ApiPOS, _userSession.BearerToken);
            model.PromotionId = promotionId;
            model.PurchaseProductIdCurrnet = model.PurchaseProductId;
            model.GiftProductIdCurrent = model.GiftProductId;
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PromotionGiftViewModel model)
        {
            var result = await _apiFactory.PostAsync<PromotionGiftViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(PromotionGiftViewModel model)
        {
            var result = await _apiFactory.PutAsync<PromotionGiftViewModel, int>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int promotionId, int giftProductId, int purchaseProductId)
        {
            var result = await _apiFactory.DeleteAsync<bool>
                ($"PromotionGift/Delete?promotionId={promotionId}&giftProductId={giftProductId}&purchaseProductId={purchaseProductId}",
                Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Check
        public async Task<JsonResult> CheckExist(int promotionId, int giftProductId, int purchaseProductId, int giftProductIdCurrent, int purchaseProductIdCurrent)
        {
            if (giftProductIdCurrent == giftProductId && purchaseProductIdCurrent == purchaseProductId && giftProductIdCurrent != 0 && purchaseProductIdCurrent != 0)
            {
                return Json(true);
            }


            var checkExist = await _apiFactory.GetAsync<int>
                ($"PromotionGift/CheckExist?promotionId={promotionId}&giftProductId={giftProductId}&purchaseProductId={purchaseProductId}",
                Endpoints.ApiPOS, _userSession.BearerToken);

            if (checkExist == 1)
                return Json(false);
            return Json(true);
        }
        #endregion
    }
}
