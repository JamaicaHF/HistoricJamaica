using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;

namespace SQL_Library
{
    public static partial class SQL
    {
        private static SqlConnection sqlConnection;
        private const string sSecurity = "Integrated Security=SSPI";
        private static int m_iPersonLevel = 0;
        public static string m_sDataDirectory;
        private static bool m_bFullDatabase = false;
        private static string m_sDatabaseName;

        //****************************************************************************************************************************
        public static void OpenConnection(string sDatabaseName,
                                          string sServer,
                                          string sDataDirectory,
                                          bool   bFullDatabase)
        {
            try
            {
                string connectionStr = sServer + sDatabaseName + sSecurity;
                sqlConnection = new SqlConnection(connectionStr);

                m_sDatabaseName = sDatabaseName;
                sqlConnection.Open();
                m_sDataDirectory = sDataDirectory;
                m_bFullDatabase = bFullDatabase;
                return;
            }
            catch (SqlException expsql)
            {
                sqlConnection = null;
                errors.SQLShowError(expsql.ToString());
            }
        }
        //****************************************************************************************************************************
		public static void CloseConnection()
		{
            if (sqlConnection != null)
            {
                sqlConnection.Close();
                sqlConnection = null;
            }
        }
        //****************************************************************************************************************************
        private static string ColumnLikexx(string sColumnName,
                                         string sValue)
        {
            string newValue = RemoveWildCardChars(sValue);
            return sColumnName + " Like '%" + newValue + "%'";
        }
        //****************************************************************************************************************************
        private static string ColumnLike(string sColumnName,
                                         string sValue)
        {
            string newValue = RemoveWildCardChars(sValue);
            return sColumnName + " Like '" + newValue + "%'";
        }
        //****************************************************************************************************************************
        public static bool UpdateAllNoKeysDA(DataTable tbl,
                                         string sTableName,
                                         ArrayList FieldsModified)
        {
            SqlTransaction txn = null;
            return UpdateAllNoKeysDA(txn, tbl, sTableName, FieldsModified);
        }
        //****************************************************************************************************************************
        public static bool UpdateAllNoKeysDA(SqlTransaction txn,
                                         DataTable tbl,
                                         string sTableName,
                                         ArrayList FieldsModified)
        {
            if (tbl.Rows.Count == 0 || FieldsModified.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = UpdateCommandNoKeys(txn, tbl.Columns, sTableName, FieldsModified);
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        public static bool UpdateWithDA(DataTable tbl,
                                        string sTableName,
                                        string sKeyColumn,
                                        ArrayList FieldsModified)
        {
            SqlTransaction txn = null;
            return UpdateWithDA(txn, tbl, sTableName, sKeyColumn, FieldsModified);
        }
        //****************************************************************************************************************************
        public static bool UpdateWithDA(DataTable tbl,
                                        string sTableName,
                                        string[] sKeyColumns,
                                        ArrayList FieldsModified)
        {
            SqlTransaction txn = null;
            return UpdateWithDA(txn, tbl, sTableName, sKeyColumns, FieldsModified);
        }
        //****************************************************************************************************************************
        public static bool UpdateWithDA(SqlTransaction txn,
                                         DataTable tbl,
                                         string sTableName,
                                         string sKeyColumn,
                                         ArrayList FieldsModified)
        {
            string[] sKeyColumns = new string[] { sKeyColumn };
            return UpdateWithDA(txn, tbl, sTableName, sKeyColumns, FieldsModified);
        }
        //****************************************************************************************************************************
        public static bool UpdateWithDA(SqlTransaction txn,
                                         DataTable tbl,
                                         string sTableName,
                                         string[] sKeyColumn,
                                         ArrayList FieldsModified)
        {
            if (tbl.Rows.Count == 0 || FieldsModified.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = UpdateCommand(txn, tbl.Columns, sTableName, sKeyColumn, FieldsModified);
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        private static bool UpdateInsertWithDA(DataTable  tbl,
                                               SqlCommand UpdateCommand,
                                               SqlCommand InsertCommand)
        {
            if (tbl.Rows.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = UpdateCommand;
            da.InsertCommand = InsertCommand;
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        public static bool InsertWithDA(DataTable tbl, 
                                        SqlCommand Insert_cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.InsertCommand = Insert_cmd;
            return Update(da, tbl) > 0;
        }
        //****************************************************************************************************************************
        private static bool DeleteWithDA(DataTable tbl,
                                         SqlCommand DeleteCommand)
        {
            if (tbl.Rows.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.DeleteCommand = DeleteCommand;
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        private static bool DeleteUpdateWithDA(DataTable tbl,
                                               SqlCommand DeleteCommand,
                                               SqlCommand UpdateCommand)
        {
            if (tbl.Rows.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.DeleteCommand = DeleteCommand;
            da.UpdateCommand = UpdateCommand;
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        private static bool UpdateWithDA(DataTable tbl,
                                         SqlCommand UpdateCommand)
        {
            if (tbl.Rows.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = UpdateCommand;
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        private static bool DeleteUpdateInsertWithDA(DataTable tbl,
                                               SqlCommand DeleteCommand,
                                               SqlCommand UpdateCommand,
                                               SqlCommand InsertCommand)
        {
            if (tbl.Rows.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.DeleteCommand = DeleteCommand;
            da.UpdateCommand = UpdateCommand;
            da.InsertCommand = InsertCommand;
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        private static bool DeleteInsertWithDA(DataTable tbl,
                                               SqlCommand DeleteCommand,
                                               SqlCommand InsertCommand)
        {
            if (tbl.Rows.Count == 0)
                return true;
            SqlDataAdapter da = new SqlDataAdapter();
            da.DeleteCommand = DeleteCommand;
            da.InsertCommand = InsertCommand;
            return (Update(da, tbl) > 0);
        }
        //****************************************************************************************************************************
        private static int ValueLength(DataColumnCollection columns,
                                       string fieldValue)
        {
            int maxLength = columns[fieldValue].MaxLength;
            if (columns[fieldValue].DataType.ToString() == "System.Int32")
                maxLength = 0;
            else if (columns[fieldValue].DataType.ToString() == "System.Char")
                maxLength = 1;
            return maxLength;
        }
        //****************************************************************************************************************************
        public static void UpdateWithParms(string tableName,
                                            NameValuePair keyValuePair,
                                            params NameValuePair[] valuePairs)
        {
            NameValuePair[] whereValues = { keyValuePair };
            UpdateWithParms(tableName, whereValues, valuePairs);
        }
        //****************************************************************************************************************************
        private static void UpdateWithParms(string tableName,
                                            NameValuePair[] keyValuePair,
                                            params NameValuePair[] valuePairs)
        {
            SqlCommand cmd = new SqlCommand();
            string whereString = whereStringWithParms(cmd.Parameters, keyValuePair);
            string updateString = "";
            foreach (NameValuePair nameValuePair in valuePairs)
            {
                updateString += AddOneOfTwoStrings(updateString.Length, " set ", ",");
                updateString += ColumnEquals(nameValuePair.Name());
                cmd.Parameters.Add(SqlParm(nameValuePair.Name(), nameValuePair.Value()));
            }
            cmd.CommandText = UpdateTableString(tableName) + updateString + whereString;
            cmd.Connection = sqlConnection;
            ExecuteSQLNonQueryCommand(cmd);
        }
        //****************************************************************************************************************************
        public static ArrayList ColumnList(params string[] columns)
        {
            ArrayList columnList = new ArrayList(columns);
            return columnList;
        }
        //****************************************************************************************************************************
        private static SqlCommand UpdateCommand(SqlTransaction txn,
                                         DataColumnCollection columns,
                                         string sTableName,
                                         string sKeyColumn,
                                         ArrayList FieldsModified)
        {
            string[] keyColumns = (new string[] { sKeyColumn });
            return UpdateCommand(txn, columns, sTableName, keyColumns, FieldsModified);
        }
        //****************************************************************************************************************************
        private static SqlCommand UpdateCommandNoKeys(SqlTransaction txn,
                                         DataColumnCollection columns,
                                         string sTableName,
                                         ArrayList FieldsModified)
        {
            SqlCommand Update_cmd = new SqlCommand();
            SqlParameterCollection Parms = Update_cmd.Parameters;
            string sUpdateString = "";
            string sWhereString = "";
            foreach (string changedColumn in FieldsModified)
            {
                int valueLength = ValueLength(columns, changedColumn);
                AddUpdateValue(changedColumn, valueLength, ref sUpdateString, ref sWhereString, ref Parms);
            }
            Update_cmd.CommandText = UpdateTableString(sTableName) + sUpdateString + sWhereString + ";";
            Update_cmd.UpdatedRowSource = UpdateRowSource.Both;
            Update_cmd.Connection = sqlConnection;
            if (txn != null)
                Update_cmd.Transaction = txn;
            return Update_cmd;
        }
        //****************************************************************************************************************************
        private static SqlCommand UpdateCommand(SqlTransaction txn,
                                         DataColumnCollection columns,
                                         string sTableName,
                                         string[] keyColumns,
                                         ArrayList FieldsModified)
        {
            SqlCommand Update_cmd = new SqlCommand();
            SqlParameterCollection Parms = Update_cmd.Parameters;
            string sUpdateString = "";
            string sWhereString = "";
            if (FieldsModified.Count == 0 || keyColumns[0] == FieldsModified[0].ToString())
            {
                sWhereString = "";
            }
            else
            {
                foreach (string keyColumn in keyColumns)
                {
                    if (!String.IsNullOrEmpty(keyColumn))
                    {
                        if (String.IsNullOrEmpty(sWhereString))
                        {
                            sWhereString = WhereString(keyColumns);
                        }
                        AddParm(ref Parms, columns, keyColumn);
                    }
                }
            }
            foreach (string changedColumn in FieldsModified)
            {
                int valueLength = ValueLength(columns, changedColumn);
                AddUpdateValue(changedColumn, valueLength, ref sUpdateString, ref sWhereString, ref Parms);
            }
            Update_cmd.CommandText = UpdateTableString(sTableName) + sUpdateString + sWhereString + ";";
            Update_cmd.UpdatedRowSource = UpdateRowSource.Both;
            Update_cmd.Connection = sqlConnection;
            if (txn != null)
                Update_cmd.Transaction = txn;
            return Update_cmd;
        }
        //****************************************************************************************************************************
        private static void AddParm(ref SqlParameterCollection Parms,
                                    DataColumnCollection columns,
                                    string fieldValue)
        {
            SqlDbType dbType = SqlDbType.VarChar;
            int maxLength = columns[fieldValue].MaxLength;
            if (columns[fieldValue].DataType.ToString() == "System.Int32")
            {
                dbType = SqlDbType.Int;
                maxLength = 0;
            }
            else if (columns[fieldValue].DataType.ToString() == "System.Char")
            {
                dbType = SqlDbType.Char;
                maxLength = 1;
            }
            Parms.Add(fieldValue, dbType, maxLength, fieldValue);
        }
        //****************************************************************************************************************************
        private static int ExecuteSQLNonQueryCommand(SqlCommand cmd)
        {
            try
            {
                int iNumValues = cmd.ExecuteNonQuery();
                return iNumValues;
            }
            catch (SqlException expsql)
            {
                HistoricJamaicaException ex = new HistoricJamaicaException(expsql.ToString());
                return 0;
            }
            catch (Exception ex)
            {
                HistoricJamaicaException historicJamaicaException = new HistoricJamaicaException(ex.Message.ToString());
                throw historicJamaicaException;
            }
        }
        //****************************************************************************************************************************
        private static void ExecuteSQLNonQueryCommand(string sSQLString)
        {
            SqlCommand sqlcommand = new SqlCommand(sSQLString, sqlConnection);
            try
            {
                sqlcommand.ExecuteNonQuery();
            }
            catch (SqlException expsql)
            {
                sqlConnection = null;
                HistoricJamaicaException historicJamaicaException = new HistoricJamaicaException(expsql.ToString());
                throw historicJamaicaException;
            }
            catch (Exception ex)
            {
                HistoricJamaicaException historicJamaicaException = new HistoricJamaicaException(ex.Message.ToString());
                throw historicJamaicaException;
            }
        }
        //****************************************************************************************************************************
        public static int Update(SqlDataAdapter da,
                                 DataTable      tbl)
        {
            try
            {
                int numUpdated = da.Update(tbl);
                return numUpdated;
            }
            catch (DBConcurrencyException dbcx)
            {
                string xxxx = dbcx.ToString();
                return 9999;
            }
            catch (Exception ex)
            {
                throw new HistoricJamaicaException(ex.Message.ToString());
            }
        }
        //****************************************************************************************************************************
        private static SqlParameter SqlParm(string name,
                                            object value)
        {
            return new SqlParameter("@" + name, value.ToString());
        }
        //****************************************************************************************************************************
        private static string ColumnEquals(string sColumnName)
        {
            return sColumnName + " = @" + sColumnName;
        }
        //****************************************************************************************************************************
        private static string ColumnEquals(string sColumnName,
                                           string sColumnValueName)
        {
            return sColumnName + " = @" + sColumnValueName;
        }
        //****************************************************************************************************************************
        private static void AddParameter(SqlCommand cmd,
                                           string parameterCol,
                                           string parameterValue)
        {
            cmd.Parameters.Add(new SqlParameter("@" + parameterCol, parameterValue));
        }
        //****************************************************************************************************************************
        private static SqlDbType DbType(int fieldLength)
        {
            if (fieldLength == 0)
                return SqlDbType.Int;
            else if (fieldLength == 1)
                return SqlDbType.Char;
            else
                return SqlDbType.VarChar;
        }
        //****************************************************************************************************************************
        private static void AddUpdateValue(string FieldModified,
                                           int maxLength,
                                           ref string sUpdateString,
                                           ref string sWhereString,
                                           ref SqlParameterCollection Parms)
        {
            SqlDbType dbType = DbType(maxLength);
            sUpdateString += AddOneOfTwoStrings(sUpdateString.Length, " Set ", ",");
            sUpdateString += FieldModified;
            sUpdateString += "=@";
            sUpdateString += FieldModified;
            Parms.Add(FieldModified, DbType(maxLength), maxLength, FieldModified);
            string origStringName = "Orig" + FieldModified;
            SqlParameter parem = Parms.Add(origStringName, dbType, maxLength, FieldModified);
            parem.SourceVersion = DataRowVersion.Original;
            sWhereString += AddOneOfTwoStrings(sWhereString.Length, " where ", " and ");
            sWhereString += FieldModified + " = @" + origStringName;
        }
        //****************************************************************************************************************************
        private static void AddUpdateValue(ref string sUpdateString,
                                           ref string sWhereString,
                                           ref SqlParameterCollection Parms,
                                           string sStringName,
                                           SqlDbType dbtype,
                                           int iLength)
        {
            if (dbtype == SqlDbType.Char)
                iLength = 1;
            else if (dbtype == SqlDbType.Int)
                iLength = 0;
            AddUpdateValue(sStringName, iLength, ref sUpdateString, ref sWhereString, ref Parms);
        }
        //****************************************************************************************************************************
        private static string InsertTableString(string tableName)
        {
            return "Insert into [" + tableName + "]";
        }
        //****************************************************************************************************************************
        public static int DeleteWithParms(string tableName,
                                          params NameValuePair[] nameValuePairs)
        {
            SqlTransaction txn = null;
            return DeleteWithParms(txn, tableName, nameValuePairs);
        }
        //****************************************************************************************************************************
        private static int DeleteWithParms(SqlTransaction txn,
                                           string sTable,
                                           params NameValuePair[] nameValuePairs)
        {
            SqlCommand cmd = new SqlCommand();
            string whereString = whereStringWithParms(cmd.Parameters, nameValuePairs);
            cmd.CommandText = DeleteFromString(sTable) + whereString;
            cmd.Connection = sqlConnection;
            cmd.Transaction = txn;
            int iNumRecordsDeleted = ExecuteSQLNonQueryCommand(cmd);
            return iNumRecordsDeleted;
        }
        //****************************************************************************************************************************
        private static SqlCommand DeleteCommandWithDAOneParm(SqlTransaction txn, 
                                                string tableName,
                                                NameValuePair parmField,
                                                params string[] deleteColumns)
        {
            SqlCommand Delete_cmd = DeleteCommandWithDA(txn, tableName, deleteColumns);
            Delete_cmd.CommandText += " and " + parmField.Name() + " = " + parmField.Value();
            //Delete_cmd.Parameters.AddWithValue(parmField.Name(), parmField.Value().ToInt());
            return Delete_cmd;
        }
        //****************************************************************************************************************************
        private static SqlCommand DeleteCommandWithDA(SqlTransaction txn,
                                                string tableName,
                                                params string[] deleteColumns)
        {
            SqlCommand Delete_cmd = new SqlCommand();
            string sWhereString = WhereString(deleteColumns);
            foreach (string changedColumn in deleteColumns)
            {
                Delete_cmd.Parameters.Add(changedColumn, SqlDbType.Int, 0, changedColumn);
            }
            Delete_cmd.CommandText = DeleteFromString(tableName) + sWhereString;
            Delete_cmd.Connection = sqlConnection;
            Delete_cmd.Transaction = txn;
            return Delete_cmd;
        }
        //****************************************************************************************************************************
        private static SqlCommand DeleteCommandWithDA(SqlTransaction txn,
                                                DataTable tbl,
                                                string tableName,
                                                params string[] deleteColumns)
        {
            SqlCommand Delete_cmd = new SqlCommand();
            string sWhereString = WhereString(deleteColumns);
            foreach (string changedColumn in deleteColumns)
            {
                if (tbl.Columns[changedColumn].DataType.Name == "Int32")
                {
                    Delete_cmd.Parameters.Add(changedColumn, SqlDbType.Int, 0, changedColumn);
                }
                else
                {
                    Delete_cmd.Parameters.Add(changedColumn, SqlDbType.NVarChar, 0, changedColumn);
                }
            }
            Delete_cmd.CommandText = DeleteFromString(tableName) + sWhereString;
            Delete_cmd.Connection = sqlConnection;
            Delete_cmd.Transaction = txn;
            return Delete_cmd;
        }
        //****************************************************************************************************************************
        private static string DeleteFromString(string tableName)
        {
            return "Delete from [" + tableName + "]";
        }
        //****************************************************************************************************************************
        private static void ExecuteUpdateStatement(SqlCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = cmd;
            int iNumRecordUpdated = 0;
            try
            {
                iNumRecordUpdated = da.UpdateCommand.ExecuteNonQuery();
            }
            catch (SqlException expsql)
            {
                sqlConnection = null;
                HistoricJamaicaException historicJamaicaException = new HistoricJamaicaException(expsql.ToString());
                throw historicJamaicaException;
            }
            catch (Exception ex)
            {
                HistoricJamaicaException historicJamaicaException = new HistoricJamaicaException(ex.Message.ToString());
                throw historicJamaicaException;
            }
            if (iNumRecordUpdated == 0) //  must be after the catch because the Exception catch will catch this exception;
            {
                HistoricJamaicaException historicJamaicaException = new HistoricJamaicaException(ErrorCodes.eUpdateUnsuccessful);
                throw historicJamaicaException;
            }
        }
        //****************************************************************************************************************************
        private static string AddValue(NameValuePair nameValuePair)
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
        private static string whereStringWithParms(SqlParameterCollection Parms,
                                                   NameValuePair[] keyValuePairs)
        {
            string whereString = "";
            foreach (NameValuePair nameValuePair in keyValuePairs)
            {
                whereString += AddOneOfTwoStrings(whereString.Length, " where ", " and ");
                whereString += ColumnEquals(nameValuePair.Name());
                Parms.Add(SqlParm(nameValuePair.Name(), nameValuePair.Value()));
            }
            return whereString;
        }
        //****************************************************************************************************************************
        private static string WhereString(bool useOrNotAnd,
                                          params NameValuePair[] nameValuePairs)
        {
            string conjunction = " and ";
            if (useOrNotAnd)
            {
                conjunction = " or ";
            }
            string whereString = "";
            foreach (NameValuePair NameValuePair in nameValuePairs)
            {
                whereString += AddOneOfTwoStrings(whereString.Length, " Where ", conjunction);
                whereString += AddValue(NameValuePair);
            }
            return whereString;
        }
        //****************************************************************************************************************************
        private static string WhereString(params NameValuePair[] nameValuePairs)
        {
            return WhereString(false, nameValuePairs);
        }
        //****************************************************************************************************************************
        private static string WhereStringOrSameKey(string keyString,
                                                   params string[] columnNames)
        {
            string whereString = " where ";
            whereString += ColumnEquals(keyString);
            foreach (string column in columnNames)
            {
                whereString += AddOneOfTwoStrings(whereString.Length, " Where ", " or ");
                whereString += ColumnEquals(column, keyString);
            }
            return whereString;
        }
        //****************************************************************************************************************************
        private static string WhereString(params string[] columnNames)
        {
            return ColumnEqualsNames(" Where ", " and ", columnNames);
        }
        //****************************************************************************************************************************
        private static string AdditionalWhereNotEqual(NameValuePair nameValuePair)
        {
            return " and " + nameValuePair.Name() + " <> " + nameValuePair.Value();
        }
        //****************************************************************************************************************************
        private static string WhereNotEqual(NameValuePair nameValuePair)
        {
            string value = nameValuePair.Value();
            if (value.GetType().ToString() == "System.String")
            {
                value = "'" + value.ToString() + "'";
            }
            return " where " + nameValuePair.Name() + " <> " + value;
        }
        //****************************************************************************************************************************
        private static string UpdateString(string tableName,
                                           params string[] columnNames)
        {
            string updateString = ColumnEqualsNames(" Set ", ",", columnNames);
            return UpdateTableString(tableName) + updateString;
        }
        //****************************************************************************************************************************
        private static string UpdateTableString(string tableName)
        {
            return "Update [" + tableName + "]";
        }
        //****************************************************************************************************************************
        private static string ColumnEqualsNames(string sOne,
                                                string sTwo,
                                                params string[] columnNames)
        {
            string sReturnString = "";
            foreach (string columnName in columnNames)
            {
                sReturnString += AddOneOfTwoStrings(sReturnString.Length, sOne, sTwo);
                sReturnString += ColumnEquals(columnName);
            }
            return sReturnString;
        }
        //****************************************************************************************************************************
        private static SqlCommand DummySqlCommandCommand(SqlTransaction txn)
        {
            SqlCommand SqlCommand = new SqlCommand();
            SqlCommand.CommandText = ";";
            SqlCommand.Connection = sqlConnection;
            if (txn != null)
                SqlCommand.Transaction = txn;
            return SqlCommand;
        }
        //****************************************************************************************************************************
        public static SqlCommand InsertCommand(SqlTransaction txn, 
                                                DataTable tbl,
                                                string tableName,
                                                bool includeIdentity)
        {
            NameValuePair parmValue = new NameValuePair("Null",0);
            SqlCommand insertCommand = InsertCommand(txn, tbl, tableName, parmValue, includeIdentity);
            return insertCommand;
        }
        //****************************************************************************************************************************
        private static SqlCommand InsertCommand(DataTable tbl,
                                                string tableName,
                                                bool includeIdentity)
        {
            SqlTransaction txn = null;
            return InsertCommand(txn, tbl, tableName, includeIdentity);
        }
        //****************************************************************************************************************************
        private static SqlCommand InsertCommand(SqlTransaction txn, 
                                                DataTable tbl,
                                                string tableName,
                                                NameValuePair parmField,
                                                bool includeIdentity)
        {
            SqlCommand Insert_cmd = new SqlCommand();
            string insertString = "";
            string valuesString = "";
            for (int i = StartIndex(includeIdentity); i < tbl.Columns.Count; i++)
            {
                DataColumn column = tbl.Columns[i];
                insertString += AddOneOfTwoStrings(insertString.Length, "(", U.comma);
                valuesString += AddOneOfTwoStrings(valuesString.Length, ") values (", U.comma);
                insertString += column.ColumnName;
                valuesString += "@" + column.ColumnName;
                if (column.ColumnName == parmField.Name())
                    Insert_cmd.Parameters.AddWithValue(parmField.Name(), parmField.Value());
                else
                    Insert_cmd.Parameters.Add(column.ColumnName, DBType(column.DataType), column.MaxLength, column.ColumnName);
            }
            insertString = InsertTableString(tableName) + insertString + valuesString + ");";
            if (includeIdentity)
            {
                insertString += SelectIdentityQuery(tbl.Columns[0].ToString());
                Insert_cmd.UpdatedRowSource = UpdateRowSource.Both;
            }
            else
            {
                Insert_cmd.UpdatedRowSource = UpdateRowSource.None;
            }
            Insert_cmd.CommandText = insertString;
            Insert_cmd.Connection = sqlConnection;
            if (txn != null)
                Insert_cmd.Transaction = txn;
            return Insert_cmd;
        }
        //****************************************************************************************************************************
        private static string SelectIdentityQuery(string sIdentityField)
        {
            return "Select @@Identity as " + sIdentityField + ";";
        }
        //****************************************************************************************************************************
        private static int StartIndex(bool includeIdentity)
        {
            int startIndex = 0;
            if (includeIdentity)
                startIndex = 1;
            return startIndex;
        }
        //****************************************************************************************************************************
        private static string AddOneOfTwoStrings(int iStringLength,
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
        public static int GetNextPersonLevel()
        {
            m_iPersonLevel++;
            return m_iPersonLevel;
        }
        //****************************************************************************************************************************
        public static bool resetPersonLevel()
        {
            if (m_iPersonLevel > 0)
                m_iPersonLevel--;
            return true;
        }
        //****************************************************************************************************************************
        public static bool IsFullDatabase()
        {
            return m_bFullDatabase;
        }
        //****************************************************************************************************************************
        public static string DataDirectory()
        {
            return m_sDataDirectory;
        }
        //****************************************************************************************************************************
        public static string OrderBy(params string[] orderByColumns)
        {
            string orderByString = "";
            foreach (string orderByColumn in orderByColumns)
            {
                orderByString += AddOneOfTwoStrings(orderByString.Length, " Order By ", ",");
                orderByString += orderByColumn;
            }
            return orderByString;
        }
        //****************************************************************************************************************************
    }
}
