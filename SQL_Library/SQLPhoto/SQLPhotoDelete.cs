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
        private static void DeletePhotos(ArrayList PhotosToDelete)
        {
            foreach (string sPhoto in PhotosToDelete)
            {
                U.DeleteFile(sPhoto);
            }
        }
        //****************************************************************************************************************************
        public static bool DeletePhoto(int iPhotoID)
        {
            bool bSuccess = false;
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (DeleteWithParms(txn, U.Photo_Table, new NameValuePair(U.PhotoID_col, iPhotoID)) != U.Exception &&
                DeleteWithParms(txn, U.PhotoCategoryValue_Table, new NameValuePair(U.PhotoID_col, iPhotoID)) != U.Exception &&
                DeleteWithParms(txn, U.PicturedBuilding_Table, new NameValuePair(U.PhotoID_col, iPhotoID)) != U.Exception &&
                DeleteWithParms(txn, U.PicturedPerson_Table, new NameValuePair(U.PhotoID_col, iPhotoID)) != U.Exception)
            {
                bSuccess = true;
            }
            if (bSuccess)
                txn.Commit();
            else
                txn.Rollback();
            return true;
        }
        //****************************************************************************************************************************
    }
}
