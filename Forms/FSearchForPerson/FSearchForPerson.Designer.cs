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
    partial class FSearchForPerson
    {
        private Button StartingWith_button;
        private Button Similar_button;
        private System.Windows.Forms.TextBox LastName_textBox;
        private System.Windows.Forms.TextBox FirstName_textBox;
        private System.Windows.Forms.GroupBox Name_groupBox;
        private System.Windows.Forms.Label label2;
        private Label label11;
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
            this.Name_groupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.StartingWith_button = new System.Windows.Forms.Button();
            this.Similar_button = new System.Windows.Forms.Button();
            this.buttonNewPerson = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonVitalRecords = new System.Windows.Forms.RadioButton();
            this.radioButtonPerson = new System.Windows.Forms.RadioButton();
            this.radioButtonCemeteryRecords = new System.Windows.Forms.RadioButton();
            this.Name_groupBox.SuspendLayout();
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
            // 
            // Name_groupBox
            // 
            this.Name_groupBox.Controls.Add(this.LastName_textBox);
            this.Name_groupBox.Controls.Add(this.FirstName_textBox);
            this.Name_groupBox.Location = new System.Drawing.Point(118, 49);
            this.Name_groupBox.Name = "Name_groupBox";
            this.Name_groupBox.Size = new System.Drawing.Size(144, 104);
            this.Name_groupBox.TabIndex = 0;
            this.Name_groupBox.TabStop = false;
            this.Name_groupBox.Text = "Name";
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
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 71;
            this.label11.Text = "Last Name";
            // 
            // StartingWith_button
            // 
            this.StartingWith_button.Location = new System.Drawing.Point(123, 181);
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
            this.Similar_button.Location = new System.Drawing.Point(123, 210);
            this.Similar_button.Name = "Similar_button";
            this.Similar_button.Size = new System.Drawing.Size(117, 23);
            this.Similar_button.TabIndex = 79;
            this.Similar_button.TabStop = false;
            this.Similar_button.Text = "Similar Names";
            this.Similar_button.UseVisualStyleBackColor = true;
            this.Similar_button.Click += new System.EventHandler(this.SearchSimilar_button_Click);
            // 
            // buttonNewPerson
            // 
            this.buttonNewPerson.Location = new System.Drawing.Point(391, 210);
            this.buttonNewPerson.Name = "buttonNewPerson";
            this.buttonNewPerson.Size = new System.Drawing.Size(117, 23);
            this.buttonNewPerson.TabIndex = 80;
            this.buttonNewPerson.TabStop = false;
            this.buttonNewPerson.Text = "New Person";
            this.buttonNewPerson.UseVisualStyleBackColor = true;
            this.buttonNewPerson.Click += new System.EventHandler(this.NewPersonButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonCemeteryRecords);
            this.groupBox1.Controls.Add(this.radioButtonAll);
            this.groupBox1.Controls.Add(this.radioButtonVitalRecords);
            this.groupBox1.Controls.Add(this.radioButtonPerson);
            this.groupBox1.Location = new System.Drawing.Point(354, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 118);
            this.groupBox1.TabIndex = 81;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Option";
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Checked = true;
            this.radioButtonAll.Location = new System.Drawing.Point(26, 88);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(79, 17);
            this.radioButtonAll.TabIndex = 2;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All Records";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonVitalRecords
            // 
            this.radioButtonVitalRecords.AutoSize = true;
            this.radioButtonVitalRecords.Location = new System.Drawing.Point(26, 42);
            this.radioButtonVitalRecords.Name = "radioButtonVitalRecords";
            this.radioButtonVitalRecords.Size = new System.Drawing.Size(88, 17);
            this.radioButtonVitalRecords.TabIndex = 1;
            this.radioButtonVitalRecords.Text = "Vital Records";
            this.radioButtonVitalRecords.UseVisualStyleBackColor = true;
            // 
            // radioButtonPerson
            // 
            this.radioButtonPerson.AutoSize = true;
            this.radioButtonPerson.Location = new System.Drawing.Point(26, 22);
            this.radioButtonPerson.Name = "radioButtonPerson";
            this.radioButtonPerson.Size = new System.Drawing.Size(101, 17);
            this.radioButtonPerson.TabIndex = 0;
            this.radioButtonPerson.Text = "Person Records";
            this.radioButtonPerson.UseVisualStyleBackColor = true;
            // 
            // radioButtonCemeteryRecords
            // 
            this.radioButtonCemeteryRecords.AutoSize = true;
            this.radioButtonCemeteryRecords.Location = new System.Drawing.Point(26, 65);
            this.radioButtonCemeteryRecords.Name = "radioButtonCemeteryRecords";
            this.radioButtonCemeteryRecords.Size = new System.Drawing.Size(112, 17);
            this.radioButtonCemeteryRecords.TabIndex = 3;
            this.radioButtonCemeteryRecords.Text = "Cemetery Records";
            this.radioButtonCemeteryRecords.UseVisualStyleBackColor = true;
            // 
            // FSearchForPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(614, 337);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonNewPerson);
            this.Controls.Add(this.Similar_button);
            this.Controls.Add(this.StartingWith_button);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Name_groupBox);
            this.Name = "FSearchForPerson";
            this.Text = "Search For Person";
            this.Name_groupBox.ResumeLayout(false);
            this.Name_groupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Button buttonNewPerson;
        private GroupBox groupBox1;
        private RadioButton radioButtonAll;
        private RadioButton radioButtonVitalRecords;
        private RadioButton radioButtonPerson;
        private RadioButton radioButtonCemeteryRecords;



    }
}
