using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    class CGridVitalRecord : FGrid
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
        private int m_SelectedVitalRecordID = 0;
        //****************************************************************************************************************************
        public int SelectedVitalRecordID
        {
            get { return m_SelectedVitalRecordID; }
        }
        //****************************************************************************************************************************
        public CGridVitalRecord(CSql SQL,
                               ref DataTable tbl)
        {
            m_SQL = SQL;
            m_tbl = tbl;
            buttonPane.Visible = true;
            Filter_Button.Click += new EventHandler(SearchVitalRecord_Click);
            Abort_Button.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            int iSelectedRow = General_DataGridView.SelectedRows[0].Index;
            if (iSelectedRow >= m_iNumElements)
                m_SelectedVitalRecordID = 0;
            else
            {
                DataGridViewRow s = General_DataGridView.Rows[iSelectedRow];
                m_SelectedVitalRecordID = s.Cells[m_NumColumns - 1].Value.ToInt();
                string vitalRecordType = s.Cells[0].Value.ToString();
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
            this.Text = "    VitalRecords";
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
            m_NumColumns = 16;
            General_DataGridView.ColumnCount = m_NumColumns;
            General_DataGridView.Columns[0].Name = "Record Type";
            General_DataGridView.Columns[1].Name = "";
            General_DataGridView.Columns[1].Width = 18;
            General_DataGridView.Columns[2].Name = "";
            General_DataGridView.Columns[2].Width = 13;
            General_DataGridView.Columns[3].Name = "Name";
            General_DataGridView.Columns[3].Width = 180;
            General_DataGridView.Columns[4].Name = "Born";
            General_DataGridView.Columns[4].Width = 70;
            General_DataGridView.Columns[5].Name = "Name of Partner";
            General_DataGridView.Columns[5].Width = 180;
            General_DataGridView.Columns[6].Name = "";
            General_DataGridView.Columns[6].Width = 13;
            General_DataGridView.Columns[7].Name = "Name of Father";
            General_DataGridView.Columns[7].Width = 180;
            General_DataGridView.Columns[8].Name = "";
            General_DataGridView.Columns[8].Width = 13;
            General_DataGridView.Columns[9].Name = "Maiden Name of Mother";
            General_DataGridView.Columns[9].Width = 180;
            General_DataGridView.Columns[10].Name = "Date";
            General_DataGridView.Columns[10].Width = 70;
            General_DataGridView.Columns[11].Name = "Book";
            General_DataGridView.Columns[11].Width = 40;
            General_DataGridView.Columns[12].Name = "Page";
            General_DataGridView.Columns[12].Width = 40;
            General_DataGridView.Columns[13].Name = "Cemetery/Age";
            General_DataGridView.Columns[13].Width = 150;
            General_DataGridView.Columns[14].Name = "Notes";
            General_DataGridView.Columns[14].Width = 500;    
            General_DataGridView.Columns[15].Name = U.VitalRecordID_col;

            General_DataGridView.CellMouseEnter += dataGridView_CellMouseEnter;
            toolStripItem1.Text = "Delete Vital Record From Database";
            toolStripItem1.Click += new EventHandler(DeleteVitalRecord_Click);
            strip.Items.Add(toolStripItem1);
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = strip;
            }
        }
        //****************************************************************************************************************************
        private string OrderByStatement()
        {
            return " order by LastName,FirstName,MiddleName,Suffix,Prefix";
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWithPerson)
        {
            m_iNumElements = 0;
            int rowToDisplay = 0;
            General_DataGridView.Rows.Clear();
            var stripAdded = false;
            foreach (DataRow row in m_tbl.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    EVitalRecordType eVitalRecordType = (EVitalRecordType)row[U.VitalRecordType_col].ToInt();
                    string sPersonName = SQL.BuildNameLastNameFirst(row[U.FirstName_col].ToString(), row[U.MiddleName_col].ToString(),
                                                         row[U.LastName_col].ToString(), row[U.Suffix_col].ToString(),
                                                         row[U.Prefix_col].ToString(), "", "");
                    string sSpouseName = "";
                    string sFathersName = "";
                    string sMothersName = "";
                    string sBornDate = "";
                    bool studentRowFound = false;
                    if (eVitalRecordType == EVitalRecordType.eSchool) // cemetery
                    {
                        int SchoolRecordId = row[U.VitalRecordID_col].ToInt() - U.SchoolRecordOffset_col;
                        DataTable SchoolRecord_tbl = SQL.GetSchoolRecord(SchoolRecordId);
                        if (SchoolRecord_tbl.Rows.Count != 0)
                        {
                            DataRow SchoolRecord_row = SchoolRecord_tbl.Rows[0];
                            sBornDate = SchoolRecord_row[U.BornDate_col].ToString();
                            studentRowFound = true;
                        }
                    }
                    else if (eVitalRecordType == EVitalRecordType.eIntegrateAll) // cemetery
                    {
                        DataTable CemeteryRecord_tbl = SQL.DefineCemeteryRecordTable();
                        int CemeteryRecordId = row[U.VitalRecordID_col].ToInt() - 9900000;
                        if (SQL.GetCemeteryRecord(CemeteryRecord_tbl, CemeteryRecordId))
                        {
                            DataRow CemeteryRecord_row = CemeteryRecord_tbl.Rows[0];
                            sSpouseName = SQL.BuildNameLastNameFirst(CemeteryRecord_row[U.SpouseFirstName_col].ToString(), 
                                                                 CemeteryRecord_row[U.SpouseMiddleName_col].ToString(),
                                                                 CemeteryRecord_row[U.SpouseLastName_col].ToString(), 
                                                                 CemeteryRecord_row[U.SpouseSuffix_col].ToString(),
                                                                 CemeteryRecord_row[U.SpousePrefix_col].ToString(), "", "");
                            sBornDate = U.VitalRecordBornDate(EVitalRecordType.eBurial, row, "");
                        }
                    }
                    else if (eVitalRecordType == EVitalRecordType.eSearch) // person
                    {
                        int personId = row[U.VitalRecordID_col].ToInt() - 900000;
                        DataRow personRow = SQL.GetPerson(personId);
                        if (personRow != null)
                        {
                            sPersonName = SQL.BuildNameLastNameFirst(personRow[U.FirstName_col].ToString(),
                               personRow[U.MiddleName_col].ToString(),
                               personRow[U.LastName_col].ToString(),
                               personRow[U.Suffix_col].ToString(),
                               personRow[U.Prefix_col].ToString(),
                               personRow[U.MarriedName_col].ToString(),
                               personRow[U.KnownAs_col].ToString());
                        }
                        sSpouseName = SQL.GetSpouse(personId);
                        sBornDate = GetBornDate(row, personId);
                    }
                    else // vitalrecord
                    {
                        sBornDate = U.VitalRecordBornDate(eVitalRecordType, row, "");
                    }
                    string sDeathInfo = "";
                    if (eVitalRecordType.MarriageRecord())
                    {
                        int iSpouseVitalRecordID = row[U.SpouseID_col].ToInt();
                        DataTable tbl = SQL.DefineVitalRecord_Table();
                        SQL.GetVitalRecord(tbl, iSpouseVitalRecordID);
                        sSpouseName = SQL.BuildNameLastNameFirst(tbl.Rows[0][U.FirstName_col].ToString(),
                                                              tbl.Rows[0][U.MiddleName_col].ToString(),
                                                              tbl.Rows[0][U.LastName_col].ToString(),
                                                              tbl.Rows[0][U.Suffix_col].ToString(),
                                                              tbl.Rows[0][U.Prefix_col].ToString(), "", "");
                        sDeathInfo = U.DeathInfo("", "", row[U.AgeYears_col].ToInt(), row[U.AgeMonths_col].ToInt(), row[U.AgeDays_col].ToInt());
                    }
                    else if (!eVitalRecordType.IsBirthRecord())
                    {
                        sDeathInfo = U.DeathInfo(row[U.CemeteryName_col].ToString(), row[U.LotNumber_col].ToString(),
                                                      row[U.AgeYears_col].ToInt(), row[U.AgeMonths_col].ToInt(), row[U.AgeDays_col].ToInt());
                    }
                    sFathersName = SQL.BuildNameLastNameFirst(row[U.FatherFirstName_col].ToString(), row[U.FatherMiddleName_col].ToString(),
                                                         row[U.FatherLastName_col].ToString(), row[U.FatherSuffix_col].ToString(),
                                                         row[U.FatherPrefix_col].ToString(), "", "");
                    sMothersName = SQL.BuildNameLastNameFirst(row[U.MotherFirstName_col].ToString(), row[U.MotherMiddleName_col].ToString(),
                                                         row[U.MotherLastName_col].ToString(), row[U.MotherSuffix_col].ToString(),
                                                         row[U.MotherPrefix_col].ToString(), "", "");
                    string sDate = U.BuildDate(row[U.DateYear_col].ToInt(), row[U.DateMonth_col].ToInt(), row[U.DateDay_col].ToInt());
                    string sVitalRecordType = eVitalRecordType.VitalRecordTypeToString();
                    string sPersonIntegrated;
                    string sFatherIntegrated;
                    string sMotherIntegrated;
                    string sex = eVitalRecordType.RecordTypeSex(row[U.Sex_col].ToString(), row[U.FirstName_col].ToString());
                    SetIntegrated(row, out sPersonIntegrated, out sFatherIntegrated, out sMotherIntegrated);
                    int rowindex = General_DataGridView.Rows.Add(sVitalRecordType, sex, sPersonIntegrated, sPersonName, sBornDate, sSpouseName, sFatherIntegrated, sFathersName,
                                                  sMotherIntegrated, sMothersName, sDate, row[U.Book_col].ToString(), row[U.Page_col].ToString(), 
                                                  sDeathInfo, row[U.Notes_col].Notes(), row[U.VitalRecordID_col].ToString());
                    if (studentRowFound && !stripAdded)
                    {
                        stripAdded = true;
                        toolStripItem2.Text = "Update School Record Name to Person Record Name";
                        toolStripItem2.Click += new EventHandler(UpdateStudentRecord_Click);
                        strip.Items.Add(toolStripItem2);
                    }
                    if (!String.IsNullOrEmpty(startingWithPerson) && startingWithPerson.ToLower().Trim() == sPersonName.ToLower().Trim())
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
            General_DataGridView.FirstDisplayedScrollingRowIndex = (this.Height < U.iMaxSizeOfGrid) ? rowToDisplay : 0;
        }
        //****************************************************************************************************************************
        public static string GetBornDate(DataRow vitalRecord_row, int iPersonID)
        {
            DataTable CemeteryRecord_tbl = SQL.DefineCemeteryRecordTable();
            SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, iPersonID);
            DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
            SQL.GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col);
            string sBornDate = U.BuildDate(vitalRecord_row[U.DateYear_col].ToInt(), vitalRecord_row[U.DateMonth_col].ToInt(), vitalRecord_row[U.DateDay_col].ToInt());
            string sBornPlace = "";
            string sBornHome = "";
            DataRow personRow = SQL.GetPerson(iPersonID);
            SQL.VitalRecord(VitalRecord_tbl, CemeteryRecord_tbl, personRow, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale, ref sBornDate, ref sBornPlace, ref sBornHome);
            return sBornDate;
        }
        //****************************************************************************************************************************
        private void SetIntegrated(DataRow row,
                                   out string sPersonIntegrated,
                                   out string sFatherIntegrated,
                                   out string sMotherIntegrated)
        {
            if (row[U.PersonID_col].ToInt() == 0)
                sPersonIntegrated = NotIntegrated;
            else
                sPersonIntegrated = Integrated;
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
        private void AllVitalRecord_Click(object sender, EventArgs e)
        {
            GetAllNames();
            ShowAllValues("");
        }
        //****************************************************************************************************************************
        private void NewPerson_Click(object sender, EventArgs e)
        {
            FPerson Person = new FPerson(m_SQL, true, 0);
            Person.ShowDialog();
        }
        //****************************************************************************************************************************
        private void SearchVitalRecord_Click(object sender, EventArgs e)
        {
            CPersonFilter PersonFilter = new CPersonFilter(m_SQL, m_tbl, false, U.VitalRecord_Table, U.VitalRecordID_col);
            PersonFilter.ShowDialog();
            ShowAllValues("");
        }
        //****************************************************************************************************************************
        private int GetRowIndexFromVitalRecordID(int iVitalRecordID)
        {
            if (iVitalRecordID == 0)
                return -1;
            foreach (DataGridViewRow dataGridViewRow in General_DataGridView.Rows)
            {
                if (dataGridViewRow.Cells[m_NumColumns - 1].Value.ToInt() == iVitalRecordID)
                    return dataGridViewRow.Index;
            }
            return -1;
        }
        //****************************************************************************************************************************
        private void RemoveRows(int iRow1,
                                int iRow2)
        {
            if (iRow1 >= 0)
            {
                General_DataGridView.Rows.RemoveAt(iRow1);
                m_iNumGridElements--;
            }
            if (iRow2 >= 0)
            {
                General_DataGridView.Rows.RemoveAt(iRow2);
                m_iNumGridElements--;
            }
        }
        //****************************************************************************************************************************
        private void UpdateStudentRecord_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iSchoolRecordID = dataGridViewRow.Cells[m_NumColumns - 1].Value.ToInt();
            if (iSchoolRecordID > 8999999 || iSchoolRecordID < U.SchoolRecordOffset_col)
            {
                return;
            }
            iSchoolRecordID -= U.SchoolRecordOffset_col;
            DataRow personRow;
            if (SQL.UpdateSchoolrecordsFromPersonRecord(iSchoolRecordID, out personRow))
            {
                foreach (DataRow row in m_tbl.Rows)
                {
                    if (row[U.VitalRecordID_col].ToInt() > U.SchoolRecordOffset_col)
                    {
                        row[U.FirstName_col] = personRow[U.FirstName_col];
                        row[U.MiddleName_col] = personRow[U.MiddleName_col];
                        row[U.LastName_col] = personRow[U.LastName_col];
                        row[U.Suffix_col] = personRow[U.Suffix_col];
                        row[U.Prefix_col] = personRow[U.Prefix_col];
                    }
                }
                ShowAllValues("");
            }
            else
            {
                MessageBox.Show("Unable to modify Student Records");
            }
        }
        //****************************************************************************************************************************
        private void DeleteVitalRecord_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            int iVitalRecordID = dataGridViewRow.Cells[m_NumColumns - 1].Value.ToInt();
            DataTable tbl = SQL.DefineVitalRecord_Table();
            SQL.GetVitalRecord(tbl, iVitalRecordID);
            if (tbl.Rows.Count != 0)
            {
                int iSpouseIndex = tbl.Rows[0][U.SpouseID_col].ToInt();
                int iSpouseRowIndex = GetRowIndexFromVitalRecordID(iSpouseIndex);
                if (iVitalRecordID != 0 && MessageBox.Show("Delete this Record?", "", MessageBoxButtons.YesNo) == DialogResult.Yes &&
                    SQL.DeleteVitalRecordFromDatabase(iVitalRecordID, iSpouseIndex))
                {
                    if (iRowIndex > iSpouseRowIndex)
                        RemoveRows(iRowIndex,iSpouseRowIndex);
                    else
                        RemoveRows(iSpouseRowIndex,iRowIndex);
                    SetSizeOfGrid(U.iMaxSizeOfGrid);
                }
            }
        }
    }
}
