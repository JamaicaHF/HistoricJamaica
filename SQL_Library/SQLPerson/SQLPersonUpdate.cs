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
        public static void UpdatePersonTableForField(DataTable personTable,
                                                     params string[] field_cols)
        {
            ArrayList fieldValues = SQL.ColumnList(field_cols);
            SQL.UpdateWithDA(personTable, U.Person_Table, U.PersonID_col, fieldValues);
        }
        //****************************************************************************************************************************
        public static void UpdatePersonTableForField(int personID,
                                                     params NameValuePair[] fields)
        {

            UpdateWithParms(U.Person_Table, new NameValuePair(U.PersonID_col, personID), fields);
        }
        //****************************************************************************************************************************
        public static void RemoveLastNameWhenSameSaMarriedName()
        {
            int count = 0;
            DataTable personTbl = GetAllPersons();
            foreach (DataRow personRow in personTbl.Rows)
            {
                if (personRow[U.LastName_col].ToString() == personRow[U.MarriedName_col].ToString())
                {
                    personRow[U.LastName_col] = "";
                    count++;
                }
            }
            UpdateWithDA(personTbl, U.Person_Table, U.PersonID_col, ColumnList(U.LastName_col));
        }
        //****************************************************************************************************************************
        public static void CheckSchoolRecordsBirthDate()
        {
            DataTable AlternativeSpellingsFirstNameTbl = SQL.GetAllAlternativeSpellings(U.AlternativeSpellingsFirstName_Table);
            DataTable AlternativeSpellingsLastNameTbl = SQL.GetAllAlternativeSpellings(U.AlternativeSpellingsLastName_Table);
            DataTable personTbl = GetAllPersons();
            int count = 0;
            foreach (DataRow personRow in personTbl.Rows)
            {
                if (String.IsNullOrEmpty(personRow[U.BornDate_col].ToString()))
                {
                    int personId = personRow[U.PersonID_col].ToInt();
                    DataTable schoolRecordsTbl = GetSchoolRecordsFromPersonId(personId);
                    foreach (DataRow schoolRecordsRow in schoolRecordsTbl.Rows)
                    {
                        DataTable schoolRecordsForPersonIdTbl = GetBornDateForPersonFromSchoolRecords(AlternativeSpellingsFirstNameTbl,
                                                                           AlternativeSpellingsLastNameTbl,
                                                                           personRow,
                                                                           schoolRecordsRow,
                                                                           personId, "");
                        count++;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        public static void CheckCensusBirthDate()
        {
            DataTable personTbl = GetAllPersons();
            int count = 0;
            foreach (DataRow personRow in personTbl.Rows)
            {
                bool found = false;
                int latestYear = 0;
                CheckCensusYear(1850, personRow[U.Census1850_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1860, personRow[U.Census1860_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1870, personRow[U.Census1870_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1880, personRow[U.Census1880_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1900, personRow[U.Census1900_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1910, personRow[U.Census1910_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1920, personRow[U.Census1920_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1930, personRow[U.Census1930_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1940, personRow[U.Census1940_col].ToInt(), ref found, ref latestYear);
                CheckCensusYear(1950, personRow[U.Census1950_col].ToInt(), ref found, ref latestYear);
                if (found && String.IsNullOrEmpty(personRow[U.BornDate_col].ToString()))
                {
                    int personId = personRow[U.PersonID_col].ToInt();
                    DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
                    SQL.GetVitalRecordsForPerson(VitalRecord_tbl, personRow[U.PersonID_col].ToInt(), U.PersonID_col);
                    DataTable CemeteryRecord_tbl = SQL.DefineVitalRecord_Table();
                    SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, personId);
                    string birthDate = ""; string place = ""; string home = ""; string book = ""; string page = ""; string source = "";
                    bool verified = U.GetPersonVitalStatistics(VitalRecord_tbl, CemeteryRecord_tbl, personRow, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                                    ref book, ref page, ref birthDate, ref place, ref home, ref source, true);
                    string bornSource = personRow[U.BornSource_col].ToString();
                    if (!KnownNoAgeInCensus(personId, bornSource))
                    {
                        count++;
                        if (birthDate.Length > 3)
                        {
                            int birthYear = birthDate.Substring(0, 4).ToInt();
                            int age = latestYear - birthYear;
                            if (age < 0)
                            {
                                age = 1;
                            }
                            personRow[U.BornDate_col] = birthYear;
                            personRow[U.BornSource_col] = latestYear + " Census Age " + age;
                        }
                    }
                }
            }
            UpdateWithDA(personTbl, U.Person_Table, U.PersonID_col, ColumnList(U.BornDate_col, U.BornSource_col));
        }
        //****************************************************************************************************************************
        private static bool KnownNoAgeInCensus(int personId, string bornSource)
        {
            if (bornSource == "No Age in Census")
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private static void CheckCensusYear(int censusYear, int censusPage, ref bool found, ref int year)
        {
            if (censusPage != 0)
            {
                year = censusYear;
                found = true;
            }
        }
        //****************************************************************************************************************************
    }
}
