
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Controllers
{
    public class PrintTestController : Controller
    {
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        private string reportFolder;
        private string reportPath;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PrintTestController(IWebHostEnvironment hostingEnvironment)
        {

            _hostingEnvironment = hostingEnvironment;
            reportPath = $"{this._hostingEnvironment.WebRootPath}\\Reports\\";
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> PrintTemplate(int id)
        {

            try
            {
                await PrintBarcode();
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task PrintBarcode()
        {
            reportFolder = Environment.MachineName.Replace(" ", "");
            reportFolder = System.IO.Path.Combine(reportPath, reportFolder);
            if (Directory.Exists(reportFolder)) Directory.Delete(reportFolder, true);
            if (!Directory.Exists(reportFolder)) Directory.CreateDirectory(reportFolder);
            try
            {
                var reportFile = System.IO.Path.Combine(reportPath, "Barcode.rdlc");
                List<ReportParameter> reportParameters = new List<ReportParameter>();
                reportParameters.Add(new ReportParameter("Barcode", Helper.BarCodeHelper.Generation("4356345435")));


                LocalReport localReport = new LocalReport();
                localReport.ReportPath = reportFile;
                localReport.DataSources.Add(new ReportDataSource
                    ("DataSet"));
                localReport.SetParameters(reportParameters);
                Log.Information("Test printer 1");
                Export(localReport);
                Print();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>2.8in</PageWidth>
                <PageHeight>11.7in</PageHeight>
                <MarginTop>0.15in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0.15in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
            Log.Information("Test printer 2");
        }
        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name,
          string fileNameExtension, Encoding encoding,
          string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                0,
                0,
               280,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
            Log.Information("Test printer 3");
        }
        private void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            Log.Information("Test printer 4");
            if (!printDoc.PrinterSettings.IsValid)
            {
                Log.Information("Test printer 5.1");
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
                Log.Information("Test printer 5.2");
            }
        }
    }
}
