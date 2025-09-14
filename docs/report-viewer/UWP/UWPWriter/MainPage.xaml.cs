using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BoldReports.Writer;
using Windows.Storage.Pickers;
using System.Reflection;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Popups;
using System.Collections;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPWriter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        async void Button_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker fileSavePicker = new FileSavePicker();
            WriterFormat format = WriterFormat.PDF;

            if (pdf.IsChecked == true)
            {
                fileSavePicker.FileTypeChoices.Add("PDF", new List<string> { ".pdf" });
                fileSavePicker.DefaultFileExtension = ".pdf";
                format = WriterFormat.PDF;
            }
            else if (excel.IsChecked == true)
            {
                fileSavePicker.FileTypeChoices.Add("Excel", new List<string> { ".xlsx" });
                fileSavePicker.DefaultFileExtension = ".xlsx";
                format = WriterFormat.Excel;
            }
            else if (word.IsChecked == true)
            {
                fileSavePicker.FileTypeChoices.Add("Word", new List<string> { ".docx" });
                fileSavePicker.DefaultFileExtension = ".docx";
                format = WriterFormat.Word;
            }
            else if (html.IsChecked == true)
            {
                fileSavePicker.FileTypeChoices.Add("Html", new List<string> { ".html" });
                fileSavePicker.DefaultFileExtension = ".html";
                format = WriterFormat.HTML;
            }

            fileSavePicker.SuggestedFileName = "ExportReport";
            var savedItem = await fileSavePicker.PickSaveFileAsync();

            if (savedItem != null)
            {
                MemoryStream exportFileStream = new MemoryStream();
                Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;
                Stream reportStream = assembly.GetManifestResourceStream("UWPWriter.Resources.Table_Summaries.rdlc");

                BoldReports.UI.Xaml.ReportDataSourceCollection datas = new BoldReports.UI.Xaml.ReportDataSourceCollection();
                datas.Add(new BoldReports.UI.Xaml.ReportDataSource { Name = "Sales", Value = ReportData.GetData() });

                ReportWriter writer = new ReportWriter(reportStream, datas);
                writer.ExportMode = ExportMode.Local;
                writer.ExportCompleted += Writer_ExportCompleted;
                await writer.SaveASync(exportFileStream, format);

                try
                {
                    using (IRandomAccessStream stream = await savedItem.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        // Write compressed data from memory to file
                        using (Stream outstream = stream.AsStreamForWrite())
                        {
                            byte[] buffer = exportFileStream.ToArray();
                            outstream.Write(buffer, 0, buffer.Length);
                            outstream.Flush();
                        }
                    }
                    exportFileStream.Dispose();
                }
                catch { }
            }
        }

        private void Writer_ExportCompleted(object sender, byte[] e)
        {
            MessageDialog msgDialog = new MessageDialog("Report exporting completed successfully");
            msgDialog.ShowAsync();
        }
        public class ReportData
        {
            public string ProdCat { get; set; }
            public string SubCat { get; set; }
            public double? OrderYear { get; set; }
            public string OrderQtr { get; set; }
            public double? Sales { get; set; }
            public static IList GetData()
            {
                List<ReportData> datas = new List<ReportData>();
                ReportData data = null;
                data = new ReportData()
                {
                    ProdCat = "Accessories",
                    SubCat = "Helmets",
                    OrderYear = 2002,
                    OrderQtr = "Q1",
                    Sales = 4945.6925
                };
                datas.Add(data);
                data = new ReportData()
                {
                    ProdCat = "Components",
                    SubCat = "Road Frames",
                    OrderYear = 2002,
                    OrderQtr = "Q3",
                    Sales = 957715.1942
                };
                datas.Add(data);
                data = new ReportData()
                {
                    ProdCat = "Components",
                    SubCat = "Forks",
                    OrderYear = 2002,
                    OrderQtr = "Q4",
                    Sales = 23543.1060
                };
                datas.Add(data);
                data = new ReportData()
                {
                    ProdCat = "Bikes",
                    SubCat = "Road Bikes",
                    OrderYear = 2002,
                    OrderQtr = "Q1",
                    Sales = 3171787.6112
                };
                datas.Add(data);
                data = new ReportData()
                {
                    ProdCat = "Accessories",
                    SubCat = "Helmets",
                    OrderYear = 2002,
                    OrderQtr = "Q3",
                    Sales = 33853.1033
                };
                datas.Add(data);
                return datas;
            }
        }
    }
}
