using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace SQL_Library
{
    public class CensusPerson : CensusCheckBox
    {
        //****************************************************************************************************************************
        public CensusPerson(CheckedListBox census_checkedListBox) : base(census_checkedListBox)
        {
            LoadCensusData(0);
        }
        //****************************************************************************************************************************
        public override void LoadCensusData(Int64 yearsInCensus)
        {
            censusYears = (CensusYears)yearsInCensus;
            census_checkedListBox.Items.Clear();
            for (int year = 1790; year <= 1940; year += 10)
            {
                AddThisYearToCensusCheckboxIfInCensusYears(year);
            }
            AddThisYearToCensusCheckboxIfInCensusYears(1935);
            originalCensusYears = censusYears;
        }
        //****************************************************************************************************************************
        public int yearChecked()
        {
            Int64 x = GetCensusYearsFromCheckBox();
            return 0;
        }
        //****************************************************************************************************************************
        public bool CensusYearsChanged()
        {
            Int64 curCensusYears = GetCensusYearsFromCheckBox();
            Int64 origCensusYrars = (Int64)originalCensusYears;
            return (curCensusYears != origCensusYrars);
        }
        //****************************************************************************************************************************
    }
}
