using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoldReports.Web;
using BoldReports.Writer;
namespace MVCWriter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        protected void ExportButton_Click(object sender, EventArgs e)
        {
        }
        [HttpPost]
        public ActionResult Export(string writerFormat)
        {
            // Here, we have loaded the sales-order-detail sample report from application the folder Resources.
            FileStream reportStream = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath(@"\Resources\sales-order-detail.rdl"), FileMode.Open, FileAccess.Read);
            BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter(reportStream);

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
            else if (writerFormat == "HTML")
            {
                fileName = "sales-order-detail.html";
                type = "html";
                format = WriterFormat.HTML;
            }
            else if (writerFormat == "PPT")
            {
                fileName = "sales-order-detail.ppt";
                type = "ppt";
                format = WriterFormat.PPT;
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
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
    }
}