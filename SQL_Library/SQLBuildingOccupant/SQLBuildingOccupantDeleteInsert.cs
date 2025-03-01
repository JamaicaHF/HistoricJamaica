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
        public static bool DeleteInsertBuildingOccupantDataRows(SqlTransaction txn,
                                                             DataTable buildingTbl,
                                                             int iPersonID)
        {
            int numAdded = buildingTbl.NumStateRecordsInTable(DataViewRowState.Added);
            int numDeleted = buildingTbl.NumStateRecordsInTable(DataViewRowState.Deleted);
            if (numAdded == 0 && numDeleted == 0)
            {
                return true;
            }
            SqlCommand deleteCommand = DeleteCommandWithDAOneParm(txn, U.BuildingOccupant_Table, new NameValuePair(U.PersonID_col, iPersonID), U.BuildingID_col);
            SqlCommand insertCommand = BuildingOccupantInsertCommand(txn, iPersonID);
            return DeleteInsertWithDA(buildingTbl, deleteCommand, insertCommand);
        }
        //****************************************************************************************************************************
        private static SqlCommand BuildingOccupantInsertCommand(SqlTransaction txn,
                                                                int iPersonID)
        {
            DataTable personBuildingTable = DefineBuildingOccupantTable();
            return InsertCommand(txn, personBuildingTable, U.BuildingOccupant_Table, new NameValuePair(U.PersonID_col, iPersonID), false);
        }
        //****************************************************************************************************************************
        public static bool InsertBuildingOccupant(int iPersonID,
                                         int iSpouseLivedWithID,
                                         int iBuildingID,
                                         int iBuildingValueOrder,
                                         string buildingNotes,
                                         Int64 CensusYears)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            bool bSuccess = InsertBuildingOccupant(iPersonID, iSpouseLivedWithID, iBuildingID, iBuildingValueOrder,
                                                      buildingNotes, CensusYears, txn);
            if (bSuccess)
                txn.Commit();
            else
                txn.Rollback();
            return bSuccess;
        }
        //****************************************************************************************************************************
        public static bool InsertBuildingOccupant(int iPersonID,
                                         int iSpouseLivedWithID,
                                         int iBuildingID,
                                         int iBuildingValueOrder,
                                         string buildingNotes,
                                         Int64 CensusYears,
                                         SqlTransaction txn)
        {
            DataTable tbl = DefineBuildingOccupantTable();
            DataRow Building_row = tbl.NewRow();
            Building_row[U.PersonID_col] = iPersonID;
            Building_row[U.BuildingID_col] = iBuildingID;
            Building_row[U.SpouseLivedWithID_col] = iSpouseLivedWithID;
            Building_row[U.Notes_col] = "";
            Building_row[U.CensusYears_col] = 0;
            Building_row[U.BuildingValueOrder_col] = iBuildingValueOrder;
            Building_row[U.Notes_col] = buildingNotes;
            Building_row[U.CensusYears_col] = CensusYears;
            tbl.Rows.Add(Building_row);
            SqlCommand insertCommand = InsertCommand(txn, tbl, U.BuildingOccupant_Table, false);
            return InsertWithDA(tbl, insertCommand);
        }
        //****************************************************************************************************************************
    }
}
