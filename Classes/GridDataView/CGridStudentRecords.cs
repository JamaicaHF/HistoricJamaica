using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    class CGridStudentRecords : FGrid
    {
        private CSql m_SQL;
        private int m_iSchoolId;
        private int m_iSchoolYear;
        private int m_iGrade;
        private int m_iSelectedRow = -1;
        private DataGridViewCellEventArgs mouseLocation;
        private FlowLayoutPanel FlowLayoutPanel1 = new FlowLayoutPanel();
        private ListBox Values_listBox = new System.Windows.Forms.ListBox();
        private bool m_bExitOnDoubleClick = false;
        //****************************************************************************************************************************
        public int SelectedRow
        {
            get { return m_iSelectedRow; }
        }
        //****************************************************************************************************************************
        public CGridStudentRecords(ref CSql Sql,
                                  int iSchoolId,
                                  int iSchoolYear,
                                  int iGrade,
                                  bool bExitOnDoubleClick)
        {
            m_SQL = Sql;
            m_iSchoolId = iSchoolId;
            m_iSchoolYear = iSchoolYear;
            m_iGrade = iGrade;
            InitializeComponent();
            this.Values_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Values_listBox.FormattingEnabled = true;
            this.Values_listBox.Location = new System.Drawing.Point(0, 0);
            this.Values_listBox.BorderStyle = BorderStyle.None;
            this.Values_listBox.Name = "Values_listBox";
            this.Values_listBox.Size = new System.Drawing.Size(300,500);
            Abort_Button.Visible = false;
            Filter_Button.Visible = false;
            buttonPane.Visible = false;
            m_bExitOnDoubleClick = bExitOnDoubleClick;
        }
        //****************************************************************************************************************************
        void General_DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
//            e.CellStyle.WrapMode = DataGridViewTriState.True;
        }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
            string text = SQL.GetSchool(m_iSchoolId) + "-" + m_iSchoolYear;
            if (m_iGrade != 0)
            {
                text += (" Grade " + m_iGrade);
            }
            this.Text = text;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            FlowLayoutPanel1.Location = new System.Drawing.Point(850, 10);
            FlowLayoutPanel1.AutoSize = true;
            FlowLayoutPanel1.Controls.Add(this.Values_listBox);

            Controls.Add(FlowLayoutPanel1);
            Controls.Add(General_DataGridView);
//            General_DataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            General_DataGridView.Location = new System.Drawing.Point(100, 50);
            General_DataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            General_DataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            General_DataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(General_DataGridView.Font, FontStyle.Bold);
            General_DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            General_DataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            General_DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            General_DataGridView.GridColor = Color.Black;
            General_DataGridView.RowHeadersVisible = false;
            General_DataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            General_DataGridView.MultiSelect = false;
            General_DataGridView.Dock = DockStyle.Fill;
            General_DataGridView.ColumnCount = 5;
            General_DataGridView.Columns[0].Name = "Name";
            General_DataGridView.Columns[0].Width = 300;
            General_DataGridView.Columns[0].Visible = true;
            General_DataGridView.Columns[1].Name = "";
            General_DataGridView.Columns[1].Width = 10;
            General_DataGridView.Columns[1].Visible = true;
            General_DataGridView.Columns[2].Name = "Grade";
            General_DataGridView.Columns[2].Width = 100;
            General_DataGridView.Columns[2].Visible = true;
            General_DataGridView.Columns[3].Name = "Birth Date";
            General_DataGridView.Columns[3].Width = 100;
            General_DataGridView.Columns[3].Visible = true;
            General_DataGridView.Columns[4].Name = "RecordID";
            General_DataGridView.Columns[4].Width = 40;
            General_DataGridView.Columns[4].Visible = false;

            General_DataGridView.CellDoubleClick += dataGridView_CellMouseEnter;
            General_DataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(Right_Click);
        }
        //****************************************************************************************************************************
        private void Right_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
                int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
                ContextMenuStrip strip = new ContextMenuStrip();
                ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
                toolStripItem1.Text = "Integrate Person";
                toolStripItem1.Click += new EventHandler(IntegrateStudent_Click);
                strip.Items.Add(toolStripItem1);
                ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
                toolStripItem2.Text = "View Integrated Records";
                toolStripItem2.Click += new EventHandler(ViewIntegratedPerson_button_Click);
                strip.Items.Add(toolStripItem2);
                dataGridViewRow.Cells[iCol].ContextMenuStrip = strip;
            }
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
        //****************************************************************************************************************************
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(1232, 666);
            this.Location = new System.Drawing.Point(600, 20);
            this.Name = "CGridRoadBuildings";
            this.ResumeLayout(false);
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            m_iNumGridElements = 0;
            DataTable teacherRecords_tbl = SQL.GetSchoolTeacherRecord(m_iSchoolId, m_iSchoolYear, m_iGrade);
            DataTable schoolRecords_tbl = SQL.GetSchoolRecords(m_iSchoolId, m_iSchoolYear, m_iGrade);
            bool foundBornDate = false;
            foreach (DataRow teacherRecord_row in teacherRecords_tbl.Rows)
            {
                m_iNumGridElements++;
                AddSchoolRecord(teacherRecord_row, ref foundBornDate);
            }
            foreach (DataRow studentRecord_row in schoolRecords_tbl.Rows)
            {
                m_iNumGridElements++;
                AddSchoolRecord(studentRecord_row, ref foundBornDate);
            }
            General_DataGridView.Columns[3].Visible = foundBornDate;
            int columnWidth = 0;
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                if (column.Visible)
                {
                    columnWidth += column.Width;
                }
            }
            SetSizeOfGrid(columnWidth + 19);
        }
        //****************************************************************************************************************************
        private void AddSchoolRecord(DataRow schoolRecord_row, ref bool foundBornDate)
        {
            string person = schoolRecord_row[U.Person_col].ToString();
            if (schoolRecord_row[U.SchoolRecordType_col].ToSchoolRecordType() == SQL.SchoolRecordType.teacherType)
            {
                person += " - Teacher";
            }
            string grade = schoolRecord_row[U.Grade_col].ToString();
            string bornDate = schoolRecord_row[U.BornDate_col].ToString();
            string integrated = (schoolRecord_row[U.PersonID_col].ToInt() == 0) ? "" : "x";
            int schoolRecordId = schoolRecord_row[U.SchoolRecordID_col].ToInt();
            if (!String.IsNullOrEmpty(bornDate))
            {
                foundBornDate = true;
            }
            General_DataGridView.Rows.Add(person, integrated, grade, bornDate, schoolRecordId);
        }
        //****************************************************************************************************************************
        private void ViewIntegratedPerson_button_Click(object sender, System.EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int schoolRecordID = dataGridViewRow.Cells[4].Value.ToInt();
            DataTable schoolRecord_tbl = SQL.GetSchoolRecord(schoolRecordID);
            if (schoolRecord_tbl.Rows.Count == 0)
            {
                return;
            }
            int personId = schoolRecord_tbl.Rows[0][U.PersonID_col].ToInt();
            if (personId == 0)
                MessageBox.Show("Person Has not been integrated");
            else
            {
                FPerson Person = new FPerson(m_SQL, personId, false);
                Person.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private bool GetMessage()
        {
            string msg1 = "ID1: " + "\r";
            string msg2 = "ID2: " + "\r";
            string msg3 = "ID3: " + "\r";
            string msg4 = "ID4: " + "\r";
            string msg5 = "ID5: " + "\r";
            FMsgBox msgBox = new FMsgBox(msg1, msg2, msg3, msg4, msg5);
            msgBox.ShowDialog();
            return msgBox.yesno == FMsgBox.Yesno.yes;
        }
        //****************************************************************************************************************************
        private void IntegrateStudent_Click(object sender, EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int schoolRecordID = dataGridViewRow.Cells[4].Value.ToInt();
            DataTable SchoolRecord_tbl = SQL.GetSchoolRecord(schoolRecordID);
            if (SchoolRecord_tbl.Rows.Count == 0)
            {
                MessageBox.Show("Unable to Get School Record: " + schoolRecordID);
                return;
            }
            string alreadyIntegrated = dataGridViewRow.Cells[1].Value.ToString().Trim();
            if (!String.IsNullOrEmpty(alreadyIntegrated))
            {
                string msg = "This Person Has Already Been Integrated.  Do you wish to reintegrate?\rClick Cancel to remove Integration";
                DialogResult result = MessageBox.Show(msg, "", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes: break;
                    case DialogResult.No: return;
                    case DialogResult.Cancel:
                        RemoveIntegration(dataGridViewRow, SchoolRecord_tbl);
                        return;
                    default: return;
                }
            }
            IntegrateStudent(dataGridViewRow, SchoolRecord_tbl);
        }
        //****************************************************************************************************************************
        private void RemoveIntegration(DataGridViewRow dataGridViewRow, DataTable SchoolRecord_tbl)
        {
            SchoolRecord_tbl.Rows[0]["PersonID"] = 0;
            if (!SQL.SaveIntegratedSchoolRecords(SchoolRecord_tbl))
            {
                MessageBox.Show("Integrate Unsuccesful");
            }
            dataGridViewRow.Cells[1].Value = " ";
        }
        //****************************************************************************************************************************
        private void IntegrateStudent(DataGridViewRow dataGridViewRow, DataTable SchoolRecord_tbl)
        {
            DataRow SchoolRecord_row = SchoolRecord_tbl.Rows[0];
            DataTable AlternativeSpellingsFirstNameTbl = SQL.GetAllAlternativeSpellings(U.AlternativeSpellingsFirstName_Table);
            DataTable AlternativeSpellingsLastNameTbl = SQL.GetAllAlternativeSpellings(U.AlternativeSpellingsLastName_Table);

            CIntegrateSchoolRecord integrateSchoolRecord = new CIntegrateSchoolRecord(m_SQL, true);
            int originalPersonId = SchoolRecord_row["PersonID"].ToInt();
            bool saveRecord = false;
            if (integrateSchoolRecord.IntegrateRecord(SchoolRecord_row,
                                                  AlternativeSpellingsFirstNameTbl,
                                                  AlternativeSpellingsLastNameTbl))
            {
                saveRecord = true;
                dataGridViewRow.Cells[1].Value = "x";
            }
            else
            if (originalPersonId != 0)
            {
                //saveRecord = true;
                dataGridViewRow.Cells[1].Value = "x";
            }
            if (saveRecord && !SQL.SaveIntegratedSchoolRecords(SchoolRecord_tbl))
            {
                MessageBox.Show("Integrate Unsuccesful");
            }
        }
        //****************************************************************************************************************************
        private bool PersonIntegrated(bool NameIntegratedChecked,
                                      string sLastName,
                                      string sMaidenName)
        {
            return (NameIntegratedChecked || (sLastName.Length == 0 && sMaidenName.Length == 0));
        }
        //****************************************************************************************************************************
    }
}
