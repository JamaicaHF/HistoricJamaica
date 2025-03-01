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
        public static int CreatePersonData(DataSet person_ds)
        {
            int iPersonID = 0;
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, person_ds.Tables[U.Person_Table], U.Person_Table, true);
                if (InsertWithDA(person_ds.Tables[U.PersonType], insertCommand))
                {
                    iPersonID = person_ds.Tables[U.PersonType].Rows[0][U.PersonID_col].ToInt();
                    if (iPersonID == 0)
                    {
                        HistoricJamaicaException ex = new HistoricJamaicaException(ErrorCodes.eSaveUnsuccessful);
                        throw ex;
                    }
                    if (DeleteBuildingCategoryValues(txn, iPersonID, person_ds.Tables[U.PersonCategoryValue_Table],
                                                                    person_ds.Tables[U.BuildingOccupant_Table]))
                    {
                        txn.Commit();
                        return iPersonID;
                    }
                }
                txn.Rollback();
                return 0;
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
        public static bool IntegratePersonUpdate(SqlTransaction txn,
                                 DataTable Person_tbl)
        {
            ArrayList fieldValues = ColumnList(U.FatherID_col, U.MotherID_col, U.LastName_col, U.MarriedName_col, U.MiddleName_col, U.Prefix_col,
                                               U.Suffix_col, U.Notes_col, U.Sex_col, U.BornDate_col, U.BornSource_col, U.DiedDate_col, U.DiedSource_col);
            SqlCommand updateCommand = UpdateCommand(txn, Person_tbl.Columns, U.Person_Table, U.PersonID_col, fieldValues);
            SqlCommand insertCommand = InsertCommand(txn, Person_tbl, U.Person_Table, true);
            return UpdateInsertWithDA(Person_tbl, updateCommand, insertCommand);
        }
        //****************************************************************************************************************************
        public static int SavePersonRecordsWithFatherMotherIDs(SqlTransaction txn,
                                                        DataTable Person_tbl)
        {
            IntegratePersonUpdate(txn, Person_tbl);
            return Person_tbl.Rows[0][U.PersonID_col].ToInt();
        }
        //****************************************************************************************************************************
        private static bool UpdateParentIDsInMainDatabase(SqlTransaction txn,
                                                   DataTable MainPerson_tbl)
        // Update the person records in the main table with the FatherID and MotherID based on the ImportPersonID in the New and Updated
        // records in the main person table
        {
            foreach (DataRow Person_row in MainPerson_tbl.Rows)
            {
                int iFatherID = Person_row[U.FatherID_col].ToInt();
                if (iFatherID != 0)
                    Person_row[U.FatherID_col] = GetPersonIDFromImportPersonIDFromTable(MainPerson_tbl, iFatherID);
                int iMotherID = Person_row[U.MotherID_col].ToInt();
                if (iMotherID != 0)
                    Person_row[U.MotherID_col] = GetPersonIDFromImportPersonIDFromTable(MainPerson_tbl, iMotherID);
            }
            return IntegratePersonUpdate(txn, MainPerson_tbl);
        }
        //****************************************************************************************************************************
        private static SqlDbType DBType(System.Type columnType)
        {
            SqlDbType dbType = SqlDbType.Int;
            if (columnType == typeof(int))
                dbType = SqlDbType.Int;
            else if (columnType == typeof(char))
                dbType = SqlDbType.VarChar;
            else
                dbType = SqlDbType.VarChar;
            return dbType;
        }
        //****************************************************************************************************************************
        public static bool InsertPersonTable(DataTable FromPerson_tbl,
                                      DataTable InsertPerson_tbl,
                                      DataTable MarriageTable,
                                      DataTable Cemetery_tbl)
        // First insert all the new Persons into the main database.  The ImportPersonID contains the PersonID from the From Database.
        // The Update method will load the PersonID in the main database for all the added rows.
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (Cemetery_tbl.Rows.Count != 0)
            {
                int iCemeteryID = Cemetery_tbl.Rows[0][U.CemeteryID_col].ToInt();
                DeleteWithParms(txn, U.CemeteryRecord_Table, new NameValuePair(U.CemeteryID_col, iCemeteryID));
            }
            bool bSuccess = false;
            SqlCommand insertCommand = InsertCommand(txn, InsertPerson_tbl, U.Person_Table, true);
            if (InsertWithDA(InsertPerson_tbl, insertCommand))
            // Add the Person rows that already exist in the Main Database to those just inserted.  They are the records added to the
            // FromPerson_tbl with the ImportPersonID in the From table as well as the PersonIDs for the already existing rows in the main Person table
            {
                DataViewRowState dvRowState = DataViewRowState.Added;
                foreach (DataRow row in FromPerson_tbl.Select("", "", dvRowState))
                {
                    InsertPerson_tbl.Rows.Add(row.ItemArray);
                }
                if (UpdateParentIDsInMainDatabase(txn, InsertPerson_tbl) &&
                    InsertMarriagesWithUpdatedPersonID(txn, InsertPerson_tbl, MarriageTable) &&
                    InsertCemeteryRecordsWithUpdatedPersonID(txn, InsertPerson_tbl, Cemetery_tbl))
                {
                    bSuccess = true;
                }
            }
            if (bSuccess)
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
        private static bool InsertMarriagesWithUpdatedPersonID(SqlTransaction txn,
                                                        DataTable Person_tbl,
                                                        DataTable Marriage_tbl)
        {
            DataTable NewMarriage_tbl = DefineMarriageTable();
            foreach (DataRow Marriage_row in Marriage_tbl.Rows)
            {
                Marriage_row[U.PersonID_col] = GetPersonIDFromImportPersonIDFromTable(Person_tbl, Marriage_row[U.PersonID_col].ToInt());
                Marriage_row[U.SpouseID_col] = GetPersonIDFromImportPersonIDFromTable(Person_tbl, Marriage_row[U.SpouseID_col].ToInt());
                NewMarriage_tbl.Rows.Add(Marriage_row.ItemArray);
            }
            SqlCommand insertCommand = InsertCommand(txn, Marriage_tbl, U.Marriage_Table, false);
            return InsertWithDA(NewMarriage_tbl, insertCommand);
        }
        //****************************************************************************************************************************
        private static DataTable GetVitalRecords(int iPersonID,
                                            int iMergePersonID)
        {
            DataTable VitalRecord_tbl = SQL.ChangePersonIDInVitalRecords(iMergePersonID, iPersonID);
            if (VitalRecord_tbl == null)
            {
                string message = "This merger has different Vital Record Types for each person record";
                throw new HistoricJamaicaException(message);
            }
            return VitalRecord_tbl;
        }
        //****************************************************************************************************************************
        public static bool MergeTwoPersons(DataSet person_ds,
                                           DataSet mergePerson_ds,
                                    int iPersonID,
                                    int iMergePersonID)
        {
            DataRow PersonRow = person_ds.Tables[U.Person_Table].Rows[0];
            DataRow MergePersonRow = mergePerson_ds.Tables[U.Person_Table].Rows[0];
            U.CheckForNonDuplicateMarriages(person_ds.Tables[U.Marriage_Table], mergePerson_ds.Tables[U.Marriage_Table], iPersonID, iMergePersonID);
            U.MergeTables(person_ds.Tables[U.PersonCategoryValue_Table], mergePerson_ds.Tables[U.PersonCategoryValue_Table], U.CategoryValueID_col);
            U.MergeTables(person_ds.Tables[U.BuildingOccupant_Table], mergePerson_ds.Tables[U.BuildingOccupant_Table], U.BuildingID_col);
            DataTable PicturedPerson_tbl = SetNewPersonId(U.PicturedPerson_Table, U.PersonID_col, iPersonID, iMergePersonID);
            DataTable PersonFather_tbl = SetNewPersonId(U.Person_Table, U.FatherID_col, iPersonID, iMergePersonID);
            DataTable PersonMother_tbl = SetNewPersonId(U.Person_Table, U.MotherID_col, iPersonID, iMergePersonID);

            DataTable CemeteryRecord_tbl = SQL.ChangePersonIDInCemeteryRecords(iMergePersonID, iPersonID);
            DataTable SchoolRecord_tbl = SQL.ChangePersonIDInSchoolRecords(iMergePersonID, iPersonID);
            DataTable VitalRecord_tbl = GetVitalRecords(iPersonID, iMergePersonID);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            bool bSuccess = UpdateWithDA(txn, CemeteryRecord_tbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col,
                                         ColumnList(U.PersonID_col, U.FatherID_col, U.MotherID_col));
            if (bSuccess) bSuccess = UpdateWithDA(txn, SchoolRecord_tbl, U.SchoolRecord_Table, U.SchoolRecordID_col,
                                         ColumnList(U.PersonID_col));
            if (bSuccess) bSuccess = UpdateWithDA(txn, VitalRecord_tbl, U.VitalRecord_Table, U.VitalRecordID_col,
                                         ColumnList(U.PersonID_col, U.FatherID_col, U.MotherID_col));
            if (bSuccess) bSuccess = UpdateAllNoKeysDA(txn, PicturedPerson_tbl, U.PicturedPerson_Table, ColumnList(U.PersonID_col, U.PhotoID_col));
            if (bSuccess) bSuccess = UpdateWithDA(txn, PersonFather_tbl, U.Person_Table, U.PersonID_col, ColumnList(U.FatherID_col));
            if (bSuccess) bSuccess = UpdateWithDA(txn, PersonMother_tbl, U.Person_Table, U.PersonID_col, ColumnList(U.MotherID_col));
            if (bSuccess)
            {
                ArrayList FieldsModified = new ArrayList();
                MoveMergeInfoToPerson(FieldsModified, iPersonID, person_ds.Tables[U.Person_Table], MergePersonRow);
                bSuccess = UpdatePersonData(txn, iPersonID, person_ds, FieldsModified);
            }
            if (bSuccess)
            {
                txn.Commit();
                bSuccess = DeletePersonFromDatabase(iMergePersonID);
            }
            else
            {
                txn.Rollback();
            }
            return bSuccess;
        }
        public static DataTable GetSpouseNames(int iPersonID)
        {
            DataTable tblSpouses = new DataTable();
            GetAllSpouses(tblSpouses, iPersonID);
            DataTable personTbl = new DataTable();
            foreach (DataRow spouseRow in tblSpouses.Rows)
            {
                int searchID = spouseRow[U.PersonID_col].ToInt() == iPersonID ? spouseRow[U.SpouseID_col].ToInt() : spouseRow[U.PersonID_col].ToInt();
                GetPerson(personTbl, searchID);
            }
            return personTbl;
        }
        //****************************************************************************************************************************
        public static bool GetAllSpouses(DataTable tblSpouses,
                                  int iPersonID)
        {
            GetMarriages(tblSpouses, U.PersonID_col, iPersonID);
            GetMarriages(tblSpouses, U.SpouseID_col, iPersonID);
            return true;
        }
        //****************************************************************************************************************************
        private static bool ShowSuccess(SqlTransaction txn,
                                 bool bSuccess)
        {
            if (bSuccess)
            {
                if (txn != null)
                    txn.Commit();
            }
            else
            {
                if (txn != null)
                    txn.Rollback();
            }
            return true;
        }
        //****************************************************************************************************************************
        private static DataTable SetNewPersonId(string sTableName,
                                    string sColumnName,
                                    int iPersonID,
                                    int iNewPersonID)
        {
            DataTable tbl = new DataTable();
            SelectAll(sTableName, tbl, new NameValuePair(sColumnName, iNewPersonID));
            foreach (DataRow row in tbl.Rows)
            {
                row[sColumnName] = iPersonID.ToInt();
            }
            return tbl;
        }
        //****************************************************************************************************************************
    }
}
