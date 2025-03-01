using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static DataTable DefineCemeteryTable()
        {
            DataTable tbl = new DataTable(U.Cemetery_Table);
            tbl.Columns.Add(U.CemeteryID_col, typeof(int));
            tbl.Columns.Add(U.CemeteryName_col, typeof(string));
            tbl.Columns[U.CemeteryName_col].MaxLength = U.iMaxValueLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefineCemeteryRecordTable()
        {
            DataTable tbl = new DataTable(U.CemeteryRecord_Table);
            tbl.Columns.Add(U.CemeteryRecordID_col, typeof(int));
            tbl.Columns.Add(U.CemeteryID_col, typeof(int));
            tbl.Columns.Add(U.NameOnGrave_col, typeof(string));
            tbl.Columns.Add(U.FirstName_col, typeof(string));
            tbl.Columns.Add(U.MiddleName_col, typeof(string));
            tbl.Columns.Add(U.LastName_col, typeof(string));
            tbl.Columns.Add(U.Suffix_col, typeof(string));
            tbl.Columns.Add(U.Prefix_col, typeof(string));
            tbl.Columns.Add(U.SpouseNameOnGrave_col, typeof(string));
            tbl.Columns.Add(U.SpouseFirstName_col, typeof(string));
            tbl.Columns.Add(U.SpouseMiddleName_col, typeof(string));
            tbl.Columns.Add(U.SpouseLastName_col, typeof(string));
            tbl.Columns.Add(U.SpouseSuffix_col, typeof(string));
            tbl.Columns.Add(U.SpousePrefix_col, typeof(string));
            tbl.Columns.Add(U.FatherNameOnGrave_col, typeof(string));
            tbl.Columns.Add(U.FatherFirstName_col, typeof(string));
            tbl.Columns.Add(U.FatherMiddleName_col, typeof(string));
            tbl.Columns.Add(U.FatherLastName_col, typeof(string));
            tbl.Columns.Add(U.FatherSuffix_col, typeof(string));
            tbl.Columns.Add(U.FatherPrefix_col, typeof(string));
            tbl.Columns.Add(U.MotherNameOnGrave_col, typeof(string));
            tbl.Columns.Add(U.MotherFirstName_col, typeof(string));
            tbl.Columns.Add(U.MotherMiddleName_col, typeof(string));
            tbl.Columns.Add(U.MotherLastName_col, typeof(string));
            tbl.Columns.Add(U.MotherSuffix_col, typeof(string));
            tbl.Columns.Add(U.MotherPrefix_col, typeof(string));
            tbl.Columns.Add(U.BornDate_col, typeof(string));
            tbl.Columns.Add(U.DiedDate_col, typeof(string));
            tbl.Columns.Add(U.AgeYears_col, typeof(int));
            tbl.Columns.Add(U.AgeMonths_col, typeof(int));
            tbl.Columns.Add(U.AgeDays_col, typeof(int));
            tbl.Columns.Add(U.LotNumber_col, typeof(string));
            tbl.Columns.Add(U.Disposition_col, typeof(char));
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.SpouseID_col, typeof(int));
            tbl.Columns.Add(U.FatherID_col, typeof(int));
            tbl.Columns.Add(U.MotherID_col, typeof(int));
            tbl.Columns.Add(U.Sex_col, typeof(char));
            tbl.Columns.Add(U.Epitaph_col, typeof(string));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            SetCemeteryRecordVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetCemeteryRecordVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.NameOnGrave_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.FirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.LastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.Suffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.Prefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.SpouseNameOnGrave_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.SpouseFirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.SpouseMiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.SpouseLastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.SpouseSuffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.SpousePrefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.FatherNameOnGrave_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.FatherFirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.FatherMiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.FatherLastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.FatherSuffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.FatherPrefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.MotherNameOnGrave_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.MotherFirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MotherMiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MotherLastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MotherSuffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.MotherPrefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.BornDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.DiedDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.Epitaph_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.LotNumber_col].MaxLength = U.iMaxStoneLength;
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
        }
        //****************************************************************************************************************************
    }
}
