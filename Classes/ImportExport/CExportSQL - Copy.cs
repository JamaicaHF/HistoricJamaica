using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class CExportSQL : CImport
    {
        const string sFullName_col = "FullName";

        private CSql m_SQL;
        private StreamWriter m_StreamWriter;
        string ExportSQLTableHeaders;
        private const string ExportFolder = @"c:\JHF_SQLTables\";
        const string TabChar = ";";
        private bool forLocalHost = false;
        private int count = 0;

        public CExportSQL(CSql SQL, string sDataDirectory)
            : base(SQL, sDataDirectory)
        {
            m_SQL = SQL;
            try
            {
                forLocalHost = MessageBox.Show("For LocalHost?", "", MessageBoxButtons.YesNo) == DialogResult.Yes ? true : false;
                ExportDatabaseTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        public void ExportDatabaseTables()
        {
            if (!CreateExportFolder())
            {
                return;
            }
            if (OpenOutputFile(ExportFolder + "HistoricJamaica.sql"))
            {/*
                WriteAlternateSpellings("alternativespellingsfirstname");
                WriteAlternateSpellings("alternativespellingslastname");
                WriteSchool();
                WriteSchoolRecord();
                WriteCategory();
                WriteCategoryValue();
                WriteBuilding();
                WriteBuildingValue();
                WriteModernRoadValue();
                WriteGrandList();
                WritePicturedPersonRecords();
                WritePicturedBuildingRecords();
                WritePersonBuildingRecords();
                WritePhotoRecords();
                WritePhotoCategoryValue();
                WriteMarriageRecords();*/
                WritePersonCWRecords();/*
                bool firstRow = true;
                int rowNumber = 0;
                CloseAndReopenFile("VitalRecords.sql", ref rowNumber, ref firstRow);
                WriteVitalRecords();
                firstRow = true;
                CloseAndReopenFile("Persons.sql", ref rowNumber, ref firstRow);
                WritePersonRecords();*/
                CloseOutputFile();
                MessageBox.Show("Export Complete to folder " + ExportFolder);
            }
        }
        private void CloseAndReopenFile(string filename, ref int rowNumber, ref bool firstRow)
        {
            if (forLocalHost) // vital record or Person table is too large for one SQL import file in the MySQL database
            {
                CloseOutputFile();
                if (!OpenOutputFile(ExportFolder + filename))
                {
                    throw new Exception("Unable to Open File " + filename);
                }
                if (!firstRow)
                {
                    if (filename.ToLower().Contains("person"))
                    {
                        CreateTable("person", false);
                    }
                    else
                    {
                        CreateTable("vitalrecord", false);
                    }
                }
                rowNumber = 1;
                firstRow = true;
            }
            else
            {
                firstRow = false;
            }
        }
        //****************************************************************************************************************************
        private bool CreateExportFolder()
        {
            try
            {
                ExportSQLTableHeaders = SQL.DataDirectory() + @"\ExportSQLTableHeaders\";
                if (Directory.Exists(ExportFolder))
                {
                    var files = Directory.GetFiles(ExportFolder);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
                else
                {
                    Directory.CreateDirectory(ExportFolder);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Remove Export Folder: " + ex.Message);
                return false;
            }
        }
        //****************************************************************************************************************************
        public bool OpenOutputFile(string sFileNameWithPath)
        {
            if (sFileNameWithPath.Length == 0)
            {
                return false;
            }
            string sFileName = GetFileNameFromPath(sFileNameWithPath);
            try
            {
                m_StreamWriter = new StreamWriter(sFileNameWithPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public void CloseOutputFile()
        {
            m_StreamWriter.Close();
        }
        //****************************************************************************************************************************
        private void WriteValue(string value,
                                bool firstField)
        {
            string prefix = (firstField) ? "(" : ",";
            value = value.Replace("'", "''");
            m_StreamWriter.Write(prefix + "'" + value + "'");
        }
        //****************************************************************************************************************************
        private void WriteValue(int value,
                                bool firstField)
        {
            string prefix = (firstField) ? "(" : ",";
            m_StreamWriter.Write(prefix + value.ToString());
        }
        //****************************************************************************************************************************
        private void FirstRow(ref bool firstRow)
        {
            if (firstRow)
            {
                firstRow = false;
            }
            else
            {
                m_StreamWriter.WriteLine(",");
            }
        }
        //****************************************************************************************************************************
        private void WriteSchool()
        {
            CreateTable("school");
            DataTable schoolTbl = SQL.GetAllSchools();
            bool firstRow = true;
            foreach (DataRow row in schoolTbl.Rows)
            {
                FirstRow(ref firstRow);
                WriteSchoolRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteSchoolRecord(DataRow SchoolRow)
        {
            WriteValue(SchoolRow[U.SchoolID_col].ToInt(), true);
            WriteValue(SchoolRow[U.School_col].ToString(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteSchoolRecord()
        {
            CreateTable("schoolrecord");
            DataTable schoolTbl = SQL.GetAllSchoolRecords();
            bool firstRow = true;
            foreach (DataRow row in schoolTbl.Rows)
            {
                FirstRow(ref firstRow);
                WriteSchoolRecordRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteSchoolRecordRecord(DataRow SchoolRow)
        {
            WriteValue(SchoolRow[U.SchoolRecordID_col].ToInt(), true);
            WriteValue(SchoolRow[U.SchoolID_col].ToInt(), false);
            WriteValue(SchoolRow[U.SchoolRecordType_col].ToInt(), false);
            WriteValue(SchoolRow[U.Year_col].ToInt(), false);
            WriteValue(SchoolRow[U.Grade_col].ToString(), false);
            WriteValue(SchoolRow[U.BornDate_col].ToString(), false);
            WriteValue(SchoolRow[U.Person_col].ToString(), false);
            WriteValue(SchoolRow[U.PersonID_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteCategory()
        {
            CreateTable("category");
            DataTable categoryTbl = SQL.GetAllCategoriesWithPhotos();
            bool firstRow = true;
            foreach (DataRow row in categoryTbl.Rows)
            {
                FirstRow(ref firstRow);
                WriteCategoryRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteCategoryValue()
        {
            CreateTable("categoryvalue");
            DataTable categoryValueTbl = SQL.GetAllCategoryValuesWithPhotos();
            bool firstRow = true;
            foreach (DataRow row in categoryValueTbl.Rows)
            {
                FirstRow(ref firstRow);
                WriteCategoryValueRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteCategoryValueRecord(DataRow categoryValueRow)
        {
            WriteValue(categoryValueRow[U.CategoryValueID_col].ToInt(), true);
            WriteValue(categoryValueRow[U.CategoryID_col].ToInt(), false);
            WriteValue(categoryValueRow[U.CategoryValueValue_col].ToString(), false);
            WriteValue(categoryValueRow[U.CategoryValueOrder_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WritePhotoCategoryValue()
        {
            CreateTable("photocategoryvalue");
            DataTable photoCategoryValueTbl = SQL.GetAllPhotoCategoryValues();
            bool firstRow = true;
            foreach (DataRow row in photoCategoryValueTbl.Rows)
            {
                FirstRow(ref firstRow);
                WritePhotoCategoryValueRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WritePhotoCategoryValueRecord(DataRow categoryValueRow)
        {
            WriteValue(categoryValueRow[U.PhotoID_col].ToInt(), true);
            WriteValue(categoryValueRow[U.CategoryValueID_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteCategoryRecord(DataRow categoryRow)
        {
            WriteValue(categoryRow[U.CategoryID_col].ToInt(), true);
            WriteValue(categoryRow[U.CategoryName_col].ToString(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteBuilding()
        {
            CreateTable("building");
            DataTable buildingTbl = SQL.GetAllBuildings();
            bool firstRow = true;
            foreach (DataRow row in buildingTbl.Rows)
            {
                FirstRow(ref firstRow);
                CheckNotes(row, U.Notes_col);
                CheckNotes(row, U.NotesCurrentOwner_col);
                CheckNotes(row, U.Notes1869Name_col);
                WriteBuildingRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteBuildingRecord(DataRow buildingRow)
        {
            WriteValue(buildingRow[U.BuildingID_col].ToInt(), true);
            WriteValue(buildingRow[U.BuildingName_col].ToString(), false);
            WriteValue(buildingRow[U.BuildingRoadValueID_col].ToInt(), false);
            WriteValue(buildingRow[U.StreetNum_col].ToInt(), false);
            WriteValue(buildingRow[U.BuildingGrandListID_col].ToInt(), false);
            WriteValue(buildingRow[U.Building1869Name_col].ToString(), false);
            WriteValue(buildingRow[U.Notes_col].ToString().Trim(), false);
            WriteValue(buildingRow[U.NotesCurrentOwner_col].ToString(), false);
            WriteValue(buildingRow[U.Notes1869Name_col].ToString(), false);
            WriteValue(buildingRow[U.BuildingValueOrderCurrentOwner_col].ToInt(), false);
            WriteValue(buildingRow[U.BuildingValueOrder1869Name_col].ToInt(), false);
            WriteValue(buildingRow[U.BuildingArchitectureArticleID_col].ToInt(), false);
            WriteValue(buildingRow[U.BuildingDescriptionArticleID_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteBuildingValue()
        {
            CreateTable("buildingvalue");
            DataTable buildingValueTbl = new DataTable();
            SQL.GetAllBuildingValues(buildingValueTbl);
            bool firstRow = true;
            foreach (DataRow row in buildingValueTbl.Rows)
            {
                FirstRow(ref firstRow);
                CheckNotes(row, U.Notes_col);
                WriteBuildingValueRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteBuildingValueRecord(DataRow buildingValueRow)
        {
            WriteValue(buildingValueRow[U.BuildingValueID_col].ToInt(), true);
            WriteValue(buildingValueRow[U.BuildingID_col].ToInt(), false);
            WriteValue(buildingValueRow[U.BuildingValueValue_col].ToString(), false);
            WriteValue(buildingValueRow[U.BuildingValueOrder_col].ToInt(), false);
            WriteValue(buildingValueRow[U.Notes_col].ToString().Trim(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteModernRoadValue()
        {
            CreateTable("modernroadvalue");
            DataTable ModernRoadValueTbl = SQL.GetAllModernRoadValues();
            bool firstRow = true;
            foreach (DataRow row in ModernRoadValueTbl.Rows)
            {
                if (row[U.ModernRoadValueSection_col].ToInt() != 0)
                {
                    FirstRow(ref firstRow);
                    WriteModernRoadValueRecord(row);
                }
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteModernRoadValueRecord(DataRow modernRoadValueRow)
        {
            WriteValue(modernRoadValueRow[U.ModernRoadValueID_col].ToInt(), true);
            WriteValue(modernRoadValueRow[U.ModernRoadValueValue_col].ToString(), false);
            WriteValue(modernRoadValueRow[U.ModernRoadValueOrder_col].ToInt(), false);
            WriteValue(modernRoadValueRow[U.ModernRoadValueSection_col].ToInt(), false);
            WriteValue(modernRoadValueRow[U.JRoadName_col].ToString(), false);
            WriteValue(modernRoadValueRow[U.HistoricRoad_col].ToString(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteGrandList()
        {
            CreateTable("grandlist");
            DataTable grandListTbl = SQL.GetGrandListPropertys();
            bool firstRow = true;
            foreach (DataRow row in grandListTbl.Rows)
            {
                FirstRow(ref firstRow);
                WriteGrandListRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteGrandListRecord(DataRow grandListRow)
        {
            WriteValue(grandListRow[U.GrandListID_col].ToInt(), true);
            WriteValue(grandListRow[U.TaxMapID_col].ToString(), false);
            WriteValue(grandListRow[U.StreetName_col].ToString(), false);
            WriteValue(grandListRow[U.StreetNum_col].ToInt(), false);
            WriteValue(grandListRow[U.Name1_col].ToString(), false);
            WriteValue(grandListRow[U.Name2_col].ToString(), false);
            WriteValue(grandListRow[U.BuildingRoadValueID_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WritePhotoRecords()
        {
            CreateTable("photo");
            DataTable PhotoPerson_tbl = SQL.GetAllPhotos();
            bool firstRow = true;
            foreach (DataRow row in PhotoPerson_tbl.Rows)
            {
                FirstRow(ref firstRow);
                CheckNotes(row, U.PhotoNotes_col);
                WritePhotoRecord(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WritePhotoRecord(DataRow Photo_row)
        {
            WriteValue(Photo_row[U.PhotoID_col].ToInt(), true);
            WriteValue(Photo_row[U.PhotoName_col].ToString(), false);
            WriteValue(Photo_row[U.PhotoExtension_col].ToString(), false);
            WriteValue(Photo_row[U.PhotoNotes_col].ToString().Trim(), false);
            WriteValue(Photo_row[U.PhotoYear_col].ToInt(), false);
            WriteValue(Photo_row[U.PhotoSource_col].ToString(), false);
            WriteValue(Photo_row[U.PhotoDrawer_col].ToInt(), false);
            WriteValue(Photo_row[U.PhotoFolder_col].ToInt(), false);
            WriteValue(Photo_row[U.NumPicturedPersons_col].ToInt(), false);
            WriteValue(Photo_row[U.NumPicturedBuildings_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WritePicturedPersonRecords()
        {
            CreateTable("picturedperson");
            DataTable PicturedPerson_tbl = SQL.GetAllPicturedPersons();
            bool firstRow = true;
            foreach (DataRow row in PicturedPerson_tbl.Rows)
            {
                FirstRow(ref firstRow);
                WritePicturedPerson(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WritePicturedPerson(DataRow Person_row)
        {
            WriteValue(Person_row[U.PicturedPersonNumber_col].ToInt(), true);
            WriteValue(Person_row[U.PhotoID_col].ToInt(), false);
            WriteValue(Person_row[U.PersonID_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WritePicturedBuildingRecords()
        {
            CreateTable("picturedbuilding");
            DataTable PicturedBuilding_tbl = SQL.GetAllPicturedBuildings();
            bool firstRow = true;
            foreach (DataRow row in PicturedBuilding_tbl.Rows)
            {
                FirstRow(ref firstRow);
                WritePicturedBuilding(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WritePicturedBuilding(DataRow Building_row)
        {
            WriteValue(Building_row[U.PicturedBuildingNumber_col].ToInt(), true);
            WriteValue(Building_row[U.PhotoID_col].ToInt(), false);
            WriteValue(Building_row[U.BuildingID_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteMarriageRecords()
        {
            CreateTable("marriage");
            DataTable tbl = SQL.GetMarriageRecords();
            bool firstRow = true;
            foreach (DataRow row in tbl.Rows)
            {
                FirstRow(ref firstRow);
                WriteMarriageRecords(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteMarriageRecords(DataRow row)
        {
            WriteValue(row[U.PersonID_col].ToInt(), true);
            WriteValue(row[U.SpouseID_col].ToInt(), false);
            WriteValue(row[U.DateMarried_col].ToString().toDate(), false);
            WriteValue(row[U.Divorced_col].ToString(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WritePersonBuildingRecords()
        {
            CreateTable("buildingoccupant");
            DataTable tbl = SQL.GetBuildingOccupantRecords();
            bool firstRow = true;
            foreach (DataRow row in tbl.Rows)
            {
                FirstRow(ref firstRow);
                CheckNotes(row, U.Notes_col);
                WritePersonBuilding(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WritePersonBuilding(DataRow Person_row)
        {
            WriteValue(Person_row[U.PersonID_col].ToInt(), true);
            WriteValue(Person_row[U.SpouseLivedWithID_col].ToInt(), false);
            WriteValue(Person_row[U.Notes_col].ToString().Trim(), false);
            WriteValue(Person_row[U.BuildingValueOrder_col].ToInt(), false);
            WriteValue(Person_row[U.BuildingID_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteAlternateSpellings(string sAlternativeSpelling_Table)
        {
            CreateTable(sAlternativeSpelling_Table);
            DataTable tbl = SQL.GetAllAlternativeSpellings(sAlternativeSpelling_Table);
            bool firstRow = true;
            foreach (DataRow row in tbl.Rows)
            {
                FirstRow(ref firstRow);
                WriteAlternateSpellings(row);
            }
            m_StreamWriter.WriteLine(";");
        }
        //****************************************************************************************************************************
        private void WriteAlternateSpellings(DataRow row)
        {
            WriteValue(row[U.NameSpelling1_Col].ToString(), true);
            WriteValue(row[U.NameSpelling2_Col].ToString(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WritePersonCWRecords()
        {
            CreateTable("personcw");
            DataTable PersonCW_tbl = SQL.GetAllPersonCWRecords();
            WriteAllPersonsCW(PersonCW_tbl);
        }
        //****************************************************************************************************************************
        private void WriteAllPersonsCW(DataTable PersonCW_tbl)
        {
            string sFullName_col = "FullName";
            PersonCW_tbl.Columns.Add(sFullName_col, typeof(string));
            foreach (DataRow row in PersonCW_tbl.Rows)
            {
                CheckNotes(row, U.Notes_col);
                string sFullName = SQL.BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                                      row[U.MiddleName_col].ToString(),
                                                      row[U.LastName_col].ToString(),
                                                      "", "", "", "");
                row[sFullName_col] = sFullName;
            }
            try
            {
                string sortExp = sFullName_col;
                DataRow[] drarray;
                drarray = PersonCW_tbl.Select(null, sortExp, DataViewRowState.CurrentRows);
                int num = drarray.Length;
                bool firstRow = true;
                for (int iSortOrder = 0; iSortOrder < num; iSortOrder++)
                {
                    DataRow PersonCW_row = drarray[iSortOrder];
                    FirstRow(ref firstRow);
                    WritePersonCW(PersonCW_row);
                }
                m_StreamWriter.WriteLine(";");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void WritePersonCW(DataRow PersonCW_row)
        {
            int personId = PersonCW_row[U.PersonID_col].ToInt();
/*            WriteValue(PersonCW_row[U.PersonCWID_col].ToInt(), true);
            WriteValue(PersonCW_row[U.FirstName_col].ToString(), false);
            WriteValue(PersonCW_row[U.MiddleName_col].ToString(), false);
            WriteValue(PersonCW_row[U.LastName_col].ToString(), false);
            WritePersonCWVitalRecords(PersonCW_row, personId);
            WriteValue(PersonCW_row[U.EnlistmentDate_col].ToString(), false);
            WriteValue(PersonCW_row[U.CemeteryName_col].ToString().Trim(), false);
            WriteValue(PersonCW_row[U.BattleSiteKilled_col].ToString().Trim(), false);*/
            WriteDataMilitary(PersonCW_row[U.DataMilitary_col].ToString().Trim());
/*            WriteValue(PersonCW_row[U.Reference_col].ToString().Trim(), false);
            WriteValue(PersonCW_row[U.Notes_col].ToString().Trim(), false);
            WriteValue(personId, false);*/
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WriteDataMilitary(string dataMilitary)
        {
            try
            {
                string orgStr = dataMilitary;
                string outputStr = CreditedTo(ref dataMilitary) + "|";
                outputStr += Service(ref dataMilitary) + "|";
                string DataMilitaryBeforeTrTo;
                string TransferToStr = TransferTo(ref dataMilitary, out DataMilitaryBeforeTrTo);
                if (!string.IsNullOrEmpty(DataMilitaryBeforeTrTo))
                {
                    outputStr += ParseRemainingStr(DataMilitaryBeforeTrTo);
                }
                if (!string.IsNullOrEmpty(TransferToStr))
                {
                    outputStr += TransferToStr;
                }
                if (!string.IsNullOrEmpty(dataMilitary))
                {
                    outputStr += ParseRemainingStr(dataMilitary);
                }
                if (string.IsNullOrEmpty(outputStr))
                {
                    outputStr.Remove(outputStr.Length - 1);
                }
                else
                {
                    //outputStr += dataMilitary;
                }
                if (!string.IsNullOrEmpty(outputStr.Trim()))
                {
                    WriteValue(outputStr, false);
                }
            }
            catch (Exception ex)
            {
                ParseException(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private string ParseRemainingStr(string remainingStr)
        {
            return remainingStr;
        }
        //****************************************************************************************************************************
        private void ParseException(string dataMilitary)
        {
            if (string.IsNullOrEmpty(dataMilitary))
            {
                return;
            }
            string[] inputFields = dataMilitary.Split(';');
            string outputStr = "";
            foreach (string inputField in inputFields)
            {
                string outputField = inputField.Trim();
                int indexOf = outputField.ToLower().IndexOf("and ");
                if (indexOf == 0)
                {
                    outputField = outputField.Remove(0, 4);
                }
                indexOf = outputField.ToLower().IndexOf("was ");
                if (indexOf == 0)
                {
                    outputField = outputField.Remove(0, 4);
                }
                outputStr += CapitalizeFirstChar(outputField) + "|";
            }
            outputStr.Remove(outputStr.Length - 1);
            //WriteValue(outputStr, false);
        }
        //****************************************************************************************************************************
        private string  CapitalizeFirstChar(string str)
        {
            string firstChar = str[0].ToString().ToUpper();
            return firstChar + str.Remove(0, 1);
        }
        //****************************************************************************************************************************
        private string Service(ref string dataMilitary)
        {
            string returnString = "";
            dataMilitary = dataMilitary.Replace("service: ", "");
            LookForGroup("INF", ref returnString, ref dataMilitary);
            LookForGroup("USN", ref returnString, ref dataMilitary);
            LookForGroup("CAV", ref returnString, ref dataMilitary);
            LookForGroup("ARTY", ref returnString, ref dataMilitary);
            LookForGroup("BTRY", ref returnString, ref dataMilitary);
            LookForGroup("RCRT", ref returnString, ref dataMilitary);
            LookForGroup("USSS", ref returnString, ref dataMilitary);
            LookForGroup("USCT", ref returnString, ref dataMilitary);
            LookForGroup("CO.", ref returnString, ref dataMilitary);
            if (string.IsNullOrEmpty(returnString))
            {
                count++;
            }
            int indexOf = returnString.ToLower().IndexOf("enl");
            if (indexOf >= 0)
            {
                returnString = returnString.Remove(indexOf, 3);
                returnString = returnString.Insert(indexOf, "enlisted");
            }
            return CapitalizeFirstChar(returnString);
        }
        //****************************************************************************************************************************
        private string TransferTo(ref string dataMilitary, out string DataMilitaryBeforeTrTo)
        {
            DataMilitaryBeforeTrTo = "";
            int indexOf = dataMilitary.ToLower().IndexOf("tr to");
            if (indexOf < 0)
            {
                return "";
            }
            if (indexOf > 0)
            {
                DataMilitaryBeforeTrTo = dataMilitary.Substring(0, indexOf - 1);
                int lastChar = DataMilitaryBeforeTrTo.Length - 1;
                if (DataMilitaryBeforeTrTo[lastChar] == ',')
                {
                    DataMilitaryBeforeTrTo = DataMilitaryBeforeTrTo.Remove(lastChar);
                }
                dataMilitary = dataMilitary.Substring(indexOf);
            }
            string returnString = dataMilitary.Remove(0, 5);
            returnString = returnString.Insert(0, "Transfer To");
            indexOf = returnString.IndexOf(';');
            if (indexOf < 0)
            {
                indexOf = returnString.IndexOf(',');
            }
            if (indexOf > 0)
            {
                returnString = returnString.Remove(indexOf, 1);
                dataMilitary = returnString.Substring(indexOf).Trim();
                returnString = returnString.Substring(0, indexOf).Trim();
            }
            else
            {
                dataMilitary = "";
            }
            return returnString + "|";
        }
        //****************************************************************************************************************************
        private void LookForGroup(string group, ref string returnString, ref string dataMilitary)
        {
            if (!string.IsNullOrEmpty(returnString))
            {
                return;
            }
            int indexOfInf = dataMilitary.ToUpper().IndexOf(group);
            if (indexOfInf > 0)
            {
                returnString = dataMilitary.Substring(0, indexOfInf + group.Length);
                dataMilitary = dataMilitary.Substring(indexOfInf + group.Length).Trim();
                if (!string.IsNullOrEmpty(dataMilitary) && dataMilitary[0] == ',')
                {
                    dataMilitary = dataMilitary.Remove(0, 1);
                }
                dataMilitary = dataMilitary.Trim();
            }
        }
        //****************************************************************************************************************************
        private string CreditedTo(ref string dataMilitary)
        {
            string returnString;
            int indexOfVT = dataMilitary.IndexOf("VT");
            if (indexOfVT > 0)
            {
                int indexOfSemicolon = dataMilitary.IndexOf(';');
                if (indexOfSemicolon == indexOfVT + 2)
                {
                    dataMilitary = dataMilitary.Remove(indexOfSemicolon, 1);
                }
                returnString = dataMilitary.Substring(0, indexOfVT + 2);
                dataMilitary = dataMilitary.Substring(indexOfVT + 2).Trim();
            }
            else
            {
                returnString = VtExceptions(ref dataMilitary);
            }
            returnString = returnString.Replace("credited to", "Credited To");
            returnString = returnString.Replace("cred.", "Credited To");
            returnString = returnString.Replace("Age: 0, ", "");
            return returnString;
        }
        //****************************************************************************************************************************
        private string VtExceptions(ref string dataMilitary)
        {
            int indexOfUSN = dataMilitary.IndexOf("USN");
            if (indexOfUSN < 0)
            {
                int indexOf = dataMilitary.ToLower().IndexOf("phelps:");
                if (indexOf > 0)
                {
                    dataMilitary = dataMilitary.Substring(indexOf + 7);
                    throw new Exception(dataMilitary);
                }
                return "";
            }
            string returnString = dataMilitary.Substring(0, indexOfUSN + 3);
            dataMilitary = dataMilitary.Substring(indexOfUSN + 3).Trim();
            return returnString;
        }
        //****************************************************************************************************************************
        private void WritePersonCWVitalRecords(DataRow PersonCW_row, int personId)
        {
            DataRow person_row = SQL.GetPerson(personId);
            DataTable CemeteryRecord_tbl = SQL.DefineVitalRecord_Table();
            DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
            if (personId != 0)
            {
                SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, person_row[U.PersonID_col].ToInt());
                SQL.GetVitalRecordsForPerson(VitalRecord_tbl, person_row[U.PersonID_col].ToInt(), U.PersonID_col);
            }
            else
            {
            }
            string bornDate = PersonCW_row[U.BornDate_col].ToString();
            string diedDate = PersonCW_row[U.DiedDate_col].ToString();
            string book = "", page = "", place = "", home = "", source = "";
            U.GetPersonVitalStatistics(VitalRecord_tbl, CemeteryRecord_tbl, person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                                    ref book, ref page, ref bornDate, ref place, ref home, ref source);
            WriteValue(bornDate.toDate(), false);
            U.GetPersonVitalStatistics(VitalRecord_tbl, CemeteryRecord_tbl, person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                                    ref book, ref page, ref diedDate, ref place, ref home, ref source);
            WriteValue(diedDate.toDate(), false);
        }
        //****************************************************************************************************************************
        private void WritePersonRecords()
        {
            CreateTable("person");
            DataTable Person_tbl = SQL.GetAllPersons();
            WriteAllPersons(Person_tbl);
        }
        //****************************************************************************************************************************
        private void WriteAllPersons(DataTable Person_tbl)
        {
            string sFullName_col = "FullName";
            Person_tbl.Columns.Add(sFullName_col, typeof(string));
            foreach (DataRow row in Person_tbl.Rows)
            {
                CheckNotes(row, U.Notes_col);
                string sFullName = SQL.BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                                      row[U.MiddleName_col].ToString(),
                                                      row[U.LastName_col].ToString(),
                                                      row[U.Suffix_col].ToString(),
                                                      row[U.Prefix_col].ToString(),
                                                      row[U.MarriedName_col].ToString(),
                                                      row[U.KnownAs_col].ToString());
                row[sFullName_col] = sFullName;
            }
            try
            {
                string sortExp = sFullName_col;
                DataRow[] drarray;
                drarray = Person_tbl.Select(null, sortExp, DataViewRowState.CurrentRows);
                bool firstRow = true;
                int num = drarray.Length;
                int fileNumber = 1;
                int rowNumber = 0;
                for (int iSortOrder = 0; iSortOrder < num; iSortOrder++)
                {
                    DataRow Person_row = drarray[iSortOrder];
                    if (!Person_row[U.ExcludeFromSite_col].ToBool() &&
                        Person_row[U.FirstName_col].ToString().Length != 0 &&
                       (Person_row[U.MarriedName_col].ToString().Length != 0 ||
                        Person_row[U.LastName_col].ToString().Length != 0))
                    {
                        rowNumber++;
                        if (rowNumber > 4000)
                        {
                            CloseAndReopenFile("Persons" + (++fileNumber) + ".sql", ref rowNumber, ref firstRow);
                        }
                        FirstRow(ref firstRow);
                        WritePerson(Person_row, iSortOrder);
                    }
                }
                m_StreamWriter.WriteLine(";");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void WritePerson(DataRow Person_row,
                                 int iSortOrder)
        {
            WriteValue(Person_row[U.PersonID_col].ToInt(), true);
            WriteValue(Person_row[U.FirstName_col].ToString(), false);
            WriteValue(Person_row[U.MiddleName_col].ToString(), false);
            WriteValue(Person_row[U.LastName_col].ToString(), false);
            WriteValue(Person_row[U.MarriedName_col].ToString(), false);
            WriteValue(Person_row[U.MarriedName2_col].ToString(), false);
            WriteValue(Person_row[U.MarriedName3_col].ToString(), false);
            WriteValue(Person_row[U.KnownAs_col].ToString(), false);
            WriteValue(Person_row[U.Suffix_col].ToString(), false);
            WriteValue(Person_row[U.Prefix_col].ToString(), false);
            WriteValue(Person_row[U.FatherID_col].ToInt(), false);
            WriteValue(Person_row[U.MotherID_col].ToInt(), false);
            WriteValue(Person_row[U.Notes_col].ToString().Trim(), false);
            WriteValue(Person_row[U.Source_col].ToString(), false);
            WriteValue(Person_row[U.Sex_col].ToString(), false);
            WritePersonVitalRecords(Person_row);
            WriteValue(iSortOrder.ToInt(), false);
            WriteValue(Person_row[U.Census1790_col].ToInt(), false);
            WriteValue(Person_row[U.Census1800_col].ToInt(), false);
            WriteValue(Person_row[U.Census1810_col].ToInt(), false);
            WriteValue(Person_row[U.Census1820_col].ToInt(), false);
            WriteValue(Person_row[U.Census1830_col].ToInt(), false);
            WriteValue(Person_row[U.Census1840_col].ToInt(), false);
            WriteValue(Person_row[U.Census1850_col].ToInt(), false);
            WriteValue(Person_row[U.Census1860_col].ToInt(), false);
            WriteValue(Person_row[U.Census1870_col].ToInt(), false);
            WriteValue(Person_row[U.Census1880_col].ToInt(), false);
            WriteValue(Person_row[U.Census1900_col].ToInt(), false);
            WriteValue(Person_row[U.Census1910_col].ToInt(), false);
            WriteValue(Person_row[U.Census1920_col].ToInt(), false);
            WriteValue(Person_row[U.Census1930_col].ToInt(), false);
            WriteValue(Person_row[U.Census1940_col].ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private void WritePersonVitalRecords(DataRow Person_row)
        {
            DataTable CemeteryRecord_tbl = SQL.DefineVitalRecord_Table();
            SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, Person_row[U.PersonID_col].ToInt());
            DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
            SQL.GetVitalRecordsForPerson(VitalRecord_tbl, Person_row[U.PersonID_col].ToInt(), U.PersonID_col);
            VitalRecord(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                        Person_row[U.BornDate_col].ToString(), Person_row[U.BornPlace_col].ToString(),
                        Person_row[U.BornHome_col].ToString(), Person_row[U.BornBook_col].ToString(),
                        Person_row[U.BornPage_col].ToString(), Person_row[U.BornSource_col].ToString());
            VitalRecord(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale,
                        Person_row[U.DiedDate_col].ToString(), Person_row[U.DiedPlace_col].ToString(),
                        Person_row[U.DiedHome_col].ToString(), Person_row[U.DiedBook_col].ToString(),
                        Person_row[U.DiedPage_col].ToString(), Person_row[U.DiedSource_col].ToString());
            VitalRecord(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, EVitalRecordType.eBurial, EVitalRecordType.eSearch,
                        Person_row[U.BuriedDate_col].ToString(), Person_row[U.BuriedPlace_col].ToString(),
                        Person_row[U.BuriedStone_col].ToString(), Person_row[U.BuriedBook_col].ToString(),
                        Person_row[U.BuriedPage_col].ToString(), Person_row[U.BuriedSource_col].ToString());
        }
        //****************************************************************************************************************************
        private void VitalRecord(DataTable VitalRecord_tbl,
                                 DataTable CemeteryRecord_tbl,
                                 DataRow Person_row,
                                 EVitalRecordType eVitalRecordType1,
                                 EVitalRecordType eVitalRecordType2,
                                 string date,
                                 string place,
                                 string home,
                                 string book,
                                 string page,
                                 string source)
        {
            bool verified = U.GetPersonVitalStatistics(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, eVitalRecordType1, eVitalRecordType2,
                                                                ref book, ref page, ref date, ref place, ref home, ref source);
            WriteValue(date.toDate(), false);
            WriteValue(place, false);
            WriteValue(home, false);
            if (verified)
                WriteValue("Y", false);
            else
                WriteValue("N", false);
            WriteValue(source, false);
            WriteValue(book, false);
            WriteValue(page, false);
        }
        //****************************************************************************************************************************
        private void WriteCemeteryRecords()
        {
            CreateTable("cemeteryrecord");
            DataTable CemeteryRecord_tbl = SQL.GetAllCemeteryRecords();
            WriteAllCemeteryRecords(CemeteryRecord_tbl);
        }
        //****************************************************************************************************************************
        private void WriteVitalRecords()
        {
            CreateTable("vitalrecord");
            DataTable VitalRecord_tbl = SQL.GetAllVitalRecords();
            WriteAllVitalRecords(VitalRecord_tbl);
        }
        //****************************************************************************************************************************
        private void CheckNotes(DataRow row, string str_col)
        {
            string notes = row[str_col].ToString();
            StringBuilder notesString = null;
            for (int i = 0; i < notes.Length; i++)
            {
                char c = notes[i];
                if (c == '\n')
                {
                    if (notesString == null)
                    {
                        notesString = new StringBuilder(notes);
                    }
                    notesString[i] = '-';
                }
            }
            if (notesString != null)
            {
                row[str_col] = notesString.ToString();
            }
        }
        //****************************************************************************************************************************
        private void WriteAllCemeteryRecords(DataTable CemeteryRecord_tbl)
        {
            CemeteryRecord_tbl.Columns.Add(sFullName_col, typeof(string));
            const string knownAs = "";
            foreach (DataRow row in CemeteryRecord_tbl.Rows)
            {
                CheckNotes(row, U.Notes_col);
                string sFullName = SQL.BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                                      row[U.MiddleName_col].ToString(),
                                                      row[U.LastName_col].ToString(),
                                                      row[U.Suffix_col].ToString(),
                                                      row[U.Prefix_col].ToString(),
                                                      row[U.SpouseLastName_col].ToString(),
                                                      knownAs);
                row[sFullName_col] = sFullName;
            }
            try
            {
                string sortExp = sFullName_col;
                DataRow[] drarray;
                drarray = CemeteryRecord_tbl.Select(null, sortExp, DataViewRowState.CurrentRows);
                bool firstRow = true;
                int num = drarray.Length;
                for (int iSortOrder = 0; iSortOrder < num; iSortOrder++)
                {
                    DataRow row = drarray[iSortOrder];
                    if (row[U.FirstName_col].ToString().Length != 0 &&
                        row[U.LastName_col].ToString().Length != 0)
                    {
                        FirstRow(ref firstRow);
                        WriteCemeteryRecord(row, iSortOrder);
                    }
                }
                m_StreamWriter.WriteLine(";");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void WriteCemeteryRecord(DataRow row,
                                      int iSortOrder)
        {
            WriteValue(row[U.CemeteryRecordID_col].ToInt(), true);
            WriteValue(row[U.FirstName_col].ToString(), false);
            WriteValue(row[U.MiddleName_col].ToString(), false);
            WriteValue(row[U.LastName_col].ToString(), false);
            WriteValue(row[U.SpouseLastName_col].ToString(), false);
            WriteValue("", false);
            WriteValue(row[U.Suffix_col].ToString(), false);
            WriteValue(row[U.Prefix_col].ToString(), false);

            WriteValue(row[U.SpouseFirstName_col].ToString(), false);
            WriteValue(row[U.SpouseMiddleName_col].ToString(), false);
            WriteValue(row[U.SpouseLastName_col].ToString(), false);
            WriteValue(row[U.SpouseSuffix_col].ToString(), false);
            WriteValue(row[U.SpousePrefix_col].ToString(), false);

            WriteValue(row[U.FatherFirstName_col].ToString(), false);
            WriteValue(row[U.FatherMiddleName_col].ToString(), false);
            WriteValue(row[U.FatherLastName_col].ToString(), false);
            WriteValue(row[U.FatherSuffix_col].ToString(), false);
            WriteValue(row[U.FatherPrefix_col].ToString(), false);

            WriteValue(row[U.MotherFirstName_col].ToString(), false);
            WriteValue(row[U.MotherMiddleName_col].ToString(), false);
            WriteValue(row[U.MotherLastName_col].ToString(), false);
            WriteValue(row[U.MotherSuffix_col].ToString(), false);
            WriteValue(row[U.MotherPrefix_col].ToString(), false);

            WriteValue(row[U.Sex_col].ToString(), false);
            WriteValue(row[U.Notes_col].ToString().Trim(), false);
            WriteValue(row[U.Epitaph_col].ToString(), false);

            //sBornDate = U.VitalRecordBornDate(EVitalRecordType.eBurial, row);
            WriteValue(row[U.BornDate_col].ToString(), false);
            WriteValue(row[U.DiedDate_col].ToString(), false);
            string cemeteryName = U.CemeteryName((Cemetery)row[U.CemeteryID_col].ToInt());
            WriteValue(U.AbbreviatedDeathInfo(cemeteryName, row[U.LotNumber_col].ToString(), row[U.BornDate_col].ToString(),
                        row[U.AgeYears_col].ToInt(), row[U.AgeMonths_col].ToInt(), row[U.AgeDays_col].ToInt()), false);
            WriteValue(row[U.Disposition_col].ToString(), false);

            WriteValue(row[U.SpouseID_col].ToInt(), false);
            WriteValue(row[U.PersonID_col].ToInt(), false);
            WriteValue(row[U.FatherID_col].ToInt(), false);
            WriteValue(row[U.MotherID_col].ToInt(), false);
            WriteValue(iSortOrder.ToInt(), false);
            m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private static string GetPersonBornDate(DataTable personTbl, int personId)
        {
            if (personId == 0)
            {
                return "";
            }
            string selectStatement = "PersonID = '" + personId + "'";
            DataRow[] results = personTbl.Select(selectStatement);
            return (results.Length == 0) ? "" : results[0][U.BornDate_col].ToString();
        }
        //****************************************************************************************************************************
        private void WriteAllVitalRecords(DataTable VitalRecord_tbl)
        {
            VitalRecord_tbl.Columns.Add(sFullName_col, typeof(string));
            VitalRecord_tbl.Columns.Add(U.SpouseFirstName_col, typeof(string));
            VitalRecord_tbl.Columns.Add(U.SpouseMiddleName_col, typeof(string));
            VitalRecord_tbl.Columns.Add(U.SpouseLastName_col, typeof(string));
            VitalRecord_tbl.Columns.Add(U.SpouseSuffix_col, typeof(string));
            VitalRecord_tbl.Columns.Add(U.SpousePrefix_col, typeof(string));
            VitalRecord_tbl.Columns.Add(U.BornDate_col, typeof(string));
            VitalRecord_tbl.Columns.Add(U.Date_col, typeof(string));
            DataTable personTbl = SQL.GetAllPersons();
            const string knownAs = "";
            foreach (DataRow row in VitalRecord_tbl.Rows)
            {
                try
                {
                    CheckNotes(row, U.Notes_col);
                    EVitalRecordType vitalRecordType = (EVitalRecordType)row[U.VitalRecordType_col].ToInt();
                    string marriedName = AddSpouseNameToRow(row, vitalRecordType, row[U.SpouseID_col].ToInt());
                    string sFullName = SQL.BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                                          row[U.MiddleName_col].ToString(),
                                                          row[U.LastName_col].ToString(),
                                                          row[U.Suffix_col].ToString(),
                                                          row[U.Prefix_col].ToString(),
                                                          marriedName,
                                                          knownAs);
                    row[sFullName_col] = sFullName;
                    row[U.CemeteryName_col] = U.AbbreviatedDeathInfo(row[U.CemeteryName_col].ToString(), row[U.LotNumber_col].ToString(), row[U.BornDate_col].ToString(),
                                                          row[U.AgeYears_col].ToInt(), row[U.AgeMonths_col].ToInt(), row[U.AgeDays_col].ToInt());
                    row[U.BornDate_col] = U.VitalRecordBornDate(vitalRecordType, row, GetPersonBornDate(personTbl, row[U.PersonID_col].ToInt()));
                    string BuildDate = U.BuildDate(row[U.DateYear_col].ToInt(), row[U.DateMonth_col].ToInt(), row[U.DateDay_col].ToInt());
                    //row[U.Date_col] = (vitalRecordType.IsBirthRecord()) ? "" : BuildDate;
                    row[U.Date_col] = BuildDate;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Vital records: " + ex.Message);
                }
            }
            AddAllCemeteryRecords(VitalRecord_tbl, personTbl);
            try
            {
                DataView VitalRecord_dv = new DataView(VitalRecord_tbl);
                string sortFields = "FullName, " + U.VitalRecordType_col;
                VitalRecord_dv.Sort = sortFields;
                VitalRecord_tbl = VitalRecord_dv.ToTable();  

                //string sortExp = sFullName_col;
                //DataRow[] drarray;
                //drarray = VitalRecord_tbl.Select(null, sortExp, DataViewRowState.CurrentRows);
                bool firstRow = true;
                int num = VitalRecord_dv.Count;
                int fileNumber = 1;
                int rowNumber = 0;
                for (int iSortOrder = 0; iSortOrder < num; iSortOrder++)
                {
                    DataRow row = VitalRecord_tbl.Rows[iSortOrder];
                    if (!row[U.ExcludeFromSite_col].ToBool() &&
                         row[U.FirstName_col].ToString().Length != 0 &&
                         row[U.LastName_col].ToString().Length != 0)
                    {
                        rowNumber++;
                        if (rowNumber > 4000)
                        {
                            CloseAndReopenFile("VitalRecords" + (++fileNumber) + ".sql", ref rowNumber, ref firstRow);
                        }
                        FirstRow(ref firstRow);
                        WriteVitalRecord(row, iSortOrder);
                    }
                }
                m_StreamWriter.WriteLine(";");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void AddAllCemeteryRecords(DataTable VitalRecord_tbl, DataTable personTbl)
        {
            DataTable CemeteryRecord_tbl = SQL.GetAllCemeteryRecords();
            const string knownAs = "";
            int count = 0;
            foreach (DataRow cemeteryRecordRow in CemeteryRecord_tbl.Rows)
            {
                int cemeteryId = cemeteryRecordRow[U.CemeteryRecordID_col].ToInt();
                if (cemeteryId == 198)
                {
                }
                if (String.IsNullOrEmpty(cemeteryRecordRow[U.LastName_col].ToString()) && String.IsNullOrEmpty(cemeteryRecordRow[U.FirstName_col].ToString()))
                {
                    count++;
                    continue;
                }
                SQL.ConvertCemeteryRecordsToVitalRecords(cemeteryRecordRow, VitalRecord_tbl);
                int AddedRow = VitalRecord_tbl.Rows.Count - 1;
                DataRow vitalRecordRow = VitalRecord_tbl.Rows[AddedRow];
                CheckNotes(vitalRecordRow, U.Notes_col);
                string sFullName = SQL.BuildNameLastNameFirst(vitalRecordRow[U.FirstName_col].ToString(),
                                                      vitalRecordRow[U.MiddleName_col].ToString(),
                                                      vitalRecordRow[U.LastName_col].ToString(),
                                                      vitalRecordRow[U.Suffix_col].ToString(),
                                                      vitalRecordRow[U.Prefix_col].ToString(),
                                                      vitalRecordRow[U.SpouseLastName_col].ToString(),
                                                      knownAs);
                vitalRecordRow[sFullName_col] = sFullName;
                vitalRecordRow[U.SpouseFirstName_col] = cemeteryRecordRow[U.SpouseFirstName_col];
                vitalRecordRow[U.SpouseMiddleName_col] = cemeteryRecordRow[U.SpouseMiddleName_col];
                vitalRecordRow[U.SpouseLastName_col] = cemeteryRecordRow[U.SpouseLastName_col];
                vitalRecordRow[U.SpouseSuffix_col] = cemeteryRecordRow[U.SpouseSuffix_col];
                vitalRecordRow[U.SpousePrefix_col] = cemeteryRecordRow[U.SpousePrefix_col];
                vitalRecordRow[U.BornDate_col] = !String.IsNullOrEmpty(cemeteryRecordRow[U.BornDate_col].ToString()) ? cemeteryRecordRow[U.BornDate_col].ToString() :
                                                                      U.VitalRecordBornDate(EVitalRecordType.eBurial, vitalRecordRow, GetPersonBornDate(personTbl, vitalRecordRow[U.PersonID_col].ToInt()));
                EVitalRecordType vitalRecordType = (EVitalRecordType) vitalRecordRow[U.VitalRecordType_col].ToInt();
                vitalRecordRow[U.Date_col] = cemeteryRecordRow[U.DiedDate_col];
                vitalRecordRow[U.CemeteryName_col] = U.AbbreviatedDeathInfo(vitalRecordRow[U.CemeteryName_col].ToString(), vitalRecordRow[U.LotNumber_col].ToString(), vitalRecordRow[U.BornDate_col].ToString(),
                                                      vitalRecordRow[U.AgeYears_col].ToInt(), vitalRecordRow[U.AgeMonths_col].ToInt(), vitalRecordRow[U.AgeDays_col].ToInt());
            }
        }
        //****************************************************************************************************************************
        private void CloseFileAndOpenNewFile(string filename)
        {
            CloseOutputFile();
            if (OpenOutputFile(filename)) // vital record table is too large for one SQL file in the MySQL database
            {
                if (filename.ToLower().Contains("person"))
                {
                    CreateTable("person", false);
                }
                else
                {
                    CreateTable("vitalrecord", false);
                }
            }
            else
            {
                throw new Exception("Unable to Open " + filename);
            }
        }
        //****************************************************************************************************************************
        private void WriteVitalRecord(DataRow row,
                                      int iSortOrder)
        {
            if (iSortOrder == 1831)
            {
            }
                WriteValue(row[U.VitalRecordID_col].ToInt(), true);
                WriteValue(row[U.VitalRecordType_col].ToInt(), false);
                WriteValue(row[U.FirstName_col].ToString(), false);
                WriteValue(row[U.MiddleName_col].ToString(), false);
                WriteValue(row[U.LastName_col].ToString(), false);
                if ((EVitalRecordType)row[U.VitalRecordType_col].ToInt() == EVitalRecordType.eMarriageBride ||
                     row[U.Sex_col].ToChar() == 'F')
                {
                    WriteValue(row[U.SpouseLastName_col].ToString(), false);
                }
                else
                {
                    WriteValue("", false);
                }
                WriteValue("", false);
                WriteValue(row[U.Suffix_col].ToString(), false);
                WriteValue(row[U.Prefix_col].ToString(), false);

                WriteValue(row[U.SpouseFirstName_col].ToString(), false);
                WriteValue(row[U.SpouseMiddleName_col].ToString(), false);
                WriteValue(row[U.SpouseLastName_col].ToString(), false);
                WriteValue(row[U.SpouseSuffix_col].ToString(), false);
                WriteValue(row[U.SpousePrefix_col].ToString(), false);

                WriteValue(row[U.FatherFirstName_col].ToString(), false);
                WriteValue(row[U.FatherMiddleName_col].ToString(), false);
                WriteValue(row[U.FatherLastName_col].ToString(), false);
                WriteValue(row[U.FatherSuffix_col].ToString(), false);
                WriteValue(row[U.FatherPrefix_col].ToString(), false);

                WriteValue(row[U.MotherFirstName_col].ToString(), false);
                WriteValue(row[U.MotherMiddleName_col].ToString(), false);
                WriteValue(row[U.MotherLastName_col].ToString(), false);
                WriteValue(row[U.MotherSuffix_col].ToString(), false);
                WriteValue(row[U.MotherPrefix_col].ToString(), false);

                WriteValue(row[U.Sex_col].ToString(), false);
                WriteValue(row[U.Book_col].ToString(), false);
                WriteValue(row[U.Page_col].ToString(), false);
                WriteValue(row[U.Notes_col].ToString().Trim(), false);

                WriteValue(row[U.BornDate_col].ToString(), false);
                WriteValue(row[U.Date_col].ToString(), false);
                WriteValue(row[U.CemeteryName_col].ToString(), false);
                WriteValue(row[U.Disposition_col].ToString(), false);

                WriteValue(row[U.SpouseID_col].ToInt(), false);
                WriteValue(row[U.PersonID_col].ToInt(), false);
                WriteValue(row[U.FatherID_col].ToInt(), false);
                WriteValue(row[U.MotherID_col].ToInt(), false);
                WriteValue(iSortOrder.ToInt(), false);
                m_StreamWriter.Write(")");
        }
        //****************************************************************************************************************************
        private string AddSpouseNameToRow(DataRow vitalRecordRow,
                                        EVitalRecordType eVitalRecordType,
                                        int spouseID)
        {
            if (spouseID == 0 || eVitalRecordType.SpouseRecordType() == EVitalRecordType.eSearch)
            {
                SetSpouseNameSpaces(vitalRecordRow);
                return "";
            }
            DataRow spouseRow = SQL.GetVitalRecord(spouseID);
            if (spouseRow == null)
            {
                SetSpouseNameSpaces(vitalRecordRow);
                return "";
            }
            else
            {
                vitalRecordRow[U.SpouseFirstName_col] = spouseRow[U.FirstName_col];
                vitalRecordRow[U.SpouseMiddleName_col] = spouseRow[U.MiddleName_col];
                vitalRecordRow[U.SpouseLastName_col] = spouseRow[U.LastName_col];
                vitalRecordRow[U.SpouseSuffix_col] = spouseRow[U.Suffix_col];
                vitalRecordRow[U.SpousePrefix_col] = spouseRow[U.Prefix_col];
                if (eVitalRecordType == EVitalRecordType.eMarriageBride)
                {
                    return spouseRow[U.LastName_col].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        //****************************************************************************************************************************
        private void SetSpouseNameSpaces(DataRow vitalRecordRow)
        {
            vitalRecordRow[U.SpouseFirstName_col] = "";
            vitalRecordRow[U.SpouseMiddleName_col] = "";
            vitalRecordRow[U.SpouseLastName_col] = "";
            vitalRecordRow[U.SpouseSuffix_col] = "";
            vitalRecordRow[U.SpousePrefix_col] = "";
        }
        //****************************************************************************************************************************
        private void CreateTable(string tableName, bool dropCreateTable=true)
        {
            string sFileName = ExportSQLTableHeaders + @"Create" + tableName + ".sql";
            try
            {
                if (!OpenFileForInput(sFileName))
                    return;
                string sInputRecord;
                sInputRecord = ReadRecord();
                ArrayList fieldList = new ArrayList();
                while (sInputRecord != null)
                {
                    string fieldName = WriteInsertFields(sInputRecord, dropCreateTable);
                    if (fieldName.Length != 0)
                    {
                        fieldList.Add(fieldName);
                    }
                    sInputRecord = ReadRecord();
                }
                string insertString = "Insert Into " + tableName + " (";
                m_StreamWriter.Write(insertString);
                bool firstTime = true;
                foreach (string field in fieldList)
                {
                    if (firstTime) 
                    { 
                        firstTime = false; 
                    } 
                    else 
                    { 
                        m_StreamWriter.Write(','); 
                    }
                    m_StreamWriter.Write(field);
                }
                m_StreamWriter.WriteLine(") values ");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //****************************************************************************************************************************
        private string WriteInsertFields(string sInputRecord, 
                                         bool dropCreateTable)
        {
            if (dropCreateTable) // if this is not the 1st file for table, do not include the drop and create commands
            {
                m_StreamWriter.WriteLine(sInputRecord);
            }
            if (sInputRecord.Length < 4 || sInputRecord[0] != ' ')
            {
                return "";
            }
            int location = sInputRecord.IndexOf(' ', 4);
            if (location < 0) { return ""; }
            if (sInputRecord.IndexOf("PRIMARY KEY") >= 0) 
            { 
                return ""; 
            }
            return sInputRecord.Substring(0, location).Trim();
        }
    }
}
