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
        public static bool DeleteCategoryValue(int iCategoryValueID)
        {
            int iCategoryID = GetCategoryIDFromCategoryValueID(iCategoryValueID);
            DataTable tbl = DefineCategoryValueTable(U.CategoryValue_Table);
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.CategoryValueID_col] };
            GetAllCategoryValues(tbl, iCategoryID);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            SqlCommand updateCommand = null;
            if (ThereAreCategoryRowsToUpdateOtherThanDeletedRow(tbl, iCategoryValueID))
            {
                updateCommand = CategoryUpdateCommand(txn, tbl);
            }
            SqlCommand deleteCommand = DeleteCommandWithDA(txn, U.CategoryValue_Table, U.CategoryValueID_col);
            if (DeleteUpdateWithDA(tbl, deleteCommand, updateCommand))
            {
                txn.Commit();
                return true;
            }
            else
            {
                txn.Rollback();
                return false;
            }
        }
        //****************************************************************************************************************************
        public static int GetCategoryIDFromCategoryValueID(int iCategoryValueID)
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
        private static bool ThereAreCategoryRowsToUpdateOtherThanDeletedRow(DataTable tbl,
                                                             int iCategoryValueID)
        {
            bool bDoUpdate = false;
            int iOrderNumber = 0;
            int iDeletedRowCategoryValueID = 99999999;
            foreach (DataRow row in tbl.Rows)
            {
                iOrderNumber++;
                int iOldCategoryValueID = row[U.CategoryValueID_col].ToInt();
                if (iOldCategoryValueID == iCategoryValueID)
                {
                    iDeletedRowCategoryValueID = iOrderNumber;
                    row.Delete();
                    iOrderNumber--;
                }
                else if (iOrderNumber >= iDeletedRowCategoryValueID)
                {
                    row[U.CategoryValueOrder_col] = iOrderNumber;
                    bDoUpdate = true;
                }
                else
                {
                    int iOldCategoryValueOrder = row[U.CategoryValueOrder_col].ToInt();
                    if (iOldCategoryValueOrder != iOrderNumber)
                    {
                        row[U.CategoryValueOrder_col] = iOrderNumber;
                        bDoUpdate = true;
                    }
                }
            }
            return bDoUpdate;
        }
    }
}
