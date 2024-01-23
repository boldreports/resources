using Microsoft.AspNetCore.Mvc;
using BoldReports.Web.ReportViewer;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RDLCDynamicBinding.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ReportViewerController : Controller, IReportController
    {
        private IMemoryCache _cache;
        private IWebHostEnvironment _hostingEnvironment;

        public ReportViewerController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }
        string productID = null;

        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonResult)
        {
            if (jsonResult != null)
            {
                if (jsonResult.ContainsKey("customData"))
                {
                    //Get client side custom data and store in local variable.
                    productID = jsonResult["customData"].ToString();
                }
            }

            return ReportHelper.ProcessReport(jsonResult, this, this._cache);
        }
        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, this._cache);
        }

        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            string basePath = _hostingEnvironment.WebRootPath;
            reportOption.ReportModel.ProcessingMode = ProcessingMode.Local;
            // Load the RDLC report file
            FileStream inputStream = new FileStream(basePath + @"\Resources\ProductList.rdlc", FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();

            reportOption.ReportModel.Stream = reportStream;

            // Set the data source directly
            reportOption.ReportModel.DataSources.Add(new BoldReports.Web.ReportDataSource
            {
                Name = "list", // Ensure this matches the dataset name in your RDLC file
                Value = ProductList.GetData().Where(x => x.ProductId.ToString() == productID).ToList()
            });
        }

        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, this._cache);
        }
    }
}
