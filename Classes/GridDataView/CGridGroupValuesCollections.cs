using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    class CGridGroupValuesCollection : CGridGroupValue
    {
        //****************************************************************************************************************************
        public CGridGroupValuesCollection(CSql SQL)
            : base(SQL, "")
        {
            m_bUseDoubleClick = false;
        }
        //****************************************************************************************************************************
        protected override void InitializeGroupObjects()
        {
            General_DataGridView.Columns[0].Name = "Collections";
            toolStripItem1.Click += new EventHandler(ViewGroup_Click);
            toolStripItem1.Text = "View Photographs by Collection";
            ToolStrip.Items.Add(toolStripItem1);
        }
        //****************************************************************************************************************************
        protected override void PopulateDataGridView()
        {
            DataTable tbl = new DataTable();
            SQL.GetAllCollections(tbl);
            int iRow = 0;
            General_DataGridView.Rows.Clear();
            foreach (DataRow row in tbl.Rows)
            {
                General_DataGridView.Rows.Add(row[U.PhotoSource_col].ToString(), "", 0);
                iRow++;
            }
        }
        //****************************************************************************************************************************
        protected override void ViewGroup_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataTable Photo_tbl = new DataTable(m_PhotoGroupValueTable);
            Photo_tbl.PrimaryKey = new DataColumn[] { Photo_tbl.Columns[U.PhotoID_col] };
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sInputString = dataGridViewRow.Cells[0].Value.ToString();
            GetAllGroupValues(Photo_tbl, sInputString);
            if (Photo_tbl.Rows.Count == 0)
            {
                return;
            }
            FPhotoViewer PhotoViewer = new FPhotoViewer(ref m_SQL, Photo_tbl, m_PhotoGroupValueTable, FHistoricJamaica.RunPhotoSlideShow);
            PhotoViewer.ShowDialog();
        }
        //****************************************************************************************************************************
        protected override void GetAllGroupValues(DataTable tbl,
                                                  string sGroupID)
        {
            SQL.GetPhotosFromCollection(tbl, sGroupID);
        }
        //****************************************************************************************************************************
        protected override void GetAllPhotosFromValue(DataTable tbl,
                                                       int iGroupValueID)
        {
        }
        protected override string GetValueOrderString()
        {
            return "";
        }
        //****************************************************************************************************************************
        protected override string GroupValueValue(DataRow row)
        {
            return "";
        }
        //****************************************************************************************************************************
        protected override string GroupValueID(DataRow row)
        {
            return "";
        }
        //****************************************************************************************************************************
        protected override string GroupID(DataRow row)
        {
            return "";
        }
        //****************************************************************************************************************************
        protected override string GroupValueOrder(DataRow row)
        {
            return "";
        }
        //****************************************************************************************************************************
        protected override void DeleteGroupAndValue(int iGroupID) 
        {
        }
        //****************************************************************************************************************************
        protected override string UpdateGroupValue(int    iID,
                                                   string sOriginalValue,
                                                   string sNewValue)
        {
            return "";
        }
        //****************************************************************************************************************************
        protected override void UpdateGroup(int iValueID,
                                            string sOriginalValue,
                                            string sNewValue)
        {
            if (sNewValue != sOriginalValue)
            {
                DataTable tbl = new DataTable();
                SQL.GetPhotosFromCollection(tbl, sOriginalValue);
                foreach (DataRow row in tbl.Rows)
                {
                    row[U.PhotoSource_col] = sNewValue;
                }
                if (!SQL.UpdatePhotoField(tbl, U.PhotoSource_col))
                    MessageBox.Show("Save Unsuccessful");
                PopulateDataGridView();
            }
        }
        //****************************************************************************************************************************
        protected override void Item5_Click(object sender, EventArgs e)
        {
        }
        //****************************************************************************************************************************
        protected override int GetGroupValueID(int iGroupID,
                                              string sGroupValue)
        {
            return 0;
        }
        //****************************************************************************************************************************
        protected override int GetGroupValueOrder(int iGroupID,
                                                  string sPreviousGroupValue)
        {
            return 0;
        }
        //****************************************************************************************************************************
        protected override int GetGroupIDFromName(string sGroup)
        {
            return 0;
        }
        //****************************************************************************************************************************
        protected override int AddGroupToDatabase(ref string sNewGroup)
        {
            return 0;
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
        }
    }
}
