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
        public static DataTable DefineBuildingTable()
        {
            DataTable tbl = new DataTable(U.Building_Table);
            tbl.Columns.Add(U.BuildingID_col, typeof(int));
            tbl.Columns.Add(U.BuildingName_col, typeof(string));
            tbl.Columns.Add(U.BuildingRoadValueID_col, typeof(int));
            tbl.Columns.Add(U.StreetNum_col, typeof(int));
            tbl.Columns.Add(U.BuildingGrandListID_col, typeof(int));
            tbl.Columns.Add(U.Building1856Name_col, typeof(string));
            tbl.Columns.Add(U.Building1869Name_col, typeof(string));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            tbl.Columns.Add(U.NotesCurrentOwner_col, typeof(string));
            tbl.Columns.Add(U.Notes1856Name_col, typeof(string));
            tbl.Columns.Add(U.Notes1869Name_col, typeof(string));
            tbl.Columns.Add(U.BuildingValueOrder1856Name_col, typeof(int));
            tbl.Columns.Add(U.BuildingValueOrder1869Name_col, typeof(int));
            tbl.Columns.Add(U.BuildingArchitectureArticleID_col, typeof(int));
            tbl.Columns.Add(U.BuildingDescriptionArticleID_col, typeof(int));
            tbl.Columns.Add(U.QRCode_col, typeof(string));
            tbl.Columns.Add(U.Then1_col, typeof(string));
            tbl.Columns.Add(U.Then2_col, typeof(string));
            tbl.Columns.Add(U.Now1_col, typeof(string));
            tbl.Columns.Add(U.Now2_col, typeof(string));
            tbl.Columns.Add(U.Then1Title_col, typeof(string));
            tbl.Columns.Add(U.Then2Title_col, typeof(string));
            tbl.Columns.Add(U.Now1Title_col, typeof(string));
            tbl.Columns.Add(U.Now2Title_col, typeof(string));
            SetBuildingVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetBuildingVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.BuildingName_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Building1856Name_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Building1869Name_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.NotesCurrentOwner_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.Notes1856Name_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.Notes1869Name_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.QRCode_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.Then1_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.Then2_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.Now1_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.Now2_col].MaxLength = U.iMaxBookPageLength;
            tbl.Columns[U.Then1Title_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Then2Title_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Now1Title_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Now2Title_col].MaxLength = U.iMaxValueLength;
        }
        //****************************************************************************************************************************
        public static string StreetAddress(int iGrandListID)
        {
            DataTable tbl = GetGrandList(iGrandListID);
            if (tbl.Rows.Count == 0)
                return "";
            DataRow row = tbl.Rows[0];
            int iStreetNum = row[U.StreetNum_col].ToInt();
            string sRoadName = GetModernRoadName(row[U.BuildingRoadValueID_col].ToInt());
            return iStreetNum.ToString() + " " + sRoadName;
        }
        //****************************************************************************************************************************
    }
}
