using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class SupplierContactController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public SupplierContactController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public IActionResult Index(int id)
        {
            ViewBag.SupplierId = id;
            return PartialView();
        }
        #endregion

        #region Read
        [HttpPost]
        public async Task<List<SupplierContactViewModel>> Read(int supplierId)
        {
            var response = await _apiFactory.GetAsync<List<SupplierContactViewModel>>("SupplierContact/Read?supplierId="
                + supplierId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int supplierId, int idx)
        {
            var model = new SupplierContactViewModel();
            model.SupplierId = supplierId;
            if (idx != 0)
            {
                model = await _apiFactory.GetAsync<SupplierContactViewModel>($"SupplierContact/ReadById?supplierId={supplierId}&idx={idx}", Endpoints.ApiPOS, _userSession.BearerToken);
            }
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(SupplierContactViewModel model)
        {
            var result = await _apiFactory.PostAsync<SupplierContactViewModel, bool>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(SupplierContactViewModel model)
        {
            var result = await _apiFactory.PutAsync<SupplierContactViewModel, bool>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int supplierId, int idx)
        {
            var result = await _apiFactory.DeleteAsync<bool>($"Promotion/Delete?supplierId={supplierId}&idx={idx}", Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion
    }
}
