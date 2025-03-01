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
    abstract class CGridGroupValue: CGrid
    {
        protected CSql m_SQL = null;
        protected int contextMenuRowIndex;
        protected ContextMenuStrip ToolStrip = new ContextMenuStrip();
        protected string m_PhotoGroupValueTable;
        protected ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem4 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem5 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem6 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem7 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem8 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem9 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem10 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem11 = new ToolStripMenuItem();
        protected ToolStripMenuItem toolStripItem12 = new ToolStripMenuItem();
        protected DataGridViewTextBoxColumn GroupColumn = new DataGridViewTextBoxColumn();
        protected DataGridViewTextBoxColumn IDColumn = new DataGridViewTextBoxColumn();
        protected DataGridViewTextBoxColumn OrderColumn = new DataGridViewTextBoxColumn();
        protected DataGridViewTextBoxColumn ValueIDColumn = new DataGridViewTextBoxColumn();
        protected DataGridViewTextBoxColumn BuildingType = new DataGridViewTextBoxColumn();
        protected DataGridViewTextBoxColumn Notes = new DataGridViewTextBoxColumn();
        protected bool m_bValueModeOnly = false;
        protected bool m_bSelectionModeOnly = false;
        protected string m_sOriginalValue = "";
        protected int m_iSelectedGroupID = 0;
        protected int m_iSelectedGroupValueID = 0;
        protected bool m_bLookingToMerge = false;
        protected bool m_bUseDoubleClick = true;
        //****************************************************************************************************************************
        public int SelectedGroupID
        {
            get { return m_iSelectedGroupID; }
        }
        //****************************************************************************************************************************
        public int SelectedValueID
        {
            get { return m_iSelectedGroupValueID; }
        }
        //****************************************************************************************************************************
        public CGridGroupValue(CSql SQL)
        {
            m_SQL = SQL;
            m_PhotoGroupValueTable = "";
            buttonPane.Visible = false;
        }
        //****************************************************************************************************************************
        public CGridGroupValue(CSql SQL,
                           string PhotoGroupValueTable)
        {
            m_SQL = SQL;
            m_PhotoGroupValueTable = PhotoGroupValueTable;
            buttonPane.Visible = false;
        }
        //****************************************************************************************************************************
        private void RemoveValuesFromGrid(int iRowIndex)
        {
            UpdateOrder();
            RemoveColumn2();
            string sGroup;
            if (NotWithinLimitsOfGrid(iRowIndex))
            {
                return;
            }
            iRowIndex++;
            int i = General_DataGridView.Rows.Count - 1;
            do
            {
                General_DataGridView.Rows.RemoveAt(iRowIndex);
                int iCount = General_DataGridView.Rows.Count-1;  // account for blank row at bottom
                if (iRowIndex >= iCount)
                    break;
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                sGroup = dataGridViewRow.Cells[0].Value.ToString();
            } while (sGroup[0] == ' ');
        }
        //****************************************************************************************************************************
        protected virtual int GetGroupValueIDFromGrid(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            return dataGridViewRow.Cells[1].Value.ToInt();
        }
        //****************************************************************************************************************************
        protected void ShowGroupWithSelectedValues()
        {
            General_DataGridView.Columns[0].Name = "Buildings";
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sGroupValue = dataGridViewRow.Cells[0].Value.ToString();
            int iGroupValueID = GetGroupValueIDFromGrid(iRowIndex);
            int iGroupID = GetGroupIDFromValueID(iGroupValueID);
            ShowAllValues("");
            for (int i = 0; i < General_DataGridView.Rows.Count; ++i)
            {
                dataGridViewRow = General_DataGridView.Rows[i];
                int iGridGroupID = dataGridViewRow.Cells[1].Value.ToInt();
                if (iGridGroupID == iGroupID)
                {
                    iRowIndex = i;
                    CurrentGroupIndex = iRowIndex;
                    General_DataGridView.Rows[iRowIndex].Cells[0].Selected = true;
                    General_DataGridView.FirstDisplayedCell = General_DataGridView[0, iRowIndex];
                    NextGroupIndex = NextGroup(iRowIndex);
                }
            }
            AddValuesToGrid(iRowIndex);
        }
        //****************************************************************************************************************************
        private void ExpandOrContractValues(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sGroupID = dataGridViewRow.Cells[0].Value.ToString();
            if (sGroupID.Length == 0 || sGroupID[0] == ' ')
            {
                int iRowIndexOfGroup = RowOfGroupBasedOnValue(iRowIndex);
                RemoveValuesFromGrid(iRowIndexOfGroup);
                General_DataGridView.Rows[iRowIndexOfGroup].Cells[0].Selected = true;
            }
            else
            {
                int iNumValues = 0;
                int iNextRowIndex = iRowIndex + 1;
                if (iNextRowIndex < (General_DataGridView.Rows.Count - 1))
                {
                    dataGridViewRow = General_DataGridView.Rows[iNextRowIndex];
                    string sNextGroupID = dataGridViewRow.Cells[0].Value.ToString();
                    if (sNextGroupID[0] == ' ')
                        RemoveValuesFromGrid(iRowIndex);
                    else
                        iNumValues = AddValuesToGrid(iRowIndex);
                }
                else
                {
                    iNumValues = AddValuesToGrid(iRowIndex);
                }
                if (iNumValues > 0)
                    iRowIndex++;
                if (iRowIndex < General_DataGridView.Rows.Count)
                {
                    General_DataGridView.Rows[iRowIndex + 1].Cells[0].Selected = true;
                    // needed to be sure the row changes to avoid edit mode
                    General_DataGridView.Rows[iRowIndex].Cells[0].Selected = true;
                }
                else
                    General_DataGridView.Rows[0].Cells[0].Selected = true;
            }
        }
        //****************************************************************************************************************************
        protected virtual void GetSelectedGroupAndValueID(int iRowIndex)
        {
            m_iSelectedGroupValueID = General_DataGridView.Rows[iRowIndex].Cells[1].Value.ToInt();
            m_iSelectedGroupID = GetGroupIDFromValueID(m_iSelectedGroupValueID);
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            if (m_bValueModeOnly || m_bSelectionModeOnly)
            {
                if (IsGroupValue(iRowIndex))
                {
                    GetSelectedGroupAndValueID(iRowIndex);
                }
                else
                {
                    DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                    m_iSelectedGroupID = dataGridViewRow.Cells[1].Value.ToInt();
                }
                Close();
            }
            else 
            if (m_bLookingToMerge)
            {
                m_iSelectedGroupID = General_DataGridView.Rows[iRowIndex].Cells[1].Value.ToInt();
                Close();
            }
            else 
            if (m_bShowValues)
                ShowGroupWithSelectedValues();
            else
            if (m_eInsertMode != InsertMode.InsertGroupValue)
            {
                ExpandOrContractValues(iRowIndex);
            }
        }
        //****************************************************************************************************************************
        protected virtual int AddGroupToDatabase(ref string sNewGroup) { return 0; }
        protected virtual void RemoveColumn2() { }
        protected virtual void UpdateValueOrder(int iDefaultOrder,
                                                int iValueID) { }
        //****************************************************************************************************************************
        protected virtual bool DeleteValue(DataGridViewRow dataGridViewRow)
        {
            int iValueID = dataGridViewRow.Cells[1].Value.ToInt();
            return DeleteGroupValue(iValueID);
        }
        //****************************************************************************************************************************
        protected virtual void GetAnyAdditionalValues(int iDefaultOrder,
                                                      int iGroupID,
                                                      ref int iRow) { }
        //****************************************************************************************************************************
        protected virtual int AddValueToDatabase(int iGroupID,
                                       string sGroupValueValue,
                                       int iGroupValueOrder) { return 0; }
        //****************************************************************************************************************************
        protected virtual string UpdateGroupValue(int iValueID,
                                                string sOriginalValue,
                                                string sNewValue) { return ""; } 
        //****************************************************************************************************************************
        protected virtual void UpdateGroup(int iValueID,
                                           string sOriginalValue,
                                           string sNewValue) { }
        //****************************************************************************************************************************
        protected virtual int GetGroupIDFromValueID(int iGroupValueID) { return 0; }
        //****************************************************************************************************************************
        protected virtual void NullValueEntered() 
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            dataGridViewRow.Cells[0].Value = m_sOriginalValue;
        }
        //****************************************************************************************************************************
        protected virtual bool RowCannotBeModified(string m_sOriginalValue) { return false; }
        //****************************************************************************************************************************
        private void ModifyValue()
        {
            int iPreviousRowIndex = m_iEditLocation + 1;
            General_DataGridView.Rows[iPreviousRowIndex].Cells[0].Selected = true;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            if (dataGridViewRow.Cells[0].Value == null)
                NullValueEntered();
            else
            {
                string sInputString = dataGridViewRow.Cells[0].Value.TrimString();
                int iStartPosition = sInputString.IndexOf('(');
                if (iStartPosition > 0)
                    sInputString = sInputString.Remove(iStartPosition);
                sInputString.TrimString();
                if (RowCannotBeModified(m_sOriginalValue))
                { // do nothing
                }
                else
                if (sInputString.Length == 0)
                    dataGridViewRow.Cells[0].Value = m_sOriginalValue;
                else
                if (m_eInsertMode == InsertMode.EditGroupValue)
                {
                    int iValueID = dataGridViewRow.Cells[1].Value.ToInt();
                    sInputString = UpdateGroupValue(iValueID, m_sOriginalValue, sInputString);
                    dataGridViewRow.Cells[0].Value = UU.ShowGroupValue(sInputString);

                }
                else
                {
                    int iGroupID = dataGridViewRow.Cells[1].Value.ToInt();
                    UpdateGroup(iGroupID, m_sOriginalValue, sInputString);
                }
            }
            int iRowIndex = m_iEditLocation;
            if (iRowIndex < 0)
                MessageBox.Show("Negative");
            General_DataGridView.Rows[iRowIndex].Cells[0].Selected = true;
            m_eInsertMode = InsertMode.NotInsertEditMode;
            m_sOriginalValue = "";
        }
        //****************************************************************************************************************************
        protected virtual int FirstValueIndex() { return 1; }
        //****************************************************************************************************************************
        protected virtual int BuildingValueType() { return 0; }
        //****************************************************************************************************************************
        protected virtual string ValueString(int iRowIndex,
                                             string sInputString,
                                             int iBuildingType,
                                             int iOccupantYear=0)
        {
            return UU.ShowGroupValue(sInputString);
        }
        //****************************************************************************************************************************
        protected virtual void InsertGroupValue(string sInputString)
        {
            int iRowIndex = m_iEditLocation - 1;
            int iRowOfGroup = RowOfGroupBasedOnValue(iRowIndex);
            string sGroupName = General_DataGridView.Rows[iRowOfGroup].Cells[0].Value.ToString();
            int iGroupID = General_DataGridView.Rows[iRowOfGroup].Cells[1].Value.ToInt();
            int iGroupValueOrder = 0; // primary Index;
            if (iRowOfGroup == iRowIndex)
                iGroupValueOrder = FirstValueIndex();
            else
                iGroupValueOrder = General_DataGridView.Rows[iRowIndex].Cells[2].Value.ToInt() + 1;
            if (iGroupID != 0)
            {
                int iGroupValueID = AddValueToDatabase(iGroupID, sInputString, iGroupValueOrder);
                if (iGroupValueID == 0)
                {
                    MessageBox.Show("Value Already Exists");
                    General_DataGridView.Rows.RemoveAt(m_iEditLocation);
                }
                else
                {
                    int iBuildingType = BuildingValueType();
                    General_DataGridView.Rows[m_iEditLocation].Cells[1].Value = iGroupValueID;
                    string sValue = ValueString(m_iEditLocation, sInputString, iBuildingType);
                    General_DataGridView.Rows[m_iEditLocation].Cells[0].Value = sValue;
                    General_DataGridView.Rows[m_iEditLocation].Cells[2].Value = 0;
                    General_DataGridView.Rows[m_iEditLocation].Cells[3].Value = iGroupValueID;
                }
            }
        }
        //****************************************************************************************************************************
        private void InsertValue()
        {
            if (m_iEditLocation < 0)
            {
                m_eInsertMode = InsertMode.NotInsertEditMode;
                return;
            }
            General_DataGridView.Rows[m_iEditLocation].Cells[0].Selected = true;
            string sInputString = General_DataGridView.Rows[m_iEditLocation].Cells[0].Value.TrimString();
            if (sInputString.Length == 0)
            {
                General_DataGridView.Rows.RemoveAt(m_iEditLocation);
                return;
            }
            General_DataGridView.Rows[m_iEditLocation].Cells[0].Selected = true;
            if (m_eInsertMode == InsertMode.InsertGroup)
            {
                General_DataGridView.Rows[m_iEditLocation].Cells[1].Value = AddGroupToDatabase(ref sInputString);
                General_DataGridView.Rows[m_iEditLocation].Cells[0].Value = sInputString;
            }
            else
            {
                InsertGroupValue(sInputString);
            }
            m_eInsertMode = InsertMode.NotInsertEditMode;
        }
        //****************************************************************************************************************************
        protected virtual int GetGroupValueOrder(int iGroupID,
                                                 string sPreviousGroupValue) { return 0; }
        //****************************************************************************************************************************
        protected override void CheckValue()
        {
            if (m_eInsertMode == InsertMode.EditGroupValue || m_eInsertMode == InsertMode.EditGroup)
            {
                ModifyValue();
            }
            else
            if (m_eInsertMode != InsertMode.NotInsertEditMode)
            {
                InsertValue();
            }
        }
        //****************************************************************************************************************************
        protected override void SetToEditMode()
        {
            if (m_eInsertMode == InsertMode.NotInsertEditMode)
            {
                m_iEditLocation = m_iCurrentRow;
                if (General_DataGridView.Rows[m_iCurrentRow].Cells[0].Value == null)
                {
                    General_DataGridView.Rows[m_iCurrentRow].Cells[0].Value = "";
                }
                m_sOriginalValue = General_DataGridView.Rows[m_iCurrentRow].Cells[0].Value.ToString();
                if (m_sOriginalValue.Length > 0 && m_sOriginalValue[0] == ' ')
                    m_eInsertMode = InsertMode.EditGroupValue;
                else
                    m_eInsertMode = InsertMode.EditGroup;
            }
        }
        //****************************************************************************************************************************
        protected override void SetupLayout()
        {
            if (m_bUseDoubleClick)
                General_DataGridView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SelectRowButton_DoubleClick);
            General_DataGridView.CellEndEdit += new DataGridViewCellEventHandler(CellEndEditEvent);
        }
        //****************************************************************************************************************************
        protected abstract void InitializeGroupObjects();
        protected virtual void InitializeSpecialGridValues() { }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
            this.Location = new System.Drawing.Point(400, 80);
            this.Size = new Size(350, 750);
            this.BackColor = Color.LightSteelBlue;
            this.Controls.Add(General_DataGridView);
            General_DataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            General_DataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            General_DataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(General_DataGridView.Font, FontStyle.Bold);
            General_DataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            General_DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            General_DataGridView.GridColor = Color.LightSteelBlue;
            General_DataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            General_DataGridView.RowHeadersVisible = false;

//            DataGridViewTextBoxColumn GroupColumn = new DataGridViewTextBoxColumn();
            GroupColumn.MaxInputLength = U.iMaxValueLength;
            General_DataGridView.Columns.Add(GroupColumn);
            General_DataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            General_DataGridView.Columns[0].Width = 500;
            General_DataGridView.Columns.Add(IDColumn);
            General_DataGridView.Columns[1].Visible = false;
            General_DataGridView.MultiSelect = false;
            General_DataGridView.Dock = DockStyle.Fill;
            General_DataGridView.Columns.Add(OrderColumn);
            General_DataGridView.Columns[2].Visible = false;
            General_DataGridView.MultiSelect = false;
            General_DataGridView.Dock = DockStyle.Fill;
            General_DataGridView.Columns.Add(ValueIDColumn);
            General_DataGridView.Columns[3].Visible = false;
            General_DataGridView.MultiSelect = false;
            General_DataGridView.Dock = DockStyle.Fill;
            InitializeSpecialGridValues();
            General_DataGridView.DefaultCellStyle.BackColor = Color.LightSteelBlue;
            General_DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
//            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);
            General_DataGridView.CellMouseEnter += dataGridView_CellMouseEnter;

            General_DataGridView.RowContextMenuStripNeeded +=
                new DataGridViewRowContextMenuStripNeededEventHandler(dataGridView1_RowContextMenuStripNeeded);
            General_DataGridView.ContextMenuStrip = new ContextMenuStrip();
            General_DataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick_click);

            InitializeGroupObjects();
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = ToolStrip;
            }
            ToolStrip.MouseEnter += new EventHandler(Right_Click);
        }
        //****************************************************************************************************************************
        protected virtual void SelectRightClickOptions() {}
        //****************************************************************************************************************************
        private void Right_Click(object sender, EventArgs e)
        {
            m_bRightClick = true;
            movingMouse = false;
            int iRowIndex = mouseLocation.RowIndex;
            General_DataGridView.Rows[iRowIndex].Cells[0].Selected = true;
            SelectRightClickOptions();
        }
        //****************************************************************************************************************************
        void dataGridView1_RowContextMenuStripNeeded(object sender,DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[e.RowIndex];
            e.ContextMenuStrip = ToolStrip;
            contextMenuRowIndex = e.RowIndex;
        }
        //****************************************************************************************************************************
        protected virtual void GetAllGroupValues(DataTable tbl,
                                                 int iGroupID,
                                                 int iGroupID2=0) {}
        //****************************************************************************************************************************
        protected virtual void GetAllGroupValues(DataTable tbl,
                                                  string sGroupID) {}
        //****************************************************************************************************************************
        protected virtual string GroupValueValue(DataRow row) { return ""; }
        //****************************************************************************************************************************
        protected virtual string GroupValueID(DataRow row) { return ""; }
        //****************************************************************************************************************************
        protected virtual string GroupID(DataRow row) { return ""; }
        //****************************************************************************************************************************
        protected virtual string GroupValueOrder(DataRow row) { return ""; }
        //****************************************************************************************************************************
        protected int AddValuesToGrid(int iRow)
        {
            CurrentGroupIndex = iRow;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int iGroupID = dataGridViewRow.Cells[1].Value.ToInt();
            int iGroupID2 = General_DataGridView.Columns.Count > 1 ? dataGridViewRow.Cells[2].Value.ToInt() : 0;
            int iNumValues = 0;
            if (iGroupID != 0)
            {
                DataTable tbl = new DataTable();
                GetAllGroupValues(tbl, iGroupID, iGroupID2);
                int iDefaultOrder = 1;
                foreach (DataRow tblRow in tbl.Rows)
                {
                    string sOrder_col = GetValueOrderString();
                    if (!String.IsNullOrEmpty(sOrder_col))
                    {
                        int iOrder = tblRow[sOrder_col].ToInt();
                        if (iOrder == 0)
                        {
                            UpdateValueOrder(iDefaultOrder, tblRow[U.BuildingValueID_col].ToInt());
                            iDefaultOrder++;
                            iOrder = iDefaultOrder;
                        }
                    }
                    iRow++;
                    General_DataGridView.Rows.Insert(iRow, "", GroupValueID(tblRow), GroupValueOrder(tblRow),
                                                            GroupValueID(tblRow));
                    General_DataGridView.Rows[iRow].Cells[0].Value =
                        ValueString(iRow, GroupValueValue(tblRow), GroupValueOrder(tblRow).ToInt());
                    iNumValues++;
                }
                GetAnyAdditionalValues(iDefaultOrder,iGroupID, ref iRow);
            }
            return iNumValues;
        }
        //****************************************************************************************************************************
        private void SortGrid()
        {
            if (General_DataGridView.Rows.Count != 0 && General_DataGridView.CurrentCell != null)
            {
                General_DataGridView.Sort(OrderColumn, System.ComponentModel.ListSortDirection.Ascending);
                General_DataGridView.CurrentCell.Selected = false;
                General_DataGridView.Rows[0].Cells[0].Selected = true;
                General_DataGridView.CurrentCell = General_DataGridView.SelectedCells[0];
            }
        }
        //****************************************************************************************************************************
        protected int GetRowOfGroupIDFromGrid(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sGroup = dataGridViewRow.Cells[0].Value.ToString();
            while (sGroup[0] == ' ')
            {
                iRowIndex--;
                dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                sGroup = dataGridViewRow.Cells[0].Value.ToString();
            }
            return iRowIndex;
        }
        //****************************************************************************************************************************
        protected int GetRowOfLastGroupValueFromGrid(int iRowIndex)
        {
            string sGroup;
            do
            {
                iRowIndex++;
                if (iRowIndex >= General_DataGridView.Rows.Count-1)
                    return iRowIndex - 1;
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                sGroup = dataGridViewRow.Cells[0].Value.ToString();
            } while (sGroup[0] == ' ');
            return iRowIndex - 1;
        }
        //****************************************************************************************************************************
        protected virtual int GetGroupValueID(int    iGroupID,
                                               string sGroupValue) { return 0; }
        //****************************************************************************************************************************
        protected virtual string GetValueOrderString() { return ""; }
        //****************************************************************************************************************************
        protected virtual bool NotPrimaryValue(int iOrder) { return true; }
        //****************************************************************************************************************************
        protected virtual int GetGroupIDFromName(string sGroup) { return 0; }
        //****************************************************************************************************************************
        protected abstract void GetAllPhotosFromValue(DataTable tbl,
                                                        int       iGroupValueID);
        //****************************************************************************************************************************
        protected virtual void ViewGroup_Click(object sender, EventArgs e)
        {
            if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                return;
            int iRowIndex = mouseLocation.RowIndex;
            DataTable Photo_tbl = new DataTable(m_PhotoGroupValueTable);
            Photo_tbl.PrimaryKey = new DataColumn[] { Photo_tbl.Columns[U.PhotoID_col] };

            int iGroupID = 0;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sInputString = dataGridViewRow.Cells[0].Value.ToString();
            if (sInputString[0] == ' ')
            {
                int iGroupValueID = dataGridViewRow.Cells[1].Value.ToInt();
                GetAllPhotosFromValue(Photo_tbl, iGroupValueID);
            }
            else
            {
                iGroupID = dataGridViewRow.Cells[1].Value.ToInt();
                DataTable GroupValue_tbl = new DataTable();
                GetAllGroupValues(GroupValue_tbl, iGroupID);
                foreach (DataRow row in GroupValue_tbl.Rows)
                {
                    GetAllPhotosFromValue(Photo_tbl, row[0].ToInt());
                }
            }
            if (Photo_tbl.Rows.Count == 0)
            {
                return;
            }
            FPhotoViewer PhotoViewer = new FPhotoViewer(ref m_SQL, Photo_tbl, m_PhotoGroupValueTable, FHistoricJamaica.RunPhotoSlideShow);
            PhotoViewer.ShowDialog();
        }
        //****************************************************************************************************************************
        protected void InsertValue_Click(object sender, EventArgs e)
        {
            if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                return;
            m_eInsertMode = InsertMode.InsertGroupValue;
            bool bDoExpandValues = true;
            int iRowIndex = mouseLocation.RowIndex;
            int iNextRowIndex = iRowIndex + 1;
            if (IsGroupValue(iRowIndex))
                bDoExpandValues = false;
            else if (IsGroupValue(iNextRowIndex))
                bDoExpandValues = false;
            if (bDoExpandValues)
                AddValuesToGrid(iRowIndex);
            int iCount = General_DataGridView.Rows.Count - 1;
//            if (iNextRowIndex >= iCount)
            //                General_DataGridView.Rows.Add();
            InsertGroupRow(iNextRowIndex);
        }
        //****************************************************************************************************************************
        protected void InsertGroupRow(int iNewRow)
        {
            m_iEditLocation = iNewRow;
//            if (IsGroupValue(iRowIndex))
//            {
//                m_iEditLocation = RowOfGroupBasedOnValue(m_iEditLocation);
//            }
//            int iCount = General_DataGridView.Rows.Count - 1;
//            string sGroupValue;
//            m_iEditLocation++;
//            if (m_iEditLocation < iCount)
//            {
//                dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
//                sGroupValue = dataGridViewRow.Cells[0].Value.ToString();
//                if (sGroupValue[0] == ' ')
//                    RemoveValuesFromGrid(RowOfGroupBasedOnValue(m_iEditLocation));
//            }
            General_DataGridView.Rows.Insert(m_iEditLocation, "", 0);
            General_DataGridView.Rows[m_iEditLocation].Cells[0].Selected = true;
        }
        //****************************************************************************************************************************
        protected virtual void InsertGroup_Click(object sender, EventArgs e)
        {
            if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                return;
            toolStripItem4.Visible = true;
            int iNewRow = mouseLocation.RowIndex;
            if (m_bValueModeOnly)
            {
                m_bSelectionModeOnly = true;
                ShowAllValues("");
                iNewRow = General_DataGridView.Rows.Count - 1;
            }
            m_eInsertMode = InsertMode.InsertGroup;
            InsertGroupRow(iNewRow);
        }
        //****************************************************************************************************************************
        protected virtual bool DeleteGroupValue(int iValueID) { return true; }
        protected virtual void DeleteGroupAndValue(int iGroupID) {}
        protected virtual void PopulateDataGridView() {}
        //****************************************************************************************************************************
        protected virtual void CloseIfValueSelected() {}
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            if (m_bShowValues)
            {
                General_DataGridView.Rows.Clear();
                PopulateDataGridView();
                m_bShowValues = false;
            }
            else
            {
                if (mouseLocation != null)
                {
                    int iRowIndex = mouseLocation.RowIndex;
                    DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                    string sValueName = dataGridViewRow.Cells[0].Value.ToString();
                }
                General_DataGridView.Rows.Clear();
                PopulateDataGridViewValues();
                m_bShowValues = true;
            }
            CloseIfValueSelected();
        }
        //****************************************************************************************************************************
        protected int GetModernRoad()
        {
            CGridModernRoads GridModernRoads = new CGridModernRoads(m_SQL);
            GridModernRoads.ShowDialog();
            return GridModernRoads.SelectedRoad;
        }
        //****************************************************************************************************************************
        protected virtual void Item5_Click(object sender, EventArgs e) {}
        //****************************************************************************************************************************
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                return;
            if (!UU.MessageReply("Remove from Database?"))
            {
                return;
            }
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sInputString = dataGridViewRow.Cells[0].Value.ToString();
            bool bSuccess = true;
            if (sInputString[0] == ' ')
                bSuccess = DeleteValue(dataGridViewRow);
            else
            {
                int iGroupID = dataGridViewRow.Cells[1].Value.ToInt();
                int iNextRowIndex = iRowIndex + 1;
                if (iNextRowIndex < (General_DataGridView.Rows.Count - 1))
                {
                    dataGridViewRow = General_DataGridView.Rows[iNextRowIndex];
                    string sNextGroupID = dataGridViewRow.Cells[0].Value.ToString();
                    if (sNextGroupID[0] == ' ')
                        RemoveValuesFromGrid(iRowIndex);
                }
                if (bSuccess)
                    DeleteGroupAndValue(iGroupID);
            }
            if (bSuccess)
            {
                General_DataGridView.Rows.RemoveAt(iRowIndex);
                NextGroupIndex--;
            }
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
    }
}
