using System;
using System.Windows;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SQL_Library;

namespace SQL_Library
{
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static bool UpdateCemeteryRecord(ArrayList FieldsModified,
                                             DataTable CemeteryRecord_tbl,
                                             PersonSpouseWithParents personSpouseWithParents)
        {
            DataRow CemeteryRecord_row = CemeteryRecord_tbl.Rows[0];
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                if (UpdateWithDA(txn, CemeteryRecord_tbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col, FieldsModified))
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
        public class PersonInfo
        {
            public string personName;
            public string spouseName;
            public string fatherName;
            public string motherName;
            public string bornDate;
            public string diedDate;
            public PersonInfo(string personName, string spouseName, string fatherName, string motherName, string bornDate, string diedDate)
            {
                this.personName = personName;
                this.spouseName = spouseName;
                this.fatherName = fatherName;
                this.motherName = motherName;
                this.bornDate = bornDate;
                this.diedDate = diedDate;
            }
        }
        //****************************************************************************************************************************
        private static void SearchForSimilarNames(DataTable VitalRecord_tbl, DataRow cemeteryRecord_row)
        {
            PersonName personName = new PersonName(cemeteryRecord_row[U.FirstName_col].ToString(), "", cemeteryRecord_row[U.LastName_col].ToString(), "", "");
            SearchVitalRecords(VitalRecord_tbl, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
            //SearchVitalRecords(VitalRecord_tbl, U.FatherFirstName_col, U.FatherMiddleName_col, U.FatherLastName_col, personName);
            //SearchVitalRecords(VitalRecord_tbl, U.MotherFirstName_col, U.MotherMiddleName_col, U.MotherLastName_col, personName);
        }
        //****************************************************************************************************************************
        private static void SearchVitalRecords(DataTable tbl,
                                        string firstName_col,
                                        string middleName_col,
                                        string lastNameCol,
                                        PersonName personName)
        {
            if (String.IsNullOrEmpty(personName.lastName) &&
                String.IsNullOrEmpty(personName.firstName))
            {
                return;
            }
            PersonSearchTableAndColumns personSearchTableAndColumns = new PersonSearchTableAndColumns(U.VitalRecord_Table,
                              firstName_col, middleName_col, lastNameCol);
            SQL.PersonsBasedOnNameOptions(tbl, false, personSearchTableAndColumns, U.VitalRecordID_col, eSearchOption.SO_Similar, personName);
        }
        //****************************************************************************************************************************
        private static string BuildName(string firstname, string middleName, string lastName)
        {
            return firstname + " " + middleName + " " + lastName;
        }
        //****************************************************************************************************************************
        public static bool IntegrateCemeteryRecord(DataRow CemeteryRecord_row,
                                         DataTable Person_tbl,
                                         DataTable Marriage_tbl,
                                         bool getFromGrid)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                bool bSuccess = false;
                int iPersonID = SavePersonRecordsWithFatherMotherIDs(txn, Person_tbl);
                if (iPersonID != 0)
                {
                    CemeteryRecord_row[U.PersonID_col] = iPersonID;
                    CheckCemeteryRecordFatherMotherIDs(txn, CemeteryRecord_row, Person_tbl, Marriage_tbl, U.PersonType);
                    IntegratePersonUpdate(txn, Person_tbl);
                    bSuccess = SaveMarriages(txn, Marriage_tbl);
                }
                if (bSuccess && (!getFromGrid || UserDoesNotWantToAbort()))
                {
                    txn.Commit();
                    return true;
                }
                else
                {
                    CemeteryRecord_row[U.PersonID_col] = 0;
                    CemeteryRecord_row[U.SpouseID_col] = 0;
                    CemeteryRecord_row[U.FatherID_col] = 0;
                    CemeteryRecord_row[U.MotherID_col] = 0;
                    txn.Rollback();
                    return false;
                }
            }
            catch (HistoricJamaicaException Exception)
            {
                txn.Rollback();
                throw Exception;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw ex;
            }
        }
        //****************************************************************************************************************************
        public static bool SaveIntegratedCemeteryRecords(DataTable CemeteryRecord_tbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (UpdateWithDA(txn, CemeteryRecord_tbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col,
                                  ColumnList(U.PersonID_col, U.SpouseID_col, U.FatherID_col, U.MotherID_col)))
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
        private static DataRow GetCemeteryRecordPersonRow(DataTable Person_tbl,
                                     int iVitalRecordPersonType)
        {
            string sSelect = U.ImportPersonID_col + " = " + iVitalRecordPersonType.ToString();
            foreach (DataRow row in Person_tbl.Select(sSelect))
            {
                return row;
            }
            return null;
        }
        //****************************************************************************************************************************
        private static int CheckCemeteryRecordFatherMotherIDs(SqlTransaction txn,
                                          DataRow CemeteryRecord_row,
                                          DataTable Person_tbl,
                                          DataTable Marriage_tbl,
                                          int iVitalRecordPersonType)
        {
            bool bUpdatePerson = false;
            int iPersonID = CemeteryRecord_row[U.PersonID_col].ToInt();
            int iSpouseID = CemeteryRecord_row[U.SpouseID_col].ToInt();
            int iFatherID = CemeteryRecord_row[U.FatherID_col].ToInt();
            int iMotherID = CemeteryRecord_row[U.MotherID_col].ToInt();
            DataRow Person_row = GetCemeteryRecordPersonRow(Person_tbl, iVitalRecordPersonType);
            DataRow Spouse_row = GetCemeteryRecordPersonRow(Person_tbl, iVitalRecordPersonType + 1);
            if (Spouse_row != null)
            {
                int iPersonSpouseID = Spouse_row[U.PersonID_col].ToInt();
                if (iSpouseID != iPersonSpouseID)
                {
                    iSpouseID = iPersonSpouseID;
                    CemeteryRecord_row[U.SpouseID_col] = iSpouseID;
                    bUpdatePerson = true;
                }
                string sMarriedName = Spouse_row[U.MarriedName_col].ToString();
                if (sMarriedName.Length == 0 && Person_row != null && Spouse_row[U.LastName_col].ToString() != Person_row[U.LastName_col].ToString())
                {
                    if (Spouse_row[U.Sex_col].ToChar() == 'F')
                    {
                        Spouse_row[U.MarriedName_col] = Person_row[U.LastName_col];
                        bUpdatePerson = true;
                    }
                }
            }
            DataRow Father_row = GetCemeteryRecordPersonRow(Person_tbl, iVitalRecordPersonType + 2);
            if (Father_row != null)
            {
                int iPersonFatherID = Father_row[U.PersonID_col].ToInt();
                if (iFatherID != iPersonFatherID)
                {
                    iFatherID = iPersonFatherID;
                    CemeteryRecord_row[U.FatherID_col] = iFatherID;
                    bUpdatePerson = true;
                }
            }
            DataRow Mother_row = GetCemeteryRecordPersonRow(Person_tbl, iVitalRecordPersonType + 3);
            if (Mother_row != null)
            {
                int iPersonMotherID = Mother_row[U.PersonID_col].ToInt();
                if (iMotherID != iPersonMotherID)
                {
                    iMotherID = iPersonMotherID;
                    CemeteryRecord_row[U.MotherID_col] = iMotherID;
                    bUpdatePerson = true;
                }
                string sMarriedName = Mother_row[U.MarriedName_col].ToString();
                if (sMarriedName.Length == 0 && Father_row != null && Mother_row[U.LastName_col].ToString() != Father_row[U.LastName_col].ToString())
                {
                    Mother_row[U.MarriedName_col] = Father_row[U.LastName_col];
                    bUpdatePerson = true;
                }
            }
            if (Person_row == null)
                iPersonID = 0;
            else if (iPersonID != Person_row[U.PersonID_col].ToInt() || bUpdatePerson)
            {
                iPersonID = Person_row[U.PersonID_col].ToInt();
                if (Person_row[U.FatherID_col].ToInt() == 0)
                {
                    Person_row[U.FatherID_col] = iFatherID;
                }
                if (Person_row[U.MotherID_col].ToInt() == 0)
                {
                    Person_row[U.MotherID_col] = iMotherID;
                }
            }
            if (MarriageDoesNotExist(Marriage_tbl, iFatherID, iMotherID))
            {
                AddNewMarriage(Marriage_tbl, iFatherID, iMotherID);
            }
            if (MarriageDoesNotExist(Marriage_tbl, iPersonID, iSpouseID))
            {
                AddNewMarriage(Marriage_tbl, iPersonID, iSpouseID);
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private static void AddNewMarriage(DataTable Marriage_tbl, int personId, int spouseId)
        {
            if (personId != 0 && spouseId != 0)
                {
                    DataRow Marriage_row = GetMarriageRow(Marriage_tbl, personId, spouseId);
                    if (Marriage_row == null)
                    {
                        AddMarriageToDataTable(Marriage_tbl, personId, spouseId, "", "N");
                    }
                }
        }
        //****************************************************************************************************************************
        private static bool MarriageDoesNotExist(DataTable Marriage_tbl, int personId, int spouseId)
        {
            if (personId == 0 || spouseId == 0)
            {
                return false;
            }
            foreach (DataRow Marriage_row in Marriage_tbl.Rows)
            {
                if (Marriage_row[U.PersonID_col].ToInt() == personId && Marriage_row[U.SpouseID_col].ToInt() == spouseId)
                {
                    return false;
                }
                if (Marriage_row[U.PersonID_col].ToInt() == spouseId && Marriage_row[U.SpouseID_col].ToInt() == personId)
                {
                    return false;
                }
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
