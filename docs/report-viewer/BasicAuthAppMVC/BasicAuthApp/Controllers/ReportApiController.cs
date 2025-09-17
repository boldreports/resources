using BasicAuthApp.AuthHelper;
using BoldReports.Web.ReportViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BasicAuthApp.Controllers
{
   // [BasicAuthAction]
    public class ReportApiController : ApiController, IReportController
    {
        // Post action for processing the RDL/RDLC report
        public object PostReportAction(Dictionary<string, object> jsonResult)
        {
            return ReportHelper.ProcessReport(jsonResult, this);
        }

        // Get action for getting resources from the report
        [System.Web.Http.ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(string key, string resourcetype, bool isPrint)
        {
            return ReportHelper.GetResource(key, resourcetype, isPrint);
        }

        // Method that will be called when initialize the report options before start processing the report
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            // You can update report options here
        }

        // Method that will be called when reported is loaded
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            // You can update report options here
        }
    }
}