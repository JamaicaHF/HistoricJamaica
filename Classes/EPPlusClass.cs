using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using OfficeOpenXml;

namespace HistoricJamaica
{
    //****************************************************************************************************************************
    public static class Utilities
    {
        public static double ToDouble(this string dbl)
        {
            if (String.IsNullOrEmpty(dbl))
            {
                return 0.0;
            }
            try
            {
                return Convert.ToDouble(dbl);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid double: " + dbl);
                return 0.0;
            }
        }
    }
    //****************************************************************************************************************************
    public class EPPlusClass : IDisposable
    {
        ExcelPackage ExcelPackageOutput;
        public ExcelPackage excelPackageInput;
        public OfficeOpenXml.ExcelWorksheet inputWorksheet;
        public int numRows;
        public int numCols;

        protected System.Windows.Forms.ProgressBar progressBar;
        protected System.Windows.Forms.Label ProgressBarLabel;
        protected string progressBarPrefix = "";
        protected int numProgress = 0;
        protected OfficeOpenXml.ExcelWorksheet outputWorksheet;
        protected string workbookPath;

        private bool _notYetDisposed = true;

        //****************************************************************************************************************************
        protected string GetCellValue(OfficeOpenXml.ExcelWorksheet worksheet, int rowIndex, int colIndex)
        {
            if (worksheet.Cells[rowIndex, colIndex] == null)
            {
                return "";
            }
            if (worksheet.Cells[rowIndex, colIndex].Value == null)
            {
                return "";
            }
            return worksheet.Cells[rowIndex, colIndex].Value.ToString().Trim();
        }
        //****************************************************************************************************************************
        protected void CreateNewExcelWorkbook(string name)
        {
            try
            {
                ExcelPackageOutput = new ExcelPackage();
                outputWorksheet = ExcelPackageOutput.Workbook.Worksheets.Add(name);
                //xlApp.DisplayAlerts = false;
                //outputWorkbook = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                //outputWorksheet = (Worksheet)outputWorkbook.Worksheets[1];
                //if (outputWorksheet == null)
                //{
                //    Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
                //}
                //outputWorkbook.Worksheets.Add();
                //outputWorksheet = (Worksheet)outputWorkbook.Worksheets[1];  // The added worksheet becomde worksheet 1
                //outputWorksheet.Name = name;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //****************************************************************************************************************************
        protected string ExtractContiguousId(string value)
        {
            int indexOf = value.IndexOf('@');
            if (indexOf > 0)
            {
                value = value.Remove(indexOf).Trim();
            }
            indexOf = value.IndexOf(' ');
            if (indexOf > 0)
            {
                value = value.Remove(indexOf).Trim();
            }
            indexOf = value.IndexOf('=');
            if (indexOf > 0)
            {
                value = value.Remove(indexOf).Trim();
            }
            return TrimLeadingZeroes(value);
        }
        //****************************************************************************************************************************
        protected string TrimLeadingZeroes(string value)
        {
            int indexOf = 0;
            while (indexOf < value.Length && value[indexOf] == '0')
            {
                indexOf++;
            }
            if (indexOf > 0)
            {
                return value.Substring(indexOf);
            }
            return value;
        }
        //****************************************************************************************************************************
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose();
                // dispose managed resources
            }
            // free native resources
        }
        //****************************************************************************************************************************
        public void Dispose()
        {
            if (_notYetDisposed)
            {
                if (ExcelPackageOutput != null)
                {
                    try
                    {
                        CloseOutput();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Output File Must Be Open. Close and Try Again" );
                        CloseOutput();
                    }
                }
                this.ProgressBarLabel.Text = progressBarPrefix + progressBar.Maximum + " of " + this.progressBar.Maximum;
                progressBar.Value = progressBar.Maximum;
                _notYetDisposed = false;
                MessageBox.Show("Report Complete");
            }
            progressBar.Visible = false;
            ProgressBarLabel.Visible = false;
        }
        //****************************************************************************************************************************
        public void OpenWithEPPlus(string workbookPathname)
        {
            if (!File.Exists(workbookPathname))
            {
                throw new Exception("Input file does not exist: " + workbookPathname);
            }
            excelPackageInput = OpenExcelWorkbook(workbookPathname);
            if (excelPackageInput.Workbook.Worksheets.Count == 0)
            {
                throw new Exception("Input file does not exist: " + workbookPathname);
            }
            this.inputWorksheet = excelPackageInput.Workbook.Worksheets[1];
            this.numRows = inputWorksheet.Dimension.End.Row;
            this.numCols = inputWorksheet.Dimension.End.Column;
        }
        //****************************************************************************************************************************
        protected ExcelPackage OpenExcelWorkbook(string workbookPathname)
        {
            var fi = new FileInfo(workbookPathname);
            ExcelPackage excelPackage = new ExcelPackage(fi);
            return excelPackage;
        }
        //****************************************************************************************************************************
        public void CloseWorkbook()
        {
            if (excelPackageInput != null)
            {
                excelPackageInput.Dispose();
                excelPackageInput = null;
            }
        }
        //****************************************************************************************************************************
        protected void CloseOutput()
        {
            outputWorksheet.Cells[outputWorksheet.Dimension.Address].AutoFitColumns();
            outputWorksheet.View.FreezePanes(2, 1);
            ExcelPackageOutput.SaveAs(new FileInfo(workbookPath));
        }
        //****************************************************************************************************************************
        public void FormatStringCells(string cellRange)
        {
            outputWorksheet.Cells[outputWorksheet.Dimension.Address].Style.Numberformat.Format = "@";
            //Range range = outputWorksheet.Range[cellRange];
            //range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            //range.NumberFormat = "@";
        }
        //****************************************************************************************************************************
    }
}
