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
    class CGridRoadBuildings : FGrid
    {
        private CSql m_SQL;
        private int m_iModernRoadID;
        private int m_iBuildingID;
        private int m_iSelectedRow = -1;
        private DataGridViewCellEventArgs mouseLocation;
        private FlowLayoutPanel FlowLayoutPanel1 = new FlowLayoutPanel();
        private ListBox Values_listBox = new System.Windows.Forms.ListBox();
        private string m_sStreetName;
        private bool m_bExitOnDoubleClick = false;
        //****************************************************************************************************************************
        public int SelectedRow
        {
            get { return m_iSelectedRow; }
        }
        //****************************************************************************************************************************
        public CGridRoadBuildings(ref CSql Sql,
                                  int iModernRoadID,
                                  int iBuildingID,
                                  bool bExitOnDoubleClick)
        {
            m_SQL = Sql;
            m_iModernRoadID = iModernRoadID;
            m_iBuildingID = iBuildingID;
            m_sStreetName = SQL.GetModernRoadName(m_iModernRoadID);
            InitializeComponent();
            this.Values_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Values_listBox.FormattingEnabled = true;
            this.Values_listBox.Location = new System.Drawing.Point(0, 0);
            this.Values_listBox.BorderStyle = BorderStyle.None;
            this.Values_listBox.Name = "Values_listBox";
            this.Values_listBox.Size = new System.Drawing.Size(800,500);
            Abort_Button.Visible = false;
            Filter_Button.Visible = false;
            buttonPane.Visible = false;
            m_bExitOnDoubleClick = bExitOnDoubleClick;
        }
        //****************************************************************************************************************************
        private string GetNewBuildingName()
        {
            CGridGroupValuesBuilding GridGroupValuesBuilding = new CGridGroupValuesBuilding(m_SQL, m_iModernRoadID, 0, 0, true);
            GridGroupValuesBuilding.ShowDialog();
            return GridGroupValuesBuilding.GetNewBuildingName();
        }
        //****************************************************************************************************************************
        private int GetBuildingID(int iOriginalBuildingID, int grandListId)
        {
            //if (grandListId == 0)
            {
              //  return iOriginalBuildingID;
            }
            CGridGroupValuesBuilding GridGroupValuesBuilding = new CGridGroupValuesBuilding(m_SQL, m_iModernRoadID, grandListId, iOriginalBuildingID);
            GridGroupValuesBuilding.ShowDialog();
            return GridGroupValuesBuilding.SelectedGroupID;
        }
        //****************************************************************************************************************************
        protected override void SelectRowButton_DoubleClick(object sender, EventArgs e)
        {
            if (!m_bExitOnDoubleClick)
                return;
            m_iSelectedRow = BuildingIDFromGrid();
            if (m_iSelectedRow == 0)
            {
                string message = "This building is not assigned to an historic building";
                MessageBox.Show(message);
                return;
            }
            this.Close();
        }
        //****************************************************************************************************************************
        void General_DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
//            e.CellStyle.WrapMode = DataGridViewTriState.True;
        }
        //****************************************************************************************************************************
        protected override void SetupDataGridView()
        {
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Text = m_sStreetName;
            FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            FlowLayoutPanel1.Location = new System.Drawing.Point(670, 10);
            FlowLayoutPanel1.AutoSize = true;
            FlowLayoutPanel1.Controls.Add(this.Values_listBox);

            Controls.Add(FlowLayoutPanel1);
            Controls.Add(General_DataGridView);
//            General_DataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            General_DataGridView.Location = new System.Drawing.Point(100, 100);
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
//            General_DataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(General_DataGridView_CellFormatting);
//            General_DataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(General_DataGridView_CellPainting); ;
            General_DataGridView.ColumnCount = 11;
            General_DataGridView.Columns[0].Name = "";
            General_DataGridView.Columns[0].Width = 300;
            General_DataGridView.Columns[0].Visible = true;
            General_DataGridView.Columns[1].Name = "Odd ID";
            General_DataGridView.Columns[1].Width = 22;
            General_DataGridView.Columns[1].Visible = false;
            General_DataGridView.Columns[2].Name = "Address";
            General_DataGridView.Columns[2].Width = 150;
            General_DataGridView.Columns[2].Visible = false;
            General_DataGridView.Columns[3].Name = "Current Owner";
            General_DataGridView.Columns[3].Width = 150;
            General_DataGridView.Columns[3].Visible = false;
            General_DataGridView.Columns[4].Name = "Second Owner";
            General_DataGridView.Columns[4].Width = 150;
            General_DataGridView.Columns[4].Visible = false;
            General_DataGridView.Columns[5].Name = "";
            General_DataGridView.Columns[5].Width = 50;
            General_DataGridView.Columns[5].Visible = true;
            General_DataGridView.Columns[6].Name = "";
            General_DataGridView.Columns[6].Width = 300;
            General_DataGridView.Columns[6].Visible = true;
            General_DataGridView.Columns[7].Name = "Even ID";
            General_DataGridView.Columns[7].Width = 22;
            General_DataGridView.Columns[7].Visible = false;
            General_DataGridView.Columns[8].Name = "Address";
            General_DataGridView.Columns[8].Width = 150;
            General_DataGridView.Columns[8].Visible = false;
            General_DataGridView.Columns[9].Name = "Current Owner";
            General_DataGridView.Columns[9].Width = 150;
            General_DataGridView.Columns[9].Visible = false;
            General_DataGridView.Columns[10].Name = "Second Owner";
            General_DataGridView.Columns[10].Width = 150;
            General_DataGridView.Columns[10].Visible = false;

            General_DataGridView.SelectionChanged += new EventHandler(Cell_clicked);

            General_DataGridView.CellDoubleClick += dataGridView_CellMouseEnter;
            General_DataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(Right_Click);
        }
        //****************************************************************************************************************************
        private bool IsExistingBuilding(DataGridViewRow dataGridViewRow,
                                        int col)
        {
            if (dataGridViewRow.Cells[col + 3].Value == null)
            {
                return false;
            }
            else
            {
                string sName1 = dataGridViewRow.Cells[col + 3].Value.ToString();
                return (sName1.Length != 0);
            }
        }
        //****************************************************************************************************************************
        private void Right_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
                int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
                int iGrandListOrBuildingID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                int iBuildingID = GetBuildingIDFromGrandListId(dataGridViewRow, iGrandListOrBuildingID, iCol);
                ShowRightClickOptions(dataGridViewRow, iCol, iGrandListOrBuildingID, iBuildingID);
            }
        }
        //****************************************************************************************************************************
        private ContextMenuStrip ShowRightClickOptions(DataGridViewRow dataGridViewRow, int iCol, int iGrandListOrBuildingID, int iBuildingID)
        {
            ContextMenuStrip strip = new ContextMenuStrip();
            if (dataGridViewRow.Cells[iCol].Value == null || dataGridViewRow.Cells[iCol].Value.ToString() == "")
            {
                ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
                toolStripItem1.Text = "Add Old Building No Longer Standing";
                toolStripItem1.Click += new EventHandler(BuildingNotOnGrandList_Click);
                strip.Items.Add(toolStripItem1);
            }
            else if (iBuildingID == 0)
            {
                ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
                toolStripItem1.Text = "Define New Building";
                toolStripItem1.Click += new EventHandler(DefineNewBuilding_Click);
                strip.Items.Add(toolStripItem1);
            }
            else
            {
                ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
                toolStripItem1.Text = "View Photographs for Building";
                toolStripItem1.Click += new EventHandler(ViewPhotosForBuilding_Click);
                strip.Items.Add(toolStripItem1);
                ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
                toolStripItem2.Text = "Modify Building";
                toolStripItem2.Click += new EventHandler(ModifyBuilding_Click);
                strip.Items.Add(toolStripItem2);
                ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
                toolStripItem3.Text = "Remove Building";
                toolStripItem3.Click += new EventHandler(RemoveBuilding_Click);
                strip.Items.Add(toolStripItem3);
                ToolStripMenuItem toolStripItem4 = new ToolStripMenuItem();
                toolStripItem4.Text = "Delete Building From Datebase";
                toolStripItem4.Click += new EventHandler(DeleteBuilding_Click);
                strip.Items.Add(toolStripItem4);
                if (iBuildingID != 0)
                {
                    ToolStripMenuItem toolStripItem5 = new ToolStripMenuItem();
                    toolStripItem5.Text = "Change Relative Address";
                    toolStripItem5.Click += new EventHandler(ChangeRelativeAddress_Click);
                    strip.Items.Add(toolStripItem5);
                    ToolStripMenuItem toolStripItem6 = new ToolStripMenuItem();
                    toolStripItem6.Text = "Update QRCode";
                    toolStripItem6.Click += new EventHandler(UpdateQRCode_Click);
                    strip.Items.Add(toolStripItem6);
                    ToolStripMenuItem toolStripItem7 = new ToolStripMenuItem();
                    toolStripItem7.Text = "Modify Architecture Notes";
                    toolStripItem7.Click += new EventHandler(BuildingArcitle_Click);
                    strip.Items.Add(toolStripItem7);
                    ToolStripMenuItem toolStripItem8 = new ToolStripMenuItem();
                    toolStripItem8.Text = "Modify Building Description";
                    toolStripItem8.Click += new EventHandler(BuildingDescription_Click);
                    strip.Items.Add(toolStripItem8);
                }
            }
            dataGridViewRow.Cells[iCol].ContextMenuStrip = strip;
            return strip;
        }
        //****************************************************************************************************************************
        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }
        //****************************************************************************************************************************
        private void ViewPhotosForBuilding_Click(object sender, EventArgs e)
        {
            int iBuildingID = BuildingIDFromGrid();
            DataTable Photo_tbl = new DataTable("BuildingPhotos");
            Photo_tbl.PrimaryKey = new DataColumn[] { Photo_tbl.Columns[U.PhotoID_col] };
            DataTable GroupValue_tbl = new DataTable();
            SQL.GetAllBuildingPhotosFromID(Photo_tbl, iBuildingID);
            if (Photo_tbl.Rows.Count == 0)
            {
                return;
            }
            FPhotoViewer PhotoViewer = new FPhotoViewer(ref m_SQL, Photo_tbl, "BuildingPhotos", FHistoricJamaica.RunPhotoSlideShow, true);
            PhotoViewer.ShowDialog();
        }
        //****************************************************************************************************************************
        private int BuildingIDFromGrid()
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            if (IsExistingBuilding(dataGridViewRow, iCol))
            {
                int iGrandListID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                return SQL.BuildingIDFromGrandListID(iGrandListID);
            }
            else
            {
                return dataGridViewRow.Cells[iCol + 1].Value.ToInt();
            }
        }
        //****************************************************************************************************************************
        private void BuildingArcitle_Click(object sender, EventArgs e)
        {
            GetArticle(U.BuildingArchitectureArticleID_col);
        }
        //****************************************************************************************************************************
        private void BuildingDescription_Click(object sender, EventArgs e)
        {
            GetArticle(U.BuildingDescriptionArticleID_col);
        }
        //****************************************************************************************************************************
        private void GetArticle(string BuildingArticleID_col)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int iGrandListOrBuildingID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
            int iBuildingID = GetBuildingIDFromGrandListId(dataGridViewRow, iGrandListOrBuildingID, iCol);
            int iArticleID = SQL.GetBuildingArticleID(iBuildingID, BuildingArticleID_col);
            int iOldArticleID = iArticleID;
            FNotes Notes = new FNotes(m_SQL, iArticleID, Location.X, Location.Y, iRow);
            Notes.ShowDialog();
            iArticleID = Notes.ArticleID;
            if (iArticleID == 0)
            {
                if (iOldArticleID != 0)
                {
                    SQL.UpdateBuildingArticleID(iBuildingID, BuildingArticleID_col, 0);
                }
            }
            else if (iOldArticleID == 0)
            {
                SQL.UpdateBuildingArticleID(iBuildingID, BuildingArticleID_col, iArticleID);
            }
        }
        //****************************************************************************************************************************
        private void UpdateQRCode_Click(object sender, EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int iGrandListOrBuildingID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
            int iBuildingID = GetBuildingIDFromGrandListId(dataGridViewRow, iGrandListOrBuildingID, iCol);
            UpdateQRCode(dataGridViewRow, iRow, iBuildingID);
            ShowBuildingStatistics();
        }
        //****************************************************************************************************************************
        private void UpdateQRCode(DataGridViewRow dataGridViewRow,
                                     int rowIndex,
                                     int iBuildingId)
        {
            DataTable buildingTbl = SQL.GetBuilding(iBuildingId);
            if (buildingTbl.Rows.Count == 0)
            {
                return;
            }
            string qrcode = buildingTbl.Rows[0][U.QRCode_col].ToString();
            qrcode += "\nThen1=" + buildingTbl.Rows[0][U.Then1Title_col].ToString();
            qrcode += "\nThen2=" + buildingTbl.Rows[0][U.Then2Title_col].ToString();
            qrcode += "\nNow1=" + buildingTbl.Rows[0][U.Now1Title_col].ToString();
            qrcode += "\nNow2=" + buildingTbl.Rows[0][U.Now2Title_col].ToString();
            FNotes Notes = new FNotes(m_SQL, qrcode, Location.X, Location.Y, rowIndex);
            Notes.ShowDialog();
            string newQrcode = Notes.GetText();
            string[] qrcodeStrings = newQrcode.Split('\n');
            if (qrcodeStrings.Length < 5)
            {
                MessageBox.Show("Invalid QRCode String: " + newQrcode);
                return;
            }
            for (int i = 1; i < 5; i++)
            {
                int indexOf = qrcodeStrings[i].IndexOf('=');
                if (indexOf < 0)
                {
                    MessageBox.Show("Invalid QRCode String: " + qrcodeStrings[i]);
                    return;
                }
                qrcodeStrings[i] = qrcodeStrings[i].Substring(indexOf + 1);
            }
            if (newQrcode != qrcode)
            {
                buildingTbl.Rows[0][U.QRCode_col] = qrcodeStrings[0];
                buildingTbl.Rows[0][U.Then1Title_col] = qrcodeStrings[1];
                buildingTbl.Rows[0][U.Then2Title_col] = qrcodeStrings[2];
                buildingTbl.Rows[0][U.Now1Title_col] = qrcodeStrings[3];
                buildingTbl.Rows[0][U.Now2Title_col] = qrcodeStrings[4];
                SQL.UpdateBuildingValues(buildingTbl, U.QRCode_col, U.Then1Title_col, U.Then2Title_col, U.Now1Title_col, U.Now2Title_col);
            }
        }
        //****************************************************************************************************************************
        private void ChangeRelativeAddress_Click(object sender, EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int newAddress;
            do
            {
                newAddress = ChangeRelativeStreetNum(iRow, GetStreetNum(dataGridViewRow, iCol), dataGridViewRow.Cells[iCol + 1].Value.ToInt());
            }
            while (newAddress == 0);
        }
        //****************************************************************************************************************************
        private int GetStreetNum(DataGridViewRow dataGridViewRow, int iCol)
        {
            string addressString = dataGridViewRow.Cells[iCol + 2].Value.ToString();
            int location = addressString.IndexOf(' ');
            return (location < 0) ? 0 : addressString.Substring(0, location).ToInt();
        }
        //****************************************************************************************************************************
        private int ChangeRelativeStreetNum(int iRow, 
                                             int streetNum,
                                             int iBuildingID)
        {
            FBuildingItem BuildingItem = new FBuildingItem(m_SQL, Location.X, Location.Y, iRow, streetNum, "Relative Street Num");
            BuildingItem.ShowDialog();
            int newStreetNum = BuildingItem.BuildingItem;
            if (newStreetNum != 0 && newStreetNum != 9999 && newStreetNum != streetNum)
            {
                SQL.UpdateBuildingValue(iBuildingID, U.StreetNum_col, newStreetNum.ToString());
                Clear(General_DataGridView);
                ShowAllValues("");
                General_DataGridView.Rows[0].Cells[0].Selected = true;
                return newStreetNum;
            }
            return streetNum;
        }
        //****************************************************************************************************************************
        private void Clear(DataGridView dataGridView)
        {
            for (int i = dataGridView.Rows.Count - 2; i >= 0; i--)
            {
                dataGridView.Rows.RemoveAt(i);
            }
        }
        //****************************************************************************************************************************
        private void BuildingNotOnGrandList_Click(object sender, EventArgs e)
        {
            int iRowIndex = this.General_DataGridView.SelectedCells[0].RowIndex;
            string buildingName = GetNewBuildingName();
            if (!String.IsNullOrEmpty(buildingName))
            {
                int buildingId = SQL.InsertBuilding(buildingName, m_iModernRoadID, 0);
                if (buildingId != 0)
                {
                    int streetNum = 0;
                    do
                    {
                        streetNum = ChangeRelativeStreetNum(iRowIndex, streetNum, buildingId);
                    }
                    while (streetNum == 0);
                }
            }
        }
        //****************************************************************************************************************************
        private bool AlreadyDefinedBuilding()
        {
            string message = "Do you wish to use a building already defined?";
            return (MessageBox.Show(message, "", MessageBoxButtons.YesNo) == DialogResult.Yes);
        }
        //****************************************************************************************************************************
        private void DefineNewBuilding_Click(object sender, EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            string sAddress = dataGridViewRow.Cells[iCol + 2].Value.ToString();
            int iGrandListID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
            int iBuildingID = GetBuildingID(0, iGrandListID);
            //dataGridViewRow.Cells[iCol].Value = iBuildingID;
            ShowBuildingStatistics();
        }
        //****************************************************************************************************************************
        private void DeleteBuilding_Click(object sender, EventArgs e)
        {
            if (!UU.MessageReply("Delete Building From Database?"))
            {
                return;
            }
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            if (IsExistingBuilding(dataGridViewRow, iCol))
            {
                int iGrandListID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                SQL.DeleteAllBuildingWithGrandListID(iGrandListID);
            }
            else
            {
                int iBuildingID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                SQL.DeleteBuilding(iBuildingID);
                General_DataGridView.Rows.RemoveAt(iRow);
            }
            ShowBuildingStatistics();
        }
        //****************************************************************************************************************************
        private void RemoveBuilding_Click(object sender, EventArgs e)
        {/*
            if (!UU.MessageReply("Remove Building From Address?"))
            {
                return;
            }
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            if (IsExistingBuilding(dataGridViewRow, iCol))
            {
                int iGrandListID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                SQL.RemoveGrandListIDFromAllBuildings(iGrandListID);
            }
            else
            {
                int iBuildingID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                DataTable tbl = SQL.GetBuilding(iBuildingID);
                if (tbl.Rows.Count != 0)
                {
                    SQL.UpdateBuildingRoadValueID(iBuildingID, 0);
                    General_DataGridView.Rows.RemoveAt(iRow);
                }
            }
            ShowBuildingStatistics();
            */
            MessageBox.Show("Cannot remove Building.  Add Code to Move Building");
        }
        //****************************************************************************************************************************
        private void ModifyBuilding_Click(object sender, EventArgs e)
        {
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            int iGrandListOrBuildingID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
            if (iGrandListOrBuildingID == 0)
                return;
            string sAddress = dataGridViewRow.Cells[iCol + 2].Value.ToString();
            int iBuildingID = GetBuildingIDFromGrandListId(dataGridViewRow, iGrandListOrBuildingID, iCol);
            if (iBuildingID != 0)
            {
                iBuildingID = GetBuildingID(iBuildingID, 0);
                ShowBuildingStatistics();
            }
            else
            {
                MessageBox.Show("No Building has been assigned to this address");
            }
        }
        //****************************************************************************************************************************
        private int GetBuildingIDFromGrandListId(DataGridViewRow dataGridViewRow, 
                                                 int iGrandListOrBuildingID,
                                                 int iCol)
        {
            int iBuildingID = iGrandListOrBuildingID;
            int iGrandListID = 0;
            if (IsExistingBuilding(dataGridViewRow, iCol))
            {
                iGrandListID = iGrandListOrBuildingID;
                return SQL.BuildingIDFromGrandListID(iGrandListOrBuildingID);
            }
            return iGrandListOrBuildingID;
        }
        //****************************************************************************************************************************
        private void Cell_clicked(object sender, EventArgs e)
        {
            ShowBuildingStatistics();
        }
        //****************************************************************************************************************************
        private void ShowNotes(string sHome,
                               string sNotes)
        {
            sNotes = sNotes.Trim().Notes();
            Values_listBox.Items.Add("    " + sHome);
            if (sNotes.Length == 0)
                return;
            bool bFoundHyphen = true;
            while (bFoundHyphen)
            {
                int iIndexOf = sNotes.IndexOf('-');
                if (iIndexOf < 0)
                    bFoundHyphen = false;
                else
                {
                    Values_listBox.Items.Add("        " + sNotes.Substring(0, iIndexOf).Trim());
                    sNotes = sNotes.Remove(0, iIndexOf+1);
                }
            }
            Values_listBox.Items.Add("        " + sNotes.Notes());
        }
        //****************************************************************************************************************************
        private void ShowOldNotes(string sHome,
                               string sNotes)
        {
            sNotes = sNotes.Trim();
            if (sNotes.Length == 0)
            {
                Values_listBox.Items.Add("    " + sHome);
                return;
            }
            int iIndexOf = sNotes.IndexOf('-');
            //            if (iIndexOf == 0)
            Values_listBox.Items.Add("    " + sHome + " - " + sNotes.Notes());
        }
        //****************************************************************************************************************************
        private void ShowBuildingStatistics()
        {
            if (General_DataGridView.Rows.Count <= 1)
                return;
            int iRow = this.General_DataGridView.SelectedCells[0].RowIndex;
            if (NotWithinLimitsOfGrid(iRow))
            {
                return;
            }
            int iCol = this.General_DataGridView.SelectedCells[0].ColumnIndex.ToInt();
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRow];
            string sValue = dataGridViewRow.Cells[iCol].Value.ToString();
            Values_listBox.Items.Clear();

            string sName1 = dataGridViewRow.Cells[iCol + 3].Value.ToString();
            string sName2 = dataGridViewRow.Cells[iCol + 4].Value.ToString();
            string sCurrentOwners = U.CombineName1AndName2(sName1, sName2);
            string sAddress = dataGridViewRow.Cells[iCol + 2].Value.ToString();

            int iBuildingID;
            DataTable Building_tbl;
            if (sCurrentOwners.Length == 0)
            {
                iBuildingID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                Building_tbl = SQL.GetBuilding(iBuildingID);
                Building_tbl = ShowAddressWithQRCode(m_sStreetName, iBuildingID);
            }
            else
            {
                int iGrandListID = dataGridViewRow.Cells[iCol + 1].Value.ToInt();
                iBuildingID = SQL.BuildingIDFromGrandListID(iGrandListID);
                Building_tbl = ShowAddressWithQRCode(sAddress, iBuildingID);
                Values_listBox.Items.Add("");
                ShowCurrentOwners(iGrandListID, sCurrentOwners);
            }
            if (Building_tbl.Rows.Count != 0)
            {
                string OldBuildingName = General_DataGridView.Rows[iRow].Cells[iCol].Value.ToString().Trim();
                string buildingName = ShowBuildings(Building_tbl.Rows[0]).Trim();
                int indexOf = OldBuildingName.IndexOf('-');
                if (indexOf < 0 && buildingName != OldBuildingName)
                {
                        General_DataGridView.Rows[iRow].Cells[iCol].Value = buildingName;
                }
                ShowBuildingOccupants(iBuildingID);
            }
        }
        //****************************************************************************************************************************
        private DataTable ShowAddressWithQRCode(string address, int iBuildingID)
        {
            DataTable Building_tbl = SQL.GetBuilding(iBuildingID);
            if (Building_tbl.Rows.Count > 0)
            {
                string qrCode = Building_tbl.Rows[0][U.QRCode_col].ToString();
                if (!String.IsNullOrEmpty(qrCode))
                {
                    address += " " + qrCode;
                }
            }
            Values_listBox.Items.Add(address);
            return Building_tbl;
        }
        //****************************************************************************************************************************
        private void ShowBuildingOccupants(int iBuildingID)
        {
            DataTable Occupant_tbl = new DataTable();
            SQL.GetAllBuildingOccupants(Occupant_tbl, iBuildingID);
            if (Occupant_tbl.Rows.Count != 0)
            {
                Values_listBox.Items.Add("");
                Values_listBox.Items.Add("Building Occupants");
                foreach (DataRow row in Occupant_tbl.Rows)
                {
                    int iPersonID = row[U.PersonID_col].ToInt();
                    iBuildingID = row[U.BuildingID_col].ToInt();
                    int iSpouseLivedWithID = row[U.SpouseLivedWithID_col].ToInt();
                    string sHome = SQL.PersonHomeName(iPersonID, iBuildingID, iSpouseLivedWithID, false); 
                    string occupantString = U.GetOccupantString(iPersonID, iBuildingID);
                    if (!String.IsNullOrEmpty(occupantString) && occupantString != "(Occupant)")
                    {
                        sHome += " " + occupantString;
                    }
                    string sNotes = SQL.GetBuildingOccupantNotes(iPersonID, iBuildingID);
                    sNotes = BuildingNotes.AddCensusValues(sNotes, iPersonID, iBuildingID);
                    ShowNotes(sHome, sNotes);
                }
            }
        }
        //****************************************************************************************************************************
        private string ShowBuildings(DataRow BuildingRow)
        {
            int iBuildingID = BuildingRow[U.BuildingID_col].ToInt();
            Values_listBox.Items.Add("");
            Values_listBox.Items.Add("Building Names");
            string buildingName = BuildingRow[U.BuildingName_col].ToString();
            ShowNotes(buildingName, BuildingRow[U.Notes_col].ToString());

            ShowMapBuildingName(BuildingRow, U.s1856BuildingNameString, U.Building1856Name_col, U.Notes1856Name_col);
            ShowMapBuildingName(BuildingRow, U.s1869BuildingNameString, U.Building1869Name_col, U.Notes1869Name_col);
            DataTable BuildingValues_tbl = new DataTable();
            SQL.GetAllBuildingValues(BuildingValues_tbl, iBuildingID);
            foreach (DataRow row in BuildingValues_tbl.Rows)
            {
                int iBuildingValueID = row[U.BuildingValueID_col].ToInt();
                string sBuildingValueValue = row[U.BuildingValueValue_col].ToString();
                //if (sBuildingValueValue != BuildingRow[U.BuildingName_col].ToString())
                {
                    string sNotes = row[U.Notes_col].ToString();
                    if (sBuildingValueValue.Trim() != buildingName.Trim() || !String.IsNullOrEmpty(sNotes))
                    {
                        ShowNotes(sBuildingValueValue, sNotes);
                    }
                }
            }
            return buildingName;
        }
        //****************************************************************************************************************************
        private void ShowCurrentOwners(int iGrandListID, string sCurrentOwners)
        {
            DataTable grandListHistoryTbl = SQL.GetGrandListHistory(iGrandListID);
            if (grandListHistoryTbl.Rows.Count == 0)
            {
                Values_listBox.Items.Add("Current Owner");
                Values_listBox.Items.Add("    " + sCurrentOwners);
                return;
            }
            Values_listBox.Items.Add("Owners");
            sCurrentOwners = U.BuildingString(sCurrentOwners, U.iCurrentOwners, 0, 0, 0);
            Values_listBox.Items.Add("    " + sCurrentOwners);
            for (int index = grandListHistoryTbl.Rows.Count - 1; index >= 0; index--)
            {
                DataRow grandListHistoryRow = grandListHistoryTbl.Rows[index];
                string owner = U.CombineName1AndName2(grandListHistoryRow[U.Name1_col].ToString(), grandListHistoryRow[U.Name2_col].ToString());
                owner = U.BuildingString(owner, U.iCurrentOwners, 0, 0, grandListHistoryRow[U.Year_col].ToInt());
                Values_listBox.Items.Add("    " + owner);
            }
        }
        //****************************************************************************************************************************
        private void ShowMapBuildingName(DataRow BuildingRow, string sMapBuildingNameString, string buildingMapNameCol, string notesMapBuildingNameCol)
        {
            string sMapBuildingName = BuildingRow[buildingMapNameCol].ToString();
            if (sMapBuildingName.Length != 0)
            {
                sMapBuildingName += " " + sMapBuildingNameString;
                ShowNotes(sMapBuildingName, BuildingRow[notesMapBuildingNameCol].ToString());
            }
        }
        //****************************************************************************************************************************
        private bool IsOdd(int iNum)
        {
            return (iNum % 2 == 1);
        }
        //****************************************************************************************************************************
        private string GetNextRow(ArrayList addressList,
                                  ref int iCurrentAddress,
                                  bool    bLookingFor,
                                      string sStreetName,
                                  out string sName1,
                                  out string sName2,
                                  out string grandListID,
                                  out string value)
        {
            BuildingInfo info = (BuildingInfo)addressList[iCurrentAddress];
            int iAddress = info.address;
            bool bIs = IsOdd(iAddress);
            if (bLookingFor == bIs)
            {
                grandListID = info.id;
                sName1 = info.name1;
                sName2 = info.name2;
                iCurrentAddress++;
                value = info.displayName;
                return iAddress.ToString() + " " + sStreetName;
            }
            else
            {
                grandListID = "";
                sName1 = "";
                sName2 = "";
                value = "";
                return "";
            }
        }
        //****************************************************************************************************************************
        private void GetData(DataTable building_tbl,
                             int address,
                             out string sName1,
                             out string sName2,
                             out string grandListID)
        {
            sName1 = "";
            sName2 = "";
            grandListID = "";
            foreach (DataRow row in building_tbl.Rows)
            {
                if (row[U.StreetNum_col].ToInt() == address)
                {
                    sName1 = row[U.Name1_col].ToString();
                    sName2 = row[U.Name2_col].ToString();
                    grandListID = row[U.GrandListID_col].ToString();
                    return;
                }
            }
        }
        //****************************************************************************************************************************
        private void ShowBuildingOnGrandList(ArrayList addressList, DataTable Building_tbl, DataTable road_tbl)
        {
            int iCurrentRow = 0;
            int iNumBuildings = addressList.Count;
            while (iCurrentRow < iNumBuildings)
            {
                string sOddGrandListID;
                string sEvenGrandListID = "";
                string sOddName1;
                string sOddName2;
                string sEvenName1 = "";
                string sEvenName2 = "";
                string sEvenAddress = "";
                string sOddValue = "";
                string sEvenValue = "";
                string sOddAddress = GetNextRow(addressList, ref iCurrentRow, true, m_sStreetName,
                                                  out sOddName1, out sOddName2, out sOddGrandListID, out sOddValue);
                if (iCurrentRow < iNumBuildings)
                {
                    sEvenAddress = GetNextRow(addressList, ref iCurrentRow, false, m_sStreetName,
                                                out sEvenName1, out sEvenName2, out sEvenGrandListID, out sEvenValue);
                }
                General_DataGridView.Rows.Add(sEvenValue, sEvenGrandListID, sEvenAddress, sEvenName1, sEvenName2, " ",
                                              sOddValue, sOddGrandListID, sOddAddress, sOddName1, sOddName2);
                m_iNumGridElements++;
            }
        }
        private const int MaxAddress = 999999;
        private struct BuildingInfo
        {
            public int address;
            public string name1;
            public string name2;
            public string displayName;
            public string id;
        };
        //****************************************************************************************************************************
        private ArrayList MergeGrandListWithNoLongerStanding(DataTable building_tbl,
                                                             DataTable road_tbl)
        {
            int buildingIndex = 0;
            int notOnGrandListIndex = 0;
            while (buildingIndex < building_tbl.Rows.Count && building_tbl.Rows[buildingIndex][U.StreetNum_col].ToInt() == 0)
            {
                buildingIndex++;
            }
            int buildingAddress = GetAddress(buildingIndex, building_tbl);
            int notOnGrandListAddress = GetAddress(notOnGrandListIndex, road_tbl);
            ArrayList addressList = new ArrayList();
            while (buildingAddress < MaxAddress || notOnGrandListAddress < MaxAddress)
            {
                BuildingInfo info = new BuildingInfo();
                if (notOnGrandListAddress < buildingAddress)
                {
                    DataRow row = road_tbl.Rows[notOnGrandListIndex];
                    info.address = notOnGrandListAddress;
                    info.name1 = "";
                    info.name2 = "";
                    info.displayName = row[U.BuildingName_col].ToString();
                    info.id = row[U.BuildingID_col].ToString();
                    notOnGrandListAddress = GetAddress(++notOnGrandListIndex, road_tbl);
                }
                else
                {
                    info.address = buildingAddress;
                    DataRow row = building_tbl.Rows[buildingIndex];
                    info.name1 = row[U.Name1_col].ToString();
                    info.name2 = row[U.Name2_col].ToString();
                    info.displayName = buildingAddress.ToString() + " - " + U.CombineName1AndName2(info.name1, info.name2);
                    info.id = row[U.GrandListID_col].ToString();
                    buildingAddress = GetAddress(++buildingIndex, building_tbl);
                }
                addressList.Add(info);
            }
            return addressList;
        }
        //****************************************************************************************************************************
        private int GetAddress(int index, DataTable tbl)
        {
            return (index < tbl.Rows.Count) ? tbl.Rows[index][U.StreetNum_col].ToInt() : MaxAddress;
        }
        //****************************************************************************************************************************
        protected override void ShowAllValues(string startingWith)
        {
            m_iNumGridElements = 0;
            DataTable building_tbl = SQL.GetBuildingByModernRoadID(m_iModernRoadID);
            DataTable road_tbl = SQL.GetBuildingByRoadNoGrandListID(m_iModernRoadID);
            ArrayList addressList = MergeGrandListWithNoLongerStanding(building_tbl, road_tbl);
            if (addressList.Count != 0)
            {
                ShowBuildingOnGrandList(addressList, building_tbl, road_tbl);
            }
            if (m_iBuildingID != 0)
            {
                int iSelectedRow = 0;
                int iSelectedCell = 0;
                int iGrandListID = SQL.GetBuildingGrandListID(m_iBuildingID);
                for (int i = 0; i < m_iNumGridElements; ++i)
                {
                    DataGridViewRow dGrid = General_DataGridView.Rows[i];
                    if (iGrandListID == 0)
                    {
                        int iBuildingID = dGrid.Cells[1].Value.ToInt();
                        if (iBuildingID == m_iBuildingID)
                        {
                            iSelectedRow = i;
                            iSelectedCell = 0;
                            break;
                        }
                    }
                    else
                    {
                        int iOddBuildingID = dGrid.Cells[1].Value.ToInt();
                        if (iOddBuildingID == iGrandListID)
                        {
                            iSelectedRow = i;
                            iSelectedCell = 0;
                            break;
                        }
                        int iEvenBuildingID = dGrid.Cells[7].Value.ToInt();
                        if (iEvenBuildingID == iGrandListID)
                        {
                            iSelectedRow = i;
                            iSelectedCell = 6;
                            break;
                        }
                    }
                }
                General_DataGridView.Rows[iSelectedRow].Cells[iSelectedCell].Selected = true;
            }
            SetSizeOfGrid(1200);
        }
        //****************************************************************************************************************************
        private void AddBuildingNotOnGrandListToDataGrid(int iRow)
        {
            General_DataGridView.Rows.Add("", 0, "", "", "", "", "", "", "", "", "");
            m_iNumGridElements++;
            this.General_DataGridView.Rows[iRow].Selected = true;
            this.General_DataGridView.Rows[iRow].Cells[0].Selected = true;
        }
        //****************************************************************************************************************************
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(1432, 666);
            this.Size = new Size(1200,400);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "CGridRoadBuildings";
            this.ResumeLayout(false);
        }
    }
}
