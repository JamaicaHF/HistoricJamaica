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
        public static bool SetGrandListInsertParms(ref SqlCommand cmd)
        {
            cmd.Parameters.Add(U.TaxMapID_col, SqlDbType.VarChar, U.iMaxNameLength, U.TaxMapID_col);
            cmd.Parameters.Add(U.StreetName_col, SqlDbType.VarChar, U.iMaxValueLength, U.StreetName_col);
            cmd.Parameters.Add(U.StreetNum_col, SqlDbType.Int, 0, U.StreetNum_col);
            cmd.Parameters.Add(U.Name1_col, SqlDbType.VarChar, U.iMaxValueLength, U.Name1_col);
            cmd.Parameters.Add(U.Name2_col, SqlDbType.VarChar, U.iMaxValueLength, U.Name2_col);
            cmd.Parameters.Add(U.WhereOwnerLiveID_col, SqlDbType.Char, 1, U.WhereOwnerLiveID_col);
            cmd.Parameters.Add(U.BuildingRoadValueID_col, SqlDbType.Char, 1, U.BuildingRoadValueID_col);
            return true;
        }
        //****************************************************************************************************************************
        public static DataTable DefineModernRoadTable()
        {
            DataTable tbl = new DataTable(U.ModernRoadValue_Table);
            tbl.Columns.Add(U.ModernRoadValueID_col, typeof(int));
            tbl.Columns.Add(U.ModernRoadValueValue_col, typeof(string));
            tbl.Columns.Add(U.ModernRoadValueOrder_col, typeof(int));
            return tbl;
        }
        //****************************************************************************************************************************
        public static bool MoveModernRoads()
        {
            DataTable tbl = new DataTable();
            GetAllCategoryValues(tbl, 3);
            foreach (DataRow row in tbl.Rows)
            {
                InsertModernRoad(row[U.CategoryValueID_col].ToInt(), row[U.CategoryValueValue_col].ToString());
            }
            return true;
        }
        //****************************************************************************************************************************
        public static int InsertModernRoad(int iModernRoadValueID,
                                    string sModernRoadValueValue)
        {
            DataTable tbl = DefineModernRoadTable();
            tbl.Rows.Add(iModernRoadValueID, sModernRoadValueValue, 0);
            SqlCommand insertCommand = InsertCommand(tbl, U.ModernRoadValue_Table, false);
            if (InsertWithDA(tbl, insertCommand))
                return iModernRoadValueID;
            else
                return 0;
        }
        //****************************************************************************************************************************
        public static bool InsertGrandList(DataTable GrandList_tbl)
        {
            SqlCommand insertCommand = InsertCommand(GrandList_tbl, U.GrandList_Table, true);
            return InsertWithDA(GrandList_tbl, insertCommand);
        }
        //****************************************************************************************************************************
    }
}
