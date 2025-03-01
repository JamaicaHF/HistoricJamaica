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
        public static void UpdateAlternativeSpelling(string sTable,
                                             string sOldName1,
                                             string sOldName2,
                                             string sName1,
                                             string sName2)
        {
            DataTable tbl = DefineAlternativeSpellingsTable(sTable);
            GetAlternativeSpellingForNameCombination(tbl, sTable, sOldName1, sOldName2);
            if (tbl.Rows.Count == 0)
            {
                InsertAlternativeSpelling(sTable, sName1, sName2);
                return;
            }
            DataRow row = tbl.Rows[0];
            row[U.NameSpelling1_Col] = sName1;
            row[U.NameSpelling2_Col] = sName2;

            ArrayList updateColumns = ColumnList(U.NameSpelling1_Col, U.NameSpelling2_Col);
            UpdateAllNoKeysDA(tbl, sTable, updateColumns);
        }
        //****************************************************************************************************************************
        public static void InsertAlternativeSpelling(string sTable,
                                                     DataTable alternativeSpelling_tbl)
        {
            SqlCommand insertCommand = InsertCommand(alternativeSpelling_tbl, sTable, false);
            InsertWithDA(alternativeSpelling_tbl, insertCommand);
        }
        //****************************************************************************************************************************
        public static void InsertAlternativeSpelling(string sTable,
                                             string sName1,
                                             string sName2)
        {
            DataTable alternativeSpelling_tbl = DefineAlternativeSpellingsTable(sTable);
            DataRow alternativeSpelling_row = alternativeSpelling_tbl.NewRow();
            alternativeSpelling_row[U.NameSpelling1_Col] = sName1;
            alternativeSpelling_row[U.NameSpelling2_Col] = sName2;
            alternativeSpelling_tbl.Rows.Add(alternativeSpelling_row);
            SqlCommand insertCommand = InsertCommand(alternativeSpelling_tbl, sTable, false);
            InsertWithDA(alternativeSpelling_tbl, insertCommand);
        }
        //****************************************************************************************************************************
        public static bool GetAlternativeSpellingForNameCombination(DataTable tbl,
                                                             string sTable,
                                                             string sName1,
                                                             string sName2)
        {
            SelectAll(sTable, tbl, new NameValuePair(U.NameSpelling1_Col, sName1), 
                                   new NameValuePair(U.NameSpelling2_Col, sName2));
            if (tbl.Rows.Count == 0)
            {
                SelectAll(sTable, tbl, new NameValuePair(U.NameSpelling2_Col, sName1),
                                       new NameValuePair(U.NameSpelling1_Col, sName2));
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
