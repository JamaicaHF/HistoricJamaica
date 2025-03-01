using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace SQL_Library
{
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static bool UpdateVitalRecord(ArrayList FieldsModified,
                                             DataTable VitalRecord_tbl,
                                             int iVitalRecordID,
                                             PersonSpouseWithParents personSpouseWithParents)
        {
            DataRow VitalRecord_row = VitalRecord_tbl.Rows[0];
            int iSpouseVitalRecordID = VitalRecord_row[U.SpouseID_col].ToInt();
            DataTable Marriage_tbl = GetMarriageVitalRecord(personSpouseWithParents.SpouseVitalRecordType,
                                              personSpouseWithParents.PersonName.integratedPersonID,
                                              personSpouseWithParents.SpouseName.integratedPersonID);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                UpdateSpouseVitalRecord(txn, VitalRecord_tbl, Marriage_tbl, iVitalRecordID, personSpouseWithParents);
                if (UpdateWithDA(txn, VitalRecord_tbl, U.VitalRecord_Table, U.VitalRecordID_col, FieldsModified))
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
        private static void UpdateSpouseVitalRecord(SqlTransaction txn,
                                                    DataTable VitalRecord_tbl,
                                                    DataTable Marriage_tbl,
                                                    int iVitalRecordID,
                                                    PersonSpouseWithParents personSpouseWithParents)
        {
            if (Marriage_tbl.Rows.Count == 0)
                return;
            if (personSpouseWithParents.SpouseVitalRecordType == EVitalRecordType.eSearch)
                return;
            if (VitalRecord_tbl.Rows.Count < 2)
                ThrowVitalErrorException(ErrorCodes.eSpouseRecordDoesNotExist);
            if (MarriageRecordNeedsToBeUpdated(Marriage_tbl.Rows[0], VitalRecord_tbl.Rows[0]))
            {
                UpdateMarriage(txn, Marriage_tbl);
            }
        }
        //****************************************************************************************************************************
        private static bool MarriageRecordNeedsToBeUpdated(DataRow Marriage_row,
                                                    DataRow VitalRecord_row)
        {
            string VitalRecordDate = U.BuildDate(VitalRecord_row[U.DateYear_col].ToInt(), VitalRecord_row[U.DateMonth_col].ToInt(),
                                               VitalRecord_row[U.DateDay_col].ToInt());
            bool recordsNeedsToBeUpdated = false;
            if (Marriage_row[U.DateMarried_col].ToString() != VitalRecordDate)
            {
                Marriage_row[U.DateMarried_col] = VitalRecordDate;
                recordsNeedsToBeUpdated = true;
            }
            if (Marriage_row[U.Divorced_col].ToString().Length == 0 || 
                Marriage_row[U.Divorced_col].ToString() == "N" ||
                Marriage_row[U.Divorced_col].ToString() == " ")
            {
                Marriage_row[U.Divorced_col] = "M";
                recordsNeedsToBeUpdated = true;
            }
            return recordsNeedsToBeUpdated;
        }
        //****************************************************************************************************************************
        public static bool UpdateVitalRecordBurialParents()
        {
            UpdateBurialParent(U.FatherID_col);
            UpdateBurialParent(U.MotherID_col);
            return true;
        }
        //****************************************************************************************************************************
        private static bool UpdateBurialParent(string sParentCol)
        {
            DataTable VitalRecords_tbl = new DataTable();
            int iVitalRecordType = (int)EVitalRecordType.eBurial;
            SelectAll(U.VitalRecord_Table, VitalRecords_tbl, new NameValuePair(U.VitalRecordType_col, iVitalRecordType.ToString()),
                                                             new NameValuePair(sParentCol," <> 0"));
            if (VitalRecords_tbl.Rows.Count != 0)
            {
                foreach (DataRow row in VitalRecords_tbl.Rows)
                {
                    row[sParentCol] = 0;
                }
                UpdateWithDA(VitalRecords_tbl, U.VitalRecord_Table, U.VitalRecordID_col, ColumnList(sParentCol));
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool UpdateVitalRecordMotherLastName()
        {
            DataTable VitalRecords_tbl = new DataTable();
            SelectAll(U.VitalRecord_Table, VitalRecords_tbl, new NameValuePair(U.MotherLastName_col, " <> ''"),
                                                             new NameValuePair(U.FatherLastName_col, U.MotherLastName_col));
            //             " Where " + U.MotherLastName_col + " <> '' and " + U.FatherLastName_col + " = " + U.MotherLastName_col);
            if (VitalRecords_tbl.Rows.Count != 0)
            {
                foreach (DataRow row in VitalRecords_tbl.Rows)
                {
                    row[U.MotherLastName_col] = "";
                }
                UpdateWithDA(VitalRecords_tbl, U.VitalRecord_Table, U.VitalRecordID_col, ColumnList(U.MotherLastName_col));
            }
            return true;
        }
        //****************************************************************************************************************************
        public static void SetBurialAgeFromDeathRecord()
        {
            DataTable burialTable = GetAllBurialRecords();
            DataTable deathTable = GetAllDeathVitalRecords();
            foreach (DataRow burialRow in burialTable.Rows)
            {
                string selectStatement = "(VitalRecordType = 3 or VitalRecordType = 4) and DateYear = " + burialRow[U.DateYear_col] +
                                           " and DateMonth = " + burialRow[U.DateMonth_col] + " and DateDay = " + burialRow[U.DateDay_col];
                DataRow[] foundRows = deathTable.Select(selectStatement);
                if (foundRows != null && foundRows.Length != 0)
                {
                    foreach (DataRow deathRow in foundRows)
                    {
                        if (burialRow[U.LastName_col].ToString() == deathRow[U.LastName_col].ToString() &&
                            burialRow[U.AgeYears_col].ToInt() == 0 && 
                            burialRow[U.AgeMonths_col].ToInt() == 0 &&
                            burialRow[U.AgeDays_col].ToInt() == 0)
                        {
                            burialRow[U.AgeYears_col] = deathRow[U.AgeYears_col];
                            burialRow[U.AgeMonths_col] = deathRow[U.AgeMonths_col];
                            burialRow[U.AgeDays_col] = deathRow[U.AgeDays_col];
                        }
                    }
                }
            }
            SqlTransaction txn = sqlConnection.BeginTransaction();
            ArrayList FieldsModified = new ArrayList();
            FieldsModified.Add(U.AgeYears_col);
            FieldsModified.Add(U.AgeMonths_col);
            FieldsModified.Add(U.AgeDays_col);
            if (UpdateWithDA(txn, burialTable, U.VitalRecord_Table, U.VitalRecordID_col, FieldsModified))
            {
                txn.Commit();
            }
            else
            {
                txn.Rollback();
            }
        }
        //****************************************************************************************************************************
        private static DataTable GetMarriageVitalRecord(EVitalRecordType SpouseVitalRecordType,
                                                        int iPersonID,
                                                        int iSpouseID)
        {
            DataTable marriage_tbl = DefineMarriageTable();
            if (ThereMayBeAMarriageRecord(SpouseVitalRecordType, iPersonID, iSpouseID))
            {
                if (!GetMarriage(marriage_tbl, iPersonID, iSpouseID))
                {
                    AddMarriageToDataTable(marriage_tbl, iPersonID, iSpouseID, "", "N");
                }
            }
            return marriage_tbl;
        }
        //****************************************************************************************************************************
        private static bool ThereMayBeAMarriageRecord(EVitalRecordType SpouseVitalRecordType,
                                                        int iPersonID,
                                                        int iSpouseID)
        {
            if (SpouseVitalRecordType == EVitalRecordType.eSearch)
                return false;
            if (iPersonID != 0 && iSpouseID != 0)
                return true;
            return false;
        }
        //****************************************************************************************************************************
        public static void CheckSexForAllVitalRecord()
        {
            DataTable vitalRecordTbl = SelectAll(U.VitalRecord_Table);
            foreach (DataRow vitalRecordRow in vitalRecordTbl.Rows)
            {
                if (vitalRecordRow[U.VitalRecordID_col].ToInt() == 447)
                {
                }
                EVitalRecordType vitalRecordType = vitalRecordRow[U.VitalRecordType_col].ToVitalRecordType();
                char personSex = vitalRecordRow[U.Sex_col].ToChar();
                if (vitalRecordType.RecordTypeSex(personSex) != eSex.eUnknown)
                {
                    char recordSex = U.RecordTypeSexChar(vitalRecordType, personSex);
                    if (personSex != recordSex)
                    {
                        vitalRecordRow[U.Sex_col] = recordSex;
                    }
                    CheckSexOfPartner(vitalRecordTbl, vitalRecordRow);
                }
                else
                {
                }
                //checkMarriageRecords(vitalRecordRow, vitalRecordType, personSex);
            }
           // UpdateWithDA(vitalRecordTbl, U.VitalRecord_Table, U.VitalRecordID_col, ColumnList(U.Sex_col, U.AgeDays_col, U.AgeMonths_col, U.AgeYears_col));
        }
        //****************************************************************************************************************************
        public static void CheckSexOfPartner(DataTable vitalRecordTbl, DataRow vitalRecordRow)
        {
            EVitalRecordType vitalRecordType = vitalRecordRow[U.VitalRecordType_col].ToVitalRecordType();
            switch (vitalRecordType)
            {
                case EVitalRecordType.eBurial:
                case EVitalRecordType.eBirthMale:
                case EVitalRecordType.eBirthFemale:
                case EVitalRecordType.eDeathMale:
                case EVitalRecordType.eDeathFemale: return;
            }
            ////////CheckAge(vitalRecordRow);
            char sex = vitalRecordRow[U.Sex_col].ToChar();
            int spouseId = vitalRecordRow[U.SpouseID_col].ToInt();
            
            string selectStatement = U.VitalRecordID_col + "=" + spouseId;
            DataRow[] spouseRecordRows = vitalRecordTbl.Select(selectStatement);
            if (spouseRecordRows.Length != 1)
            {
                MessageBox.Show("No Spouse Record");
                return;
            }
            DataRow spouseRecordRow = spouseRecordRows[0];
            char spouseSex = spouseRecordRow[U.Sex_col].ToChar();
            //if (foundRows.Length == 0)
            switch (vitalRecordType)
            {
                case EVitalRecordType.eMarriageBride:
                case EVitalRecordType.eMarriageGroom:
                    {
                        if (sex == spouseSex)
                        {
                        }
                        break;
                    }
                case EVitalRecordType.eCivilUnionPartyA:
                case EVitalRecordType.eCivilUnionPartyB:
                    {
                        if (sex != spouseSex)
                        {
                        }
                        break;
                    }
            }
        }
        //****************************************************************************************************************************
        public static void CheckAge(DataRow vitalRecordRow)
        {
            int ageYear = vitalRecordRow[U.AgeYears_col].ToInt();
            if (ageYear == 0)
            {
                return;
            }
            if (ageYear < 1900 || ageYear > 2019)
            {
                MessageBox.Show("Invalid Date: " + ageYear);
                return;
            }
            string oldDateStr = U.BuildDate(vitalRecordRow[U.AgeYears_col].ToInt(),
                               vitalRecordRow[U.AgeMonths_col].ToInt(),
                               vitalRecordRow[U.AgeDays_col].ToInt());
            int ageYears, ageMonths, ageDays;
            U.AgeBetweenTwoDates(vitalRecordRow[U.AgeYears_col].ToInt(),
                               vitalRecordRow[U.AgeMonths_col].ToInt(),
                               vitalRecordRow[U.AgeDays_col].ToInt(),
                               vitalRecordRow[U.DateYear_col].ToInt(),
                               vitalRecordRow[U.DateMonth_col].ToInt(),
                               vitalRecordRow[U.DateDay_col].ToInt(),
                                          out ageYears,
                                          out ageMonths,
                                          out ageDays);
            string dateStr = U.BornDateFromDiedDateMinusAge(vitalRecordRow[U.DateYear_col].ToInt(),
                                   vitalRecordRow[U.DateMonth_col].ToInt(),
                                   vitalRecordRow[U.DateDay_col].ToInt(),
                                   ageYears, ageMonths, ageDays, "");
            if (dateStr != oldDateStr)
            {
                MessageBox.Show("Poor Conversion");
            }
        }
        //****************************************************************************************************************************
        public static void checkMarriageRecords(DataRow vitalRecordRow,
                                                EVitalRecordType vitalRecordType,
                                                char sex)
        {
            if (!vitalRecordType.MarriageRecord())
                return;
            int spouseID = vitalRecordRow[U.SpouseID_col].ToInt();
            DataRow row = GetVitalRecord(spouseID);
            char spouseSex = row[U.Sex_col].ToChar();
            if (row == null)
                return;
            if (sex == spouseSex)
            {
                if (vitalRecordType == EVitalRecordType.eMarriageBride ||
                    vitalRecordType == EVitalRecordType.eMarriageGroom)
                {
                    int vitalRecordID = vitalRecordRow[U.VitalRecordID_col].ToInt();
                    DataTable VitalRecord_tbl = new DataTable();
                    GetVitalRecordWithSpouseRecord(VitalRecord_tbl, vitalRecordID);
                    if (VitalRecord_tbl.Rows.Count < 2)
                    {
                        return;
                    }
                    DataRow personRow = VitalRecord_tbl.Rows[0];
                    DataRow spouseRow = VitalRecord_tbl.Rows[1];
                    personRow[U.VitalRecordType_col] = EVitalRecordType.eCivilUnionPartyA;
                    spouseRow[U.VitalRecordType_col] = EVitalRecordType.eCivilUnionPartyB;
                    UpdateWithDA(VitalRecord_tbl, U.VitalRecord_Table, U.VitalRecordID_col, ColumnList(U.VitalRecordType_col));
                }
            }
        }
        //****************************************************************************************************************************
        public static bool ChangeToOppositeVitalRecordType(int iVitalRecordID)
        {
            DataTable VitalRecord_tbl = new DataTable();
            GetVitalRecordWithSpouseRecord(VitalRecord_tbl, iVitalRecordID);
            if (VitalRecord_tbl.Rows.Count == 0)
            {
                return false;
            }
            DataRow personRow = VitalRecord_tbl.Rows[0];
            EVitalRecordType newVitalRecordType = (EVitalRecordType) personRow[U.VitalRecordType_col].ToInt();
            newVitalRecordType = newVitalRecordType.OppositeRecordType();
            personRow[U.VitalRecordType_col] = newVitalRecordType;
            personRow[U.Sex_col] = U.RecordTypeSexChar(newVitalRecordType, personRow[U.Sex_col].ToChar());
            EVitalRecordType spouseVitalRecordType = newVitalRecordType.SpouseRecordType();
            if (spouseVitalRecordType != EVitalRecordType.eSearch)
            {
                if (VitalRecord_tbl.Rows.Count <= 1)
                {
                    return false;
                }
                DataRow spouseRow = VitalRecord_tbl.Rows[1];
                spouseRow[U.VitalRecordType_col] = (int) spouseVitalRecordType;
                spouseRow[U.Sex_col] = U.RecordTypeSexChar(spouseVitalRecordType, spouseRow[U.Sex_col].ToChar());
            }
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (UpdateWithDA(txn, VitalRecord_tbl, U.VitalRecord_Table, U.VitalRecordID_col, ColumnList(U.VitalRecordType_col, U.Sex_col)))
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
        public static bool SaveIntegratedVitalRecords(DataTable VitalRecord_tbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            if (UpdateWithDA(txn, VitalRecord_tbl, U.VitalRecord_Table, U.VitalRecordID_col, 
                                  ColumnList(U.PersonID_col, U.FatherID_col, U.MotherID_col)))
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
