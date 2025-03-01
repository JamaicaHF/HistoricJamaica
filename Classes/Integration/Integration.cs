using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class CIntegration
    {
        protected const int doNotProcessId = 999999;
        protected bool getFromGrid;
        protected DataTable Person_tbl;
        protected DataTable Marriage_tbl;
        protected CIntegrationInfo II;
        protected CSql m_SQL;

        protected class CIntegrationInfo
        {
            public EVitalRecordType vitalRecordType;
            public string recordBornDate;
            public int recordBornYear;
            public int recordDiedYear;
            public int recordDiedMonth;
            public eSex Sex;
            public int iPersonType;
            public int iOriginalPersonID;
            public int iSpouseId;
            public int iPersonFatherId;
            public int iPersonMotherId;
            public int iSpouseFatherId;
            public int iSpouseMotherId;
            public bool bPersonNameIntegratedChecked;
            public bool bSpouseNameIntegratedChecked;
            public string sMarriedName;
            public PersonName personName;
            public PersonName spouseName;
            public PersonName fatherName;
            public PersonName motherName;
            public PersonName spouseFatherName;
            public PersonName spouseMotherName;
            public string sNotes;
            public CIntegrationInfo(EVitalRecordType vitalRecordType,
                                    int iPersonType,
                                    eSex Sex,
                                    bool bPersonNameIntegratedChecked,
                                    bool bSpouseNameIntegratedChecked,
                                    string sMarriedName,
                                    PersonName personName,
                                    PersonName spouseName,
                                    PersonName fatherName,
                                    PersonName motherName,
                                    PersonName spouseFatherName,
                                    PersonName spouseMotherName,
                                    int iOriginalPersonID,
                                    int iSpouseId,
                                    int iPersonFatherId,
                                    int iPersonMotherId,
                                    int iSpouseFatherId,
                                    int iSpouseMotherId,
                                    int recordBornYear,
                                    int recordDiedYear,
                                    int recordDiedMonth,
                                    string sNotes)
            {
                this.vitalRecordType = vitalRecordType;
                this.iOriginalPersonID = iOriginalPersonID;
                this.iSpouseId = iSpouseId;
                this.iPersonFatherId = iPersonFatherId;
                this.iPersonMotherId = iPersonMotherId;
                this.iSpouseFatherId = iSpouseFatherId;
                this.iSpouseMotherId = iSpouseMotherId;
                this.recordBornYear = recordBornYear;
                this.recordDiedYear = recordDiedYear;
                this.recordDiedMonth = recordDiedMonth;
                this.iPersonType = iPersonType;
                this.Sex = Sex;
                this.sMarriedName = sMarriedName;
                this.personName = personName;
                this.spouseName = spouseName;
                this.fatherName = fatherName;
                this.motherName = motherName;
                this.spouseFatherName = spouseFatherName;
                this.spouseMotherName = spouseMotherName;
                this.bPersonNameIntegratedChecked = bPersonNameIntegratedChecked;
                this.bSpouseNameIntegratedChecked = bSpouseNameIntegratedChecked;
                this.sNotes = sNotes;
                this.recordBornDate = "";
            }
            public CIntegrationInfo(EVitalRecordType vitalRecordType,
                                    int iPersonType,
                                    bool bPersonNameIntegratedChecked,
                                    bool bSpouseNameIntegratedChecked,
                                    string sMarriedName,
                                    PersonName personName,
                                    PersonName spouseName,
                                    int iOriginalPersonID,
                                    int iSpouseId)
            {
                Sex = eSex.eMale;
                iPersonFatherId = 0;
                iPersonMotherId = 0;
                iSpouseFatherId = 0;
                iSpouseMotherId = 0;
                recordBornYear = 0;
                recordDiedYear = 0;
                recordDiedMonth = 0;
                sNotes = "";
                fatherName = new PersonName();
                motherName = new PersonName();
                spouseFatherName = new PersonName();
                spouseMotherName = new PersonName();
                this.personName = personName;
                this.spouseName = spouseName;
                this.iOriginalPersonID = iOriginalPersonID;
                this.vitalRecordType = vitalRecordType;
                this.iSpouseId = iSpouseId;
                this.iPersonType = iPersonType;
                this.sMarriedName = sMarriedName;
                this.bPersonNameIntegratedChecked = bPersonNameIntegratedChecked;
                this.bSpouseNameIntegratedChecked = bSpouseNameIntegratedChecked;
                this.recordBornDate = "";
            }
            public CIntegrationInfo(PersonName personName, int recordBornYear, eSex Sex, bool bPersonNameIntegratedChecked, string recordBornDate)
            {
                this.vitalRecordType = EVitalRecordType.eBirthFemale;
                this.personName = personName;
                this.recordBornYear = recordBornYear;
                this.Sex = Sex;
                this.bPersonNameIntegratedChecked = bPersonNameIntegratedChecked;
                this.spouseName = new PersonName();
                this.fatherName = new PersonName();
                this.motherName = new PersonName();
                this.spouseFatherName = new PersonName();
                this.spouseMotherName = new PersonName();
                this.iPersonType = 0;
                this.bPersonNameIntegratedChecked = bPersonNameIntegratedChecked;
                this.bSpouseNameIntegratedChecked = false;
                this.sMarriedName = "";
                this.iOriginalPersonID = 0;
                this.iSpouseId = 0;
                this.iPersonFatherId = 0;
                this.iPersonMotherId = 0;
                this.iSpouseFatherId = 0;
                this.iSpouseMotherId = 0;
                this.recordDiedYear = 0;
                this.recordDiedMonth = 0;
                this.sNotes = "";
                this.recordBornDate = recordBornDate;
            }
        }
        //****************************************************************************************************************************
        public CIntegration(CSql cSQL, bool getFromGrid)
        {
            m_SQL = cSQL;
            this.getFromGrid = getFromGrid;
            Person_tbl = SQL.DefinePersonTable();
            Marriage_tbl = SQL.DefineMarriageTable();
        }
        //****************************************************************************************************************************
        protected int SimilarPersonExists(CIntegrationInfo II)
        {
            this.II = II;
            string sMaidenName = MaidenName(II.Sex, II.personName.lastName, II.fatherName.lastName);
            DataTable SimilarPerson_tbl;
            if (!getFromGrid && II.personName.firstName.Length < 3 && !String.IsNullOrEmpty(II.personName.middleName))
            {
                PersonName personMiddleName = new PersonName(II.personName.middleName, "", II.personName.lastName, "", "");
                SimilarPerson_tbl = GetSimilarPersons(personMiddleName, sMaidenName);
            }
            else
            {
                SimilarPerson_tbl = GetSimilarPersons(II.personName, sMaidenName);
            }
            if (getFromGrid)
            {
                return GetPersonAndSpouseFromGrid(SimilarPerson_tbl, II.personName, II.spouseName, II.iPersonType, II.Sex);
            }
            else if (SimilarPerson_tbl.Rows.Count == 0)
            {
                return AddNewPersonAndSpouse(SimilarPerson_tbl, II.personName, II.spouseName, II.iPersonType, II.Sex);
            }
            else
            {
                return GetPersonFromRules(SimilarPerson_tbl, II.personName, II.spouseName, II.fatherName, II.motherName, II.iPersonType, II.Sex);
            }
        }
        //****************************************************************************************************************************
        private int PersonSpouseId(PersonName spouseName,
                                   int personType,
                                   eSex sex)
        {
            if (spouseName.IsEmpty(getFromGrid))
            {
                II.iSpouseId = doNotProcessId;
                return doNotProcessId;
            }
            DataTable spouseSimilarPerson_tbl = GetSimilarPersons(spouseName, MaidenName(sex.SpouseSex(), spouseName.lastName, ""));
            if (spouseSimilarPerson_tbl.Rows.Count == 0)
            {
                II.iSpouseId = 0;
                AddPersonToTable(II.iSpouseId, personType, spouseName, sex);
                return 0;
            }
            else
            {
                II.iSpouseId = doNotProcessId;
                return doNotProcessId;
            }
        }
        //****************************************************************************************************************************
        private int GetSpouseFromDate(PersonName spouseName,
                                      eSex Sex)
        {
            DataTable SimilarPerson_tbl = GetSimilarPersons(spouseName, MaidenName(Sex.SpouseSex(), spouseName.lastName, ""));
            if (SimilarPerson_tbl.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return ChoosePersonFromDates(SimilarPerson_tbl);
            }
        }
        //****************************************************************************************************************************
        private int ChoosePersonFromDates(DataTable SimilarPerson_tbl)
        {
            if (II.recordBornYear == 0 && II.recordDiedYear == 0)
            {
                return doNotProcessId;
            }
            int finalPersonId = doNotProcessId;
            string finalDiedDate = "";
            bool allBornDatesNotZero = true;
            bool allDiedDatesNotZero = true;
            bool ExactMatcheAlreadyFound = false;
            bool multipleExactMatches = false;
            foreach (DataRow SimilarPerson_row in SimilarPerson_tbl.Rows)
            {
                string bornDate, diedDate;
                bool exactMatch;
                int personId = CheckPersonBornDate(SimilarPerson_row[U.PersonID_col].ToInt(), out bornDate, out diedDate, out exactMatch);
                if (exactMatch)
                {
                    if (ExactMatcheAlreadyFound)
                    {
                        multipleExactMatches = true;
                    }
                    ExactMatcheAlreadyFound = true;
                }
                if (String.IsNullOrEmpty(diedDate))
                {
                    allDiedDatesNotZero = false;
                }
                if (String.IsNullOrEmpty(bornDate))
                {
                    allBornDatesNotZero = false;
                }
                if (personId != doNotProcessId)
                {
                    if (finalPersonId == doNotProcessId || finalPersonId == 0)
                    {
                        finalPersonId = personId;
                        finalDiedDate = diedDate;
                    }
                    else if (personId != 0)
                    {
                        return doNotProcessId; // can not distinguish between the two
                    }
                }
            }
            if (II.vitalRecordType == EVitalRecordType.eBurial)
            {
                if (BurialRecordAndDiedDateVerySimilar(finalDiedDate))
                {
                    return finalPersonId;
                }
                return (allBornDatesNotZero || allDiedDatesNotZero) ? finalPersonId : doNotProcessId;
            }
            if (ExactMatcheAlreadyFound && !multipleExactMatches)
            {
                return finalPersonId;
            }
            return (allBornDatesNotZero) ? finalPersonId : doNotProcessId;
        }
        //****************************************************************************************************************************
        private bool BurialRecordAndDiedDateVerySimilar(string finalDiedDate)
        {
            int year, month, day;
            U.GetYMD(finalDiedDate, out year, out month, out day);
            if (year == II.recordDiedYear && month == II.recordDiedMonth)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private int CheckPersonBornDate(int personId,
                                    out string bornDate,
                                    out string diedDate,
                                    out bool exactMatch)
        {
            int bornYear, diedYear;
            exactMatch = false;
            string sBornPlace, sBornHome, sDiedPlace, sDiedHome;
            DataRow person_row = SQL.GetPerson(personId);
            SQL.GetBornDiedDate(personId, person_row, out bornDate, out sBornPlace, out sBornHome, out diedDate, out sDiedPlace, out sDiedHome);
            bornYear = U.GetYearFromDate(bornDate);
            diedYear = U.GetYearFromDate(diedDate);
            int finalPersonId = 0;
            if (diedYear != 0 && II.recordDiedYear != 0)
            {
                if (II.recordDiedYear > diedYear - 2 && II.recordDiedYear < diedYear + 2)
                {
                    finalPersonId = personId;
                }
                else
                {
                    return 0;
                }
            }
            if (bornYear != 0 && II.recordBornYear != 0)
            {
                if (!String.IsNullOrEmpty(bornDate) && II.recordBornDate == bornDate)
                {
                    finalPersonId = personId;
                    exactMatch = true;
                }
                else if (II.recordBornYear == bornYear && II.recordBornYear == bornYear)
                {
                    finalPersonId = personId;
                    exactMatch = true;
                }
                else if (II.recordBornYear >= bornYear - 2 && II.recordBornYear <= bornYear + 2)
                {
                    finalPersonId = personId;
                }
                else
                {
                    return 0;
                }

            }
            return finalPersonId;
        }
        //****************************************************************************************************************************
        public string MaidenName(eSex Sex,
                                 string lastName,
                                 string fathersLastName)
        {
            if (Sex != eSex.eFemale || fathersLastName.Length == 0)
            {
                return lastName;
            }
            if (lastName != fathersLastName)
            {
                return fathersLastName;
            }
            else
            {
                return lastName;
            }
        }
        //****************************************************************************************************************************
        private int GetPersonFromRules(DataTable SimilarPerson_tbl,
                                       PersonName personName,
                                       PersonName spouseName,
                                       PersonName fatherName,
                                       PersonName motherName,
                                       int iPersonType,
                                       eSex Sex)
        {
            int iPersonID = 0;
            if (!getFromGrid && personName.firstName.Length < 3 && personName.middleName.Length < 3)
            {
                II.iSpouseId = doNotProcessId;
                return doNotProcessId;
            }
            if (spouseName.IsEmpty(getFromGrid))
            {
                iPersonID = 0;
                II.iSpouseId = doNotProcessId;
                if (!fatherName.IsEmpty(getFromGrid) || !motherName.IsEmpty(getFromGrid))
                {
                    iPersonID = ChoosePersonFromFatherMother(SimilarPerson_tbl, fatherName, motherName, ref II.iPersonFatherId, ref II.iPersonMotherId);
                }
                if (iPersonID == 0)
                {
                    iPersonID = ChoosePersonFromDates(SimilarPerson_tbl);
                }
                AddPersonAndSpouseToPersonTable(iPersonID, II.iSpouseId, iPersonType, Sex, personName, spouseName);
                II.iSpouseId = 0;
            }
            else
            {
                int originalSpouseId = II.iSpouseId;
                bool noNameMatches;
                ChoosePersonFromRules(SimilarPerson_tbl, personName, spouseName, out iPersonID, out II.iSpouseId, out noNameMatches);
                if (iPersonID != doNotProcessId && iPersonID != 0)
                {
                    AddPersonAndSpouseToPersonTable(iPersonID, II.iSpouseId, iPersonType, Sex, personName, spouseName);
                    return iPersonID;
                }
                if (noNameMatches)
                {
                    iPersonID = AddNewPersonAndSpouse(SimilarPerson_tbl, II.personName, II.spouseName, II.iPersonType, II.Sex);
                }
                if (iPersonID == doNotProcessId)
                {
                    if (iPersonType == 0) // search for person, not father. Try Date
                    {
                        iPersonID = ChooseBothPersonAndSpouseFromParents(SimilarPerson_tbl, Sex, personName, spouseName);
                        //iPersonID = ChoosePersonFromDates(SimilarPerson_tbl);
                        //II.iSpouseId = GetSpouseFromDate(spouseName, Sex);
                    }
                    if (iPersonID == doNotProcessId && noNameMatches)
                    {
                        II.iSpouseId = 0;
                        iPersonID = 0;
                        AddPersonAndSpouseToPersonTable(iPersonID, II.iSpouseId, iPersonType, Sex, personName, spouseName);
                    }
                }
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private void AddPersonAndSpouseToPersonTable(int iPersonID,
                                                     int iSpouseId,
                                                     int iPersonType,
                                                     eSex Sex,
                                                     PersonName personName,
                                                     PersonName spouseName)
        {
            bool gayMarriage = GayMarriage(personName, spouseName);

            if (iPersonID != doNotProcessId)
            {
                AddPersonToTable(iPersonID, iPersonType, personName, Sex, gayMarriage);
            }
            if (iSpouseId != doNotProcessId)
            {
                AddPersonToTable(iSpouseId, iPersonType + 1, spouseName, Sex.SpouseSex(), gayMarriage);
            }
        }
        //****************************************************************************************************************************
        private int ChooseBothPersonAndSpouseFromParents(DataTable SimilarPerson_tbl,
                                                         eSex Sex,
                                                         PersonName personName,
                                                         PersonName spouseName)
        {
            int iPersonID = ChoosePersonFromFatherMother(SimilarPerson_tbl, II.fatherName, II.motherName, ref II.iPersonFatherId, ref II.iPersonMotherId);
            if (iPersonID == doNotProcessId)
            {
                return iPersonID;
            }
            DataTable SpouseSimilarPerson_tbl = GetSimilarPersons(II.spouseName, MaidenName(II.Sex.SpouseSex(), II.spouseName.lastName, II.spouseFatherName.lastName));
            II.iSpouseId = ChoosePersonFromFatherMother(SpouseSimilarPerson_tbl, II.spouseFatherName, II.spouseMotherName, ref II.iSpouseFatherId, ref II.iSpouseMotherId);
            if (II.iSpouseId == doNotProcessId)
            {
                return iPersonID;
            }
            if (iPersonID == 0 && II.iSpouseId == 0)
            {
                return iPersonID;
            }
            AddPersonAndSpouseToPersonTable(iPersonID, II.iSpouseId, 0, Sex, personName, spouseName);
            if (II.iPersonFatherId != 0)
            {
                AddPersonAndSpouseToPersonTable(II.iPersonFatherId, II.iPersonMotherId, 2, eSex.eMale, II.fatherName, II.motherName);
            }
            if (II.iSpouseFatherId != 0)
            {
                AddPersonAndSpouseToPersonTable(II.iSpouseFatherId, II.iSpouseMotherId, 4, eSex.eMale, II.spouseFatherName, II.spouseMotherName);
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private void ChoosePersonFromRules(DataTable SimilarPerson_tbl,
                                           PersonName personName,
                                           PersonName spouseName,
                                          out int finalPersonId,
                                          out int finalSpouseId,
                                          out bool noNameMatches)
        {
            finalPersonId = doNotProcessId;
            finalSpouseId = doNotProcessId;
            noNameMatches = true;
            if (spouseName.IsEmpty(getFromGrid))
            {
                return;
            }
            foreach (DataRow SimilarPerson_row in SimilarPerson_tbl.Rows)
            {
                if (!PersonHasSameName(SimilarPerson_row, personName, II.sMarriedName))
                {
                    continue;
                }
                noNameMatches = false;
                int personId = SimilarPerson_row[U.PersonID_col].ToInt();
                DataTable Marriage_tbl = new DataTable();
                SQL.GetAllSpouses(Marriage_tbl, personId);
                foreach (DataRow Marriage_row in Marriage_tbl.Rows)
                {
                    int spouseId = 0;
                    int spouse1 = Marriage_row[U.PersonID_col].ToInt();
                    int spouse2 = Marriage_row[U.SpouseID_col].ToInt();
                    if (personId == spouse1)
                        spouseId = spouse2;
                    else if (personId == spouse2)
                        spouseId = spouse1;
                    DataRow spouseRow = SQL.GetPerson(spouseId);
                    if (spouseRow != null && PersonHasSameName(spouseRow, spouseName, II.sMarriedName))
                    {
                        if (finalPersonId == doNotProcessId && finalSpouseId == doNotProcessId)
                        {
                            if (finalPersonId != doNotProcessId)
                            {
                                finalPersonId = doNotProcessId;
                                finalSpouseId = doNotProcessId;
                                return; // can not distinguish between the two
                            }
                            finalPersonId = personId;
                            finalSpouseId = spouseId;
                        }
                        else
                        {
                            return;  // duplicate possibilities
                        }
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private int ChoosePersonFromFatherMother(DataTable SimilarPerson_tbl,
                                                PersonName fatherName,
                                                PersonName motherName, 
                                            ref int returnFatherId, 
                                            ref int returnMotherId)
        {
            int finalPersonId = 0;
            int finalFatherId = 0;
            int finalMotherId = 0;
            bool allPersonsHaveParents = true;
            foreach (DataRow SimilarPerson_row in SimilarPerson_tbl.Rows)
            {
                int fatherPersonId = doNotProcessId;
                int motherPersonId = doNotProcessId;
                if (SimilarPerson_row[U.FatherID_col].ToInt() == 0 || SimilarPerson_row[U.FatherID_col].ToInt() == 0)
                {
                    allPersonsHaveParents = false;
                }
                else
                {
                    int fatherId = SimilarPerson_row[U.FatherID_col].ToInt();
                    if (ParentMatch(fatherName, fatherId, fatherName.lastName))
                    {
                        fatherPersonId = SimilarPerson_row[U.PersonID_col].ToInt();
                    }
                    int motherId = SimilarPerson_row[U.MotherID_col].ToInt();
                    if (ParentMatch(motherName, motherId, fatherName.lastName))
                    {
                        motherPersonId = SimilarPerson_row[U.PersonID_col].ToInt();
                    }
                    if (fatherPersonId != doNotProcessId && motherPersonId != doNotProcessId && fatherPersonId == motherPersonId)
                    {
                        if (finalPersonId == 0 || finalPersonId == fatherPersonId)
                        {
                            finalPersonId = fatherPersonId;
                            finalFatherId = fatherId;
                            finalMotherId = motherId;
                        }
                        else
                        {
                            return doNotProcessId; // can not distinguish between the two
                        }
                    }
                }
            }
            if (finalPersonId == 0)
            {
                return (allPersonsHaveParents) ? finalPersonId : doNotProcessId;
            }
            return CheckNewParentIds(finalPersonId, finalFatherId, finalMotherId, ref returnFatherId, ref returnMotherId);
        }
        //****************************************************************************************************************************
        private int CheckNewParentIds(int personId,
                                      int fatherId,
                                      int motherId,
                                      ref int originalFatherId,
                                      ref int originalMotherId)
        {
            if (originalFatherId != 0 && originalFatherId != fatherId)
            {
                return doNotProcessId;
            }
            if (originalMotherId != 0 && originalMotherId != motherId)
            {
                return doNotProcessId;
            }
            originalFatherId = fatherId;
            originalMotherId = motherId;
            return personId;
        }
        //****************************************************************************************************************************
        private bool ParentMatch(PersonName parentName,
                                 int similarParentId,
                                 string fatherLastName)
        {
            if (similarParentId == 0)
            {
                return false;
            }
            DataRow similarParentRow = SQL.GetPerson(similarParentId);
            if (PersonHasSameName(similarParentRow, parentName, fatherLastName))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool PersonHasSameName(DataRow person_row,
                                       PersonName thisPersonName,
                                       string marriedName)
        {
            if (person_row == null)
            {
                return false;
            }
            if (!PersonHasSameFirstName(person_row, thisPersonName))
            {
                return false;
            }
            if (person_row[U.LastName_col].ToString() == thisPersonName.lastName)
            {
                return true;
            }
            if (person_row[U.Sex_col].ToChar() != 'M')
            {
                if (person_row[U.MarriedName_col].ToString() == thisPersonName.lastName || person_row[U.MarriedName_col].ToString() == marriedName)
                {
                    return true;
                }
            }
            bool success = AlternativePersonHasSameName(person_row, person_row[U.LastName_col].ToString(), thisPersonName);
            if (!success && person_row[U.Sex_col].ToChar() != 'M')
            {
                return AlternativePersonHasSameName(person_row, person_row[U.MarriedName_col].ToString(), thisPersonName);
            }
            return success;
        }
        //****************************************************************************************************************************
        private bool AlternativePersonHasSameName(DataRow person_row,
                                                  string lastName,
                                                  PersonName thisPersonName)
        {
            DataTable LastNameAlternativeSpellings_tbl = SQL.GetAlternativeSpellings(U.AlternativeSpellingsLastName_Table, lastName);
            foreach (DataRow LastNameAlternativeSpellings_row in LastNameAlternativeSpellings_tbl.Rows)
            {
                string alternatelastName = LastNameAlternativeSpellings_row[U.AlternativeSpelling_Col].ToString();
                if (alternatelastName == thisPersonName.lastName)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool PersonHasSameFirstName(DataRow person_row,
                                            PersonName thisPersonName)
        {
            bool success = PersonHasSameFirstOrMiddleName(person_row[U.FirstName_col].ToString(), thisPersonName.firstName);
            if (!success)
            {
                success = PersonHasSameFirstOrMiddleName(person_row[U.MiddleName_col].ToString(), thisPersonName.firstName);
            }
            if (!success)
            {
                success = PersonHasSameFirstOrMiddleName(person_row[U.KnownAs_col].ToString(), thisPersonName.firstName);
            }
            if (!success)
            {
                success = PersonHasSameFirstOrMiddleName(person_row[U.FirstName_col].ToString(), thisPersonName.middleName);
            }
            if (!success)
            {
                success = PersonHasSameFirstOrMiddleName(person_row[U.MiddleName_col].ToString(), thisPersonName.middleName);
            }
            if (!success)
            {
                success = PersonHasSameFirstOrMiddleName(person_row[U.KnownAs_col].ToString(), thisPersonName.middleName);
            }
            return success;
        }
        //****************************************************************************************************************************
        private bool PersonHasSameFirstOrMiddleName(string name,
                                                    string thisPersonName)
        {
            if (String.IsNullOrEmpty(thisPersonName))
            {
                return false;
            }
            if (name == thisPersonName)
            {
                return true;
            }
            DataTable FirstNameAlternativeSpellings_tbl = SQL.GetAlternativeSpellings(U.AlternativeSpellingsFirstName_Table, name);
            foreach (DataRow FirstNameAlternativeSpellings_row in FirstNameAlternativeSpellings_tbl.Rows)
            {
                if (FirstNameAlternativeSpellings_row[U.AlternativeSpelling_Col].ToString() == thisPersonName)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private DataTable GetSimilarPersons(PersonName personName,
                                            string sMaidenName)
        {
            DataTable SimilarPerson_tbl = SQL.DefinePersonTable();
            if (personName.lastName.Length != 0)
            {
                SQL.PersonExists(SimilarPerson_tbl, true, U.Person_Table, U.PersonID_col, personName);
            }
            if (II.sMarriedName.Length != 0)
            {
                PersonName marriedName = new PersonName(personName);
                marriedName.lastName = II.sMarriedName;
                SQL.PersonExists(SimilarPerson_tbl, true, U.Person_Table, U.PersonID_col, marriedName);
            }
            if (sMaidenName.Length != 0 && sMaidenName != personName.lastName)
            {
                PersonName maidenName = new PersonName(personName);
                maidenName.lastName = sMaidenName;
                SQL.PersonExists(SimilarPerson_tbl, true, U.Person_Table, U.PersonID_col, maidenName);
            }
            return SimilarPerson_tbl;
        }
        //****************************************************************************************************************************
        private bool PersonAlreadyExistsOrEmptyName(PersonName personName,
                                                    int iPersonId,
                                                    int iPersonType,
                                                    bool bNameIntegratedChecked)
        {
            if (personName.lastName.Length == 0 && personName.firstName.Length == 0)
                return true;
            if (personName.lastName.Length == 0 && II.sMarriedName.Length == 0)
                return true;
            if (bNameIntegratedChecked) // be sure there really is an integrated name
            {
                if (GetPersonIfExists(iPersonId, iPersonType))
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool GetPersonIfExists(int iPersonID,
                                       int iPersonType)
        {
            if (iPersonID == 0)
                return false;
            int iCountBefore = Person_tbl.Rows.Count;
            SQL.GetPerson(Person_tbl, iPersonID);
            if (Person_tbl.Rows.Count == iCountBefore)
                return false;
            else
            {
                int iIndex = Person_tbl.Rows.Count - 1;
                Person_tbl.Rows[iIndex][U.ImportPersonID_col] = iPersonType;
                return true;
            }
        }
        //****************************************************************************************************************************
        private int GetPersonFromGrid(DataTable SimilarPerson_tbl,
                                      string sGridFormTitle,
                                      PersonName personName,
                                      int iPersonID,
                                      int iPersonType,
                                      eSex sex)
        {
            int iNewPersonID = ChoosePersonFromGrid(ref SimilarPerson_tbl, sGridFormTitle);
            if (iNewPersonID == U.Exception)
                return U.Exception;
            if (iPersonID != 0 && iPersonID == iNewPersonID)
            {
                if (GetPersonIfExists(iPersonID, iPersonType))
                    return iPersonID;
                else
                    return U.Exception;
            }
            AddPersonToTable(iNewPersonID, iPersonType, personName, sex);
            return iNewPersonID;
        }
        //****************************************************************************************************************************
        private int ChoosePersonFromGrid(ref DataTable SimilarPerson_tbl, string sGridFormTitle)
        {
            CGridPerson GridDataViewPerson = new CGridPerson(m_SQL, ref SimilarPerson_tbl, false, 0, true, sGridFormTitle);
            GridDataViewPerson.ShowDialog();
            return GridDataViewPerson.SelectedPersonID;
        }
        //****************************************************************************************************************************
        private int GetPersonAndSpouseFromGrid(DataTable SimilarPerson_tbl,
                                        PersonName personName,
                                        PersonName spouseName,
                                        int iPersonType,
                                        eSex Sex)
        {
            int iPersonID = 0;
            if (PersonAlreadyExistsOrEmptyName(personName, II.iOriginalPersonID, iPersonType, II.bPersonNameIntegratedChecked))
            {
                iPersonID = II.iOriginalPersonID;
            }
            else
            {
                if (SimilarPerson_tbl.Rows.Count == 0)
                {
                    AddPersonToTable(iPersonID, iPersonType, personName, Sex);
                }
                else
                {
                    iPersonID = GetPersonFromGrid(SimilarPerson_tbl, GridTitle(iPersonType), personName, iPersonID, iPersonType, Sex);
                    if (iPersonID == U.Exception)
                    {
                        return U.Exception;
                    }
                }
            }
            if (PersonAlreadyExistsOrEmptyName(spouseName, II.iSpouseId, iPersonType + 1, II.bSpouseNameIntegratedChecked))
            {
                return iPersonID;
            }
            SimilarPerson_tbl.Clear();
            SimilarPerson_tbl = GetSimilarPersons(spouseName, MaidenName(Sex.SpouseSex(), spouseName.lastName, ""));
            if (SimilarPerson_tbl.Rows.Count == 0)
            {
                II.iSpouseId = 0;
                AddPersonToTable(II.iSpouseId, iPersonType + 1, spouseName, Sex.SpouseSex());
            }
            else
            {
                II.iSpouseId = GetPersonFromGrid(SimilarPerson_tbl, GridTitle(iPersonType + 1), spouseName, II.iSpouseId, iPersonType + 1, Sex.SpouseSex());
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private string GridTitle(int iPersonType)
        {
            switch (iPersonType)
            {
                default:
                case 0: return "Person";
                case 1: return "Spouse";
                case 2: return "Person Father";
                case 3: return "Person Mother";
                case 4: return "Spouse Father";
                case 5: return "Spouse Mother";
            }
        }
        //****************************************************************************************************************************
        private int AddNewPersonAndSpouse(DataTable SimilarPerson_tbl,
                                          PersonName personName,
                                          PersonName spouseName,
                                          int iPersonType,
                                          eSex Sex)
        {
            int iPersonID = 0;
            if (iPersonType == 0 && !spouseName.IsEmpty(getFromGrid))
            {
                iPersonID = ChooseBothPersonAndSpouseFromParents(SimilarPerson_tbl, Sex, personName, spouseName);
                if (iPersonID != 0 || II.iSpouseId != 0)  // could be doNotProcessId or valid ID
                {
                    return iPersonID;
                }
            }
            II.iSpouseId = doNotProcessId;
            bool gayMarriage = GayMarriage(personName, spouseName);
            AddPersonToTable(iPersonID, iPersonType, personName, Sex);
            if (spouseName.IsEmpty(getFromGrid))
            {
                II.iSpouseId = 0;
            }
            else
            {
                SimilarPerson_tbl.Clear();
                if (SQL.PersonExists(SimilarPerson_tbl, true, U.Person_Table, U.PersonID_col, spouseName))
                {
                    return doNotProcessId;  //No person, possible spouse match. Do NOt Process
                }
                II.iSpouseId = 0;
                AddPersonToTable(II.iSpouseId, iPersonType + 1, spouseName, Sex.SpouseSex());
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private bool GayMarriage(PersonName personName,
                                 PersonName spouseName)
        {
            char personSex = SQL.GetFirstNameSex(personName.firstName);
            char spouseSex = SQL.GetFirstNameSex(spouseName.firstName);
            if (personSex == 'B' || personSex == ' ' || spouseSex == 'B' || spouseSex == ' ')
            {
                return false;
            }
            return (personSex == spouseSex);
        }
        //****************************************************************************************************************************
        private void AddPersonToTable(int personID,
                                      int iPersonType,
                                      PersonName personName,
                                      eSex Sex,
                                      bool gayMarriage=false)
        {
            if (personID == 0)
            {
                NewPersonWithRecordName(iPersonType, personName, Sex, gayMarriage);
            }
            else
            {
                AddExistingPersonToTable(personID, iPersonType, personName, Sex, gayMarriage);
            }
        }
        //****************************************************************************************************************************
        private void AddExistingPersonToTable(int iPersonID,
                                              int iPersonType,
                                              PersonName personName,
                                              eSex Sex,
                                              bool gayMarriage)
        {
            SQL.GetPerson(Person_tbl, iPersonID);
            if (Person_tbl.Rows.Count == 0)
            {
                NewPersonWithRecordName(iPersonType, personName, Sex, gayMarriage);
            }
            DataRow Person_row = GetLastRow(Person_tbl);
            string sMarriedName = II.sMarriedName;
            if (!gayMarriage && Person_row[U.Sex_col].ToChar() == 'F' && String.IsNullOrEmpty(sMarriedName))
            {
                sMarriedName = GetMarriedNameFromMarriageRecords(iPersonID);
            }
            Person_row[U.ImportPersonID_col] = iPersonType;
            if (Person_row[U.Notes_col].ToString().Length != 0)
                Person_row[U.Notes_col] += "\n";
            Person_row[U.Notes_col] += II.sNotes;
            if (Sex != eSex.eUnknown)
                Person_row[U.Sex_col] = GetSex(Sex);
            Person_row[U.FirstName_col] = SQL.MoveMergeStringToPerson(Person_row[U.FirstName_col].ToString(), personName.firstName);
            Person_row[U.MiddleName_col] = SQL.MoveMergeStringToPerson(Person_row[U.MiddleName_col].ToString(), personName.middleName);
            Person_row[U.LastName_col] = SQL.MoveMergeStringToPerson(Person_row[U.LastName_col].ToString(), personName.lastName);
            Person_row[U.Suffix_col] = SQL.MoveMergeStringToPerson(Person_row[U.Suffix_col].ToString(), personName.suffix);
            Person_row[U.Prefix_col] = SQL.MoveMergeStringToPerson(Person_row[U.Prefix_col].ToString(), personName.prefix);
            if (!gayMarriage && Sex == eSex.eFemale && sMarriedName.Length != 0)
            {
                string sPersonLastName = Person_row[U.LastName_col].ToString();
                Person_row[U.MarriedName_col] = sMarriedName;
                if (sMarriedName == sPersonLastName)
                {
                    string message = "Last Name Same as Married Name for " + Person_row[U.FirstName_col] + " " + sPersonLastName;
                    message += " Should the Lastname be Blank?";
                    if (MessageBox.Show(message, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Person_row[U.LastName_col] = "";
                    }
                }
            }
        }
        //****************************************************************************************************************************
        public DataRow GetLastRow(DataTable tbl)
        {
            int iRowIndex = tbl.Rows.Count - 1;
            return tbl.Rows[iRowIndex];
        }
        //****************************************************************************************************************************
        private string GetMarriedNameFromMarriageRecords(int personId)
        {
            DataTable marriage_tbl = new DataTable();
            SQL.GetAllSpouses(marriage_tbl, personId);
            if (marriage_tbl.Rows.Count == 0)
            {
                return "";
            }
            DataRow Marriage_row = marriage_tbl.Rows[0];
            int spouseId = 0;
            int spouse1 = Marriage_row[U.PersonID_col].ToInt();
            int spouse2 = Marriage_row[U.SpouseID_col].ToInt();
            if (personId == spouse1)
                spouseId = spouse2;
            else if (personId == spouse2)
                spouseId = spouse1;
            DataRow spouseRow = SQL.GetPerson(spouseId);
            if (spouseRow == null || spouseRow[U.Sex_col].ToChar() == 'F')
            {
                return "";
            }
            return spouseRow[U.LastName_col].ToString();
        }
        //****************************************************************************************************************************
        private void NewPersonWithRecordName(int iPersonType,
                                                  PersonName personName,
                                                  eSex Sex,
                                                  bool gayMarriage)
        {
            DataRow row = Person_tbl.NewRow();
            SQL.InitializePersonTable(row);
            row[U.PersonID_col] = 0;
            row[U.FirstName_col] = personName.firstName;
            row[U.MiddleName_col] = personName.middleName;
            row[U.Suffix_col] = personName.suffix;
            row[U.Prefix_col] = personName.prefix;
            if (!gayMarriage && Sex == eSex.eFemale)
            {
                row[U.MarriedName_col] = II.sMarriedName;
                if (personName.lastName == II.sMarriedName)
                {
                    row[U.LastName_col] = "";
                }
                else
                {
                    row[U.LastName_col] = personName.lastName;
                }
            }
            else
            {
                row[U.LastName_col] = personName.lastName;
            }
            row[U.KnownAs_col] = "";
            row[U.Source_col] = U.JamaicaVitalRecords;
            row[U.ImportPersonID_col] = iPersonType;
            row[U.Notes_col] = II.sNotes;
            row[U.Sex_col] = GetSex(Sex);
            Person_tbl.Rows.Add(row);
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
        protected bool ValidData(int iPersonID, int iSpouseID, string sPersonTitle, string spouseTitle)
        {
            if (iPersonID == doNotProcessId || iSpouseID == doNotProcessId)
            {
                return false;
            }
            if (iPersonID == U.Exception || iSpouseID == U.Exception)
            {
                return false;
            }
            if (DuplicateOfPersonID(sPersonTitle, spouseTitle, iSpouseID, iPersonID))
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        protected bool DuplicateOfPersonID(string sErrorID,
                                         string sPersonSpouse,
                                         int iParentID,
                                         int iPersonID)
        {
            if (iParentID != 0 && iParentID != doNotProcessId && iParentID == iPersonID)
            {
                MessageBox.Show(sPersonSpouse + "'s " + sErrorID + " cannot be the same name as the " + sPersonSpouse);
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected bool FatherMotherEmpty(PersonName fatherName, PersonName motherName)
        {
            if (fatherName.IsEmpty(getFromGrid) && motherName.IsEmpty(getFromGrid))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected bool GetMarriageRecord(int iPersonID, int iSpouseID)
        {
            if (iPersonID == 0 && iSpouseID == 0)
            {
                return true;
            }
            if (iPersonID == 0 && SpouseIsAlreadyMarried(iSpouseID))
            {
                return false;
            }
            if (iSpouseID == 0 && SpouseIsAlreadyMarried(iPersonID))
            {
                return false;
            }
            SQL.GetMarriageRecord(Marriage_tbl, iPersonID, iSpouseID);
            return true;
        }
        //****************************************************************************************************************************
        private bool SpouseIsAlreadyMarried(int spouseId)
        {
            if (getFromGrid)
            {
                return false;
            }
            DataTable spouseTable = new DataTable();
            SQL.GetAllSpouses(spouseTable, spouseId);
            return spouseTable.Rows.Count != 0;
        }
        //****************************************************************************************************************************
        protected bool UseSelectedFatherMother(DataRow Person_row,
                                               PersonName personName,
                                             int iFatherID,
                                             int iMotherID)
        {
            if (!UseSelectedPerson("Father", personName, Person_row[U.FatherID_col].ToInt(), iFatherID))
            {
                return false;
            }
            else if (iFatherID != 0)
            {
                Person_row[U.FatherID_col] = iFatherID;
            }
            if (!UseSelectedPerson("Mother", personName, Person_row[U.MotherID_col].ToInt(), iMotherID))
            {
                return false;
            }
            else if (iMotherID != 0)
            {
                Person_row[U.MotherID_col] = iMotherID;
            }
            return true;
        }
        //****************************************************************************************************************************
        protected bool UseSelectedPerson(string sNameString,
                                       PersonName personName,
                                       int iPersonID,
                                       int iSelectedID)
        {
            if (iPersonID != 0 && iSelectedID != 0 && iSelectedID != iPersonID)
            {
                if (!getFromGrid)
                {
                    return false;
                }
                if (!UseSelectedPerson(sNameString, personName, SQL.GetPersonName(iPersonID), SQL.GetPersonName(iSelectedID)))
                    return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        protected bool UseSelectedPerson(string sMessageName,
                                       PersonName personName,
                                       string sParentName,
                                       string sSelectedName)
        {
            string sName = SQL.BuildNameString(personName.firstName,
                                            personName.middleName,
                                            personName.lastName,
                                            personName.suffix,
                                            personName.prefix, "", "");
            string sMessage = sMessageName + " of " + sName + " is different than the one Just Selected\n\n" +
                              "Name in " + sMessageName + " Record:    " + sParentName + "\n" +
                              "Selected " + sMessageName + ":                  " + sSelectedName + "\n\n" +
                              "Use Selected " + sMessageName + " or cancel integration";
            switch (MessageBox.Show(sMessage, "", MessageBoxButtons.OKCancel))
            {
                case DialogResult.OK: return true;
                default: return false;
            }
        }
        //****************************************************************************************************************************
    }
}
