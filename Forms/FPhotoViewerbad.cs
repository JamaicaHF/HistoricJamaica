using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace HistoricJamaica
{
    public class FPhotoViewer : FPaintPhoto
    {
        protected CSql m_SQL;
        protected string m_sTableName = "";
        protected int m_iNumPeoplePictured = 0;
        protected int m_iNumBuildingsPictured = 0;
        protected bool m_bPhotoAlreadyDrawn = false;
        protected bool m_aborted = false;
        public Bitmap m_Photo = null;
        protected string m_myFileName = "";
        protected DataTable m_tbl = null;
        protected int m_iCountOfPhotos;
        protected int m_iCurrentPhoto = 0;
        private Button FullSize_button;
        protected ListBoxWithDoubleClick Categories_listBox;
        protected Label label6;
        private GroupBox Num_groupBox;
        private bool m_bGetNumPeople;
        private bool m_bGetNumBuilding;
        protected Button Previous_button;
        private System.ComponentModel.IContainer components = null;
        protected int m_iPhotoID = 0;
        protected ListBoxWithDoubleClick PicturedPersons_listBox;
        protected DataSet m_Photo_ds = new DataSet();
        protected ListBoxWithDoubleClick PicturedBuildings_ListBox;
        private GroupBox groupBox1;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label12;
        protected TextBox PhotoFolder_textBox;
        protected TextBox PhotoID_textBox;
        protected TextBox PhotoSource_textBox;
        private GroupBox groupBox2;
        protected TextBox PhotoDrawer_textBox;
        private Label label5;
        private ContextMenuStrip PicturedPersonMenuStrip = new ContextMenuStrip();
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
            this.ContextMenu = new ContextMenu();
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
            m_sTableName = sTableName;
            InitializePhoto();
            DisplayPhotoInViewer(0);
        }
        //****************************************************************************************************************************
        private void InitializePhoto()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(1100, 800);
            this.ContextMenu = new ContextMenu();
            PhotoNotes_TextBox.MaxLength = U.iMaxDescriptionLength;
            PhotoSource_textBox.MaxLength = U.iMaxNameLength;
            PhotoID_textBox.MaxLength = U.iMaxNameLength;
            PhotoYear_TextBox.MaxLength = 4;
            PhotoDrawer_textBox.MaxLength = 1;
            PhotoFolder_textBox.MaxLength = 2;
            NumPicturedPersons_textBox.MaxLength = 2;
            NumPicturedBuildings_textBox.MaxLength = 2;
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
            switch (e.Button)
            {
                case MouseButtons.Left:
                    MessageBox.Show(this, "Left Button Click");
                    break;
                case MouseButtons.Right:
                    MessageBox.Show(this, "Right Button Click");
                    break;
                case MouseButtons.Middle:
                    break;
                default:
                    break;
            }
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
            this.Next_button = new System.Windows.Forms.Button();
            this.PhotoYear_TextBox = new System.Windows.Forms.TextBox();
            this.NumPicturedPersons_textBox = new System.Windows.Forms.TextBox();
            this.NumPhotos_Label = new System.Windows.Forms.Label();
            this.PhotoList_button = new System.Windows.Forms.Button();
            this.FullSize_button = new System.Windows.Forms.Button();
            this.Categories_listBox = new Utilities.ListBoxWithDoubleClick();
            this.label6 = new System.Windows.Forms.Label();
            this.Num_groupBox = new System.Windows.Forms.GroupBox();
            this.Previous_button = new System.Windows.Forms.Button();
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
            this.PhotoID_textBox = new System.Windows.Forms.TextBox();
            this.PhotoSource_textBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PhotoDrawer_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Num_groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhotoNotes_TextBox
            // 
            this.PhotoNotes_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoNotes_TextBox.Location = new System.Drawing.Point(351, 534);
            this.PhotoNotes_TextBox.Name = "PhotoNotes_TextBox";
            this.PhotoNotes_TextBox.Size = new System.Drawing.Size(225, 186);
            this.PhotoNotes_TextBox.TabIndex = 3;
            this.PhotoNotes_TextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 534);
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
            this.label4.Location = new System.Drawing.Point(194, 534);
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
            this.Save_button.Location = new System.Drawing.Point(171, 735);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(75, 23);
            this.Save_button.TabIndex = 6;
            this.Save_button.Text = "Save";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Next_button
            // 
            this.Next_button.Location = new System.Drawing.Point(429, 735);
            this.Next_button.Name = "Next_button";
            this.Next_button.Size = new System.Drawing.Size(75, 23);
            this.Next_button.TabIndex = 7;
            this.Next_button.Text = "Next";
            this.Next_button.UseVisualStyleBackColor = true;
            this.Next_button.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PhotoYear_TextBox
            // 
            this.PhotoYear_TextBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoYear_TextBox.Location = new System.Drawing.Point(292, 533);
            this.PhotoYear_TextBox.Name = "PhotoYear_TextBox";
            this.PhotoYear_TextBox.Size = new System.Drawing.Size(40, 20);
            this.PhotoYear_TextBox.TabIndex = 4;
            // 
            // NumPicturedPersons_textBox
            // 
            this.NumPicturedPersons_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.NumPicturedPersons_textBox.Location = new System.Drawing.Point(160, 24);
            this.NumPicturedPersons_textBox.Name = "NumPicturedPersons_textBox";
            this.NumPicturedPersons_textBox.Size = new System.Drawing.Size(30, 20);
            this.NumPicturedPersons_textBox.TabIndex = 8;
            this.NumPicturedPersons_textBox.Leave += new System.EventHandler(this.PeoplePictured_Leave);
            this.NumPicturedPersons_textBox.Enter += new System.EventHandler(this.PeoplePictured_Enter);
            // 
            // NumPhotos_Label
            // 
            this.NumPhotos_Label.AutoSize = true;
            this.NumPhotos_Label.Location = new System.Drawing.Point(46, 740);
            this.NumPhotos_Label.Name = "NumPhotos_Label";
            this.NumPhotos_Label.Size = new System.Drawing.Size(62, 13);
            this.NumPhotos_Label.TabIndex = 6;
            this.NumPhotos_Label.Text = "NumPhotos";
            // 
            // PhotoList_button
            // 
            this.PhotoList_button.Location = new System.Drawing.Point(257, 735);
            this.PhotoList_button.Name = "PhotoList_button";
            this.PhotoList_button.Size = new System.Drawing.Size(75, 23);
            this.PhotoList_button.TabIndex = 5;
            this.PhotoList_button.Text = "Picture List";
            this.PhotoList_button.UseVisualStyleBackColor = true;
            this.PhotoList_button.Click += new System.EventHandler(this.PhotoList_Click);
            // 
            // FullSize_button
            // 
            this.FullSize_button.Location = new System.Drawing.Point(514, 735);
            this.FullSize_button.Name = "FullSize_button";
            this.FullSize_button.Size = new System.Drawing.Size(75, 23);
            this.FullSize_button.TabIndex = 50;
            this.FullSize_button.Text = "Full Size";
            this.FullSize_button.UseVisualStyleBackColor = true;
            this.FullSize_button.Click += new System.EventHandler(this.FullSize_Click);
            // 
            // Categories_listBox
            // 
            this.Categories_listBox.BackColor = System.Drawing.SystemColors.Window;
            this.Categories_listBox.FormattingEnabled = true;
            this.Categories_listBox.Location = new System.Drawing.Point(603, 534);
            this.Categories_listBox.Name = "Categories_listBox";
            this.Categories_listBox.Size = new System.Drawing.Size(232, 186);
            this.Categories_listBox.TabIndex = 51;
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
            this.Num_groupBox.Location = new System.Drawing.Point(854, 42);
            this.Num_groupBox.Name = "Num_groupBox";
            this.Num_groupBox.Size = new System.Drawing.Size(200, 61);
            this.Num_groupBox.TabIndex = 53;
            this.Num_groupBox.TabStop = false;
            // 
            // Previous_button
            // 
            this.Previous_button.Location = new System.Drawing.Point(343, 735);
            this.Previous_button.Name = "Previous_button";
            this.Previous_button.Size = new System.Drawing.Size(75, 23);
            this.Previous_button.TabIndex = 54;
            this.Previous_button.Text = "Previous";
            this.Previous_button.UseVisualStyleBackColor = true;
            this.Previous_button.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // PicturedPersons_listBox
            // 
            this.PicturedPersons_listBox.BackColor = System.Drawing.SystemColors.Window;
            this.PicturedPersons_listBox.FormattingEnabled = true;
            this.PicturedPersons_listBox.Location = new System.Drawing.Point(854, 129);
            this.PicturedPersons_listBox.Name = "PicturedPersons_listBox";
            this.PicturedPersons_listBox.Size = new System.Drawing.Size(200, 251);
            this.PicturedPersons_listBox.TabIndex = 56;
            // 
            // PicturedBuildings_ListBox
            // 
            this.PicturedBuildings_ListBox.BackColor = System.Drawing.SystemColors.Window;
            this.PicturedBuildings_ListBox.FormattingEnabled = true;
            this.PicturedBuildings_ListBox.Location = new System.Drawing.Point(854, 469);
            this.PicturedBuildings_ListBox.Name = "PicturedBuildings_ListBox";
            this.PicturedBuildings_ListBox.Size = new System.Drawing.Size(200, 251);
            this.PicturedBuildings_ListBox.TabIndex = 57;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.NumPicturedBuildings_textBox);
            this.groupBox1.Location = new System.Drawing.Point(854, 383);
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
            this.NumPicturedBuildings_textBox.TabIndex = 8;
            this.NumPicturedBuildings_textBox.Leave += new System.EventHandler(this.BuildingPictured_Leave);
            this.NumPicturedBuildings_textBox.Enter += new System.EventHandler(this.BuildingPictured_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(600, 518);
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
            this.label12.Location = new System.Drawing.Point(46, 600);
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
            // PhotoID_textBox
            // 
            this.PhotoID_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoID_textBox.Location = new System.Drawing.Point(95, 530);
            this.PhotoID_textBox.Name = "PhotoID_textBox";
            this.PhotoID_textBox.Size = new System.Drawing.Size(75, 20);
            this.PhotoID_textBox.TabIndex = 65;
            this.PhotoID_textBox.Text = "HF";
            this.PhotoID_textBox.TextChanged += new System.EventHandler(this.PhotoID_textBox_TextChanged);
            // 
            // PhotoSource_textBox
            // 
            this.PhotoSource_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.PhotoSource_textBox.Location = new System.Drawing.Point(95, 600);
            this.PhotoSource_textBox.Name = "PhotoSource_textBox";
            this.PhotoSource_textBox.Size = new System.Drawing.Size(237, 20);
            this.PhotoSource_textBox.TabIndex = 66;
            this.PhotoSource_textBox.Text = "Collection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.PhotoDrawer_textBox);
            this.groupBox2.Controls.Add(this.PhotoFolder_textBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(38, 657);
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
            this.label5.Location = new System.Drawing.Point(348, 518);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 68;
            this.label5.Text = "Notes";
            // 
            // FPhotoViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1112, 789);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PhotoID_textBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.PhotoSource_textBox);
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
        public virtual void DisplayPhotoInViewer(int    iPersonID)
        {
            this.Categories_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CategorylistBox_Click);
            this.PicturedPersons_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedPersonlistBox_Click);
            this.PicturedBuildings_ListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PicturedBuildinglistBox_Click);
            m_SQL.DefinePhoto(m_Photo_ds);
            if (m_tbl == null)
            {
                string sWhereStatement = "";
                if (iPersonID != 0)
                    sWhereStatement = " where PersonID = " + iPersonID.ToString();
                m_tbl = new DataTable(m_sTableName);
                m_SQL.selectdistinctPhotoID(m_tbl, m_sTableName, sWhereStatement);
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
            PhotoNotes_TextBox.Text = m_SQL.PhotoNotes(row);
            int iYear = m_SQL.PhotoYear(row);
            if (iYear == 0)
                PhotoYear_TextBox.Text = "";
            else
                PhotoYear_TextBox.Text = iYear.ToString();
            PhotoSource_textBox.Text = row[U.PhotoSource_col].ToString();
            PhotoFolder_textBox.Text = row[U.PhotoDrawer_col].ToString();
            PhotoFolder_textBox.Text = row[U.PhotoFolder_col].ToString();
            PhotoID_textBox.Text = row[U.PhotoName_col].ToString();
            m_myFileName = m_SQL.GetPhotoFilename(row, U.PhotoFolder, U.Photo_Table);
            m_Photo = GetPhotoBitmap(m_myFileName);
            DisplayNumPhotos(U.Photo_Table);
        }
        //****************************************************************************************************************************
        private Bitmap GetPhotoBitmap(string sFileName)
        {
            try
            {
                return new Bitmap(sFileName);
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
                    sPersonName = m_SQL.GetPersonName(iPersonID);
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
            string s1 = row[U.PersonFirstName_col].ToString();
            string s2 = row[U.PersonLastName_col].ToString();
            return m_SQL.BuildNameString(row[U.PersonFirstName_col].ToString(),
                                                               row[U.PersonMiddleName_col].ToString(),
                                                               row[U.PersonLastName_col].ToString(),
                                                               row[U.PersonSuffix_col].ToString(),
                                                               row[U.PersonPrefix_col].ToString(),
                                                               row[U.PersonMaidenMarriedName_col].ToString(),
                                                               row[U.PersonKnownAs_col].ToString());      
        }
        //****************************************************************************************************************************
        protected void DisplayPhotograph(int iPhotoID)
        {
            m_SQL.GetPhoto(ref m_Photo_ds, iPhotoID);
            if (m_Photo_ds.Tables[U.Photo_Table].Rows.Count != 0)
            {
                DataRow row = m_Photo_ds.Tables[U.Photo_Table].Rows[0];
                LoadPhotoIntoControls(row);
                DisplayPhotodPersons(m_SQL.NumPicturedPersons(row));
                DisplayPhotodBuildings(m_SQL.NumPicturedBuildings(row));
                m_SQL.LoadCategory_listBox(m_Photo_ds.Tables[U.PhotoCategoryValue_Table], Categories_listBox,
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
                Previous_button.Enabled = true;
                Next_button.Enabled = false;
                PhotoList_button.Visible = true;
            }
            else 
            if (iPhotoToDisplay <= 0)
            {
                Previous_button.Enabled = false;
                Next_button.Enabled = true;
                PhotoList_button.Visible = true;
            }
            else
            {
                Previous_button.Enabled = true;
                Next_button.Enabled = true;
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
            }
            EnableCorrectButtonsBasedOnWhereInListWeAre(iPhotoToDisplay);
        }
        //****************************************************************************************************************************
        protected override void OnPaint(PaintEventArgs myArgs)
        {
            if (m_Photo == null)
            {
//                PaintPicture(myArgs, m_myPicture, 500, 800);
                if (m_myFileName.Length > 0) // check to see if we now have a filename
                    m_Photo = GetPhotoBitmap(m_myFileName);
                m_bPhotoAlreadyDrawn = false;
            }
            if (m_Photo != null && !m_bPhotoAlreadyDrawn)
            {
                PaintPhoto(myArgs, m_Photo, 500, 800);
                m_bPhotoAlreadyDrawn = true;
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
        public virtual void PreviousButton_Click(object sender, System.EventArgs e)
        {
            if (CancelIfChanged())
                return;
            if (m_iCurrentPhoto > 0)
            {
                m_iCurrentPhoto--;
                DisplayPhoto(m_iCurrentPhoto);
                this.Invalidate();
            }
        }
        //****************************************************************************************************************************
        public virtual void NextButton_Click(object sender, System.EventArgs e)
        {
            if (CancelIfChanged())
                return;
            if (m_iCurrentPhoto < (m_iCountOfPhotos - 1))
            {
                m_iCurrentPhoto++;
                DisplayPhoto(m_iCurrentPhoto);
                this.Invalidate();
            }
        }
        //****************************************************************************************************************************
        public virtual void PhotoList_Click(object sender, System.EventArgs e)
        {
            if (CancelIfChanged())
                return;
            CGridDataViewPhoto GridDataViewPhoto = new CGridDataViewPhoto(ref m_SQL, ref m_tbl, U.Photo_Table);
            GridDataViewPhoto.ShowDialog();
            m_iCurrentPhoto = GridDataViewPhoto.SelectedRow;
            if (m_iCurrentPhoto >= 0)
            {
                DisplayPhoto(m_iCurrentPhoto);
                this.Invalidate();
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
            
            int iPersonID = iOldPersonID;
//          DataTable Person_tbl = m_SQL.DefinePersonTable();
//          CPersonGridDataView PersonGridDataView = new CPersonGridDataView(m_SQL, ref Person_tbl, false, false);
//          PersonGridDataView.ShowDialog();
//          iPersonID = PersonGridDataView.SelectedPersonID;
//          if (iPersonID == 0)
                iPersonID = m_SQL.GetPersonFromDatabase(0, true);
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
                sString = m_SQL.GetPersonName(iPersonID);
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
            DataTable Building_tbl = m_SQL.DefineBuildingTable();
            CGridDataViewGroupValuesBuilding GridDataViewGroupValuesBuilding = new CGridDataViewGroupValuesBuilding(m_SQL, true);
            GridDataViewGroupValuesBuilding.ShowDialog();
            iBuildingValueID = GridDataViewGroupValuesBuilding.SelectedValueID;

            string sString = "";
            if (iBuildingValueID == 0)
            {
                if (iOldBuildingValueID == 0)
                    sString = U.Unknown;
                else
                    sString = OldBuildingName;
            }
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
                sString = m_SQL.GetBuildingValueValue(iBuildingValueID);
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
            m_bGetNumPeople = false;
        }
        //****************************************************************************************************************************
        private void BuildingPictured_Enter(object sender, System.EventArgs e)
        {
            m_bGetNumBuilding = true;
        }
        //****************************************************************************************************************************
        private void BuildingPictured_Leave(object sender, System.EventArgs e)
        {
            m_bGetNumBuilding = false;
        }
        //****************************************************************************************************************************
        public virtual void SaveButton_Click(object sender, System.EventArgs e)
        {
            string sPhotoName = PhotoID_textBox.Text.TrimString();
            string sFileName = U.FileNameWithExtension(sPhotoName, U.PhotoFolder);
            if (m_iPhotoID == 0 && sPhotoName == m_SQL.GetPhotoName(sPhotoName))
            {
                MessageBox.Show("Photo Name Already Exists");
                return;
            }
            if (sPhotoName.Length == 0)
            {
                MessageBox.Show("PhotoName cannot be empty");
                return;
            }
            if (!CheckForInvalidCharacters(sPhotoName))
            {
                return;  // message printed witin function
            }
            SavePhotograph();
            SetToUnmodified();
//            m_myFileName = sFileName;
            DisplayPhotograph(m_iPhotoID);
        }
        //****************************************************************************************************************************
        protected void SetToUnmodified()
        {
            PhotoID_textBox.Modified = false;
            PhotoYear_TextBox.Modified = false;
            PhotoSource_textBox.Modified = false;
            PhotoDrawer_textBox.Modified = false;
            PhotoFolder_textBox.Modified = false;
            PhotoNotes_TextBox.Modified = false;
            NumPicturedPersons_textBox.Modified = false;
            NumPicturedBuildings_textBox.Modified = false;
        }
        //****************************************************************************************************************************
        private void SavePhotograph()
        {
            m_iPhotoID = m_SQL.SavePhoto(ref m_Photo_ds, U.PhotoCategoryValue_Table, m_iNumPeoplePictured, m_iNumBuildingsPictured, m_iPhotoID,
                         PhotoNotes_TextBox.Text.TrimString(),
                         PhotoYear_TextBox.Text.ToInt(), PhotoSource_textBox.Text.TrimString(),
                         PhotoDrawer_textBox.Text.ToInt(), PhotoFolder_textBox.Text.ToInt(),
                         PhotoID_textBox.Text.TrimString(), m_myFileName);
        }
        //****************************************************************************************************************************
        private bool PhotoChanged()
        {
            if (PhotoID_textBox.Modified || 
                PhotoYear_TextBox.Modified ||
                PhotoSource_textBox.Modified ||
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
            if (!PhotoChanged() || m_iPhotoID == 0)
                return false;
            switch (MessageBox.Show("Save Changes?", "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    SavePhotograph();
                    return false;
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

        private void PhotoID_textBox_TextChanged(object sender, EventArgs e)
        {

        }
        //****************************************************************************************************************************
    }
}