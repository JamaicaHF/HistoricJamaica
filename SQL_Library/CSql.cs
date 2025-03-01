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
        private const string sSecurity = "Integrated Security=SSPI;";
        public string m_sDataDirectory;
        private string m_sErrorMessage = U.NoSQLError;
        private bool m_bFoundFatalError = false;
        public bool bFoundFatalError { get { return m_bFoundFatalError; } }

        public SqlConnection sqlConnection;
        public bool m_bFullDatabase = false;
        //****************************************************************************************************************************
        public CSql(string sDatabaseName,
                    string sServer,
                    string sDataDirectory,
                    bool bFullDatabase)
        {
            m_bFullDatabase = bFullDatabase;
            m_sDataDirectory = sDataDirectory;
            OpenConnection(sDatabaseName, sServer);
        }
        //****************************************************************************************************************************
        private void OpenConnection(string sDatabaseName,
                                    string sServer)
        {
			try
			{
                sqlConnection = new SqlConnection(sServer + sDatabaseName + sSecurity);
				sqlConnection.Open();
			    return;
			}
            catch (SqlException expsql)
            {
                sqlConnection = null;
                m_sErrorMessage += expsql.ToString();
                m_bFoundFatalError = true;
            }
        }
        public SqlTransaction GetTxn()
        {
            return sqlConnection.BeginTransaction();
        }
        //****************************************************************************************************************************
        public int Update(SqlDataAdapter da,
                          DataTable      tbl)
        {
            try
            {
                return da.Update(tbl);
            }
            catch (SqlException ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
            }
            catch (DBConcurrencyException ex)
            {
                string s = ex.Message;
                m_sErrorMessage += "Someone else modified this person before your update was processed.\n" +
                                       "The latest properties of this person will be redisplayed.";
                m_bFoundFatalError = true;
            }
            catch (Exception ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
            }
            return U.Exception;
        }
        //****************************************************************************************************************************
		public bool CloseConnection()
		{
//            if (sqlConnection != null)
//            {
//                sqlConnection.Close();
//                            sqlConnection = null;
//            }
            return true;
        }
        //****************************************************************************************************************************
        private int ExecuteSQLNonQueryCommand(SqlCommand cmd)
        {
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (SqlException expsql)
            {
                m_sErrorMessage += expsql.ToString();
                m_bFoundFatalError = true;
                return U.Exception;
            }
        }
        //****************************************************************************************************************************
        private int ExecuteSQLNonQueryCommand(string sSQLString)
        {
            int iNumRowsAffected = 0;
            SqlCommand sqlcommand = new SqlCommand(sSQLString, sqlConnection);
            try
            {
                iNumRowsAffected = sqlcommand.ExecuteNonQuery();
            }
            catch (SqlException expsql)
            {
                m_sErrorMessage += expsql.ToString();
                m_bFoundFatalError = true;
                return 0;
            }
            return iNumRowsAffected;
        }
        //****************************************************************************************************************************
        public int InsertTableWithAutoIncrement(SqlCommand Insert_cmd,
                                                DataTable tbl)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.InsertCommand = Insert_cmd;
            return Update(da, tbl);
        }
        //****************************************************************************************************************************
        public bool CopyDataSet(DataSet ds1,
                                DataSet ds2)
        {
            ds2.Clear();
            foreach (DataTable tbl in ds1.Tables)
            {
                DataTable aCategory = new DataTable(tbl.TableName);

                foreach (DataRow row in tbl.Rows)
                {
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        public int GetMaxValue(string sTableName,
                               string sColumn)
        {
            DataTable tbl = new DataTable(sTableName);
            string sSelectStatement = "Select Max(" + sColumn + ") as MaxValue from " + sTableName;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = sSelectStatement;
            ExecuteSelectStatement(tbl, cmd);
            if (tbl.Rows.Count == 0)
                return 0;
            else
                return tbl.Rows[0]["MaxValue"].ToInt();
        }
        //****************************************************************************************************************************
        public bool GetDistinctValues(DataTable tbl,
                                     string sTableName,
                                     string sColumn)
        {
            string sSelectStatement = "Select Distinct " + sColumn + " from " + sTableName +
                                      " order by " + sColumn;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = sSelectStatement;
            ExecuteSelectStatement(tbl, cmd);
            return true;
        }
        //****************************************************************************************************************************
        public int GetFunctionResult(string sFunction,
                                     string sTableName,
                                     string sFieldName)
        // Get the result of a function such as min or max storing the result in the first row and first column
        {
            DataTable tbl = new DataTable(sTableName);
            string inputs = "Select " + sFunction + "(" + sFieldName + ") from " + sTableName;
            SqlDataAdapter da = new SqlDataAdapter(inputs, sqlConnection);
            try
            {
                if (da.Fill(tbl) == 0)
                    return 0;
                else
                {
                    DataRow row = tbl.Rows[0];
                    string s = row[0].ToString();
                    if (s.Length == 0)
                        return 0;
                    else
                        return row[0].ToInt();
                }
            }
            catch (Exception ex)
            {
                m_sErrorMessage += ex.ToString();
                m_bFoundFatalError = true;
            }
            return 0;
        }
        //****************************************************************************************************************************
        private string ColumnEqualsNameNotBurial(string sColumnName1,
                                                 string sColumnName2)
        {
            return "(" + U.VitalRecordType_col + " <> @" + U.VitalRecordType_col + " and " +
                         sColumnName1 + " <> @" + sColumnName1 + " and " + 
                         sColumnName2 + " = @" + sColumnName2 + ")";
        }
        //****************************************************************************************************************************
        private string ColumnEquals(string sColumnName)
        {
            return sColumnName + " = @" + sColumnName;
        }
        //****************************************************************************************************************************
        private string ColumnEquals(string sColumnName,
                                    string sColumnValueName)
        {
            return sColumnName + " = @" + sColumnValueName;
        }
        //****************************************************************************************************************************
        private string ColumnLike(string sColumnName,
                                  string sValue)
        {
            return sColumnName + " Like '" + sValue + "%'";
        }
        //****************************************************************************************************************************
        private int ExecuteUpdateStatement(SqlCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = cmd;
            try
            {
                return da.UpdateCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
            }
            return U.Exception;
        }
        //****************************************************************************************************************************
        private int ExecuteDeleteStatement(SqlCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.DeleteCommand = cmd;
            try
            {
                return da.DeleteCommand.ExecuteNonQuery();
            }
            catch (SqlException expsql)
            {
                sqlConnection = null;
                m_sErrorMessage += expsql.ToString();
                m_bFoundFatalError = true;
                return U.Exception;
            }
            catch (Exception ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
                return U.Exception;
            }
        }
        //****************************************************************************************************************************
        public int DeleteFromTable(SqlTransaction txn,
                                   string sTableName,
                                   string sColumnName,
                                   int iColumnValue)
        {
            string sCommand = "Delete from " + sTableName + " Where " + ColumnEquals(sColumnName);
            SqlCommand cmd = new SqlCommand(sCommand, sqlConnection);
            cmd.Transaction = txn;
            cmd.Parameters.Add(new SqlParameter("@" + sColumnName, iColumnValue));
            return ExecuteDeleteStatement(cmd);
        }
        //****************************************************************************************************************************
        public int DeleteFromTable(string sTableID)
        {
            string sCommand = "Delete from " + sTableID;
            SqlCommand cmd = new SqlCommand(sCommand, sqlConnection);
            return ExecuteDeleteStatement(cmd);
        }
        //****************************************************************************************************************************
        public int DeleteFromTable(string sTableID,
                                   string sColumnName,
                                   int iColumnValue)
        {
            string sCommand = "Delete from " + sTableID + " Where " + ColumnEquals(sColumnName);
            SqlCommand cmd = new SqlCommand(sCommand, sqlConnection);
            cmd.Parameters.Add(new SqlParameter("@" + sColumnName, iColumnValue));
            return ExecuteDeleteStatement(cmd);
        }
        //****************************************************************************************************************************
        public int DeleteFromTable(string sTableID,
                                   string sColumnName,
                                   int iColumnValue,
                                   string sColumnName2,
                                   int iColumnValue2)
        {
            string sCommand = "Delete from " + sTableID + " Where " + ColumnEquals(sColumnName);
            SqlCommand cmd = new SqlCommand(sCommand, sqlConnection);
            cmd.Parameters.Add(new SqlParameter("@" + sColumnName, iColumnValue));
            cmd.Parameters.Add(new SqlParameter("@" + sColumnName2, iColumnValue2));
            return ExecuteDeleteStatement(cmd);
        }
        //****************************************************************************************************************************
        public bool ExecuteSelectStatement(DataSet ds,
                                           SqlDataAdapter da)
        {
            try
            {
                da.Fill(ds);
            }
            catch (ArgumentException ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
            }
            catch (Exception ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
            }
            return true;
        }
        //****************************************************************************************************************************
        public bool ExecuteSelectStatement(DataSet ds,
                                           SqlCommand cmd,
                                           params string[] sTables)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            int iTableNum = 0;
            foreach (string sTable in sTables)
            {
                string sTableName = "Table" + iTableNum.ToStringSpacesWhenZero();
                da.TableMappings.Add(sTableName, sTable);
                iTableNum++;
            }
            ExecuteSelectStatement(ds, da);
            return true;
        }
        //****************************************************************************************************************************
        public int ExecuteSelectStatement(DataTable tbl,
                                          SqlCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            try
            {
                return da.Fill(tbl);
            }
            catch (ArgumentException ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
            }
            catch (Exception ex)
            {
                m_sErrorMessage += ex.Message.ToString();
                m_bFoundFatalError = true;
            }
            return 0;
        }
        //****************************************************************************************************************************
        public bool SelectAllWhere(DataTable tbl, 
                                   string sTableName,
                                   string sWhere)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from " + sTableName + sWhere;
            cmd.Connection = sqlConnection;
            ExecuteSelectStatement(tbl, cmd);
            return true;
        }
        //****************************************************************************************************************************
        public bool selectall(DataTable tbl,
                              string    sTableName,
                              string    sOrderBy,
                       params NameValuePair[] ColumnNameValuePairs)
        {
            SqlCommand cmd = new SqlCommand();
            string sWhere = "";
            int iParamNum = 0;
            foreach (NameValuePair ColumnNameValuePair in ColumnNameValuePairs)
            {
                string sValue = ColumnNameValuePair.Value();
//                if (sValue.Length != 0)
                {
                    if (iParamNum == 0)
                        sWhere += " Where ";
                    else
                        sWhere += " And ";
                    sWhere += ColumnEquals(ColumnNameValuePair.Name());
                    cmd.Parameters.Add(new SqlParameter("@" + ColumnNameValuePair.Name(), sValue));
                    iParamNum++;
                }
            }
            cmd.CommandText = "Select * from " + sTableName + sWhere + sOrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        private int NumRecordsToAdd(DataTable tbl)
        {
            int iNumRecsToAdd = 0;
            DataViewRowState dvRowState = DataViewRowState.Added;
            foreach (DataRow row in tbl.Select("", "", dvRowState))
            {
                iNumRecsToAdd++;
            }
            return iNumRecsToAdd;
        }
        //****************************************************************************************************************************
        private int NumRecordsToDelete(DataTable tbl)
        {
            int iNumRecsToDelete = 0;
            DataViewRowState dvRowState = DataViewRowState.Deleted;
            foreach (DataRow row in tbl.Select("", "", dvRowState))
            {
                iNumRecsToDelete++;
            }
            return iNumRecsToDelete;
        }
        //****************************************************************************************************************************
        public bool AddUpdateValue(ref string sUpdateString,
                                   ref string sWhereString,
                                   ref SqlParameterCollection Parms,
                                       string sStringName,
                                       SqlDbType Dbtype,
                                       int iLength)
        {
            if (sUpdateString.Length > 0)
                sUpdateString += ",";
            sUpdateString += sStringName;
            sUpdateString += "=@";
            sUpdateString += sStringName;
            Parms.Add(sStringName, Dbtype, iLength, sStringName);
            string sOrigStringName = "Orig" + sStringName;
            SqlParameter Parem = Parms.Add(sOrigStringName, Dbtype, iLength, sStringName);
            Parem.SourceVersion = DataRowVersion.Original;
            if (sWhereString.Length != 0)
                sWhereString += " and ";
            sWhereString += sStringName + " = @" + sOrigStringName;
            return true;
        }
        //****************************************************************************************************************************
        public bool AddUpdateValueNoCheckOrig(ref string sUpdateString,
                                   ref SqlParameterCollection Parms,
                                       string sStringName,
                                       SqlDbType Dbtype,
                                       int iLength)
        {
            if (sUpdateString.Length > 0)
                sUpdateString += ",";
            sUpdateString += sStringName;
            sUpdateString += "=@";
            sUpdateString += sStringName;
            Parms.Add(sStringName, Dbtype, iLength, sStringName);
            return true;
        }
        //****************************************************************************************************************************
        public string selectaUsingOrCommand(string sTableName,
                                            string sOrderBy,
                                            params NameValuePair[] ColumnNameValuePairs)
        {
            SqlCommand cmd = new SqlCommand();
            string sWhere = "";
            for (int i = 0; i < ColumnNameValuePairs.Length; i++)
            {
                sWhere += AddOneOfTwoStrings(sWhere.Length, " Where ", " or ");
                sWhere += AddValue(ColumnNameValuePairs[i]);
            }
            return "Select * from " + sTableName + sWhere + sOrderBy;
        }
        //****************************************************************************************************************************
        private string AddOneOfTwoStrings(int iStringLength,
                                                 string sOne,
                                                 string sTwo)
        {
            if (iStringLength == 0)
            {
                return sOne;
            }
            else
            {
                return sTwo;
            }
        }
        //****************************************************************************************************************************
        private string AddValue(NameValuePair nameValuePair)
        {
            object value = nameValuePair.ObjectValue();
            string sReturnString = nameValuePair.Name() + "=";
            if (value.GetType().ToString() == "System.String")
            {
                sReturnString += "'" + value.ToString() + "'";
            }
            else
            {
                sReturnString += value.ToString();
            }
            return sReturnString;
        }
        //****************************************************************************************************************************
        private string SelectAllWhereString(string sTableName,
                                            string sColumnName)
        {
            return "Select * from " + sTableName +
                  " Where " + ColumnEquals(sColumnName);
        }
        //****************************************************************************************************************************
        private string SelectIdentityQuery(string sIdentityField)
        {
            return "Select @@Identity as " + sIdentityField + ";";
        }
        //****************************************************************************************************************************
        private string NameOrderByStatement()
        {
            return " order by LastName,FirstName,MiddleName,Suffix,Prefix;";
        }
        //****************************************************************************************************************************
        private void SearchStartingWith(DataTable tbl,
                                        string sTableName,
                                        string sLastNameCol,
                                        string sLastName)
        {
            string sWhereStatement = " where " + sLastNameCol + " like '" + sLastName + "%'" + NameOrderByStatement();
            SqlCommand cmd = new SqlCommand("Select * From " + sTableName + " " + sWhereStatement, sqlConnection);
            ExecuteSelectStatement(tbl, cmd);
        }
        //****************************************************************************************************************************
        private void AlternativeFirstNames(DataTable tbl,
                                           string sTableName,
                                           DataTable FirstNameAlternativeSpellings_tbl,
                                           string sFirstNameCol,
                                           string sLastNameCol,
                                           string sLastName)
        {
            foreach (DataRow row in FirstNameAlternativeSpellings_tbl.Rows)
            {
                selectall(tbl, sTableName, NameOrderByStatement(), new NameValuePair(sLastNameCol, sLastName),
                                                               new NameValuePair(sFirstNameCol, row[U.AlternativeSpelling_Col].ToString()));
            }
        }
        //****************************************************************************************************************************
        private void PartialWithOrWithoutMiddleName(DataTable tbl,
                                   string sTableName,
                                   string sFirstNameCol,
                                   string sLastNameCol,
                                   string sMiddleNameCol,
                                   string sLastName,
                                   string sFirstName,
                                   string sMiddleName,
                                   string sSuffix,
                                   string sPrefix)
        {
            if (sMiddleName.Length == 0)
            {
                selectall(tbl, sTableName, NameOrderByStatement(), new NameValuePair(sLastNameCol, sLastName),
                                                               new NameValuePair(sFirstNameCol, sFirstName),
                                                               new NameValuePair(U.Suffix_col, sSuffix),
                                                               new NameValuePair(U.Prefix_col, sPrefix));
            }
            else
            {
                selectall(tbl, sTableName, NameOrderByStatement(), new NameValuePair(sLastNameCol, sLastName),
                                                               new NameValuePair(sFirstNameCol, sFirstName),
                                                               new NameValuePair(sMiddleNameCol, sMiddleName),
                                                               new NameValuePair(U.Suffix_col, sSuffix),
                                                               new NameValuePair(U.Prefix_col, sPrefix));
            }
        }
        //****************************************************************************************************************************
        private bool IsAlreadyInList(string sName,
                                     ArrayList myList)
        {
            int iNumNames = myList.Count;
            for (int i=0;i < iNumNames;++i)
            {
                if (myList[i].ToString() == sName)
                    return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        public bool UpdateTableValues(SqlTransaction txn,
                                      DataTable      VitalRecord_tbl,
                                      string         sTableName,
                                      string         sKeyColumn,
                                      string         sColumnName1,
                                      string         sColumnName2,
                                      string         sColumnName3)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand Update_cmd = new SqlCommand();
            SqlParameterCollection Parms = Update_cmd.Parameters;

            string sUpdateString = "";
            string sWhereString = " where " + ColumnEquals(sKeyColumn);
            Parms.Add(sKeyColumn, SqlDbType.Int, 0, sKeyColumn);
            AddUpdateValue(ref sUpdateString, ref sWhereString, ref Parms, sColumnName1, SqlDbType.Int, 0);
            if (sColumnName2.Length > 0)
            {
                AddUpdateValue(ref sUpdateString, ref sWhereString, ref Parms, sColumnName2, SqlDbType.Int, 0);
                if (sColumnName3.Length > 0)
                    AddUpdateValue(ref sUpdateString, ref sWhereString, ref Parms, sColumnName3, SqlDbType.Int, 0);
            }
            Update_cmd.CommandText = "Update [" + sTableName + "] set " + sUpdateString + sWhereString + ";";
            Update_cmd.Connection = sqlConnection;
            Update_cmd.Transaction = txn;
            da.UpdateCommand = Update_cmd;
            return (Update(da, VitalRecord_tbl) > 0);
        }
        //****************************************************************************************************************************
        public bool UpdateTableValues(SqlTransaction txn,
                                      DataTable VitalRecord_tbl,
                                      string sTableName,
                                      string sKeyColumn,
                                      string sColumnName1,
                                      string sColumnName2)
        {
            return UpdateTableValues(txn, VitalRecord_tbl, sTableName, sKeyColumn, sColumnName1, sColumnName2, "");
        }
        //****************************************************************************************************************************
        public bool UpdateTableValues(SqlTransaction txn,
                                      DataTable VitalRecord_tbl,
                                      string sTableName,
                                      string sKeyColumn,
                                      string sColumnName1)
        {
            return UpdateTableValues(txn, VitalRecord_tbl, sTableName, sKeyColumn, sColumnName1, "", "");
        }
        //****************************************************************************************************************************
        public bool UpdateValue(DataTable tbl,
                         string sTable,
                         string sID_col,
                         string sValueCol,
                         SqlDbType DbType,
                         int DbLen)
        {
            SqlCommand Update_cmd = new SqlCommand();
            SqlParameterCollection Parms = Update_cmd.Parameters;
            Parms.Add(sID_col, SqlDbType.Int, 0, sID_col);
            string sUpdateString = "";
            string sWhereString = " where " + ColumnEquals(sID_col);
            AddUpdateValue(ref sUpdateString, ref sWhereString, ref Parms, sValueCol, DbType, DbLen);
            Update_cmd.CommandText = "Update [" + sTable + "] set " + sUpdateString + sWhereString + ";";
            Update_cmd.Connection = sqlConnection;
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = Update_cmd;
            SqlTransaction txn = sqlConnection.BeginTransaction();
            da.UpdateCommand.Transaction = txn;
            if (Update(da, tbl) > 0)
                txn.Commit();
            else
                txn.Rollback();
            return true;
        }
        //****************************************************************************************************************************
        private bool ThereAreDeletedRows(DataTable tbl)
        {
            bool bFoundPersonsToDelete = false;
            DataViewRowState dvRowState = DataViewRowState.Deleted;
            foreach (DataRow row in tbl.Select("", "", dvRowState))
            {
                bFoundPersonsToDelete = true;
            }
            return bFoundPersonsToDelete;
        }
        //****************************************************************************************************************************
        public string GetSQLErrorMessage()
        {
            string sErrorMessage = m_sErrorMessage;
            m_sErrorMessage = U.NoSQLError;
            m_bFoundFatalError = false;
            return sErrorMessage;
        }
        //****************************************************************************************************************************
    }
}
