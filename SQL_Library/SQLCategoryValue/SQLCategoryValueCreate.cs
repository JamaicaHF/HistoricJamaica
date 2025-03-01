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
        public static int InsertCategoryValue(int iCategoryID,
                                              string sCategoryIDValue,
                                              int iCategoryValueOrder)
        {
            DataTable tbl = DefineCategoryValueTable(U.CategoryValue_Table);
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.CategoryValueID_col] };
            GetAllCategoryValues(tbl, iCategoryID);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            SqlCommand updateCommand = null;
            if (ThereAreCategoryRowsToUpdate(tbl, iCategoryID, iCategoryValueOrder))
            {
                updateCommand = CategoryUpdateCommand(txn, tbl);
            }
            tbl.Rows.Add(0, iCategoryID, sCategoryIDValue, iCategoryValueOrder);
            SqlCommand insertCommand = InsertCommand(txn, tbl, U.CategoryValue_Table, true);
            if (UpdateInsertWithDA(tbl, updateCommand, insertCommand))
            {
                txn.Commit();
                int iAddedValueIndex = tbl.Rows.Count;
                int i = tbl.Rows[iAddedValueIndex - 1][U.CategoryValueID_col].ToInt();
                return tbl.Rows[iAddedValueIndex - 1][U.CategoryValueID_col].ToInt();
            }
            else
            {
                txn.Rollback();
                return 0;
            }
        }
        public static void MovePeoplePhotos()
        {
            DataTable photoCategoryValueTbl = DefinePhotoCategoryValue();
            DataTable slideShowTbl = GetSlideShow();
            DataSet photo_ds = new DataSet();
            foreach (DataRow slideShowRow in slideShowTbl.Rows)
            {
                int iPhotoID = slideShowRow[U.PhotoID_col].ToInt();
                SQL.GetPhoto(ref photo_ds, iPhotoID);
                if (IncludePhotoInCategory(photo_ds))
                {
                    DataRow categoryValueRow = photoCategoryValueTbl.NewRow();
                    categoryValueRow[U.PhotoID_col] = iPhotoID;
                    categoryValueRow[U.CategoryValueID_col] = 300;
                    photoCategoryValueTbl.Rows.Add(categoryValueRow);
                }
                photo_ds.Clear();
            }
            InsertPhotoCategoryValues(photoCategoryValueTbl);
        }
        private static bool IncludePhotoInCategory(DataSet photo_ds)
        {
            if (photo_ds.Tables[U.Photo_Table].Rows.Count == 0)
            {
                return false;
            }
            if (photo_ds.Tables[U.PicturedPerson_Table].Rows.Count == 0)
            {
                return false;
            }
            if (photo_ds.Tables[U.PhotoCategoryValue_Table].Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private static void InsertPhotoCategoryValues(DataTable photoCategoryValueTbl)
        {
            SqlCommand insertCommand = InsertCommand(photoCategoryValueTbl, U.PhotoCategoryValue_Table, false);
            InsertWithDA(photoCategoryValueTbl, insertCommand);
        }
        //****************************************************************************************************************************
        private static string CategoryValueInsertString(string sParmChar)
        {
            return "(" +
                    sParmChar + U.CategoryID_col + U.comma +
                    sParmChar + U.CategoryValueValue_col + U.comma +
                    sParmChar + U.CategoryValueOrder_col +
                    ")";
        }
        //****************************************************************************************************************************
        private static bool ThereAreCategoryRowsToUpdate(DataTable tbl,
                                                         int iCategoryID,
                                                         int iCategoryValueOrder)
        {
            bool bDoUpdate = false;
            int iOrderNumber = 0;
            foreach (DataRow row in tbl.Rows)
            {
                iOrderNumber++;
                int iOldCategoryValueOrder = row[U.CategoryValueOrder_col].ToInt();
                if (iOrderNumber >= iCategoryValueOrder)
                {
                    row[U.CategoryValueOrder_col] = iOrderNumber + 1;
                    bDoUpdate = true;
                }
                else if (iOldCategoryValueOrder != iOrderNumber)
                {
                    row[U.CategoryValueOrder_col] = iOrderNumber;
                    bDoUpdate = true;
                }
            }
            return bDoUpdate;
        }
        //****************************************************************************************************************************
        private static SqlCommand CategoryUpdateCommand(SqlTransaction txn,
                                                        DataTable tbl)
        {
            return UpdateCommand(txn, tbl.Columns, U.CategoryValue_Table, U.CategoryValueID_col, ColumnList(U.CategoryValueOrder_col));
//            SqlCommand Update_cmd = new SqlCommand();
//            string sUpdateString = "";
//            string sWhereString = " where " + ColumnEquals(U.CategoryValueID_col);
//            SqlParameterCollection Parms = Update_cmd.Parameters;
//            Parms.Add(U.CategoryValueID_col, SqlDbType.Int, 0, U.CategoryValueID_col);
//            AddUpdateValue(ref sUpdateString, ref sWhereString, ref Parms, U.CategoryValueOrder_col, SqlDbType.Int, 0);
//            Update_cmd.CommandText = "Update [" + U.CategoryValue_Table + "] " + sUpdateString + sWhereString + ";";
//            updateCommand.Connection = sqlConnection;
//            updateCommand.Transaction = txn;
//            return Update_cmd;
        }
    }
}
