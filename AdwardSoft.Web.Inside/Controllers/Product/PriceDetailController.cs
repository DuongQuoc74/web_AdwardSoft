using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models.Common;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DocumentFormat.OpenXml.EMMA;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class PriceDetailController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public PriceDetailController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion

        #region View

        [HttpGet]
        [Route(("[controller]/{date}"))]
        public async Task<IActionResult> Index(DateTime date)
        {
            ViewBag.Products = (await _ProductSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Locations = (await _LocationSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.DeliveryPoints = (await _DeliveryPointSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Date = _ConvertDateToShortDateStr(date);

            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _FormEditPrice(int productId, int locationId, int deliveryPointId, short deliveryType, DateTime date)
        {
            var priceDetail = await _ReadById(productId, locationId, deliveryPointId, deliveryType, date);
            var isExist = await _CheckIsExisting(productId, locationId, deliveryPointId, deliveryType, date);
            ViewBag.IsExist = isExist;
            return PartialView(priceDetail);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(int productId, int locationId, int locationChildId, int deliveryPointId, short deliveryType, DateTime date)
        {

            var response = await _apiFactory.GetAsync<List<PriceDetailDatatableViewModel>>(
                this.ApiResources($"Read?productId={productId}&locationId={locationId}&locationChildId={locationChildId}&deliveryPointId={deliveryPointId}&deliveryType={deliveryType}&date={_ConvertDateToShortDateStr(date)}"),
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            return Json(new { data = response });
        }

        [HttpPost]
        public async Task<IActionResult> ReadSelect(int parentId)
        {
            var response = (await _LocationChildSelect(parentId)).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return Json(new { data = response });
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(PriceDetailViewModel vm)
        {

            var result = await _apiFactory.PostAsync<PriceDetailViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Thêm mới",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> UpdatePrice(PriceDetailViewModel vm)
        {

            var result = await _apiFactory.PutAsync<PriceDetailViewModel, int>(
                data: vm,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Điều chỉnh",
                Action = "Update",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int productId, int locationId, int deliveryPointId, short deliveryType)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?productId={productId}&locationId={locationId}&deliveryPointId={deliveryPointId}&deliveryType={deliveryType}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Xóa",
                Succeeded = result > 0 ? true : false
            };
            return response;
        }

        #endregion

        #region Methods
        private async Task<List<Select>> _ProductSelect()
        {
            var ProductList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Product/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return ProductList;
        }
        private async Task<List<Select>> _LocationSelect()
        {
            var LocationList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Location/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return LocationList;
        }
        private async Task<List<Select>> _LocationChildSelect(int parentId)
        {
            var LocationList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Location/ReadChildSelect?parentId={parentId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return LocationList;
        }
        private async Task<List<Select>> _DeliveryPointSelect()
        {
            var DeliveryPointList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "DeliveryPoint/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return DeliveryPointList;
        }

        public async Task<PriceDetailViewModel> _ReadById(int productId, int locationId, int deliveryPointId, short deliveryType, DateTime date)
        {
            var response = await _apiFactory.GetAsync<PriceDetailViewModel>(
                apiUrl: this.ApiResources($"ReadById?productId={productId}&locationId={locationId}&deliveryPointId={deliveryPointId}&deliveryType={deliveryType}&date={_ConvertDateToShortDateStr(date)}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }
        public async Task<int> _CheckIsExisting(int productId, int locationId, int deliveryPointId, short deliveryType, DateTime date)
        {
            var response = await _apiFactory.GetAsync<int>(
                apiUrl: this.ApiResources($"CheckIsExisting?productId={productId}&locationId={locationId}&deliveryPointId={deliveryPointId}&deliveryType={deliveryType}&date={_ConvertDateToShortDateStr(date)}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }
        public string _ConvertDateToShortDateStr(DateTime date)
        {
            string[] DateStr = date.ToShortDateString().Split("/");

            return DateStr[2] + "-" + DateStr[1] + "-" + DateStr[0];
        }

        #endregion
    }
}
