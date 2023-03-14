using System.Collections.Generic;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Mvc;


namespace AdwardSoft.Web.Inside.Controllers.Employee
{
    [AdsAuthorization]
    public class EmployeeUserController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public EmployeeUserController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int employeeId)
        {
            ViewBag.User = await _apiFactory.GetAsync<List<Select>>("User/ReadSelect", Endpoints.ApiIdentity, _userSession.BearerToken);
            var model = new EmployeeUserViewModel();
            model = await _apiFactory.GetAsync<EmployeeUserViewModel>
                ($"EmployeeUser/ReadById?employeeId={employeeId}",
                Endpoints.ApiPOS, _userSession.BearerToken);
            model.EmployeeId = employeeId;
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(EmployeeUserViewModel model)
        {
            var result = await _apiFactory.PostAsync<EmployeeUserViewModel, int>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Link";
            response.Succeeded = result > 0 ? true : false;
            return response;
        }
        #endregion

        #region Check
        public async Task<JsonResult> CheckExist(int employeeId, long userId)
        {
            var checkExist = await _apiFactory.GetAsync<int>
                ($"EmployeeUser/CheckExist?employeeId={employeeId}&userId={userId}",
                Endpoints.ApiPOS, _userSession.BearerToken);

            if (checkExist == 1)
                return Json(false);
            return Json(true);
        }
        #endregion
    }
}
