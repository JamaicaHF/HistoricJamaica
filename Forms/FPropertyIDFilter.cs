using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FPropertyIDFilter : Form
    {
        private int m_SelectedPropertyID = 0;
        private CSql m_SQL;
        private TextBox StreetName_textBox;
        private Button SearchByName_button;
        private Button SearchByStreet_button;
        private Label label1;
        private string m_SelectedGrandListID = "";
        public FPropertyIDFilter(CSql SQL)
        {
            m_SQL = SQL;
            InitializeComponent();
//            this.Load += new EventHandler(PropertyIDFilter_Load);
        }
        //****************************************************************************************************************************
        private void PropertyIDFilter_Load(System.Object sender, System.EventArgs e)
        {
//            GetPropertyFromGrid();
        }
        //****************************************************************************************************************************
        public int SelectedPropertyID
        {
            get { return m_SelectedPropertyID; }
        }
        //****************************************************************************************************************************
        public string SelectedGrandListID
        {
            get { return m_SelectedGrandListID; }
        }
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
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.StreetName_textBox = new System.Windows.Forms.TextBox();
            this.SearchByName_button = new System.Windows.Forms.Button();
            this.SearchByStreet_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StreetName_textBox
            // 
            this.StreetName_textBox.Location = new System.Drawing.Point(60, 157);
            this.StreetName_textBox.Name = "StreetName_textBox";
            this.StreetName_textBox.Size = new System.Drawing.Size(196, 20);
            this.StreetName_textBox.TabIndex = 0;
            // 
            // SearchByName_button
            // 
            this.SearchByName_button.Location = new System.Drawing.Point(36, 53);
            this.SearchByName_button.Name = "SearchByName_button";
            this.SearchByName_button.Size = new System.Drawing.Size(103, 23);
            this.SearchByName_button.TabIndex = 1;
            this.SearchByName_button.Text = "Search By Name";
            this.SearchByName_button.UseVisualStyleBackColor = true;
            this.SearchByName_button.Click += new System.EventHandler(this.GetPropertyIDByStreetName_Click);
            // 
            // SearchByStreet_button
            // 
            this.SearchByStreet_button.Location = new System.Drawing.Point(177, 53);
            this.SearchByStreet_button.Name = "SearchByStreet_button";
            this.SearchByStreet_button.Size = new System.Drawing.Size(103, 23);
            this.SearchByStreet_button.TabIndex = 2;
            this.SearchByStreet_button.Text = "Search By Street";
            this.SearchByStreet_button.UseVisualStyleBackColor = true;
            this.SearchByStreet_button.Click += new System.EventHandler(this.GetPropertyID_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name or Street Name";
            // 
            // FPropertyIDFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(309, 266);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SearchByStreet_button);
            this.Controls.Add(this.SearchByName_button);
            this.Controls.Add(this.StreetName_textBox);
            this.Location = new System.Drawing.Point(500, 200);
            this.Name = "FPropertyIDFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FPropertyIDFilter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        //****************************************************************************************************************************
        private void GetPropertyFromGrid(DataTable tbl)
        {
            CGridPropertyID GridPropertyID = new CGridPropertyID(m_SQL, tbl);
            GridPropertyID.ShowDialog();
            m_SelectedPropertyID = GridPropertyID.SelectedPropertyID;
        }
        //****************************************************************************************************************************
        private void GetPropertyIDByStreetName_Click(object sender, EventArgs e)
        {
            DataTable tbl = Q.t(m_SQL, m_SQL.GetGrandListPropertiesSortByName(StreetName_textBox.Text.ToString(),
                                                                              StreetName_textBox.Text.ToString()));
            GetPropertyFromGrid(tbl);
            if (m_SelectedPropertyID != 0)
                this.Close();
        }
        //****************************************************************************************************************************
        private void GetPropertyID_Click(object sender, EventArgs e)
        {
            DataTable tbl = Q.t(m_SQL, m_SQL.GetGrandListProperties(StreetName_textBox.Text.ToString()));
            GetPropertyFromGrid(tbl);
            if (m_SelectedPropertyID != 0)
                this.Close();
        }
        //****************************************************************************************************************************
    }
}
