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
        public static bool UpdatePhoto(ArrayList FieldsModified,
                                             DataSet Photo_ds,
                                             int photoID)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try 
            {
                if (UpdateWithDA(txn, Photo_ds.Tables[U.Photo_Table], U.Photo_Table, U.PhotoID_col, FieldsModified) &&
                    SavePicturedInfo(txn, Photo_ds, photoID, U.PhotoCategoryValue_Table))
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
            catch (HistoricJamaicaException e)
            {
                txn.Rollback();
                throw e;
            }
        }
        //****************************************************************************************************************************
        public static bool UpdatePhotoField(DataTable tbl,
                                      string sFieldToUpdate)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (UpdateWithDA(txn, tbl, U.Photo_Table, U.PhotoID_col, ColumnList(sFieldToUpdate)))
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
    }
}
