using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class CSql
    {
        //****************************************************************************************************************************
        public bool AddMarriageToDataTable(DataTable tbl,
                                           int iPerson,
                                           int iSpouse,
                                           string sDataMarried,
                                           string sDivorced)
        {
            DataRow Marriage_row = tbl.NewRow();
            Marriage_row[U.PersonID_col] = iPerson;
            Marriage_row[U.SpouseID_col] = iSpouse;
            Marriage_row[U.DateMarried_col] = sDataMarried;
            if (sDivorced.Length == 0)
                Marriage_row[U.Divorced_col] = ' ';
            else
                Marriage_row[U.Divorced_col] = sDivorced[0];
            tbl.Rows.Add(Marriage_row);
            return true;
        }
        //****************************************************************************************************************************
        public static DataTable DefineCemeteryRecordTable()
        {
            DataTable tbl = new DataTable(U.CemeteryRecord_Table);
            tbl.Columns.Add(U.CemeteryRecordID_col, typeof(int));
            tbl.Columns.Add(U.CemeteryID_col, typeof(int));
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.BuriedStone_col, typeof(string));
            tbl.Columns.Add(U.PersonName_col, typeof(string));
            tbl.Columns.Add(U.FatherName_col, typeof(string));
            tbl.Columns.Add(U.MotherName_col, typeof(string));
            tbl.Columns.Add(U.SpouseName_col, typeof(string));
            tbl.Columns.Add(U.Sex_col, typeof(char));
            tbl.Columns.Add(U.BornDate_col, typeof(string));
            tbl.Columns.Add(U.DiedDate_col, typeof(string));
            tbl.Columns.Add(U.PersonAge_col, typeof(string));
            tbl.Columns.Add(U.Epitaph_col, typeof(string));
            tbl.Columns.Add(U.CemeteryNote1_col, typeof(string));
            tbl.Columns.Add(U.CemeteryNote2_col, typeof(string));
            tbl.Columns.Add(U.CemeteryNote3_col, typeof(string));
            SetCemeteryVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetCemeteryVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.BuriedStone_col].MaxLength = U.iMaxStoneLength;
            tbl.Columns[U.PersonName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.FatherName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.MotherName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.SpouseName_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.BornDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.DiedDate_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.PersonAge_col].MaxLength = U.iMaxDateLength;
            tbl.Columns[U.Epitaph_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.CemeteryNote1_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.CemeteryNote2_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.CemeteryNote3_col].MaxLength = U.iMaxValueLength;
        }
        //****************************************************************************************************************************
        public int GetCemeteryIDFromName(string sCemeteryName)
        {
            DataTable tbl = new DataTable(U.Cemetery_Table);
            selectall(tbl, U.Cemetery_Table, U.NoOrderBy, new NameValuePair(U.CemeteryName_col, sCemeteryName));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.CemeteryID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public bool AddCemeteryToDataTable(DataTable tbl,
                                           int iCemeteryID,
                                           int iPersonID,
                                           string sBuriedStone,
                                           string sPersonName,
                                           string sFatherName,
                                           string sMotherName,
                                           string sSpouseName,
                                           string sPersonSex,
                                           string sBornDate,
                                           string sDiedDate,
                                           string sPersonAge,
                                           string sEpitaph,
                                           string sNote1,
                                           string sNote2,
                                           string sNote3)
        {
            DataRow Cemetery_row = tbl.NewRow();
            Cemetery_row[U.CemeteryID_col] = iCemeteryID;
            Cemetery_row[U.PersonID_col] = iPersonID;
            Cemetery_row[U.BuriedStone_col] = sBuriedStone;
            Cemetery_row[U.PersonName_col] = sPersonName;
            Cemetery_row[U.FatherName_col] = sFatherName;
            Cemetery_row[U.MotherName_col] = sMotherName;
            Cemetery_row[U.SpouseName_col] = sSpouseName;
            if (sPersonSex.Length == 0)
                Cemetery_row[U.Sex_col] = ' ';
            else
                Cemetery_row[U.Sex_col] = sPersonSex[0];
            Cemetery_row[U.BornDate_col] = sBornDate;
            Cemetery_row[U.DiedDate_col] = sDiedDate;
            Cemetery_row[U.PersonAge_col] = sPersonAge;
            Cemetery_row[U.Epitaph_col] = sEpitaph;
            Cemetery_row[U.CemeteryNote1_col] = sNote1;
            Cemetery_row[U.CemeteryNote2_col] = sNote2;
            Cemetery_row[U.CemeteryNote3_col] = sNote3;
            tbl.Rows.Add(Cemetery_row);
            return true;
        }
        //****************************************************************************************************************************
        public int GetPersonIDFromImportPersonIDFromTable(DataTable Person_tbl,
                                                           int iImportPersonID)
        // Look to see if the ImportPersonID exists in the DataTable.  
        // The ImportPersonID is the PersonID which exist in the From Database
        // Return the PersonId from the main database which corresponds to the ImportPersonID.
        {
            DataRow Person_row = Person_tbl.Rows.Find(iImportPersonID);
            if (Person_row == null)
                return 0;
            else
            {
                return Person_row[U.PersonID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public bool AddPersonToDataTable(DataTable m_Person_tbl,
                                         CPersonRecord p)
        {
            AddPersonToDataTable(m_Person_tbl, p.PersonFirstName, p.PersonMiddleName, p.PersonLastName, p.PersonSuffix, p.PersonPrefix, p.PersonMarriedName,
                                          p.PersonKnownAs, p.PersonFatherID, p.PersonMotherID, p.PersonDescription, p.PersonSource, p.PersonSex,
                                          p.PersonBornDate, p.PersonBornPlace, p.PersonBornHome, p.PersonBornVerified, p.PersonBornSource, p.PersonBornBook, p.PersonBornPage,
                                          p.PersonDiedDate, p.PersonDiedPlace, p.PersonDiedHome, p.PersonDiedVerified, p.PersonDiedSource, p.PersonDiedBook, p.PersonDiedPage,
                                          p.PersonBuriedDate, p.PersonBuriedPlace, p.PersonBuriedStone, p.PersonBuriedVerified, p.PersonBuriedSource,
                                          p.PersonBuriedBook, p.PersonBuriedPage, p.iImportPersonID);
            return true;
        }
        //****************************************************************************************************************************
        public bool AddPersonToDataTable(DataTable tbl,
                                         string sFirstName,
                                         string sMiddleName,
                                         string sLastName,
                                         string sSuffix,
                                         string sPrefix,
                                         string sMarriedName,
                                         string sKnownAs,
                                         int iFatherID,
                                         int iMotherID,
                                         string sDescription,
                                         string sSource,
                                         string sSex,
                                         string sBornDate,
                                         string sBornPlace,
                                         string sBornHome,
                                         string sBornVerified,
                                         string sBornSource,
                                         string sBornBook,
                                         string sBornPage,
                                         string sDiedDate,
                                         string sDiedPlace,
                                         string sDiedHome,
                                         string sDiedVerified,
                                         string sDiedSource,
                                         string sDiedBook,
                                         string sDiedPage,
                                         string sBuriedDate,
                                         string sBuriedPlace,
                                         string sBuriedStone,
                                         string sBuriedVerified,
                                         string sBuriedSource,
                                         string sBuriedBook,
                                         string sBuriedPage,
                                         int iImportPersonID)
        {
            DataRow Person_row = tbl.NewRow();
            Person_row[U.PersonID_col] = 0;
            Person_row[U.FirstName_col] = sFirstName;
            Person_row[U.MiddleName_col] = sMiddleName;
            Person_row[U.LastName_col] = sLastName;
            Person_row[U.Suffix_col] = sSuffix;
            Person_row[U.Prefix_col] = sPrefix;
            Person_row[U.MarriedName_col] = sMarriedName;
            Person_row[U.KnownAs_col] = sKnownAs;
            Person_row[U.FatherID_col] = iFatherID;
            Person_row[U.MotherID_col] = iMotherID;
            Person_row[U.Notes_col] = sDescription;
            Person_row[U.Source_col] = sSource;
            if (sSex.Length == 0)
                Person_row[U.Sex_col] = ' ';
            else
                Person_row[U.Sex_col] = sSex[0];
            Person_row[U.BornDate_col] = sBornDate;
            Person_row[U.BornPlace_col] = sBornPlace;
            Person_row[U.BornHome_col] = sBornHome;
            if (sBornVerified.Length == 0)
                Person_row[U.BornVerified_col] = ' ';
            else
                Person_row[U.BornVerified_col] = sBornVerified;
            Person_row[U.BornSource_col] = sBornSource;
            Person_row[U.BornBook_col] = sBornBook;
            Person_row[U.BornPage_col] = sBornPage;
            Person_row[U.DiedDate_col] = sDiedDate;
            Person_row[U.DiedPlace_col] = sDiedPlace;
            Person_row[U.DiedHome_col] = sDiedHome;
            if (sDiedVerified.Length == 0)
                Person_row[U.DiedVerified_col] = ' ';
            else
                Person_row[U.DiedVerified_col] = sDiedVerified;
            Person_row[U.DiedSource_col] = sDiedSource;
            Person_row[U.DiedBook_col] = sDiedBook;
            Person_row[U.DiedPage_col] = sDiedPage;
            Person_row[U.BuriedDate_col] = sBuriedDate;
            Person_row[U.BuriedPlace_col] = sBuriedPlace;
            Person_row[U.BuriedStone_col] = sBuriedStone;
            if (sBuriedVerified.Length == 0)
                Person_row[U.BuriedVerified_col] = ' ';
            else
                Person_row[U.BuriedVerified_col] = sBuriedVerified;
            Person_row[U.BuriedSource_col] = sBuriedSource;
            Person_row[U.BuriedBook_col] = sBuriedBook;
            Person_row[U.BuriedPage_col] = sBuriedPage;
            Person_row[U.ImportPersonID_col] = iImportPersonID;
            for (int i = 1790; i <= 1940; i += 10)
            {
                Person_row[U.CensusYearCol(1)] = 0;
            }
            tbl.Rows.Add(Person_row);
            return true;
        }
        //****************************************************************************************************************************
        public bool SetAllImportPersonIDsToZero()
        {
            DataTable tbl = new DataTable(U.Person_Table);
            string sCommand = "Select [" + U.ImportPersonID_col + "],[" + U.PersonID_col + "] from [" + U.Person_Table + "]" +
                              " Where [" + U.ImportPersonID_col + "] <> 0";
            SqlCommand SelectCmd = new SqlCommand(sCommand, sqlConnection);
            ExecuteSelectStatement(tbl, SelectCmd);
            foreach (DataRow row in tbl.Rows)
            {
                row[U.ImportPersonID_col] = 0;
            }
            UpdatePersonImportPersonID(tbl);
            return true;
        }
        //****************************************************************************************************************************
        public bool UpdatePersonImportPersonID(DataTable tbl)
        {
            SqlCommand Update_cmd = new SqlCommand();
            SqlParameterCollection Parms = Update_cmd.Parameters;
            string sUpdateString = "";
            string sWhereString = " where " + ColumnEquals(U.PersonID_col);
            Parms.Add(U.PersonID_col, SqlDbType.Int, 0, U.PersonID_col);
            AddUpdateValue(ref sUpdateString, ref sWhereString, ref Parms, U.ImportPersonID_col,
                           SqlDbType.Int, 0);
            Update_cmd.CommandText = "Update [" + U.Person_Table + "] set " + sUpdateString + sWhereString + ";";
            Update_cmd.UpdatedRowSource = UpdateRowSource.Both;
            Update_cmd.Connection = sqlConnection;
            SqlDataAdapter da = new SqlDataAdapter();
            SqlTransaction txn = sqlConnection.BeginTransaction();
            da.UpdateCommand = Update_cmd;
            da.UpdateCommand.Transaction = txn;
            if (Update(da, tbl) >= 0)
                txn.Commit();
            else
                txn.Rollback();
            return true;
        }
    }
}
