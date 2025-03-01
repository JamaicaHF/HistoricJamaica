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
    public class CIntegratePersonCW : CIntegration
    {
        //private bool getFromGrid;
        public CIntegratePersonCW(CSql SQL, bool getFromGrid)
            : base(SQL, getFromGrid)
        {
            this.getFromGrid = getFromGrid;
        }
        //****************************************************************************************************************************
        public bool IntegrateRecord(DataRow personCW_row)
        {
            bool success = false;
            try
            {
                Person_tbl = SQL.DefinePersonTable();
                bool bPersonIntegrated = (personCW_row[U.PersonID_col].ToInt() != 0);
                if (IntegratePerson(personCW_row, bPersonIntegrated))
                {
                    return SQL.IntegratePersonCWRecord(personCW_row, Person_tbl, getFromGrid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Encounter during Integration: " + ex.Message);
            }
            return success;
        }
        //****************************************************************************************************************************
        public bool IntegratePerson(DataRow personCW_row,
                                    bool    bPersonIntegrated)
        {
            PersonName personName = new PersonName(personCW_row[U.FirstName_col].ToString(), personCW_row[U.MiddleName_col].ToString(), personCW_row[U.LastName_col].ToString(), "", "");
            eSex Sex = SQL.GetSex(personName.firstName);
            int recordBornYear = U.GetYearFromDate(personCW_row[U.BornDate_col].ToString());
            CIntegrationInfo integrationInfo = new CIntegrationInfo(personName, recordBornYear, Sex, bPersonIntegrated, personCW_row[U.BornDate_col].ToString());
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
