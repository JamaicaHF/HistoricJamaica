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
        public static void DeleteAllBuildingWithGrandListID(int iBuildingGrandListID)
        {
            DataTable buildingTbl = SelectAll(U.Building_Table, new NameValuePair(U.BuildingGrandListID_col, iBuildingGrandListID));
            foreach (DataRow buildingRow in buildingTbl.Rows)
            {
                DeleteBuilding(buildingRow[U.BuildingID_col].ToInt());
            }
        }
        //****************************************************************************************************************************
        public static void RemoveGrandListIDFromAllBuildings(int iBuildingGrandListID)
        {
            DataTable buildingTbl = SelectAll(U.Building_Table, new NameValuePair(U.BuildingGrandListID_col, iBuildingGrandListID));
            foreach (DataRow buildingRow in buildingTbl.Rows)
            {
                UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, buildingRow[U.BuildingID_col]),
                                                  new NameValuePair(U.BuildingGrandListID_col, 0));
            }
        }
        //****************************************************************************************************************************
        public static void UpdateBuildingGrandListID(int iBuildingID,
                                              int iBuildingGrandListID)
        {
            RemoveGrandListIDFromAllBuildings(iBuildingGrandListID);
            UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, iBuildingID),
                                              new NameValuePair(U.BuildingGrandListID_col, iBuildingGrandListID));
        }
        //****************************************************************************************************************************
        public static void UpdateBuildingValues(DataTable tbl,
                                                params string[] cols) 
        {
            ArrayList columnsToUpdate = new ArrayList();
            foreach (string col in cols)
            {
                columnsToUpdate.Add(col);
            }
            UpdateWithDA(tbl, U.Building_Table, U.BuildingID_col, columnsToUpdate);
        }
        //****************************************************************************************************************************
        public static void UpdateBuildingRoadValueID(int iBuildingID,
                                              int iBuildingRoadValueID)
        {
            UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, iBuildingID),
                                              new NameValuePair(U.BuildingRoadValueID_col, iBuildingRoadValueID));
        }
        //****************************************************************************************************************************
        public static void UpdateBuildingValue(int iBuildingID,
                                        string sValue_col,
                                        string sValue)
        {
            UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, iBuildingID),
                                              new NameValuePair(sValue_col, sValue, false));
        }
        //****************************************************************************************************************************
        public static void UpdateBuildingArticleID(int iBuildingID,
                                            string BuildingArticleID_col,
                                            int iArticleID)
        {
            UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, iBuildingID),
                                              new NameValuePair(BuildingArticleID_col, iArticleID));
        }
        //****************************************************************************************************************************
        public static void UpdateBuildingName(int iBuildingID,
                                              string sBuildingName)
        {
            UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, iBuildingID),
                                              new NameValuePair(U.BuildingName_col, sBuildingName));
        }
        //****************************************************************************************************************************
        public static void UpdateMapBuildingName(int iBuildingID,
                                                  string sBuildingMapName,
                                                  string BuildingMapName_col,
                                                  int iPersonID)
        {
            UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, iBuildingID),
                                              new NameValuePair(BuildingMapName_col, sBuildingMapName));
        }
        //****************************************************************************************************************************
        public static void UpdateOccupant(int iBuildingID,
                                   string sOccupant_col,
                                   int iOccupant,
                                   string sOccupantOrder_col,
                                   int iOrder)
        {
            UpdateWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, iBuildingID),
                                              new NameValuePair(sOccupant_col, sOccupant_col),
                                              new NameValuePair(sOccupantOrder_col, iOrder));
        }
        //****************************************************************************************************************************
    }
}
