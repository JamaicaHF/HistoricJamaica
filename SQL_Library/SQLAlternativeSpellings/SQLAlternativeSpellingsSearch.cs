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
        private static void AlternativeFirstNames(DataTable tbl,
                                                  DataTable FirstNameAlternativeSpellings_tbl,
                                                  PersonSearchTableAndColumns personSearchTableAndColumns,
                                                  string sLastName)
        {
            foreach (DataRow row in FirstNameAlternativeSpellings_tbl.Rows)
            {
                SelectAll(personSearchTableAndColumns.tableName, NameOrderByStatement(), tbl, 
                                             new NameValuePair(personSearchTableAndColumns.lastNameCol, sLastName),
                                             new NameValuePair(personSearchTableAndColumns.firstNameCol, row[U.AlternativeSpelling_Col].ToString()));
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllAlternativeSpellings(string sTable)
        {
            DataTable tbl = DefineAlternativeSpellingsTable(sTable);
            SelectAll(sTable, OrderBy(U.NameSpelling1_Col, U.NameSpelling2_Col), tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAlternativeSpellings(string sTable,
                                                        string sName)
        {
            DataTable AlternativeSpellings_tbl = new DataTable(sTable);
            AlternativeSpellings_tbl.Columns.Add(U.AlternativeSpelling_Col);
            AlternativeSpellings_tbl.PrimaryKey = new DataColumn[] { AlternativeSpellings_tbl.Columns[U.AlternativeSpelling_Col] };
            GetAlternativeSpellingsForName(AlternativeSpellings_tbl, sTable, sName);
            int iCountBefore = AlternativeSpellings_tbl.Rows.Count;
            /*
            while (iCountBefore != iCountAfter && iCounter < 10)
            {
                iCountBefore = AlternativeSpellings_tbl.Rows.Count;
                DataTable CopyTable = AlternativeSpellings_tbl.Copy();
                foreach (DataRow row in CopyTable.Rows)
                {
                    GetAlternativeSpellingsForName(AlternativeSpellings_tbl, sTable, row[U.AlternativeSpelling_Col].ToString());
                }
                iCountAfter = AlternativeSpellings_tbl.Rows.Count;
                iCounter++;
            }
            */
            return AlternativeSpellings_tbl;
        }
        //****************************************************************************************************************************
        private static void GetAlternativeSpellingsForName(DataTable tbl,
                                                    string sTable,
                                                    string sName)
        {
            SelectColumnsAs(tbl, U.NameSpelling2_Col, U.AlternativeSpelling_Col, sTable, new NameValuePair(U.NameSpelling1_Col, sName));
            SelectColumnsAs(tbl, U.NameSpelling1_Col, U.AlternativeSpelling_Col, sTable, new NameValuePair(U.NameSpelling2_Col, sName));
        }
        //****************************************************************************************************************************
        public static bool AlternativeSpellingAlreadyExists(string sTable,
                                                            string sName1,
                                                            string sName2)
        {
            DataTable tbl = GetAlternativeSpellings(sTable, sName1, sName2);
            if (tbl.Rows.Count == 0)
            {
                tbl = GetAlternativeSpellings(sTable, sName2, sName1);
            }
            return (tbl.Rows.Count != 0);
        }
        //****************************************************************************************************************************
        public static DataTable GetAlternativeSpellings(string sTable,
                                                   string sName1,
                                                   string sName2)
        {
            DataTable tbl = new DataTable();
            SelectAll(sTable, tbl, new NameValuePair(U.NameSpelling1_Col, sName1), new NameValuePair(U.NameSpelling2_Col, sName2));
            return tbl;
        }
        //****************************************************************************************************************************
    }
}
