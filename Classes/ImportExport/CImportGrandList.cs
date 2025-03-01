using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    public class CImportGrandList : CImport
    {
        private ArrayList taxMapList = new ArrayList();
        private ArrayList taxMapIdNoInNewFile = new ArrayList();
        private DataTable grandListTbl;
        private DataTable modernRoadValueTbl;
        ArrayList checkList = new ArrayList();
        ArrayList UpdateList = new ArrayList();
        ArrayList VacantLand = new ArrayList();
        ArrayList DeleteList = new ArrayList();
        DataTable grandListHistoryTbl;
        private EPPlus epPlus;
        //****************************************************************************************************************************
        public CImportGrandList(CSql Sql, string sDataDirectory)
            : base(Sql, sDataDirectory)
        {
        }
        //****************************************************************************************************************************
        public void ImportGrandList()
        {
            try
            {
                grandListHistoryTbl = SQL.DefineGrandListHistoryTable();
                using (epPlus = new EPPlus())
                {
                    grandListTbl = SQL.GetAllGrandList();
                    modernRoadValueTbl = SQL.GetAllModernRoadValues();
                    GetActivesInactives("Actives");
                    GetActivesInactives("Inactives");
                    CheckAllInactiveGrandlistRecords(grandListTbl);
                    WriteGrandListImportReport writeGrandListImportReport = new WriteGrandListImportReport();
                    writeGrandListImportReport.PrintReport(grandListHistoryTbl, grandListTbl);
                    SaveImportTables();
                    MessageBox.Show("Import Complete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void SaveImportTables()
        {
            if (MessageBox.Show("Examine Report for Exceptions.  Press Yes to Save Import", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SQL.UpdateInsertDeleteGrandList(grandListTbl);
                SQL.InsertGrandListHistory(grandListHistoryTbl);
            }
        }
        //****************************************************************************************************************************
        private void GetActivesInactives(string ActivesInactives)
        {
            string filename;
            GetExcelInputFile(@"c:\Reports\", ActivesInactives, out filename);
            if (!string.IsNullOrEmpty(filename))
            {
                char ActiveStatus = ActivesInactives[0];
                GetGrandListRecords(filename, ActiveStatus);
            }
        }
        //****************************************************************************************************************************
        private void CheckAllInactiveGrandlistRecords(DataTable grandListTbl)
        {
            DataTable buildingTbl = SQL.GetAllBuildings();
            foreach (DataRow grandListRow in grandListTbl.Rows)
            {
                try
                {
                    if (grandListRow.RowState != DataRowState.Deleted)
                    {
                        string grandListId = grandListRow[U.GrandListID_col].ToString();
                        string taxMapId = grandListRow[U.TaxMapID_col].ToString();
                        char vacantLand = grandListRow[U.VacantLand_col].ToChar();
                        if (!String.IsNullOrEmpty(taxMapId) && grandListId != "0" && !IsTaxMapIdInList(taxMapId, vacantLand))
                        {
                            string selectStatement = U.BuildingGrandListID_col + "=" + grandListId;
                            DataRow[] foundRows = buildingTbl.Select(selectStatement);
                            if (foundRows.Length != 0)
                            {
                                checkList.Add(taxMapId);
                                taxMapIdNoInNewFile.Add(taxMapId);  // Inactive Parcel with building attached
                                //MessageBox.Show("TaxMapID in Database not in new File: ", taxMapId);
                            }
                            else
                            {
                                if (vacantLand == 0)
                                {
                                    grandListRow.Delete();
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        //****************************************************************************************************************************
        private bool IsTaxMapIdInList(string taxMapId, char vacantLand)
        {
            if (vacantLand == '1')
            {
                return false;
            }
            foreach (string taxMapIdInList in taxMapList)
            {
                if (taxMapId == taxMapIdInList)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private void GetGrandListRecords(string filename, char ActiveStatus)
        {
            epPlus.OpenWithEPPlus(filename);
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            int rowIndex = 2;
            while (rowIndex <= epPlus.numRows)
            {
                try
                {
                    AddRecordToDatabase(rowIndex, ActiveStatus);
                    rowIndex++;
                }
                catch (Exception ex)
                {
                    string message = "Row: " + rowIndex + " - " + ex.Message;
                    throw new Exception(message);
                }
            }
        }
        //****************************************************************************************************************************
        private void AddRecordToDatabase(int rowIndex, char ActiveStatus)
        {
            CNemrcExtract nemrcExtract = new CNemrcExtract(epPlus, modernRoadValueTbl, rowIndex, ActiveStatus);
            string selectStatement = U.TaxMapID_col + " = '" + nemrcExtract.TaxMapID + "'";
            //string selectStatement = U.Span_col + " = '" + nemrcExtract.Span + "'";
            DataRow[] foundRows = grandListTbl.Select(selectStatement);
            if (ExcludedProperty(nemrcExtract.Name1, nemrcExtract.Name2, nemrcExtract.TaxMapID, nemrcExtract.Owner))
            {
                if (foundRows.Length == 1)
                {
                    foundRows[0].Delete();
                    DeleteList.Add(nemrcExtract.TaxMapID);
                }
                return;
            }
            if (foundRows.Length == 1)
            {
                if (foundRows[0]["Span"].ToString() == "324-101-10513")
                {
                }
                nemrcExtract.UpdateExistingGrandListRecord(grandListHistoryTbl, foundRows[0], 2019);

            }
            else if (nemrcExtract.VacantLand != '1')
            {
                DataRow grandListNewRow = grandListTbl.NewRow();
                nemrcExtract.CreateNewGrandListRecord(grandListNewRow, ActiveStatus);
                grandListTbl.Rows.Add(grandListNewRow);
                VacantLand.Add(nemrcExtract.TaxMapID);
            }
            taxMapList.Add(nemrcExtract.TaxMapID);
        }
        //****************************************************************************************************************************
        private bool ExcludedProperty(string name1, string name2, string taxMapId, string owner)
        {
            if (name1.ToUpper().Contains("AGENCY OF TRANSPORT") || name2.ToUpper().Contains("AGENCY OF TRANSPORT"))
            {
                return true;
            }
            if (String.IsNullOrEmpty(taxMapId))
            {
                return true;
            }
            if (String.IsNullOrEmpty(owner) || owner.Length == 0 || owner[0] == ' ') // Utility
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        public void ImportGrandListHistory()
        {
            try
            {
                using (epPlus = new EPPlus())
                {

                    string filename = @"c:\JHF\GrandListHistory.xlsx";
                    if (!File.Exists(filename))
                    {
                        return;
                    }
                    epPlus.OpenWithEPPlus(filename);
                    if (string.IsNullOrEmpty(filename))
                    {
                        return;
                    }
                    ImportHistory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void ImportHistory()
        {
            grandListTbl = SQL.GetAllGrandList();
            DataTable grandListHistoryTbl = SQL.DefineGrandListHistoryTable();
            int rowIndex = 2;
            while (rowIndex <= epPlus.numRows)
            {
                string historyGrandlistId = epPlus.GetCellValue(rowIndex, 1).ToString();
                string historySpan = epPlus.GetCellValue(rowIndex, 2).ToString();
                string historyTaxMapId = epPlus.GetCellValue(rowIndex, 3).ToString();
                string selectStatement = U.Span_col + " = '" + historySpan + "'";
                DataRow[] foundRows = grandListTbl.Select(selectStatement);
                if (foundRows.Length == 0)
                {
                    throw new Exception("Unable to Get Grandlist Record: " + historySpan);
                }
                string grandlistId = foundRows[0][U.GrandListID_col].ToString();
                string taxMapId = foundRows[0][U.TaxMapID_col].ToString();
                if (taxMapId != historyTaxMapId)
                {
                    throw new Exception("Different TaxMapId: " + grandlistId + ", " + historyTaxMapId);
                }
                DataRow grandListHistoryRow = grandListHistoryTbl.NewRow();
                grandListHistoryRow[U.GrandListID_col] = grandlistId;
                grandListHistoryRow[U.Year_col] = epPlus.GetCellValue(rowIndex, 4).ToInt();
                grandListHistoryRow[U.Name1_col] = epPlus.GetCellValue(rowIndex, 5).ToString();
                grandListHistoryRow[U.Name2_col] = epPlus.GetCellValue(rowIndex, 6).ToString();
                grandListHistoryTbl.Rows.Add(grandListHistoryRow);
                rowIndex++;
            }
            SQL.InsertGrandListHistory(grandListHistoryTbl);
            MessageBox.Show("Import Complete");
        }
        //****************************************************************************************************************************
    }
}
