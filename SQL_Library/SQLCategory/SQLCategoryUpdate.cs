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
        public static void UpdateCategoryName(int iCategoryID,
                                              string sCategoryName)
        {
            UpdateWithParms(U.Category_Table, new NameValuePair(U.CategoryID_col, iCategoryID),
                                              new NameValuePair(U.CategoryName_col, sCategoryName));
//            string UpdateString = "Update " + U.Category_Table + " Set " + ColumnEquals(U.CategoryName_col) +
//                                 " where " + ColumnEquals(U.CategoryID_col);
//            SqlCommand cmd = new SqlCommand(UpdateString, sqlConnection);
//            cmd.Parameters.Add(new SqlParameter("@" + U.CategoryID_col, iCategoryID));
//            cmd.Parameters.Add(new SqlParameter("@" + U.CategoryName_col, sCategoryName));
//            ExecuteUpdateStatement(cmd);
//            return true;
        }
        //****************************************************************************************************************************
    }
}
