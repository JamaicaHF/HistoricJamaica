using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace SQL_Library
{
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static bool DeleteBuildingValue(int iBuildingValueID)
        {
            int iBuildingID = GetBuildingIDFromBuildingValueID(iBuildingValueID);
            DataTable tbl = DefineBuildingValueTable(U.BuildingValue_Table);
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.BuildingValueID_col] };
            GetAllBuildingValues(tbl, iBuildingID);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            SqlCommand updateCommand = null;
            if (ThereAreBuildingRowsToUpdateOtherThanDeletedRow(tbl, iBuildingValueID))
            {
                updateCommand = UpdateCommand(txn, tbl.Columns, U.BuildingValue_Table, U.BuildingValueID_col, 
                                              ColumnList(U.BuildingValueOrder_col));
            }
            SqlCommand deleteCommand = DeleteCommandWithDA(txn, U.BuildingValue_Table, U.BuildingValueID_col);
            if (DeleteUpdateWithDA(tbl, deleteCommand, updateCommand))
            {
                txn.Commit();
                return true;
            }
            else
            {
                txn.Rollback();
                return false;
            }
        }
        //****************************************************************************************************************************
        private static bool ThereAreBuildingRowsToUpdateOtherThanDeletedRow(DataTable tbl,
                                                                            int iBuildingValueID)
        {
            bool bDoUpdate = false;
            int iOrderNumber = 0;
            int iDeletedRowBuildingValueID = 99999999;
            foreach (DataRow row in tbl.Rows)
            {
                iOrderNumber++;
                int iOldBuildingValueID = row[U.BuildingValueID_col].ToInt();
                if (iOldBuildingValueID == iBuildingValueID)
                {
                    iDeletedRowBuildingValueID = iOrderNumber;
                    row.Delete();
                    iOrderNumber--;
                }
                else if (iOrderNumber >= iDeletedRowBuildingValueID)
                {
                    row[U.BuildingValueOrder_col] = iOrderNumber;
                    bDoUpdate = true;
                }
                else
                {
                    int iOldBuildingValueOrder = row[U.BuildingValueOrder_col].ToInt();
                    if (iOldBuildingValueOrder != iOrderNumber)
                    {
                        row[U.BuildingValueOrder_col] = iOrderNumber;
                        bDoUpdate = true;
                    }
                }
            }
            return bDoUpdate;
        }
        //****************************************************************************************************************************
        public static int GetBuildingIDFromBuildingValueID(int iBuildingValueID)
        {
            DataTable tbl = new DataTable(U.BuildingValue_Table);
            SelectAll(U.BuildingValue_Table, tbl, new NameValuePair(U.BuildingValueID_col, iBuildingValueID));
            if (tbl.Rows.Count == 0)
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static void CheckForDuplicateBuildingValues()
        {
            int count = 0;
            DataTable buildingsTbl = SQL.GetAllBuildings(false);
            DataTable buildingValuesTbl = new DataTable();
            SQL.GetAllBuildingValues(buildingValuesTbl);
            ArrayList buildingList = new ArrayList();
            foreach (DataRow buildingsRow in buildingsTbl.Rows)
            {
                int buildingId = buildingsRow[U.BuildingID_col].ToInt();
                if (buildingId == 242)
                {
                }
                string buildingName = buildingsRow[U.BuildingName_col].ToString().Trim();
                string selectStatement = U.BuildingID_col + " = " + buildingId;
                DataRow[] results = buildingValuesTbl.Select(selectStatement);
                if (results.Length > 0)
                {
                    foreach (DataRow buildValueRow in results)
                    {
                        string buildingValueName = buildValueRow[U.BuildingValueValue_col].ToString().Trim();
                        if (buildingValueName == buildingName)
                        {
                            buildValueRow.Delete();
                            count++;
                        }
                    }
                }
            }
            SqlCommand deleteCommand = DeleteCommandWithDA(null, U.BuildingValue_Table, U.BuildingValueID_col);
            DeleteWithDA(buildingValuesTbl, deleteCommand);
            MessageBox.Show("Number Duplicates: " + count);
        }
        //****************************************************************************************************************************
    }
}
