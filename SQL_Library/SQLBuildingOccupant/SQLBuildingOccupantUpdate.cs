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
        public static void UpdateBuildingOccupant(int iPersonID,
                                              int iBuildingID,
                                              string Value_col,
                                              object Value)
        {
            NameValuePair[] keyColumns = new NameValuePair[] { new NameValuePair(U.PersonID_col, iPersonID), 
                                                               new NameValuePair(U.BuildingID_col, iBuildingID)};
            UpdateWithParms(U.BuildingOccupant_Table, keyColumns, new NameValuePair(Value_col, Value, false));
        }
        //****************************************************************************************************************************
    }
}
