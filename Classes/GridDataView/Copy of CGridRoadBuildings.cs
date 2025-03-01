using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    public class CGridRoadBuildings : FGrid
    {
        private CSql m_SQL;
        private Panel buttonPanel = new Panel();
        private int m_iModernRoadID;
        private int m_iSelectedRow = -1;
        private int m_iSelectedCol = -1;
        ContextMenuStrip strip = new ContextMenuStrip();
        ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        private int m_iSelectedIDLocation = U.Exception;
        private DataGridViewCellEventArgs mouseLocation;
        //****************************************************************************************************************************
        public int SelectedRow
        {
            get { return m_iSelectedRow; }
        }
        //****************************************************************************************************************************
        public CGridRoadBuildings(ref CSql SQL,
                                  int iModernRoadID)
        {
            m_SQL = SQL;
            m_iModernRoadID = iModernRoadID;
            buttonPane.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            m_iSelectedRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            string s = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToString();
            this.Close();
        }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Controls.Add(General_DataGridView);
            General_DataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            General_DataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            General_DataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(General_DataGridView.Font, FontStyle.Bold);
            General_DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            General_DataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            General_DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            General_DataGridView.GridColor = Color.Black;
            General_DataGridView.RowHeadersVisible = false;
            General_DataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            General_DataGridView.MultiSelect = false;
            General_DataGridView.Dock = DockStyle.Fill;
            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);

            General_DataGridView.ColumnCount = 4;
            General_DataGridView.Columns[0].Name = "Name";
            General_DataGridView.Columns[0].Width = 250;
            General_DataGridView.Columns[1].Name = "Approximate Year";
            General_DataGridView.Columns[1].Width = 122;
            General_DataGridView.Columns[2].Name = "Notes";
            General_DataGridView.Columns[2].Width = 700;
            General_DataGridView.Columns[3].Name = U.PhotoID_col;
            General_DataGridView.Columns[3].Visible = false;
            m_iSelectedIDLocation = 3;

            General_DataGridView.CellDoubleClick += dataGridView_CellMouseEnter;
            toolStripItem1.Text = "Delete Photo From Database";
            toolStripItem1.Click += new EventHandler(DeletePhoto_Click);
            strip.Items.Add(toolStripItem1);
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = strip;
            }
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
        //****************************************************************************************************************************
        private void RemovePhotoFromGrid(int iRowIndex)
        {
            General_DataGridView.Rows.RemoveAt(iRowIndex);
            m_iNumGridElements--;
            SetSizeOfGrid(U.iMaxSizeOfGrid);
        }
        //****************************************************************************************************************************
        private void DeletePhoto_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iPhotoID = dataGridViewRow.Cells[m_iSelectedIDLocation].Value.ToInt();
            if (m_SQL.DeleteFromTable(U.Photo_Table, U.PhotoID_col, iPhotoID) != 0)
            {
                RemovePhotoFromGrid(iRowIndex);
            }
        }
        //****************************************************************************************************************************
        private bool IsOdd(int iNum)
        {
            return (iNum % 2 == 1);
        }
        //****************************************************************************************************************************
        private string GetNextRow(DataRow row,
                                  bool    bLookingFor)
        {
            int iAddress = row[U.StreetNum_col].ToInt();
            bool bIs = IsOdd(iAddress);
            if (bLookingFor == bIs)
            {
                return row[U.StreetNum_col].ToInt() + " " + row[U.StreetName_col].ToString() + "--" +
                       row[U.Name1_col].ToString();
            }
            else
                return "";
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues()
        {
            DataTable Building_tbl = Q.t(m_SQL, m_SQL.GetGuildingByModernRoadID(m_iModernRoadID));
            int iNumBuildings = Building_tbl.Rows.Count;
            if (iNumBuildings == 0)
                return;
            int iCurrentRow = 0;
            DataRow row = Building_tbl.Rows[iCurrentRow];
            while (row[U.StreetNum_col].ToInt() == 0)
            {
                iCurrentRow++;
                if (iCurrentRow >= iNumBuildings)
                    return;
                row = Building_tbl.Rows[iCurrentRow];
            }
            m_iNumGridElements = 0;
            while (iCurrentRow < iNumBuildings)
            {
                row = Building_tbl.Rows[iCurrentRow];
                string sEvenAddress = "";
                string OddAddress = GetNextRow(row, true);
                if (OddAddress.Length != 0)
                {
                    iCurrentRow++;
                    if (iCurrentRow < iNumBuildings)
                        row = Building_tbl.Rows[iCurrentRow];
                }
                if (iCurrentRow < iNumBuildings)
                {
                    sEvenAddress = GetNextRow(row, false);
                    if (sEvenAddress.Length != 0)
                    {
                        iCurrentRow++;
                        if (iCurrentRow < iNumBuildings)
                            row = Building_tbl.Rows[iCurrentRow];
                    }
                }
                General_DataGridView.Rows.Add(OddAddress, sEvenAddress, "", 0);
                m_iNumGridElements++;
            }
            SetSizeOfGrid(1082);
        }
    }
}
