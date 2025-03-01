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
    public class CImportSchoolRecords : CImport
    {
        enum SchoolType
        {
            Primary = 0,
            Grammer = 1,
            OneRoom = 2,
            multipleGrades = 3
        }
        private SchoolType schoolType;
        private string m_sInputRecord;
        private ArrayList HeaderRow;
        private int schoolID;
        private string school;
        private DataTable schoolRecordsTbl;
        private DataTable personTbl;
        private int schoolYear;
        private bool messageAlreadyAsked = false;
        int numberOfExpectedFields;
        //****************************************************************************************************************************
        public CImportSchoolRecords(CSql Sql, string sDataDirectory)
            : base(Sql, sDataDirectory)
        {
            try
            {
                if (!OpenFileAndGetSchoolID())
                {
                    return;
                }
                personTbl = SQL.GetAllPersons();
                schoolRecordsTbl = SQL.GetAllSchoolRecords();
                m_sInputRecord = ReadRecord();
                int rowIndex = 1;
                while (m_sInputRecord != null)
                {
                    try
                    {
                        rowIndex++;
                        ArrayList row = ReadCommaDelimitedRow(numberOfExpectedFields, m_sInputRecord);
                        switch (rowIndex)
                        {
                            case 1: HeaderRow = row; break;
                            case 2: AddTeachersDatabase(row); break;
                            default: AddStudentToDatabase(row); break;
                        }
                        m_sInputRecord = ReadRecord();
                    }
                    catch (Exception ex)
                    {
                        string message = "Row: " + rowIndex + " - " + ex.Message;
                        MessageBox.Show(message);
                        throw new Exception("School Records Aborted");
                    }
                }
                SQL.InsertSchoolRecords(schoolRecordsTbl, personTbl);
                MessageBox.Show("School Records for " + school + " " + schoolYear + " Complete");
                CloseInputFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private class PersonGrade
        {
            public string person;
            public string grade;
            public PersonGrade(string person, string grade)
            {
                this.person = person;
                this.grade = grade;
            }
            public void AddGrade(string grade)
            {
                this.grade += ("," + grade);
            }
        }
        //****************************************************************************************************************************
        private void AddTeachersDatabase(ArrayList row)
        {
            if (row[0].ToString() != "Teachers")
            {
                throw new Exception("Row 2 does not contain string 'Teachers'");
            }
            int year = HeaderRow[0].ToInt();
            if (year != schoolYear)
            {
                throw new Exception("Warning: School Year in Sheet (" + year + ") <> School Year in File Name (" + schoolYear + ")");
            }
            if (schoolType == SchoolType.multipleGrades)
            {
                AddTeachers(row);
            }
        }
        //****************************************************************************************************************************
        private void SetSchoolType()
        {
            schoolType = SchoolType.multipleGrades;
            if (HeaderRow.Count < 4)
            {
                if (school.ToLower().Contains("primary"))
                {
                    school = school.Replace("Primary", "");
                    schoolType = SchoolType.Primary;
                }
                else if (school.ToLower().Contains("grammer"))
                {
                    school = school.Replace("Grammer", "");
                    schoolType = SchoolType.Grammer;
                }
                else
                {
                    schoolType = SchoolType.OneRoom;
                }
            }
        }
        //****************************************************************************************************************************
        private void AddTeachers(ArrayList row)
        {
            int colIndex = 2;
            ArrayList teachers = new ArrayList();
            while (colIndex < row.Count)
            {
                string teacher = row[colIndex].ToString().Trim();
                if (!String.IsNullOrEmpty(teacher))
                {
                    string grade = HeaderRow[colIndex].ToString();
                    if (String.IsNullOrEmpty(grade))
                    {
                        throw new Exception("Fatal Error - Teacher found with no Grade: " + teacher);
                    }
                    bool found = false;
                    foreach (PersonGrade personGrade in teachers)
                    {
                        if (teacher == personGrade.person)
                        {
                            personGrade.AddGrade(grade);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        teachers.Add(new PersonGrade(teacher, grade));
                    }
                }
                colIndex++;
            }
            foreach (PersonGrade personGrade in teachers)
            {
                AddPersonToDatabase(personGrade.person, SQL.SchoolRecordType.teacherType, personGrade.grade, "");
            }
        }
        //****************************************************************************************************************************
        private void AddOneRoomStudentToDatabase(ArrayList row)
        {
            string teacher = row[0].ToString().Trim();
            var grade = "";
            if (schoolType == SchoolType.Primary)
            {
                grade = "Primary";
            }
            if (schoolType == SchoolType.Grammer)
            {
                grade = "Grammer";
            }
            if (!String.IsNullOrEmpty(teacher))
            {
                AddPersonToDatabase(teacher, SQL.SchoolRecordType.teacherType, grade, "");
            }
            string age = row[1].ToString().Trim();
            string student = row[2].ToString().Trim();
            if (!String.IsNullOrEmpty(student))
            {
                AddPersonToDatabase(student, SQL.SchoolRecordType.studentType, grade, StudentBirthYear(age));
            }
        }
        //****************************************************************************************************************************
        private string StudentBirthYear(string age)
        {
            if (string.IsNullOrEmpty(age))
            {
                return "";
            }
            int birthYear = schoolYear + 1 - age.ToInt();
            return birthYear.ToString();
        }
        //****************************************************************************************************************************
        private void AddStudentToDatabase(ArrayList row)
        {
            int colIndex = 0;
            string personDate = "";
            while (colIndex < row.Count)
            {
                string str = row[colIndex].ToString().Trim();
                bool evenCol = colIndex % 2 == 1;
                if (colIndex == 0)
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        AddPersonToDatabase(str, SQL.SchoolRecordType.teacherType, "", "");
                    }
                }
                else if (evenCol)
                {
                    personDate = ParseDate(str);
                }
                else if (!String.IsNullOrEmpty(str))
                {
                    string grade = HeaderRow[colIndex].ToString();
                    AddPersonToDatabase(str, SQL.SchoolRecordType.studentType, grade, personDate);
                    personDate = "";
                }
                colIndex++;
            }
        }
        //****************************************************************************************************************************
        private string ParseDate(string dateStr)
        {
            if (String.IsNullOrEmpty(dateStr))
            {
                return "";
            }
            int indexOfDash = dateStr.IndexOf('-');
            if (indexOfDash > 0)
            {
                return DetermineDate(dateStr, indexOfDash);
            }
            int indexOfFirstSlash = dateStr.IndexOf('/');
            if (indexOfFirstSlash <= 0)
            {
                return GetDateYear(dateStr);
            }
            int indexOfSecondSlash = dateStr.IndexOf('/', (indexOfFirstSlash + 1)).ToInt();
            if (indexOfSecondSlash <= 0)
            {
                throw new Exception("Invalid Date: " + dateStr);
            }
            int dayLength = indexOfSecondSlash - indexOfFirstSlash - 1;
            int month = dateStr.Substring(0, indexOfFirstSlash).ToInt();
            int day = dateStr.Substring(indexOfFirstSlash + 1, dayLength).ToInt();
            int year = dateStr.Substring(indexOfSecondSlash + 1).ToInt();
            CheckDateValues(month, day, year);
            try
            {
                DateTime thisDate = new DateTime(year, month, day);
                return thisDate.ToString("yyyy/MM/dd");
            }
            catch (Exception)
            {
                throw new Exception("Date Values Invalid: " + month + "/" + day + "/" + year);
            }
        }
        //****************************************************************************************************************************
        private string DetermineDate(string dateStr, int indexOfDash)
        {
            int years = dateStr.Substring(0, indexOfDash).ToInt();
            int month = dateStr.Substring(indexOfDash + 1).ToInt();
            return "";
        }
        //****************************************************************************************************************************
        private string GetDateYear(string dateStr)
        {
            if (dateStr.Length == 4)
            {
                return dateStr;
            }
            else
            {
                throw new Exception("Invalid Date: " + dateStr);
            }
        }
        //****************************************************************************************************************************
        private void CheckDateValues(int month,
                                     int day,
                                     int year)
        {
            if (year < 1750 || year >= schoolYear - 4)
            {
                throw new Exception("Invalid Year: " + year);
            }
            if (month <= 0 || month > 12)
            {
                throw new Exception("Invalid Month: " + month);
            }
            int lastDay;
            switch (month)
            {
                case 1: 
                case 3: 
                case 5: 
                case 7: 
                case 8: 
                case 10: 
                case 12: lastDay = 31; break;
                case 2: lastDay = 29; break;
                default: lastDay = 30; break;
            }
            if (month == 2)
            {
                lastDay = U.LastDayOfMonth(month, year);
            }
            if (day <= 0 || day > lastDay)
            {
                throw new Exception("Invalid Day: " + day);
            }
        }
        //****************************************************************************************************************************
        private void AddPersonToDatabase(string person,
                                         SQL.SchoolRecordType schoolRecordType,
                                         string grade,
                                         string bornDate)
        {
            if (!person.Contains(","))
            {
                string message = "Fatal Error - Person with no first name.  Probable missing comma: " + person;
                throw new Exception(message);
            }
            person = person.Replace("'", "''");
            string selectStatement = U.SchoolID_col + "=" + schoolID + " and " +
                                     U.Year_col + "=" + schoolYear + " and " +
                                     U.Person_col + " = '" + person + "' and " +
                                     U.Grade_col + "= '" + grade + "'";
            DataRow[] foundRows = schoolRecordsTbl.Select(selectStatement);
            if (foundRows.Length != 0)
            {
                ShowMessageOnce(person, selectStatement);
                updateDate(foundRows[0], bornDate);
                return;
            }
            DataRow personRow = schoolRecordsTbl.NewRow();
            personRow[U.SchoolRecordID_col] = 0;
            personRow[U.SchoolID_col] = schoolID;
            personRow[U.SchoolRecordType_col] = (int) schoolRecordType;
            personRow[U.Year_col] = schoolYear;
            personRow[U.Grade_col] = grade;
            personRow[U.BornDate_col] = bornDate;
            personRow[U.Person_col] = person;
            personRow[U.PersonID_col] = 0;
            schoolRecordsTbl.Rows.Add(personRow);
        }
        //****************************************************************************************************************************
        private void ShowMessageOnce(string person, string selectStatement)
        {
            if (messageAlreadyAsked)
            {
                return;
            }
            messageAlreadyAsked = true;
            string message = "Person/Grade combination already in Database: \r\r" + selectStatement;
            if (MessageBox.Show("Abort(Yes / No) - " + message, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                throw new Exception("Fatal Error - " + message);
            }
        }
        //****************************************************************************************************************************
        private void updateDate(DataRow schoolRow,
                                string  bornDate)
        {
            if (String.IsNullOrEmpty(bornDate) || bornDate.Length < 10)
            {
                return;
            }
            if (bornDate == schoolRow[U.BornDate_col].ToString())
            {
                return;
            }
            schoolRow[U.BornDate_col] = bornDate;
            int personId = schoolRow[U.PersonID_col].ToInt();
            if (personId == 0)
            {
                return;
            }
            string selectStatement = U.PersonID_col + "=" + personId;
            DataRow[] foundRows = personTbl.Select(selectStatement);
            if (foundRows.Length == 0)
            {
                MessageBox.Show("Unable to Find Person wuth id " + personId);
                return;
            }
            DataRow personRow = foundRows[0];
            string source = personRow[U.BornSource_col].ToString();
            if (source == "School Records" && personRow[U.BornDate_col].ToString().Length < 10)
            {
                personRow[U.BornDate_col] = bornDate;
            }
        }
        //****************************************************************************************************************************
        private bool OpenFileAndGetSchoolID()
        {
            string sFilter = "CSV Delimited Files (csv)|*.csv";
            string filename;
            OpenInputFile(sFilter, out filename);
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }
            school = Path.GetFileNameWithoutExtension(filename);
            int firstSpaceIndex = school.IndexOf(' ');
            if (firstSpaceIndex <= 0)
            {
                MessageBox.Show("Invalid School FileName: " + school);
                return false;
            }
            schoolYear = school.Remove(0, firstSpaceIndex).ToInt();
            DateTime thisDay = DateTime.Today;
            if (schoolYear == 0 || schoolYear < 1800 || schoolYear > thisDay.Year)
            {
                throw new Exception("Invalid School Year in Filename: " + school);
            }
            m_sInputRecord = ReadRecord();
            string[] sInputFields = m_sInputRecord.Split(',');
            numberOfExpectedFields = sInputFields.Length;
            HeaderRow = ReadCommaDelimitedRow(numberOfExpectedFields, m_sInputRecord);

            school = school.Remove(firstSpaceIndex);
            SetSchoolType();
            schoolID = SQL.GetSchoolID(school);
            if (schoolID == 0)
            {
                schoolID = SQL.InsertSchool(school);
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
