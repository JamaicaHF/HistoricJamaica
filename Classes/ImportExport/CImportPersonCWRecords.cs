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
    public class CImportPersonCWRecords : CImport
    {
        private string m_sInputRecord;
        private ArrayList HeaderRow;
        private ArrayList columnsFound = new ArrayList();
        private int numberOfExpectedFields;
        private ArrayList DateCheck = new ArrayList();
        private int rowIndex = 1;
        private ArrayList row;
        private DataTable PersonCWTbl;
        //****************************************************************************************************************************
        public CImportPersonCWRecords(CSql Sql, string sDataDirectory)
            : base(Sql, sDataDirectory)
        {
            try
            {
                PersonCWTbl = SQL.DefinePersonCWTable();
                if (!OpenFile())
                {
                    return;
                }
                m_sInputRecord = ReadRecord();
                while (m_sInputRecord != null)
                {
                    try
                    {
                        rowIndex++;
                        row = ReadCommaDelimitedRow(numberOfExpectedFields, m_sInputRecord);
                        AddPersonToDatabase();
                    }
                    catch (Exception ex)
                    {
                        string message = "Row: " + rowIndex + " - " + ex.Message;
                        MessageBox.Show(message);
                        throw new Exception("Civil War Records Aborted");
                    }
                }
                AddUpdateDatabase();
                MessageBox.Show("Cival War Records Complete");
                CloseInputFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void AddUpdateDatabase()
        {
            string pathNew = @"c:\JHF\TestFile.csv";
            string crlf = "";
            crlf += Convert.ToChar(carriageReturn);
            crlf += Convert.ToChar(lineFeed);
            using (StreamWriter srNew = new StreamWriter(pathNew))
            {
                int count = 0;
                foreach (DataRow PersonCWRow in PersonCWTbl.Rows)
                {
                    string outpurStr = "";
                    outpurStr += CreateString(PersonCWRow[U.LastName_col]);
                    string firstName = AddPeriodToInitial(PersonCWRow[U.FirstName_col]) + " " + AddPeriodToInitial(PersonCWRow[U.MiddleName_col]);
                    outpurStr += CreateString(firstName.Trim());
                    outpurStr += CreateDate(PersonCWRow[U.BornDate_col]);
                    outpurStr += CreateString(PersonCWRow[U.EnlistmentDate_col]);
                    outpurStr += CreateDate(PersonCWRow[U.DiedDate_col]);
                    outpurStr += CreateString(PersonCWRow[U.CemeteryName_col]);
                    outpurStr += CreateString(PersonCWRow[U.BattleSiteKilled_col]);
                    outpurStr += CreateString(PersonCWRow[U.DataMilitary_col]);
                    outpurStr += CreateString(PersonCWRow[U.Reference_col]);
                    outpurStr += CreateString(PersonCWRow[U.Notes_col]);
                    srNew.WriteLine(outpurStr);
                    srNew.WriteLine("");
                    PersonCWRow[U.BornDate_col] = ConvertDateFromMDYToYMD(PersonCWRow[U.BornDate_col].ToString());
                    PersonCWRow[U.DiedDate_col] = ConvertDateFromMDYToYMD(PersonCWRow[U.DiedDate_col].ToString());
                    PersonCWRow[U.EnlistmentDate_col] = ConvertDateFromMDYToYMD(PersonCWRow[U.EnlistmentDate_col].ToString());
                    count++;
                }
            }
            SQL.CreateNewPersonCWRecords(PersonCWTbl);
        }
        private const char cQuote = '"';
        private string quote = cQuote.ToString();
        //****************************************************************************************************************************
        private string CreateDate(object obj)
        {
            string str = obj.ToString();
            if (str.Length == 4)
            {
                str = "1/1/" + str + "*";
            }
            return CreateString(str);
        }
        //****************************************************************************************************************************
        private string CreateString(object obj)
        {
            string str = obj.ToString();
            if (str.Contains(quote))
            {
                str = str.Replace(quote, quote + quote);
            }
            return quote + str + quote + ',';
        }
        //****************************************************************************************************************************
        private string AddPeriodToInitial(object obj)
        {
            string str = obj.ToString();
            if (str.Length == 1)
            {
                str += ".";
            }
            return str;
        }
        //****************************************************************************************************************************
        private void  AddPersonToDatabase()
        {
            for (int i = 0; i < numberOfExpectedFields; i++)
            {
                CheckColumn(i, row[i].ToString());
            }
            string lastName = row[0].ToString();
            string firstname = row[1].ToString();
            if (String.IsNullOrEmpty(lastName))
            {
                m_sInputRecord = ReadRecord();
                return;
            }
            CheckName(ref lastName, ref firstname);
            string middleName = "";
            int indexOfSpace = firstname.IndexOf(' ');
            if (indexOfSpace > 0)
            {
                middleName = firstname.Substring(indexOfSpace + 1).Trim().Replace(".", "");
                firstname = firstname.Substring(0, indexOfSpace).Trim();
            }
            firstname = firstname.Replace(".", "");
            string bornDate = row[2].ToString();
            string bornLocation = "";
            string enlistmentDate = row[3].ToString();
            string diedDate = row[4].ToString();
            indexOfSpace = diedDate.IndexOf(' ');
            if (indexOfSpace > 0)
            {
                diedDate = diedDate.Substring(0, indexOfSpace);
            }
            string cemetery = row[5].ToString();
            string BattleSiteKilled = row[6].ToString();
            string militaryData = row[7].ToString();
            string notes = row[8].ToString();
            string reference = row[9].ToString();
            string ten = row[10].ToString();
            string eleven = row[11].ToString();
            string twelve = row[12].ToString();
            if (!String.IsNullOrEmpty(twelve))
            {
                notes += twelve;
            }
            if (String.IsNullOrEmpty(militaryData) && !String.IsNullOrEmpty(notes))
            {
                militaryData = notes;
                notes = "";
            }
            if (String.IsNullOrEmpty(bornDate))
            {
                GetDatesAndCemeteryFromNextRecord(ref bornDate, ref bornLocation, ref diedDate, ref cemetery);
            }
            else
            {
                m_sInputRecord = ReadRecord();
            }
            DataRow PersonCWRow = PersonCWTbl.NewRow();
            PersonCWRow[U.LastName_col] = lastName;
            PersonCWRow[U.FirstName_col] = firstname;
            PersonCWRow[U.MiddleName_col] = middleName;
            PersonCWRow[U.BornDate_col] = bornDate;
            PersonCWRow[U.DiedDate_col] = diedDate;
            PersonCWRow[U.EnlistmentDate_col] = enlistmentDate;
            PersonCWRow[U.CemeteryName_col] = cemetery;
            PersonCWRow[U.BattleSiteKilled_col] = BattleSiteKilled;
            PersonCWRow[U.DataMilitary_col] = militaryData;
            PersonCWRow[U.Reference_col] = reference;
            PersonCWRow[U.Notes_col] = notes;
            PersonCWRow[U.PersonID_col] = 0;
            PersonCWRow[U.PersonCWID_col] = 0;
            PersonCWTbl.Rows.Add(PersonCWRow);
        }
        //****************************************************************************************************************************
        private static string ConvertDateFromMDYToYMD(string dateStr)
        {
            if (String.IsNullOrEmpty(dateStr) || dateStr.ToLower() == "unknown")
            {
                return "";
            }
            dateStr = dateStr.Replace("//", "/");
            int indexOfSemicolon = dateStr.IndexOf(';');
            if (indexOfSemicolon > 0)
            {
                dateStr = dateStr.Remove(indexOfSemicolon);
            }
            if (dateStr.Length == 4)
            {
                return dateStr;
            }
            bool foundYearOnly = false;
            if (dateStr.Contains("*"))
            {
                dateStr = dateStr.Replace("*", "");
                foundYearOnly = true;
            }
            int int1, int2, int3;
            U.SplitDate(dateStr, out int1, out int2, out int3);
            if (int3 == 0)
            {
                if (int2 > 1800)
                {
                    return int2.ToString();
                }
                return dateStr;
            }
            if (int3 < 100)
            {
                int3 += 1800;
            }
            if (foundYearOnly || int2 == 0 || int1 == 0)
            {
                return int3.ToString();
            }
            return U.BuildDate(int3, int1, int2);
        }
        //****************************************************************************************************************************
        private void GetDatesAndCemeteryFromNextRecord(ref string bornDate, ref string bornLocation, ref string diedDate, ref string cemetery)
        {
            rowIndex++;
            m_sInputRecord = ReadRecord();
            if (m_sInputRecord == null)
            {
                bornDate = "unknown";
                bornLocation = "unknown";
                diedDate = "unknown";
                cemetery = "unknown";
            }
            else
            {
                row = ReadCommaDelimitedRow(numberOfExpectedFields, m_sInputRecord);
                string dates = row[8].ToString();
                if (!dates.ToLower().Contains("born:"))
                {
                    bornDate = "unknown";
                    bornLocation = "unknown";
                    diedDate = "unknown";
                    cemetery = "unknown";
                }
                else
                {
                    GetDatesAndCemeteryFromInputRecord(dates, ref bornDate, ref bornLocation, ref diedDate, ref cemetery);
                    m_sInputRecord = ReadRecord();
                }
            }
        }
        //****************************************************************************************************************************
        private void GetDatesAndCemeteryFromInputRecord(string dates, ref string bornDate, ref string bornLocation, ref string diedDate, ref string cemetery)
        {
            int indexOfBornDate = dates.ToLower().IndexOf("born:");
            bornDate = GetInfoFromDateString(dates, indexOfBornDate, 5);
            int indexOfFirstComma = bornDate.IndexOf(',');
            if (indexOfFirstComma > 0)
            {
                bornLocation = bornDate.Substring(indexOfFirstComma + 1).Trim();
                bornDate = bornDate.Substring(0, indexOfFirstComma).Trim();
            }
            int indexOfAbt = bornDate.ToLower().IndexOf("abt");
            if (indexOfAbt >= 0)
            {
                bornDate = bornDate.Substring(indexOfAbt + 4).Trim();
            }
            int indexOfDiedDate = dates.ToLower().IndexOf("died:");
            diedDate = GetInfoFromDateString(dates, indexOfDiedDate, 5);
            int indexOfAft = diedDate.ToLower().IndexOf("aft");
            if (indexOfAft >= 0)
            {
                diedDate = diedDate.Substring(indexOfAft + 4).Trim();
            }
            int indexOfBuried = dates.ToLower().IndexOf("buried:");
            cemetery = GetInfoFromDateString(dates, indexOfBuried, 7, false);
            DateCheck.Add(cemetery);
        }
        //****************************************************************************************************************************
        private string GetInfoFromDateString(string dates, int indexOfString, int lengthOfIdentifier, bool lowerCase = true)
        {
            if (indexOfString < 0)
            {
                return "";
            }
            int indexOfSemicolon = dates.IndexOf(';', indexOfString);
            if (indexOfSemicolon > 0)
            {
                return ReturnString(dates.Substring(indexOfString + lengthOfIdentifier, indexOfSemicolon - indexOfString - lengthOfIdentifier), lowerCase);
            }
            return ReturnString(dates.Substring(indexOfString + lengthOfIdentifier), lowerCase);
        }
        //****************************************************************************************************************************
        private string ReturnString(string returnStr, bool lowerCase)
        {
            returnStr = returnStr.Trim();
            if (String.IsNullOrEmpty(returnStr))
            {
                return "unknown";
            }
            if (lowerCase)
            {
                return char.ToLower(returnStr[0]) + returnStr.Substring(1);
            }
            return returnStr;
        }
        //****************************************************************************************************************************
        private void CheckName(ref string lastName, ref string firstName)
        {
            int indexOfComma = lastName.IndexOf(',');
            if (indexOfComma > 0)
            {
                firstName = lastName.Substring(indexOfComma + 1).Trim();
                lastName = lastName.Substring(0, indexOfComma).Trim();
            }
            if (String.IsNullOrEmpty(firstName))
            {
                MessageBox.Show("Row contains no first name: " + lastName);
            }
        }
        //****************************************************************************************************************************
        private void CheckColumn(int colIndex, string colString)
        {
            if (String.IsNullOrEmpty(colString))
            {
                return;
            }
            foreach (int col in columnsFound)
            {
                if (col == colIndex)
                {
                    return;
                }
            }
            if (colIndex > 10)
            {
            }
            columnsFound.Add(colIndex);
        }
        //****************************************************************************************************************************
        private bool OpenFile()
        {
            string sFilter = "CSV Delimited Files (csv)|*.csv";
            string filename;
            OpenInputFile(sFilter, out filename, true);
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }
            m_sInputRecord = ReadRecord();
            string[] sInputFields = m_sInputRecord.Split(',');
            numberOfExpectedFields = sInputFields.Length;
            HeaderRow = ReadCommaDelimitedRow(numberOfExpectedFields, m_sInputRecord);
            return true;
        }
        //****************************************************************************************************************************
    }
}
