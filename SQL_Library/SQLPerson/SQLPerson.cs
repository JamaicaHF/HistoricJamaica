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
        public static void InitializePersonTable(DataRow row)
        {
            row[U.PersonID_col] = 0;
            row[U.FirstName_col] = "";
            row[U.MiddleName_col] = "";
            row[U.LastName_col] = "";
            row[U.Suffix_col] = "";
            row[U.Prefix_col] = "";
            row[U.MarriedName_col] = "";
            row[U.MarriedName2_col] = "";
            row[U.MarriedName3_col] = "";
            row[U.KnownAs_col] = "";
            row[U.FatherID_col] = 0;
            row[U.MotherID_col] = 0;
            row[U.Notes_col] = "";
            row[U.GazetteerRoad_col] = 0;
            row[U.Beers1869District_col] = 0;
            row[U.McClellan1856District_col] = 0;
            row[U.Source_col] = "";
            row[U.Sex_col] = ' ';
            row[U.BornDate_col] = "";
            row[U.BornPlace_col] = "";
            row[U.BornHome_col] = "";
            row[U.BornVerified_col] = ' ';
            row[U.BornSource_col] = "";
            row[U.BornBook_col] = "";
            row[U.BornPage_col] = "";
            row[U.DiedDate_col] = "";
            row[U.DiedPlace_col] = "";
            row[U.DiedHome_col] = "";
            row[U.DiedVerified_col] = ' ';
            row[U.DiedSource_col] = "";
            row[U.DiedBook_col] = "";
            row[U.DiedPage_col] = "";
            row[U.BuriedDate_col] = "";
            row[U.BuriedPlace_col] = "";
            row[U.BuriedStone_col] = "";
            row[U.BuriedVerified_col] = ' ';
            row[U.BuriedSource_col] = "";
            row[U.BuriedBook_col] = "";
            row[U.BuriedPage_col] = "";
            row[U.ImportPersonID_col] = 0;
            row[U.Census1790_col] = 0;
            row[U.Census1800_col] = 0;
            row[U.Census1810_col] = 0;
            row[U.Census1820_col] = 0;
            row[U.Census1830_col] = 0;
            row[U.Census1840_col] = 0;
            row[U.Census1850_col] = 0;
            row[U.Census1860_col] = 0;
            row[U.Census1870_col] = 0;
            row[U.Census1880_col] = 0;
            row[U.Census1890_col] = 0;
            row[U.Census1900_col] = 0;
            row[U.Census1910_col] = 0;
            row[U.Census1920_col] = 0;
            row[U.Census1930_col] = 0;
            row[U.Census1940_col] = 0;
            row[U.Census1950_col] = 0;
            row[U.ExcludeFromSite_col] = 0;
        }
        //****************************************************************************************************************************
        public static DataTable DefinePersonImportIDTable()
        {
            DataTable tbl = new DataTable(U.Person_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.ImportPersonID_col, typeof(int));
            tbl.Columns.Add(U.FatherID_col, typeof(int));
            tbl.Columns.Add(U.MotherID_col, typeof(int));
            tbl.Columns.Add(U.MainFatherID_col, typeof(int));
            tbl.Columns.Add(U.MainMotherID_col, typeof(int));
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.ImportPersonID_col] };
            return tbl;
        }
        //****************************************************************************************************************************
        public static string BuildNameString(string sFirstName,
                                       string sMiddleName,
                                       string sLastName,
                                       string sSuffix,
                                       string sPrefix,
                                       string sMarriedName,
                                       string sKnownAs)
        {
            string sPersonName = "";
            if (sPrefix.Length != 0)
                sPersonName += sPrefix + " ";
            if (sFirstName.Length != 0)
                sPersonName += sFirstName + " ";
            if (sKnownAs.Length != 0)
            {
                sPersonName += "[" + sKnownAs + "] ";
            }
            if (sMiddleName.Length != 0)
                sPersonName += sMiddleName + " ";
            if (sMarriedName.Length != 0)
            {
                if (sLastName.Length != 0)
                    sPersonName += "(" + sLastName + ") ";
                sPersonName += sMarriedName + " ";
            }
            else
            if (sLastName.Length != 0)
                sPersonName += sLastName + " ";
            if (sSuffix.Length != 0)
                sPersonName += sSuffix;
            return sPersonName.Trim();
        }
        //****************************************************************************************************************************
        public static void MoveMergeInfoToPerson(ArrayList FieldsModified,
                               int iPersonID,
                               DataTable person_tbl,
                               DataRow MergePersonRow)
        {
            DataRow person_row = person_tbl.Rows[0];
            DataColumnCollection columns = person_tbl.Columns;
            int numColumns = columns.Count;
            for (int i = 1; i < numColumns; i++) // skip the key
            {
                SetToNewValue(FieldsModified, columns, person_row, MergePersonRow, columns[i].ToString());
            }
        }
        //****************************************************************************************************************************
        private static void SetToNewValue(ArrayList FieldsModified,
                                          DataColumnCollection columns,
                                          DataRow PersonRow,
                                          DataRow MergePersonRow,
                                          string value_col)
        {
            object value = MergePersonRow[value_col];
            int i = columns[value_col].MaxLength;
            if (value.GetType().ToString() == "System.String")
            {
                string valueStr = MoveMergeStringToPerson(PersonRow[value_col].ToString(), MergePersonRow[value_col].ToString());
                U.SetToNewValueIfDifferent(FieldsModified, columns, PersonRow, value_col, valueStr);
            }
            else if (value.GetType().ToString() == "System.Int32")
            {
                int valueInt = MoveMergeIntToPerson(PersonRow[value_col].ToInt(), MergePersonRow[value_col].ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, PersonRow, value_col, valueInt);
            }
            else
            {
                string valueStr = MoveMergeStringToPerson(PersonRow[value_col].ToString(), MergePersonRow[value_col].ToString());
                U.SetToNewValueIfDifferent(FieldsModified, PersonRow, value_col, valueStr[0]);
            }
        }
        //****************************************************************************************************************************
        public static Int64 MoveMergeCensusToPerson(Int64 iPersonCensus,
                                                  Int64 iMergeCensus)
        {
            if (iPersonCensus == 0)
                return iMergeCensus;
            Census census = new Census(iPersonCensus);
            census.MergeCensusYears(iMergeCensus);
            return census.GetCencusYears();
        }
        //****************************************************************************************************************************
        public static int MoveMergeIntToPerson(int iPersonInt,
                                            int iMergeInt)
        {
            if (iPersonInt == 0)
                return iMergeInt;
            else
                return iPersonInt;
        }
        //****************************************************************************************************************************
        public static string MoveMergeStringToPerson(string sPersonString,
                                               string sMergeString)
        {
            if (sPersonString.Length == 0)
                return sMergeString;
            else
                if (sPersonString.Length == 1 && sMergeString.Length > 1) // generally replaces an initial with a name
                    return sMergeString;
                else
                    return sPersonString;
        }
        //****************************************************************************************************************************
    }
}
