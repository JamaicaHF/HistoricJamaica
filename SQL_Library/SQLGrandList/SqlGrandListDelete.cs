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
        public static bool DeleteModernRoadValueCommand(int iModernRoadValueID)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            int iNumDeleted = DeleteWithParms(txn, U.ModernRoadValue_Table, new NameValuePair(U.ModernRoadValueID_col, iModernRoadValueID));
            return (iNumDeleted > 0);
        }
        //****************************************************************************************************************************
    }
}
