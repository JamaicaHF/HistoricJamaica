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
        public static bool DeleteBuildingOccupants(int iPersonID,
                                            int iBuildingID)
        {
            int iNumDeleted = DeleteWithParms(U.BuildingOccupant_Table, new NameValuePair(U.PersonID_col, iPersonID), 
                                                         new NameValuePair(U.BuildingID_col, iBuildingID));
            return (iNumDeleted > 0);
        }
        //****************************************************************************************************************************
        public static void DeleteBuilding(int buildingID)
        {
            DeleteWithParms(U.BuildingOccupant_Table, new NameValuePair(U.BuildingID_col, buildingID));
            SQL.DeleteWithParms(U.BuildingValue_Table, new NameValuePair(U.BuildingID_col, buildingID));
            SQL.DeleteWithParms(U.Building_Table, new NameValuePair(U.BuildingID_col, buildingID));
        }
        //****************************************************************************************************************************
    }
}
