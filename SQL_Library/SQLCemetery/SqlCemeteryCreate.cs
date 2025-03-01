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
        public static void CreateNewCemeteryRecordRecords(DataTable CemeteryRecord_tbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            bool bSuccess = false;
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, CemeteryRecord_tbl, U.CemeteryRecord_Table, true);
                bSuccess = InsertWithDA(CemeteryRecord_tbl, insertCommand);
            }
            catch (HistoricJamaicaException e)
            {
                txn.Rollback();
                throw e;
            }
            if (bSuccess)
            {
                txn.Commit();
            }
            else
            {
                txn.Rollback();
                throw new Exception("Unable to Create Cemetery Records");
            }
        }
        //****************************************************************************************************************************
        public static void CreateNewPersonCWRecords(DataTable personCW_tbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            bool bSuccess = false;
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, personCW_tbl, U.PersonCW_Table, true);
                bSuccess = InsertWithDA(personCW_tbl, insertCommand);
            }
            catch (HistoricJamaicaException e)
            {
                txn.Rollback();
                throw e;
            }
            if (bSuccess)
            {
                txn.Commit();
            }
            else
            {
                txn.Rollback();
                throw new Exception("Unable to Create PersonCW Records");
            }
        }
        //****************************************************************************************************************************
        public static bool InsertCemeteryTable(SqlTransaction txn,
                                       DataTable tbl)
        {
            SqlCommand insertCommand = InsertCommand(txn, tbl, U.CemeteryRecord_Table, true);
            return InsertWithDA(tbl, insertCommand);
        }
        //****************************************************************************************************************************
        public static bool InsertCemeteryRecordsWithUpdatedPersonID(SqlTransaction txn,
                                                            DataTable Person_tbl,
                                                            DataTable Cemetery_tbl)
        {
            if (Cemetery_tbl.Rows.Count == 0)
                return true;
            Person_tbl.PrimaryKey = new DataColumn[] { Person_tbl.Columns[U.ImportPersonID_col] };
            foreach (DataRow row in Cemetery_tbl.Rows)
            {
                int iPersonID = row[U.PersonID_col].ToInt();
                row[U.PersonID_col] = GetPersonIDFromImportPersonIDFromTable(Person_tbl, iPersonID);
            }
            return (InsertCemeteryTable(txn, Cemetery_tbl));
        }
        //****************************************************************************************************************************
    }
}
