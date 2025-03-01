using System;
using System.Drawing;
using System.Windows;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    class CGridGroupValuesBuilding : CGridGroupValue
    {
        private bool m_bOriginalValueModeOnly = false;
        private int m_iModernRoad = -1;
        private bool m_bSingleBuilding = false;
        private int m_iBuildingID = 0;
        private bool m_b1856BuildingName = false;
        private bool m_b1869BuildingName = false;
        private int m_BuildingValueType;
        private int m_GrandListIdForNewBuilding = 0;
        private bool m_bNewBuilding = false;
        //****************************************************************************************************************************
        public CGridGroupValuesBuilding(CSql SQL,
                         int iModernRoad,
                         int grandListId,
                         int iBuildingID,
                         bool bNewBuilding = false) : base(SQL, U.PicturedBuilding_Table)
        {
            m_GrandListIdForNewBuilding = grandListId;
            m_bSingleBuilding = true;
            m_bValueModeOnly = false;
            m_bLookingToMerge = false;
            m_bShowValues = true;
            m_bNewBuilding = bNewBuilding;
            m_iModernRoad = iModernRoad;
            m_iBuildingID = iBuildingID;
            m_bUseDoubleClick = false;
        }
        //****************************************************************************************************************************
        protected override void InitializeSpecialGridValues()
        {
            m_bSelectionModeOnly = m_bValueModeOnly;
            m_bOriginalValueModeOnly = m_bValueModeOnly;
            toolStripItem4.Visible = false;
            toolStripItem6.Visible = false;
            General_DataGridView.Columns.Add(BuildingType);
            General_DataGridView.Columns[4].Visible = false;
            General_DataGridView.Columns.Add(Notes);
            General_DataGridView.Columns[5].Width = 300;
            General_DataGridView.Columns[0].Width = 350;
            this.Size = new Size(650, 750);
            General_DataGridView.MouseDown += new MouseEventHandler(OnMouseDownClick);
            General_DataGridView.MouseUp += new MouseEventHandler(OnMouseUpClick);
            General_DataGridView.MouseMove += new MouseEventHandler(OnMouseMoveClick);
        }
        //****************************************************************************************************************************
        protected override void ResetBooleans()
        {
            m_b1856BuildingName = false;
            m_b1869BuildingName = false;
        }
        //****************************************************************************************************************************
        protected bool IsBuildingValue(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            return (iBuildingType == U.iBuilding); // Building
        }
        //****************************************************************************************************************************
        protected bool IsGroupValueOccupant(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            return (iBuildingType == U.iOccupant); // Occupant
        }
        //****************************************************************************************************************************
        protected bool IsGroupValue1856BuildingName(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            return (iBuildingType == U.i1856BuildingName); // building name
        }
        //****************************************************************************************************************************
        protected bool IsGroupValue1869BuildingName(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            return (iBuildingType == U.i1869BuildingName); // building name
        }
        //****************************************************************************************************************************
        protected bool IsGroupValueCurrentOwner(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            return (iBuildingType == U.iCurrentOwners); // Occupant
        }
        //****************************************************************************************************************************
        protected void GetOccupant(bool bInsert,
                                   int iPersonID,
                                   int iBuildingID,
                                   int iSpouseLivedWithID,
                                   int iOrder,
                                   int iBuildingType,
                                   ref int iRow,
                                   bool bShowRoad)
        {
            string sHome = SQL.PersonHomeName(iPersonID, iBuildingID, iSpouseLivedWithID, bShowRoad);
            if (sHome.Length != 0)
            {
                if (bInsert)
                {
                    InsertIntoGridByOrder(iOrder, iPersonID, iBuildingID, sHome, iBuildingType);
                    iRow++;
                }
                else
                {
                    General_DataGridView.Rows.Add("", iPersonID.ToString(), iOrder, iBuildingID);
                    General_DataGridView.Rows[iRow].Cells[0].Value = ValueString(iRow, sHome, iBuildingType);
                    iRow++;
                }
            }
        }
        //****************************************************************************************************************************
        protected override void UpdateValueOrder(int iDefaultOrder,
                                                 int iValueID)
        {
            iDefaultOrder++;
            SQL.UpdateBuildingValueField(iValueID, U.BuildingValueOrder_col, iDefaultOrder.ToString());
        }
        //****************************************************************************************************************************
        protected override void UpdateOrder()
        {
            bool bUpdateAlreadyAsked = false;
            bool bUpdateOrder = true;
            int iOrder = 2;
            DataTable tbl = new DataTable();
            if (NextGroupIndex >= General_DataGridView.RowCount)
            {
                NextGroupIndex = General_DataGridView.RowCount - 1;
            }
            for (int iRow = CurrentGroupIndex; iRow < General_DataGridView.RowCount - 1; iRow++)
            {
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
                int iBuildingOrder = dataGridViewRow.Cells[2].Value.ToInt();
                int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
                if (iBuildingType != U.iBuilding && iBuildingType != U.iCurrentOwners)
                {
                    UpdateOrder(dataGridViewRow,
                                iBuildingType,
                                iOrder,
                                iBuildingOrder,
                                ref bUpdateAlreadyAsked,
                                ref bUpdateOrder);
                    iOrder++;
                }
            }
        }
        //****************************************************************************************************************************
        private void UpdateOrder(DataGridViewRow dataGridViewRow,
                                 int iBuildingType,
                                 int iOrder,
                                 int iBuildingOrder,
                                 ref bool bUpdateAlreadyAsked,
                                 ref bool bUpdateOrder)
        {
            if (iOrder != iBuildingOrder && bUpdateOrder)
            {
                if (m_bOrderChanged && !bUpdateAlreadyAsked)
                {
                    bUpdateAlreadyAsked = true;
                    if (MessageBox.Show("Do You wish to update order", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        bUpdateOrder = false;
                        return;
                    }
                }
                int iID = dataGridViewRow.Cells[1].Value.ToInt();
                int iBuildingValueID = dataGridViewRow.Cells[3].Value.ToInt();
                SQL.SaveOrder(iBuildingType, iID, iBuildingValueID, iOrder);
            }
        }
        //****************************************************************************************************************************
        protected override int BuildingValueType()
        {
            if (m_b1856BuildingName)
                return U.i1856BuildingName;
            else
            if (m_b1869BuildingName)
                return U.i1869BuildingName;
            else
                return U.iBuildingName;
        }
        //****************************************************************************************************************************
        protected override string ValueString(int iRowIndex,
                                              string sInputString,
                                              int iBuildingType,
                                              int iOccupantYear = 0)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            dataGridViewRow.Cells[4].Value = iBuildingType;
            int iID = dataGridViewRow.Cells[1].Value.ToInt();
            int iBuildingID = dataGridViewRow.Cells[3].Value.ToInt();
            dataGridViewRow.Cells[5].Value = BuildingNotes.GetNotesWithCensusInfo(iBuildingType, iID, iBuildingID).Notes();
            int iLen = sInputString.Length;
            if (sInputString[iLen - 1] == ')')
                return UU.ShowGroupValue(sInputString);
            else
            {

                return UU.ShowGroupValue(U.BuildingString(sInputString, iBuildingType, iID, iBuildingID, iOccupantYear));
            }
        }
        //****************************************************************************************************************************
        private void InsertIntoGridByOrder(int iNewRowOrder,
                                           int iID,
                                           int iBuildingValueID,
                                           string sName,
                                           int iBuildingType,
                                           int iOccupantYear = 0)
        {
            int iNumElements = General_DataGridView.Rows.Count - 1;
            int iRow = CurrentGroupIndex + 1;
            while (iRow < iNumElements)
            {
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
                int iGridRowOrder = dataGridViewRow.Cells[2].Value.ToInt();
                int iGroupBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
                if (iGroupBuildingType == U.iBuilding || iNewRowOrder < iGridRowOrder)
                {
                    General_DataGridView.Rows.Insert(iRow, "", iID.ToString(), iNewRowOrder, iBuildingValueID);
                    General_DataGridView.Rows[iRow].Cells[0].Value = ValueString(iRow, sName, iBuildingType);
                    return;
                }
                iRow++;
            }
            General_DataGridView.Rows.Add("", iID, iNewRowOrder, iBuildingValueID);
            General_DataGridView.Rows[iRow].Cells[0].Value = ValueString(iRow, sName, iBuildingType, iOccupantYear);
        }
        //****************************************************************************************************************************
        private void GetGrandlistOwners(int iBuildingID,
                                        int iDefaultOrder,
                                        ref int iRow)
        {
            DataTable Building_tbl = SQL.GetBuilding(iBuildingID);
            if (Building_tbl.Rows.Count == 0)
                return;
            DataRow Building_row = Building_tbl.Rows[0];
            int iGrandlistID = Building_row[U.BuildingGrandListID_col].ToInt();
            int iOrder = General_DataGridView.Rows.Count;
            DataTable tbl = SQL.GetGrandList(iGrandlistID);
            if (tbl.Rows.Count != 0)
            {
                AddNameToGrid(tbl.Rows[0], iOrder, 0);
                iRow++;
                iOrder++;
            }
            DataTable grandListHistoryTbl = SQL.GetGrandListHistory(iGrandlistID);
            foreach (DataRow grandListHistoryRow in grandListHistoryTbl.Rows)
            {
                int iYear = grandListHistoryRow[U.Year_col].ToInt();
                AddNameToGrid(grandListHistoryRow, iOrder, iYear);
                iRow++;
                iOrder++;
            }
        }
        //****************************************************************************************************************************
        protected void AddNameToGrid(DataRow row, int iOrder, int iYear)
        {
            int iGrandlistID = row[U.GrandListID_col].ToInt();
            string sName1 = row[U.Name1_col].ToString();

            if (sName1.Length != 0)
            {
                string sName2 = row[U.Name2_col].ToString();
                string sGridName = U.CombineName1AndName2(sName1, sName2);
                InsertIntoGridByOrder(iOrder, iGrandlistID, 1, sGridName, U.iCurrentOwners, iYear);
            }
        }
        //****************************************************************************************************************************
        protected override bool NotPrimaryValue(int iOrder)
        {
            return (iOrder != 1);
        }
        //****************************************************************************************************************************
        protected override void GetAnyAdditionalValues(int iDefaultOrder,
                                                       int iBuildingID,
                                                       ref int iRow)
        {
            DataTable Building_tbl = SQL.GetBuilding(iBuildingID);
            if (Building_tbl.Rows.Count == 0)
                return;
            DataRow BuildingRow = Building_tbl.Rows[0];
            GetMapBuildingName(BuildingRow, iBuildingID, U.Building1856Name_col, U.BuildingValueOrder1856Name_col, U.i1856BuildingName, ref iDefaultOrder, ref iRow);
            GetMapBuildingName(BuildingRow, iBuildingID, U.Building1869Name_col, U.BuildingValueOrder1869Name_col, U.i1869BuildingName, ref iDefaultOrder, ref iRow);
            GetOccupantFromBuildingID(iBuildingID, ref iDefaultOrder, ref iRow);
            GetGrandlistOwners(iBuildingID, iDefaultOrder, ref iRow);
        }
        //****************************************************************************************************************************
        protected void GetMapBuildingName(DataRow row,
                                           int iBuildingID,
                                           string buildingMapNameCol,
                                           string buildingValueMapOrder_col,
                                           int iBuildingType,
                                           ref int iDefaultOrder,
                                           ref int iRow)
        {
            string sMapBuildingName = row[buildingMapNameCol].ToString();
            int iOrder = row[buildingValueMapOrder_col].ToInt();
            if (iOrder <= 0)
            {
                iDefaultOrder++;
                iOrder = iDefaultOrder;
                SQL.UpdateBuildingValue(iBuildingID, buildingValueMapOrder_col, iDefaultOrder.ToString());
            }
            if (sMapBuildingName.Length != 0)
            {
                InsertIntoGridByOrder(iOrder, iBuildingID, 1, sMapBuildingName, iBuildingType);
                iRow++;
            }
        }
        //****************************************************************************************************************************
        private void GetOccupantFromBuildingID(int iBuildingID,
                                               ref int iDefaultOrder,
                                               ref int iRow)
        {
            DataTable BuildingOccupant_tbl = SQL.GetAllBuildingOccupants(iBuildingID);
            foreach (DataRow BuildingOccupant_row in BuildingOccupant_tbl.Rows)
            {
                int iPersonID = BuildingOccupant_row[U.PersonID_col].ToInt();
                int iSpouseLivedWithID = BuildingOccupant_row[U.SpouseLivedWithID_col].ToInt();
                int iOrder = BuildingOccupant_row[U.BuildingValueOrder_col].ToInt();
                if (iOrder == 0)
                {
                    iDefaultOrder++;
                    iOrder = iDefaultOrder;
                    SQL.UpdateBuildingOccupant(iPersonID, iBuildingID, U.BuildingValueOrder_col, iDefaultOrder.ToString());
                }
                GetOccupant(true, iPersonID, iBuildingID, iSpouseLivedWithID, iOrder, U.iOccupant, ref iRow, false);
            }
        }
        //****************************************************************************************************************************
        protected override void ChangePropertiesOfGridIfNecessary()
        {
            if (m_bLookingToMerge)
            {
                toolStripItem7.Visible = false;
                this.Location = new System.Drawing.Point(500, 120);
            }
        }
        //****************************************************************************************************************************
        protected override void InitializeGroupObjects()
        {
            if (m_BuildingValueType == U.iBuildingName)
                General_DataGridView.Columns[0].Name = "Building Names";
            else if (m_BuildingValueType == U.iOccupant)
                General_DataGridView.Columns[0].Name = "Building Occupants";
            else if (m_BuildingValueType == U.iCurrentOwners)
                General_DataGridView.Columns[0].Name = "Current Owner";
            else
                General_DataGridView.Columns[0].Name = "Buildings";
            toolStripItem1.Click += new EventHandler(ViewGroup_Click);
            toolStripItem1.Text = "View Photographs by Building";
            ToolStrip.Items.Add(toolStripItem1);
            toolStripItem2.Text = "Delete Building/Building Name/Building Occupant";
            toolStripItem2.Click += new EventHandler(Delete_Click);
            ToolStrip.Items.Add(toolStripItem2);
            toolStripItem3.Text = "New Building, Name or Occupant";
            toolStripItem3.Click += new EventHandler(InsertGroup_Click);
            ToolStrip.Items.Add(toolStripItem3);
            toolStripItem4.Text = "Show Buildings, Names or Occupants";
            toolStripItem4.Click += new EventHandler(ShowBuildingItem_Click);
            ToolStrip.Items.Add(toolStripItem4);
            toolStripItem5.Text = "Modify Notes";
            toolStripItem5.Click += new EventHandler(ModifyNotes_Click);
            ToolStrip.Items.Add(toolStripItem5);
            toolStripItem6.Text = "Edit Occupant";
            toolStripItem6.Click += new EventHandler(EditPersonFromDatabase_click);
            ToolStrip.Items.Add(toolStripItem6);
            toolStripItem7.Text = "Modify Building Name";
            toolStripItem7.Text = "Change Spouse Lived With in Home";
            toolStripItem7.Click += new EventHandler(BuildingItem7_Click);
            ToolStrip.Items.Add(toolStripItem7);
            toolStripItem8.Text = "Show This Building";
            toolStripItem8.Click += new EventHandler(BuildingItem8_Click);
            ToolStrip.Items.Add(toolStripItem8);
            toolStripItem9.Text = "Show All Buildings";
            toolStripItem9.Click += new EventHandler(BuildingItem9_Click);
            ToolStrip.Items.Add(toolStripItem9);
            toolStripItem10.Text = "Building Occupant Census";
            toolStripItem10.Click += new EventHandler(BuildingItem10_Click);
            ToolStrip.Items.Add(toolStripItem10);
        }
        //****************************************************************************************************************************
        protected void ShowBuildingNameOccupantsOptions()
        {
            toolStripItem2.Visible = true;
            if (m_bShowValues)
                toolStripItem3.Visible = false;
            else
                toolStripItem3.Visible = true;
            if (m_iModernRoad < 0)
                toolStripItem4.Visible = true;
            else
                toolStripItem4.Visible = false;
            toolStripItem6.Visible = false;
            toolStripItem7.Visible = true;
            toolStripItem8.Visible = false;
            toolStripItem12.Visible = false;
            if (m_bShowValues)
                toolStripItem9.Visible = true;
            else
                toolStripItem9.Visible = false;
            int iRowIndex = mouseLocation.RowIndex;
            if (IsBuildingValue(iRowIndex))
            {
                toolStripItem8.Visible = false;
                toolStripItem2.Text = "Delete Building";
                toolStripItem7.Text = "Modify Building Name";
                toolStripItem10.Visible = true;
                toolStripItem11.Visible = true;
            }
            else if (IsGroupValue1856BuildingName(iRowIndex))
            {
                toolStripItem2.Text = "Remove 1856 Building Name";
                toolStripItem7.Visible = false;
                toolStripItem8.Visible = true;
                toolStripItem10.Visible = true;
            }
            else if (IsGroupValue1869BuildingName(iRowIndex))
            {
                toolStripItem2.Text = "Remove 1869 Building Name";
                toolStripItem7.Visible = false;
                toolStripItem8.Visible = true;
                toolStripItem10.Visible = true;
            }
            else if (IsGroupValueOccupant(iRowIndex))
            {
                toolStripItem2.Text = "Remove Building Occupant";
                toolStripItem6.Visible = true;
                toolStripItem7.Text = "Change Spouse Lived With in Home";
                toolStripItem8.Visible = true;
                toolStripItem12.Visible = true;
                toolStripItem10.Visible = true;
            }
            else
            {
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
                if (iBuildingType == 1 || iBuildingType == U.iCurrentOwners)
                    toolStripItem2.Visible = false;
                else
                    toolStripItem2.Text = "Delete Building Name";
                toolStripItem7.Visible = false;
                toolStripItem8.Visible = true;
            }
            if (!m_bShowValues)
            {
                toolStripItem8.Visible = false;
            }
        }
        //****************************************************************************************************************************
        protected override void InsertGroup_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            FBuildingItem BuildingItem = new FBuildingItem(m_SQL, "New", Location.X, Location.Y, iRowIndex);
            BuildingItem.ShowDialog();
            m_b1856BuildingName = false;
            m_b1869BuildingName = false;
            switch (BuildingItem.BuildingItem)
            {
                case U.iBuildingName:
                    InsertValue_Click(sender, e);
                    break;
                case U.iOccupant:
                    NewOccupant_Click(sender, e);
                    break;
                case U.i1856BuildingName:
                    GetBuilding1856BuildingName(sender, e);
                    break;
                case U.i1869BuildingName:
                    GetBuilding1869BuildingName(sender, e);
                    break;
                case U.iCurrentOwners:
                    break;
            }
        }
        //****************************************************************************************************************************
        protected override void SelectRightClickOptions()
        {
            toolStripItem1.Visible = false;
            toolStripItem7.Visible = false;
            toolStripItem8.Visible = false;
            toolStripItem9.Visible = false;
            toolStripItem10.Visible = false;
            toolStripItem11.Visible = false;
            toolStripItem12.Visible = false;
            ShowBuildingNameOccupantsOptions();
        }
        //****************************************************************************************************************************
        protected override string GetValueOrderString()
        {
            return U.BuildingValueOrder_col;
        }
        //****************************************************************************************************************************
        protected override void GetSelectedGroupAndValueID(int iRowIndex)
        {
            if (IsGroupValueOccupant(iRowIndex))
            {
                int iPersonID = General_DataGridView.Rows[iRowIndex].Cells[1].Value.ToInt();
                m_iSelectedGroupValueID = General_DataGridView.Rows[iRowIndex].Cells[3].Value.ToInt();
                m_iSelectedGroupID = 0;
            }
            else
            {
                m_iSelectedGroupValueID = General_DataGridView.Rows[iRowIndex].Cells[1].Value.ToInt();
                m_iSelectedGroupID = GetGroupIDFromValueID(m_iSelectedGroupValueID);
            }
        }
        //****************************************************************************************************************************
        protected override bool RowCannotBeModified(string m_sOriginalValue)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            if (IsGroupValueOccupant(m_iEditLocation) ||
                IsGroupValueCurrentOwner(m_iEditLocation))
            {
                MessageBox.Show("The text is composed of information which cannot be modified");
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected override void NullValueEntered()
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            General_DataGridView.Rows.RemoveAt(m_iEditLocation);
        }
        //****************************************************************************************************************************
        protected override int GetGroupValueIDFromGrid(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            if (IsGroupValueOccupant(iRowIndex))
                return dataGridViewRow.Cells[3].Value.ToInt();
            else
                return dataGridViewRow.Cells[1].Value.ToInt();
        }
        //****************************************************************************************************************************
        protected override void GetAllPhotosFromValue(DataTable tbl,
                                                       int iGroupID)
        {
            SQL.GetAllBuildingPhotosFromID(tbl, iGroupID);
        }
        //****************************************************************************************************************************
        protected override void ViewGroup_Click(object sender, EventArgs e)
        {
            if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                return;
            int iRowIndex = mouseLocation.RowIndex;
            int iGroupID = 0;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sInputString = dataGridViewRow.Cells[0].Value.ToString();
            if (sInputString[0] != ' ')
            {
                iGroupID = dataGridViewRow.Cells[1].Value.ToInt();
            }
            else
            {
                int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
                if (iBuildingType == U.i1869BuildingName)
                {
                    iGroupID = dataGridViewRow.Cells[1].Value.ToInt();
                }
                else
                {
                    int iBuildingValueID;
                    if (iBuildingType == U.iOccupant)
                    {
                        iBuildingValueID = dataGridViewRow.Cells[3].Value.ToInt();
                    }
                    else
                    {
                        iBuildingValueID = dataGridViewRow.Cells[1].Value.ToInt();
                    }
                    iGroupID = SQL.GetBuildingIDFromValueID(iBuildingValueID);
                }
            }
            DataTable Photo_tbl = new DataTable(m_PhotoGroupValueTable);
            Photo_tbl.PrimaryKey = new DataColumn[] { Photo_tbl.Columns[U.PhotoID_col] };
            if (iGroupID == 0)
                GetAllGroupValues(Photo_tbl, sInputString);
            else
            {
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
        protected override string GroupValueValue(DataRow row)
        {
            return row[U.BuildingValueValue_col].ToString();
        }
        //****************************************************************************************************************************
        protected override string GroupID(DataRow row)
        {
            return row[U.BuildingID_col].ToString();
        }
        //****************************************************************************************************************************
        protected override string GroupValueID(DataRow row)
        {
            return row[U.BuildingValueID_col].ToString();
        }
        //****************************************************************************************************************************
        protected override string GroupValueOrder(DataRow row)
        {
            int iOrder = row[U.BuildingValueOrder_col].ToInt();
            if (iOrder == U.iOccupant)
            {
                int iBuildingID = row[U.BuildingID_col].ToInt();
                DataTable Building_tbl = SQL.GetBuilding(iBuildingID);
                if (Building_tbl.Rows.Count != 0)
                {
                    DataRow BuildingRow = Building_tbl.Rows[0];
                }
            }
            return iOrder.ToString();
        }
        //****************************************************************************************************************************
        private void PopulateBuildingDataGridView(int iBuildingID)
        {
            DataTable tbl;
            if (iBuildingID == 0)
            {
                tbl = SQL.GetAllBuildings();
            }
            else
            {
                tbl = SQL.DefineBuildingTable();
                tbl = SQL.GetBuilding(iBuildingID);
            }
            int iRow = 0;
            foreach (DataRow row in tbl.Rows)
            {
                string sGroup = SQL.AddRoadToGroup(row[U.BuildingName_col].ToString(), row[U.BuildingRoadValueID_col].ToInt(),
                                row[U.BuildingGrandListID_col].ToInt(), iBuildingID);
                sGroup += " " + row[U.QRCode_col].ToString().Trim();
                General_DataGridView.Rows.Add(sGroup, row[U.BuildingID_col].ToString(), 0, 0, "", row[U.Notes_col].ToString().Notes());
                iRow++;
            }
            m_bValueModeOnly = false;
        }
        //****************************************************************************************************************************
        protected override void PopulateDataGridView()
        {
            if (m_bSingleBuilding)
            {
                if (m_iBuildingID != 0)
                {
                    PopulateBuildingDataGridView(m_iBuildingID);
                    AddValuesToGrid(0);
                }
                else if (!m_bNewBuilding)
                {
                    General_DataGridView.Rows.Insert(0, "", 0, U.iOccupant, 0);
                    m_eInsertMode = InsertMode.InsertGroup;
                    m_iEditLocation = 0;
                }
            }
            else
                PopulateBuildingDataGridView(0);
        }
        //****************************************************************************************************************************
        protected override int GetGroupIDFromValueID(int iGroupValueID)
        {
            return SQL.GetBuildingIDFromBuildingValueID(iGroupValueID);
        }
        //****************************************************************************************************************************
        private void LoadBuildingOccupants(ref int iRow)
        {
            DataTable BuildingOccupant_tbl = SQL.GetAllBuildingOccupants();
            foreach (DataRow BuildingOccupant_row in BuildingOccupant_tbl.Rows)
            {
                int iPersonID = BuildingOccupant_row[U.PersonID_col].ToInt();
                int iBuildingID = BuildingOccupant_row[U.BuildingID_col].ToInt();
                int iOrder = BuildingOccupant_row[U.BuildingValueOrder_col].ToInt();
                int iSpouseLivedWithID = BuildingOccupant_row[U.SpouseLivedWithID_col].ToInt();
                GetOccupant(false, iPersonID, iBuildingID, iSpouseLivedWithID, iOrder, U.iOccupant, ref iRow, true);
            }
        }
        //****************************************************************************************************************************
        private void LoadBuildingNames(ref int iRow)
        {
            DataTable tbl = new DataTable();
            SQL.GetAllBuildingValues(tbl);
            foreach (DataRow row in tbl.Rows)
            {
                string sBuildingValueID = row[U.BuildingValueID_col].ToString();
                string sBuildingValueValue = row[U.BuildingValueValue_col].ToString();
                int iBuildingValueOrder = row[U.BuildingValueOrder_col].ToInt();
                string sNotes = row[U.Notes_col].ToString();
                int iBuildingID = SQL.GetBuildingIDFromValueID(sBuildingValueID.ToInt());
                DataTable buildingTbl = SQL.GetBuilding(iBuildingID);
                if (buildingTbl.Rows.Count == 0)
                    continue;
                DataRow buildingRow = buildingTbl.Rows[0];
                int iGrandListID = buildingRow[U.BuildingGrandListID_col].ToInt();
                int iRoadID = buildingRow[U.BuildingRoadValueID_col].ToInt();
                sBuildingValueValue = SQL.AddRoadToGroup(sBuildingValueValue, sBuildingValueID.ToInt(), iGrandListID);
                string sBuildingName = UU.ShowGroupValue(sBuildingValueValue);
                General_DataGridView.Rows.Add(sBuildingName, sBuildingValueID, iBuildingValueOrder.ToString(),
                                              sBuildingValueID, "", sNotes.Notes());
                iRow++;
            }
        }
        //****************************************************************************************************************************
        protected override void PopulateDataGridViewValues()
        {
            int iRow = 0;
            if (m_BuildingValueType == U.iBuildingName)
                LoadBuildingNames(ref iRow);
            else
                LoadBuildingOccupants(ref iRow);
            SortGrid(GroupColumn, true);
            General_DataGridView.Rows[0].Cells[0].Selected = true;
            toolStripItem8.Text = "Show This Building";
            toolStripItem9.Text = "Show All Buildings";
            m_bValueModeOnly = m_bOriginalValueModeOnly;
            toolStripItem7.Visible = false;
        }
        //****************************************************************************************************************************
        private void SortGrid(DataGridViewColumn newColumn,
                              bool bAlwaysSortAscending)
        {
            // Check which column is selected, otherwise set NewColumn to null.
            newColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            DataGridViewColumn oldColumn = General_DataGridView.SortedColumn;
            ListSortDirection direction;

            // If oldColumn is null, then the DataGridView is not currently sorted.
            if (oldColumn == null || bAlwaysSortAscending)
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                // Sort the same column again, reversing the SortOrder.
                if (oldColumn == newColumn &&
                    General_DataGridView.SortOrder == System.Windows.Forms.SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                }
            }

            // If no column has been selected, display an error dialog  box.
            if (newColumn == null)
            {
                MessageBox.Show("Select a single column and try again.",
                    "Error: Invalid Selection", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                General_DataGridView.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                    direction == ListSortDirection.Ascending ?
                    System.Windows.Forms.SortOrder.Ascending : System.Windows.Forms.SortOrder.Descending;
            }
        }
        //****************************************************************************************************************************
        protected override void GetAllGroupValues(DataTable tbl,
                                                  int iGroupID,
                                                 int iGroupID2 = 0)
        {
            SQL.GetAllBuildingValues(tbl, iGroupID);
        }
        //****************************************************************************************************************************
        protected override void DeleteGroupAndValue(int iGroupID)
        {
            SQL.DeleteBuilding(iGroupID);
        }
        //****************************************************************************************************************************
        protected override string UpdateGroupValue(int iBuildingValueID,
                                                   string sOriginalValue,
                                                   string sNewValue)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            if (iBuildingType == U.i1856BuildingName)
            {
                SQL.UpdateMapBuildingName(iBuildingValueID, sNewValue, U.Building1856Name_col, 0);
                return U.BuildingString(sNewValue, iBuildingType, 0, 0);
            }
            else
            if (iBuildingType == U.i1869BuildingName)
            {
                SQL.UpdateMapBuildingName(iBuildingValueID, sNewValue, U.Building1869Name_col, 0);
                return U.BuildingString(sNewValue, iBuildingType, 0, 0);
            }
            else
            {
                DataTable tbl = new DataTable();
                tbl = SQL.GetBuildingIDValue(iBuildingValueID);
                if (tbl.Rows.Count == 0)
                {
                    MessageBox.Show("Value no longer exists");
                    return sOriginalValue;
                }
                else
                {
                    int iBuildingID = tbl.Rows[0][U.BuildingID_col].ToInt();
                    tbl.Rows[0][U.BuildingValueValue_col] = sNewValue;
                    if (SQL.UpdateBuildingValue(tbl))
                    {
                        int iOrder = tbl.Rows[0][U.BuildingValueOrder_col].ToInt();
                        if (iOrder == 1) // PrimaryBuildingValue
                        {
                            SQL.UpdateBuildingName(iBuildingID, sNewValue.Trim());
                        }
                        return U.BuildingString(sNewValue, iBuildingType, 0, 0);
                    }
                    else
                        return sOriginalValue;
                }
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
                SQL.UpdateBuildingName(iValueID, sNewValue.Trim());
                DataTable BuildingValue_tbl = new DataTable();
                SQL.GetAllBuildingValues(BuildingValue_tbl, iValueID);
                if (BuildingValue_tbl.Rows.Count != 0)
                {
                    DataRow row = BuildingValue_tbl.Rows[0];
                    int iBuildingValueID = row[U.BuildingValueID_col].ToInt();
                    SQL.UpdateBuildingValueField(iBuildingValueID, U.BuildingValueValue_col, sNewValue);
                }
            }
        }
        //****************************************************************************************************************************
        protected override int GetGroupValueID(int iGroupID,
                                              string sGroupValue)
        {
            return SQL.GetBuildingValueID(iGroupID.ToString(), sGroupValue);
        }
        //****************************************************************************************************************************
        protected override int GetGroupIDFromName(string sGroup)
        {
            return SQL.GetBuildingIDFromName(sGroup);
        }
        //****************************************************************************************************************************
        protected override int FirstValueIndex()
        {
            return 2;
        }
        //****************************************************************************************************************************
        protected override int AddValueToDatabase(int iGroupID,
                                                  string sGroupValueValue,
                                                  int iGroupValueOrder)
        {
            if (m_b1856BuildingName)
            {
                SQL.UpdateMapBuildingName(iGroupID, sGroupValueValue, U.Building1856Name_col, 0);

                return iGroupID;
            }
            if (m_b1869BuildingName)
            {
                SQL.UpdateMapBuildingName(iGroupID, sGroupValueValue, U.Building1869Name_col, 0);

                return iGroupID;
            }
            else
            {
                DataTable tbl = new DataTable(U.BuildingValue_Table);
                SQL.GetBuildingIDValue(tbl, iGroupID.ToString(), sGroupValueValue);
                if (tbl.Rows.Count == 0)     // does not exist
                {
                    return SQL.InsertBuildingValue(iGroupID, sGroupValueValue, iGroupValueOrder);
                }
                else
                {
                    return 0;
                }
            }
        }
        //****************************************************************************************************************************
        protected override void Item5_Click(object sender, EventArgs e)
        {
            if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                return;
        }
        //****************************************************************************************************************************
        private void AddOccupantToGrid(int iNextRowIndex,
                                       int iCount,
                                       int iPersonID,
                                       int iBuildingID,
                                       string sHome,
                                       int iOrder,
                                       int iBuildingType)
        {
            if (iNextRowIndex >= iCount)
                General_DataGridView.Rows.Add("", 0, iOrder, 0, iBuildingType);
            else
                General_DataGridView.Rows.Insert(iNextRowIndex, "", 0, iOrder, 0, iBuildingType);
            DataGridViewRow new_dataGridViewRow = General_DataGridView.Rows[iNextRowIndex];
            new_dataGridViewRow.Cells[1].Value = iPersonID;
            new_dataGridViewRow.Cells[3].Value = iBuildingID; // must be set before call to ValueString
            new_dataGridViewRow.Cells[0].Value = ValueString(iNextRowIndex, sHome, iBuildingType);
            new_dataGridViewRow.Cells[0].Selected = true;
        }
        //****************************************************************************************************************************
        private int NewOccupant(bool b1869Occupant,
                                bool b1856Occupant)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow new_dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iOrder = new_dataGridViewRow.Cells[2].Value.ToInt() + 1;
            FPerson Person = new FPerson(m_SQL, false);
            Person.ShowDialog();
            int iPersonID = Person.GetPersonID();
            if (iPersonID != 0)
            {
                int iCount = General_DataGridView.Rows.Count - 1;
                int iNextRowIndex = iRowIndex + 1;
                int iBuildingIndex = RowOfGroupBasedOnValue(iRowIndex);
                DataGridViewRow Building_dataGridViewRow = General_DataGridView.Rows[iBuildingIndex];
                if (!IsGroupValue(iBuildingIndex))
                {
                    int iBuildingID = Building_dataGridViewRow.Cells[1].Value.ToInt();
                    string sBuildingName = Building_dataGridViewRow.Cells[0].Value.ToString();
                    int iSpouseLivedWithID;
                    int iBuildingType;
                    string sHome;
                    if (SQL.PersonLivedInBuilding(iPersonID, iBuildingID))
                    {
                        MessageBox.Show("This person has already been associated with this building");
                        return 0;
                    }
                    else
                    {
                        iBuildingType = U.iOccupant;
                        sHome = SQL.PersonHomeName(iPersonID, iBuildingID, out iSpouseLivedWithID, false);
                        SQL.InsertBuildingOccupant(iPersonID, iSpouseLivedWithID, iBuildingID, iOrder, "", 0);
                        AddOccupantToGrid(iNextRowIndex, iCount, iPersonID, iBuildingID, sHome, iOrder, iBuildingType);
                    }
                }
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private void NewOccupant_Click(object sender, EventArgs e)
        {
            NewOccupant(false, false);
        }
        //****************************************************************************************************************************
        private void BuildingItem8_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            if (m_bShowValues)
            {
                m_bSingleBuilding = true;
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                int iBuildingValueID;
                if (IsGroupValueOccupant(iRowIndex))
                    iBuildingValueID = dataGridViewRow.Cells[3].Value.ToInt();
                else
                    iBuildingValueID = dataGridViewRow.Cells[1].Value.ToInt();
                m_iBuildingID = SQL.GetBuildingIDFromValueID(iBuildingValueID);
                General_DataGridView.Rows.Clear();
                PopulateDataGridView();
                m_bShowValues = false;
            }
        }
        //****************************************************************************************************************************
        private void BuildingItem10_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int personID = dataGridViewRow.Cells[1].Value.ToInt();
            int iBuildingID = dataGridViewRow.Cells[3].Value.ToInt();
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            DataTable occupantTbl = SQL.GetBuildingOccupant(personID, iBuildingID);
            if (occupantTbl.Rows.Count == 0)
                return;
            CCheckedListboxCensus Census_listBox = new CCheckedListboxCensus(m_SQL, occupantTbl, U.BuildingOccupant_Table, personID);
            Census_listBox.ShowDialog();
            Census_listBox.ShowValues_Click();
            Census_listBox.UpdateBuildingOccupantCensusYears(personID, iBuildingID);
            int iSpouseLivedWithID;
            string sHome = SQL.PersonHomeName(personID, iBuildingID, out iSpouseLivedWithID, false);
            dataGridViewRow.Cells[0].Value = UU.ShowGroupValue(U.BuildingString(sHome, iBuildingType, personID, iBuildingID));
            dataGridViewRow.Cells[5].Value = BuildingNotes.GetNotesWithCensusInfo(iBuildingType, personID, iBuildingID);
        }
        //****************************************************************************************************************************
        private void BuildingItem9_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            if (m_bShowValues)
            {
                m_bSingleBuilding = false;
                General_DataGridView.Rows.Clear();
                PopulateDataGridView();
                m_bShowValues = false;
            }
            /* Leave code here Just in case it is still required
            else
            {
                if (IsGroupValue(iRowIndex))
                    iRowIndex = RowOfGroupBasedOnValue(iRowIndex);
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                int iGroupID = dataGridViewRow.Cells[1].Value.ToInt();
                UpdateRoadValue(dataGridViewRow, iGroupID);
            }
             * */
        }
        //****************************************************************************************************************************
        private void GetBuilding1856BuildingName(object sender, EventArgs e)
        {
            m_b1856BuildingName = GetBuildingMapBuildingName(U.Building1856Name_col, "1856");
            InsertValue_Click(sender, e);
        }
        //****************************************************************************************************************************
        private void GetBuilding1869BuildingName(object sender, EventArgs e)
        {
            m_b1869BuildingName = GetBuildingMapBuildingName(U.Building1869Name_col, "1869");
            InsertValue_Click(sender, e);
        }
        //****************************************************************************************************************************
        private bool GetBuildingMapBuildingName(string BuildingMapNameCol, string title)
        {
            int iRowIndex = mouseLocation.RowIndex;
            int iBuildingRowIndex = GetRowOfGroupIDFromGrid(iRowIndex);
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iBuildingRowIndex];
            int iBuildingID = dataGridViewRow.Cells[1].Value.ToInt();
            string sBuildingMapOccupant = SQL.GetBuildingMapBuildingName(iBuildingID, BuildingMapNameCol);
            if (sBuildingMapOccupant.Length != 0)
            {
                string sMessage = "There is already an " + title + " Building Name defined (" + sBuildingMapOccupant + 
                                  ").  Do you wish to define a new " + title + " Building Name";
                if (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;
            }
            string sInputString = dataGridViewRow.Cells[0].Value.ToString();
            return true;
        }
        //****************************************************************************************************************************
        private void RemoveOccupantFromGrid(int iRowIndex,
                                            int iPersonID)
        {
            int iFirstRowOfGroup = GetRowOfGroupIDFromGrid(iRowIndex);
            int iLastRowOfGroup = GetRowOfLastGroupValueFromGrid(iRowIndex);
            for (int i=iFirstRowOfGroup;i < iLastRowOfGroup;i++)
            {
                DataGridViewRow row = General_DataGridView.Rows[i];
                int iBuildingType = row.Cells[4].Value.ToInt();
                if (iBuildingType == U.iOccupant)
                {
                    int iThisPersonID = row.Cells[1].Value.ToInt();
                    if (iThisPersonID == iPersonID)
                    {
                        RemovePersonFromGrid(i);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void RemovePersonFromGrid(int iRowIndex)
        {
            General_DataGridView.Rows.RemoveAt(iRowIndex);
            m_iNumGridElements--;
        }
        //****************************************************************************************************************************
        private void BuildingItem7_Click(object sender, EventArgs e)
        {
            if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                return;
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sGroupValue = dataGridViewRow.Cells[0].Value.ToString();
            if (IsGroupValueOccupant(iRowIndex))
            {
                int iPersonID = dataGridViewRow.Cells[1].Value.ToInt();
                int iBuildingID = dataGridViewRow.Cells[3].Value.ToInt();
                int iOldSpouseLivedWithID = SQL.GetSpouseLivedWith(iPersonID, iBuildingID);

                FSpouseLivedWith SpouseLivedWith = new FSpouseLivedWith(m_SQL, iPersonID, iBuildingID, iOldSpouseLivedWithID);
                SpouseLivedWith.ShowDialog();
                int iSpouseLivedWithID = SpouseLivedWith.GetSelectedSpouse();
                if (iSpouseLivedWithID >= 0 && iOldSpouseLivedWithID != iSpouseLivedWithID)
                {
                    string sHome = SQL.PersonHomeName(iPersonID, iBuildingID, iSpouseLivedWithID, false);
                    dataGridViewRow.Cells[0].Value = UU.ShowGroupValue(sHome);
                    SQL.UpdateBuildingOccupant(iPersonID, iBuildingID, U.SpouseLivedWithID_col, iSpouseLivedWithID);
                    if (iSpouseLivedWithID != 0)
                    {
                        SQL.DeleteBuildingOccupants(iSpouseLivedWithID, iBuildingID);
                        RemoveOccupantFromGrid(iRowIndex, iSpouseLivedWithID);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        public string GetNewBuildingName()
        {
            if (General_DataGridView.Rows[0].Cells[0].Value == null)
            {
                return "";
            }
            return General_DataGridView.Rows[0].Cells[0].Value.ToString();
        }
        //****************************************************************************************************************************
        public void EditPersonFromDatabase_click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iPersonID = dataGridViewRow.Cells[1].Value.ToInt();
            FPerson Person = new FPerson(m_SQL, iPersonID, false);
            Person.ShowDialog();
        }
        //****************************************************************************************************************************
        private void ModifyNotes_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iID = dataGridViewRow.Cells[1].Value.ToInt();
            int iBuildingID = dataGridViewRow.Cells[3].Value.ToInt();
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            FNotes Notes = new FNotes(m_SQL, iBuildingType, iID, iBuildingID, Location.X, Location.Y, 
                                                              mouseLocation.RowIndex);
            Notes.ShowDialog();
            dataGridViewRow.Cells[5].Value = Notes.sNotes.Notes();
        }
        //****************************************************************************************************************************
        private void ShowBuildingItem_Click(object sender, EventArgs e)
        {
            m_bSingleBuilding = false;
            if (m_eInsertMode != InsertMode.InsertGroup && m_eInsertMode != InsertMode.InsertGroupValue)
            {
                CurrentGroupIndex = 0;
                NextGroupIndex = 0;
                int iRowIndex = mouseLocation.RowIndex;
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                FBuildingItem BuildingItem = new FBuildingItem(m_SQL, "Show", Location.X, Location.Y, mouseLocation.RowIndex);
                BuildingItem.ShowDialog();
                switch (BuildingItem.BuildingItem)
                {
                    case U.iBuilding:
                        General_DataGridView.Columns[0].Name = "Buildings";
                        m_bShowValues = true;
                        m_bValueModeOnly = false;
                        m_bSelectionModeOnly = m_bValueModeOnly;
                        m_bOriginalValueModeOnly = m_bValueModeOnly;
                        m_bLookingToMerge = false;
                        ShowAllValues("");
                        break;
                    case U.iBuildingName:
                        General_DataGridView.Columns[0].Name = "Building Names";
                        m_bShowValues = false;
                        m_BuildingValueType = U.iBuildingName;
                        ShowAllValues("");
                        break;
                    case U.iOccupant:
                        General_DataGridView.Columns[0].Name = "Building Occupants";
                        m_bShowValues = false;
                        m_BuildingValueType = U.iOccupant;
                        ShowAllValues("");
                        break;
                    default:
                        break;
                }
            }
        }
        //****************************************************************************************************************************
        protected override int AddGroupToDatabase(ref string sNewGroup)
        {
            if (SQL.GetBuildingIDFromName(sNewGroup) == 0) // does not exist
            {
                int iValueID = m_iModernRoad;
                if (m_iModernRoad == 0)
                    iValueID = GetModernRoad();
                int iBuildingID = SQL.InsertBuilding(sNewGroup.Trim(), 0, m_GrandListIdForNewBuilding);
                if (iBuildingID > 0)
                {
                    DataTable tbl = SQL.GetBuilding(iBuildingID);
                    int iGrandListID = tbl.Rows[0][U.BuildingGrandListID_col].ToInt();
                    sNewGroup = SQL.AddRoadToGroup(sNewGroup, iValueID, iGrandListID);
                }
                return iBuildingID;
            }
            return 0;
        }
        //****************************************************************************************************************************
        protected override bool DeleteValue(DataGridViewRow dataGridViewRow)
        {
            bool bSuccess;
            int iValueID = dataGridViewRow.Cells[1].Value.ToInt();
            int iOrder = dataGridViewRow.Cells[2].Value.ToInt();
            int iBuildingType = dataGridViewRow.Cells[4].Value.ToInt();
            if (iBuildingType == U.i1856BuildingName)
            {
                int iBuildingID = dataGridViewRow.Cells[1].Value.ToInt();
                SQL.UpdateMapBuildingName(iBuildingID, "", U.Building1856Name_col, 0);
                bSuccess = true;
            }
            else if (iBuildingType == U.i1869BuildingName)
            {
                int iBuildingID = dataGridViewRow.Cells[1].Value.ToInt();
                SQL.UpdateMapBuildingName(iBuildingID, "", U.Building1869Name_col, 0);
                bSuccess = true;
            }
            else if (iBuildingType == U.iOccupant)
            {
                int iPersonID = dataGridViewRow.Cells[1].Value.ToInt();
                int iBuildingID = dataGridViewRow.Cells[3].Value.ToInt();
                return SQL.DeleteBuildingOccupants(iPersonID, iBuildingID);
            }
            else if (iBuildingType == U.iCurrentOwners)
                bSuccess = true;
            else
            {
                if (iOrder == 1)
                {
                    MessageBox.Show("You Cannot Delete The Primary Building");
                    return false;
                }
                else
                {
                    return SQL.DeleteBuildingValue(iValueID);
                }
            }
            return bSuccess;
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (m_bSingleBuilding)
                m_iSelectedGroupID = General_DataGridView.Rows[0].Cells[1].Value.ToInt();
            if (!m_bShowValues && !m_bNewBuilding)
                UpdateOrder();
        }
        //****************************************************************************************************************************
        protected override void SelectSpouse_Click(object sender, System.EventArgs e)
        {
            int iSelectedIndex = Building_Listbox.SelectedIndex;
        }
        //****************************************************************************************************************************
    }
}
