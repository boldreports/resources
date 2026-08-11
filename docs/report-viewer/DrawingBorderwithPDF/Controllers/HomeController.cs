using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BoldReports.Web;
using BoldReports.Writer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using WriterCORE.Models;

namespace WriterCORE.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public HomeController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult RDLPDF()
        {

            // Export the RDL To PDF.
            FileStream mainReportStream = new FileStream(_hostingEnvironment.WebRootPath + @"\Reports\SampleReport.rdl", FileMode.Open, FileAccess.Read);
            BoldReports.Writer.ReportWriter writer = new BoldReports.Writer.ReportWriter(mainReportStream);
            MemoryStream memoryStream = new MemoryStream();
            writer.Save(memoryStream, BoldReports.Writer.WriterFormat.PDF);

            // Download the generated export document to the client side.
            memoryStream.Position = 0;
            FileStreamResult fileStreamResult = new FileStreamResult(memoryStream, "application/pdf");

            var fileStream1 = fileStreamResult.FileStream;


            // Combine the Exported PDF and template
            PdfDocument finalDoc = new PdfDocument();

            FileStream fileStream2 = new FileStream(_hostingEnvironment.WebRootPath + @"\PDF\Template.pdf", FileMode.Open, FileAccess.Read);

            // Creates a PDF stream for merging

            Stream[] streams = { fileStream1, fileStream2 };

            // Merges PDFDocument.

            PdfDocumentBase.Merge(finalDoc, streams);

            //Save the document into stream

            MemoryStream combinestream = new MemoryStream();

            finalDoc.Save(combinestream);

            combinestream.Position = 0;

            FileStreamResult fileStreamResult1 = new FileStreamResult(combinestream, "application/pdf");

            var pagenumber = fileStreamResult1.FileStream;

            // Add Page number for combined PDF
            PdfLoadedDocument loadedDoc = new PdfLoadedDocument(pagenumber);

            // Create a new PDF document
            PdfDocument doc = new PdfDocument();
            doc.PageSettings.Margins.All = 0;

            //Add a page to the document
            PdfPage page = doc.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            FileStream fileStream3 = new FileStream(_hostingEnvironment.WebRootPath + @"\f25e9269edfa67ef53e840eea0a98c30.jpg", FileMode.Open, FileAccess.Read);

            //Load the image from the disk
            PdfBitmap image = new PdfBitmap(fileStream3);

            //Draw the image
            graphics.DrawImage(image, 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);

            //Save the document
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);

            //Close the document
            doc.Close(true);

            //Load the border design PDF document 
            PdfLoadedDocument loadedDocument1 = new PdfLoadedDocument(ms);

            //Create the new document
            PdfDocument document = new PdfDocument();
            document.PageSettings.Margins.All = 0;

            //Add the border design in each page of an existing PDF document 
            for (int i = 0; i < loadedDoc.Pages.Count; i++)
            {
                //Add PDF page
                PdfPage pdfPage = document.Pages.Add();

                //Create a template from the first document
                PdfPageBase loadedPage = loadedDocument1.Pages[0];
                PdfTemplate template = loadedPage.CreateTemplate();

                //Draw the loaded template into new document
                pdfPage.Graphics.DrawPdfTemplate(template, PointF.Empty, page.GetClientSize());

                //Create a template from the second document
                loadedPage = loadedDoc.Pages[i];
                template = loadedPage.CreateTemplate();

                //Draw the loaded template into new document
                pdfPage.Graphics.DrawPdfTemplate(template, PointF.Empty, page.GetClientSize());
            }

            MemoryStream pagenumberstream = new MemoryStream();

            document.Save(pagenumberstream);
            //Close the PDF documents 
            document.Close(true);
            loadedDocument1.Close(true);
            loadedDoc.Close(true);

            string contentType = "application/pdf";

            //Define the file name
            string fileName = "Combinewithpagenumber.pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name
            pagenumberstream.Position = 0;

            return File(pagenumberstream, contentType, fileName);

        }

        [HttpPost]
        public IActionResult CombinePDF()
        {
            PdfDocument finalDoc = new PdfDocument();

            FileStream stream1 = new FileStream(_hostingEnvironment.WebRootPath + @"\PDF\RDLExport.pdf", FileMode.Open, FileAccess.Read);

            FileStream stream2 = new FileStream(_hostingEnvironment.WebRootPath + @"\PDF\Template.pdf", FileMode.Open, FileAccess.Read);

            // Creates a PDF stream for merging

            Stream[] streams = { stream1, stream2 };

            // Merges PDFDocument.

            PdfDocumentBase.Merge(finalDoc, streams);

            //Save the document into stream

            MemoryStream stream = new MemoryStream();

            finalDoc.Save(stream);

            stream.Position = 0;

            //Close the document

            finalDoc.Close(true);

            //Disposes the streams.

            stream1.Dispose();

            stream2.Dispose();

            //Defining the ContentType for pdf file

            string contentType = "application/pdf";

            //Define the file name

            string fileName = "Combine.pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name

            return File(stream, contentType, fileName);
        }

        [HttpPost]
        public IActionResult Pagenumber()
        {

            PdfLoadedDocument loadedDoc = new PdfLoadedDocument(new FileStream(_hostingEnvironment.WebRootPath + @"\PDF\Combine.pdf", FileMode.Open, FileAccess.Read));

            //Set the font.
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12f);

            //Create page number field.
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, PdfBrushes.Black);

            //Create page count field.
            PdfPageCountField count = new PdfPageCountField(font, PdfBrushes.Black);

            //Add the fields in composite fields.
            PdfCompositeField compositeField = new PdfCompositeField(font, PdfBrushes.Black, "Page {0} of {1}", pageNumber, count);

            for (int i = 0; i < loadedDoc.Pages.Count; i++)
            {
                //Draw the composite field.
                compositeField.Draw(loadedDoc.Pages[i].Graphics, new PointF(loadedDoc.Pages[i].Size.Width / 2 - 20, loadedDoc.Pages[i].Size.Height - 20));
            }
            MemoryStream stream = new MemoryStream();

            loadedDoc.Save(stream);
            //Save the document.

            stream.Position = 0;
            //Close the document.
            loadedDoc.Close(true);
            string contentType = "application/pdf";

            //Define the file name

            string fileName = "Combinewithpagenumber.pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name

            return File(stream, contentType, fileName);
        }


    }
}
