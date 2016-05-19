using DevExpress.Xpf.Grid;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelServices
{
    public class ExcelService : IExcelService
    {
        Excel.Application excelApp;
        public void OpenWorkbook(string path)
        {
            excelApp = new Excel.Application();
            excelApp.Visible = false;
            excelApp.DisplayAlerts = false;
            excelApp.Application.Interactive = false;

            excelApp.Workbooks.Open(path);
            Format(ref excelApp);
        }

        public void Export(TableView tv, string path)
        {
            //using DevExpress.Xpf.Grid;
            //using DevExpress.XtraPrinting;
            tv.ExportToXls(path, new XlsExportOptions() { TextExportMode = TextExportMode.Text });
            OpenWorkbook(path);
        }
        private void Format(ref Excel.Application excel)
        {

            Excel.Workbook wb = excelApp.ActiveWorkbook;
            Excel.Worksheet ws = wb.ActiveSheet;

            ws.PageSetup.LeftMargin = 1;
            ws.PageSetup.RightMargin = 1;
            ws.PageSetup.TopMargin = 1;
            ws.PageSetup.BottomMargin = 1;

            ws.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;

            ws.get_Range("A:A").EntireColumn.ColumnWidth = 3;
            ws.get_Range("B:B").EntireColumn.ColumnWidth = 4;
            ws.get_Range("C:C").EntireColumn.ColumnWidth = 7;
            ws.get_Range("D:D").EntireColumn.ColumnWidth = 9;
            ws.get_Range("E:E").EntireColumn.ColumnWidth = 17;
            ws.get_Range("F:F").EntireColumn.ColumnWidth = 17;
            ws.get_Range("G:G").EntireColumn.ColumnWidth = 17;
            ws.get_Range("H:H").EntireColumn.ColumnWidth = 10;
            ws.get_Range("I:I").EntireColumn.ColumnWidth = 10;
            ws.get_Range("J:J").EntireColumn.ColumnWidth = 3;
            ws.get_Range("K:K").EntireColumn.ColumnWidth = 13;
            ws.get_Range("L:L").EntireColumn.ColumnWidth = 12;
            ws.get_Range("M:M").EntireColumn.ColumnWidth = 10;

            excelApp.Visible = true;
            excelApp.DisplayAlerts = true;
            excelApp.Application.Interactive = true;
            wb.Save();
        }
    }
}
