using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Claims;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class BeginingStockController : Controller
    {
        #region Constructor
        private int _BranchId;
        private IUserSession _userSession;
        private IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BeginingStockController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor)
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
        [HttpGet]
        public async Task<IActionResult> _FormExcel()
        {
            ViewBag.Stocks = (await _StockSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> _FormEditQuantity(int productId, int stockId, string year, int unitId)
        {
            var stock = await _ReadByYear(productId, stockId, year, unitId);

            return PartialView(stock);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, string year, int stockId, int productId)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            // No Need To Parse Anymore
            // var year = _ParseStringToYear(date);

            var response = await _apiFactory.GetAsync<List<BeginingStockDataTableViewModel>>(
                this.ApiResources($"Read?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}" +
                    $"&year={year}&stockId={stockId}&productId={productId}&branchId={_BranchId}"),
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }

        [HttpGet]
        public async Task<JsonResult> CheckIsLock(int stockId, string year)
        {
            // No Need To Parse Anymore
            // var year = _ParseStringToYear(date);

            // => Check is Lock
            var isLock = await _apiFactory.GetAsync<int>
            (
                apiUrl: this.ApiResources($"CheckIsLock?stockId={stockId}&year={year}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(isLock);
        }

        [HttpGet]
        public async Task<JsonResult> CheckHaveData(int stockId, string year)
        {
            // No Need To Parse Anymore
            // var year = _ParseStringToYear(date);

            // => Check is Forward
            var haveData = await _apiFactory.GetAsync<int>
            (
                apiUrl: $"BeginingStock/CheckHaveData?stockId={stockId}&year={year}",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(haveData);
        }


        #endregion

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(List<BeginingStockDataViewModel> model, string dateStr, int stockId)
        {
            var year = _ParseStringToYear(dateStr);

            model.Select(
                m => { 
                    m.Year = year; 
                    m.StockId = stockId; 
                    m.UserId = long.Parse(_userSession.UserId); 
                    return m; 
                }).ToList();

            var result = await _apiFactory.PostAsync<List<BeginingStockDataViewModel>, int>
            (
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
        public async Task<ResponseContainer> CreateExcel(BeginingStockExcelViewModel model)
        {
            ResponseContainer response = new ResponseContainer
            {
                Action = "Nhập từ Excel",
                Activity = "Nhập từ Excel",
                Succeeded = false
            };

            var resultList = _GetBeginingStockFromExcel(model);

            if (!resultList.success)
            {
                response.CustomMessage = "Nhập từ Excel thất bại! Vui lòng kiểm tra lại định dạng hoặc cấu trúc dữ liệu.";
                return response;
            }

            var result = await _apiFactory.PutAsync<List<BeginingStockDataViewModel>, int>(
                data: resultList.list,
                apiUrl: this.ApiResources("Upsert"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            response.Succeeded = result > 0;
            response.CustomMessage = "Nhập từ Excel thành công!";

            return response;
        }

        #endregion

        #region Update

        public async Task<ResponseContainer> UpdateQuantity(BeginingStockViewModel obj)
        {
            var result = await _apiFactory.PutAsync<BeginingStockViewModel, int>(
                data: obj,
                apiUrl: this.ApiResources($"UpdateQuantity"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            obj.UserId = long.Parse(_userSession.UserId);

            var response = new ResponseContainer
            {
                Action = "Cập nhật",
                Activity = "Cập nhật số lượng tồn kho",
                Succeeded = result > 0 ? true : false,
                CustomMessage = result > 0 ? "Cập nhật số lượng tồn kho thành công" : "Cập nhật số lượng tồn kho không thành công"
            };

            return response;
        }

        #endregion

        #region Lock/Unlock

        [HttpPut]
        public async Task<JsonResult> Lock(string year, int stockId)
        {
            var result = await _apiFactory.PutAsync<List<BeginingStockDataViewModel>, int>(
                data: null,
                apiUrl: this.ApiResources($"Lock?stockId={stockId}&year={year}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(result);
        }

        [HttpPut]
        public async Task<JsonResult> UnLock(string year, int stockId)
        {
            var result = await _apiFactory.PutAsync<List<BeginingStockDataViewModel>, int>(
                data: null,
                apiUrl: this.ApiResources($"Unlock?stockId={stockId}&year={year}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return Json(result);
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<ResponseContainer> DeleteAll(int stockId, string year)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"DeleteByYear?stockId={stockId}&year={year}"),
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
        public async Task<ResponseContainer> Delete(int productId, int stockId, string year, int unitId)
        {
            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?productId={productId}&stockId={stockId}&year={year}&unitId={unitId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = result > 0 ? true : false,
                CustomMessage = result > 0 ? "Xóa thành công." : "Xóa không thành công."
            };


            return response;
        }

        #endregion

        #region Methods

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
                apiUrl: "Stock/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return StockList;
        }

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

        private (bool success, List<BeginingStockDataViewModel> list) _GetBeginingStockFromExcel(BeginingStockExcelViewModel model)
        {

            var beginingStocks = new List<BeginingStockDataViewModel>();

            try
            {
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
                        do
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    beginingStocks.Add(
                                        new BeginingStockDataViewModel
                                        {
                                            StockId = model.StockId,
                                            ProductCode = reader.GetValue(1).ToString(),
                                            Quantity = (decimal)Convert.ToSingle(reader.GetValue(2)),
                                            UnitId = (int)Convert.ToSingle(reader.GetValue(3)),
                                            Year = model.Year,
                                            UserId = long.Parse(_userSession.UserId)
                                        }
                                    );
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                    throw ex;
                                }
                            }
                        } while (reader.NextResult());
                    }
                }

                // If can't read any row => return false
                if (beginingStocks.Count == 0)
                {
                    return (false, beginingStocks);
                }

                return (true, beginingStocks);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (false, beginingStocks);
            }
        }

        private bool _ValidateFileExcel(IFormFile File)
        {
            try
            {
                if (File == null || File.Length == 0)
                    return false;

                FileInfo file = new FileInfo(File.FileName);

                if (!file.Extension.Contains("xls"))
                    return false;

                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public string _ParseStringToYear(string date)
        {
            string[] datespl = date.Split("/");
            return datespl[2];
        }

        public async Task<BeginingStockViewModel> _ReadByYear(int productId, int stockId, string year, int unitId)
        {
            var response = await _apiFactory.GetAsync<BeginingStockDataTableViewModel>(
                apiUrl: this.ApiResources($"ReadByYear?productId={productId}&stockId={stockId}&year={year}&unitId={unitId}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        #endregion
    }
}
