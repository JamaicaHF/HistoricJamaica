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
        public static int InsertCategory(string sCategoryID)
        {
            DataTable tbl = DefineCategoryTable();
            tbl.Rows.Add(0, sCategoryID);
            SqlCommand insertCommand = InsertCommand(tbl, U.Category_Table, true);
            if (InsertWithDA(tbl, insertCommand))
                return tbl.Rows[0][U.CategoryID_col].ToInt();
            else
                return 0;
        }
        //****************************************************************************************************************************
    }
}
