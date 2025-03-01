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
        public static bool DeleteVitalRecordFromDatabase(int iVitalRecordID,
                                                         int iSpouseVitalRecordID)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (DeleteWithParms(txn, U.VitalRecord_Table, new NameValuePair(U.VitalRecordID_col, iVitalRecordID)) == U.Exception ||
                DeleteWithParms(txn, U.VitalRecord_Table, new NameValuePair(U.VitalRecordID_col, iSpouseVitalRecordID)) == U.Exception)
            {
                txn.Rollback();
            }
            else
                txn.Commit();
            return true;
        }
    }
}
