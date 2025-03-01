using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class ImportExportEPPlus : CImport
    {
        private EPPlus epPlus;
        //****************************************************************************************************************************
        public ImportExportEPPlus(CSql Sql, string sDataDirectory)
            : base(Sql, sDataDirectory)
        {
        }
        //****************************************************************************************************************************
        public void GetInputFile()
        {
            using (epPlus = new EPPlus())
            {
                string filename;
                GetExcelInputFile(@"c:\JHF\SchoolRecords", "School", out filename);
                if (!string.IsNullOrEmpty(filename))
                {
                }
            }
        }
        //****************************************************************************************************************************
        public void RestoreCensusInfo()
        {
            try
            {
                DataTable personTbl = SQL.GetAllPersons();
                int count = 0;
                int count1 = 0;
                int count2 = 0;
                int count3 = 0;
                int count4 = 0;
                using (epPlus = new EPPlus())
                {
                    string filename = @"C:\JHF\Transfer\Transfer.xlsx";
                    epPlus.OpenWithEPPlus(filename);
                    for (int row = 1; row < epPlus.numRows + 1; row++)
                    {
                        int personId = epPlus.GetCellValue(row, 1).ToInt();
                        string bornDate = epPlus.GetCellValue(row, 2);
                        string bornSource = epPlus.GetCellValue(row, 3);
                        string personSelectStatement = U.PersonID_col + " = " + personId.ToString();
                        DataRow[] personRows = personTbl.Select(personSelectStatement);
                        if (personRows.Length == 0)
                        {
                            count3++;
                        }
                        else
                        {
                            count4++;
                            DataRow personRow = personRows[0];
                            string personSource = personRow[U.BornSource_col].ToString();
                            if (personSource.ToLower().Contains("school"))
                            {
                                MoveBornToPerson(personRow, bornDate, bornSource);
                                count1++;
                            }
                            else if (personSource.ToLower().Contains("census"))
                            {
                                count2++;
                            }
                            else if (personSource.ToLower().Contains("civil"))
                            {
                                count3++;
                            }
                            else if (String.IsNullOrEmpty(personSource))
                            {
                                MoveBornToPerson(personRow, bornDate, bornSource);
                                count++;
                            }
                        }

                    }
                }
                SQL.UpdateWithDA(personTbl, U.Person_Table, U.PersonID_col, SQL.ColumnList(U.BornDate_col, U.BornSource_col));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void MoveBornToPerson(DataRow personRow, string bornDate, string bornSource)
        {
            if (!personRow[U.BornDate_col].ToString().Contains("/"))
            {
                personRow[U.BornDate_col] = bornDate;
            }
            personRow[U.BornSource_col] = bornSource;
        }
        //****************************************************************************************************************************
        public void CopyCensusInfo()
        {
            using (epPlus = new EPPlus())
            {
                epPlus.CreateNewExcelWorkbook(@"C:\JHF\Transfer\Transfer.xlsx");
                DataTable personTbl = SQL.GetAllPersons();
                int row = 0;
                foreach (DataRow personRow in personTbl.Rows)
                {
                    string bornSource = personRow[U.BornSource_col].ToString();
                    if (bornSource.ToLower().Contains("census"))
                    {
                        row++;
                        epPlus.WriteCellValue(personRow[U.PersonID_col].ToString(), row, 1);
                        epPlus.WriteCellValue(personRow[U.BornDate_col].ToString(), row, 2);
                        epPlus.WriteCellValue(bornSource, row, 3);
                    }
                }
                epPlus.CloseOutput();
            }
        }
    }
}
