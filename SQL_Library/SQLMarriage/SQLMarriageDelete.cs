using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {
        //****************************************************************************************************************************
        public static bool RemoveMarriagesWithoutAssociatedPersonRecords()
        {
            DataTable MarriageTable = new DataTable();
            SelectAll(U.Marriage_Table, MarriageTable);
            foreach (DataRow Marriage_row in MarriageTable.Rows)
            {
                int iPersonID = Marriage_row[U.PersonID_col].ToInt();
                int iSpouseID = Marriage_row[U.SpouseID_col].ToInt();
                DataRow Person_row = GetPerson(iPersonID);
                DataRow Spouse_row = GetPerson(iSpouseID);
                if (Person_row == null || Spouse_row == null)
                    Marriage_row.Delete();
            }
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (DeleteMarriages(txn, MarriageTable))
                txn.Commit();
            else
                txn.Rollback();
            return true;
        }
        //****************************************************************************************************************************
        public static bool DeleteThisMarriage(int iPersonID,
                                        int iSpouseID)
        {
            if (iPersonID != 0 && iSpouseID != 0)
            {
                DeleteMarriage(iPersonID, iSpouseID);
                DeleteMarriage(iSpouseID, iPersonID);
            }
            return true;
        }
        //****************************************************************************************************************************
        public static void DeleteMarriage(int iPersonID,
                                   int iSouseID)
        {
            DeleteWithParms(U.Marriage_Table, new NameValuePair(U.PersonID_col, iPersonID), new NameValuePair(U.SpouseID_col, iSouseID));
        }
        //****************************************************************************************************************************
        private static bool DeleteMarriages(SqlTransaction txn,
                                    DataTable Marriage_tbl)
        {
            SqlCommand DeleteCommand = DeleteCommandWithDA(txn, U.Marriage_Table, U.PersonID_col, U.SpouseID_col);
            return DeleteWithDA(Marriage_tbl, DeleteCommand);
        }
        //****************************************************************************************************************************
    }
}
