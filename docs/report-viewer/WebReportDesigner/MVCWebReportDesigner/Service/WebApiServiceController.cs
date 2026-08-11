using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MVCWebReportDesigner.Service
{
    public class WebApiServiceController : ApiController
    {
        [HttpPost]
        public object PostParamData([FromBody]string stateCode)
        {
            var data = StoreSales.GetData(stateCode);
            return data;
        }

        [HttpPost]
        public object PostParamInt([FromBody]int id)
        {
            return null;
        }

        [HttpGet]
        public object GetParamData(string stateCode)
        {
            var data = CustomerSales.GetData(stateCode);
            return data;
        }

        [HttpGet]
        public object GetData()
        {
            var data = StoreSales.GetData(string.Empty);
            return data;
        }
    }
}