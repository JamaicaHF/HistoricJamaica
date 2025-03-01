using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    public class CImportGedcom:CImport
    {
        //****************************************************************************************************************************
        public CImportGedcom(CSql SQL, string sDataDirectory)
            : base(SQL, sDataDirectory)
        {
            if (SetupCopySQLDatabase())
                ImportFile();
        }
        //****************************************************************************************************************************
        protected virtual void ImportFile()
        {
            string sFilter = "GEDCOM Files (GED)|*.GED|GEDCOM Files(*.GED)|*.GED";
            if (!OpenInputFile(sFilter))
                return;
            string sInputRecord = null;
            bool bContinue = true;
            if (ProcessHeaderRecord(ref sInputRecord))
            {
                do
                {
                    if (sInputRecord[2] == '@')
                        bContinue = ProcessRecord(ref sInputRecord);
                    else
                    {
                        if (sInputRecord.Substring(2, 4) != "TRLR")
                            MessageBox.Show("Invalid message at level 0: " + sInputRecord);
                        sInputRecord = "";
                        bContinue = false;
                    }
                } while (bContinue && sInputRecord != null);
            }
            CloseInputFile();
            AddPersonsAndMariagesToDatabase();
            SQLCopy.CloseConnection();
        }
        //****************************************************************************************************************************
        private bool ProcessHeaderRecord(ref string sInputRecord)
        {
            string sHeaderRec = ReadRecord();
            if (sHeaderRec == null || sHeaderRec.Length < 6 || sHeaderRec[0] != '0' || sHeaderRec.Substring(2, 4) != "HEAD")
            {
                MessageBox.Show("Invalid header record for GED file");
                return false;
            }
            return NextInputRecord_0(ref sInputRecord);
        }
        //****************************************************************************************************************************
        private void ProcessFamily(ref string sInputRecord)
        {
            sInputRecord = ReadRecord();
            int iFatherID = 0;
            int iMotherID = 0;
            string sDataMarried =  U.Unknown;
            string sDivorced = "N";
            while (sInputRecord != null && sInputRecord[0] != '0')
            {
                string sRecordIdentifier = RecordIdentifier(sInputRecord.TrimString());
                if (sRecordIdentifier == "HUSB")
                {
                    iFatherID = GetID(sInputRecord);
                    sInputRecord = ReadRecord();
                }
                else if (sRecordIdentifier == "WIFE")
                {
                    iMotherID = GetID(sInputRecord);
                    sInputRecord = ReadRecord();
                }
                else if (sRecordIdentifier == "CHIL")
                {
                    SetChildren(sInputRecord,iFatherID, iMotherID);
                    sInputRecord = ReadRecord();
                }
                else if (sRecordIdentifier == "DIV")
                {
                    sDivorced = sInputRecord.Substring(5).TrimString();
                    sInputRecord = ReadRecord();
                }
                else if (sRecordIdentifier == "_FA1")
                {
                    sDivorced = "Y";
                    sInputRecord = ReadRecord();
                }
                else if (sRecordIdentifier == "MARR")
                    sInputRecord = GetDateMarried(ref sDataMarried, ref sDivorced);
                else
                {
                    if (sInputRecord.Length > 6)
                    {
                        string s = sInputRecord.Substring(7).TrimString();
                        if (s.ToLower() == "divorce")
                            sDivorced = "Y";
                    }
                    sInputRecord = ReadRecord();
                }
            }
            if (iFatherID != 0 && iMotherID != 0)
                SQLCopy.AddMarriageToDataTable(m_Marriage_tbl,iFatherID, iMotherID, sDataMarried, sDivorced);
        }
        //****************************************************************************************************************************
        private string GetDateMarried(ref string sDataMarried,
                                      ref string sDivorced)
        {
            string sInputRecord = ReadRecord();
            while (sInputRecord != null && sInputRecord[0] != '0' && sInputRecord[0] != '1')
            {
                string sRecordIdentifier = RecordIdentifier(sInputRecord.TrimString());
                if (sRecordIdentifier == "DATE")
                    sDataMarried = sInputRecord.Substring(6).TrimString();
                sInputRecord = ReadRecord();
            }
            return sInputRecord;
        }
        //****************************************************************************************************************************
        private string GetDivorced()
        {
            return "";
        }
        //****************************************************************************************************************************
        private void SetChildren(string sInputRecord,
                                 int    iFatherID, 
                                 int    iMotherID)
        {
            int iImportPersonID = GetID(sInputRecord);
            DataRow Person_row = m_Person_tbl.Rows.Find(iImportPersonID);
            if (Person_row != null)
            {
                Person_row[U.FatherID_col] = iFatherID;
                Person_row[U.MotherID_col] = iMotherID;
            }
        }
        //****************************************************************************************************************************
        private int GetID(string sInputRecord)
        {
            int iID = 0;
            try
            {
                int iStartIndex = sInputRecord.IndexOf('@') + 2;  // first for zero relative, second for the ID char
                int iEndIndex = sInputRecord.Substring(iStartIndex).IndexOf('@');
                string sID = sInputRecord.Substring(iStartIndex, iEndIndex);
                iID = sInputRecord.Substring(iStartIndex, iEndIndex).ToInt();
            }
            catch
            {
                MessageBox.Show("Invalid ID value: " + sInputRecord);
            }
            return iID;
        }
        private string RecordIdentifier(string sInputRecord)
        {
            int iLength = 4;
            if (sInputRecord.Length < (2 + 4))
                iLength = sInputRecord.Length - 2;
            return sInputRecord.Substring(2, iLength).TrimString();
        }
        //****************************************************************************************************************************
        private void ProcessIndividual(ref string sInputRecord)
        {
            CPersonRecord p = new CPersonRecord();
            p.iImportPersonID = GetID(sInputRecord);
            sInputRecord = ReadRecord();
            while (sInputRecord != null && sInputRecord[0] != '0')
            {
                string sRecordIdentifier = RecordIdentifier(sInputRecord.TrimString());
                if (sRecordIdentifier == "SEX")
                {
                    p.PersonSex = GetSex(sInputRecord.Substring(5, sInputRecord.Length - 5)).TrimString();
                    sInputRecord = ReadRecord();
                }
                else if (sRecordIdentifier == "BIRT")
                {
                    sInputRecord = GetBirthOrDeath(ref p.PersonBornDate, ref p.PersonBornPlace);
                    p.PersonBornSource = m_sFileName;
                }
                else if (sRecordIdentifier == "DEAT")
                {
                    sInputRecord = GetBirthOrDeath(ref p.PersonDiedDate, ref p.PersonDiedPlace);
                    p.PersonDiedSource = m_sFileName;
                }
                else if (sRecordIdentifier == "NAME")
                {
                    GetName(sInputRecord.Substring(6, sInputRecord.Length - 6).TrimString(),
                            ref p.PersonPrefix, ref p.PersonFirstName, ref p.PersonMiddleName, ref p.PersonLastName, ref p.PersonSuffix);
                    sInputRecord = ReadRecord();
                }
                else
                    sInputRecord = ReadRecord();
            }
            p.PersonSource = m_sFileName;
            SQLCopy.AddPersonToDataTable(m_Person_tbl, p);
        }
        private string GetBirthOrDeath(ref string PersonDate, 
                                       ref string PersonPlace)
        {
            string sInputRecord = ReadRecord();
            while (sInputRecord != null && sInputRecord[0] != '0' && sInputRecord[0] != '1')
            {
                string sRecordIdentifier = RecordIdentifier(sInputRecord.TrimString());
                if (sRecordIdentifier == "DATE")
                    PersonDate = sInputRecord.Substring(6).TrimString();
                else if (sRecordIdentifier == "PLAC")
                    PersonPlace = sInputRecord.Substring(6).TrimString();
                sInputRecord = ReadRecord();
            }
            return sInputRecord;
        }
        //****************************************************************************************************************************
        private string GetSex(string sInputRecord)
        {
            if (sInputRecord.Length == 0)
                return "";
            else
                return sInputRecord.Substring(0,1);
        }
        //****************************************************************************************************************************
        private void GetName(string sInputRecord,
                         ref string sPrefix,
                         ref string sFirstName,
                         ref string sMiddleName,
                         ref string sLastName,
                         ref string sSuffix)
        {
            if (sInputRecord.Length == 0)
                return;
            int iStartPositionSurname = sInputRecord.IndexOf('/'); 
            sLastName = sInputRecord.Substring(iStartPositionSurname + 1);
            int iEndPositionSurname = sLastName.IndexOf('/');
            if (sLastName.Length > iEndPositionSurname)
                sSuffix = ValidSuffix(sLastName.Substring(iEndPositionSurname + 1, sLastName.Length - (iEndPositionSurname + 1)).TrimString());
            sLastName = sLastName.Substring(0, iEndPositionSurname).TrimString();
            string sGivenName = sInputRecord.Substring(0, iStartPositionSurname).TrimString();
            int iEndOfFirstName = sGivenName.IndexOf(' ');
            if (iEndOfFirstName < 0)
            {
                sPrefix = ValidPrefix(sGivenName);
                if (sPrefix.Length == 0)
                    sFirstName = sGivenName;
            }
            else
            {
                sFirstName = sGivenName.Substring(0, iEndOfFirstName).TrimString();
                sPrefix = ValidPrefix(sFirstName);
                if (sPrefix.Length == 0)  // There was no valid prefix
                {
                    sMiddleName = sGivenName.Substring(iEndOfFirstName, sGivenName.Length - iEndOfFirstName).TrimString();
                }
                else
                {
                    sFirstName = sGivenName.Substring(iEndOfFirstName, sGivenName.Length - iEndOfFirstName).TrimString();
                    iEndOfFirstName = sFirstName.IndexOf(' ');
                    if (iEndOfFirstName > 0)
                    {
                        sMiddleName = sFirstName.Substring(iEndOfFirstName, sFirstName.Length - iEndOfFirstName).TrimString();
                        sFirstName = sFirstName.Substring(0, iEndOfFirstName).TrimString();
                    }
                }
            }
        }
        //****************************************************************************************************************************
        protected bool NextInputRecord_0(ref string sInputRecord)
        {
            do
            {
                sInputRecord = ReadRecord();
            } while (sInputRecord != null && sInputRecord[0] != '0');
            return sInputRecord != null;
        }
        //****************************************************************************************************************************
        private bool ProcessRecord(ref string sInputRecord)
        {
            switch (sInputRecord[3])
            {
                case 'S': NextInputRecord_0(ref sInputRecord); break;
                case 'I': ProcessIndividual(ref sInputRecord); break;
                case 'F': ProcessFamily(ref sInputRecord); break;
                default: NextInputRecord_0(ref sInputRecord); break;
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
