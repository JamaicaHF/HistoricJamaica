using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    class CGridCemeteryRecord : FGrid
    {
        private const string Integrated = "x";
        private const string NotIntegrated = " ";
        private CSql m_SQL;
        private DataTable m_tbl;
        private int m_iNumElements = 0;
        ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
        private DataGridViewCellEventArgs mouseLocation;
        ContextMenuStrip strip = new ContextMenuStrip();
        int m_NumColumns = 0;
        Cemetery cemetery;
        private int m_SelectedCemeteryRecordID = 0;
        //****************************************************************************************************************************
        public int SelectedCemeteryRecordID
        {
            get { return m_SelectedCemeteryRecordID; }
        }
        //****************************************************************************************************************************
        public CGridCemeteryRecord(CSql SQL,
                                   DataTable tbl,
                                   Cemetery cemetery)
        {
            this.cemetery = cemetery;
            m_SQL = SQL;
            m_tbl = tbl;
            buttonPane.Visible = true;
            Filter_Button.Click += new EventHandler(SearchCemeteryRecord_Click);
            Abort_Button.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            int iSelectedRow = General_DataGridView.SelectedRows[0].Index;
            if (iSelectedRow >= m_iNumElements)
                m_SelectedCemeteryRecordID = 0;
            else
            {
                DataGridViewRow s = General_DataGridView.Rows[iSelectedRow];
                m_SelectedCemeteryRecordID = s.Cells[m_NumColumns - 1].Value.ToInt();
            }
            Close();
        }
        //****************************************************************************************************************************
        private int GridHeight()
        {
            if (m_iNumGridElements > 19)
                return 60;
            else 
                return 422;
        }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
            this.Text = "Cemetery Records";
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
//            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);
            General_DataGridView.ColumnHeadersVisible = true;
            m_NumColumns = 17;
            General_DataGridView.ColumnCount = m_NumColumns;
            General_DataGridView.Columns[0].Name = "Name on Grave";
            General_DataGridView.Columns[0].Width = 150;
            General_DataGridView.Columns[1].Name = "";
            General_DataGridView.Columns[1].Width = 13;
            General_DataGridView.Columns[2].Name = "Name";
            General_DataGridView.Columns[2].Width = 120;
            General_DataGridView.Columns[3].Name = "";
            General_DataGridView.Columns[3].Width = 13;
            General_DataGridView.Columns[4].Name = "Name of Partner";
            General_DataGridView.Columns[4].Width = 120;
            General_DataGridView.Columns[5].Name = "";
            General_DataGridView.Columns[5].Width = 13;
            General_DataGridView.Columns[6].Name = "Name of Father";
            General_DataGridView.Columns[6].Width = 120;
            General_DataGridView.Columns[7].Name = "";
            General_DataGridView.Columns[7].Width = 13;
            General_DataGridView.Columns[8].Name = "Maiden Mother";
            General_DataGridView.Columns[8].Width = 120;
            General_DataGridView.Columns[9].Name = "";
            General_DataGridView.Columns[9].Width = 15;
            General_DataGridView.Columns[10].Name = "Born";
            General_DataGridView.Columns[10].Width = 70;
            General_DataGridView.Columns[11].Name = "DiedDate";
            General_DataGridView.Columns[11].Width = 70;
            General_DataGridView.Columns[12].Name = "Age";
            General_DataGridView.Columns[12].Width = 150;
            General_DataGridView.Columns[13].Name = "Cemetery";
            General_DataGridView.Columns[13].Width = 120;
            General_DataGridView.Columns[14].Name = "LotNumber";
            General_DataGridView.Columns[14].Width = 100;
            General_DataGridView.Columns[15].Name = "Notes";
            General_DataGridView.Columns[15].Width = 500;
            General_DataGridView.Columns[16].Name = U.CemeteryRecordID_col;
            General_DataGridView.Columns[16].Width = 70;

            General_DataGridView.CellMouseEnter += dataGridView_CellMouseEnter;
            toolStripItem1.Text = "Delete Cemetery Record From Database";
            toolStripItem1.Click += new EventHandler(DeleteCemeteryRecord_Click);
            strip.Items.Add(toolStripItem1);
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = strip;
            }
            this.Text = U.CemeteryName(cemetery) + " " + this.Text;
        }
        //****************************************************************************************************************************
        private string OrderByStatement()
        {
            return " order by LastName,FirstName,MiddleName,Suffix,Prefix";
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWithPerson)
        {
            int startingCemeteryRecordId = startingWithPerson.ToInt();
            m_iNumElements = 0;
            int rowToDisplay = 0;
            General_DataGridView.Rows.Clear();
            foreach (DataRow row in m_tbl.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    string sPersonName = SQL.BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                                              row[U.MiddleName_col].ToString(),
                                                              row[U.LastName_col].ToString(),
                                                              row[U.Suffix_col].ToString(),
                                                              row[U.Prefix_col].ToString(), "", "");
                    string sSpouseName = "";
                    string sFathersName = "";
                    string sMothersName = "";
                    string sBornDate;
                    sBornDate = BornDate(row);
                    int iSpouseID = row[U.SpouseID_col].ToInt();
                    DataTable tbl = SQL.DefineCemeteryRecordTable();
                    SQL.GetCemeteryRecord(tbl, iSpouseID);
                    sSpouseName = SQL.BuildNameLastNameFirst(row[U.SpouseFirstName_col].ToString(),
                                                              row[U.SpouseMiddleName_col].ToString(),
                                                              row[U.SpouseLastName_col].ToString(),
                                                              row[U.SpouseSuffix_col].ToString(),
                                                              row[U.SpousePrefix_col].ToString(), "", "");
                    sFathersName = SQL.BuildNameLastNameFirst(row[U.FatherFirstName_col].ToString(), row[U.FatherMiddleName_col].ToString(),
                                                         row[U.FatherLastName_col].ToString(), row[U.FatherSuffix_col].ToString(),
                                                         row[U.FatherPrefix_col].ToString(), "", "");
                    sMothersName = SQL.BuildNameLastNameFirst(row[U.MotherFirstName_col].ToString(), row[U.MotherMiddleName_col].ToString(),
                                                         row[U.MotherLastName_col].ToString(), row[U.MotherSuffix_col].ToString(),
                                                         row[U.MotherPrefix_col].ToString(), "", "");
                    string sAge = U.AgeString(row[U.AgeYears_col].ToInt(), row[U.AgeMonths_col].ToInt(), row[U.AgeDays_col].ToInt());
                    string sPersonIntegrated;
                    string sSpouseIntegrated;
                    string sFatherIntegrated;
                    string sMotherIntegrated;
                    string cemeteryName = U.CemeteryName((Cemetery)row[U.CemeteryID_col].ToInt());
                    SetIntegrated(row, out sPersonIntegrated, out sSpouseIntegrated, out sFatherIntegrated, out sMotherIntegrated);
                    General_DataGridView.Rows.Add(row[U.NameOnGrave_col].ToString(), sPersonIntegrated, sPersonName, sSpouseIntegrated, sSpouseName, sFatherIntegrated, sFathersName,
                                                  sMotherIntegrated, sMothersName, row[U.Sex_col].ToString(), sBornDate, row[U.DiedDate_col].ToString(), sAge, cemeteryName,
                                                  row[U.LotNumber_col].ToString(), row[U.Notes_col].Notes(), row[U.CemeteryRecordID_col].ToString());

                    string sPersonNameLastNameFirst = SQL.BuildNameLastNameFirst(row[U.FirstName_col].ToString(),
                                                              row[U.MiddleName_col].ToString(),
                                                              row[U.LastName_col].ToString(),
                                                              row[U.Suffix_col].ToString(),
                                                              row[U.Prefix_col].ToString(), "", "");
                    if (startingCemeteryRecordId == row[U.CemeteryRecordID_col].ToInt())
                    {
                        rowToDisplay = m_iNumElements;
                    }
                    m_iNumElements++;
                }
            }
            m_iNumGridElements = m_tbl.Rows.Count;
            this.Location = new System.Drawing.Point(30, GridHeight());
            SetSizeOfGrid(U.iMaxSizeOfGrid);
            General_DataGridView.Rows[rowToDisplay].Cells[0].Selected = true;
            General_DataGridView.FirstDisplayedScrollingRowIndex = rowToDisplay;
        }
        //****************************************************************************************************************************
        private string BornDate(DataRow row)
        {
            if (!String.IsNullOrEmpty(row[U.BornDate_col].ToString()))
            {
                return row[U.BornDate_col].ToString();
            }
            else if (!String.IsNullOrEmpty(row[U.DiedDate_col].ToString()))
            {
                int ageYears = row[U.AgeYears_col].ToInt();
                int ageMonths = row[U.AgeMonths_col].ToInt();
                int ageDays = row[U.AgeDays_col].ToInt();
                string[] dateValues = row[U.DiedDate_col].ToString().Split('/');
                int diedYear = dateValues[0].ToInt();
                if (dateValues.Length == 1)
                {
                    int iYear = diedYear - ageYears;
                    return iYear.ToString();
                }
                if (dateValues.Length == 2)
                {
                    int diedMonth = dateValues[1].ToInt();
                    return U.BornDateFromDiedDateMinusAgeNoDay(diedYear, diedMonth,
                                                          ageYears, ageMonths, ageDays);
                }
                else
                {
                    int diedMonth = dateValues[1].ToInt();
                    int diedDay = dateValues[2].ToInt();
                    return U.BornDateFromDiedDateMinusAge(diedYear, diedMonth, diedDay,
                                                          ageYears, ageMonths, ageDays, "");
                }
            }
            return "";
        }
        //****************************************************************************************************************************
        private void SetIntegrated(DataRow row,
                                   out string sPersonIntegrated,
                                   out string sSpouseIntegrated,
                                   out string sFatherIntegrated,
                                   out string sMotherIntegrated)
        {
            if (row[U.PersonID_col].ToInt() == 0)
                sPersonIntegrated = NotIntegrated;
            else
                sPersonIntegrated = Integrated;
            if (row[U.SpouseID_col].ToInt() == 0)
                sSpouseIntegrated = NotIntegrated;
            else
                sSpouseIntegrated = Integrated;
            if (row[U.FatherID_col].ToInt() == 0)
                sFatherIntegrated = NotIntegrated;
            else
                sFatherIntegrated = Integrated;
            if (row[U.MotherID_col].ToInt() == 0)
                sMotherIntegrated = NotIntegrated;
            else
                sMotherIntegrated = Integrated;
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
        //****************************************************************************************************************************
        private void GetAllNames()
        {
            SQL.SelectAll(U.Person_Table, OrderByStatement(), m_tbl);
        }
        //****************************************************************************************************************************
        private void SearchCemeteryRecord_Click(object sender, EventArgs e)
        {
            CPersonFilter PersonFilter = new CPersonFilter(m_SQL, m_tbl, false, U.CemeteryRecord_Table, U.CemeteryRecordID_col);
            PersonFilter.ShowDialog();
            ShowAllValues("");
        }
        //****************************************************************************************************************************
        private void DeleteCemeteryRecord_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iCemeteryRecordID = dataGridViewRow.Cells[m_NumColumns - 1].Value.ToInt();
            DataTable cemetery_tbl = SQL.DefineCemeteryRecordTable();
            SQL.GetCemeteryRecord(cemetery_tbl, iCemeteryRecordID);
            if (cemetery_tbl.Rows.Count != 0)
            {
                if (MessageBox.Show("Delete this Record?", "", MessageBoxButtons.YesNo) == DialogResult.Yes )
                {
                    int numRows = SQL.DeleteWithParms(U.CemeteryRecord_Table, new NameValuePair(U.CemeteryRecordID_col, iCemeteryRecordID));
                    if (numRows == 1)
                    {
                        General_DataGridView.Rows.RemoveAt(iRowIndex);
                        SetSizeOfGrid(U.iMaxSizeOfGrid);
                    }
                }
            }
        }
    }
}
