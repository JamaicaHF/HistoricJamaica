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
        public static int GetCategoryIDFromName(string sCategoryName)
        {
            DataTable tbl = new DataTable(U.Category_Table);
            SelectAll(U.Category_Table, tbl, new NameValuePair(U.CategoryName_col, sCategoryName));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CategoryID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static void GetAllCollections(DataTable tbl)
        {
            GetDistinctValues(tbl, U.Photo_Table, U.PhotoSource_col);
        }
        //****************************************************************************************************************************
        public static string GetCategoryName(int iCategoryID)
        {
            DataTable tbl = new DataTable(U.Category_Table);
            SelectAll(U.Category_Table, tbl, new NameValuePair(U.CategoryID_col, iCategoryID));
            if (tbl.Rows.Count == 0)     // does not exist
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CategoryName_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllCategoriesWithPhotos()
        {
            var selectStatement = "Select Distinct Category.CategoryID, Category.CategoryName from Category Join CategoryValue on Category.CategoryID = CategoryValue.CategoryId " +
                                  JoinPhotosWithCategoryValues();
            DataTable tbl = new DataTable(U.Category_Table);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = selectStatement;
            ExecuteSelectStatement(tbl, cmd);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllCategoryValuesWithPhotos()
        {
            var selectStatement = "Select distinct CategoryValue.CategoryValueID, CategoryValue.CategoryID, CategoryValue.CategoryValueValue, CategoryValue.CategoryValueOrder " +
                                  "from CategoryValue " + JoinPhotosWithCategoryValues();
            DataTable tbl = new DataTable(U.Category_Table);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = selectStatement;
            ExecuteSelectStatement(tbl, cmd);
            return tbl;
        }
        //****************************************************************************************************************************
        private static string JoinPhotosWithCategoryValues()
        {
            return "JOIN PhotoCategoryValue ON CategoryValue.CategoryValueID = PhotoCategoryValue.CategoryValueID";
        }
        //****************************************************************************************************************************
    }
}
