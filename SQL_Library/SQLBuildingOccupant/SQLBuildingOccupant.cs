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
        public static DataTable DefineBuildingOccupantTable()
        {
            DataTable tbl = new DataTable(U.BuildingOccupant_Table);
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.SpouseLivedWithID_col, typeof(int));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            tbl.Columns.Add(U.BuildingValueOrder_col, typeof(int));
            tbl.Columns.Add(U.CensusYears_col, typeof(int));
            tbl.Columns.Add(U.BuildingID_col, typeof(int));
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefineBuildingOccupantTableForPerson()
        {
            DataTable tbl = new DataTable(U.BuildingOccupant_Table);
            tbl.Columns.Add(U.BuildingName_col, typeof(string));
            tbl.Columns.Add(U.SpouseLivedWithID_col, typeof(int));
            tbl.Columns.Add(U.Notes_col, typeof(string));
            tbl.Columns.Add(U.BuildingValueOrder_col, typeof(int));
            tbl.Columns.Add(U.CensusYears_col, typeof(int));
            tbl.Columns.Add(U.BuildingID_col, typeof(int));
            tbl.Columns[U.Notes_col].MaxLength = U.iMaxDescriptionLength;
            return tbl;
        }
        //****************************************************************************************************************************
    }
}
