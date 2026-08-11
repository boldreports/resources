using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using MVCWebReportDesigner.Controllers;
using System.Collections;
using System.Threading;
using System.Net.Mail;
using System.Net.Mime;
using BoldReports.Web.ReportDesigner;
using BoldReports.Web.ReportViewer;

namespace MVCWebReportDesigner.WebAPI
{
    public class DesignerAPIController : ApiController, IReportDesignerController
    {
        const string CachePath = "App_Data\\ReportServer\\Cache\\";

        internal ExternalServer Server
        {
            get;
            set;
        }

        internal string ServerURL
        {
            get;
            set;
        }

        internal string AuthorizationHeaderValue
        {
            get;
            set;
        }

        public DesignerAPIController()
        {
            ExternalServer externalServer = new ExternalServer();
            this.Server = externalServer;
            this.ServerURL = "Sample";
            externalServer.ReportServerUrl = this.ServerURL;
            ReportDesignerHelper.ReportingServer = externalServer;
        }

        [HttpPost]
        public void UploadReportAction()
        {
            ReportDesignerHelper.ProcessDesigner(null, this, HttpContext.Current.Request.Files[0]);
        }

        [HttpGet]
        public object GetImage(string key, string image)
        {
            return ReportDesignerHelper.GetImage(key, image, this);
        }

        [HttpPost]
        public object PostDesignerAction(Dictionary<string, object> jsonResult)
        {
            return ReportDesignerHelper.ProcessDesigner(jsonResult, this, null);
        }

        public object PostReportAction(Dictionary<string, object> jsonResult)
        {
            if (jsonResult != null && jsonResult.ContainsKey("resourcetype"))
            {
                var _stream = ReportHelper.GetReport(jsonResult["controlID"].ToString(), "Html");
                _stream.Position = 0;
                StreamReader _reader = new StreamReader(_stream);
                var mailBody = _reader.ReadToEnd();
                mailBody = mailBody.Replace("<HTML>", "<html><meta http-equiv=\"X-UA-Compatible\" content=\"IE=9\" />");
                mailBody = mailBody.Replace("</HTML>", "<script>(function () {var rules = document.styleSheets[document.styleSheets.length - 1].rules;for (var idx = 0; idx < rules.length; idx++) {var elements = document.querySelectorAll(rules[idx].selectorText);for (var i = 0; i < elements.length; i++) {elements[i].style.cssText += rules[idx].style.cssText;}}var _length = rules.length;for (var i = 0; i < _length; i++) {document.styleSheets[document.styleSheets.length - 1].removeRule(0);}document.styleSheets[document.styleSheets.length - 1] = [];document.scripts[document.scripts.length - 1] = [];})();</script></html>");
                var t = new Thread(delegate ()
                {
                    System.Windows.Forms.WebBrowser _webBrowser = new System.Windows.Forms.WebBrowser();
                    _webBrowser.DocumentText = mailBody;
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        Thread.Sleep(500);
                    } while (_webBrowser.ReadyState != System.Windows.Forms.WebBrowserReadyState.Complete || _webBrowser.IsBusy);
                    mailBody = "<html><body>" + _webBrowser.Document.Body.InnerHtml + "</body></html>";
                });

                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();

                var _PdfStream = ReportHelper.GetReport(jsonResult["controlID"].ToString(), "Pdf");
                _PdfStream.Position = 0;

                var data = ReportHelper.GetResource(jsonResult["controlID"].ToString(), "Pdf", false);
                this.SendMsg(_PdfStream, mailBody, jsonResult["reportName"].ToString());
            }
            return ReportHelper.ProcessReport(jsonResult, this as IReportController);
        }

        public bool SendMsg(Stream str, string mailBody, string reportName)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.IsBodyHtml = true;
                mail.From = new MailAddress("mahe061195.shan@gmail.com");
                mail.To.Add("mahendran.shanmugam@syncfusion.com");
                mail.Subject = "Report_Name : " + reportName + " paramName : ";
                str.Position = 0;
                if (str != null)
                {
                    mail.Body = mailBody;
                    ContentType ct = new ContentType();
                    ct.Name = "report" + DateTime.Now.ToString() + ".html";
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(str, ct);
                    mail.Attachments.Add(attachment);
                }

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mahe061195.shan@gmail.com", "xxxxxxxx");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return false;
        }
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            reportOption.ReportModel.ReportingServer = this.Server;
            reportOption.ReportModel.ReportServerUrl = this.ServerURL;
            reportOption.ReportModel.ReportServerCredential = new NetworkCredential("Sample", "Passwprd");
            //reportOption.ReportModel.ProcessingMode = ProcessingMode.Local;
            //reportOption.ReportModel.DataSources.Clear();
            //reportOption.ReportModel.DataSources.Add(new ReportDataSource { Name = "StoreSales", Value = StoreSales.GetData() });
        }

        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        public object GetResource(string key, string resourcetype, bool isPrint)
        {
            return ReportHelper.GetResource(key, resourcetype, isPrint);
        }

        public bool UploadFile(HttpPostedFile httpPostedFile)
        {
            string targetFolder = HttpContext.Current.Server.MapPath("~/");
            string fileName = !string.IsNullOrEmpty(ReportDesignerHelper.SaveFileName) ? ReportDesignerHelper.SaveFileName : Path.GetFileName(httpPostedFile.FileName);
            targetFolder += CachePath;

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            if (!Directory.Exists(targetFolder + "\\" + ReportDesignerHelper.EJReportDesignerToken))
            {
                Directory.CreateDirectory(targetFolder + "\\" + ReportDesignerHelper.EJReportDesignerToken);
            }

            httpPostedFile.SaveAs(targetFolder + "\\" + ReportDesignerHelper.EJReportDesignerToken + "\\" + fileName);
            return true;
        }

        public List<FileModel> GetFiles(FileType fileType)
        {
            List<FileModel> databases = new List<FileModel>();
            var folderPath = HttpContext.Current.Server.MapPath("~/") + CachePath + ReportDesignerHelper.EJReportDesignerToken + "\\";

            if (Directory.Exists(folderPath))
            {
                DirectoryInfo dinfo = new DirectoryInfo(folderPath);
                FileInfo[] Files = dinfo.GetFiles(this.GetFileExtension(fileType));

                foreach (FileInfo file in Files)
                {
                    databases.Add(new FileModel() { Name = file.Name, Path = "../" + CachePath + ReportDesignerHelper.EJReportDesignerToken + "/" + file.Name });
                }
            }

            return databases;
        }

        private string GetFileExtension(FileType fileType)
        {
            if (fileType == FileType.Sdf)
            {
                return "*.sdf";
            }
            else if (fileType == FileType.Xml)
            {
                return "*.xml";
            }

            return "*.rdl";
        }

        public string GetFilePath(string fileName)
        {
            string targetFolder = HttpContext.Current.Server.MapPath("~/");
            targetFolder += CachePath;

            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            if (!Directory.Exists(targetFolder + "\\" + ReportDesignerHelper.EJReportDesignerToken))
            {
                Directory.CreateDirectory(targetFolder + "\\" + ReportDesignerHelper.EJReportDesignerToken);
            }

            var folderPath = HttpContext.Current.Server.MapPath("~/") + CachePath + ReportDesignerHelper.EJReportDesignerToken + "\\";
            return folderPath + fileName;
        }


        public FileModel GetFile(string filename, bool isOverride)
        {
            throw new NotImplementedException();
        }
    }

    
}