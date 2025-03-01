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
        //****************************************************************************************************************************
        private static void UpdateMarriage(SqlTransaction txn,
                                           DataTable marriage_tbl)
        {
            SqlCommand updateCommand = MarriageUpdateCommand(txn, marriage_tbl);
            SqlCommand insertCommamd = InsertCommand(txn, marriage_tbl, U.Marriage_Table, false);
            if (!UpdateInsertWithDA(marriage_tbl, updateCommand, insertCommamd))
            {
                ThrowVitalErrorException(ErrorCodes.eSaveUnsuccessful);
            }
        }
        //****************************************************************************************************************************
        private static SqlCommand MarriageUpdateCommand(SqlTransaction txn,
                                                        DataTable marriage_tbl)
        {
            string [] keyColumns = { U.PersonID_col, U.SpouseID_col };
            return UpdateCommand(txn, marriage_tbl.Columns, U.Marriage_Table, keyColumns, ColumnList(U.DateMarried_col, U.Divorced_col));
        }
        //****************************************************************************************************************************
        public static void UpdateMarriageDate(int iPersonID,
                                       int iSpouseID,
                                       string sDate)
        {
            DataTable tbl = new DataTable(U.Marriage_Table);
            GetMarriage(tbl, iPersonID, iSpouseID);
            if (tbl.Rows.Count == 0)
            {
                sDate.CheckStringLength(U.iMaxDateLength);
                InsertMarriage(iPersonID, iSpouseID);
            }
            else
            {
                DataRow row = tbl.Rows[0];
                NameValuePair[] whereValues = { new NameValuePair(U.PersonID_col, row[U.PersonID_col]),
                                                new NameValuePair(U.SpouseID_col, row[U.SpouseID_col]) };
                UpdateWithParms(U.Marriage_Table, whereValues, new NameValuePair(U.DateMarried_col, sDate));
            }
        }
        //****************************************************************************************************************************
        public static void UpdateMarriageDivorced(int iPersonID,
                                           int iSpouseID,
                                           string sDivorced)
        {
            DataTable tbl = new DataTable(U.Marriage_Table);
            GetMarriage(tbl, iPersonID, iSpouseID);
            if (tbl.Rows.Count == 0)
            {
                InsertMarriage(iPersonID, iSpouseID);
            }
            else
            {
                DataRow row = tbl.Rows[0];
                NameValuePair[] whereValues = { new NameValuePair(U.PersonID_col, row[U.PersonID_col]),
                                                new NameValuePair(U.SpouseID_col, row[U.SpouseID_col]) };
                UpdateWithParms(U.Marriage_Table, whereValues, new NameValuePair(U.Divorced_col, DivorcedChar(sDivorced)));
            }
        }
        //****************************************************************************************************************************
        public static char DivorcedChar(string sDivorced)
        {
            char cChar = 'M';
            if (sDivorced.Length != 0)
            {
                cChar = sDivorced[0];
                if (cChar == 'Y' || cChar == 'y')
                    cChar = 'M';
                else if (cChar == 'M' || cChar == 'm')
                    cChar = 'M';
                else if (cChar == 'C' || cChar == 'c')
                    cChar = 'C';
                else if (cChar == 'L' || cChar == 'l')
                    cChar = 'L';
                else if (cChar == 'P' || cChar == 'p')
                    cChar = 'P';
                else if (cChar == 'D' || cChar == 'd')
                    cChar = 'D';
                else
                    cChar = 'M';
            }
            return cChar;
        }
        //****************************************************************************************************************************
        public static void UpdateMultipleMarriages()
        {
            int countMales = 0;
            int countFemales = 0;
            int countNotMarried = 0;
            int countMarried = 0;
            int countWithMarriedName = 0;
            int countNullSpouse = 0;
            int countNotMale = 0;
            int countLastNameAlreadyInPersonRow = 0;
            int countLastNameAlreadyInPersonRowAlternate = 0;
            int countAddedToPerson = 0;
            int multipleMarriages = 0;
            int countWithMarriedNameNoLastName = 0;
            int countAddToMarriedName = 0;
            int countAddToMarriedName2 = 0;
            int countAddToMarriedName3 = 0;
            int threeMarriages = 0;
            DataTable person_tbl = SQL.GetAllPersons();
            //SQL.GetPerson(person_tbl, 1502);
            foreach (DataRow personRow in person_tbl.Rows)
            {
                if (personRow[U.Sex_col].ToChar() == 'M')
                {
                    countMales++;
                    continue;
                }
                countFemales++;
                int personId = personRow[U.PersonID_col].ToInt();
                DataTable marriageTbl = new DataTable();
                SQL.GetMarriages(marriageTbl, U.PersonID_col, personId);
                SQL.GetMarriages(marriageTbl, U.SpouseID_col, personId);
                if (marriageTbl.Rows.Count == 0)
                {
                    countNotMarried++;
                    if (!String.IsNullOrEmpty(personRow[U.MarriedName_col].ToString()))
                    {
                        countWithMarriedName++;
                        if (String.IsNullOrEmpty(personRow[U.LastName_col].ToString()))
                        {
                            countWithMarriedNameNoLastName++;
                        }
                    }
                }
                else
                {
                    countMarried++;
                    if (marriageTbl.Rows.Count > 1)
                    {
                        multipleMarriages++;
                    }
                    if (marriageTbl.Rows.Count > 2)
                    {
                        threeMarriages++;
                    }
                }
                foreach (DataRow marriageRow in marriageTbl.Rows)
                {
                    AddLastnameToPersonRecord(personRow, marriageRow, personId, ref countNullSpouse, ref countNotMale, ref countLastNameAlreadyInPersonRow,
                                              ref countLastNameAlreadyInPersonRowAlternate, ref countAddedToPerson,
                                              ref countAddToMarriedName, ref countAddToMarriedName2, ref countAddToMarriedName3);
                }
            }
            if (countAddedToPerson == 0)
            {
                MessageBox.Show("No New Marriages to Add");
                return;
            }
            string message = "First Marriages: " + countAddToMarriedName + "   Second Marriages: " + countAddToMarriedName2 + "   Third Marriage: " + countAddToMarriedName3;
            if (MessageBox.Show(message + "\nUpdate Person?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SQL.UpdateWithDA(person_tbl, U.Person_Table, U.PersonID_col, new ArrayList(new string[] { U.MarriedName_col, U.MarriedName2_col, U.MarriedName3_col }));
            }
        }
        //****************************************************************************************************************************
        public static void AddLastnameToPersonRecord(DataRow personRow, DataRow marriageRow, int personId,
                        ref int countNullSpouse, ref int countNotMale, ref int countLastNameAlreadyInPersonRow, ref int countLastNameAlreadyInPersonRowAlternate, ref int countAddedToPerson,
                        ref int countAddToMarriedName, ref int countAddToMarriedName2, ref int countAddToMarriedName3)
        {
            if (marriageRow.RowState == DataRowState.Deleted)
            {
                RemoveSpouseFromPersonRow(personRow, marriageRow, personId, ref countAddToMarriedName, ref countAddToMarriedName2, ref countAddToMarriedName3);
                return;
            }
            int spouseId = (marriageRow[U.PersonID_col].ToInt() == personId) ? marriageRow[U.SpouseID_col].ToInt() : marriageRow[U.PersonID_col].ToInt();
            DataRow spouseRow = SQL.GetPerson(spouseId);
            if (spouseRow == null)
            {
                countNullSpouse++;
                return;
            }
            if (spouseRow[U.Sex_col].ToChar() != 'M' || spouseRow[U.Sex_col].ToChar() == personRow[U.Sex_col].ToChar())
            {
                countNotMale++;
                return;
            }
            string spouseLastName = spouseRow[U.LastName_col].ToString();
            if (SpouseLastNameAlreadyInPersonRow(personRow, spouseLastName))
            {
                countLastNameAlreadyInPersonRow++;
                return;
            }
            DataTable LastNameAlternativeSpellings_tbl = SQL.GetAlternativeSpellings(U.AlternativeSpellingsLastName_Table, spouseLastName);
            foreach (DataRow LastNameAlternativeSpellings_row in LastNameAlternativeSpellings_tbl.Rows)
            {
                string alternateSpousLastName = LastNameAlternativeSpellings_row[U.AlternativeSpelling_Col].ToString();
                if (SpouseLastNameAlreadyInPersonRow(personRow, alternateSpousLastName))
                {
                    countLastNameAlreadyInPersonRowAlternate++;
                    return;
                }
            }
            countAddedToPerson++;
            if (String.IsNullOrEmpty(personRow[U.MarriedName_col].ToString()))
            {
                countAddToMarriedName++;
                personRow[U.MarriedName_col] = spouseLastName;
            }
            else if (String.IsNullOrEmpty(personRow[U.MarriedName2_col].ToString()))
            {
                countAddToMarriedName2++;
                personRow[U.MarriedName2_col] = spouseLastName;
            }
            else if (String.IsNullOrEmpty(personRow[U.MarriedName3_col].ToString()))
            {
                countAddToMarriedName3++;
                personRow[U.MarriedName3_col] = spouseLastName;
            }
            else
            {
                MessageBox.Show("More than 3 marriages");
            }
        }
        //****************************************************************************************************************************
        private static void RemoveSpouseFromPersonRow(DataRow personRow, DataRow marriageRow, int personId,
                                                      ref int countAddToMarriedName, ref int countAddToMarriedName2, ref int countAddToMarriedName3)
        {
            int mrgPersonId = (marriageRow[U.PersonID_col, DataRowVersion.Original].ToInt());
            int mrgSpouseId = (marriageRow[U.SpouseID_col, DataRowVersion.Original].ToInt());
            int spouseId = (mrgPersonId == personId) ? mrgSpouseId : mrgPersonId;
            DataRow spouseRow = SQL.GetPerson(spouseId);
            string spouseLastName = spouseRow[U.LastName_col].ToString();
            if (personRow[U.MarriedName_col].ToString() == spouseLastName)
            {
                personRow[U.MarriedName_col] = personRow[U.MarriedName2_col];
                countAddToMarriedName++;
                if (!String.IsNullOrEmpty(personRow[U.MarriedName2_col].ToString()))
                {
                    personRow[U.MarriedName2_col] = personRow[U.MarriedName3_col];
                    countAddToMarriedName2++;
                }
                if (!String.IsNullOrEmpty(personRow[U.MarriedName2_col].ToString()))
                {
                    personRow[U.MarriedName3_col] = "";
                    countAddToMarriedName3++;
                }
            }
            if (personRow[U.MarriedName2_col].ToString() == spouseLastName)
            {
                personRow[U.MarriedName2_col] = personRow[U.MarriedName3_col];
                countAddToMarriedName2++;
                if (!String.IsNullOrEmpty(personRow[U.MarriedName2_col].ToString()))
                {
                    personRow[U.MarriedName3_col] = "";
                    countAddToMarriedName3++;
                }
            }
            if (personRow[U.MarriedName3_col].ToString() == spouseLastName)
            {
                personRow[U.MarriedName3_col] = "";
                countAddToMarriedName3++;
            }
        }
        //****************************************************************************************************************************
        private static bool SpouseLastNameAlreadyInPersonRow(DataRow personRow, string spouseLastName)
        {
            if (personRow[U.LastName_col].ToString() == spouseLastName)
            {
                return true;
            }
            if (personRow[U.MarriedName_col].ToString() == spouseLastName)
            {
                return true;
            }
            if (personRow[U.MarriedName2_col].ToString() == spouseLastName)
            {
                return true;
            }
            if (personRow[U.MarriedName3_col].ToString() == spouseLastName)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
    }
}
