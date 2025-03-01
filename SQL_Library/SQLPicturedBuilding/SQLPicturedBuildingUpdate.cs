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
        public static void UpdateBuildingValueID(int buildingValueID,
                                                 int buildingID)
        {
            UpdateWithParms(U.PicturedBuilding_Table, new NameValuePair(U.BuildingValueID_col, buildingValueID),
                                              new NameValuePair(U.BuildingID_col, buildingID));
        }
        //****************************************************************************************************************************
    }
}
