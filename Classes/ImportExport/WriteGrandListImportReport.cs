using System;
using System.Diagnostics;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    class WriteGrandListImportReport : EPPlus
    {
        //****************************************************************************************************************************
        public WriteGrandListImportReport()
        {
        }
        //****************************************************************************************************************************
        public void PrintReport(DataTable grandListHistoryTbl, DataTable grandListTbl)
        {
            CreateNewExcelWorkbook(@"C:\JHF\GrandListImport.xlsx");
            printHeaders();
            int rowIndex = 2;
            foreach (DataRow grandListHistoryRow in grandListHistoryTbl.Rows)
            {
                int grandListId = grandListHistoryRow[U.GrandListID_col].ToInt();

                string sSelectStatement = U.GrandListID_col + " = " + grandListId.ToString();
                DataRow[] grandListRows = grandListTbl.Select(sSelectStatement);
                if (grandListRows.Length == 0)
                {

                }
                else
                {
                    DataRow grandListRow = grandListRows[0];
                    outputWorksheet.Cells[rowIndex, 1].Value = grandListRow[U.TaxMapID_col].ToString();
                    outputWorksheet.Cells[rowIndex, 2].Value = grandListRow[U.Name1_col].ToString();
                    outputWorksheet.Cells[rowIndex, 3].Value = grandListHistoryRow[U.Name1_col].ToString();
                }
                rowIndex++;
            }
            CloseOutput();
        }
        private void printHeaders()
        {
            int rowIndex = 1;
            outputWorksheet.Cells[rowIndex, 1].Value = "Tax Map ID";
            outputWorksheet.Cells[rowIndex, 2].Value = "Current";
            outputWorksheet.Cells[rowIndex, 3].Value = "Previous";
        }
    }
}
