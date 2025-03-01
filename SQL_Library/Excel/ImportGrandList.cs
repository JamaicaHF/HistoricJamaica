using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SQL_Library
{
    /*
    public static partial class SQL
    {
        private enum NemrcColumns
        {
            ParcelId = 1,
            SubParcelId = 2,
            Name1 = 3,
            Name2 = 4,
            Address1 = 5,
            Address2 = 6,
            City = 7,
            State = 8,
            ZipCode = 9,
            LocationA = 10,
            LocationB = 11,
            LocationC = 12,
            E911Address = 13,
            E911Suffix = 14,
            E911Street = 15,
            TaxMapID = 16,
            PropertyDescription = 17,
            Price = 18,
            Valid = 19,
            ReasonForValidorInvalid = 20,
            LastBook = 21,
            LastPage = 22,
            LastTransferDate = 23,
            TransferId = 24,
            SaleDate = 25,
            FromLan = 26,
            TransferInfo1 = 27,
            TransferInfo2 = 28,
            SchoolCode = 29,
            ParcelStatus = 30,
            SpanNumber = 31,
            Code1 = 32,
            Category = 33,
            Owner = 34,
            WoodAcres = 35,
            CropAcres = 36,
            PastureAcres = 37,
            otheracres = 38,
            SiteAcres = 39,
            TotalAcres = 40
        }
        private EPPlus epPlus;
        public static void ImportActivesInactives(string folder,
                                                  bool includeAddress)
        {
            Worksheet worksheet;
            SQL_Library.ExcelTools excelTools = new ExcelTools();
            ArrayList numAddedList = new ArrayList();
            ArrayList numDifferent = new ArrayList();
            System.Data.DataTable grandList_tbl = SQL.GetAllGrandList();
            try
            {
                excelTools.OpenExcelWorkbook(false, folder + "ActiveParcels.csv");
                excelTools.FormatColumnAsText("B:B");
                worksheet = excelTools.GetWorksheet();
                ImportGrandList(numAddedList, numDifferent, grandList_tbl, worksheet, includeAddress);
                excelTools.CloseExcel();
                excelTools.OpenExcelWorkbook(false, folder + "InactiveParcels.csv");
                worksheet = excelTools.GetWorksheet();
                ImportGrandList(numAddedList, numDifferent, grandList_tbl, worksheet, includeAddress);
                ArrayList numDeletedList = new ArrayList();
                foreach (DataRow row in grandList_tbl.Rows)
                {
                    DataRowState rowState = row.RowState;
                    if (rowState != DataRowState.Modified && rowState != DataRowState.Added)
                    {
                        numDeletedList.Add(row[U.TaxMapID_col].ToString());
                        row.Delete();
                    }
                    else
                    {
                    }
                }
                UpdateInsertDeleteGrandList(grandList_tbl);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                excelTools.CloseExcel();
            }
        }
        public static void ImportGrandList(ArrayList numAddedList,
                                           ArrayList numDifferent, 
                                     System.Data.DataTable GrandList_tbl,
                                     Worksheet worksheet,
                                     bool includeAddress)
        {
            int rowIndex = 1;
            try
            {
                while (worksheet.Cells[rowIndex, 1].Value != null)
                {
                    string propertyId = GetValue(worksheet, rowIndex, (int)NemrcColumns.ParcelId);
                    string propertySubId = GetValue(worksheet, rowIndex, (int)NemrcColumns.SubParcelId);
                    if (!String.IsNullOrEmpty(propertySubId) && propertySubId[0] == '0' && propertySubId.Length > 1)
                    {
                        propertySubId = propertySubId.Remove(0, 2);
                    }
                    string fullPropertyID = propertyId + "." + propertySubId;
                    string taxMapID = GetValue(worksheet, rowIndex, (int)NemrcColumns.TaxMapID);
                    if (!String.IsNullOrEmpty(taxMapID))
                    {
                        CheckTaxMapID(numDifferent, fullPropertyID, taxMapID);
                        string selectStatement = (U.TaxMapID_col + " = '" + taxMapID) + "'";
                        var results = GrandList_tbl.Select(selectStatement, U.TaxMapID_col + " ASC");
                        if (results == null || results.Length == 0)
                        {
                            DataRow grandList_row = GrandList_tbl.NewRow();
                            grandList_row[U.GrandListID_col] = 0;
                            AddExcelValueToRow(grandList_row, worksheet, rowIndex, includeAddress);
                            grandList_row[U.WhereOwnerLiveID_col] = "N";
                            if (!includeAddress)
                            {
                                grandList_row[U.BuildingRoadValueID_col] = 0;
                            }
                            GrandList_tbl.Rows.Add(grandList_row);
                            numAddedList.Add(taxMapID);
                        }
                        else
                        {
                            DataRow GrandList_row = results[0];
                            int grandListID = GrandList_row[U.GrandListID_col].ToInt();
                            AddExcelValueToRow(GrandList_row, worksheet, rowIndex, includeAddress);
                        }
                    }
                    rowIndex++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + rowIndex + ex.Message);
            }
        }
        private static void CheckTaxMapID(ArrayList numDifferent,
                                          string fullPropertyID, 
                                          string taxMapID)
        {
            string originalFullPropertyID = fullPropertyID;
            if (string.IsNullOrEmpty(fullPropertyID))
            {
                return;
            }
            int indexOfFirstNonZeroChar = 0;
            while (indexOfFirstNonZeroChar < fullPropertyID.Length && fullPropertyID[indexOfFirstNonZeroChar] == '0')
            {
                indexOfFirstNonZeroChar++;
            }
            if (indexOfFirstNonZeroChar > 0 && indexOfFirstNonZeroChar < fullPropertyID[indexOfFirstNonZeroChar])
            {
                fullPropertyID = fullPropertyID.Substring(indexOfFirstNonZeroChar);
            }
            int indexOfLastChar = fullPropertyID.Length - 1;
            if (fullPropertyID[indexOfLastChar] == '.')
            {
                fullPropertyID = fullPropertyID.Remove(indexOfLastChar);
                indexOfLastChar--;
            }
            int indexOfLastCharTaxMapID = taxMapID.Length - 1;
            if (taxMapID[indexOfLastCharTaxMapID] == '0' && fullPropertyID[indexOfLastChar] != '0')
            {
                fullPropertyID = fullPropertyID + "0";
            }
            if (fullPropertyID != taxMapID)
            {
                numDifferent.Add(originalFullPropertyID);
            }
        }
        private static void AddExcelValueToRow(DataRow grandListRow,
                                        Worksheet worksheet,
                                        int rowIndex,
                                        bool includeAddress)
        {
            //SetToNewValue(grandListRow, U.TaxMapID_col, GetValue(worksheet, rowIndex, (int)NemrcColumns.TaxMapID));

            grandListRow[U.TaxMapID_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.TaxMapID);
            grandListRow[U.StreetName_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.E911Street);
            grandListRow[U.StreetNum_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.E911Address).ToInt();
            grandListRow[U.Name1_col] = AllNamesLower(GetValue(worksheet, rowIndex, (int)NemrcColumns.Name1));
            grandListRow[U.Name2_col] = AllNamesLower(GetValue(worksheet, rowIndex, (int)NemrcColumns.Name2));
            if (includeAddress)
            {
                grandListRow[U.AddressA_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.Address1);
                grandListRow[U.AddressB_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.Address2);
                grandListRow[U.City_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.City);
                grandListRow[U.State_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.State);
                grandListRow[U.Zip_col] = GetValue(worksheet, rowIndex, (int)NemrcColumns.ZipCode);
            }
        }
        private static void SetToNewValue(DataRow grandListRow,
                                   string rowCol,
                                   string NemrcValue)
        {
            if (grandListRow[U.TaxMapID_col].ToString() != NemrcValue)
            {
                grandListRow[U.TaxMapID_col] = NemrcValue;
            }
        }
        private static string AllNamesLower(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return name;
            }
            StringBuilder nameString = new StringBuilder(name.ToLower());
            if (!IsCareOf(nameString, 0))
            {
                nameString[0] = SetToUpper(nameString[0]);
            }
            for (int i = 1; i < nameString.Length - 1; i++)
            {
                if (i < nameString.Length - 2 && IsLLC(nameString[i], nameString[i + 1], nameString[i + 2]))
                {
                    nameString[i] = 'L';
                    nameString[i + 1] = 'L';
                    nameString[i + 2] = 'C';
                }
                else if (i < nameString.Length - 2 && IsTheIII(nameString[i], nameString[i + 1], nameString[i + 2])) // must come before the II
                {
                    nameString[i] = 'I';
                    nameString[i + 1] = 'I';
                    nameString[i + 2] = 'I';
                }
                else if (IsTheII(nameString[i], nameString[i + 1]))
                {
                    nameString[i] = 'I';
                    nameString[i + 1] = 'I';
                }
                else if (IsTheIV(nameString[i], nameString[i + 1]))
                {
                    if (i == nameString.Length - 2 || nameString[i + 2] == ' ')
                    {
                        nameString[i] = 'I';
                        nameString[i + 1] = 'V';
                    }
                }
                else if (!IsCareOf(nameString, i) && (NextCharShouldBeSetToUpper(nameString[i], nameString[i + 1])))
                {
                    nameString[i + 1] = SetToUpper(nameString[i + 1]);
                }
            }
            return nameString.ToString();
        }
        private static bool IsTheII(char ch, char chPlus1)
        {
            return (ch == 'I' && chPlus1 == 'i');
        }
        private static bool IsTheIV(char ch, char chPlus1)
        {
            return (ch == 'I' && chPlus1 == 'v');
        }
        private static bool IsTheIII(char ch, char chPlus1, char chPlus2)
        {
            return (ch == 'I' && chPlus1 == 'i' && chPlus2 == 'i');
        }
        private static bool IsCareOf(StringBuilder nameString,
                              int i)
        {
            if (i < nameString.Length - 2 && IsCareOf(nameString[i], nameString[i + 1], nameString[i + 2]))
            {
                nameString[i] = 'C';
                nameString[i + 1] = '/';
                nameString[i + 2] = 'O';
                return true;
            }
            return false;
        }
        private static bool IsCareOf(char ch, char chPlus1, char chPlus2)
        {
            if (ch != 'C' && ch != 'c')
            {
                return false;
            }
            return (chPlus1 == '/' && chPlus2 == 'o');
        }
        private static bool IsLLC(char ch, char chPlus1, char chPlus2)
        {
            return (ch == 'L' && chPlus1 == 'l' && chPlus2 == 'c');
        }
        private static bool NextCharShouldBeSetToUpper(char ch, char chPlus1)
        {
            if (ch == ' ')
            {
                return true;
            }
            if (ch == '-')
            {
                return true;
            }
            if (ch == ',' && chPlus1 != ' ')
            {
                return true;
            }
            if (ch == '.' && chPlus1 != ' ')
            {
                return true;
            }
            return false;
        }
        private static char SetToUpper(char ch)
        {
            if (Char.IsLetter(ch))
            {
                return Char.ToUpper(ch);
            }
            return ch;
        }
        private static string GetValue(Worksheet worksheet, int rowIndex, int NemrcColumn)
        {
            if (worksheet.Cells[rowIndex, NemrcColumn].Value == null)
            {
                return "";
            }
            return worksheet.Cells[rowIndex, NemrcColumn].Value.ToString().Trim();
        }
    }
    */
}
