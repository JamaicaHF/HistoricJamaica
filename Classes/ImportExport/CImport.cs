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
using Utilities;

namespace HistoricJamaica
{
    public abstract class CImport
    {
        protected const int carriageReturn = 13;
        protected const int lineFeed = 10;
        private const string databasename = "DataBase=HistoricJamaicaCopy;";
        private string m_sDataDirectory = "";
        private byte[] bytes;
        private int bytesRead;
        private TextReader m_tr;
        protected CSql SQLMain = null;
        protected DataTable m_Person_tbl;
        protected DataTable m_Marriage_tbl;
        protected DataTable m_Cemetery_tbl;
        protected string m_sFileName;
        protected CSql SQLCopy = null;
        private bool m_streamReader = false;
        //****************************************************************************************************************************
        public CImport(CSql SQL,
                       string sDataDirectory)
        {
            SQLMain = SQL;
            m_sDataDirectory = sDataDirectory;
        }
        //****************************************************************************************************************************
        protected bool SetupCopySQLDatabase()
        {
            string sDataDirectory = "";
            string sServer = UU.GetServerFromIniFile(".\\HistoricJamaica.ini", ref sDataDirectory);
            SQLCopy = new CSql(databasename, sServer, sDataDirectory, false);
            string SQLError = SQLCopy.GetSQLErrorMessage();
            if (SQLError != U.NoSQLError)
            {
                MessageBox.Show(SQLError);
                return false;
            }
            SQLCopy.DeleteFromTable(U.Marriage_Table);
            SQLCopy.DeleteFromTable(U.Person_Table);
            SQLCopy.DeleteFromTable(U.CemeteryRecord_Table);
            m_Person_tbl = SQL.DefinePersonTable();
//            m_Person_tbl = SQLCopy.DefinePersonTable();
            m_Person_tbl.PrimaryKey = new DataColumn[] { m_Person_tbl.Columns[U.ImportPersonID_col] };
             m_Marriage_tbl = SQL.DefineMarriageTable();
            //            m_Marriage_tbl = SQLCopy.DefineMarriageTable();
            //            m_Marriage_tbl = SQLCopy.DefineMarriageTable();
            //m_Cemetery_tbl = SQLCopy.DefineCemeteryValueTable();
            return true;
        }
        //****************************************************************************************************************************
        protected void GetExcelInputFile(string folder, string title, out string filename)
        {
            string sFilter = "Excel Files (xls)|*.xlsx";
            filename = UU.SelectFile(sFilter, folder, title);
            if (String.IsNullOrEmpty(filename))
            {
                return;
            }
        }
        //****************************************************************************************************************************
        protected void OpenInputFile(string sFilter, out string filename, bool streamReader=false)
        {
            m_streamReader = streamReader;
            filename = UU.SelectFile(sFilter, m_sDataDirectory);
            if (String.IsNullOrEmpty(filename))
            {
                return;
            }
            if (!OpenFileForInput(filename))
            {
                filename = "";
            }
        }
        //****************************************************************************************************************************
        protected bool OpenInputFile(string sFilter)
        {
            return OpenFileForInput(UU.SelectFile(sFilter, m_sDataDirectory));
        }
        //****************************************************************************************************************************
        protected bool OpenFileForInput(string sFileNameWithPath)
        {
            if (sFileNameWithPath.Length == 0)
            {
                return false;
            }
            m_sFileName = GetFileNameFromPath(sFileNameWithPath);
            try
            {
                if (m_streamReader)
                {
                    ReadFile(sFileNameWithPath);
                }
                else
                {
                    m_tr = new StreamReader(sFileNameWithPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private void ReadFile(string pathSource)
        {
            using (FileStream m_sr = new FileStream(pathSource,
                        FileMode.Open, FileAccess.Read))
            {
                int numBytesToRead = (int)m_sr.Length;
                if (numBytesToRead > 131072)
                {
                    throw new Exception("Input Buffer greater than 128 meg");
                }
                bytes = new byte[numBytesToRead];
                int numBytesRead = 0;
                int n = m_sr.Read(bytes, numBytesRead, numBytesToRead);
                if (n != numBytesToRead)
                {
                    throw new Exception("Input Size Mismatch");
                }
            }
            int count = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                char ch = Convert.ToChar(bytes[i]);
                count++;
                if (count > 50)
                {
                    count = 0;
                }
            }
        }
        //****************************************************************************************************************************
        private void ReadBuffer(string pathSource)
        {
            using (FileStream fsSource = new FileStream(pathSource,
                        FileMode.Open, FileAccess.Read))
            {

                // Read the source file into a byte array.
                byte[] bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                int bufferSize = 30000;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    if (numBytesToRead < bufferSize)
                    {
                        bufferSize = numBytesToRead;
                    }
                    int n = fsSource.Read(bytes, numBytesRead, bufferSize);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                numBytesToRead = bytes.Length;
                string pathNew = @"c:\JHF\TestFile.csv";
                // Write the byte array to the other FileStream.
                using (FileStream fsNew = new FileStream(pathNew,
                    FileMode.Create, FileAccess.Write))
                {
                    fsNew.Write(bytes, 0, numBytesToRead);
                }
            }
        }
        //****************************************************************************************************************************
        protected void CloseInputFile()
        {
            if (!m_streamReader)
            {
                m_tr.Close();
            }
        }
        //****************************************************************************************************************************
        protected void AddPersonsAndMariagesToDatabase()
        {
            if (SQL.InsertPersonTable(m_Person_tbl, m_Person_tbl, m_Marriage_tbl, m_Cemetery_tbl))
          //if (SQLCopy.InsertPersonTable(m_Person_tbl, m_Person_tbl, m_Marriage_tbl, m_Cemetery_tbl))
            {
                DataTable tbl = new DataTable(U.Person_Table);
                SQLCopy.selectall(tbl, U.Person_Table, U.NoOrderBy);
                int iFirstPersonID = 0;
                if (tbl.Rows.Count != 0)
                    iFirstPersonID = tbl.Rows[0][U.PersonID_col].ToInt();
                CFamilyMoveAllToDatabase FamilyMoveAllToDatabase = new CFamilyMoveAllToDatabase(SQLCopy, SQLMain, iFirstPersonID);
                FamilyMoveAllToDatabase.ShowDialog();
            }
            SQLCopy.CloseConnection();
        }
        //****************************************************************************************************************************
        protected string GetFileNameFromPath(string sFileNameWithPath)
        {
            char[] c = new char[1];
            c[0] = '\\';
            int iIndexOfLastBackslash = sFileNameWithPath.LastIndexOfAny(c);
            return sFileNameWithPath.Substring(iIndexOfLastBackslash+1);
        }
        //****************************************************************************************************************************
        protected string ReadExcelRecord(int rowIndex)
        {
            if (m_streamReader)
            {
                return ReadRecordFromStream();
            }
            else
            {
                return m_tr.ReadLine();
            }
        }
        //****************************************************************************************************************************
        protected string ReadRecord()
        {
            if (m_streamReader)
            {
                return ReadRecordFromStream();
            }
            else
            {
                return m_tr.ReadLine();
            }
        }
        //****************************************************************************************************************************
        private string ReadRecordFromStream()
        {
            string str = "";
            while (bytesRead < bytes.Length)
            {
                byte ch = bytes[bytesRead];
                bytesRead++;
                if (ch == lineFeed)
                {
                }
                else if (ch == carriageReturn)
                {
                    ch = bytes[bytesRead];
                    if (ch == lineFeed)
                    {
                        bytesRead++;
                    }
                    return str;
                }
                else if (ch < 32 || ch > 247)
                {
                    MessageBox.Show("Unprintable Character Found");
                }
                else
                {
                    char newChar = Convert.ToChar(ch);
                    str = str.Insert(str.Length, newChar.ToString());
                }
            }
            return null;
        }
        //****************************************************************************************************************************
        protected string RemoveChar(string sString,
                                    char cCharToRemove)
        {
            bool dDone = false;
            do
            {
                int ifoundChar = sString.IndexOf(cCharToRemove);
                if (ifoundChar < 0)
                    dDone = true;
                else
                {
                    sString = sString.Remove(ifoundChar, 1);
                }
            } while (!dDone);
            return sString;
        }
        //****************************************************************************************************************************
        private string RemoveAllCharacterAndReturnLowerCase(string sString,
                                                            char cCharToRemove)
        {
            sString = RemoveChar(sString, cCharToRemove);
            return sString.ToLower().TrimString();
        }
        //****************************************************************************************************************************
        protected string ValidPrefix(string sName)
        {
            string sNameWithoutDots = RemoveAllCharacterAndReturnLowerCase(sName, '.');
            string sNameWithoutCommasDots = RemoveAllCharacterAndReturnLowerCase(sNameWithoutDots, ',');
            if (sNameWithoutCommasDots == "dr")
                return "Dr";
            else if (sNameWithoutCommasDots == "rev")
                return "Rev";
            else if (sNameWithoutCommasDots == "col" || sNameWithoutCommasDots == "colonel")
                return "Col";
            else if (sNameWithoutCommasDots == "Sgt")
                return "Sgt";
            else if (sNameWithoutCommasDots == "gen")
                return "Gen";
            else if (sNameWithoutCommasDots == "cpt" || sNameWithoutCommasDots == "capt")
                return "Cpt";
            else if (sNameWithoutCommasDots == "ltg")
                return "Ltg";
            else
                return "";
        }
        //****************************************************************************************************************************
        protected string ValidSuffix(string sName)
        {
            string sNameWithoutDots = RemoveAllCharacterAndReturnLowerCase(sName, '.');
            string sNameWithoutCommasDots = RemoveAllCharacterAndReturnLowerCase(sNameWithoutDots, ',');
            if (sNameWithoutCommasDots == "sr")
                return "Sr";
            else if (sNameWithoutCommasDots == "jr")
                return "Jr";
            else if (sNameWithoutCommasDots == "ii")
                return "II";
            else if (sNameWithoutCommasDots == "2nd")
                return "II";
            else if (sNameWithoutCommasDots == "iii")
                return "III";
            else if (sNameWithoutCommasDots == "iv")
                return "IV";
            else if (sNameWithoutCommasDots == "esq")
                return "Esq";
            else if (sNameWithoutCommasDots == "phd")
                return "PhD";
            else if (sNameWithoutCommasDots == "md")
                return "MD";
            else if (sNameWithoutCommasDots == "cpa")
                return "CPA";
            else if (sNameWithoutCommasDots == "usn")
                return "USN";
            else if (sNameWithoutCommasDots == "usmc")
                return "USMC";
            else if (sNameWithoutCommasDots == "pe")
                return "PE";
            else if (sNameWithoutCommasDots == "ra")
                return "RA";
            else
                return "";
        }
        //****************************************************************************************************************************
        protected ArrayList ReadCommaDelimitedRow(int numberOfExpectedFields, string inputRecord)
        {
            ParseInputRecord ParseInputRecord = new ParseInputRecord(inputRecord);
            ArrayList sInputFields = ParseInputRecord.GetInputFields();
            if (sInputFields.Count != numberOfExpectedFields)
            {
                string message = "Number of fields (" + sInputFields.Count + ") does not equal number expected (" + numberOfExpectedFields + ")";
                throw new Exception(message);
            }
            return sInputFields;
        }
        private const string sQuote = @"""";
        private const char cQuote = '"';
        private const char cComma = ',';
        private class ParseInputRecord
        {
            private ArrayList sInputFields = new ArrayList();
            private bool quoteFound = false;
            private string inputString = "";
            private char nextChar;
            int charIndex = 0;
            //****************************************************************************************************************************
            public ParseInputRecord(string inputRecord)
            {
                while (charIndex < inputRecord.Length)
                {
                    char newChar = inputRecord[charIndex];
                    charIndex++;
                    nextChar = (charIndex < inputRecord.Length) ? inputRecord[charIndex] : '|';
                    if (newChar == cQuote)
                    {
                        HandleQuote();
                    }
                    else if (newChar == cComma)
                    {
                        HandleComma(sInputFields, ref quoteFound);
                    }
                    else
                    {
                        inputString = inputString.Insert(inputString.Length, newChar.ToString());
                    }
                }
                if (quoteFound)
                {
                    throw new Exception("Quoted Field Found without closing quote");
                }
                sInputFields.Add(inputString);
            }
            //****************************************************************************************************************************
            public ArrayList GetInputFields()
            {
                return sInputFields;
            }
            //****************************************************************************************************************************
            private void HandleQuote()
            {
                if (!quoteFound)
                {
                    quoteFound = true;
                    return;
                }
                if (nextChar == cQuote)
                {
                    charIndex++;
                    inputString = inputString.Insert(inputString.Length, sQuote);
                }
                else
                {
                    quoteFound = false;
                }
            }
            //****************************************************************************************************************************
            private void HandleComma(ArrayList sInputFields, ref bool quoteFound)
            {
                if (quoteFound)
                {
                    inputString = inputString.Insert(inputString.Length, cComma.ToString());
                }
                else
                {
                    sInputFields.Add(inputString);
                    inputString = "";
                }
            }
        }
        //****************************************************************************************************************************
        private string Field(string[] sInputFields,
                             ref int iFieldNum)
        {
            if (iFieldNum >= sInputFields.Length)
            {
                return "";
            }
            string sInputField = sInputFields[iFieldNum];
            if (String.IsNullOrEmpty(sInputField))
            {
                return "";
            }
            if (sInputField[0] != '"')
            {
                return sInputField;
            }
            string partialString = sInputField.Remove(0, 1);
            if (partialString[partialString.Length - 1] == '"')
            {
                return partialString.Remove(partialString.Length - 1);
            }
            iFieldNum++;
            sInputField = "";
            if (iFieldNum < sInputFields.Length)
            {
                sInputField = sInputFields[iFieldNum];
                if (sInputField[sInputField.Length - 1] == '"')
                {
                    return partialString + ", " + sInputField.Remove(sInputField.Length - 1).Trim();
                }
            }
            throw new Exception("Unable to Read Complete Quoted Field " + sQuote + partialString + "," + sInputField + sQuote);
        }
    }
}
