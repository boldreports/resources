using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Bold.Licensing;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using BoldReports.Web;
using System.Reflection;


namespace Web_ReportViewer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //string License = File.ReadAllText(Server.MapPath("BoldLicense.txt"), Encoding.UTF8);
            //BoldLicenseProvider.RegisterLicense(License);

            System.Web.Http.GlobalConfiguration.Configuration.Routes.MapHttpRoute(
          name: "DefaultApi",
          routeTemplate: "api/{controller}/{action}/{id}",
          defaults: new { id = RouteParameter.Optional });

            AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);
        }
    }
}
