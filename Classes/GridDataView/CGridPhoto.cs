using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    public class CGridPhoto : FGrid
    {
        private CSql m_SQL;
        private string m_sTable;
        private Panel buttonPanel = new Panel();
        private DataTable m_tbl;
        private int m_iSelectedRow = -1;
        ContextMenuStrip strip = new ContextMenuStrip();
        ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        private DataGridViewCellEventArgs mouseLocation;
        private int m_iStartingRow = 0;
        private bool m_bAllowDragAndDrop = false;
        //****************************************************************************************************************************
        public int SelectedRow
        {
            get { return m_iSelectedRow; }
        }
        //****************************************************************************************************************************
        public CGridPhoto(ref CSql SQL,
                                    ref DataTable tbl,
                                    string sTable,
                                    int startingRow = 0,
                                    bool allowDragAndDrop = false)
        {
            m_SQL = SQL;
            m_tbl = tbl;
            m_iNumGridElements = m_tbl.Rows.Count;
            m_sTable = sTable;
            m_iStartingRow = startingRow;
            m_bAllowDragAndDrop = allowDragAndDrop;
            buttonPane.Visible = false;
        }
        public void ReorderSlideShow(DataTable slideShowTbl)
        {
            foreach (DataRow row in slideShowTbl.Rows)
            {
                GetSequence(row);
            }
        }
        private void GetSequence(DataRow row)
        {
            int photoId = row[U.PhotoID_col].ToInt();
            int sequence = 0;
            foreach (DataGridViewRow gridRow in General_DataGridView.Rows)
            {
                if (gridRow.Cells[U.PhotoID_col].Value.ToInt() == photoId)
                {
                    if (row[U.PhotoSequence_col].ToInt() != sequence)
                    {
                        row[U.PhotoSequence_col] = sequence;
                    }
                    return;
                }
                sequence++;
            }
            MessageBox.Show("Unable to Find Photo: " + photoId);
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            m_iSelectedRow = this.General_DataGridView.SelectedRows[0].Index;
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
//            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);

            General_DataGridView.ColumnCount = 4;
            General_DataGridView.Columns[0].Name = "Name";
            General_DataGridView.Columns[0].Width = 250;
            General_DataGridView.Columns[1].Name = "Approximate Year";
            General_DataGridView.Columns[1].Width = 122;
            General_DataGridView.Columns[2].Name = "Notes";
            General_DataGridView.Columns[2].Width = 700;
            General_DataGridView.Columns[3].Name = U.PhotoID_col;
            General_DataGridView.Columns[3].Visible = false;

            if (m_bAllowDragAndDrop)
            {
                this.General_DataGridView.AllowDrop = true;
                //this.General_DataGridView.MouseClick += new MouseEventHandler(dataGridView_MouseClick);
                //this.General_DataGridView.DragEnter += new DragEventHandler(dataGridView_DragEnter);
                //this.General_DataGridView.DragDrop += new DragEventHandler(dataGridView_DragDrop);
                this.General_DataGridView.MouseMove += new MouseEventHandler(dataGridView_MouseMove);
                this.General_DataGridView.MouseDown += new MouseEventHandler(dataGridView_MouseDown);
                this.General_DataGridView.DragOver += new DragEventHandler(dataGridView_DragOver);
                this.General_DataGridView.DragDrop += new DragEventHandler(dataGridView_DragDrop);
            }

            //General_DataGridView.CellMouseEnter += dataGridView_CellMouseEnter;
//            toolStripItem1.Text = "Delete Photo From Database";
//            toolStripItem1.Click += new EventHandler(DeletePhoto_Click);
//            strip.Items.Add(toolStripItem1);
//            foreach (DataGridViewColumn column in General_DataGridView.Columns)
//            {
//                column.ContextMenuStrip = strip;
//            }
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
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            foreach (DataRow row in m_tbl.Rows)
            {
                DataRowState rowstate = row.RowState;
                if (rowstate != DataRowState.Deleted)
                {
                    string sPhotoID = row[m_sTable + "ID"].ToString();
                    DataTable tbl = new DataTable(m_sTable);
                    SQL.SelectAll(m_sTable, tbl, new NameValuePair(m_sTable + "ID", sPhotoID));
                    if (tbl.Rows.Count > 0)
                    {
                        DataRow dsrow = tbl.Rows[0];
                        string sNotes = dsrow[m_sTable + "Notes"].ToString();
                        General_DataGridView.Rows.Add(dsrow[m_sTable + "Name"].ToString(),
                                                      SQL.TableYear(dsrow, m_sTable + "Year"),
                                                      sNotes.Notes(),
                                                      dsrow[m_sTable + "ID"].ToInt());
                    }
                }
            }
            General_DataGridView.FirstDisplayedScrollingRowIndex = m_iStartingRow;
            m_iNumGridElements = m_tbl.Rows.Count;
            SetSizeOfGrid(1082);
        }
    }
}
