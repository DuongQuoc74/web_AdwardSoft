using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class ProductUnitConverterController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public ProductUnitConverterController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public IActionResult Index(int id)
        {
            ViewBag.productId = id;
            return PartialView();
        }
        #endregion

        #region Read
        [HttpPost]
        public async Task<List<ProductUnitConverterDatatableViewModel>> Read(int productId)
        {
            var response = await _apiFactory.GetAsync<List<ProductUnitConverterDatatableViewModel>>("ProductUnitConverter/Read?productId="
                + productId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int productId, int unitId)
        {       
            ViewBag.Units = (await _UnitSelect(productId)).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            var model = new ProductUnitConverterViewModel();
            model = await _apiFactory.GetAsync<ProductUnitConverterViewModel>
                ($"ProductUnitConverter/ReadById?productId={productId}&unitId={unitId}",
                Endpoints.ApiPOS, _userSession.BearerToken);
            model.ProductId = productId;
            model.UnitIdCurrnet = model.UnitId;
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(ProductUnitConverterViewModel model)
        {
            var result = await _apiFactory.PostAsync<ProductUnitConverterViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(ProductUnitConverterViewModel model)
        {
            var result = await _apiFactory.PutAsync<ProductUnitConverterViewModel, int>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int productId, int unitId)
        {
            var result = await _apiFactory.DeleteAsync<bool>(
                apiUrl: this.ApiResources($"Delete?productId={productId}&unitId={unitId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Check
        public async Task<JsonResult> CheckExist(int productId, int unitId, int unitIdCurrnet)
        {
            if (unitIdCurrnet == unitId && unitIdCurrnet != 0)
            {
                return Json(true);
            }

            var checkExist = await _apiFactory.GetAsync<int>
                ($"ProductUnitConverter/CheckExist?productId={productId}&unitId={unitId}",
                Endpoints.ApiPOS, _userSession.BearerToken);

            if (checkExist == 1)
                return Json(false);
            return Json(true);


        }
        #endregion

        #region Method
        private async Task<List<Select>> _UnitSelect(int productId)
        {
            var units = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Unit/ReadSelectByExcludeProduct?productId={productId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return units;
        }
        #endregion
    }
}
