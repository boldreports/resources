using BasicAuthApp.AuthHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BasicAuthApp
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            // Web API routes
            configuration.MapHttpAttributeRoutes();
            configuration.Filters.Add(new BasicAuthActionAttribute());
            configuration.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
        }
    }
}