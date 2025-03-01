using System;
using System.Collections;
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
    public class FSpouseLivedWith : Form
    {
        private CSql m_SQL = null;
        private ListBoxWithDoubleClick SpouseLivedWith_listBox;
        private int m_iSelectedSpouse = -1;
        ArrayList ListOfSpouseIDs = new ArrayList();
        public FSpouseLivedWith(CSql SQL,
                                int iPersonID,
                                int iBuildingValueID,
                                int iSpouseLivedWithID)
        {
            m_SQL = SQL;
            InitializeComponent();
            this.SpouseLivedWith_listBox.ContextMenu = new ContextMenu();
            this.SpouseLivedWith_listBox.ContextMenu.MenuItems.Clear();
            SpouseLivedWith_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SelectSpouse_Click);
            LoadSpousesIntoListbox(iPersonID, iSpouseLivedWithID);
        }
        //****************************************************************************************************************************
        private void LoadSpousesIntoListbox(int iPersonID,
                                            int iSpouseLivedWithID)
        {
            DataTable Marriages_tbl = SQL.DefineMarriageTable();
            int iNumberSpouses = 0;
            int iSpouseLocationInArray;
            SQL.GetMarriagesID(Marriages_tbl, ref iNumberSpouses, out iSpouseLocationInArray, iPersonID);
            ListOfSpouseIDs.Add(0);
            SpouseLivedWith_listBox.Items.Add("Did Not Live With Spouse In This House");
            SpouseLivedWith_listBox.SelectedIndex = 0;
            foreach (DataRow row in Marriages_tbl.Rows)
            {
                int iSpouseID = row[iSpouseLocationInArray].ToInt();
                if (iSpouseID != 0)
                {
                    ListOfSpouseIDs.Add(iSpouseID);
                    DataRow Spouse_row = SQL.GetPerson(iSpouseID);
                    if (Spouse_row != null)
                    {
                        SpouseLivedWith_listBox.Items.Add(SQL.GetPersonNameFromRow(Spouse_row));
                        if (iSpouseID == iSpouseLivedWithID)
                            SpouseLivedWith_listBox.SelectedIndex = SpouseLivedWith_listBox.Items.Count - 1;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        public int GetSelectedSpouse()
        {
            return m_iSelectedSpouse;
        }
        //****************************************************************************************************************************
        private void SelectSpouse_Click(object sender, System.EventArgs e)
        {
            int iSelectedIndex = SpouseLivedWith_listBox.SelectedIndex;
            if (iSelectedIndex <= ListOfSpouseIDs.Count)
                m_iSelectedSpouse = ListOfSpouseIDs[iSelectedIndex].ToInt();
            this.Close();
        }

        private System.ComponentModel.IContainer components = null;
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
            this.SpouseLivedWith_listBox = new Utilities.ListBoxWithDoubleClick();
            this.SuspendLayout();
            // 
            // SpouseLivedWith_listBox
            // 
            this.SpouseLivedWith_listBox.FormattingEnabled = true;
            this.SpouseLivedWith_listBox.Location = new System.Drawing.Point(24, 38);
            this.SpouseLivedWith_listBox.Name = "SpouseLivedWith_listBox";
            this.SpouseLivedWith_listBox.Size = new System.Drawing.Size(233, 56);
            this.SpouseLivedWith_listBox.TabIndex = 0;
            // 
            // FSpouseLivedWith
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 142);
            this.Controls.Add(this.SpouseLivedWith_listBox);
            this.Name = "FSpouseLivedWith";
            this.Text = "New Building Options";
            this.ResumeLayout(false);

        }

        #endregion

    }
}
