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
        public static bool GetAllCategoryValues(DataTable tbl,
                                                int iCategoryID)
        {
            SelectAll(U.CategoryValue_Table, OrderBy(U.CategoryValueOrder_col), tbl, new NameValuePair(U.CategoryID_col, iCategoryID));
            return true;
        }
        //****************************************************************************************************************************
        public static bool GetCategoryValueRecords(DataTable tbl,
                                                   params int[] categoryValueIds)
        {
            int iPreviousRecordNumber = tbl.Rows.Count;
            string sOrder = " order by " + U.CategoryValueID_col;
            NameValuePair[] nameValuePairs = new NameValuePair[categoryValueIds.Length];
            for (int i = 0;i < categoryValueIds.Length; i++)
            {
                NameValuePair categoryValuePair = new NameValuePair(U.CategoryValueID_col, categoryValueIds[i]);
                nameValuePairs[i] = categoryValuePair;
            }
            SelectAllUsingOr(tbl, U.CategoryValue_Table, sOrder, nameValuePairs);
            return (tbl.Rows.Count != iPreviousRecordNumber);
        }
        //****************************************************************************************************************************
        public static int GetCategoryValueID(string sCategoryID,
                                             string sCategoryValueValue)
        {
            DataTable tbl = new DataTable(U.CategoryValue_Table);
            SelectAll(U.CategoryValue_Table, tbl, new NameValuePair(U.CategoryID_col, sCategoryID),
                                                  new NameValuePair(U.CategoryValueValue_col, sCategoryValueValue));
            if (tbl.Rows.Count == 0)
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CategoryValueID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static int GetCategoryValueOrder(int iCategoryID,
                                                string sCategoryValueValue)
        {
            DataTable tbl = new DataTable(U.CategoryValue_Table);
            SelectAll(U.CategoryValue_Table, tbl, new NameValuePair(U.CategoryID_col, iCategoryID),
                                                  new NameValuePair(U.CategoryValueValue_col, sCategoryValueValue));
            if (tbl.Rows.Count == 0)
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CategoryValueOrder_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static bool GetAllCategoryValuesAlphabetical(DataTable tbl,
                                                     int iCategoryID)
        {
            SelectAll(U.CategoryValue_Table, OrderBy(U.CategoryValueValue_col), tbl, new NameValuePair(U.CategoryID_col, iCategoryID));
            return true;
        }
        //****************************************************************************************************************************
        private static string CategoryValueSelectCommand(string PicturePerson_CategoryValueTable,
                                                  string PicturePerson_ID_col,
                                                  int personID)
        {
            string tableNames = PicturePerson_CategoryValueTable + "],[" + U.CategoryValue_Table;
            string[] selectColumns = { U.TableAndColumn(U.CategoryValue_Table, U.CategoryID_col),
                                       U.TableAndColumn(PicturePerson_CategoryValueTable, U.CategoryValueID_col),
                                       U.TableAndColumn(U.CategoryValue_Table, U.CategoryValueValue_col) };
            string[] orderByColumns = { U.TableAndColumn(U.CategoryValue_Table, U.CategoryID_col),
                                        U.TableAndColumn(U.CategoryValue_Table, U.CategoryValueOrder_col) };
            return SelectColumnAsString(selectColumns, orderByColumns, tableNames,
                             new NameValuePair(U.TableAndColumn(PicturePerson_CategoryValueTable, PicturePerson_ID_col), personID),
                             new NameValuePair(U.TableAndColumn(PicturePerson_CategoryValueTable, U.CategoryValueID_col),
                                               U.TableAndColumn(U.CategoryValue_Table, U.CategoryValueID_col)));
        }
        //****************************************************************************************************************************
        public static bool GetAllCategoryPhotosFromValue(DataTable PictureTBL,
                                                         int iCategoryValueID)
        {
            SelectAll(U.PhotoCategoryValue_Table, OrderBy(new string[] {U.PhotoID_col}), PictureTBL, new NameValuePair(U.CategoryValueID_col, iCategoryValueID));
            return true;
        }
        //****************************************************************************************************************************
        public static int GetCategoryId(int iCategoryValueID)
        {
            DataTable tbl = new DataTable(U.CategoryValue_Table);
            SelectAll(U.CategoryValue_Table, tbl, new NameValuePair(U.CategoryValueID_col, iCategoryValueID));
            if (tbl.Rows.Count == 0)
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CategoryID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static string GetCategoryValueValue(int iCategoryValueID)
        {
            DataTable tbl = new DataTable(U.CategoryValue_Table);
            SelectAll(U.CategoryValue_Table, tbl, new NameValuePair(U.CategoryValueID_col, iCategoryValueID));
            if (tbl.Rows.Count == 0)
                return "";
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CategoryValueValue_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static bool GetCategoryIDValue(DataTable tbl,
                                       int iCategoryValueID)
        {
            int iPreviousRecordNumber = tbl.Rows.Count;
            SelectAll(U.CategoryValue_Table, tbl, new NameValuePair(U.CategoryValueID_col, iCategoryValueID));
            return (tbl.Rows.Count != iPreviousRecordNumber);
        }
        //****************************************************************************************************************************
        public static DataTable GetCategoryIDValue(int iCategoryValueID)
        {
            DataTable tbl = new DataTable(U.CategoryValue_Table);
            SelectAll(U.CategoryValue_Table, tbl, new NameValuePair(U.CategoryValueID_col, iCategoryValueID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static bool GetCategoryIDValue(DataTable tbl,
                                       string sCategoryID,
                                       string sCategoryValueValue)
        {
            SelectAll(U.CategoryValue_Table, tbl, new NameValuePair(U.CategoryID_col, sCategoryID),
                                                  new NameValuePair(U.CategoryValueValue_col, sCategoryValueValue));
            return true;
        }
        //****************************************************************************************************************************
    }
}
