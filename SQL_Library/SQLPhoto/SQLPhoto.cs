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
        public static DataTable DefinePhotoTable()
        {
            DataTable tbl = new DataTable(U.Photo_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PhotoID_col, typeof(int));
            tbl.Columns.Add(U.PhotoName_col, typeof(string));
            tbl.Columns.Add(U.PhotoExtension_col, typeof(string));
            tbl.Columns.Add(U.PhotoNotes_col, typeof(string));
            tbl.Columns.Add(U.PhotoYear_col, typeof(int));
            tbl.Columns.Add(U.PhotoSource_col, typeof(string));
            tbl.Columns.Add(U.PhotoDrawer_col, typeof(int));
            tbl.Columns.Add(U.PhotoFolder_col, typeof(int));
            tbl.Columns.Add(U.NumPicturedPersons_col, typeof(int));
            tbl.Columns.Add(U.NumPicturedBuildings_col, typeof(int));
            tbl.Columns[U.PhotoName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.PhotoNotes_col].MaxLength = U.iMaxDescriptionLength;
            tbl.Columns[U.PhotoSource_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.PhotoExtension_col].MaxLength = 4;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefineSlideShowTable()
        {
            DataTable tbl = new DataTable(U.SlideShow_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PhotoID_col, typeof(int));
            tbl.Columns.Add(U.PhotoSequence_col, typeof(int));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefinePhotoCategoryValue()
        {
            DataTable tbl = new DataTable(U.PhotoCategoryValue_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PhotoID_col, typeof(int));
            tbl.Columns.Add(U.CategoryValueID_col, typeof(int));
            return tbl;
        }
        //****************************************************************************************************************************
        public static bool DefinePhoto(DataSet Photo_ds)
        {
            Photo_ds.Tables.Add(DefinePhotoTable());
            DataTable CV_tbl = DefineCategoryValueTable(U.PhotoCategoryValue_Table);
            CV_tbl.PrimaryKey = new DataColumn[] { CV_tbl.Columns[U.CategoryValueID_col] };
            Photo_ds.Tables.Add(CV_tbl);
            Photo_ds.Tables.Add(DefinePicturedPersonForPhoto());
            Photo_ds.Tables.Add(DefinePicturedBuilding());
            return true;
        }
        //****************************************************************************************************************************
        public static string TableYear(DataRow row,
                                string sTableYear)
        {
            int iYear = row[sTableYear].ToInt();
            if (iYear == 0)
                return "";
            else
                return iYear.ToString();
        }
        //****************************************************************************************************************************
        public static string TifFolder()
        {
            return m_sDataDirectory + "\\HF TIFs\\";
        }
        //****************************************************************************************************************************
        public static string JpgFolder()
        {
            return m_sDataDirectory + "\\HF Jpegs\\";
        }
        //****************************************************************************************************************************
        public static string MapFileName(DataRow MapRow)
        {
            string sMapFolder = m_sDataDirectory + "\\Maps\\";
            return GetPhotoFilename(MapRow, sMapFolder, U.Map_Table);
        }
        //****************************************************************************************************************************
        public static string GetPhotoFilename(DataRow row,
                                         string sFolder,
                                         string sTable)
        {
            string sTableName = sTable + "Name";
            string sTableExtension = sTable + "Extension";
            return U.FileNameWithExtension(row[sTableName].ToString(), sFolder, U.sJPGExtension);
        }
        //***************************************************************************************************************************
        private static bool SaveAllPicturedValues(SqlTransaction txn,
                                            DataTable PicturedValue_tbl,
                                            DataTable pictured_tbl,
                                            string picturedTable,
                                            string picturedNumber_col,
                                            string picturedID_col)
        {
            if (NoRecordsToSave(PicturedValue_tbl))
                return true;
            ArrayList fieldsModified = ColumnList(picturedID_col, picturedNumber_col);
            SqlCommand updateCommand = UpdateCommand(txn, PicturedValue_tbl.Columns, picturedTable, U.PhotoID_col, fieldsModified);
            SqlCommand insertCommand = InsertCommand(txn, pictured_tbl, picturedTable, false);
            SqlCommand deleteCommand = DeleteCommandWithDA(txn, picturedTable, picturedNumber_col, U.PhotoID_col);
            return DeleteUpdateInsertWithDA(PicturedValue_tbl, deleteCommand, updateCommand, insertCommand);
        }
        //****************************************************************************************************************************
        private static bool NoRecordsToSave(DataTable PicturedValue_tbl)
        {
            if (PicturedValue_tbl == null || PicturedValue_tbl.Rows.Count == 0)
                return true;
            int numAdded = PicturedValue_tbl.NumStateRecordsInTable(DataViewRowState.Added);
            int numUpdated = PicturedValue_tbl.NumStateRecordsInTable(DataViewRowState.ModifiedCurrent);
            int numDeleted = PicturedValue_tbl.NumStateRecordsInTable(DataViewRowState.Deleted);
            if (numAdded == 0 && numDeleted == 0 && numUpdated == 0)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        public static bool SavePicturedInfo(SqlTransaction txn,
                                            DataSet Photo_ds,
                                            int iPhotoID,
                                 string sCategoryValueTableName)
        {
            DataTable picturedPerson_tbl = DefinePicturedPerson();
            bool bSuccess = SaveAllPicturedValues(txn, Photo_ds.Tables[U.PicturedPerson_Table], picturedPerson_tbl,
                                                   U.PicturedPerson_Table, U.PicturedPersonNumber_col, U.PersonID_col);
            DataTable picturedBuilding_tbl = DefinePicturedBuilding();
            if (bSuccess) bSuccess = SaveAllPicturedValues(txn, Photo_ds.Tables[U.PicturedBuilding_Table], picturedBuilding_tbl,
                                                   U.PicturedBuilding_Table, U.PicturedBuildingNumber_col, U.BuildingID_col);
            if (bSuccess) bSuccess = DeleteInsertCategoryValueDataRows(txn, Photo_ds.Tables[sCategoryValueTableName],
                                                                       sCategoryValueTableName, U.PhotoID_col, iPhotoID);
            return bSuccess;
        }
        //****************************************************************************************************************************
        private static void ChangePhotoIDFromZero(DataTable tbl,
                                           int iPhotoID)
        {
            DataViewRowState dvRowState = DataViewRowState.Added | DataViewRowState.Deleted;
            foreach (DataRow row in tbl.Select("", "", dvRowState))
            {
                if (row[U.PhotoID_col].ToInt() == 0)
                    row[U.PhotoID_col] = iPhotoID;
            }
        }
        //****************************************************************************************************************************
        public static bool RemoveJPGExtension()
        {
            DataTable tbl = new DataTable(U.Photo_Table);
            SelectAll(U.Photo_Table, tbl);
            if (tbl.Rows.Count == 0)     // does not exist
                return false;
            else
            {
                foreach (DataRow row in tbl.Rows)
                {
                    string sPhotoName = row[U.PhotoName_col].ToString();
                    if (sPhotoName.Substring(0, 2) == "HF" && sPhotoName.Length > 8)
                    {
                        row[U.PhotoName_col] = sPhotoName.Remove(8);
                    }
                }
                UpdatePhotoField(tbl, U.PhotoName_col);
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
