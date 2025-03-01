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
        public static DataTable DefineCategoryTable()
        {
            DataTable tbl = new DataTable(U.Category_Table);
            tbl.Columns.Add(U.CategoryID_col, typeof(int));
            tbl.Columns.Add(U.CategoryName_col, typeof(string));
            tbl.Columns[U.CategoryName_col].MaxLength = U.iMaxValueLength;
            return tbl;
        }
        //****************************************************************************************************************************
    }
}
