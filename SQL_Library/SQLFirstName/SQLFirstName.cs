using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {
        //****************************************************************************************************************************
        public static DataTable DefineFirstNameTable()
        {
            DataTable tbl = new DataTable(U.Cemetery_Table);
            tbl.Columns.Add(U.FirstName_col, typeof(string));
            tbl.Columns.Add(U.Sex_col, typeof(char));
            tbl.Columns[U.FirstName_col].MaxLength = U.iMaxNameLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static void CheckVitalRecordsFirstname()
        {
            DataTable vitalRecordTbl = GetAllVitalRecords();
            foreach (DataRow vitalRecordRow in vitalRecordTbl.Rows)
            {
                EVitalRecordType eVitalRecordType = (EVitalRecordType)vitalRecordRow[U.VitalRecordType_col].ToInt();
                switch (eVitalRecordType)
                {
                    case EVitalRecordType.eBirthMale: CheckVitalRecordSex(vitalRecordRow, 'M');  break;
                    case EVitalRecordType.eBirthFemale: CheckVitalRecordSex(vitalRecordRow, 'F'); break;
                    case EVitalRecordType.eDeathMale: CheckVitalRecordSex(vitalRecordRow, 'M'); break;
                    case EVitalRecordType.eDeathFemale: CheckVitalRecordSex(vitalRecordRow, 'F'); break;
                    case EVitalRecordType.eMarriageGroom: CheckVitalRecordSex(vitalRecordRow, 'M'); break;
                    case EVitalRecordType.eMarriageBride: CheckVitalRecordSex(vitalRecordRow, 'F'); break;
                    default: break;
                }
            }
            ArrayList fieldsModified = new ArrayList();
            fieldsModified.Add(U.Sex_col);
            UpdateWithDA(vitalRecordTbl, U.VitalRecord_Table, U.VitalRecordID_col, fieldsModified);
        }
        private static void CheckVitalRecordSex(DataRow vitalRecordRow, char expectedSex)
        {
            char sex = vitalRecordRow[U.Sex_col].ToChar();
            if (sex != expectedSex)
            {
                vitalRecordRow[U.Sex_col] = expectedSex;
            }
        }
        //****************************************************************************************************************************
        public static bool CreateFirstNameTable()
        {
            try
            {
                UpdateTheFirstNameTable(U.Person_Table);
                //UpdateTheFirstNameTable(U.VitalRecord_Table, true);
                //UpdateTheFirstNameTable(U.CemeteryRecord_Table, true, true);
            }
            catch (SqlException expsql)
            {
                string sErrorMessage = expsql.ToString();
                MessageBox.Show("Save Unsuccesful: " + sErrorMessage);
            }
            return true;
        }
        //****************************************************************************************************************************
        public static void CheckPersonMaidenMarriedName()
        {
            ArrayList nums = new ArrayList();
            ArrayList nums1 = new ArrayList();
            int count = 0;
            DataTable Person_tbl = new DataTable();
            SelectAll(U.Person_Table, Person_tbl);
            int highestCount = 0;
            int numHighestCount = 0;
            foreach (DataRow person_row in Person_tbl.Rows)
            {
                string personSex = person_row[U.Sex_col].ToString();
                if (personSex == "M")
                {
                    continue;
                }
                DataTable marriage_tbl = new DataTable();
                int personId = person_row[U.PersonID_col].ToInt();
                GetAllSpouses(marriage_tbl, personId);
                if (marriage_tbl.Rows.Count > 1)
                {
                    if (marriage_tbl.Rows.Count > highestCount)
                    {
                        highestCount = marriage_tbl.Rows.Count;
                        numHighestCount = 0;
                    }
                    else if (marriage_tbl.Rows.Count == highestCount)
                    {
                        numHighestCount++;
                    }
                    if (String.IsNullOrEmpty(person_row[U.MarriedName_col].ToString().Trim()))
                    {
                        count++;
                    }
                }
                else
                {
                    foreach (DataRow marriage_row in marriage_tbl.Rows)
                    {
                        CheckMaidenMarriedName(person_row, marriage_row, nums, nums1);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private static void CheckMaidenMarriedName(DataRow person_row,
                                                   DataRow marriage_row,
                                                   ArrayList nums,
                                                   ArrayList nums1)
        {
            int personId = person_row[U.PersonID_col].ToInt();
            string personSex = person_row[U.Sex_col].ToString();
            string personMaidenName = GetFatherLastName(person_row);
            string personMarriedName = person_row[U.MarriedName_col].ToString().Trim();
            if (personMaidenName == personMarriedName)
            {
                nums1.Add(personId);
            }
            /*
            int spouseId = 0;
            int spouse1 = marriage_row[U.PersonID_col].ToInt();
            int spouse2 = marriage_row[U.SpouseID_col].ToInt();
            if (personId == spouse1)
                spouseId = spouse2;
            else if (personId == spouse2)
                spouseId = spouse1;
            DataRow spouse_row = GetPerson(spouseId);
            if (spouse_row == null)
            {
                return;
            }
            string spouseSex = spouse_row[U.Sex_col].ToString();
            if (personSex == spouseSex)
            {
                return;
            }
            string spouseLastName = spouse_row[U.LastName_col].ToString().Trim();
            if (spouseLastName == personMaidenName)
            {
                nums.Add(personId);
                if (!string.IsNullOrEmpty(personMarriedName.Trim()) && personMaidenName != personMarriedName)
                {
                    person_row[U.LastName_col] = personMarriedName;
                }
                else
                {
                    person_row[U.LastName_col] = "";
                }
                person_row[U.MarriedName_col] = spouseLastName;
            }
            else if (String.IsNullOrEmpty(personMarriedName))
            {
                nums1.Add(personId);
                person_row[U.MarriedName_col] = spouseLastName;
            }*/
        }
        //****************************************************************************************************************************
        private static string GetFatherLastName(DataRow person_row)
        {
            int fatherId = person_row[U.FatherID_col].ToInt();
            if (fatherId == 0)
            {
                return person_row[U.LastName_col].ToString().Trim();
            }
            DataRow fatherRow = SQL.GetPerson(fatherId);
            if (fatherRow == null)
            {
                return person_row[U.LastName_col].ToString().Trim();
            }
            return fatherRow[U.LastName_col].ToString().Trim();
        }
        //****************************************************************************************************************************
        public static void FixPersonSource()
        {
            DataTable Person_tbl = new DataTable();
            SelectAll(U.Person_Table, Person_tbl);
            int count = 0; 
            foreach (DataRow Person_row in Person_tbl.Rows)
            {
                if (Person_row[U.Source_col].ToString().ToLower().Contains(U.JamaicaVitalRecords.ToLower()))
                {
                    Person_row[U.Source_col] = "";
                    count++;
                }
                if (SourceFromVitalOrCemeteryRecords(Person_row[U.BornSource_col].ToString()))
                {
                    SetBornPropertiesToBlank(Person_row);
                }
                if (SourceFromVitalOrCemeteryRecords(Person_row[U.DiedSource_col].ToString()))
                {
                    SetDiedPropertiesToBlank(Person_row);
                }
                if (SourceFromVitalOrCemeteryRecords(Person_row[U.BuriedSource_col].ToString()))
                {
                    SetBuriedPropertiesToBlank(Person_row);
                }
            }
            SQL.UpdateWithDA(Person_tbl, U.Person_Table, U.PersonID_col, new ArrayList(new string[] 
                { U.Source_col, U.BornDate_col, U.BornPlace_col, U.BornHome_col, U.BornVerified_col, U.BornSource_col, U.BornBook_col, U.BornPage_col,
                                U.DiedDate_col, U.DiedPlace_col, U.DiedHome_col, U.DiedVerified_col, U.DiedSource_col, U.DiedBook_col, U.DiedPage_col,
                                U.BuriedDate_col, U.BuriedPlace_col, U.BuriedStone_col, U.BuriedVerified_col, U.BuriedSource_col, U.BuriedBook_col, U.BuriedPage_col }));
        }
        //****************************************************************************************************************************
        private static bool SourceFromVitalOrCemeteryRecords(string source)
        {
            if (source.ToLower().Contains("from"))
            {
                return true;
            }
            if (source.ToLower().Contains(U.JamaicaVitalRecords.ToLower()))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private static void SetBornPropertiesToBlank(DataRow Person_row)
        {
            Person_row[U.BornDate_col] = "";
            Person_row[U.BornPlace_col] = "";
            Person_row[U.BornHome_col] = "";
            Person_row[U.BornVerified_col] = 'N';
            Person_row[U.BornSource_col] = "";
            Person_row[U.BornBook_col] = "";
            Person_row[U.BornPage_col] = "";
        }
        //****************************************************************************************************************************
        private static void SetDiedPropertiesToBlank(DataRow Person_row)
        {
            Person_row[U.DiedDate_col] = "";
            Person_row[U.DiedPlace_col] = "";
            Person_row[U.DiedHome_col] = "";
            Person_row[U.DiedVerified_col] = 'N';
            Person_row[U.DiedSource_col] = "";
            Person_row[U.DiedBook_col] = "";
            Person_row[U.DiedPage_col] = "";
        }
        //****************************************************************************************************************************
        private static void SetBuriedPropertiesToBlank(DataRow Person_row)
        {
            Person_row[U.BuriedDate_col] = "";
            Person_row[U.BuriedPlace_col] = "";
            Person_row[U.BuriedStone_col] = "";
            Person_row[U.BuriedVerified_col] = 'N';
            Person_row[U.BuriedSource_col] = "";
            Person_row[U.BuriedBook_col] = "";
            Person_row[U.BuriedPage_col] = "";
        }
        //****************************************************************************************************************************
        public static void CheckPersonSex()
        {
            DataTable Person_tbl = new DataTable();
            SelectAll(U.Person_Table, Person_tbl);
            foreach (DataRow Person_row in Person_tbl.Rows)
            {
                string sFirstName = Person_row[U.FirstName_col].ToString();
                string sSex = Person_row[U.Sex_col].ToString();
                if (sFirstName.Length == 0)
                {
                    continue;
                }
                if (sSex != "M" && sSex != "F")
                {
                    continue;
                }
                char firstNameSex = SQL.GetFirstNameSex(sFirstName);
                if (firstNameSex != sSex[0])
                {
                    FixSexProblem(Person_row);
                }
            }
            SQL.UpdateWithDA(Person_tbl, U.Person_Table, U.PersonID_col, new ArrayList(new string[] { U.Sex_col, U.MarriedName_col }));
        }
        //****************************************************************************************************************************
        private static void FixSexProblem(DataRow personRow)
        {
            DataTable Marriage_tbl = new DataTable();
            int personId = personRow[U.PersonID_col].ToInt();
            char personSex = personRow[U.Sex_col].ToChar();
            GetAllSpouses(Marriage_tbl, personId);
            if (Marriage_tbl.Rows.Count == 0)
            {
                return;
            }
            char spouseSex = ' ';
            foreach (DataRow Marriage_row in Marriage_tbl.Rows)
            {
                int marriagePersonID = Marriage_row[U.PersonID_col].ToInt();
                int marriageSpouseID = Marriage_row[U.SpouseID_col].ToInt();
                int iSpouseId = (personId == marriagePersonID) ? marriageSpouseID : marriagePersonID;
                if (iSpouseId != 0)
                {
                    DataRow spouseRow = GetPerson(iSpouseId);
                    if (spouseSex == ' ')
                    {
                        spouseSex = spouseRow[U.Sex_col].ToChar();
                    }
                    else if (spouseRow[U.Sex_col].ToChar() != spouseSex)
                    {
                        MessageBox.Show("Spouse with different sex");
                    }
                }
            }
            if (spouseSex == ' ')
            {
                return;
            }
            if (spouseSex != personSex)
            {
                return;
            }
            personRow[U.Sex_col] = (personSex == 'F') ? 'M' : 'F';
            if (personRow[U.Sex_col].ToChar() == 'M')
            {
                personRow[U.MarriedName_col] = "";
            }
        }
        //****************************************************************************************************************************
        private static void UpdateTheFirstNameTable(string sInputTable, bool includeFathermother=false, bool includeSpouse=false)
        {
            DataTable tbl = new DataTable();
            SelectAll(sInputTable, tbl);
            foreach (DataRow row in tbl.Rows)
            {
                string sFirstName = row[U.FirstName_col].ToString();
                string sSex = row[U.Sex_col].ToString();
                if (sFirstName.Length == 0)
                {
                    continue;
                }
                if (sSex != "M" && sSex != "F")
                {
                    continue;
                }
                else
                {
                    UpdateFirstNameTable(sFirstName, sSex);
                }
                if (includeFathermother)
                {
                    string sFatherFirstName = row[U.FatherFirstName_col].ToString();
                    if (sFatherFirstName.Length != 0)
                    {
                        UpdateFirstNameTable(sFatherFirstName, "M");
                    }
                    string sMotherFirstName = row[U.MotherFirstName_col].ToString();
                    if (sMotherFirstName.Length != 0)
                    {
                        UpdateFirstNameTable(sMotherFirstName, "F");
                    }
                }
                if (includeSpouse)
                {
                    string sSpouseFirstName = row[U.SpouseFirstName_col].ToString();
                    sSex = (sSex == "F") ? "M" : "F";
                    UpdateFirstNameTable(sSpouseFirstName, sSex);
                }
            }
        }
        //****************************************************************************************************************************
        public static bool SetFirstNameSexToValueInFirstnameTable(string sInputTable,
                                                                   string id_col)
        {
            DataTable tbl = new DataTable();
            SelectAll(sInputTable, tbl);
            foreach (DataRow row in tbl.Rows)
            {
                int id = row[id_col].ToInt();
                string sFirstName = row[U.FirstName_col].ToString();
                char personSex = row[U.Sex_col].ToChar();
                char firstNameSex = GetFirstNameSex(sFirstName);
                if (sexDifference(firstNameSex, personSex))
                {
                    UpdateWithParms(sInputTable, new NameValuePair(id_col, id), new NameValuePair(U.Sex_col, firstNameSex));
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        private static bool sexDifference(char firstNameSex,
                                   char personSex)
        {
            if (firstNameSex != 'M' && firstNameSex != 'F')
            {
                return false;
            }
            if (personSex != firstNameSex)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        public static void UpdateFirstNameTable(string sFirstName,
                                                 string sSex)
        {
            if (String.IsNullOrEmpty(sFirstName.Trim()) || sFirstName.Length <= 1)
            {
                return;
            }
            DataRow FirstName_row = GetFirstName(sFirstName);
            if (FirstName_row == null)
            {
                AddFirstNameToTable(sFirstName, sSex);
            }
            else
            {
                CheckForFirstNameUsedForBothSexes(FirstName_row, sSex);
            }
        }
        //****************************************************************************************************************************
        public static DataRow GetFirstName(string sFirstName)
        {
            DataTable FirstName_tbl = new DataTable();
            SelectAll(U.FirstName_Table, FirstName_tbl, new NameValuePair(U.FirstName_col, sFirstName));
            if (FirstName_tbl.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FirstName_tbl.Rows[0];
            }
        }
        //****************************************************************************************************************************
        public static char GetFirstNameSex(string sFirstName)
        {
            DataRow FirstName_row = GetFirstName(sFirstName);
            if (FirstName_row == null)
            {
                return ' ';
            }
            else
            {
                return FirstName_row[U.Sex_col].ToString().ToUpper()[0];
            }
        }
        //****************************************************************************************************************************
        public static eSex GetSex(string firstName)
        {
            char sex = SQL.GetFirstNameSex(firstName);
            if (sex == 'M')
            {
                return eSex.eMale;
            }
            else if (sex == 'F')
            {
                return eSex.eFemale;
            }
            else
            {
                return eSex.eUnknown;
            }
        }
        //****************************************************************************************************************************
        private static void UpdateFirstName(string sFirstName)
        {
            UpdateWithParms(U.FirstName_Table, new NameValuePair(U.FirstName_col, sFirstName),
                                               new NameValuePair(U.Sex_col, 'B'));
        }
        //****************************************************************************************************************************
        private static void AddFirstNameToTable(string sFirstName,
                                                string sSex)
        {
            DataTable firstName_tbl = DefineFirstNameTable();
            DataRow firstName_row = firstName_tbl.NewRow();
            firstName_row[U.FirstName_col] = sFirstName;
            firstName_row[U.Sex_col] = sSex;
            firstName_tbl.Rows.Add(firstName_row);
            SqlCommand insertCommand = InsertCommand(firstName_tbl, U.FirstName_Table, false);
            InsertWithDA(firstName_tbl, insertCommand);
        }
        //****************************************************************************************************************************
        private static void CheckForFirstNameUsedForBothSexes(DataRow FirstName_row,
                                                              string sSex)
        {
            string FirstNameSex = FirstName_row[U.Sex_col].ToString();
            if (FirstNameSex != "B" && FirstNameSex.Trim() != sSex.Trim())
            {
                if (FirstName_row[U.FirstName_col].ToString().ToLower() == "sheila")
                {
                }
                UpdateFirstName(FirstName_row[U.FirstName_col].ToString());
            }
        }
        //****************************************************************************************************************************
    }
}
