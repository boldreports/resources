using BoldReports.Writer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebformsWriter
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ExportButton_Click(object sender, EventArgs e)
        {
            string fileName = null;
            WriterFormat format;
            HttpContext httpContext = System.Web.HttpContext.Current;
            BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter();
            writer.ReportPath = Server.MapPath("~/Resources/sales-order-detail.rdl");

            if (this.ExportFormat.SelectedValue == "PDF")
            {
                fileName = "sales-order-detail.pdf";
                format = WriterFormat.PDF;
            }
            else if (this.ExportFormat.SelectedValue == "Word")
            {
                fileName = "sales-order-detail.docx";
                format = WriterFormat.Word;
            }
            else if (this.ExportFormat.SelectedValue == "Html")
            {
                fileName = "sales-order-detail.Html";
                format = WriterFormat.HTML;
            }
            else if (this.ExportFormat.SelectedValue == "PPT")
            {
                fileName = "sales-order-detail.ppt";
                format = WriterFormat.PPT;
            }
            else
            {
                fileName = "sales-order-detail.xlsx";
                format = WriterFormat.Excel;
            }
            writer.Save(fileName, format, httpContext.Response);
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