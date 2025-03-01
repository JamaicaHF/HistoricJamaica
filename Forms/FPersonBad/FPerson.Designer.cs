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
    partial class FPerson
    {
        protected Label label21;
        protected System.ComponentModel.IContainer components = null;
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
            this.ExcludeFromSite_checkBox = new System.Windows.Forms.CheckBox();
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
            this.label21 = new System.Windows.Forms.Label();
            this.CensusPage_groupBox = new System.Windows.Forms.GroupBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.Census1940_textBox = new System.Windows.Forms.TextBox();
            this.Census1930_textBox = new System.Windows.Forms.TextBox();
            this.Census1920_textBox = new System.Windows.Forms.TextBox();
            this.Census1910_textBox = new System.Windows.Forms.TextBox();
            this.Census1900_textBox = new System.Windows.Forms.TextBox();
            this.Census1890_textBox = new System.Windows.Forms.TextBox();
            this.Census1880_textBox = new System.Windows.Forms.TextBox();
            this.Census1870_textBox = new System.Windows.Forms.TextBox();
            this.Census1860_textBox = new System.Windows.Forms.TextBox();
            this.Census1850_textBox = new System.Windows.Forms.TextBox();
            this.Census1840_textBox = new System.Windows.Forms.TextBox();
            this.Census1830_textBox = new System.Windows.Forms.TextBox();
            this.Census1820_textBox = new System.Windows.Forms.TextBox();
            this.Census1810_textBox = new System.Windows.Forms.TextBox();
            this.Census1800_textBox = new System.Windows.Forms.TextBox();
            this.Census1790_textBox = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.GazetteerRoad_textBox = new System.Windows.Forms.TextBox();
            this.McClellan1856District_textBox = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.Beers1869District_textBox = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.BornFrom_textBox = new System.Windows.Forms.TextBox();
            this.BornFrom_label = new System.Windows.Forms.Label();
            this.BornDate_label = new System.Windows.Forms.Label();
            this.Census1950_textBox = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.Sex_groupBox.SuspendLayout();
            this.Born_groupBox.SuspendLayout();
            this.Died_groupBox.SuspendLayout();
            this.NameOptions_groupBox.SuspendLayout();
            this.Buried_groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.CensusPage_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LastName_textBox
            // 
            this.LastName_textBox.Location = new System.Drawing.Point(14, 56);
            this.LastName_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.LastName_textBox.Name = "LastName_textBox";
            this.LastName_textBox.Size = new System.Drawing.Size(321, 35);
            this.LastName_textBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 118);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 29);
            this.label1.TabIndex = 11;
            this.label1.Text = "Last/Maiden Name";
            // 
            // FirstName_textBox
            // 
            this.FirstName_textBox.Location = new System.Drawing.Point(14, 120);
            this.FirstName_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.FirstName_textBox.Name = "FirstName_textBox";
            this.FirstName_textBox.Size = new System.Drawing.Size(321, 35);
            this.FirstName_textBox.TabIndex = 2;
            this.FirstName_textBox.Leave += new System.EventHandler(this.FirstName_textBox_Leave);
            // 
            // MiddleName_textBox
            // 
            this.MiddleName_textBox.Location = new System.Drawing.Point(14, 185);
            this.MiddleName_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MiddleName_textBox.Name = "MiddleName_textBox";
            this.MiddleName_textBox.Size = new System.Drawing.Size(321, 35);
            this.MiddleName_textBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 252);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 29);
            this.label2.TabIndex = 10;
            this.label2.Text = "Middle Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 187);
            this.label3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "First Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 381);
            this.label4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 29);
            this.label4.TabIndex = 8;
            this.label4.Text = "Suffix/Prefix";
            // 
            // SearchAll_button
            // 
            this.SearchAll_button.Location = new System.Drawing.Point(411, 982);
            this.SearchAll_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.SearchAll_button.Name = "SearchAll_button";
            this.SearchAll_button.Size = new System.Drawing.Size(273, 51);
            this.SearchAll_button.TabIndex = 37;
            this.SearchAll_button.TabStop = false;
            this.SearchAll_button.Text = "Search All Persons";
            this.SearchAll_button.UseVisualStyleBackColor = true;
            this.SearchAll_button.Click += new System.EventHandler(this.SearchAll_button_Click);
            // 
            // Description_TextBox
            // 
            this.Description_TextBox.Location = new System.Drawing.Point(1057, 877);
            this.Description_TextBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Description_TextBox.Name = "Description_TextBox";
            this.Description_TextBox.Size = new System.Drawing.Size(734, 412);
            this.Description_TextBox.TabIndex = 35;
            this.Description_TextBox.TabStop = false;
            this.Description_TextBox.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1066, 837);
            this.label5.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 29);
            this.label5.TabIndex = 1;
            this.label5.Text = "Notes";
            // 
            // ViewPhotographs_button
            // 
            this.ViewPhotographs_button.Location = new System.Drawing.Point(735, 982);
            this.ViewPhotographs_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.ViewPhotographs_button.Name = "ViewPhotographs_button";
            this.ViewPhotographs_button.Size = new System.Drawing.Size(273, 51);
            this.ViewPhotographs_button.TabIndex = 38;
            this.ViewPhotographs_button.TabStop = false;
            this.ViewPhotographs_button.Text = "View Photographs";
            this.ViewPhotographs_button.UseVisualStyleBackColor = true;
            this.ViewPhotographs_button.Click += new System.EventHandler(this.ViewPhotographs_Click);
            // 
            // BornDate_textBox
            // 
            this.BornDate_textBox.Location = new System.Drawing.Point(145, 56);
            this.BornDate_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornDate_textBox.Name = "BornDate_textBox";
            this.BornDate_textBox.Size = new System.Drawing.Size(193, 35);
            this.BornDate_textBox.TabIndex = 13;
            this.BornDate_textBox.Leave += new System.EventHandler(this.DateTextbox_Leave);
            // 
            // BornPlace_textBox
            // 
            this.BornPlace_textBox.Location = new System.Drawing.Point(145, 120);
            this.BornPlace_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornPlace_textBox.Name = "BornPlace_textBox";
            this.BornPlace_textBox.Size = new System.Drawing.Size(195, 35);
            this.BornPlace_textBox.TabIndex = 14;
            // 
            // DiedDate_textBox
            // 
            this.DiedDate_textBox.Location = new System.Drawing.Point(145, 56);
            this.DiedDate_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.DiedDate_textBox.Name = "DiedDate_textBox";
            this.DiedDate_textBox.Size = new System.Drawing.Size(193, 35);
            this.DiedDate_textBox.TabIndex = 20;
            this.DiedDate_textBox.Leave += new System.EventHandler(this.DateTextbox_Leave);
            // 
            // DiedPlace_textBox
            // 
            this.DiedPlace_textBox.Location = new System.Drawing.Point(145, 120);
            this.DiedPlace_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.DiedPlace_textBox.Name = "DiedPlace_textBox";
            this.DiedPlace_textBox.Size = new System.Drawing.Size(193, 35);
            this.DiedPlace_textBox.TabIndex = 21;
            // 
            // Male_radioButton
            // 
            this.Male_radioButton.AutoSize = true;
            this.Male_radioButton.Location = new System.Drawing.Point(51, 33);
            this.Male_radioButton.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Male_radioButton.Name = "Male_radioButton";
            this.Male_radioButton.Size = new System.Drawing.Size(97, 33);
            this.Male_radioButton.TabIndex = 7;
            this.Male_radioButton.TabStop = true;
            this.Male_radioButton.Text = "Male";
            this.Male_radioButton.UseVisualStyleBackColor = true;
            // 
            // BornVerified_checkBox
            // 
            this.BornVerified_checkBox.AutoSize = true;
            this.BornVerified_checkBox.Location = new System.Drawing.Point(203, 257);
            this.BornVerified_checkBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornVerified_checkBox.Name = "BornVerified_checkBox";
            this.BornVerified_checkBox.Size = new System.Drawing.Size(128, 33);
            this.BornVerified_checkBox.TabIndex = 16;
            this.BornVerified_checkBox.Text = "Verified";
            this.BornVerified_checkBox.UseVisualStyleBackColor = true;
            // 
            // DiedVerified_checkBox
            // 
            this.DiedVerified_checkBox.AutoSize = true;
            this.DiedVerified_checkBox.Location = new System.Drawing.Point(201, 254);
            this.DiedVerified_checkBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.DiedVerified_checkBox.Name = "DiedVerified_checkBox";
            this.DiedVerified_checkBox.Size = new System.Drawing.Size(128, 33);
            this.DiedVerified_checkBox.TabIndex = 23;
            this.DiedVerified_checkBox.Text = "Verified";
            this.DiedVerified_checkBox.UseVisualStyleBackColor = true;
            // 
            // Female_radioButton
            // 
            this.Female_radioButton.AutoSize = true;
            this.Female_radioButton.Location = new System.Drawing.Point(201, 33);
            this.Female_radioButton.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Female_radioButton.Name = "Female_radioButton";
            this.Female_radioButton.Size = new System.Drawing.Size(126, 33);
            this.Female_radioButton.TabIndex = 8;
            this.Female_radioButton.TabStop = true;
            this.Female_radioButton.Text = "Female";
            this.Female_radioButton.UseVisualStyleBackColor = true;
            // 
            // Suffix_comboBox
            // 
            this.Suffix_comboBox.FormattingEnabled = true;
            this.Suffix_comboBox.Location = new System.Drawing.Point(14, 315);
            this.Suffix_comboBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Suffix_comboBox.Name = "Suffix_comboBox";
            this.Suffix_comboBox.Size = new System.Drawing.Size(135, 37);
            this.Suffix_comboBox.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 67);
            this.label8.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 29);
            this.label8.TabIndex = 28;
            this.label8.Text = "Date";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 127);
            this.label9.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 29);
            this.label9.TabIndex = 29;
            this.label9.Text = "Place";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 67);
            this.label10.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 29);
            this.label10.TabIndex = 30;
            this.label10.Text = "Date";
            // 
            // DiedPlace_label
            // 
            this.DiedPlace_label.AutoSize = true;
            this.DiedPlace_label.Location = new System.Drawing.Point(5, 127);
            this.DiedPlace_label.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.DiedPlace_label.Name = "DiedPlace_label";
            this.DiedPlace_label.Size = new System.Drawing.Size(74, 29);
            this.DiedPlace_label.TabIndex = 31;
            this.DiedPlace_label.Text = "Place";
            // 
            // Sex_groupBox
            // 
            this.Sex_groupBox.Controls.Add(this.Male_radioButton);
            this.Sex_groupBox.Controls.Add(this.Female_radioButton);
            this.Sex_groupBox.Location = new System.Drawing.Point(289, 529);
            this.Sex_groupBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Sex_groupBox.Name = "Sex_groupBox";
            this.Sex_groupBox.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Sex_groupBox.Size = new System.Drawing.Size(359, 89);
            this.Sex_groupBox.TabIndex = 5;
            this.Sex_groupBox.TabStop = false;
            this.Sex_groupBox.Text = "Sex";
            // 
            // BornSource_textBox
            // 
            this.BornSource_textBox.Location = new System.Drawing.Point(14, 315);
            this.BornSource_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornSource_textBox.Name = "BornSource_textBox";
            this.BornSource_textBox.Size = new System.Drawing.Size(321, 35);
            this.BornSource_textBox.TabIndex = 17;
            this.BornSource_textBox.Leave += new System.EventHandler(this.Born_textBox_Leave);
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
            this.Born_groupBox.Location = new System.Drawing.Point(672, 60);
            this.Born_groupBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Born_groupBox.Name = "Born_groupBox";
            this.Born_groupBox.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Born_groupBox.Size = new System.Drawing.Size(359, 455);
            this.Born_groupBox.TabIndex = 2;
            this.Born_groupBox.TabStop = false;
            this.Born_groupBox.Text = "Born";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(5, 185);
            this.label25.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(129, 29);
            this.label25.TabIndex = 44;
            this.label25.Text = "Residence";
            // 
            // BornHome_textBox
            // 
            this.BornHome_textBox.Location = new System.Drawing.Point(145, 185);
            this.BornHome_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornHome_textBox.Name = "BornHome_textBox";
            this.BornHome_textBox.Size = new System.Drawing.Size(193, 35);
            this.BornHome_textBox.TabIndex = 15;
            // 
            // BornPage_textBox
            // 
            this.BornPage_textBox.Location = new System.Drawing.Point(271, 379);
            this.BornPage_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornPage_textBox.Name = "BornPage_textBox";
            this.BornPage_textBox.Size = new System.Drawing.Size(69, 35);
            this.BornPage_textBox.TabIndex = 19;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(177, 386);
            this.label15.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 29);
            this.label15.TabIndex = 42;
            this.label15.Text = "Page";
            // 
            // BornBook_textBox
            // 
            this.BornBook_textBox.Location = new System.Drawing.Point(93, 379);
            this.BornBook_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornBook_textBox.Name = "BornBook_textBox";
            this.BornBook_textBox.Size = new System.Drawing.Size(55, 35);
            this.BornBook_textBox.TabIndex = 18;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 386);
            this.label14.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 29);
            this.label14.TabIndex = 41;
            this.label14.Text = "Book";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 257);
            this.label12.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 29);
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
            this.Died_groupBox.Location = new System.Drawing.Point(1057, 60);
            this.Died_groupBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Died_groupBox.Name = "Died_groupBox";
            this.Died_groupBox.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Died_groupBox.Size = new System.Drawing.Size(359, 455);
            this.Died_groupBox.TabIndex = 3;
            this.Died_groupBox.TabStop = false;
            this.Died_groupBox.Text = "Died";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(5, 185);
            this.label26.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(129, 29);
            this.label26.TabIndex = 45;
            this.label26.Text = "Residence";
            // 
            // DiedHome_textBox
            // 
            this.DiedHome_textBox.Location = new System.Drawing.Point(145, 185);
            this.DiedHome_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.DiedHome_textBox.Name = "DiedHome_textBox";
            this.DiedHome_textBox.Size = new System.Drawing.Size(193, 35);
            this.DiedHome_textBox.TabIndex = 22;
            // 
            // DiedPage_textBox
            // 
            this.DiedPage_textBox.Location = new System.Drawing.Point(268, 379);
            this.DiedPage_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.DiedPage_textBox.Name = "DiedPage_textBox";
            this.DiedPage_textBox.Size = new System.Drawing.Size(69, 35);
            this.DiedPage_textBox.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 386);
            this.label11.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 29);
            this.label11.TabIndex = 39;
            this.label11.Text = "Book";
            // 
            // DiedBook_textBox
            // 
            this.DiedBook_textBox.Location = new System.Drawing.Point(93, 379);
            this.DiedBook_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.DiedBook_textBox.Name = "DiedBook_textBox";
            this.DiedBook_textBox.Size = new System.Drawing.Size(55, 35);
            this.DiedBook_textBox.TabIndex = 25;
            // 
            // DiedPage_label
            // 
            this.DiedPage_label.AutoSize = true;
            this.DiedPage_label.Location = new System.Drawing.Point(177, 386);
            this.DiedPage_label.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.DiedPage_label.Name = "DiedPage_label";
            this.DiedPage_label.Size = new System.Drawing.Size(70, 29);
            this.DiedPage_label.TabIndex = 37;
            this.DiedPage_label.Text = "Page";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 257);
            this.label13.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 29);
            this.label13.TabIndex = 36;
            this.label13.Text = "Source";
            // 
            // DiedSource_textBox
            // 
            this.DiedSource_textBox.Location = new System.Drawing.Point(14, 315);
            this.DiedSource_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.DiedSource_textBox.Name = "DiedSource_textBox";
            this.DiedSource_textBox.Size = new System.Drawing.Size(321, 35);
            this.DiedSource_textBox.TabIndex = 24;
            // 
            // Prefix_comboBox
            // 
            this.Prefix_comboBox.FormattingEnabled = true;
            this.Prefix_comboBox.Location = new System.Drawing.Point(201, 315);
            this.Prefix_comboBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Prefix_comboBox.Name = "Prefix_comboBox";
            this.Prefix_comboBox.Size = new System.Drawing.Size(135, 37);
            this.Prefix_comboBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 446);
            this.label6.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 29);
            this.label6.TabIndex = 37;
            this.label6.Text = "Known As";
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(72, 982);
            this.Save_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(273, 51);
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
            this.SimilarNames_checkBox.Location = new System.Drawing.Point(23, 45);
            this.SimilarNames_checkBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.SimilarNames_checkBox.Name = "SimilarNames_checkBox";
            this.SimilarNames_checkBox.Size = new System.Drawing.Size(277, 33);
            this.SimilarNames_checkBox.TabIndex = 39;
            this.SimilarNames_checkBox.TabStop = false;
            this.SimilarNames_checkBox.Text = "Check Similar Names";
            this.SimilarNames_checkBox.UseVisualStyleBackColor = true;
            // 
            // NameOptions_groupBox
            // 
            this.NameOptions_groupBox.Controls.Add(this.ExcludeFromSite_checkBox);
            this.NameOptions_groupBox.Controls.Add(this.SimilarNames_checkBox);
            this.NameOptions_groupBox.Location = new System.Drawing.Point(49, 1160);
            this.NameOptions_groupBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.NameOptions_groupBox.Name = "NameOptions_groupBox";
            this.NameOptions_groupBox.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.NameOptions_groupBox.Size = new System.Drawing.Size(327, 147);
            this.NameOptions_groupBox.TabIndex = 41;
            this.NameOptions_groupBox.TabStop = false;
            this.NameOptions_groupBox.Text = "Save Option";
            // 
            // ExcludeFromSite_checkBox
            // 
            this.ExcludeFromSite_checkBox.AutoSize = true;
            this.ExcludeFromSite_checkBox.Location = new System.Drawing.Point(23, 96);
            this.ExcludeFromSite_checkBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.ExcludeFromSite_checkBox.Name = "ExcludeFromSite_checkBox";
            this.ExcludeFromSite_checkBox.Size = new System.Drawing.Size(233, 33);
            this.ExcludeFromSite_checkBox.TabIndex = 40;
            this.ExcludeFromSite_checkBox.TabStop = false;
            this.ExcludeFromSite_checkBox.Text = "Exclude from Site";
            this.ExcludeFromSite_checkBox.UseVisualStyleBackColor = true;
            this.ExcludeFromSite_checkBox.CheckedChanged += new System.EventHandler(this.ExcludeFromSiteChanged_click);
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
            this.Buried_groupBox.Location = new System.Drawing.Point(1437, 60);
            this.Buried_groupBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Buried_groupBox.Name = "Buried_groupBox";
            this.Buried_groupBox.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Buried_groupBox.Size = new System.Drawing.Size(359, 455);
            this.Buried_groupBox.TabIndex = 4;
            this.Buried_groupBox.TabStop = false;
            this.Buried_groupBox.Text = "Buried";
            // 
            // BuriedVerified_checkBox
            // 
            this.BuriedVerified_checkBox.AutoSize = true;
            this.BuriedVerified_checkBox.Location = new System.Drawing.Point(196, 257);
            this.BuriedVerified_checkBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BuriedVerified_checkBox.Name = "BuriedVerified_checkBox";
            this.BuriedVerified_checkBox.Size = new System.Drawing.Size(128, 33);
            this.BuriedVerified_checkBox.TabIndex = 31;
            this.BuriedVerified_checkBox.Text = "Verified";
            this.BuriedVerified_checkBox.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(5, 257);
            this.label29.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(90, 29);
            this.label29.TabIndex = 51;
            this.label29.Text = "Source";
            // 
            // BuriedSource_textBox
            // 
            this.BuriedSource_textBox.Location = new System.Drawing.Point(14, 315);
            this.BuriedSource_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BuriedSource_textBox.Name = "BuriedSource_textBox";
            this.BuriedSource_textBox.Size = new System.Drawing.Size(321, 35);
            this.BuriedSource_textBox.TabIndex = 32;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(177, 386);
            this.label28.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(70, 29);
            this.label28.TabIndex = 46;
            this.label28.Text = "Page";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(5, 386);
            this.label27.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(69, 29);
            this.label27.TabIndex = 46;
            this.label27.Text = "Book";
            // 
            // BuriedPage_textBox
            // 
            this.BuriedPage_textBox.Location = new System.Drawing.Point(271, 379);
            this.BuriedPage_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BuriedPage_textBox.Name = "BuriedPage_textBox";
            this.BuriedPage_textBox.Size = new System.Drawing.Size(69, 35);
            this.BuriedPage_textBox.TabIndex = 34;
            // 
            // BuriedBook_textBox
            // 
            this.BuriedBook_textBox.Location = new System.Drawing.Point(93, 379);
            this.BuriedBook_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BuriedBook_textBox.Name = "BuriedBook_textBox";
            this.BuriedBook_textBox.Size = new System.Drawing.Size(55, 35);
            this.BuriedBook_textBox.TabIndex = 33;
            // 
            // BuriedDate_textBox
            // 
            this.BuriedDate_textBox.Location = new System.Drawing.Point(145, 56);
            this.BuriedDate_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BuriedDate_textBox.Name = "BuriedDate_textBox";
            this.BuriedDate_textBox.Size = new System.Drawing.Size(193, 35);
            this.BuriedDate_textBox.TabIndex = 27;
            this.BuriedDate_textBox.Leave += new System.EventHandler(this.DateTextbox_Leave);
            // 
            // BuriedStone_textBox
            // 
            this.BuriedStone_textBox.Location = new System.Drawing.Point(98, 185);
            this.BuriedStone_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BuriedStone_textBox.Name = "BuriedStone_textBox";
            this.BuriedStone_textBox.Size = new System.Drawing.Size(240, 35);
            this.BuriedStone_textBox.TabIndex = 30;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(5, 194);
            this.label22.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(90, 29);
            this.label22.TabIndex = 46;
            this.label22.Text = "Lot No.";
            // 
            // BuriedPlace_textBox
            // 
            this.BuriedPlace_textBox.BackColor = System.Drawing.Color.White;
            this.BuriedPlace_textBox.Location = new System.Drawing.Point(145, 120);
            this.BuriedPlace_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BuriedPlace_textBox.Name = "BuriedPlace_textBox";
            this.BuriedPlace_textBox.Size = new System.Drawing.Size(193, 35);
            this.BuriedPlace_textBox.TabIndex = 28;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(5, 71);
            this.label17.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(63, 29);
            this.label17.TabIndex = 44;
            this.label17.Text = "Date";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 127);
            this.label16.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(117, 29);
            this.label16.TabIndex = 41;
            this.label16.Text = "Cemetery";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(47, 814);
            this.label18.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(82, 29);
            this.label18.TabIndex = 43;
            this.label18.Text = "Father";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(47, 881);
            this.label19.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(88, 29);
            this.label19.TabIndex = 44;
            this.label19.Text = "Mother";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(47, 747);
            this.label23.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(96, 29);
            this.label23.TabIndex = 49;
            this.label23.Text = "Spouse";
            // 
            // MarriedName_textBox
            // 
            this.MarriedName_textBox.Location = new System.Drawing.Point(14, 250);
            this.MarriedName_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MarriedName_textBox.Name = "MarriedName_textBox";
            this.MarriedName_textBox.Size = new System.Drawing.Size(321, 35);
            this.MarriedName_textBox.TabIndex = 4;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(47, 319);
            this.label24.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(167, 29);
            this.label24.TabIndex = 51;
            this.label24.Text = "Married Name";
            // 
            // Family_button
            // 
            this.Family_button.Location = new System.Drawing.Point(735, 1071);
            this.Family_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Family_button.Name = "Family_button";
            this.Family_button.Size = new System.Drawing.Size(273, 51);
            this.Family_button.TabIndex = 45;
            this.Family_button.TabStop = false;
            this.Family_button.Text = "Family";
            this.Family_button.UseVisualStyleBackColor = true;
            this.Family_button.Click += new System.EventHandler(this.Family_button_Click);
            // 
            // UniquePhotos_button
            // 
            this.UniquePhotos_button.Location = new System.Drawing.Point(735, 1249);
            this.UniquePhotos_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.UniquePhotos_button.Name = "UniquePhotos_button";
            this.UniquePhotos_button.Size = new System.Drawing.Size(273, 51);
            this.UniquePhotos_button.TabIndex = 46;
            this.UniquePhotos_button.TabStop = false;
            this.UniquePhotos_button.Text = "Add Unique Photo";
            this.UniquePhotos_button.UseVisualStyleBackColor = true;
            this.UniquePhotos_button.Click += new System.EventHandler(this.UniquePhotos_button_Click);
            // 
            // AdditionalSpouse_button
            // 
            this.AdditionalSpouse_button.Location = new System.Drawing.Point(210, 747);
            this.AdditionalSpouse_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.AdditionalSpouse_button.Name = "AdditionalSpouse_button";
            this.AdditionalSpouse_button.Size = new System.Drawing.Size(54, 45);
            this.AdditionalSpouse_button.TabIndex = 54;
            this.AdditionalSpouse_button.TabStop = false;
            this.AdditionalSpouse_button.Text = "...";
            this.AdditionalSpouse_button.UseVisualStyleBackColor = true;
            this.AdditionalSpouse_button.Click += new System.EventHandler(this.AdditionalSpouse_Click);
            // 
            // SearchPartial_Button
            // 
            this.SearchPartial_Button.Location = new System.Drawing.Point(411, 1071);
            this.SearchPartial_Button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.SearchPartial_Button.Name = "SearchPartial_Button";
            this.SearchPartial_Button.Size = new System.Drawing.Size(273, 51);
            this.SearchPartial_Button.TabIndex = 55;
            this.SearchPartial_Button.TabStop = false;
            this.SearchPartial_Button.Text = "Partial Name";
            this.SearchPartial_Button.UseVisualStyleBackColor = true;
            this.SearchPartial_Button.Click += new System.EventHandler(this.SearchPartial_button_Click);
            // 
            // StartingWith_button
            // 
            this.StartingWith_button.Location = new System.Drawing.Point(411, 1160);
            this.StartingWith_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.StartingWith_button.Name = "StartingWith_button";
            this.StartingWith_button.Size = new System.Drawing.Size(273, 51);
            this.StartingWith_button.TabIndex = 56;
            this.StartingWith_button.TabStop = false;
            this.StartingWith_button.Text = "Last Starting With";
            this.StartingWith_button.UseVisualStyleBackColor = true;
            this.StartingWith_button.Click += new System.EventHandler(this.SearchStartingWith_button_Click);
            // 
            // Similar_button
            // 
            this.Similar_button.Location = new System.Drawing.Point(411, 1249);
            this.Similar_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Similar_button.Name = "Similar_button";
            this.Similar_button.Size = new System.Drawing.Size(273, 51);
            this.Similar_button.TabIndex = 57;
            this.Similar_button.TabStop = false;
            this.Similar_button.Text = "Similar Names";
            this.Similar_button.UseVisualStyleBackColor = true;
            this.Similar_button.Click += new System.EventHandler(this.SearchSimilar_button_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(1066, 531);
            this.label30.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(237, 29);
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
            this.groupBox1.Location = new System.Drawing.Point(289, 60);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.groupBox1.Size = new System.Drawing.Size(359, 455);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Person";
            // 
            // KnownAs_textBox
            // 
            this.KnownAs_textBox.Location = new System.Drawing.Point(14, 379);
            this.KnownAs_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.KnownAs_textBox.Name = "KnownAs_textBox";
            this.KnownAs_textBox.Size = new System.Drawing.Size(321, 35);
            this.KnownAs_textBox.TabIndex = 7;
            // 
            // VitalRecord_button
            // 
            this.VitalRecord_button.Location = new System.Drawing.Point(735, 1160);
            this.VitalRecord_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.VitalRecord_button.Name = "VitalRecord_button";
            this.VitalRecord_button.Size = new System.Drawing.Size(273, 51);
            this.VitalRecord_button.TabIndex = 61;
            this.VitalRecord_button.TabStop = false;
            this.VitalRecord_button.Text = "Vital Records";
            this.VitalRecord_button.UseVisualStyleBackColor = true;
            this.VitalRecord_button.Click += new System.EventHandler(this.VitalRecord_button_Click);
            // 
            // Source_textBox
            // 
            this.Source_textBox.Location = new System.Drawing.Point(303, 676);
            this.Source_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Source_textBox.Name = "Source_textBox";
            this.Source_textBox.Size = new System.Drawing.Size(321, 35);
            this.Source_textBox.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(303, 636);
            this.label7.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 29);
            this.label7.TabIndex = 45;
            this.label7.Text = "Source";
            // 
            // NewPerson_button
            // 
            this.NewPerson_button.Location = new System.Drawing.Point(72, 1071);
            this.NewPerson_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.NewPerson_button.Name = "NewPerson_button";
            this.NewPerson_button.Size = new System.Drawing.Size(273, 51);
            this.NewPerson_button.TabIndex = 62;
            this.NewPerson_button.TabStop = false;
            this.NewPerson_button.Text = "New Person";
            this.NewPerson_button.UseVisualStyleBackColor = true;
            this.NewPerson_button.Click += new System.EventHandler(this.NewPerson_click);
            // 
            // AdditionalMarried_button
            // 
            this.AdditionalMarried_button.Location = new System.Drawing.Point(222, 312);
            this.AdditionalMarried_button.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.AdditionalMarried_button.Name = "AdditionalMarried_button";
            this.AdditionalMarried_button.Size = new System.Drawing.Size(54, 45);
            this.AdditionalMarried_button.TabIndex = 63;
            this.AdditionalMarried_button.TabStop = false;
            this.AdditionalMarried_button.Text = "...";
            this.AdditionalMarried_button.UseVisualStyleBackColor = true;
            // 
            // PersonID_textBox
            // 
            this.PersonID_textBox.Location = new System.Drawing.Point(119, 60);
            this.PersonID_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.PersonID_textBox.Name = "PersonID_textBox";
            this.PersonID_textBox.Size = new System.Drawing.Size(93, 35);
            this.PersonID_textBox.TabIndex = 46;
            this.PersonID_textBox.Visible = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(1652, 522);
            this.label21.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(94, 29);
            this.label21.TabIndex = 65;
            this.label21.Text = "Census";
            // 
            // CensusPage_groupBox
            // 
            this.CensusPage_groupBox.Controls.Add(this.label20);
            this.CensusPage_groupBox.Controls.Add(this.Census1950_textBox);
            this.CensusPage_groupBox.Controls.Add(this.label46);
            this.CensusPage_groupBox.Controls.Add(this.label45);
            this.CensusPage_groupBox.Controls.Add(this.label44);
            this.CensusPage_groupBox.Controls.Add(this.label43);
            this.CensusPage_groupBox.Controls.Add(this.label42);
            this.CensusPage_groupBox.Controls.Add(this.label41);
            this.CensusPage_groupBox.Controls.Add(this.label40);
            this.CensusPage_groupBox.Controls.Add(this.label39);
            this.CensusPage_groupBox.Controls.Add(this.label38);
            this.CensusPage_groupBox.Controls.Add(this.label37);
            this.CensusPage_groupBox.Controls.Add(this.label36);
            this.CensusPage_groupBox.Controls.Add(this.label35);
            this.CensusPage_groupBox.Controls.Add(this.label34);
            this.CensusPage_groupBox.Controls.Add(this.label33);
            this.CensusPage_groupBox.Controls.Add(this.label32);
            this.CensusPage_groupBox.Controls.Add(this.label31);
            this.CensusPage_groupBox.Controls.Add(this.Census1940_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1930_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1920_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1910_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1900_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1890_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1880_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1870_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1860_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1850_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1840_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1830_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1820_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1810_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1800_textBox);
            this.CensusPage_groupBox.Controls.Add(this.Census1790_textBox);
            this.CensusPage_groupBox.Location = new System.Drawing.Point(1853, 60);
            this.CensusPage_groupBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.CensusPage_groupBox.Name = "CensusPage_groupBox";
            this.CensusPage_groupBox.Padding = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.CensusPage_groupBox.Size = new System.Drawing.Size(168, 1114);
            this.CensusPage_groupBox.TabIndex = 66;
            this.CensusPage_groupBox.TabStop = false;
            this.CensusPage_groupBox.Text = "Census Page";
            this.CensusPage_groupBox.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(14, 988);
            this.label46.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(65, 29);
            this.label46.TabIndex = 31;
            this.label46.Text = "1940";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(14, 932);
            this.label45.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(65, 29);
            this.label45.TabIndex = 30;
            this.label45.Text = "1930";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(14, 868);
            this.label44.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(65, 29);
            this.label44.TabIndex = 29;
            this.label44.Text = "1920";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(14, 808);
            this.label43.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(65, 29);
            this.label43.TabIndex = 28;
            this.label43.Text = "1910";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(14, 747);
            this.label42.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(65, 29);
            this.label42.TabIndex = 27;
            this.label42.Text = "1900";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(14, 687);
            this.label41.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(65, 29);
            this.label41.TabIndex = 26;
            this.label41.Text = "1890";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(14, 627);
            this.label40.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(65, 29);
            this.label40.TabIndex = 25;
            this.label40.Text = "1880";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(14, 567);
            this.label39.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(65, 29);
            this.label39.TabIndex = 24;
            this.label39.Text = "1870";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(14, 506);
            this.label38.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(65, 29);
            this.label38.TabIndex = 23;
            this.label38.Text = "1860";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(14, 446);
            this.label37.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(65, 29);
            this.label37.TabIndex = 22;
            this.label37.Text = "1850";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(14, 386);
            this.label36.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(65, 29);
            this.label36.TabIndex = 21;
            this.label36.Text = "1840";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(14, 326);
            this.label35.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(65, 29);
            this.label35.TabIndex = 20;
            this.label35.Text = "1830";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(14, 265);
            this.label34.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(65, 29);
            this.label34.TabIndex = 19;
            this.label34.Text = "1820";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(14, 205);
            this.label33.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(65, 29);
            this.label33.TabIndex = 18;
            this.label33.Text = "1810";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(14, 145);
            this.label32.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(65, 29);
            this.label32.TabIndex = 17;
            this.label32.Text = "1800";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(14, 85);
            this.label31.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(65, 29);
            this.label31.TabIndex = 1;
            this.label31.Text = "1790";
            // 
            // Census1940_textBox
            // 
            this.Census1940_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1940_textBox.Location = new System.Drawing.Point(100, 982);
            this.Census1940_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1940_textBox.Name = "Census1940_textBox";
            this.Census1940_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1940_textBox.TabIndex = 16;
            this.Census1940_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1930_textBox
            // 
            this.Census1930_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1930_textBox.Location = new System.Drawing.Point(100, 921);
            this.Census1930_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1930_textBox.Name = "Census1930_textBox";
            this.Census1930_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1930_textBox.TabIndex = 15;
            this.Census1930_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1920_textBox
            // 
            this.Census1920_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1920_textBox.Location = new System.Drawing.Point(100, 861);
            this.Census1920_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1920_textBox.Name = "Census1920_textBox";
            this.Census1920_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1920_textBox.TabIndex = 14;
            this.Census1920_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1910_textBox
            // 
            this.Census1910_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1910_textBox.Location = new System.Drawing.Point(100, 801);
            this.Census1910_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1910_textBox.Name = "Census1910_textBox";
            this.Census1910_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1910_textBox.TabIndex = 13;
            this.Census1910_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1900_textBox
            // 
            this.Census1900_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1900_textBox.Location = new System.Drawing.Point(100, 741);
            this.Census1900_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1900_textBox.Name = "Census1900_textBox";
            this.Census1900_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1900_textBox.TabIndex = 12;
            this.Census1900_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1890_textBox
            // 
            this.Census1890_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1890_textBox.Location = new System.Drawing.Point(100, 680);
            this.Census1890_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1890_textBox.Name = "Census1890_textBox";
            this.Census1890_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1890_textBox.TabIndex = 11;
            this.Census1890_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1880_textBox
            // 
            this.Census1880_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1880_textBox.Location = new System.Drawing.Point(100, 620);
            this.Census1880_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1880_textBox.Name = "Census1880_textBox";
            this.Census1880_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1880_textBox.TabIndex = 10;
            this.Census1880_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1870_textBox
            // 
            this.Census1870_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1870_textBox.Location = new System.Drawing.Point(100, 560);
            this.Census1870_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1870_textBox.Name = "Census1870_textBox";
            this.Census1870_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1870_textBox.TabIndex = 9;
            this.Census1870_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1860_textBox
            // 
            this.Census1860_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1860_textBox.Location = new System.Drawing.Point(100, 500);
            this.Census1860_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1860_textBox.Name = "Census1860_textBox";
            this.Census1860_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1860_textBox.TabIndex = 8;
            this.Census1860_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1850_textBox
            // 
            this.Census1850_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1850_textBox.Location = new System.Drawing.Point(100, 439);
            this.Census1850_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1850_textBox.Name = "Census1850_textBox";
            this.Census1850_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1850_textBox.TabIndex = 7;
            this.Census1850_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1840_textBox
            // 
            this.Census1840_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1840_textBox.Location = new System.Drawing.Point(100, 379);
            this.Census1840_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1840_textBox.Name = "Census1840_textBox";
            this.Census1840_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1840_textBox.TabIndex = 6;
            this.Census1840_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1830_textBox
            // 
            this.Census1830_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1830_textBox.Location = new System.Drawing.Point(100, 319);
            this.Census1830_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1830_textBox.Name = "Census1830_textBox";
            this.Census1830_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1830_textBox.TabIndex = 5;
            this.Census1830_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1820_textBox
            // 
            this.Census1820_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1820_textBox.Location = new System.Drawing.Point(100, 259);
            this.Census1820_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1820_textBox.Name = "Census1820_textBox";
            this.Census1820_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1820_textBox.TabIndex = 4;
            this.Census1820_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1810_textBox
            // 
            this.Census1810_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1810_textBox.Location = new System.Drawing.Point(100, 199);
            this.Census1810_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1810_textBox.Name = "Census1810_textBox";
            this.Census1810_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1810_textBox.TabIndex = 3;
            this.Census1810_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1800_textBox
            // 
            this.Census1800_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1800_textBox.Location = new System.Drawing.Point(100, 138);
            this.Census1800_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1800_textBox.Name = "Census1800_textBox";
            this.Census1800_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1800_textBox.TabIndex = 2;
            this.Census1800_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // Census1790_textBox
            // 
            this.Census1790_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1790_textBox.Location = new System.Drawing.Point(100, 78);
            this.Census1790_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Census1790_textBox.Name = "Census1790_textBox";
            this.Census1790_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1790_textBox.TabIndex = 0;
            this.Census1790_textBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.Census1790_textBox.Leave += new System.EventHandler(this.Census_Checked);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(1839, 1300);
            this.label48.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(78, 29);
            this.label48.TabIndex = 69;
            this.label48.Text = "1884#";
            // 
            // GazetteerRoad_textBox
            // 
            this.GazetteerRoad_textBox.Location = new System.Drawing.Point(1941, 1300);
            this.GazetteerRoad_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.GazetteerRoad_textBox.Name = "GazetteerRoad_textBox";
            this.GazetteerRoad_textBox.Size = new System.Drawing.Size(65, 35);
            this.GazetteerRoad_textBox.TabIndex = 0;
            // 
            // McClellan1856District_textBox
            // 
            this.McClellan1856District_textBox.Location = new System.Drawing.Point(1941, 1188);
            this.McClellan1856District_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.McClellan1856District_textBox.Name = "McClellan1856District_textBox";
            this.McClellan1856District_textBox.Size = new System.Drawing.Size(65, 35);
            this.McClellan1856District_textBox.TabIndex = 70;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(1839, 1244);
            this.label47.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(78, 29);
            this.label47.TabIndex = 71;
            this.label47.Text = "1869#";
            // 
            // Beers1869District_textBox
            // 
            this.Beers1869District_textBox.Location = new System.Drawing.Point(1941, 1244);
            this.Beers1869District_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Beers1869District_textBox.Name = "Beers1869District_textBox";
            this.Beers1869District_textBox.Size = new System.Drawing.Size(65, 35);
            this.Beers1869District_textBox.TabIndex = 72;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(1839, 1188);
            this.label49.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(78, 29);
            this.label49.TabIndex = 73;
            this.label49.Text = "1856#";
            // 
            // BornFrom_textBox
            // 
            this.BornFrom_textBox.Location = new System.Drawing.Point(821, 573);
            this.BornFrom_textBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.BornFrom_textBox.Name = "BornFrom_textBox";
            this.BornFrom_textBox.Size = new System.Drawing.Size(193, 35);
            this.BornFrom_textBox.TabIndex = 45;
            // 
            // BornFrom_label
            // 
            this.BornFrom_label.AutoSize = true;
            this.BornFrom_label.Location = new System.Drawing.Point(679, 538);
            this.BornFrom_label.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.BornFrom_label.Name = "BornFrom_label";
            this.BornFrom_label.Size = new System.Drawing.Size(90, 29);
            this.BornFrom_label.TabIndex = 45;
            this.BornFrom_label.Text = "Source";
            // 
            // BornDate_label
            // 
            this.BornDate_label.AutoSize = true;
            this.BornDate_label.Location = new System.Drawing.Point(679, 580);
            this.BornDate_label.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.BornDate_label.Name = "BornDate_label";
            this.BornDate_label.Size = new System.Drawing.Size(120, 29);
            this.BornDate_label.TabIndex = 74;
            this.BornDate_label.Text = "Born Date";
            // 
            // Census1950_textBox
            // 
            this.Census1950_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Census1950_textBox.Location = new System.Drawing.Point(100, 1043);
            this.Census1950_textBox.Margin = new System.Windows.Forms.Padding(7);
            this.Census1950_textBox.Name = "Census1950_textBox";
            this.Census1950_textBox.Size = new System.Drawing.Size(37, 36);
            this.Census1950_textBox.TabIndex = 32;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(21, 1047);
            this.label20.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 29);
            this.label20.TabIndex = 33;
            this.label20.Text = "1950";
            // 
            // FPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(2088, 1370);
            this.Controls.Add(this.BornDate_label);
            this.Controls.Add(this.BornFrom_label);
            this.Controls.Add(this.BornFrom_textBox);
            this.Controls.Add(this.label49);
            this.Controls.Add(this.Beers1869District_textBox);
            this.Controls.Add(this.label47);
            this.Controls.Add(this.McClellan1856District_textBox);
            this.Controls.Add(this.GazetteerRoad_textBox);
            this.Controls.Add(this.label48);
            this.Controls.Add(this.CensusPage_groupBox);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.PersonID_textBox);
            this.Controls.Add(this.AdditionalMarried_button);
            this.Controls.Add(this.NewPerson_button);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Source_textBox);
            this.Controls.Add(this.VitalRecord_button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.Sex_groupBox);
            this.Controls.Add(this.Similar_button);
            this.Controls.Add(this.StartingWith_button);
            this.Controls.Add(this.SearchPartial_Button);
            this.Controls.Add(this.AdditionalSpouse_button);
            this.Controls.Add(this.UniquePhotos_button);
            this.Controls.Add(this.Family_button);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.Buried_groupBox);
            this.Controls.Add(this.NameOptions_groupBox);
            this.Controls.Add(this.Save_button);
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
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
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
            this.CensusPage_groupBox.ResumeLayout(false);
            this.CensusPage_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        //****************************************************************************************************************************
        #endregion
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private GroupBox CensusPage_groupBox;
        private TextBox Census1790_textBox;
        private Label label31;
        private Label label46;
        private Label label45;
        private Label label44;
        private Label label43;
        private Label label42;
        private Label label41;
        private Label label40;
        private Label label39;
        private Label label38;
        private Label label37;
        private Label label36;
        private Label label35;
        private Label label34;
        private Label label33;
        private Label label32;
        private TextBox Census1940_textBox;
        private TextBox Census1930_textBox;
        private TextBox Census1920_textBox;
        private TextBox Census1910_textBox;
        private TextBox Census1900_textBox;
        private TextBox Census1890_textBox;
        private TextBox Census1880_textBox;
        private TextBox Census1870_textBox;
        private TextBox Census1860_textBox;
        private TextBox Census1850_textBox;
        private TextBox Census1840_textBox;
        private TextBox Census1830_textBox;
        private TextBox Census1820_textBox;
        private TextBox Census1810_textBox;
        private TextBox Census1800_textBox;
        protected CheckBox ExcludeFromSite_checkBox;
        protected Label label47;
        protected Label label48;
        protected Label label49;
        private TextBox GazetteerRoad_textBox;
        private TextBox Beers1869District_textBox;
        private TextBox McClellan1856District_textBox;
        protected TextBox BornFrom_textBox;
        protected Label BornFrom_label;
        protected Label BornDate_label;
        private Label label20;
        private TextBox Census1950_textBox;
        //****************************************************************************************************************************
    }
}
