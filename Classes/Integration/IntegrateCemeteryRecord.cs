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
    public class CIntegrateCemeteryRecord : CIntegration
    {
        private DataRow cemeteryRecord_row;
        public CIntegrateCemeteryRecord(CSql SQL, bool getFromGrid)
            : base(SQL, getFromGrid)
        {
        }
        //****************************************************************************************************************************
        public bool IntegrateRecord(DataRow cemeteryRecord_row,
                                     bool bPersonIntegrated,
                                     bool bFatherIntegrated,
                                     bool bMotherIntegrated,
                                     bool bSpouseIntegrated)
        {
            try
            {
                this.cemeteryRecord_row = cemeteryRecord_row;
                if (IntegratePerson(bPersonIntegrated, bFatherIntegrated,
                                                    bMotherIntegrated, bSpouseIntegrated))
                {
                    if (!bPersonIntegrated && SQL.CemeteryRecordAlreadyExists(cemeteryRecord_row[U.PersonID_col].ToInt()))
                    {
                        MessageBox.Show("This person is already integrated with another Cemetery Record");
                        return false;
                    }
                    return SQL.IntegrateCemeteryRecord(cemeteryRecord_row, Person_tbl, Marriage_tbl, getFromGrid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        //****************************************************************************************************************************
        public static eSex RecordTypeSex(char sex)
        {
            switch (sex)
            {
                case 'M': return eSex.eMale;
                case 'F': return eSex.eFemale;
                default: return eSex.eUnknown;
            }
        }
        //****************************************************************************************************************************
        private string GetMarriedName()
        {
            if (cemeteryRecord_row[U.Sex_col].ToChar() != 'F')
            {
                return cemeteryRecord_row[U.LastName_col].ToString();
            }
            string lastName = cemeteryRecord_row[U.LastName_col].ToString();
            string fathersLastName = cemeteryRecord_row[U.FatherLastName_col].ToString();
            string spouseLastName = cemeteryRecord_row[U.SpouseLastName_col].ToString();
            if (lastName != spouseLastName)
            {
                return spouseLastName;
            }
            if (lastName != fathersLastName)
            {
                return lastName;
            }
            else
            {
                return "";
            }
        }
        //****************************************************************************************************************************
        public bool IntegratePerson(bool bPersonIntegrated,
                                    bool bFatherIntegrated,
                                    bool bMotherIntegrated,
                                    bool bSpouseIntegrated)
        {
            int cemeteryBornYear = U.GetCemeteryBornYear(cemeteryRecord_row, "");
            int cemeteryDiedYear = U.GetYearFromDate(cemeteryRecord_row[U.DiedDate_col].ToString());
            string sMarriedName = GetMarriedName();
            eSex sex = RecordTypeSex(cemeteryRecord_row[U.Sex_col].ToChar());
            string sFatherLastName = cemeteryRecord_row[U.FatherLastName_col].ToString();
            PersonName personName = new PersonName(cemeteryRecord_row[U.FirstName_col].ToString(),
                                                   cemeteryRecord_row[U.MiddleName_col].ToString(),
                                                   cemeteryRecord_row[U.LastName_col].ToString(),
                                                   cemeteryRecord_row[U.Suffix_col].ToString(),
                                                   cemeteryRecord_row[U.Prefix_col].ToString());
            PersonName spouseName = new PersonName(cemeteryRecord_row[U.SpouseFirstName_col].ToString(),
                                       cemeteryRecord_row[U.SpouseMiddleName_col].ToString(),
                                       cemeteryRecord_row[U.SpouseLastName_col].ToString(),
                                       cemeteryRecord_row[U.SpouseSuffix_col].ToString(),
                                       cemeteryRecord_row[U.SpousePrefix_col].ToString());
            PersonName fatherName = new PersonName(cemeteryRecord_row[U.FatherFirstName_col].ToString(),
                                       cemeteryRecord_row[U.FatherMiddleName_col].ToString(),
                                       sFatherLastName,
                                       cemeteryRecord_row[U.FatherSuffix_col].ToString(),
                                       cemeteryRecord_row[U.FatherPrefix_col].ToString());
            PersonName motherName = new PersonName(cemeteryRecord_row[U.MotherFirstName_col].ToString(),
                                       cemeteryRecord_row[U.MotherMiddleName_col].ToString(),
                                       cemeteryRecord_row[U.MotherLastName_col].ToString(),
                                       cemeteryRecord_row[U.MotherSuffix_col].ToString(),
                                       cemeteryRecord_row[U.MotherPrefix_col].ToString());
            PersonName emptyName = new PersonName();
            if (personName.IsEmpty(getFromGrid))
            {
                return false;
            }

            CIntegrationInfo integrationInfo = new CIntegrationInfo(EVitalRecordType.eSearch, 0, sex, bPersonIntegrated, bSpouseIntegrated, 
                                                                    sMarriedName, personName, spouseName, fatherName, motherName, emptyName, emptyName,
                                                                    cemeteryRecord_row[U.PersonID_col].ToInt(), cemeteryRecord_row[U.SpouseID_col].ToInt(),
                                                                    cemeteryRecord_row[U.FatherID_col].ToInt(), cemeteryRecord_row[U.MotherID_col].ToInt(), 0, 0,
                                                                    cemeteryBornYear, cemeteryDiedYear, 0, cemeteryRecord_row[U.Notes_col].ToString());

            int iPersonID = SimilarPersonExists(integrationInfo);
            cemeteryRecord_row[U.PersonID_col] = iPersonID;
            cemeteryRecord_row[U.SpouseID_col] = integrationInfo.iSpouseId;
            if (Person_tbl.Rows.Count == 0)
            {
                return false;
            }
            if (!ValidData(iPersonID, integrationInfo.iSpouseId, "Person", "Spouse"))
            {
                return false;
            }
            if (!GetMarriageRecord(iPersonID, integrationInfo.iSpouseId))
            {
                return false;
            }

            if (fatherName.IsEmpty(getFromGrid) && motherName.IsEmpty(getFromGrid))
            {
                return true;
            }


            int fatherId = cemeteryRecord_row[U.FatherID_col].ToInt();
            int motherId = cemeteryRecord_row[U.MotherID_col].ToInt();
            if (fatherId == 0 || motherId == 0)
            {
                CIntegrationInfo fatherIntegrationInfo = new CIntegrationInfo(EVitalRecordType.eSearch, 2, bFatherIntegrated, bMotherIntegrated, 
                                                                              fatherName.lastName, fatherName, motherName, fatherId, motherId);
                fatherId = SimilarPersonExists(fatherIntegrationInfo);
                motherId = fatherIntegrationInfo.iSpouseId;
                cemeteryRecord_row[U.FatherID_col] = fatherId;
                cemeteryRecord_row[U.MotherID_col] = motherId;
            }
            if (!ValidData(fatherId, motherId, "Father", "Mother"))
            {
                return false;
            }
            if (DuplicateOfPersonID("Father", "Person", fatherId, iPersonID))
            {
                return false;
            }
            if (DuplicateOfPersonID("Mother", "Person", motherId, iPersonID))
            {
                return false;
            }
            DataRow Person_row = Person_tbl.Rows[0];
            if (UseSelectedFatherMother(Person_row, personName, fatherId, motherId))
            {
                fatherId = Person_row[U.FatherID_col].ToInt();
                motherId = Person_row[U.MotherID_col].ToInt();
                return GetMarriageRecord(fatherId, motherId);
            }
            return false;
        }
        //****************************************************************************************************************************
    }
}
