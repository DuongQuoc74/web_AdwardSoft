using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Claims;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers.ReceivingReport
{
    public class ReceivingReportController : Controller
    {
        #region Contructor
        private int _BranchId;
        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvirnoment;


        public ReceivingReportController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvirnoment = webHostEnvironment;

            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);

            Int32.TryParse(branchId, out _BranchId);


        }
        #endregion

        #region View
        public async Task<IActionResult> Index()
        {
            ViewBag.Suppliers = (await _SupplierSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Employees = (await _EmployeeSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(string id)
        {
            ViewBag.PaymentMethods = (await _PaymentMethodSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            ViewBag.Suppliers = (await _SupplierSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            var model = await _apiFactory.GetAsync<ReceivingReportTmpViewModel>("ReceivingReport/ReadById?id=" + id
                   , Endpoints.ApiPOS, _userSession.BearerToken);

            return PartialView(model);
        }
        #endregion

        #region Update
        [HttpPost]
        public async Task<ResponseContainer> Update(ReceivingReportTmpViewModel model)
        {
            model.ReceivingReport.ModifiedUser = long.Parse(_userSession.UserId);
            model.ReceivingReportDetail = model.ReceivingReportDetail.Select(c => { c.ReceivingReportId = model.ReceivingReport.Id; return c; }).ToList();
            var result = await _apiFactory.PutAsync<ReceivingReportTmpViewModel, bool>
            (
                 data: model,
                 apiUrl: this.ApiResources("Update"),
                 endpoint: Endpoints.ApiPOS,
                 token: _userSession.BearerToken
            );

            var response = new ResponseContainer
            {
                Activity = "Update",
                Action = "Update",
                Succeeded = result
            };

            return response;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStatus(ReceivingReportStatusViewModel model)
        {
            model.ModifiedUser = long.Parse(_userSession.UserId);
            var result = await _apiFactory.PutAsync<ReceivingReportStatusViewModel, bool>
            (
                 data: model,
                 apiUrl: this.ApiResources("UpdateStatus"),
                 endpoint: Endpoints.ApiPOS,
                 token: _userSession.BearerToken
            );
            return Json(result);
        }
        #endregion

        #region Read
        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, string date, int supplierId, long createUser)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<List<ReceivingReportDataTableViewModel>>(
                $"ReceivingReport/Read?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&dateFrom={dateConverted.dateFrom}&dateTo={dateConverted.dateTo}" +
                    $"&createUser={createUser}&supplierId={supplierId}&branchId={_BranchId}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        #endregion

        #region Function
        private async Task<List<Select>> _SupplierSelect()
        {
            var List = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Supplier/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return List;
        }        
        private async Task<List<Select>> _EmployeeSelect()
        {
            var List = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Employee/ReadSelectUser?branchId=" + _BranchId,
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return List;
        }

        private async Task<List<Select>> _PaymentMethodSelect()
        {
            var List = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"PaymentMethod/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return List;
        }

        private (DateTime dateFrom, DateTime dateTo) _CovertStrToDateRange(string date)
        {
            string[] datespl = date.Split(" ");
            string[] datesplForm = datespl[0].Split("/");
            string[] datesplTo = datespl[2].Split("/");

            DateTime dateFrom = DateTime.Parse(datesplForm[2] + "-" + datesplForm[1] + "-" + datesplForm[0]);
            DateTime dateTo = DateTime.Parse(datesplTo[2] + "-" + datesplTo[1] + "-" + datesplTo[0]);

            return (dateFrom, dateTo);
        }
        #endregion


        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(int id)
        {
            var result = await _apiFactory.DeleteAsync<bool>(
                    apiUrl: this.ApiResources($"Delete?Id={id}"),
                    endpoint: Endpoints.ApiPOS,
                    token: _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Activity = "Delete";
            response.Succeeded = result;

            return response;
        }
        #endregion

    }
}
