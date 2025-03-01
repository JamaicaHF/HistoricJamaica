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
    class CGridModernRoads : FGrid
    {
        CSql m_SQL;
        private int m_iSelectedRoad = -1;
        protected ContextMenuStrip ToolStrip = new ContextMenuStrip();
        protected ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        //****************************************************************************************************************************
        public int SelectedRoad
        {
            get
            {
                if (m_iSelectedRoad < 0)
                    return 0;
                else
                    return m_iSelectedRoad;
            }
        }
        //****************************************************************************************************************************
        public CGridModernRoads(CSql SQL)
        {
            m_SQL = SQL;
            buttonPane.Visible = false;
            Abort_Button.Visible = false;
            Filter_Button.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void SetupLayout()
        {
            General_DataGridView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SelectRowButton_DoubleClick);
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            int iSelectedRow = this.General_DataGridView.SelectedRows[0].Index;
            m_iSelectedRoad = General_DataGridView.Rows[iSelectedRow].Cells[1].Value.ToInt();
            this.Close();
        }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
            Location = new System.Drawing.Point(500, 100);
            Size = new Size(200, 750);
            Controls.Add(General_DataGridView);
            this.BackColor = Color.LightSteelBlue;

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
            General_DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
//            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);
            DataGridViewTextBoxColumn ModernRoads = new DataGridViewTextBoxColumn();
            ModernRoads.MaxInputLength = U.iMaxNameLength;
            ModernRoads.HeaderText = "Modern Roads";
            General_DataGridView.Columns.Add(ModernRoads);

            toolStripItem1.Text = "Map click";
            toolStripItem1.Click += new EventHandler(Map_Click);
            ToolStrip.Items.Add(toolStripItem1);
            foreach (DataGridViewColumn column in General_DataGridView.Columns)
            {
                column.ContextMenuStrip = ToolStrip;
            }

            General_DataGridView.ColumnCount = 2;
            General_DataGridView.Columns[0].Name = "Modern Roads";
            General_DataGridView.Columns[0].Width = 200;
            General_DataGridView.Columns[1].Name = "ID";
            General_DataGridView.Columns[1].Visible = false;
        }
        private void Map_Click(object sender, EventArgs e)
        {
            DataTable tbl = new DataTable(U.Map_Table);
            SQL.SelectAll(U.Map_Table, tbl);
            string sMapFileName = SQL.MapFileName(tbl.Rows[4]);
            Bitmap HFPhoto = new Bitmap(sMapFileName);
            FPhotoFullSize PhotoFullSize = new FPhotoFullSize(HFPhoto);
            PhotoFullSize.ShowDialog();
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            DataTable tbl = new DataTable();
            SQL.GetAllCategoryValues(tbl,3);
            General_DataGridView.Rows.Clear();
            if (tbl.Rows.Count != 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    int iCategoryValueId = row[U.CategoryValueID_col].ToInt();
                    string SCategoryValue = row[U.CategoryValueValue_col].ToString();
                    General_DataGridView.Rows.Add(SCategoryValue, iCategoryValueId);
                }
            }
        }
        //****************************************************************************************************************************
    }
}
