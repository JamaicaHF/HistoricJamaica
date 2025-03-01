using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

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
        private int schoolID;
        private string school;
        private DataTable schoolRecordsTbl;
        private DataTable personTbl;
        private int schoolYear;
        private int ageAdjustment = 0;
        private bool messageAlreadyAsked = false;
        private EPPlus epPlus;
        //****************************************************************************************************************************
        public CImportSchoolRecords(CSql Sql, string sDataDirectory)
            : base(Sql, sDataDirectory)
        {
            personTbl = SQL.GetAllPersons();
            schoolRecordsTbl = SQL.GetAllSchoolRecords();
        }
        //****************************************************************************************************************************
        public void GetSchoolRecordsForFile()
        {
            using (epPlus = new EPPlus())
            {
                string filename;
                GetExcelInputFile(@"c:\JHF\SchoolRecords", "School", out filename);
                if (!string.IsNullOrEmpty(filename))
                {
                    GetSchoolRecords(filename);
                    SQL.UpdateInsertSchoolRecords(schoolRecordsTbl, personTbl);
                    MessageBox.Show("School Records for " + school + " " + schoolYear + " Complete");
                }
            }
        }
        //****************************************************************************************************************************
        public void GetSchoolRecordsForAllFiles()
        {
            using (epPlus = new EPPlus())
            {
                string[] files = Directory.GetFiles(@"C:\JHF\SchoolRecords");
                foreach (string filename in files)
                {
                    string schoolAndYear = Path.GetFileNameWithoutExtension(filename);
                    int indexOfSpace = schoolAndYear.IndexOf(' ');
                    if (indexOfSpace > 0)
                    {
                        string school = schoolAndYear.Substring(0, indexOfSpace).Trim();
                        int schoolYear = schoolAndYear.Substring(indexOfSpace + 1).Trim().ToInt();
                        if (school.ToLower() != "elementary" && schoolYear < 1938)
                        {
                            GetSchoolRecords(filename);
                        }
                    }
                }
            }
            SQL.UpdateInsertSchoolRecords(schoolRecordsTbl, personTbl);
            MessageBox.Show("All School Records Import Complete");
        }
        //****************************************************************************************************************************
        private void GetSchoolRecords(string filename)
        {
            try
            {
                if (!OpenFileAndGetSchoolID(filename))
                {
                    return;
                }
                int rowIndex = 2;
                AddTeachersDatabase(rowIndex);
                while (rowIndex < epPlus.numRows)
                {
                    try
                    {
                        rowIndex++;
                        AddStudentToDatabase(rowIndex);
                    }
                    catch (Exception ex)
                    {
                        string message = "Row: " + rowIndex + " - " + ex.Message;
                        MessageBox.Show(message);
                        throw new Exception("School Records Aborted");
                    }
                }
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
        private void AddTeachersDatabase(int rowIndex)
        {
            if (epPlus.GetCellValue(rowIndex, 1).ToString() != "Teachers")
            {
                throw new Exception("Row 2 does not contain string 'Teachers'");
            }
            int year = epPlus.GetCellValue(1, 1).ToInt();
            if (year != schoolYear)
            {
                throw new Exception("Warning: School Year in Sheet (" + year + ") <> School Year in File Name (" + schoolYear + ")");
            }
            if (epPlus.GetCellValue(1, 2).ToString().ToLower() == "age")
            {
                SetAgeAdjustment(epPlus.GetCellValue(rowIndex, 2).ToString());
            }
            if (schoolType == SchoolType.multipleGrades)
            {
                AddTeachers(rowIndex);
            }
        }
        //****************************************************************************************************************************
        private void SetAgeAdjustment(string asOfDate)
        {
            try
            {
                if (string.IsNullOrEmpty(asOfDate))
                {
                    ageAdjustment = 0;
                    return;
                }
                int indexOfSlash = asOfDate.IndexOf('/');
                long dateNum;
                if (indexOfSlash > 0)
                {
                    dateNum = asOfDate.Substring(0, indexOfSlash).ToInt();
                }
                else
                {
                    dateNum = long.Parse(asOfDate);
                }
                if (dateNum == 0)
                {
                    throw new Exception("");
                }
                if (dateNum > 12)
                {
                    DateTime result = DateTime.FromOADate(dateNum);
                    dateNum = result.Month;
                    if (!ValidSchoolYear(result.Year))
                    {
                        throw new Exception("");
                    }
                }
                ageAdjustment = (dateNum > 7 || schoolYear < 1920) ? 0 : 1;  // 1 means asOfMonth is end of school year when school year is sept through June (after 1910)
            }
            catch (Exception)
            {
                throw new Exception("Invalid Date [2, 2]: " + asOfDate);
            }
        }
        //****************************************************************************************************************************
        private bool ValidSchoolYear(int year)
        {
            if (year < 1780 || year > 2030)
            {
                return false;
            }
            if (year == schoolYear || year == schoolYear + 1)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private void SetSchoolType()
        {
            schoolType = SchoolType.multipleGrades;
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
            else if (String.IsNullOrEmpty(epPlus.GetCellValue(1, 3).ToString()))
            {
                schoolType = SchoolType.OneRoom;
            }
        }
        //****************************************************************************************************************************
        private void AddTeachers(int rowIndex)
        {
            int colIndex = 3;
            ArrayList teachers = new ArrayList();
            while (colIndex <= epPlus.numCols)
            {
                string teacher = epPlus.GetCellValue(rowIndex, colIndex).ToString().Trim();
                if (!String.IsNullOrEmpty(teacher))
                {
                    string grade = epPlus.GetCellValue(1, colIndex).ToString();
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
        private string StudentBirthYear(string ageStr)
        {
            if (string.IsNullOrEmpty(ageStr))
            {
                return "";
            }
            int age = ageStr.ToInt();
            if (age < 4 || age > 21)
            {
                throw new Exception("Invalid Date Age: " + age);
            }
            int birthYear = schoolYear + ageAdjustment - age.ToInt();
            return birthYear.ToString();
        }
        //****************************************************************************************************************************
        private void AddStudentToDatabase(int rowIndex)
        {
            string personDate = "";
            int colIndex = 1;
            while (colIndex <= epPlus.numCols)
            {
                string str = epPlus.GetCellValue(rowIndex, colIndex).ToString().Trim();
                bool evenCol = colIndex % 2 == 0;
                if (colIndex == 1)
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        string grade = SetGrade(colIndex);
                        AddPersonToDatabase(str, SQL.SchoolRecordType.teacherType, grade, "");
                    }
                }
                else if (evenCol)
                {
                    personDate = ParseDate(str, rowIndex, colIndex);
                }
                else if (!String.IsNullOrEmpty(str))
                {
                    string grade = SetGrade(colIndex);
                    AddPersonToDatabase(str, SQL.SchoolRecordType.studentType, grade, personDate);
                    personDate = "";
                }
                colIndex++;
            }
        }
        //****************************************************************************************************************************
        private string SetGrade(int colIndex)
        {
            if (schoolType == SchoolType.Primary)
            {
                return "Primary";
            }
            if (schoolType == SchoolType.Grammer)
            {
                return "Grammer";
            }
            if (colIndex == 1)
            {
                return "";
            }
            return epPlus.GetCellValue(1, colIndex).ToString();
        }
        //****************************************************************************************************************************
        private string ParseDate(string dateStr, int rowIndex, int colIndex)
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
            if (indexOfFirstSlash > 0)
            {
                return ParseDateString(dateStr, indexOfFirstSlash, rowIndex, colIndex);
            }
            long dateNum = long.Parse(dateStr);
            if (dateNum < 2030)
            {
                return GetDateYear(dateStr);
            }
//                throw new Exception("Invalid Date: " + dateStr);
            DateTime result = DateTime.FromOADate(dateNum);
            int month = result.Month;
            int day = result.Day;
            int year = result.Year;
            if (!U.ValidDate(year, month, day, schoolYear - 4))
            {
                throw new Exception("Invalid Day: " + dateStr);
            }
            try
            {
                DateTime thisDate = new DateTime(year, month, day);
                return thisDate.ToString("yyyy/MM/dd");
            }
            catch (Exception)
            {
                throw new Exception("Invalid Date [" + rowIndex + ", " + colIndex + "]: " + month + "/" + day + "/" + year);
            }
        }
        //****************************************************************************************************************************
        private string ParseDateString(string dateStr, int indexOfFirstSlash, int rowIndex, int colIndex)
        {
            if (indexOfFirstSlash <= 0)
            {
                return GetDateYear(dateStr);
            }
            int indexOfSecondSlash = dateStr.IndexOf('/', (indexOfFirstSlash + 1)).ToInt();
            if (indexOfSecondSlash <= 0)
            {
                throw new Exception("Invalid Date [" + rowIndex + ", " + colIndex + "]: " + dateStr);
            }
            int dayLength = indexOfSecondSlash - indexOfFirstSlash - 1;
            int month = dateStr.Substring(0, indexOfFirstSlash).ToInt();
            int day = dateStr.Substring(indexOfFirstSlash + 1, dayLength).ToInt();
            int year = dateStr.Substring(indexOfSecondSlash + 1).ToInt();
            if (!U.ValidDate(year, month, day, schoolYear - 4))
            {
                throw new Exception("Invalid Day: " + dateStr);
            }
            try
            {
                DateTime thisDate = new DateTime(year, month, day);
                return thisDate.ToString("yyyy/MM/dd");
            }
            catch (Exception)
            {
                throw new Exception("Invalid Date [" + rowIndex + ", " + colIndex + "]: " + month + "/" + day + "/" + year);
            }
        }
        //****************************************************************************************************************************
        private string DetermineDate(string dateStr, int indexOfDash)
        {
            int years = dateStr.Substring(0, indexOfDash).ToInt();
            int month = dateStr.Substring(indexOfDash + 1).ToInt();
            int birthYear = schoolYear - years;
            int birthMonth = 8 - month;
            if (birthMonth < 0)
            {
                birthYear--;
                birthMonth += 12;
            }
            return birthYear.ToString();
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
                return StudentBirthYear(dateStr);
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
                                     U.Person_col + " = '" + person + "'";
            if (schoolRecordType == SQL.SchoolRecordType.studentType)
            {
                selectStatement += " and " + U.Grade_col + " = '" + grade + "'";
            }
            DataRow[] foundRows = schoolRecordsTbl.Select(selectStatement);
            if (foundRows.Length != 0)
            {
                if (schoolRecordType == SQL.SchoolRecordType.studentType)
                {
                    updateDate(foundRows[0], bornDate);
                }
                else
                {
                    foundRows[0][U.Grade_col] = grade;
                }
                return;
            }
            DataRow personRow = schoolRecordsTbl.NewRow();
            personRow[U.SchoolRecordID_col] = 0;
            personRow[U.SchoolID_col] = schoolID;
            personRow[U.SchoolRecordType_col] = (int) schoolRecordType;
            personRow[U.Year_col] = schoolYear;
            personRow[U.Grade_col] = grade;
            personRow[U.BornDate_col] = bornDate;
            person = person.Replace("''", "'");
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
                string oldBornDate = schoolRow[U.BornDate_col].ToString();
                if (oldBornDate.Length < 10 && oldBornDate != bornDate)
                {
                    schoolRow[U.BornDate_col] = bornDate;
                }
                return;
            }
            if (bornDate != schoolRow[U.BornDate_col].ToString())
            {
                schoolRow[U.BornDate_col] = bornDate;
            }
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
        private bool OpenFileAndGetSchoolID(string filename)
        {
            epPlus.OpenWithEPPlus(filename);
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }
            return GetSchoolID(filename);
        }
        //****************************************************************************************************************************
        private bool OpenFileAndGetSchoolIDCsv()
        {
            string sFilter = "CSV Delimited Files (csv)|*.csv";
            string filename;
            OpenInputFile(sFilter, out filename);
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }
            return GetSchoolID(filename);
        }
        //****************************************************************************************************************************
        private bool GetSchoolID(string filename)
        {
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
