using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {
        //****************************************************************************************************************************
        public static bool GetMarriage(DataTable tblSpouses,
                                       int iPersonID,
                                       int iSpouseID)
        {
            int iCountBeforeRead = tblSpouses.Rows.Count;
            SelectAll(U.Marriage_Table, tblSpouses, new NameValuePair(U.PersonID_col, iPersonID),
                                                    new NameValuePair(U.SpouseID_col, iSpouseID));
            if (tblSpouses.Rows.Count == iCountBeforeRead)
            {
                SelectAll(U.Marriage_Table, tblSpouses, new NameValuePair(U.PersonID_col, iSpouseID),
                                                        new NameValuePair(U.SpouseID_col, iPersonID));
            }
            return (tblSpouses.Rows.Count != iCountBeforeRead);
        }
        //****************************************************************************************************************************
        private static string MarriageSelectPersonCommand()
        {
            string whereString = WhereStringOrSameKey(U.PersonID_col, U.SpouseID_col);
            return SelectAllWhereString(U.Marriage_Table) + whereString;
        }
        //****************************************************************************************************************************
        public static bool GetAllCategories(DataTable tbl)
        {
            SelectAll(U.Category_Table, OrderBy(U.CategoryName_col), tbl);
            return true;
        }
        //****************************************************************************************************************************
        public static void GetMarriages(DataTable tblSpouses,
                                  string sKey,
                                  int iPersonID)
        {
            SelectAll(U.Marriage_Table, tblSpouses, new NameValuePair(sKey, iPersonID));
        }
        //****************************************************************************************************************************
        public static void GetPersonGridData(DataRow row,
                                      string sSortLastName,
                                      string sRealLastName,
                                      string sRealMarriedName,
                                      out string sSortName,
                                      out string sName,
                                      out string sBornDate,
                                      out string sBornPlace,
                                      out string sBornHome,
                                      out string sDiedDate,
                                      out string sDiedPlace,
                                      out string sDiedHome,
                                      out string sSpouse,
                                      out string sFather,
                                      out string sMother)
        {
            int iPersonID = row[U.PersonID_col].ToInt();
            sSpouse = GetSpouse(iPersonID);
            sFather = GetPersonLastNameFirst(row[U.FatherID_col].ToInt());
            sMother = GetPersonLastNameFirst(row[U.MotherID_col].ToInt());
            GetBornDiedDate(iPersonID, row, out sBornDate, out sBornPlace, out sBornHome, out sDiedDate, out sDiedPlace, out sDiedHome);
            sSortName = BuildSortName(sSortLastName.ToLower(),
                                      row[U.FirstName_col].ToString(),
                                      row[U.MiddleName_col].ToString(),
                                      sRealLastName, sRealMarriedName);
            sName = BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                           row[U.MiddleName_col].ToString(),
                                           row[U.LastName_col].ToString(),
                                           row[U.Suffix_col].ToString(),
                                           row[U.Prefix_col].ToString(),
                                           row[U.MarriedName_col].ToString(),
                                           row[U.KnownAs_col].ToString());
        }
        public static void GetBornDiedDate(int iPersonID,
                                          DataRow person_row,
                                      out string sBornDate,
                                      out string sBornPlace,
                                      out string sBornHome,
                                      out string sDiedDate,
                                      out string sDiedPlace,
                                      out string sDiedHome)
        {
            DataTable CemeteryRecord_tbl = DefineCemeteryRecordTable();
            GetCemeteryRecordForPerson(CemeteryRecord_tbl, iPersonID);
            DataTable VitalRecord_tbl = DefineVitalRecord_Table();
            GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col);
            sBornDate = person_row[U.BornDate_col].ToString();
            sBornPlace = person_row[U.BornPlace_col].ToString();
            sBornHome = person_row[U.BornHome_col].ToString();
            VitalRecord(VitalRecord_tbl, CemeteryRecord_tbl, person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale, ref sBornDate, ref sBornPlace, ref sBornHome);
            sDiedDate = person_row[U.DiedDate_col].ToString();
            sDiedPlace = person_row[U.DiedPlace_col].ToString();
            sDiedHome = person_row[U.DiedHome_col].ToString();
            VitalRecord(VitalRecord_tbl, CemeteryRecord_tbl, person_row, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale, ref sDiedDate, ref sDiedPlace, ref sDiedHome);
        }
        //****************************************************************************************************************************
        public static string GetSpouse(int iPersonID)
        {
            string sSpouse = "";
            int iNumberSpouses = 0;
            int iSpouseLocationInArray;
            DataTable Marriage_tbl = DefineMarriageTable();
            GetMarriagesID(Marriage_tbl, ref iNumberSpouses, out iSpouseLocationInArray, iPersonID);
            int iNum = 0;
            foreach (DataRow Marriage_row in Marriage_tbl.Rows)
            {
                iNum++;
                if (iNum > 1)
                    sSpouse += "-";
                int iSpouseID = Marriage_row[iSpouseLocationInArray].ToInt();
                sSpouse += GetPersonNameWithoutMarriedName(iSpouseID);
            }
            return sSpouse;
        }
        //****************************************************************************************************************************
        public static void VitalRecord(DataTable VitalRecord_tbl,
                                 DataTable CemeteryRecord_tbl, 
                                 DataRow Person_row,
                                 EVitalRecordType eVitalRecordType1,
                                 EVitalRecordType eVitalRecordType2,
                                 ref string sDate,
                                 ref string sPlace,
                                 ref string sHome)
        {
            string sBook = "";
            string sPage = "";
            string sSource = "";
            U.GetPersonVitalStatistics(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, eVitalRecordType1, eVitalRecordType2,
                                                    ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
        }
        //****************************************************************************************************************************
        public static int GetMarriagesID(DataTable tblSpouses,
                                  ref int iNumberSpouses,
                                  out int iSpouseLocationInArray,
                                      int iPersonID)
        {
            if (iNumberSpouses != 0)
                tblSpouses.Rows.Clear();
            GetMarriages(tblSpouses, U.PersonID_col, iPersonID);
            iSpouseLocationInArray = 1;
            if (tblSpouses.Rows.Count == 0)
            {
                GetMarriages(tblSpouses, "SpouseID", iPersonID);
                if (tblSpouses.Rows.Count != 0)
                    iSpouseLocationInArray = 0;
            }
            else
                AddAdditionalMarriages(tblSpouses, iPersonID);
            if (tblSpouses.Rows.Count == 0)
            {
                iNumberSpouses = 0;
                return 0;
            }
            else
            {
                iNumberSpouses = tblSpouses.Rows.Count;
                DataRow row = tblSpouses.Rows[0];
                return row[iSpouseLocationInArray].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetMarriageRecords()
        {
            DataTable tblSpouses = new DataTable();
            SelectAll(U.Marriage_Table, tblSpouses);
            return tblSpouses;
        }
        //****************************************************************************************************************************
        public static bool GetMarriageRecord(DataTable Marriage_tbl,
                                       int iSpouseID,
                                       int iPersonID)
        {
            int iNumMarriagesBeforeRead = Marriage_tbl.Rows.Count;
            if (iPersonID != 0 && iSpouseID != 0)
                GetMarriage(Marriage_tbl, iPersonID, iSpouseID);
            if (iNumMarriagesBeforeRead != Marriage_tbl.Rows.Count)
            {
                int iMarriageIndex = Marriage_tbl.Rows.Count - 1;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static DataRow GetMarriageRow(DataTable Marriage_tbl,
                                      int iPersonID,
                                      int iSpouseID)
        {
            string sSelect = U.PersonID_col + " = " + iPersonID.ToString() + " and " + U.SpouseID_col + " = " + iSpouseID.ToString();
            foreach (DataRow row in Marriage_tbl.Select(sSelect))
            {
                return row;
            }
            sSelect = U.PersonID_col + " = " + iSpouseID.ToString() + " and " + U.SpouseID_col + " = " + iPersonID.ToString();
            foreach (DataRow row in Marriage_tbl.Select(sSelect))
            {
                return row;
            }
            return null;
            //        DataRow[] rows = m_PersonDS.Tables[MarriageTable].Select("", "", dvrs);
        }
        //****************************************************************************************************************************
    }
}
