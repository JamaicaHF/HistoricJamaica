using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    public class CImportCemetery : CImport
    {
        private string m_sInputRecord;
        private string m_sCemeteryName = "";
        private int m_iCemeteryID = 0;
        private CPersonRecord p = new CPersonRecord();
        private int m_iPersonImportPersonID = 0;
        private string PersonFather_col = "PersonFather";
        private string PersonMother_col = "PersonMother";
        private string PersonSpouse_col = "PersonSpouse";
        private bool m_bGettingOther = false;
        //****************************************************************************************************************************
        public CImportCemetery(CSql SQL, string sDataDirectory)
            : base(SQL, sDataDirectory)
        {
            if (SetupCopySQLDatabase())
            {
                m_Person_tbl.Columns.Add(PersonFather_col, typeof(string));
                m_Person_tbl.Columns.Add(PersonMother_col, typeof(string));
                m_Person_tbl.Columns.Add(PersonSpouse_col, typeof(string));
                ImportFile();
            }
        }
        //****************************************************************************************************************************
        private bool ProcessHeaderRecord()
        {
            m_sCemeteryName = ReadRecord();
            int iIndex = m_sCemeteryName.IndexOf(U.Tab);
            m_sCemeteryName = m_sCemeteryName.Remove(iIndex);
            m_sCemeteryName = m_sCemeteryName.Replace("Cemetery", "");
            m_iCemeteryID = SQLCopy.GetCemeteryIDFromName(m_sCemeteryName);
            if (m_iCemeteryID == 0)
            {
                MessageBox.Show("Unable to find Record for " + m_sCemeteryName);
                return false;
            }
            else
            {
                return true;
            }
        }
        //****************************************************************************************************************************
        protected void ImportFile()
        {
            string sFilter = "Tab Delimited Files (txt)|*.txt";
            if (!OpenInputFile(sFilter))
                return;
            if (!ProcessHeaderRecord())
                return;
            m_sInputRecord = ReadRecord();
            while (m_sInputRecord != null)
            {
                ProcessRecord();
                m_sInputRecord = ReadRecord();
            }
            CloseInputFile();
            AssignFatherMotherSpouse();
            AddPersonsAndMariagesToDatabase();
        }
        //****************************************************************************************************************************
        private int GetImportPersonID(string sPerson)
        {
            string sFirstName = "";
            string sMiddleName = "";
            string sLastName = "";
            string sPrefix = "";
            string sSuffix = "";
            string sMarriedName = "";
            GetName(sPerson, "M", ref sPrefix, ref sSuffix, ref sFirstName, ref sMiddleName, ref sLastName, ref sMarriedName);

            int iImportPersonID = 0;
            foreach (DataRow row in m_Person_tbl.Rows)
            {
                if (row[U.FirstName_col].ToString() == sFirstName &&
                    row[U.MiddleName_col].ToString() == sMiddleName &&
                    row[U.LastName_col].ToString() == sLastName &&
                    row[U.Prefix_col].ToString() == sPrefix &&
                    row[U.Suffix_col].ToString() == sSuffix)
                {
                    return row[U.ImportPersonID_col].ToInt();
                }
                 // no exact, check for first and last name
                if (row[U.FirstName_col].ToString() == sFirstName &&
                    row[U.LastName_col].ToString() == sLastName)
                {
                    iImportPersonID = row[U.ImportPersonID_col].ToInt();
                }
                // If First and Last Name not found, look for first initial
                if (iImportPersonID == 0 && row[U.LastName_col].ToString() == sLastName)
                {
                    if (sFirstName.Length == 1 && row[U.FirstName_col].ToString().Length != 0 &&
                        row[U.FirstName_col].ToString()[0] == sFirstName[0])
                    {
                        iImportPersonID = row[U.ImportPersonID_col].ToInt();
                    }
                    if (sFirstName.Length != 0 && row[U.FirstName_col].ToString().Length == 1 &&
                        row[U.FirstName_col].ToString()[0] == sFirstName[0])
                    {
                        iImportPersonID = row[U.ImportPersonID_col].ToInt();
                    }
                }
            }
            return iImportPersonID;
        }
        //****************************************************************************************************************************
        private bool MarriageDoesNotYetExist(int iPersonID,
                                             int iSpouseID)
        {
            foreach (DataRow row in m_Marriage_tbl.Rows)
            {
                if (row[U.PersonID_col].ToInt() == iPersonID && row[U.SpouseID_col].ToInt() == iSpouseID)
                    return false;
                if (row[U.PersonID_col].ToInt() == iSpouseID && row[U.SpouseID_col].ToInt() == iPersonID)
                    return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private void AssignFatherMotherSpouse()
        {
            foreach (DataRow row in m_Person_tbl.Rows)
            {
                string sFather = row[PersonFather_col].ToString();
                string sMother = row[PersonMother_col].ToString();
                string sSpouse = row[PersonSpouse_col].ToString();
                int iFatherID = 0;
                int iMotherID = 0;
                if (sFather.Length != 0)
                {
                    iFatherID = GetImportPersonID(sFather);
                    row[U.FatherID_col] = iFatherID;
                }
                if (sMother.Length != 0)
                {
                    iMotherID = GetImportPersonID(sMother);
                    row[U.MotherID_col] = iMotherID;
                }
                if (iFatherID > 0 && iMotherID > 0 && MarriageDoesNotYetExist(iFatherID, iMotherID))
                {
                    SQLCopy.AddMarriageToDataTable(m_Marriage_tbl, iFatherID, iMotherID, "", "N");
                }
                if (sSpouse.Length != 0)
                {
                    int iSpouseID = GetImportPersonID(sSpouse);
                    int iPersonID = row[U.ImportPersonID_col].ToInt();
                    if (iSpouseID != 0 && MarriageDoesNotYetExist(iPersonID, iSpouseID))
                    {
                        SQLCopy.AddMarriageToDataTable(m_Marriage_tbl, iPersonID, iSpouseID, "", "N");
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private string GetOtherLastName(string sOtherName)
        {
            if (m_bGettingOther) // be sure recursive call does not go into an infinate loop
            {
                m_bGettingOther = false;
                return "";
            }
            m_bGettingOther = true;
            string sPrefix = "";
            string sSuffix = "";
            string sFirstName = "";
            string sMiddleName = "";
            string sLastName = "";
            string sMarriedName = "";
            GetName(sOtherName, "M", ref sPrefix, ref sSuffix, ref sFirstName, ref sMiddleName, ref sLastName, ref sMarriedName);
            return sLastName;
        }
        //****************************************************************************************************************************
        private void GetName(string sInputField,
                             string sPersonSex,
                         ref string sPrefix,
                         ref string sSuffix,
                         ref string sFirstName,
                         ref string sMiddleName,
                         ref string sLastName,
                         ref string sMarriedName)
        {
            sInputField = sInputField.Replace(@"""", "");
            sInputField = sInputField.Replace("?", "");
            sInputField = sInputField.Replace("Father", "");
            sInputField = sInputField.Replace("Mother", "");
            sInputField = sInputField.Replace("front", "");
            sInputField = sInputField.Replace("back", "");
            sInputField = sInputField.Replace("right", "");
            sInputField = sInputField.Replace("left", "");
            sInputField = sInputField.Replace("(", "");
            sInputField = sInputField.Replace(")", "");
            string[] sNames = sInputField.Split(' ');
            int iFirstFieldNum = 0;
            int iFinalFieldNum = sNames.Length - 1;
            while (iFirstFieldNum < iFinalFieldNum && sNames[iFirstFieldNum].Length == 0)
            {
                iFirstFieldNum++;
            }
            while (iFirstFieldNum < iFinalFieldNum && sNames[iFinalFieldNum].Length == 0)
            {
                iFinalFieldNum--;
            }
            sPrefix = ValidPrefix(sNames[iFirstFieldNum]);
            if (sPrefix.Length != 0)
                iFirstFieldNum++;
            sSuffix = ValidSuffix(sNames[iFinalFieldNum]);
            if (sSuffix.Length != 0)
                iFinalFieldNum--;

            if (iFirstFieldNum <= iFinalFieldNum)
            {
                sLastName = RemoveChar(sNames[iFinalFieldNum], '.');
                if (sLastName.Length == 1)
                    sLastName = GetOtherLastName(p.PersonFather);
                else
                {
                    if (LastNameHasGenericStrings(sLastName))
                    {
                        sLastName = GetOtherLastName(p.PersonFather);
                        while (iFirstFieldNum <= iFinalFieldNum)
                        {
                            sFirstName += sNames[iFirstFieldNum].ToString() + " ";
                            iFirstFieldNum++;
                        }
                        sFirstName.TrimString();
                    }
                    iFinalFieldNum--;
                }
                if (sPersonSex.Length > 0 && sPersonSex[0] == 'F')
                {
                    string sHusbandLastName = GetOtherLastName(p.PersonSpouse);
                    if (sHusbandLastName.Length > 0 && sLastName != sHusbandLastName)
                        sMarriedName = sHusbandLastName;
                }
            }
            if (iFirstFieldNum <= iFinalFieldNum)
            {
                sFirstName = RemoveChar(sNames[iFirstFieldNum], '.');
                iFirstFieldNum++;
            }
            while (iFirstFieldNum <= iFinalFieldNum)
            {
                sMiddleName += RemoveChar(sNames[iFirstFieldNum], '.');
                iFirstFieldNum++;
            }
            sFirstName = sFirstName.Replace(@"Nath'l", "Nathaniel");
        }
        //****************************************************************************************************************************
        private void SetPersonName(string sInputField,
                                   string sPersonSex)
        {
            GetName(sInputField, sPersonSex, ref p.PersonPrefix, ref p.PersonSuffix, ref p.PersonFirstName,
                                 ref p.PersonMiddleName, ref p.PersonLastName, ref p.PersonMarriedName);
        }
        //****************************************************************************************************************************
        private string SetPersonSex(string sInputField)
        {
            if (sInputField.Length == 0)
                return "";
            else
            {
                if (sInputField.Length > 1)
                    sInputField = sInputField.Remove(1);
                sInputField.ToUpper();
                if (sInputField[0] == 'M' || sInputField[0] == 'F')
                {
                    p.PersonSex = sInputField; 
                    return sInputField;
                }
                else
                {
                    return "";
                }
            }
        }
        //****************************************************************************************************************************
        private string ParseDate(string sDate)
        {
            string sOriginalDate = sDate;
            sOriginalDate = sOriginalDate.Replace(@"""", "");
            sDate = sDate.ToLower().TrimString();
            sDate = sDate.Replace("not as of", "");
            sDate = sDate.Replace(" ", ""); //  must be after "not as of"
            sDate = sDate.Replace("may", "5");
            sDate = sDate.Replace("stillliving", "");
            sDate = sDate.Replace("blank", "00");
            sDate = sDate.Replace("?", "");
            sDate = sDate.Replace("*", "");
            sDate = sDate.Replace(")", "");
            sDate = sDate.Replace("(", "");
            sDate = sDate.Replace(".", "");
            sDate = sDate.Replace("-", "/");
            sDate = sDate.Replace(@"\", "");
            sDate = sDate.Replace(@"""", "");
            string[] sInputFields = sDate.Split(U.slash);
            if (sInputFields[0].Length >= 3 && sInputFields[0].Substring(0,3) == "feb")
                return sOriginalDate;
            int iYear = Field(sInputFields, 0).ToInt();
            int iMonth = Field(sInputFields, 1).ToInt();
            int iDay = Field(sInputFields, 2).ToInt();
            if (iYear < 1700 && iDay > 1699) // mm/dd/yyyy date, not a yyyy/mm/dd
            {
                int iTemp = iDay;
                iDay = iMonth;
                iMonth = iYear;
                iYear = iTemp;
            }
            return U.BuildDate(iYear, iMonth, iDay);
        }
        //****************************************************************************************************************************
        private string SetBornDate(string sInputField)
        {
            if (sInputField.Length > 0)
            {

                p.PersonBornDate = ParseDate(sInputField);
                p.PersonBornSource = m_sFileName;
            }
            return p.PersonBornDate;
        }
        //****************************************************************************************************************************
        private string SetDiedDate(string sInputField)
        {
            if (sInputField.Length > 0)
            {
                p.PersonDiedDate = ParseDate(sInputField);
                p.PersonDiedSource = m_sFileName;
            }
            return p.PersonDiedDate;
        }
        //****************************************************************************************************************************
        private int GetDateElement(ref string iDateElement,
                                      char       cElement)
        {
            int iLocation = iDateElement.IndexOf(cElement);
            if (iLocation > 0)
            {
                int iElement = iDateElement.Substring(0, iLocation).ToIntNoError();
                iDateElement = iDateElement.Remove(0, iLocation + 1);
                return iElement;
            }
            return 0;
        }
        //****************************************************************************************************************************
        private string SetAge(string sInputField)
        {
            string sOrig = "|" + sInputField;
            if (p.PersonBornDate.Length != 0)
                return sInputField;
            if (sInputField.Length > 0)
            {
                sInputField = sInputField.Replace(",", "");
                sInputField = sInputField.Replace(".", "");
                sInputField = sInputField.Replace("-", "");
                sInputField = sInputField.ToLower().TrimString();
                int iYear = GetDateElement(ref sInputField,'y');
                int iMonth = GetDateElement(ref sInputField, 'm');
                int iDay = GetDateElement(ref sInputField, 'd');
                if (sInputField.Length != 0 && iDay == 0)
                    iDay = sInputField.ToIntNoError();
//                p.PersonBornDate = iYear.ToString() + '|' + iMonth + '|' + iDay + "^" + sOrig;
                string[] sDiedDate = p.PersonDiedDate.Split('/');
                p.PersonBornDate = U.BornDateFromDiedDateMinusAge(Field(sDiedDate, 0).ToInt(), // Died Year
                                             Field(sDiedDate, 1).ToInt(), // Died Month
                                             Field(sDiedDate, 2).ToInt(), // Died Day
                                             iYear,iMonth,iDay, "");
                return p.PersonBornDate;
            }
            return sInputField;
        }
        //****************************************************************************************************************************
        private string Field(string[] sInputFields,
                             int      iFieldNum)
        {
            if (sInputFields.Length > iFieldNum)
                return sInputFields[iFieldNum];
            else
                return "";
        }
        //****************************************************************************************************************************
        private bool GenericStringInName(string sName,
                                         string sSearchString)
        {
            int ii = sName.IndexOf(sSearchString);
            bool bb = (sName.IndexOf(sSearchString) >= 0);
            return (sName.IndexOf(sSearchString) >= 0);
        }
        //****************************************************************************************************************************
        private bool NameHasGenericStrings(string sName)
        {
            string sLowerCaseString = sName.ToLower();
            return (GenericStringInName(sLowerCaseString, "footstone") ||
                    GenericStringInName(sLowerCaseString, "unmarked") ||
                    GenericStringInName(sLowerCaseString, "fieldstone") ||
                    GenericStringInName(sLowerCaseString, "marker"));
        }
        //****************************************************************************************************************************
        private bool LastNameHasGenericStrings(string sName)
        {
            sName = sName.ToLower();
            if (sName == "son")
                return true;
            return (GenericStringInName(sName, " son") ||
                    GenericStringInName(sName, "son of") ||
                    GenericStringInName(sName, "dau") ||
                    GenericStringInName(sName, "daug") ||
                    GenericStringInName(sName, "daughter"));
        }
        //****************************************************************************************************************************
        private bool ThisStoneIsForAPerson()
        {
            if (NameHasGenericStrings(p.PersonFirstName) || NameHasGenericStrings(p.PersonLastName))
                return false;
            if (p.PersonLastName.Length == 0)
                return false;
            if (p.PersonBornDate.Length == 0 && p.PersonDiedDate.Length == 0)
                return false;
            return true;
        }
        //****************************************************************************************************************************
        private void ProcessRecord()
        {
            p.InitializePerson();
            string[] sInputFields = m_sInputRecord.Split(U.Tab);
            if (Field(sInputFields, 0) == "")
                return;
            string sStone = Field(sInputFields, 0);
            string sPersonName = Field(sInputFields, 1);
            string sPersonSex  = Field(sInputFields, 2);
            string sFatherName = Field(sInputFields, 3);
            string sMotherName = Field(sInputFields, 4);
            string sSpouseName = Field(sInputFields, 5);
            string sBornDate   = Field(sInputFields, 6);
            string sDiedDate   = Field(sInputFields, 7);
            string sPersonAge  = Field(sInputFields, 8);
            sPersonAge = sPersonAge.Replace(@"""", "");
            string sEpitaph = Field(sInputFields, 9);
            string sNote1 = Field(sInputFields, 10);
            string sNote2 = Field(sInputFields, 11);
            string sNote3 = Field(sInputFields, 12);
            p.PersonBuriedStone = sStone;
            p.PersonFather = sFatherName; // Add Father before parsing name to have father last name available for children and spouse name
            p.PersonMother = sMotherName;
            p.PersonSpouse = sSpouseName;
            sPersonSex = SetPersonSex(sPersonSex);
            SetPersonName(sPersonName, sPersonSex);
            sBornDate = SetBornDate(sBornDate);
            sDiedDate = SetDiedDate(sDiedDate);
            sPersonAge = SetAge(sPersonAge);
            p.PersonDescription = sEpitaph + sNote1 + sNote2 + sNote3;
            p.PersonSource = m_sFileName;
            p.PersonBuriedSource = m_sFileName;
            p.PersonBuriedPlace = m_sCemeteryName;
            m_iPersonImportPersonID++;
            p.iImportPersonID = m_iPersonImportPersonID;

            SQLCopy.AddCemeteryToDataTable(m_Cemetery_tbl, m_iCemeteryID, m_iPersonImportPersonID, sStone, sPersonName, sFatherName, sMotherName, sSpouseName,
                                   sPersonSex, sBornDate, sDiedDate, sPersonAge, sEpitaph, sNote1, sNote2, sNote3);
            if (ThisStoneIsForAPerson())
            {
                SQLCopy.AddPersonToDataTable(m_Person_tbl, p);
                int iLastRow = m_Person_tbl.Rows.Count - 1;
                DataRow row = m_Person_tbl.Rows[iLastRow];
                row[PersonFather_col] = p.PersonFather;
                row[PersonMother_col] = p.PersonMother;
                row[PersonSpouse_col] = p.PersonSpouse;
            }
        }
        //****************************************************************************************************************************
    }
}
