using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    class CGridPropertyID : FGrid
    {
        private CSql m_SQL;
        private DataTable m_tbl;
        private int m_iNumElements = 0;
        private int m_iSelectedIDLocation = U.Exception;
        private int m_iSelectedGrandListIDLocation = U.Exception;
        private int m_iSelectedOwnerName1Location = 0;
        private int m_iSelectedOwnerName2Location = 0;
        private int m_iSelectedStreetNameLocation = 0;
        private DataGridViewCellEventArgs mouseLocation;
        ContextMenuStrip strip = new ContextMenuStrip();
        private int m_SelectedPropertyID = 0;
        private string m_SelectedGrandListID = "";
        private string m_OwnerName1 = "";
        private string m_OwnerName2 = "";
        private string m_StreetName = "";
        private int m_StreetNum = 0;
        //****************************************************************************************************************************
        public int SelectedPropertyID
        {
            get
            {
                return m_SelectedPropertyID;
            }
        }
        //****************************************************************************************************************************
        public string SelectedGrandListID
        {
            get { return m_SelectedGrandListID; }
        }
        //****************************************************************************************************************************
        public string OwnerName1
        {
            get { return m_OwnerName1; }
        }
        //****************************************************************************************************************************
        public string OwnerName2
        {
            get { return m_OwnerName2; }
        }
        //****************************************************************************************************************************
        public string StreetName
        {
            get { return m_StreetName; }
        }
        //****************************************************************************************************************************
        public int StreetNum
        {
            get { return m_StreetNum; }
        }
        //****************************************************************************************************************************
        public CGridPropertyID(CSql SQL,
                               DataTable tbl)
        {
            m_SQL = SQL;
            m_tbl = tbl;
            buttonPane.Visible = true;
            this.Text = "Grand List";
            Abort_Button.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            int iSelectedRow = General_DataGridView.SelectedRows[0].Index;
            if (iSelectedRow >= m_iNumElements)
            {
                m_SelectedPropertyID = 0;
                m_SelectedGrandListID = "";
                m_OwnerName1 = "";
                m_OwnerName2 = "";
                m_StreetName = "";
                m_StreetNum = 0;
            }
            else
            {
                DataGridViewRow s = General_DataGridView.Rows[iSelectedRow];
                m_SelectedPropertyID = s.Cells[m_iSelectedIDLocation].Value.ToInt();
                m_SelectedGrandListID = s.Cells[m_iSelectedGrandListIDLocation].Value.ToString();
                m_OwnerName1 = s.Cells[m_iSelectedOwnerName1Location].Value.ToString();
                m_OwnerName2 = s.Cells[m_iSelectedOwnerName2Location].Value.ToString();
                m_StreetName = s.Cells[m_iSelectedStreetNameLocation].Value.ToString();
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
            General_DataGridView.ColumnCount = 15;
            General_DataGridView.Columns[0].Name = "StreetName";
            General_DataGridView.Columns[0].Width = 200;
            General_DataGridView.Columns[1].Name = "";
            General_DataGridView.Columns[1].Width = 20;
            General_DataGridView.Columns[2].Name = "Name1";
            General_DataGridView.Columns[2].Width = 200;
            General_DataGridView.Columns[3].Name = "Name2";
            General_DataGridView.Columns[3].Width = 200;
            General_DataGridView.Columns[4].Name = "AddressA";
            General_DataGridView.Columns[4].Width = 200;
            General_DataGridView.Columns[5].Name = "AddressB";
            General_DataGridView.Columns[5].Width = 200;
            General_DataGridView.Columns[6].Name = "City";
            General_DataGridView.Columns[6].Width = 150;
            General_DataGridView.Columns[7].Name = "St.";
            General_DataGridView.Columns[7].Width = 25;
            General_DataGridView.Columns[8].Name = "Zip";
            General_DataGridView.Columns[8].Width = 50;
            General_DataGridView.Columns[9].Name = "Description";
            General_DataGridView.Columns[9].Width = 200;
            General_DataGridView.Columns[10].Name = "Location A";
            General_DataGridView.Columns[10].Width = 200;
            General_DataGridView.Columns[11].Name = "Location B";
            General_DataGridView.Columns[11].Width = 200;
            General_DataGridView.Columns[11].Name = "Location C";
            General_DataGridView.Columns[11].Width = 200;
            General_DataGridView.Columns[11].Name = "PropID";
            General_DataGridView.Columns[11].Width = 80;
            General_DataGridView.Columns[11].Name = "ID";
            General_DataGridView.Columns[11].Width = 50;
            m_iSelectedIDLocation = 14;
            m_iSelectedGrandListIDLocation = 13;
            m_iSelectedOwnerName1Location = 2;
            m_iSelectedOwnerName2Location = 3;
            m_iSelectedStreetNameLocation = 0;

            General_DataGridView.CellMouseEnter += dataGridView_CellMouseEnter;
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = strip;
            }
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues()
        {
            m_iNumElements = 0;
            General_DataGridView.Rows.Clear();
            foreach (DataRow row in m_tbl.Rows)
            {
                string sStreetAddress = U.ConvertStreetNames(row[U.StreetNum_col].ToInt(), row[U.StreetName_col].ToString());
                General_DataGridView.Rows.Add(sStreetAddress, row[U.WhereOwnerLiveID_col], 
                                              row[U.Name1_col], row[U.Name2_col], row[U.AddressA_col], row[U.AddressB_col],
                                              row[U.City_col], row[U.State_col], row[U.Zip_col],
                                              row[U.Description_col], row[U.LoactionA_col], row[U.LoactionB_col],
                                              row[U.LoactionC_col], row[U.GrandListIDChar_col],
                                              row[U.GrandListID_col]);
                m_iNumElements++;
            }
            m_iNumGridElements = m_tbl.Rows.Count;
            this.Location = new Point(325, 80);
//            SetSizeOfGrid(U.iMaxSizeOfGrid-200);
            this.Size = new Size(900, 600);
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
    }
}
