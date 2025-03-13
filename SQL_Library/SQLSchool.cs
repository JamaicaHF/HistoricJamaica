using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace SQL_Library
{
    public partial class SQL
    {
        public enum SchoolRecordType
        {
            teacherType = 0,
            studentType = 1
        }
        //****************************************************************************************************************************
        public static SchoolRecordType ToSchoolRecordType(this object recordType)
        {
            return (SchoolRecordType)recordType;
        }
        //****************************************************************************************************************************
        public static DataTable DefineSchoolTable()
        {
            DataTable tbl = new DataTable(U.School_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.SchoolID_col, typeof(int));
            tbl.Columns.Add(U.School_col, typeof(string));
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.SchoolID_col] };
            tbl.Columns[U.School_col].MaxLength = U.iMaxValueLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefineSchoolRecordTable()
        {
            DataTable tbl = new DataTable(U.SchoolRecord_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.SchoolRecordID_col, typeof(int));
            tbl.Columns.Add(U.SchoolID_col, typeof(int));
            tbl.Columns.Add(U.SchoolRecordType_col, typeof(int));
            tbl.Columns.Add(U.Year_col, typeof(int));
            tbl.Columns.Add(U.Grade_col, typeof(string));
            tbl.Columns.Add(U.BornDate_col, typeof(string));
            tbl.Columns.Add(U.Person_col, typeof(string));
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns[U.Grade_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.BornDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.Person_col].MaxLength = U.iMaxValueLength;
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.SchoolRecordID_col] };
            return tbl;
        }
        //****************************************************************************************************************************
        public static string GetSchool(int schoolId)
        {
            DataTable tbl = new DataTable(U.School_Table);
            SelectAll(U.School_Table, tbl, new NameValuePair(U.SchoolID_col, schoolId));
            if (tbl.Rows.Count == 0)     // does not exist
            {
                return "";
            }
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.School_col].ToString();
            }
        }
        //****************************************************************************************************************************
        public static int GetSchoolID(string school)
        {
            DataTable tbl = new DataTable(U.School_Table);
            SelectAll(U.School_Table, tbl, new NameValuePair(U.School_col, school));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.SchoolID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        private struct SchoolPersonInfo
        {
            public int schoolRecordID;
            public int schoolRecordType;
            public string bornDate;
            public string personName;
            public SchoolPersonInfo(int schoolRecordID, int schoolRecordType, string bornDate, string personName)
            {
                this.schoolRecordID = schoolRecordID;
                this.schoolRecordType = schoolRecordType;
                this.bornDate = bornDate;
                this.personName = personName;
            }
        }
        //****************************************************************************************************************************
        public static void CheckSchoolRecordDates()
        {
            DataTable birthRecordsTbl = GetAllBirthVitalRecords(OrderBy(U.PersonID_col));
            DataTable schoolRecordsTbl = GetAllSchoolRecords(OrderBy(U.PersonID_col));
            DataTable personRecordsTbl = GetAllPersonsOrderBy(U.PersonID_col);
            DataTable AlternativeSpellingsFirstNameTbl = GetAllAlternativeSpellings(U.AlternativeSpellingsFirstName_Table);
            DataTable AlternativeSpellingsLastNameTbl = GetAllAlternativeSpellings(U.AlternativeSpellingsLastName_Table);
            int previousPersonID = 0;
            string previousPerson = "";
            ArrayList personInfoList = new ArrayList();
            ArrayList allPersons = new ArrayList();
            foreach (DataRow schoolRecordsRow in schoolRecordsTbl.Rows)
            {
                int personID = schoolRecordsRow[U.PersonID_col].ToInt();
                if (personID != previousPersonID)
                {
                    //if (previousPersonID == 856)
                    CheckAllDates(allPersons, personInfoList, birthRecordsTbl, schoolRecordsTbl, AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, personRecordsTbl, previousPersonID, previousPerson);
                    previousPersonID = personID;
                    previousPerson = schoolRecordsRow[U.Person_col].ToString();
                    personInfoList.Clear();
                }
                if (personID != 0)
                {
                    personInfoList.Add(new SchoolPersonInfo(schoolRecordsRow[U.SchoolRecordID_col].ToInt(), schoolRecordsRow[U.SchoolRecordType_col].ToInt(), schoolRecordsRow[U.BornDate_col].ToString(), schoolRecordsRow[U.Person_col].ToString()));
                }
            }
            CheckAllDates(allPersons, personInfoList, birthRecordsTbl, schoolRecordsTbl, AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, personRecordsTbl, previousPersonID, previousPerson);
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                UpdateWithDA(txn, schoolRecordsTbl, U.SchoolRecord_Table, U.SchoolRecordID_col, ColumnList(U.BornDate_col, U.Person_col));
                UpdateWithDA(txn, personRecordsTbl, U.Person_Table, U.PersonID_col, ColumnList(U.BornDate_col, U.BornSource_col));
                txn.Commit();
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw new Exception("Unable to update school records: " + ex.Message);
            }
        }
        private struct FoundName
        {
            public string name;
            public int num;
            public int schoolRecordType;
            public FoundName(string name, int schoolRecordType)
            {
                this.name = name;
                this.schoolRecordType = schoolRecordType;
                this.num = 1;
            }
            public void AddOne()
            {
                num++;
            }
        }
        private struct FoundDate
        {
            public string date;
            public int num;
            public FoundDate(string date)
            {
                this.date = date;
                this.num = 1;
            }
            public void AddOne()
            {
                num++;
            }
        }
        //****************************************************************************************************************************
        public static void CheckAllDates(ArrayList allPersons, 
                                         ArrayList personInfoList, 
                                         DataTable birthRecordsTbl,
                                         DataTable schoolRecordsForPersonIdTbl,
                                         DataTable AlternativeSpellingsFirstNameTbl,
                                         DataTable AlternativeSpellingsLastNameTbl,
                                         DataTable personRecordsTbl,
                                         int personID, 
                                         string person)
        {
            if (personInfoList.Count == 0)
            {
                return;
            }
            PersonName personName;
            string vitalRecordBirthDate = GetVitalRecordBirthDate(personInfoList, birthRecordsTbl, personID, out personName);
            if (!string.IsNullOrEmpty(vitalRecordBirthDate))
            {
                SetAllSchoolRecordsAndPersonRecordToBirthDate(personInfoList, schoolRecordsForPersonIdTbl, personRecordsTbl.Rows[0], vitalRecordBirthDate, personID, true);
                return;
            }
            string personSelectStatement = U.PersonID_col + " = " + personID.ToString();
            DataRow[] personResult = personRecordsTbl.Select(personSelectStatement);
            if (personResult.Length == 0)
            {
                return;
            }
            bool foundDifference = false;
            SetSchoolRecordsNameBornDates(personResult[0], schoolRecordsForPersonIdTbl, AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, personInfoList, personID, ref foundDifference);
        }
        //****************************************************************************************************************************
        private static string SetSchoolRecordsNameBornDates(DataRow personRow, 
                                                            DataTable schoolRecordsForPersonIdTbl,
                                                            DataTable AlternativeSpellingsFirstNameTbl,
                                                            DataTable AlternativeSpellingsLastNameTbl,
                                                            ArrayList personInfoList, 
                                                            int personId,
                                                        ref bool foundDifference,
                                                            string vitalRecordsBornDate="",
                                                            string schoolRecordsBornDate="")
        {
            ArrayList bornDateList = new ArrayList();
            ArrayList nameList = new ArrayList();
            GetSchoolRecordsNameBornDatesList(personInfoList, bornDateList, nameList);
            foundDifference = SetSchoolRecordsName(personRow, schoolRecordsForPersonIdTbl, AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, personInfoList, nameList, personId);
            if (!String.IsNullOrEmpty(vitalRecordsBornDate))
            {
                if (SetAllSchoolRecordsToNameOrBirthDate(personInfoList, schoolRecordsForPersonIdTbl, vitalRecordsBornDate, U.BornDate_col, personId))
                {
                    foundDifference = true;
                }
                return vitalRecordsBornDate;
            }
            else
            {
                return SetSchoolRecordsBornDate(personRow, schoolRecordsForPersonIdTbl, personInfoList, bornDateList, schoolRecordsBornDate, ref foundDifference, personId);
            }
        }
        //****************************************************************************************************************************
        private static string SetSchoolRecordsBornDate(DataRow personRow,
                                                 DataTable schoolRecordsForPersonIdTbl,
                                                 ArrayList personInfoList,
                                                 ArrayList bornDateList,
                                                 string schoolRecordBornDate,
                                             ref bool foundDifference,
                                                 int personId)
        {
            if (bornDateList.Count == 1)
            {
                FoundDate foundBornDate = (FoundDate)bornDateList[0];
                if (String.IsNullOrEmpty( foundBornDate.date))
                {
                    foundBornDate.date = schoolRecordBornDate;
                    foundDifference = SetAllSchoolRecordsAndPersonRecordToBirthDate(personInfoList, schoolRecordsForPersonIdTbl, personRow, schoolRecordBornDate, personId, false);
                }
                SetPersonBornDate(personRow, personId, foundBornDate.date, false);
                return foundBornDate.date;
            }
            else
            {
                string bestDate = SelectDateForAllRecords(bornDateList, schoolRecordBornDate);
                foundDifference = SetAllSchoolRecordsAndPersonRecordToBirthDate(personInfoList, schoolRecordsForPersonIdTbl, personRow, bestDate, personId, false);
                return bestDate;
            }
        }
        //****************************************************************************************************************************
        private static bool SetSchoolRecordsName(DataRow personRow,
                                                 DataTable schoolRecordsForPersonIdTbl,
                                                 DataTable AlternativeSpellingsFirstNameTbl,
                                                 DataTable AlternativeSpellingsLastNameTbl,
                                                 ArrayList personInfoList,
                                                 ArrayList nameList,
                                                 int personId)
        {
            PersonName schoolName = new PersonName(personRow);
            bool schoolNameChanged = SelectNameForAllRecords(nameList, AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, schoolName);
            if (schoolNameChanged)
            {
                string newName = BuildNameLastNameFirst(schoolName);
                return SetAllSchoolRecordsToNameOrBirthDate(personInfoList, schoolRecordsForPersonIdTbl, newName, U.Person_col, personId);
            }
            return false;
        }
        //****************************************************************************************************************************
        private static bool SetAllSchoolRecordsAndPersonRecordToBirthDate(ArrayList personInfoList,
                                                                 DataTable schoolRecordsTbl,
                                                                 DataRow personRow,
                                                                 string setBirthDate,
                                                                 int personId,
                                                                 bool vitalRecordFound)
        {
            SetPersonBornDate(personRow, personId, setBirthDate, vitalRecordFound);
            return SetAllSchoolRecordsToNameOrBirthDate(personInfoList, schoolRecordsTbl, setBirthDate, U.BornDate_col, personId);
        }
        //****************************************************************************************************************************
        private static void GetSchoolRecordsNameBornDatesList(ArrayList personInfoList, ArrayList dateListFound, ArrayList nameListFound)
        {
            SchoolPersonInfo firstSchoolPersonInfo = (SchoolPersonInfo)personInfoList[0];
            foreach (SchoolPersonInfo schoolPersonInfo in personInfoList)
            {
                AddFoundDate(dateListFound, schoolPersonInfo.bornDate);
                AddFoundName(nameListFound, schoolPersonInfo.personName, schoolPersonInfo.schoolRecordType);
            }
            return ;
        }
        //****************************************************************************************************************************
        private static void AddFoundName(ArrayList nameListFound, string thisName, int schoolRecordType)
        {

            bool found = false;
            int foundIndex = 0;
            foreach (FoundName foundName in nameListFound)
            {
                if (foundName.name == thisName)
                {
                    foundName.AddOne();
                    nameListFound[foundIndex] = foundName;
                    found = true;
                    return;
                }
                foundIndex++;
            }
            if (!found)
            {
                nameListFound.Add(new FoundName(thisName, schoolRecordType));
            }
        }
        //****************************************************************************************************************************
        private static void AddFoundDate(ArrayList dateListFound, string thisDate)
        {

            bool found = false;
            int foundIndex = 0;
            foreach (FoundDate foundDate in dateListFound)
            {
                if (foundDate.date == thisDate)
                {
                    foundDate.AddOne();
                    dateListFound[foundIndex] = foundDate;
                    found = true;
                    return;
                }
                foundIndex++;
            }
            if (!found)
            {
                dateListFound.Add(new FoundDate(thisDate));
            }
        }
        //****************************************************************************************************************************
        private static bool SelectNameForAllRecords(ArrayList nameListFound, 
                                                    DataTable AlternativeSpellingsFirstNameTbl,
                                                    DataTable AlternativeSpellingsLastNameTbl,
                                                    PersonName schoolName)
        {
            string newFirstName = schoolName.firstName;
            string newMiddleName = schoolName.middleName;
            string newSuffix = schoolName.suffix;
            bool atLeastOneSchoolRecordChanged = false;
            foreach (FoundName foundName in nameListFound)
            {
                PersonName foundPersonName = new PersonName(foundName.name);
                if (schoolName.lastName.ToLower() == foundPersonName.lastName.ToLower())
                {  // doNothing - Use Last Name
                }
                else if (foundName.schoolRecordType == 0 && schoolName.marriedName == foundPersonName.lastName)
                {  //  teacher with married name in school record
                }
                else if (AlternativeSpelling(AlternativeSpellingsLastNameTbl, schoolName.lastName, foundPersonName.lastName))
                {  // doNothing - Use Last Name
                }
                else if (foundName.schoolRecordType == 0 && AlternativeSpelling(AlternativeSpellingsLastNameTbl, schoolName.marriedName, foundPersonName.lastName))
                {
                }
                else
                {
                    MessageBox.Show("Married Last Name: " + schoolName.lastName + " " + foundPersonName.lastName);
                    schoolName.lastName = foundPersonName.lastName;
                    schoolName.marriedName = "";
                }
                newFirstName = schoolName.firstName.SelectFirstName(foundPersonName.firstName);
                if (foundPersonName.firstName != newFirstName)
                {
                    atLeastOneSchoolRecordChanged = true;
                }
                newMiddleName = schoolName.middleName.SelectMiddleName(foundPersonName.middleName, foundPersonName.firstName, schoolName.lastName, AlternativeSpellingsFirstNameTbl);
                if (foundPersonName.middleName != newMiddleName)
                {
                    atLeastOneSchoolRecordChanged = true;
                }
                newSuffix = schoolName.suffix.SelectSuffix(foundPersonName.suffix);
                if (foundPersonName.suffix != newSuffix)
                {
                    atLeastOneSchoolRecordChanged = true;
                }
            }
            schoolName.firstName = newFirstName;
            schoolName.middleName = newMiddleName;
            schoolName.suffix = newSuffix;
            return atLeastOneSchoolRecordChanged;
        }
        //****************************************************************************************************************************
        private static bool AlternativeSpelling(DataTable AlternativeSpellingsTbl, string name, string compareName)
        {
            string selectStatement = U.NameSpelling1_Col + " = '" + name + "'";
            DataRow[] results = AlternativeSpellingsTbl.Select(selectStatement);
            foreach (DataRow alternativeSpelling in results)
            {
                if (alternativeSpelling[U.NameSpelling2_Col].ToString() == compareName)
                {
                    return true;
                }
            }
            selectStatement = U.NameSpelling2_Col + " = '" + name + "'";
            results = AlternativeSpellingsTbl.Select(selectStatement);
            foreach (DataRow alternativeSpelling in results)
            {
                if (alternativeSpelling[U.NameSpelling1_Col].ToString() == compareName)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private static string SelectFirstName(this string firstName, string personFirstName)
        {
            if (firstName.Length == 0)
            {
                return personFirstName;
            }
            if (firstName.Length == 1 && personFirstName.Length > 1)
            {
                return personFirstName;
            }
            if (firstName.Length > 1)
            {
                return firstName;
            }
            if (firstName != personFirstName)
            {
                MessageBox.Show("Different First Names: " + firstName + " " + personFirstName);
            }
            return firstName;
        }
        //****************************************************************************************************************************
        private static string SelectMiddleName(this string middleName, string personMiddleName, string personFirstName, string lastName, DataTable AlternativeSpellingsFirstNameTbl)
        {
            if (middleName.Length == 0)
            {
                return personMiddleName;
            }
            if (middleName == personMiddleName)
            {
                return middleName;
            }
            if (middleName.Length == 1)
            {
                if (personMiddleName.Length > 1)
                {
                    return personMiddleName;
                }
            }
            if (personMiddleName.Length <= 1) // || middleName.Length > 1)
            {
                return middleName;
            }
            if (middleName == personFirstName)
            {
                return middleName;
            }
            if (personMiddleName == lastName)
            {
                return middleName;
            }
            if (AlternativeSpelling(AlternativeSpellingsFirstNameTbl, middleName, personMiddleName))
            {
                return middleName;
            }
            else
            {
                MessageBox.Show("Different Middle Names: " + middleName + " " + personMiddleName);
            }
            return middleName;
        }
        //****************************************************************************************************************************
        private static string SelectSuffix(this string suffix, string personSuffix)
        {
            if (suffix.Length == 0 || personSuffix.Length == 0)
            {
                return personSuffix;
            }
            if (suffix != personSuffix)
            {
                MessageBox.Show("Different Suffix: " + suffix + " " + personSuffix);
            }
            return suffix;
        }
        //****************************************************************************************************************************
        private static string SelectDateForAllRecords(ArrayList dateListFound, string schoolRecordBornDate)
        {
            FoundDate bestDate = new FoundDate("");
            FoundDate bestYear = new FoundDate("");
            foreach (FoundDate foundDate in dateListFound)
            {
                if (foundDate.date.Contains("/"))
                {
                    SetBestDate(ref bestDate, foundDate);
                }
                else if (foundDate.date != bestYear.date)
                {
                    SetBestDate(ref bestYear, foundDate);
                }
            }
            if (String.IsNullOrEmpty(bestDate.date) && String.IsNullOrEmpty(bestYear.date))
            {
                return schoolRecordBornDate;
            }
            if (!string.IsNullOrEmpty(bestDate.date))
            {
                return bestDate.date;
            }
            else
            {
                return bestYear.date;
            }
        }
        //****************************************************************************************************************************
        private static void SetBestDate(ref FoundDate bestDate,
                                 FoundDate foundDate)
        {
            if (string.IsNullOrEmpty(bestDate.date))
            {
                bestDate = foundDate;
            }
            else if (foundDate.num > bestDate.num)
            {
                bestDate = foundDate;
            }
        }
        //****************************************************************************************************************************
        private static string GetVitalRecordBirthDate(ArrayList personInfoList,
                                              DataTable birthRecordsTbl,
                                              int personId,
                                          out PersonName personName)
        {
            string selectStatement = U.PersonID_col + " = " + personId.ToString();
            DataRow[] result = birthRecordsTbl.Select(selectStatement);
            if (result.Length != 0)
            {
                personName = new PersonName(result[0][U.FirstName_col].ToString(), 
                                                       result[0][U.MiddleName_col].ToString(), 
                                                       result[0][U.LastName_col].ToString(), 
                                                       result[0][U.Suffix_col].ToString(), 
                                                       result[0][U.Prefix_col].ToString());
                return U.BuildDate(result[0][U.DateYear_col].ToInt(), result[0][U.DateMonth_col].ToInt(), result[0][U.DateDay_col].ToInt());
            }
            personName = null;
            return "";
        }
        //****************************************************************************************************************************
        private static bool SetAllSchoolRecordsToNameOrBirthDate(ArrayList personInfoList,
                                                           DataTable schoolRecordsTbl,
                                                           string setNameOrBirthDate,
                                                           string colName,
                                                           int personId)
        {
            bool foundDifferentNameOrDate = false;
            foreach (SchoolPersonInfo schoolPersonInfo in personInfoList)
            {
                string selectStatement = U.SchoolRecordID_col + " = " + schoolPersonInfo.schoolRecordID.ToString();
                DataRow[] result = schoolRecordsTbl.Select(selectStatement);
                if (result.Length != 0)
                {
                    string recordNameOrBirthDate = result[0][colName].ToString();
                    if (recordNameOrBirthDate != setNameOrBirthDate)
                    {
                        result[0][colName] = setNameOrBirthDate;
                        if (!foundDifferentNameOrDate)
                        {
                            foundDifferentNameOrDate = true;
                        }
                    }
                }
            }
            return foundDifferentNameOrDate;
        }
        //****************************************************************************************************************************
        private static void SetAllSchoolRecordsToName(ArrayList personInfoList,
                                                      DataTable schoolRecordsTbl,
                                                      string setName,
                                                      int personId)
        {
            bool foundDifferentName = false;
            foreach (SchoolPersonInfo schoolPersonInfo in personInfoList)
            {
                string selectStatement = U.SchoolRecordID_col + " = " + schoolPersonInfo.schoolRecordID.ToString();
                DataRow[] result = schoolRecordsTbl.Select(selectStatement);
                if (result.Length != 0)
                {
                    string recordName = result[0][U.Person_col].ToString();
                    if (recordName != setName)
                    {
                        result[0][U.Person_col] = setName;
                        if (!foundDifferentName)
                        {
                            foundDifferentName = true;
                        }
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private static void SetPersonBornDate(DataRow personRow, int personId, string date, bool vitalRecordFound)
        {
            if (personRow[U.BornSource_col].ToString().ToLower().Contains("school"))
            {
                if (string.IsNullOrEmpty(date) || vitalRecordFound)
                {
                    NewValueIfChanged(personRow, U.BornDate_col, "");
                    NewValueIfChanged(personRow, U.BornSource_col, "");
                }
                else if (personRow[U.BornDate_col].ToString() != date)
                {
                    NewValueIfChanged(personRow, U.BornDate_col, date);
                    NewValueIfChanged(personRow, U.BornSource_col, "School Records");
                }
            }
            else if (string.IsNullOrEmpty(personRow[U.BornSource_col].ToString()))
            {
                if (string.IsNullOrEmpty(date) || vitalRecordFound)
                {
                    NewValueIfChanged(personRow, U.BornDate_col, "");
                    NewValueIfChanged(personRow, U.BornSource_col, "");
                }
                else
                {
                    NewValueIfChanged(personRow, U.BornDate_col, date);
                    NewValueIfChanged(personRow, U.BornSource_col, "School Records");
                }
            }
        }
        //****************************************************************************************************************************
        public static void NewValueIfChanged(DataRow personRow, string colName, string newValue)
        {
            if (personRow[colName].ToString() != newValue)
            {
                personRow[colName] = newValue;
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllSchools()
        {
            DataTable tbl = new DataTable(U.School_Table);
            SelectAll(U.School_Table, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllSchoolRecords()
        {
            return GetAllSchoolRecords(OrderBy(U.Person_col, U.Year_col));
        }
        //****************************************************************************************************************************
        public static DataTable GetAllSchoolRecords(string orderBy)
        {
            DataTable tbl = new DataTable(U.SchoolRecord_Table);
            SelectAll(U.SchoolRecord_Table, orderBy, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetSchoolTeacherRecord(int schoolId,
                                                       int year,
                                                       int grade)
        {
            DataTable tbl = new DataTable(U.SchoolRecord_Table);
            if (grade == 0)
            {
                SelectAll(U.SchoolRecord_Table, OrderBy(U.Grade_col, U.Person_col), tbl,
                                                        new NameValuePair(U.SchoolID_col, schoolId),
                                                        new NameValuePair(U.Year_col, year),
                                                        new NameValuePair(U.SchoolRecordType_col, 0));
            }
            else
            {
                SelectAllLike(tbl, U.SchoolRecord_Table, OrderBy(U.Grade_col, U.Person_col),
                                                         new NameValuePair(U.SchoolID_col, schoolId),
                                                         new NameValuePair(U.Year_col, year),
                                                         new NameValuePair(U.SchoolRecordType_col, 0),
                                                         new NameValuePair(U.Grade_col, grade.ToString()));
            }
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPersonCWRecords()
        {
            DataTable tbl = new DataTable(U.PersonCW_Table);
            SelectAll(U.PersonCW_Table, OrderBy(U.LastName_col, U.FirstName_col, U.MiddleName_col), tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetSchoolRecords(int schoolId,
                                                 int year,
                                                 int grade)
        {
            DataTable tbl = new DataTable(U.SchoolRecord_Table);
            if (grade == 0)
            {
                SelectAll(U.SchoolRecord_Table, OrderBy(U.Grade_col, U.Person_col), tbl, 
                                                        new NameValuePair(U.SchoolID_col, schoolId),
                                                        new NameValuePair(U.SchoolRecordType_col, 1),
                                                        new NameValuePair(U.Year_col, year));
            }
            else
            {
                SelectAll(U.SchoolRecord_Table, OrderBy(U.Grade_col, U.Person_col), tbl,
                                                new NameValuePair(U.SchoolID_col, schoolId),
                                                new NameValuePair(U.SchoolRecordType_col, 1),
                                                new NameValuePair(U.Year_col, year),
                                                new NameValuePair(U.Grade_col, grade.ToString()));
            }
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetPersonCWRecord(int PersonCWID)
        {
            DataTable tbl = new DataTable(U.PersonCWID_col);
            SelectAll(U.PersonCW_Table, tbl, new NameValuePair(U.PersonCWID_col, PersonCWID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetSchoolRecord(int schoolRecordId)
        {
            DataTable tbl = new DataTable(U.SchoolRecord_Table);
            SelectAll(U.SchoolRecord_Table, tbl, new NameValuePair(U.SchoolRecordID_col, schoolRecordId));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetSchoolRecordsFromPersonId(int personId)
        {
            DataTable tbl = new DataTable(U.SchoolRecord_Table);
            SelectAll(U.SchoolRecord_Table, tbl, new NameValuePair(U.PersonID_col, personId));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataRow GetSchoolRecord(int schoolId,
                                              int year)
        {
            DataTable tbl = new DataTable(U.SchoolRecord_Table);
            SelectAll(U.SchoolRecord_Table, tbl, new NameValuePair(U.SchoolID_col, schoolId), new NameValuePair(U.Year_col, year));
            if (tbl.Rows.Count == 0)     // does not exist
            {
                return null;
            }
            else
            {
                return tbl.Rows[0];
            }
        }
        //****************************************************************************************************************************
        public static bool UpdateSchoolrecordsFromPersonRecord(int schoolRecordId, out DataRow personRow)
        {
            personRow = null;
            try
            {
                DataTable schoolRecordTbl = GetSchoolRecord(schoolRecordId);
                if (schoolRecordTbl.Rows.Count <= 0)
                {
                    return false;
                }
                int personId = schoolRecordTbl.Rows[0][U.PersonID_col].ToInt();
                string schoolRecordName = schoolRecordTbl.Rows[0][U.Person_col].ToString();
                if (personId == 0)
                {
                    return false;
                }
                personRow = GetPerson(personId);
                if (personRow == null)
                {
                    return false;
                }
                string personName = BuildNameLastNameFirst(personRow);
                if (schoolRecordName == personName)
                {
                    return false;
                }
                string personNameWithoutMiddleName = BuildNameLastNameFirstNoMiddleName(personRow);
                if (schoolRecordName == personNameWithoutMiddleName)
                {
                    return false;
                }
                schoolRecordTbl.Clear();
                schoolRecordTbl = GetSchoolRecordsFromPersonId(personId);
                foreach (DataRow schoolRecordRow in schoolRecordTbl.Rows)
                {
                    schoolRecordRow[U.Person_col] = personName;
                }
                UpdateWithDA(schoolRecordTbl, U.SchoolRecord_Table, U.SchoolRecordID_col, ColumnList(U.Person_col));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //****************************************************************************************************************************
        public static int InsertSchool(string school)
        {
            DataTable school_tbl = DefineSchoolTable();
            DataRow school_row = school_tbl.NewRow();
            school_row[U.SchoolID_col] = 0;
            school_row[U.School_col] = school;
            school_tbl.Rows.Add(school_row);
            SqlCommand insertCommand = InsertCommand(school_tbl, U.School_Table, true);
            if (InsertWithDA(school_tbl, insertCommand))
            {
                return school_tbl.Rows[0][U.SchoolID_col].ToInt();
            }
            else
            {
                return 0;
            }
        }
        //****************************************************************************************************************************
        public static void UpdateInsertSchoolRecords(DataTable schoolRecordTbl,
                                                     DataTable personTbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                SqlCommand updateCommand = UpdateCommand(txn, schoolRecordTbl.Columns, U.SchoolRecord_Table, U.SchoolRecordID_col, ColumnList(U.BornDate_col, U.Grade_col));
                SqlCommand insertCommand = InsertCommand(txn,schoolRecordTbl, U.SchoolRecord_Table, true);
                UpdateInsertWithDA(schoolRecordTbl, updateCommand, insertCommand);
                SqlCommand personUpdateCommand = UpdateCommand(txn, personTbl.Columns, U.Person_Table, U.PersonID_col, ColumnList(U.BornDate_col));
                UpdateWithDA(txn, personTbl, U.Person_Table, U.PersonID_col, ColumnList(U.BornDate_col));
                txn.Commit();
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw new Exception(ex.Message);
            }
        }
        //****************************************************************************************************************************
        public static int InsertSchoolRecord(DataTable schoolRecord_tbl)
        {
            SqlCommand insertCommand = InsertCommand(schoolRecord_tbl, U.SchoolRecord_Table, true);
            if (InsertWithDA(schoolRecord_tbl, insertCommand))
            {
                return schoolRecord_tbl.Rows[0][U.SchoolID_col].ToInt();
            }
            else
            {
                return 0;
            }
        }
        //****************************************************************************************************************************
        public static void SetPersonNameToIntegratedName()
        {
            try
            {
                DataTable schoolRecord_tbl = GetAllSchoolRecords();
                DataTable person_tbl = GetAllPersons();
                foreach (DataRow schoolRecord_row in schoolRecord_tbl.Rows)
                {
                    if (schoolRecord_row[U.PersonID_col].ToInt() != 0)
                    {
                        string selectStatement = U.PersonID_col + " = " + schoolRecord_row[U.PersonID_col].ToString();
                        DataRow[] result = person_tbl.Select(selectStatement);
                        if (result.Length == 0)
                        {
                            throw new Exception("Unable to Find Person: " + schoolRecord_row[U.PersonID_col].ToString());
                        }
                        PersonName personName = new PersonName(result[0]);
                        string name = personName.LastNameFirst();
                        if (schoolRecord_row[U.Person_col].ToString() != name)
                        {
                            schoolRecord_row[U.Person_col] = name;
                        }
                    }
                }
                DataView dv = schoolRecord_tbl.DefaultView;
                dv.Sort = U.Person_col + ", " + U.Year_col;
                LookForProblems(dv.ToTable());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //****************************************************************************************************************************
        public static void LookForProblems(DataTable schoolRecord_tbl)
        {
            DataRow previousRow = schoolRecord_tbl.Rows[0];
            string previousPerson = previousRow[U.Person_col].ToString();
            int previousYear = previousRow[U.Year_col].ToInt();
            string previoueBornDate = previousRow[U.BornDate_col].ToString();
            for (int i = 1; i < schoolRecord_tbl.Rows.Count; i++)
            {
                DataRow CurrentRow = schoolRecord_tbl.Rows[i];
                string currentPerson = CurrentRow[U.Person_col].ToString();
                string currentBornDate = previousRow[U.BornDate_col].ToString();
                int currentYear = CurrentRow[U.Year_col].ToInt();
                if (currentPerson == previousPerson)
                {
                    if (currentBornDate != previoueBornDate)
                    {
                    }
                    if ((currentYear - previousYear) > 1)
                    {
                    }
                }
                else
                {
                    previoueBornDate = CurrentRow[U.BornDate_col].ToString();
                }
                previousRow = CurrentRow;
                previousPerson = currentPerson;
                previousYear = currentYear;
            }
        }
        //****************************************************************************************************************************
        public static bool SaveIntegratedPersonCWRecords(DataTable personCW_tbl)
        {
            return UpdateWithDA(personCW_tbl, U.PersonCW_Table, U.PersonCWID_col, ColumnList(U.PersonID_col));
        }
        //****************************************************************************************************************************
        public static bool SaveIntegratedSchoolRecords(DataTable schoolRecord_tbl)
        {
            return UpdateWithDA(schoolRecord_tbl, U.SchoolRecord_Table, U.SchoolRecordID_col, ColumnList(U.PersonID_col, U.BornDate_col, U.Person_col));
        }
        //****************************************************************************************************************************
        public static bool PersonCWRecordAlreadyExists(int iPersonId, int schoolId, int iSchoolYear)
        {
            if (iPersonId == 0)
                return false;
            DataTable tbl = new DataTable();
            SelectAll(U.SchoolRecord_Table, tbl, new NameValuePair(U.PersonID_col, iPersonId), new NameValuePair(U.SchoolID_col, schoolId), new NameValuePair(U.Year_col, iSchoolYear));
            return (tbl.Rows.Count != 0);
        }
        //****************************************************************************************************************************
        public static bool SchoolRecordAlreadyExists(int iPersonId, int schoolId, int iSchoolYear)
        {
            if (iPersonId == 0)
                return false;
            DataTable tbl = new DataTable();
            SelectAll(U.SchoolRecord_Table, tbl, new NameValuePair(U.PersonID_col, iPersonId), new NameValuePair(U.SchoolID_col, schoolId), new NameValuePair(U.Year_col, iSchoolYear));
            return (tbl.Rows.Count != 0);
        }
        //****************************************************************************************************************************
        public static bool IntegrateSchoolRecord(DataTable AlternativeSpellingsFirstNameTbl,
                                                  DataTable AlternativeSpellingsLastNameTbl,
                                                  DataRow SchoolRecord_row,
                                         DataTable Person_tbl,
                                         bool getFromGrid)
        {
            SqlTransaction txn = null;
            try
            {
                txn = (SchoolRecord_row[U.SchoolRecordType_col].ToInt() == 0) ? sqlConnection.BeginTransaction() : 
                                                                                SetBornDate(AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, SchoolRecord_row, Person_tbl.Rows[0]);
                bool bSuccess = false;

                int iPersonID = SavePersonRecordsWithFatherMotherIDs(txn, Person_tbl);
                if (iPersonID != 0)
                {
                    bSuccess = true;
                    SchoolRecord_row[U.PersonID_col] = iPersonID;
                }
                if (bSuccess && (!getFromGrid || UserDoesNotWantToAbort()))
                {
                    txn.Commit();
                    return true;
                }
                else
                {
                    SchoolRecord_row[U.PersonID_col] = 0;
                    txn.Rollback();
                    return false;
                }
            }
            catch (HistoricJamaicaException Exception)
            {
                if (txn != null)
                {
                    txn.Rollback();
                }
                throw Exception;
            }
            catch (Exception ex)
            {
                if (txn != null)
                {
                    txn.Rollback();
                }
                throw ex;
            }
        }
        //****************************************************************************************************************************
        private static SqlTransaction SetBornDate(DataTable AlternativeSpellingsFirstNameTbl,
                                                  DataTable AlternativeSpellingsLastNameTbl,
                                                  DataRow SchoolRecord_row, DataRow personRow)
        {
            int personID = personRow[U.PersonID_col].ToInt();
            string vitalRecordsBornDate = "";
            DataRow row = GetBirthRecordForPerson(personID);
            if (row != null)
            {
                vitalRecordsBornDate = U.BuildDate(row[U.DateYear_col].ToInt(), row[U.DateMonth_col].ToInt(), row[U.DateDay_col].ToInt());
                SetPersonBornDate(personRow, personID, vitalRecordsBornDate, true);
            }
            if (personID != 0)
            {
                GetBornDateForPersonFromSchoolRecords(AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, personRow, SchoolRecord_row, personID, vitalRecordsBornDate);
            }
            SqlTransaction txn = sqlConnection.BeginTransaction();  // BeginTransaction must occur after GetBornDateForPersonFromSchoolRecords
            UpdatePersonBornDateWithSchoolBornDate(personRow, SchoolRecord_row[U.BornDate_col].ToString(), vitalRecordsBornDate);
            return txn;
        }
        //****************************************************************************************************************************
        private static void UpdatePersonBornDateWithSchoolBornDate(DataRow personRow, string schoolBornDate, string vitalRecordsBornDate)
        {
            string personBornDate = personRow[U.BornDate_col].ToString();
            if (string.IsNullOrEmpty(schoolBornDate))
            {
                return;
            }
            if (!String.IsNullOrEmpty(vitalRecordsBornDate))
            {
                NewValueIfChanged(personRow, U.BornDate_col, "");
                NewValueIfChanged(personRow, U.BornSource_col, "");
            }
            else if (string.IsNullOrEmpty(personBornDate) || personRow[U.BornSource_col].ToString().ToLower().Contains("school"))
            {
                NewValueIfChanged(personRow, U.BornDate_col, schoolBornDate);
                NewValueIfChanged(personRow, U.BornSource_col, "School Records");
            }
        }
        //****************************************************************************************************************************
        private static DataTable GetBornDateForPersonFromSchoolRecords(DataTable AlternativeSpellingsFirstNameTbl,
                                                                       DataTable AlternativeSpellingsLastNameTbl,
                                                                       DataRow personRow,
                                                                       DataRow SchoolRecord_row,
                                                                       int personID,
                                                                       string vitalRecordsBornDate)
        {
            ArrayList personInfoList = new ArrayList();
            DataTable schoolRecordsForPersonIdTbl = GetSchoolRecordsFromPersonId(personID);
            if (schoolRecordsForPersonIdTbl.Rows.Count == 0)
            {
                ThisIsFirstSchoolRecordForPerson(AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, personRow, SchoolRecord_row, vitalRecordsBornDate);
                return schoolRecordsForPersonIdTbl;
            }
            foreach (DataRow schoolRecordsRow in schoolRecordsForPersonIdTbl.Rows)
            {
                personInfoList.Add(new SchoolPersonInfo(schoolRecordsRow[U.SchoolRecordID_col].ToInt(), schoolRecordsRow[U.SchoolRecordType_col].ToInt(), 
                                   schoolRecordsRow[U.BornDate_col].ToString(), schoolRecordsRow[U.Person_col].ToString()));
            }
            bool foundDifference = false;
            SchoolRecord_row[U.BornDate_col] = SetSchoolRecordsNameBornDates(personRow, schoolRecordsForPersonIdTbl, AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl,
                                                                             personInfoList, personID, ref foundDifference, vitalRecordsBornDate, SchoolRecord_row[U.BornDate_col].ToString());
            if (foundDifference)
            {
                UpdateWithDA(schoolRecordsForPersonIdTbl, U.SchoolRecord_Table, U.SchoolRecordID_col, ColumnList(U.BornDate_col, U.Person_col));
            }
            return schoolRecordsForPersonIdTbl;
        }
        //****************************************************************************************************************************
        private static void ThisIsFirstSchoolRecordForPerson(DataTable AlternativeSpellingsFirstNameTbl,  
                                                             DataTable AlternativeSpellingsLastNameTbl,
                                                             DataRow personRow,
                                                             DataRow SchoolRecord_row,
                                                             string vitalRecordsBornDate)
        {
            if (!String.IsNullOrEmpty(vitalRecordsBornDate))
            {
                SchoolRecord_row[U.BornDate_col] = vitalRecordsBornDate;
            }
            else if (!String.IsNullOrEmpty(personRow[U.BornDate_col].ToString()))
            {
                SchoolRecord_row[U.BornDate_col] = personRow[U.BornDate_col];
            }
            PersonName schoolName = new PersonName(personRow);
            ArrayList nameList = new ArrayList();
            AddFoundName(nameList, SchoolRecord_row[U.Person_col].ToString(), SchoolRecord_row[U.SchoolRecordType_col].ToInt());

            bool schoolNameChanged = SelectNameForAllRecords(nameList, AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, schoolName);
            if (schoolNameChanged)
            {
                SchoolRecord_row[U.Person_col] = BuildNameLastNameFirst(schoolName);
            }
        }
        //****************************************************************************************************************************
        private static void SetSchoolBornDate(SqlTransaction txn,
                                 DataTable schoolRecordsForPersonIdTbl,
                                 DataRow SchoolRecord_row,
                                 string personSchoolBornDate)
        {
            string thisRecordBornDate = SchoolRecord_row[U.BornDate_col].ToString();
            if (thisRecordBornDate != personSchoolBornDate)
            {
                if (thisRecordBornDate.Contains("/") && !personSchoolBornDate.Contains("/"))
                {
                    ChangeAllSchoolRecordsToUpdatedDate(txn, schoolRecordsForPersonIdTbl, thisRecordBornDate);
                }
                else
                {
                    SchoolRecord_row[U.BornDate_col] = personSchoolBornDate;
                }
            }
        }
        //****************************************************************************************************************************
        private static void ChangeAllSchoolRecordsToUpdatedDate(SqlTransaction txn, DataTable schoolRecordsTbl, string newBornDate)
        {
            foreach (DataRow schoolRecordsRow in schoolRecordsTbl.Rows)
            {
                schoolRecordsRow[U.BornDate_col] = newBornDate;
            }
            UpdateWithDA(txn, schoolRecordsTbl, U.SchoolRecord_Table, U.SchoolRecordID_col, ColumnList(U.BornDate_col));
        }
        //****************************************************************************************************************************
        public static bool IntegratePersonCWRecord(DataRow PersonCWRecord_row,
                                         DataTable Person_tbl,
                                         bool getFromGrid)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                bool bSuccess = false;
                DataRow personRow = Person_tbl.Rows[0];
                CheckBornDiedDates(personRow, PersonCWRecord_row);
                int iPersonID = SavePersonRecordsWithFatherMotherIDs(txn, Person_tbl);
                if (iPersonID != 0)
                {
                    bSuccess = true;
                    PersonCWRecord_row[U.PersonID_col] = iPersonID;
                }
                if (bSuccess && (!getFromGrid || UserDoesNotWantToAbort()))
                {
                    txn.Commit();
                    return true;
                }
                else
                {
                    PersonCWRecord_row[U.PersonID_col] = 0;
                    txn.Rollback();
                    return false;
                }
            }
            catch (HistoricJamaicaException Exception)
            {
                txn.Rollback();
                throw Exception;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw ex;
            }
        }
        //****************************************************************************************************************************
        public static void CheckBornDiedDates(DataRow personRow, DataRow PersonCWRecord_row)
        {
            if (DoSetBornDateFromCivilWarRecord(personRow[U.BornDate_col].ToString(), PersonCWRecord_row[U.BornDate_col].ToString()))
            {
                personRow[U.BornDate_col] = (PersonCWRecord_row[U.BornDate_col]);
                personRow[U.BornSource_col] = "Civil War Record";
            }
            if (DoSetBornDateFromCivilWarRecord(personRow[U.DiedDate_col].ToString(), PersonCWRecord_row[U.DiedDate_col].ToString()))
            {
                personRow[U.DiedDate_col] = (PersonCWRecord_row[U.DiedDate_col]);
                personRow[U.DiedSource_col] = "Civil War Record";
            }
        }
        //****************************************************************************************************************************
        public static bool DoSetBornDateFromCivilWarRecord(string personDate, string personCWDate)
        {
            if (String.IsNullOrEmpty(personCWDate))
            {
                return false;
            }
            if (string.IsNullOrEmpty(personDate))
            {
                return true;
            }
            if (personCWDate.Length == 10 && personDate.Length != 10)
            {
                return true;
            }
            return false;
        }
    }
}
