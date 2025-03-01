using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    partial class FVitalRecord
    {
        private MenuStrip menuStrip1;
        private ToolStripMenuItem newRecordToolStripMenuItem;
        private ToolStripMenuItem birthMaleToolStripMenuItem;
        private ToolStripMenuItem birthFemaleToolStripMenuItem;
        private ToolStripMenuItem deathMaleToolStripMenuItem;
        private ToolStripMenuItem deathFemaleToolStripMenuItem;
        private ToolStripMenuItem marriageBrideToolStripMenuItem;
        private ToolStripMenuItem marriageGroomToolStripMenuItem;
        private ToolStripMenuItem CivilUnionPartyAToolStripMenuItem;
        private ToolStripMenuItem CivilUnionPartyBToolStripMenuItem;
        private ToolStripMenuItem burialToolStripMenuItem;
        private CheckBox NameIntegrated_checkBox;
        private CheckBox MotherIntegrated_checkBox;
        private CheckBox FatherIntegrated_checkBox;
        private CheckBox SpouseIntegrated_checkBox;
        private CheckBox SpouseFatherIntegrated_checkBox;
        private CheckBox SpouseMotherIntegrated_checkBox;
        private ToolStripMenuItem integrationToolStripMenuItem;
        private ToolStripMenuItem integrateRecord_ToolStripMenuItem;
        private ToolStripMenuItem integratedPersonToolStripMenuItem;
        private GroupBox Spouse_groupBox;
        private TextBox SpouseLastName_textBox;
        private TextBox SpouseFirstName_textBox;
        private TextBox SpouseMiddleName_textBox;
        private ComboBox SpouseSuffix_comboBox;
        private ComboBox SpousePrefix_comboBox;
        private Label label13;
        private GroupBox Disposition_groupBox;
        private GroupBox SpouseFather_groupBox;
        private TextBox SpouseFatherLastName_textBox;
        private TextBox SpouseFatherFirstName_textBox;
        private TextBox SpouseFatherMiddleName_textBox;
        private ComboBox SpouseFatherSuffix_comboBox;
        private ComboBox SpouseFatherPrefix_comboBox;
        private GroupBox SpouseMother_groupBox;
        private TextBox SpouseMotherLastName_textBox;
        private TextBox SpouseMotherFirstName_textBox;
        private TextBox SpouseMotherMiddleName_textBox;
        private ComboBox SpouseMotherSuffix_comboBox;
        private ComboBox SpouseMotherPrefix_comboBox;
        private RadioButton Buried_radioButton;
        private RadioButton Other_radioButton;
        private RadioButton Cremated_radioButton;
        private Label SpouseLastName_label;
        private Label SpouseFirstName_label;
        private Label SpouseMiddleName_label;
        private Label SpouseSuffix_label;
        private Label SpousePrefix_label;
        private ToolStripMenuItem changeToolStripMenuItem;
        private GroupBox SearchRequest_groupBox;
        private Button LastMother_button;
        private Button LastSpouse_button;
        private Button LastSpouseMother_button;
        private Button SearchAll_button;
        private Button SearchPartial_Button;
        private Button StartingWith_button;
        private Button Similar_button;
        private Button Save_button;
        private RichTextBox Notes_TextBox;
        private Label Notes_label;
        private GroupBox Sex_groupBox;
        private RadioButton Female_radioButton;
        private RadioButton Male_radioButton;
        private RadioButton SearchAllNames_radioButton;
        private RadioButton SearchAllNamesFather_radioButton;
        private System.Windows.Forms.TextBox LastName_textBox;
        private System.Windows.Forms.TextBox FirstName_textBox;
        private System.Windows.Forms.TextBox MiddleName_textBox;
        private System.Windows.Forms.ComboBox Suffix_comboBox;
        private System.Windows.Forms.ComboBox Prefix_comboBox;
        private System.Windows.Forms.TextBox FatherLastName_textBox;
        private System.Windows.Forms.TextBox FatherFirstName_textBox;
        private System.Windows.Forms.TextBox FatherMiddleName_textBox;
        private System.Windows.Forms.ComboBox FatherSuffix_comboBox;
        private System.Windows.Forms.ComboBox FatherPrefix_comboBox;
        private System.Windows.Forms.TextBox MotherLastName_textBox;
        private System.Windows.Forms.TextBox MotherFirstName_textBox;
        private System.Windows.Forms.TextBox MotherMiddleName_textBox;
        private System.Windows.Forms.ComboBox MotherSuffix_comboBox;
        private System.Windows.Forms.ComboBox MotherPrefix_comboBox;
        private System.Windows.Forms.TextBox DateMonth_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DateDay_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DateYear_textBox;
        private System.Windows.Forms.GroupBox Date_groupBox;
        private System.Windows.Forms.GroupBox BookPage_groupBox;
        private System.Windows.Forms.TextBox Page_textBox;
        private System.Windows.Forms.TextBox Book_textBox;
        private System.Windows.Forms.GroupBox Name_groupBox;
        private System.Windows.Forms.GroupBox Mother_groupBox;
        private System.Windows.Forms.GroupBox Father_groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox Age_groupBox;
        private System.Windows.Forms.TextBox AgeDays_textBox;
        private System.Windows.Forms.TextBox AgeMonths_textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox AgeYears_textBox;
        private System.Windows.Forms.Label label10;
        private GroupBox Burial_groupBox;
        private TextBox LotNumber_textBox;
        private ComboBox Cemetery_comboBox;
        private Label label11;
        private System.Windows.Forms.Label label9;
        //****************************************************************************************************************************
        private System.ComponentModel.IContainer components = null;
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
        #region Windows Form Designer generated code
        //****************************************************************************************************************************
        private void InitializeComponent()
        {
            this.LastName_textBox = new System.Windows.Forms.TextBox();
            this.FirstName_textBox = new System.Windows.Forms.TextBox();
            this.MiddleName_textBox = new System.Windows.Forms.TextBox();
            this.Suffix_comboBox = new System.Windows.Forms.ComboBox();
            this.Prefix_comboBox = new System.Windows.Forms.ComboBox();
            this.FatherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.FatherLastName_textBox = new System.Windows.Forms.TextBox();
            this.FatherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.FatherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.FatherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.MotherLastName_textBox = new System.Windows.Forms.TextBox();
            this.MotherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.MotherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.MotherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.MotherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.DateMonth_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DateDay_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DateYear_textBox = new System.Windows.Forms.TextBox();
            this.Date_groupBox = new System.Windows.Forms.GroupBox();
            this.BookPage_groupBox = new System.Windows.Forms.GroupBox();
            this.Page_textBox = new System.Windows.Forms.TextBox();
            this.Book_textBox = new System.Windows.Forms.TextBox();
            this.Name_groupBox = new System.Windows.Forms.GroupBox();
            this.NameIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.Mother_groupBox = new System.Windows.Forms.GroupBox();
            this.MotherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.Father_groupBox = new System.Windows.Forms.GroupBox();
            this.FatherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Age_groupBox = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.AgeDays_textBox = new System.Windows.Forms.TextBox();
            this.AgeMonths_textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.AgeYears_textBox = new System.Windows.Forms.TextBox();
            this.Burial_groupBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.Disposition_groupBox = new System.Windows.Forms.GroupBox();
            this.Other_radioButton = new System.Windows.Forms.RadioButton();
            this.Cremated_radioButton = new System.Windows.Forms.RadioButton();
            this.Buried_radioButton = new System.Windows.Forms.RadioButton();
            this.Cemetery_comboBox = new System.Windows.Forms.ComboBox();
            this.LotNumber_textBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Save_button = new System.Windows.Forms.Button();
            this.Notes_TextBox = new System.Windows.Forms.RichTextBox();
            this.Notes_label = new System.Windows.Forms.Label();
            this.SearchAll_button = new System.Windows.Forms.Button();
            this.SearchPartial_Button = new System.Windows.Forms.Button();
            this.StartingWith_button = new System.Windows.Forms.Button();
            this.Similar_button = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.birthMaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.birthFemaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deathMaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deathFemaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marriageBrideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marriageGroomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CivilUnionPartyAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CivilUnionPartyBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.burialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integrateRecord_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integratedPersonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Spouse_groupBox = new System.Windows.Forms.GroupBox();
            this.SpouseIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.SpouseLastName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFirstName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpousePrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseFather_groupBox = new System.Windows.Forms.GroupBox();
            this.SpouseFatherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.SpouseFatherLastName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFatherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFatherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFatherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseFatherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseMother_groupBox = new System.Windows.Forms.GroupBox();
            this.SpouseMotherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.SpouseMotherLastName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMotherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMotherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMotherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseMotherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseLastName_label = new System.Windows.Forms.Label();
            this.SpouseFirstName_label = new System.Windows.Forms.Label();
            this.SpouseMiddleName_label = new System.Windows.Forms.Label();
            this.SpouseSuffix_label = new System.Windows.Forms.Label();
            this.SpousePrefix_label = new System.Windows.Forms.Label();
            this.SearchRequest_groupBox = new System.Windows.Forms.GroupBox();
            this.SearchAllNamesSpouse_radioButton = new System.Windows.Forms.RadioButton();
            this.SearchAllNamesFather_radioButton = new System.Windows.Forms.RadioButton();
            this.SearchAllNames_radioButton = new System.Windows.Forms.RadioButton();
            this.LastMother_button = new System.Windows.Forms.Button();
            this.LastSpouse_button = new System.Windows.Forms.Button();
            this.LastSpouseMother_button = new System.Windows.Forms.Button();
            this.Sex_groupBox = new System.Windows.Forms.GroupBox();
            this.Female_radioButton = new System.Windows.Forms.RadioButton();
            this.Male_radioButton = new System.Windows.Forms.RadioButton();
            this.ReturnToList_button = new System.Windows.Forms.Button();
            this.ExcludeFromSite_checkBox = new System.Windows.Forms.CheckBox();
            this.CalcBornDate_groupBox = new System.Windows.Forms.GroupBox();
            this.CalcDateMonth_textBox = new System.Windows.Forms.TextBox();
            this.CalcDateYear_textBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.CalcDateDay_textBox = new System.Windows.Forms.TextBox();
            this.BirthDate_groupBox = new System.Windows.Forms.GroupBox();
            this.BornDateMonth_textBox = new System.Windows.Forms.TextBox();
            this.BornDateYear_textBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.BornDateDay_textBox = new System.Windows.Forms.TextBox();
            this.SpouseBornDateDay_textBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.SpouseBornDateYear_textBox = new System.Windows.Forms.TextBox();
            this.SpouseBornDateMonth_textBox = new System.Windows.Forms.TextBox();
            this.SpouseBirthDate_groupBox = new System.Windows.Forms.GroupBox();
            this.SpouseAge_groupBox = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.SpouseAgeDays_textBox = new System.Windows.Forms.TextBox();
            this.SpouseAgeMonths_textBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.SpouseAgeYears_textBox = new System.Windows.Forms.TextBox();
            this.Date_groupBox.SuspendLayout();
            this.BookPage_groupBox.SuspendLayout();
            this.Name_groupBox.SuspendLayout();
            this.Mother_groupBox.SuspendLayout();
            this.Father_groupBox.SuspendLayout();
            this.Age_groupBox.SuspendLayout();
            this.Burial_groupBox.SuspendLayout();
            this.Disposition_groupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.Spouse_groupBox.SuspendLayout();
            this.SpouseFather_groupBox.SuspendLayout();
            this.SpouseMother_groupBox.SuspendLayout();
            this.SearchRequest_groupBox.SuspendLayout();
            this.Sex_groupBox.SuspendLayout();
            this.CalcBornDate_groupBox.SuspendLayout();
            this.BirthDate_groupBox.SuspendLayout();
            this.SpouseBirthDate_groupBox.SuspendLayout();
            this.SpouseAge_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LastName_textBox
            // 
            this.LastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.LastName_textBox.Name = "LastName_textBox";
            this.LastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.LastName_textBox.TabIndex = 0;
            // 
            // FirstName_textBox
            // 
            this.FirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.FirstName_textBox.Name = "FirstName_textBox";
            this.FirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FirstName_textBox.TabIndex = 2;
            this.FirstName_textBox.Leave += new System.EventHandler(this.FirstName_textBox_Leave);
            // 
            // MiddleName_textBox
            // 
            this.MiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.MiddleName_textBox.Name = "MiddleName_textBox";
            this.MiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MiddleName_textBox.TabIndex = 3;
            // 
            // Suffix_comboBox
            // 
            this.Suffix_comboBox.FormattingEnabled = true;
            this.Suffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.Suffix_comboBox.Name = "Suffix_comboBox";
            this.Suffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.Suffix_comboBox.TabIndex = 4;
            // 
            // Prefix_comboBox
            // 
            this.Prefix_comboBox.FormattingEnabled = true;
            this.Prefix_comboBox.Location = new System.Drawing.Point(22, 143);
            this.Prefix_comboBox.Name = "Prefix_comboBox";
            this.Prefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.Prefix_comboBox.TabIndex = 5;
            // 
            // FatherPrefix_comboBox
            // 
            this.FatherPrefix_comboBox.FormattingEnabled = true;
            this.FatherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.FatherPrefix_comboBox.Name = "FatherPrefix_comboBox";
            this.FatherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.FatherPrefix_comboBox.TabIndex = 99;
            // 
            // FatherLastName_textBox
            // 
            this.FatherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.FatherLastName_textBox.Name = "FatherLastName_textBox";
            this.FatherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FatherLastName_textBox.TabIndex = 6;
            // 
            // FatherFirstName_textBox
            // 
            this.FatherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.FatherFirstName_textBox.Name = "FatherFirstName_textBox";
            this.FatherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FatherFirstName_textBox.TabIndex = 7;
            // 
            // FatherMiddleName_textBox
            // 
            this.FatherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.FatherMiddleName_textBox.Name = "FatherMiddleName_textBox";
            this.FatherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FatherMiddleName_textBox.TabIndex = 8;
            // 
            // FatherSuffix_comboBox
            // 
            this.FatherSuffix_comboBox.FormattingEnabled = true;
            this.FatherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.FatherSuffix_comboBox.Name = "FatherSuffix_comboBox";
            this.FatherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.FatherSuffix_comboBox.TabIndex = 9;
            // 
            // MotherLastName_textBox
            // 
            this.MotherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.MotherLastName_textBox.Name = "MotherLastName_textBox";
            this.MotherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MotherLastName_textBox.TabIndex = 11;
            // 
            // MotherFirstName_textBox
            // 
            this.MotherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.MotherFirstName_textBox.Name = "MotherFirstName_textBox";
            this.MotherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MotherFirstName_textBox.TabIndex = 12;
            // 
            // MotherMiddleName_textBox
            // 
            this.MotherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.MotherMiddleName_textBox.Name = "MotherMiddleName_textBox";
            this.MotherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MotherMiddleName_textBox.TabIndex = 13;
            // 
            // MotherSuffix_comboBox
            // 
            this.MotherSuffix_comboBox.FormattingEnabled = true;
            this.MotherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.MotherSuffix_comboBox.Name = "MotherSuffix_comboBox";
            this.MotherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.MotherSuffix_comboBox.TabIndex = 14;
            // 
            // MotherPrefix_comboBox
            // 
            this.MotherPrefix_comboBox.FormattingEnabled = true;
            this.MotherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.MotherPrefix_comboBox.Name = "MotherPrefix_comboBox";
            this.MotherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.MotherPrefix_comboBox.TabIndex = 15;
            // 
            // DateMonth_textBox
            // 
            this.DateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.DateMonth_textBox.Name = "DateMonth_textBox";
            this.DateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.DateMonth_textBox.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "/";
            // 
            // DateDay_textBox
            // 
            this.DateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.DateDay_textBox.Name = "DateDay_textBox";
            this.DateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.DateDay_textBox.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "/";
            // 
            // DateYear_textBox
            // 
            this.DateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.DateYear_textBox.Name = "DateYear_textBox";
            this.DateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.DateYear_textBox.TabIndex = 20;
            // 
            // Date_groupBox
            // 
            this.Date_groupBox.Controls.Add(this.DateMonth_textBox);
            this.Date_groupBox.Controls.Add(this.DateYear_textBox);
            this.Date_groupBox.Controls.Add(this.label4);
            this.Date_groupBox.Controls.Add(this.label5);
            this.Date_groupBox.Controls.Add(this.DateDay_textBox);
            this.Date_groupBox.Location = new System.Drawing.Point(628, 122);
            this.Date_groupBox.Name = "Date_groupBox";
            this.Date_groupBox.Size = new System.Drawing.Size(169, 56);
            this.Date_groupBox.TabIndex = 3;
            this.Date_groupBox.TabStop = false;
            this.Date_groupBox.Text = "Date";
            // 
            // BookPage_groupBox
            // 
            this.BookPage_groupBox.Controls.Add(this.Page_textBox);
            this.BookPage_groupBox.Controls.Add(this.Book_textBox);
            this.BookPage_groupBox.Location = new System.Drawing.Point(628, 49);
            this.BookPage_groupBox.Name = "BookPage_groupBox";
            this.BookPage_groupBox.Size = new System.Drawing.Size(169, 56);
            this.BookPage_groupBox.TabIndex = 2;
            this.BookPage_groupBox.TabStop = false;
            this.BookPage_groupBox.Text = "Book-Page";
            // 
            // Page_textBox
            // 
            this.Page_textBox.Location = new System.Drawing.Point(94, 25);
            this.Page_textBox.Name = "Page_textBox";
            this.Page_textBox.Size = new System.Drawing.Size(52, 20);
            this.Page_textBox.TabIndex = 17;
            // 
            // Book_textBox
            // 
            this.Book_textBox.Location = new System.Drawing.Point(22, 25);
            this.Book_textBox.Name = "Book_textBox";
            this.Book_textBox.Size = new System.Drawing.Size(50, 20);
            this.Book_textBox.TabIndex = 16;
            // 
            // Name_groupBox
            // 
            this.Name_groupBox.Controls.Add(this.NameIntegrated_checkBox);
            this.Name_groupBox.Controls.Add(this.LastName_textBox);
            this.Name_groupBox.Controls.Add(this.FirstName_textBox);
            this.Name_groupBox.Controls.Add(this.MiddleName_textBox);
            this.Name_groupBox.Controls.Add(this.Suffix_comboBox);
            this.Name_groupBox.Controls.Add(this.Prefix_comboBox);
            this.Name_groupBox.Location = new System.Drawing.Point(118, 49);
            this.Name_groupBox.Name = "Name_groupBox";
            this.Name_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Name_groupBox.TabIndex = 0;
            this.Name_groupBox.TabStop = false;
            this.Name_groupBox.Text = "Name";
            // 
            // NameIntegrated_checkBox
            // 
            this.NameIntegrated_checkBox.AutoSize = true;
            this.NameIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NameIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.NameIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.NameIntegrated_checkBox.Name = "NameIntegrated_checkBox";
            this.NameIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.NameIntegrated_checkBox.TabIndex = 82;
            this.NameIntegrated_checkBox.TabStop = false;
            this.NameIntegrated_checkBox.Text = "Integrated";
            this.NameIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // Mother_groupBox
            // 
            this.Mother_groupBox.Controls.Add(this.MotherIntegrated_checkBox);
            this.Mother_groupBox.Controls.Add(this.MotherLastName_textBox);
            this.Mother_groupBox.Controls.Add(this.MotherFirstName_textBox);
            this.Mother_groupBox.Controls.Add(this.MotherMiddleName_textBox);
            this.Mother_groupBox.Controls.Add(this.MotherSuffix_comboBox);
            this.Mother_groupBox.Controls.Add(this.MotherPrefix_comboBox);
            this.Mother_groupBox.Location = new System.Drawing.Point(459, 49);
            this.Mother_groupBox.Name = "Mother_groupBox";
            this.Mother_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Mother_groupBox.TabIndex = 7;
            this.Mother_groupBox.TabStop = false;
            this.Mother_groupBox.Text = "Mother Maiden Name";
            // 
            // MotherIntegrated_checkBox
            // 
            this.MotherIntegrated_checkBox.AutoSize = true;
            this.MotherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MotherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MotherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.MotherIntegrated_checkBox.Name = "MotherIntegrated_checkBox";
            this.MotherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.MotherIntegrated_checkBox.TabIndex = 84;
            this.MotherIntegrated_checkBox.TabStop = false;
            this.MotherIntegrated_checkBox.Text = "Integrated";
            this.MotherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // Father_groupBox
            // 
            this.Father_groupBox.Controls.Add(this.FatherIntegrated_checkBox);
            this.Father_groupBox.Controls.Add(this.FatherLastName_textBox);
            this.Father_groupBox.Controls.Add(this.FatherFirstName_textBox);
            this.Father_groupBox.Controls.Add(this.FatherMiddleName_textBox);
            this.Father_groupBox.Controls.Add(this.FatherSuffix_comboBox);
            this.Father_groupBox.Controls.Add(this.FatherPrefix_comboBox);
            this.Father_groupBox.Location = new System.Drawing.Point(293, 49);
            this.Father_groupBox.Name = "Father_groupBox";
            this.Father_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Father_groupBox.TabIndex = 6;
            this.Father_groupBox.TabStop = false;
            this.Father_groupBox.Text = "Name of Father";
            // 
            // FatherIntegrated_checkBox
            // 
            this.FatherIntegrated_checkBox.AutoSize = true;
            this.FatherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FatherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.FatherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.FatherIntegrated_checkBox.Name = "FatherIntegrated_checkBox";
            this.FatherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.FatherIntegrated_checkBox.TabIndex = 83;
            this.FatherIntegrated_checkBox.TabStop = false;
            this.FatherIntegrated_checkBox.Text = "Integrated";
            this.FatherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "Middle Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "Lot No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "First Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 67;
            this.label6.Text = "Suffix";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 68;
            this.label7.Text = "Prefix";
            // 
            // Age_groupBox
            // 
            this.Age_groupBox.Controls.Add(this.label10);
            this.Age_groupBox.Controls.Add(this.label9);
            this.Age_groupBox.Controls.Add(this.AgeDays_textBox);
            this.Age_groupBox.Controls.Add(this.AgeMonths_textBox);
            this.Age_groupBox.Controls.Add(this.label8);
            this.Age_groupBox.Controls.Add(this.AgeYears_textBox);
            this.Age_groupBox.Location = new System.Drawing.Point(628, 189);
            this.Age_groupBox.Name = "Age_groupBox";
            this.Age_groupBox.Size = new System.Drawing.Size(169, 56);
            this.Age_groupBox.TabIndex = 120;
            this.Age_groupBox.TabStop = false;
            this.Age_groupBox.Visible = true;
            this.Age_groupBox.Text = "Age";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(138, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 75;
            this.label10.Text = "Ds";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(96, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 13);
            this.label9.TabIndex = 74;
            this.label9.Text = "Ms";
            // 
            // AgeDays_textBox
            // 
            this.AgeDays_textBox.Location = new System.Drawing.Point(116, 25);
            this.AgeDays_textBox.Name = "AgeDays_textBox";
            this.AgeDays_textBox.Size = new System.Drawing.Size(22, 20);
            this.AgeDays_textBox.TabIndex = 123;
            // 
            // AgeMonths_textBox
            // 
            this.AgeMonths_textBox.Location = new System.Drawing.Point(74, 25);
            this.AgeMonths_textBox.Name = "AgeMonths_textBox";
            this.AgeMonths_textBox.Size = new System.Drawing.Size(22, 20);
            this.AgeMonths_textBox.TabIndex = 122;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 71;
            this.label8.Text = "Ys";
            // 
            // AgeYears_textBox
            // 
            this.AgeYears_textBox.Location = new System.Drawing.Point(22, 25);
            this.AgeYears_textBox.Name = "AgeYears_textBox";
            this.AgeYears_textBox.Size = new System.Drawing.Size(28, 20);
            this.AgeYears_textBox.TabIndex = 121;
            // 
            // Burial_groupBox
            // 
            this.Burial_groupBox.Controls.Add(this.label13);
            this.Burial_groupBox.Controls.Add(this.Disposition_groupBox);
            this.Burial_groupBox.Controls.Add(this.Cemetery_comboBox);
            this.Burial_groupBox.Controls.Add(this.LotNumber_textBox);
            this.Burial_groupBox.Controls.Add(this.label3);
            this.Burial_groupBox.Location = new System.Drawing.Point(820, 49);
            this.Burial_groupBox.Name = "Burial_groupBox";
            this.Burial_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Burial_groupBox.TabIndex = 11;
            this.Burial_groupBox.TabStop = false;
            this.Burial_groupBox.Text = "Burial";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 13);
            this.label13.TabIndex = 67;
            this.label13.Text = "Cemetery";
            // 
            // Disposition_groupBox
            // 
            this.Disposition_groupBox.Controls.Add(this.Other_radioButton);
            this.Disposition_groupBox.Controls.Add(this.Cremated_radioButton);
            this.Disposition_groupBox.Controls.Add(this.Buried_radioButton);
            this.Disposition_groupBox.Location = new System.Drawing.Point(25, 23);
            this.Disposition_groupBox.Name = "Disposition_groupBox";
            this.Disposition_groupBox.Size = new System.Drawing.Size(102, 85);
            this.Disposition_groupBox.TabIndex = 6;
            this.Disposition_groupBox.TabStop = false;
            this.Disposition_groupBox.Text = "Disposition";
            // 
            // Other_radioButton
            // 
            this.Other_radioButton.AutoSize = true;
            this.Other_radioButton.Location = new System.Drawing.Point(6, 59);
            this.Other_radioButton.Name = "Other_radioButton";
            this.Other_radioButton.Size = new System.Drawing.Size(51, 17);
            this.Other_radioButton.TabIndex = 68;
            this.Other_radioButton.Text = "Other";
            this.Other_radioButton.UseVisualStyleBackColor = true;
            // 
            // Cremated_radioButton
            // 
            this.Cremated_radioButton.AutoSize = true;
            this.Cremated_radioButton.Location = new System.Drawing.Point(6, 38);
            this.Cremated_radioButton.Name = "Cremated_radioButton";
            this.Cremated_radioButton.Size = new System.Drawing.Size(70, 17);
            this.Cremated_radioButton.TabIndex = 67;
            this.Cremated_radioButton.Text = "Cremated";
            this.Cremated_radioButton.UseVisualStyleBackColor = true;
            // 
            // Buried_radioButton
            // 
            this.Buried_radioButton.AutoSize = true;
            this.Buried_radioButton.Checked = true;
            this.Buried_radioButton.Location = new System.Drawing.Point(6, 19);
            this.Buried_radioButton.Name = "Buried_radioButton";
            this.Buried_radioButton.Size = new System.Drawing.Size(55, 17);
            this.Buried_radioButton.TabIndex = 66;
            this.Buried_radioButton.TabStop = true;
            this.Buried_radioButton.Text = "Buried";
            this.Buried_radioButton.UseVisualStyleBackColor = true;
            // 
            // Cemetery_comboBox
            // 
            this.Cemetery_comboBox.FormattingEnabled = true;
            this.Cemetery_comboBox.Location = new System.Drawing.Point(25, 137);
            this.Cemetery_comboBox.Name = "Cemetery_comboBox";
            this.Cemetery_comboBox.Size = new System.Drawing.Size(102, 21);
            this.Cemetery_comboBox.TabIndex = 37;
            // 
            // LotNumber_textBox
            // 
            this.LotNumber_textBox.Location = new System.Drawing.Point(83, 164);
            this.LotNumber_textBox.Name = "LotNumber_textBox";
            this.LotNumber_textBox.Size = new System.Drawing.Size(44, 20);
            this.LotNumber_textBox.TabIndex = 38;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 71;
            this.label11.Text = "Last Name";
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(131, 480);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(117, 23);
            this.Save_button.TabIndex = 72;
            this.Save_button.TabStop = false;
            this.Save_button.Text = "Save";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Notes_TextBox
            // 
            this.Notes_TextBox.Location = new System.Drawing.Point(627, 269);
            this.Notes_TextBox.Name = "Notes_TextBox";
            this.Notes_TextBox.Size = new System.Drawing.Size(170, 158);
            this.Notes_TextBox.TabIndex = 36;
            this.Notes_TextBox.Text = "";
            // 
            // Notes_label
            // 
            this.Notes_label.AutoSize = true;
            this.Notes_label.Location = new System.Drawing.Point(625, 253);
            this.Notes_label.Name = "Notes_label";
            this.Notes_label.Size = new System.Drawing.Size(35, 13);
            this.Notes_label.TabIndex = 75;
            this.Notes_label.Text = "Notes";
            // 
            // SearchAll_button
            // 
            this.SearchAll_button.Location = new System.Drawing.Point(306, 478);
            this.SearchAll_button.Name = "SearchAll_button";
            this.SearchAll_button.Size = new System.Drawing.Size(117, 23);
            this.SearchAll_button.TabIndex = 76;
            this.SearchAll_button.TabStop = false;
            this.SearchAll_button.Text = "Search All Records";
            this.SearchAll_button.UseVisualStyleBackColor = true;
            this.SearchAll_button.Visible = false;
            this.SearchAll_button.Click += new System.EventHandler(this.SearchAll_button_Click);
            // 
            // SearchPartial_Button
            // 
            this.SearchPartial_Button.Location = new System.Drawing.Point(306, 510);
            this.SearchPartial_Button.Name = "SearchPartial_Button";
            this.SearchPartial_Button.Size = new System.Drawing.Size(117, 23);
            this.SearchPartial_Button.TabIndex = 77;
            this.SearchPartial_Button.TabStop = false;
            this.SearchPartial_Button.Text = "Partial Name";
            this.SearchPartial_Button.UseVisualStyleBackColor = true;
            this.SearchPartial_Button.Visible = false;
            this.SearchPartial_Button.Click += new System.EventHandler(this.SearchPartial_button_Click);
            // 
            // StartingWith_button
            // 
            this.StartingWith_button.Location = new System.Drawing.Point(306, 540);
            this.StartingWith_button.Name = "StartingWith_button";
            this.StartingWith_button.Size = new System.Drawing.Size(117, 23);
            this.StartingWith_button.TabIndex = 78;
            this.StartingWith_button.TabStop = false;
            this.StartingWith_button.Text = "Last Starting With";
            this.StartingWith_button.UseVisualStyleBackColor = true;
            this.StartingWith_button.Click += new System.EventHandler(this.SearchStartingWith_button_Click);
            // 
            // Similar_button
            // 
            this.Similar_button.Location = new System.Drawing.Point(306, 570);
            this.Similar_button.Name = "Similar_button";
            this.Similar_button.Size = new System.Drawing.Size(117, 23);
            this.Similar_button.TabIndex = 79;
            this.Similar_button.TabStop = false;
            this.Similar_button.Text = "Similar Names";
            this.Similar_button.UseVisualStyleBackColor = true;
            this.Similar_button.Click += new System.EventHandler(this.SearchSimilar_button_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newRecordToolStripMenuItem,
            this.integrationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1010, 24);
            this.menuStrip1.TabIndex = 80;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newRecordToolStripMenuItem
            // 
            this.newRecordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.birthMaleToolStripMenuItem,
            this.birthFemaleToolStripMenuItem,
            this.deathMaleToolStripMenuItem,
            this.deathFemaleToolStripMenuItem,
            this.marriageBrideToolStripMenuItem,
            this.marriageGroomToolStripMenuItem,
            this.CivilUnionPartyAToolStripMenuItem,
            this.CivilUnionPartyBToolStripMenuItem,
            this.burialToolStripMenuItem,
            this.changeToolStripMenuItem});
            this.newRecordToolStripMenuItem.Name = "newRecordToolStripMenuItem";
            this.newRecordToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.newRecordToolStripMenuItem.Text = "New Record";
            // 
            // birthMaleToolStripMenuItem
            // 
            this.birthMaleToolStripMenuItem.Name = "birthMaleToolStripMenuItem";
            this.birthMaleToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.birthMaleToolStripMenuItem.Text = "Birth-Male";
            this.birthMaleToolStripMenuItem.Click += new System.EventHandler(this.BirthMalebutton_Click);
            // 
            // birthFemaleToolStripMenuItem
            // 
            this.birthFemaleToolStripMenuItem.Name = "birthFemaleToolStripMenuItem";
            this.birthFemaleToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.birthFemaleToolStripMenuItem.Text = "Birth-Female";
            this.birthFemaleToolStripMenuItem.Click += new System.EventHandler(this.BirthFemalebutton_Click);
            // 
            // deathMaleToolStripMenuItem
            // 
            this.deathMaleToolStripMenuItem.Name = "deathMaleToolStripMenuItem";
            this.deathMaleToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deathMaleToolStripMenuItem.Text = "Death-Male";
            this.deathMaleToolStripMenuItem.Click += new System.EventHandler(this.DeathMalebutton_Click);
            // 
            // deathFemaleToolStripMenuItem
            // 
            this.deathFemaleToolStripMenuItem.Name = "deathFemaleToolStripMenuItem";
            this.deathFemaleToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deathFemaleToolStripMenuItem.Text = "Death-Female";
            this.deathFemaleToolStripMenuItem.Click += new System.EventHandler(this.DeathFemalebutton_Click);
            // 
            // marriageBrideToolStripMenuItem
            // 
            this.marriageBrideToolStripMenuItem.Name = "marriageBrideToolStripMenuItem";
            this.marriageBrideToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.marriageBrideToolStripMenuItem.Text = "Marriage-Bride";
            this.marriageBrideToolStripMenuItem.Click += new System.EventHandler(this.MarriageBridebutton_Click);
            // 
            // marriageGroomToolStripMenuItem
            // 
            this.marriageGroomToolStripMenuItem.Name = "marriageGroomToolStripMenuItem";
            this.marriageGroomToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.marriageGroomToolStripMenuItem.Text = "Marriage-Groom";
            this.marriageGroomToolStripMenuItem.Click += new System.EventHandler(this.MarriageGroombutton_Click);
            // 
            // CivilUnionPartyAToolStripMenuItem
            // 
            this.CivilUnionPartyAToolStripMenuItem.Name = "CivilUnionPartyAToolStripMenuItem";
            this.CivilUnionPartyAToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.CivilUnionPartyAToolStripMenuItem.Text = "Marriage-Party A";
            this.CivilUnionPartyAToolStripMenuItem.Click += new System.EventHandler(this.CivilUnionPartyAbutton_Click);
            // 
            // CivilUnionPartyBToolStripMenuItem
            // 
            this.CivilUnionPartyBToolStripMenuItem.Name = "CivilUnionPartyBToolStripMenuItem";
            this.CivilUnionPartyBToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.CivilUnionPartyBToolStripMenuItem.Text = "Marriage-Party B";
            this.CivilUnionPartyBToolStripMenuItem.Click += new System.EventHandler(this.CivilUnionPartyBbutton_Click);
            // 
            // burialToolStripMenuItem
            // 
            this.burialToolStripMenuItem.Name = "burialToolStripMenuItem";
            this.burialToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.burialToolStripMenuItem.Text = "Burial";
            this.burialToolStripMenuItem.Click += new System.EventHandler(this.Burialbutton_Click);
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.changeToolStripMenuItem.Text = "Change";
            this.changeToolStripMenuItem.Click += new System.EventHandler(this.Changebutton_Click);
            // 
            // integrationToolStripMenuItem
            // 
            this.integrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.integrateRecord_ToolStripMenuItem,
            this.integratedPersonToolStripMenuItem});
            this.integrationToolStripMenuItem.Name = "integrationToolStripMenuItem";
            this.integrationToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.integrationToolStripMenuItem.Text = "Integration";
            // 
            // integrateRecord_ToolStripMenuItem
            // 
            this.integrateRecord_ToolStripMenuItem.Name = "integrateRecord_ToolStripMenuItem";
            this.integrateRecord_ToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.integrateRecord_ToolStripMenuItem.Text = "Integrate Record";
            this.integrateRecord_ToolStripMenuItem.Click += new System.EventHandler(this.Integratebutton_Click);
            // 
            // integratedPersonToolStripMenuItem
            // 
            this.integratedPersonToolStripMenuItem.Name = "integratedPersonToolStripMenuItem";
            this.integratedPersonToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.integratedPersonToolStripMenuItem.Text = "View Integrated Records";
            this.integratedPersonToolStripMenuItem.Click += new System.EventHandler(this.IntegratedPerson_button_Click);
            // 
            // Spouse_groupBox
            // 
            this.Spouse_groupBox.Controls.Add(this.SpouseIntegrated_checkBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseLastName_textBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseFirstName_textBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseMiddleName_textBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseSuffix_comboBox);
            this.Spouse_groupBox.Controls.Add(this.SpousePrefix_comboBox);
            this.Spouse_groupBox.Location = new System.Drawing.Point(118, 263);
            this.Spouse_groupBox.Name = "Spouse_groupBox";
            this.Spouse_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Spouse_groupBox.TabIndex = 1;
            this.Spouse_groupBox.TabStop = false;
            this.Spouse_groupBox.Text = "Spouse";
            // 
            // SpouseIntegrated_checkBox
            // 
            this.SpouseIntegrated_checkBox.AutoSize = true;
            this.SpouseIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpouseIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SpouseIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.SpouseIntegrated_checkBox.Name = "SpouseIntegrated_checkBox";
            this.SpouseIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.SpouseIntegrated_checkBox.TabIndex = 82;
            this.SpouseIntegrated_checkBox.TabStop = false;
            this.SpouseIntegrated_checkBox.Text = "Integrated";
            this.SpouseIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // SpouseLastName_textBox
            // 
            this.SpouseLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseLastName_textBox.Name = "SpouseLastName_textBox";
            this.SpouseLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseLastName_textBox.TabIndex = 21;
            this.SpouseLastName_textBox.Leave += new System.EventHandler(this.SpouseLastNameTextBox_Click);
            // 
            // SpouseFirstName_textBox
            // 
            this.SpouseFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.SpouseFirstName_textBox.Name = "SpouseFirstName_textBox";
            this.SpouseFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFirstName_textBox.TabIndex = 22;
            // 
            // SpouseMiddleName_textBox
            // 
            this.SpouseMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.SpouseMiddleName_textBox.Name = "SpouseMiddleName_textBox";
            this.SpouseMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMiddleName_textBox.TabIndex = 23;
            // 
            // SpouseSuffix_comboBox
            // 
            this.SpouseSuffix_comboBox.FormattingEnabled = true;
            this.SpouseSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.SpouseSuffix_comboBox.Name = "SpouseSuffix_comboBox";
            this.SpouseSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseSuffix_comboBox.TabIndex = 24;
            // 
            // SpousePrefix_comboBox
            // 
            this.SpousePrefix_comboBox.FormattingEnabled = true;
            this.SpousePrefix_comboBox.Location = new System.Drawing.Point(22, 143);
            this.SpousePrefix_comboBox.Name = "SpousePrefix_comboBox";
            this.SpousePrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpousePrefix_comboBox.TabIndex = 25;
            // 
            // SpouseFather_groupBox
            // 
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherIntegrated_checkBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherLastName_textBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherFirstName_textBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherMiddleName_textBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherSuffix_comboBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherPrefix_comboBox);
            this.SpouseFather_groupBox.Location = new System.Drawing.Point(293, 263);
            this.SpouseFather_groupBox.Name = "SpouseFather_groupBox";
            this.SpouseFather_groupBox.Size = new System.Drawing.Size(144, 197);
            this.SpouseFather_groupBox.TabIndex = 8;
            this.SpouseFather_groupBox.TabStop = false;
            this.SpouseFather_groupBox.Text = "Name of Father";
            // 
            // SpouseFatherIntegrated_checkBox
            // 
            this.SpouseFatherIntegrated_checkBox.AutoSize = true;
            this.SpouseFatherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpouseFatherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SpouseFatherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.SpouseFatherIntegrated_checkBox.Name = "SpouseFatherIntegrated_checkBox";
            this.SpouseFatherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.SpouseFatherIntegrated_checkBox.TabIndex = 83;
            this.SpouseFatherIntegrated_checkBox.TabStop = false;
            this.SpouseFatherIntegrated_checkBox.Text = "Integrated";
            this.SpouseFatherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // SpouseFatherLastName_textBox
            // 
            this.SpouseFatherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseFatherLastName_textBox.Name = "SpouseFatherLastName_textBox";
            this.SpouseFatherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFatherLastName_textBox.TabIndex = 26;
            // 
            // SpouseFatherFirstName_textBox
            // 
            this.SpouseFatherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.SpouseFatherFirstName_textBox.Name = "SpouseFatherFirstName_textBox";
            this.SpouseFatherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFatherFirstName_textBox.TabIndex = 27;
            // 
            // SpouseFatherMiddleName_textBox
            // 
            this.SpouseFatherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.SpouseFatherMiddleName_textBox.Name = "SpouseFatherMiddleName_textBox";
            this.SpouseFatherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFatherMiddleName_textBox.TabIndex = 28;
            // 
            // SpouseFatherSuffix_comboBox
            // 
            this.SpouseFatherSuffix_comboBox.FormattingEnabled = true;
            this.SpouseFatherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.SpouseFatherSuffix_comboBox.Name = "SpouseFatherSuffix_comboBox";
            this.SpouseFatherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseFatherSuffix_comboBox.TabIndex = 29;
            // 
            // SpouseFatherPrefix_comboBox
            // 
            this.SpouseFatherPrefix_comboBox.FormattingEnabled = true;
            this.SpouseFatherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.SpouseFatherPrefix_comboBox.Name = "SpouseFatherPrefix_comboBox";
            this.SpouseFatherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseFatherPrefix_comboBox.TabIndex = 30;
            // 
            // SpouseMother_groupBox
            // 
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherIntegrated_checkBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherLastName_textBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherFirstName_textBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherMiddleName_textBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherSuffix_comboBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherPrefix_comboBox);
            this.SpouseMother_groupBox.Location = new System.Drawing.Point(459, 263);
            this.SpouseMother_groupBox.Name = "SpouseMother_groupBox";
            this.SpouseMother_groupBox.Size = new System.Drawing.Size(144, 197);
            this.SpouseMother_groupBox.TabIndex = 9;
            this.SpouseMother_groupBox.TabStop = false;
            this.SpouseMother_groupBox.Text = "Mother Maiden Name";
            // 
            // SpouseMotherIntegrated_checkBox
            // 
            this.SpouseMotherIntegrated_checkBox.AutoSize = true;
            this.SpouseMotherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpouseMotherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SpouseMotherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.SpouseMotherIntegrated_checkBox.Name = "SpouseMotherIntegrated_checkBox";
            this.SpouseMotherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.SpouseMotherIntegrated_checkBox.TabIndex = 84;
            this.SpouseMotherIntegrated_checkBox.TabStop = false;
            this.SpouseMotherIntegrated_checkBox.Text = "Integrated";
            this.SpouseMotherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // SpouseMotherLastName_textBox
            // 
            this.SpouseMotherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseMotherLastName_textBox.Name = "SpouseMotherLastName_textBox";
            this.SpouseMotherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMotherLastName_textBox.TabIndex = 31;
            // 
            // SpouseMotherFirstName_textBox
            // 
            this.SpouseMotherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.SpouseMotherFirstName_textBox.Name = "SpouseMotherFirstName_textBox";
            this.SpouseMotherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMotherFirstName_textBox.TabIndex = 32;
            // 
            // SpouseMotherMiddleName_textBox
            // 
            this.SpouseMotherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.SpouseMotherMiddleName_textBox.Name = "SpouseMotherMiddleName_textBox";
            this.SpouseMotherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMotherMiddleName_textBox.TabIndex = 33;
            // 
            // SpouseMotherSuffix_comboBox
            // 
            this.SpouseMotherSuffix_comboBox.FormattingEnabled = true;
            this.SpouseMotherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.SpouseMotherSuffix_comboBox.Name = "SpouseMotherSuffix_comboBox";
            this.SpouseMotherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseMotherSuffix_comboBox.TabIndex = 34;
            // 
            // SpouseMotherPrefix_comboBox
            // 
            this.SpouseMotherPrefix_comboBox.FormattingEnabled = true;
            this.SpouseMotherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.SpouseMotherPrefix_comboBox.Name = "SpouseMotherPrefix_comboBox";
            this.SpouseMotherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseMotherPrefix_comboBox.TabIndex = 35;
            // 
            // SpouseLastName_label
            // 
            this.SpouseLastName_label.AutoSize = true;
            this.SpouseLastName_label.Location = new System.Drawing.Point(31, 290);
            this.SpouseLastName_label.Name = "SpouseLastName_label";
            this.SpouseLastName_label.Size = new System.Drawing.Size(58, 13);
            this.SpouseLastName_label.TabIndex = 86;
            this.SpouseLastName_label.Text = "Last Name";
            // 
            // SpouseFirstName_label
            // 
            this.SpouseFirstName_label.AutoSize = true;
            this.SpouseFirstName_label.Location = new System.Drawing.Point(31, 319);
            this.SpouseFirstName_label.Name = "SpouseFirstName_label";
            this.SpouseFirstName_label.Size = new System.Drawing.Size(57, 13);
            this.SpouseFirstName_label.TabIndex = 87;
            this.SpouseFirstName_label.Text = "First Name";
            // 
            // SpouseMiddleName_label
            // 
            this.SpouseMiddleName_label.AutoSize = true;
            this.SpouseMiddleName_label.Location = new System.Drawing.Point(31, 348);
            this.SpouseMiddleName_label.Name = "SpouseMiddleName_label";
            this.SpouseMiddleName_label.Size = new System.Drawing.Size(69, 13);
            this.SpouseMiddleName_label.TabIndex = 88;
            this.SpouseMiddleName_label.Text = "Middle Name";
            // 
            // SpouseSuffix_label
            // 
            this.SpouseSuffix_label.AutoSize = true;
            this.SpouseSuffix_label.Location = new System.Drawing.Point(31, 377);
            this.SpouseSuffix_label.Name = "SpouseSuffix_label";
            this.SpouseSuffix_label.Size = new System.Drawing.Size(33, 13);
            this.SpouseSuffix_label.TabIndex = 89;
            this.SpouseSuffix_label.Text = "Suffix";
            // 
            // SpousePrefix_label
            // 
            this.SpousePrefix_label.AutoSize = true;
            this.SpousePrefix_label.Location = new System.Drawing.Point(31, 406);
            this.SpousePrefix_label.Name = "SpousePrefix_label";
            this.SpousePrefix_label.Size = new System.Drawing.Size(33, 13);
            this.SpousePrefix_label.TabIndex = 90;
            this.SpousePrefix_label.Text = "Prefix";
            // 
            // SearchRequest_groupBox
            // 
            this.SearchRequest_groupBox.Controls.Add(this.SearchAllNamesSpouse_radioButton);
            this.SearchRequest_groupBox.Controls.Add(this.SearchAllNamesFather_radioButton);
            this.SearchRequest_groupBox.Controls.Add(this.SearchAllNames_radioButton);
            this.SearchRequest_groupBox.Location = new System.Drawing.Point(11, 510);
            this.SearchRequest_groupBox.Name = "SearchRequest_groupBox";
            this.SearchRequest_groupBox.Size = new System.Drawing.Size(288, 111);
            this.SearchRequest_groupBox.TabIndex = 91;
            this.SearchRequest_groupBox.TabStop = false;
            this.SearchRequest_groupBox.Text = "Search Options";
            // 
            // SearchAllNamesSpouse_radioButton
            // 
            this.SearchAllNamesSpouse_radioButton.AutoSize = true;
            this.SearchAllNamesSpouse_radioButton.Location = new System.Drawing.Point(6, 19);
            this.SearchAllNamesSpouse_radioButton.Name = "SearchAllNamesSpouse_radioButton";
            this.SearchAllNamesSpouse_radioButton.Size = new System.Drawing.Size(226, 17);
            this.SearchAllNamesSpouse_radioButton.TabIndex = 7;
            this.SearchAllNamesSpouse_radioButton.Text = "Search All Names based on Spouse Name";
            this.SearchAllNamesSpouse_radioButton.UseVisualStyleBackColor = true;
            // 
            // SearchAllNamesFather_radioButton
            // 
            this.SearchAllNamesFather_radioButton.AutoSize = true;
            this.SearchAllNamesFather_radioButton.Location = new System.Drawing.Point(6, 79);
            this.SearchAllNamesFather_radioButton.Name = "SearchAllNamesFather_radioButton";
            this.SearchAllNamesFather_radioButton.Size = new System.Drawing.Size(220, 17);
            this.SearchAllNamesFather_radioButton.TabIndex = 6;
            this.SearchAllNamesFather_radioButton.Text = "Search All Names based on Father Name";
            this.SearchAllNamesFather_radioButton.UseVisualStyleBackColor = true;
            // 
            // SearchAllNames_radioButton
            // 
            this.SearchAllNames_radioButton.AutoSize = true;
            this.SearchAllNames_radioButton.Checked = true;
            this.SearchAllNames_radioButton.Location = new System.Drawing.Point(6, 56);
            this.SearchAllNames_radioButton.Name = "SearchAllNames_radioButton";
            this.SearchAllNames_radioButton.Size = new System.Drawing.Size(109, 17);
            this.SearchAllNames_radioButton.TabIndex = 5;
            this.SearchAllNames_radioButton.TabStop = true;
            this.SearchAllNames_radioButton.Text = "Search All Names";
            this.SearchAllNames_radioButton.UseVisualStyleBackColor = true;
            // 
            // LastMother_button
            // 
            this.LastMother_button.Location = new System.Drawing.Point(472, 510);
            this.LastMother_button.Name = "LastMother_button";
            this.LastMother_button.Size = new System.Drawing.Size(117, 23);
            this.LastMother_button.TabIndex = 92;
            this.LastMother_button.TabStop = false;
            this.LastMother_button.Text = "Last Mother";
            this.LastMother_button.UseVisualStyleBackColor = true;
            this.LastMother_button.Click += new System.EventHandler(this.LastMother_button_Click);
            // 
            // LastSpouse_button
            // 
            this.LastSpouse_button.Location = new System.Drawing.Point(472, 540);
            this.LastSpouse_button.Name = "LastSpouse_button";
            this.LastSpouse_button.Size = new System.Drawing.Size(117, 23);
            this.LastSpouse_button.TabIndex = 93;
            this.LastSpouse_button.TabStop = false;
            this.LastSpouse_button.Text = "Last Spouse";
            this.LastSpouse_button.UseVisualStyleBackColor = true;
            this.LastSpouse_button.Click += new System.EventHandler(this.LastSpouse_button_Click);
            // 
            // LastSpouseMother_button
            // 
            this.LastSpouseMother_button.Location = new System.Drawing.Point(472, 570);
            this.LastSpouseMother_button.Name = "LastSpouseMother_button";
            this.LastSpouseMother_button.Size = new System.Drawing.Size(117, 23);
            this.LastSpouseMother_button.TabIndex = 94;
            this.LastSpouseMother_button.TabStop = false;
            this.LastSpouseMother_button.Text = "Last Spouse Mother";
            this.LastSpouseMother_button.UseVisualStyleBackColor = true;
            this.LastSpouseMother_button.Click += new System.EventHandler(this.LastSpouseMother_button_Click);
            // 
            // Sex_groupBox
            // 
            this.Sex_groupBox.Controls.Add(this.Female_radioButton);
            this.Sex_groupBox.Controls.Add(this.Male_radioButton);
            this.Sex_groupBox.Location = new System.Drawing.Point(820, 269);
            this.Sex_groupBox.Name = "Sex_groupBox";
            this.Sex_groupBox.Size = new System.Drawing.Size(144, 56);
            this.Sex_groupBox.TabIndex = 10;
            this.Sex_groupBox.TabStop = false;
            this.Sex_groupBox.Text = "Sex";
            // 
            // Female_radioButton
            // 
            this.Female_radioButton.AutoSize = true;
            this.Female_radioButton.Location = new System.Drawing.Point(70, 22);
            this.Female_radioButton.Name = "Female_radioButton";
            this.Female_radioButton.Size = new System.Drawing.Size(59, 17);
            this.Female_radioButton.TabIndex = 1;
            this.Female_radioButton.TabStop = true;
            this.Female_radioButton.Text = "Female";
            this.Female_radioButton.UseVisualStyleBackColor = true;
            // 
            // Male_radioButton
            // 
            this.Male_radioButton.AutoSize = true;
            this.Male_radioButton.Location = new System.Drawing.Point(16, 21);
            this.Male_radioButton.Name = "Male_radioButton";
            this.Male_radioButton.Size = new System.Drawing.Size(48, 17);
            this.Male_radioButton.TabIndex = 0;
            this.Male_radioButton.TabStop = true;
            this.Male_radioButton.Text = "Male";
            this.Male_radioButton.UseVisualStyleBackColor = true;
            // 
            // ReturnToList_button
            // 
            this.ReturnToList_button.Location = new System.Drawing.Point(306, 599);
            this.ReturnToList_button.Name = "ReturnToList_button";
            this.ReturnToList_button.Size = new System.Drawing.Size(117, 23);
            this.ReturnToList_button.TabIndex = 95;
            this.ReturnToList_button.TabStop = false;
            this.ReturnToList_button.Text = "Return To List";
            this.ReturnToList_button.UseVisualStyleBackColor = true;
            this.ReturnToList_button.Click += new System.EventHandler(this.ReturnToList_Click);
            // 
            // ExcludeFromSite_checkBox
            // 
            this.ExcludeFromSite_checkBox.AutoSize = true;
            this.ExcludeFromSite_checkBox.Location = new System.Drawing.Point(12, 484);
            this.ExcludeFromSite_checkBox.Name = "ExcludeFromSite_checkBox";
            this.ExcludeFromSite_checkBox.Size = new System.Drawing.Size(108, 17);
            this.ExcludeFromSite_checkBox.TabIndex = 96;
            this.ExcludeFromSite_checkBox.TabStop = false;
            this.ExcludeFromSite_checkBox.Text = "Exclude from Site";
            this.ExcludeFromSite_checkBox.UseVisualStyleBackColor = true;
            this.ExcludeFromSite_checkBox.CheckedChanged += new System.EventHandler(this.ExcludeFromSiteChanged_click);
            // 
            // CalcBornDate_groupBox
            // 
            this.CalcBornDate_groupBox.Controls.Add(this.CalcDateMonth_textBox);
            this.CalcBornDate_groupBox.Controls.Add(this.CalcDateYear_textBox);
            this.CalcBornDate_groupBox.Controls.Add(this.label12);
            this.CalcBornDate_groupBox.Controls.Add(this.label14);
            this.CalcBornDate_groupBox.Controls.Add(this.CalcDateDay_textBox);
            this.CalcBornDate_groupBox.Enabled = false;
            this.CalcBornDate_groupBox.Location = new System.Drawing.Point(820, 340);
            this.CalcBornDate_groupBox.Name = "CalcBornDate_groupBox";
            this.CalcBornDate_groupBox.Size = new System.Drawing.Size(169, 56);
            this.CalcBornDate_groupBox.TabIndex = 97;
            this.CalcBornDate_groupBox.TabStop = false;
            this.CalcBornDate_groupBox.Text = "Calculated Born Date";
            // 
            // CalcDateMonth_textBox
            // 
            this.CalcDateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.CalcDateMonth_textBox.Name = "CalcDateMonth_textBox";
            this.CalcDateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.CalcDateMonth_textBox.TabIndex = 18;
            // 
            // CalcDateYear_textBox
            // 
            this.CalcDateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.CalcDateYear_textBox.Name = "CalcDateYear_textBox";
            this.CalcDateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.CalcDateYear_textBox.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(48, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "/";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(91, 28);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(12, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "/";
            // 
            // CalcDateDay_textBox
            // 
            this.CalcDateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.CalcDateDay_textBox.Name = "CalcDateDay_textBox";
            this.CalcDateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.CalcDateDay_textBox.TabIndex = 19;
            // 
            // BirthDate_groupBox
            // 
            this.BirthDate_groupBox.Controls.Add(this.BornDateMonth_textBox);
            this.BirthDate_groupBox.Controls.Add(this.BornDateYear_textBox);
            this.BirthDate_groupBox.Controls.Add(this.label15);
            this.BirthDate_groupBox.Controls.Add(this.label16);
            this.BirthDate_groupBox.Controls.Add(this.BornDateDay_textBox);
            this.BirthDate_groupBox.Location = new System.Drawing.Point(628, 477);
            this.BirthDate_groupBox.Name = "BirthDate_groupBox";
            this.BirthDate_groupBox.Size = new System.Drawing.Size(169, 56);
            this.BirthDate_groupBox.TabIndex = 145;
            this.BirthDate_groupBox.TabStop = false;
            this.BirthDate_groupBox.Text = "Birth Date";
            // 
            // BornDateMonth_textBox
            // 
            this.BornDateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.BornDateMonth_textBox.Name = "BornDateMonth_textBox";
            this.BornDateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.BornDateMonth_textBox.TabIndex = 148;
            // 
            // BornDateYear_textBox
            // 
            this.BornDateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.BornDateYear_textBox.Name = "BornDateYear_textBox";
            this.BornDateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.BornDateYear_textBox.TabIndex = 150;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(48, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(12, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "/";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(91, 28);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(12, 13);
            this.label16.TabIndex = 22;
            this.label16.Text = "/";
            // 
            // BornDateDay_textBox
            // 
            this.BornDateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.BornDateDay_textBox.Name = "BornDateDay_textBox";
            this.BornDateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.BornDateDay_textBox.TabIndex = 149;
            // 
            // SpouseBornDateDay_textBox
            // 
            this.SpouseBornDateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.SpouseBornDateDay_textBox.Name = "SpouseBornDateDay_textBox";
            this.SpouseBornDateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.SpouseBornDateDay_textBox.TabIndex = 153;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(91, 28);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(12, 13);
            this.label18.TabIndex = 22;
            this.label18.Text = "/";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(48, 28);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(12, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "/";
            // 
            // SpouseBornDateYear_textBox
            // 
            this.SpouseBornDateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.SpouseBornDateYear_textBox.Name = "SpouseBornDateYear_textBox";
            this.SpouseBornDateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.SpouseBornDateYear_textBox.TabIndex = 154;
            // 
            // SpouseBornDateMonth_textBox
            // 
            this.SpouseBornDateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseBornDateMonth_textBox.Name = "SpouseBornDateMonth_textBox";
            this.SpouseBornDateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.SpouseBornDateMonth_textBox.TabIndex = 152;
            // 
            // SpouseBirthDate_groupBox
            // 
            this.SpouseBirthDate_groupBox.Controls.Add(this.SpouseBornDateMonth_textBox);
            this.SpouseBirthDate_groupBox.Controls.Add(this.SpouseBornDateYear_textBox);
            this.SpouseBirthDate_groupBox.Controls.Add(this.label17);
            this.SpouseBirthDate_groupBox.Controls.Add(this.label18);
            this.SpouseBirthDate_groupBox.Controls.Add(this.SpouseBornDateDay_textBox);
            this.SpouseBirthDate_groupBox.Location = new System.Drawing.Point(628, 565);
            this.SpouseBirthDate_groupBox.Name = "SpouseBirthDate_groupBox";
            this.SpouseBirthDate_groupBox.Size = new System.Drawing.Size(169, 56);
            this.SpouseBirthDate_groupBox.TabIndex = 151;
            this.SpouseBirthDate_groupBox.TabStop = false;
            this.SpouseBirthDate_groupBox.Text = "Spouse Birth Date";
            // 
            // SpouseAge_groupBox
            // 
            this.SpouseAge_groupBox.Controls.Add(this.label19);
            this.SpouseAge_groupBox.Controls.Add(this.label20);
            this.SpouseAge_groupBox.Controls.Add(this.SpouseAgeDays_textBox);
            this.SpouseAge_groupBox.Controls.Add(this.SpouseAgeMonths_textBox);
            this.SpouseAge_groupBox.Controls.Add(this.label21);
            this.SpouseAge_groupBox.Controls.Add(this.SpouseAgeYears_textBox);
            this.SpouseAge_groupBox.Location = new System.Drawing.Point(820, 570);
            this.SpouseAge_groupBox.Name = "SpouseAge_groupBox";
            this.SpouseAge_groupBox.Size = new System.Drawing.Size(169, 56);
            this.SpouseAge_groupBox.TabIndex = 124;
            this.SpouseAge_groupBox.TabStop = false;
            this.SpouseAge_groupBox.Text = "Spouse Age";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(138, 28);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(20, 13);
            this.label19.TabIndex = 75;
            this.label19.Text = "Ds";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(96, 28);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(21, 13);
            this.label20.TabIndex = 74;
            this.label20.Text = "Ms";
            // 
            // SpouseAgeDays_textBox
            // 
            this.SpouseAgeDays_textBox.Location = new System.Drawing.Point(116, 25);
            this.SpouseAgeDays_textBox.Name = "SpouseAgeDays_textBox";
            this.SpouseAgeDays_textBox.Size = new System.Drawing.Size(22, 20);
            this.SpouseAgeDays_textBox.TabIndex = 127;
            // 
            // SpouseAgeMonths_textBox
            // 
            this.SpouseAgeMonths_textBox.Location = new System.Drawing.Point(74, 25);
            this.SpouseAgeMonths_textBox.Name = "SpouseAgeMonths_textBox";
            this.SpouseAgeMonths_textBox.Size = new System.Drawing.Size(22, 20);
            this.SpouseAgeMonths_textBox.TabIndex = 126;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(50, 28);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(19, 13);
            this.label21.TabIndex = 71;
            this.label21.Text = "Ys";
            // 
            // SpouseAgeYears_textBox
            // 
            this.SpouseAgeYears_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseAgeYears_textBox.Name = "SpouseAgeYears_textBox";
            this.SpouseAgeYears_textBox.Size = new System.Drawing.Size(28, 20);
            this.SpouseAgeYears_textBox.TabIndex = 125;
            // 
            // FVitalRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1010, 654);
            this.Controls.Add(this.SpouseAge_groupBox);
            this.Controls.Add(this.SpouseBirthDate_groupBox);
            this.Controls.Add(this.BirthDate_groupBox);
            this.Controls.Add(this.CalcBornDate_groupBox);
            this.Controls.Add(this.ExcludeFromSite_checkBox);
            this.Controls.Add(this.ReturnToList_button);
            this.Controls.Add(this.Sex_groupBox);
            this.Controls.Add(this.LastSpouseMother_button);
            this.Controls.Add(this.LastSpouse_button);
            this.Controls.Add(this.LastMother_button);
            this.Controls.Add(this.SearchRequest_groupBox);
            this.Controls.Add(this.SpousePrefix_label);
            this.Controls.Add(this.SpouseSuffix_label);
            this.Controls.Add(this.SpouseMiddleName_label);
            this.Controls.Add(this.SpouseFirstName_label);
            this.Controls.Add(this.SpouseLastName_label);
            this.Controls.Add(this.SpouseMother_groupBox);
            this.Controls.Add(this.SpouseFather_groupBox);
            this.Controls.Add(this.Spouse_groupBox);
            this.Controls.Add(this.Similar_button);
            this.Controls.Add(this.StartingWith_button);
            this.Controls.Add(this.SearchPartial_Button);
            this.Controls.Add(this.SearchAll_button);
            this.Controls.Add(this.Notes_label);
            this.Controls.Add(this.Notes_TextBox);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Burial_groupBox);
            this.Controls.Add(this.Age_groupBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Name_groupBox);
            this.Controls.Add(this.Father_groupBox);
            this.Controls.Add(this.Mother_groupBox);
            this.Controls.Add(this.BookPage_groupBox);
            this.Controls.Add(this.Date_groupBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FVitalRecord";
            this.Text = "Vital Record";
            this.Date_groupBox.ResumeLayout(false);
            this.Date_groupBox.PerformLayout();
            this.BookPage_groupBox.ResumeLayout(false);
            this.BookPage_groupBox.PerformLayout();
            this.Name_groupBox.ResumeLayout(false);
            this.Name_groupBox.PerformLayout();
            this.Mother_groupBox.ResumeLayout(false);
            this.Mother_groupBox.PerformLayout();
            this.Father_groupBox.ResumeLayout(false);
            this.Father_groupBox.PerformLayout();
            this.Age_groupBox.ResumeLayout(false);
            this.Age_groupBox.PerformLayout();
            this.Burial_groupBox.ResumeLayout(false);
            this.Burial_groupBox.PerformLayout();
            this.Disposition_groupBox.ResumeLayout(false);
            this.Disposition_groupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Spouse_groupBox.ResumeLayout(false);
            this.Spouse_groupBox.PerformLayout();
            this.SpouseFather_groupBox.ResumeLayout(false);
            this.SpouseFather_groupBox.PerformLayout();
            this.SpouseMother_groupBox.ResumeLayout(false);
            this.SpouseMother_groupBox.PerformLayout();
            this.SearchRequest_groupBox.ResumeLayout(false);
            this.SearchRequest_groupBox.PerformLayout();
            this.Sex_groupBox.ResumeLayout(false);
            this.Sex_groupBox.PerformLayout();
            this.CalcBornDate_groupBox.ResumeLayout(false);
            this.CalcBornDate_groupBox.PerformLayout();
            this.BirthDate_groupBox.ResumeLayout(false);
            this.BirthDate_groupBox.PerformLayout();
            this.SpouseBirthDate_groupBox.ResumeLayout(false);
            this.SpouseBirthDate_groupBox.PerformLayout();
            this.SpouseAge_groupBox.ResumeLayout(false);
            this.SpouseAge_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private Button ReturnToList_button;
        protected CheckBox ExcludeFromSite_checkBox;
        private GroupBox CalcBornDate_groupBox;
        private TextBox CalcDateMonth_textBox;
        private TextBox CalcDateYear_textBox;
        private Label label12;
        private Label label14;
        private TextBox CalcDateDay_textBox;
        private RadioButton SearchAllNamesSpouse_radioButton;
        private GroupBox BirthDate_groupBox;
        private TextBox BornDateMonth_textBox;
        private TextBox BornDateYear_textBox;
        private Label label15;
        private Label label16;
        private TextBox BornDateDay_textBox;
        private TextBox SpouseBornDateDay_textBox;
        private Label label18;
        private Label label17;
        private TextBox SpouseBornDateYear_textBox;
        private TextBox SpouseBornDateMonth_textBox;
        private GroupBox SpouseBirthDate_groupBox;
        private GroupBox SpouseAge_groupBox;
        private Label label19;
        private Label label20;
        private TextBox SpouseAgeDays_textBox;
        private TextBox SpouseAgeMonths_textBox;
        private Label label21;
        private TextBox SpouseAgeYears_textBox;


    }
}
