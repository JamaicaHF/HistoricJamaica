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
        public static DataTable GetBuilding(int iBuildingID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static int BuildingIDFromGrandListID(int iBuildingGrandListID)
        {
            DataTable tbl = DefineBuildingTable();
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingGrandListID_col, iBuildingGrandListID.ToString()));
            if (tbl.Rows.Count != 0)
                return tbl.Rows[0][U.BuildingID_col].ToInt();
            else
                return 0;
        }
        //****************************************************************************************************************************
        public static int GetBuildingRoadValueID(int sBuildingID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, sBuildingID));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                int iBuildingRoadValueID = row[U.BuildingRoadValueID_col].ToInt();
                if (iBuildingRoadValueID == 0)
                {
                    int iGrandListID = row[U.BuildingGrandListID_col].ToInt();
                    if (iGrandListID != 0)
                    {
                        iBuildingRoadValueID = GetModernRoadID(iGrandListID);
                    }
                }
                return iBuildingRoadValueID;
            }
        }
        //****************************************************************************************************************************
        public static int GetBuildingIDFromGrandListID(int iGrandlistID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingGrandListID_col, iGrandlistID));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static int GetBuildingGrandListID(int iBuildingID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                int iGrandListID = row[U.BuildingGrandListID_col].ToInt();
                return iGrandListID;
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllBuildings(bool checkPrimaryValues=false)
        {
            DataTable tbl = DefineBuildingTable();
            SelectAll(U.Building_Table, OrderBy(U.BuildingName_col), tbl);
            if (!checkPrimaryValues)
            {
                return tbl;
            }
            foreach (DataRow row in tbl.Rows)
            {
                int iBuildingID = row[U.BuildingID_col].ToInt();
                string sBuildingName = row[U.BuildingName_col].ToString();
                //CheckToEnsureThereIsARecordForPrimaryValue(iBuildingID, sBuildingName);
            }
            return tbl;
        }
        //****************************************************************************************************************************
        public static int CheckToEnsureThereIsARecordForPrimaryValue(int iBuildingID,
                                                               string sBuildingName)
        {
            DataTable BuildingID_tbl = new DataTable();
            GetAllBuildingValues(BuildingID_tbl, iBuildingID);
            foreach (DataRow row in BuildingID_tbl.Rows)
            {
                if (row[U.BuildingValueOrder_col].ToInt() == 1)
                {
                    return row[U.BuildingValueID_col].ToInt();
                }
            }
            return InsertBuildingValue(iBuildingID, sBuildingName, 1);
        }
        //****************************************************************************************************************************
        public static string GetBuildingName(int sBuildingID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, sBuildingID));
            if (tbl.Rows.Count == 0)     // does not exist
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingName_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static string GetBuildingMapBuildingName(int sBuildingID, string BuildingMapNameCol)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, sBuildingID));
            if (tbl.Rows.Count == 0)     // does not exist
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[BuildingMapNameCol].ToString();
            }
        }
        //****************************************************************************************************************************
        public static int GetBuildingArticleID(int iBuildingID,
                                        string BuildingArticleID_col)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[BuildingArticleID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static int GetBuildingDescriptionArticleID(int iBuildingID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingDescriptionArticleID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static int GetBuildingIDFromName(string sBuildingName)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingName_col, sBuildingName));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetBuildingByRoadNoGrandListID(int buildingRoadValueID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, OrderBy(U.StreetNum_col), tbl, new NameValuePair(U.BuildingRoadValueID_col, buildingRoadValueID),
                                                                          new NameValuePair(U.BuildingGrandListID_col, "0"));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetBuildingBuyRoad(int BuildingRoadValueID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingRoadValueID_col, BuildingRoadValueID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static int GetBuildingValueID(string sBuildingID,
                                      string sBuildingValueValue)
        {
            DataTable tbl = new DataTable(U.BuildingValue_Table);
            SelectAll(U.BuildingValue_Table, tbl, new NameValuePair(U.BuildingID_col, sBuildingID),
                                                  new NameValuePair(U.BuildingValueValue_col, sBuildingValueValue));
            if (tbl.Rows.Count == 0)
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingValueID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
    }
}
