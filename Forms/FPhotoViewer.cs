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
    public class FPhotoViewer : FPaintPhoto
    {
        protected CSql m_SQL;
        protected string m_sDefaultSource = "Historic Foundation";
        protected string m_sTableName = "";
        protected int m_iNumPeoplePictured = 0;
        protected int m_iNumBuildingsPictured = 0;
        protected bool m_bPhotoAlreadyDrawn = false;
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
        private Button FullSize_button;
        protected ListBoxWithDoubleClick Categories_listBox;
        protected Label label6;
        private GroupBox Num_groupBox;
        private bool m_bGetNumPeople;
        private bool m_bGetNumBuilding;
        protected Button Previous_button;
        private System.ComponentModel.IContainer components = null;
        protected int m_iPhotoID = 0;
        protected DataSet m_Photo_ds = new DataSet();
        protected ListBoxWithDoubleClick PicturedPersons_listBox;
        protected ListBoxWithDoubleClick PicturedBuildings_ListBox;
        private GroupBox groupBox1;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label12;
        protected TextBox PhotoFolder_textBox;
        protected TextBox PhotoName_textBox;
        private GroupBox groupBox2;
        protected TextBox PhotoDrawer_textBox;
        private Label label5;
        protected RadioButton NextPhotoID_radioButton;
        protected Label PhotoTitle_label;
        private ContextMenuStrip ToolStrip = new ContextMenuStrip();
        protected ComboBox PhotoSource_comboBox;
        protected Button Delete_button;

        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql SQL)
        {
            m_SQL = SQL;
            InitializePhoto();
            DisplayPhotoInViewer(0);
        }
        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql SQL,
                              string sTableName)
        {
            m_SQL = SQL;
            m_sTableName = sTableName;
            InitializePhoto();
            DisplayPhotoInViewer(0);
        }
        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql SQL,
                              int     iPersonID,
                              string  sTableName)
        {
            m_SQL = SQL;
            m_sTableName = sTableName;
            InitializePhoto();
            DisplayPhotoInViewer(iPersonID);
        }
        //****************************************************************************************************************************
        public FPhotoViewer(ref CSql SQL,
                              DataTable tbl,
                              string   sTableName)
        // DataSet already loaded with the photos to view
        {
            m_SQL = SQL;
            m_tbl = tbl;
            m_tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PhotoID_col] };
            m_sTableName = sTableName;
            InitializePhoto();
            DisplayPhotoInViewer(0);
        }
        //****************************************************************************************************************************
        private void InitializePhoto()
        {
            int iMaxScreenHeight = Screen.PrimaryScreen.Bounds.Height - 50;
            if (Screen.PrimaryScreen.Bounds.Height <= 800)
                this.WindowState = FormWindowState.Maximized;

            InitializeComponent();
            NextPhotoID_radioButton.Visible = false;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick_click);

            this.Size = new System.Drawing.Size(1100, 760);
            PhotoNotes_TextBox.MaxLength = U.iMaxDescriptionLength;
            PhotoSource_comboBox.MaxLength = U.iMaxFullNameLength;
            PhotoName_textBox.MaxLength = U.iMaxNameLength;
            PhotoYear_TextBox.MaxLength = 4;
            PhotoDrawer_textBox.MaxLength = 1;
            PhotoFolder_textBox.MaxLength = 2;
            NumPicturedPersons_textBox.MaxLength = 2;
            NumPicturedBuildings_textBox.MaxLength = 2;
            UU.LoadSourceComboBox(m_SQL,PhotoSource_comboBox);
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
                return;
            //            PicturedPersons_listBox.Items = 
            //            foreach (ListBoxWithDoubleClick.Items itm in PicturedPersons_listBox.Items)
            //            {
            //                column.ContextMenuStrip = ToolStrip;
            //            }
            ContextMenuStrip strip = new ContextMenuStrip();
            ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
            toolStripItem1.Text = "Remove";
            strip.Items.Add(toolStripItem1);
            ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
            toolStripItem2.Text = "Move Up";
            strip.Items.Add(toolStripItem2);
            LBox.ContextMenuStrip = strip;
            ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
            toolStripItem3.Text = "Move Down";
            strip.Items.Add(toolStripItem3);
            LBox.ContextMenuStrip = strip;
            if (bPerson)
            {
                toolStripItem1.Click += new EventHandler(DeletePerson_Click);
                toolStripItem2.Click += new EventHandler(MovePersonUp_Click);
                toolStripItem3.Click += new EventHandler(MovePersonDown_Click);
            }
            else
            {
                toolStripItem1.Click += new EventHandler(DeleteBuilding_Click);
                toolStripItem2.Click += new EventHandler(MoveBuildingUp_Click);
                toolStripItem3.Click += new EventHandler(MoveBuildingDown_Click);
            }
        }
        //****************************************************************************************************************************
        private void SetTextboxModified(TextBox TBox,
                                        ref int iNumItems,
                                        int     iNumToAdjust)
        {
            iNumItems = TBox.Text.ToInt() + iNumToAdjust;
            TBox.Text = iNumItems.ToString();
            TBox.Modified = true; // be sure setting modified is after the actual set above
        }
        //****************************************************************************************************************************
        private void DeletePerson_Click(object sender, EventArgs e)
        {
            if (DeleteItem_Click(PicturedPersons_listBox, U.PicturedPerson_Table))
                SetTextboxModified(NumPicturedPersons_textBox, ref m_iNumPeoplePictured, - 1);
        }
        //****************************************************************************************************************************
        private void DeleteBuilding_Click(object sender, EventArgs e)
        {
            if (DeleteItem_Click(PicturedBuildings_ListBox, U.PicturedBuilding_Table))
                SetTextboxModified(NumPicturedBuildings_textBox, ref m_iNumBuildingsPictured, - 1);
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
            LBox.Items.Insert(iRowIndex,U.Unknown);
            int iPhotoNumber = iRowIndex + 1;
            int iNumPhotos = m_Photo_ds.Tables[sTable].Rows.Count;
            for (int i = iNumPhotos-1; i >= 0; i--)
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.PhotoNotes_TextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LeftToRight = new System.Windows.Forms.Label();
            this.Save_button = new System.Windows.Forms.Button();
            this.Previous_button = new System.Windows.Forms.Button();
            this.Next_button = new System.Windows.Forms.Button();
            this.PhotoYear_TextBox = new System.Windows.Forms.TextBox();
            this.NumPicturedPersons_textBox = new System.Windows.Forms.TextBox();
            this.NumPhotos_Label = new System.Windows.Forms.Label();
            this.PhotoList_button = new System.Windows.Forms.Button();
            this.FullSize_button = new System.Windows.Forms.Button();
            this.Categories_listBox = new Utilities.ListBoxWithDoubleClick();
            this.label6 = new System.Windows.Forms.Label();
            this.Num_groupBox = new System.Windows.Forms.GroupBox();
            this.PicturedPersons_listBox = new Utilities.ListBoxWithDoubleClick();
            this.PicturedBuildings_ListBox = new Utilities.ListBoxWithDoubleClick();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NumPicturedBuildings_textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.PhotoFolder_textBox = new System.Windows.Forms.TextBox();
            this.PhotoName_textBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PhotoDrawer_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.NextPhotoID_radioButton = new System.Windows.Forms.RadioButton();
            this.PhotoTitle_label = new System.Windows.Forms.Label();
            this.PhotoSource_comboBox = new System.Windows.Forms.ComboBox();
            this.Delete_button = new System.Windows.Forms.Button();
            this.Num_groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhotoNotes_TextBox
            // 
            this.PhotoNotes_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoNotes_TextBox.Location = new System.Drawing.Point(351, 558);
            this.PhotoNotes_TextBox.Name = "PhotoNotes_TextBox";
            this.PhotoNotes_TextBox.Size = new System.Drawing.Size(225, 147);
            this.PhotoNotes_TextBox.TabIndex = 2;
            this.PhotoNotes_TextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 561);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Photo ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Number People Pictured";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 561);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Approximate Year";
            // 
            // LeftToRight
            // 
            this.LeftToRight.AutoSize = true;
            this.LeftToRight.BackColor = System.Drawing.Color.LightSteelBlue;
            this.LeftToRight.Location = new System.Drawing.Point(6, 38);
            this.LeftToRight.Name = "LeftToRight";
            this.LeftToRight.Size = new System.Drawing.Size(135, 13);
            this.LeftToRight.TabIndex = 43;
            this.LeftToRight.Text = "Left to Rignt, Front to Back";
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(171, 711);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(78, 23);
            this.Save_button.TabIndex = 6;
            this.Save_button.TabStop = false;
            this.Save_button.Text = "Save";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Previous_button
            // 
            this.Previous_button.Location = new System.Drawing.Point(343, 711);
            this.Previous_button.Name = "Previous_button";
            this.Previous_button.Size = new System.Drawing.Size(78, 23);
            this.Previous_button.TabIndex = 54;
            this.Previous_button.TabStop = false;
            this.Previous_button.Text = "Previous";
            this.Previous_button.UseVisualStyleBackColor = true;
            this.Previous_button.Click += new System.EventHandler(this.Previous_Button_Click);
            // 
            // Next_button
            // 
            this.Next_button.Location = new System.Drawing.Point(430, 711);
            this.Next_button.Name = "Next_button";
            this.Next_button.Size = new System.Drawing.Size(78, 23);
            this.Next_button.TabIndex = 7;
            this.Next_button.TabStop = false;
            this.Next_button.Text = "Next";
            this.Next_button.UseVisualStyleBackColor = true;
            this.Next_button.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PhotoYear_TextBox
            // 
            this.PhotoYear_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoYear_TextBox.Location = new System.Drawing.Point(292, 558);
            this.PhotoYear_TextBox.Name = "PhotoYear_TextBox";
            this.PhotoYear_TextBox.Size = new System.Drawing.Size(40, 20);
            this.PhotoYear_TextBox.TabIndex = 1;
            // 
            // NumPicturedPersons_textBox
            // 
            this.NumPicturedPersons_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.NumPicturedPersons_textBox.Location = new System.Drawing.Point(160, 24);
            this.NumPicturedPersons_textBox.Name = "NumPicturedPersons_textBox";
            this.NumPicturedPersons_textBox.Size = new System.Drawing.Size(30, 20);
            this.NumPicturedPersons_textBox.TabIndex = 4;
            this.NumPicturedPersons_textBox.Leave += new System.EventHandler(this.PeoplePictured_Leave);
            this.NumPicturedPersons_textBox.Enter += new System.EventHandler(this.PeoplePictured_Enter);
            // 
            // NumPhotos_Label
            // 
            this.NumPhotos_Label.AutoSize = true;
            this.NumPhotos_Label.Location = new System.Drawing.Point(44, 716);
            this.NumPhotos_Label.Name = "NumPhotos_Label";
            this.NumPhotos_Label.Size = new System.Drawing.Size(62, 13);
            this.NumPhotos_Label.TabIndex = 6;
            this.NumPhotos_Label.Text = "NumPhotos";
            // 
            // PhotoList_button
            // 
            this.PhotoList_button.Location = new System.Drawing.Point(259, 711);
            this.PhotoList_button.Name = "PhotoList_button";
            this.PhotoList_button.Size = new System.Drawing.Size(78, 23);
            this.PhotoList_button.TabIndex = 5;
            this.PhotoList_button.TabStop = false;
            this.PhotoList_button.Text = "Picture List";
            this.PhotoList_button.UseVisualStyleBackColor = true;
            this.PhotoList_button.Click += new System.EventHandler(this.PhotoList_Click);
            // 
            // FullSize_button
            // 
            this.FullSize_button.Location = new System.Drawing.Point(683, 711);
            this.FullSize_button.Name = "FullSize_button";
            this.FullSize_button.Size = new System.Drawing.Size(78, 23);
            this.FullSize_button.TabIndex = 50;
            this.FullSize_button.TabStop = false;
            this.FullSize_button.Text = "Full Size";
            this.FullSize_button.UseVisualStyleBackColor = true;
            this.FullSize_button.Click += new System.EventHandler(this.FullSize_Click);
            // 
            // Categories_listBox
            // 
            this.Categories_listBox.BackColor = System.Drawing.SystemColors.Window;
            this.Categories_listBox.FormattingEnabled = true;
            this.Categories_listBox.Location = new System.Drawing.Point(603, 558);
            this.Categories_listBox.Name = "Categories_listBox";
            this.Categories_listBox.Size = new System.Drawing.Size(232, 147);
            this.Categories_listBox.TabIndex = 3;
            this.Categories_listBox.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(898, 486);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 52;
            this.label6.Text = "Category Values";
            // 
            // Num_groupBox
            // 
            this.Num_groupBox.Controls.Add(this.label2);
            this.Num_groupBox.Controls.Add(this.LeftToRight);
            this.Num_groupBox.Controls.Add(this.NumPicturedPersons_textBox);
            this.Num_groupBox.Location = new System.Drawing.Point(854, 31);
            this.Num_groupBox.Name = "Num_groupBox";
            this.Num_groupBox.Size = new System.Drawing.Size(200, 61);
            this.Num_groupBox.TabIndex = 53;
            this.Num_groupBox.TabStop = false;
            // 
            // PicturedPersons_listBox
            // 
            this.PicturedPersons_listBox.BackColor = System.Drawing.SystemColors.Window;
            this.PicturedPersons_listBox.FormattingEnabled = true;
            this.PicturedPersons_listBox.Location = new System.Drawing.Point(854, 98);
            this.PicturedPersons_listBox.Name = "PicturedPersons_listBox";
            this.PicturedPersons_listBox.Size = new System.Drawing.Size(200, 251);
            this.PicturedPersons_listBox.TabIndex = 56;
            this.PicturedPersons_listBox.TabStop = false;
            // 
            // PicturedBuildings_ListBox
            // 
            this.PicturedBuildings_ListBox.BackColor = System.Drawing.SystemColors.Window;
            this.PicturedBuildings_ListBox.FormattingEnabled = true;
            this.PicturedBuildings_ListBox.Location = new System.Drawing.Point(854, 454);
            this.PicturedBuildings_ListBox.Name = "PicturedBuildings_ListBox";
            this.PicturedBuildings_ListBox.Size = new System.Drawing.Size(200, 251);
            this.PicturedBuildings_ListBox.TabIndex = 57;
            this.PicturedBuildings_ListBox.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.NumPicturedBuildings_textBox);
            this.groupBox1.Location = new System.Drawing.Point(854, 387);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 61);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 13);
            this.label7.TabIndex = 48;
            this.label7.Text = "Number Buildings Pictured";
            // 
            // NumPicturedBuildings_textBox
            // 
            this.NumPicturedBuildings_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.NumPicturedBuildings_textBox.Location = new System.Drawing.Point(160, 24);
            this.NumPicturedBuildings_textBox.Name = "NumPicturedBuildings_textBox";
            this.NumPicturedBuildings_textBox.Size = new System.Drawing.Size(30, 20);
            this.NumPicturedBuildings_textBox.TabIndex = 5;
            this.NumPicturedBuildings_textBox.Leave += new System.EventHandler(this.BuildingPictured_Leave);
            this.NumPicturedBuildings_textBox.Enter += new System.EventHandler(this.BuildingPictured_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(600, 535);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 58;
            this.label8.Text = "Categories";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 59;
            this.label9.Text = "Drawer";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(130, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 60;
            this.label10.Text = "Folder";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(44, 604);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Source";
            // 
            // PhotoFolder_textBox
            // 
            this.PhotoFolder_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoFolder_textBox.Location = new System.Drawing.Point(194, 28);
            this.PhotoFolder_textBox.Name = "PhotoFolder_textBox";
            this.PhotoFolder_textBox.Size = new System.Drawing.Size(28, 20);
            this.PhotoFolder_textBox.TabIndex = 64;
            this.PhotoFolder_textBox.Text = "1";
            // 
            // PhotoName_textBox
            // 
            this.PhotoName_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoName_textBox.Location = new System.Drawing.Point(99, 558);
            this.PhotoName_textBox.Name = "PhotoName_textBox";
            this.PhotoName_textBox.Size = new System.Drawing.Size(75, 20);
            this.PhotoName_textBox.TabIndex = 65;
            this.PhotoName_textBox.TabStop = false;
            this.PhotoName_textBox.Text = "HF";
            this.PhotoName_textBox.ModifiedChanged += new System.EventHandler(this.NameModified_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.PhotoDrawer_textBox);
            this.groupBox2.Controls.Add(this.PhotoFolder_textBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(38, 642);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 63);
            this.groupBox2.TabIndex = 67;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Storage Location";
            // 
            // PhotoDrawer_textBox
            // 
            this.PhotoDrawer_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoDrawer_textBox.Location = new System.Drawing.Point(72, 28);
            this.PhotoDrawer_textBox.Name = "PhotoDrawer_textBox";
            this.PhotoDrawer_textBox.Size = new System.Drawing.Size(28, 20);
            this.PhotoDrawer_textBox.TabIndex = 63;
            this.PhotoDrawer_textBox.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(348, 535);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 68;
            this.label5.Text = "Notes";
            // 
            // NextPhotoID_radioButton
            // 
            this.NextPhotoID_radioButton.AutoSize = true;
            this.NextPhotoID_radioButton.Location = new System.Drawing.Point(47, 535);
            this.NextPhotoID_radioButton.Name = "NextPhotoID_radioButton";
            this.NextPhotoID_radioButton.Size = new System.Drawing.Size(101, 17);
            this.NextPhotoID_radioButton.TabIndex = 69;
            this.NextPhotoID_radioButton.Text = "Modify Photo ID";
            this.NextPhotoID_radioButton.UseVisualStyleBackColor = true;
            this.NextPhotoID_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // PhotoTitle_label
            // 
            this.PhotoTitle_label.AutoSize = true;
            this.PhotoTitle_label.BackColor = System.Drawing.Color.Navy;
            this.PhotoTitle_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhotoTitle_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PhotoTitle_label.Location = new System.Drawing.Point(106, 0);
            this.PhotoTitle_label.Name = "PhotoTitle_label";
            this.PhotoTitle_label.Size = new System.Drawing.Size(138, 20);
            this.PhotoTitle_label.TabIndex = 70;
            this.PhotoTitle_label.Text = "PhotoTitle_label";
            // 
            // PhotoSource_comboBox
            // 
            this.PhotoSource_comboBox.FormattingEnabled = true;
            this.PhotoSource_comboBox.Location = new System.Drawing.Point(99, 601);
            this.PhotoSource_comboBox.Name = "PhotoSource_comboBox";
            this.PhotoSource_comboBox.Size = new System.Drawing.Size(233, 21);
            this.PhotoSource_comboBox.TabIndex = 0;
            // 
            // Delete_button
            // 
            this.Delete_button.Location = new System.Drawing.Point(514, 711);
            this.Delete_button.Name = "Delete_button";
            this.Delete_button.Size = new System.Drawing.Size(78, 23);
            this.Delete_button.TabIndex = 72;
            this.Delete_button.TabStop = false;
            this.Delete_button.Text = "Delete";
            this.Delete_button.UseVisualStyleBackColor = true;
            this.Delete_button.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // FPhotoViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1284, 736);
            this.Controls.Add(this.Delete_button);
            this.Controls.Add(this.PhotoSource_comboBox);
            this.Controls.Add(this.PhotoTitle_label);
            this.Controls.Add(this.NextPhotoID_radioButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PhotoName_textBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PicturedBuildings_ListBox);
            this.Controls.Add(this.PicturedPersons_listBox);
            this.Controls.Add(this.Previous_button);
            this.Controls.Add(this.Num_groupBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Categories_listBox);
            this.Controls.Add(this.FullSize_button);
            this.Controls.Add(this.PhotoList_button);
            this.Controls.Add(this.NumPhotos_Label);
            this.Controls.Add(this.PhotoYear_TextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PhotoNotes_TextBox);
            this.Controls.Add(this.Next_button);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(20, 50);
            this.Name = "FPhotoViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Picture Viewer";
            this.Num_groupBox.ResumeLayout(false);
            this.Num_groupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //****************************************************************************************************************************
        protected System.Windows.Forms.RichTextBox PhotoNotes_TextBox;
        protected System.Windows.Forms.TextBox PhotoYear_TextBox;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Button Save_button;
        protected System.Windows.Forms.Button Next_button;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label4;
        protected System.Windows.Forms.TextBox NumPicturedPersons_textBox;
        protected System.Windows.Forms.TextBox NumPicturedBuildings_textBox;
        protected System.Windows.Forms.Label LeftToRight;
        protected System.Windows.Forms.Label NumPhotos_Label;
        protected System.Windows.Forms.Button PhotoList_button;
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
        public virtual void DisplayPhotoInViewer(int    iPersonID)
        {
            this.Categories_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CategorylistBox_Click);
            this.PicturedPersons_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedPersonlistBox_Click);
            this.PicturedBuildings_ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedBuildinglistBox_Click);
            PicturedPersons_listBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicturedPersons_listBox_RightClick);
            PicturedBuildings_ListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicturedBuildings_listBox_RightClick);
            Q.v(m_SQL,m_SQL.DefinePhoto(m_Photo_ds));
            if (m_tbl == null)
            {
                string sWhereStatement = "";
                if (iPersonID != 0)
                    sWhereStatement = " where PersonID = " + iPersonID.ToString();
                m_tbl = new DataTable(m_sTableName);
                Q.v(m_SQL,m_SQL.selectdistinctPhotoID(m_tbl, m_sTableName, sWhereStatement));
                m_tbl.PrimaryKey = new DataColumn[] { m_tbl.Columns[U.PhotoID_col] };
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
                SetNextPhotoIDVisible(PhotoName_textBox.Text.ToString());
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
            PhotoTitle_label.Text = "Photograph from the " + sSource + "   ";
            PhotoNotes_TextBox.Text = Q.s(m_SQL,m_SQL.PhotoNotes(row));
            int iYear = Q.i(m_SQL,m_SQL.PhotoYear(row));
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
            m_myFileName = U.FileNameWithExtension(sPhotoName, Q.s(m_SQL, m_SQL.JpgFolder()), U.sJPGExtension);
            //            m_myFileName = Q.s(m_SQL,m_SQL.GetPhotoFilename(row, U.JPGPhotoFolder, U.Photo_Table));
//            m_Photo = GetPhotoBitmap(m_myFileName);
            DisplayNumPhotos(U.Photo_Table);
        }
        //****************************************************************************************************************************
        private Bitmap GetPhotoBitmap(string sFileName)
        {
            try
            {
                FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read);//Read only
                Image Im = System.Drawing.Image.FromStream(fs);
                fs.Close();
                fs.Dispose();
                return (Bitmap) Im;
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
                    sPersonName = Q.s(m_SQL,m_SQL.GetPersonName(iPersonID));
                }
                PicturedPersons_listBox.Items.Add(sPersonName);
            }
        }
        //****************************************************************************************************************************
        protected void SetNextPhotoIDVisible(string sPhotoName)
        {
            int iName = U.GetPhotoNumber(sPhotoName, 2);
            if (sPhotoName.Substring(0, 2) != "HF" ||
                U.GetPhotoNumber(sPhotoName, 2) <= 0)
            {
                NextPhotoID_radioButton.Text = "Set To Next HF Photo ID";
                NextPhotoID_radioButton.Visible = true;
            }
            else
            {
                NextPhotoID_radioButton.Text = "Set To This Photo Name";
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
                    sBuildingName = row[U.BuildingValueValue_col].ToString();
                }
                PicturedBuildings_ListBox.Items.Add(sBuildingName);
            }
        }
        //****************************************************************************************************************************
        private string BuildNameFromPicturedPersonTable(DataRow row)
        {
            string s1 = row[U.FirstName_col].ToString();
            string s2 = row[U.LastName_col].ToString();
            return Q.s(m_SQL,m_SQL.BuildNameString(row[U.FirstName_col].ToString(),
                                                               row[U.MiddleName_col].ToString(),
                                                               row[U.LastName_col].ToString(),
                                                               row[U.Suffix_col].ToString(),
                                                               row[U.Prefix_col].ToString(),
                                                               row[U.MarriedName_col].ToString(),
                                                               row[U.KnownAs_col].ToString()));      
        }
        //****************************************************************************************************************************
        protected void DisplayPhotograph(int iPhotoID)
        {
            PhotoTitle_label.Visible = iPhotoID != 0;
            Q.v(m_SQL,m_SQL.GetPhoto(ref m_Photo_ds, iPhotoID));
            if (m_Photo_ds.Tables[U.Photo_Table].Rows.Count != 0)
            {
                DataRow row = m_Photo_ds.Tables[U.Photo_Table].Rows[0];
                LoadPhotoIntoControls(row);
                DisplayPhotodPersons(m_SQL.NumPicturedPersons(row));
                DisplayPhotodBuildings(m_SQL.NumPicturedBuildings(row));
                UU.LoadCategory_listBox(m_Photo_ds.Tables[U.PhotoCategoryValue_Table], Categories_listBox, m_SQL,
                                           U.PhotoCategoryValue_Table, U.Photo_Table, row[U.PhotoID_col].ToInt());
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
            if (iPhotoToDisplay >= m_iCountOfPhotos-1)
            {
                PhotoList_button.Visible = true;
            }
            else 
            if (iPhotoToDisplay <= 0)
            {
                PhotoList_button.Visible = true;
            }
            else
            {
                PhotoList_button.Visible = true;
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
            if (m_bPhotoIsNull && m_myFileName.Length > 0 && !m_bLockPhotoFromDisplay)
            {
                m_bPhotoIsNull = false;
                m_Photo = GetPhotoBitmap(m_myFileName);
                if (m_Photo == null)
                    m_bPhotoIsNull = true;
                else
                {
                    m_bPhotoAlreadyDrawn = false;
                    m_bPhotoIsNull = false;
                }
            }
            if (m_Photo != null) // && !m_bPhotoAlreadyDrawn)
            {
                PaintPhoto(myArgs, m_Photo, 500, 800, 24);
//                m_bPhotoAlreadyDrawn = true;
            }
        }
        //****************************************************************************************************************************
        private void PeoplePictured_button_Click()
        {
            int iOldNumPeoplePictured = m_iNumPeoplePictured;
            m_iNumPeoplePictured = NumPicturedPersons_textBox.Text.ToInt();
            for (int i = iOldNumPeoplePictured; i > m_iNumPeoplePictured; --i)
                PicturedPersons_listBox.Items.RemoveAt(i-1);
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
            NextPhotoID_radioButton.Visible = false;
            NextPhotoID_radioButton.Checked = false;
            SetNextPhotoIDVisible(PhotoName_textBox.Text.ToString()); 
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
            CGridPhoto GridPhoto = new CGridPhoto(ref m_SQL, ref m_tbl, U.Photo_Table);
            GridPhoto.ShowDialog();
            int iPhoto = GridPhoto.SelectedRow;
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
        private string GetPersonName(int    iPicturedPictureNumber,
                                     string OldPersonName)
        {
            int iOldPersonID = 0;;
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
                sString = Q.s(m_SQL,m_SQL.GetPersonName(iPersonID));
            }
            return sString;
        }
        //****************************************************************************************************************************
        private string GetBuildingName(int iPicturedBuildingNumber,
                                       string OldBuildingName)
        {
            int iOldBuildingValueID = 0; ;
            DataTable PicturedBuilding_tbl = m_Photo_ds.Tables[U.PicturedBuilding_Table];
            DataRow row = PicturedBuilding_tbl.Rows.Find(iPicturedBuildingNumber);
            if (row != null)
                iOldBuildingValueID = row[U.BuildingValueID_col].ToInt();

            int iBuildingValueID = iOldBuildingValueID;
            int iBuildingID = Q.i(m_SQL, m_SQL.GetBuildingIDFromBuildingValueID(iBuildingValueID));
            CGridGroupValuesModernRoads GridGroupValuesModernRoads = new CGridGroupValuesModernRoads(m_SQL, iBuildingID, false, false, true);
            GridGroupValuesModernRoads.ShowDialog();
            iBuildingValueID = GridGroupValuesModernRoads.SelectedBuilding;
            string sString = "";
            if (iBuildingValueID <= 0)
                sString = OldBuildingName;
            else
            {
                if (row != null)
                    row[U.BuildingValueID_col] = iBuildingValueID;
                else
                {
                    row = PicturedBuilding_tbl.NewRow();
                    row[U.PicturedBuildingNumber_col] = iPicturedBuildingNumber;
                    row[U.PhotoID_col] = m_iPhotoID;
                    row[U.BuildingValueID_col] = iBuildingValueID;
                    PicturedBuilding_tbl.Rows.Add(row);
                }
                sString = Q.s(m_SQL,m_SQL.GetBuildingValueValue(iBuildingValueID));
            }
            return sString;
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
            if (PhotoName_textBox.Text.ToString() != m_OldPhotoName)
                NextPhotoID_radioButton.Visible = true;
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
                ///////////            DisplayPhotograph(m_iPhotoID);
            }
        }
        //****************************************************************************************************************************
        public virtual void SaveButton_Click(object sender, System.EventArgs e)
        {
            SaveAndDisplayPhoto();
        }
        //****************************************************************************************************************************
        public virtual void DeleteButton_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Delete Photo?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DataRow row = m_tbl.Rows[m_iCurrentPhoto];
                int iPhotoID = row[U.PhotoID_col].ToInt();
                if (m_SQL.DeletePhoto(iPhotoID))
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
            NextPhotoID_radioButton.Checked = false;
            NumPicturedPersons_textBox.Modified = false;
            NumPicturedBuildings_textBox.Modified = false;
        }
        //****************************************************************************************************************************
        private bool SavePhotograph()
        {
            string sPhotoID = PhotoName_textBox.Text.TrimString();
            if (m_iPhotoID == 0 && sPhotoID == Q.s(m_SQL,m_SQL.GetPhotoName(sPhotoID)))
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
            string sOldFileName = m_myFileName;
            if (m_Photo == null)
                sOldFileName = "";
            string sNewFileName = U.FileNameWithExtension(sPhotoName, Q.s(m_SQL, m_SQL.JpgFolder()), U.sJPGExtension);
            m_bLockPhotoFromDisplay = true;
            if (!m_bPhotoIsNull)
            {
                m_Photo.Dispose();
                m_bPhotoIsNull = true;
            }
            m_Photo = null;
            int iPhotoID = Q.i(m_SQL, m_SQL.SavePhoto(ref m_Photo_ds, U.PhotoCategoryValue_Table, m_iNumPeoplePictured, 
                         m_iNumBuildingsPictured, m_iPhotoID,
                         PhotoNotes_TextBox.Text.TrimString(),
                         PhotoYear_TextBox.Text.ToInt(), PhotoSource_comboBox.Text.TrimString(),
                         PhotoDrawer_textBox.Text.ToInt(), PhotoFolder_textBox.Text.ToInt(),
                         PhotoName_textBox.Text.TrimString(), sNewFileName, sOldFileName, m_OldPhotoName));
            if (iPhotoID != 0)
            {
                m_iPhotoID = iPhotoID;
                m_bLockPhotoFromDisplay = false;
//                m_Photo = GetPhotoBitmap(sNewFileName);
                m_myFileName = sNewFileName;
                m_OldPhotoName = sPhotoName;
                if (m_Photo == null)
                    m_bPhotoIsNull = true;
                this.Refresh();
                m_sPhotoSource = PhotoSource_comboBox.Text.ToString();
                UU.LoadSourceComboBox(m_SQL, PhotoSource_comboBox);
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
        private bool PhotoChanged()
        {
            if (PhotoName_textBox.Modified || 
                PhotoYear_TextBox.Modified ||
                U.Modified(PhotoSource_comboBox.Text.TrimString(), m_Photo_ds.Tables[U.Photo_Table], "", U.PhotoSource_col) ||
                PhotoDrawer_textBox.Modified || 
                PhotoFolder_textBox.Modified ||
                PhotoNotes_TextBox.Modified ||
                NumPicturedPersons_textBox.Modified ||
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
        protected virtual void radioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            if (m_OldPhotoName.Substring(0,2) == "HF" && PhotoName_textBox.Text.ToString() == m_OldPhotoName)
            {
                NextPhotoID_radioButton.Visible = false;
                NextPhotoID_radioButton.Checked = false;
                PhotoName_textBox.Modified = false;
            }
            else if (m_OldPhotoName != PhotoName_textBox.Text.ToString())
            {
                int iNewPhotoID = Q.i(m_SQL, m_SQL.GetPhotoID(PhotoName_textBox.Text.ToString()));
                int iCurrentRow = CurrentLocation(iNewPhotoID);
                if (iCurrentRow == U.Exception)
                {
                    MessageBox.Show("Photo Name Not In This Photo List");
                    PhotoName_textBox.Text = m_OldPhotoName;
                    NextPhotoID_radioButton.Visible = false;
                }
                else
                {
                    m_iCurrentPhoto = iCurrentRow;
                    DisplayNewPhoto(m_iCurrentPhoto);
                    m_bLockPhotoFromDisplay = false;
                }
            }
            else if (((RadioButton)sender).Checked == true)
            {
                string sNextPhotoName = Q.s(m_SQL, m_SQL.GetNextPhotoName());
                PhotoName_textBox.Text = U.GetNextFilename(sNextPhotoName, Q.s(m_SQL, m_SQL.JpgFolder()));
                PhotoName_textBox.Modified = true;
            }
        }
        //****************************************************************************************************************************
        public virtual void NewPhotoButton_Click(object sender, System.EventArgs e)
        {
        }
        //****************************************************************************************************************************
        protected void CategorylistBox_Click(object sender, EventArgs e)
        {
            CListboxGroup GroupListbox = new CListboxGroup(m_SQL, Categories_listBox, U.PhotoCategoryValue_Table, U.CategoryValueValue_col);
            GroupListbox.ShowGroupListbox(ref m_Photo_ds);
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