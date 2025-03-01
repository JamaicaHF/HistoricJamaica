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
    class CCheckedListboxCensus : FCheckedListboxGroup
    {
        private CensusCheckBox census;
        //****************************************************************************************************************************
        public CCheckedListboxCensus(CSql sql,
                                     DataTable Building_tbl, 
                                     string sBuildingIDTableName,
                                     int personID) : base(sql, Building_tbl, sBuildingIDTableName)
        {
            this.Text = "Building Names";
            this.ContextMenu.MenuItems.Add("Add New Building Name", new EventHandler(AddGroupValue_Click));
            census = new CensusCheckBox(GroupValues_checkedListBox);
            DataRow buildingRow = m_Group_tbl.Rows[0];
            Int64 censusYears = buildingRow[U.CensusYears_col].ToInt64();
            if (censusYears != 0)
            {
                census.LoadCensusData(censusYears);
            }
            else
            {
                DataRow personRow = SQL.GetPerson(personID);
                census.AddPersonToCensusCheckBox(personRow);
            }
        }
        //****************************************************************************************************************************
        public void UpdateBuildingOccupantCensusYears(int iPersonID, 
                                                      int iBuildingID)
        {
            SQL.UpdateBuildingOccupant(iPersonID, iBuildingID, U.CensusYears_col, census.GetCensusYearsFromCheckBox());
        }
        //****************************************************************************************************************************
    }
}
