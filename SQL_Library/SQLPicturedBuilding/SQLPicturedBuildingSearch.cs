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
        public static DataTable GetAllPicturedBuildings()
        {
            DataTable PicturedBuilding_tbl = new DataTable();
            SelectAll(U.PicturedBuilding_Table, OrderBy(U.BuildingID_col), PicturedBuilding_tbl);
            return PicturedBuilding_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetPicturedBuildings(int photoID)
        {
            DataTable PicturedBuilding_tbl = new DataTable();
            SelectAll(U.PicturedBuilding_Table, OrderBy(U.PicturedBuildingNumber_col), PicturedBuilding_tbl,
                      new NameValuePair(U.PhotoID_col, photoID));
            return PicturedBuilding_tbl;
        }
        //****************************************************************************************************************************
    }
}
