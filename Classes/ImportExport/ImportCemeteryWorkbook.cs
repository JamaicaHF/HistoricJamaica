using System;
using System.IO;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SQL_Library;
using Utilities;
//using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HistoricJamaica
{
    public class CImportCemeteryWorkbook
    {
        /*
        private CSql m_SQL;
        private int rowIndex = 1;
        private System.Data.DataTable CemeteryRecordTbl = SQL.DefineCemeteryRecordTable();
        CPersonName personName;
        CPersonName spouseName;
        CPersonName fatherName;
        CPersonName motherName;

        private SQL_Library.ExcelTools excelApplication;
        
        public CImportCemeteryWorkbook(ref CSql cSQL)
        {
            m_SQL = cSQL;
            excelApplication = new ExcelTools();
            try
            {
                excelApplication.OpenExcelWorkbook(true, m_SQL.m_sDataDirectory + "\\CemeteryIndexes.xlsx");
                int numWorksheets = excelApplication.workbook.Worksheets.Count;
                int currentWorksheet = 0;
                foreach (Worksheet worksheet in excelApplication.workbook.Worksheets)
                {
                    currentWorksheet++;
                    if (currentWorksheet == 3)
                    {
                        ImportCemeteryInfo(worksheet, GetCemeteryId(currentWorksheet));
                    }
                }
                if (MessageBox.Show("Do you wish to import", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SQL.CreateNewCemeteryRecordRecords(CemeteryRecordTbl);
                }
                MessageBox.Show("Cemetery Import Successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(rowIndex + " " + ex.Message);
            }
            finally
            {
                excelApplication.CloseExcel();
            }
        }
        private Cemetery GetCemeteryId(int currentWorksheet)
        {
            switch (currentWorksheet)
            {
                case 1: return Cemetery.SouthWindham_Cemetery;
                case 2: return Cemetery.Robbins_Cemetery;
                case 3: return Cemetery.Rawsonville_Cemetery;
                case 4: return Cemetery.EastJamaica_Cemetery;
                case 5: return Cemetery.PikesFalls_Cemetery;
                case 6: return Cemetery.SeventhDayAdventist_Cemetery;
                case 7: return Cemetery.WestJamaica_Cemetery;
                default: return Cemetery.Village_Cemetery;
            }
        }
        private void ImportCemeteryInfo(Worksheet worksheet, Cemetery cemeteryId)
        {
            rowIndex = 2;
            string section = "";
            while (worksheet.Cells[rowIndex, 2].Value != null || worksheet.Cells[rowIndex, 4].Value != null)
            {
                string col1 = GetFromCol(worksheet, 1);
                string plot = GetFromCol(worksheet, 2);
                if (plot.ToString().ToLower().StartsWith("section"))
                {
                    section = plot;
                }
                else if (col1 == "xxxx")
                {
                    ProcessRow(worksheet, cemeteryId, section);
                }
                rowIndex++;
            }
        }
        private void ProcessRow(Worksheet worksheet, Cemetery cemeteryId, string Section)
        {
            personName = new CPersonName(ExcelValue(worksheet, 3));
            fatherName = new CPersonName(ExcelValue(worksheet, 6));
            motherName = new CPersonName(ExcelValue(worksheet, 8));
            spouseName = new CPersonName(ExcelValue(worksheet, 10));
            CAge age = new CAge(GetAndRemoveFromCol(worksheet, 14));

            DataRow cemeteryRow = CemeteryRecordTbl.NewRow();
            cemeteryRow[U.FirstName_col] = personName.firstName;
            cemeteryRow[U.MiddleName_col] = personName.middleName;
            cemeteryRow[U.LastName_col] = personName.lastName;
            cemeteryRow[U.Suffix_col] = personName.suffix;
            cemeteryRow[U.Prefix_col] = personName.prefix;
            cemeteryRow[U.FatherFirstName_col] = fatherName.firstName;
            cemeteryRow[U.FatherMiddleName_col] = fatherName.middleName;
            cemeteryRow[U.FatherLastName_col] = fatherName.lastName;
            cemeteryRow[U.FatherSuffix_col] = fatherName.suffix;
            cemeteryRow[U.FatherPrefix_col] = fatherName.prefix;
            cemeteryRow[U.MotherFirstName_col] = motherName.firstName;
            cemeteryRow[U.MotherMiddleName_col] = motherName.middleName;
            cemeteryRow[U.MotherLastName_col] = motherName.lastName;
            cemeteryRow[U.MotherSuffix_col] = motherName.suffix;
            cemeteryRow[U.MotherPrefix_col] = motherName.prefix;
            cemeteryRow[U.SpouseFirstName_col] = spouseName.firstName;
            cemeteryRow[U.SpouseMiddleName_col] = spouseName.middleName;
            cemeteryRow[U.SpouseLastName_col] = spouseName.lastName;
            cemeteryRow[U.SpouseSuffix_col] = spouseName.suffix;
            cemeteryRow[U.SpousePrefix_col] = spouseName.prefix;
            cemeteryRow[U.AgeYears_col] = age.numYears;
            cemeteryRow[U.AgeMonths_col] = age.numMonths;
            cemeteryRow[U.AgeDays_col] = age.numDays;
            
            cemeteryRow[U.CemeteryID_col] = (int)cemeteryId;
            cemeteryRow[U.LotNumber_col] = Section + GetFromCol(worksheet, 2);
            cemeteryRow[U.NameOnGrave_col] = GetFromCol(worksheet, 4);
            cemeteryRow[U.Sex_col] = GetSex(GetAndRemoveFromCol(worksheet, 5), personName.firstName);
            cemeteryRow[U.FatherNameOnGrave_col] = GetFromCol(worksheet, 7);
            cemeteryRow[U.MotherNameOnGrave_col] = GetFromCol(worksheet, 9);
            cemeteryRow[U.SpouseNameOnGrave_col] = GetFromCol(worksheet, 11);
            cemeteryRow[U.BornDate_col] = GetDate(worksheet, 12);
            cemeteryRow[U.DiedDate_col] = GetDate(worksheet, 13);
            cemeteryRow[U.Epitaph_col] = GetFromCol(worksheet, 15);
            cemeteryRow[U.Notes_col] = GetFromCol(worksheet, 16) + GetFromCol(worksheet, 17);

            cemeteryRow[U.Disposition_col] = 'B';
            cemeteryRow[U.PersonID_col] = 0;
            cemeteryRow[U.FatherID_col] = 0;
            cemeteryRow[U.MotherID_col] = 0;
            cemeteryRow[U.SpouseID_col] = 0;
            CemeteryRecordTbl.Rows.Add(cemeteryRow);
        }
        private string GetAndRemoveFromCol(Worksheet worksheet, int colIndex)
        {
            string strIn = GetFromCol(worksheet, colIndex);
            strIn = strIn.Replace("(?)", "");
            strIn = strIn.Replace("?", "");
            strIn = strIn.Replace("Mrs.", "");
            strIn = strIn.Replace("Mrs", "");
            strIn = strIn.Replace("mrs.", "");
            strIn = strIn.Replace("mrs", "");
            strIn = strIn.Replace("Miss.", "");
            strIn = strIn.Replace("Miss", "");
            strIn = strIn.Replace("miss.", "");
            strIn = strIn.Replace("miss", "");
            strIn = strIn.Replace("Mr.", "");
            strIn = strIn.Replace("Mr", "");
            strIn = strIn.Replace("mr.", "");
            strIn = strIn.Replace("mr", "");
            return strIn;
        }
        private string GetFromCol(Worksheet worksheet, int colIndex)
        {
            object cellData = worksheet.Cells[rowIndex, colIndex].Value;
            if (cellData == null)
            {
                return "";
            }
            return cellData.ToString().Trim();
        }
        private char GetSex(string inSex, string sFirstName)
        {
            if (String.IsNullOrEmpty(sFirstName))
            {
                return ' ';
            }
            if (String.IsNullOrEmpty(inSex))
            {
                return SQL.GetFirstNameSex(sFirstName);
            }
            return inSex[0];
        }
        private string GetDate(Worksheet worksheet, int colIndex)
        {
            string dateIn = GetAndRemoveFromCol(worksheet, colIndex);
            if (String.IsNullOrEmpty(dateIn))
            {
                return "";
            }
            if (dateIn.ToLower().Contains("still living"))
            {
                return "";
            }
            if (dateIn.Length > 10)
            {
                int indexOfSpace = dateIn.IndexOf(" ");
                dateIn = dateIn.Remove(indexOfSpace);
            }
            char[] delimiterChars = { '/', '-' };
            string[] values = dateIn.Split(delimiterChars);
            values[0] = values[0].Trim();
            if (values.Length == 1)
            {
                if (values[0].Length != 4)
                {
                    MessageBox.Show("invalid Date for row " + rowIndex.ToString() + "," + colIndex.ToString() + ": " + dateIn);
                    return "";
                }
                return values[0];
            }
            values[0] = values[0].Trim();
            values[1] = values[1].Trim();
            int val1 = GetNumericValForDate(values[0]);
            int val2 = GetNumericValForDate(values[1]);
            int val3 = 0;
            if (values.Length == 3)
            {
                values[2] = values[2].Trim();
                val3 = GetNumericValForDate(values[2]);
            }
            if (values[0].Length == 4)
            {
                return U.BuildDate(val1, val2, val3);
            }
            if (values.Length == 2 && values[1].Length == 4)
            {
                return U.BuildDate(val2, val1, val3);
            }
            else if (values[2].Length == 4)
            {
                return U.BuildDate(val3, val1, val2);
            }
            MessageBox.Show("invalid Date for row " + rowIndex.ToString() + "," + colIndex.ToString() + ": " + dateIn);
            return "";
        }
        private int GetNumericValForDate(string value)
        {
            int intVal = value.ToIntNoError();
            if (intVal == U.Exception)
            {
                if (value.ToLower() == "jan") return 5;
                if (value.ToLower() == "feb") return 5;
                if (value.ToLower() == "mar") return 5;
                if (value.ToLower() == "apr") return 5;
                if (value.ToLower() == "may") return 5;
                if (value.ToLower() == "jun") return 5;
                if (value.ToLower() == "jul") return 5;
                if (value.ToLower() == "aug") return 5;
                if (value.ToLower() == "sep") return 5;
                if (value.ToLower() == "oct") return 5;
                if (value.ToLower() == "nov") return 5;
                if (value.ToLower() == "dec") return 5;
                MessageBox.Show("invalid Date for row " + rowIndex.ToString() + ": " + value);
                return 0;
            }
            return intVal;
        }
        private string ExcelValue(Worksheet worksheet, int colIndex)
        {
            string value = worksheet.Cells[rowIndex, colIndex].Value;
            if (String.IsNullOrEmpty(value))
            {
                return "";
            }
            return value;
        }
         */
    }
}
