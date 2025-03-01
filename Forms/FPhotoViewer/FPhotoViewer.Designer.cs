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
        protected RichTextBox PhotoNotes_TextBox;
        protected TextBox PhotoYear_TextBox;
        protected Label label1;
        protected Button Save_button;
        protected Button Next_button;
        protected Label label2;
        protected Label label4;
        protected TextBox NumPicturedPersons_textBox;
        protected TextBox NumPicturedBuildings_textBox;
        protected Label LeftToRight;
        protected Label NumPhotos_Label;
        protected Button PhotoList_button;
        protected ListBoxWithDoubleClick PicturedPersons_listBox;
        protected ListBoxWithDoubleClick PicturedBuildings_ListBox;
        protected GroupBox groupBox1;
        protected Label label6;
        protected Label label7;
        protected Label label8;
        protected Label label9;
        protected Label label10;
        protected Label label12;
        protected TextBox PhotoFolder_textBox;
        protected TextBox PhotoName_textBox;
        protected GroupBox groupBox2;
        protected TextBox PhotoDrawer_textBox;
        protected Label label5;
        protected Label PhotoTitle_label;
        protected ContextMenuStrip ToolStrip = new ContextMenuStrip();
        protected ComboBox PhotoSource_comboBox;
        protected Button Delete_button;
        protected Button CopyCategory_button;
        protected Button PasteCategory_button;
        protected TextBox GetPhoto_textBox;
        protected Button buttonSlideShow;
        protected Button Previous_button;
        protected Button FullSize_button;
        private GroupBox Num_groupBox;
        protected ListBoxWithDoubleClick Categories_listBox;
        #region Windows Form Designer generated code
        //****************************************************************************************************************************
        private void InitializePhoto()
        {
            int iMaxScreenHeight = Screen.PrimaryScreen.Bounds.Height - 50;
            if (Screen.PrimaryScreen.Bounds.Height <= 800)
                this.WindowState = FormWindowState.Maximized;

            InitializeComponent();
            PhotoName_textBox.Enabled = false;
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
            UU.LoadSourceComboBox(PhotoSource_comboBox);
            CopyCategory_button.Visible = false;
            if (m_CopyCategoryValues == null)
            {
                PasteCategory_button.Visible = false;
            }
            if (IsSlideShow)
            {
                ResetControlsForSlideShow();
            }
        }
        //****************************************************************************************************************************
        private void ResetControlsForSlideShow()
        {
            this.label1.Visible = false;
            this.label4.Visible = false;
            this.Save_button.Visible = false;
            this.PhotoYear_TextBox.Visible = false;
            this.NumPhotos_Label.Visible = false;
            this.PhotoList_button.Visible = false;
            this.label6.Visible = false;
            this.label10.Visible = false;
            this.label12.Visible = false;
            this.PhotoName_textBox.Visible = false;
            this.groupBox2.Visible = false;
            this.Delete_button.Visible = false;
            this.CopyCategory_button.Visible = false;
            this.PasteCategory_button.Visible = false;
            this.GetPhoto_textBox.Visible = false;

            this.PhotoNotes_TextBox.Location = new System.Drawing.Point(1360, 520);
            this.PhotoNotes_TextBox.Size = new System.Drawing.Size(200, 100);
            this.FullSize_button.Location = new System.Drawing.Point(1419, 771);
            this.FullSize_button.Text = "Zoom";
            this.Previous_button.Location = new System.Drawing.Point(1369, 800);
            this.Next_button.Location = new System.Drawing.Point(1482, 800);
            this.Categories_listBox.Location = new System.Drawing.Point(1360, 655);
            this.Categories_listBox.Size = new System.Drawing.Size(200, 108);
            this.Num_groupBox.Location = new System.Drawing.Point(1360, 45);
            this.PicturedPersons_listBox.Location = new System.Drawing.Point(1360, 112);
            this.PicturedPersons_listBox.Size = new System.Drawing.Size(200, 160);
            this.PicturedBuildings_ListBox.Location = new System.Drawing.Point(1360, 345);
            this.PicturedBuildings_ListBox.Size = new System.Drawing.Size(200, 147);
            this.groupBox1.Location = new System.Drawing.Point(1360, 278);
            this.label8.Location = new System.Drawing.Point(1360, 639);
            this.label5.Location = new System.Drawing.Point(1360, 504);
            this.PhotoTitle_label.Location = new System.Drawing.Point(1359, 17);
            this.ClientSize = new System.Drawing.Size(1596, 835);
            this.Name = "FPhotoSlideShow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
        //****************************************************************************************************************************
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
            this.PhotoTitle_label = new System.Windows.Forms.Label();
            this.PhotoSource_comboBox = new System.Windows.Forms.ComboBox();
            this.Delete_button = new System.Windows.Forms.Button();
            this.CopyCategory_button = new System.Windows.Forms.Button();
            this.PasteCategory_button = new System.Windows.Forms.Button();
            this.GetPhoto_textBox = new System.Windows.Forms.TextBox();
            this.buttonSlideShow = new System.Windows.Forms.Button();
            this.Then_button = new System.Windows.Forms.Button();
            this.Now_button = new System.Windows.Forms.Button();
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
            this.Save_button.Location = new System.Drawing.Point(110, 711);
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
            this.Previous_button.Location = new System.Drawing.Point(351, 711);
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
            this.Next_button.Location = new System.Drawing.Point(439, 711);
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
            this.NumPicturedPersons_textBox.Enter += new System.EventHandler(this.PeoplePictured_Enter);
            this.NumPicturedPersons_textBox.Leave += new System.EventHandler(this.PeoplePictured_Leave);
            // 
            // NumPhotos_Label
            // 
            this.NumPhotos_Label.AutoSize = true;
            this.NumPhotos_Label.Location = new System.Drawing.Point(0, 717);
            this.NumPhotos_Label.Name = "NumPhotos_Label";
            this.NumPhotos_Label.Size = new System.Drawing.Size(62, 13);
            this.NumPhotos_Label.TabIndex = 6;
            this.NumPhotos_Label.Text = "NumPhotos";
            // 
            // PhotoList_button
            // 
            this.PhotoList_button.Location = new System.Drawing.Point(194, 711);
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
            this.FullSize_button.Location = new System.Drawing.Point(863, 712);
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
            this.NumPicturedBuildings_textBox.Enter += new System.EventHandler(this.BuildingPictured_Enter);
            this.NumPicturedBuildings_textBox.Leave += new System.EventHandler(this.BuildingPictured_Leave);
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
            // PhotoTitle_label
            // 
            this.PhotoTitle_label.AutoSize = true;
            this.PhotoTitle_label.BackColor = System.Drawing.Color.RoyalBlue;
            this.PhotoTitle_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PhotoTitle_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PhotoTitle_label.Location = new System.Drawing.Point(0, 0);
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
            this.Delete_button.Location = new System.Drawing.Point(523, 711);
            this.Delete_button.Name = "Delete_button";
            this.Delete_button.Size = new System.Drawing.Size(78, 23);
            this.Delete_button.TabIndex = 72;
            this.Delete_button.TabStop = false;
            this.Delete_button.Text = "Delete";
            this.Delete_button.UseVisualStyleBackColor = true;
            this.Delete_button.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // CopyCategory_button
            // 
            this.CopyCategory_button.Location = new System.Drawing.Point(745, 711);
            this.CopyCategory_button.Name = "CopyCategory_button";
            this.CopyCategory_button.Size = new System.Drawing.Size(39, 23);
            this.CopyCategory_button.TabIndex = 73;
            this.CopyCategory_button.TabStop = false;
            this.CopyCategory_button.Tag = "CopyCategoryValues";
            this.CopyCategory_button.Text = "Copy";
            this.CopyCategory_button.UseVisualStyleBackColor = true;
            this.CopyCategory_button.Click += new System.EventHandler(this.CopyCategory_Click);
            // 
            // PasteCategory_button
            // 
            this.PasteCategory_button.Location = new System.Drawing.Point(790, 711);
            this.PasteCategory_button.Name = "PasteCategory_button";
            this.PasteCategory_button.Size = new System.Drawing.Size(44, 23);
            this.PasteCategory_button.TabIndex = 74;
            this.PasteCategory_button.TabStop = false;
            this.PasteCategory_button.Tag = "CopyCategoryValues";
            this.PasteCategory_button.Text = "Paste";
            this.PasteCategory_button.UseVisualStyleBackColor = true;
            this.PasteCategory_button.Click += new System.EventHandler(this.PasteCategory_Click);
            // 
            // GetPhoto_textBox
            // 
            this.GetPhoto_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.GetPhoto_textBox.Location = new System.Drawing.Point(278, 714);
            this.GetPhoto_textBox.Name = "GetPhoto_textBox";
            this.GetPhoto_textBox.Size = new System.Drawing.Size(67, 20);
            this.GetPhoto_textBox.TabIndex = 75;
            this.GetPhoto_textBox.Text = "Photo Num";
            this.GetPhoto_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDown_Click);
            // 
            // buttonSlideShow
            // 
            this.buttonSlideShow.Location = new System.Drawing.Point(956, 714);
            this.buttonSlideShow.Name = "buttonSlideShow";
            this.buttonSlideShow.Size = new System.Drawing.Size(78, 23);
            this.buttonSlideShow.TabIndex = 76;
            this.buttonSlideShow.TabStop = false;
            this.buttonSlideShow.Text = "Slide Show";
            this.buttonSlideShow.UseVisualStyleBackColor = true;
            this.buttonSlideShow.Visible = false;
            this.buttonSlideShow.Click += new System.EventHandler(this.SlideShow_Click);
            // 
            // Then_button
            // 
            this.Then_button.Location = new System.Drawing.Point(620, 711);
            this.Then_button.Name = "Then_button";
            this.Then_button.Size = new System.Drawing.Size(50, 23);
            this.Then_button.TabIndex = 77;
            this.Then_button.TabStop = false;
            this.Then_button.Tag = "CopyCategoryValues";
            this.Then_button.Text = "Then2";
            this.Then_button.UseVisualStyleBackColor = true;
            this.Then_button.Click += new System.EventHandler(this.ThenButton_Click);
            // 
            // Now_button
            // 
            this.Now_button.Location = new System.Drawing.Point(675, 711);
            this.Now_button.Name = "Now_button";
            this.Now_button.Size = new System.Drawing.Size(50, 23);
            this.Now_button.TabIndex = 78;
            this.Now_button.TabStop = false;
            this.Now_button.Tag = "CopyCategoryValues";
            this.Now_button.Text = "Now";
            this.Now_button.UseVisualStyleBackColor = true;
            this.Now_button.Click += new System.EventHandler(this.NowButton_Click);
            // 
            // FPhotoViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1318, 808);
            this.Controls.Add(this.Now_button);
            this.Controls.Add(this.Then_button);
            this.Controls.Add(this.PhotoTitle_label);
            this.Controls.Add(this.buttonSlideShow);
            this.Controls.Add(this.GetPhoto_textBox);
            this.Controls.Add(this.PasteCategory_button);
            this.Controls.Add(this.CopyCategory_button);
            this.Controls.Add(this.Delete_button);
            this.Controls.Add(this.PhotoSource_comboBox);
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected Button Then_button;
        protected Button Now_button;
    }
}
