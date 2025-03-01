using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    //****************************************************************************************************************************
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static void UpdateVitalRecordSex()
        {
            DataTable VitalRecords_tbl = new DataTable();
            SelectAll(U.VitalRecord_Table, VitalRecords_tbl, new NameValuePair(U.Sex_col, " = ''"));
            if (VitalRecords_tbl.Rows.Count != 0)
            {
                SetSexBasedOnType(VitalRecords_tbl);
                UpdateWithDA(VitalRecords_tbl, U.VitalRecord_Table, U.VitalRecordID_col, ColumnList(U.VitalRecordType_col));

            }
        }
        //****************************************************************************************************************************
        public static void SetSexBasedOnType(DataTable tbl)
        {
            foreach (DataRow row in tbl.Rows)
            {
                string sSex = row[U.Sex_col].ToString();
                if (sSex.Length == 0 || sSex[0] == ' ')
                {
                    row[U.Sex_col] = U.RecordTypeSexChar((EVitalRecordType)row[U.VitalRecordType_col].ToInt(), ' ');
                }
            }
        }
        //****************************************************************************************************************************
    }
}
