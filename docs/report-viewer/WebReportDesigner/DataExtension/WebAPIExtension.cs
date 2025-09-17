using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml;
using BoldReports.Data;
using System.Reflection;
using BoldReports.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoldReports.DataExtensions.WebAPI
{
    public class WebAPIExtension : BoldReports.Data.IDataExtension
    {
        private Dictionary<string, object> _customProperties;

        public BoldReports.Data.ConnectionProperties ConnectionProperties
        {
            get;
            set;
        }

        public BoldReports.Data.Command Command
        {
            get;
            set;
        }

        public RDL.DOM.DataSource DataSource
        {
            get;
            set;
        }

        public RDL.DOM.DataSet DataSet
        {
            get;
            set;
        }

        public ExtensionHelper Helper
        {
            get;
            set;
        }

        public Dictionary<string, object> CustomProperties
        {
            get
            {
                if (this._customProperties != null && this._customProperties.Count > 0)
                {
                    if (this._customProperties.ContainsKey("QueryDesignerEnabled"))
                    {
                        this._customProperties["QueryDesignerEnabled"] = this.IsValidSchemaPath().ToString();
                    }
                }
                return this._customProperties;
            }
            set
            {
                this._customProperties = value;
            }
        }

        public QueryDataInfo QueryInfo
        {
            get;
            set;
        }

        public QueryBuilderDesignInfo DesignInfo
        {
            get;
            set;
        }

        public WebAPIExtension()
        {
            this.QueryInfo = new QueryDataInfo();
        }

        #region Extension Interface Methods
        public bool TestConnection(out string error)
        {
            error = string.Empty;
            return true;
        }

        public object GetData(out string error)
        {
            try
            {
                error = string.Empty;
                this.updateQueryData(this.Command.Text);
                var settings = new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.DateTimeOffset
                };
                return this.GetReportData(
                    Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.InvokeServiceRequest(this.ConnectionProperties.ConnectionString).ToString(), settings));
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        public object GetQuerySchema(out string error)
        {
            try
            {
                error = string.Empty;
                this.updateQueryData(this.Command.Text);
                dynamic tableData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(this.InvokeServiceRequest(this.ConnectionProperties.ConnectionString).ToString());
                if (this.DesignInfo != null)
                {
                    this.Command.Text = this.BuildWebApiQuery(this.DesignInfo, true);
                }
                return this.GetReportData(tableData);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        public SchemaData GetDataSourceSchema(SchemaDataInfo schemaData, out string error)
        {
            try
            {
                error = string.Empty;
                return this.GetXmlSchemaLsit();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        public string GetCommandText(QueryBuilderDesignInfo designInfo, out string error)
        {
            try
            {
                error = string.Empty;
                this.DesignInfo = designInfo;
                return this.BuildWebApiQuery(designInfo, false);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
        #endregion

        #region Helpers
        #region Parsing QueryString
        private void updateQueryData(string queryInfo)
        {
            try
            {
                string queryString = this.GetJsonString(queryInfo);
                queryString = this.UpdateQueryParameter(queryString);
                QueryDataInfo JsonInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryDataInfo>(queryString);

                this.QueryInfo.Method = JsonInfo.Method;
                this.QueryInfo.ActionType = JsonInfo.ActionType;
                this.QueryInfo.RawData = JsonInfo.RawData;
                this.QueryInfo.Parameters = JsonInfo.Parameters;
                this.QueryInfo.Headers = JsonInfo.Headers;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred at Query String Deserialize: " + ex.Message);
            }
        }

        private string GetJsonString(string queryInfo)
        {
            try
            {
                if (!string.IsNullOrEmpty(queryInfo) && queryInfo.Contains(";"))
                {
                    string[] queryDatas = queryInfo.Split(';').Where(data => !string.IsNullOrEmpty(data)).ToArray();
                    string queryString = "{";
                    for (int index = 0; index < queryDatas.Length; index++)
                    {
                        if (!string.IsNullOrEmpty(queryDatas[index]) && queryDatas[index].Contains("="))
                        {
                            string[] datas = queryDatas[index].Split('=');
                            queryString = queryString + string.Format("\"{0}\":", datas[0]);
                            if (datas[0].Contains("Parameters") || datas[0].Contains("Headers") || datas[0].Contains("RawData"))
                            {
                                queryString = queryString + datas[1];
                            }
                            else
                            {
                                queryString = queryString + string.Format("\"{0}\"", datas[1]);
                            }

                            if (index < (queryDatas.Length - 1))
                            {
                                queryString = queryString + ",";
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Input String Format");
                        }
                    }
                    queryString = queryString + "}";
                    return queryString;
                }
                else
                {
                    throw new Exception("Invalid Input String Format");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string UpdateQueryParameter(string queryInfo)
        {
            if (this.Command.Parameters == null || this.Command.Parameters.Count < 0)
            {
                return queryInfo;
            }

            try
            {
                var cmdObj = JsonConvert.DeserializeObject<QueryDataInfo>(queryInfo);

                if (cmdObj != null)
                {
                    if (cmdObj.ActionType.ToLower() == "get")
                    {
                        if (cmdObj.Parameters != null && cmdObj.Parameters.Any())
                        {
                            queryInfo = this.UpdateQueryString(queryInfo);
                        }
                        else
                        {
                            cmdObj.Parameters = new List<ConnectionData>();
                            foreach (var cmdParam in this.Command.Parameters)
                            {
                                cmdObj.Parameters.Add(new ConnectionData() { Key = cmdParam.Name, Value = cmdParam.Value.ToString() });
                            }
                            queryInfo = JsonConvert.SerializeObject(cmdObj);
                        }
                    }
                    else
                    {
                        if (cmdObj.RawData != null)
                        {
                            queryInfo = this.UpdateQueryString(queryInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                queryInfo = this.updateQueryFormat(queryInfo);
            }
            return queryInfo;
        }

        private string UpdateQueryString(string queryString)
        {
            Regex queryRegex = new Regex(@"(\@)([a-zA-Z0-9]+)", RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                foreach (Match match in queryRegex.Matches(queryString))
                {
                    if (match.Success)
                    {
                        foreach (var cmdParam in this.Command.Parameters)
                        {
                            if (string.Equals(cmdParam.Name, match.Value, StringComparison.InvariantCultureIgnoreCase))
                            {
                                queryString = queryString.Replace(cmdParam.Name, cmdParam.Value == null ? string.Empty : cmdParam.Value.ToString());
                            }
                        }
                    }
                }
            }
            return queryString;
        }

        private string updateQueryFormat(string resultquery)
        {
            foreach (var cmdParam in this.Command.Parameters)
            {
                string cmdParamValue = JsonConvert.SerializeObject(string.IsNullOrEmpty(cmdParam.Value.ToString()) ? string.Empty : cmdParam.Value);
                if (cmdParam.Value != null && cmdParam.Value.ToString().Contains(","))
                {
                    cmdParamValue = string.Empty;
                    string[] multiValues = cmdParam.Value.ToString().Split(',');

                    for (int value = 0; value < multiValues.Length; value++)
                    {
                        cmdParamValue = cmdParamValue + JsonConvert.SerializeObject(multiValues[value].Trim());
                        if (value < multiValues.Length - 1)
                        {
                            cmdParamValue = cmdParamValue + ", ";
                        }
                    }
                }
                resultquery = resultquery.Replace(cmdParam.Name, cmdParamValue == null ? string.Empty : cmdParamValue);
            }
            return resultquery;
        }

        private bool IsValidSchemaPath()
        {
            if (this.ConnectionProperties != null && this.ConnectionProperties.CustomProperties != null)
            {
                RDL.DOM.CustomProperty SchemaFile = this.ConnectionProperties.CustomProperties.Where(data => data.Name == "SchemaPath").FirstOrDefault();
                if (SchemaFile != null)
                {
                    return this.IsFileExists(SchemaFile.Value);
                }
            }
            return false;
        }
        #endregion

        #region Service Process
        private object InvokeServiceRequest(string serviceUrl)
        {
            HttpClient client = new HttpClient();

            if (this.ConnectionProperties != null && this.ConnectionProperties.CustomProperties != null)
            {
                RDL.DOM.CustomProperty securityType = this.ConnectionProperties.CustomProperties.Where(data => data.Name == "SecurityType").FirstOrDefault();
                if (securityType != null && securityType.Value == "Windows")
                {
                    HttpClientHandler authtHandler = new HttpClientHandler()
                    {
                        Credentials = CredentialCache.DefaultNetworkCredentials
                    };
                    client = new HttpClient(authtHandler);
                }
            }

            using (client)
            {
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpRequestMessage request = new HttpRequestMessage();
                    this.UpdateHttpRequestHeaders(request, this.QueryInfo.Headers);
                    string methodName = (string.IsNullOrEmpty(this.QueryInfo.Method) ? string.Empty : this.QueryInfo.Method);

                    if (!string.IsNullOrEmpty(this.QueryInfo.ActionType))
                    {
                        switch (this.QueryInfo.ActionType.ToLower())
                        {
                            case "post":
                                request.Method = System.Net.Http.HttpMethod.Post;
                                request.RequestUri = new Uri(serviceUrl + "/" + methodName);
                                string queryParam = this.GetQueryParam(this.QueryInfo.RawData);
                                queryParam = !string.IsNullOrEmpty(queryParam) ? queryParam : string.Empty;
                                request.Content = new StringContent(queryParam, Encoding.UTF8, "application/json");
                                response = client.SendAsync(request).Result;
                                break;
                            case "get":
                                request.Method = System.Net.Http.HttpMethod.Get;
                                string param = this.GetQueryParam(this.QueryInfo.Parameters);
                                request.RequestUri = new Uri(serviceUrl + "/" + methodName + param);
                                response = client.SendAsync(request).Result;
                                break;
                            default:
                                throw new Exception("Exception occurred at Service Request: Invalid Action Type: " + this.QueryInfo.ActionType);
                        }

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (response.Content.Headers.ContentType.MediaType.Equals("application/json"))
                            {
                                return response.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                throw new Exception("Exception occurred at Service Request: Response of the API must be in JSON format.");
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("Exception occurred at Service Request: Response Status: {0}", response.StatusCode));
                        }
                    }
                    else
                    {
                        throw new Exception("Exception occurred at Service Request: Action Type is not available");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private void UpdateHttpRequestHeaders(HttpRequestMessage httpRequest, List<ConnectionData> headers)
        {
            httpRequest.Headers.Accept.Clear();
            if (headers != null && headers.Count() > 0)
            {
                foreach (ConnectionData header in headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }
            }
            if (this.ConnectionProperties != null && this.ConnectionProperties.CustomProperties != null)
            {
                RDL.DOM.CustomProperty securityType = this.ConnectionProperties.CustomProperties.Where(data => data.Name == "SecurityType").FirstOrDefault();
                if (securityType != null)
                {
                    if (securityType.Value == "Basic Http")
                    {
                        RDL.DOM.CustomProperty credential = this.ConnectionProperties.CustomProperties.Where(data => data.Name == "Credential").FirstOrDefault();
                        if (credential != null)
                        {
                            httpRequest.Headers.Add("Authorization", "Basic " + credential.Value);
                        }
                    }
                    else if (securityType.Value == "Custom")
                    {
                        RDL.DOM.CustomProperty customHeaders = this.ConnectionProperties.CustomProperties.Where(data => data.Name == "Headers").FirstOrDefault();
                        if (customHeaders != null)
                        {
                            this.updateCustomHeaders(httpRequest, customHeaders.Value);
                        }
                    }
                }
            }
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void updateCustomHeaders(HttpRequestMessage httpRequest, string headerString)
        {
            if (!string.IsNullOrEmpty(headerString) && headerString.Contains(";"))
            {
                string[] headers = headerString.Split(';').Where(data => !string.IsNullOrEmpty(data)).ToArray();
                for (int index = 0; index < headers.Length; index++)
                {
                    if (!string.IsNullOrEmpty(headers[index]) && headers[index].Contains(":"))
                    {
                        string[] header = headers[index].Split(':');
                        httpRequest.Headers.Add(header[0], header[1]);
                    }
                }
            }
        }

        private string GetQueryParam(object rawParameters)
        {
            string queryParam = string.Empty;
            if (rawParameters != null)
            {
                queryParam = Newtonsoft.Json.JsonConvert.SerializeObject(rawParameters);
            }
            return queryParam;
        }

        private string GetQueryParam(List<ConnectionData> parameters)
        {
            string queryParam = string.Empty;
            if (parameters != null && parameters.Count > 0)
            {
                queryParam = queryParam + "?";
                foreach (var param in parameters)
                {
                    if (parameters.IndexOf(param) != 0)
                    {
                        queryParam = queryParam + "&";
                    }
                    queryParam = queryParam + param.Key + "=" + param.Value;
                }
            }
            return queryParam;
        }
        #endregion

        #region XMLParsing
        private bool IsFileExists(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    response.Close();
                    return (response.StatusCode == HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        private Stream DownloadString(string address)
        {
            Stream xmlData = new MemoryStream();
            if (!string.IsNullOrEmpty(address))
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        byte[] fileData = client.DownloadData(address);
                        xmlData = new MemoryStream(fileData);
                    }
                    return xmlData;
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid URL" + ex.Message);
                }
            }
            return xmlData;
        }

        private SchemaData GetXmlSchemaLsit()
        {
            try
            {
                if (this.ConnectionProperties != null && this.ConnectionProperties.CustomProperties != null)
                {
                    RDL.DOM.CustomProperty SchemaFile = this.ConnectionProperties.CustomProperties.Where(data => data.Name == "SchemaPath").FirstOrDefault();
                    if (SchemaFile != null)
                    {
                        if (this.IsFileExists(SchemaFile.Value))
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(DownloadString(SchemaFile.Value));
                            Dictionary<string, object> xmlSchema = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(this.XmlToJSON(xmlDoc));

                            if (xmlSchema != null && xmlSchema.Count > 0)
                            {
                                List<SchemaData> _schemas = new List<SchemaData>();
                                if (xmlSchema.ContainsKey("Schema") && xmlSchema["Schema"] != null)
                                {
                                    var schema = xmlSchema["Schema"] as Dictionary<string, object>;
                                    var schemaData = new SchemaData();
                                    schemaData.Name = schema["Name"].ToString();
                                    schemaData.SchemaType = SchemaTypes.Schema;
                                    var data = new List<SchemaData>();
                                    var xmlMethod = schema["Method"];
                                    if (xmlMethod != null)
                                    {
                                        if (xmlMethod is Dictionary<string, object>)
                                        {
                                            SchemaData procedure = this.GetSchemaProcedure(xmlMethod as Dictionary<string, object>);
                                            if (procedure != null)
                                            {
                                                data.Add(procedure);
                                            }
                                        }
                                        else if (xmlMethod is ArrayList)
                                        {
                                            List<SchemaData> proceduresList = this.GetProcedureList(xmlMethod as ArrayList);
                                            if (proceduresList != null)
                                            {
                                                data.AddRange(proceduresList);
                                            }
                                        }
                                    }
                                    schemaData.Data = data.Any() ? data : null;
                                    _schemas.Add(schemaData);
                                }
                                else
                                {
                                    throw new Exception("Invalid Schema informations. Root Node \"Controller\" not found.");
                                }

                                SchemaData rootData = new SchemaData();
                                rootData.Name = "Schema";
                                rootData.SchemaType = SchemaTypes.Database;
                                rootData.Data = _schemas;
                                return rootData;
                            }
                            else
                            {
                                throw new Exception("Schema informations are not available.");
                            }
                        }
                        else
                        {
                            throw new Exception("File not found from the URL or Invalid Xml Document");
                        }
                    }
                    else
                    {
                        throw new Exception("SchemaPath Not found from the CustomProperty");
                    }
                }
                else
                {
                    throw new Exception("Instance not created for CustomProperties");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception Occurred at Parsing Schema file: " + ex.Message);
            }
        }

        private string XmlToJSON(XmlDocument xmlDoc)
        {
            StringBuilder sbJSON = new StringBuilder();
            sbJSON.Append("{ ");
            this.XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
            sbJSON.Append("}");
            return sbJSON.ToString();
        }

        private void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            if (showNodeName)
                sbJSON.Append("\"" + this.GetXmlString(node.Name) + "\": ");
            sbJSON.Append("{");
            SortedList childNodeNames = new SortedList();

            if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    this.SetChildNode(childNodeNames, attr.Name, attr.InnerText);

            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode is XmlText)
                    this.SetChildNode(childNodeNames, "value", childNode.InnerText);
                else if (childNode is XmlElement)
                    this.SetChildNode(childNodeNames, childNode.Name, childNode);
            }

            foreach (string childname in childNodeNames.Keys)
            {
                ArrayList childNodes = (ArrayList)childNodeNames[childname];
                if (childNodes.Count == 1)
                    this.SetXmlString(childname, childNodes[0], sbJSON, true);
                else
                {
                    sbJSON.Append(" \"" + this.GetXmlString(childname) + "\": [ ");
                    foreach (object child in childNodes)
                        this.SetXmlString(childname, child, sbJSON, false);
                    sbJSON.Remove(sbJSON.Length - 2, 2);
                    sbJSON.Append(" ], ");
                }
            }
            sbJSON.Remove(sbJSON.Length - 2, 2);
            sbJSON.Append(" }");
        }

        private void SetChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
        {
            if (nodeValue is XmlElement)
            {
                XmlNode childNode = (XmlNode)nodeValue;
                if (childNode.Attributes.Count == 0)
                {
                    XmlNodeList children = childNode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
            object oldValues = childNodeNames[nodeName];
            ArrayList Values;
            if (oldValues == null)
            {
                Values = new ArrayList();
                childNodeNames[nodeName] = Values;
            }
            else
                Values = (ArrayList)oldValues;
            Values.Add(nodeValue);
        }

        private void SetXmlString(string childName, object childNode, StringBuilder sbJSON, bool showNodeName)
        {
            if (childNode == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + this.GetXmlString(childName) + "\": ");
                sbJSON.Append("null");
            }
            else if (childNode is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + this.GetXmlString(childName) + "\": ");
                string childString = (string)childNode;
                childString = childString.Trim();
                sbJSON.Append("\"" + this.GetXmlString(childString) + "\"");
            }
            else
                this.XmlToJSONnode(sbJSON, (XmlElement)childNode, showNodeName);
            sbJSON.Append(", ");
        }

        private string GetXmlString(string childString)
        {
            StringBuilder outString = new StringBuilder(childString.Length);
            foreach (char ch in childString)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    outString.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    outString.Append('\\');
                }
                outString.Append(ch);
            }
            return outString.ToString();
        }
        #endregion

        #region SchemaInformations
        private List<SchemaData> GetProcedureList(ArrayList xmlTableList)
        {
            List<SchemaData> _schemas = new List<SchemaData>();

            if (xmlTableList != null && xmlTableList.Count > 0)
            {
                foreach (var item in xmlTableList)
                {
                    _schemas.Add(this.GetSchemaProcedure(item as Dictionary<string, object>));
                }
                return _schemas;
            }
            return null;
        }

        private SchemaData GetSchemaProcedure(Dictionary<string, object> xmlTable)
        {
            if (xmlTable != null && xmlTable.Count > 0)
            {
                SchemaData schemaProcedure = new SchemaData();
                schemaProcedure.SchemaType = SchemaTypes.Procedure;
                schemaProcedure.CustomProperties = new Dictionary<string, object>();
                List<SchemaData> tableData = new List<SchemaData>();

                foreach (var item in xmlTable)
                {
                    switch (item.Key.ToLower())
                    {
                        case "name":
                            schemaProcedure.Name = item.Value.ToString();
                            break;
                        case "parameter":
                            var xmlParameters = xmlTable["Parameter"];
                            if (xmlParameters != null)
                            {
                                if (xmlParameters is Dictionary<string, object>)
                                {
                                    SchemaData parameter = this.GetProcParam(xmlParameters as Dictionary<string, object>);
                                    if (parameter != null)
                                    {
                                        tableData.Add(parameter);
                                    }
                                }
                                else if (xmlParameters is ArrayList)
                                {
                                    List<SchemaData> parameters = this.GetProcParameters(xmlParameters as ArrayList);
                                    if (parameters != null)
                                    {
                                        tableData.AddRange(parameters);
                                    }
                                }
                            }
                            schemaProcedure.Data = tableData.Any() ? tableData : null;
                            break;
                        case "method":
                            schemaProcedure.CustomProperties.Add(item.Key, item.Value);
                            break;
                        case "actiontype":
                            schemaProcedure.CustomProperties.Add(item.Key, item.Value);
                            break;
                        case "rawdata":
                            schemaProcedure.CustomProperties.Add(item.Key, item.Value);
                            break;
                        case "headers":
                            schemaProcedure.CustomProperties.Add(item.Key, item.Value);
                            break;
                    }
                }
                return schemaProcedure;
            }
            return null;
        }

        private List<SchemaData> GetProcParameters(ArrayList xmlProcParams)
        {
            List<SchemaData> _schemas = new List<SchemaData>();

            if (xmlProcParams != null && xmlProcParams.Count > 0)
            {
                foreach (var item in xmlProcParams)
                {
                    SchemaData procedureParam = this.GetProcParam(item as Dictionary<string, object>);
                    _schemas.Add(procedureParam);
                }
            }
            return _schemas;
        }

        private SchemaData GetProcParam(Dictionary<string, object> procParam)
        {
            SchemaData procedureParam = new SchemaData();
            Dictionary<string, string> procParams = new Dictionary<string, string>();
            if (procParam != null && procParam.Count > 0)
            {
                procedureParam.Name = procParam["Name"].ToString();
                procedureParam.SchemaType = SchemaTypes.Parameter;
                procedureParam.Parameter = new ProcParameter()
                {
                    DataType = procParam["Type"].ToString()
                };
                return procedureParam;
            }
            return null;
        }

        private string BuildWebApiQuery(QueryBuilderDesignInfo queryInfo, bool IsSave)
        {
            var query = string.Empty;
            if (queryInfo != null && queryInfo.StoredProcedure != null && queryInfo.StoredProcedure.CustomProperties != null
                && queryInfo.StoredProcedure.CustomProperties.Count > 0)
            {
                foreach (var item in queryInfo.StoredProcedure.CustomProperties)
                {
                    query = query + string.Format("{0}={1}", item.Key, item.Value);
                    if (queryInfo.StoredProcedure.CustomProperties.ToList().IndexOf(item) < (queryInfo.StoredProcedure.CustomProperties.Count - 1))
                    {
                        query = query + ";";
                    }
                }
                if (!IsSave)
                {
                    string parameters = this.UpdateParameters();
                    query = query + (string.IsNullOrEmpty(parameters) ? string.Empty : ";" + parameters);
                }
            }
            else
            {
                throw new Exception("Exception occurred at Command Text: SchemaInformations are not found to frame the Command Text.");
            }
            return query;
        }

        private string UpdateParameters()
        {
            string paramString = string.Empty;
            if (this.Command.Parameters != null && this.Command.Parameters.Count > 0)
            {
                paramString = "Parameters=[";
                foreach (var param in this.Command.Parameters)
                {
                    paramString = paramString + "{" + string.Format("\"Key\":\"{0}\",\"Value\":\"{1}\"", param.Name, param.Value.ToString()) + "}";
                    if (this.Command.Parameters.IndexOf(param) < (this.Command.Parameters.Count - 1))
                    {
                        paramString = paramString + ",";
                    }
                }
                paramString = paramString + "]";
            }
            return paramString;
        }
        #endregion

        #region Build ReportData
        private List<Dictionary<string, object>> GetReportData(dynamic datasource)
        {
            List<Dictionary<string, object>> reportData = new List<Dictionary<string, object>>();
            if (datasource != null && datasource is IEnumerable)
            {
                foreach (var data in datasource)
                {
                    reportData.Add(this.GetRecords(data));
                }
            }
            return reportData;
        }

        Dictionary<string, object> GetRecords(object data)
        {
            if (data is IDictionary<string, object>)
            {
                try
                {
                    return new Dictionary<string, object>((data as IDictionary<string, object>));
                }
                catch
                {
                    return null;
                }
            }
            else if (data is System.Data.DataRowView)
            {
                Dictionary<string, object> fieldValues = new Dictionary<string, object>();
                var items = (data as System.Data.DataRowView).Row.ItemArray;
                var row = (data as System.Data.DataRowView).Row;
                for (int pos = 0; pos < items.Count(); pos++)
                {
                    fieldValues.Add(row.Table.Columns[pos].ColumnName, items[pos]);
                }
                return fieldValues;
            }
            else if (data is JObject)
            {
                var settings = new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.None
                };
                var property = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.ToString(), settings);
                Dictionary<string, object> fieldValues = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> prop in property)
                {
                    if (prop.Value is JArray)
                    {
                        fieldValues.Add(prop.Key, GetReportData(prop.Value));
                    }
                    else if (prop.Value is JObject)
                    {
                        fieldValues.Add(prop.Key, GetRecords(prop.Value).ToString());
                    }
                    else
                    {
                        fieldValues.Add(prop.Key, prop.Value);
                    }
                }

                return fieldValues;
            }
            return null;
        }

        public Dictionary<string, SchemaData> GetTableColumns(List<SchemaDataInfo> schemaData, out string error)
        {
            throw new NotImplementedException();
        }

        public List<Joiner> ValidateAutoJoin(List<string> tableNames, out string error)
        {
            throw new NotImplementedException();
        }

        public RemoveTableData RemoveQueryTable(QueryJoinerInfo joinerInfo, out string error)
        {
            throw new NotImplementedException();
        }

        public List<Joiner> ValidateTableRelations(List<Joiner> joins, out string error)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }

    public class QueryDataInfo
    {
        public string Method { get; set; }

        public string ActionType { get; set; }

        public object RawData { get; set; }

        public List<ConnectionData> Headers { get; set; }

        public List<ConnectionData> Parameters { get; set; }

        public QueryDataInfo()
        {
            this.Headers = new List<ConnectionData>();
            this.Parameters = new List<ConnectionData>();
        }
    }

    public class ConnectionData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
