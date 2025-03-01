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
    partial class FCemeteryRecord
    {
        private MenuStrip menuStrip1;
        private CheckBox NameIntegrated_checkBox;
        private CheckBox MotherIntegrated_checkBox;
        private CheckBox FatherIntegrated_checkBox;
        private CheckBox SpouseIntegrated_checkBox;
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
        private RadioButton Buried_radioButton;
        private RadioButton Other_radioButton;
        private RadioButton Cremated_radioButton;
        private Label SpouseLastName_label;
        private Label SpouseFirstName_label;
        private Label SpouseMiddleName_label;
        private Label SpouseSuffix_label;
        private Label SpousePrefix_label;
        private GroupBox SearchRequest_groupBox;
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
        private System.Windows.Forms.TextBox DiedDateMonth_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DiedDateDay_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DiedDateYear_textBox;
        private System.Windows.Forms.GroupBox DiedDate_groupBox;
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
            this.DiedDateMonth_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DiedDateDay_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DiedDateYear_textBox = new System.Windows.Forms.TextBox();
            this.DiedDate_groupBox = new System.Windows.Forms.GroupBox();
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
            this.SearchPartial_Button = new System.Windows.Forms.Button();
            this.StartingWith_button = new System.Windows.Forms.Button();
            this.Similar_button = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
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
            this.SpouseLastName_label = new System.Windows.Forms.Label();
            this.SpouseFirstName_label = new System.Windows.Forms.Label();
            this.SpouseMiddleName_label = new System.Windows.Forms.Label();
            this.SpouseSuffix_label = new System.Windows.Forms.Label();
            this.SpousePrefix_label = new System.Windows.Forms.Label();
            this.SearchRequest_groupBox = new System.Windows.Forms.GroupBox();
            this.SearchAllNamesSpouse_radioButton = new System.Windows.Forms.RadioButton();
            this.SearchAllNamesFather_radioButton = new System.Windows.Forms.RadioButton();
            this.SearchAllNames_radioButton = new System.Windows.Forms.RadioButton();
            this.Sex_groupBox = new System.Windows.Forms.GroupBox();
            this.Female_radioButton = new System.Windows.Forms.RadioButton();
            this.Male_radioButton = new System.Windows.Forms.RadioButton();
            this.Unknown_radioButton = new System.Windows.Forms.RadioButton();
            this.BornDategroupBox = new System.Windows.Forms.GroupBox();
            this.BornDateMonth_textBox = new System.Windows.Forms.TextBox();
            this.BornDateYear_textBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.BornDateDay_textBox = new System.Windows.Forms.TextBox();
            this.PersonNameOnGrave_textBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.SpouseNameOnGrave_textBox = new System.Windows.Forms.TextBox();
            this.MotherNameOnGrave_textBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.FatherNameOnGrave_textBox = new System.Windows.Forms.TextBox();
            this.Epitaph_TextBox = new System.Windows.Forms.RichTextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.ReturnToListbutton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CalcBornDateMonth_textBox = new System.Windows.Forms.TextBox();
            this.CalcBornDateYear_textBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.CalcBornDateDay_textBox = new System.Windows.Forms.TextBox();
            this.DiedDate_groupBox.SuspendLayout();
            this.Name_groupBox.SuspendLayout();
            this.Mother_groupBox.SuspendLayout();
            this.Father_groupBox.SuspendLayout();
            this.Age_groupBox.SuspendLayout();
            this.Burial_groupBox.SuspendLayout();
            this.Disposition_groupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.Spouse_groupBox.SuspendLayout();
            this.SearchRequest_groupBox.SuspendLayout();
            this.Sex_groupBox.SuspendLayout();
            this.BornDategroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.FirstName_textBox.Leave += new System.EventHandler(this.FirstNameLeave_Click);
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
            // DiedDateMonth_textBox
            // 
            this.DiedDateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.DiedDateMonth_textBox.Name = "DiedDateMonth_textBox";
            this.DiedDateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.DiedDateMonth_textBox.TabIndex = 18;
            this.DiedDateMonth_textBox.Leave += new System.EventHandler(this.ValidateMonth_Click);
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
            // DiedDateDay_textBox
            // 
            this.DiedDateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.DiedDateDay_textBox.Name = "DiedDateDay_textBox";
            this.DiedDateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.DiedDateDay_textBox.TabIndex = 19;
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
            // DiedDateYear_textBox
            // 
            this.DiedDateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.DiedDateYear_textBox.Name = "DiedDateYear_textBox";
            this.DiedDateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.DiedDateYear_textBox.TabIndex = 20;
            // 
            // DiedDate_groupBox
            // 
            this.DiedDate_groupBox.Controls.Add(this.DiedDateMonth_textBox);
            this.DiedDate_groupBox.Controls.Add(this.DiedDateYear_textBox);
            this.DiedDate_groupBox.Controls.Add(this.label4);
            this.DiedDate_groupBox.Controls.Add(this.label5);
            this.DiedDate_groupBox.Controls.Add(this.DiedDateDay_textBox);
            this.DiedDate_groupBox.Location = new System.Drawing.Point(491, 49);
            this.DiedDate_groupBox.Name = "DiedDate_groupBox";
            this.DiedDate_groupBox.Size = new System.Drawing.Size(169, 56);
            this.DiedDate_groupBox.TabIndex = 3;
            this.DiedDate_groupBox.TabStop = false;
            this.DiedDate_groupBox.Text = "Died Date";
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
            this.Mother_groupBox.Location = new System.Drawing.Point(306, 330);
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
            this.Father_groupBox.Location = new System.Drawing.Point(118, 330);
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
            this.label3.Location = new System.Drawing.Point(12, 167);
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
            this.Age_groupBox.Location = new System.Drawing.Point(491, 132);
            this.Age_groupBox.Name = "Age_groupBox";
            this.Age_groupBox.Size = new System.Drawing.Size(169, 56);
            this.Age_groupBox.TabIndex = 4;
            this.Age_groupBox.TabStop = false;
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
            this.AgeDays_textBox.TabIndex = 23;
            // 
            // AgeMonths_textBox
            // 
            this.AgeMonths_textBox.Location = new System.Drawing.Point(74, 25);
            this.AgeMonths_textBox.Name = "AgeMonths_textBox";
            this.AgeMonths_textBox.Size = new System.Drawing.Size(22, 20);
            this.AgeMonths_textBox.TabIndex = 22;
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
            this.AgeYears_textBox.TabIndex = 21;
            // 
            // Burial_groupBox
            // 
            this.Burial_groupBox.Controls.Add(this.label13);
            this.Burial_groupBox.Controls.Add(this.Disposition_groupBox);
            this.Burial_groupBox.Controls.Add(this.Cemetery_comboBox);
            this.Burial_groupBox.Controls.Add(this.LotNumber_textBox);
            this.Burial_groupBox.Controls.Add(this.label3);
            this.Burial_groupBox.Location = new System.Drawing.Point(491, 358);
            this.Burial_groupBox.Name = "Burial_groupBox";
            this.Burial_groupBox.Size = new System.Drawing.Size(169, 197);
            this.Burial_groupBox.TabIndex = 11;
            this.Burial_groupBox.TabStop = false;
            this.Burial_groupBox.Text = "Burial";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 115);
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
            this.Disposition_groupBox.Location = new System.Drawing.Point(36, 25);
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
            this.Cemetery_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Cemetery_comboBox.FormattingEnabled = true;
            this.Cemetery_comboBox.Location = new System.Drawing.Point(15, 137);
            this.Cemetery_comboBox.Name = "Cemetery_comboBox";
            this.Cemetery_comboBox.Size = new System.Drawing.Size(143, 21);
            this.Cemetery_comboBox.TabIndex = 37;
            // 
            // LotNumber_textBox
            // 
            this.LotNumber_textBox.Location = new System.Drawing.Point(50, 164);
            this.LotNumber_textBox.Name = "LotNumber_textBox";
            this.LotNumber_textBox.Size = new System.Drawing.Size(108, 20);
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
            this.Save_button.Location = new System.Drawing.Point(516, 604);
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
            this.Notes_TextBox.Location = new System.Drawing.Point(694, 369);
            this.Notes_TextBox.Name = "Notes_TextBox";
            this.Notes_TextBox.Size = new System.Drawing.Size(219, 186);
            this.Notes_TextBox.TabIndex = 36;
            this.Notes_TextBox.Text = "";
            // 
            // Notes_label
            // 
            this.Notes_label.AutoSize = true;
            this.Notes_label.Location = new System.Drawing.Point(691, 319);
            this.Notes_label.Name = "Notes_label";
            this.Notes_label.Size = new System.Drawing.Size(35, 13);
            this.Notes_label.TabIndex = 75;
            this.Notes_label.Text = "Notes";
            // 
            // SearchPartial_Button
            // 
            this.SearchPartial_Button.Location = new System.Drawing.Point(321, 575);
            this.SearchPartial_Button.Name = "SearchPartial_Button";
            this.SearchPartial_Button.Size = new System.Drawing.Size(117, 23);
            this.SearchPartial_Button.TabIndex = 77;
            this.SearchPartial_Button.TabStop = false;
            this.SearchPartial_Button.Text = "Partial Name";
            this.SearchPartial_Button.UseVisualStyleBackColor = true;
            this.SearchPartial_Button.Click += new System.EventHandler(this.SearchPartial_button_Click);
            // 
            // StartingWith_button
            // 
            this.StartingWith_button.Location = new System.Drawing.Point(321, 604);
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
            this.Similar_button.Location = new System.Drawing.Point(321, 633);
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
            this.integrationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(940, 24);
            this.menuStrip1.TabIndex = 80;
            this.menuStrip1.Text = "menuStrip1";
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
            this.Spouse_groupBox.Location = new System.Drawing.Point(306, 49);
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
            // SpouseLastName_label
            // 
            this.SpouseLastName_label.AutoSize = true;
            this.SpouseLastName_label.Location = new System.Drawing.Point(30, 355);
            this.SpouseLastName_label.Name = "SpouseLastName_label";
            this.SpouseLastName_label.Size = new System.Drawing.Size(58, 13);
            this.SpouseLastName_label.TabIndex = 86;
            this.SpouseLastName_label.Text = "Last Name";
            // 
            // SpouseFirstName_label
            // 
            this.SpouseFirstName_label.AutoSize = true;
            this.SpouseFirstName_label.Location = new System.Drawing.Point(31, 387);
            this.SpouseFirstName_label.Name = "SpouseFirstName_label";
            this.SpouseFirstName_label.Size = new System.Drawing.Size(57, 13);
            this.SpouseFirstName_label.TabIndex = 87;
            this.SpouseFirstName_label.Text = "First Name";
            // 
            // SpouseMiddleName_label
            // 
            this.SpouseMiddleName_label.AutoSize = true;
            this.SpouseMiddleName_label.Location = new System.Drawing.Point(31, 416);
            this.SpouseMiddleName_label.Name = "SpouseMiddleName_label";
            this.SpouseMiddleName_label.Size = new System.Drawing.Size(69, 13);
            this.SpouseMiddleName_label.TabIndex = 88;
            this.SpouseMiddleName_label.Text = "Middle Name";
            // 
            // SpouseSuffix_label
            // 
            this.SpouseSuffix_label.AutoSize = true;
            this.SpouseSuffix_label.Location = new System.Drawing.Point(31, 445);
            this.SpouseSuffix_label.Name = "SpouseSuffix_label";
            this.SpouseSuffix_label.Size = new System.Drawing.Size(33, 13);
            this.SpouseSuffix_label.TabIndex = 89;
            this.SpouseSuffix_label.Text = "Suffix";
            // 
            // SpousePrefix_label
            // 
            this.SpousePrefix_label.AutoSize = true;
            this.SpousePrefix_label.Location = new System.Drawing.Point(31, 471);
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
            this.SearchRequest_groupBox.Location = new System.Drawing.Point(12, 575);
            this.SearchRequest_groupBox.Name = "SearchRequest_groupBox";
            this.SearchRequest_groupBox.Size = new System.Drawing.Size(288, 105);
            this.SearchRequest_groupBox.TabIndex = 91;
            this.SearchRequest_groupBox.TabStop = false;
            this.SearchRequest_groupBox.Text = "Search Options";
            // 
            // SearchAllNamesSpouse_radioButton
            // 
            this.SearchAllNamesSpouse_radioButton.AutoSize = true;
            this.SearchAllNamesSpouse_radioButton.Location = new System.Drawing.Point(6, 70);
            this.SearchAllNamesSpouse_radioButton.Name = "SearchAllNamesSpouse_radioButton";
            this.SearchAllNamesSpouse_radioButton.Size = new System.Drawing.Size(233, 17);
            this.SearchAllNamesSpouse_radioButton.TabIndex = 7;
            this.SearchAllNamesSpouse_radioButton.Text = "Search All Names based on Spouse\'s Name";
            this.SearchAllNamesSpouse_radioButton.UseVisualStyleBackColor = true;
            // 
            // SearchAllNamesFather_radioButton
            // 
            this.SearchAllNamesFather_radioButton.AutoSize = true;
            this.SearchAllNamesFather_radioButton.Location = new System.Drawing.Point(6, 43);
            this.SearchAllNamesFather_radioButton.Name = "SearchAllNamesFather_radioButton";
            this.SearchAllNamesFather_radioButton.Size = new System.Drawing.Size(227, 17);
            this.SearchAllNamesFather_radioButton.TabIndex = 6;
            this.SearchAllNamesFather_radioButton.Text = "Search All Names based on Father\'s Name";
            this.SearchAllNamesFather_radioButton.UseVisualStyleBackColor = true;
            // 
            // SearchAllNames_radioButton
            // 
            this.SearchAllNames_radioButton.AutoSize = true;
            this.SearchAllNames_radioButton.Checked = true;
            this.SearchAllNames_radioButton.Location = new System.Drawing.Point(6, 19);
            this.SearchAllNames_radioButton.Name = "SearchAllNames_radioButton";
            this.SearchAllNames_radioButton.Size = new System.Drawing.Size(109, 17);
            this.SearchAllNames_radioButton.TabIndex = 5;
            this.SearchAllNames_radioButton.TabStop = true;
            this.SearchAllNames_radioButton.Text = "Search All Names";
            this.SearchAllNames_radioButton.UseVisualStyleBackColor = true;
            // 
            // Sex_groupBox
            // 
            this.Sex_groupBox.Controls.Add(this.Unknown_radioButton);
            this.Sex_groupBox.Controls.Add(this.Female_radioButton);
            this.Sex_groupBox.Controls.Add(this.Male_radioButton);
            this.Sex_groupBox.Location = new System.Drawing.Point(694, 49);
            this.Sex_groupBox.Name = "Sex_groupBox";
            this.Sex_groupBox.Size = new System.Drawing.Size(219, 56);
            this.Sex_groupBox.TabIndex = 10;
            this.Sex_groupBox.TabStop = false;
            this.Sex_groupBox.Text = "Sex";
            // 
            // Female_radioButton
            // 
            this.Female_radioButton.AutoSize = true;
            this.Female_radioButton.Location = new System.Drawing.Point(72, 19);
            this.Female_radioButton.Name = "Female_radioButton";
            this.Female_radioButton.Size = new System.Drawing.Size(59, 17);
            this.Female_radioButton.TabIndex = 1;
            this.Female_radioButton.TabStop = true;
            this.Female_radioButton.Text = "Female";
            this.Female_radioButton.UseVisualStyleBackColor = true;
            this.Female_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // Male_radioButton
            // 
            this.Male_radioButton.AutoSize = true;
            this.Male_radioButton.Location = new System.Drawing.Point(18, 19);
            this.Male_radioButton.Name = "Male_radioButton";
            this.Male_radioButton.Size = new System.Drawing.Size(48, 17);
            this.Male_radioButton.TabIndex = 0;
            this.Male_radioButton.TabStop = true;
            this.Male_radioButton.Text = "Male";
            this.Male_radioButton.UseVisualStyleBackColor = true;
            this.Male_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // Unknown_radioButton
            // 
            this.Unknown_radioButton.AutoSize = true;
            this.Unknown_radioButton.Checked = true;
            this.Unknown_radioButton.Location = new System.Drawing.Point(831, 68);
            this.Unknown_radioButton.Name = "Unknown_radioButton";
            this.Unknown_radioButton.Size = new System.Drawing.Size(71, 17);
            this.Unknown_radioButton.TabIndex = 2;
            this.Unknown_radioButton.TabStop = true;
            this.Unknown_radioButton.Text = "Unknown";
            this.Unknown_radioButton.UseVisualStyleBackColor = true;
            this.Unknown_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // BornDategroupBox
            // 
            this.BornDategroupBox.Controls.Add(this.BornDateMonth_textBox);
            this.BornDategroupBox.Controls.Add(this.BornDateYear_textBox);
            this.BornDategroupBox.Controls.Add(this.label12);
            this.BornDategroupBox.Controls.Add(this.label14);
            this.BornDategroupBox.Controls.Add(this.BornDateDay_textBox);
            this.BornDategroupBox.Location = new System.Drawing.Point(491, 216);
            this.BornDategroupBox.Name = "BornDategroupBox";
            this.BornDategroupBox.Size = new System.Drawing.Size(169, 56);
            this.BornDategroupBox.TabIndex = 23;
            this.BornDategroupBox.TabStop = false;
            this.BornDategroupBox.Text = "Born Date";
            // 
            // BornDateMonth_textBox
            // 
            this.BornDateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.BornDateMonth_textBox.Name = "BornDateMonth_textBox";
            this.BornDateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.BornDateMonth_textBox.TabIndex = 18;
            this.BornDateMonth_textBox.Leave += new System.EventHandler(this.ValidateMonth_Click);
            // 
            // BornDateYear_textBox
            // 
            this.BornDateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.BornDateYear_textBox.Name = "BornDateYear_textBox";
            this.BornDateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.BornDateYear_textBox.TabIndex = 20;
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
            // BornDateDay_textBox
            // 
            this.BornDateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.BornDateDay_textBox.Name = "BornDateDay_textBox";
            this.BornDateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.BornDateDay_textBox.TabIndex = 19;
            // 
            // PersonNameOnGrave_textBox
            // 
            this.PersonNameOnGrave_textBox.Location = new System.Drawing.Point(118, 252);
            this.PersonNameOnGrave_textBox.Name = "PersonNameOnGrave_textBox";
            this.PersonNameOnGrave_textBox.Size = new System.Drawing.Size(144, 20);
            this.PersonNameOnGrave_textBox.TabIndex = 85;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(31, 255);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(82, 13);
            this.label15.TabIndex = 92;
            this.label15.Text = "Name on Grave";
            // 
            // SpouseNameOnGrave_textBox
            // 
            this.SpouseNameOnGrave_textBox.Location = new System.Drawing.Point(306, 252);
            this.SpouseNameOnGrave_textBox.Name = "SpouseNameOnGrave_textBox";
            this.SpouseNameOnGrave_textBox.Size = new System.Drawing.Size(144, 20);
            this.SpouseNameOnGrave_textBox.TabIndex = 93;
            // 
            // MotherNameOnGrave_textBox
            // 
            this.MotherNameOnGrave_textBox.Location = new System.Drawing.Point(306, 535);
            this.MotherNameOnGrave_textBox.Name = "MotherNameOnGrave_textBox";
            this.MotherNameOnGrave_textBox.Size = new System.Drawing.Size(144, 20);
            this.MotherNameOnGrave_textBox.TabIndex = 96;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(31, 538);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(82, 13);
            this.label16.TabIndex = 95;
            this.label16.Text = "Name on Grave";
            // 
            // FatherNameOnGrave_textBox
            // 
            this.FatherNameOnGrave_textBox.Location = new System.Drawing.Point(118, 535);
            this.FatherNameOnGrave_textBox.Name = "FatherNameOnGrave_textBox";
            this.FatherNameOnGrave_textBox.Size = new System.Drawing.Size(144, 20);
            this.FatherNameOnGrave_textBox.TabIndex = 94;
            // 
            // Epitaph_TextBox
            // 
            this.Epitaph_TextBox.Location = new System.Drawing.Point(694, 136);
            this.Epitaph_TextBox.Name = "Epitaph_TextBox";
            this.Epitaph_TextBox.Size = new System.Drawing.Size(219, 136);
            this.Epitaph_TextBox.TabIndex = 97;
            this.Epitaph_TextBox.Text = "";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(691, 120);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(43, 13);
            this.label17.TabIndex = 98;
            this.label17.Text = "Epitaph";
            // 
            // ReturnToListbutton
            // 
            this.ReturnToListbutton.Location = new System.Drawing.Point(745, 604);
            this.ReturnToListbutton.Name = "ReturnToListbutton";
            this.ReturnToListbutton.Size = new System.Drawing.Size(117, 23);
            this.ReturnToListbutton.TabIndex = 99;
            this.ReturnToListbutton.TabStop = false;
            this.ReturnToListbutton.Text = "Return To List";
            this.ReturnToListbutton.UseVisualStyleBackColor = true;
            this.ReturnToListbutton.Click += new System.EventHandler(this.ReturnToList_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CalcBornDateMonth_textBox);
            this.groupBox1.Controls.Add(this.CalcBornDateYear_textBox);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.CalcBornDateDay_textBox);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(491, 296);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 56);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calculated Born Date";
            // 
            // CalcBornDateMonth_textBox
            // 
            this.CalcBornDateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.CalcBornDateMonth_textBox.Name = "CalcBornDateMonth_textBox";
            this.CalcBornDateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.CalcBornDateMonth_textBox.TabIndex = 18;
            // 
            // CalcBornDateYear_textBox
            // 
            this.CalcBornDateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.CalcBornDateYear_textBox.Name = "CalcBornDateYear_textBox";
            this.CalcBornDateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.CalcBornDateYear_textBox.TabIndex = 20;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(48, 28);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(12, 13);
            this.label18.TabIndex = 20;
            this.label18.Text = "/";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(91, 28);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(12, 13);
            this.label19.TabIndex = 22;
            this.label19.Text = "/";
            // 
            // CalcBornDateDay_textBox
            // 
            this.CalcBornDateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.CalcBornDateDay_textBox.Name = "CalcBornDateDay_textBox";
            this.CalcBornDateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.CalcBornDateDay_textBox.TabIndex = 19;
            // 
            // FCemeteryRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(940, 683);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ReturnToListbutton);
            this.Controls.Add(this.Unknown_radioButton);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.Epitaph_TextBox);
            this.Controls.Add(this.MotherNameOnGrave_textBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.FatherNameOnGrave_textBox);
            this.Controls.Add(this.SpouseNameOnGrave_textBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.PersonNameOnGrave_textBox);
            this.Controls.Add(this.BornDategroupBox);
            this.Controls.Add(this.Sex_groupBox);
            this.Controls.Add(this.SearchRequest_groupBox);
            this.Controls.Add(this.SpousePrefix_label);
            this.Controls.Add(this.SpouseSuffix_label);
            this.Controls.Add(this.SpouseMiddleName_label);
            this.Controls.Add(this.SpouseFirstName_label);
            this.Controls.Add(this.SpouseLastName_label);
            this.Controls.Add(this.Spouse_groupBox);
            this.Controls.Add(this.Similar_button);
            this.Controls.Add(this.StartingWith_button);
            this.Controls.Add(this.SearchPartial_Button);
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
            this.Controls.Add(this.DiedDate_groupBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FCemeteryRecord";
            this.Text = "Cemetery Record";
            this.DiedDate_groupBox.ResumeLayout(false);
            this.DiedDate_groupBox.PerformLayout();
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
            this.SearchRequest_groupBox.ResumeLayout(false);
            this.SearchRequest_groupBox.PerformLayout();
            this.Sex_groupBox.ResumeLayout(false);
            this.Sex_groupBox.PerformLayout();
            this.BornDategroupBox.ResumeLayout(false);
            this.BornDategroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private RadioButton SearchAllNamesFather_radioButton;
        private GroupBox BornDategroupBox;
        private TextBox BornDateMonth_textBox;
        private TextBox BornDateYear_textBox;
        private Label label12;
        private Label label14;
        private TextBox BornDateDay_textBox;
        private TextBox PersonNameOnGrave_textBox;
        private Label label15;
        private TextBox SpouseNameOnGrave_textBox;
        private TextBox MotherNameOnGrave_textBox;
        private Label label16;
        private TextBox FatherNameOnGrave_textBox;
        private RichTextBox Epitaph_TextBox;
        private Label label17;
        private RadioButton Unknown_radioButton;
        private Button ReturnToListbutton;
        private GroupBox groupBox1;
        private TextBox CalcBornDateMonth_textBox;
        private TextBox CalcBornDateYear_textBox;
        private Label label18;
        private Label label19;
        private TextBox CalcBornDateDay_textBox;
        private RadioButton SearchAllNamesSpouse_radioButton;


    }
}
