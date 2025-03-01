using System;
using System.Drawing;
using System.Windows;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FBuildingItem : Form
    {
        private int m_iBuildingItem = 9999;
        public int BuildingItem { get { return m_iBuildingItem; } }
        private Label BuildingName_label;
        private Label BuildingOccupant_label;
        private Label Building1856Name_label;
        private Label Building1869Name_label;
        private TextBox input_textBox;
        private System.ComponentModel.IContainer components = null;
        //****************************************************************************************************************************
        public FBuildingItem(CSql SQL,
                             int iLocationX,
                             int iLocationY,
                             int iSelectedRow,
                             int streetNum,
                             string label)
        {
            InitializeComponent();
            if (streetNum != 0) { input_textBox.Text = streetNum.ToString(); }
            BuildingName_label.Visible = false;
            BuildingOccupant_label.Visible = false;
            Building1856Name_label.Click += new EventHandler(SaveAddress_Click);
            Building1856Name_label.Text = "Save Relative Address";
            Building1869Name_label.Click += new EventHandler(SaveAddress_Click);
            Building1869Name_label.Text = "Save Relative Address";
            int iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            int iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            int iNewLocationX = iLocationX + 150;
            int iNewLocationY = iLocationY + (iSelectedRow * 22);
            int iMaxLocationY = iScreenHeight - 192;
            if (iMaxLocationY < iNewLocationY)
                iNewLocationY = iMaxLocationY;
            this.Location = new System.Drawing.Point(iNewLocationX, iNewLocationY);
        }
        //****************************************************************************************************************************
        public FBuildingItem(CSql SQL,
                             string TitleText,
                             int iLocationX,
                             int iLocationY,
                             int iSelectedRow)
        {
            InitializeComponent();
            input_textBox.Visible = false;
            BuildingName_label.Click += new EventHandler(BuildingName_Click);
            BuildingName_label.Text = TitleText + " " + BuildingName_label.Text;
            BuildingOccupant_label.Click += new EventHandler(BuildingOccupant_Click);
            BuildingOccupant_label.Text = TitleText + " " + BuildingOccupant_label.Text;
            if (TitleText == "New")
            {
                Building1856Name_label.Click += new EventHandler(Building1856Name_Click);
                Building1856Name_label.Text = TitleText + " " + Building1856Name_label.Text;
                Building1869Name_label.Click += new EventHandler(Building1869Name_Click);
                Building1869Name_label.Text = TitleText + " " + Building1869Name_label.Text;
            }
            else
            {
                Building1856Name_label.Visible = false;
                Building1869Name_label.Visible = false;
                this.Size = new System.Drawing.Size(201, 120);
            }
            int iScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            int iScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            int iNewLocationX = iLocationX + 150;
            int iNewLocationY = iLocationY + (iSelectedRow * 22);
            int iMaxLocationY = iScreenHeight - 192;
            if (iMaxLocationY < iNewLocationY)
                iNewLocationY = iMaxLocationY;
            this.Location = new System.Drawing.Point(iNewLocationX, iNewLocationY);
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
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.BuildingOccupant_label = new System.Windows.Forms.Label();
            this.BuildingName_label = new System.Windows.Forms.Label();
            this.Building1856Name_label = new System.Windows.Forms.Label();
            this.Building1869Name_label = new System.Windows.Forms.Label();
            this.input_textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BuildingOccupant_label
            // 
            this.BuildingOccupant_label.AutoSize = true;
            this.BuildingOccupant_label.Location = new System.Drawing.Point(34, 51);
            this.BuildingOccupant_label.Name = "BuildingOccupant_label";
            this.BuildingOccupant_label.Size = new System.Drawing.Size(94, 13);
            this.BuildingOccupant_label.TabIndex = 2;
            this.BuildingOccupant_label.Text = "Building Occupant";
            this.BuildingOccupant_label.Click += new System.EventHandler(this.BuildingOccupant_label_Click);
            // 
            // BuildingName_label
            // 
            this.BuildingName_label.AutoSize = true;
            this.BuildingName_label.Location = new System.Drawing.Point(34, 22);
            this.BuildingName_label.Name = "BuildingName_label";
            this.BuildingName_label.Size = new System.Drawing.Size(75, 13);
            this.BuildingName_label.TabIndex = 1;
            this.BuildingName_label.Text = "Building Name";
            // 
            // Building1856Name_label
            // 
            this.Building1856Name_label.AutoSize = true;
            this.Building1856Name_label.Location = new System.Drawing.Point(34, 79);
            this.Building1856Name_label.Name = "Building1856Name_label";
            this.Building1856Name_label.Size = new System.Drawing.Size(102, 13);
            this.Building1856Name_label.TabIndex = 4;
            this.Building1856Name_label.Text = "Building 1856 Name";
            // 
            // Building1869Name_label
            // 
            this.Building1869Name_label.AutoSize = true;
            this.Building1869Name_label.Location = new System.Drawing.Point(34, 112);
            this.Building1869Name_label.Name = "Building1869Name_label";
            this.Building1869Name_label.Size = new System.Drawing.Size(102, 13);
            this.Building1869Name_label.TabIndex = 4;
            this.Building1869Name_label.Text = "Building 1869 Name";
            // 
            // input_textBox
            // 
            this.input_textBox.Location = new System.Drawing.Point(37, 38);
            this.input_textBox.Name = "input_textBox";
            this.input_textBox.Size = new System.Drawing.Size(100, 20);
            this.input_textBox.TabIndex = 5;
            // 
            // FBuildingItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 179);
            this.Controls.Add(this.input_textBox);
            this.Controls.Add(this.Building1856Name_label);
            this.Controls.Add(this.Building1869Name_label);
            this.Controls.Add(this.BuildingOccupant_label);
            this.Controls.Add(this.BuildingName_label);
            this.Name = "FBuildingItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        //****************************************************************************************************************************
        private void SaveAddress_Click(object sender, EventArgs e)
        {
            m_iBuildingItem = input_textBox.Text.ToInt();
            if (m_iBuildingItem != 0) { this.Close(); }
        }
        //****************************************************************************************************************************
        private void Building_Click(object sender, EventArgs e)
        {
            m_iBuildingItem = U.iBuilding;
            this.Close();
        }
        //****************************************************************************************************************************
        private void BuildingName_Click(object sender, EventArgs e)
        {
            m_iBuildingItem = U.iBuildingName;
            this.Close();
        }
        //****************************************************************************************************************************
        private void BuildingOccupant_Click(object sender, EventArgs e)
        {
            m_iBuildingItem = U.iOccupant;
            this.Close();
        }
        //****************************************************************************************************************************
        private void Building1856Name_Click(object sender, EventArgs e)
        {
            m_iBuildingItem = U.i1856BuildingName;
            this.Close();
        }
        //****************************************************************************************************************************
        private void Building1869Name_Click(object sender, EventArgs e)
        {
            m_iBuildingItem = U.i1869BuildingName;
            this.Close();
        }
        //****************************************************************************************************************************
        private void BuildingCurrentOccupant_Click(object sender, EventArgs e)
        {
            m_iBuildingItem = U.iCurrentOwners;
            this.Close();
        }

        private void BuildingOccupant_label_Click(object sender, EventArgs e)
        {

        }
    }
}
