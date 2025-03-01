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
        public static void UpdatePersonData(int iPersonID,
                                            int iSpouseID,
                                            DataSet person_ds,
                                            ArrayList FieldsModified)
        {
            if (!MarriedNameIsModified(FieldsModified))
            {
                DataTable marriage_tbl = person_ds.Tables[U.Marriage_Table];
                foreach (DataRow marriageRow in marriage_tbl.Rows)
                {
                    AddMarriedNames(FieldsModified, person_ds.Tables[U.Person_Table].Rows[0], marriageRow, iPersonID);
                }
            }
            SqlTransaction txn = sqlConnection.BeginTransaction();
            AddPersonSpouseNotesAndOrderToBuildingTable(person_ds.Tables[U.BuildingOccupant_Table], iPersonID, iSpouseID);
            if (UpdatePersonData(txn, iPersonID, person_ds, FieldsModified))
            {
                txn.Commit();
            }
            else
            {
                txn.Rollback();
            }
        }
        //****************************************************************************************************************************
        private static bool MarriedNameIsModified(ArrayList FieldsModified)
        {
            foreach (string FieldModified in FieldsModified)
            {
                if (FieldModified == U.MarriedName_col)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        public static bool UpdatePersonData(SqlTransaction txn,
                                            int iPersonID,
                                            DataSet person_ds,
                                            ArrayList FieldsModified)
        {
            try
            {
                if (UpdateWithDA(txn, person_ds.Tables[U.Person_Table], U.Person_Table, U.PersonID_col, FieldsModified))
                {
                    return SavePersonData(txn, person_ds, iPersonID);
                }
                else
                    return false;
            }
            catch (HistoricJamaicaException ex)
            {
                txn.Rollback();
                throw ex;
            }
            catch (Exception e)
            {
                txn.Rollback();
                throw e;
            }
        }
        //****************************************************************************************************************************
        private static void AddMarriedNames(ArrayList FieldsModified, DataRow personRow, DataRow marriageRow, int personId)
        {
            int countNullSpouse = 0;
            int countNotMale = 0;
            int countLastNameAlreadyInPersonRow = 0;
            int countLastNameAlreadyInPersonRowAlternate = 0;
            int countAddedToPerson = 0;
            int countAddToMarriedName = 0;
            int countAddToMarriedName2 = 0;
            int countAddToMarriedName3 = 0;
            SQL.AddLastnameToPersonRecord(personRow, marriageRow, personId, ref countNullSpouse, ref countNotMale, ref countLastNameAlreadyInPersonRow,
                                      ref countLastNameAlreadyInPersonRowAlternate, ref countAddedToPerson,
                                      ref countAddToMarriedName, ref countAddToMarriedName2, ref countAddToMarriedName3);
            if (countAddToMarriedName > 0)
            {
                FieldsModified.Add(U.MarriedName_col);
            }
            if (countAddToMarriedName2 > 0)
            {
                FieldsModified.Add(U.MarriedName2_col);
            }
            if (countAddToMarriedName3 > 0)
            {
                FieldsModified.Add(U.MarriedName3_col);
            }
        }
        //****************************************************************************************************************************
        private static void AddPersonSpouseNotesAndOrderToBuildingTable(DataTable BuildingOccupant_tbl,
                                                                  int personID,
                                                                  int spouseID)
        {
            foreach (DataRow row in BuildingOccupant_tbl.Rows)
            {
                if (row.RowState == DataRowState.Added)
                {
                    row[U.SpouseLivedWithID_col] = spouseID;
                    row[U.Notes_col] = "";
                    row[U.BuildingValueOrder_col] = 0;
                    row[U.CensusYears_col] = 0;
                }
            }
        }
        //****************************************************************************************************************************
        private static bool SavePersonData(SqlTransaction txn,
                                           DataSet person_ds,
                                           int iPersonID)
        {
            if (DeleteBuildingCategoryValues(txn, iPersonID, person_ds.Tables[U.PersonCategoryValue_Table],
                                            person_ds.Tables[U.BuildingOccupant_Table]))
            {
                if (DeleteInsertMarriages(txn, person_ds.Tables[U.Person_Table], person_ds.Tables[U.Marriage_Table]))
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        public static bool ThereAreNoConflictsForFatherOrMotherIDs(DataTable MainUpdatePerson_tbl)
        // check to see if the FatherID and MotherID in the From Database point to the same FatherID amd MotherID
        // in the main database if the Person already existed in the Main Database.
        // If there is no conflict, update the FatherID and MotherID in the Main database 
        {
            bool bThereAreNoConflicts = true;
            foreach (DataRow Person_row in MainUpdatePerson_tbl.Rows)
            {
                if (CheckFatherMother(MainUpdatePerson_tbl, Person_row[U.FatherID_col],
                                                            Person_row[U.FatherID_col]) &&
                    CheckFatherMother(MainUpdatePerson_tbl, Person_row[U.MotherID_col],
                                                            Person_row[U.MotherID_col]))
                {
                    UpdateWithParms(U.PersonID_col, new NameValuePair(U.PersonID_col, Person_row[U.PersonID_col]),
                                                    new NameValuePair(U.ImportPersonID_col, Person_row[U.ImportPersonID_col]));
//                    string UpdateString = "Update Person " + ColumnEquals(U.ImportPersonID_col) +
//                                         " where " + ColumnEquals(U.PersonID_col);
//                    SqlCommand cmd = new SqlCommand(UpdateString, sqlConnection);
//                    cmd.Parameters.Add(new SqlParameter("@" + U.PersonID_col, Person_row[U.PersonID_col]));
//                    cmd.Parameters.Add(new SqlParameter("@" + U.ImportPersonID_col, Person_row[U.ImportPersonID_col]));
//                    ExecuteUpdateStatement(cmd);
                }
            }
            return (bThereAreNoConflicts);
        }
        //****************************************************************************************************************************
        private static bool CheckFatherMother(DataTable MainUpdatePerson_tbl,
                                       object ImportParent,
                                       object MainParent)
        {
            int iMainParent = MainParent.ToInt();
            int iImportParent = ImportParent.ToInt();
            if (iImportParent == 0)
                return true;
            if (iMainParent == 0)
                return true;
            DataRow row = MainUpdatePerson_tbl.Rows.Find(iImportParent);
            if (row == null)
                return false;
            if (row[U.PersonID_col].ToInt() != iMainParent)
                return false;
            return true;
        }
        //****************************************************************************************************************************
    }
}
