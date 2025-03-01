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
        public static string GetCemeteryName(int iCemeteryID)
        {
            DataTable tbl = new DataTable(U.Cemetery_Table);
            SelectAll(U.Cemetery_Table, tbl, new NameValuePair(U.CemeteryID_col, iCemeteryID));
            if (tbl.Rows.Count == 0)     // does not exist
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CemeteryName_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllCemeteryRecords()
        {
            DataTable CemeteryRecord_tbl  = DefineCemeteryRecordTable();
            SelectAll(U.CemeteryRecord_Table, CemeteryRecord_tbl);
            return CemeteryRecord_tbl;
        }
        //****************************************************************************************************************************
        public static bool GetCemeteryRecord(DataTable CemeteryRecord_tbl,
                                             int iCemeteryRecordID)
        {
            return SelectAll(U.CemeteryRecord_Table, CemeteryRecord_tbl, new NameValuePair(U.CemeteryRecordID_col, iCemeteryRecordID));
        }
        //****************************************************************************************************************************
        public static bool GetCemeteryRecordLastNameFirstname(DataTable CemeteryRecord_tbl,
                                                              string personLastName,
                                                              string personFirstName)
        {
            return SelectAll(U.CemeteryRecord_Table, CemeteryRecord_tbl, new NameValuePair(U.LastName_col, personLastName), new NameValuePair(U.FirstName_col, personFirstName));
        }
        //****************************************************************************************************************************
        public static bool GetCemeteryRecordForPerson(DataTable CemeteryRecord_tbl,
                                             int iPersonID)
        {
            return SelectAll(U.CemeteryRecord_Table, CemeteryRecord_tbl, new NameValuePair(U.PersonID_col, iPersonID));
        }
        //****************************************************************************************************************************
        public static int GetCemeteryIDFromName(string sCemeteryName)
        {
            DataTable tbl = new DataTable(U.Cemetery_Table);
            SelectAll(U.Cemetery_Table, tbl, new NameValuePair(U.CemeteryName_col, sCemeteryName));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CemeteryID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static bool CemeteryRecordAlreadyExists(int iPersonId)
        {
            if (iPersonId == 0)
                return false;
            DataTable tbl = new DataTable();
            SelectAll(U.CemeteryRecord_Table, tbl, new NameValuePair(U.PersonID_col, iPersonId));
            return (tbl.Rows.Count != 0);
        }
        //****************************************************************************************************************************
    }
}
