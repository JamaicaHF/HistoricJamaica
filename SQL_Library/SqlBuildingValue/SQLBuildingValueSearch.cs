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
        public static DataTable GetBuildingIDValue(int iBuildingValueID)
        {
            DataTable tbl = new DataTable(U.BuildingValue_Table);
            SelectAll(U.BuildingValue_Table, tbl, new NameValuePair(U.BuildingValueID_col, iBuildingValueID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static void GetAllBuildingValues(DataTable tbl,
                                         int iBuildingID)
        {
            SelectAll(U.BuildingValue_Table, OrderBy(U.BuildingValueOrder_col), tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
        }
        //****************************************************************************************************************************
        public static int GetBuildingIDFromValueID(int iBuildingValueID)
        {
            DataTable tbl = GetBuildingIDValue(iBuildingValueID);
            if (tbl.Rows.Count == 0)
                return 0;
            else
                return tbl.Rows[0][U.BuildingID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static bool GetBuildingIDValue(DataTable tbl,
                                       string sBuildingID,
                                       string sBuildingValueValue)
        {
            SelectAll(U.BuildingValue_Table, tbl, new NameValuePair(U.BuildingID_col, sBuildingID),
                                                  new NameValuePair(U.BuildingValueValue_col, sBuildingValueValue));
            return true;
        }
        //****************************************************************************************************************************
        public static string GetBuildingValue(string sValue_col,
                                       int sBuildingID)
        {
            DataTable tbl = new DataTable(U.Building_Table);
            SelectAll(U.Building_Table, tbl, new NameValuePair(U.BuildingID_col, sBuildingID));
            if (tbl.Rows.Count == 0)     // does not exist
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[sValue_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static string GetBuildingValueNotes(int iBuildingValueID)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.BuildingValue_Table, tbl, new NameValuePair(U.BuildingValueID_col, iBuildingValueID));
            if (tbl.Rows.Count == 0)
                return "";
            else
                return tbl.Rows[0][U.Notes_col].ToString();
        }
        //****************************************************************************************************************************
        public static void GetAllBuildingValues(DataTable tbl)
        {
            SelectAll(U.BuildingValue_Table, OrderBy(U.BuildingValueValue_col), tbl);
        }
        //****************************************************************************************************************************
        public static bool CheckAllBuildingValueForOccupants()
        {
            DataTable tbl = new DataTable();
            SelectAllGreaterThan(U.BuildingValue_Table, tbl, new NameValuePair(U.BuildingValueOrder_col, "1"));
            foreach (DataRow row in tbl.Rows)
            {
                int iBuildingValueID = row[U.BuildingValueID_col].ToInt();
                DataTable Occupant_tbl = new DataTable();
                GetAllBuildingOccupants(Occupant_tbl, iBuildingValueID);
                if (Occupant_tbl.Rows.Count != 0)
                {

                }
            }
            return true;
        }
        //****************************************************************************************************************************
        public static string GetBuildingOccupantNotes(int iPersonID,
                                                         int iBuildingID)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.BuildingOccupant_Table, tbl, new NameValuePair(U.PersonID_col, iPersonID),
                                                        new NameValuePair(U.BuildingID_col, iBuildingID));
            if (tbl.Rows.Count == 0)
                return "";
            else
                return tbl.Rows[0][U.Notes_col].ToString();
        }
        //****************************************************************************************************************************
        public static bool PersonLivedInBuilding(int iPersonID,
                                          int iBuildingID)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.BuildingOccupant_Table, tbl, new NameValuePair(U.PersonID_col, iPersonID),
                                                        new NameValuePair(U.BuildingID_col, iBuildingID));
            return (tbl.Rows.Count != 0);
        }
        //****************************************************************************************************************************
        public static DataTable GetBuildingOccupantRecords()
        {
            DataTable tbl = new DataTable();
            SelectAll(U.BuildingOccupant_Table, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static string GetBuildingValueValue(int iBuildingValueID)
        {
            DataTable tbl = new DataTable(U.BuildingValue_Table);
            SelectAll(U.BuildingValue_Table, tbl, new NameValuePair(U.BuildingValueID_col, iBuildingValueID));
            if (tbl.Rows.Count == 0)
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingValueValue_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static bool GetAllBuildingPhotosFromID(DataTable PictureTBL,
                                                      int iBuildingID)
        {
            SelectAll(U.PicturedBuilding_Table, PictureTBL, new NameValuePair(U.BuildingID_col, iBuildingID));
            return true;
        }
        //****************************************************************************************************************************
    }
}
