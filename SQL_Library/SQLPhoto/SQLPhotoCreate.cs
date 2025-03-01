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
        public static bool CreateNewPhoto(DataSet Photo_ds)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, Photo_ds.Tables[U.Photo_Table], U.Photo_Table, true);
                if (InsertWithDA(Photo_ds.Tables[U.Photo_Table], insertCommand))
                {
                    int iPhotoID = PhotoIDFromDataSet(Photo_ds);
                    AddPhotoIDToPicturedInfo(Photo_ds.Tables[U.PicturedPerson_Table], iPhotoID);
                    AddPhotoIDToPicturedInfo(Photo_ds.Tables[U.PicturedBuilding_Table], iPhotoID);
                    if (SavePicturedInfo(txn, Photo_ds, iPhotoID, U.PhotoCategoryValue_Table))
                    {
                        ChangePhotoIDFromZero(Photo_ds.Tables[U.PicturedPerson_Table], iPhotoID);
                        ChangePhotoIDFromZero(Photo_ds.Tables[U.PicturedBuilding_Table], iPhotoID);
                        txn.Commit();
                        return true;
                    }
                }
                txn.Rollback();
                return false;
            }
            catch (HistoricJamaicaException e)
            {
                txn.Rollback();
                throw e;
            }
        }
        //****************************************************************************************************************************
        private static void AddPhotoIDToPicturedInfo(DataTable tbl,
                                              int iPhotoID)
        {
            foreach (DataRow row in tbl.Rows)
            {
                row[U.PhotoID_col] = iPhotoID;
            }
        }
        //****************************************************************************************************************************
        public static int PhotoIDFromDataSet(DataSet Photo_ds)
        {
            return Photo_ds.Tables[U.Photo_Table].Rows[0][U.PhotoID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static bool AddAllNewPhotos(DataTable Photo_tbl,
                                    ArrayList PhotosToDelete,
                                    ArrayList PhotosCopied,
                                    string sFolder,
                                    string sPrefix,
                                    int iStartPhotoNumber,
                                    int iEndPhotoNumber,
                                    string sSource)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            SqlCommand  insertCommand = InsertCommand(txn, Photo_tbl, U.Photo_Table, true);
            if (InsertWithDA(Photo_tbl, insertCommand))
            {
                if (PhotosToDelete.Count != 0)
                    DeletePhotos(PhotosToDelete);
                txn.Commit();
                return true;
            }
            else
            {
                if (PhotosCopied.Count != 0)
                    DeletePhotos(PhotosCopied);
                txn.Rollback();
                return false;
            }
        }
        //****************************************************************************************************************************
    }
}
