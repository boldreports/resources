using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BoldReports.Web.ReportViewer;

namespace Web_ReportViewer.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportViewerController : ApiController, IReportController
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

        }

        // Method that will be called when reported is loaded
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            // You can update report options here

            List<BoldReports.Web.ReportParameter> userParameters = new List<BoldReports.Web.ReportParameter>();
            userParameters.Add(new BoldReports.Web.ReportParameter()
            {
                Name = "ProductCategory",
                Values = new List<string>() { "4" }
            });
            reportOption.ReportModel.Parameters = userParameters;
        }
    }
}