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

namespace AdwardSoft.Web.Inside.Controllers.Employee
{
    [AdsAuthorization]
    public class EmployeeSalaryController : Controller
    {
        #region Contructor
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;

        public EmployeeSalaryController(IUserSession userSession, IAPIFactory apiFactory)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
        }
        #endregion

        #region View
        public IActionResult Index(int employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            return PartialView();
        }
        #endregion

        #region Read
        [HttpPost]
        public async Task<List<EmployeeSalaryViewModel>> Read(int employeeId)
        {
            var response = await _apiFactory.GetAsync<List<EmployeeSalaryViewModel>>("EmployeeSalary/Read?employeeId="
                + employeeId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int employeeId, DateTime effectiveDate)
        {
            var TopSalary  = await _apiFactory.GetAsync<EmployeeSalaryViewModel>($"EmployeeSalary/ReadTop?employeeId={employeeId}", Endpoints.ApiPOS, _userSession.BearerToken);
            ViewBag.DateMin = TopSalary.EffectiveDate.ToString("yyyy-MM-dd");
            ViewBag.isNew = 1;
            var model = new EmployeeSalaryViewModel();
            
            if (effectiveDate == DateTime.MinValue)
            {
                model.EffectiveDate = DateTime.Now;
            }
            else
            {
                model.EffectiveDate = effectiveDate;
                model = await _apiFactory.GetAsync<EmployeeSalaryViewModel>($"EmployeeSalary/ReadById?employeeId={employeeId}&effectiveDate={model.EffectiveDate}", Endpoints.ApiPOS, _userSession.BearerToken);
            }               
            model.EmployeeId = employeeId;
            if (effectiveDate == DateTime.MinValue)
            {
                ViewBag.isNew = 0;
                model.EffectiveDate = effectiveDate;
            }
            return PartialView(model);
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(EmployeeSalaryViewModel model)
        {
            string[] effectiveDateStr = model.EffectiveDateStr.Split("/");
            model.EffectiveDate = DateTime.Parse(effectiveDateStr[2] + "-" + effectiveDateStr[1] + "-" + effectiveDateStr[0]);
            var result = await _apiFactory.PostAsync<EmployeeSalaryViewModel, bool>(model, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "create";
            response.Activity = "Create new";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(EmployeeSalaryViewModel model)
        {
            string[] effectiveDateStr = model.EffectiveDateStr.Split("/");
            model.EffectiveDate = DateTime.Parse(effectiveDateStr[2] + "-" + effectiveDateStr[1] + "-" + effectiveDateStr[0]);
            var result = await _apiFactory.PutAsync<EmployeeSalaryViewModel, bool>(model, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            ResponseContainer response = new ResponseContainer();
            response.Action = "update";
            response.Activity = "Update";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int employeeId, DateTime effectiveDate)
        {
            var result = await _apiFactory.DeleteAsync<bool>($"employee/Delete?employeeId={employeeId}&effectiveDate={effectiveDate}", Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion
    }
}
