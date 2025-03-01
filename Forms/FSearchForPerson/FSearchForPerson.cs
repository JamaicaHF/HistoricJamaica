using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public partial class FSearchForPerson : Form
    {
        private CSql m_SQL;
        private eSearchOption SearchBy = eSearchOption.SO_None;
        //****************************************************************************************************************************
        public FSearchForPerson(CSql SQL)
        {
            m_SQL = SQL;
            InitializeComponent();
        }
        //****************************************************************************************************************************
        private void SearchStartingWith_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_StartingWith;
            SearchForPerson();
        }
        //****************************************************************************************************************************
        private void SearchSimilar_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_Similar;
            SearchForPerson();
        }
        //****************************************************************************************************************************
        private void NewPersonButton_Click(object sender, System.EventArgs e)
        {
            FPerson Person = new FPerson(m_SQL, false);
            Person.ShowDialog();
        }
        //****************************************************************************************************************************
        private void SearchForPerson()
        {
            PersonName personName = new PersonName(FirstName_textBox.Text, "", LastName_textBox.Text, "", "");
            DataTable vitalRecordTbl = SQL.DefineVitalRecord_Table();
            vitalRecordTbl.PrimaryKey = new DataColumn[] { vitalRecordTbl.Columns[U.VitalRecordID_col] };
            if (radioButtonPerson.Checked || radioButtonAll.Checked)
            {
                DataTable personTbl = SQL.DefinePersonTable();
                personTbl.PrimaryKey = new DataColumn[] { personTbl.Columns[U.PersonID_col] };
                SQL.PersonsBasedOnNameOptions(personTbl, true, U.Person_Table, U.PersonID_col, SearchBy, personName, "");
                SQL.ConvertPersonRecordsToVitalRecords(personTbl, vitalRecordTbl);
            }
            if (radioButtonCemeteryRecords.Checked || radioButtonAll.Checked)
            {
                DataTable CemeteryRecordTbl = SQL.DefineCemeteryRecordTable();
                CemeteryRecordTbl.PrimaryKey = new DataColumn[] { CemeteryRecordTbl.Columns[U.CemeteryRecordID_col] };
                SearchVitalRecords(CemeteryRecordTbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
                SearchVitalRecords(CemeteryRecordTbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col, U.SpouseFirstName_col, U.SpouseMiddleName_col, U.SpouseLastName_col, personName);
                SearchVitalRecords(CemeteryRecordTbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col, U.FatherFirstName_col, U.FatherMiddleName_col, U.FatherLastName_col, personName);
                SearchVitalRecords(CemeteryRecordTbl, U.CemeteryRecord_Table, U.CemeteryRecordID_col, U.MotherFirstName_col, U.MotherMiddleName_col, U.MotherLastName_col, personName);
                foreach (DataRow cemeteryRecordRow in CemeteryRecordTbl.Rows)
                {
                    SQL.ConvertCemeteryRecordsToVitalRecords(cemeteryRecordRow, vitalRecordTbl);
                }
            }
            if (radioButtonVitalRecords.Checked || radioButtonAll.Checked)
            {
                SearchVitalRecords(vitalRecordTbl, U.VitalRecord_Table, U.VitalRecordID_col, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
                SearchVitalRecords(vitalRecordTbl, U.VitalRecord_Table, U.VitalRecordID_col, U.FirstName_col, U.MiddleName_col, U.FatherLastName_col, personName);
                SearchVitalRecords(vitalRecordTbl, U.VitalRecord_Table, U.VitalRecordID_col, U.FatherFirstName_col, U.FatherMiddleName_col, U.FatherLastName_col, personName);
                SearchVitalRecords(vitalRecordTbl, U.VitalRecord_Table, U.VitalRecordID_col, U.MotherFirstName_col, U.MotherMiddleName_col, U.MotherLastName_col, personName);
                SearchVitalRecords(vitalRecordTbl, U.VitalRecord_Table, U.VitalRecordID_col, U.MotherFirstName_col, U.MotherMiddleName_col, U.FatherLastName_col, personName);
            }
            if (vitalRecordTbl.Rows.Count == 0)
            {
                MessageBox.Show("No Records Found");
            }
            else
            {
                GetVitalRecordFromGrid(vitalRecordTbl, "");
            }
        }
        //****************************************************************************************************************************
        private void SearchVitalRecords(DataTable tbl,
                                        string tableName,
                                        string idCol,
                                        string firstName_col,
                                        string middleName_col,
                                        string lastNameCol,
                                        PersonName personName)
        {
            if (String.IsNullOrEmpty(personName.lastName) &&
                String.IsNullOrEmpty(personName.firstName))
            {
                return;
            }
            PersonSearchTableAndColumns personSearchTableAndColumns = new PersonSearchTableAndColumns(tableName,
                              firstName_col, middleName_col, lastNameCol);
            SQL.PersonsBasedOnNameOptions(tbl, false, personSearchTableAndColumns, idCol, SearchBy, personName);
        }
        //****************************************************************************************************************************
        private void GetVitalRecordFromGrid(DataTable tbl,
                                            string startingWith)
        {
            CGridVitalRecord GridVitalRecord = new CGridVitalRecord(m_SQL, ref tbl);
            GridVitalRecord.SetStartingWith(startingWith);
            GridVitalRecord.ShowDialog();
            int iVitalRecordID = GridVitalRecord.SelectedVitalRecordID;
            if (iVitalRecordID > 9900000)
            {
                int cemeteryRecordId = iVitalRecordID - 9900000;
                FCemeteryRecord cemeteryRecord = new FCemeteryRecord(m_SQL, cemeteryRecordId);
                cemeteryRecord.ShowDialog();
            }
            else
            if (iVitalRecordID > 900000)
            {
                int personId = iVitalRecordID - 900000;
                DataRow personRow = SQL.GetPerson(personId);
                if (personRow != null)
                {
                    //LastName_textBox.Text = (String.IsNullOrEmpty(personRow[U.LastName_col].ToString())) ? 
                    //                         personRow[U.MarriedName_col].ToString() :
                    //                         personRow[U.LastName_col].ToString();
                    //FirstName_textBox.Text = personRow[U.FirstName_col].ToString();
                    FPerson personRecord = new FPerson(m_SQL, personId, false);
                    personRecord.ShowDialog();
                }
            }
            else
            if (iVitalRecordID != 0)
            {
                DataRow vitalRecordRow = SQL.GetVitalRecord(iVitalRecordID);
                if (vitalRecordRow != null)
                {
                    //LastName_textBox.Text = vitalRecordRow[U.LastName_col].ToString();
                    //FirstName_textBox.Text = vitalRecordRow[U.FirstName_col].ToString();
                    FVitalRecord VitalRecord = new FVitalRecord(m_SQL, iVitalRecordID);
                    VitalRecord.ShowDialog();
                }
            }
        }
    }
}
