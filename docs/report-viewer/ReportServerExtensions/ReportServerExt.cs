using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace ReportServer
{
    class ReportingServerExt : BoldReports.RDL.ServerProcessor.ReportingServer
    {
        internal static Token Token;

        private T Deserialize<T>(string json)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        T DeserializeObj<T>(Stream str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(str);
            return (T)serializer.Deserialize(reader);
        }

        BoldReports.RDL.ServerProcessor.DataSourceDefinition GetDataSourceDefinition(ItemResponse response)
        {
            var _rptDefinition = new BoldReports.RDL.ServerProcessor.DataSourceDefinition();
            var _datasourceStream = GetFileToStream(response.FileContent);
            var _umpDefinition = DeserializeObj<DataSourceDefinition>(_datasourceStream);

            // Map properties from _umpDefinition to _rptDefinition
            // ...

            return _rptDefinition;
        }

        public override BoldReports.RDL.ServerProcessor.DataSourceDefinition GetDataSourceDefinition(string dataSource)
        {
            try
            {
                var _credential = ReportServerCredential as NetworkCredential;

                ItemRequest itemRequest = new ItemRequest
                {
                    ItemType = ItemType.Datasource,
                    ReportReferences = new List<string> { dataSource }
                };

                using (var proxy = new CustomWebClient())
                {
                    var ser = new DataContractJsonSerializer(typeof(ItemRequest));
                    var mem = new MemoryStream();
                    ser.WriteObject(mem, itemRequest);

                    UpdateServerUrlIfNeeded();

                    UpdateProxy(proxy, ReportServerUrl, _credential.UserName, _credential.Password);

                    var data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    var rdata = proxy.UploadString(new Uri($"{ReportServerUrl}/reports/data-sources/download"), "POST", data);
                    var result = JsonConvert.DeserializeObject<List<ItemResponse>>(rdata);

                    return GetDataSourceDefinition(result.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
            return null;
        }

        public override Stream GetReport()
        {
            try
            {
                var _credential = ReportServerCredential as NetworkCredential;

                var itemRequest = new ItemRequest
                {
                    ItemType = ItemType.Report,
                    ItemId = Guid.TryParse(ReportPath, out var itemId) ? itemId : Guid.Empty,
                    ServerPath = !Guid.TryParse(ReportPath, out _) ? ReportPath : null
                };

                using (var proxy = new CustomWebClient())
                {
                    var ser = new DataContractJsonSerializer(typeof(ItemRequest));
                    var mem = new MemoryStream();
                    ser.WriteObject(mem, itemRequest);

                    UpdateServerUrlIfNeeded();

                    UpdateProxy(proxy, ReportServerUrl, _credential.UserName, _credential.Password);

                    var data = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    var rdlData = proxy.UploadString(new Uri($"{ReportServerUrl}/reports/download"), "POST", data);
                    var result = JsonConvert.DeserializeObject<ItemResponse>(rdlData);

                    if (result.Status && result.ItemType == ItemType.Report)
                    {
                        throw new Exception("Report is in incorrect format");
                    }

                    return GetFileToStream(result.FileContent);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }
            return null;
        }

        private void UpdateServerUrlIfNeeded()
        {
            if (!ReportServerUrl.Contains("/site/"))
            {
                ReportServerUrl = $"{ReportServerUrl.Replace("/reporting/api", "").TrimEnd('/')}/reporting/api";
            }
        }

        private Stream GetFileToStream(byte[] _fileContent)
        {
            return new MemoryStream(_fileContent);
        }

        internal static void UpdateProxy(CustomWebClient proxy, string serverUrl, string userName, string password)
        {
            if (Token == null)
            {
                Token = GenerateToken(userName, password, serverUrl);
            }

            proxy.Headers["Content-type"] = "application/json";
            proxy.Headers["Authorization"] = $"{Token.token_type} {Token.access_token}";
            proxy.Encoding = Encoding.UTF8;
        }

        public static Token GenerateToken(string userName, string password, string serverUrl)
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

                var result = client.PostAsync($"{serverUrl}/token", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Token>(resultContent);
            }
        }
    }

    class CustomWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            var request = base.GetWebRequest(uri);
            request.Timeout = 4 * 60 * 1000; // Increase timeout
            return request;
        }
    }
}