using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Security.Claims;
using AdwardSoft.Utilities.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class EmployeeShiftController : Controller
    {
        #region Constructor
        private int _BranchId;
        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeShiftController(IUserSession userSession, IAPIFactory apiFactory, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);
            Int32.TryParse(branchId, out _BranchId);
        }

        #endregion

        #region View
        public async Task<IActionResult> Index()
        {
            // Viewbag
            ViewBag.ShiftsSelect = await _ReadShiftSelect();

            return View();
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(int employeeId, int shiftId, int year, int month)
        {
            // Viewbag
            ViewBag.Shifts = await _ReadShiftSelect();
            ViewBag.CheckoutCounters = await _ReadCheckoutCounterSelect();

            var model = new EmployeeShiftViewModel();

            if (employeeId > 0)
                model = await _ReadByIdAsync(employeeId, shiftId, year, month);

            if (model.EmployeeId == 0)
            {
                model.EmployeeId = employeeId;
                model.ShiftId = shiftId;
                model.Year = year;
                model.Month = month;
                model.IsNew = true;
            }
            else
            {
                var response = await _apiFactory.GetAsync<EmployeeViewModel>(
                    apiUrl: $"Employee/ReadById?id={employeeId}",
                    endpoint: Endpoints.ApiPOS,
                    token: _userSession.BearerToken
                );
                model.Employee = response.Name;
                model.IsNew = false;
            }
     
            return PartialView(model);
        }
        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, int employeeId, int shiftId, string date)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            string[] DateStr = date.Split("/");
            var dateTime = DateTime.Parse(DateStr[2] + "-" + DateStr[1] + "-" + DateStr[0]);

            var response = await _apiFactory.GetAsync<List<EmployeeShiftDataTableViewModel>>(
                apiUrl: this.ApiResources($"Read?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&employeeId={employeeId}&shiftId={shiftId}&date={dateTime}&branchId={_BranchId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }


        #endregion

        #region Create
        [HttpPost]
        public async Task<ResponseContainer> Create(EmployeeShiftViewModel vm)
        {
          
            var result = await _apiFactory.PostAsync<EmployeeShiftViewModel, bool>(
                data: vm,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Create",
                Action = "Create",
                Succeeded = result
            };

            return response;
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int employeeId, int shiftId, int year, int month)
        {
            var result = await _apiFactory.DeleteAsync<bool>(
                apiUrl: this.ApiResources($"Delete?employeeId={employeeId}&shiftId={shiftId}&year={year}&month={month}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result
            };

            return response;
        }
        #endregion

        #region Methods
        private async Task<EmployeeShiftViewModel> _ReadByIdAsync(int employeeId, int shiftId, int year, int month)
        {
            var response = await _apiFactory.GetAsync<EmployeeShiftViewModel>(
                apiUrl: this.ApiResources($"ReadById?employeeId={employeeId}&shiftId={shiftId}&year={year}&month={month}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<IEnumerable<Select>> _ReadShiftSelect()
        {
            var PositionSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"Shift/ReadSelect?branchId=" + _BranchId,
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return PositionSelect;
        }

        private async Task<IEnumerable<Select>> _ReadCheckoutCounterSelect()
        {
            var PositionSelect = await _apiFactory.GetAsync<IEnumerable<Select>>
            (
                apiUrl: $"CheckoutCounter/ReadSelect?branchId=" + _BranchId,
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return PositionSelect;
        }
        #endregion


        #region Check
        public async Task<JsonResult> CheckExist(int employeeId, int shiftId, int year, int month)
        {

            var checkExist = await _apiFactory.GetAsync<int>
                (this.ApiResources($"CheckExist?employeeId={employeeId}&shiftId={shiftId}&year={year}&month={month}"),
                Endpoints.ApiPOS, _userSession.BearerToken);

            if (checkExist == 1)
                return Json(false);
            return Json(true);
        }
        #endregion

    }
}
