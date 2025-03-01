using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static DataTable DefineMarriageTable()
        {
            DataTable tbl = DefineMarriageTableNoConstraints();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PersonID_col], tbl.Columns[U.SpouseID_col] };
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefineMarriageTableNoConstraints()
        {
            DataTable tbl = new DataTable(U.Marriage_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.SpouseID_col, typeof(int));
            tbl.Columns.Add(U.DateMarried_col, typeof(string));
            tbl.Columns.Add(U.Divorced_col, typeof(char));
            tbl.Columns[U.DateMarried_col].MaxLength = U.iMaxDateLength;
            return tbl;
        }
        //****************************************************************************************************************************
        private static bool DeleteInsertMarriages(SqlTransaction txn,
                                                  DataTable PersonTbl,
                                                  DataTable Marriage_tbl)
        {
            int iNumRecordsToAdd = Marriage_tbl.NumStateRecordsInTable(DataViewRowState.Added);
            int iNumRecordsToDelete = Marriage_tbl.NumStateRecordsInTable(DataViewRowState.Deleted);
            if (iNumRecordsToAdd == 0 && iNumRecordsToDelete == 0)
                return true;
            SqlCommand insertCommand = InsertCommand(txn, Marriage_tbl, U.Marriage_Table, false);
            SqlCommand deleteCommand = DeleteCommandWithDA(txn, U.Marriage_Table, U.PersonID_col, U.SpouseID_col);
            SqlCommand updateCommand = DummySqlCommandCommand(txn);
            return DeleteUpdateInsertWithDA(Marriage_tbl, deleteCommand, updateCommand, insertCommand);
        }
        //****************************************************************************************************************************
        public static string GetDivorcedFromRow(DataRow row)
        {
            string sDivorced = row[3].ToString();
            if (sDivorced.Length == 0)
                return "Married Traditional";
            else if (sDivorced[0] == 'Y' || sDivorced[0] == 'y')
                return "Divorced";
            else if (sDivorced[0] == 'D' || sDivorced[0] == 'd')
                return "Divorced";
            else if (sDivorced[0] == 'C' || sDivorced[0] == 'c')
                return "Married Non-Traditional";
            else if (sDivorced[0] == 'P' || sDivorced[0] == 'p')
                return "Parents Of";
            else if (sDivorced[0] == 'L' || sDivorced[0] == 'L')
                return "Living Together";
            else
                return "Married Traditional";
        }
        //****************************************************************************************************************************
        public static bool CheckForMarriageDuplicates()
        {
            DataTable MarrageNames_tbl = new DataTable("MarrageNames");
            MarrageNames_tbl.Columns.Add(U.PersonID_col, typeof(int));
            MarrageNames_tbl.Columns.Add(U.SpouseID_col, typeof(int));
            MarrageNames_tbl.Columns.Add("SortName", typeof(string));
            DataTable tblSpouses = DefineMarriageTable();
            SelectAll(U.Marriage_Table, tblSpouses);
            int iCount = 0;
            foreach (DataRow row in tblSpouses.Rows)
            {
                int iPersonID = row[U.PersonID_col].ToInt();
                int iSpouseID = row[U.SpouseID_col].ToInt();
                DataRow Person_row = GetPerson(iPersonID);
                DataRow Spouse_row = GetPerson(iSpouseID);
                if (Person_row == null || Spouse_row == null)
                {
                    iCount++;
                }
                else
                {
                    char sex = Person_row[U.Sex_col].ToString()[0];
                    DataRow MarriageNamesRow = MarrageNames_tbl.NewRow();
                    MarriageNamesRow[U.PersonID_col] = iPersonID;
                    MarriageNamesRow[U.SpouseID_col] = iSpouseID;
                    string s = SortString(Person_row, Spouse_row);
                    if (sex == 'M')
                        MarriageNamesRow["SortName"] = SortString(Person_row, Spouse_row);
                    else
                        MarriageNamesRow["SortName"] = SortString(Spouse_row, Person_row);
                    if (s == "Howe Ora Mary")
                    {
                    }
                    MarrageNames_tbl.Rows.Add(MarriageNamesRow);
                }
            }
            string sortExp = "SortName";
            DataRow[] drarray = MarrageNames_tbl.Select(null, sortExp, DataViewRowState.CurrentRows);
            string sLastName = "";
            iCount = 0;
            foreach (DataRow SortRow in drarray)
            {
                string sThisName = SortRow["SortName"].ToString();
                if (sLastName == sThisName)
                {
                    iCount++;
                }
                sLastName = sThisName;
            }
            return true;
        }
        //****************************************************************************************************************************
        private static string SortString(DataRow PersonRow,
                                  DataRow SpouseRow)
        {
            return PersonRow[U.LastName_col].ToString() + " " +
                   PersonRow[U.FirstName_col].ToString() + " " +
                   SpouseRow[U.FirstName_col].ToString();
        }
        //****************************************************************************************************************************
        public static bool CheckSpouseLastNames()
        {
            DataTable tbl = GetMarriageRecords();
            int iCounter = 0;
            int iCounter2 = 0;
            foreach (DataRow row in tbl.Rows)
            {
                int iPersonID = row[U.PersonID_col].ToInt();
                int iSpouseID = row[U.SpouseID_col].ToInt();
                if (iPersonID != 0 && iSpouseID != 0)
                {
                    DataRow PersonRow = GetPerson(iPersonID);
                    DataRow SpouseRow = GetPerson(iSpouseID);
                    if (PersonRow == null || SpouseRow == null)
                    {
                        iCounter2++;
                    }
                    else
                    {
                        string sPersonLastName = PersonRow[U.LastName_col].ToString();
                        string sSpouseLastName = SpouseRow[U.LastName_col].ToString();
                        if (sPersonLastName == sSpouseLastName)
                        {
                            iCounter++;
                        }
                    }
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool SetAllBlankMarriageDateWithVitalRecordDates()
        {
            DataTable MarriageTable = new DataTable();
            SelectAll(U.Marriage_Table, MarriageTable, new NameValuePair(U.DateMarried_col, " = ''"));
            foreach (DataRow Marriage_row in MarriageTable.Rows)
            {
                int iPersonID = Marriage_row[U.PersonID_col].ToInt();
                int iSpouseID = Marriage_row[U.SpouseID_col].ToInt();
                DataTable VitalRecordTable = new DataTable();
                GetVitalRecordTypeRecordForPerson(VitalRecordTable, EVitalRecordType.eMarriageBride, iPersonID, U.PersonID_col);
                GetVitalRecordTypeRecordForPerson(VitalRecordTable, EVitalRecordType.eMarriageGroom, iPersonID, U.PersonID_col);
                foreach (DataRow VitalRecord_row in VitalRecordTable.Rows)
                {
                    DataTable SpouseVitalRecord_tbl = new DataTable();
                    DataRow row = GetVitalRecord(VitalRecord_row[U.SpouseID_col].ToInt());
                    if (row != null && row[U.PersonID_col].ToInt() == iSpouseID)
                    {
                        Marriage_row[U.DateMarried_col] = U.BuildDate(row[U.DateYear_col].ToInt(), row[U.DateMonth_col].ToInt(), row[U.DateDay_col].ToInt());
                    }
                }
            }
            SqlTransaction txn = sqlConnection.BeginTransaction();
            UpdateMarriage(txn, MarriageTable);
            txn.Commit();
            return true;
        }
        //****************************************************************************************************************************
        public static bool CheckAllFemaleMarriedMaidenNames()
        {
            DataTable Person_tbl = new DataTable();
            SelectAll(U.Person_Table, Person_tbl, new NameValuePair(U.Sex_col, "'F'"));
            int iNumMultipleMariages = 0;
            foreach (DataRow Person_row in Person_tbl.Rows)
            {
                string sPersonLastName = Person_row[U.LastName_col].ToString();
                string sMarriedName = Person_row[U.MarriedName_col].ToString();
                if (sPersonLastName.Length != 0 && sMarriedName == sPersonLastName)
                    Person_row[U.LastName_col] = "";
                else
                {
                    DataTable Marriage_tbl = DefineMarriageTable();
                    int iNumberSpouses = 0;
                    int iSpouseLocationInArray;
                    int iPersonID = Person_row[U.PersonID_col].ToInt();
                    int iSpouseID = GetMarriagesID(Marriage_tbl, ref iNumberSpouses, out iSpouseLocationInArray, iPersonID);
                    if (iSpouseID != 0)
                    {
                        if (Marriage_tbl.Rows.Count != 1)
                            iNumMultipleMariages++;
                        else
                        {
                            DataRow Spouse_row = GetPerson(iSpouseID);
                            string sSpouseLastName = Spouse_row[U.LastName_col].ToString();
                            if (Person_row[U.Sex_col].ToString() != Spouse_row[U.Sex_col].ToString() &&
                                sPersonLastName == sSpouseLastName) // Person LastName is Married Name
                            {
                                Person_row[U.MarriedName_col] = sSpouseLastName;
                                if (sMarriedName.Length != 0)
                                    Person_row[U.LastName_col] = sMarriedName;
                            }
                        }
                    }
                }
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
