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
        public static DataTable DefineAlternativeSpellingsTable(string sTable)
        {
            DataTable tbl = new DataTable(sTable);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.NameSpelling1_Col, typeof(string));
            tbl.Columns.Add(U.NameSpelling2_Col, typeof(string));
            tbl.Columns[U.NameSpelling1_Col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.NameSpelling2_Col].MaxLength = U.iMaxNameLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static void MergeAlternativeSpellingsTables()
        {
            DataTable AlternativeSpellings_tbl = GetAllAlternativeSpellings(U.AlternativeSpellingsFirstName_Table);
            DataTable old_tbl = GetAllAlternativeSpellings(U.AlternativeSpellingsFirstName_Table + "Old");
            foreach (DataRow old_row in old_tbl.Rows)
            {
                string name1 = old_row[U.NameSpelling1_Col].ToString();
                string name2 = old_row[U.NameSpelling2_Col].ToString();
                if (!foundNames(AlternativeSpellings_tbl, name1, name2))
                {
                    NamesToRow(AlternativeSpellings_tbl, name1, name2);
                }
                else
                {
                }
            }
            SqlCommand insertCommand = InsertCommand(AlternativeSpellings_tbl, U.AlternativeSpellingsFirstName_Table, false);
            InsertWithDA(AlternativeSpellings_tbl, insertCommand);
        }
        //****************************************************************************************************************************
        private static void NamesToRow(DataTable AlternativeSpellings_tbl,
                                string name1,
                                string name2)
        {
            DataRow AlternativeSpellings_row = AlternativeSpellings_tbl.NewRow();

            string selectStatement = U.NameSpelling1_Col + "='" + name2 + "'";
            DataRow[] foundRows = AlternativeSpellings_tbl.Select(selectStatement);
            if (foundRows.Length == 0)
            {
                AlternativeSpellings_row[U.NameSpelling1_Col] = name1;
                AlternativeSpellings_row[U.NameSpelling2_Col] = name2;
            }
            else
            {
                AlternativeSpellings_row[U.NameSpelling1_Col] = name2;
                AlternativeSpellings_row[U.NameSpelling2_Col] = name1;
            }
            AlternativeSpellings_tbl.Rows.Add(AlternativeSpellings_row);
        }
        //****************************************************************************************************************************
        private static bool foundNames(DataTable AlternativeSpellings_tbl,
                                string Name1,
                                string Name2)
        {
            string selectStatement = U.NameSpelling1_Col + "='" + Name1 + "' and " + U.NameSpelling2_Col + "='" + Name2 + "'";
            DataRow[] foundRows = AlternativeSpellings_tbl.Select(selectStatement);
            return foundRows.Length != 0;
        }
        //****************************************************************************************************************************
    }
}
