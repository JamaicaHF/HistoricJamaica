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
        public static DataSet DefinePerson()
        {
            DataSet Person_ds = new DataSet();
            Person_ds.Tables.Add(DefinePersonTable());
            DataTable VitalRecord_tbl = DefineVitalRecord_Table();
            Person_ds.Tables.Add(VitalRecord_tbl);
            DataTable Marriage_tbl = DefineMarriageTable();
            Person_ds.Tables.Add(Marriage_tbl);
            DataTable CV_tbl = DefineCategoryValueTable(U.PersonCategoryValue_Table);
            CV_tbl.PrimaryKey = new DataColumn[] { CV_tbl.Columns[U.CategoryValueID_col] };
            Person_ds.Tables.Add(CV_tbl);
            DataTable PV_tbl = DefineBuildingOccupantTableForPerson();
            Person_ds.Tables.Add(PV_tbl);
            return Person_ds;
        }
        //****************************************************************************************************************************
        public static DataTable DefinePersonCWTable()
        {
            DataTable tbl = new DataTable(U.Person_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PersonCWID_col, typeof(int));
            tbl.Columns.Add(U.FirstName_col, typeof(string));
            tbl.Columns.Add(U.MiddleName_col, typeof(string));
            tbl.Columns.Add(U.LastName_col, typeof(string));
            tbl.Columns.Add(U.BornDate_col, typeof(string));
            tbl.Columns.Add(U.DiedDate_col, typeof(string));
            tbl.Columns.Add(U.EnlistmentDate_col, typeof(string));
            tbl.Columns.Add(U.CemeteryName_col, typeof(string));
            tbl.Columns.Add(U.BattleSiteKilled_col, typeof(string));
            tbl.Columns.Add(U.DataMilitary_col, typeof(string));
            tbl.Columns.Add(U.Reference_col, typeof(string));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            SetPersonCWVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetPersonCWVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.FirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.LastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.BornDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.DiedDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.EnlistmentDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.CemeteryName_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.BattleSiteKilled_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.DataMilitary_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.Reference_col].MaxLength = U.iMaxArticleLength;
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
        }
        //****************************************************************************************************************************
        public static DataTable DefinePersonTable()
        {
            DataTable tbl = new DataTable(U.Person_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.FirstName_col, typeof(string));
            tbl.Columns.Add(U.MiddleName_col, typeof(string));
            tbl.Columns.Add(U.LastName_col, typeof(string));
            tbl.Columns.Add(U.Suffix_col, typeof(string));
            tbl.Columns.Add(U.Prefix_col, typeof(string));
            tbl.Columns.Add(U.MarriedName_col, typeof(string));
            tbl.Columns.Add(U.MarriedName2_col, typeof(string));
            tbl.Columns.Add(U.MarriedName3_col, typeof(string));
            tbl.Columns.Add(U.KnownAs_col, typeof(string));
            tbl.Columns.Add(U.FatherID_col, typeof(int));
            tbl.Columns.Add(U.MotherID_col, typeof(int));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            tbl.Columns.Add(U.Source_col, typeof(string));
            tbl.Columns.Add(U.Sex_col, typeof(Char));
            tbl.Columns.Add(U.BornDate_col, typeof(string));
            tbl.Columns.Add(U.BornPlace_col, typeof(string));
            tbl.Columns.Add(U.BornHome_col, typeof(string));
            tbl.Columns.Add(U.BornVerified_col, typeof(Char));
            tbl.Columns.Add(U.BornSource_col, typeof(string));
            tbl.Columns.Add(U.BornBook_col, typeof(string));
            tbl.Columns.Add(U.BornPage_col, typeof(string));
            tbl.Columns.Add(U.DiedDate_col, typeof(string));
            tbl.Columns.Add(U.DiedPlace_col, typeof(string));
            tbl.Columns.Add(U.DiedHome_col, typeof(string));
            tbl.Columns.Add(U.DiedVerified_col, typeof(Char));
            tbl.Columns.Add(U.DiedSource_col, typeof(string));
            tbl.Columns.Add(U.DiedBook_col, typeof(string));
            tbl.Columns.Add(U.DiedPage_col, typeof(string));
            tbl.Columns.Add(U.BuriedDate_col, typeof(string));
            tbl.Columns.Add(U.BuriedPlace_col, typeof(string));
            tbl.Columns.Add(U.BuriedStone_col, typeof(string));
            tbl.Columns.Add(U.BuriedVerified_col, typeof(Char));
            tbl.Columns.Add(U.BuriedSource_col, typeof(string));
            tbl.Columns.Add(U.BuriedBook_col, typeof(string));
            tbl.Columns.Add(U.BuriedPage_col, typeof(string));
            tbl.Columns.Add(U.ImportPersonID_col, typeof(int));
            tbl.Columns.Add(U.Census1790_col, typeof(int));
            tbl.Columns.Add(U.Census1800_col, typeof(int));
            tbl.Columns.Add(U.Census1810_col, typeof(int));
            tbl.Columns.Add(U.Census1820_col, typeof(int));
            tbl.Columns.Add(U.Census1830_col, typeof(int));
            tbl.Columns.Add(U.Census1840_col, typeof(int));
            tbl.Columns.Add(U.Census1850_col, typeof(int));
            tbl.Columns.Add(U.Census1860_col, typeof(int));
            tbl.Columns.Add(U.Census1870_col, typeof(int));
            tbl.Columns.Add(U.Census1880_col, typeof(int));
            tbl.Columns.Add(U.Census1890_col, typeof(int));
            tbl.Columns.Add(U.Census1900_col, typeof(int));
            tbl.Columns.Add(U.Census1910_col, typeof(int));
            tbl.Columns.Add(U.Census1920_col, typeof(int));
            tbl.Columns.Add(U.Census1930_col, typeof(int));
            tbl.Columns.Add(U.Census1940_col, typeof(int));
            tbl.Columns.Add(U.Census1950_col, typeof(int));
            tbl.Columns.Add(U.ExcludeFromSite_col, typeof(int));
            tbl.Columns.Add(U.GazetteerRoad_col, typeof(int));
            tbl.Columns.Add(U.Beers1869District_col, typeof(int));
            tbl.Columns.Add(U.McClellan1856District_col, typeof(int));
            SetPersonVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static int PersonIDInDataset(DataSet Person_ds)
        {
            return Person_ds.Tables[U.Person_Table].Rows[0][U.PersonID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static void SetPersonVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.FirstName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MiddleName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.LastName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.Suffix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.Prefix_col].MaxLength = U.iMaxPrefixSuffixLength;
            tbl.Columns[U.MarriedName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MarriedName2_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MarriedName3_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.KnownAs_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.Source_col].MaxLength = U.iMaxPlaceLength;
            tbl.Columns[U.BornDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.BornPlace_col].MaxLength = U.iMaxPlaceLength;
            tbl.Columns[U.BornHome_col].MaxLength = U.iMaxPlaceLength;
            tbl.Columns[U.BornSource_col].MaxLength = U.iMaxPlaceLength;
            tbl.Columns[U.BornBook_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.BornPage_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.DiedDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.DiedPlace_col].MaxLength = U.iMaxPlaceLength;;
            tbl.Columns[U.DiedHome_col].MaxLength = U.iMaxPlaceLength;;
            tbl.Columns[U.DiedSource_col].MaxLength = U.iMaxPlaceLength;;
            tbl.Columns[U.DiedBook_col].MaxLength = U.iMaxBookPageLength;;
            tbl.Columns[U.DiedPage_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.BuriedDate_col].MaxLength = U.iMaxDateLength;;
            tbl.Columns[U.BuriedPlace_col].MaxLength = U.iMaxPlaceLength;
            tbl.Columns[U.BuriedStone_col].MaxLength = U.iMaxPlaceLength;
            tbl.Columns[U.BuriedSource_col].MaxLength = U.iMaxPlaceLength;
            tbl.Columns[U.BuriedBook_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.BuriedPage_col].MaxLength = U.iMaxBookPageLength;
        }
        //****************************************************************************************************************************
        private static bool UpdateFatherMother(SqlTransaction txn,
                                              DataTable tbl)
        {
            return UpdateWithDA(txn, tbl, U.Person_Table, U.PersonID_col, ColumnList(U.FatherID_col, U.MotherID_col));
        }
        //****************************************************************************************************************************
        private static bool DeleteBuildingCategoryValues(SqlTransaction txn,
                                                         int iPersonID,
                                                         DataTable CategoryValue_tbl,
                                                         DataTable BuildingValue_tbl)
        {
            return DeleteInsertCategoryValueDataRows(txn, CategoryValue_tbl, U.PersonCategoryValue_Table, U.PersonID_col, iPersonID) &&
                   DeleteInsertBuildingOccupantDataRows(txn, BuildingValue_tbl, iPersonID);
        }
        //****************************************************************************************************************************
        public static string BuildSortName(string sSortLastName,
                                    string sFirstName,
                                    string sMiddleName,
                                    string sLastName,
                                    string sMarriedName)
        {
            string sPersonName = "";
            int iLengthOfSortName = sSortLastName.Length;
            if (sSortLastName.Length != 0)
            {
                if (sMarriedName.Length < iLengthOfSortName)
                {
                    sPersonName = sLastName;
                }
                else
                {
                    string sCompareName = sMarriedName.Substring(0, iLengthOfSortName).ToLower();
                    if (sCompareName == sSortLastName)
                        sPersonName = sMarriedName;
                    else
                        sPersonName = sLastName;
                }
            }
            else
                if (sMarriedName.Length != 0)
                    sPersonName += sMarriedName;
                else
                    sPersonName += sLastName;
            sPersonName = (sPersonName + " " + sFirstName + " " + sMiddleName);
            return sPersonName;
        }
        //****************************************************************************************************************************
        public static string BuildNameStringWithoutMarriedName(string sFirstName,
                                       string sMiddleName,
                                       string sLastName,
                                       string sSuffix,
                                       string sPrefix,
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
            if (sLastName.Length != 0)
            {
                sPersonName += sLastName + " ";
                if (sSuffix.Length != 0)
                    sPersonName += sSuffix;
            }
            return sPersonName.Trim();
        }
        //****************************************************************************************************************************
        public static string BuildNameLastNameFirst(PersonName personName)
        {
            return BuildNameLastNameFirst(personName.firstName, personName.middleName,
                                                personName.lastName, personName.suffix,
                                                personName.prefix, "", "");
        }
        //****************************************************************************************************************************
        public static string BuildNameLastNameFirst(DataRow personRow)
        {
            return BuildNameLastNameFirst(personRow[U.FirstName_col].ToString(), personRow[U.MiddleName_col].ToString(),
                                                personRow[U.LastName_col].ToString(), personRow[U.Suffix_col].ToString(),
                                                personRow[U.Prefix_col].ToString(), "", "");
        }
        //****************************************************************************************************************************
        public static string BuildNameLastNameFirstNoMiddleName(DataRow personRow)
        {
            return BuildNameLastNameFirst(personRow[U.FirstName_col].ToString(), "",
                                                personRow[U.LastName_col].ToString(), personRow[U.Suffix_col].ToString(),
                                                personRow[U.Prefix_col].ToString(), "", "");
        }
        //****************************************************************************************************************************
        public static string BuildNameLastNameFirst(string sFirstName,
                                             string sMiddleName,
                                             string sLastName,
                                             string sSuffix,
                                             string sPrefix = "",
                                             string sMarriedName = "",
                                             string sKnownAs = "")
        {
            string sPersonName = "";
            if (sMarriedName.Length != 0)
                sPersonName += sMarriedName + ", ";
            else
                if (sLastName.Length != 0)
                {
                    sPersonName += sLastName;
                    if (sSuffix.Length != 0)
                        sPersonName += " " + sSuffix;
                    sPersonName += ", ";
                }
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
            if (sMarriedName.Length != 0 && sLastName.Length != 0) // both married and maiden name
                sPersonName += "(" + sLastName + ") ";
            return sPersonName.Trim();
        }
        //****************************************************************************************************************************
        public static string PersonHomeName(int iPersonID,
                                     int iBuildingID,
                                     int iSpouseLivedWithID,
                                     bool bShowRoad)
        {
            string sFamilyHome = "";
            DataTable PersonTable = DefinePersonTable();
            GetPerson(PersonTable, iPersonID);
            if (PersonTable.Rows.Count != 0)
            {
                DataRow Person_row = PersonTable.Rows[0];
                sFamilyHome = FamilyName(Person_row);
                if (iSpouseLivedWithID != 0)
                {
                    DataTable spouseTable = DefinePersonTable();
                    if (GetPerson(spouseTable, iSpouseLivedWithID) && spouseTable.Rows.Count != 0)
                    {
                        DataRow spouse_row = spouseTable.Rows[0];
                        sFamilyHome += " and " + SpouseName(Person_row, spouse_row);
                    }
                }
                DataTable Building_tbl = GetBuilding(iBuildingID);
                if (Building_tbl.Rows.Count == 0)
                {
                    DeleteBuildingOccupants(iPersonID, iBuildingID);
                    sFamilyHome = "";
                }
                else
                {
                    DataRow row = Building_tbl.Rows[0];
                    if (bShowRoad)
                    {
                        sFamilyHome = AddRoadToGroup(sFamilyHome, row[U.BuildingRoadValueID_col].ToInt(),
                                        row[U.BuildingGrandListID_col].ToInt());
                    }
                }
            }
            return sFamilyHome;
        }
        //****************************************************************************************************************************
        private static string SpouseName(DataRow personRow,
                                         DataRow spouseRow)
        {
            if (spouseRow == null)
            {
                return "";
            }
            string spouseName = spouseRow[U.FirstName_col].ToString() + " " + spouseRow[U.MiddleName_col].ToString().Trim();
            string marriedName = spouseRow[U.MarriedName_col].ToString();
            string spouseLastName = String.IsNullOrEmpty(spouseRow[U.MarriedName_col].ToString()) ? spouseRow[U.LastName_col].ToString() : spouseRow[U.MarriedName_col].ToString();
            string personLastName = String.IsNullOrEmpty(personRow[U.MarriedName_col].ToString()) ? personRow[U.LastName_col].ToString() : personRow[U.MarriedName_col].ToString();
            if (String.IsNullOrEmpty(marriedName))
            {
                marriedName = spouseRow[U.LastName_col].ToString();
            }
            if (marriedName == personLastName)
            {
                return spouseName;
            }
            return marriedName + ", " + spouseName;
        }
        //****************************************************************************************************************************
        private static string FamilyName(DataRow Person_row)
        {
            string sMarriedName = Person_row[U.MarriedName_col].ToString();
            if (sMarriedName.Length == 0)
            {
                sMarriedName = Person_row[U.LastName_col].ToString();
                if (Person_row[U.Suffix_col].ToString().Length != 0)
                    sMarriedName = sMarriedName + " " + Person_row[U.Suffix_col].ToString();
            }
            sMarriedName = sMarriedName + ", ";
            if (Person_row[U.Prefix_col].ToString().Length != 0)
                sMarriedName = sMarriedName + Person_row[U.Prefix_col].ToString() + " ";
            string sFirstAndMiddle = Person_row[U.FirstName_col].ToString();
            if (Person_row[U.MiddleName_col].ToString().Length != 0)
            {
                sFirstAndMiddle += " " + Person_row[U.MiddleName_col].ToString();
            }
            return sMarriedName + sFirstAndMiddle;
        }
        //****************************************************************************************************************************
        public static string PersonHomeName(int iPersonID,
                                     int iBuildingID,
                                     out int iSpouseLivedWithID,
                                     bool bShowRoad)
        {
            DataTable PersonTable = DefinePersonTable();
            DataTable Marriages_tbl = new DataTable();
            int iNumberSpouses = 0;
            int iSpouseLocationInArray;
            iSpouseLivedWithID = GetMarriagesID(Marriages_tbl, ref iNumberSpouses,
                                           out iSpouseLocationInArray, iPersonID);
            return PersonHomeName(iPersonID, iBuildingID, iSpouseLivedWithID, bShowRoad);
        }
        //****************************************************************************************************************************
    }
}
