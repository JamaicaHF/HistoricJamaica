using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace HistoricJamaica
{
    //****************************************************************************************************************************
    public class ExcelApplication
    {
        private Microsoft.Office.Interop.Excel.Application xlApp;
        public Workbook workbook;

        public ExcelApplication()
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("EXCEL could not be started");
                return;
            }
        }
        public void CloseExcel()
        {
            xlApp.Quit();
        }
        public Worksheet GetWorksheet()
        {
            return workbook.Worksheets[1];
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
            catch
            {
                throw new Exception("Workbook must be Open");
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
    }
}
