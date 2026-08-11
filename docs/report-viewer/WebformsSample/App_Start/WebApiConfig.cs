using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BoldReports.Web;

namespace Webforms_sample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Use the below code to register extensions assembly into report viewer
            ReportConfig.DefaultSettings = new ReportSettings().RegisterExtensions(new List<string> { "BoldReports.Data.WebData" });
           
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional }
           );
        }
    }
}
