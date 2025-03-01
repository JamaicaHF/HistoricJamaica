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
    public class CensusCheckBox : Census
    {
        protected CheckedListBox census_checkedListBox;
        protected CensusYears originalCensusYears = 0;
        //****************************************************************************************************************************
        public CensusCheckBox(CheckedListBox census_checkedListBox): base()
        {
            this.census_checkedListBox = census_checkedListBox;
        }
        //****************************************************************************************************************************
        public override void Clear()
        {
            census_checkedListBox.Items.Clear();
            LoadCensusData(0);
            originalCensusYears = 0;
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
            AddThisYearToCensusCheckboxIfInCensusYears(1856);
            AddThisYearToCensusCheckboxIfInCensusYears(1869);
            AddThisYearToCensusCheckboxIfInCensusYears(1884);
            originalCensusYears = censusYears;
        }
        //****************************************************************************************************************************
        public void AddPersonToCensusCheckBox(DataRow personRow)
        {
            censusYears = 0;
            for (int i = 1790; i <= 1940; i += 10)
            {
                if (personRow[U.CensusYearCol(i)].ToInt() != 0)
                {
                    AddThisYearToCensusYears(i);
                }
            }
            LoadCensusData((Int64)censusYears);
        }
        //****************************************************************************************************************************
        public void AddThisYearToCensusYears(int year)
        {
            if (!CensusContainsYear(censusYears, year))
            {
                AddYearToCensusYears(ref censusYears, year);
                LoadCensusData((Int64) censusYears);
            }
        }
        //****************************************************************************************************************************
        public void AddThisYearToCensusCheckboxIfInCensusYears(int year)
        {
            if (CensusContainsYear(censusYears, year))
            {
                census_checkedListBox.Items.Add(year.ToString(), CheckState.Checked);
            }
            else
            {
                census_checkedListBox.Items.Add(year.ToString(), CheckState.Unchecked);
            }
        }
        //****************************************************************************************************************************
        public Int64 GetCensusYearsFromCheckBox()
        {
            censusYears = (int)0;
            for (int i = 0; i < census_checkedListBox.CheckedItems.Count; i++)
            {
                int yearChecked = census_checkedListBox.CheckedItems[i].ToInt();
                AddYearToCensusYears(ref censusYears, yearChecked);
            }
            return (Int64)censusYears;
        }
        //****************************************************************************************************************************
    }
}
