using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WaterBilling.Pages;
using WaterBilling.Windows;
using WaterBillingProject.Pages;

namespace WaterBillingProject
{
    /// <summary>
    /// Interaction logic for CrystalReport.xaml
    /// </summary>
    public partial class CrystalReport : Window
    {
        public ReportDocument cryRpt;
        public String searchText = "";

        public CrystalReport()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleKeys);
            _CrystalReport.Owner = this;
            
        }



        private void HandleKeys(object sender, KeyEventArgs e)
        {
            // Control + F9 : Print Document
            if (e.Key == Key.F9 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                printDocument();
            }
            // Control + X : Exit
            else if (e.Key == Key.X && Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.Close();
            }
            // Control + P : Previous Page
            else if (e.Key == Key.P && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _CrystalReport.ViewerCore.ShowPreviousPage();
            }
            // Control + N : Next Page
            else if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _CrystalReport.ViewerCore.ShowNextPage();
            }
            // Control + A : First Page
            else if (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _CrystalReport.ViewerCore.ShowFirstPage();
            }
            // Control + Z : Last Page
            else if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                _CrystalReport.ViewerCore.ShowLastPage();
            }
            // Control + F : Find Text
            else if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                CrystalSearch searcher = new CrystalSearch(this);
                searcher.ShowDialog();

            }
            // F3 : Find Next
            else if (e.Key == Key.F3)
            {
                if (!_CrystalReport.ViewerCore.SearchForText(this.searchText, false, false))
                {
                    if (_CrystalReport.ViewerCore.CurrentPageNumber > 1)
                    {
                        if (MessageBox.Show("Search could not find any more instance of specified text after this page.\n\nDo you wish to search from the first page?", "Could Not find text", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            _CrystalReport.ViewerCore.ShowFirstPage();

                            CrystalSearch searcher = new CrystalSearch(this, this.searchText);
                            searcher.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Search could not find any instance of the specified text in this document.", "Text not found", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }


        private void exportExcel(object sender, RoutedEventArgs e)
        {
            //ButtonAutomationPeer peer = new ButtonAutomationPeer(this.focuser);
            //IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider; invokeProv.Invoke();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            path += "\\Billing Reports\\Excel";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                // handle them here
            }


            path += "\\" + (this.Title.Replace(@"/", "").Length > 100 ? this.Title.Replace(@"/", "").Substring(0, 100) : this.Title.Replace(@"/", ""));

            DateTime nw = DateTime.Now;


            path += DateTime.Now.ToString("yyyymmddHHmmss") + ".xls";

            try
            {

                ExportOptions CrExportOptions;

                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = path;
                CrExportOptions = cryRpt.ExportOptions;
                CrFormatTypeOptions.ExcelUseConstantColumnWidth = true;
                CrFormatTypeOptions.ExcelConstantColumnWidth = 1500;
                CrFormatTypeOptions.ShowGridLines = true;
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                cryRpt.Export();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("Export Complete");
            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            Process.Start(startInfo);
        }

        private void exportToPDF_Click(object sender, RoutedEventArgs e)
        {


            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            path += "\\Billing Reports\\PDF";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                // handle them here
            }


            path += "\\" + this.Title.Replace(@"/","");

            DateTime nw = DateTime.Now;


            path += DateTime.Now.ToString("yyyymmddHHmmss") + ".pdf";
            try
            {
                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = path;
                CrExportOptions = cryRpt.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                cryRpt.Export();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            ProcessStartInfo startInfo = new ProcessStartInfo(path);
            Process.Start(startInfo);



        }

        private void exportToExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            printDocument();
        }

        public void printDocument()
        {

            System.Windows.Forms.PrintDialog printer = new System.Windows.Forms.PrintDialog();
            //printer.AllowCurrentPage = true;
            printer.AllowSomePages = true;
            if (printer.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();
                int copies = printer.PrinterSettings.Copies;
                int fromPage = printer.PrinterSettings.FromPage;
                int toPage = printer.PrinterSettings.ToPage;
                bool collate = printer.PrinterSettings.Collate;

                cryRpt.PrintOptions.PrinterName = printer.PrinterSettings.PrinterName;
                cryRpt.PrintToPrinter(copies, collate, fromPage, toPage);


            }
        }
    }
}
