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
        public static bool GetPhotosFromCollection(DataTable tbl,
                                            string sPhotoSource)
        {
            SelectAll(U.Photo_Table, tbl, new NameValuePair(U.PhotoSource_col, sPhotoSource));
            return true;
        }
        //****************************************************************************************************************************
        public static void selectdistinctPhotoID(DataTable tbl, 
                                                 string tablename, 
                                                 int personID)
        {
            if (personID == 0)
            {
                GetDistinctValues(tbl, tablename, U.PhotoID_col);
            }
            else
            {
                GetDistinctValues(tbl, tablename, U.PhotoID_col, new NameValuePair(U.PersonID_col, personID));
            }
        }
        //****************************************************************************************************************************
        public static int GetPhotoID(string sPhotoName)
        {
            DataTable tbl = new DataTable(U.Photo_Table);
            SelectAll(U.Photo_Table, tbl, new NameValuePair(U.PhotoName_col, sPhotoName));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
                return tbl.Rows[0][U.PhotoID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPhotos()
        {
            DataTable Photo_tbl = new DataTable();
            SelectAll(U.Photo_Table, OrderBy(U.PhotoName_col), Photo_tbl);
            return Photo_tbl;
        }
        //****************************************************************************************************************************
        public static void UpdateSlideShow(DataTable slideShowTbl)
        {
            UpdateWithDA(slideShowTbl, U.SlideShow_Table, U.PhotoID_col, new ArrayList(new string[] { U.PhotoSequence_col }));
        }
        //****************************************************************************************************************************
        public static void DeleteFromSlideShow(int iPhotoId)
        {
            DeleteWithParms(U.SlideShow_Table, new NameValuePair(U.PhotoID_col, iPhotoId));
        }
        //****************************************************************************************************************************
        public static DataTable GetSlideShow()
        {
            DataTable SlideShow_tbl = new DataTable();
            SelectAll(U.SlideShow_Table, OrderBy(U.PhotoSequence_col), SlideShow_tbl);
            return SlideShow_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetSlideShow(int photoId)
        {
            DataTable SlideShow_tbl = new DataTable();
            SelectAll(U.SlideShow_Table, OrderBy(U.PhotoSequence_col), SlideShow_tbl, new NameValuePair(U.PhotoID_col, photoId));
            return SlideShow_tbl;
        }
        //****************************************************************************************************************************
        public static void AddPhotoToSlideShow(int photoId)
        {
            bool found = false;
            int highestSequenceNumber = 0;
            DataTable slideShowTbl = GetSlideShow();
            foreach (DataRow slideShowRow in slideShowTbl.Rows)
            {
                if (slideShowRow[U.PhotoID_col].ToInt() == photoId)
                {
                    found = true;
                }
                if (slideShowRow[U.PhotoSequence_col].ToInt() > highestSequenceNumber)
                {
                    highestSequenceNumber = slideShowRow[U.PhotoSequence_col].ToInt();
                }
            }
            if (!found)
            {
                DataRow row = slideShowTbl.NewRow();
                row[U.PhotoID_col] = photoId;
                row[U.PhotoSequence_col] = highestSequenceNumber + 1;
                slideShowTbl.Rows.Add(row);
                SqlCommand insertCommand = SQL.InsertCommand(null, slideShowTbl, U.SlideShow_Table, false);
                SQL.InsertWithDA(slideShowTbl, insertCommand);
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPhotoCategoryValues()
        {
            DataTable PhotoCategoryValue_tbl = new DataTable();
            SelectAll(U.PhotoCategoryValue_Table, OrderBy(U.PhotoID_col), PhotoCategoryValue_tbl);
            return PhotoCategoryValue_tbl;
        }
        //****************************************************************************************************************************
        public static bool PhotoNameExists(string sPhotoName)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.Photo_Table, tbl, new NameValuePair(U.PhotoName_col, sPhotoName));
            return tbl.Rows.Count != 0;
        }
        //****************************************************************************************************************************
        public static string GetPhotoName(string sPhotoName)
        {
            DataTable tbl = new DataTable(U.Photo_Table);
            SelectAll(U.Photo_Table, tbl, new NameValuePair(U.PhotoName_col, sPhotoName));
            if (tbl.Rows.Count == 0)     // does not exist
                return "";
            else
                return tbl.Rows[0][U.PhotoName_col].ToString();
        }
        //****************************************************************************************************************************
        public static string GetNextPhotoName()
        {
            DataTable tbl = new DataTable();
            if (SelectTopOneLike(tbl, U.Photo_Table, U.PhotoName_col, "HF0", "Desc"))
            {
                return U.AddOneToPhotoName(tbl.Rows[0][U.PhotoName_col].ToString());
            }
            return "HF000";
        }
        //****************************************************************************************************************************
        public static bool GetPhotosFromBuildingID(DataTable tbl,
                                              int iBuildingID)
        {
            SelectAll(U.PicturedBuilding_Table, tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
            return true;
        }
        //****************************************************************************************************************************
        public static bool GetPhoto(ref DataSet Photo_ds,
                                   int iPhotoID)
        {
            Photo_ds.Clear();
            string sPhotoSelect_cmd = SelectAllString(U.Photo_Table, U.NoOrderBy, U.PhotoID_col);
            string sPeopleSelectcmd = PicturedPersonSelectString(iPhotoID);
            string sBuildingSelectcmd = PicturedBuildingSelectString(iPhotoID);
            string sCategorySelectcmd = CategoryValueSelectCommand(U.PhotoCategoryValue_Table, U.PhotoID_col, iPhotoID);

            SqlCommand cmd = new SqlCommand(sPhotoSelect_cmd + sPeopleSelectcmd + sBuildingSelectcmd + sCategorySelectcmd, sqlConnection);
            cmd.Parameters.Add(new SqlParameter("@" + U.PhotoID_col, iPhotoID));
            ExecuteSelectStatement(Photo_ds, cmd, U.Photo_Table, U.PicturedPerson_Table, U.PicturedBuilding_Table, U.PhotoCategoryValue_Table);
            return true;
        }
        //****************************************************************************************************************************
        private static string PicturedBuildingSelectString(int photoID)
        {
            string tableNames = U.PicturedBuilding_Table + "],[" + U.Building_Table;
            string[] selectColumns = { U.TableAndColumn(U.PicturedBuilding_Table, U.PicturedBuildingNumber_col),
                                       U.TableAndColumn(U.PicturedBuilding_Table, U.PhotoID_col),
                                       U.TableAndColumn(U.PicturedBuilding_Table, U.BuildingID_col),
                                       U.TableAndColumn(U.Building_Table, U.BuildingName_col) };
            string[] orderByColumns = { U.TableAndColumn(U.PicturedBuilding_Table, U.PicturedBuildingNumber_col) };
            return SelectColumnAsString(selectColumns, orderByColumns, tableNames,
                                         new NameValuePair(U.TableAndColumn(U.PicturedBuilding_Table, U.PhotoID_col), photoID),
                                         new NameValuePair(U.TableAndColumn(U.PicturedBuilding_Table, U.BuildingID_col),
                                                           U.TableAndColumn(U.Building_Table, U.BuildingID_col)));
        }
        //****************************************************************************************************************************
        private static string PicturedPersonSelectString(int photoID)
        {
            string tableNames = U.PicturedPerson_Table + "],[" + U.Person_Table;
            string[] selectColumns = { U.TableAndColumn(U.PicturedPerson_Table, U.PicturedPersonNumber_col),
                                       U.TableAndColumn(U.PicturedPerson_Table, U.PhotoID_col),
                                       U.TableAndColumn(U.PicturedPerson_Table, U.PersonID_col),
                                       U.TableAndColumn(U.Person_Table, U.FirstName_col),
                                       U.TableAndColumn(U.Person_Table, U.MiddleName_col),
                                       U.TableAndColumn(U.Person_Table, U.LastName_col),
                                       U.TableAndColumn(U.Person_Table, U.Suffix_col),
                                       U.TableAndColumn(U.Person_Table, U.Prefix_col) };
            string[] orderByColumns = { U.TableAndColumn(U.PicturedPerson_Table, U.PicturedPersonNumber_col) };
            return SelectColumnAsString(selectColumns, orderByColumns, tableNames,
                                         new NameValuePair(U.TableAndColumn(U.PicturedPerson_Table, U.PhotoID_col), photoID),
                                         new NameValuePair(U.TableAndColumn(U.PicturedPerson_Table, U.PersonID_col),
                                                           U.TableAndColumn(U.Person_Table, U.PersonID_col)));
        }
        //****************************************************************************************************************************
    }
}
