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
        public static bool CreateNewVitalRecord(DataTable VitalRecord_tbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            bool bSuccess = false;
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, VitalRecord_tbl, U.VitalRecord_Table, true);
                if (InsertWithDA(VitalRecord_tbl, insertCommand))
                {
                    bSuccess = UpdateVitalRecordsWithSpouseID(txn, VitalRecord_tbl);
                }
            }
            catch (HistoricJamaicaException e)
            {
                txn.Rollback();
                throw e;
            }
            if (bSuccess)
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
        private static bool UpdateVitalRecordsWithSpouseID(SqlTransaction txn,
                                                           DataTable VitalRecord_tbl)
        {
            int iVitalRecordID = VitalRecord_tbl.Rows[0][U.VitalRecordID_col].ToInt();
            if (VitalRecord_tbl.Rows.Count > 1)
            {
                int iSpouseVitalRecordID = VitalRecord_tbl.Rows[1][U.VitalRecordID_col].ToInt();
                VitalRecord_tbl.Rows[0][U.SpouseID_col] = iSpouseVitalRecordID;
                VitalRecord_tbl.Rows[1][U.SpouseID_col] = iVitalRecordID;
                return UpdateWithDA(txn, VitalRecord_tbl, U.VitalRecord_Table, U.VitalRecordID_col, ColumnList(U.SpouseID_col));
            }
            return true;
        }
        //****************************************************************************************************************************
        private static void ThrowVitalErrorException(ErrorCodes errorCode)
        {
            HistoricJamaicaException ex = new HistoricJamaicaException(errorCode);
            throw ex;
        }
        //****************************************************************************************************************************
    }
}
