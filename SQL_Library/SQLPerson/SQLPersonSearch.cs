using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {
        //****************************************************************************************************************************
        public static DataRow GetPerson(int iPersonID)
        {
            DataTable Person_tbl = DefinePersonTable();
            GetPerson(Person_tbl, iPersonID);
            if (Person_tbl.Rows.Count == 0)
                return null;
            else
                return Person_tbl.Rows[0];
        }
        //****************************************************************************************************************************
        public static DataTable GetPersonTbl(int iPersonID)
        {
            DataTable Person_tbl = DefinePersonTable();
            GetPerson(Person_tbl, iPersonID);
            return Person_tbl;
        }
        //****************************************************************************************************************************
        private static string GetPersonBornDate(int personId)
        {
            DataRow personRow = SQL.GetPerson(personId);
            return (personRow == null) ? "" : personRow[U.BornDate_col].ToString();
        }
        //****************************************************************************************************************************
        public static DataTable GetPersonByFirstLastNames(string firstname,
                                                        string lastname)
        {
            DataTable Person_tbl = DefinePersonTable();
            SelectAll(U.Person_Table, Person_tbl, new NameValuePair(U.LastName_col, lastname), new NameValuePair(U.FirstName_col, firstname));
            return Person_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPersons()
        {
            DataTable Person_tbl = new DataTable();
            string sOrderBy = OrderBy(U.LastName_col,U.FirstName_col,U.MiddleName_col);
            SelectAll(U.Person_Table, sOrderBy, Person_tbl);
            return Person_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPersonsOrderBy(string orderBy_col)
        {
            DataTable Person_tbl = new DataTable();
            string sOrderBy = OrderBy(orderBy_col);
            SelectAll(U.Person_Table, sOrderBy, Person_tbl);
            return Person_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPersonsFirst(string last_col)
        {
            DataTable Person_tbl = new DataTable();
            string sOrderBy = OrderBy(U.FirstName_col, last_col);
            SelectAll(U.Person_Table, sOrderBy, Person_tbl);
            return Person_tbl;
        }
        //****************************************************************************************************************************
        public static int GetPersonIDFromImportPersonIDFromDatabase(int iImportPersonID)
        {
            DataTable tbl = new DataTable(U.Person_Table);
            SelectColumns(tbl, U.Person_Table, U.PersonID_col, new NameValuePair(U.ImportPersonID_col, iImportPersonID));
            if (tbl.Rows.Count == 0)
                return 0;
            else
                return tbl.Rows[0][U.PersonID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static bool GetRecordsToMerge(DataSet person_ds,
                                             DataSet mergePerson_ds,
                                      int iPersonID,
                                      int iMergePersonID)
        {
            GetPerson(ref person_ds, iPersonID);
            GetPerson(ref mergePerson_ds, iMergePersonID);
            return (person_ds.Tables[U.Person_Table].Rows.Count != 0 &&
                    mergePerson_ds.Tables[U.Person_Table].Rows.Count != 0);
        }
        //****************************************************************************************************************************
        public static bool SetSpouseMarriedName(int iPersonID,
                                                int iSpouseID)
        {
            DataTable Person_tbl = GetPersonTbl(iPersonID);
            if (Person_tbl.Rows.Count == 0)
            {
                return false;
            }
            DataTable Spouse_tbl = GetPersonTbl(iSpouseID);
            if (Spouse_tbl.Rows.Count == 0)
            {
                return false;
            }
            DataRow Person_row = Person_tbl.Rows[0];
            DataRow Spouse_row = Spouse_tbl.Rows[0];
            string sMarriedName = "";
            bool bPersonIsTheFemale = false;
            if (Person_row[U.Sex_col].ToString() == Spouse_row[U.Sex_col].ToString())
                return true;
            if (Person_row[U.Sex_col].ToString() == "M")
            {
                sMarriedName = Person_row[U.LastName_col].ToString();
                bPersonIsTheFemale = false;
            }
            else if (Person_row[U.Sex_col].ToString() == "F")
            {
                sMarriedName = Spouse_row[U.LastName_col].ToString();
                bPersonIsTheFemale = true;
            }
            else if (Spouse_row[U.Sex_col].ToString() == "M")
            {
                sMarriedName = Spouse_row[U.LastName_col].ToString();
                bPersonIsTheFemale = true;
            }
            else if (Spouse_row[U.Sex_col].ToString() == "F")
            {
                sMarriedName = Person_row[U.LastName_col].ToString();
                bPersonIsTheFemale = false;
            }
            else
            {
                return true;  // Don;t know which is the male and female
            }
            if (bPersonIsTheFemale)
                UpdatePersonMarriedName(Person_tbl, sMarriedName);
            else
                UpdatePersonMarriedName(Spouse_tbl, sMarriedName);
            return true;
        }
        //****************************************************************************************************************************
        public static void UpdatePersonMarriedName(DataTable tbl, string sMarriedName)
        {
            if (String.IsNullOrEmpty(sMarriedName))
            {
                return;
            }
            ArrayList FieldsModified = new ArrayList();
            FieldsModified.Add(U.MarriedName_col);
            tbl.Rows[0][U.MarriedName_col] = sMarriedName;
            string lastName = tbl.Rows[0][U.LastName_col].ToString();
            if (sMarriedName == lastName)
            {
                tbl.Rows[0][U.LastName_col] = "";
                FieldsModified.Add(U.LastName_col);
            }
            UpdateWithDA(tbl, U.Person_Table, U.PersonID_col, FieldsModified);
        }
        //****************************************************************************************************************************
        public static void UpdatePersonFatherAndMother(int iChildID,
                                               int iPersonID,
                                               int iSpouseID)
        {
            DataRow Person_row = GetPerson(iPersonID);
            DataRow Spouse_row = GetPerson(iSpouseID);
            string PersonSex = "";
            string SpouseSex = "";
            string PersonFirstName = "";
            string SpouseFirstName = "";
            if (Person_row == null)
                iPersonID = 0;
            else
            {
                PersonSex = Person_row[U.Sex_col].ToString();
                PersonFirstName = Person_row[U.FirstName_col].ToString();
            }
            if (Spouse_row == null)
                iSpouseID = 0;
            else
            {
                SpouseSex = Spouse_row[U.Sex_col].ToString();
                SpouseFirstName = Spouse_row[U.FirstName_col].ToString();
            }
            if ((SpouseSex != "" && SpouseSex == "M") ||
                (iSpouseID == 0 && PersonSex == "F"))
            {
                int iTmpID = iPersonID;
                iPersonID = iSpouseID;
                iSpouseID = iTmpID;
            }
            UpdateWithParms(U.Person_Table, new NameValuePair(U.PersonID_col, iChildID.ToString()),
                                            new NameValuePair(U.FatherID_col, iPersonID),
                                            new NameValuePair(U.MotherID_col, iSpouseID));
//            string UpdateString = "Update " + U.Person_Table + " Set " +
//                                   U.FatherID_col + "=" + iPersonID + "," +
//                                   U.MotherID_col + "=" + iSpouseID +
//                                 " where PersonID = " + iChildID.ToString();
//            ExecuteSQLNonQueryCommand(UpdateString);
//            return true;
        }
        //****************************************************************************************************************************
        public static bool PersonsBasedOnNameOptions(DataTable tbl,
                                                     bool bDoCheckMarriedKnownAsNames,
                                                     string sTableName,
                                                     string sPrimaryIDCol,
                                                     eSearchOption SearchBy,
                                                     PersonName personName,
                                                     string marriedName)
        {
            bool foundNames = false;
            PersonSearchTableAndColumns personSearchTableAndColumns = new PersonSearchTableAndColumns(sTableName,
                                                              U.FirstName_col, U.MiddleName_col, U.LastName_col);
            if (SearchBy == eSearchOption.SO_Similar && personName.lastName.Length == 0 && marriedName.Length == 0 && personName.firstName.Length != 0)
            {
                foundNames = PersonsBasedOnNameOptions(tbl, false, personSearchTableAndColumns, sPrimaryIDCol, SearchBy, personName);
            }
            else
            if (SearchBy == eSearchOption.SO_AllNames || personName.lastName.Length != 0)
            {
                foundNames = PersonsBasedOnNameOptions(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumns, sPrimaryIDCol, SearchBy, personName);
            }
            if (marriedName.Length != 0)
            {
                PersonName personMarriedName = personName;
                personMarriedName.lastName = marriedName;
                if (PersonsBasedOnNameOptions(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumns, sPrimaryIDCol, SearchBy, personMarriedName))
                {
                    foundNames = true;
                }
            }
            return foundNames;
        }
        //****************************************************************************************************************************
        public static string GetPersonLastName(int iPersonID)
        {
            if (iPersonID == 0)
            {
                return "";
            }
            DataTable Person_tbl = DefinePersonTable();
            GetPerson(Person_tbl, iPersonID);
            if (Person_tbl.Rows.Count == 0)
                return "";
            else
                return Person_tbl.Rows[0][U.LastName_col].ToString();
        }
        //****************************************************************************************************************************
        public static bool PersonsBasedOnNameOptions(DataTable tbl,
                                                     bool bDoCheckMarriedKnownAsNames,
                                                     PersonSearchTableAndColumns personSearchTableAndColumns,
                                                     string sPrimaryIDCol,
                                                     eSearchOption SearchBy,
                                                     PersonName personName)
        {
            if (SearchBy == eSearchOption.SO_AllNames)
            {
                SelectAll(personSearchTableAndColumns.tableName, NameOrderByStatement(), tbl);
            }
            else if (SearchBy == eSearchOption.SO_Similar)
            {
                if (personName.firstName.Length == 0)
                {
                    SearchStartingWith(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumns, personName);
                }
                else
                {
                    PersonExists(tbl, bDoCheckMarriedKnownAsNames, sPrimaryIDCol, personSearchTableAndColumns, personName);
                }
            }
            else if (SearchByPartialNamesOrLastNameBlank(personName.lastName, SearchBy))
            {
                PartialNameSearch(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumns, personName);
            }
            else if (personName.lastName.Length == 0 || SearchBy == eSearchOption.SO_AllNames)
            {
                SelectAll(personSearchTableAndColumns.tableName, NameOrderByStatement(), tbl);
            }
            else if (SearchBy == eSearchOption.SO_ReturnToLast && personName.lastName.Length != 0)
            {
                SearchStartingWith(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumns, personName);
            }
            else if (SearchBy == eSearchOption.SO_StartingWith && personName.lastName.Length != 0)
            {
                SearchStartingWith(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumns, personName);
            }
            else
            {
                SelectAll(personSearchTableAndColumns.tableName, NameOrderByStatement(), tbl);
            }
            return true;
        }
        //****************************************************************************************************************************
        private static void SearchStartingWith(DataTable tbl,
                                              bool bDoCheckMarriedKnownAsNames,
                                              PersonSearchTableAndColumns personSearchTableAndColumns,
                                              PersonName personName)
        {
            SearchStartingWith(tbl, personSearchTableAndColumns.tableName, personSearchTableAndColumns.lastNameCol, personName.lastName);
            if (bDoCheckMarriedKnownAsNames)
            {
                SearchStartingWith(tbl, personSearchTableAndColumns.tableName, U.MarriedName_col, personName.lastName);
                SearchStartingWith(tbl, personSearchTableAndColumns.tableName, U.MarriedName2_col, personName.lastName);
                SearchStartingWith(tbl, personSearchTableAndColumns.tableName, U.MarriedName3_col, personName.lastName);
            }
        }
        //****************************************************************************************************************************
        private static void PartialNameSearch(DataTable tbl,
                                              bool bDoCheckMarriedKnownAsNames,
                                              PersonSearchTableAndColumns personSearchTableAndColumns,
                                              PersonName personName)
        {
            SearchPartial(tbl, personSearchTableAndColumns, personName);
            if (bDoCheckMarriedKnownAsNames)
            {
                PartialSearchWithAlternatives(tbl, personSearchTableAndColumns, personName, "", U.MarriedName_col);
                PartialSearchWithAlternatives(tbl, personSearchTableAndColumns, personName, U.KnownAs_col, "");
                PartialSearchWithAlternatives(tbl, personSearchTableAndColumns, personName, U.KnownAs_col, U.MarriedName_col);
            }
        }
        //****************************************************************************************************************************
        private static void PartialSearchWithAlternatives(DataTable tbl,
                                                          PersonSearchTableAndColumns personSearchTableAndColumns,
                                                          PersonName personName,
                                                          string sAlternativeFirstNameCol,
                                                          string sAlternativeLastNameCol)
        {
            PersonSearchTableAndColumns personSearchTableAndColumnsAlternative = new PersonSearchTableAndColumns(personSearchTableAndColumns);
            if (sAlternativeFirstNameCol.Length != 0)
            {
                personSearchTableAndColumnsAlternative.firstNameCol = sAlternativeFirstNameCol;
            }
            if (sAlternativeLastNameCol.Length != 0)
            {
                personSearchTableAndColumnsAlternative.lastNameCol = sAlternativeLastNameCol;
            }
            SearchPartial(tbl, personSearchTableAndColumnsAlternative, personName);
        }
        //****************************************************************************************************************************
        private static void SearchPartial(DataTable tbl,
                                          PersonSearchTableAndColumns personSearchTableAndColumns,
                                          PersonName personName)
        {
            PartialWithOrWithoutMiddleName(tbl, personSearchTableAndColumns, personName);
            DataTable LastNameAlternativeSpellings_tbl = GetAlternativeSpellings(U.AlternativeSpellingsLastName_Table, personName.lastName);
            DataTable FirstNameAlternativeSpellings_tbl = GetAlternativeSpellings(U.AlternativeSpellingsFirstName_Table, personName.firstName);
            AlternativeFirstNames(tbl, FirstNameAlternativeSpellings_tbl, personSearchTableAndColumns, personName.lastName);
            foreach (DataRow lastName_row in LastNameAlternativeSpellings_tbl.Rows)
            {
                PersonName personNameAlternateLast = new PersonName(personName);
                personNameAlternateLast.lastName = lastName_row[U.AlternativeSpelling_Col].ToString();
                PartialWithOrWithoutMiddleName(tbl, personSearchTableAndColumns, personNameAlternateLast);
                AlternativeFirstNames(tbl, FirstNameAlternativeSpellings_tbl, personSearchTableAndColumns,
                                      lastName_row[U.AlternativeSpelling_Col].ToString());
            }
        }
        //****************************************************************************************************************************
        private static void PartialWithOrWithoutMiddleName(DataTable tbl,
                                                           PersonSearchTableAndColumns personSearchTableAndColumns,
                                                           PersonName personName)
        {
            if (personName.middleName.Length == 0)
            {
                SelectAll(personSearchTableAndColumns.tableName, NameOrderByStatement(), tbl, 
                                                               new NameValuePair(personSearchTableAndColumns.lastNameCol, personName.lastName),
                                                               new NameValuePair(personSearchTableAndColumns.firstNameCol, personName.firstName),
                                                               new NameValuePair(U.Suffix_col, personName.suffix),
                                                               new NameValuePair(U.Prefix_col, personName.prefix));
            }
            else
            {
                SelectAll(personSearchTableAndColumns.tableName, NameOrderByStatement(), tbl, 
                                                               new NameValuePair(personSearchTableAndColumns.lastNameCol, personName.lastName),
                                                               new NameValuePair(personSearchTableAndColumns.firstNameCol, personName.firstName),
                                                               new NameValuePair(personSearchTableAndColumns.middleNameCol, personName.middleName),
                                                               new NameValuePair(U.Suffix_col, personName.suffix),
                                                               new NameValuePair(U.Prefix_col, personName.prefix));
            }
        }
        //****************************************************************************************************************************
        private static string NameOrderByStatement()
        {
            return " order by LastName,FirstName,MiddleName,Suffix,Prefix;";
        }
        //****************************************************************************************************************************
        private static bool SearchByPartialNamesOrLastNameBlank(string sLastName,
                                                                eSearchOption SearchBy)
        {
            return (sLastName.Length == 0 || SearchBy == eSearchOption.SO_PartialNames);
        }
        //****************************************************************************************************************************
        private static void SearchStartingWith(DataTable tbl,
                                               string sTableName,
                                               string sLastNameCol,
                                               string sLastName)
        {
            DataTable LastNameAlternativeSpellings_tbl = GetAlternativeSpellings(U.AlternativeSpellingsLastName_Table, sLastName);
            foreach (DataRow LastNameAlternativeSpellings_row in LastNameAlternativeSpellings_tbl.Rows)
            {
                string lastname = LastNameAlternativeSpellings_row["AlternativeSpelling"].ToString();
                SelectAll(sTableName, tbl, new NameValuePair(sLastNameCol, lastname));
            }
            SelectAllLike(tbl, sTableName, NameOrderByStatement(), new NameValuePair(sLastNameCol, sLastName));
        }
        //****************************************************************************************************************************
        public static DataTable GetSimilarPersons(DataRow personRow)
        {
            DataTable SimilarPerson_tbl = SQL.DefinePersonTable();
            PersonName otherSpouseName = new PersonName(personRow);
            if (otherSpouseName.lastName.Length != 0)
            {
                SQL.PersonExists(SimilarPerson_tbl, true, U.Person_Table, U.PersonID_col, otherSpouseName);
            }
            otherSpouseName.lastName = personRow[U.MarriedName_col].ToString();
            if (!String.IsNullOrEmpty(otherSpouseName.lastName))
            {
                SQL.PersonExists(SimilarPerson_tbl, true, U.Person_Table, U.PersonID_col, otherSpouseName);
            }
            return SimilarPerson_tbl;
        }
        //****************************************************************************************************************************
        public static bool PersonExists(DataTable tbl,
                                        bool bDoCheckMarriedKnownAsNames,
                                        string sTableName,
                                        string sPrimaryIDCol,
                                        PersonName personName)
        {
            PersonSearchTableAndColumns personSearchTableAndColumns = new PersonSearchTableAndColumns(sTableName,
                                                                          U.FirstName_col, U.MiddleName_col, U.LastName_col);
            return PersonExists(tbl, bDoCheckMarriedKnownAsNames, sPrimaryIDCol, personSearchTableAndColumns, personName);
        }
        //****************************************************************************************************************************
        public static bool PersonExists(DataTable tbl,
                                        bool bDoCheckMarriedKnownAsNames,
                                        string sPrimaryIDCol,
                                        PersonSearchTableAndColumns personSearchTableAndColumns,
                                        PersonName personName)
        {
            GetAllSimilarPersons(tbl, bDoCheckMarriedKnownAsNames, sPrimaryIDCol, personSearchTableAndColumns, personName);
            return tbl.Rows.Count != 0;
        }
        //****************************************************************************************************************************
        public static void GetAllSimilarPersons(DataTable tbl, 
                                                     bool      bDoCheckMarriedKnownAsNames,
                                                     string    sPrimaryIDCol,
                                                     PersonSearchTableAndColumns personSearchTableAndColumns,
                                                     PersonName personName)
        {
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[sPrimaryIDCol] };
            PersonSearchTableAndColumns personSearchTableAndColumnsKnownAs = new PersonSearchTableAndColumns(personSearchTableAndColumns);
            personSearchTableAndColumnsKnownAs.firstNameCol = U.KnownAs_col;
            FirstNameWithLastAndMaiden(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumns, personName);

            if (bDoCheckMarriedKnownAsNames)
            {
                FirstNameWithLastAndMaiden(tbl, bDoCheckMarriedKnownAsNames, personSearchTableAndColumnsKnownAs, personName);
            }
        }
        //****************************************************************************************************************************
        public static string GetPersonName(int personID)
        {
            DataTable tbl = new DataTable(U.Person_Table);
            if (GetPersonNameValues(tbl, personID))
            {
                DataRow row = tbl.Rows[0];
                return GetPersonNameFromRow(row);
            }
            else
            {
                return "";
            }
        }
        //****************************************************************************************************************************
        public static string GetPersonNameFromRow(DataRow row)
        {
            return BuildNameString(row[U.FirstName_col].ToString(), row[U.MiddleName_col].ToString(), row[U.LastName_col].ToString(),
                                   row[U.Suffix_col].ToString(),
                                   row[U.Prefix_col].ToString(), row[U.MarriedName_col].ToString(), row[U.KnownAs_col].ToString());
        }
        //****************************************************************************************************************************
        private static void FirstNameWithLastAndMaiden(DataTable tbl,
                                                       bool bDoCheckMarriedKnownAsNames,
                                                       PersonSearchTableAndColumns personSearchTableAndColumns,
                                                       PersonName personName)
        {
            PersonSearchTableAndColumns personSearchTableAndColumnsMarriedName = new
                       PersonSearchTableAndColumns(personSearchTableAndColumns);
            personSearchTableAndColumnsMarriedName.lastNameCol = U.MarriedName_col;
            CheckForPerson(tbl, personSearchTableAndColumns, personName);
            if (bDoCheckMarriedKnownAsNames)
            {
                CheckForPerson(tbl, personSearchTableAndColumnsMarriedName, personName);
                personSearchTableAndColumnsMarriedName.lastNameCol = U.MarriedName2_col;
                CheckForPerson(tbl, personSearchTableAndColumnsMarriedName, personName);
                personSearchTableAndColumnsMarriedName.lastNameCol = U.MarriedName3_col;
                CheckForPerson(tbl, personSearchTableAndColumnsMarriedName, personName);
            }
        }
        //****************************************************************************************************************************
        private static void VerifyFatherOrMotherID(ref int iFatherOrMotherID,
                                                int iPersonID,
                                                string sFatherOfMother)
        {
            if (iFatherOrMotherID != 0)
            {
                DataTable tbl = new DataTable(U.Person_Table);
                SelectAll(U.Person_Table, tbl, new NameValuePair(U.PersonID_col, iFatherOrMotherID));
                if (tbl.Rows.Count == 0)
                {
                    iFatherOrMotherID = 0;
                    UpdatePersonFatherOrMother(sFatherOfMother, iPersonID, iFatherOrMotherID);
                }
            }
        }
        //****************************************************************************************************************************
        public static void UpdatePersonFatherOrMother(string sFieldToBeUpdated,
                                               int iPersonID,
                                               int iFatherOrMotherID)
        {
            UpdateWithParms(U.Person_Table, new NameValuePair(U.PersonID_col, iPersonID), new NameValuePair(sFieldToBeUpdated, iFatherOrMotherID));
//            string UpdateString = "Update Person " + sFieldToBeUpdated + "=" + iFatherOrMotherID +
//                                 " where PersonID = " + iPersonID.ToString();
//            ExecuteSQLNonQueryCommand(UpdateString);
        }
        //****************************************************************************************************************************
        public static bool GetAllChildren(DataTable tblChildren,
                                      int iFatherID,
                                      int iMotherID)
        {
            if (iFatherID == 0 && iMotherID == 0)
                return true;
            if (iMotherID == 0)
                SelectAll(U.Person_Table, tblChildren, new NameValuePair(U.FatherID_col, iFatherID),
                                                       new NameValuePair(U.MotherID_col, 0));
            else if (iFatherID == 0)
                SelectAll(U.Person_Table, tblChildren, new NameValuePair(U.FatherID_col, 0),
                                                       new NameValuePair(U.MotherID_col, iMotherID));
            else
                SelectAll(U.Person_Table, tblChildren, new NameValuePair(U.FatherID_col, iFatherID),
                                                       new NameValuePair(U.MotherID_col, iMotherID));
            return true;
        }
        //****************************************************************************************************************************
        public static void GetFatherMother(int iPerson,
                                    out int iFather,
                                    out int iMother)
        {
            DataTable tbl = new DataTable();
            string [] selectColumns = { U.FatherID_col, U.MotherID_col };
            SelectColumns(tbl, selectColumns, U.Person_Table, new NameValuePair(U.PersonID_col, iPerson));
            if (tbl.Rows.Count == 0)
            {
                iFather = 0;
                iMother = 0;
            }
            else
            {
                DataRow row = tbl.Rows[0];
                iFather = GetPersonFromRow(row[0].ToString());
                iMother = GetPersonFromRow(row[1].ToString());
            }
            VerifyFatherOrMotherID(ref iFather, iPerson, U.FatherID_col);
            VerifyFatherOrMotherID(ref iMother, iPerson, U.MotherID_col);
        }
        //****************************************************************************************************************************
        public static void GetFatherMother(DataRow row,
                                    out int iFather,
                                    out int iMother)
        {
            int iPerson = row[U.PersonID_col].ToInt();
            iFather = GetPersonFromRow(row[U.FatherID_col].ToString());
            iMother = GetPersonFromRow(row[U.MotherID_col].ToString());
            VerifyFatherOrMotherID(ref iFather, iPerson, U.FatherID_col);
            VerifyFatherOrMotherID(ref iMother, iPerson, U.MotherID_col);
        }
        //****************************************************************************************************************************
        private static int GetPersonFromRow(string sPerson)
        {
            if (sPerson.Length == 0)
                return 0;
            else
                return sPerson.ToInt();
        }
        //****************************************************************************************************************************
        private static void CheckForPerson(DataTable tbl,
                                           PersonSearchTableAndColumns personSearchTableAndColumns,
                                           PersonName personName)
        {
            PersonSearchTableAndColumns personSearchTableAndColumnsMiddleFirst =
                           new PersonSearchTableAndColumns(personSearchTableAndColumns);
            personSearchTableAndColumnsMiddleFirst.firstNameCol = personSearchTableAndColumns.middleNameCol;
            personSearchTableAndColumnsMiddleFirst.middleNameCol = personSearchTableAndColumns.firstNameCol;
            GetSimilarNames(tbl, personSearchTableAndColumns, personName);
            GetSimilarNames(tbl, personSearchTableAndColumnsMiddleFirst, personName);
            CheckForPersonAlternateFirstName(tbl, personSearchTableAndColumns, personSearchTableAndColumnsMiddleFirst, personName);

            DataTable AlternativeSpellings_tbl = GetAlternativeSpellings(U.AlternativeSpellingsLastName_Table, personName.lastName);
            foreach (DataRow row in AlternativeSpellings_tbl.Rows)
            {
                PersonName PersonAlternateLastName = new PersonName(personName);
                PersonAlternateLastName.lastName = row[U.AlternativeSpelling_Col].ToString();
                GetSimilarNames(tbl, personSearchTableAndColumns, PersonAlternateLastName);
                GetSimilarNames(tbl, personSearchTableAndColumnsMiddleFirst, PersonAlternateLastName);
                CheckForPersonAlternateFirstName(tbl, personSearchTableAndColumns, personSearchTableAndColumnsMiddleFirst, PersonAlternateLastName);
            }
        }
        //****************************************************************************************************************************
        private static void CheckForPersonAlternateFirstName(DataTable tbl, 
                                                             PersonSearchTableAndColumns personSearchTableAndColumns,
                                                             PersonSearchTableAndColumns personSearchTableAndColumnsMiddleFirst,
                                                             PersonName personName)
        {
            DataTable AlternativeSpellings_tbl = GetAlternativeSpellings(U.AlternativeSpellingsFirstName_Table, personName.firstName);
            foreach (DataRow row in AlternativeSpellings_tbl.Rows)
            {
                PersonName PersonAlternateFirstName = new PersonName(personName);
                PersonAlternateFirstName.firstName = row[U.AlternativeSpelling_Col].ToString();
                GetSimilarNames(tbl, personSearchTableAndColumns, PersonAlternateFirstName);
                GetSimilarNames(tbl, personSearchTableAndColumnsMiddleFirst, PersonAlternateFirstName);
            }
        }
        //****************************************************************************************************************************
        private static void GetSimilarNames(DataTable tbl,
                                            PersonSearchTableAndColumns personSearchTableAndColumns,
                                            PersonName personName)
        {
            GetPersonFromName(tbl, personSearchTableAndColumns, personName);
            GetPersonMiddleInitialName(tbl, personSearchTableAndColumns, personName);
            GetPersonBlankMiddleName(tbl, personSearchTableAndColumns, personName);
            GetPersonLikeFirstNoMiddleName(tbl, personSearchTableAndColumns, personName);
            if (personName.suffix.Length != 0 || personName.prefix.Length != 0)
            {
                PersonName personNameNoPrefixSuffix = new PersonName(personName);
                personNameNoPrefixSuffix.prefix = "";
                personNameNoPrefixSuffix.suffix = "";
                GetSimilarNames(tbl, personSearchTableAndColumns, personNameNoPrefixSuffix);
            }
        }
        //****************************************************************************************************************************
        private static void GetPersonFromName(DataTable person_tbl,
                                              PersonSearchTableAndColumns personSearchTableAndColumns,
                                              PersonName personName)
        {
            ArrayList columns = new ArrayList();
            if (personName.lastName.Length > 0)
            {
                columns.Add(new NameValuePair(personSearchTableAndColumns.lastNameCol, personName.lastName));
            }
            if (personName.firstName.Length > 0)
            {
                columns.Add(new NameValuePair(personSearchTableAndColumns.firstNameCol, personName.firstName));
            }
            if (personName.middleName.Length > 0)
            {
                columns.Add(new NameValuePair(personSearchTableAndColumns.middleNameCol, personName.middleName));
            }
            if (personName.suffix.Length > 0)
            {
                columns.Add(new NameValuePair(U.Suffix_col, personName.suffix));
            }
            if (personName.prefix.Length > 0)
            {
                columns.Add(new NameValuePair(U.Prefix_col, personName.prefix));
            }
            SelectAll(personSearchTableAndColumns.tableName, person_tbl, columns.ToArray(typeof(NameValuePair)) as NameValuePair[]);
        }
        //****************************************************************************************************************************
        private static void GetPersonMiddleInitialName(DataTable person_tbl,
                                                       PersonSearchTableAndColumns personSearchTableAndColumns,
                                                       PersonName personName)
        {
            if (personName.middleName.Length > 1)
            {
                // check for middle initial
                PersonName personWithMiddleInitial = new PersonName(personName);
                personWithMiddleInitial.middleName = personName.middleName[0].ToString();
                GetPersonFromName(person_tbl, personSearchTableAndColumns, personName);
                string sFirstNameCmd = ColumnEquals(personSearchTableAndColumns.firstNameCol);
                string sMiddleNameCmd = ColumnLike(personSearchTableAndColumns.middleNameCol, personName.middleName);
                NameWithWildCard(person_tbl, personSearchTableAndColumns, personName,
                                 sFirstNameCmd, sMiddleNameCmd);
            }
        }
        //****************************************************************************************************************************
        private static void GetPersonBlankMiddleName(DataTable person_tbl,
                                                     PersonSearchTableAndColumns personSearchTableAndColumns,
                                                     PersonName personName)
        {
            if (personName.middleName.Length != 0) // check for no middle name
            {
                PersonName PersonWithBlankMiddleName = new PersonName(personName);
                PersonWithBlankMiddleName.middleName = "";
                GetPersonFromName(person_tbl, personSearchTableAndColumns, PersonWithBlankMiddleName);
            }
        }
        //****************************************************************************************************************************
        private static void GetPersonLikeFirstNoMiddleName(DataTable person_tbl,
                                                          PersonSearchTableAndColumns personSearchTableAndColumns,
                                                          PersonName personName)
        {
            string sFirstNameCmd = ColumnLike(personSearchTableAndColumns.firstNameCol, personName.firstName);
            string sMiddleNameCmd = "";
            NameWithWildCard(person_tbl, personSearchTableAndColumns, personName,
                             sFirstNameCmd, sMiddleNameCmd);
        }
        //****************************************************************************************************************************
        private static void NameWithWildCard(DataTable tbl,
                                               PersonSearchTableAndColumns personSearchTableAndColumns,
                                               PersonName personName,
                                               string sFirstNameCommand,
                                               string sMiddleNameCommand)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.Parameters.Add(new SqlParameter("@" + personSearchTableAndColumns.firstNameCol, personName.firstName));
            cmd.Parameters.Add(new SqlParameter("@" + personSearchTableAndColumns.middleNameCol, personName.middleName));
            cmd.Parameters.Add(new SqlParameter("@" + personSearchTableAndColumns.lastNameCol, personName.lastName));
            string SelectCommard = "";
            if (personName.lastName.Length > 0)
            {
                SelectCommard = SelectAllWhereString(personSearchTableAndColumns.tableName, personSearchTableAndColumns.lastNameCol);
                if (personName.firstName.Length > 0)
                {
                    SelectCommard += " and " + sFirstNameCommand;
                }
                if (DoMiddleName(personName, sMiddleNameCommand))
                {
                    SelectCommard += " and " + sMiddleNameCommand;
                }
            }
            else
            {
                SelectCommard = SelectAllFromString(personSearchTableAndColumns.tableName) + " where " + sFirstNameCommand;
            }
            SelectCommard += ";";
            cmd.CommandText = SelectCommard;
            ExecuteSelectStatement(tbl, cmd);
        }
        //****************************************************************************************************************************
        public static string GetPersonLastNameFirst(int personID)
        {
            DataTable tbl = new DataTable(U.Person_Table);
            if (GetPersonNameValues(tbl, personID))
            {
                DataRow row = tbl.Rows[0];
                return BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                                      row[U.MiddleName_col].ToString(),
                                                      row[U.LastName_col].ToString(),
                                                      row[U.Suffix_col].ToString(),
                                                      row[U.Prefix_col].ToString(),
                                                      row[U.MarriedName_col].ToString(),
                                                      row[U.KnownAs_col].ToString());
            }
            else
                return "";
        }
        //****************************************************************************************************************************
        public static bool GetPersonNameValues(DataTable tbl,
                                                 int personID)
        {
            string[] columnNames = {U.PersonID_col, U.FirstName_col, U.MiddleName_col, U.LastName_col, U.Suffix_col,
                                    U.Prefix_col, U.MarriedName_col, U.KnownAs_col };
            SelectColumns(tbl, columnNames, U.Person_Table, new NameValuePair(U.PersonID_col, personID));
            return tbl.Rows.Count != 0;
        }
        //****************************************************************************************************************************
        private static bool DoMiddleName(PersonName personName,
                                         string sMiddleNameCommand)
        {
            return sMiddleNameCommand.Length > 0 && personName.middleName.Length > 0;
        }
        //****************************************************************************************************************************
    }
}
