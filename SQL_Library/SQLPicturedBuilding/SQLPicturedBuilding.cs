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
        public static DataTable DefinePicturedBuilding()
        {
            DataTable tbl = new DataTable(U.PicturedBuilding_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PicturedBuildingNumber_col, typeof(int));
            tbl.Columns.Add(U.PhotoID_col, typeof(int));
            tbl.Columns.Add(U.BuildingID_col, typeof(int));
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PicturedBuildingNumber_col] };
            return tbl;
        }
        //***************************************************************************************************************************
        public static int CheckDuplicatePicturedPeople()
        {
            DataTable tbl = new DataTable();
            SelectAll(U.PicturedPerson_Table, OrderBy(U.PhotoID_col, U.PicturedPersonNumber_col), tbl);
            int iPreviousNumber = 0;
            int iPreviousPhotoID = 0;
            int iNumDuplicates = 0;
            foreach (DataRow row in tbl.Rows)
            {
                int iNumber = row[U.PicturedPersonNumber_col].ToInt();
                int iPhotoID = row[U.PhotoID_col].ToInt();
                if (iNumber == iPreviousNumber && iPhotoID == iPreviousPhotoID)
                {
                    iNumDuplicates++;
                }
                iPreviousNumber = iNumber;
                iPreviousPhotoID = iPhotoID;
            }
            return iNumDuplicates;
        }
        //***************************************************************************************************************************
        public static int CheckDuplicatePicturedBuildings()
        {
            DataTable tbl = new DataTable();
            SelectAll(U.PicturedBuilding_Table, OrderBy(U.PhotoID_col, U.PicturedBuildingNumber_col), tbl);
            int iPreviousNumber = 0;
            int iPreviousPhotoID = 0;
            int iNumDuplicates = 0;
            foreach (DataRow row in tbl.Rows)
            {
                int iNumber = row[U.PicturedBuildingNumber_col].ToInt();
                int iPhotoID = row[U.PhotoID_col].ToInt();
                if (iNumber == iPreviousNumber && iPhotoID == iPreviousPhotoID)
                {
                    iNumDuplicates++;
                }
                iPreviousNumber = iNumber;
                iPreviousPhotoID = iPhotoID;
            }
            return iNumDuplicates;
        }
        //****************************************************************************************************************************
    }
}
