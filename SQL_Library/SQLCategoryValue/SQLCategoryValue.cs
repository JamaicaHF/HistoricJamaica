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
        public static DataTable DefineCategoryValueTable(string sCategoryValueTable)
        {
            DataTable tbl = new DataTable(sCategoryValueTable);
            tbl.Columns.Add(U.CategoryValueID_col, typeof(int));
            tbl.Columns.Add(U.CategoryID_col, typeof(int));
            tbl.Columns.Add(U.CategoryValueValue_col, typeof(string));
            tbl.Columns.Add(U.CategoryValueOrder_col, typeof(int));
            tbl.Columns[U.CategoryValueValue_col].MaxLength = U.iMaxValueLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefinePersonPhotoCategoryValueTable(string categoryValueTable,
                                                                    string tableID_col)
        {
            DataTable tbl = new DataTable(categoryValueTable);
            tbl.Columns.Add(tableID_col, typeof(int));
            tbl.Columns.Add(U.CategoryValueID_col, typeof(int));
            return tbl;
        }
        //****************************************************************************************************************************
        private static SqlCommand CategoryInsertCommand(SqlTransaction txn,
                                                        string sCategoryValueTableName,
                                                        string sTableID_col,
                                                        int iTableID)
        {
            DataTable CategoryTable = DefinePersonPhotoCategoryValueTable(sCategoryValueTableName, sTableID_col);
            return InsertCommand(txn, CategoryTable, sCategoryValueTableName, new NameValuePair(sTableID_col, iTableID), false);
        }
        //****************************************************************************************************************************
        public static bool DeleteInsertCategoryValueDataRows(SqlTransaction txn,
                                                      DataTable CategoryValueTBL,
                                                      string sCategoryValueTableName,
                                                      string sTableID,
                                                      int iTableID)
        {   // Even though the CategoryTable includes CategoryValue records, it is the PersonCategoryValueTable or the
            // Photo Category Value table being inserted into or deleted from.
            int numAdded = CategoryValueTBL.NumStateRecordsInTable(DataViewRowState.Added);
            int numDeleted = CategoryValueTBL.NumStateRecordsInTable(DataViewRowState.Deleted);
            if (numAdded == 0 && numDeleted == 0)
            {
                return true;
            }
            //SqlCommand deleteCommand = DeleteCommandWithDA(txn, sCategoryValueTableName, sTableID, U.CategoryValueID_col);
            SqlCommand deleteCommand = DeleteCommandWithDAOneParm(txn, sCategoryValueTableName, new NameValuePair(sTableID, iTableID), U.CategoryValueID_col);
            SqlCommand insertCommand = CategoryInsertCommand(txn, sCategoryValueTableName, sTableID, iTableID);
            return DeleteInsertWithDA(CategoryValueTBL, deleteCommand, insertCommand);
        }
        //****************************************************************************************************************************
    }
}
