using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdwardSoft.Provider.API;
using AdwardSoft.Provider.Common;
using AdwardSoft.Provider.Helper;
using AdwardSoft.Security.Authorization;
using AdwardSoft.Security.Claims;
using AdwardSoft.Web.Inside.Models;
using AdwardSoft.Web.Inside.Models.Common;
using AspNetCore.Reporting;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace AdwardSoft.Web.Inside.Controllers
{
    [AdsAuthorization]
    public class SaleReceiptController : Controller
    {
        #region Contructor
        private int _BranchId;
        private readonly IUserSession _userSession;
        private readonly IAPIFactory _apiFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvirnoment;
        private Font printFont;
        private StreamReader streamToPrint;
        private string reportPdf;
        private string reportFolder;
        private string reportPath;
        private string pdfPrintContent;

        public SaleReceiptController(IUserSession userSession, IAPIFactory apiFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _userSession = userSession;
            _apiFactory = apiFactory;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvirnoment = webHostEnvironment;

            var branchId = ClaimsPrincipalIdentity.GetClaim("branch_id", _httpContextAccessor.HttpContext);

            Int32.TryParse(branchId, out _BranchId);

            reportPath = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\";

        }
        #endregion

        #region View
        public async Task<IActionResult> Index()
        {
            ViewBag.Shifts = await _apiFactory.GetAsync<List<Select>>("Shift/ReadSelect?userId=" + _userSession.UserId
                , Endpoints.ApiPOS, _userSession.BearerToken);
            ViewBag.CheckoutCounters = await _apiFactory.GetAsync<List<Select>>("CheckoutCounter/ReadSelect?branchId=" + _BranchId
                , Endpoints.ApiPOS, _userSession.BearerToken);
            ViewBag.PriceTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Wholesale Price", Value = "0"},
                new SelectListItem { Text = "Wholesale Price 2", Value = "1" },
                new SelectListItem { Text = "Retail Price", Value = "2", Selected = true },
                new SelectListItem { Text = "Retail Price 2", Value = "3" }
            };
            return View();
        }
        #endregion

        #region Form
        public async Task<IActionResult> _Form(string id)
        {
            ViewBag.PaymentMethods = await _apiFactory.GetAsync<List<Select>>("PaymentMethod/ReadSelect"
                , Endpoints.ApiPOS, _userSession.BearerToken);

            var CustomerDefault = await _apiFactory.GetAsync<CustomerViewModel>("Customer/ReadDefault"
               , Endpoints.ApiPOS, _userSession.BearerToken);

            ViewBag.CustomerDefault = CustomerDefault;

            ViewBag.PriceTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Retail Price", Value = "0",  Selected = true},
                new SelectListItem { Text = "Wholesale Price", Value = "1" }
            };

            var model = new SaleReceiptAllViewModel();
            model.Infor = new SaleReceiptViewModel();
            model.Infor.Date = DateTime.Now;
            model.Infor.EmployeeId = long.Parse(_userSession.UserId);
            model.Infor.CustomerId = CustomerDefault.Id;
            model.Infor.BranchId = _BranchId;
            model.Infor.PaymentMethodId = CustomerDefault.PaymentMethodId;
            model.Infor.CreateDate = DateTime.Now;
            model.Infor.CreatedUser =  long.Parse(_userSession.UserId);
            model.Infor.PriceType = 2; // RetailPrice


            //test
            model.Infor.ShiftId = 0;
            model.Infor.CheckoutCounterId = 0;

            model.Detail = new List<SaleReceiptDetailDataTableViewModel>();
            if (!string.IsNullOrEmpty(id))
            {
                model.Infor = await _apiFactory.GetAsync<SaleReceiptViewModel>("SaleReceipt/ReadById?id=" + id
                    , Endpoints.ApiPOS, _userSession.BearerToken);
                model.Detail = await _apiFactory.GetAsync<List<SaleReceiptDetailDataTableViewModel>>("SaleReceiptDetail/Read?saleReceiptId=" + id
                    , Endpoints.ApiPOS, _userSession.BearerToken);
            }

            model.Infor.ModifiedDate = DateTime.Now;
            model.Infor.ModifiedUser = long.Parse(_userSession.UserId);
            return View(model);
        }
        #endregion

        #region Read
        [HttpPost]
        public async Task<IActionResult> Read(DataTableAjaxPostModel model, string date, int shiftIdId, int checkoutCounterId)
        {
            if (String.IsNullOrEmpty(model.Search.Value))
            {
                model.Search.Value = "NULL";
            }
            string[] datespl = date.Split(" ");
            string[] datesplForm = datespl[0].Split("/");
            string[] datesplTo = datespl[2].Split("/");
            DateTime dateFrom = DateTime.Parse(datesplForm[2] + "-" + datesplForm[1] + "-" + datesplForm[0]);
            DateTime dateTo = DateTime.Parse(datesplTo[2] + "-" + datesplTo[1] + "-" + datesplTo[0]);

            var result = await _apiFactory.GetAsync<List<SaleReceiptDataTableViewModel>>(this.ApiResources("Read?pageSize=" + model.Length + "&skip=" + model.Start
                            + "&searchBy=" + model.Search.Value + "&shiftIdId=" + shiftIdId + "&checkoutCounterId=" + checkoutCounterId
                            + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo), Endpoints.ApiPOS, _userSession.BearerToken);

            long recordsTotal = result.Count() > 0 ? result.FirstOrDefault().Count : 0;
            return Json(new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = result });
        }
        #endregion

        #region Create
        [HttpPost]
        public async Task<JsonResult> Create(SaleReceiptAllViewModel model)
        {


            var resultInfo = await _apiFactory.PostAsync<SaleReceiptViewModel, string>(model.Infor, this.ApiResources("Create"), Endpoints.ApiPOS, _userSession.BearerToken);
            if (String.IsNullOrEmpty(resultInfo))
            {
                return Json(0);
            }

            model.Detail = model.Detail.Select(c => { c.SaleReceiptId = resultInfo; return c; }).ToList();
            var resultDetail = await _apiFactory.PostAsync<List<SaleReceiptDetailDataTableViewModel>, int>(model.Detail, "SaleReceiptDetail/CreateMultiple", Endpoints.ApiPOS, _userSession.BearerToken);
            if (resultDetail == 0)
            {
                var resultDelete = await _apiFactory.DeleteAsync<bool>(this.ApiResources("Promotion/Delete?id=" + resultDetail), Endpoints.ApiPOS, _userSession.BearerToken);

                return Json(0);
            }

            await PrintReceipt(resultInfo);

            return Json(1);
        }
        #endregion

        #region Update
        [HttpPut]
        public async Task<JsonResult> Update(SaleReceiptAllViewModel model)
        {
            var resultInfo = await _apiFactory.PutAsync<SaleReceiptViewModel, int>(model.Infor, this.ApiResources("Update"), Endpoints.ApiPOS, _userSession.BearerToken);
            if (resultInfo == 0)
            {
                return Json(0);
            }
            model.Detail = model.Detail.Select(c => { c.SaleReceiptId = model.Infor.Id; return c; }).ToList();
            var resultDetail = await _apiFactory.PostAsync<List<SaleReceiptDetailDataTableViewModel>, int>(model.Detail, "SaleReceiptDetail/CreateMultiple", Endpoints.ApiPOS, _userSession.BearerToken);
            if (resultDetail == 0)
            {
                return Json(0);
            }

            await PrintReceipt(model.Infor.Id);

            return Json(1);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<ResponseContainer> Delete(string id)
        {
            var result = await _apiFactory.DeleteAsync<bool>(this.ApiResources("Delete?id=" + id), Endpoints.ApiPOS, _userSession.BearerToken);
            var response = new ResponseContainer();
            response.Action = "delete";
            response.Activity = "Delete";
            response.Succeeded = result;
            return response;
        }
        #endregion

        #region Print

        private async Task PrintReceipt(string id)
        {
            //reportFolder =_userSession.FullName.Trim();
            reportFolder = Environment.MachineName.Replace(" ", ""); // HttpContext.Connection.RemoteIpAddress.ToString().Replace(".", "_").Replace(":", "_");
            reportFolder = System.IO.Path.Combine(reportPath, reportFolder);
            if (Directory.Exists(reportFolder)) Directory.Delete(reportFolder, true);
            if (!Directory.Exists(reportFolder)) Directory.CreateDirectory(reportFolder);
            //var Infor = await _apiFactory.GetAsync<SaleReceiptViewModel>("SaleReceipt/ReadById?id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);

            var master = await _apiFactory.GetAsync<SaleReceiptDisplayViewModel>("SaleReceipt/Display/Master?id=" + id, Endpoints.ApiPOS, _userSession.BearerToken);
            var detail = await _apiFactory.GetAsync<List<SaleReceiptDetailDataTableViewModel>>("SaleReceiptDetail/Read?saleReceiptId=" + id, Endpoints.ApiPOS, _userSession.BearerToken);

            //var customer = await _apiFactory.GetAsync<CustomerViewModel>("Customer/ReadById?id=" + Infor.CustomerId, Endpoints.ApiPOS,_userSession.BearerToken);
            // var counter = await _apiFactory.GetAsync<CheckoutCounterViewModel>("CheckoutCounter/ReadById?id=" + Infor.CheckoutCounterId, Endpoints.ApiPOS,_userSession.BearerToken);
            try
            {
                string mimtype = "";
                int extension = 1;
                //var path = $"{this._webHostEnvirnoment.WebRootPath}\\Reports\\";

                var reportFile = System.IO.Path.Combine(reportPath, "SaleReceipt.rdlc");

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("No", master.No);
                parameters.Add("EmployeeName", _userSession.FullName);
                parameters.Add("Date", master.Date.ToString("dd/MM/yyyy HH:ss"));
                parameters.Add("CheckoutCounterName", master.CheckoutCounterName);
                parameters.Add("TotalQuantity", String.Format("{0:#,##0.##}", master.TotalQuantity));
                parameters.Add("TotalAmount", String.Format("{0:#,##0.##}", master.TotalAmount));
                parameters.Add("Id", master.Id);
                parameters.Add("CustomerPhone", master.CustomerPhone);
                parameters.Add("CustomerName", master.CustomerName);
                parameters.Add("CustomerAddress", master.CustomerAddress);
                parameters.Add("PaymentMethodName", master.PaymentMethodName);

                LocalReport localReport = new LocalReport(reportFile);

                localReport.AddDataSource("DataSet", detail);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

                PrintWithProcess(result.MainStream, reportFolder);
                //ManualPrint(result.MainStream, reportPdf);
                //PrintPdf(result.MainStream);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Print(string id)
        {
            await PrintReceipt(id);
            return RedirectToAction("Index");
            //return File(result.MainStream, "application/pdf");
        }

        //private void PrintPdf(byte[] stream)
        //{
        //    try
        //    {
        //        Random random = new Random();
        //        string reportFileName = "sc-" + DateTime.Now.ToLocalTime().ToString("hhmmss") + random.Next(1, 100).ToString() + ".pdf";
        //        reportPdf = System.IO.Path.Combine(reportFolder, reportFileName);

        //        using (FileStream fs = new FileStream(reportPdf, FileMode.Create))
        //        {
        //            fs.Write(stream, 0, stream.Length);
        //        }


        //        PdfDocument document = 
        //        PdfDocumentRenderer renderer = new PdfDocumentRenderer();
        //        renderer.Document = new MigraDoc.DocumentObjectModel.Document();

        //        if (renderer != null)
        //        {
        //            int pageCount = renderer.FormattedDocument.PageCount;
        //            PrinterSettings ps = new PrinterSettings();
        //            PrintDocument printDoc = new PrintDocument();
        //            printDoc.PrinterSettings = ps;
        //            printFont = new Font("Arial", 9);

        //            printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 311, 1110); //80x297 
        //                                                                                         //printDoc.OriginAtMargins = true;

        //            // Set some preferences, our method should print a box with any 
        //            // combination of these properties being true/false.
        //            printDoc.DefaultPageSettings.Landscape = false;
        //            printDoc.DefaultPageSettings.Margins.Top = 39;
        //            printDoc.DefaultPageSettings.Margins.Left = 19;
        //            printDoc.DefaultPageSettings.Margins.Right = 0;
        //            printDoc.DefaultPageSettings.Margins.Bottom = 0;

        //            if (!printDoc.PrinterSettings.IsValid)
        //            {
        //                throw new Exception("Error: cannot find the default printer.");
        //            }
        //            else
        //            {
        //                // Creates a PrintDocument that simplyfies printing of MigraDoc documents
        //                MigraDocPrintDocument printDocument = new MigraDocPrintDocument();
        //                // Attach the current printer settings
        //                printDocument.PrinterSettings = printDoc.PrinterSettings;
        //                printDocument.DefaultPageSettings = printDoc.DefaultPageSettings;

        //                // Attach the current document renderer
        //                printDocument.Renderer = renderer;

        //                // Print the document
        //                printDocument.Print();
        //            }
        //        }
        //    }
        //    catch (Exception) { throw; }
        //    finally
        //    {
        //        //streamToPrint.Close();
        //    }
        //}

        private void ManualPrint(byte[] stream, string reportPdf)
        {
            Random random = new Random();
            string reportFileName = "sc-" + DateTime.Now.ToLocalTime().ToString("hhmmss") + random.Next(1, 100).ToString() + ".pdf";
            reportPdf = System.IO.Path.Combine(reportFolder, reportFileName);


            //var fileContentResult = File(result.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, reportFileName);

            using (FileStream fs = new FileStream(reportPdf, FileMode.Create))
            {
                fs.Write(stream, 0, stream.Length);
            }


            try
            {
                //streamToPrint = new StreamReader(reportPdf);

                using (PdfReader reader = new PdfReader(reportPdf))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        stringBuilder.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    }

                    pdfPrintContent = stringBuilder.ToString();


                    PrinterSettings ps = new PrinterSettings();
                    PrintDocument printDoc = new PrintDocument();
                    printDoc.PrinterSettings = ps;
                    printFont = new Font("Arial", 9);

                    printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 311, 1110); //80x297 
                                                                                                 //printDoc.OriginAtMargins = true;

                    // Set some preferences, our method should print a box with any 
                    // combination of these properties being true/false.
                    printDoc.DefaultPageSettings.Landscape = false;
                    printDoc.DefaultPageSettings.Margins.Top = 39;
                    printDoc.DefaultPageSettings.Margins.Left = 19;
                    printDoc.DefaultPageSettings.Margins.Right = 0;
                    printDoc.DefaultPageSettings.Margins.Bottom = 0;

                    if (!printDoc.PrinterSettings.IsValid)
                    {
                        throw new Exception("Error: cannot find the default printer.");
                    }
                    else
                    {
                        printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                        //m_currentPageIndex = 0;
                        printDoc.Print();

                    }
                }


            }
            catch (Exception) { throw; }
            finally
            {
                //streamToPrint.Close();
            }
        }
        private void PrintWithProcess(byte[] stream, string reportFolder)
        {
            Random random = new Random();
            string reportFileName = "sc-" + DateTime.Now.ToLocalTime().ToString("hhmmss") + random.Next(1, 100).ToString() + ".pdf";
            reportPdf = System.IO.Path.Combine(reportFolder, reportFileName);


            // var fileContentResult = File(result.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, reportFileName);

            using (FileStream fs = new FileStream(reportPdf, FileMode.Create))
            {
                fs.Write(stream, 0, stream.Length);
            }

            if (System.IO.File.Exists(reportPdf))
            {
                //Print
                //var _printer = _userSession.FullName.Trim();
                //var printerName = @"\\192.168.1." + _printer.Substring(0, 2) + @"\HHMART-" + _printer.Substring(2) + "-XP80";
                PrinterSettings ps = new PrinterSettings();
                //Log.Information(ps.PrinterName);
                //ps.PrinterName = printerName;
                
                Log.Information(ps.PrinterName);

                PrintDocument printDoc = new PrintDocument();
                printDoc.PrinterSettings = ps;
                

                //if (!printDoc.PrinterSettings.IsValid)
                //{
                //    throw new Exception("Error: cannot find the default printer.");
                //}
                //else
                //{
                    Process process = new Process();
                    //process.StartInfo.FileName = "lpr";
                    //process.StartInfo.Arguments = " -P " + printDoc.PrinterSettings.PrinterName + " " + reportPdf;
                    process.StartInfo.Verb = "printto";
                    process.StartInfo.FileName = reportPdf;
                    process.StartInfo.Arguments = printDoc.PrinterSettings.PrinterName;

                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.UseShellExecute = true;
                    
                    //process.EnableRaisingEvents = true;
                    //process.Exited += new EventHandler(ProcessPrintEnded);

                    process.Start();

                    //process.WaitForInputIdle();
                    //process.WaitForExit();
                //}
            }
        }

        private void ProcessPrintEnded(object sender, EventArgs e)
        {
            var process = sender as Process;
            if(process != null)
            {
                if (System.IO.File.Exists(reportPdf))
                    System.IO.File.Delete(reportPdf);
            }
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {

            ev.Graphics.PageUnit = GraphicsUnit.Display;
            //ev.Graphics.ResetTransform();
            //ev.Graphics.TranslateTransform((ev.MarginBounds.X - ev.PageSettings.HardMarginX) / 100.0F, (ev.MarginBounds.X - ev.PageSettings.HardMarginY) / 100.0F);


            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page. 
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);
            //var content = streamToPrint.ReadToEnd();
            ev.Graphics.DrawString(pdfPrintContent, printFont, Brushes.Black, leftMargin, yPos,new StringFormat());


            // Print each line of the file.
            //while (count < linesPerPage && 
            //   ((line = streamToPrint.ReadLine()) != null))
            //{
            //    yPos = topMargin + (count *
            //       printFont.GetHeight(ev.Graphics));
            //    ev.Graphics.DrawString(line, printFont, Brushes.Black,
            //       leftMargin, yPos, new StringFormat());
            //    count++;
            //}

            //// If more lines exist, print another page.
            //if (line != null)
            //    ev.HasMorePages = true;
            //else
            //    ev.HasMorePages = false;
        }
        #endregion
    }
}
