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
        public static string AddRoadToGroup(string sGroup,
                                     int iValueID,
                                     int iGrandListID,
                                     int buildingId=0)
        {
            string sValueValue = "";
            if (iGrandListID == 0)
            {
                if (buildingId != 0)
                {
                    DataTable buildingTbl = SQL.GetBuilding(buildingId);
                    if (buildingTbl.Rows.Count > 0)
                    {
                        DataRow buildingRow = buildingTbl.Rows[0];
                        string sRoadName = GetModernRoadName(buildingRow[U.BuildingRoadValueID_col].ToInt());
                        sValueValue = buildingRow[U.StreetNum_col].ToInt() + " " + sRoadName;
                    }
                }
                else
                {
                    sValueValue = GetCategoryValueValue(iValueID);
                }
            }
            else
                sValueValue = StreetAddress(iGrandListID);
            int iLocationOfRoad = sGroup.IndexOf("(");
            if (iLocationOfRoad > 0)
                sGroup = sGroup.Remove(iLocationOfRoad);
            sGroup = sGroup.TrimString();
            return sGroup.TrimString() + "  (" + sValueValue + ")";
        }
        //****************************************************************************************************************************
        public static int InsertBuilding(string sBuildingName,
                                         int buildingRoadValueId, // buildingRoadValueId should be zero when grandListId M< 0
                                         int grandListId)  // Grandlist should be zero when buildingRoadValueId <> 0 
        {
            DataTable tbl = DefineBuildingTable();
            DataRow row = tbl.NewRow();
            row[U.BuildingID_col] = 0;
            row[U.BuildingName_col] = sBuildingName;
            row[U.BuildingRoadValueID_col] = buildingRoadValueId;
            row[U.StreetNum_col] = 0;
            row[U.BuildingGrandListID_col] = grandListId;
            row[U.Building1856Name_col] = "";
            row[U.Building1869Name_col] = "";
            row[U.Notes_col] = "";
            row[U.NotesCurrentOwner_col] = "";
            row[U.Notes1856Name_col] = "";
            row[U.Notes1869Name_col] = "";
            row[U.BuildingValueOrder1856Name_col] = 0;
            row[U.BuildingValueOrder1869Name_col] = 0;
            row[U.BuildingArchitectureArticleID_col] = 0;
            row[U.BuildingDescriptionArticleID_col] = 0;
            row[U.QRCode_col] = "";
            row[U.Then1_col] = "";
            row[U.Then2_col] = "";
            row[U.Now1_col] = "";
            row[U.Now2_col] = "";
            row[U.Then1Title_col] = "";
            row[U.Then2Title_col] = "";
            row[U.Now1Title_col] = "";
            row[U.Now2Title_col] = "";
            tbl.Rows.Add(row);
            SqlCommand insertCommand = InsertCommand(tbl, U.Building_Table, true);
            if (InsertWithDA(tbl, insertCommand))
                return tbl.Rows[0][U.BuildingID_col].ToInt();
            else
                return 0;
        }
        //****************************************************************************************************************************
    }
}
