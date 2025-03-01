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
        public static DataTable GetAllMarriageRecords()
        {
            DataTable tbl = new DataTable();
            SelectAll(true, U.VitalRecord_Table, "", tbl,
                            new NameValuePair(U.VitalRecordType_col, (int) EVitalRecordType.eMarriageBride),
                            new NameValuePair(U.VitalRecordType_col, (int) EVitalRecordType.eMarriageGroom),
                            new NameValuePair(U.VitalRecordType_col, (int) EVitalRecordType.eCivilUnionPartyA),
                            new NameValuePair(U.VitalRecordType_col, (int) EVitalRecordType.eCivilUnionPartyB));
            if (tbl.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return tbl;
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllBurialRecords()
        {
            DataTable tbl = new DataTable();
            SelectAll(true, U.VitalRecord_Table, "", tbl,
                            new NameValuePair(U.VitalRecordType_col, (int)EVitalRecordType.eBurial));
            if (tbl.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return tbl;
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllDeathFemaleVitalRecords()
        {
            DataTable tbl = new DataTable();
            SelectAll(true, U.VitalRecord_Table, "", tbl, new NameValuePair(U.VitalRecordType_col, (int) EVitalRecordType.eDeathFemale));
            if (tbl.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return tbl;
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllDeathVitalRecords()
        {
            DataTable tbl = new DataTable();
            SelectAll(true, U.VitalRecord_Table, "", tbl, new NameValuePair(U.VitalRecordType_col, (int)EVitalRecordType.eDeathMale),
                                                          new NameValuePair(U.VitalRecordType_col, (int)EVitalRecordType.eDeathFemale));
            if (tbl.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return tbl;
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllBirthVitalRecords(string orderBy="")
        {
            DataTable tbl = new DataTable();
            SelectAll(true, U.VitalRecord_Table, orderBy, tbl, new NameValuePair(U.VitalRecordType_col, (int)EVitalRecordType.eBirthMale),
                                                          new NameValuePair(U.VitalRecordType_col, (int)EVitalRecordType.eBirthFemale));
            if (tbl.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return tbl;
            }
        }
        //****************************************************************************************************************************
        public static DataRow GetVitalRecord(int vitalRecordID)
        {
            DataTable tbl = SelectAll(U.VitalRecord_Table, new NameValuePair(U.VitalRecordID_col, vitalRecordID));
            if (tbl.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return tbl.Rows[0];
            }
        }
        //****************************************************************************************************************************
        public static int GetVitalRecordParents(int personId)
        {
            DataTable father_tbl = SelectAll(U.VitalRecord_Table, new NameValuePair(U.FatherID_col, personId));
            DataTable mother_tbl = SelectAll(U.VitalRecord_Table, new NameValuePair(U.MotherID_col, personId));
            return father_tbl.Rows.Count + mother_tbl.Rows.Count;
        }
        //****************************************************************************************************************************
        public static bool GetVitalRecord(DataTable VitalRecord_tbl,
                                          int iVitalRecordID)
        {
            return SelectAll(U.VitalRecord_Table, VitalRecord_tbl, new NameValuePair(U.VitalRecordID_col, iVitalRecordID));
        }
        //****************************************************************************************************************************
        public static DataTable GetAllVitalRecords()
        {
            DataTable VitalRecord_tbl = new DataTable();
            SelectAll(U.VitalRecord_Table, VitalRecord_tbl);
            return VitalRecord_tbl;
        }
        //****************************************************************************************************************************
        public static bool GetVitalRecordWithSpouseRecord(DataTable VitalRecord_tbl,
                                                          int iVitalRecordID)
        {
            VitalRecord_tbl.Clear();
            GetVitalRecord(VitalRecord_tbl, iVitalRecordID);
            if (VitalRecord_tbl.Rows.Count != 0)
            {
                GetSpouseRecord(VitalRecord_tbl, (EVitalRecordType)VitalRecord_tbl.Rows[0][U.VitalRecordType_col].ToInt());
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool GetAllFamilyRecordsForIntegrationSelectCmd(DataTable VitalRecord_tbl,
                                                                      string sFamilyLastName)
        {
            string sSelectCmd = SelectAllFromString(U.VitalRecord_Table) + " Where ";
            sSelectCmd += ColumnEquals(U.LastName_col) + " and ";
            SqlCommand RecordsToIntegrate_cmd = IntegrationCmd(sSelectCmd);
            RecordsToIntegrate_cmd.Parameters.Add(new SqlParameter("@" + U.LastName_col, sFamilyLastName));
            ExecuteSelectStatement(VitalRecord_tbl, RecordsToIntegrate_cmd);
            return true;
        }
        //****************************************************************************************************************************
        public static SqlCommand IntegrationCmd(string sSelectCmd)
        {
            int iZeroPersonID = 0;
            int iBurial = (int)EVitalRecordType.eBurial;
            sSelectCmd += "(" + ColumnEquals(U.PersonID_col) + " or " +
                                ColumnEqualsNameNotBurial(U.FatherFirstName_col, U.FatherID_col) + " or " +
                                ColumnEqualsNameNotBurial(U.MotherFirstName_col, U.MotherID_col) + ");";
            SqlCommand RecordsToIntegrate_cmd = new SqlCommand(sSelectCmd, sqlConnection);
            RecordsToIntegrate_cmd.Parameters.Add(new SqlParameter("@" + U.VitalRecordType_col, iBurial));
            RecordsToIntegrate_cmd.Parameters.Add(new SqlParameter("@" + U.FatherFirstName_col, ""));
            RecordsToIntegrate_cmd.Parameters.Add(new SqlParameter("@" + U.MotherFirstName_col, ""));
            RecordsToIntegrate_cmd.Parameters.Add(new SqlParameter("@" + U.PersonID_col, iZeroPersonID));
            RecordsToIntegrate_cmd.Parameters.Add(new SqlParameter("@" + U.FatherID_col, iZeroPersonID));
            RecordsToIntegrate_cmd.Parameters.Add(new SqlParameter("@" + U.MotherID_col, iZeroPersonID));
            return RecordsToIntegrate_cmd;
        }
        //****************************************************************************************************************************
        public static bool SelectAllRecordsForIntegration(DataTable VitalRecord_tbl)
        {
            string sSelectCmd = SelectAllFromString(U.VitalRecord_Table) + " Where ";
            ExecuteSelectStatement(VitalRecord_tbl, IntegrationCmd(sSelectCmd));
            return true;
        }
        //****************************************************************************************************************************
        private static string ColumnEqualsNameNotBurial(string sColumnName1,
                                                        string sColumnName2)
        {
            return "(" + U.VitalRecordType_col + " <> @" + U.VitalRecordType_col + " and " +
                         sColumnName1 + " <> @" + sColumnName1 + " and " +
                         sColumnName2 + " = @" + sColumnName2 + ")";
        }
        //****************************************************************************************************************************
        public static bool GetSpouseRecord(DataTable VitalRecord_tbl,
                                           EVitalRecordType eVitalRecordType)
        {
            if (eVitalRecordType.MarriageRecord()) // This is a marriage record
            {
                int iSpouseVitalRecordID = VitalRecord_tbl.Rows[0][U.SpouseID_col].ToInt();
                GetVitalRecord(VitalRecord_tbl, iSpouseVitalRecordID);
            }
            return true;
        }
        //****************************************************************************************************************************
        public static string GetBornDate(DataRow Person_row, EVitalRecordType vitalRecordTypeMale, EVitalRecordType vitalRecordTypeFemale)
        {
            string sBook = "";
            string sPage = "";
            string sDate = "";
            string sPlace = "";
            string sHome = "";
            string sSource = "";
            DataTable CemeteryRecord_tbl = SQL.DefineVitalRecord_Table();
            SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, Person_row[U.PersonID_col].ToInt());
            DataTable vitalRecordsTbl = new DataTable();
            GetVitalRecordsForPerson(vitalRecordsTbl, Person_row[U.PersonID_col].ToInt(), U.PersonID_col);
            U.GetPersonVitalStatistics(vitalRecordsTbl, CemeteryRecord_tbl, Person_row, vitalRecordTypeMale, vitalRecordTypeFemale,
                                       ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
            return sDate;
        }
        //****************************************************************************************************************************
        public static bool GetAllRecordsForPerson(DataTable VitalRecord_tbl,
                                                  int iPersonID,
                                                  string sex = "")
        {
            GetCemeteryRecordsIntoVitalRecordForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col);
            GetCemeteryRecordsIntoVitalRecordForPerson(VitalRecord_tbl, iPersonID, U.SpouseID_col);
            GetCemeteryRecordsIntoVitalRecordForPerson(VitalRecord_tbl, iPersonID, U.FatherID_col);
            GetCemeteryRecordsIntoVitalRecordForPerson(VitalRecord_tbl, iPersonID, U.MotherID_col);
            GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col);
            GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.FatherID_col);
            GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.MotherID_col);
            GetSchoolRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col, sex);
            return true;
        }
        //****************************************************************************************************************************
        public static DataRow GetBirthRecordForPerson(int iPersonID)
        {
            if (iPersonID == 0)
            {
                return null;
            }
            DataTable VitalRecord_tbl = new DataTable();
            SelectAll(U.VitalRecord_Table, VitalRecord_tbl, new NameValuePair(U.PersonID_col, iPersonID));
            foreach (DataRow VitalRecord_row in VitalRecord_tbl.Rows)
            {
                EVitalRecordType vitalRecordType = (EVitalRecordType)VitalRecord_row[U.VitalRecordType_col].ToInt();
                if (vitalRecordType == EVitalRecordType.eBirthMale || vitalRecordType == EVitalRecordType.eBirthFemale)
                {
                    return VitalRecord_row;
                }
            }
            return null;
        }
        //****************************************************************************************************************************
        public static bool GetVitalRecordsForPerson(DataTable VitalRecord_tbl,
                                                    int iPersonID,
                                                    string sIDColumn)
        {
            SelectAll(U.VitalRecord_Table, VitalRecord_tbl, new NameValuePair(sIDColumn, iPersonID));
            return true;
        }
        //****************************************************************************************************************************
        public static bool GetSchoolRecordsForPerson(DataTable VitalRecord_tbl,
                                                     int iPersonID,
                                                     string sIDColumn,
                                                     string sex)
        {
            DataTable schoolRecord_tbl = DefineSchoolRecordTable();
            SelectAll(U.SchoolRecord_Table, OrderBy(U.Year_col, U.Grade_col), schoolRecord_tbl, new NameValuePair(sIDColumn, iPersonID));
            foreach (DataRow schoolRecordRow in schoolRecord_tbl.Rows)
            {
                VitalRecord_tbl.Columns[U.MotherMiddleName_col].MaxLength = 30;
                ConvertSchoolRecordsToVitalRecords(schoolRecordRow, VitalRecord_tbl, sex);
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool GetCemeteryRecordsIntoVitalRecordForPerson(DataTable VitalRecord_tbl,
                                                    int iPersonID,
                                                    string sIDColumn)
        {
            DataTable cemeteryRecord_tbl = DefineCemeteryRecordTable();
            SelectAll(U.CemeteryRecord_Table, cemeteryRecord_tbl, new NameValuePair(sIDColumn, iPersonID));
            foreach (DataRow cemeteryRecordRow in cemeteryRecord_tbl.Rows)
            {
                ConvertCemeteryRecordsToVitalRecords(cemeteryRecordRow, VitalRecord_tbl);
            }
            return true;
        }
        //****************************************************************************************************************************
        public static void ConvertPersonRecordsToVitalRecords(DataTable personTbl, DataTable vitalRecordTbl)
        {
            foreach (DataRow personRow in personTbl.Rows)
            {
                DataRow fatherRow = SQL.GetPerson(personRow[U.FatherID_col].ToInt());
                DataRow motherRow = SQL.GetPerson(personRow[U.MotherID_col].ToInt());

                DataRow VitalRecordRow = vitalRecordTbl.NewRow();
                VitalRecordRow[U.VitalRecordID_col] = personRow[U.PersonID_col].ToInt() + 900000;
                VitalRecordRow[U.VitalRecordType_col] = EVitalRecordType.eSearch;
                VitalRecordRow[U.FirstName_col] = personRow[U.FirstName_col];
                VitalRecordRow[U.MiddleName_col] = personRow[U.MiddleName_col];
                VitalRecordRow[U.LastName_col] = personRow[U.LastName_col];
                VitalRecordRow[U.Suffix_col] = personRow[U.Suffix_col];
                VitalRecordRow[U.Prefix_col] = personRow[U.Prefix_col];
                if (fatherRow == null)
                {
                    VitalRecordRow[U.FatherFirstName_col] = "";
                    VitalRecordRow[U.FatherMiddleName_col] = "";
                    VitalRecordRow[U.FatherLastName_col] = "";
                    VitalRecordRow[U.FatherSuffix_col] = "";
                    VitalRecordRow[U.FatherPrefix_col] = "";
                }
                else
                {
                    VitalRecordRow[U.FatherFirstName_col] = fatherRow[U.FirstName_col];
                    VitalRecordRow[U.FatherMiddleName_col] = fatherRow[U.MiddleName_col];
                    VitalRecordRow[U.FatherLastName_col] = fatherRow[U.LastName_col];
                    VitalRecordRow[U.FatherSuffix_col] = fatherRow[U.Suffix_col];
                    VitalRecordRow[U.FatherPrefix_col] = fatherRow[U.Prefix_col];
                }
                if (motherRow == null)
                {
                    VitalRecordRow[U.MotherFirstName_col] = "";
                    VitalRecordRow[U.MotherMiddleName_col] = "";
                    VitalRecordRow[U.MotherLastName_col] = "";
                    VitalRecordRow[U.MotherSuffix_col] = "";
                    VitalRecordRow[U.MotherPrefix_col] = "";
                }
                else
                {
                    VitalRecordRow[U.MotherFirstName_col] = motherRow[U.FirstName_col];
                    VitalRecordRow[U.MotherMiddleName_col] = motherRow[U.MiddleName_col];
                    VitalRecordRow[U.MotherLastName_col] = motherRow[U.LastName_col];
                    VitalRecordRow[U.MotherSuffix_col] = motherRow[U.Suffix_col];
                    VitalRecordRow[U.MotherPrefix_col] = motherRow[U.Prefix_col];
                }
                VitalRecordRow[U.SpouseID_col] = 0;
                VitalRecordRow[U.Book_col] = "";
                VitalRecordRow[U.Page_col] = "";
                int year;
                int month;
                int day;
                string sDate = GetBornDate(personRow, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale);

                U.GetYMD(sDate, out year, out month, out day);
                VitalRecordRow[U.DateYear_col] = year;
                VitalRecordRow[U.DateMonth_col] = month;
                VitalRecordRow[U.DateDay_col] = day;
                VitalRecordRow[U.AgeYears_col] = 0;
                VitalRecordRow[U.AgeMonths_col] = 0;
                VitalRecordRow[U.AgeDays_col] = 0;
                VitalRecordRow[U.Disposition_col] = ' ';
                VitalRecordRow[U.CemeteryName_col] = "";
                VitalRecordRow[U.LotNumber_col] = "";
                VitalRecordRow[U.Notes_col] = "";
                VitalRecordRow[U.PersonID_col] = 0;
                VitalRecordRow[U.FatherID_col] = 0;
                VitalRecordRow[U.MotherID_col] = 0;
                VitalRecordRow[U.Sex_col] = personRow[U.Sex_col];
                VitalRecordRow[U.ExcludeFromSite_col] = 0;
                vitalRecordTbl.Rows.Add(VitalRecordRow);
            }
        }
        //****************************************************************************************************************************
        public static void ConvertSchoolRecordsToVitalRecords(DataRow schoolRecordRow, DataTable vitalRecordTbl, string sex)
        {
            DataRow VitalRecordRow = vitalRecordTbl.NewRow();
            VitalRecordRow[U.VitalRecordID_col] = schoolRecordRow[U.SchoolRecordID_col].ToInt() + 8900000;
            VitalRecordRow[U.VitalRecordType_col] = EVitalRecordType.eSchool;
            PersonName personName = new PersonName(schoolRecordRow[U.Person_col].ToString());
            VitalRecordRow[U.FirstName_col] = personName.firstName;
            VitalRecordRow[U.MiddleName_col] = personName.middleName;
            VitalRecordRow[U.LastName_col] = personName.lastName;
            VitalRecordRow[U.Suffix_col] = personName.suffix;
            VitalRecordRow[U.Prefix_col] = personName.prefix;
            VitalRecordRow[U.FatherFirstName_col] = GetSchool(schoolRecordRow[U.SchoolID_col].ToInt());
            VitalRecordRow[U.FatherMiddleName_col] = "School";
            VitalRecordRow[U.FatherLastName_col] = "";
            VitalRecordRow[U.FatherSuffix_col] = "";
            VitalRecordRow[U.FatherPrefix_col] = "";
            VitalRecordRow[U.MotherMiddleName_col] = "";
            SchoolRecordType schoolRecordType = (SchoolRecordType)schoolRecordRow[U.SchoolRecordType_col].ToInt();
            string grade = schoolRecordRow[U.Grade_col].ToString();
            if (String.IsNullOrEmpty(grade))
            {
                if (schoolRecordType == SchoolRecordType.teacherType)
                {
                    VitalRecordRow[U.MotherFirstName_col] = "Teacher";
                }
                else
                {
                    VitalRecordRow[U.MotherFirstName_col] = VitalRecordRow[U.FatherFirstName_col];
                    VitalRecordRow[U.MotherMiddleName_col] = VitalRecordRow[U.FatherMiddleName_col];
                    VitalRecordRow[U.FatherFirstName_col] = "";
                    VitalRecordRow[U.FatherMiddleName_col] = "";
                }
            }
            else if (grade.Length > 1)
            {
                if (schoolRecordType == SchoolRecordType.teacherType)
                {
                    VitalRecordRow[U.MotherFirstName_col] = "Teacher ";
                }
                if (grade == "Grammer" || grade == "Primary")
                {
                    VitalRecordRow[U.MotherMiddleName_col] += grade;
                }
                else
                {
                    VitalRecordRow[U.MotherMiddleName_col] += "Grades " + grade;
                }
            }
            else
            {
                VitalRecordRow[U.MotherFirstName_col] = "Grade " + grade;
            }
            VitalRecordRow[U.MotherLastName_col] = "";
            VitalRecordRow[U.MotherSuffix_col] = "";
            VitalRecordRow[U.MotherPrefix_col] = "";
            VitalRecordRow[U.Book_col] = "";
            VitalRecordRow[U.Page_col] = "";
            VitalRecordRow[U.DateYear_col] = schoolRecordRow[U.Year_col].ToInt();
            VitalRecordRow[U.DateMonth_col] = 0;
            VitalRecordRow[U.DateDay_col] = 0;
            VitalRecordRow[U.AgeYears_col] = 0;
            VitalRecordRow[U.AgeMonths_col] = 0;
            VitalRecordRow[U.AgeDays_col] = 0;
            VitalRecordRow[U.Disposition_col] = ' ';
            VitalRecordRow[U.CemeteryName_col] = "";
            VitalRecordRow[U.LotNumber_col] = "";
            VitalRecordRow[U.Notes_col] = "";
            VitalRecordRow[U.PersonID_col] = schoolRecordRow[U.PersonID_col].ToInt();
            VitalRecordRow[U.FatherID_col] = 0;
            VitalRecordRow[U.MotherID_col] = 0;
            VitalRecordRow[U.SpouseID_col] = 0;
            if (String.IsNullOrEmpty(sex))
            {
                eSex esex = GetSex(personName.firstName);
                VitalRecordRow[U.Sex_col] = (esex == eSex.eMale) ? 'M' : 'F';
            }
            else
            {
                VitalRecordRow[U.Sex_col] = sex[0];
            }
            VitalRecordRow[U.ExcludeFromSite_col] = 0;
            vitalRecordTbl.Rows.Add(VitalRecordRow);
        }
        //****************************************************************************************************************************
        public static void ConvertCemeteryRecordsToVitalRecords(DataRow cemeteryRecordRow, DataTable vitalRecordTbl)
        {
            if (String.IsNullOrEmpty(cemeteryRecordRow[U.LastName_col].ToString()) && String.IsNullOrEmpty(cemeteryRecordRow[U.FirstName_col].ToString()))
            {
                return;
            }
            DataRow VitalRecordRow = vitalRecordTbl.NewRow();
            VitalRecordRow[U.VitalRecordID_col] = cemeteryRecordRow[U.CemeteryRecordID_col].ToInt() + 9900000;
            VitalRecordRow[U.VitalRecordType_col] = EVitalRecordType.eIntegrateAll;
            VitalRecordRow[U.FirstName_col] = cemeteryRecordRow[U.FirstName_col];
            VitalRecordRow[U.MiddleName_col] = cemeteryRecordRow[U.MiddleName_col];
            VitalRecordRow[U.LastName_col] = cemeteryRecordRow[U.LastName_col];
            VitalRecordRow[U.Suffix_col] = cemeteryRecordRow[U.Suffix_col];
            VitalRecordRow[U.Prefix_col] = cemeteryRecordRow[U.Prefix_col];
            VitalRecordRow[U.FatherFirstName_col] = cemeteryRecordRow[U.FatherFirstName_col];
            VitalRecordRow[U.FatherMiddleName_col] = cemeteryRecordRow[U.FatherMiddleName_col];
            VitalRecordRow[U.FatherLastName_col] = cemeteryRecordRow[U.FatherLastName_col];
            VitalRecordRow[U.FatherSuffix_col] = cemeteryRecordRow[U.FatherSuffix_col];
            VitalRecordRow[U.FatherPrefix_col] = cemeteryRecordRow[U.FatherPrefix_col];
            VitalRecordRow[U.MotherFirstName_col] = cemeteryRecordRow[U.MotherFirstName_col];
            VitalRecordRow[U.MotherMiddleName_col] = cemeteryRecordRow[U.MotherMiddleName_col];
            VitalRecordRow[U.MotherLastName_col] = cemeteryRecordRow[U.MotherLastName_col];
            VitalRecordRow[U.MotherSuffix_col] = cemeteryRecordRow[U.MotherSuffix_col];
            VitalRecordRow[U.MotherPrefix_col] = cemeteryRecordRow[U.MotherPrefix_col];
            VitalRecordRow[U.Book_col] = "";
            VitalRecordRow[U.Page_col] = "";
            ConvertCemeteryDatesToVitalRecordDatesAge(VitalRecordRow, cemeteryRecordRow);
            VitalRecordRow[U.Disposition_col] = ' ';
            VitalRecordRow[U.CemeteryName_col] = U.CemeteryName((Cemetery)cemeteryRecordRow[U.CemeteryID_col].ToInt());
            VitalRecordRow[U.LotNumber_col] = cemeteryRecordRow[U.LotNumber_col];
            VitalRecordRow[U.Notes_col] = cemeteryRecordRow[U.Notes_col];
            VitalRecordRow[U.PersonID_col] = cemeteryRecordRow[U.PersonID_col].ToInt();
            VitalRecordRow[U.FatherID_col] = cemeteryRecordRow[U.FatherID_col].ToInt();
            VitalRecordRow[U.MotherID_col] = cemeteryRecordRow[U.MotherID_col].ToInt();
            VitalRecordRow[U.SpouseID_col] = cemeteryRecordRow[U.SpouseID_col].ToInt();
            VitalRecordRow[U.Sex_col] = cemeteryRecordRow[U.Sex_col];
            VitalRecordRow[U.ExcludeFromSite_col] = 0;
            vitalRecordTbl.Rows.Add(VitalRecordRow);
        }
        //****************************************************************************************************************************
        public static void ConvertCemeteryDatesToVitalRecordDatesAge(DataRow VitalRecordRow, DataRow cemeteryRecordRow)
        {
            int year;
            int month;
            int day;
            U.GetYMD(cemeteryRecordRow[U.DiedDate_col].ToString(), out year, out month, out day);
            VitalRecordRow[U.DateYear_col] = year;
            VitalRecordRow[U.DateMonth_col] = month;
            VitalRecordRow[U.DateDay_col] = day;
            if (cemeteryRecordRow[U.AgeYears_col].ToInt() > 0 || cemeteryRecordRow[U.AgeMonths_col].ToInt() > 0 || cemeteryRecordRow[U.AgeDays_col].ToInt() > 0)
            {
                VitalRecordRow[U.AgeYears_col] = cemeteryRecordRow[U.AgeYears_col];
                VitalRecordRow[U.AgeMonths_col] = cemeteryRecordRow[U.AgeMonths_col];
                VitalRecordRow[U.AgeDays_col] = cemeteryRecordRow[U.AgeDays_col];
            }
            else if (!String.IsNullOrEmpty(cemeteryRecordRow[U.BornDate_col].ToString()))
            {
                U.GetAge(cemeteryRecordRow[U.BornDate_col].ToString(), cemeteryRecordRow[U.DiedDate_col].ToString(), out year, out month, out day);
                VitalRecordRow[U.AgeYears_col] = year;
                VitalRecordRow[U.AgeMonths_col] = month;
                VitalRecordRow[U.AgeDays_col] = day;
            }
        }
        //****************************************************************************************************************************
        private static int CheckVitalRecordFatherMotherIDs(SqlTransaction txn,
                                          DataRow VitalRecord_row,
                                          DataTable Person_tbl,
                                          DataTable Marriage_tbl,
                                          int iVitalRecordPersonType,
                                          int iVitalRecordFatherType)
        {
            int iPersonID = VitalRecord_row[U.PersonID_col].ToInt();
            int iFatherID = VitalRecord_row[U.FatherID_col].ToInt();
            int iMotherID = VitalRecord_row[U.MotherID_col].ToInt();
            DataRow Father_row = GetVitalRecordPersonRow(Person_tbl, iVitalRecordFatherType);
            bool bUpdatePerson = false;
            if (Father_row != null)
            {
                int iPersonFatherID = Father_row[U.PersonID_col].ToInt();
                if (iFatherID != iPersonFatherID)
                {
                    iFatherID = iPersonFatherID;
                    VitalRecord_row[U.FatherID_col] = iFatherID;
                    bUpdatePerson = true;
                }
            }
            DataRow Mother_row = GetVitalRecordPersonRow(Person_tbl, iVitalRecordFatherType + 1);
            if (Mother_row != null)
            {
                int iPersonMotherID = Mother_row[U.PersonID_col].ToInt();
                if (iMotherID != iPersonMotherID)
                {
                    iMotherID = iPersonMotherID;
                    VitalRecord_row[U.MotherID_col] = iMotherID;
                    bUpdatePerson = true;
                }
                string sMarriedName = Mother_row[U.MarriedName_col].ToString();
                if (sMarriedName.Length == 0 && Father_row != null && Mother_row[U.LastName_col].ToString() != Father_row[U.LastName_col].ToString())
                {
                    Mother_row[U.MarriedName_col] = Father_row[U.LastName_col];
                    bUpdatePerson = true;
                }
            }
            DataRow Person_row = GetVitalRecordPersonRow(Person_tbl, iVitalRecordPersonType);
            if (Person_row == null)
                iPersonID = 0;
            else if (iPersonID != Person_row[U.PersonID_col].ToInt() || bUpdatePerson)
            {
                iPersonID = Person_row[U.PersonID_col].ToInt();
                Person_row[U.FatherID_col] = iFatherID;
                Person_row[U.MotherID_col] = iMotherID;
                if (iFatherID != 0 && iMotherID != 0)
                {
                    DataRow Marriage_row = GetMarriageRow(Marriage_tbl, iFatherID, iMotherID);
                    if (Marriage_row == null)
                    {
                        AddMarriageToDataTable(Marriage_tbl, iFatherID, iMotherID, "", "N");
                    }
                }
            }
            VitalRecord_row[U.PersonID_col] = iPersonID;
            return iPersonID;
        }
        //****************************************************************************************************************************
        private static DataRow GetVitalRecordPersonRow(DataTable Person_tbl,
                                     int iVitalRecordPersonType)
        {
            string sSelect = U.ImportPersonID_col + " = " + iVitalRecordPersonType.ToString();
            foreach (DataRow row in Person_tbl.Select(sSelect))
            {
                return row;
            }
            return null;
        }
        //****************************************************************************************************************************
        public static bool VitalRecordAlreadyExists(EVitalRecordType eVitalRecordType,
                                                    int iPersonID)
        {
            if (iPersonID == 0)
                return false;
            DataTable tbl = new DataTable();
            SelectAll(U.VitalRecord_Table, tbl, new NameValuePair(U.PersonID_col, iPersonID),
                                                new NameValuePair(U.VitalRecordType_col, (int)eVitalRecordType));
            return (tbl.Rows.Count != 0);
        }
        //****************************************************************************************************************************
        public static bool RemoveIntegratedIDIfPersonDoesNotExist()
        {
            DataTable VitalRecord_tbl = new DataTable();
            SelectAll(U.VitalRecord_Table, VitalRecord_tbl);
            foreach (DataRow VitalRecord_row in VitalRecord_tbl.Rows)
            {
                CheckID(VitalRecord_row[U.PersonID_col]);
                CheckID(VitalRecord_row[U.FatherID_col]);
                CheckID(VitalRecord_row[U.MotherID_col]);
            }
            return true;
        }
        //****************************************************************************************************************************
        private static void CheckID(object PersonID)
        {
            int iPersonID = PersonID.ToInt();
            if (iPersonID == 0)
                return;
            DataRow Person_row = GetPerson(iPersonID);
            if (Person_row == null)
                PersonID = 0;
        }
        //****************************************************************************************************************************
        public static void GetVitalRecordFromNameNoMiddleSuffixPrefixDate(DataTable VitalRecord_tbl,
                                   EVitalRecordType VitalRecordType,
                                   PersonName personName,
                                   VitalRecordProperties vitalRecordProperties)
        {
            SelectAll(U.VitalRecord_Table, VitalRecord_tbl, new NameValuePair(U.VitalRecordType_col, (int)VitalRecordType),
                                                            new NameValuePair(U.FirstName_col, personName.firstName),
                                                            new NameValuePair(U.LastName_col, personName.lastName),
                                                            new NameValuePair(U.DateDay_col, vitalRecordProperties.DateDay),
                                                            new NameValuePair(U.DateMonth_col, vitalRecordProperties.DateMonth),
                                                            new NameValuePair(U.DateYear_col, vitalRecordProperties.DateYear));
        }
        //****************************************************************************************************************************
        public static void GetVitalRecordFromNameDate(DataTable VitalRecord_tbl,
                                   EVitalRecordType VitalRecordType,
                                   PersonName personName,
                                   VitalRecordProperties vitalRecordProperties)
        {
            SelectAll(U.VitalRecord_Table, VitalRecord_tbl,  
                                       new NameValuePair(U.VitalRecordType_col, (int)VitalRecordType),
                                       new NameValuePair(U.FirstName_col, personName.firstName),
                                       new NameValuePair(U.MiddleName_col, personName.middleName),
                                       new NameValuePair(U.LastName_col, personName.lastName),
                                       new NameValuePair(U.Suffix_col, personName.suffix),
                                       new NameValuePair(U.Prefix_col, personName.prefix),
                                       new NameValuePair(U.DateDay_col, vitalRecordProperties.DateDay),
                                       new NameValuePair(U.DateMonth_col, vitalRecordProperties.DateMonth),
                                       new NameValuePair(U.DateYear_col, vitalRecordProperties.DateYear));
        }
        //****************************************************************************************************************************
        public static void GetVitalRecordsFromName(DataTable VitalRecord_tbl,
                                   string firstName,
                                   string lastName)
        {
            SelectAll(U.VitalRecord_Table, SQL.OrderBy(U.VitalRecordType_col), VitalRecord_tbl, 
                                       new NameValuePair(U.FirstName_col, firstName),
                                       new NameValuePair(U.LastName_col, lastName));
        }
        //****************************************************************************************************************************
        public static bool GetVitalRecordTypeRecordForPerson(DataTable VitalRecord_tbl,
                                                      EVitalRecordType VitalRecordType,
                                                      int iPersonID,
                                                      string sIDColumn)
        {
            int iVitalRecordType = (int)VitalRecordType;
            SelectAll(U.VitalRecord_Table, VitalRecord_tbl, new NameValuePair(sIDColumn, iPersonID),
                                                            new NameValuePair(U.VitalRecordType_col, iVitalRecordType));
            return true;
        }
        //****************************************************************************************************************************
    }
}
