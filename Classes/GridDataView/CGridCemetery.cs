using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    class CGridCemetery : FGrid
    {
        private CSql m_SQL;
        private DataTable m_tbl;
        private int m_iNumElements = 0;
        private int m_SelectedPersonID = 0;
        //****************************************************************************************************************************
        public int SelectedPersonID
        {
            get { return m_SelectedPersonID; }
        }
        //****************************************************************************************************************************
        public CGridCemetery(CSql SQL,
                                     DataTable tbl)
        {
            m_SQL = SQL;
            m_tbl = tbl;
            buttonPane.Visible = false;
            Abort_Button.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            int iSelectedRow = General_DataGridView.SelectedRows[0].Index;
            if (iSelectedRow >= m_iNumElements)
                m_SelectedPersonID = 0;
            else
            {
                DataGridViewRow s = General_DataGridView.Rows[iSelectedRow];
                m_SelectedPersonID = s.Cells[13].Value.ToInt();
            }
            Close();
        }
        //****************************************************************************************************************************
        private int GridHeight()
        {
            if (m_iNumGridElements > 18)
                return 60;
            else 
                return 422;
        }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
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

            General_DataGridView.ColumnCount = 14;
            General_DataGridView.Columns[0].Name = "Plot #";
            General_DataGridView.Columns[0].Width = 70;
            General_DataGridView.Columns[1].Name = "Person Name";
            General_DataGridView.Columns[1].Width = 150;
            General_DataGridView.Columns[2].Name = "";
            General_DataGridView.Columns[2].Width = 16;
            General_DataGridView.Columns[3].Name = "Father Name";
            General_DataGridView.Columns[3].Width = 150;
            General_DataGridView.Columns[4].Name = "Mother Name";
            General_DataGridView.Columns[4].Width = 150;
            General_DataGridView.Columns[5].Name = "Spouse Name";
            General_DataGridView.Columns[5].Width = 150;
            General_DataGridView.Columns[6].Name = "Birth";
            General_DataGridView.Columns[6].Width = 70;
            General_DataGridView.Columns[7].Name = "Death";
            General_DataGridView.Columns[7].Width = 70;
            General_DataGridView.Columns[8].Name = "Age";
            General_DataGridView.Columns[8].Width = 90;
            General_DataGridView.Columns[9].Name = "Epitaph";
            General_DataGridView.Columns[10].Name = "Note";
            General_DataGridView.Columns[11].Name = "Note";
            General_DataGridView.Columns[12].Name = "Note";
            General_DataGridView.Columns[13].Name = "PersonID";

        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            General_DataGridView.Rows.Clear();
            m_iNumElements = 0;
            this.Text = "";
            if (m_tbl.Rows.Count != 0)
            {
                this.Text = SQL.GetCemeteryName(m_tbl.Rows[0][U.CemeteryID_col].ToInt());
            }
            this.Text += " Cemetery";
            foreach (DataRow row in m_tbl.Rows)
            {
                General_DataGridView.Rows.Add(row[U.BuriedStone_col],row[U.PersonName_col], row[U.Sex_col],
                                              row[U.FatherName_col], row[U.MotherName_col], row[U.SpouseName_col],
                                              row[U.BornDate_col], row[U.DiedDate_col],
                                              row[U.PersonAge_col], row[U.Epitaph_col], row[U.CemeteryNote1_col],
                                              row[U.CemeteryNote2_col], row[U.CemeteryNote3_col], row[U.PersonID_col]);
                m_iNumElements++;
            }
            m_iNumGridElements = m_tbl.Rows.Count;
            this.Location = new System.Drawing.Point(20, GridHeight());
            SetSizeOfGrid(1300);
        }
        //****************************************************************************************************************************
    }
}
