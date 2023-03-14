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
    public class PromotionController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public PromotionController(IUserSession userSession, IAPIFactory apiFactory)
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

        #region Read
        [HttpPost]
        public async Task<List<PromotionViewModel>> Read(int status, int type, int year)
        {
            var response = await _apiFactory.GetAsync<List<PromotionViewModel>>("Promotion/Read?status="
                + status + "&type=" + type  + "&year=" + year, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int id)
        {
            var model = new PromotionViewModel();
            model.ExpiryDate = DateTime.Now;
            model.EffectiveDate = DateTime.Now;
            if (id != 0)
            {
                model = await _apiFactory.GetAsync<PromotionViewModel>($"Promotion/ReadById?id={id}", Endpoints.ApiPOS, _userSession.BearerToken);
            }
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PromotionViewModel model)
        {           
            string[] effectiveDateStr = model.EffectiveDateStr.Split("/");
            model.EffectiveDate = DateTime.Parse(effectiveDateStr[2] + "-" + effectiveDateStr[1] + "-" + effectiveDateStr[0]);
            string[] expiryDateStr = model.ExpiryDateStr.Split("/");
            model.ExpiryDate = DateTime.Parse(expiryDateStr[2] + "-" + expiryDateStr[1] + "-" + expiryDateStr[0]);
            var result = await _apiFactory.PostAsync<PromotionViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(PromotionViewModel model)
        {
            string[] effectiveDateStr = model.EffectiveDateStr.Split("/");
            model.EffectiveDate = DateTime.Parse(effectiveDateStr[2] + "-" + effectiveDateStr[1] + "-" + effectiveDateStr[0]);
            string[] expiryDateStr = model.ExpiryDateStr.Split("/");
            model.ExpiryDate = DateTime.Parse(expiryDateStr[2] + "-" + expiryDateStr[1] + "-" + expiryDateStr[0]);
            var result = await _apiFactory.PutAsync<PromotionViewModel, int>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
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
            var result = await _apiFactory.DeleteAsync<bool>("Promotion/Delete?id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Function
        [HttpPost]
        public async Task<JsonResult> CheckDate(int id, short type, string effectiveDateStr, string expiryDateStr)
        {
            var model = new PromotionViewModel();
            model.Id = id;
            model.Type = type;
            string[] EffectiveDateStr = effectiveDateStr.Split("/");
            model.EffectiveDate = DateTime.Parse(EffectiveDateStr[2] + "-" + EffectiveDateStr[1] + "-" + EffectiveDateStr[0]);
            string[] ExpiryDateStr = expiryDateStr.Split("/");
            model.ExpiryDate = DateTime.Parse(ExpiryDateStr[2] + "-" + ExpiryDateStr[1] + "-" + ExpiryDateStr[0]);

            if (model.EffectiveDate.Date > model.ExpiryDate.Date)
            {
                return Json(false);
            }

            var result = await _apiFactory.PostAsync<PromotionViewModel, bool>(model, this.ApiResources("CheckDate"), Endpoints.ApiPOS, _userSession.BearerToken);
            return Json(result);
        }
        #endregion
    }
}
