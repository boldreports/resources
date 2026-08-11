using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace BasicAuthApp.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]/{id?}")]
  //  [ApiController]
    public class ReportApiController : ControllerBase, IReportController
    {
        private IMemoryCache _cache;
        private IWebHostEnvironment _hostingEnvironment;

        public ReportApiController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public object PostReportAction([FromBody]Dictionary<string, object> jsonResult)
        {
            return ReportHelper.ProcessReport(jsonResult, this, this._cache);
        }

        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        [AllowAnonymous]
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        [HttpPost]
        [AllowAnonymous]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, this._cache);
        }

        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            string basePath = _hostingEnvironment.WebRootPath;
            FileStream reportStream = new FileStream(basePath + @"\DemoRDL.rdl", FileMode.Open, FileAccess.Read);
            reportOption.ReportModel.Stream = reportStream;
        }

        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }
    }
}
