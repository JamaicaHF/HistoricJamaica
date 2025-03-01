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
        //****************************************************************************************************************************
        public static bool SelectAllUsingOr(DataTable tbl,
                                       string sTableName,
                                       string sOrderBy,
                                       params NameValuePair[] ColumnNameValuePairs)
        {
            return SelectAll(true, sTableName, sOrderBy, tbl, ColumnNameValuePairs);
        }
        //****************************************************************************************************************************
        public static int SelectCount(string sTableName, string id_col, int id)
        {
            DataTable tbl = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select Count(*) from [" + sTableName + "] where " + id_col + " = " + id.ToString();
            cmd.Connection = sqlConnection;
            ExecuteSelectStatement(tbl, cmd);
            if (tbl.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return tbl.Rows[0][0].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static DataTable SelectAll(string sTableName)
        {
            DataTable tbl = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName);
            cmd.Connection = sqlConnection;
            ExecuteSelectStatement(tbl, cmd);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable SelectAll(string sTableName,
                                          NameValuePair valuePair)
        {
            DataTable tbl = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName) + WhereString(valuePair);
            cmd.Connection = sqlConnection;
            ExecuteSelectStatement(tbl, cmd);
            return tbl;
        }
        //****************************************************************************************************************************
        public static bool SelectAll(string sTableName,
                                     DataTable tbl,
                                     params NameValuePair[] ColumnNameValuePairs)
        {
            return SelectAll(false, sTableName, U.NoOrderBy, tbl, ColumnNameValuePairs);
        }
        //****************************************************************************************************************************
        public static bool SelectAll(string sTableName,
                                     string sOrderBy,
                                     DataTable tbl,
                                     params NameValuePair[] ColumnNameValuePairs)
        {
            return SelectAll(false, sTableName, sOrderBy, tbl, ColumnNameValuePairs);
        }
        //****************************************************************************************************************************
        public static bool SelectAllJoin(string sTableName,
                                              string joinTableName,
                                              string joinType,
                                              string sOrderBy,
                                              DataTable tbl,
                                              string keyCol,
                                              params NameValuePair[] ColumnNameValuePairs)

        {
            SqlCommand cmd = new SqlCommand();
            var selectStatement = SelectAllFromString(sTableName);
            selectStatement += InnerJoin(sTableName, joinTableName, joinType, keyCol);
            cmd.CommandText = selectStatement + WhereString(false, ColumnNameValuePairs) + sOrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        private static string InnerJoin(string sTableName, string joinTableName, string joinType, string keyCol)
        {
            return " " + joinType + " Join " + joinTableName + " on " + sTableName + "." + keyCol + " = " + joinTableName + "." + keyCol;
        }
        //****************************************************************************************************************************
        public static bool SelectAllNotEqual(string sTableName,
                                     string sOrderBy,
                                     DataTable tbl,
                                     params NameValuePair[] ColumnNameValuePairs)
        {
            SqlCommand cmd = new SqlCommand();
            string whereString = WhereNotEqual(ColumnNameValuePairs[0]);
            cmd.CommandText = SelectAllFromString(sTableName) + whereString + sOrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        public static int GetMultipleVitalRecords(DataTable father_tbl, string lastName, string firstname)
        {
            lastName = lastName.Replace("'", "''");
            string selectString = "Select * from Vitalrecord where FatherLastName = '" + lastName + "' and Fatherfirstname = '" + firstname +
                                  "' and FatherId=0 and VitalRecordType <> 9";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = selectString;
            cmd.Connection = sqlConnection;
            int num = ExecuteSelectStatement(father_tbl, cmd);
            return num;
        }
        //****************************************************************************************************************************
        public static int SelectLast(string tableName,
                                     string columnName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select Top 1 " + columnName + " from [" + tableName + "] order by " + columnName + " desc";
            cmd.Connection = sqlConnection;
            DataTable tbl = new DataTable();
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return (iNum == 0) ? 0 : tbl.Rows[0][0].ToInt();
        }
        //****************************************************************************************************************************
        public static bool SelectAll(bool useOrNotAnd,
                                     string sTableName,
                                     string sOrderBy,
                                     DataTable tbl,
                                     params NameValuePair[] ColumnNameValuePairs)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName) + WhereString(useOrNotAnd, ColumnNameValuePairs) + sOrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        public static bool SelectAllGreaterThan(string sTableName,
                                     DataTable tbl,
                                     NameValuePair ColumnName)
        {
            return SelectAllGreaterThan(sTableName, "", tbl, ColumnName);
        }
        //****************************************************************************************************************************
        public static bool SelectAllGreaterThan(string sTableName,
                                     string orderBy,
                                     DataTable tbl,
                                     NameValuePair ColumnName)
        {
            string whereString = " where " + ColumnName.Name() + " > " + ColumnName.Value().ToString() + orderBy;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName) + whereString;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        public static bool SelectAllBetween(string sTableName,
                                     string sOrderBy,
                                     DataTable tbl,
                                     params NameValuePair[] ColumnNameValuePairs)
        {
            if (ColumnNameValuePairs.Length != 2)
            {
                HistoricJamaicaException ex = new HistoricJamaicaException("Select all Between must have only 2 Name Value Pairs");
                throw ex;
            }
            NameValuePair lowerValue = ColumnNameValuePairs[0];
            NameValuePair higherValue = ColumnNameValuePairs[1];
            string whereString = " where " + lowerValue.Name() + " >= " + lowerValue.Value().ToString() +
                                 " and " + higherValue.Name() + " <= " + higherValue.Value().ToString();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName) + whereString + sOrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        public static bool SelectAllLike(DataTable tbl,
                                     string sTableName,
                                     NameValuePair ColumnName)
        {
            return SelectAllLike(tbl, sTableName, U.NoOrderBy, ColumnName);
        }
        //****************************************************************************************************************************
        public static bool SelectAllLike(DataTable tbl,
                                     string sTableName,
                                     string OrderBy,
                                     NameValuePair ColumnName)
        {
            string value = RemoveWildCardChars(ColumnName.Value());
            string whereString = " where " + ColumnLike(ColumnName.Name(), value);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName) + whereString + OrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        public static string RemoveWildCardChars(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            int lastCharIndex = value.Length - 1;
            if (value.Contains("'"))
            {
                value = value.Replace("'", "''");
            }
            string newValue = value.Replace('*', '%');
            if (newValue[lastCharIndex] == '%')
            {
                newValue = newValue.Remove(lastCharIndex);
            }
            return newValue;
        }
        //****************************************************************************************************************************
        public static bool SelectAllLike(DataTable tbl,
                                     string sTableName,
                                     string OrderBy,
                                     NameValuePair ColumnName,
                                     NameValuePair ColumnNameLike)
        {
            string whereString = WhereString(ColumnName) + " and " + ColumnLike(ColumnNameLike.Name(), ColumnNameLike.Value());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName) + whereString + OrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        public static bool SelectAllLike(DataTable tbl,
                                     string sTableName,
                                     string OrderBy,
                                     NameValuePair ColumnName,
                                     NameValuePair ColumnName2,
                                     NameValuePair ColumnName3,
                                     NameValuePair ColumnNameLike)
        {
            string whereString = WhereString(ColumnName, ColumnName2, ColumnName3) + " and " + ColumnLike(ColumnNameLike.Name(), ColumnNameLike.Value());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SelectAllFromString(sTableName) + whereString + OrderBy;
            cmd.Connection = sqlConnection;
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        public static bool GetDistinctValues(DataTable tbl,
                                     string sTableName,
                                     string sColumn,
                                     params NameValuePair[] columnIDs)
        {
            SqlCommand cmd = new SqlCommand();
            string whereString = whereStringWithParms(cmd.Parameters, columnIDs);
            string sSelectStatement = "Select Distinct " + sColumn + " from " + sTableName + whereString +
                                      " order by " + sColumn;
            cmd.Connection = sqlConnection;
            cmd.CommandText = sSelectStatement;
            ExecuteSelectStatement(tbl, cmd);
            return tbl.Rows.Count != 0;
        }
        //****************************************************************************************************************************
        public static bool GetDistinctValues(DataTable tbl,
                                     string sTableName,
                                     string sColumn,
                                     string sColumn2,
                                     string sColumn3,
                                     params NameValuePair[] columnIDs)
        {
            SqlCommand cmd = new SqlCommand();
            string whereString = WhereString(columnIDs[0]);
            string columns = sColumn + "," + sColumn2 + "," + sColumn3;
            string sSelectStatement = "Select Distinct " + columns + " from " + sTableName + whereString +
                                      " order by " + sColumn + "," + sColumn2 + ","  + sColumn3;
            cmd.Connection = sqlConnection;
            cmd.CommandText = sSelectStatement;
            ExecuteSelectStatement(tbl, cmd);
            return tbl.Rows.Count != 0;
        }
        //****************************************************************************************************************************
        public static int GetMaxValue(string sTableName,
                               string sColumn,
                               string sIDColumn,
                               int iID)
        {
            DataTable tbl = new DataTable(sTableName);
            string sSelectStatement = "Select Max(" + sColumn + ") as MaxValue from " + sTableName +
                                      " where " + sIDColumn + " = " + iID.ToString();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = sSelectStatement;
            ExecuteSelectStatement(tbl, cmd);
            return tbl.Rows[0]["MaxValue"].ToInt();
        }
        //****************************************************************************************************************************
        public static int GetMaxValue(string sTableName,
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
        private static bool SelectTopOneLike(DataTable tbl,
                                             string tableName,
                                             string column,
                                             string likeColumnValue,
                                             string orderByAscDesc)
        {
            string orderby = " " + orderByAscDesc.ToUpper().Trim();
            string sSelectStatement = "Select Top 1 " + column + " From " + tableName +
                                     " Where " + ColumnLike(column, likeColumnValue) +
                                     " Order by " + column + orderby;
            SqlCommand cmd = new SqlCommand(sSelectStatement, sqlConnection);
            int iNum = ExecuteSelectStatement(tbl, cmd);
            return iNum != 0;
        }
        //****************************************************************************************************************************
        private static string SelectAllString(string sTableName,
                                     string sOrderBy,
                                     params string[] ColumnNames)
        {
            return SelectAllFromString(sTableName) + WhereString(ColumnNames) + sOrderBy + ";";
        }
        //****************************************************************************************************************************
        private static void SelectColumns(DataTable tbl,
                                          string selectColumn,
                                          string tableName,
                                          NameValuePair keyColumn)
        {
            string[] selectColumns = { selectColumn };
            string[] asColumns = null;
            SelectColumnsAs(tbl, selectColumns, asColumns, tableName, keyColumn);
        }
        //****************************************************************************************************************************
        private static void SelectColumns(DataTable tbl,
                                          string[] selectColumns,
                                          string tableName,
                                          NameValuePair keyColumn)
        {
            string[] asColumns = null;
            SelectColumnsAs(tbl, selectColumns, asColumns, tableName, keyColumn);
        }
        //****************************************************************************************************************************
        private static void SelectColumnsAs(DataTable tbl,
                                            string selectColumn,
                                            string asColumn,
                                            string tableName,
                                            NameValuePair keyColumn)
        {
            string[] selectColumns = { selectColumn };
            string[] asColumns = { asColumn };
            SelectColumnsAs(tbl, selectColumns, asColumns, tableName, keyColumn);
        }
        //****************************************************************************************************************************
        private static void SelectColumnsAs(DataTable tbl,
                                            string[] selectColumns,
                                            string[] asColumns,
                                            string tableName,
                                            params NameValuePair[] keyColumns)
        {
            SqlCommand cmd = new SqlCommand();
            int numKeyColumns = keyColumns.Length;
            string whereString = "";
            foreach (NameValuePair keyColumn in keyColumns)
            {
                whereString += AddOneOfTwoStrings(whereString.Length, " Where ", " and ");
                whereString += ColumnEquals(keyColumn.Name());
                cmd.Parameters.Add(SqlParm(keyColumn.Name(), keyColumn.Value()));
            }
            string selectString = SelectColumnAsString(selectColumns, tableName, asColumns);
            cmd.CommandText = selectString + whereString;
            cmd.Connection = sqlConnection;
            ExecuteSelectStatement(tbl, cmd);
        }
        //****************************************************************************************************************************
        public static string SelectColumnAsString(string[] selectColumns,
                                            string[] orderByColumns,
                                            string tableName,
                                            params NameValuePair[] keyColumns)
        {
            string[] asColumns = null;
            string orderBy = OrderBy(orderByColumns);
            string selectString = SelectColumnAsString(selectColumns, tableName, asColumns);
            string whereString = "";
            foreach (NameValuePair keyColumn in keyColumns)
            {
                whereString += AddOneOfTwoStrings(whereString.Length, " Where ", " and ");
                whereString += keyColumn.Name() + "=" + keyColumn.Value();
            }
            return selectString + whereString + orderBy + ";";
        }
        //****************************************************************************************************************************
        public static string SelectColumnAsString(string[] selectColumns,
                                            string tableName,
                                            string[] asColumns)
        {
            string selectAsString = "";
            int i = 0;
            foreach (string selectColumn in selectColumns)
            {
                selectAsString += AddOneOfTwoStrings(selectAsString.Length, "Select ", ",");
                selectAsString += selectColumn;
                if (asColumns != null)
                {
                    selectAsString += " as " + asColumns[i];
                }
                i++;
            }
            return selectAsString + " From [" + tableName + "]";
        }
        //****************************************************************************************************************************
        private static string SelectAllWhereString(string sTableName,
                                                   params string[] sColumnNames)
        {
            return SelectAllFromString(sTableName) + WhereString(sColumnNames);
        }
        //****************************************************************************************************************************
        private static string SelectAllFromString(string tableName)
        {
            return "Select * from [" + tableName + "]";
        }
        //****************************************************************************************************************************
        public static int ExecuteSelectStatement(DataTable tbl,
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
                errors.SQLShowError(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                errors.SQLShowError(ex.Message.ToString());
            }
            return 0;
        }
        //****************************************************************************************************************************
        public static void ExecuteSelectStatement(DataSet ds,
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
            try
            {
                da.Fill(ds);
            }
            catch (ArgumentException ex)
            {
                throw new HistoricJamaicaException(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new HistoricJamaicaException(ex.Message.ToString());
            }
        }
        //****************************************************************************************************************************
    }
}
