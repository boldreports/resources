using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportServerIntermediateService.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace ReportServerIntermediateService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ActionResult Index()
        {
            ViewBag.ServiceAuthorizationToken = this.GenerateToken("guest@boldreports.com", "demo");
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }

        public string GenerateToken(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();

                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
                  });

                var result = client.PostAsync("https://on-premise-demo.boldreports.com/reporting/api/site/site1/token", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                var token = JsonConvert.DeserializeObject<Token>(resultContent);

                return token.token_type + " " + token.access_token;
            }
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Privacy()
        {
            return View();
        }

    }
}
public class Token
{
    public string access_token { get; set; }

    public string token_type { get; set; }

    public string expires_in { get; set; }

    public string userName { get; set; }

    public string serverUrl { get; set; }

    public string password { get; set; }
}