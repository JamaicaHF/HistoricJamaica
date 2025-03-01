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
        public static void DeletePicturedBuilding(int photoID,
                                          int buildingValueID)
        {
            DeleteWithParms(U.PicturedBuilding_Table, new NameValuePair(U.PhotoID_col, photoID), new NameValuePair(U.BuildingValueID_col, buildingValueID));
        }
        //****************************************************************************************************************************
    }
}
