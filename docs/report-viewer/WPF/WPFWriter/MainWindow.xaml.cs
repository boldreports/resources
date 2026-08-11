using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoldReports.Writer;

namespace WPFWriter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = Environment.CurrentDirectory;
                filePath = filePath.Replace("bin\\Debug", "");
                string reportPath = filePath + @"Resources\sales-order-detail.rdl";
                

                string fileName = null;
                WriterFormat format;

                //Step 1 : Instantiate the report writer with the parameter "ReportPath".
                ReportWriter reportWriter = new ReportWriter(reportPath);

                reportWriter.PDFOptions = new PDFOptions();
                reportWriter.PDFOptions.EnableComplexScript = true;

                //Step 2 : Save the report as Pdf or Word or Excel
                if (pdf.IsChecked == true)
                {
                    fileName = "AreaCharts.pdf";
                    format = WriterFormat.PDF;
                }
                else if (word.IsChecked == true)
                {
                    fileName = "AreaCharts.docx";
                    format = WriterFormat.Word;
                }
                else if (excel.IsChecked == true)
                {
                    fileName = "AreaCharts.xlsx";
                    format = WriterFormat.Excel;
                }
                else
                {
                    fileName = "AreaCharts.html";
                    format = WriterFormat.HTML;
                }
                reportWriter.Save(fileName, format);
                //Message box confirmation to view the created report document.
                if (MessageBox.Show("Do you want to view the " + format + " file?", "" + format + " report Created",
                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the PDF file using the default Application.[Acrobat Reader]
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}
