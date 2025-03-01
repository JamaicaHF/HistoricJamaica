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
    class CGridGroupValuesModernRoads : CGridGroupValue
    {
        private bool m_bOriginalValueModeOnly = false;
        private int m_iSelectedBuildingID = 0;
        private bool m_bExitOnDoubleClick = false;
        private int m_iBuildingID = 0;
        //****************************************************************************************************************************
        public CGridGroupValuesModernRoads(CSql SQL,
                         int  iBuildingID,
                         bool bValueModeOnly,
                         bool bLookingToMerge,
                         bool bExitOnDoubleClick): base(SQL)
        {
            m_iBuildingID = iBuildingID;
            m_bShowValues = false;
            m_bValueModeOnly = bValueModeOnly;
            m_bSelectionModeOnly = m_bValueModeOnly;
            m_bOriginalValueModeOnly = m_bValueModeOnly;
            m_bLookingToMerge = bLookingToMerge;
            m_bExitOnDoubleClick = bExitOnDoubleClick;
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
            General_DataGridView.Columns[0].Name = "Modern Roads";
            toolStripItem1.Click += new EventHandler(ViewGroup_Click);
            toolStripItem1.Text = "View Photographs by Modern Road";
            ToolStrip.Items.Add(toolStripItem1);
            toolStripItem2.Click += new EventHandler(Delete_Click);
            toolStripItem2.Text = "Delete Road";
            ToolStrip.Items.Add(toolStripItem2);
            toolStripItem4.Click += new EventHandler(InsertValue_Click);
            toolStripItem4.Text = "New Road";
            ToolStrip.Items.Add(toolStripItem4);
            toolStripItem5.Click += new EventHandler(Item5_Click);
            toolStripItem5.Text = "Show Houses on Road";
            ToolStrip.Items.Add(toolStripItem5);
            toolStripItem6.Click += new EventHandler(Item6_Click);
            toolStripItem6.Text = "Make Historic Road";
            ToolStrip.Items.Add(toolStripItem6);
        }
        //****************************************************************************************************************************
        protected override void GetAllPhotosFromValue(DataTable tbl,
                                                       int iGroupValueID) 
        {
        }
        //****************************************************************************************************************************
        protected override void PopulateDataGridViewValues()
        {
            DataTable tbl = SQL.GetModernRoads();
            int iRow = 0;
            foreach (DataRow row in tbl.Rows)
            {
                iRow++;
                General_DataGridView.Rows.Add(UU.ShowGroupValue(row[U.ModernRoadValueValue_col].ToString()),
                                              row[U.ModernRoadValueID_col].ToInt(), iRow);
            }
            General_DataGridView.Rows[0].Cells[0].Selected = true;
            m_bValueModeOnly = m_bOriginalValueModeOnly;
            toolStripItem4.Visible = true;
            toolStripItem3.Visible = false;
            toolStripItem5.Visible = true;
            toolStripItem7.Visible = true;
        }
        //****************************************************************************************************************************
        public int SelectedBuilding
        {
            get { return m_iSelectedBuildingID; }
        }
        //****************************************************************************************************************************
        protected override void CloseIfValueSelected()
        {
            if (m_iBuildingID != 0)
            {
                int iSelectedBuildID = GetBuilding();
                if (iSelectedBuildID > 0)
                {
                    m_iSelectedGroupID = iSelectedBuildID;
                    Close();
                }
            }
        }
        //****************************************************************************************************************************
        protected int AddValueToDatabasexx(int iGroupID, //override
                                                  string sModernRoadValueValue,
                                                  ref int iGroupValueOrder)
        {
//            if (SQL.GetModernRoadIDFromValue(sModernRoadValueValue) == 0) // does not exist
//            {
//                int iModernRoadValueID = SQL.InsertModernRoad(iGroupValueOrder,sModernRoadValueValue);
//                return iModernRoadValueID;
//            }
            return 0;
        }
        //****************************************************************************************************************************
        protected override bool DeleteGroupValue(int iValueID)
        {
            return SQL.DeleteModernRoadValueCommand(iValueID);
        }
        //****************************************************************************************************************************
        private int GetBuilding()
        {
            int iModernRoadID = SQL.GetBuildingRoadValueID(m_iBuildingID);
            if (iModernRoadID == 0)
                return 0;
            m_bExitOnDoubleClick = true;
            CGridRoadBuildings GridRoadBuildings = new CGridRoadBuildings(ref m_SQL, iModernRoadID, m_iBuildingID, m_bExitOnDoubleClick);
            GridRoadBuildings.ShowDialog();
            m_iSelectedBuildingID = GridRoadBuildings.SelectedRow;
            return m_iSelectedBuildingID;
        }
        //****************************************************************************************************************************
        protected override void InsertGroupValue(string sInputString)
        {
            int iOrder = 0;
            General_DataGridView.Rows[m_iEditLocation].Cells[1].Value = AddValueToDatabasexx(0, sInputString, ref iOrder);
            string sValue = UU.ShowGroupValue(sInputString);
            General_DataGridView.Rows[m_iEditLocation].Cells[0].Value = sValue;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            Item5_Click(sender, e);
        }
        //****************************************************************************************************************************
        protected override void PopulateDataGridView()
        {
            toolStripItem4.Visible = false;
            toolStripItem3.Visible = true;
        }
        //****************************************************************************************************************************
        protected void Item6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Wish To Make An Historic Road", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int iRowIndex = mouseLocation.RowIndex;
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                int iModernRoadID = dataGridViewRow.Cells[1].Value.ToInt();
                SQL.UpdateModernRoadValue(iModernRoadID, U.HistoricRoad_col, "Y");
            }
        }
        //****************************************************************************************************************************
        protected override void Item5_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iModernRoadID = dataGridViewRow.Cells[1].Value.ToInt();
            CGridRoadBuildings GridRoadBuildings = new CGridRoadBuildings(ref m_SQL, iModernRoadID, 0, m_bExitOnDoubleClick);
            GridRoadBuildings.ShowDialog();
            m_iSelectedBuildingID = GridRoadBuildings.SelectedRow;
            if (m_bExitOnDoubleClick)
                Close();
        }
        //****************************************************************************************************************************
    }
}
