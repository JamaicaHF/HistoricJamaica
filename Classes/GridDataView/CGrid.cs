using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    enum InsertMode
    {
        NotInsertEditMode,
        EditGroup,
        EditGroupValue,
        InsertGroup,
        InsertGroupValue
    }
    abstract class CGrid: FGrid
    {
        protected int mouseDownRow = 0;
        protected int currentMouseRow = 0;
        protected bool movingMouse = false;
        protected int CurrentGroupIndex = 0;
        protected int NextGroupIndex = 0;
        protected int m_iEditLocation = -1;
        protected int m_iCurrentRow = 0;
        protected int m_iCurrentColumn = 0;
        protected bool m_bOrderChanged = false;
        protected bool m_bRightClick = false;
        protected bool m_bShowValues = true;
        protected InsertMode m_eInsertMode = InsertMode.NotInsertEditMode;
        protected DataGridViewCellEventArgs mouseLocation;
        //****************************************************************************************************************************
        public CGrid()
        {
        }
        //****************************************************************************************************************************
        protected virtual void CheckValue() {}
        protected virtual void SetToEditMode() {}
        protected virtual void ResetBooleans() {}
        //****************************************************************************************************************************
        protected int NumElements()
        {
            return General_DataGridView.Rows.Count - 1;
        }
        //****************************************************************************************************************************
        protected void SetActiveRowColumn(int iRow,
                                          int iColumn)
        {
//            General_DataGridView.FirstDisplayedScrollingRowIndex = 0;
//            General_DataGridView.Refresh(); 
            General_DataGridView.CurrentCell.Selected = false;
            m_iCurrentRow = iRow;
            m_iCurrentColumn = iColumn;
//            General_DataGridView.Rows[m_iCurrentRow].Cells[m_iCurrentColumn].Selected = true;
            General_DataGridView.CurrentCell = General_DataGridView.Rows[m_iCurrentRow].Cells[m_iCurrentColumn];
        }
        //****************************************************************************************************************************
        private bool InvalidRow()
        {
            if (m_iCurrentRow >= NumElements())
            {
                int iActiveRow = NumElements() - 1;
                if (iActiveRow < 0)
                    iActiveRow = 0;
                SetActiveRowColumn(iActiveRow, 0);
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected void MouseClick_click(object sender, EventArgs e)
        {
            m_iCurrentRow = mouseLocation.RowIndex;
            m_iCurrentColumn = mouseLocation.ColumnIndex;
            if (mouseLocation.RowIndex != m_iEditLocation)
                CheckValue();
            if (InvalidRow())
                return;
        }
        //****************************************************************************************************************************
        protected void CellEndEditEvent(object sender, DataGridViewCellEventArgs e)
        {
        }  //Although this event has not code, it is required to make the ProcessCmdKey below event work with the enter key.
        //****************************************************************************************************************************
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            m_iCurrentRow = General_DataGridView.CurrentCell.RowIndex;
            m_iCurrentColumn = General_DataGridView.CurrentCell.ColumnIndex;
            int KeyValue = msg.WParam.ToInt32();
            if (InvalidRow())
                return false;
            switch (KeyValue)
            {
                case 9:  //tab
                    if (InvalidRow())
                        return false;
                    break;
                case 13: //enter
                case 40: //DownArrow
                    {

                        if (m_eInsertMode == InsertMode.NotInsertEditMode)
                        {
                            if (m_iCurrentRow < (NumElements() - 1))
                                m_iCurrentRow++;
                        }
                        else
                        {
                            General_DataGridView.Rows[m_iCurrentRow + 1].Cells[0].Selected = true;
                            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[m_iCurrentRow];
                            dataGridViewRow.Cells[0].Selected = true;

                            if (dataGridViewRow.Cells[0].Value == null)
                            {
                                string currentRowValue = General_DataGridView.CurrentCell.Value.ToString();
                            }
                            General_DataGridView.Rows[m_iCurrentRow].Cells[0].Selected = true;
                            CheckValue();
                        }
                        break;
                    }
                case 16: // shift 
                case 17: // Control
                case 18: // Alt
                case 20: // CapsLock
                case 91: // microsoft
                    break;
                case 38: //UpArrow
                    {
                        if (m_eInsertMode == InsertMode.NotInsertEditMode)
                        {
                            if (m_iCurrentRow > 0)
                                m_iCurrentRow--;
                        }
                        else
                        {
                            General_DataGridView.Rows[m_iCurrentRow + 1].Cells[0].Selected = true;
                            CheckValue();
                        }
                    }
                    break;
                case 27: //escape
                    {
                        if (m_eInsertMode == InsertMode.InsertGroup || m_eInsertMode == InsertMode.InsertGroupValue)
                        {
                            General_DataGridView.Rows.RemoveAt(m_iEditLocation);
                            m_eInsertMode = InsertMode.NotInsertEditMode;
                        }
                        ResetBooleans();
                        break;
                    }
                case 8:  // BackSpace
                case 32: // Space
                case 37: // Left Arrow
                case 39: // Right Arrow
                case 46: // Del
                    SetToEditMode();
                    break;
                default:
                    {
                        if (KeyValue > 47)
                            SetToEditMode();
                        break;
                    }
            }
//            SetActiveRowColumn(m_iCurrentRow, 0);
            return false;
        }
        //****************************************************************************************************************************
        protected void OnMouseMoveClick(object sender, MouseEventArgs e)
        {
            if (movingMouse)
            {
                DataGridView.HitTestInfo hit = General_DataGridView.HitTest(e.X, e.Y);
                int iNewLocation = hit.RowIndex;
                if (iNewLocation >= 0 && iNewLocation > CurrentGroupIndex && iNewLocation < NextGroupIndex &&
                    iNewLocation < General_DataGridView.Rows.Count && iNewLocation != currentMouseRow)
                {
                    General_DataGridView.Rows[iNewLocation].Cells[0].Selected = true;
                    currentMouseRow = iNewLocation;
                }
            }
        }
        //****************************************************************************************************************************
        private void CopyRow(DataGridViewRow row,
                             int iRowIndex)
        {
            int iNumObjects = General_DataGridView.Columns.Count;
            for (int i = 0; i < iNumObjects; i++)
            {
                General_DataGridView.Rows[iRowIndex].Cells[i].Value = row.Cells[i].Value;
            }
        }
        //****************************************************************************************************************************
        private void MoveRowUp()
        {
            for (int i = mouseDownRow; i > currentMouseRow; i--)
            {
                DataGridViewRow row = General_DataGridView.Rows[i - 1];
                CopyRow(row, i);
            }
        }
        //****************************************************************************************************************************
        private void MoveRowDown()
        {
            for (int i = mouseDownRow; i < currentMouseRow; i++)
            {
                DataGridViewRow row = General_DataGridView.Rows[i + 1];
                CopyRow(row, i);
            }
        }
        //****************************************************************************************************************************
        protected void OnMouseUpClick(object sender, MouseEventArgs e)
        {
            //if (General_DataGridView.Rows[0].Cells.Count >= 4)
            //{
            //    return;
            //}
            currentMouseRow = currentMouseRowBelowOwner(currentMouseRow);
            if (movingMouse && currentMouseRow != mouseDownRow)
            {
                m_bOrderChanged = true;
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[mouseDownRow];
                int iNumObjects = General_DataGridView.Columns.Count;
                object[] oObjects = new object[iNumObjects];
                for (int i = 0; i < iNumObjects; i++)
                {
                    oObjects[i] = dataGridViewRow.Cells[i].Value;
                }

                if (currentMouseRow < mouseDownRow)
                    MoveRowUp();
                else
                    MoveRowDown();

                dataGridViewRow = General_DataGridView.Rows[currentMouseRow];
                for (int i = 0; i < iNumObjects; i++)
                {
                    dataGridViewRow.Cells[i].Value = oObjects[i];
                }
            }
            movingMouse = false;
        }
        //****************************************************************************************************************************
        private int currentMouseRowBelowOwner(int currentMouseRow)
        {
            if (General_DataGridView.Rows[currentMouseRow].Cells.Count > 4)
            {
                while (currentMouseRow > 0 && General_DataGridView.Rows[currentMouseRow].Cells[4].Value.ToInt() == U.iCurrentOwners)
                {
                    currentMouseRow--;
                }
            }
            return currentMouseRow;
        }
        //****************************************************************************************************************************
        protected int RowOfGroupBasedOnValue(int iRowIndex)
        {
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sGroupID = dataGridViewRow.Cells[0].Value.ToString();
            while (iRowIndex > 0 && (sGroupID.Length == 0 || sGroupID[0] == ' '))
            {
                iRowIndex--;
                dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                sGroupID = dataGridViewRow.Cells[0].Value.ToString();
            }
            return iRowIndex;
        }
        //****************************************************************************************************************************
        protected bool IsGroupValue(int iRowIndex)
        {
            int iNumRows = General_DataGridView.Rows.Count - 1;
            if (iRowIndex >= iNumRows)
                return false;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            string sGroupID = dataGridViewRow.Cells[0].Value.ToString();
            return (sGroupID.Length == 0 || sGroupID[0] == ' ');
        }
        //****************************************************************************************************************************
        protected int NextGroup(int iRowIndex)
        {
            bool bFoundLocation = false;
            int iNumRows = General_DataGridView.Rows.Count - 2;
            do
            {
                iRowIndex++;
                if (iRowIndex > iNumRows)
                    return iRowIndex;
                if (!IsGroupValue(iRowIndex))
                    bFoundLocation = true;
            } while (!bFoundLocation);
            return iRowIndex;
        }
        //****************************************************************************************************************************
        protected virtual void UpdateOrder() {}
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UpdateOrder();
        }
        //****************************************************************************************************************************
        protected void OnMouseDownClick(object sender, MouseEventArgs e)
        {
            if (mouseLocation == null)
            {
                return;
            }
            int iRowIndex = mouseLocation.RowIndex;
            if (iRowIndex < 0)
            {
                return;
            }
            if (iRowIndex > 0 && iRowIndex >= NumElements())
            {
                iRowIndex--;
                General_DataGridView.Rows[iRowIndex].Selected = true;
                return;
            }
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            if (dataGridViewRow.Cells[0].Value == null)
            {
                return;
            }
            string sValue = dataGridViewRow.Cells[0].Value.ToString();
            if (!m_bShowValues && (sValue.Length == 0 || sValue[0] == ' '))
            {
                CurrentGroupIndex = RowOfGroupBasedOnValue(iRowIndex);
                mouseDownRow = iRowIndex;
                currentMouseRow = iRowIndex;
                if (General_DataGridView.Rows[iRowIndex].Cells.Count <= 4)
                    m_bRightClick = false;
                else
                if (!m_bRightClick && General_DataGridView.Rows[iRowIndex].Cells[4].Value.ToInt() != U.iCurrentOwners)
                    movingMouse = true;
                else
                    m_bRightClick = false;
            }
            if (m_bShowValues)
            {
                NextGroupIndex = 0;
            }
            else
            {
                NextGroupIndex = NextGroup(iRowIndex);
            }
        }
    }
}
