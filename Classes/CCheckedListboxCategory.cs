using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    class CCheckedListboxCategory : FCheckedListboxGroup
    {
        //****************************************************************************************************************************
        public CCheckedListboxCategory(CSql    SQL,
                                DataTable category_tbl,
                                string sCategoryValueTableName): base(SQL, category_tbl, sCategoryValueTableName)
        {
            this.Text = "Category Values";
            this.ContextMenu.MenuItems.Add("Add New Category Value", new EventHandler(AddGroupValue_Click));
        }
        //****************************************************************************************************************************
        protected override void PopulateGroupValuesListBox()
        {
            GroupValues_checkedListBox.Items.Clear();
            DataTable tbl = new DataTable();
            SQL.GetAllCategories(tbl);
            if (tbl.Rows.Count != 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    int iCategoryId = row[U.CategoryID_col].ToInt();
                    string SCategoryName = row[U.CategoryName_col].ToString();
                    GroupValues_checkedListBox.Items.Add(SCategoryName, CheckState.Unchecked);
                    AddValuesForGroup(iCategoryId);
                }
                SetToHide();
            }
        }
        //****************************************************************************************************************************
        protected override void AddGroupValue_Click(object sender, EventArgs e)
        {
            CGridGroupValuesCategory GridGroupValuesCategory = new CGridGroupValuesCategory(m_SQL);
            GridGroupValuesCategory.ShowDialog();
            PopulateGroupValuesListBox();
        }
        //****************************************************************************************************************************
        protected override void AddValuesToGrid(int iRow,
                                                ArrayList ListOfCheckedItems)
        {
            int i = ListOfCheckedItems.Count;
            string sCategory = GroupValues_checkedListBox.Items[iRow].ToString();
            int iCategoryID = SQL.GetCategoryIDFromName(sCategory);
            if (iCategoryID != 0)
            {
                DataTable tbl = new DataTable(U.CategoryValue_Table);
                SQL.GetAllCategoryValues(tbl, iCategoryID);
                foreach (DataRow row in tbl.Rows)
                {
                    iRow++;
                    string sValue = UU.ShowGroupValue(row[U.CategoryValueValue_col].ToString());
                    bool bChecked = ValueInList(ListOfCheckedItems, sValue);
                    GroupValues_checkedListBox.Items.Insert(iRow, sValue);
                    if (bChecked)
                        GroupValues_checkedListBox.SetItemChecked(iRow, true);
                }
            }
        }
        //****************************************************************************************************************************
        protected override void AddValuesForGroup(int iGroupId)
        {
            foreach (DataRow row in m_Group_tbl.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row[U.CategoryID_col].ToInt() == iGroupId)
                {
                    string sGroupIDValue = row[U.CategoryValueValue_col].ToString();
                    if (sGroupIDValue[0] != ' ')
                        sGroupIDValue = UU.ShowGroupValue(sGroupIDValue);
                    GroupValues_checkedListBox.Items.Add(sGroupIDValue, CheckState.Checked);
                }
            }
        }
        //****************************************************************************************************************************
        public override int GetGroupID(string sGroup)
        {
            return SQL.GetCategoryIDFromName(sGroup);
        }
        //****************************************************************************************************************************
        public override int GetGroupValueID(int iGroupID,
                                   string sGroupIDValue)
        {
            return SQL.GetCategoryValueID(iGroupID.ToString(), sGroupIDValue.TrimString());
        }
    }
}
