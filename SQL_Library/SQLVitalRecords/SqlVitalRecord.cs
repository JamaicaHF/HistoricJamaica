using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    //****************************************************************************************************************************
    public class VitalRecordProperties
    {
        public string Book;
        public string Page;
        public int DateYear;
        public int DateMonth;
        public int DateDay;
        public char Disposition;
        public string CemeteryName;
        public string LotNumber;
        public string Notes;
        public bool ExcludeFromSite;
        public int AgeYears;
        public int AgeMonths;
        public int AgeDays;
        public char Sex;
        public int SpouseAgeYears;
        public int SpouseAgeMonths;
        public int SpouseAgeDays;
        public char SpouseSex;
        //****************************************************************************************************************************
        public VitalRecordProperties(string book,
                                     string page,
                                     int dateYear,
                                     int dateMonth,
                                     int dateDay,
                                     char disposition,
                                     string cemeteryName,
                                     string lotNumber,
                                     string notes,
                                     int ageYears,
                                     int ageMonths,
                                     int ageDays,
                                     char sex,
                                     int spouseAgeYears,
                                     int spouseAgeMonths,
                                     int spouseAgeDays,
                                     char spouseSex,
                                     bool excludeFromSite)
        {
            this.Book = book;
            this.Page = page;
            this.DateYear = dateYear;
            this.DateMonth = dateMonth;
            this.DateDay = dateDay;
            this.Disposition = disposition;
            this.CemeteryName = cemeteryName;
            this.LotNumber = lotNumber;
            this.Notes = notes;
            this.AgeYears = ageYears;
            this.AgeMonths = ageMonths;
            this.AgeDays = ageDays;
            this.Sex = sex;
            this.SpouseAgeYears = spouseAgeYears;
            this.SpouseAgeMonths = spouseAgeMonths;
            this.SpouseAgeDays = spouseAgeDays;
            this.SpouseSex = spouseSex;
            this.ExcludeFromSite = excludeFromSite;
        }
        //****************************************************************************************************************************
        public void ChangeSexToSpouseSex()
        {
            if (Sex != 'F')
                Sex = 'F';
            else
                Sex = 'M';
        }
        //****************************************************************************************************************************
        public void ChangeDateToAge(DataRow row,
                                    int bornMonth,
                                    int bornDay,
                                    int bornYear,
                                    bool forSpouse)
        {
            if (bornMonth == 0 && bornDay == 0 && bornYear == 0)
            {
                return;
            }
            if (forSpouse)
            {
                if (!AgeChanged(row, SpouseAgeYears, SpouseAgeMonths, SpouseAgeDays))
                {
                    U.AgeBetweenTwoDates(bornYear, bornMonth, bornDay,
                                         DateYear, DateMonth, DateDay,
                                         out SpouseAgeYears, out SpouseAgeMonths, out SpouseAgeDays);
                }
            }
            else if (!AgeChanged(row, AgeYears, AgeMonths, AgeDays))
            {
                U.AgeBetweenTwoDates(bornYear, bornMonth, bornDay,
                                     DateYear, DateMonth, DateDay,
                                     out AgeYears, out AgeMonths, out AgeDays);
            }
        }
        //****************************************************************************************************************************
        private bool AgeChanged(DataRow row, int ageYears, int ageMonths, int ageDays)
        {
            if (row == null)
            {
                return !(ageYears == 0 && ageMonths == 0 && ageDays == 0);
            }
            if (row[U.AgeYears_col].ToInt() != ageYears)
            {
                return true;
            }
            if (row[U.AgeMonths_col].ToInt() != ageMonths)
            {
                return true;
            }
            if (row[U.AgeDays_col].ToInt() != ageDays)
            {
                return true;
            }
            return false;
        }
    }
    //****************************************************************************************************************************
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static DataTable DefineVitalRecord_Table()
        {
            DataTable tbl = new DataTable(U.VitalRecord_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.VitalRecordID_col, typeof(int));
            tbl.Columns.Add(U.VitalRecordType_col, typeof(int));
            tbl.Columns.Add(U.FirstName_col, typeof(string));
            tbl.Columns.Add(U.MiddleName_col, typeof(string));
            tbl.Columns.Add(U.LastName_col, typeof(string));
            tbl.Columns.Add(U.Suffix_col, typeof(string));
            tbl.Columns.Add(U.Prefix_col, typeof(string));
            tbl.Columns.Add(U.FatherFirstName_col, typeof(string));
            tbl.Columns.Add(U.FatherMiddleName_col, typeof(string));
            tbl.Columns.Add(U.FatherLastName_col, typeof(string));
            tbl.Columns.Add(U.FatherSuffix_col, typeof(string));
            tbl.Columns.Add(U.FatherPrefix_col, typeof(string));
            tbl.Columns.Add(U.MotherFirstName_col, typeof(string));
            tbl.Columns.Add(U.MotherMiddleName_col, typeof(string));
            tbl.Columns.Add(U.MotherLastName_col, typeof(string));
            tbl.Columns.Add(U.MotherSuffix_col, typeof(string));
            tbl.Columns.Add(U.MotherPrefix_col, typeof(string));
            tbl.Columns.Add(U.SpouseID_col, typeof(int));
            tbl.Columns.Add(U.Book_col, typeof(string));
            tbl.Columns.Add(U.Page_col, typeof(string));
            tbl.Columns.Add(U.DateYear_col, typeof(int));
            tbl.Columns.Add(U.DateMonth_col, typeof(int));
            tbl.Columns.Add(U.DateDay_col, typeof(int));
            tbl.Columns.Add(U.AgeYears_col, typeof(int));
            tbl.Columns.Add(U.AgeMonths_col, typeof(int));
            tbl.Columns.Add(U.AgeDays_col, typeof(int));
            tbl.Columns.Add(U.Disposition_col, typeof(char));
            tbl.Columns.Add(U.CemeteryName_col, typeof(string));
            tbl.Columns.Add(U.LotNumber_col, typeof(string));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.FatherID_col, typeof(int));
            tbl.Columns.Add(U.MotherID_col, typeof(int));
            tbl.Columns.Add(U.Sex_col, typeof(char));
            tbl.Columns.Add(U.ExcludeFromSite_col, typeof(int));
            SetVitalRecordsVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetVitalRecordsVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.FirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.LastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.Suffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.Prefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.FatherFirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.FatherMiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.FatherLastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.FatherSuffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.FatherPrefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.MotherFirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MotherMiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MotherLastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MotherSuffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.MotherPrefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.Book_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.Page_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.CemeteryName_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.LotNumber_col].MaxLength = U.iMaxStoneLength;
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
        }
        //****************************************************************************************************************************
        public static void CheckVitalRecordParents()
        {
            DataTable vitalRecordTbl = GetAllVitalRecords();
            foreach (DataRow vitalRecordRow in vitalRecordTbl.Rows)
            {
                int personId = vitalRecordRow[U.PersonID_col].ToInt();
                if (personId != 0)
                {
                    int vitalRecordId = vitalRecordRow[U.VitalRecordID_col].ToInt();
                    DataRow personRow = GetPerson(personId);
                    CheckParentForConsistency(vitalRecordId, vitalRecordRow[U.FatherID_col].ToInt(), personRow[U.FatherID_col].ToInt());
                    CheckParentForConsistency(vitalRecordId, vitalRecordRow[U.MotherID_col].ToInt(), personRow[U.MotherID_col].ToInt());
                }
            }
        }
        //****************************************************************************************************************************
        private static void CheckParentForConsistency(int vitalRecordId, int vitalRecordParentId, int personParentId)
        {
            if (vitalRecordParentId == 0)
            {
                return;
            }
            if (personParentId == 0)
            {
            }
            else if (vitalRecordParentId != personParentId)
            {
                // vitalRecordIdID 7019 results from an adoption where Hattie Felton was adopted by the Feltons but birth parents are Fessenden
            }
        }
        //****************************************************************************************************************************
    }
}
