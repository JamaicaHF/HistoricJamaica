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
        public static DataTable DefineGrandListTable()
        {
            DataTable tbl = new DataTable(U.GrandList_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.GrandListID_col, typeof(int));
            tbl.Columns.Add(U.TaxMapID_col, typeof(string));
            tbl.Columns.Add(U.Span_col, typeof(string));
            tbl.Columns.Add(U.StreetName_col, typeof(string));
            tbl.Columns.Add(U.StreetNum_col, typeof(int));
            tbl.Columns.Add(U.Name1_col, typeof(string));
            tbl.Columns.Add(U.Name2_col, typeof(string));
            tbl.Columns.Add(U.WhereOwnerLiveID_col, typeof(char));
            tbl.Columns.Add(U.BuildingRoadValueID_col, typeof(int));
            tbl.Columns.Add(U.ActiveStatus_col, typeof(char));
            tbl.Columns.Add(U.VacantLand_col, typeof(char));
            SetGrandListVarcharColumnsMaxLength(tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetGrandListVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.Span_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.TaxMapID_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.StreetName_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Name1_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Name2_col].MaxLength = U.iMaxValueLength;
        }
        //****************************************************************************************************************************
        public static DataTable DefineGrandListHistoryTable()
        {
            DataTable tbl = new DataTable(U.GrandListHistory_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.GrandListID_col, typeof(int));
            tbl.Columns.Add(U.Year_col, typeof(int));
            tbl.Columns.Add(U.Name1_col, typeof(string));
            tbl.Columns.Add(U.Name2_col, typeof(string));
            tbl.Columns[U.Name1_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Name2_col].MaxLength = U.iMaxValueLength;
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefineModernRoadValueTable()
        {
            DataTable tbl = new DataTable(U.ModernRoadValue_Table);
            tbl.Columns.Add(U.ModernRoadValueID_col, typeof(int));
            tbl.Columns.Add(U.ModernRoadValueValue_col, typeof(string));
            tbl.Columns.Add(U.ModernRoadValueOrder_col, typeof(int));
            tbl.Columns.Add(U.ModernRoadValueSection_col, typeof(int));
            tbl.Columns.Add(U.JRoadName_col, typeof(string));
            tbl.Columns.Add(U.HistoricRoad_col, typeof(char));
            return tbl;
        }
        //****************************************************************************************************************************
        public static void SetGrandListAddressVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.AddressA_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.AddressB_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.City_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.State_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.Zip_col].MaxLength = U.iMaxNameLength;
        }
        //****************************************************************************************************************************
        public static void SetGrandListWithAddressVarcharColumnsMaxLength(DataTable tbl)
        {
            tbl.Columns[U.TaxMapID_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.StreetName_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Name1_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.Name2_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.AddressA_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.AddressB_col].MaxLength = U.iMaxValueLength;
            tbl.Columns[U.City_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.State_col].MaxLength = U.iMaxNameLength;
            tbl.Columns[U.Zip_col].MaxLength = U.iMaxNameLength;
        }
        //****************************************************************************************************************************
        public static void UpdateInsertDeleteGrandList(DataTable grandListTbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            ArrayList fieldsModified = SetupFieldsModified();
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, grandListTbl, U.GrandList_Table, true);
                SqlCommand deleteCommand = DeleteCommandWithDA(txn, grandListTbl, U.GrandList_Table, U.GrandListID_col);
                SqlCommand updateCommand = UpdateCommandNoKeys(txn, grandListTbl.Columns, U.GrandList_Table, fieldsModified);
                SQL.DeleteUpdateInsertWithDA(grandListTbl, deleteCommand, updateCommand, insertCommand);
                txn.Commit();
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw new Exception("Unable to Update Grand List: " + ex.Message);
            }
        }
        //****************************************************************************************************************************
        public static void InsertGrandListHistory(DataTable grandListHistoryTbl)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            ArrayList fieldsModified = SetupFieldsModified();
            try
            {
                SqlCommand insertCommand = InsertCommand(txn, grandListHistoryTbl, U.GrandListHistory_Table, false);
                SQL.InsertWithDA(grandListHistoryTbl, insertCommand);
                txn.Commit();
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw new Exception("Unable to Create Grand History List: " + ex.Message);
            }
        }
        //****************************************************************************************************************************
        private static ArrayList SetupFieldsModified()
        {
            ArrayList fieldsModified = new ArrayList();
            System.Data.DataTable tbl = SQL.DefineGrandListTable();
            foreach (System.Data.DataColumn column in tbl.Columns)
            {
                if (!GrandListID(column.ColumnName))
                {
                    fieldsModified.Add(column.ColumnName);
                }
            }
            return fieldsModified;
        }
        //****************************************************************************************************************************
        private static bool GrandListID(string columnName)
        {
            if (columnName == U.GrandListID_col)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
    }
}
