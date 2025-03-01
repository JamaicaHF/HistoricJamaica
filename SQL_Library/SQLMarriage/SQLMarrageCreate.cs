using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {
        //****************************************************************************************************************************
        private static bool SaveMarriages(SqlTransaction txn,
                                   DataTable marriage_tbl)
        {
            int iNumRecordsToAdd = marriage_tbl.NumStateRecordsInTable(DataViewRowState.Added);
            int iNumRecordsToDelete = marriage_tbl.NumStateRecordsInTable(DataViewRowState.Deleted);
            if (iNumRecordsToAdd == 0 && iNumRecordsToDelete == 0)
                return true;
            SqlCommand insertCommand = InsertCommand(txn, marriage_tbl, U.Marriage_Table, false);
            SqlCommand deleteCommand = DeleteCommandWithDA(txn, U.Marriage_Table, U.PersonID_col, U.SpouseID_col);
            return DeleteInsertWithDA(marriage_tbl, deleteCommand, insertCommand);
        }
        //****************************************************************************************************************************
        public static void InsertMarriage(int iPersonID,
                                          int iSpouseID)
        {
            DataTable marriage_tbl = DefineMarriageTable();
            DataRow marriage_row = marriage_tbl.NewRow();
            marriage_row[U.PersonID_col] = iPersonID;
            marriage_row[U.SpouseID_col] = iSpouseID;
            marriage_row[U.DateMarried_col] = String.Empty;
            marriage_row[U.Divorced_col] = "M";
            marriage_tbl.Rows.Add(marriage_row);
            SqlCommand insertCommand = InsertCommand(marriage_tbl, U.Marriage_Table, false);
            InsertWithDA(marriage_tbl, insertCommand);
        }
        //****************************************************************************************************************************
        public static void InsertMarriageIfItDoesNotExist(int iPersonID,
                                                   int iSpouseID)
        {
            if (iPersonID == 0 || iSpouseID == 0)
                return;
            DataTable tbl = new DataTable(U.Marriage_Table);
            GetMarriage(tbl, iPersonID, iSpouseID);
            if (tbl.Rows.Count == 0)
            {
                InsertMarriage(iPersonID, iSpouseID);
            }
            return;
        }
        //****************************************************************************************************************************
        public static bool AddSpouseforChildren(DataTable tblSpouses,
                                          int iPersonID,
                                          int iSpouseLocationInArray)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.Person_Table, tbl, new NameValuePair(U.FatherID_col, iPersonID));
            foreach (DataRow row in tbl.Rows)
            {
                int iFatherID = row[U.FatherID_col].ToInt();
                int iMotherID = row[U.MotherID_col].ToInt();
                int iSpouse1 = (iSpouseLocationInArray == 0) ? iMotherID : iFatherID;
                int iSpouse2 = (iSpouseLocationInArray == 0) ? iFatherID : iMotherID;
                if (iFatherID == iPersonID)
                {
                    if (AddMarriageIfDoesNotExist(tblSpouses, iSpouse2, iSpouse1, "", ""))
                    {
                        if (iMotherID != 0)
                        {
                            InsertMarriage(iFatherID, iMotherID);
                        }
                    }
                }
                else
                {
                    if (AddMarriageIfDoesNotExist(tblSpouses, iSpouse1, iSpouse2, "", ""))
                    {
                        if (iFatherID != 0)
                            InsertMarriage(iMotherID, iFatherID);
                    }
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        private static bool AddMarriageIfDoesNotExist(DataTable tblSpouses,
                                               int iSpouseID,
                                               int iPersonID,
                                               string sDateMarried,
                                               string sDivorced)
        {
            if (MarriageDoesNotExists(tblSpouses, iSpouseID, iPersonID))
            {
                tblSpouses.Rows.Add(iPersonID, iSpouseID, sDateMarried, sDivorced);
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private static void AddAdditionalMarriages(DataTable tblSpouses,
                                                int iPersonID)
        {
            DataTable Marriage_tbl = new DataTable(U.Marriage_Table);
            GetMarriages(Marriage_tbl, "SpouseID", iPersonID);
            foreach (DataRow row in Marriage_tbl.Rows)
            {
                AddMarriageIfDoesNotExist(tblSpouses, row[U.PersonID_col].ToInt(), row[U.SpouseID_col].ToInt(),
                                          row[U.DateMarried_col].ToString(), row[U.Divorced_col].ToString());
            }
        }
        //****************************************************************************************************************************
        private static bool MarriageDoesNotExists(DataTable Marriage_tbl,
                                           int iPersonID,
                                           int iSpouseID)
        {
            DataTable tbl = new DataTable();
            bool bDoesNotExist = true;
            foreach (DataRow row in Marriage_tbl.Rows)
            {
                int iOldPersonID = row[U.PersonID_col].ToInt();
                int iOldSpouseID = row[U.SpouseID_col].ToInt();
                if (iPersonID == iOldPersonID && iSpouseID == iOldSpouseID)
                    return false;
                if (iPersonID == iOldSpouseID && iSpouseID == iOldPersonID)
                    return false;
            }
            return bDoesNotExist;
        }
        //****************************************************************************************************************************
        public static bool AddMarriageToDataTable(DataTable tbl,
                                           int iPerson,
                                           int iSpouse,
                                           string sDataMarried,
                                           string sDivorced)
        {
            DataRow Marriage_row = tbl.NewRow();
            Marriage_row[U.PersonID_col] = iPerson;
            Marriage_row[U.SpouseID_col] = iSpouse;
            Marriage_row[U.DateMarried_col] = sDataMarried;
            Marriage_row[U.Divorced_col] = sDivorced[0];
            tbl.Rows.Add(Marriage_row);
            return true;
        }
        //****************************************************************************************************************************
    }
}
