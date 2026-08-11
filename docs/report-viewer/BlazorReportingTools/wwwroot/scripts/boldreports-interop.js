//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;

window.BoldReports = {
    RenderViewer: function (elementID, reportViewerOptions) {
        $("#" + elementID).boldReportViewer({
            reportPath: reportViewerOptions.reportName,
            reportServiceUrl: reportViewerOptions.serviceURL
        });
    }
}
//namespace BlazorReportingTools.wwwroot.scripts
//{
//    public class boldreports_interop : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();


//            window.BoldReports = {
//                RenderViewer: function (elementID, reportViewerOptions) {
//                    $("#" + elementID).boldReportViewer({
//                        reportPath: reportViewerOptions.reportName,
//                        reportServiceUrl: reportViewerOptions.serviceURL
//                    });
//                }
//            }
//        }
//    }
//}
