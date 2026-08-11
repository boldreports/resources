#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Xml;
using System.Net.Http;
using System.Web;
using System.Text.RegularExpressions;
using BoldReports.ServerProcessor;

namespace MVCWebReportDesigner.Controllers
{
    public sealed class ExternalServer : ReportingServer
    {
        public override List<CatalogItem> GetItems(string folderName, ItemTypeEnum type)
        {
            List<CatalogItem> _items = new List<CatalogItem>();
            string targetFolder = HttpContext.Current.Server.MapPath("~/") + @"App_Data\ReportServer\";

            if (type == ItemTypeEnum.DataSet)
            {
                var dataSets = ExternalRptServer.Data.SQLDataHelper.DataInstance.GetDataSets(); // Directory.GetFiles(folderName + "DataSet");
                foreach (var file in dataSets)
                {
                    CatalogItem catalogItem = new CatalogItem();
                    catalogItem.Name = Path.GetFileNameWithoutExtension(file);
                    catalogItem.Type = ItemTypeEnum.DataSet;
                    catalogItem.Id = Regex.Replace(catalogItem.Name, @"[^0-9a-zA-Z]+", "_");
                    _items.Add(catalogItem);
                }
            }
            else if (type == ItemTypeEnum.DataSource)
            {
                var dataSources = ExternalRptServer.Data.SQLDataHelper.DataInstance.GetDataSources(); // Directory.GetFiles(folderName + "DataSource");
                foreach (var file in dataSources)
                {
                    CatalogItem catalogItem = new CatalogItem();
                    catalogItem.Name = Path.GetFileNameWithoutExtension(file);
                    catalogItem.Type = ItemTypeEnum.DataSource;
                    catalogItem.Id = Regex.Replace(catalogItem.Name, @"[^0-9a-zA-Z]+", "_");
                    _items.Add(catalogItem);
                }
            }
            else if (type == ItemTypeEnum.Folder)
            {
                var categories = ExternalRptServer.Data.SQLDataHelper.DataInstance.GetCategories(); //Directory.GetDirectories(targetFolder)
                foreach (var file in categories)
                {
                    CatalogItem catalogItem = new CatalogItem();
                    catalogItem.Name = Path.GetFileNameWithoutExtension(file);
                    catalogItem.Type = ItemTypeEnum.Folder;
                    catalogItem.Id = Regex.Replace(catalogItem.Name, @"[^0-9a-zA-Z]+", "_");
                    _items.Add(catalogItem);
                }
            }
            else if (type == ItemTypeEnum.Report)
            {
                var reports = ExternalRptServer.Data.SQLDataHelper.DataInstance.GetReports(folderName); // Directory.GetFiles(folderName, "*.rdl")
                foreach (var file in reports)
                {
                    CatalogItem catalogItem = new CatalogItem();
                    catalogItem.Name = Path.GetFileNameWithoutExtension(file);
                    catalogItem.Type = ItemTypeEnum.Report;
                    catalogItem.Id = Regex.Replace(catalogItem.Name, @"[^0-9a-zA-Z]+", "_");
                    _items.Add(catalogItem);
                }
            }

            return _items;
        }

        public override bool CreateReport(string reportName, string folderName, byte[] reportdata, out string exception)
        {
            string tmpReportName = reportName;
            exception = null;
            string catagoryName = null;

            if (folderName != null)
            {
                catagoryName = folderName.TrimStart('/').TrimEnd('/').Trim();
            }
            else
            {
                var reportPaths = reportName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                folderName = catagoryName = '/' + reportPaths[0];
                reportName = reportPaths[1];
            }

            if (string.IsNullOrEmpty(catagoryName) && folderName != null && folderName != "/")
            {
                exception = "Please select any category";
                return false;
            }
            return ExternalRptServer.Data.SQLDataHelper.DataInstance.SetReportData(folderName, reportName, reportdata);
        }

        public override System.IO.Stream GetReport()
        {
            string reportPath = this.ReportPath.TrimStart('/').TrimEnd('/').Trim();
            string reportName = reportPath.Substring(reportPath.IndexOf('/') + 1).Trim();
            string catagoryName = reportPath.Substring(0, reportPath.IndexOf('/') > 0 ? reportPath.IndexOf('/') : 0).Trim();
            return new MemoryStream(ExternalRptServer.Data.SQLDataHelper.DataInstance.GetReport(catagoryName, reportName));
        }

        private Stream ReadFiles(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                fileStream.Position = 0;
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                return memStream;
            }
            return null;
        }

        public override bool EditReport(byte[] reportdata)
        {
            string reportPath = this.ReportPath.TrimStart('/').TrimEnd('/').Trim();
            string reportName = reportPath.Substring(reportPath.IndexOf('/') + 1).Trim();
            string catagoryName = reportPath.Substring(0, reportPath.IndexOf('/') > 0 ? reportPath.IndexOf('/') : 0).Trim();

            string targetFolder = HttpContext.Current.Server.MapPath("~/") + @"App_Data\ReportServer\Report\";

            string reportPat = targetFolder + catagoryName + @"\" + reportName + ".rdl";
            File.WriteAllBytes(reportPat, reportdata.ToArray());

            return true;
        }

        public override DataSourceDefinition GetDataSourceDefinition(string dataSource)
        {
            return this.GetDataSourceDefinition(ExternalRptServer.Data.SQLDataHelper.DataInstance.GetDataSource(dataSource), dataSource, null);
        }

        DataSourceDefinition GetDataSourceDefinition(byte[] dataSourceContent, string name, string guid)
        {
            StringReader _data = new StringReader(System.Text.Encoding.UTF8.GetString(dataSourceContent));
            var _rptDefinition = new DataSourceDefinition();
            var _umpDefinition = this.DeseralizeObj<DataSourceDefinition>(_data);
            _rptDefinition = _umpDefinition;
            return _rptDefinition;
        }

        public override SharedDatasetinfo GetSharedDataDefinition(string dataSet)
        {
            var _sharedDatasetInfo = new SharedDatasetinfo();
            var _datasetStream = this.GetFileToStream(ExternalRptServer.Data.SQLDataHelper.DataInstance.GetDataSet(dataSet));
            _sharedDatasetInfo.DataSetStream = _datasetStream;
            _sharedDatasetInfo.Guid = Guid.Empty.ToString();
            return _sharedDatasetInfo;

            return null;
        }

        T DeseralizeObj<T>(StringReader str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(str);
            return (T)serializer.Deserialize(reader);
        }

        T DeseralizeObj<T>(Stream str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(str);
            return (T)serializer.Deserialize(reader);
        }

        private Stream GetFileToStream(byte[] _fileContent)
        {
            MemoryStream memStream = new MemoryStream();
            memStream.Write(_fileContent, 0, _fileContent.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }

        public override bool CreateItem(ItemTypeEnum itemType, string itemName, string folderName, bool Overwrite, byte[] itemData, out string exception)
        {
            exception = null;

            if (Overwrite)
            {
                this.CreateDataSet(itemName, itemData, folderName);
            }
            else
            {
                this.CreateDataSet(itemName, itemData, folderName);
            }
            return true;
        }

        public bool CreateDataSet(string dataSetName, byte[] dataSetData, string folderName)
        {
            string tmpReportName = dataSetName;
            ExternalRptServer.Data.SQLDataHelper.DataInstance.SetDataSet(dataSetName, dataSetData);
            return false;
        }

        public bool CreateDataSource(string dataSourceName, byte[] dataSourceData, string folderName)
        {
            ExternalRptServer.Data.SQLDataHelper.DataInstance.SetDataSource(dataSourceName, dataSourceData);
            return true;
        }
    }
}