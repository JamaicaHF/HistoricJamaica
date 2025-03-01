using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    class CGridPerson : FGrid
    {
        private CSql m_SQL;
        private DataTable m_tbl;
        private int m_iNumElements = 0;
        private int m_iSelectedIDLocation = U.Exception;
        private bool m_bAddMode = false;
        private int m_iLevel;
        private bool m_bImport;
        private string m_sFormTitle;
        ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
        private DataGridViewCellEventArgs mouseLocation;
        ContextMenuStrip strip = new ContextMenuStrip();
        private int m_SelectedPersonID = 0;
        private string m_FirstName = "";
        private string m_MiddleName = "";
        private string m_LastName = "";
        private string m_Suffix = "";
        private string m_Prefix = "";
        private string m_MarriedName = "";
        private string m_sSortLastName = "";
        //****************************************************************************************************************************
        public int SelectedPersonID
        {
            get { return m_SelectedPersonID; }
        }
        //****************************************************************************************************************************
        public string SelectedFirstName
        {
            get { return m_FirstName; }
        }
        //****************************************************************************************************************************
        public string SelectedMiddleName
        {
            get { return m_MiddleName; }
        }
        //****************************************************************************************************************************
        public string SelectedLastName
        {
            get { return m_LastName; }
        }
        //****************************************************************************************************************************
        public string SelectedSuffix
        {
            get { return m_Suffix; }
        }
        //****************************************************************************************************************************
        public string SelectedPrefix
        {
            get { return m_Prefix; }
        }
        //****************************************************************************************************************************
        public string SelectedMarriedName
        {
            get { return m_MarriedName; }
        }
        //****************************************************************************************************************************
        public CGridPerson(CSql SQL,
                           ref DataTable tbl,
                           bool bAddMode,
                           string sFormTitle,
                           string sSortLastName)
        {
            m_SQL = SQL;
            m_tbl = tbl;
            m_bAddMode = bAddMode;
            m_iLevel = 0;
            m_bImport = false;
            m_sFormTitle = sFormTitle;
            m_sSortLastName = sSortLastName;
            InitializeGrid();
        }
        //****************************************************************************************************************************
        public CGridPerson(CSql SQL,
                           ref DataTable tbl,
                           bool      bAddMode,
                           int       iLevel,
                           bool      bImport,
                           string    sFormTitle)
        {
            m_SQL = SQL;
            m_tbl = tbl;
            m_bAddMode = bAddMode;
            m_iLevel = iLevel;
            m_sFormTitle = sFormTitle;
            m_bImport = bImport;
            this.Text = m_sFormTitle;
            InitializeGrid();
        }
        //****************************************************************************************************************************
        private void InitializeGrid()
        {
            buttonPane.Visible = true;
            Filter_Button.Click += new EventHandler(SearchPerson_Click);
            if (m_bImport)
            {
                m_iWidthOfButtonPane = 199;
                Abort_Button.Visible = true;
                Abort_Button.Click += new EventHandler(Abort_Click);
            }
            else
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
                m_FirstName = s.Cells[0].Value.ToString();
                m_MiddleName = s.Cells[1].Value.ToString();
                m_LastName = s.Cells[2].Value.ToString();
                m_Suffix = s.Cells[3].Value.ToString();
                m_Prefix = s.Cells[4].Value.ToString();
                m_MarriedName = s.Cells[5].Value.ToString();
                m_SelectedPersonID = s.Cells[m_iSelectedIDLocation].Value.ToInt();
            }
            Close();
        }
        //****************************************************************************************************************************
        private int GridHeight()
        {
            if (m_iNumGridElements > 19)
                return 60;
            else 
                return 562;
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
            General_DataGridView.CellMouseEnter += dataGridView_CellMouseEnter;
            //            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);
            General_DataGridView.ColumnHeadersVisible = true;
            SetupDataGridViewColumns();
            SetupDataGridViewToolStrips();
        }
        //****************************************************************************************************************************
        private void SetupDataGridViewToolStrips()
        {
            toolStripItem1.Text = "Delete Person From Database";
            toolStripItem1.Click += new EventHandler(DeletePerson_Click);
            toolStripItem2.Text = "Filter People";
            toolStripItem2.Click += new EventHandler(SearchPerson_Click);
            toolStripItem3.Text = "Merge Another Person With This Person";
            toolStripItem3.Click += new EventHandler(MergePerson_Click);
            strip.Items.Add(toolStripItem1);
            strip.Items.Add(toolStripItem2);
            if (m_iLevel == 0)
            {
                strip.Items.Add(toolStripItem3);
            }
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = strip;
            }
        }
        //****************************************************************************************************************************
        private void SetupDataGridViewColumns()
        {
            General_DataGridView.ColumnCount = 15;
            General_DataGridView.Columns[0].Name = "Name";
            General_DataGridView.Columns[0].Width = 200;
            General_DataGridView.Columns[1].Name = "Sex";
            General_DataGridView.Columns[1].Width = 70;
            General_DataGridView.Columns[2].Name = "Born";
            General_DataGridView.Columns[2].Width = 70;
            General_DataGridView.Columns[3].Name = "Born Place";
            General_DataGridView.Columns[3].Width = 100;
            General_DataGridView.Columns[4].Name = "Residence";
            General_DataGridView.Columns[4].Width = 100;
            General_DataGridView.Columns[5].Name = "Died";
            General_DataGridView.Columns[5].Width = 70;
            General_DataGridView.Columns[6].Name = "Died Place";
            General_DataGridView.Columns[6].Width = 100;
            General_DataGridView.Columns[7].Name = "Residence";
            General_DataGridView.Columns[7].Width = 100;
            General_DataGridView.Columns[8].Name = "Spouse";
            General_DataGridView.Columns[8].Width = 160;
            General_DataGridView.Columns[9].Name = "Father";
            General_DataGridView.Columns[9].Width = 160;
            General_DataGridView.Columns[10].Name = "Mother";
            General_DataGridView.Columns[10].Width = 160;
            General_DataGridView.Columns[11].Name = "Source";
            General_DataGridView.Columns[11].Width = 120;
            General_DataGridView.Columns[12].Name = "Notes";
            General_DataGridView.Columns[12].Width = 500;
            General_DataGridView.Columns[13].Name = "Sort Name";
            General_DataGridView.Columns[13].Width = 500;
            General_DataGridView.Columns[14].Name = U.PersonID_col;
            m_iSelectedIDLocation = 14;
        }
        //****************************************************************************************************************************
        private string OrderByStatement()
        {
            return " order by LastName,FirstName,MiddleName,Suffix,Prefix";
        }
        //****************************************************************************************************************************
        private string AlternativeLastName(DataTable AlternativeSpellings,
                                           string sLastName)
        {
            string sRealLastName = sLastName;
            DataRow rowAlternativeSpelling = AlternativeSpellings.Rows.Find(sRealLastName);
            if (rowAlternativeSpelling != null)
                sRealLastName = rowAlternativeSpelling[U.NameSpelling1_Col].ToString();
            return sRealLastName;
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            LoadDatagrid();
            SortDatagrid();
            m_iNumGridElements = m_tbl.Rows.Count;
            SetHeight();
        }
        //****************************************************************************************************************************
        private void SetHeight()
        {
            if (m_iLevel == 1)
            {
                this.Location = new Point(100, GridHeight() + 20);
                SetSizeOfGrid(U.iMaxSizeOfGrid - 70);
            }
            else
            {
                this.Location = new Point(30, GridHeight());
                SetSizeOfGrid(U.iMaxSizeOfGrid);
            }
        }
        //****************************************************************************************************************************
        private void SortDatagrid()
        {
            if (General_DataGridView.Rows.Count != 0 && General_DataGridView.CurrentCell != null)
            {
                General_DataGridView.Sort(General_DataGridView.Columns[m_iSelectedIDLocation - 1], System.ComponentModel.ListSortDirection.Ascending);
                General_DataGridView.CurrentCell.Selected = false;
                General_DataGridView.Rows[0].Cells[0].Selected = true;
                General_DataGridView.CurrentCell = General_DataGridView.SelectedCells[0];
            }
        }
        //****************************************************************************************************************************
        private void LoadDatagrid()
        {
            string sFather;
            string sMother;
            string sBornDate;
            string sBornPlace;
            string sBornHome;
            string sDiedDate;
            string sDiedPlace;
            string sDiedHome;
            string sName;
            string sSortName;
            string sSpouse;
            m_iNumElements = 0;
            DataTable AlternativeSpellings = SQL.GetAllAlternativeSpellings(U.AlternativeSpellingsLastName_Table);
            AlternativeSpellings.PrimaryKey = new DataColumn[] { AlternativeSpellings.Columns[U.NameSpelling2_Col] };
            General_DataGridView.Rows.Clear();
            foreach (DataRow row in m_tbl.Rows)
            {
                string sRealLastName = AlternativeLastName(AlternativeSpellings, row[U.LastName_col].ToString());
                string sRealMarriedName = AlternativeLastName(AlternativeSpellings, row[U.MarriedName_col].ToString());
                SQL.GetPersonGridData(row, m_sSortLastName, sRealLastName, sRealMarriedName, out sSortName, out sName,
                                             out sBornDate, out sBornPlace, out sBornHome, out sDiedDate,
                                             out sDiedPlace, out sDiedHome, out sSpouse, out sFather, out sMother);
                General_DataGridView.Rows.Add(sName, row[U.Sex_col].ToString(), sBornDate, sBornPlace, sBornHome, sDiedDate, sDiedPlace, sDiedHome, sSpouse, sFather, sMother,
                                              row[U.Source_col], row[U.Notes_col].Notes(), sSortName, row[U.PersonID_col]);
                m_iNumElements++;
            }
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, 
                                                 DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
        //****************************************************************************************************************************
        private void Abort_Click(object sender, EventArgs e)
        {
            m_SelectedPersonID = U.Exception;
            Close();
        }
        //****************************************************************************************************************************
        private void MergePerson_Click(object sender, EventArgs e)
        {
            int iSelectedRow = General_DataGridView.SelectedRows[0].Index;
            DataGridViewRow s = General_DataGridView.Rows[iSelectedRow];
            int iPersonID = s.Cells[m_iSelectedIDLocation].Value.ToInt();
            DataRow Person_row = SQL.GetPerson(iPersonID);
            if (Person_row == null)
                return;
            string sLastName = Person_row[U.LastName_col].ToString();
            string sMarriedName = Person_row[U.MarriedName_col].ToString();
            if (sLastName.Length == 0)
            {
                sLastName = sMarriedName;
                sMarriedName = "";
            }
            DataTable Person_tbl = SQL.DefinePersonTable();
            Person_tbl.PrimaryKey = new DataColumn[] { Person_tbl.Columns[U.PersonID_col] };
            PersonName personName = new PersonName(Person_row[U.FirstName_col].ToString(), "", sLastName, "", "");
            SQL.PersonsBasedOnNameOptions(Person_tbl, true, U.Person_Table, U.PersonID_col,  
                                                      eSearchOption.SO_PartialNames, personName, "");
            if (sMarriedName.Length != 0)
            {
                personName.lastName = sMarriedName;
                SQL.PersonsBasedOnNameOptions(Person_tbl, true, U.Person_Table, U.PersonID_col,
                                                 eSearchOption.SO_PartialNames, personName, sMarriedName);
            }
            CGridPerson GridPerson = new CGridPerson(m_SQL, ref Person_tbl, false, 1, false, "Person");
            GridPerson.ShowDialog();
            int iMergePersonID = GridPerson.SelectedPersonID;
            if (iMergePersonID != 0 && iMergePersonID != iPersonID)
            {
                MergeRecordIfDesired(iPersonID, iMergePersonID);
            }
        }
        //****************************************************************************************************************************
        private void MergeRecordIfDesired(int iPersonID,
                                 int iMergePersonID)
        {
            string sPersonName = SQL.GetPersonName(iPersonID);
            string sMergePersonName = SQL.GetPersonName(iMergePersonID);
            string sMessage = "Do You wish to merge '" + sPersonName + "' with '" + sMergePersonName + "'";
            switch (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                {
                    DataSet person_ds = SQL.DefinePerson();
                    DataSet mergePerson_ds = SQL.DefinePerson();
                    if (SQL.GetRecordsToMerge(person_ds, mergePerson_ds, iPersonID, iMergePersonID))
                    {
                        MergeRecord(person_ds, mergePerson_ds, iPersonID, iMergePersonID);
                    }
                    break;
                }
                default: break;
            }
        }
        //****************************************************************************************************************************
        private void MergeRecord(DataSet person_ds,
                                 DataSet mergePerson_ds,
                                 int iPersonID,
                                 int iMergePersonID)
        {
            try
            {
                if (SQL.MergeTwoPersons(person_ds, mergePerson_ds, iPersonID, iMergePersonID))
                {
                    int iRowIndex = FindPersonInGrid(iMergePersonID);
                    RemovePersonFromGrid(iRowIndex);
                }
                else
                {
                    MessageBox.Show("Save Unsuccesful");
                }
            }
            catch (HistoricJamaicaException ex)
            {
                UU.ShowErrorMessage(ex);
            }
        }
        //****************************************************************************************************************************
        private int FindPersonInGrid(int iPersonID)
        {
            foreach (DataGridViewRow row in General_DataGridView.Rows)
            {
                int iThisPersonID = row.Cells[m_iSelectedIDLocation].Value.ToInt();
                if (iThisPersonID == iPersonID)
                {
                    return row.Index;
                }
            }
            return 0;
        }
        //****************************************************************************************************************************
        private void RemovePersonFromGrid(int iRowIndex)
        {
            General_DataGridView.Rows.RemoveAt(iRowIndex);
            m_iNumGridElements--;
            SetSizeOfGrid(U.iMaxSizeOfGrid);
        }
        //****************************************************************************************************************************
        private void SearchPerson_Click(object sender, EventArgs e)
        {
            CPersonFilter PersonFilter = new CPersonFilter(m_SQL, m_tbl, true, U.Person_Table, U.PersonID_col);
            PersonFilter.ShowDialog();
            ShowAllValues("");
        }
        //****************************************************************************************************************************
        private void DeletePerson_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iPersonID = dataGridViewRow.Cells[m_iSelectedIDLocation].Value.ToInt();
            if (iPersonID != 0 && MessageBox.Show("Delete this Person?", "", MessageBoxButtons.YesNo) == DialogResult.Yes &&
                SQL.DeletePersonFromDatabase(iPersonID))
            {
                RemovePersonFromGrid(iRowIndex);
            }
        }
    }
}
