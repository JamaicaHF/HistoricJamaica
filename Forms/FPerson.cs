using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FPerson : Form
    {
        protected eSearchOption SearchBy = eSearchOption.SO_AllNames;
        protected CSql m_SQL;
        protected DataSet m_PersonDS = new DataSet();
        protected int m_iNumberSpouses = 0;
        protected int m_iCurrentSpouseIndex = 0;
        protected int m_iSpouseLocationInArray = 1;
        protected string m_FirstName = "";
        protected string m_MiddleName = "";
        protected string m_MarriedName = "";
        protected string m_KnownAs = "";
        protected string m_LastName = "";
        protected string m_Suffix = "";
        protected string m_Prefix = "";
        protected int m_iPersonID = 0;
        protected int m_iSpouseID = 0;
        protected int m_iFatherID = 0;
        protected int m_iMotherID = 0;
        protected bool m_bDidSearch = false;
        protected bool m_bCategoryOrBuildingChanged = false;
        protected bool m_bSelectPersonForPhoto = false;
        private string m_sPersonWhereStatement = "";
        private int m_iOriginalPersonID = 0;
        protected TextBox LastName_textBox;
        protected TextBox FirstName_textBox;
        protected TextBox MiddleName_textBox;
        protected ComboBox Suffix_comboBox;
        protected TextBox BornDate_textBox;
        protected TextBox BornPlace_textBox;
        protected TextBox DiedDate_textBox;
        protected TextBox DiedPlace_textBox;
        protected TextBoxWithDoubleClick Father_textBox;
        protected TextBoxWithDoubleClick Mother_textBox;
        protected TextBoxWithDoubleClick Spouse_textBox;
        protected TextBox BuriedStone_textBox;
        protected TextBox BuriedDate_textBox;
        protected TextBox MarriedName_textBox;
        protected RadioButton Male_radioButton;
        protected CheckBox BornVerified_checkBox;
        protected CheckBox DiedVerified_checkBox;
        protected RadioButton Female_radioButton;
        protected RichTextBox Description_TextBox;
        protected Button ViewPhotographs_button;
        protected Button SearchAll_button;
        protected Label DiedPlace_label;
        protected GroupBox Sex_groupBox;
        protected TextBox BornSource_textBox;
        protected TextBox BornHome_textBox;
        protected GroupBox Born_groupBox;
        protected GroupBox Died_groupBox;
        protected TextBox DiedSource_textBox;
        protected TextBox DiedHome_textBox;
        protected ComboBox Prefix_comboBox;
        protected ListBoxWithDoubleClick Categories_listBox;
        protected ListBoxWithDoubleClick Properties_listBox;
        protected Button Save_button;
        protected CheckBox SimilarNames_checkBox;
        protected GroupBox NameOptions_groupBox;
        protected TextBox DiedBook_textBox;
        protected Label DiedPage_label;
        protected TextBox DiedPage_textBox;
        protected TextBox BornBook_textBox;
        protected TextBox BornPage_textBox;
        protected GroupBox Buried_groupBox;
        protected TextBox BuriedPlace_textBox;
        protected bool m_bVitalBornFound = false;
        protected bool m_bVitalDiedFound = false;
        protected bool m_bVitalBuriedFound = false;
        protected Label label1;
        protected Label label2;
        protected Label label3;
        protected Label label4;
        protected Label label5;
        protected Label label6;
        protected Label label8;
        protected Label label9;
        protected Label label10;
        protected Label label11;
        protected Label label12;
        protected Label label13;
        protected Label label14;
        protected Label label15;
        protected Label label17;
        protected Label label16;
        protected Label label18;
        protected Label label19;
        protected Label label22;
        protected Label label20;
        protected Label label23;
        protected Label label24;
        protected Label label25;
        protected Label label26;
        protected Button Family_button;
        protected Button UniquePhotos_button;
        protected CheckBox BuriedVerified_checkBox;
        protected Label label29;
        protected TextBox BuriedSource_textBox;
        protected Label label28;
        protected Label label27;
        protected TextBox BuriedPage_textBox;
        protected TextBox BuriedBook_textBox;
        protected Button AdditionalSpouse_button;
        protected Button SearchPartial_Button;
        protected Button StartingWith_button;
        protected Button Similar_button;
        protected Label label30;
        private GroupBox groupBox1;
        protected Button VitalRecord_button;
        protected TextBox Source_textBox;
        protected Label label7;
        protected TextBox KnownAs_textBox;
        protected Button NewPerson_button;
        protected Button AdditionalMarried_button;
        protected TextBox PersonID_textBox;
        protected System.ComponentModel.IContainer components = null;

        //****************************************************************************************************************************
        public FPerson(CSql SQL) // only called by derived class CPersonFilter
        {
            m_SQL = SQL;
            InitializeComponent();
            AdditionalMarried_button.Visible = false;
        }
        //****************************************************************************************************************************
        public FPerson(CSql SQL,
                       int  iPersonID,
                       bool bSelectPersonForPhoto)
        {
            m_SQL = SQL;
            m_iOriginalPersonID = iPersonID;
            m_bSelectPersonForPhoto = bSelectPersonForPhoto;
            m_iPersonID = iPersonID;
            InitializeComponent();
            AdditionalSpouse_button.Visible = false;
            AdditionalMarried_button.Visible = false;
            int iPersonLevel = Q.i(m_SQL,m_SQL.GetNextPersonLevel());
            this.Location = new System.Drawing.Point(200 + 30 * iPersonLevel, 30 * iPersonLevel);
            InstantiateContextMenus();
            Categories_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(CategorylistBox_Click);
            Properties_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(BuildinglistBox_Click);
            UU.LoadSuffixComboBox(Suffix_comboBox);
            UU.LoadPrefixComboBox(Prefix_comboBox);
            Q.v(m_SQL,m_SQL.DefinePerson(m_PersonDS));
            if (m_iPersonID != 0)
            {
                DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        private void InitializeFieldLengths()
        {
            FirstName_textBox.MaxLength = U.iMaxNameLength;
            MiddleName_textBox.MaxLength = U.iMaxNameLength;
            LastName_textBox.MaxLength = U.iMaxNameLength;
            Suffix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            Prefix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            MarriedName_textBox.MaxLength = U.iMaxNameLength;
            KnownAs_textBox.MaxLength = U.iMaxNameLength;
            Description_TextBox.MaxLength = U.iMaxDescriptionLength;
            Source_textBox.MaxLength = U.iMaxDescriptionLength;
            BornDate_textBox.MaxLength = U.iMaxDateLength;
            BornPlace_textBox.MaxLength = U.iMaxPlaceLength;
            BornHome_textBox.MaxLength = U.iMaxPlaceLength;
            BornSource_textBox.MaxLength = U.iMaxPlaceLength;
            BornBook_textBox.MaxLength = U.iMaxBookPageLength;
            BornPage_textBox.MaxLength = U.iMaxBookPageLength;
            DiedDate_textBox.MaxLength = U.iMaxDateLength;
            DiedPlace_textBox.MaxLength = U.iMaxPlaceLength;
            DiedHome_textBox.MaxLength = U.iMaxPlaceLength;
            DiedSource_textBox.MaxLength = U.iMaxPlaceLength;
            DiedBook_textBox.MaxLength = U.iMaxBookPageLength;
            DiedPage_textBox.MaxLength = U.iMaxBookPageLength;
            BuriedDate_textBox.MaxLength = U.iMaxPlaceLength;
            BuriedPlace_textBox.MaxLength = U.iMaxPlaceLength;
            BuriedStone_textBox.Text.ToString();
            BuriedSource_textBox.MaxLength = U.iMaxPlaceLength;
            BuriedBook_textBox.MaxLength = U.iMaxBookPageLength;
            BuriedPage_textBox.MaxLength = U.iMaxBookPageLength;
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
        private void InstantiateContextMenus()
        {
            this.Father_textBox.ContextMenu = new ContextMenu();
            this.Mother_textBox.ContextMenu = new ContextMenu();
            this.Spouse_textBox.ContextMenu = new ContextMenu();
            Spouse_textBox.ContextMenu.MenuItems.Add("Add Spouse", new EventHandler(NewSpouse_Click));
            Spouse_textBox.ContextMenu.MenuItems.Add("Remove Spouse", new EventHandler(SpouseRemove_Clicked));
            Father_textBox.ContextMenu.MenuItems.Add("Select Father", new EventHandler(Father_Click));
            Father_textBox.ContextMenu.MenuItems.Add("Remove Father", new EventHandler(FatherRemove_Clicked));
            Mother_textBox.ContextMenu.MenuItems.Add("Select Mother", new EventHandler(Mother_Click));
            Mother_textBox.ContextMenu.MenuItems.Add("Remove Mother", new EventHandler(MotherRemove_Clicked));
            Spouse_textBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(NewSpouse_Click);
            Father_textBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Father_Click);
            Mother_textBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Mother_Click);
        }
        //****************************************************************************************************************************
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.LastName_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FirstName_textBox = new System.Windows.Forms.TextBox();
            this.MiddleName_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SearchAll_button = new System.Windows.Forms.Button();
            this.Description_TextBox = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ViewPhotographs_button = new System.Windows.Forms.Button();
            this.BornDate_textBox = new System.Windows.Forms.TextBox();
            this.BornPlace_textBox = new System.Windows.Forms.TextBox();
            this.DiedDate_textBox = new System.Windows.Forms.TextBox();
            this.DiedPlace_textBox = new System.Windows.Forms.TextBox();
            this.Male_radioButton = new System.Windows.Forms.RadioButton();
            this.BornVerified_checkBox = new System.Windows.Forms.CheckBox();
            this.DiedVerified_checkBox = new System.Windows.Forms.CheckBox();
            this.Female_radioButton = new System.Windows.Forms.RadioButton();
            this.Suffix_comboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.DiedPlace_label = new System.Windows.Forms.Label();
            this.Sex_groupBox = new System.Windows.Forms.GroupBox();
            this.BornSource_textBox = new System.Windows.Forms.TextBox();
            this.Born_groupBox = new System.Windows.Forms.GroupBox();
            this.label25 = new System.Windows.Forms.Label();
            this.BornHome_textBox = new System.Windows.Forms.TextBox();
            this.BornPage_textBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.BornBook_textBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Died_groupBox = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.DiedHome_textBox = new System.Windows.Forms.TextBox();
            this.DiedPage_textBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.DiedBook_textBox = new System.Windows.Forms.TextBox();
            this.DiedPage_label = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.DiedSource_textBox = new System.Windows.Forms.TextBox();
            this.Prefix_comboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Save_button = new System.Windows.Forms.Button();
            this.SimilarNames_checkBox = new System.Windows.Forms.CheckBox();
            this.NameOptions_groupBox = new System.Windows.Forms.GroupBox();
            this.Buried_groupBox = new System.Windows.Forms.GroupBox();
            this.BuriedVerified_checkBox = new System.Windows.Forms.CheckBox();
            this.label29 = new System.Windows.Forms.Label();
            this.BuriedSource_textBox = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.BuriedPage_textBox = new System.Windows.Forms.TextBox();
            this.BuriedBook_textBox = new System.Windows.Forms.TextBox();
            this.BuriedDate_textBox = new System.Windows.Forms.TextBox();
            this.BuriedStone_textBox = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.BuriedPlace_textBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.MarriedName_textBox = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.Family_button = new System.Windows.Forms.Button();
            this.UniquePhotos_button = new System.Windows.Forms.Button();
            this.AdditionalSpouse_button = new System.Windows.Forms.Button();
            this.SearchPartial_Button = new System.Windows.Forms.Button();
            this.StartingWith_button = new System.Windows.Forms.Button();
            this.Similar_button = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.KnownAs_textBox = new System.Windows.Forms.TextBox();
            this.VitalRecord_button = new System.Windows.Forms.Button();
            this.Source_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.NewPerson_button = new System.Windows.Forms.Button();
            this.AdditionalMarried_button = new System.Windows.Forms.Button();
            this.PersonID_textBox = new System.Windows.Forms.TextBox();
            this.Properties_listBox = new Utilities.ListBoxWithDoubleClick();
            this.Spouse_textBox = new Utilities.TextBoxWithDoubleClick();
            this.Mother_textBox = new Utilities.TextBoxWithDoubleClick();
            this.Father_textBox = new Utilities.TextBoxWithDoubleClick();
            this.Categories_listBox = new Utilities.ListBoxWithDoubleClick();
            this.Sex_groupBox.SuspendLayout();
            this.Born_groupBox.SuspendLayout();
            this.Died_groupBox.SuspendLayout();
            this.NameOptions_groupBox.SuspendLayout();
            this.Buried_groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LastName_textBox
            // 
            this.LastName_textBox.Location = new System.Drawing.Point(6, 25);
            this.LastName_textBox.Name = "LastName_textBox";
            this.LastName_textBox.Size = new System.Drawing.Size(140, 20);
            this.LastName_textBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Last/Maiden Name";
            // 
            // FirstName_textBox
            // 
            this.FirstName_textBox.Location = new System.Drawing.Point(6, 54);
            this.FirstName_textBox.Name = "FirstName_textBox";
            this.FirstName_textBox.Size = new System.Drawing.Size(140, 20);
            this.FirstName_textBox.TabIndex = 2;
            // 
            // MiddleName_textBox
            // 
            this.MiddleName_textBox.Location = new System.Drawing.Point(6, 83);
            this.MiddleName_textBox.Name = "MiddleName_textBox";
            this.MiddleName_textBox.Size = new System.Drawing.Size(140, 20);
            this.MiddleName_textBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Middle Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "First Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Suffix/Prefix";
            // 
            // SearchAll_button
            // 
            this.SearchAll_button.Location = new System.Drawing.Point(176, 390);
            this.SearchAll_button.Name = "SearchAll_button";
            this.SearchAll_button.Size = new System.Drawing.Size(117, 23);
            this.SearchAll_button.TabIndex = 37;
            this.SearchAll_button.TabStop = false;
            this.SearchAll_button.Text = "Search All Persons";
            this.SearchAll_button.UseVisualStyleBackColor = true;
            this.SearchAll_button.Click += new System.EventHandler(this.SearchAll_button_Click);
            // 
            // Description_TextBox
            // 
            this.Description_TextBox.Location = new System.Drawing.Point(453, 411);
            this.Description_TextBox.Name = "Description_TextBox";
            this.Description_TextBox.Size = new System.Drawing.Size(318, 92);
            this.Description_TextBox.TabIndex = 35;
            this.Description_TextBox.TabStop = false;
            this.Description_TextBox.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(455, 395);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Notes";
            // 
            // ViewPhotographs_button
            // 
            this.ViewPhotographs_button.Location = new System.Drawing.Point(315, 390);
            this.ViewPhotographs_button.Name = "ViewPhotographs_button";
            this.ViewPhotographs_button.Size = new System.Drawing.Size(117, 23);
            this.ViewPhotographs_button.TabIndex = 38;
            this.ViewPhotographs_button.TabStop = false;
            this.ViewPhotographs_button.Text = "View Photographs";
            this.ViewPhotographs_button.UseVisualStyleBackColor = true;
            this.ViewPhotographs_button.Click += new System.EventHandler(this.ViewPhotographs_Click);
            // 
            // BornDate_textBox
            // 
            this.BornDate_textBox.Location = new System.Drawing.Point(62, 25);
            this.BornDate_textBox.Name = "BornDate_textBox";
            this.BornDate_textBox.Size = new System.Drawing.Size(85, 20);
            this.BornDate_textBox.TabIndex = 13;
            // 
            // BornPlace_textBox
            // 
            this.BornPlace_textBox.Location = new System.Drawing.Point(62, 54);
            this.BornPlace_textBox.Name = "BornPlace_textBox";
            this.BornPlace_textBox.Size = new System.Drawing.Size(86, 20);
            this.BornPlace_textBox.TabIndex = 14;
            // 
            // DiedDate_textBox
            // 
            this.DiedDate_textBox.Location = new System.Drawing.Point(62, 25);
            this.DiedDate_textBox.Name = "DiedDate_textBox";
            this.DiedDate_textBox.Size = new System.Drawing.Size(85, 20);
            this.DiedDate_textBox.TabIndex = 20;
            // 
            // DiedPlace_textBox
            // 
            this.DiedPlace_textBox.Location = new System.Drawing.Point(62, 54);
            this.DiedPlace_textBox.Name = "DiedPlace_textBox";
            this.DiedPlace_textBox.Size = new System.Drawing.Size(85, 20);
            this.DiedPlace_textBox.TabIndex = 21;
            // 
            // Male_radioButton
            // 
            this.Male_radioButton.AutoSize = true;
            this.Male_radioButton.Location = new System.Drawing.Point(22, 15);
            this.Male_radioButton.Name = "Male_radioButton";
            this.Male_radioButton.Size = new System.Drawing.Size(48, 17);
            this.Male_radioButton.TabIndex = 7;
            this.Male_radioButton.TabStop = true;
            this.Male_radioButton.Text = "Male";
            this.Male_radioButton.UseVisualStyleBackColor = true;
            // 
            // BornVerified_checkBox
            // 
            this.BornVerified_checkBox.AutoSize = true;
            this.BornVerified_checkBox.Location = new System.Drawing.Point(87, 115);
            this.BornVerified_checkBox.Name = "BornVerified_checkBox";
            this.BornVerified_checkBox.Size = new System.Drawing.Size(61, 17);
            this.BornVerified_checkBox.TabIndex = 16;
            this.BornVerified_checkBox.Text = "Verified";
            this.BornVerified_checkBox.UseVisualStyleBackColor = true;
            // 
            // DiedVerified_checkBox
            // 
            this.DiedVerified_checkBox.AutoSize = true;
            this.DiedVerified_checkBox.Location = new System.Drawing.Point(86, 114);
            this.DiedVerified_checkBox.Name = "DiedVerified_checkBox";
            this.DiedVerified_checkBox.Size = new System.Drawing.Size(61, 17);
            this.DiedVerified_checkBox.TabIndex = 23;
            this.DiedVerified_checkBox.Text = "Verified";
            this.DiedVerified_checkBox.UseVisualStyleBackColor = true;
            // 
            // Female_radioButton
            // 
            this.Female_radioButton.AutoSize = true;
            this.Female_radioButton.Location = new System.Drawing.Point(86, 15);
            this.Female_radioButton.Name = "Female_radioButton";
            this.Female_radioButton.Size = new System.Drawing.Size(59, 17);
            this.Female_radioButton.TabIndex = 8;
            this.Female_radioButton.TabStop = true;
            this.Female_radioButton.Text = "Female";
            this.Female_radioButton.UseVisualStyleBackColor = true;
            // 
            // Suffix_comboBox
            // 
            this.Suffix_comboBox.FormattingEnabled = true;
            this.Suffix_comboBox.Location = new System.Drawing.Point(6, 141);
            this.Suffix_comboBox.Name = "Suffix_comboBox";
            this.Suffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.Suffix_comboBox.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Date";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Place";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(2, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Date";
            // 
            // DiedPlace_label
            // 
            this.DiedPlace_label.AutoSize = true;
            this.DiedPlace_label.Location = new System.Drawing.Point(2, 57);
            this.DiedPlace_label.Name = "DiedPlace_label";
            this.DiedPlace_label.Size = new System.Drawing.Size(34, 13);
            this.DiedPlace_label.TabIndex = 31;
            this.DiedPlace_label.Text = "Place";
            // 
            // Sex_groupBox
            // 
            this.Sex_groupBox.Controls.Add(this.Male_radioButton);
            this.Sex_groupBox.Controls.Add(this.Female_radioButton);
            this.Sex_groupBox.Location = new System.Drawing.Point(124, 237);
            this.Sex_groupBox.Name = "Sex_groupBox";
            this.Sex_groupBox.Size = new System.Drawing.Size(154, 40);
            this.Sex_groupBox.TabIndex = 5;
            this.Sex_groupBox.TabStop = false;
            this.Sex_groupBox.Text = "Sex";
            // 
            // BornSource_textBox
            // 
            this.BornSource_textBox.Location = new System.Drawing.Point(6, 141);
            this.BornSource_textBox.Name = "BornSource_textBox";
            this.BornSource_textBox.Size = new System.Drawing.Size(140, 20);
            this.BornSource_textBox.TabIndex = 17;
            // 
            // Born_groupBox
            // 
            this.Born_groupBox.Controls.Add(this.label25);
            this.Born_groupBox.Controls.Add(this.BornHome_textBox);
            this.Born_groupBox.Controls.Add(this.BornPage_textBox);
            this.Born_groupBox.Controls.Add(this.label15);
            this.Born_groupBox.Controls.Add(this.BornBook_textBox);
            this.Born_groupBox.Controls.Add(this.label14);
            this.Born_groupBox.Controls.Add(this.label12);
            this.Born_groupBox.Controls.Add(this.BornSource_textBox);
            this.Born_groupBox.Controls.Add(this.BornVerified_checkBox);
            this.Born_groupBox.Controls.Add(this.BornDate_textBox);
            this.Born_groupBox.Controls.Add(this.BornPlace_textBox);
            this.Born_groupBox.Controls.Add(this.label8);
            this.Born_groupBox.Controls.Add(this.label9);
            this.Born_groupBox.Location = new System.Drawing.Point(288, 27);
            this.Born_groupBox.Name = "Born_groupBox";
            this.Born_groupBox.Size = new System.Drawing.Size(154, 204);
            this.Born_groupBox.TabIndex = 2;
            this.Born_groupBox.TabStop = false;
            this.Born_groupBox.Text = "Born";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(2, 83);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(58, 13);
            this.label25.TabIndex = 44;
            this.label25.Text = "Residence";
            // 
            // BornHome_textBox
            // 
            this.BornHome_textBox.Location = new System.Drawing.Point(62, 83);
            this.BornHome_textBox.Name = "BornHome_textBox";
            this.BornHome_textBox.Size = new System.Drawing.Size(85, 20);
            this.BornHome_textBox.TabIndex = 15;
            // 
            // BornPage_textBox
            // 
            this.BornPage_textBox.Location = new System.Drawing.Point(116, 170);
            this.BornPage_textBox.Name = "BornPage_textBox";
            this.BornPage_textBox.Size = new System.Drawing.Size(32, 20);
            this.BornPage_textBox.TabIndex = 19;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(76, 173);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 42;
            this.label15.Text = "Page";
            // 
            // BornBook_textBox
            // 
            this.BornBook_textBox.Location = new System.Drawing.Point(40, 170);
            this.BornBook_textBox.Name = "BornBook_textBox";
            this.BornBook_textBox.Size = new System.Drawing.Size(26, 20);
            this.BornBook_textBox.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(2, 173);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(32, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "Book";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 115);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "Source";
            // 
            // Died_groupBox
            // 
            this.Died_groupBox.Controls.Add(this.label26);
            this.Died_groupBox.Controls.Add(this.DiedHome_textBox);
            this.Died_groupBox.Controls.Add(this.DiedPage_textBox);
            this.Died_groupBox.Controls.Add(this.label11);
            this.Died_groupBox.Controls.Add(this.DiedBook_textBox);
            this.Died_groupBox.Controls.Add(this.DiedPage_label);
            this.Died_groupBox.Controls.Add(this.label13);
            this.Died_groupBox.Controls.Add(this.DiedSource_textBox);
            this.Died_groupBox.Controls.Add(this.DiedVerified_checkBox);
            this.Died_groupBox.Controls.Add(this.DiedPlace_textBox);
            this.Died_groupBox.Controls.Add(this.DiedDate_textBox);
            this.Died_groupBox.Controls.Add(this.DiedPlace_label);
            this.Died_groupBox.Controls.Add(this.label10);
            this.Died_groupBox.Location = new System.Drawing.Point(453, 27);
            this.Died_groupBox.Name = "Died_groupBox";
            this.Died_groupBox.Size = new System.Drawing.Size(154, 204);
            this.Died_groupBox.TabIndex = 3;
            this.Died_groupBox.TabStop = false;
            this.Died_groupBox.Text = "Died";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(2, 83);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(58, 13);
            this.label26.TabIndex = 45;
            this.label26.Text = "Residence";
            // 
            // DiedHome_textBox
            // 
            this.DiedHome_textBox.Location = new System.Drawing.Point(62, 83);
            this.DiedHome_textBox.Name = "DiedHome_textBox";
            this.DiedHome_textBox.Size = new System.Drawing.Size(85, 20);
            this.DiedHome_textBox.TabIndex = 22;
            // 
            // DiedPage_textBox
            // 
            this.DiedPage_textBox.Location = new System.Drawing.Point(115, 170);
            this.DiedPage_textBox.Name = "DiedPage_textBox";
            this.DiedPage_textBox.Size = new System.Drawing.Size(32, 20);
            this.DiedPage_textBox.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(2, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "Book";
            // 
            // DiedBook_textBox
            // 
            this.DiedBook_textBox.Location = new System.Drawing.Point(40, 170);
            this.DiedBook_textBox.Name = "DiedBook_textBox";
            this.DiedBook_textBox.Size = new System.Drawing.Size(26, 20);
            this.DiedBook_textBox.TabIndex = 25;
            // 
            // DiedPage_label
            // 
            this.DiedPage_label.AutoSize = true;
            this.DiedPage_label.Location = new System.Drawing.Point(76, 173);
            this.DiedPage_label.Name = "DiedPage_label";
            this.DiedPage_label.Size = new System.Drawing.Size(32, 13);
            this.DiedPage_label.TabIndex = 37;
            this.DiedPage_label.Text = "Page";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 115);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "Source";
            // 
            // DiedSource_textBox
            // 
            this.DiedSource_textBox.Location = new System.Drawing.Point(6, 141);
            this.DiedSource_textBox.Name = "DiedSource_textBox";
            this.DiedSource_textBox.Size = new System.Drawing.Size(140, 20);
            this.DiedSource_textBox.TabIndex = 24;
            // 
            // Prefix_comboBox
            // 
            this.Prefix_comboBox.FormattingEnabled = true;
            this.Prefix_comboBox.Location = new System.Drawing.Point(86, 141);
            this.Prefix_comboBox.Name = "Prefix_comboBox";
            this.Prefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.Prefix_comboBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "Known As";
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(31, 390);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(117, 23);
            this.Save_button.TabIndex = 36;
            this.Save_button.TabStop = false;
            this.Save_button.Text = "Save";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // SimilarNames_checkBox
            // 
            this.SimilarNames_checkBox.AutoSize = true;
            this.SimilarNames_checkBox.Checked = true;
            this.SimilarNames_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SimilarNames_checkBox.Location = new System.Drawing.Point(6, 23);
            this.SimilarNames_checkBox.Name = "SimilarNames_checkBox";
            this.SimilarNames_checkBox.Size = new System.Drawing.Size(126, 17);
            this.SimilarNames_checkBox.TabIndex = 39;
            this.SimilarNames_checkBox.TabStop = false;
            this.SimilarNames_checkBox.Text = "Check Similar Names";
            this.SimilarNames_checkBox.UseVisualStyleBackColor = true;
            // 
            // NameOptions_groupBox
            // 
            this.NameOptions_groupBox.Controls.Add(this.SimilarNames_checkBox);
            this.NameOptions_groupBox.Location = new System.Drawing.Point(21, 450);
            this.NameOptions_groupBox.Name = "NameOptions_groupBox";
            this.NameOptions_groupBox.Size = new System.Drawing.Size(140, 66);
            this.NameOptions_groupBox.TabIndex = 41;
            this.NameOptions_groupBox.TabStop = false;
            this.NameOptions_groupBox.Text = "Save Option";
            // 
            // Buried_groupBox
            // 
            this.Buried_groupBox.Controls.Add(this.BuriedVerified_checkBox);
            this.Buried_groupBox.Controls.Add(this.label29);
            this.Buried_groupBox.Controls.Add(this.BuriedSource_textBox);
            this.Buried_groupBox.Controls.Add(this.label28);
            this.Buried_groupBox.Controls.Add(this.label27);
            this.Buried_groupBox.Controls.Add(this.BuriedPage_textBox);
            this.Buried_groupBox.Controls.Add(this.BuriedBook_textBox);
            this.Buried_groupBox.Controls.Add(this.BuriedDate_textBox);
            this.Buried_groupBox.Controls.Add(this.BuriedStone_textBox);
            this.Buried_groupBox.Controls.Add(this.label22);
            this.Buried_groupBox.Controls.Add(this.BuriedPlace_textBox);
            this.Buried_groupBox.Controls.Add(this.label17);
            this.Buried_groupBox.Controls.Add(this.label16);
            this.Buried_groupBox.Location = new System.Drawing.Point(616, 27);
            this.Buried_groupBox.Name = "Buried_groupBox";
            this.Buried_groupBox.Size = new System.Drawing.Size(154, 204);
            this.Buried_groupBox.TabIndex = 4;
            this.Buried_groupBox.TabStop = false;
            this.Buried_groupBox.Text = "Buried";
            // 
            // BuriedVerified_checkBox
            // 
            this.BuriedVerified_checkBox.AutoSize = true;
            this.BuriedVerified_checkBox.Location = new System.Drawing.Point(84, 115);
            this.BuriedVerified_checkBox.Name = "BuriedVerified_checkBox";
            this.BuriedVerified_checkBox.Size = new System.Drawing.Size(61, 17);
            this.BuriedVerified_checkBox.TabIndex = 31;
            this.BuriedVerified_checkBox.Text = "Verified";
            this.BuriedVerified_checkBox.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(2, 115);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(41, 13);
            this.label29.TabIndex = 51;
            this.label29.Text = "Source";
            // 
            // BuriedSource_textBox
            // 
            this.BuriedSource_textBox.Location = new System.Drawing.Point(6, 141);
            this.BuriedSource_textBox.Name = "BuriedSource_textBox";
            this.BuriedSource_textBox.Size = new System.Drawing.Size(140, 20);
            this.BuriedSource_textBox.TabIndex = 32;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(76, 173);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(32, 13);
            this.label28.TabIndex = 46;
            this.label28.Text = "Page";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(2, 173);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(32, 13);
            this.label27.TabIndex = 46;
            this.label27.Text = "Book";
            // 
            // BuriedPage_textBox
            // 
            this.BuriedPage_textBox.Location = new System.Drawing.Point(116, 170);
            this.BuriedPage_textBox.Name = "BuriedPage_textBox";
            this.BuriedPage_textBox.Size = new System.Drawing.Size(32, 20);
            this.BuriedPage_textBox.TabIndex = 34;
            // 
            // BuriedBook_textBox
            // 
            this.BuriedBook_textBox.Location = new System.Drawing.Point(40, 170);
            this.BuriedBook_textBox.Name = "BuriedBook_textBox";
            this.BuriedBook_textBox.Size = new System.Drawing.Size(26, 20);
            this.BuriedBook_textBox.TabIndex = 33;
            // 
            // BuriedDate_textBox
            // 
            this.BuriedDate_textBox.Location = new System.Drawing.Point(62, 25);
            this.BuriedDate_textBox.Name = "BuriedDate_textBox";
            this.BuriedDate_textBox.Size = new System.Drawing.Size(85, 20);
            this.BuriedDate_textBox.TabIndex = 27;
            // 
            // BuriedStone_textBox
            // 
            this.BuriedStone_textBox.Location = new System.Drawing.Point(62, 83);
            this.BuriedStone_textBox.Name = "BuriedStone_textBox";
            this.BuriedStone_textBox.Size = new System.Drawing.Size(83, 20);
            this.BuriedStone_textBox.TabIndex = 30;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(2, 87);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(62, 13);
            this.label22.TabIndex = 46;
            this.label22.Text = "Lot Number";
            // 
            // BuriedPlace_textBox
            // 
            this.BuriedPlace_textBox.BackColor = System.Drawing.Color.White;
            this.BuriedPlace_textBox.Location = new System.Drawing.Point(62, 54);
            this.BuriedPlace_textBox.Name = "BuriedPlace_textBox";
            this.BuriedPlace_textBox.Size = new System.Drawing.Size(85, 20);
            this.BuriedPlace_textBox.TabIndex = 28;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(2, 32);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 44;
            this.label17.Text = "Date";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(2, 57);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 41;
            this.label16.Text = "Place";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(20, 314);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(37, 13);
            this.label18.TabIndex = 43;
            this.label18.Text = "Father";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(20, 342);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(40, 13);
            this.label19.TabIndex = 44;
            this.label19.Text = "Mother";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(455, 238);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(57, 13);
            this.label20.TabIndex = 47;
            this.label20.Text = "Categories";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(20, 286);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 13);
            this.label23.TabIndex = 49;
            this.label23.Text = "Spouse";
            // 
            // MarriedName_textBox
            // 
            this.MarriedName_textBox.Location = new System.Drawing.Point(6, 112);
            this.MarriedName_textBox.Name = "MarriedName_textBox";
            this.MarriedName_textBox.Size = new System.Drawing.Size(140, 20);
            this.MarriedName_textBox.TabIndex = 4;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(20, 143);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(73, 13);
            this.label24.TabIndex = 51;
            this.label24.Text = "Married Name";
            // 
            // Family_button
            // 
            this.Family_button.Location = new System.Drawing.Point(315, 420);
            this.Family_button.Name = "Family_button";
            this.Family_button.Size = new System.Drawing.Size(117, 23);
            this.Family_button.TabIndex = 45;
            this.Family_button.TabStop = false;
            this.Family_button.Text = "Family";
            this.Family_button.UseVisualStyleBackColor = true;
            this.Family_button.Click += new System.EventHandler(this.Family_button_Click);
            // 
            // UniquePhotos_button
            // 
            this.UniquePhotos_button.Location = new System.Drawing.Point(315, 480);
            this.UniquePhotos_button.Name = "UniquePhotos_button";
            this.UniquePhotos_button.Size = new System.Drawing.Size(117, 23);
            this.UniquePhotos_button.TabIndex = 46;
            this.UniquePhotos_button.TabStop = false;
            this.UniquePhotos_button.Text = "Add Unique Photo";
            this.UniquePhotos_button.UseVisualStyleBackColor = true;
            this.UniquePhotos_button.Click += new System.EventHandler(this.UniquePhotos_button_Click);
            // 
            // AdditionalSpouse_button
            // 
            this.AdditionalSpouse_button.Location = new System.Drawing.Point(95, 282);
            this.AdditionalSpouse_button.Name = "AdditionalSpouse_button";
            this.AdditionalSpouse_button.Size = new System.Drawing.Size(23, 20);
            this.AdditionalSpouse_button.TabIndex = 54;
            this.AdditionalSpouse_button.TabStop = false;
            this.AdditionalSpouse_button.Text = "...";
            this.AdditionalSpouse_button.UseVisualStyleBackColor = true;
            this.AdditionalSpouse_button.Click += new System.EventHandler(this.AdditionalSpouse_Click);
            // 
            // SearchPartial_Button
            // 
            this.SearchPartial_Button.Location = new System.Drawing.Point(176, 420);
            this.SearchPartial_Button.Name = "SearchPartial_Button";
            this.SearchPartial_Button.Size = new System.Drawing.Size(117, 23);
            this.SearchPartial_Button.TabIndex = 55;
            this.SearchPartial_Button.TabStop = false;
            this.SearchPartial_Button.Text = "Partial Name";
            this.SearchPartial_Button.UseVisualStyleBackColor = true;
            this.SearchPartial_Button.Click += new System.EventHandler(this.SearchPartial_button_Click);
            // 
            // StartingWith_button
            // 
            this.StartingWith_button.Location = new System.Drawing.Point(176, 450);
            this.StartingWith_button.Name = "StartingWith_button";
            this.StartingWith_button.Size = new System.Drawing.Size(117, 23);
            this.StartingWith_button.TabIndex = 56;
            this.StartingWith_button.TabStop = false;
            this.StartingWith_button.Text = "Last Starting With";
            this.StartingWith_button.UseVisualStyleBackColor = true;
            this.StartingWith_button.Click += new System.EventHandler(this.SearchStartingWith_button_Click);
            // 
            // Similar_button
            // 
            this.Similar_button.Location = new System.Drawing.Point(176, 480);
            this.Similar_button.Name = "Similar_button";
            this.Similar_button.Size = new System.Drawing.Size(117, 23);
            this.Similar_button.TabIndex = 57;
            this.Similar_button.TabStop = false;
            this.Similar_button.Text = "Similar Names";
            this.Similar_button.UseVisualStyleBackColor = true;
            this.Similar_button.Click += new System.EventHandler(this.SearchSimilar_button_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(455, 315);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(104, 13);
            this.label30.TabIndex = 59;
            this.label30.Text = "Associated Buildings";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.KnownAs_textBox);
            this.groupBox1.Controls.Add(this.MarriedName_textBox);
            this.groupBox1.Controls.Add(this.Prefix_comboBox);
            this.groupBox1.Controls.Add(this.Suffix_comboBox);
            this.groupBox1.Controls.Add(this.MiddleName_textBox);
            this.groupBox1.Controls.Add(this.FirstName_textBox);
            this.groupBox1.Controls.Add(this.LastName_textBox);
            this.groupBox1.Location = new System.Drawing.Point(124, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 204);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Person";
            // 
            // KnownAs_textBox
            // 
            this.KnownAs_textBox.Location = new System.Drawing.Point(6, 170);
            this.KnownAs_textBox.Name = "KnownAs_textBox";
            this.KnownAs_textBox.Size = new System.Drawing.Size(140, 20);
            this.KnownAs_textBox.TabIndex = 7;
            // 
            // VitalRecord_button
            // 
            this.VitalRecord_button.Location = new System.Drawing.Point(315, 450);
            this.VitalRecord_button.Name = "VitalRecord_button";
            this.VitalRecord_button.Size = new System.Drawing.Size(117, 23);
            this.VitalRecord_button.TabIndex = 61;
            this.VitalRecord_button.TabStop = false;
            this.VitalRecord_button.Text = "Vital Records";
            this.VitalRecord_button.UseVisualStyleBackColor = true;
            this.VitalRecord_button.Click += new System.EventHandler(this.VitalRecord_button_Click);
            // 
            // Source_textBox
            // 
            this.Source_textBox.Location = new System.Drawing.Point(294, 255);
            this.Source_textBox.Name = "Source_textBox";
            this.Source_textBox.Size = new System.Drawing.Size(140, 20);
            this.Source_textBox.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(295, 238);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "Source";
            // 
            // NewPerson_button
            // 
            this.NewPerson_button.Location = new System.Drawing.Point(31, 420);
            this.NewPerson_button.Name = "NewPerson_button";
            this.NewPerson_button.Size = new System.Drawing.Size(117, 23);
            this.NewPerson_button.TabIndex = 62;
            this.NewPerson_button.TabStop = false;
            this.NewPerson_button.Text = "New Person";
            this.NewPerson_button.UseVisualStyleBackColor = true;
            this.NewPerson_button.Click += new System.EventHandler(this.NewPerson_click);
            // 
            // AdditionalMarried_button
            // 
            this.AdditionalMarried_button.Location = new System.Drawing.Point(95, 140);
            this.AdditionalMarried_button.Name = "AdditionalMarried_button";
            this.AdditionalMarried_button.Size = new System.Drawing.Size(23, 20);
            this.AdditionalMarried_button.TabIndex = 63;
            this.AdditionalMarried_button.TabStop = false;
            this.AdditionalMarried_button.Text = "...";
            this.AdditionalMarried_button.UseVisualStyleBackColor = true;
            // 
            // PersonID_textBox
            // 
            this.PersonID_textBox.Location = new System.Drawing.Point(51, 27);
            this.PersonID_textBox.Name = "PersonID_textBox";
            this.PersonID_textBox.Size = new System.Drawing.Size(26, 20);
            this.PersonID_textBox.TabIndex = 46;
            this.PersonID_textBox.Visible = false;
            // 
            // Properties_listBox
            // 
            this.Properties_listBox.FormattingEnabled = true;
            this.Properties_listBox.Location = new System.Drawing.Point(453, 333);
            this.Properties_listBox.Name = "Properties_listBox";
            this.Properties_listBox.Size = new System.Drawing.Size(318, 56);
            this.Properties_listBox.TabIndex = 58;
            this.Properties_listBox.TabStop = false;
            this.Properties_listBox.SelectedIndexChanged += new System.EventHandler(this.Properties_listBox_SelectedIndexChanged);
            // 
            // Spouse_textBox
            // 
            this.Spouse_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.Spouse_textBox.Location = new System.Drawing.Point(130, 283);
            this.Spouse_textBox.MaxLength = 0;
            this.Spouse_textBox.Name = "Spouse_textBox";
            this.Spouse_textBox.ReadOnly = true;
            this.Spouse_textBox.Size = new System.Drawing.Size(304, 20);
            this.Spouse_textBox.TabIndex = 9;
            this.Spouse_textBox.TabStop = false;
            // 
            // Mother_textBox
            // 
            this.Mother_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.Mother_textBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Mother_textBox.Location = new System.Drawing.Point(130, 343);
            this.Mother_textBox.MaxLength = 0;
            this.Mother_textBox.Name = "Mother_textBox";
            this.Mother_textBox.ReadOnly = true;
            this.Mother_textBox.Size = new System.Drawing.Size(304, 20);
            this.Mother_textBox.TabIndex = 11;
            this.Mother_textBox.TabStop = false;
            // 
            // Father_textBox
            // 
            this.Father_textBox.BackColor = System.Drawing.SystemColors.Window;
            this.Father_textBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Father_textBox.Location = new System.Drawing.Point(130, 313);
            this.Father_textBox.MaxLength = 0;
            this.Father_textBox.Name = "Father_textBox";
            this.Father_textBox.ReadOnly = true;
            this.Father_textBox.Size = new System.Drawing.Size(304, 20);
            this.Father_textBox.TabIndex = 10;
            this.Father_textBox.TabStop = false;
            // 
            // Categories_listBox
            // 
            this.Categories_listBox.FormattingEnabled = true;
            this.Categories_listBox.Location = new System.Drawing.Point(453, 255);
            this.Categories_listBox.Name = "Categories_listBox";
            this.Categories_listBox.Size = new System.Drawing.Size(317, 56);
            this.Categories_listBox.TabIndex = 36;
            this.Categories_listBox.TabStop = false;
            this.Categories_listBox.SelectedIndexChanged += new System.EventHandler(this.Categories_listBox_SelectedIndexChanged);
            // 
            // FPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(807, 552);
            this.Controls.Add(this.PersonID_textBox);
            this.Controls.Add(this.AdditionalMarried_button);
            this.Controls.Add(this.NewPerson_button);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Source_textBox);
            this.Controls.Add(this.VitalRecord_button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.Properties_listBox);
            this.Controls.Add(this.Sex_groupBox);
            this.Controls.Add(this.Similar_button);
            this.Controls.Add(this.StartingWith_button);
            this.Controls.Add(this.SearchPartial_Button);
            this.Controls.Add(this.AdditionalSpouse_button);
            this.Controls.Add(this.UniquePhotos_button);
            this.Controls.Add(this.Family_button);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.Spouse_textBox);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.Mother_textBox);
            this.Controls.Add(this.Father_textBox);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.Buried_groupBox);
            this.Controls.Add(this.NameOptions_groupBox);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Categories_listBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Died_groupBox);
            this.Controls.Add(this.Born_groupBox);
            this.Controls.Add(this.ViewPhotographs_button);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Description_TextBox);
            this.Controls.Add(this.SearchAll_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(300, 50);
            this.Name = "FPerson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Person";
            this.Sex_groupBox.ResumeLayout(false);
            this.Sex_groupBox.PerformLayout();
            this.Born_groupBox.ResumeLayout(false);
            this.Born_groupBox.PerformLayout();
            this.Died_groupBox.ResumeLayout(false);
            this.Died_groupBox.PerformLayout();
            this.NameOptions_groupBox.ResumeLayout(false);
            this.NameOptions_groupBox.PerformLayout();
            this.Buried_groupBox.ResumeLayout(false);
            this.Buried_groupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        //****************************************************************************************************************************
        #endregion
        //****************************************************************************************************************************
        protected void SetAllNameValues()
        {
            m_FirstName = FirstName_textBox.Text.TrimString();
            m_MiddleName = MiddleName_textBox.Text.TrimString();
            m_LastName = LastName_textBox.Text.TrimString();
            m_Suffix = Suffix_comboBox.Text.TrimString();
            m_Prefix = Prefix_comboBox.Text.TrimString();
            m_MarriedName = MarriedName_textBox.Text.TrimString();
            m_KnownAs = KnownAs_textBox.Text.TrimString();
        }
        //****************************************************************************************************************************
        private bool VitalRecord(DataRow Person_row,
                                 EVitalRecordType eVitalRecordType1,
                                 EVitalRecordType eVitalRecordType2,
                                 TextBox  Book_textBox,
                                 TextBox  Page_textBox,
                                 TextBox  Date_textBox,
                                 TextBox  Place_textBox,
                                 TextBox  Home_textBox,
                                 CheckBox Verified_checkBox,
                                 TextBox  Source_textBox)
        {
            string sBook = "";
            string sPage = "";
            string sDate = "";
            string sPlace = "";
            string sHome = "";
            string sSource = "";
            eSex Sex = eSex.eUnknown;
            Verified_checkBox.Checked = U.GetPersonVitalStatistics(m_PersonDS.Tables[U.VitalRecord_Table], Person_row, eVitalRecordType1, eVitalRecordType2,
                                        ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource, ref Sex);
            Book_textBox.Text = sBook;
            Page_textBox.Text = sPage;
            Date_textBox.Text = sDate;
            Place_textBox.Text = sPlace;
            Home_textBox.Text = sHome;
            Source_textBox.Text = sSource;
            if (Sex == eSex.eMale)
            {
                Male_radioButton.Checked = true;
                Female_radioButton.Checked = false;
            }
            else if (Sex == eSex.eFemale)
            {
                Male_radioButton.Checked = false;
                Female_radioButton.Checked = true;
            }
            return Verified_checkBox.Checked;
        }
        //****************************************************************************************************************************
        private void LoadPersonIntoControls(DataRow Person_row)
        {
            PersonID_textBox.Visible = true;
            PersonID_textBox.Enabled = false;
            PersonID_textBox.Text = m_iPersonID.ToString();
            FirstName_textBox.Text = Q.s(m_SQL,m_SQL.PersonFirstName(Person_row));
            MiddleName_textBox.Text = Q.s(m_SQL,m_SQL.PersonMiddleName(Person_row));
            LastName_textBox.Text = Q.s(m_SQL,m_SQL.PersonLastName(Person_row));
            Suffix_comboBox.Text = Q.s(m_SQL,m_SQL.PersonSuffix(Person_row));
            Prefix_comboBox.Text = Q.s(m_SQL,m_SQL.PersonPrefix(Person_row));
            MarriedName_textBox.Text = Q.s(m_SQL,m_SQL.PersonMarriedName(Person_row));
            KnownAs_textBox.Text = Q.s(m_SQL,m_SQL.PersonKnownAs(Person_row));
            string sSex = Person_row[U.Sex_col].ToString();
            Male_radioButton.Checked = false;
            Female_radioButton.Checked = false;
            if (sSex.Length != 0)
            {
                if (sSex[0] == 'M')
                {
                    Male_radioButton.Checked = true;
                }
                else if (sSex[0] == 'F')
                {
                    Female_radioButton.Checked = true;
                }
            }
            m_bVitalBornFound = VitalRecord(Person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale, BornBook_textBox, BornPage_textBox,
                                            BornDate_textBox, BornPlace_textBox, BornHome_textBox, BornVerified_checkBox, BornSource_textBox);
            m_bVitalDiedFound = VitalRecord(Person_row, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale, DiedBook_textBox, DiedPage_textBox,
                                            DiedDate_textBox, DiedPlace_textBox, DiedHome_textBox, DiedVerified_checkBox, DiedSource_textBox);
            m_bVitalBuriedFound = VitalRecord(Person_row,EVitalRecordType.eBurial, EVitalRecordType.eSearch, BuriedBook_textBox, BuriedPage_textBox,
                                            BuriedDate_textBox, BuriedPlace_textBox, BuriedStone_textBox, BuriedVerified_checkBox, BuriedSource_textBox);
            Description_TextBox.Text = Q.s(m_SQL,m_SQL.PersonDescription(Person_row));
            Source_textBox.Text = Q.s(m_SQL,m_SQL.PersonSource(Person_row));
            m_iFatherID = Q.i(m_SQL,m_SQL.PersonFatherID(Person_row));
            m_iMotherID = Q.i(m_SQL,m_SQL.PersonMotherID(Person_row));
            GetMarriages();
            Father_textBox.Text = Q.s(m_SQL,m_SQL.GetPersonName(m_iFatherID));
            Mother_textBox.Text = Q.s(m_SQL,m_SQL.GetPersonName(m_iMotherID));
            SetToUnmodified();
        }
        //****************************************************************************************************************************
        protected void GetMarriages()
        {
            m_iSpouseID = Q.i(m_SQL,m_SQL.GetMarriagesID(m_PersonDS.Tables[U.Marriage_Table], 
                              ref m_iNumberSpouses, out m_iSpouseLocationInArray, m_iPersonID));
            m_iCurrentSpouseIndex = 0;
            DisplaySpouse();
        }
        //****************************************************************************************************************************
        protected void DisplaySpouse()
        {
            if (m_iNumberSpouses == 0)
                Spouse_textBox.Text = "";
            else
            {
                DataViewRowState dvrs = DataViewRowState.Added | DataViewRowState.Unchanged;
                DataRow[] rows = m_PersonDS.Tables[U.Marriage_Table].Select("", "", dvrs);
                DataRow row = rows[m_iCurrentSpouseIndex];
                m_iSpouseID = row[m_iSpouseLocationInArray].ToInt();
                bool bModified = Spouse_textBox.Modified;
                Spouse_textBox.Text = Q.s(m_SQL,m_SQL.GetPersonName(m_iSpouseID));
                Spouse_textBox.Modified = bModified;
                if (rows.Length > 1)
                    AdditionalSpouse_button.Visible = true;
                else
                    AdditionalSpouse_button.Visible = false;
            }
        }
        //****************************************************************************************************************************
        public void LoadBuilding_listBox(DataTable BuildingTBL,
                                         ListBoxWithDoubleClick Categories_listBox,
                                         string sTableName,
                                         string sTableId,
                                         int iTableID)
        {
            Categories_listBox.Items.Clear();
            foreach (DataRow row in BuildingTBL.Rows)
            {
                int iBuildingID = row[U.BuildingID_col].ToInt();
//                  string sBuildingName = Q.s(m_SQL,m_SQL.GetBuildingName(iBuildingID));
                DataTable BuildingTable = Q.t(m_SQL, m_SQL.GetBuilding(iBuildingID));
                if (BuildingTable.Rows.Count != 0)
                {
                    DataRow BuildingRow = BuildingTable.Rows[0];
                    string sBuildingName = Q.s(m_SQL,m_SQL.AddRoadToGroup(m_SQL.BuildingName(BuildingRow),
                                               BuildingRow[U.BuildingRoadValueID_col].ToInt(),
                                               BuildingRow[U.BuildingGrandListID_col].ToInt()));
//                  string sBuildingIDValue = row[U.BuildingValueValue_col].ToString();
                    Categories_listBox.Items.Add(sBuildingName);
                }
            }
            DataTable building_tbl = Q.t(m_SQL, m_SQL.GetAllBuildingsAsSpouse(m_iPersonID));
        }
        //****************************************************************************************************************************
        protected void DisplayPerson(int iPersonID)
        {
            m_bDidSearch = false;
            Q.v(m_SQL,m_SQL.GetPerson(ref m_PersonDS,iPersonID));
            if (m_PersonDS.Tables[U.Person_Table].Rows.Count != 0)
            {
                LoadPersonIntoControls(m_PersonDS.Tables[U.Person_Table].Rows[0]);
                SetAllNameValues();
                if (m_SQL.IsFullDatabase())
                {
                    UU.LoadCategory_listBox(m_PersonDS.Tables[U.PersonCategoryValue_Table], Categories_listBox, m_SQL,
                                               U.PersonCategoryValue_Table, U.Person_Table, m_iPersonID);
                    LoadBuilding_listBox(m_PersonDS.Tables[U.PersonBuildingValue_Table], Properties_listBox,
                                               U.PersonBuildingValue_Table, U.Person_Table, m_iPersonID);
                }
            }
        }
        //****************************************************************************************************************************
        public virtual bool AddMode()
        {
            return false;
        }
        //****************************************************************************************************************************
        public string SetPersonSex(bool MaleRadioButtonChecked,
                                   bool FemaleRadioButtonChecked)
        {
            if (MaleRadioButtonChecked)
                return "M";
            else if (FemaleRadioButtonChecked)
                return "F";
            else
                return "";
        }
        //****************************************************************************************************************************
        private void SetToUnmodified()
        {
            m_bCategoryOrBuildingChanged = false;
            m_bDidSearch = false;
            FirstName_textBox.Modified = false;
            MiddleName_textBox.Modified = false;
            LastName_textBox.Modified = false;
            // do not set Modified(TrimString(Suffix_comboBox.Text.ToString()), tbl, "", Q.s(m_SQL,m_SQL.U.Suffix_col)) ||
            //            Modified(TrimString(Prefix_comboBox.Text.ToString()), tbl, "", Q.s(m_SQL,m_SQL.U.Prefix_col)) ||
            MarriedName_textBox.Modified = false;
            KnownAs_textBox.Modified = false;
            Description_TextBox.Modified = false;
            Spouse_textBox.Modified = false;
            Father_textBox.Modified = false;
            Mother_textBox.Modified = false;
            //            Modified(SetPersonSex(Male_radioButton.Checked, Female_radioButton.Checked), tbl, "", Q.s(m_SQL,m_SQL.U.Sex_col)) ||
            BornDate_textBox.Modified = false;
            BornPlace_textBox.Modified = false;
            BornHome_textBox.Modified = false;
            //            Modified(IsChecked(BornVerified_checkBox.Checked), tbl,"N", Q.s(m_SQL,m_SQL.U.BornVerified_col)) ||
            BornSource_textBox.Modified = false;
            BornBook_textBox.Modified = false;
            BornPage_textBox.Modified = false;
            DiedDate_textBox.Modified = false;
            DiedPlace_textBox.Modified = false;
            DiedHome_textBox.Modified = false;
            //            Modified(IsChecked(DiedVerified_checkBox.Checked), tbl, "N", Q.s(m_SQL,m_SQL.U.DiedVerified_col)) ||
            DiedSource_textBox.Modified = false;
            DiedBook_textBox.Modified = false;
            DiedPage_textBox.Modified = false;
            BuriedDate_textBox.Modified = false;
            BuriedPlace_textBox.Modified = false;
            BuriedStone_textBox.Modified = false;
            //            Modified(IsChecked(BuriedVerified_checkBox.Checked), tbl, "N", Q.s(m_SQL,m_SQL.U.BuriedVerified_col)) ||
            BuriedSource_textBox.Modified = false;
            BuriedBook_textBox.Modified = false;
            BuriedPage_textBox.Modified = false;
        }
        //****************************************************************************************************************************
        private int GetPersonFromGrid(DataTable tbl,
                                      string sSortLastName)
        {
            CGridPerson GridDataViewPerson = new CGridPerson(m_SQL, ref tbl, AddMode(), "Person", sSortLastName);
            GridDataViewPerson.ShowDialog();
            int iPersonID = GridDataViewPerson.SelectedPersonID;
            if (iPersonID != 0 && m_bSelectPersonForPhoto)
            {
                m_FirstName = GridDataViewPerson.SelectedFirstName;
                m_MiddleName = GridDataViewPerson.SelectedMiddleName;
                m_LastName = GridDataViewPerson.SelectedLastName;
                m_Suffix = GridDataViewPerson.SelectedSuffix;
                m_Prefix = GridDataViewPerson.SelectedPrefix;
                m_MarriedName = GridDataViewPerson.SelectedMarriedName;
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        protected void SearchForPerson(string sSortLastName)
        {
            m_bDidSearch = true;
            SetAllNameValues();
            DataTable tbl = Q.t(m_SQL,m_SQL.DefinePersonTable());
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PersonID_col] };
            Q.v(m_SQL,m_SQL.PersonsBasedOnNameOptions(tbl, true, U.Person_Table, U.PersonID_col, SearchBy, m_LastName, m_FirstName, m_MiddleName, m_Suffix, m_Prefix, 
                                            m_MarriedName, m_KnownAs));
            m_iPersonID = GetPersonFromGrid(tbl, sSortLastName);
            if (m_iPersonID != 0)
            {
                m_iOriginalPersonID = m_iPersonID;
                if (m_bSelectPersonForPhoto)
                {
                    this.CloseForm();
                    return;
                }
                else
                    DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        protected virtual void SearchAll_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_AllNames;
            SearchForPerson("");
        }
        //****************************************************************************************************************************
        protected virtual void SearchStartingWith_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_StartingWith;
            SearchForPerson(LastName_textBox.Text);
        }
        //****************************************************************************************************************************
        protected virtual void SearchPartial_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(LastName_textBox.Text);
        }
        //****************************************************************************************************************************
        protected virtual void VitalRecord_button_Click(object sender, System.EventArgs e)
        {
            if (SaveBeforeLeaving())
            {
                FVitalRecord VitalRecordBurial = new FVitalRecord(EVitalRecordType.eSearch, m_iPersonID, m_SQL);
                if (!VitalRecordBurial.AbortVitalRecord())
                {
                    VitalRecordBurial.ShowDialog();
                    DisplayPerson(m_iPersonID);
                }
            }
        }
        //****************************************************************************************************************************
        protected virtual void SearchSimilar_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_Similar;
            SearchForPerson(LastName_textBox.Text);
        }
        //****************************************************************************************************************************
        private void ViewPhotographs_Click(object sender, EventArgs e)
        {
            SetAllNameValues();
            FPhotoViewer PhotoViewer;
            if (m_iPersonID == 0)
            {
                return;
            }
            else
            {
                PhotoViewer = new FPhotoViewer(ref m_SQL, m_iPersonID, U.PicturedPerson_Table);
            }
            if (PhotoViewer.PhotoViewerAborted())
                MessageBox.Show("No Photos found");
            else
            if (SaveBeforeLeaving())
            {
                PhotoViewer.ShowDialog();
                DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        public int GetPersonID()
        {
            return m_iPersonID;
        }
        //****************************************************************************************************************************
        public void GetPersonName(ref string sFirstName,
                                  ref string sMiddleName,
                                  ref string sLastName,
                                  ref string sSuffix,
                                  ref string sPrefix)
        {
            sFirstName = m_FirstName;
            sMiddleName = m_MiddleName;
            sLastName = m_LastName;
            sSuffix = m_Suffix;
            sPrefix = m_Prefix;
        }
        //****************************************************************************************************************************
        public void GetPersonName(DataRow row)
        {
            row[U.FirstName_col] = m_FirstName;
            row[U.MiddleName_col] = m_MiddleName;
            row[U.LastName_col] = m_LastName;
            row[U.Suffix_col] = m_Suffix;
            row[U.Prefix_col] = m_Prefix;
        }
        //****************************************************************************************************************************
        public string GetPersonIDandWhereStatement(ref int iPersonID)
        {
            iPersonID = m_iPersonID;
            return m_sPersonWhereStatement;
        }
        //****************************************************************************************************************************
        public void CloseForm()
        {
//            Q.v(m_SQL,m_SQL.resetPersonLevel());
            this.Close();
        }
        //****************************************************************************************************************************
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }
        //****************************************************************************************************************************
        private void BuildinglistBox_Click(object sender, EventArgs e)
        {
            if (m_SQL.IsFullDatabase())
            {
                CListboxGroup GroupListbox = new CListboxGroup(m_SQL, Properties_listBox, U.PersonBuildingValue_Table, U.BuildingValueValue_col);
                GroupListbox.ShowGroupListbox(ref m_PersonDS);
                m_bCategoryOrBuildingChanged = true;
            }
        }
        //****************************************************************************************************************************
        private void CategorylistBox_Click(object sender, EventArgs e)
        {
            if (m_SQL.IsFullDatabase())
            {
                CListboxGroup GroupListbox = new CListboxGroup(m_SQL, Categories_listBox, U.PersonCategoryValue_Table, U.CategoryValueValue_col);
                GroupListbox.ShowGroupListbox(ref m_PersonDS);
                m_bCategoryOrBuildingChanged = true;
            }
        }
        //****************************************************************************************************************************
        private bool SavePerson()
        {
            if (!Male_radioButton.Checked && !Female_radioButton.Checked)
            {
                MessageBox.Show("You must Choose Sex before Saving");
                return false;
            }
            string sSuffix = Suffix_comboBox.Text.ToString();
            if (Source_textBox.Text.TrimString().Length == 0)
                Source_textBox.Text = U.ManualEntry;
            int iPersonID = Q.i(m_SQL,m_SQL.SavePerson(ref m_PersonDS, 
                                           m_iOriginalPersonID,
                                           m_iSpouseID,
                                           FirstName_textBox.Text.SetNameForDatabase(),
                                           MiddleName_textBox.Text.SetNameForDatabase(),
                                           LastName_textBox.Text.SetNameForDatabase(),
                                           Suffix_comboBox.Text.SetSuffixForDatabase(),
                                           Prefix_comboBox.Text.SetPrefixForDatabase(),
                                           MarriedName_textBox.Text.SetNameForDatabase(),
                                           KnownAs_textBox.Text.SetNameForDatabase(),
                                           m_iFatherID, m_iMotherID,
                                           Description_TextBox.Text.TrimString(),
                                           Source_textBox.Text.TrimString(),
                                           SetPersonSex(Male_radioButton.Checked, Female_radioButton.Checked),
                                           m_bVitalBornFound,
                                           BornDate_textBox.Text.TrimString(),
                                           BornPlace_textBox.Text.TrimString(),
                                           BornHome_textBox.Text.TrimString(),
                                           U.IsChecked(BornVerified_checkBox.Checked),
                                           BornSource_textBox.Text.TrimString(),
                                           BornBook_textBox.Text.TrimString(),
                                           BornPage_textBox.Text.TrimString(),
                                           m_bVitalDiedFound,
                                           DiedDate_textBox.Text.TrimString(),
                                           DiedPlace_textBox.Text.TrimString(),
                                           DiedHome_textBox.Text.TrimString(),
                                           U.IsChecked(DiedVerified_checkBox.Checked),
                                           DiedSource_textBox.Text.TrimString(),
                                           DiedBook_textBox.Text.TrimString(),
                                           DiedPage_textBox.Text.TrimString(),
                                           m_bVitalBuriedFound,
                                           BuriedDate_textBox.Text.TrimString(),
                                           BuriedPlace_textBox.Text.TrimString(),
                                           BuriedStone_textBox.Text.TrimString(),
                                           U.IsChecked(BuriedVerified_checkBox.Checked),
                                           BuriedSource_textBox.Text.TrimString(),
                                           BuriedBook_textBox.Text.TrimString(),
                                           BuriedPage_textBox.Text.TrimString(), 0));
            if (iPersonID == 0)
            {
                MessageBox.Show("Save Unsuccesful");
                return false;
            }
            else
            {
                m_iPersonID = iPersonID;
                m_iOriginalPersonID = m_iPersonID;
                SetToUnmodified();
                return true;
            }
        }
        //****************************************************************************************************************************
        private bool PersonChanged()
        {
            SetAllNameValues();
            if (m_bDidSearch)
                return false;
            DataTable tbl = m_PersonDS.Tables[U.Person_Table];
            bool bBornModified = false;
            if (!m_bVitalBornFound)
            {
                bBornModified = U.Modified(SetPersonSex(Male_radioButton.Checked, Female_radioButton.Checked), tbl, "", U.Sex_col) ||
                                BornDate_textBox.Modified ||
                                BornPlace_textBox.Modified ||
                                BornHome_textBox.Modified ||
                                U.ModifiedCheckBox(U.IsChecked(BornVerified_checkBox.Checked), tbl, "N", U.BornVerified_col) ||
                                BornSource_textBox.Modified ||
                                BornBook_textBox.Modified ||
                                BornPage_textBox.Modified;
            }
            bool bDiedModified = false;
            if (!m_bVitalDiedFound)
            {
                bDiedModified = DiedDate_textBox.Modified ||
                                DiedPlace_textBox.Modified ||
                                DiedHome_textBox.Modified ||
                                U.ModifiedCheckBox(U.IsChecked(DiedVerified_checkBox.Checked), tbl, "N", U.DiedVerified_col) ||
                                DiedSource_textBox.Modified ||
                                DiedBook_textBox.Modified ||
                                DiedPage_textBox.Modified;
            }
            bool bBuriedModified = false;
            if (!m_bVitalBuriedFound)
            {
                bBuriedModified = BuriedDate_textBox.Modified ||
                                  BuriedPlace_textBox.Modified ||
                                  BuriedStone_textBox.Modified ||
                                  U.ModifiedCheckBox(U.IsChecked(BuriedVerified_checkBox.Checked), tbl, "N", U.BuriedVerified_col) ||
                                  BuriedSource_textBox.Modified ||
                                  BuriedBook_textBox.Modified ||
                                  BuriedPage_textBox.Modified;
            }
            if (bBornModified || bDiedModified || bBuriedModified ||
                m_bCategoryOrBuildingChanged ||
                FirstName_textBox.Modified ||
                MiddleName_textBox.Modified ||
                LastName_textBox.Modified ||
                U.Modified(Suffix_comboBox.Text.TrimString(), tbl, "", U.Suffix_col) ||
                U.Modified(Prefix_comboBox.Text.TrimString(), tbl, "", U.Prefix_col) ||
                MarriedName_textBox.Modified ||
                KnownAs_textBox.Modified ||
                Spouse_textBox.Modified ||
                Father_textBox.Modified ||
                Mother_textBox.Modified ||
                Source_textBox.Modified ||
                Description_TextBox.Modified)
            {
                if (m_LastName.Length != 0 || m_MarriedName.Length != 0)
                    return true;
                else
                {
                    switch (MessageBox.Show("Must have valid Last Name.  Do you wish to exit without update?", "", MessageBoxButtons.YesNo))
                    {
                        case DialogResult.Yes: return false;
                        default: return true;
                    }
                }
            }
            else
                return false;
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            bool bCancel = !SaveBeforeLeaving();
            if (bCancel)
                e.Cancel = bCancel;
            else
                Q.v(m_SQL, m_SQL.resetPersonLevel());
        }
        //****************************************************************************************************************************
        private bool SaveBeforeLeaving()
        {
            if ((m_bSelectPersonForPhoto && m_iPersonID != 0 && m_iPersonID != m_iOriginalPersonID) || !PersonChanged() ||
                (m_LastName.Length == 0 && m_MarriedName.Length == 0))
            {
                return true;
            }
            switch (MessageBox.Show("Save Changes?", "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    if (AcceptSimilarNameAlreadyInDatabase())
                        break;
                    else
                    {
                        if (SavePerson())
                            SetAllNameValues();
                        else
                            return false;
                    }
                    break;
                case DialogResult.No:
                    SetToUnmodified();
                    DisplayPerson(m_iPersonID);
                    break;
                case DialogResult.Cancel:
                    return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool AcceptSimilarNameAlreadyInDatabase()
        {
            if (m_iPersonID != 0 || !SimilarNames_checkBox.Checked)
                return false;
            DataTable Person_tbl = Q.t(m_SQL,m_SQL.DefinePersonTable());
            if (m_SQL.PersonExists(Person_tbl, true, U.Person_Table, U.PersonID_col, m_MarriedName, m_KnownAs, m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix))
            {
                switch (MessageBox.Show("Similiar Names Exists In Database.  Do you want to check?", "", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                    {
                        int iPersonID = GetPersonFromGrid(Person_tbl, m_LastName);
                        if (iPersonID != 0)
                        {
                            SetToUnmodified();
                            m_iPersonID = iPersonID;
                            DisplayPerson(m_iPersonID);
                            return true;
                        }
                        else
                        {
                            switch (MessageBox.Show("Save Changes?", "", MessageBoxButtons.YesNo))
                            {
                                case DialogResult.Yes:
                                    return false;
                                default:
                                    return true;
                            }
                        }
                    }
                    default:
                    case DialogResult.No:
                        return false;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        public void Save_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            if (m_LastName.Length == 0 && m_MarriedName.Length == 00)
            {
                MessageBox.Show("You must have a value for the Last or Married Name");
                return;
            }
            if (AcceptSimilarNameAlreadyInDatabase())
                return;
            else
            {
                if (SavePerson())
                {
                    DisplayPerson(m_iPersonID);
                }
            }
        }
        //****************************************************************************************************************************
        private void Family_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            if (m_iPersonID == 0)
            {
                return;
            }
            if (SaveBeforeLeaving())
            {
                FFamily Family = new FFamily(m_SQL, m_iPersonID);
                Family.ShowDialog();
                DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        private void UniquePhotos_button_Click(object sender, EventArgs e)
        {
            CPhotoViewerAddMode PhotoViewer = new CPhotoViewerAddMode(ref m_SQL);
            if (PhotoViewer.PhotoFound() && SaveBeforeLeaving())
            {
                PhotoViewer.ShowDialog();
                DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        private void AddSpouseToMarriageTable(int iSpouseID)
        {
            int iLocation_0_PersonID;
            int iLocation_1_PersonID;
            if (m_iSpouseLocationInArray == 1)
            {
                iLocation_0_PersonID = m_iPersonID;
                iLocation_1_PersonID = m_iSpouseID;
            }
            else
            {
                iLocation_0_PersonID = m_iSpouseID;
                iLocation_1_PersonID = m_iPersonID;
            }
            int iFoundIndex = -1;
            bool bFoundSpouse = false;
            foreach (DataRow row in m_PersonDS.Tables[U.Marriage_Table].Rows)
            {
                iFoundIndex++;
                if (row[m_iSpouseLocationInArray].ToInt() == iSpouseID)
                {
                    bFoundSpouse = true;
                    break;
                }
            }
            if (bFoundSpouse)
                m_iCurrentSpouseIndex = iFoundIndex;
            else
            {
                Spouse_textBox.Modified = true;
                m_PersonDS.Tables[U.Marriage_Table].Rows.Add(iLocation_0_PersonID, iLocation_1_PersonID, "", "M");
                m_iNumberSpouses++;
                m_iCurrentSpouseIndex = m_iNumberSpouses - 1;
                if (Female_radioButton.Checked && MarriedName_textBox.Text.ToString().Length == 0)
                {
                    DataRow Spouse_row = Q.r(m_SQL,m_SQL.GetPerson(iSpouseID));
                    MarriedName_textBox.Text = Spouse_row[U.LastName_col].ToString();
                }
            }
        }
        //****************************************************************************************************************************
        private void NewSpouse_Click(object sender, EventArgs e)
        {
            if (!Male_radioButton.Checked && !Female_radioButton.Checked)
            {
                MessageBox.Show("Choose Sex before adding spouse");
                return;
            }
            if (PersonChanged() || m_iPersonID == 0)
            {
                switch (MessageBox.Show("Save Changes before choosing spouse?", "", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        if (!SavePerson())
                            return;
                        else
                            break;
                    case DialogResult.No:
                        return;
                }
            }
            int iSpouseID = GetAPerson(m_iSpouseID);
            if (iSpouseID != 0 && iSpouseID != m_iPersonID && iSpouseID != m_iSpouseID)
            {
                m_iSpouseID = iSpouseID;
                AddSpouseToMarriageTable(iSpouseID);
                Spouse_textBox.Modified = true;
                DisplaySpouse();
            }
        }
        //****************************************************************************************************************************
        private int GetAPerson(int iPersonID)
        {
            FPerson Person = new FPerson(m_SQL, iPersonID, true);
            Person.ShowDialog();
            return Person.GetPersonID();
        }
        //****************************************************************************************************************************
        private void AddMarriageToTable(int iPersonID,
                                        int iSpouseID)
        {
//            Q.v(m_SQL, m_SQL.InsertMarriageIfItDoesNotExist(m_iFatherID, m_iMotherID));
            DataTable tbl = m_PersonDS.Tables[U.Marriage_Table];
            object[] objCriteria = new object[] { iPersonID, iSpouseID };
            DataRow row = tbl.Rows.Find(objCriteria);
            if (row == null)
            {
                object[] objCriteria1 = new object[] { iSpouseID, iPersonID };
                row = tbl.Rows.Find(objCriteria1);
            }
            if (row == null)
            {
                DataRow NewRow = tbl.NewRow();
                NewRow[U.PersonID_col] = m_iPersonID;
                NewRow[U.SpouseID_col] = m_iSpouseID;
                NewRow[U.DateMarried_col] = "";
                NewRow[U.Divorced_col] = "";
                tbl.Rows.Add(NewRow);
            }
        }
        //****************************************************************************************************************************
        private void Father_Click(object sender, EventArgs e)
        {
            int iFatherID = GetAPerson(m_iFatherID);
            if (iFatherID != 0 && iFatherID != m_iPersonID && iFatherID != m_iFatherID)
            {
                m_iFatherID = iFatherID;
                if (m_iMotherID != 0)
                    AddMarriageToTable(m_iFatherID, m_iMotherID);
                Father_textBox.Text = Q.s(m_SQL,m_SQL.GetPersonName(m_iFatherID));
                Father_textBox.Modified = true;
            }
        }
        //****************************************************************************************************************************
        private void Mother_Click(object sender, EventArgs e)
        {
            int iMotherID = GetAPerson(m_iMotherID);
            if (iMotherID != 0 && iMotherID != m_iPersonID && iMotherID != m_iMotherID)
            {
                m_iMotherID = iMotherID;
                if (m_iFatherID != 0)
                    AddMarriageToTable(m_iFatherID, m_iMotherID);
                Mother_textBox.Text = Q.s(m_SQL, m_SQL.GetPersonName(m_iMotherID));
                Mother_textBox.Modified = true;
            }
        }
        //****************************************************************************************************************************
        private void AdditionalSpouse_Click(object sender, EventArgs e)
        {
            if (m_iNumberSpouses > 1)
            {
                m_iCurrentSpouseIndex++;
                if (m_iCurrentSpouseIndex >= m_iNumberSpouses)
                    m_iCurrentSpouseIndex = 0;
                DisplaySpouse();
            }
        }
        //****************************************************************************************************************************
        private bool RemovePerson(int iPersonID)
        {
            if (iPersonID != 0 && MessageBox.Show("Remove this Person?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;
            else
                return false;
        }
        //****************************************************************************************************************************
        private void RemoveSpouse()
        {
            DataTable tbl = m_PersonDS.Tables[U.Marriage_Table];
            object[] objCriteria = new object[] { m_iPersonID, m_iSpouseID };
            DataRow row = tbl.Rows.Find(objCriteria);
            if (row == null)
            {
                object[] objCriteria1 = new object[] { m_iSpouseID, m_iPersonID };
                row = tbl.Rows.Find(objCriteria1);
            }
            if (row != null)
            {
                Q.v(m_SQL,m_SQL.DeleteThisMarriage(m_iPersonID, m_iSpouseID));
                row.Delete();
                m_iNumberSpouses--;
                m_iCurrentSpouseIndex = 0;
                DisplaySpouse();
            }
        }
        //****************************************************************************************************************************
        private void SpouseRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iSpouseID))
            {
                RemoveSpouse();
                Spouse_textBox.Text = "";
                Spouse_textBox.Modified = true;
            }
        }
        //****************************************************************************************************************************
        private void FatherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iFatherID))
            {
                Father_textBox.Text = "";
                Father_textBox.Text = "";
                m_iFatherID = 0;
            }
        }
        //****************************************************************************************************************************
        private void MotherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iMotherID))
            {
                Mother_textBox.Text = "";
                Mother_textBox.Modified = true;
                m_iMotherID = 0;
            }
        }
        //****************************************************************************************************************************
        private void Categories_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        //****************************************************************************************************************************
        private void InitializePerson()
        {
            FirstName_textBox.Text = "";
            MiddleName_textBox.Text = "";
            LastName_textBox.Text = "";
            Suffix_comboBox.Text = "";
            Prefix_comboBox.Text = "";
            MarriedName_textBox.Text = "";
            KnownAs_textBox.Text = "";
            Male_radioButton.Checked = false;
            Female_radioButton.Checked = false;
            BornBook_textBox.Text = "";
            BornPage_textBox.Text = "";
            BornDate_textBox.Text = "";
            BornPlace_textBox.Text = "";
            BornHome_textBox.Text = "";
            BornVerified_checkBox.Checked = false;
            BornSource_textBox.Text = "";
            DiedBook_textBox.Text = "";
            DiedPage_textBox.Text = "";
            DiedDate_textBox.Text = "";
            DiedPlace_textBox.Text = "";
            DiedHome_textBox.Text = "";
            DiedVerified_checkBox.Checked = false;
            DiedSource_textBox.Text = "";
            BuriedBook_textBox.Text = "";
            BuriedPage_textBox.Text = "";
            BuriedDate_textBox.Text = "";
            BuriedPlace_textBox.Text = "";
            BuriedStone_textBox.Text = "";
            BuriedVerified_checkBox.Checked = false;
            BuriedSource_textBox.Text = "";
            Description_TextBox.Text = "";
            m_iFatherID = 0;
            m_iMotherID = 0;
            Source_textBox.Text = "";
            Spouse_textBox.Text = "";
            Father_textBox.Text = "";
            Mother_textBox.Text = "";
            m_iNumberSpouses = 0;
            m_iCurrentSpouseIndex = 0;
            m_iSpouseLocationInArray = 1;
            m_FirstName = "";
            m_MiddleName = "";
            m_MarriedName = "";
            m_KnownAs = "";
            m_LastName = "";
            m_Suffix = "";
            m_Prefix = "";
            m_iPersonID = 0;
            m_iSpouseID = 0;
            m_iFatherID = 0;
            m_iMotherID = 0;
            m_iOriginalPersonID = 0;

            Categories_listBox.Items.Clear();
            Properties_listBox.Items.Clear();
            SetToUnmodified();
        }
        //****************************************************************************************************************************
        private void NewPerson_click(object sender, EventArgs e)
        {
            if (SaveBeforeLeaving())
            {
                m_iPersonID = 0;
                InitializePerson();
                m_PersonDS.Tables[U.Person_Table].Clear();
                m_PersonDS.Tables[U.VitalRecord_Table].Clear();
                m_PersonDS.Tables[U.Marriage_Table].Clear();
                m_PersonDS.Tables[U.PersonCategoryValue_Table].Clear();
                m_PersonDS.Tables[U.PersonBuildingValue_Table].Clear();
            }
        }

        private void Properties_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //****************************************************************************************************************************
    }
}