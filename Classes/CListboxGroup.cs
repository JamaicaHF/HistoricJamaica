using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Text;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    class CListboxGroup
    {
        private CSql m_SQL;
        ListBoxWithDoubleClick m_Group_listBox;
        private string m_sGroupValueTableName;
        private string m_sGroupValueValue_col;
        //****************************************************************************************************************************
        public CListboxGroup(CSql SQL,
                             ListBoxWithDoubleClick Group_listBox,
                             string                 sGroupValueTableName,
                             string                 sGroupValueValue_col)
        {
            m_Group_listBox = Group_listBox;
            m_sGroupValueTableName = sGroupValueTableName;
            m_sGroupValueValue_col = sGroupValueValue_col;
            m_SQL = SQL;
        }
        //****************************************************************************************************************************
        private void CheckForDeletes(FCheckedListboxGroup Group_listBox,
                                     ref DataTable Group_tbl)
        {
            DataViewRowState dvRowState = DataViewRowState.Added | 
                                          DataViewRowState.Unchanged;
            foreach (DataRow row in Group_tbl.Select("", "", dvRowState))
            {
                string sGroup = row[m_sGroupValueValue_col].ToString();
                if (!Group_listBox.IsCheckedGroup(sGroup))
                    row.Delete();
            }
        }
        //****************************************************************************************************************************
        private bool WasPreviouslyDeleted(DataTable Group_tbl,
                                           string GroupIDValue)
        {
            DataViewRowState dvRowState = DataViewRowState.Deleted;
            foreach (DataRow row in Group_tbl.Select("", "", dvRowState))
            {
                if (row[m_sGroupValueValue_col, DataRowVersion.Original].TrimString() == GroupIDValue)
                {
                    row.RejectChanges();
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool GroupIDValueExists(DataTable Group_tbl,
                                        string  GroupIDValue)
        {
            if (WasPreviouslyDeleted(Group_tbl, GroupIDValue))
                return true;
            DataViewRowState dvRowState = DataViewRowState.Added |
                                          DataViewRowState.Unchanged;
            foreach (DataRow row in Group_tbl.Select("", "", dvRowState))
            {
                if (row[m_sGroupValueValue_col].TrimString() == GroupIDValue)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        public bool ShowGroupListbox(ref DataTable Group_tbl)
        {
            ArrayList PreviousGroupItems = new ArrayList();
            foreach (string item in m_Group_listBox.Items)
            {
                PreviousGroupItems.Add(item);
            }
            FCheckedListboxGroup Group_listBox;
            Group_listBox = new CCheckedListboxCategory(m_SQL, Group_tbl, m_sGroupValueTableName);
            Group_listBox.ShowDialog();
            Group_listBox.ShowValues_Click();
            string sPreviousGroup = "";
            int iRowIndex = 0;
            CheckForDeletes(Group_listBox, ref Group_tbl);
            ArrayList newListboxValues = new ArrayList();
            m_Group_listBox.Items.Clear();
            do
            {
                string sGroup = "";
                string sGroupIDValue = "";
                iRowIndex = Group_listBox.GetGroupValues(iRowIndex, ref sGroup, ref sGroupIDValue);
                if (iRowIndex != 0)
                {
                    if (sGroup != sPreviousGroup)
                    {
                        m_Group_listBox.Items.Add(sGroup);
                        sPreviousGroup = sGroup;
                    }
                    m_Group_listBox.Items.Add(sGroupIDValue);
                    if (!GroupIDValueExists(Group_tbl, sGroupIDValue.TrimString()))
                    {
                        int iGroupID = Group_listBox.GetGroupID(sGroup);
                        Group_tbl.Rows.Add(Group_listBox.GetGroupValueID(iGroupID, sGroupIDValue.TrimString()),
                                                                        iGroupID, sGroupIDValue.TrimString());
                    }
                }
            } while (iRowIndex != 0);
            if (m_Group_listBox.Items.Count != PreviousGroupItems.Count)
            {
                return true;
            }
            int count = 0;
            foreach (string item in m_Group_listBox.Items)
            {
                if (PreviousGroupItems[count].ToString().ToLower().Trim() != item.ToLower().Trim())
                {
                    return true;
                }
                count++;
            }
            return false;
        }
    }
}
