using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace Webforms_sample
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //Register Bold licenseKey
            string licensekey = System.IO.File.ReadAllText(@"C:\Users\JeevalakshmiBaskaran\source\repos\Webforms sample\BoldLicense.txt");// Replace it with actual license key file path
            Bold.Licensing.BoldLicenseProvider.RegisterLicense(licensekey, isOfflineValidation: true);
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}