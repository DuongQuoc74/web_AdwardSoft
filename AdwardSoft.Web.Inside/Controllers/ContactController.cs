using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class ContactController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public ContactController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public IActionResult Index() => View();
        #endregion

        #region Read
        [HttpGet]
        public async Task<List<ContactViewModel>> Read(int status)
        {
            var response = await _apiFactory.GetAsync<List<ContactViewModel>>("Contact/Read?status=" + status, HostConstants.ApiCore, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int id)
        {        
            var model = await _apiFactory.GetAsync<ContactViewModel>("Contact/ReadById?Id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
            return PartialView(model);
        }
        #endregion

        #region Update
        [HttpPost]
        public async Task<ResponseContainer> Update(ContactViewModel vm)
        {
            var result = await _apiFactory.PutAsync<ContactViewModel, int>(vm, "Contact/Update", HostConstants.ApiCore, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Điều chỉnh";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<bool>("Contact/Delete?id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Activity = "Xóa";
            response.Succeeded = result;
            return response;
        }
        #endregion
    }
}