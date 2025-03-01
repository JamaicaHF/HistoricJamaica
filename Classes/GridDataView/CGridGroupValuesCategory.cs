    using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    class CGridGroupValuesCategory : CGridGroupValue
    {
        //****************************************************************************************************************************
        public CGridGroupValuesCategory(CSql SQL): base(SQL, U.PhotoCategoryValue_Table)
        {
        }
        //****************************************************************************************************************************
        protected override void InitializeSpecialGridValues()
        {
            General_DataGridView.MouseDown += new MouseEventHandler(OnMouseDownClick);
            General_DataGridView.MouseUp += new MouseEventHandler(OnMouseUpClick);
            General_DataGridView.MouseMove += new MouseEventHandler(OnMouseMoveClick);
        }
        //****************************************************************************************************************************
        protected override void InitializeGroupObjects()
        {
            General_DataGridView.Columns[0].Name = U.Category_Table;
            toolStripItem1.Text = "View Photographs by Category";
            toolStripItem1.Click += new EventHandler(ViewGroup_Click);
            ToolStrip.Items.Add(toolStripItem1);
            toolStripItem2.Text = "Delete Category/Category Value";
            toolStripItem2.Click += new EventHandler(Delete_Click);
            ToolStrip.Items.Add(toolStripItem2);
            toolStripItem3.Text = "New Category";
            toolStripItem3.Click += new EventHandler(InsertGroup_Click);
            ToolStrip.Items.Add(toolStripItem3);
            toolStripItem4.Text = "New Category Value";
            toolStripItem4.Click += new EventHandler(InsertValue_Click);
            toolStripItem5.Text = "Order Alphabetically";
            ToolStrip.Items.Add(toolStripItem4);
            toolStripItem5.Click += new EventHandler(Item5_Click);
            ToolStrip.Items.Add(toolStripItem5);
        }
        //****************************************************************************************************************************
        protected override void UpdateOrder()
        {
            if (!m_bOrderChanged)
                return;
            DataTable tbl = new DataTable();
            for (int iRow = CurrentGroupIndex; iRow <= NextGroupIndex; iRow++)
            {
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
                int iCategoryOrder = dataGridViewRow.Cells[2].Value.ToInt();
            }
            SQL.UpdateAllCategoryValues(tbl);
            m_bOrderChanged = false;
        }
        //****************************************************************************************************************************
        protected override void GetAllPhotosFromValue(DataTable tbl,
                                                       int iGroupValueID)
        {
            SQL.GetAllCategoryPhotosFromValue(tbl, iGroupValueID);
        }
        //****************************************************************************************************************************
        protected override string GroupValueValue(DataRow row)
        {
            return row[U.CategoryValueValue_col].ToString();
        }
        //****************************************************************************************************************************
        protected override string GetValueOrderString()
        {
            return U.CategoryValueOrder_col;
        }
        //****************************************************************************************************************************
        protected override string GroupValueID(DataRow row)
        {
            return row[U.CategoryValueID_col].ToString();
        }
        //****************************************************************************************************************************
        protected override string GroupID(DataRow row)
        {
            return row[U.CategoryID_col].ToString();
        }
        //****************************************************************************************************************************
        protected override string GroupValueOrder(DataRow row)
        {
            return row[U.CategoryValueOrder_col].ToString();
        }
        protected override void PopulateDataGridViewValues()
        {
            toolStripItem4.Visible = false;
            toolStripItem5.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void PopulateDataGridView()
        {
            DataTable tbl = new DataTable();
            SQL.GetAllCategories(tbl);
            int iRow = 0;
            foreach (DataRow row in tbl.Rows)
            {
                General_DataGridView.Rows.Add(row[U.CategoryName_col].ToString(), row[U.CategoryID_col].ToString(), 0);
                iRow++;
            }
            toolStripItem4.Visible = true;
            toolStripItem5.Visible = true;
        }
        //****************************************************************************************************************************
        protected override void GetAllGroupValues(DataTable tbl,
                                                  int iGroupID,
                                                 int iGroupID2 = 0)
        {
            SQL.GetAllCategoryValues(tbl, iGroupID);
        }
        //****************************************************************************************************************************
        protected override void DeleteGroupAndValue(int iGroupID) 
        {
            SQL.DeleteWithParms(U.CategoryValue_Table, new NameValuePair(U.CategoryID_col, iGroupID));
            SQL.DeleteWithParms(U.Category_Table, new NameValuePair(U.CategoryID_col, iGroupID));
        }
        //****************************************************************************************************************************
        protected override string UpdateGroupValue(int    iCategoryValueID,
                                                 string sOriginalValue,
                                                 string sNewValue)
        {
            DataTable tbl = new DataTable();
            tbl = SQL.GetCategoryIDValue(iCategoryValueID);
            if (tbl.Rows.Count == 0)
            {
                MessageBox.Show("Value no longer exists");
                return sOriginalValue;
            }
            else
            {
                tbl.Rows[0][U.CategoryValueValue_col] = sNewValue;
                if (SQL.UpdateAllCategoryValues(tbl))
                    return sOriginalValue;
                else
                    return sNewValue;
            }
        }
        //****************************************************************************************************************************
        protected override void UpdateGroup(int iValueID,
                                            string sOriginalValue,
                                            string sNewValue)
        {
            if (sNewValue != sOriginalValue)
            {
                DataTable tbl = new DataTable();
                SQL.UpdateCategoryName(iValueID,sNewValue);
            }
        }
        //****************************************************************************************************************************
        protected override void Item5_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sInputString = dataGridViewRow.Cells[0].Value.ToString();
            int iCategoryID = 0;
            if (sInputString[0] == ' ')
            {
                iRowIndex = GetRowOfGroupIDFromGrid(iRowIndex);
                dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            }
            iCategoryID = dataGridViewRow.Cells[1].Value.ToInt();
            DataTable CategoryValue_tbl = SQL.DefineCategoryValueTable(U.CategoryValue_Table);
            SQL.GetAllCategoryValuesAlphabetical(CategoryValue_tbl, iCategoryID);
            if (CategoryValue_tbl.Rows.Count == 0)
                return;
            int iNumRows = General_DataGridView.Rows.Count - 1;
            int iRow = 0;
            ++iRowIndex;
            if (iRowIndex < iNumRows)
            {
                dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                sInputString = dataGridViewRow.Cells[0].Value.ToString();
                if (sInputString[0] != ' ')
                    iNumRows = iRowIndex - 1;
            }
            bool bDoUpdate = false;
            foreach (DataRow row in CategoryValue_tbl.Rows)
            {
                ++iRow;
                int iPreviousOrder = row[U.CategoryValueOrder_col].ToInt();
                if (iPreviousOrder != iRow)
                {
                    row[U.CategoryValueOrder_col] = iRow;
                    bDoUpdate = true;
                }
                if (iRowIndex < iNumRows && sInputString[0] == ' ')
                {
                    dataGridViewRow.Cells[0].Value = UU.ShowGroupValue(row[U.CategoryValueValue_col].ToString());
                    ++iRowIndex;
                    dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                    sInputString = dataGridViewRow.Cells[0].Value.ToString();
                }
            }
            if (bDoUpdate && !SQL.UpdateAllCategoryValues(CategoryValue_tbl))
                MessageBox.Show("Save Unsuccesful");
        }
        //****************************************************************************************************************************
        protected override int GetGroupValueID(int iGroupID,
                                              string sGroupValue)
        {
            return SQL.GetCategoryValueID(iGroupID.ToString(), sGroupValue);
        }
        //****************************************************************************************************************************
        protected override int GetGroupValueOrder(int iGroupID,
                                                  string sPreviousGroupValue)
        {
            return SQL.GetCategoryValueOrder(iGroupID, sPreviousGroupValue.TrimString());
        }
        //****************************************************************************************************************************
        protected override int GetGroupIDFromName(string sGroup)
        {
            return SQL.GetCategoryIDFromName(sGroup);
        }
        //****************************************************************************************************************************
        protected override int AddValueToDatabase(int    iGroupID,
                                                  string sGroupValueValue,
                                                  int    iGroupValueOrder)
        {
            DataTable tbl = new DataTable(U.CategoryValue_Table);
            SQL.GetCategoryIDValue(tbl, iGroupID.ToString(), sGroupValueValue);
            if (tbl.Rows.Count == 0)     // does not exist
            {
                return SQL.InsertCategoryValue(iGroupID, sGroupValueValue, iGroupValueOrder);
            }
            return 0;
        }
        //****************************************************************************************************************************
        protected override int GetGroupIDFromValueID(int iGroupValueID) 
        {
            return SQL.GetCategoryId(iGroupValueID); 
        }
        //****************************************************************************************************************************
        protected override int AddGroupToDatabase(ref string sNewGroup)
        {
            if (SQL.GetCategoryIDFromName(sNewGroup) == 0) // does not exist
            {
                return SQL.InsertCategory(sNewGroup);
            }
            return 0;
        }
        //****************************************************************************************************************************
        protected override bool DeleteGroupValue(int iValueID)
        {
            return SQL.DeleteCategoryValue(iValueID);
        }
    }
}
