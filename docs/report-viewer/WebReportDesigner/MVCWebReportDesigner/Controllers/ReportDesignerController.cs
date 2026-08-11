using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MVCWebReportDesigner.Controllers
{
    public class ReportDesignerController : Controller
    {
        public ActionResult Index()
        {
            ViewData["DataSource"] = AddWebAPI.GetData();
            return View();
        }

    }

    public class AddWebAPI
    {
        public string className { get; set; }
        public string name { get; set; }
        public string imageClass { get; set; }
        public string displayName { get; set; }

        public static IList GetData()
        {
            List<AddWebAPI> dataList = new List<AddWebAPI>();
            AddWebAPI data = null;
            data = new AddWebAPI() { className = "WebAPIDataSource", name = "WebAPI", imageClass = "e-reportdesigner-datasource-webapi", displayName = "WebAPI" };
            dataList.Add(data);
            return dataList;
        }
    }
}
