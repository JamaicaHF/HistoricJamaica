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
    class CGridSchoolRecords : CGridGroupValue
    {
        private bool m_bExitOnDoubleClick = false;
        private DataTable schoolRecords_tbl = new DataTable();
        //****************************************************************************************************************************
        public CGridSchoolRecords(CSql SQL,
                         bool bExitOnDoubleClick): base(SQL)
        {
            m_bExitOnDoubleClick = bExitOnDoubleClick;
        }
        //****************************************************************************************************************************
        protected override void InitializeGroupObjects()
        {
            General_DataGridView.Columns[0].Name = "School Records";
            toolStripItem1.Click += new EventHandler(IntegrateGroup_Click);
            toolStripItem1.Text = "Integrate Students and Teachers";
            ToolStrip.Items.Add(toolStripItem2);
            toolStripItem2.Click += new EventHandler(ViewGroup_Click);
            toolStripItem2.Text = "View Students and Teachers";
            ToolStrip.Items.Add(toolStripItem1);
        }
        //****************************************************************************************************************************
        protected override void ViewGroup_Click(object sender, EventArgs e)
        {
            int iRowIndex = mouseLocation.RowIndex;
            DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
            //General_DataGridView.Rows.Add(school + "-" + currentSchoolYear, currentSchoolId, currentSchoolYear);
            int schoolId = dataGridViewRow.Cells[1].Value.ToInt();
            int schoolYear = dataGridViewRow.Cells[2].Value.ToInt();
            int grade = Grade(dataGridViewRow);
            CGridStudentRecords gridStudentRecords = new CGridStudentRecords(ref m_SQL, schoolId, schoolYear, grade, m_bExitOnDoubleClick);
            gridStudentRecords.ShowDialog();
        }
        //****************************************************************************************************************************
        private int Grade(DataGridViewRow dataGridViewRow)
        {
            string value = dataGridViewRow.Cells[0].Value.ToString();
            int indexOf = value.ToLower().IndexOf("grade");
            int grade = 0;
            if (indexOf > 0)
            {
                grade = value.Substring(indexOf + 6).ToInt(); ;
            }
            return grade;
        }
        //****************************************************************************************************************************
        protected void IntegrateGroup_Click(object sender, EventArgs e)
        {
            try
            {
                int iRowIndex = mouseLocation.RowIndex;
                DataGridViewRow dataGridViewRow = General_DataGridView.Rows[iRowIndex];
                //General_DataGridView.Rows.Add(school + "-" + currentSchoolYear, currentSchoolId, currentSchoolYear);
                int schoolId = dataGridViewRow.Cells[1].Value.ToInt();
                int schoolYear = dataGridViewRow.Cells[2].Value.ToInt();
                int grade = Grade(dataGridViewRow);

                DataTable teacherRecords_tbl = SQL.GetSchoolTeacherRecord(schoolId, schoolYear, grade);
                DataTable schoolRecords_tbl = SQL.GetSchoolRecords(schoolId, schoolYear, grade);
                foreach (DataRow teacherRecord_row in teacherRecords_tbl.Rows)
                {
                    integrate(teacherRecord_row);
                }
                SQL.SaveIntegratedSchoolRecords(teacherRecords_tbl);
                foreach (DataRow studentRecord_row in schoolRecords_tbl.Rows)
                {
                    integrate(studentRecord_row);
                }
                SQL.SaveIntegratedSchoolRecords(schoolRecords_tbl);
                MessageBox.Show("Integration Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //****************************************************************************************************************************
        private void integrate(DataRow schoolRecord_row)
        {
            if (schoolRecord_row[U.PersonID_col].ToInt() != 0)
            {
                return;
            }
            DataTable AlternativeSpellingsFirstNameTbl = SQL.GetAllAlternativeSpellings(U.AlternativeSpellingsFirstName_Table);
            DataTable AlternativeSpellingsLastNameTbl = SQL.GetAllAlternativeSpellings(U.AlternativeSpellingsLastName_Table);
            CIntegrateSchoolRecord integrateSchoolRecord = new CIntegrateSchoolRecord(m_SQL, false);
            integrateSchoolRecord.IntegrateRecord(schoolRecord_row,
                                                  AlternativeSpellingsFirstNameTbl,
                                                  AlternativeSpellingsLastNameTbl);
        }
        //****************************************************************************************************************************
        protected override string GroupValueID(DataRow row)
        {
            return row[U.SchoolID_col].ToString();
        }
        //****************************************************************************************************************************
        protected override string GroupValueOrder(DataRow row)
        {
            return row[U.Year_col].ToString();
        }
        //****************************************************************************************************************************
        protected override void GetAllPhotosFromValue(DataTable tbl,
                                                       int iGroupValueID)
        {
        }
        //****************************************************************************************************************************
        protected override string GroupValueValue(DataRow row)
        {
            string grade = row[U.Grade_col].ToString();
            if (String.IsNullOrEmpty(grade))
            {
                return "All Grades";
            }
            if (grade.ToLower() == "primary" || grade.ToLower() == "grammer")
            {
                return grade;
            }
            return "Grade " + grade;
        }
        //****************************************************************************************************************************
        protected override void PopulateDataGridView()
        {
            SQL.GetDistinctValues(schoolRecords_tbl, U.SchoolRecord_Table, U.SchoolID_col, U.Year_col, U.Grade_col, new NameValuePair(U.SchoolRecordType_col, 1));
            int iRow = 0;
            int currentSchoolId = 0;
            int currentSchoolYear = 0;
            string school = "";
            foreach (DataRow row in schoolRecords_tbl.Rows)
            {
                iRow++;
                if (row[U.SchoolID_col].ToInt() != currentSchoolId)
                {
                    currentSchoolId = row[U.SchoolID_col].ToInt();
                    school = SQL.GetSchool(currentSchoolId);
                    currentSchoolYear = 0;
                }
                if (row[U.Year_col].ToInt() != currentSchoolYear)
                {
                    currentSchoolYear = row[U.Year_col].ToInt();
                    General_DataGridView.Rows.Add(school + "-" + currentSchoolYear, currentSchoolId, currentSchoolYear);
                }
            }
            General_DataGridView.Rows[0].Cells[0].Selected = true;
        }
        //****************************************************************************************************************************
        protected override void GetAllGroupValues(DataTable tbl,
                                                  int iGroupID,
                                                 int iGroupID2 = 0)
        {
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.SchoolID_col, typeof(int));
            tbl.Columns.Add(U.Year_col, typeof(int));
            tbl.Columns.Add(U.Grade_col, typeof(string));
            string selectStatement = U.SchoolID_col + "=" + iGroupID + " and " +
                                     U.Year_col + "=" + iGroupID2;
            DataRow[] foundRows = schoolRecords_tbl.Select(selectStatement);
            foreach (DataRow dr in foundRows) 
            {
                tbl.Rows.Add(dr.ItemArray);
            }
        }
        //****************************************************************************************************************************
    }
}
