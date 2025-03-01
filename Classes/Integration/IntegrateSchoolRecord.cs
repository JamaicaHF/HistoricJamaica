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
    public class CIntegrateSchoolRecord : CIntegration
    {
        //private bool getFromGrid;
        public CIntegrateSchoolRecord(CSql SQL, bool getFromGrid)
            : base(SQL, getFromGrid)
        {
            this.getFromGrid = getFromGrid;
        }
        //****************************************************************************************************************************
        public bool IntegrateRecord(DataRow SchoolRecord_row, 
                                    DataTable AlternativeSpellingsFirstNameTbl,
                                    DataTable AlternativeSpellingsLastNameTbl)
        {
            bool success = false;
            try
            {
                Person_tbl = SQL.DefinePersonTable();
                bool bPersonIntegrated = (SchoolRecord_row[U.PersonID_col].ToInt() != 0);
                if (IntegratePerson(SchoolRecord_row, bPersonIntegrated))
                {
                    if (!bPersonIntegrated && SQL.SchoolRecordAlreadyExists(Person_tbl.Rows[0][U.PersonID_col].ToInt(), SchoolRecord_row[U.SchoolID_col].ToInt(), SchoolRecord_row[U.Year_col].ToInt()))
                    {
                        string message = @"Another Student with same name in another grade: " + Environment.NewLine + SchoolRecord_row[U.Person_col].ToString() + ". Continue?";
                        if (MessageBox.Show(message, "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    return SQL.IntegrateSchoolRecord(AlternativeSpellingsFirstNameTbl, AlternativeSpellingsLastNameTbl, SchoolRecord_row, Person_tbl, getFromGrid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Encounter during Integration: " + ex.Message);
            }
            return success;
        }
        //****************************************************************************************************************************
        public bool IntegratePerson(DataRow SchoolRecord_row,
                                    bool    bPersonIntegrated)
        {
            PersonName personName = new PersonName(SchoolRecord_row[U.Person_col].ToString());
            eSex Sex = SQL.GetSex(personName.firstName);
            int recordBornYear = U.GetBornYearFromSchoolRecord(SchoolRecord_row);
            CIntegrationInfo integrationInfo = new CIntegrationInfo(personName, recordBornYear, Sex, bPersonIntegrated, SchoolRecord_row[U.BornDate_col].ToString());
            int iPersonID = SimilarPersonExists(integrationInfo);
            if (Person_tbl.Rows.Count == 0)
            {
                return false;
            }
            if (!ValidData(iPersonID, integrationInfo.iSpouseId, "Person", "Spouse"))
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
    }
}
