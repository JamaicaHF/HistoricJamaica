    using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    class CGridAlternativeSpellings : CGrid // Base Class for when grid is used to add and delete values
    {
        private DataTable m_tbl;
        private string m_sTable;
        private ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        private ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
        private ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
        private ContextMenuStrip strip = new ContextMenuStrip();
        private string m_sOriginalName1 = "";
        private string m_sOriginalName2 = "";
        //****************************************************************************************************************************
        public CGridAlternativeSpellings(string sTable)
        {
            m_sTable = sTable;
            m_tbl = SQL.GetAllAlternativeSpellings(sTable);
            buttonPane.Visible = false;
            Abort_Button.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
//            int iSelectedRow = General_DataGridView.SelectedRows[0].Index;
//            Close();
        }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
            this.Location = new System.Drawing.Point(400, 80);
            this.Size = new Size(300, 750);
            this.Text = "    Alternative Spellings";
            this.BackColor = Color.LightSteelBlue;
            this.Controls.Add(General_DataGridView);

            General_DataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            General_DataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            General_DataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(General_DataGridView.Font, FontStyle.Bold);
            General_DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            General_DataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            General_DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            General_DataGridView.DefaultCellStyle.BackColor = Color.LightSteelBlue;
            General_DataGridView.GridColor = Color.Black;
            General_DataGridView.RowHeadersVisible = false;
            General_DataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            General_DataGridView.MultiSelect = false;
            General_DataGridView.Dock = DockStyle.Fill;
//            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);
            General_DataGridView.ColumnHeadersVisible = true;
            DataGridViewTextBoxColumn NameColumn = new DataGridViewTextBoxColumn();
            NameColumn.MaxInputLength = U.iMaxNameLength;
            NameColumn.HeaderText = "Name";
            NameColumn.Width = 136;
            General_DataGridView.Columns.Add(NameColumn);
            DataGridViewTextBoxColumn AlternateNameColumn = new DataGridViewTextBoxColumn();
            AlternateNameColumn.MaxInputLength = U.iMaxNameLength;
            AlternateNameColumn.HeaderText = "Alternate Name";
            AlternateNameColumn.Width = 136;
            General_DataGridView.Columns.Add(AlternateNameColumn);
            General_DataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick_click);

            General_DataGridView.CellMouseEnter += dataGridView_CellMouseEnter;
            toolStripItem1.Text = "New Alternative Spelling";
            toolStripItem1.Click += new EventHandler(NewAlternativeSpellings_Click);
            toolStripItem2.Text = "Another Alternative Spelling for Current Name";
            toolStripItem2.Click += new EventHandler(AmotherAlternativeSpellings_Click);
            toolStripItem3.Text = "Delete Alternative Spelling";
            toolStripItem3.Click += new EventHandler(DeleteAlternativeSpellings_Click);
            strip.Items.Add(toolStripItem1);
            strip.Items.Add(toolStripItem2);
            strip.Items.Add(toolStripItem3);
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = strip;
            }
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            General_DataGridView.Rows.Clear();
            foreach (DataRow row in m_tbl.Rows)
            {
                General_DataGridView.Rows.Add(row[U.NameSpelling1_Col], row[U.NameSpelling2_Col]);
            }
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
        //****************************************************************************************************************************
        private bool NameCombinationExists(string sName1,
                                           string sName2)
        {
            if (SQL.AlternativeSpellingAlreadyExists(m_sTable, sName1, sName2))
            {
                MessageBox.Show("This Alternative Spelling Already Exists");
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private void ModifyValue()
        {
            int iPreviousRowIndex = m_iEditLocation + 1;
            General_DataGridView.Rows[iPreviousRowIndex].Cells[0].Selected = true;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            if (dataGridViewRow.Cells[0].Value == null)
            {
                dataGridViewRow.Cells[0].Value = m_sOriginalName1;
                dataGridViewRow.Cells[1].Value = m_sOriginalName2;
            }
            else
            {
                string sName1 = dataGridViewRow.Cells[0].Value.SetNameForDatabase();
                string sName2 = dataGridViewRow.Cells[1].Value.SetNameForDatabase();
                if (sName1.Length != 0 && sName2.Length != 0)
                    SQL.UpdateAlternativeSpelling(m_sTable, m_sOriginalName1, m_sOriginalName2, sName1, sName2);
                else
                {
                    dataGridViewRow.Cells[0].Value = m_sOriginalName1;
                    dataGridViewRow.Cells[1].Value = m_sOriginalName2;
                }
            }
            int iRowIndex = mouseLocation.RowIndex;
            General_DataGridView.Rows[iRowIndex].Cells[0].Selected = true;
            m_eInsertMode = InsertMode.NotInsertEditMode;
            m_sOriginalName1 = "";
            m_sOriginalName2 = "";
        }
        //****************************************************************************************************************************
        private void InsertValue()
        {
            if (m_iEditLocation < 0)
            {
                return;
            }
            if (m_iEditLocation > 0)
            {
                int iPreviousRowIndex = m_iEditLocation - 1;
                General_DataGridView.Rows[iPreviousRowIndex].Cells[0].Selected = true;
            }
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            string sName1 = dataGridViewRow.Cells[0].Value.SetNameForDatabase();
            string sName2 = dataGridViewRow.Cells[1].Value.SetNameForDatabase();
            if (sName1.Length == 0 || sName2.Length == 0)
            {
                General_DataGridView.Rows.RemoveAt(m_iEditLocation);
                return;
            }
            General_DataGridView.Rows[m_iEditLocation].Cells[0].Selected = true;
            if (NameCombinationExists(sName1, sName2))
            {
                General_DataGridView.Rows.RemoveAt(m_iEditLocation);
                return;
            }
            SQL.InsertAlternativeSpelling(m_sTable, sName1, sName2);
        }   
        //****************************************************************************************************************************
        protected override void CheckValue()
        {
            if (m_eInsertMode == InsertMode.EditGroupValue || m_eInsertMode == InsertMode.EditGroup)
                ModifyValue();
            else
            if (m_eInsertMode != InsertMode.NotInsertEditMode)
            {
                InsertValue();
                m_eInsertMode = InsertMode.NotInsertEditMode;
            }
        }
        //****************************************************************************************************************************
        protected override void SetToEditMode()
        {
            if (m_eInsertMode == InsertMode.NotInsertEditMode)
            {
                m_iEditLocation = m_iCurrentRow;
                m_iEditLocation = m_iCurrentRow;
                m_sOriginalName1 = General_DataGridView.Rows[m_iCurrentRow].Cells[0].Value.ToString();
                m_sOriginalName2 = General_DataGridView.Rows[m_iCurrentRow].Cells[1].Value.ToString();
                m_eInsertMode = InsertMode.EditGroup;
            }
        }
        //****************************************************************************************************************************
        private void NewAlternativeSpellings_Click(object sender, EventArgs e)
        {
            m_eInsertMode = InsertMode.InsertGroup;
            m_iEditLocation = mouseLocation.RowIndex + 1;
            General_DataGridView.Rows.Insert(m_iEditLocation, "", "");
            General_DataGridView.Rows[m_iEditLocation].Cells[0].Selected = true;
        }
        //****************************************************************************************************************************
        private void AmotherAlternativeSpellings_Click(object sender, EventArgs e)
        {
            m_eInsertMode = InsertMode.InsertGroup;
            m_iEditLocation = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iEditLocation];
            string sName1 = dataGridViewRow.Cells[0].Value.ToString();
            m_iEditLocation++;
            General_DataGridView.Rows.Insert(m_iEditLocation, sName1, "");
            General_DataGridView.Rows[m_iEditLocation].Cells[1].Selected = true;
        }
        //****************************************************************************************************************************
        private void DeleteAlternativeSpellings_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sInputString = dataGridViewRow.Cells[0].Value.ToString();
            string sName1 = dataGridViewRow.Cells[0].Value.ToString();
            string sName2 = dataGridViewRow.Cells[1].Value.ToString();
            SQL.DeleteAlternativeSpelling(m_sTable, sName1, sName2);
            General_DataGridView.Rows.RemoveAt(m_iCurrentRow);
            if (m_iCurrentRow + 1 >= General_DataGridView.Rows.Count)
                m_iCurrentRow--;
            General_DataGridView.Rows[m_iCurrentRow].Cells[0].Selected = true;
            m_eInsertMode = InsertMode.NotInsertEditMode;
        }
    }
}
