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
        public static DataTable DefineBuildingValueTable(string sBuildingValueTable)
        {
            DataTable tbl = new DataTable(sBuildingValueTable);
            tbl.Columns.Add(U.BuildingValueID_col, typeof(int));
            tbl.Columns.Add(U.BuildingID_col, typeof(int));
            tbl.Columns.Add(U.BuildingValueValue_col, typeof(string));
            tbl.Columns.Add(U.BuildingValueOrder_col, typeof(int));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            tbl.Columns[U.BuildingValueValue_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static bool CheckPrimaryBuildingValue()
        {
            DataTable ValueTbl = new DataTable();
            DataTable BuildingTbl = GetAllBuildings();
            int iCount = 0;
            foreach (DataRow row in BuildingTbl.Rows)
            {
                ValueTbl.Clear();
                int iBuildingID = row[U.BuildingID_col].ToInt();
                GetAllBuildingValues(ValueTbl, iBuildingID);
                if (ValueTbl.Rows.Count != 0)
                {
                    string sValueName = ValueTbl.Rows[0][U.BuildingValueValue_col].ToString();
                    string sBuildingName = row[U.BuildingName_col].ToString();
                    if (sValueName != sBuildingName)
                    {
                        iCount++;
                    }
                }
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
