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
        public static void UpdateGrandlist(DataTable tbl,
                                           ArrayList FieldsModified)
        {
            UpdateWithDA(tbl, U.GrandList_Table, U.GrandListID_col, FieldsModified);
        }
        //****************************************************************************************************************************
        public static void UpdateModernRoadValue(int iModernRoadValueID,
                                                 string field_col,
                                                 int fieldValue)
        {
            UpdateWithParms(U.ModernRoadValue_Table, new NameValuePair(U.ModernRoadValueID_col, iModernRoadValueID),
                                              new NameValuePair(field_col, fieldValue));
        }
        //****************************************************************************************************************************
        public static void UpdateModernRoadValue(int iModernRoadValueID,
                                                 string field_col,
                                                 string fieldValue)
        {
            UpdateWithParms(U.ModernRoadValue_Table, new NameValuePair(U.ModernRoadValueID_col, iModernRoadValueID),
                                              new NameValuePair(field_col, fieldValue));
        }
        //****************************************************************************************************************************
        public static void InsertModernRoadName(int iID,
                                           string sName)
        {
            DataTable modernRoadValue_tbl = SQL.DefineModernRoadValueTable();
            DataRow modernRoadValue_row = modernRoadValue_tbl.NewRow();
            modernRoadValue_row[U.ModernRoadValueID_col] = iID;
            modernRoadValue_row[U.ModernRoadValueValue_col] = sName;
            modernRoadValue_row[U.ModernRoadValueOrder_col] = 9999;
            modernRoadValue_row[U.ModernRoadValueSection_col] = 0;
            modernRoadValue_row[U.JRoadName_col] = 0;
            modernRoadValue_tbl.Rows.Add(modernRoadValue_row);
            SqlCommand insertCommand = InsertCommand(modernRoadValue_tbl, U.ModernRoadValue_Table, false);
            InsertWithDA(modernRoadValue_tbl, insertCommand);
        }
    }
}
