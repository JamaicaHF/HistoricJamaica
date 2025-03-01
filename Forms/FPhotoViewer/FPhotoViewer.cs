using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public partial class FPhotoViewer : FPaintPhoto
    {
        protected CSql m_SQL;
        protected bool initialLoad = true;
        protected string m_sDefaultSource = "Historic Foundation";
        protected string m_sTableName = "";
        protected int m_iNumPeoplePictured = 0;
        protected int m_iNumBuildingsPictured = 0;
        protected bool m_aborted = false;
        protected Bitmap m_Photo = null;
        protected bool m_bPhotoIsNull = true;
        protected bool m_bLockPhotoFromDisplay = false;
        protected string m_myFileName = "";
        protected string m_OldPhotoName = "";
        protected DataTable m_tbl = null;
        protected int m_iCountOfPhotos;
        protected int m_iCurrentPhoto = 0;
        protected string m_sPhotoSource = "";
        protected bool m_bGetNumPeople;
        protected bool m_bGetNumBuilding;
        protected bool m_CategoryListboxChanged = false;
        protected System.ComponentModel.IContainer components = null;
        protected int m_iPhotoID = 0;
        protected DataSet m_Photo_ds = new DataSet();
        protected ArrayList m_CopyCategoryValues;
        protected bool IsSlideShow = false;
        protected bool m_bshowThenAndNowButtons = true;
        protected int m_buildingIdDisplay = 0;

        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql sql,
                              bool isSlideShow)
        {
            IsSlideShow = false;
            m_SQL = sql;
            InitializePhoto();
            DisplayPhotoInViewer(0);
        }
        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql sql,
                              ArrayList CopyCategoryValues,
                              string sTableName,
                              bool isSlideShow)
        {
            this.IsSlideShow = isSlideShow;
            m_SQL = sql;
            m_CopyCategoryValues = CopyCategoryValues;
            m_sTableName = sTableName;
            InitializePhoto();
            DisplayPhotoInViewer(0);
        }
        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql SQL,
                              int iPersonID,
                              string sTableName,
                              bool isSlideShow)
        {
            m_SQL = SQL;
            m_sTableName = sTableName;
            InitializePhoto();
            DisplayPhotoInViewer(iPersonID);
        }
        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql SQL,
                              DataTable tbl,
                              string sTableName,
                              bool isSlideShow,
                              bool showThenAndNowButtons=false)
        // DataSet already loaded with the photos to view
        {
            m_SQL = SQL;
            m_tbl = tbl;
            //m_tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PhotoID_col] };
            m_sTableName = sTableName;
            InitializePhoto();
            if (showThenAndNowButtons && tbl.Rows.Count > 0)
            {
                m_bshowThenAndNowButtons = showThenAndNowButtons;
                m_buildingIdDisplay = tbl.Rows[0][U.BuildingID_col].ToInt();
                Then_button.Visible = true;
                Now_button.Visible = true;
            }
            DisplayPhotoInViewer(0);
            if (m_sTableName == U.SlideShow_Table)
            {
                buttonSlideShow.Visible = true;
                buttonSlideShow.Text = "Remove";
            }
        }
        //****************************************************************************************************************************
        private void PicturedPersons_listBox_RightClick(object sender, MouseEventArgs e) //(1)
        {
            int indexOfItemUnderMouse = PicturedPersons_listBox.IndexFromPoint(e.X, e.Y);
            RightClickMenu(PicturedPersons_listBox, indexOfItemUnderMouse, true);
        }
        //****************************************************************************************************************************
        private void PicturedBuildings_listBox_RightClick(object sender, MouseEventArgs e) //(1)
        {
            int indexOfItemUnderMouse = PicturedBuildings_ListBox.IndexFromPoint(e.X, e.Y);
            RightClickMenu(PicturedBuildings_ListBox, indexOfItemUnderMouse, false);
        }
        //****************************************************************************************************************************
        private void RightClickMenu(ListBoxWithDoubleClick LBox,
                                    int indexOfItemUnderMouse,
                                    bool bPerson)
        {
            if (indexOfItemUnderMouse < 0)
            {
                if (LBox.ContextMenuStrip != null)
                {
                    LBox.ContextMenuStrip.Visible = false;
                }
                return;
            }
            ContextMenuStrip strip = new ContextMenuStrip();
            ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
            toolStripItem1.Text = "Remove";
            strip.Items.Add(toolStripItem1);
            if (LBox.Items.Count > 1)
            {
                ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
                toolStripItem2.Text = "Move Up";
                toolStripItem2.Click += (bPerson) ? new EventHandler(MovePersonUp_Click)
                                                  : new EventHandler(MoveBuildingUp_Click);
                strip.Items.Add(toolStripItem2);
                ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
                toolStripItem3.Text = "Move Down";
                toolStripItem3.Click += (bPerson) ? new EventHandler(MovePersonDown_Click)
                                                  : new EventHandler(MoveBuildingDown_Click);
                strip.Items.Add(toolStripItem3);
            }
            toolStripItem1.Click += (bPerson) ? new EventHandler(DeletePerson_Click)
                                              : new EventHandler(DeleteBuilding_Click);
            LBox.ContextMenuStrip = strip;
        }
        //****************************************************************************************************************************
        private void SetTextboxModified(TextBox TBox,
                                        ref int iNumItems,
                                        int iNumToAdjust)
        {
            iNumItems = TBox.Text.ToInt() + iNumToAdjust;
            TBox.Text = iNumItems.ToString();
            TBox.Modified = true; // be sure setting modified is after the actual set above
        }
        //****************************************************************************************************************************
        private void DeletePerson_Click(object sender, EventArgs e)
        {
            if (DeleteItem_Click(PicturedPersons_listBox, U.PicturedPerson_Table))
                SetTextboxModified(NumPicturedPersons_textBox, ref m_iNumPeoplePictured, -1);
        }
        //****************************************************************************************************************************
        private void DeleteBuilding_Click(object sender, EventArgs e)
        {
            if (DeleteItem_Click(PicturedBuildings_ListBox, U.PicturedBuilding_Table))
                SetTextboxModified(NumPicturedBuildings_textBox, ref m_iNumBuildingsPictured, -1);
        }
        //****************************************************************************************************************************
        private void MovePersonUp_Click(object sender, EventArgs e)
        {
            if (MoveItemUp(PicturedPersons_listBox, U.PicturedPerson_Table, U.PicturedPersonNumber_col))
                SetTextboxModified(NumPicturedPersons_textBox, ref m_iNumPeoplePictured, -1);
        }
        //****************************************************************************************************************************
        private void MoveBuildingUp_Click(object sender, EventArgs e)
        {
            if (MoveItemUp(PicturedBuildings_ListBox, U.PicturedBuilding_Table, U.PicturedBuildingNumber_col))
                SetTextboxModified(NumPicturedBuildings_textBox, ref m_iNumBuildingsPictured, -1);
        }
        //****************************************************************************************************************************
        private void MovePersonDown_Click(object sender, EventArgs e)
        {
            if (MoveItemDown(PicturedPersons_listBox, U.PicturedPerson_Table, U.PicturedPersonNumber_col))
                SetTextboxModified(NumPicturedPersons_textBox, ref m_iNumPeoplePictured, +1);
        }
        //****************************************************************************************************************************
        private void MoveBuildingDown_Click(object sender, EventArgs e)
        {
            if (MoveItemDown(PicturedBuildings_ListBox, U.PicturedBuilding_Table, U.PicturedBuildingNumber_col))
                SetTextboxModified(NumPicturedBuildings_textBox, ref m_iNumBuildingsPictured, +1);
        }
        //****************************************************************************************************************************
        private bool DeleteItem_Click(ListBoxWithDoubleClick LBox,
                                      string sTable)
        {
            LBox.ContextMenuStrip = null;
            int iRowIndex = LBox.SelectedIndex;
            if (iRowIndex < 0)
                return false;
            string sItem = LBox.Items[iRowIndex].ToString();
            bool bSuccess = false;
            if (sItem != U.Unknown)
            {
                string sMessage = "Remove " + sItem + "?";
                if (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataRow row = m_Photo_ds.Tables[sTable].Rows.Find(iRowIndex + 1);
                    if (row != null)
                    {
                        row.Delete();
                        LBox.Items[iRowIndex] = U.Unknown;
                        bSuccess = true;
                    }
                }
            }
            return bSuccess;
        }
        //****************************************************************************************************************************
        private bool MoveItemUp(ListBoxWithDoubleClick LBox,
                                string sTable,
                                string sNumberColumn)
        {
            LBox.ContextMenuStrip = null;
            int iRowIndex = LBox.SelectedIndex;
            if (iRowIndex < 0)
                return false;
            bool bDoRemove = false;
            if (iRowIndex == 0)
            {
                MessageBox.Show("You Cannot Move The First Item Up");
                return false;
            }
            if (iRowIndex > 0)
            {
                string sItemToMove = LBox.Items[iRowIndex].ToString();
                if (sItemToMove == U.Unknown)
                    return false;
                iRowIndex--;
                string sItem = LBox.Items[iRowIndex].ToString();
                if (sItem == U.Unknown)
                    bDoRemove = true;
                else
                {
                    string sMessage = "Move all from " + sItemToMove + " up Overwriting " + sItem;
                    bDoRemove = (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo) == DialogResult.Yes);
                }
            }
            if (bDoRemove)
            {
                LBox.Items.RemoveAt(iRowIndex);
                int iPhotoNumber = iRowIndex + 1;
                DataViewRowState dvRowState = DataViewRowState.ModifiedCurrent |
                                              DataViewRowState.Unchanged;
                foreach (DataRow row in m_Photo_ds.Tables[sTable].Select("", "", dvRowState))
                {
                    int iRowNumber = row[sNumberColumn].ToInt();
                    if (iRowNumber == iPhotoNumber)
                        row.Delete();
                    else
                    if (iRowNumber > iPhotoNumber)
                        row[sNumberColumn] = iRowNumber - 1;
                }
            }
            return bDoRemove;
        }
        //****************************************************************************************************************************
        private bool MoveItemDown(ListBoxWithDoubleClick LBox,
                                string sTable,
                                string sNumberColumn)
        {
            int indexOfItemUnderMouse = PicturedBuildings_ListBox.SelectedIndex;
            LBox.ContextMenuStrip = null;
            int iRowIndex = LBox.SelectedIndex;
            if (iRowIndex < 0)
                return false;
            LBox.Items.Insert(iRowIndex, U.Unknown);
            int iPhotoNumber = iRowIndex + 1;
            int iNumPhotos = m_Photo_ds.Tables[sTable].Rows.Count;
            for (int i = iNumPhotos - 1; i >= 0; i--)
            {
                DataRow row = m_Photo_ds.Tables[sTable].Rows[i];
                if (row.RowState != DataRowState.Deleted)
                {
                    int iRowNumber = row[sNumberColumn].ToInt();
                    if (iRowNumber >= iPhotoNumber)
                        row[sNumberColumn] = iRowNumber + 1;
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        public ArrayList GetCopyCategoryValues()
        {
            return m_CopyCategoryValues;
        }
        //****************************************************************************************************************************
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (m_bGetNumPeople || m_bGetNumBuilding)
            {
                int KeyValue = msg.WParam.ToInt32();
                switch (KeyValue)
                {
                    case 9:  //tab
                    case 13: //enter
                    case 40: //DownArrow
                    case 16: //shift tab
                    case 38: //UpArrow
                    case 27: //escape
                        {
                            if (m_bGetNumPeople)
                                PeoplePictured_button_Click();
                            else
                                BuildingPictured_button_Click();
                            break;
                        }
                    default: break;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private void MouseClick_click(Object sender, MouseEventArgs e)
        {
        }
        //****************************************************************************************************************************
        //****************************************************************************************************************************
        //****************************************************************************************************************************
        private void panel1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //            Point mouseUpLocation = new System.Drawing.Point(e.X, e.Y);
            //
            //            // Show the number of clicks in the path graphic.
            //            int numberOfClicks = e.Clicks;
            //            mousePath.AddString(UU.ShowGroupValue(numberOfClicks.ToString()),
            //                        FontFamily.GenericSerif, (int)FontStyle.Bold,
            //                        fontSize, mouseUpLocation, StringFormat.GenericDefault);
            //
            //            panel1.Invalidate();
        }

        //****************************************************************************************************************************
        public virtual void DisplayPhotoInViewer(int iPersonID)
        {
            this.Categories_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CategorylistBox_Click);
            this.PicturedPersons_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedPersonlistBox_Click);
            this.PicturedBuildings_ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedBuildinglistBox_Click);
            PicturedPersons_listBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicturedPersons_listBox_RightClick);
            PicturedBuildings_ListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicturedBuildings_listBox_RightClick);
            SQL.DefinePhoto(m_Photo_ds);
            if (m_tbl == null)
            {
                if (iPersonID == 0)
                {
                    m_tbl = SQL.GetAllPhotos();
                }
                else
                {
                    m_tbl = new DataTable(m_sTableName);
                    SQL.selectdistinctPhotoID(m_tbl, m_sTableName, iPersonID);
                    m_tbl.PrimaryKey = new DataColumn[] { m_tbl.Columns[U.PhotoID_col] };
                }
            }
            m_iCountOfPhotos = m_tbl.Rows.Count;
            if (m_iCountOfPhotos == 0)
                m_aborted = true;
            else
            {
                if (m_iCountOfPhotos == 1)
                {
                    this.Previous_button.Visible = false;
                    this.Next_button.Visible = false;
                }
                DisplayPhoto(m_iCurrentPhoto);
            }
        }
        //****************************************************************************************************************************
        public bool PhotoFound()
        {
            return m_Photo != null;
        }
        //****************************************************************************************************************************
        public bool PhotoViewerAborted()
        {
            return m_aborted;
        }
        //****************************************************************************************************************************
        protected void LoadPhotoIntoControls(DataRow row)
        {
            m_iPhotoID = row[U.PhotoID_col].ToInt();
            string sSource = row[U.PhotoSource_col].ToString();
            if (sSource == m_sDefaultSource)
                sSource = sSource.Insert(0, "the Jamaica ");
            PhotoTitle_label.Text = sSource;
            if (!sSource.Contains("Collection"))
            {
                PhotoTitle_label.Text += " Collection";
            }
            PhotoNotes_TextBox.Text = row[U.PhotoNotes_col].ToString();
            int iYear = row[U.PhotoYear_col].ToInt();
            if (iYear == 0)
                PhotoYear_TextBox.Text = "";
            else
                PhotoYear_TextBox.Text = iYear.ToString();
            PhotoSource_comboBox.Text = row[U.PhotoSource_col].ToString();
            m_sPhotoSource = PhotoSource_comboBox.Text.ToString();
            PhotoFolder_textBox.Text = row[U.PhotoDrawer_col].ToString();
            PhotoFolder_textBox.Text = row[U.PhotoFolder_col].ToString();
            string sPhotoName = row[U.PhotoName_col].ToString();
            if (sPhotoName.Length > 8)
                sPhotoName = sPhotoName.Remove(8);
            PhotoName_textBox.Text = sPhotoName;
            m_OldPhotoName = sPhotoName;
            m_myFileName = U.FileNameWithExtension(sPhotoName, SQL.JpgFolder(), U.sJPGExtension);
            //            m_myFileName = SQL.GetPhotoFilename(row, U.JPGPhotoFolder, U.Photo_Table);
            //            m_Photo = GetPhotoBitmap(m_myFileName);
            DisplayNumPhotos(U.Photo_Table);
            if (!IsSlideShow)
            {
                buttonSlideShow.Visible = (m_sTableName == U.SlideShow_Table) ? true : SQL.GetSlideShow(m_iPhotoID).Rows.Count == 0;
            }
        }
        //****************************************************************************************************************************
        protected Bitmap GetPhotoBitmap(string sFileName)
        {
            try
            {
                FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read);//Read only
                Image Im = System.Drawing.Image.FromStream(fs);
                fs.Close();
                fs.Dispose();
                return (Bitmap)Im;
                //                return (Bitmap)Image.FromFile(sFileName,true);
                //              return new Bitmap(sFileName);
            }
            catch
            {
                return null;
            }
        }
        //****************************************************************************************************************************
        private void DisplayPhotodPersons(string sNumPeoplePictured)
        {
            if (sNumPeoplePictured.Length == 0)
            {
                m_iNumPeoplePictured = 0;
                NumPicturedPersons_textBox.Text = "";
            }
            else
            {
                m_iNumPeoplePictured = sNumPeoplePictured.ToInt();
                if (m_iNumPeoplePictured == 0)
                    NumPicturedPersons_textBox.Text = "";
                else
                    NumPicturedPersons_textBox.Text = sNumPeoplePictured;
            }
            DataTable tbl = m_Photo_ds.Tables[U.PicturedPerson_Table];
            int iCount = tbl.Rows.Count;
            if (iCount > 0)
            {
                DataRow row = m_Photo_ds.Tables[U.PicturedPerson_Table].Rows[iCount - 1];
                int iLastPicturedPersonNumber = row[U.PicturedPersonNumber_col].ToInt();
                if (iLastPicturedPersonNumber > m_iNumPeoplePictured)
                {
                    m_iNumPeoplePictured = iLastPicturedPersonNumber;
                    NumPicturedPersons_textBox.Text = m_iNumPeoplePictured.ToString();
                }
            }
            PicturedPersons_listBox.Items.Clear();
            for (int i = 1; i <= m_iNumPeoplePictured; ++i)
            {
                DataRow row = m_Photo_ds.Tables[U.PicturedPerson_Table].Rows.Find(i);
                string sPersonName;
                if (row == null)
                {
                    sPersonName = U.Unknown;
                }
                else
                {
                    int iPersonID = row[U.PersonID_col].ToInt();
                    sPersonName = SQL.GetPersonName(iPersonID);
                }
                PicturedPersons_listBox.Items.Add(sPersonName);
            }
        }
        //****************************************************************************************************************************
        private void DisplayPhotodBuildings(string sNumBuildingPictured)
        {
            if (sNumBuildingPictured.Length == 0)
            {
                m_iNumBuildingsPictured = 0;
                NumPicturedBuildings_textBox.Text = "";
            }
            else
            {
                m_iNumBuildingsPictured = sNumBuildingPictured.ToInt();
                if (m_iNumBuildingsPictured == 0)
                    NumPicturedBuildings_textBox.Text = "";
                else
                    NumPicturedBuildings_textBox.Text = sNumBuildingPictured;
            }
            DataTable tbl = m_Photo_ds.Tables[U.PicturedBuilding_Table];
            int iCount = tbl.Rows.Count;
            if (iCount > 0)
            {
                DataRow row = m_Photo_ds.Tables[U.PicturedBuilding_Table].Rows[iCount - 1];
                int iLastPicturedBuildingNumber = row[U.PicturedBuildingNumber_col].ToInt();
                if (iLastPicturedBuildingNumber > m_iNumBuildingsPictured)
                {
                    m_iNumBuildingsPictured = iLastPicturedBuildingNumber;
                    NumPicturedBuildings_textBox.Text = m_iNumBuildingsPictured.ToString();
                }
            }
            PicturedBuildings_ListBox.Items.Clear();
            if (m_iNumBuildingsPictured <= 0)
            {
                Then_button.BackColor = Color.LightSteelBlue;
                Now_button.BackColor = Color.LightSteelBlue;
            }
            Then_button.BackColor = Color.LightSteelBlue;
            Now_button.BackColor = Color.LightSteelBlue;
            for (int i = 1; i <= m_iNumBuildingsPictured; ++i)
            {
                DataRow row = m_Photo_ds.Tables[U.PicturedBuilding_Table].Rows.Find(i);
                string sBuildingName;
                if (row == null)
                {
                    sBuildingName = U.Unknown;
                }
                else
                {
                    sBuildingName = row[U.BuildingName_col].ToString();
                    SetThenNowButtons(row[U.BuildingID_col].ToInt());
                }
                PicturedBuildings_ListBox.Items.Add(sBuildingName);
            }
        }
        //****************************************************************************************************************************
        private void SetThenNowButtons(int buildingId)
        {
            if (!m_bshowThenAndNowButtons)
            {
                return;
            }
            DataTable buildingTbl = SQL.GetBuilding(buildingId);
            if (buildingTbl.Rows.Count == 0)
            {
                return;
            }
            DataRow buildingRow = buildingTbl.Rows[0];

            Then_button.Text = "Then";
            Now_button.Text = "Now";
            Then_button.BackColor = Color.LightSteelBlue;
            Now_button.BackColor = Color.LightSteelBlue;
            SetButton(buildingRow, U.Then1_col, Then_button);
            SetButton(buildingRow, U.Then2_col, Then_button);
            SetButton(buildingRow, U.Now1_col, Now_button);
            SetButton(buildingRow, U.Now2_col, Now_button);
        }
        //****************************************************************************************************************************
        private void SetButton(DataRow buildingRow, string thenNowCol, Button thenNowButton)
        {
            if (!String.IsNullOrEmpty(buildingRow[thenNowCol].ToString()))
            {
                bool displayedBuilding = buildingRow[U.BuildingID_col].ToInt() == m_buildingIdDisplay || m_buildingIdDisplay == 9999;
                if (buildingRow[thenNowCol].ToString() == PhotoName_textBox.Text && displayedBuilding)
                {
                    //SQL.GetPhotoName(PhotoName_textBox.Text);
                    thenNowButton.BackColor = Color.Maroon;
                    thenNowButton.Text = thenNowCol;
                }
            }
        }
        //****************************************************************************************************************************
        private string BuildNameFromPicturedPersonTable(DataRow row)
        {
            string s1 = row[U.FirstName_col].ToString();
            string s2 = row[U.LastName_col].ToString();
            return SQL.BuildNameString(row[U.FirstName_col].ToString(),
                                                               row[U.MiddleName_col].ToString(),
                                                               row[U.LastName_col].ToString(),
                                                               row[U.Suffix_col].ToString(),
                                                               row[U.Prefix_col].ToString(),
                                                               row[U.MarriedName_col].ToString(),
                                                               row[U.KnownAs_col].ToString());
        }
        //****************************************************************************************************************************
        protected void DisplayPhotograph(int iPhotoID)
        {
            PhotoTitle_label.Visible = iPhotoID != 0;
            SQL.GetPhoto(ref m_Photo_ds, iPhotoID);
            if (m_Photo_ds.Tables[U.Photo_Table].Rows.Count != 0)
            {
                DataRow row = m_Photo_ds.Tables[U.Photo_Table].Rows[0];
                LoadPhotoIntoControls(row);
                DisplayPhotodPersons(row[U.NumPicturedPersons_col].ToString());
                DisplayPhotodBuildings(row[U.NumPicturedBuildings_col].ToString());
                UU.LoadCategory_listBox(m_Photo_ds.Tables[U.PhotoCategoryValue_Table], Categories_listBox,
                                           U.PhotoCategoryValue_Table, U.Photo_Table, row[U.PhotoID_col].ToInt());
                if (!IsSlideShow)
                {
                    CopyCategory_button.Visible = (Categories_listBox.Items.Count == 0) ? false : true;
                    PasteCategory_button.Visible = (m_CopyCategoryValues == null || m_CopyCategoryValues.Count == 0) ? false : true;
                }
                m_CategoryListboxChanged = false;
            }
        }
        //****************************************************************************************************************************
        private void EnableCorrectButtonsBasedOnWhereInListWeAre(int iPhotoToDisplay)
        {
            if (m_iCountOfPhotos <= 1)
            {
                Previous_button.Enabled = false;
                Next_button.Enabled = false;
                PhotoList_button.Visible = false;
            }
            else
            {
                PhotoList_button.Visible = IsSlideShow ? false : true;
            }
        }
        //****************************************************************************************************************************
        protected void DisplayNumPhotos(string sTableName)
        {
            int iCurrentPhotoPlus1 = m_iCurrentPhoto + 1;
            string sNumPhotos = iCurrentPhotoPlus1.ToString() + " of " + m_iCountOfPhotos.ToString() + " Photos";
            NumPhotos_Label.Text = sNumPhotos;
        }
        //****************************************************************************************************************************
        protected void DisplayPhoto(int iPhotoToDisplay)
        {
            if (m_iCountOfPhotos > 0 && iPhotoToDisplay >= 0)
            {
                DataRow row = m_tbl.Rows[iPhotoToDisplay];
                DisplayPhotograph(row[U.PhotoID_col].ToInt());
                SetToUnmodified();
            }
            EnableCorrectButtonsBasedOnWhereInListWeAre(iPhotoToDisplay);
        }
        //****************************************************************************************************************************
        protected override void OnPaint(PaintEventArgs myArgs)
        {
            if (initialLoad)
            {
                initialLoad = false;
                return;
            }
            if (m_bPhotoIsNull && m_myFileName.Length > 0 && !m_bLockPhotoFromDisplay)
            {
                m_bPhotoIsNull = false;
                m_Photo = GetPhotoBitmap(m_myFileName);
                if (m_Photo == null)
                    m_bPhotoIsNull = true;
                else
                {
                    m_bPhotoIsNull = false;
                }
            }
            if (m_Photo != null)
            {
                int width = IsSlideShow ? 1344 : 800;
                int height = IsSlideShow ? 840 : 500;
                int location = IsSlideShow ? 0 : 24;
                PaintPhoto(myArgs, m_Photo, height, width, location);
            }
        }
        //****************************************************************************************************************************
        private void PeoplePictured_button_Click()
        {
            int iOldNumPeoplePictured = m_iNumPeoplePictured;
            m_iNumPeoplePictured = NumPicturedPersons_textBox.Text.ToInt();
            for (int i = iOldNumPeoplePictured; i > m_iNumPeoplePictured; --i)
                PicturedPersons_listBox.Items.RemoveAt(i - 1);
            for (int i = iOldNumPeoplePictured; i < m_iNumPeoplePictured; ++i)
            {
                DataRow row = null;
                if (m_Photo_ds.Tables[U.PicturedPerson_Table].Rows.Count != 0)
                    row = m_Photo_ds.Tables[U.PicturedPerson_Table].Rows.Find(i + 1);
                string sName;
                if (row == null)
                    sName = U.Unknown;
                else
                    sName = BuildNameFromPicturedPersonTable(row);
                PicturedPersons_listBox.Items.Add(sName);
            }
        }
        //****************************************************************************************************************************
        private void BuildingPictured_button_Click()
        {
            int iOldNumBuildingPictured = m_iNumBuildingsPictured;
            m_iNumBuildingsPictured = NumPicturedBuildings_textBox.Text.ToInt();
            for (int i = iOldNumBuildingPictured; i > m_iNumBuildingsPictured; --i)
                PicturedBuildings_ListBox.Items.RemoveAt(i - 1);
            for (int i = iOldNumBuildingPictured; i < m_iNumBuildingsPictured; ++i)
            {
                DataRow row = null;
                if (m_Photo_ds.Tables[U.PicturedBuilding_Table].Rows.Count != 0)
                    row = m_Photo_ds.Tables[U.PicturedBuilding_Table].Rows.Find(i + 1);
                string sName;
                if (row == null)
                    sName = U.Unknown;
                else
                    sName = ""; // BuildNameFromPicturedBuildingTable(row);
                PicturedBuildings_ListBox.Items.Add(sName);
            }
        }
        //****************************************************************************************************************************
        protected bool CheckForInvalidCharacters(string sPhotoName)
        {
            int iLength = sPhotoName.Length;
            sPhotoName = sPhotoName.Replace("*", "");
            sPhotoName = sPhotoName.Replace("/", "");
            sPhotoName = sPhotoName.Replace("\\", "");
            sPhotoName = sPhotoName.Replace(":", "");
            sPhotoName = sPhotoName.Replace("?", "");
            sPhotoName = sPhotoName.Replace("<", "");
            sPhotoName = sPhotoName.Replace(">", "");
            sPhotoName = sPhotoName.Replace("|", "");
            sPhotoName = sPhotoName.Replace(@"""", "");
            if (sPhotoName.Length != iLength)
            {
                MessageBox.Show(@"A Photo Name cannot contain any of the following characters: \ / : * ? "" < > | ");
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool RowIsDeleted()
        {
            DataRow Current_row = m_tbl.Rows[m_iCurrentPhoto];
            DataRowState rowstate = Current_row.RowState;
            return (rowstate == DataRowState.Deleted);
        }
        //****************************************************************************************************************************
        private void DisposeCurrentPhoto()
        {
            if (m_Photo != null)
            {
                m_Photo.Dispose();
                m_Photo = null;
            }
        }
        //****************************************************************************************************************************
        private void DisplayNewPhoto(int iPhotoID)
        {
            DisposeCurrentPhoto();
            DisplayPhoto(iPhotoID);
            m_bPhotoIsNull = true;
            this.Invalidate();
        }
        //****************************************************************************************************************************
        public virtual void Previous_Button_Click(object sender, System.EventArgs e)
        {
            if (CancelIfChanged())
                return;
            bool bDeleted = false;
            do
            {
                bDeleted = false;
                if (m_iCurrentPhoto == 0)
                    m_iCurrentPhoto = m_iCountOfPhotos;
                m_iCurrentPhoto--;
                if (RowIsDeleted())
                    bDeleted = true;
            }
            while (bDeleted);
            DisplayNewPhoto(m_iCurrentPhoto);
        }
        //****************************************************************************************************************************
        private void NextPicture()
        {
            if (CancelIfChanged())
                return;
            bool bDeleted = false;
            int iNumPhotos = 0;
            do
            {
                bDeleted = false;
                m_iCurrentPhoto++;
                iNumPhotos++;
                if (iNumPhotos > m_iCountOfPhotos) // All rows are deleted
                {
                    this.Close();
                    return;
                }
                if (m_iCurrentPhoto >= m_iCountOfPhotos)
                    m_iCurrentPhoto = 0;
                if (RowIsDeleted())
                    bDeleted = true;
            }
            while (bDeleted);
            DisplayNewPhoto(m_iCurrentPhoto);
        }
        //****************************************************************************************************************************
        public virtual void NextButton_Click(object sender, System.EventArgs e)
        {
            NextPicture();
        }
        //****************************************************************************************************************************
        public virtual void PhotoList_Click(object sender, System.EventArgs e)
        {
            if (CancelIfChanged())
                return;
            bool allowDragAndDrop = m_sTableName == U.SlideShow_Table;
            CGridPhoto GridPhoto = new CGridPhoto(ref m_SQL, ref m_tbl, U.Photo_Table, m_iCurrentPhoto, allowDragAndDrop);
            GridPhoto.ShowDialog();
            int iPhoto = GridPhoto.SelectedRow;
            if (allowDragAndDrop && GridPhoto.m_bListOrderChanged)
            {
                GridPhoto.ReorderSlideShow(m_tbl);
                SQL.UpdateSlideShow(m_tbl);
                m_tbl = SQL.GetSlideShow();
            }
            if (iPhoto > 0)
                m_iCurrentPhoto = iPhoto;
            if (m_iCurrentPhoto >= 0)
            {
                if (m_Photo != null)
                {
                    m_Photo.Dispose();
                    m_Photo = null;
                }
                DisplayNewPhoto(m_iCurrentPhoto);
            }
        }
        //****************************************************************************************************************************
        private string GetPersonName(int iPicturedPictureNumber,
                                     string OldPersonName)
        {
            int iOldPersonID = 0; ;
            DataTable PicturedPerson_tbl = m_Photo_ds.Tables[U.PicturedPerson_Table];
            DataRow row = PicturedPerson_tbl.Rows.Find(iPicturedPictureNumber);
            if (row != null)
                iOldPersonID = row[U.PersonID_col].ToInt();

            FPerson Person = new FPerson(m_SQL, iOldPersonID, true);
            Person.ShowDialog();
            int iPersonID = Person.GetPersonID();
            string sString = "";
            if (iPersonID == 0)
            {
                if (iOldPersonID == 0)
                    sString = U.Unknown;
                else
                    sString = OldPersonName;
            }
            else
            {
                if (row != null)
                    row[U.PersonID_col] = iPersonID;
                else
                {
                    row = PicturedPerson_tbl.NewRow();
                    row[U.PicturedPersonNumber_col] = iPicturedPictureNumber;
                    row[U.PhotoID_col] = m_iPhotoID;
                    row[U.PersonID_col] = iPersonID;
                    PicturedPerson_tbl.Rows.Add(row);
                }
                sString = SQL.GetPersonName(iPersonID);
            }
            return sString;
        }
        //****************************************************************************************************************************
        private string GetBuildingName(int iPicturedBuildingNumber,
                                       string OldBuildingName)
        {
            int iOldBuildingID = 0;
            DataRow row = GetBuildingIdFromName(iPicturedBuildingNumber);
            if (row != null)
                iOldBuildingID = row[U.BuildingID_col].ToInt();

            int iBuildingID = iOldBuildingID;
            CGridGroupValuesModernRoads GridGroupValuesModernRoads = new CGridGroupValuesModernRoads(m_SQL, iBuildingID, false, false, true);
            GridGroupValuesModernRoads.ShowDialog();
            iBuildingID = GridGroupValuesModernRoads.SelectedBuilding;
            string sString = "";
            if (iBuildingID <= 0)
                sString = OldBuildingName;
            else
            {
                if (row != null)
                    row[U.BuildingID_col] = iBuildingID;
                else
                {
                    DataTable PicturedBuilding_tbl = m_Photo_ds.Tables[U.PicturedBuilding_Table];
                    row = PicturedBuilding_tbl.NewRow();
                    row[U.PicturedBuildingNumber_col] = iPicturedBuildingNumber;
                    row[U.PhotoID_col] = m_iPhotoID;
                    row[U.BuildingID_col] = iBuildingID;
                    PicturedBuilding_tbl.Rows.Add(row);
                }
                sString = SQL.GetBuildingName(iBuildingID);
            }
            return sString;
        }
        //****************************************************************************************************************************
        private DataRow GetBuildingIdFromName(int iPicturedBuildingNumber)
        {
            DataTable PicturedBuilding_tbl = m_Photo_ds.Tables[U.PicturedBuilding_Table];
            return PicturedBuilding_tbl.Rows.Find(iPicturedBuildingNumber);
        }
        //****************************************************************************************************************************
        private void FullSize_Click(object sender, System.EventArgs e)
        {
            FPhotoFullSize PhotoFullSize = new FPhotoFullSize(m_Photo);
            PhotoFullSize.ShowDialog();
        }
        //****************************************************************************************************************************
        private void PeoplePictured_Enter(object sender, System.EventArgs e)
        {
            m_bGetNumPeople = true;
        }
        //****************************************************************************************************************************
        private void PeoplePictured_Leave(object sender, System.EventArgs e)
        {
            PeoplePictured_button_Click();
            m_bGetNumPeople = false;
        }
        //****************************************************************************************************************************
        private void NameModified_Click(object sender, System.EventArgs e)
        {
        }
        //****************************************************************************************************************************
        private void BuildingPictured_Enter(object sender, System.EventArgs e)
        {
            m_bGetNumBuilding = true;
        }
        //****************************************************************************************************************************
        private void BuildingPictured_Leave(object sender, System.EventArgs e)
        {
            BuildingPictured_button_Click();
            m_bGetNumBuilding = false;
        }
        //****************************************************************************************************************************
        protected void SaveAndDisplayPhoto()
        {
            if (SavePhotograph())
            {
                SetToUnmodified();
                buttonSlideShow.Visible = IsSlideShow ? false : true;
                ///////////            DisplayPhotograph(m_iPhotoID);
            }
        }
        //****************************************************************************************************************************
        public virtual void SaveButton_Click(object sender, System.EventArgs e)
        {
            SaveAndDisplayPhoto();
        }
        //****************************************************************************************************************************
        public virtual void ThenButton_Click(object sender, System.EventArgs e)
        {
            UpdateBuildingForThenNow(U.Then1_col, U.Then2_col);
        }
        //****************************************************************************************************************************
        public virtual void NowButton_Click(object sender, System.EventArgs e)
        {
            UpdateBuildingForThenNow(U.Now1_col, U.Now2_col);
        }
        //****************************************************************************************************************************
        private void UpdateBuildingForThenNow(string thenNowCol, string thenNowCol_2)
        {
            DataRow buildingRow = GetPicturedBuildingId();
            if (buildingRow == null)
            {
                return;
            }
            string photoName = PhotoName_textBox.Text.TrimString();
            string selectedCol = ThenNowSelection(buildingRow, ref photoName, thenNowCol, thenNowCol_2);
            if (selectedCol == null)
            {
                return;
            }
            SQL.UpdateBuildingValue(buildingRow[U.BuildingID_col].ToInt(), selectedCol, photoName);
            SetThenNowButtons(buildingRow[U.BuildingID_col].ToInt());
        }
        //****************************************************************************************************************************
        private string ThenNowSelection(DataRow buildingRow, ref string photoName, string thenNowCol, string thenNowCol_2)
        {
            string buildingThenNowPhotoName = buildingRow[thenNowCol].ToString();
            string buildingThenNowPhotoName_2 = buildingRow[thenNowCol_2].ToString();
            if (String.IsNullOrEmpty(buildingThenNowPhotoName))
            {
                return thenNowCol;
            }
            if (buildingThenNowPhotoName == photoName)
            {
                return RemainDefined(thenNowCol, ref photoName);
            }
            if (buildingThenNowPhotoName == photoName)
            {
                return RemainDefined(thenNowCol_2, ref photoName);
            }
            string message = "Already Defined Value for " + thenNowCol.Replace("1", "");
            message += (String.IsNullOrEmpty(buildingThenNowPhotoName_2)) ? "" : " and " + thenNowCol_2;
            message += "\n\nPress Yes for " + thenNowCol.Replace("1", "") + ", No for " + thenNowCol_2 + " or Cancel";
            switch (MessageBox.Show(message, "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    return thenNowCol;
                case DialogResult.No:
                    return thenNowCol_2;
                case DialogResult.Cancel:
                    return null;
                default: return "";
            }
        }
        //****************************************************************************************************************************
        private string RemainDefined(string thenNowCol, ref string photoName)
        {
            string message = thenNowCol + " Already Defined";
            message += "\n\nPress Yes to Remain Defined, No to Remove";
            switch (MessageBox.Show(message, "", MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                    return null;
                case DialogResult.No:
                    photoName = "";
                    return thenNowCol;
                default: return null;
            }
        }
        //****************************************************************************************************************************
        private DataRow GetPicturedBuildingId()
        {
            if (PicturedBuildings_ListBox.Items.Count == 0)
            {
                MessageBox.Show("Please Select a Building");
                return null;
            }
            string buildingName;
            int iRowIndex;
            if (PicturedBuildings_ListBox.Items.Count == 1)
            {
                buildingName = PicturedBuildings_ListBox.Items[0].ToString();
                iRowIndex = 0;
            }
            else
            {
                iRowIndex = PicturedBuildings_ListBox.SelectedIndex;
                if (iRowIndex < 0)
                {
                    MessageBox.Show("Please Select a Building");
                    return null;
                }
                buildingName = PicturedBuildings_ListBox.Items[iRowIndex].ToString();
            }
            if (buildingName == "Unknown")
            {
                MessageBox.Show("Building Name Unknown. Please Select a Building");
                return null;
            }
            DataRow row = GetBuildingIdFromName(iRowIndex + 1);
            if (row == null)
            {
                MessageBox.Show("Unable To Get Building From Table");
                return null;
            }
            int iBuildingID = row[U.BuildingID_col].ToInt();
            DataTable buildingTbl = SQL.GetBuilding(iBuildingID);
            if (buildingTbl.Rows.Count == 0)
            {
                MessageBox.Show("Unable To Get Building From Database");
                return null;
            }
            return buildingTbl.Rows[0];
        }
        //****************************************************************************************************************************
        public virtual void CopyCategory_Click(object sender, System.EventArgs e)
        {
            if (Categories_listBox.Items.Count == 0)
            {
                return;
            }
            if (m_CopyCategoryValues == null)
            {
                m_CopyCategoryValues = new ArrayList();
            }
            else
            {
                m_CopyCategoryValues.Clear();
            }
            foreach (var item in Categories_listBox.Items)
            {
                m_CopyCategoryValues.Add(item);
            }
            PasteCategory_button.Visible = false;
        }
        //****************************************************************************************************************************
        public virtual void SlideShow_Click(object sender, System.EventArgs e)
        {
            if (m_sTableName == U.SlideShow_Table)
            {
                SQL.DeleteFromSlideShow(m_iPhotoID);
                m_tbl = SQL.GetSlideShow();
                m_iCountOfPhotos--;
                NextPicture();
            }
            else if (m_iPhotoID != 0)
            {
                SQL.AddPhotoToSlideShow(m_iPhotoID);
                buttonSlideShow.Visible = false;
            }
        }
        //****************************************************************************************************************************
        public virtual void KeyDown_Click(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int photoNum = GetPhoto_textBox.Text.ToInt();
                if (photoNum != 0)
                {
                    string photoName = "HF" + photoNum.ToString("000000");
                    int photoId = SQL.GetPhotoID(photoName);
                    m_iCurrentPhoto = FindPhoto(photoId);
                    DisplayNewPhoto(m_iCurrentPhoto);
                }
            }
        }
        private int FindPhoto(int photoID)
        {
            int index = 0;
            foreach (DataRow photoRow in m_tbl.Rows)
            {
                if (photoRow.RowState != DataRowState.Deleted && photoID == photoRow[U.PhotoID_col].ToInt())
                {
                    return index;
                }
                index++;
            }
            return 0;
        }
        //****************************************************************************************************************************
        public virtual void PasteCategory_Click(object sender, System.EventArgs e)
        {
            if (m_CopyCategoryValues == null || m_CopyCategoryValues.Count == 0)
            {
                return;
            }
            DataTable Group_tbl = m_Photo_ds.Tables[U.PhotoCategoryValue_Table];
            int iGroupID = 0;
            int groupLocation = 0;
            foreach (string value in m_CopyCategoryValues)
            {
                if (AlreadyInList(value, ref groupLocation))
                {
                    if (value[0] != ' ')
                    {
                        iGroupID = SQL.GetCategoryIDFromName(value);
                    }
                }
                else
                {
                    if (groupLocation != 0)
                    {
                        Categories_listBox.Items.Insert(groupLocation, value);
                    }
                    else
                    {
                        Categories_listBox.Items.Add(value);
                    }

                    if (value[0] != ' ')
                    {
                        iGroupID = SQL.GetCategoryIDFromName(value);
                        groupLocation = 0;
                    }
                    else
                    {
                        Group_tbl.Rows.Add(SQL.GetCategoryValueID(iGroupID.ToString(), value.TrimString()), iGroupID, value.TrimString());
                        m_CategoryListboxChanged = true;
                    }
                }
            }
            PasteCategory_button.Visible = false;
        }
        //****************************************************************************************************************************
        private bool AlreadyInList(string value, ref int groupLocation)
        {
            int index = 0;
            foreach (string item in Categories_listBox.Items)
            {
                if (item == value)
                {
                    if (value[0] != ' ' && !LastGroupInList(index))
                    {
                        groupLocation = index + 1;
                    }
                    return true;
                }
                index++;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool LastGroupInList(int checkIndex)
        {
            int index = Categories_listBox.Items.Count - 1;
            while (index > checkIndex)
            {
                if (Categories_listBox.Items[index].ToString()[0] != ' ')
                {
                    return false;
                }
                index--;
            }
            return true;
        }
        //****************************************************************************************************************************
        public virtual void DeleteButton_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Delete Photo?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow row = m_tbl.Rows[m_iCurrentPhoto];
                int iPhotoID = row[U.PhotoID_col].ToInt();
                if (SQL.DeletePhoto(iPhotoID))
                    m_tbl.Rows[m_iCurrentPhoto].Delete();
                NextPicture();
            }
        }
        //****************************************************************************************************************************
        protected void SetToUnmodified()
        {
            PhotoName_textBox.Modified = false;
            PhotoYear_TextBox.Modified = false;
            PhotoDrawer_textBox.Modified = false;
            PhotoFolder_textBox.Modified = false;
            PhotoNotes_TextBox.Modified = false;
            NumPicturedPersons_textBox.Modified = false;
            NumPicturedBuildings_textBox.Modified = false;
            m_CategoryListboxChanged = false;
        }
        //****************************************************************************************************************************
        private bool SavePhotograph()
        {
            string sPhotoID = PhotoName_textBox.Text.TrimString();
            if (m_iPhotoID == 0 && sPhotoID == SQL.GetPhotoName(sPhotoID))
            {
                MessageBox.Show("Photo Name Already Exists");
                return false;
            }
            if (sPhotoID.Length == 0)
            {
                MessageBox.Show("PhotoName cannot be empty");
                return false;
            }
            if (PhotoSource_comboBox.Text.TrimString().Length == 0)
            {
                if (MessageBox.Show("Source Required. Do you wish to set Photo Source To HF Collection?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    PhotoSource_comboBox.Text = "HF Collection";
                else
                    return false;
            }
            if (PhotoYear_TextBox.Text.ToIntNoError() == U.Exception)
            {
                MessageBox.Show("Approximate Year must be a numeric value");
                return false;
            }
            int iPhotoNum = U.GetPhotoNumber(sPhotoID,2);
//            if (iPhotoNum <= 0 || iPhotoNum == U.Exception || sPhotoID.Length != 8)
//            {
//                MessageBox.Show("PhotoName Must be 'HFxxxxxx' where xxxxxx is numeric");
//                return false;
//            }
            if (!CheckForInvalidCharacters(sPhotoID))
            {
                return false;  // message printed witin function
            }
            string sPhotoName = PhotoName_textBox.Text.TrimString();
            if (sPhotoName.Substring(0, 2) != "HF")
            {
                sPhotoName = SQL.GetNextPhotoName();
            }
            string sOldFileName = m_myFileName;
            if (m_Photo == null)
                sOldFileName = "";
            m_bLockPhotoFromDisplay = true;
            if (!m_bPhotoIsNull)
            {
                m_Photo.Dispose();
                m_bPhotoIsNull = true;
            }
            m_Photo = null;
            string sNewFileName = U.FileNameWithExtension(sPhotoName, SQL.JpgFolder(), U.sJPGExtension);
            int iPhotoID = SavePhoto(sNewFileName, sOldFileName, sPhotoName);
            if (iPhotoID != 0)
            {
                m_iPhotoID = iPhotoID;
                PhotoName_textBox.Text = sPhotoName;
                m_bLockPhotoFromDisplay = false;
//                m_Photo = GetPhotoBitmap(sNewFileName);
                m_myFileName = sNewFileName;
                m_OldPhotoName = sPhotoName;
                if (m_Photo == null)
                    m_bPhotoIsNull = true;
                this.Refresh();
                m_sPhotoSource = PhotoSource_comboBox.Text.ToString();
                UU.LoadSourceComboBox(PhotoSource_comboBox);
                PhotoSource_comboBox.Text = m_sPhotoSource;
                return true;
            }
            else
            {
                MessageBox.Show("Save Unsuccessful");
                m_bLockPhotoFromDisplay = false;
//                m_Photo = GetPhotoBitmap(m_myFileName);
                return false;
            }
        }
        //****************************************************************************************************************************
        private int SavePhoto(string sNewFileName,
                              string sOldFileName,
                              string sNewPhotoName)
        {
            try
            {
                if (m_iPhotoID == 0)
                {
                    AddPhotoToDataRow(sNewPhotoName);
                    if (SQL.CreateNewPhoto(m_Photo_ds))
                    {
                        m_iPhotoID = SQL.PhotoIDFromDataSet(m_Photo_ds);
                        if (m_bshowThenAndNowButtons)
                        {
                            m_buildingIdDisplay = 9999;
                        }
                    }
                    else
                        return 0;
                }
                else
                {
                    ArrayList FieldsModified = UpdatePhotoInDataTable(m_Photo_ds.Tables[U.Photo_Table]);
                    SQL.UpdatePhoto(FieldsModified, m_Photo_ds, m_iPhotoID);
                }
                AdjustFileNames(sNewFileName, sOldFileName, m_OldPhotoName, sNewPhotoName);
            }
            catch (HistoricJamaicaException e)
            {
                HistoricJamaicaException ex = new HistoricJamaicaException(e.errorString);
                UU.ShowErrorMessage(ex);
                return 0;
            }
            catch (Exception e)
            {
                HistoricJamaicaException ex = new HistoricJamaicaException(e.Message);
                UU.ShowErrorMessage(ex);
                return 0;
            }
            return m_iPhotoID;
        }
        //****************************************************************************************************************************
        private ArrayList UpdatePhotoInDataTable(DataTable photo_tbl)
        {
            ArrayList FieldsModified = new ArrayList();
            DataColumnCollection columns = photo_tbl.Columns;
            DataRow photo_row = photo_tbl.Rows[0];
            U.SetToNewValueIfDifferent(FieldsModified, columns, photo_row, U.PhotoName_col, PhotoName_textBox.Text.ToString());
            U.SetToNewValueIfDifferent(FieldsModified, columns, photo_row, U.PhotoExtension_col, U.sJPGExtension);
            U.SetToNewValueIfDifferent(FieldsModified, columns, photo_row, U.PhotoNotes_col, PhotoNotes_TextBox.Text.ToString());
            U.SetToNewValueIfDifferent(FieldsModified, photo_row, U.PhotoYear_col, PhotoYear_TextBox.Text.ToInt());
            U.SetToNewValueIfDifferent(FieldsModified, columns, photo_row, U.PhotoSource_col, PhotoSource_comboBox.Text.ToString());
            U.SetToNewValueIfDifferent(FieldsModified, photo_row, U.PhotoDrawer_col, PhotoDrawer_textBox.Text.ToInt());
            U.SetToNewValueIfDifferent(FieldsModified, photo_row, U.PhotoFolder_col, PhotoFolder_textBox.Text.ToInt());
            U.SetToNewValueIfDifferent(FieldsModified, photo_row, U.NumPicturedPersons_col, m_iNumPeoplePictured);
            U.SetToNewValueIfDifferent(FieldsModified, photo_row, U.NumPicturedBuildings_col, m_iNumBuildingsPictured);
            return FieldsModified;
        }
        //****************************************************************************************************************************
        private void AdjustFileNames(string sFileName,
                                     string sOldFileName,
                                     string sOldPhotoName,
                                     string sNewPhotoName)
        {
            bool bSuccess;
            ArrayList PhotosToDelete = new ArrayList();
            ArrayList PhotosCopied = new ArrayList();
            if (sOldFileName.ToLower() == sFileName.ToLower())
                bSuccess = true;
            else if (sOldFileName.Length == 0 || sOldFileName == sFileName)
                bSuccess = true;
            else if (U.CopyOrRenamePhotoFile(PhotosToDelete, PhotosCopied, sOldFileName, sFileName))
            {
                if (sOldFileName.Length != 0)
                {
                    int iEndOfDirectory = sOldFileName.LastIndexOf('\\');
                    string sDirectory = sOldFileName.Remove(iEndOfDirectory + 1);
                    string sOldDirectory = SQL.TifFolder();
                    if (sDirectory != SQL.JpgFolder())
                        sOldDirectory = sDirectory;
                    string sOldTIF = U.FileNameWithExtension(sOldPhotoName, sOldDirectory, U.sTIFExtension);
                    string sNewTIF = U.FileNameWithExtension(sNewPhotoName, SQL.TifFolder(), U.sTIFExtension);
                    bSuccess = U.CopyOrRenamePhotoFile(PhotosToDelete, PhotosCopied, sOldTIF, sNewTIF);
                }
            }
        }
        //****************************************************************************************************************************
        private void AddPhotoToDataRow(string sNewPhotoName)
        {
            string sNewFileName = U.FileNameWithExtension(PhotoName_textBox.Text.TrimString(), SQL.JpgFolder(), U.sJPGExtension);
            DataRow photo_row = m_Photo_ds.Tables[U.Photo_Table].NewRow();
            photo_row[U.PhotoID_col] = 0;
            photo_row[U.PhotoName_col] = sNewPhotoName;
            photo_row[U.PhotoExtension_col] = U.sJPGExtension;
            photo_row[U.PhotoNotes_col] = PhotoNotes_TextBox.Text.ToString();
            photo_row[U.PhotoYear_col] = PhotoYear_TextBox.Text.ToInt();
            photo_row[U.PhotoSource_col] = PhotoSource_comboBox.Text.ToString();
            photo_row[U.PhotoDrawer_col] = PhotoDrawer_textBox.Text.ToInt();
            photo_row[U.PhotoFolder_col] = PhotoFolder_textBox.Text.ToString();
            photo_row[U.NumPicturedPersons_col] = m_iNumPeoplePictured;
            photo_row[U.NumPicturedBuildings_col] = m_iNumBuildingsPictured;
            m_Photo_ds.Tables[U.Photo_Table].Rows.Add(photo_row);
        }
        //****************************************************************************************************************************
        private bool PhotoChanged()
        {
            if (PhotoName_textBox.Modified || 
                PhotoYear_TextBox.Modified ||
                U.Modified(PhotoSource_comboBox.Text.TrimString(), m_Photo_ds.Tables[U.Photo_Table], "", U.PhotoSource_col) ||
                PhotoDrawer_textBox.Modified || 
                PhotoFolder_textBox.Modified ||
                PhotoNotes_TextBox.Modified ||
                NumPicturedPersons_textBox.Modified ||
                m_CategoryListboxChanged ||
                NumPicturedBuildings_textBox.Modified)
                return true;
            return false;
        }
        //****************************************************************************************************************************
        protected bool CancelIfChanged()
        {
            if (!PhotoChanged())
                return false;
            switch (MessageBox.Show("Save Changes?", "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    if (SavePhotograph())
                        return false;
                    else
                        return true;
                case DialogResult.No:
                    return false;
                default:
                case DialogResult.Cancel:
                    return true;
            }
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = CancelIfChanged();
        }
        //****************************************************************************************************************************
        private int CurrentLocation(int iPhotoID)
        {
            int iCurrentLocation = 0;
            foreach (DataRow row in m_tbl.Rows)
            {
                if (row[U.PhotoID_col].ToInt() == iPhotoID)
                {
                    return iCurrentLocation;
                }
                iCurrentLocation++;
            }
            return U.Exception;
        }
        //****************************************************************************************************************************
        public virtual void NewPhotoButton_Click(object sender, System.EventArgs e)
        {
        }
        //****************************************************************************************************************************
        protected void CategorylistBox_Click(object sender, EventArgs e)
        {
            CListboxGroup GroupListbox = new CListboxGroup(m_SQL, Categories_listBox, U.PhotoCategoryValue_Table, U.CategoryValueValue_col);
            DataTable photo_tbl = m_Photo_ds.Tables[U.PhotoCategoryValue_Table];
            m_CategoryListboxChanged = GroupListbox.ShowGroupListbox(ref photo_tbl);
            CopyCategory_button.Visible = IsSlideShow ? false : true;
        }
        //****************************************************************************************************************************
        protected void PicturedPersonlistBox_Click(object sender, EventArgs e)
        {
            int iRowIndex = PicturedPersons_listBox.SelectedIndex;
            if (iRowIndex >= 0)
            {
                string sPersonName = PicturedPersons_listBox.Items[iRowIndex].ToString();
                PicturedPersons_listBox.Items[iRowIndex] = GetPersonName(iRowIndex + 1, sPersonName);
                if (sPersonName != PicturedPersons_listBox.Items[iRowIndex].ToString())
                {
                    NumPicturedPersons_textBox.Modified = true;
                }
            }
        }
        //****************************************************************************************************************************
        protected void PicturedBuildinglistBox_Click(object sender, EventArgs e)
        {
            int iRowIndex = PicturedBuildings_ListBox.SelectedIndex;
            if (iRowIndex >= 0)
            {
                string sBuildingName = PicturedBuildings_ListBox.Items[iRowIndex].ToString();
                PicturedBuildings_ListBox.Items[iRowIndex] = GetBuildingName(iRowIndex + 1, sBuildingName);
                if (sBuildingName != PicturedBuildings_ListBox.Items[iRowIndex].ToString())
                {
                    NumPicturedBuildings_textBox.Modified = true;
                }
            }
        }
        //****************************************************************************************************************************
    }
}