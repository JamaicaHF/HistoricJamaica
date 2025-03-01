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
        public static int InsertBuildingValue(int iBuildingID,
                                       string sBuildingIDValue,
                                       int iBuildingValueOrder)
        {
            DataTable tbl = DefineBuildingValueTable(U.BuildingValue_Table);
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.BuildingValueID_col] };
            GetAllBuildingValues(tbl, iBuildingID);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            SqlCommand updateCommand = null;
            if (ThereAreBuildingRowsToUpdate(tbl, iBuildingID, iBuildingValueOrder))
            {
                updateCommand = UpdateCommand(txn, tbl.Columns, U.BuildingValue_Table, U.BuildingValueID_col, ColumnList(U.BuildingValueOrder_col));
            }
            tbl.Rows.Add(0, iBuildingID, sBuildingIDValue, iBuildingValueOrder, "");
            SqlCommand insertCommand = InsertCommand(txn, tbl, U.BuildingValue_Table, true);
            if (UpdateInsertWithDA(tbl, updateCommand, insertCommand))
            {
                txn.Commit();
                int iAddedValueIndex = tbl.Rows.Count;
                int i = tbl.Rows[iAddedValueIndex - 1][U.BuildingValueID_col].ToInt();
                return tbl.Rows[iAddedValueIndex - 1][U.BuildingValueID_col].ToInt();
            }
            else
            {
                txn.Rollback();
                return 0;
            }
        }
        //****************************************************************************************************************************
        private static bool ThereAreBuildingRowsToUpdate(DataTable tbl,
                                          int iBuildingID,
                                          int iBuildingValueOrder)
        {
            bool bDoUpdate = false;
            int iOrderNumber = 0;
            foreach (DataRow row in tbl.Rows)
            {
                iOrderNumber++;
                int iOldBuildingValueOrder = row[U.BuildingValueOrder_col].ToInt();
                if (iOrderNumber >= iBuildingValueOrder)
                {
                    row[U.BuildingValueOrder_col] = iOrderNumber + 1;
                    bDoUpdate = true;
                }
                else if (iOldBuildingValueOrder != iOrderNumber)
                {
                    row[U.BuildingValueOrder_col] = iOrderNumber;
                    bDoUpdate = true;
                }
            }
            return bDoUpdate;
        }
        //****************************************************************************************************************************
    }
}
