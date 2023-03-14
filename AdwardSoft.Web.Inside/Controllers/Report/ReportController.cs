using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Security.Claims;
using AdwardSoft.Web.Inside.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers.Report
{
    [AdsAuthorization]
    public class ReportController : Controller
    {
        #region Contructor
        private int _BranchId;
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;
            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);

            Int32.TryParse(branchId, out _BranchId);
        }
        #endregion

        #region Stock
        public IActionResult Stock()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReadStock(DataTableAjaxPostModel model, string date)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<List<ReportStockViewModel>>(
                $"Report/ReadStock?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&dateFrom={dateConverted.dateFrom}&dateTo={dateConverted.dateTo}&branchId={_BranchId}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        #endregion

        #region Receiving
        public async Task<IActionResult> Receiving()
        {
            ViewBag.Suppliers = (await _SupplierSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReadReceiving(DataTableAjaxPostModel model, string date, int supplierId)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<List<ReportReceivingViewModel>>(
                $"Report/ReadReceiving?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&dateFrom={dateConverted.dateFrom}&dateTo={dateConverted.dateTo}" +
                    $"&supplierId={supplierId}&branchId={_BranchId}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        #endregion

        #region SellingPrice
        public async Task<IActionResult> SellingPrice(string receivingReportId)
        {
            ViewBag.ReceivingReportId = receivingReportId;
            ViewBag.Suppliers = (await _SupplierSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReadSellingPrice(DataTableAjaxPostModel model, string date, int supplierId, string receivingReportId, string receivingReportTmpId)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            string url = "";

            if (!String.IsNullOrEmpty(receivingReportTmpId))
            {
                var dateNow = DateTime.Now;
                url = $"Report/ReadSellingPrice?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                        $"&dateFrom={dateNow}&dateTo={dateNow}" +
                        $"&supplierId={supplierId}&receivingReportId={receivingReportTmpId}&branchId={_BranchId}";
            }
            else
            {
                if (String.IsNullOrEmpty(receivingReportId))
                    receivingReportId = "NULL";

                var dateConverted = _CovertStrToDateRange(date);

                url = $"Report/ReadSellingPrice?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                        $"&dateFrom={dateConverted.dateFrom}&dateTo={dateConverted.dateTo}" +
                        $"&supplierId={supplierId}&receivingReportId={receivingReportId}&branchId={_BranchId}";
            }
                                
            var response = await _apiFactory.GetAsync<List<ReportSellingPriceViewModel>>(url,Endpoints.ApiPOS,_userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        #endregion

        #region ServiceDisplay
        public async Task<IActionResult> ServiceDisplay()
        {
            ViewBag.Suppliers = (await _SupplierSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReadServiceDisplay(DataTableAjaxPostModel model, string date, int supplierId)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<List<ReportServiceDisplayViewModel>>(
                $"Report/ReadServiceDisplay?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&dateFrom={dateConverted.dateFrom}&dateTo={dateConverted.dateTo}" +
                    $"&supplierId={supplierId}&branchId={_BranchId}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        #endregion

        #region Sale Report
        public IActionResult CustomerReport()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ReadCustomerReport(DataTableAjaxPostModel model)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var response = await _apiFactory.GetAsync<List<CustomerReportViewModel>>(
                $"Report/ReadCustomerReport?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}",
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
            var StockList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Supplier/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return StockList;
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

    }
}
