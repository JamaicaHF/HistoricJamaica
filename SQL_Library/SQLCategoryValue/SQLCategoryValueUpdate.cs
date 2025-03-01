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
        public static bool UpdateAllCategoryValues(DataTable CategoryValue_tbl)
        {
            return UpdateWithDA(CategoryValue_tbl, U.CategoryValue_Table, U.CategoryValueID_col,
                                ColumnList(U.CategoryValueValue_col, U.CategoryValueOrder_col));
        }
        //****************************************************************************************************************************
    }
}
