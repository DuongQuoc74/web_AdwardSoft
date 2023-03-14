using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class ProductController : Controller
    {
        #region Constructor

        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private Font printFont;
        private StreamReader streamToPrint;
        private string reportPdf;
        private string reportFolder;
        private string reportPath;
        private string pdfPrintContent;

        public ProductController(IUserSession userSession, IAPIFactory apiFactory, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

            reportPath = $"{this._hostingEnvironment.WebRootPath}\\Reports\\";
        }
        
        #endregion

        #region View
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = (await _CategorySelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            
            return View();
        }
        #endregion

        #region Form
       
        public async Task<IActionResult> _Form(int id)
        {
            var model = new ProductDataViewModel { Id = id };

            if (model.Id > 0)
            {
                model = await _ReadById(id);
                model.Code = model.Code.TrimEnd();
            }
            else
            {
               // var list = new List<SellingPriceUnitViewModel>();
                //var tmp = new SellingPriceUnitViewModel();
                //list.Add(tmp);
               // model.UnitPrice = list;
            }
            
            ViewBag.Units = (await _UnitSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Stocks = (await _StockSelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });
            ViewBag.Categories = (await _CategorySelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return PartialView(model);
        }

        #endregion

        #region Read

        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, string[] listCategory)
        {
            var categoryIds = string.Join(',', listCategory);

            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var response = await _apiFactory.GetAsync<List<ProductDatatableViewModel>>(
                $"Product/ReadDatatable?pageSize={model.Length}&pageSkip={model.Start}&searchBy={model.Search.Value}&categoryIds={categoryIds}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        #endregion Read

        #region Create

        [HttpPost]
        public async Task<ResponseContainer> Create(ProductDataViewModel model)
        {
            if (String.IsNullOrEmpty(model.Image))
                model.Image = "/product.png";

            if (model.ImgFile != null)
                model.Image = await _CreateFileLocal(model.ImgFile);

            if (model.Code == null) model.Code = string.Empty;
            model.Code = model.Code.Trim();



            var result = await _apiFactory.PostAsync<ProductDataViewModel, int>(
                data: model,
                apiUrl: this.ApiResources("Create"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            //if (result > 0 && model.UnitPrice.Count > 0)
            //{
            //    model.UnitPrice.Select(c => { c.ProductId = result; return c; }).ToList();
            //    var resultUnitPrice = await _apiFactory.PostAsync<List<SellingPriceUnitViewModel>, int>(
            //     data: model.UnitPrice,
            //     apiUrl: this.ApiResources("CreateSellingPriceUnit"),
            //     endpoint: Endpoints.ApiPOS,
            //     token: _userSession.BearerToken);
            //}

            var response = new ResponseContainer
            {
                Activity = "Create",
                Action = "Create",
                Succeeded = result > 0 ? true : false
            };

            return response;
        }

        #endregion

        #region Update
        [HttpPut]
        public async Task<ResponseContainer> Update(ProductDataViewModel model)
        {
            if (String.IsNullOrEmpty(model.Image))
                model.Image = "";

            if (model.ImgFile != null)
            {
                //Delete old
                _DeleteFileLocal(model.Image);

                //create file local
                model.Image = await _CreateFileLocal(model.ImgFile);
            }

            model.Code = model.Code.PadRight(13);

            var result = await _apiFactory.PutAsync<ProductDataViewModel, int>(
                data: model,
                apiUrl: this.ApiResources("Update"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            var response = new ResponseContainer
            {
                Activity = "Update",
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
            var response = new ResponseContainer
            {
                Action = "Delete",
                Activity = "Delete",
                Succeeded = true
            };

            var result = await _apiFactory.DeleteAsync<int>(
                apiUrl: this.ApiResources($"Delete?Id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken);

            if (result <= 0)
            {
                response.Activity = "Data is using. Delete ";
                response.Succeeded = false;
            }

            return response;
        }

        #endregion

        #region Check Code

        public async Task<JsonResult> CheckCode(int id, string code)
        {
            if (!String.IsNullOrEmpty(code) && code.Length >= 5)
            {
                code = code.PadLeft(13, '0');
                var isExisting = await _apiFactory.GetAsync<int>
                         (
                             apiUrl: this.ApiResources($"CheckCodeIsExisting?id={id}&code={code}"),
                             endpoint: Endpoints.ApiPOS,
                             token: _userSession.BearerToken
                         );

                if (isExisting > 0)
                    return Json("Mã sản phẩm đã tồn tại");

                return Json(true);
            }
            else
            {
                return Json("The Code must be 5 to 13 character");
            }
        }

        #endregion

        #region Methods

        private async Task<List<Select>> _UnitSelect()
        {
            var units = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Unit/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return units;
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

        private async Task<List<Select>> _CategorySelect()
        {
            var categories = await _apiFactory.GetAsync<List<Select>>
            (
                apiUrl: "Category/ReadSelect",
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return categories;
        }

        private async Task<ProductDataViewModel> _ReadById(int id)
        {
            var response = await _apiFactory.GetAsync<ProductDataViewModel>(
                apiUrl: this.ApiResources($"ReadById?id={id}"),
                endpoint: Endpoints.ApiPOS,
                token: _userSession.BearerToken
            );

            return response;
        }

        private async Task<string> _CreateFileLocal(IFormFile file)
        {
            var fileName = "";
            var webRoot = _hostingEnvironment.WebRootPath;
            var type = System.IO.Path.GetExtension(file.FileName);
            var name = Guid.NewGuid().ToString("N") + type;
            var PathWithFolderName = System.IO.Path.Combine(webRoot, "file" + $@"\Product");
            if (!Directory.Exists(PathWithFolderName))
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(PathWithFolderName);
            }
            var fullPath = System.IO.Path.Combine(PathWithFolderName, name);
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
                fileName = name;
            }
            return fileName;
        }
        private void _DeleteFileLocal(string oldImageFullPath)
        {
            if (oldImageFullPath != null)
            {
                oldImageFullPath = oldImageFullPath.Replace(_configuration["DomainFile"] + "file" + $@"/Product" + $@"/", "");
                var webRoot = _hostingEnvironment.WebRootPath;
                var PathWithFolderName = System.IO.Path.Combine(webRoot, "file" + $@"\Product");
                var oldPath = System.IO.Path.Combine(PathWithFolderName, oldImageFullPath);
                if (!String.IsNullOrEmpty(oldPath) && System.IO.File.Exists(oldPath) && !String.IsNullOrEmpty(oldImageFullPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

        }

        #endregion

        #region Search
        [HttpGet]
        public async Task<JsonResult> Search(string code)
        {
            var response = await _apiFactory.GetAsync<ProductVoucherViewModel>(
                                    apiUrl: this.ApiResources($"SearchByCode?code={code}"),
                                    endpoint: Endpoints.ApiPOS,
                                    token: _userSession.BearerToken
                                );
            return Json(response);
        }

        [HttpGet]
        public async Task<JsonResult> SearchMobile(string code)
        {
            var response = await _apiFactory.GetAsync<ProductUnitSearch>(
                                    apiUrl: this.ApiResources($"SearchMobile?code={code}"),
                                    endpoint: Endpoints.ApiPOS,
                                    token: _userSession.BearerToken
                                );
            return Json(response);
        }

        [HttpGet]
        public async Task<List<ProductDataViewModel>> ReadByCategory(int categoryId)
        {
            var response = await _apiFactory.GetAsync<List<ProductDataViewModel>>("Product/ReadByCategoryId?categoryId="
                + categoryId, Endpoints.ApiPOS, _userSession.BearerToken);
            return response;
        }
        #endregion

        #region Print
        [HttpPost]
        public async Task<IActionResult> ReadPrint(DataTableAjaxPostModel model, string[] listCategory)
        {
            var categoryIds = string.Join(',', listCategory);

            if (String.IsNullOrEmpty(model.Search.Value))
                model.Search.Value = "NULL";

            var response = await _apiFactory.GetAsync<List<ProductPrintViewModel>>(
                $"Product/Print?pageSize=0&pageSkip=0&searchBy={model.Search.Value}&categoryIds={categoryIds}",
                Endpoints.ApiPOS,
                _userSession.BearerToken
            );

            int recordsTotal = response!= null && response.Any() ? response.FirstOrDefault().Count : 0;

            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
        }
        public async Task<IActionResult> Print()
        {
            ViewBag.Categories = (await _CategorySelect()).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Text });

            return View();
        }
        #endregion

        #region Print
        [HttpPost]
        public async Task<IActionResult> Print(List<int> ids)
        {
            foreach (var item in ids)
            {
                await PrintBarcode(item);
            }
            return Json(true);
        }


        private async Task PrintBarcode(int id)
        {
            reportFolder = Environment.MachineName.Replace(" ", "");
            reportFolder = System.IO.Path.Combine(reportPath, reportFolder);
            if (Directory.Exists(reportFolder)) Directory.Delete(reportFolder, true);
            if (!Directory.Exists(reportFolder)) Directory.CreateDirectory(reportFolder);       
            try
            {
                string mimtype = "";
                int extension = 1;


                var reportFile = System.IO.Path.Combine(reportPath, "Barcode.rdlc");

                var model = await _ReadById(id);

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Barcode", Helper.BarCodeHelper.Generation(model.Code));
           

                LocalReport localReport = new LocalReport(reportFile);

                localReport.AddDataSource("DataSet", parameters);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
                PrintWithProcess(result.MainStream, reportFolder);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> PrintTemplate(int id)
        {

            try
            {
                await PrintBarcode(id);
                return Ok(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReadPrintTemplate(int id)
        {
            reportFolder = Environment.MachineName.Replace(" ", "");
            reportFolder = System.IO.Path.Combine(reportPath, reportFolder);
            if (Directory.Exists(reportFolder)) Directory.Delete(reportFolder, true);
            if (!Directory.Exists(reportFolder)) Directory.CreateDirectory(reportFolder);
            try
            {
                string mimtype = "";
                int extension = 1;


                var reportFile = System.IO.Path.Combine(reportPath, "Barcode.rdlc");

                var model = await _ReadById(id);

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Barcode", Helper.BarCodeHelper.Generation(model.Code));


                LocalReport localReport = new LocalReport(reportFile);

                localReport.AddDataSource("DataSet", parameters);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);              
                return File(result.MainStream, "application/pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PrintWithProcess(byte[] stream, string reportFolder)
        {
            Random random = new Random();
            string reportFileName = "sc-" + DateTime.Now.ToLocalTime().ToString("hhmmss") + random.Next(1, 100).ToString() + ".pdf";
            reportPdf = System.IO.Path.Combine(reportFolder, reportFileName);



            using (FileStream fs = new FileStream(reportPdf, FileMode.Create))
            {
                fs.Write(stream, 0, stream.Length);
            }

            if (System.IO.File.Exists(reportPdf))
            {

                PrinterSettings ps = new PrinterSettings();


                Log.Information(ps.PrinterName);

                PrintDocument printDoc = new PrintDocument();
                printDoc.PrinterSettings = ps;



                Process process = new Process();

                process.StartInfo.Verb = "printto";
                process.StartInfo.FileName = reportPdf;
                process.StartInfo.Arguments = printDoc.PrinterSettings.PrinterName;

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = true;


                process.Start();
            }
        }     
        #endregion
    }
}
