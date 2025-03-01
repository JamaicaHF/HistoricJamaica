using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
//using Microsoft.Office.Interop.Excel;

namespace SQL_Library
{
    public class ExcelTools
    {
/*        private Microsoft.Office.Interop.Excel.Application xlApp = null;
        public Workbook workbook = null;
        private _Worksheet worksheet = null;
        private string workbookPath = null;
        public ExcelTools()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                throw new Exception("EXCEL could not be started");
            }
        }
        public void SaveExcelWorkbook()
        {
            if (workbookPath != null && workbook != null)
            {
                workbook.SaveAs(workbookPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                workbook = null;
            }
        }
        public void CloseExcel()
        {
            if (xlApp != null)
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
                xlApp.Quit();
            }
        }
        public Worksheet GetWorksheet()
        {
            return workbook.Worksheets[1];
        }
        public void FormatColumnAsText(string columns)
        {
            workbook.Worksheets[1].Cells.Range[columns].NumberFormat = "@";

        }
        public void OpenExcelWorkbook(bool visible,
                                      string filename)
        {
            xlApp.DisplayAlerts = false;
            workbook = OpenExistingExcelWorkbook(filename, visible);
        }
        public Workbook OpenExistingExcelWorkbook(string workbookPathname, bool visible)
        {
            if (!File.Exists(workbookPathname))
            {
                throw new Exception("Workbook does not Exist");
            }
            CheckIfWorkbookIsOpen(workbookPathname);
            return OpenExcelWorkbook(workbookPathname, visible);
        }
        public void CheckIfWorkbookIsOpen(string workbookPathname)
        {
            if (!File.Exists(workbookPathname)) // path does not exist.  Cannot be open
            {
                return;
            }
            try
            {
                Workbook workbook = OpenExcelWorkbook(workbookPathname, false);
                workbook.SaveAs(workbookPathname, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Workbook must be Open: " + e.Message);
            }
        }
        public Workbook OpenExcelWorkbook(string workbookPathname, bool visible)
        {
            try
            {
                xlApp.DisplayAlerts = false;
                var workbook = xlApp.Workbooks.Open(workbookPathname, ReadOnly: false);
                if (visible)
                {
                    xlApp.DisplayAlerts = true;
                    xlApp.Visible = true;
                    xlApp.UserControl = true;
                    xlApp.WindowState = XlWindowState.xlMaximized;
                }
                else
                {
                    xlApp.DisplayAlerts = false;
                    xlApp.Visible = false;
                }
                return workbook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void CreateNewExcelWorkbook(string workbookFilePath)
        {
            workbookPath = workbookFilePath;
            CheckIfWorkbookIsOpen(workbookPath);
            xlApp.DisplayAlerts = false;
            workbook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        }
 * */
    }
}
