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
    class CGridPersonCW : FGrid
    {
        private CSql m_SQL;
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
        public CGridPersonCW(ref CSql Sql,
                                  bool bExitOnDoubleClick)
        {
            m_SQL = Sql;
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
            this.Text = "Person Cival War Information";
            this.BackColor = System.Drawing.Color.LightSteelBlue;
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
            General_DataGridView.ColumnCount = 8;
            General_DataGridView.Columns[0].Name = "Name";
            General_DataGridView.Columns[0].Width = 300;
            General_DataGridView.Columns[0].Visible = true;
            General_DataGridView.Columns[1].Name = "";
            General_DataGridView.Columns[1].Width = 10;
            General_DataGridView.Columns[1].Visible = true;
            General_DataGridView.Columns[2].Name = "Birth Date";
            General_DataGridView.Columns[2].Width = 100;
            General_DataGridView.Columns[2].Visible = true;
            General_DataGridView.Columns[3].Name = "Enlistment";
            General_DataGridView.Columns[3].Width = 100;
            General_DataGridView.Columns[3].Visible = true;
            General_DataGridView.Columns[4].Name = "Died Date";
            General_DataGridView.Columns[4].Width = 100;
            General_DataGridView.Columns[4].Visible = true;
            General_DataGridView.Columns[5].Name = "Cemetery";
            General_DataGridView.Columns[5].Width = 300;
            General_DataGridView.Columns[5].Visible = true;
            General_DataGridView.Columns[6].Name = "Battle Site";
            General_DataGridView.Columns[6].Width = 300;
            General_DataGridView.Columns[6].Visible = true;
            General_DataGridView.Columns[7].Name = "RecordID";
            General_DataGridView.Columns[7].Width = 40;
            General_DataGridView.Columns[7].Visible = false;

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
                toolStripItem1.Click += new EventHandler(IntegratePersonCW_Click);
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
            DataTable personCW_tbl = SQL.GetAllPersonCWRecords();
            bool foundBornDate = false;
            foreach (DataRow personCW_row in personCW_tbl.Rows)
            {
                m_iNumGridElements++;
                AddPersonCWRecord(personCW_row, ref foundBornDate);
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
        private void AddPersonCWRecord(DataRow personCW_row, ref bool foundBornDate)
        {
            string person = SQL.BuildNameLastNameFirst(personCW_row[U.FirstName_col].ToString(),
                                       personCW_row[U.MiddleName_col].ToString(),
                                       personCW_row[U.LastName_col].ToString(), "", "", "", "");
            string bornDate = personCW_row[U.BornDate_col].ToString();
            string EnlistmentDate = personCW_row[U.EnlistmentDate_col].ToString();
            string diedDate = personCW_row[U.DiedDate_col].ToString();
            string cemeteryName = personCW_row[U.CemeteryName_col].ToString();
            string battleSiteKilled = personCW_row[U.BattleSiteKilled_col].ToString();
            string integrated = (personCW_row[U.PersonID_col].ToInt() == 0) ? "" : "x";
            int personCWID = personCW_row[U.PersonCWID_col].ToInt();
            if (!String.IsNullOrEmpty(bornDate))
            {
                foundBornDate = true;
            }
            General_DataGridView.Rows.Add(person, integrated, bornDate, EnlistmentDate, diedDate, cemeteryName, battleSiteKilled, personCWID);
        }
        //****************************************************************************************************************************
        private void ViewIntegratedPerson_button_Click(object sender, System.EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int personCWID = dataGridViewRow.Cells[7].Value.ToInt();
            DataTable personCWRecord_tbl = SQL.GetPersonCWRecord(personCWID);
            if (personCWRecord_tbl.Rows.Count == 0)
            {
                return;
            }
            int personId = personCWRecord_tbl.Rows[0][U.PersonID_col].ToInt();
            if (personId == 0)
                MessageBox.Show("Person Has not been integrated");
            else
            {
                FPerson Person = new FPerson(m_SQL, personId, false);
                Person.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private void IntegratePersonCW_Click(object sender, EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            string alreadyIntegrated = dataGridViewRow.Cells[1].Value.ToString().Trim();
            if (!String.IsNullOrEmpty(alreadyIntegrated))
            {
                if (MessageBox.Show("This Person Has Already Been Integrated.  Do you wish to reintegrate?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            int PersonCWID = dataGridViewRow.Cells[7].Value.ToInt();
            DataTable personCW_tbl = SQL.GetPersonCWRecord(PersonCWID);
            if (personCW_tbl.Rows.Count == 0)
            {
                MessageBox.Show("Unable to Get Person Cival War Record: " + PersonCWID);
                return;
            }
            DataRow personCW_row = personCW_tbl.Rows[0];
            CIntegratePersonCW integratePersonCW = new CIntegratePersonCW(m_SQL, true);
            if (integratePersonCW.IntegrateRecord(personCW_row))
            {
                if (SQL.SaveIntegratedPersonCWRecords(personCW_tbl))
                {
                    dataGridViewRow.Cells[1].Value = "x";
                }
                else
                {
                    MessageBox.Show("Integrate Unsuccesful");
                }
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
