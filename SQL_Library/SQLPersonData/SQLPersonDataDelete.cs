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
        public static bool DeletePersonFromDatabase(int iPersonID)
        {
            DataTable CemeteryRecord_tbl = ChangePersonIDInCemeteryRecords(iPersonID, 0);
            DataTable VitalRecord_tbl = ChangePersonIDInVitalRecords(iPersonID, 0);
            DataTable tblChildren = GetAllChildrenForPerson(iPersonID);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                int iSpouseMarriageResult = DeleteWithParms(txn, U.Marriage_Table, new NameValuePair(U.SpouseID_col, iPersonID));
                int iPersonMarriageResult = DeleteWithParms(txn, U.Marriage_Table, new NameValuePair(U.PersonID_col, iPersonID));
                int iCategoryValueResult = DeleteWithParms(txn, U.PersonCategoryValue_Table, new NameValuePair(U.PersonID_col, iPersonID));
                int iBuildingValueResult = DeleteWithParms(txn, U.BuildingOccupant_Table, new NameValuePair(U.PersonID_col, iPersonID));
                int iPicturedPersonResult = DeleteWithParms(txn, U.PicturedPerson_Table, new NameValuePair(U.PersonID_col, iPersonID));
                int iPersonResult = DeleteWithParms(txn, U.Person_Table, new NameValuePair(U.PersonID_col, iPersonID));
                bool bSuccess = UpdateChildrenFatherAndMother(txn, tblChildren, iPersonID);
                UpdateWithDA(txn, CemeteryRecord_tbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col,
                                  ColumnList(U.PersonID_col, U.FatherID_col, U.MotherID_col));
                UpdateWithDA(txn, VitalRecord_tbl, U.VitalRecord_Table, U.VitalRecordID_col, 
                                  ColumnList(U.PersonID_col, U.FatherID_col, U.MotherID_col));
                if (iPersonMarriageResult == U.Exception || iSpouseMarriageResult == U.Exception ||
                    iCategoryValueResult == U.Exception || iBuildingValueResult == U.Exception ||
                    iPicturedPersonResult == U.Exception || iPersonResult == U.Exception ||
                    !bSuccess || iPersonResult == U.Exception)
                {
                    txn.Rollback();
                }
                else
                    txn.Commit();
                return true;
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
        public static DataTable GetAllChildrenForPerson(int iPersonID)
        {
            DataTable tblChildren = new DataTable();
            if (iPersonID == 0)
                return tblChildren;
            SelectAll(U.Person_Table, tblChildren, new NameValuePair(U.FatherID_col, iPersonID));
            SelectAll(U.Person_Table, tblChildren, new NameValuePair(U.MotherID_col, iPersonID));
            return tblChildren;
        }
        //****************************************************************************************************************************
        private static bool UpdateChildrenFatherAndMother(SqlTransaction txn,
                                                         DataTable tblChildren,
                                                         int iPersonID)
        {
            if (tblChildren.Rows.Count == 0)
                return true;
            foreach (DataRow row in tblChildren.Rows)
            {
                if (row[U.FatherID_col].ToInt() == iPersonID)
                    row[U.FatherID_col] = 0;
                if (row[U.MotherID_col].ToInt() == iPersonID)
                    row[U.MotherID_col] = 0;
            }
            return UpdateFatherMother(txn, tblChildren);
        }
        //****************************************************************************************************************************
        public static DataTable ChangePersonIDInSchoolRecords(int iPersonID,
                                                              int iNewPersonID)
        {
            DataTable SchoolRecord_tbl = DefineSchoolRecordTable();
            SchoolRecord_tbl.PrimaryKey = new DataColumn[] { SchoolRecord_tbl.Columns[U.SchoolRecordID_col] };
            SelectAll(U.SchoolRecord_Table, SchoolRecord_tbl, new NameValuePair(U.PersonID_col, iPersonID));
            foreach (DataRow row in SchoolRecord_tbl.Rows)
            {
                if (row[U.PersonID_col].ToInt() == iPersonID)
                    row[U.PersonID_col] = iNewPersonID;
            }
            return SchoolRecord_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable ChangePersonIDInCemeteryRecords(int iPersonID,
                                                             int iNewPersonID)
        {
            DataTable cemeteryRecord_tbl = DefineCemeteryRecordTable();
            cemeteryRecord_tbl.PrimaryKey = new DataColumn[] { cemeteryRecord_tbl.Columns[U.CemeteryRecordID_col] };
            SelectAll(U.CemeteryRecord_Table, cemeteryRecord_tbl, new NameValuePair(U.PersonID_col, iPersonID));
            SelectAll(U.CemeteryRecord_Table, cemeteryRecord_tbl, new NameValuePair(U.SpouseID_col, iPersonID));
            SelectAll(U.CemeteryRecord_Table, cemeteryRecord_tbl, new NameValuePair(U.FatherID_col, iPersonID));
            SelectAll(U.CemeteryRecord_Table, cemeteryRecord_tbl, new NameValuePair(U.MotherID_col, iPersonID));
            foreach (DataRow row in cemeteryRecord_tbl.Rows)
            {
                if (row[U.PersonID_col].ToInt() == iPersonID)
                    row[U.PersonID_col] = iNewPersonID;
                if (row[U.SpouseID_col].ToInt() == iPersonID)
                    row[U.SpouseID_col] = iNewPersonID;
                if (row[U.FatherID_col].ToInt() == iPersonID)
                    row[U.FatherID_col] = iNewPersonID;
                if (row[U.MotherID_col].ToInt() == iPersonID)
                    row[U.MotherID_col] = iNewPersonID;
            }
            return cemeteryRecord_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable ChangePersonIDInVitalRecords(int iPersonID,
                                                             int iNewPersonID)
        {
            DataTable VitalRecord_tbl = DefineVitalRecord_Table();
            VitalRecord_tbl.PrimaryKey = new DataColumn[] { VitalRecord_tbl.Columns[U.VitalRecordID_col] };
            GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col);
            GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.FatherID_col);
            GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.MotherID_col);
            if (iNewPersonID != 0)
            {
                if (NewPersonHasDuplicateVitalRecordTypes(VitalRecord_tbl, iPersonID, iNewPersonID))
                    return null;
            }
            foreach (DataRow row in VitalRecord_tbl.Rows)
            {
                if (row[U.PersonID_col].ToInt() == iPersonID)
                    row[U.PersonID_col] = iNewPersonID;
                if (row[U.FatherID_col].ToInt() == iPersonID)
                    row[U.FatherID_col] = iNewPersonID;
                if (row[U.MotherID_col].ToInt() == iPersonID)
                    row[U.MotherID_col] = iNewPersonID;
            }
            return VitalRecord_tbl;
        }
        //****************************************************************************************************************************
        private static bool NewPersonHasDuplicateVitalRecordTypes(DataTable PersonVitalRecords_tbl,
                                                                  int iPersonID,
                                                                  int iNewPersonID)
        {
            DataTable NewPersonVitalRecords_tbl = DefineVitalRecord_Table();
            GetVitalRecordsForPerson(NewPersonVitalRecords_tbl, iNewPersonID, U.PersonID_col);
            foreach (DataRow Person_row in PersonVitalRecords_tbl.Rows)
            {
                int PersonVitalRecordType = Person_row[U.VitalRecordType_col].ToInt();
                int iPersonVitalRecordID = Person_row[U.VitalRecordID_col].ToInt();
                foreach (DataRow NewPerson_row in NewPersonVitalRecords_tbl.Rows)
                {
                    int iNewPersonVitalRecordID = NewPerson_row[U.VitalRecordID_col].ToInt();
                    int NewPersonVitalRecordType = NewPerson_row[U.VitalRecordType_col].ToInt();
                    if (ThisRecordIsForTheSamePerson(iPersonVitalRecordID, iPersonID, PersonVitalRecordType, 
                                                     iNewPersonVitalRecordID, iNewPersonID, NewPersonVitalRecordType))
                    {
                        EVitalRecordType VitalRecordType = (EVitalRecordType)NewPersonVitalRecordType;
                        if (VitalRecordType != EVitalRecordType.eMarriageBride &&
                            VitalRecordType != EVitalRecordType.eMarriageGroom &&
                            VitalRecordType != EVitalRecordType.eCivilUnionPartyA &&
                            VitalRecordType != EVitalRecordType.eCivilUnionPartyB)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private static bool ThisRecordIsForTheSamePerson(int iPersonVitalRecordID,
                                                         int iPersonID,
                                                         int PersonVitalRecordType,
                                                         int iNewPersonVitalRecordID,
                                                         int iNewPersonID,
                                                         int NewPersonVitalRecordType)
        {
            if (iPersonVitalRecordID == iNewPersonVitalRecordID)
            {
                return false;
            }
            if (iPersonID == iNewPersonID && PersonVitalRecordType == NewPersonVitalRecordType)
            {
                return true;
            }
            return false;
        }
    }
}
