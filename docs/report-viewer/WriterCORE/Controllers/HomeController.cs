using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BoldReports.Web;
using BoldReports.Writer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WriterCORE.Models;

namespace WriterCORE.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public HomeController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [HttpPost]
        public IActionResult Export(string writerFormat)
        {
            // Here, we have loaded the sales-order-detail sample report from application the folder wwwroot\Resources.
            //FileStream reportStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Resources\sales-order-detail.rdl", FileMode.Open, FileAccess.Read);
            FileStream reportStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Resources\MainReport.rdlc", FileMode.Open, FileAccess.Read);
            FileStream subReportStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Resources\SubReport1.rdlc", FileMode.Open, FileAccess.Read);
            //FileStream mainReportStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Resources\Side_By_SideMainReport.rdl", FileMode.Open, FileAccess.Read);
            //FileStream subReportStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Resources\Side_By_SideSubReport.rdl", FileMode.Open, FileAccess.Read);
            BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();

            //RDLC
            writer.ReportProcessingMode = ProcessingMode.Local;
            writer.SubreportProcessing += ReportWriter_SubreportProcessing;
            // Pass the dataset collection for report
            writer.DataSources.Clear();
            writer.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "list", Value = ProductList.GetData() });
            writer.LoadSubreport("SubReport1", subReportStream);

            //Loading the report going to export as PDF.
            writer.LoadReport(reportStream);
            // Report Server
            //writer.ReportProcessingMode = ProcessingMode.Remote;
            //writer.ReportServerUrl = "http://172.16.205.99/reportserver";
            //writer.ReportPath = "/Testing/manoranjan/Dataset11";
            //writer.ReportServerCredential = new System.Net.NetworkCredential("syncfusion", "coolcomp@123");

            // Sub report
            //writer.LoadSubreport("Side_By_SideSubReport", subReportStream);
            //writer.LoadReport(mainReportStream);

            //SetParameters
            //List<BoldReports.Web.ReportParameter> userParameters = new List<BoldReports.Web.ReportParameter>();
            //userParameters.Add(new BoldReports.Web.ReportParameter()
            //{
            //    Name = "SalesOrderNumber",
            //    Values = new List<string>() { "SO50756" }
            //});
            //writer.SetParameters(userParameters);

            string fileName = null;
            WriterFormat format;
            string type = null;

            if (writerFormat == "PDF")
            {
                fileName = "sales-order-detail.pdf";
                type = "pdf";
                format = WriterFormat.PDF;
            }
            else if (writerFormat == "Word")
            {
                fileName = "sales-order-detail.docx";
                type = "docx";
                format = WriterFormat.Word;
            }
            else if (writerFormat == "CSV")
            {
                fileName = "sales-order-detail.csv";
                type = "csv";
                format = WriterFormat.CSV;
            }
            else
            {
                fileName = "sales-order-detail.xlsx";
                type = "xlsx";
                format = WriterFormat.Excel;
            }

            MemoryStream memoryStream = new MemoryStream();
            writer.Save(memoryStream, format);

            // Download the generated export document to the client side.
            memoryStream.Position = 0;
            FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "application/" + type);
            fileStreamResult.FileDownloadName = fileName;
            return fileStreamResult;
        }

        private void ReportWriter_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "SubReport1")
            {
                //Pass the dataset collection for subreport
                e.DataSources.Clear();
                e.DataSources.Add(new ReportDataSource { Name = "list", Value = SubReport1.GetData() });
            }
        }

        public class ProductList
        {
            public string ProductName { get; set; }
            public string OrderId { get; set; }
            public double Price { get; set; }
            public string Category { get; set; }
            public string Ingredients { get; set; }
            public string ProductImage { get; set; }

            public static IList GetData()
            {
                List<ProductList> datas = new List<ProductList>();
                ProductList data = new ProductList()
                {
                    ProductName = "Baked Chicken and Cheese",
                    OrderId = "323B60",
                    Price = 55,
                    Category = "Non-Veg",
                    Ingredients = "grilled chicken, corn and olives.",
                    ProductImage = ""
                };
                datas.Add(data);
                data = new ProductList()
                {
                    ProductName = "Chicken Delite",
                    OrderId = "323B61",
                    Price = 100,
                    Category = "Non-Veg",
                    Ingredients = "cheese, chicken chunks, onions & pineapple chunks.",
                    ProductImage = ""
                };
                datas.Add(data);
                data = new ProductList()
                {
                    ProductName = "Chicken Tikka",
                    OrderId = "323B62",
                    Price = 64,
                    Category = "Non-Veg",
                    Ingredients = "onions, grilled chicken, chicken salami & tomatoes.",
                    ProductImage = ""
                };
                datas.Add(data);
                return datas;
            }
        }

        public class SubReport1
        {
            public string ProductName { get; set; }
            public string OrderId { get; set; }
            public double Price { get; set; }
            public string Category { get; set; }
            public string Ingredients { get; set; }
            public string ProductImage { get; set; }

            public static IList GetData()
            {
                List<SubReport1> datas = new List<SubReport1>();
                SubReport1 data = new SubReport1()
                {
                    ProductName = "Baked Chicken and Cheese",
                    OrderId = "323B60",
                    Price = 55,
                    Category = "Non-Veg",
                    Ingredients = "grilled chicken, corn and olives.",
                    ProductImage = ""
                };
                datas.Add(data);
                data = new SubReport1()
                {
                    ProductName = "Chicken Delite",
                    OrderId = "323B61",
                    Price = 100,
                    Category = "Non-Veg",
                    Ingredients = "cheese, chicken chunks, onions & pineapple chunks.",
                    ProductImage = ""
                };
                datas.Add(data);
                data = new SubReport1()
                {
                    ProductName = "Chicken Tikka",
                    OrderId = "323B62",
                    Price = 64,
                    Category = "Non-Veg",
                    Ingredients = "onions, grilled chicken, chicken salami & tomatoes.",
                    ProductImage = ""
                };
                datas.Add(data);
                return datas;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
