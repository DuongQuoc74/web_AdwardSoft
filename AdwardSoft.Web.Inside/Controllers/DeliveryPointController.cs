using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class DeliveryPointController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public DeliveryPointController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View

        [HttpGet]
        [Route("/[controller]")]
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.LocationId = id;
            ViewBag.Locations = (await _ReadLocation())
              .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }

        #endregion
        #region Form

        public async Task<IActionResult> _Form(int id)
        {
            var model = new DeliveryPointViewModel { Id = id };

            if (model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
            }
            ViewBag.Locations = (await _ReadLocation())
              .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpGet]
        public async Task<IEnumerable<DeliveryPointDatatableViewModel>> Read()
        {
            var response = await _apiFactory.GetAsync<IEnumerable<DeliveryPointDatatableViewModel>>(
                apiUrl: this.ApiResources($"Read"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<DeliveryPointViewModel> ReadById(int id) => await _ReadByIdAsync(id);
        #endregion
        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(DeliveryPointViewModel vm)
        {
            var result = await _apiFactory.PostAsync<DeliveryPointViewModel, int>(
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
        public async Task<ResponseContainer> Update(DeliveryPointViewModel vm)
        {
            var result = await _apiFactory.PutAsync<DeliveryPointViewModel, int>(
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
                Action = "Delete",
                Activity = "Xóa",
                Succeeded = result > 0 ? true : false
            };
            return response;
        }

        #endregion

        #region Methods
        private async Task<DeliveryPointViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<DeliveryPointViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }
        private async Task<IEnumerable<Select>> _ReadLocation()
        {
            var LocationSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: "Location/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return LocationSelect;
        }
        #endregion



    }
}
