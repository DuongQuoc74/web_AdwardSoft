using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Web.Inside.Models.Common;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Security.Authorization;
using DocumentFormat.OpenXml.EMMA;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Office2013.PowerPoint;
using OpenXmlPowerTools;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class DeliveryVehicleController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public DeliveryVehicleController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion

        #region View

        public async Task<IActionResult> Index()
        {
            ViewBag.Customers = (await _ReadCustomersSelect()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.VehicleTypes = (await _ReadVehicleTypesSelect()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(int id)
        {
            var model = new DeliveryVehicleViewModel { Id = id };
            
            if (model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
            }

            ViewBag.Customers = (await _ReadCustomersSelect()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.VehicleTypes = (await _ReadVehicleTypesSelect()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<DeliveryVehicleDatatableViewModel>> Read(string licensePlate, int customerId, int vehicleTypeId)
        {
            var response = await _apiFactory.GetAsync<IEnumerable<DeliveryVehicleDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatable?licensePlate={licensePlate}&customerId={customerId}&vehicleTypeId={vehicleTypeId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<DeliveryVehicleViewModel> ReadById(int id) => await _ReadByIdAsync(id);
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(DeliveryVehicleViewModel vm)
        {
            var result = await _apiFactory.PostAsync<DeliveryVehicleViewModel, int>(
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
        public async Task<ResponseContainer> Update(DeliveryVehicleViewModel vm)
        {
            var result = await _apiFactory.PutAsync<DeliveryVehicleViewModel, int>(
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
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
            token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = result == 1 ? "Xóa" : "Phương tiện giao hàng đang được sử dụng. Xóa ",
                Succeeded = result == 1 ? true : false,
                Action = "Delete"
            };

            return response;
        }

        #endregion

        #region Check

        public async Task<JsonResult> CheckDuplicateLicensePlate(int id, string licensePlate)
        {
            var duplicateDeliveryVehicle = await _ReadByLicensePlateAsync(id, licensePlate);

            if (string.IsNullOrWhiteSpace(duplicateDeliveryVehicle.LicensePlate))
                return Json(true);
            return Json(false);
        }

        #endregion

        #region Methods

        private async Task<DeliveryVehicleViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<DeliveryVehicleViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<DeliveryVehicleViewModel> _ReadByLicensePlateAsync(int id, string licensePlate)
        {
            var response = await _apiFactory.GetAsync<DeliveryVehicleViewModel>(
                apiUrl: this.ApiResources($"ReadByLicensePlate?id={id}&licensePlate={licensePlate}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<IEnumerable<Select>> _ReadCustomersSelect()
        {
            var customersSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"Customer/ReadSelect?",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return customersSelect;
        }

        private async Task<IEnumerable<Select>> _ReadVehicleTypesSelect()
        {
            var vehicleTypesSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"VehicleType/ReadSelect?",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return vehicleTypesSelect;
        }

        #endregion
    }
}
