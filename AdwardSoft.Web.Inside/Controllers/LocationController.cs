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

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class LocationController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;

        public LocationController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }

        #endregion

        #region View

        [HttpGet]
        [Route("/[controller]")]
        [Route("/[controller]/parent/{id}")]
        public IActionResult Index(int id)
        {
            ViewBag.ParentId = id; 
            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form(int id, int parentId)
        {
            var model = new LocationViewModel { Id = id };
            
            if (model.Id > 0)
            {
                model = await _ReadByIdAsync(id);
            }
            ViewBag.ParentId = parentId;
            ViewBag.Parents = (await _ReadParentsSelect()).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IEnumerable<LocationDatatableViewModel>> Read(int parentId = 0)
        {
            var response = await _apiFactory.GetAsync<IEnumerable<LocationDatatableViewModel>>(
                apiUrl: this.ApiResources($"ReadDatatable?parentId={parentId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        [HttpGet]
        public async Task<LocationViewModel> ReadById(int id) => await _ReadByIdAsync(id);
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(LocationViewModel vm)
        {
            var result = await _apiFactory.PostAsync<LocationViewModel, int>(
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
        public async Task<ResponseContainer> Update(LocationViewModel vm)
        {
            var result = await _apiFactory.PutAsync<LocationViewModel, int>(
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
        private async Task<LocationViewModel> _ReadByIdAsync(int id)
        {
            var response = await _apiFactory.GetAsync<LocationViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }
        private async Task<IEnumerable<Select>> _ReadParentsSelect()
        {
            var parentsSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"Location/ReadSelect?",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return parentsSelect;
        }
        #endregion
    }
}
