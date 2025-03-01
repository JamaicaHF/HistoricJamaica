using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    abstract class FCheckedListboxGroup : Form
    {
        protected CSql m_SQL;
        protected CheckedListBox GroupValues_checkedListBox;
        protected System.ComponentModel.IContainer components = null;
        protected DataTable m_Group_tbl;
        protected string m_sGroupValueTableName;
        private bool m_bBuildingGroup = false;
        //****************************************************************************************************************************
        public FCheckedListboxGroup(CSql SQL,
                             DataTable Group_tbl,
                             string  sGroupValueTableName)
        {
            m_SQL = SQL;
            m_Group_tbl = Group_tbl;
            m_sGroupValueTableName = sGroupValueTableName;
            InitializeComponent();
            this.GroupValues_checkedListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ShowValues_Click);
            m_bBuildingGroup = (sGroupValueTableName.IndexOf("Building") >= 0);
            PopulateGroupValuesListBox();
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
            this.GroupValues_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // GroupValues_checkedListBox
            // 
            this.GroupValues_checkedListBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.GroupValues_checkedListBox.CheckOnClick = true;
            this.GroupValues_checkedListBox.FormattingEnabled = true;
            this.GroupValues_checkedListBox.Location = new System.Drawing.Point(0, 0);
            this.GroupValues_checkedListBox.Name = "GroupValues_checkedListBox";
            this.GroupValues_checkedListBox.Size = new System.Drawing.Size(198, 499);
            this.GroupValues_checkedListBox.TabIndex = 0;
            // 
            // FCheckedListboxGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 497);
            this.Controls.Add(this.GroupValues_checkedListBox);
            this.Location = new System.Drawing.Point(750, 150);
            this.Name = "FCheckedListboxGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Group Values";
            this.ResumeLayout(false);

            this.ContextMenu = new ContextMenu();
        }
        #endregion
        //****************************************************************************************************************************
        protected virtual void AddGroupValue_Click(object sender, EventArgs e)
        {
        }
        //****************************************************************************************************************************
        protected void SetToHide()
        {
            int iCount = GroupValues_checkedListBox.Items.Count;
            string s = GroupValues_checkedListBox.GetItemCheckState(0).ToString();
        }
        //****************************************************************************************************************************
        protected virtual void AddValuesForGroup(int iGroupId) { }
        //****************************************************************************************************************************
        protected virtual void PopulateGroupValuesListBox() { }
        //****************************************************************************************************************************
        protected virtual void AddValuesToGrid(int iRow,
                                               ArrayList ListOfCheckedItems) { }
        //****************************************************************************************************************************
        private int RowOfGroupBasedOnValue(int iRowIndex)
        {
            if (m_bBuildingGroup)
                return iRowIndex;
            string sGroupID;
            if (iRowIndex >= 0)
            do
            {
                iRowIndex--;
                sGroupID = GroupValues_checkedListBox.Items[iRowIndex].ToString();
            } while (sGroupID[0] == ' ');
            return iRowIndex;
        }
        //****************************************************************************************************************************
        public virtual int GetGroupValues(int        iRowIndex,
                                     ref string sGroup,
                                     ref string sGroupValue)
        {
            if (iRowIndex >= GroupValues_checkedListBox.CheckedItems.Count)
                return 0;
            sGroup = GroupValues_checkedListBox.CheckedItems[iRowIndex].ToString();
            if (sGroup[0] == ' ') // it is really a value, not a Group
            {
                sGroupValue = sGroup;
                sGroup = "";
            }
            else
            {
                iRowIndex++;
                if (iRowIndex >= GroupValues_checkedListBox.CheckedItems.Count)
                    return 0;
                sGroupValue = GroupValues_checkedListBox.CheckedItems[iRowIndex].ToString();
                while (iRowIndex >= GroupValues_checkedListBox.CheckedItems.Count && sGroup[0] != ' ')
                { // this is really a Group
                    sGroup = sGroupValue;
                    iRowIndex++;
                    sGroupValue = GroupValues_checkedListBox.CheckedItems[iRowIndex].ToString();
                }
            }   
            if (iRowIndex >= GroupValues_checkedListBox.CheckedItems.Count)
                return 0;
            if (sGroup.Length == 0)
            {
                int iGroupValueIndex = GroupValues_checkedListBox.CheckedIndices[iRowIndex].ToInt();
                do
                {
                    iGroupValueIndex--;
                    sGroup = GroupValues_checkedListBox.Items[iGroupValueIndex].ToString();
                } while (sGroup[0] == ' ');
            }
            iRowIndex++;
            return iRowIndex;
        }
        //****************************************************************************************************************************
        public bool IsCheckedGroup(string sGroup)
        {
            bool bFoundGroup = false;
            for (int i = 0; i < GroupValues_checkedListBox.CheckedItems.Count; i++)
            {
                if (sGroup.TrimString() == GroupValues_checkedListBox.CheckedItems[i].TrimString())
                {
                    bFoundGroup = true;
                    break;
                }
            }
            return bFoundGroup;
        }
        //****************************************************************************************************************************
        private bool RemoveAllValuesFromGrid(int           iRowIndex,
                                             bool          bRemoveAllValues,
                                             ref ArrayList ListOfCheckedItems)
        {
            if (m_bBuildingGroup)
                return false;
            iRowIndex++;
            bool bAllValuesAreChecked = true;
            string sGroup;
            do
            {
                string sGroupID = GroupValues_checkedListBox.Items[iRowIndex].ToString();
                if (IsCheckedGroup(sGroupID))
                {
                    if (bRemoveAllValues)
                        GroupValues_checkedListBox.Items.RemoveAt(iRowIndex);
                    else
                    {
                        ListOfCheckedItems.Add(sGroupID);
                        iRowIndex++;
                    }
                }
                else
                {
                    bAllValuesAreChecked = false;
                    GroupValues_checkedListBox.Items.RemoveAt(iRowIndex);
                }
                if (iRowIndex >= GroupValues_checkedListBox.Items.Count)
                {
                    sGroup = "";
                }
                else
                {
                    sGroup = GroupValues_checkedListBox.Items[iRowIndex].ToString();
                }
            } while (!String.IsNullOrEmpty(sGroup) && sGroup[0] == ' ');
            return !bAllValuesAreChecked;
        }
        //****************************************************************************************************************************
        private void RemoveValuesFromGrid(int iRowIndex)
        {
            int iOriginalRowIndex = iRowIndex;
            ArrayList ListOfCheckedItems = new ArrayList();
            bool bRemoveCheckedValues = false;
            if (!RemoveAllValuesFromGrid(iRowIndex, bRemoveCheckedValues, ref ListOfCheckedItems))
            {
                bRemoveCheckedValues = true;
                RemoveAllValuesFromGrid(iRowIndex, bRemoveCheckedValues, ref ListOfCheckedItems);
                AddValuesToGrid(iOriginalRowIndex, ListOfCheckedItems);
            }
        }
        //****************************************************************************************************************************
        protected bool ValueInList(ArrayList ListOfCheckedItems,
                                 string    sValue)
        {
            bool bFound = false;
            foreach (string sString in ListOfCheckedItems)
            {
                if (sString == sValue)
                    return true;
            }
            return bFound;
        }
        //****************************************************************************************************************************
        public void ShowValues_Click(object sender, EventArgs e)
        {
            int iRowIndex = GroupValues_checkedListBox.SelectedIndex;
            CheckBox chkAll = new CheckBox();
            chkAll.Checked = true;
            string sGroup = GroupValues_checkedListBox.Items[iRowIndex].ToString();
            if (sGroup[0] == ' ')
                RemoveValuesFromGrid(RowOfGroupBasedOnValue(iRowIndex));
            else
            {
                int iNextRowIndex = iRowIndex + 1;
                ArrayList ListOfCheckedItems = new ArrayList();
                if (iNextRowIndex >= GroupValues_checkedListBox.Items.Count)
                    AddValuesToGrid(iRowIndex, ListOfCheckedItems);
                else
                {
                    string sNextGroupID = GroupValues_checkedListBox.Items[iNextRowIndex].ToString();
                    if (sNextGroupID[0] == ' ')
                        RemoveValuesFromGrid(iRowIndex);
                    else
                        AddValuesToGrid(iRowIndex, ListOfCheckedItems);
                }
            }
        }
        //****************************************************************************************************************************
        public virtual int GetGroupID(string sGroup) { return 0; }
        //****************************************************************************************************************************
        public virtual int GetGroupValueID(int iGroupID,
                                           string sGroupIDValue) { return 0; }
        //****************************************************************************************************************************
        public void ShowValues_Click()
        {
            for (int i = 0; i < GroupValues_checkedListBox.CheckedItems.Count; i++)
            {
                string SelectedValues = GroupValues_checkedListBox.CheckedItems[i].ToString();
            }
        }
        //****************************************************************************************************************************
    }
}