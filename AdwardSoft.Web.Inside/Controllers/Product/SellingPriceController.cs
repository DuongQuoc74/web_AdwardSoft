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
    public class SellingPriceController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public SellingPriceController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public async Task<IActionResult> Index(int productId)
        {
            ViewBag.ProductId = productId;
            ViewBag.UnitsSelect = await _UnitSelectByProduct(productId);
            return PartialView();
        }
        #endregion

        #region Read
        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, int productId, int unitId)
        {
            var response = await _apiFactory.GetAsync<List<SellingPriceDatatableViewModel>>
            (
                apiUrl: $"SellingPrice/Read?productId={productId}&unitId={unitId}&pageSize={model.Length}&pageSkip={model.Start}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int productId, int unitId, DateTime date)
        {
            ViewBag.Units = await _UnitSelectByProduct(productId);          
            var model = new SellingPriceViewModel();
            model.IsNew = false;
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }
            if (unitId != 0)
            {
                model = await _apiFactory.GetAsync<SellingPriceViewModel>($"SellingPrice/ReadById?productId={productId}&unitId={unitId}&date={date}", Endpoints.ApiPOS, _userSession.BearerToken);
            }

            if (model.Date == DateTime.MinValue)
            {
                model.ProductId = productId;
                model.UnitId = unitId;
                model.Date = DateTime.Now;
                model.IsNew = true;
            }         
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(SellingPriceViewModel model)
        {
            var result = await _apiFactory.PostAsync<SellingPriceViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(SellingPriceViewModel model)
        {
            var result = await _apiFactory.PutAsync<SellingPriceViewModel, int>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int productId, int unitId, DateTime date)
        {
            var result = await _apiFactory.DeleteAsync<bool>($"SellingPrice/Delete?productId={productId}&unitId={unitId}&date={date}", Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Methods

        private async Task<List<Select>> _UnitSelectByProduct(int productId)
        {
            var units = await _apiFactory.GetAsync<List<Select>>(
                apiUrl: "Unit/ReadSelectByProduct?productId=" + productId,
                endpoint: Endpoints.ApiPOS, 
                token: _userSession.BearerToken);

            return units;
        }
        #endregion
    }
}
