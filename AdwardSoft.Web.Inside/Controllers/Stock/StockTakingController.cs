using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Claims;
using AdwardSoft.Utilities.Helper;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class StockTakingController : Controller
    {
        #region Constructor
        private int _BranchId;
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StockTakingController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;

            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);

            Int32.TryParse(branchId, out _BranchId);
        }

        #endregion

        #region Views

        public async Task<IActionResult> Index()
        {
            ViewBag.Stocks = (await _StockSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Products = (await _ProductSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return View();
        }

        #endregion

        #region Form

        public async Task<IActionResult> _Form()
        {
            ViewBag.Stocks = (await _StockSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Units = (await _UnitSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return PartialView();
        }

        public async Task<IActionResult> _FormExcel()
        {
            ViewBag.Stocks = (await _StockSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return PartialView();
        }

        public async Task<IActionResult> _FormEditQuantity(int productId, int stockId, DateTime date, int unitId)
        {
            var stock = await _ReadByDate(productId, stockId, date, unitId);

            return PartialView(stock);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, string date, int stockId, int productId)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var dateConverted = _CovertStrToDateRange(date);

            var response = await _apiFactory.GetAsync<List<StockTakingDatatableViewModel>>(
                $"StockTaking/Read?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&dateFrom={dateConverted.dateFrom}&dateTo={dateConverted.dateTo}" +
                    $"&stockId={stockId}&productId={productId}&branchId={_BranchId}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }

        [HttpGet]
        public async Task<JsonResult> CheckIsLock(int stockId, string date)
        {
            var dateTime = _ParseStringToDate(date);

            // => Check is Lock
            var isLock = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckIsLock?stockId={stockId}&date={dateTime}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(isLock);
        }

        [HttpGet]
        public async Task<JsonResult> CheckIsForward(int stockId, string date)
        {
            var dateTime = _ParseStringToDate(date);

            // => Check is Forward
            var isForward = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckIsForward?stockId={stockId}&date={dateTime}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(isForward);
        }

        [HttpGet]
        public async Task<JsonResult> CheckHaveData(int stockId, string date)
        {
            var dateTime = _ParseStringToDate(date);

            // => Check is Forward
            var haveData = await _apiFactory.GetAsync<int>
            (
                apiUrl: $"StockTaking/CheckHaveData?stockId={stockId}&date={dateTime}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(haveData);
        }

        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(List<StockTakingViewModel> model, string dateStr, int stockId)
        {
            var date = _ParseStringToDate(dateStr);

            model.Select(m => { m.Date = date; m.StockId = stockId; m.UserId = long.Parse(_userSession.UserId); return m; }).ToList();

            var result = await _apiFactory.PostAsync<List<StockTakingViewModel>, int>(
                data: model,
                apiUrl: this.ApiResources("CreateMulti"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            var response = new ResponseContainer
            {
                Activity = "Create",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> CreateExcel(StockTakingExcelViewModel model)
        {
            ResponseContainer response = new ResponseContainer
            {
                Action = "Create",
                Activity = "Create",
                Succeeded = false
            };

            var resultList = _GetStockTakingFromExcel(model);

            if (!resultList.success)
                return response;

            var result = await _apiFactory.PutAsync<List<StockTakingDataViewModel>, int> (
                data: resultList.list,
                apiUrl: this.ApiResources("Upsert"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            response.Succeeded = result > 0;

            return response;
        }

        #endregion

        #region Update

        public async Task<ResponseContainer> UpdateQuantity(StockTakingViewModel obj)
        {
            var result = await _apiFactory.PutAsync<StockTakingViewModel, int>(
                data: obj,
                apiUrl: this.ApiResources($"UpdateQuantity"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            obj.UserId = long.Parse(_userSession.UserId);

            var response = new ResponseContainer
            {
                Action = "Update",
                Activity = "Update quantity",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Lock/Unlock

        [HttpPut]
        public async Task<JsonResult> Lock(string date, int stockId)
        {
            var datetime = _ParseStringToDate(date);

            var result = await _apiFactory.PutAsync<List<StockTakingDataViewModel>, int>(
                data: null,
                apiUrl: this.ApiResources($"Lock?stockId={stockId}&date={datetime}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(result);
        }

        [HttpPut]
        public async Task<JsonResult> UnLock(string date, int stockId)
        {
            var datetime = _ParseStringToDate(date);

            var result = await _apiFactory.PutAsync<List<StockTakingDataViewModel>, int>(
                data: null,
                apiUrl: this.ApiResources($"Unlock?stockId={stockId}&date={datetime}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(result);
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<ResponseContainer> DeleteAll(int stockId, string date)
        {
            var datetime = _ParseStringToDate(date);

            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"DeleteByDate?stockId={stockId}&date={datetime}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        [HttpPost]
        public async Task<ResponseContainer> Delete(int productId, int stockId, DateTime date, int unitId)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?productId={productId}&stockId={stockId}&date={date}&unitId={unitId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Methods
        private async Task<List<Select>> _UnitSelect()
        {
            var StockList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Unit/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return StockList;
        }

        private async Task<List<Select>> _ProductSelect()
        {
            var ProductList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Product/ReadSelect",   
                endpoint: Endpoints.ApiPOS, 
                token: _userSession.BearerToken
            );

            return ProductList;
        }

        private async Task<List<Select>> _StockSelect()
        {
            var StockList = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: $"Stock/ReadSelectByBranch?branchId={_BranchId}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return StockList;
        }

        private (bool success, List<StockTakingDataViewModel> list) _GetStockTakingFromExcel(StockTakingExcelViewModel model)
        {
            var stockTakings = new List<StockTakingDataViewModel>();
            var date = _ParseStringToDate(model.DateStr);

            var validate = _ValidateFileExcel(model.File);

            if (!validate)
                return (false, null);

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = new MemoryStream())
            {
                model.File.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        var x = reader;

                        stockTakings.Add(
                            new StockTakingDataViewModel
                            {
                                StockId = model.StockId,
                                ProductCode = reader.GetValue(1).ToString(),
                                Quantity = Convert.ToSingle(reader.GetValue(2)),
                                UnitId = (int)Convert.ToSingle(reader.GetValue(3)),
                                Date = date,
                                UserId = long.Parse(_userSession.UserId)
                            }
                        );
                    }
                }
            }

            return (true, stockTakings);
        }

        private bool _ValidateFileExcel(IFormFile File)
        {
            if (File == null || File.Length == 0)
                return false;

            FileInfo file = new FileInfo(File.FileName);
            
            if (!file.Extension.Contains("xls"))
                return false;

            return true;
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

        public DateTime _ParseStringToDate(string date)
        {
            string[] datespl = date.Split("/");
            return DateTime.Parse(datespl[2] + "-" + datespl[1] + "-" + datespl[0]);
        }

        public async Task<StockTakingViewModel> _ReadByDate(int productId, int stockId, DateTime date, int unitId)
        {
            var response = await _apiFactory.GetAsync<StockTakingDatatableViewModel>(
                apiUrl: this.ApiResources($"ReadByDate?productId={productId}&stockId={stockId}&date={date}&unitId={unitId}"), 
                endpoint: Endpoints.ApiPOS, 
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion
    }
}
