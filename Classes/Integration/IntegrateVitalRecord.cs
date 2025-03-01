using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class CIntegrateVitalRecord : CIntegration
    {
        //private bool getFromGrid;
        public CIntegrateVitalRecord(CSql SQL, bool getFromGrid)
            : base(SQL, getFromGrid)
        {
            this.getFromGrid = getFromGrid;
        }
        //****************************************************************************************************************************
        public bool IntegrateRecord(DataRow VitalRecord_row,
                                     DataRow SpouseVitalRecord_row,
                                     bool bPersonIntegrated,
                                     bool bFatherIntegrated,
                                     bool bMotherIntegrated,
                                     bool bSpouseIntegrated,
                                     bool bSpouseFatherIntegrated,
                                     bool bSpouseMotherIntegrated)
        {
            bool success = false;
            try
            {
                Person_tbl = SQL.DefinePersonTable();
                Marriage_tbl = SQL.DefineMarriageTable();
                EVitalRecordType eVitalRecordType = (EVitalRecordType)VitalRecord_row[U.VitalRecordType_col].ToInt();
                string sMarriedName = GetMarriedName(eVitalRecordType, VitalRecord_row, SpouseVitalRecord_row);
                if (IntegratePerson(eVitalRecordType.RecordTypeTitle(), eVitalRecordType.RecordTypeSex(VitalRecord_row[U.Sex_col].ToChar()),
                                VitalRecord_row, SpouseVitalRecord_row, U.PersonType,
                                bPersonIntegrated, bSpouseIntegrated, bFatherIntegrated, bMotherIntegrated, bSpouseFatherIntegrated, bSpouseMotherIntegrated, sMarriedName))
                {
                    success = SQL.IntegrateVitalRecord(VitalRecord_row, SpouseVitalRecord_row, Person_tbl, Marriage_tbl, getFromGrid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Encounter during Integration: " + ex.Message);
            }
            if (!success)
            {
                SetVitalRecordIdsToZeroIfNotIntegrated(VitalRecord_row, SpouseVitalRecord_row, bPersonIntegrated, bFatherIntegrated,
                                                       bMotherIntegrated, bSpouseIntegrated, bSpouseFatherIntegrated, bSpouseMotherIntegrated);
            }
            return success;
        }
        //****************************************************************************************************************************
        private void SetVitalRecordIdsToZeroIfNotIntegrated(DataRow VitalRecord_row,
                                     DataRow SpouseVitalRecord_row,
                                     bool bPersonIntegrated,
                                     bool bFatherIntegrated,
                                     bool bMotherIntegrated,
                                     bool bSpouseIntegrated,
                                     bool bSpouseFatherIntegrated,
                                     bool bSpouseMotherIntegrated)
        {
            if (!bPersonIntegrated)
            {
                VitalRecord_row[U.PersonID_col] = 0;
            }
            if (!bFatherIntegrated)
            {
                VitalRecord_row[U.FatherID_col] = 0;
            }
            if (!bMotherIntegrated)
            {
                VitalRecord_row[U.MotherID_col] = 0;
            }
            if (SpouseVitalRecord_row == null)
            {
                return;
            }
            if (!bSpouseIntegrated)
            {
                SpouseVitalRecord_row[U.PersonID_col] = 0;
            }
            if (!bSpouseFatherIntegrated)
            {
                SpouseVitalRecord_row[U.FatherID_col] = 0;
            }
            if (!bPersonIntegrated)
            {
                SpouseVitalRecord_row[U.MotherID_col] = 0;
            }
        }
        //****************************************************************************************************************************
        public static string GetMarriedName(EVitalRecordType eVitalRecordType,
                                     DataRow Person_row,
                                     DataRow Spouse_row)
        {
            if (eVitalRecordType == EVitalRecordType.eDeathFemale)
            {
                string lastName = Person_row[U.LastName_col].ToString();
                string fathersLastName = Person_row[U.FatherLastName_col].ToString();
                if (lastName != fathersLastName)
                {
                    return lastName;
                }
                else
                {
                    return "";
                }
            }
            else if (eVitalRecordType == EVitalRecordType.eMarriageBride && Spouse_row != null)
            {
                return Spouse_row[U.LastName_col].ToString();
            }
            if (Person_row[U.LastName_col].ToString() != Person_row[U.FatherLastName_col].ToString())
            {
                return Person_row[U.LastName_col].ToString();
            }
            return "";
        }
        //****************************************************************************************************************************
        private void SeupNames(DataRow VitalRecord_row, eSex Sex, out PersonName personName, out PersonName personFatherName, out PersonName personMotherName)
        {
            if (VitalRecord_row == null)
            {
                personName = new PersonName();
                personFatherName = new PersonName();
                personMotherName = new PersonName();
                return;
            }
            string sFatherLastName = VitalRecord_row[U.FatherLastName_col].ToString();
            VitalRecord_row[U.LastName_col] = GetMaidenName(Sex, VitalRecord_row[U.LastName_col].ToString(), sFatherLastName);
            personName = new PersonName(VitalRecord_row[U.FirstName_col].ToString(),
                                       VitalRecord_row[U.MiddleName_col].ToString(),
                                       VitalRecord_row[U.LastName_col].ToString(),
                                       VitalRecord_row[U.Suffix_col].ToString(),
                                       VitalRecord_row[U.Prefix_col].ToString());
            personFatherName = new PersonName(VitalRecord_row[U.FatherFirstName_col].ToString(),
                                       VitalRecord_row[U.FatherMiddleName_col].ToString(),
                                       sFatherLastName,
                                       VitalRecord_row[U.FatherSuffix_col].ToString(),
                                       VitalRecord_row[U.FatherPrefix_col].ToString());
            string personMotherLastName = (String.IsNullOrEmpty(VitalRecord_row[U.MotherLastName_col].ToString())) ? personFatherName.lastName : VitalRecord_row[U.MotherLastName_col].ToString();
            string motherFirstName = VitalRecord_row[U.MotherFirstName_col].ToString();
            if (!String.IsNullOrEmpty(VitalRecord_row[U.MotherLastName_col].ToString()) && String.IsNullOrEmpty(motherFirstName))
            {
                motherFirstName = "Unknown";
            }
            personMotherName = (String.IsNullOrEmpty(motherFirstName) ? personMotherName = new PersonName() :
                                personMotherName = new PersonName(motherFirstName,
                                                                  VitalRecord_row[U.MotherMiddleName_col].ToString(),
                                                                  personMotherLastName,
                                                                  VitalRecord_row[U.MotherSuffix_col].ToString(),
                                                                  VitalRecord_row[U.MotherPrefix_col].ToString()));
        }
        //****************************************************************************************************************************
        private void SetInitialVitalRecordIdToZero(DataRow vitalRecord_row,
                                                   bool bPersonIntegrated,
                                                   bool bFatherIntegrated,
                                                   bool bMotherIntegrated)
        {
            if (vitalRecord_row == null)
            {
                return;
            }
            if (!bPersonIntegrated)
            {
                vitalRecord_row[U.PersonID_col] = 0;
            }
            if (!bFatherIntegrated)
            {
                vitalRecord_row[U.FatherID_col] = 0;
            }
            if (!bMotherIntegrated)
            {
                vitalRecord_row[U.MotherID_col] = 0;
            }
        }
        //****************************************************************************************************************************
        public bool IntegratePerson(string sPersonTitle,
                                    eSex Sex,
                                    DataRow PersonVitalRecord_row,
                                    DataRow SpouseVitalRecord_row,
                                    int iVitalRecordPersonType,
                                    bool bPersonIntegrated,
                                    bool bSpouseIntegrated,
                                    bool bFatherIntegrated,
                                    bool bMotherIntegrated,
                                    bool bSpouseFatherIntegrated,
                                    bool bSpouseMotherIntegrated,
                                    string sMarriedName)
        {
            SetInitialVitalRecordIdToZero(PersonVitalRecord_row, bPersonIntegrated, bFatherIntegrated, bMotherIntegrated);
            SetInitialVitalRecordIdToZero(SpouseVitalRecord_row, bSpouseIntegrated, bSpouseFatherIntegrated, bSpouseMotherIntegrated);
            string sFatherLastName = PersonVitalRecord_row[U.FatherLastName_col].ToString();
            PersonVitalRecord_row[U.LastName_col] = GetMaidenName(Sex, PersonVitalRecord_row[U.LastName_col].ToString(), sFatherLastName);
            PersonName personName, personFatherName, personMotherName, spouseName, spouseFatherName, spouseMotherName;
            SeupNames(PersonVitalRecord_row, Sex, out personName, out personFatherName, out personMotherName);
            SeupNames(SpouseVitalRecord_row, Sex, out spouseName, out spouseFatherName, out spouseMotherName);
            EVitalRecordType vitalRecordType = (EVitalRecordType)PersonVitalRecord_row[U.VitalRecordType_col].ToInt();
            if (!getFromGrid && vitalRecordType == EVitalRecordType.eBurial)
            {
                GetParentsFromDeathRecord(PersonVitalRecord_row, personName, personFatherName, personMotherName);
            }

             EVitalRecordType eVitalRecordType = (EVitalRecordType)PersonVitalRecord_row[U.VitalRecordType_col].ToInt();
            string sBornDate = U.VitalRecordBornDate(eVitalRecordType, PersonVitalRecord_row, "");
            int vitalRecordBornYear = U.GetYearFromDate(sBornDate);
            int diedMonth;
            int vitalRecordDiedYear = GetDiedDate(eVitalRecordType, PersonVitalRecord_row, out diedMonth);
            int originalSpouseId = (SpouseVitalRecord_row == null) ? 0 : SpouseVitalRecord_row[U.PersonID_col].ToInt();
            int spouseFatherId = (SpouseVitalRecord_row == null) ? 0 : SpouseVitalRecord_row[U.FatherID_col].ToInt();
            int spouseMotherId = (SpouseVitalRecord_row == null) ? 0 : SpouseVitalRecord_row[U.MotherID_col].ToInt();

            CIntegrationInfo integrationInfo = new CIntegrationInfo(eVitalRecordType, 0, Sex, bPersonIntegrated, bSpouseIntegrated, sMarriedName,
                                                                    personName, spouseName, personFatherName, personMotherName, spouseFatherName, spouseMotherName,
                                                                    PersonVitalRecord_row[U.PersonID_col].ToInt(), originalSpouseId,
                                                                    PersonVitalRecord_row[U.FatherID_col].ToInt(), PersonVitalRecord_row[U.MotherID_col].ToInt(),
                                                                    spouseFatherId, spouseMotherId,
                                                                    vitalRecordBornYear, vitalRecordDiedYear, diedMonth, PersonVitalRecord_row[U.Notes_col].ToString());
            int iPersonID = IntegratePersonAndSpouse(integrationInfo, PersonVitalRecord_row, SpouseVitalRecord_row);
            if (iPersonID == doNotProcessId)
            {
                return false;
            }
            if (IntegrateParents(PersonVitalRecord_row, Person_tbl.Rows[0], bFatherIntegrated, bMotherIntegrated, iPersonID, 2, integrationInfo.iPersonFatherId,
                                 integrationInfo.iPersonMotherId, "Person Father", personName, personFatherName, personMotherName))
            {
                if (SpouseVitalRecord_row == null || Person_tbl.Rows.Count < 2)
                {
                    return true;
                }
                SpouseVitalRecord_row[U.FatherID_col] = spouseFatherId;
                SpouseVitalRecord_row[U.MotherID_col] = spouseMotherId;
                return IntegrateParents(SpouseVitalRecord_row, Person_tbl.Rows[1], bSpouseFatherIntegrated, bSpouseMotherIntegrated, integrationInfo.iSpouseId, 4, integrationInfo.iSpouseFatherId,
                                 integrationInfo.iSpouseMotherId, "Spouse Father", spouseName, spouseFatherName, spouseMotherName);
            }
            return false;
        }
        //****************************************************************************************************************************
        private int IntegratePersonAndSpouse(CIntegrationInfo integrationInfo,
                                             DataRow PersonVitalRecord_row,
                                             DataRow SpouseVitalRecord_row)
        {
            int iPersonID = SimilarPersonExists(integrationInfo);
            int iSpouseID = integrationInfo.iSpouseId;
            if (iPersonID == doNotProcessId || Person_tbl.Rows.Count == 0)
            {
                return doNotProcessId;
            }
            if (iSpouseID == doNotProcessId)
            {
                iSpouseID = 0;
            }
            if (!ValidData(iPersonID, iSpouseID, "Person", "Spouse"))
            {
                return doNotProcessId;
            }
            PersonVitalRecord_row[U.PersonID_col] = iPersonID;
            if (SpouseVitalRecord_row != null)
            {
                SpouseVitalRecord_row[U.PersonID_col] = iSpouseID;
                if (!GetMarriageRecord(iPersonID, integrationInfo.iSpouseId))
                {
                    return doNotProcessId;
                }
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private bool IntegrateParents(DataRow VitalRecord_row,
                                      DataRow Person_row,
                                      bool bFatherIntegrated,
                                      bool bMotherIntegrated,
                                      int iPersonID,
                                      int iPersonType,
                                      int fatherId,
                                      int motherId,
                                      string sPersonTitle,
                                      PersonName personName,
                                      PersonName fatherName,
                                      PersonName motherName)
        {
            if (fatherName.lastName.Length == 0 && fatherName.firstName.Length == 0 &&
                motherName.lastName.Length == 0 && motherName.firstName.Length == 0)
            {
                VitalRecord_row[U.FatherID_col] = 0;
                VitalRecord_row[U.MotherID_col] = 0;
                return true;
            }
            if (fatherId == 0 || motherId == 0)
            {
                PersonName emptyName = new PersonName();
                CIntegrationInfo fatherIntegrationInfo = new CIntegrationInfo(EVitalRecordType.eSearch, iPersonType, bFatherIntegrated, bMotherIntegrated,
                                                             fatherName.lastName, fatherName, motherName, fatherId, motherId);
                fatherId = SimilarPersonExists(fatherIntegrationInfo);
                motherId = fatherIntegrationInfo.iSpouseId;
            }
            if (!ValidData(fatherId, motherId, "Father", "Mother"))
            {
                return false;
            }
            if (DuplicateOfPersonID("Father", sPersonTitle, fatherId, iPersonID))
            {
                return false;
            }
            if (DuplicateOfPersonID("Mother", sPersonTitle, motherId, iPersonID))
            {
                return false;
            }
            if (UseSelectedFatherMother(Person_row, personName, fatherId, motherId))
            {
                VitalRecord_row[U.FatherID_col] = fatherId;
                VitalRecord_row[U.MotherID_col] = motherId;
                fatherId = Person_row[U.FatherID_col].ToInt();
                motherId = Person_row[U.MotherID_col].ToInt();
                return GetMarriageRecord(fatherId, motherId);
            }
            return false;
        }
        //****************************************************************************************************************************
        private int GetDiedDate(EVitalRecordType eVitalRecordType,
                                DataRow VitalRecord_row, out int diedMonth)
        {
            if (eVitalRecordType == EVitalRecordType.eDeathFemale || eVitalRecordType == EVitalRecordType.eDeathMale || eVitalRecordType == EVitalRecordType.eBurial)
            {
                diedMonth = VitalRecord_row[U.DateMonth_col].ToInt();
                return VitalRecord_row[U.DateYear_col].ToInt();
            }
            else
            {
                diedMonth = 0;
                return 0;
            }
        }
        //****************************************************************************************************************************
        private string GetMaidenName(eSex Sex,
                                     string lastName,
                                     string fatherLastName)
        {
            if (Sex != eSex.eFemale)
            {
                return lastName;
            }
            if (fatherLastName.Length == 0)
            {
                return lastName;
            }
            else
            {
                return fatherLastName;
            }
        }
        //****************************************************************************************************************************
        private string GetSex(eSex Sex)
        {
            switch (Sex)
            {
                case eSex.eMale: return "M";
                case eSex.eFemale: return "F";
                default: return " ";
            }
        }
        //****************************************************************************************************************************
        public void GetParentsFromDeathRecord(DataRow VitalRecord_row,
                                              PersonName personName,
                                              PersonName fatherName,
                                              PersonName motherName)
        {
            EVitalRecordType deathType;
            switch (VitalRecord_row[U.Sex_col].ToChar())
            {
                case 'M': deathType = EVitalRecordType.eDeathMale; break;
                case 'F': deathType = EVitalRecordType.eDeathFemale; break;
                default: return;
           }
            VitalRecordProperties vitalRecordProperties = new VitalRecordProperties("", "", VitalRecord_row[U.DateYear_col].ToInt(),
                                                                                            VitalRecord_row[U.DateMonth_col].ToInt(),
                                                                                            VitalRecord_row[U.DateDay_col].ToInt(),
                                                                                            ' ', "", "", "", 0, 0, 0, ' ', 0, 0, 0, ' ', false);
            DataTable vitalRecord_tbl = SQL.DefineVitalRecord_Table();
            SQL.GetVitalRecordFromNameDate(vitalRecord_tbl, deathType, personName, vitalRecordProperties);
            if (vitalRecord_tbl.Rows.Count == 0)
            {
                return;
            }
            VitalRecord_row = vitalRecord_tbl.Rows[0];
            fatherName.firstName = VitalRecord_row[U.FatherFirstName_col].ToString();
            fatherName.middleName = VitalRecord_row[U.FatherMiddleName_col].ToString();
            fatherName.lastName = VitalRecord_row[U.FatherLastName_col].ToString();
            fatherName.suffix = VitalRecord_row[U.FatherSuffix_col].ToString();
            fatherName.prefix = VitalRecord_row[U.FatherPrefix_col].ToString();
            string motherFirstName = VitalRecord_row[U.MotherFirstName_col].ToString();
            if (!String.IsNullOrEmpty(motherFirstName))
            {
                string motherLastName = (String.IsNullOrEmpty(VitalRecord_row[U.MotherLastName_col].ToString())) ? fatherName.lastName : VitalRecord_row[U.MotherLastName_col].ToString();
                motherName.firstName = motherFirstName;
                motherName.middleName = VitalRecord_row[U.MotherMiddleName_col].ToString();
                motherName.lastName = motherLastName;
                motherName.suffix = VitalRecord_row[U.MotherSuffix_col].ToString();
                motherName.prefix = VitalRecord_row[U.MotherPrefix_col].ToString();
            }
        }
        //****************************************************************************************************************************
    }
}
