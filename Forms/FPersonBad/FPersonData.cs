using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public partial class FPerson
    {
        public class PersonData
        {
            private FPerson person;
            private DataSet person_ds;
            public PersonData(FPerson person)
            {
                this.person = person;
                person_ds = SQL.DefinePerson();
            }
            //****************************************************************************************************************************
            public void ClearPersonInfo()
            {
                person_ds.Clear();
            }
            //****************************************************************************************************************************
            public bool NoPersonExists()
            {
                return PersonTable().Rows.Count == 0;
            }
            //****************************************************************************************************************************
            public DataSet PersonDataSet()
            {
                return person_ds;
            }
            //****************************************************************************************************************************
            public DataTable PersonTable()
            {
                return person_ds.Tables[U.Person_Table];
            }
            //****************************************************************************************************************************
            public DataTable CemeteryRecordsTable()
            {
                return person_ds.Tables[U.CemeteryRecord_Table];
            }
            //****************************************************************************************************************************
            public DataTable VitalRecordsTable()
            {
                return person_ds.Tables[U.VitalRecord_Table];
            }
            //****************************************************************************************************************************
            public DataTable MarriageTable()
            {
                return person_ds.Tables[U.Marriage_Table];
            }
            //****************************************************************************************************************************
            public int NumberSpouses()
            {
                return MarriageTable().Rows.Count;
            }
            //****************************************************************************************************************************
            public DataTable CategoryValueTable()
            {
                return person_ds.Tables[U.PersonCategoryValue_Table];
            }
            //****************************************************************************************************************************
            public DataTable BuildingOccupantTable()
            {
                return person_ds.Tables[U.BuildingOccupant_Table];
            }
            //****************************************************************************************************************************
            public DataRow PersonRow()
            {
                DataTable person_tbl = PersonTable();
                if (person_tbl.Rows.Count == 0)
                    return null;
                else
                    return person_tbl.Rows[0];
            }
            //****************************************************************************************************************************
            public void AddPersonToDataTable()
            {
                DataTable tbl = PersonTable();
                DataRow Person_row = tbl.NewRow();
                Person_row[U.PersonID_col] = 0;
                Person_row[U.FirstName_col] = person.FirstName_textBox.Text.SetNameForDatabase();
                Person_row[U.MiddleName_col] = person.MiddleName_textBox.Text.SetNameForDatabase();
                Person_row[U.LastName_col] = person.LastName_textBox.Text.SetNameForDatabase();
                Person_row[U.Suffix_col] = person.Suffix_comboBox.Text.SetSuffixForDatabase();
                Person_row[U.Prefix_col] = person.Prefix_comboBox.Text.SetPrefixForDatabase();
                Person_row[U.MarriedName_col] = person.MarriedName_textBox.Text.SetNameForDatabase();
                Person_row[U.MarriedName2_col] = "";
                Person_row[U.MarriedName3_col] = "";
                Person_row[U.KnownAs_col] = person.KnownAs_textBox.Text.SetNameForDatabase();
                Person_row[U.FatherID_col] = person.m_iFatherID;
                Person_row[U.MotherID_col] = person.m_iMotherID;
                Person_row[U.Notes_col] = person.Description_TextBox.Text.TrimString();
                Person_row[U.GazetteerRoad_col] = person.GazetteerRoad_textBox.Text.ToInt();
                string beers1869District = person.Beers1869District_textBox.Text.ToString().ToUpper();
                string mcClellan1856District = person.McClellan1856District_textBox.Text.ToString();
                if (String.IsNullOrEmpty(beers1869District))
                {
                    Person_row[U.Beers1869District_col] = 0;
                }
                else
                switch (beers1869District[0])
                {
                    case 'V': Person_row[U.Beers1869District_col] = 99; break;
                    case 'R': Person_row[U.Beers1869District_col] = 89; break;
                    case 'W': Person_row[U.Beers1869District_col] = 79; break;
                    default: Person_row[U.Beers1869District_col] = beers1869District.ToInt(); break;
                }
                Person_row[U.McClellan1856District_col] = (mcClellan1856District.Contains("V")) ? 99 : beers1869District.ToInt();
                Person_row[U.Source_col] = person.Source_textBox.Text.TrimString();
                Person_row[U.Sex_col] = person.SetPersonSexChar();
                Person_row[U.BornDate_col] = person.BornDate_textBox.Text.TrimString();
                Person_row[U.BornPlace_col] = person.BornPlace_textBox.Text.TrimString();
                Person_row[U.BornHome_col] = person.BornHome_textBox.Text.TrimString();
                Person_row[U.BornVerified_col] = U.IsChecked(person.BornVerified_checkBox.Checked);
                Person_row[U.BornSource_col] = person.BornSource_textBox.Text.TrimString();
                Person_row[U.BornBook_col] = person.BornBook_textBox.Text.TrimString();
                Person_row[U.BornPage_col] = person.BornPage_textBox.Text.TrimString();
                Person_row[U.DiedDate_col] = person.DiedDate_textBox.Text.TrimString();
                Person_row[U.DiedPlace_col] = person.DiedPlace_textBox.Text.TrimString();
                Person_row[U.DiedHome_col] = person.DiedHome_textBox.Text.TrimString();
                Person_row[U.DiedVerified_col] = U.IsChecked(person.DiedVerified_checkBox.Checked);
                Person_row[U.DiedSource_col] = person.DiedSource_textBox.Text.TrimString();
                Person_row[U.DiedBook_col] = person.DiedBook_textBox.Text.TrimString();
                Person_row[U.DiedPage_col] = person.DiedPage_textBox.Text.TrimString();
                Person_row[U.BuriedDate_col] = person.BuriedDate_textBox.Text.TrimString();
                Person_row[U.BuriedPlace_col] = person.BuriedPlace_textBox.Text.TrimString();
                Person_row[U.BuriedStone_col] = person.BuriedStone_textBox.Text.TrimString();
                Person_row[U.BuriedVerified_col] = U.IsChecked(person.BuriedVerified_checkBox.Checked);
                Person_row[U.BuriedSource_col] = person.BuriedSource_textBox.Text.TrimString();
                Person_row[U.BuriedBook_col] = person.BuriedBook_textBox.Text.TrimString();
                Person_row[U.BuriedPage_col] = person.BuriedPage_textBox.Text.TrimString();
                Person_row[U.ImportPersonID_col] = 0;
                Person_row[U.Census1790_col] = person.Census1790_textBox.Text.ToInt();
                Person_row[U.Census1800_col] = person.Census1800_textBox.Text.ToInt();
                Person_row[U.Census1810_col] = person.Census1810_textBox.Text.ToInt();
                Person_row[U.Census1820_col] = person.Census1820_textBox.Text.ToInt();
                Person_row[U.Census1830_col] = person.Census1830_textBox.Text.ToInt();
                Person_row[U.Census1840_col] = person.Census1840_textBox.Text.ToInt();
                Person_row[U.Census1850_col] = person.Census1850_textBox.Text.ToInt();
                Person_row[U.Census1860_col] = person.Census1860_textBox.Text.ToInt();
                Person_row[U.Census1870_col] = person.Census1870_textBox.Text.ToInt();
                Person_row[U.Census1880_col] = person.Census1880_textBox.Text.ToInt();
                Person_row[U.Census1890_col] = person.Census1890_textBox.Text.ToInt();
                Person_row[U.Census1900_col] = person.Census1900_textBox.Text.ToInt();
                Person_row[U.Census1910_col] = person.Census1910_textBox.Text.ToInt();
                Person_row[U.Census1920_col] = person.Census1920_textBox.Text.ToInt();
                Person_row[U.Census1930_col] = person.Census1930_textBox.Text.ToInt();
                Person_row[U.Census1940_col] = person.Census1940_textBox.Text.ToInt();
                Person_row[U.Census1950_col] = person.Census1950_textBox.Text.ToInt();
                Person_row[U.ExcludeFromSite_col] = person.ExcludeFromSite_checkBox.Checked.ToInt();
                tbl.Rows.Add(Person_row);
            }
            //****************************************************************************************************************************
            public void GetPerson(int iPersonID)
            {
                SQL.GetPerson(ref person_ds, iPersonID);
            }
            //****************************************************************************************************************************
            public void UpdatePersonInDataTable(ArrayList FieldsModified)
            {
                DataRow row = PersonRow();
                DataColumnCollection columns = PersonTable().Columns;
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.FirstName_col, person.FirstName_textBox.Text.SetNameForDatabase());
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.MiddleName_col, person.MiddleName_textBox.Text.SetNameForDatabase());
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.LastName_col, person.LastName_textBox.Text.SetNameForDatabase());
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Suffix_col, person.Suffix_comboBox.Text.SetNameForDatabase());
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Prefix_col, person.Prefix_comboBox.Text.SetNameForDatabase());
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.MarriedName_col, person.MarriedName_textBox.Text.SetNameForDatabase());
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.KnownAs_col, person.KnownAs_textBox.Text.SetNameForDatabase());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.FatherID_col, person.m_iFatherID);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.MotherID_col, person.m_iMotherID);
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Notes_col, person.Description_TextBox.Text.Trim());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.GazetteerRoad_col, person.GazetteerRoad_textBox.Text.ToInt());
                string beers1869District = person.Beers1869District_textBox.Text.ToString().ToUpper();
                string mcClellan1856District = person.McClellan1856District_textBox.Text.ToString();
                int iBeers1869District;
                if (String.IsNullOrEmpty(beers1869District))
                {
                    iBeers1869District = 0;
                }
                else
                {
                    switch (beers1869District[0])
                    {
                        case 'V': iBeers1869District = 99; break;
                        case 'R': iBeers1869District = 89; break;
                        case 'W': iBeers1869District = 79; break;
                        default: iBeers1869District = beers1869District.ToInt(); break;
                    }
                }
                int iMcClellan1856District = (mcClellan1856District.Contains("V")) ? 99 : mcClellan1856District.ToInt();
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Beers1869District_col, iBeers1869District);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.McClellan1856District_col, iMcClellan1856District);
                U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Source_col, person.Source_textBox.Text.Trim());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Sex_col, person.SetPersonSexChar());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1790_col, person.Census1790_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1800_col, person.Census1800_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1810_col, person.Census1810_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1820_col, person.Census1820_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1830_col, person.Census1830_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1840_col, person.Census1840_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1850_col, person.Census1850_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1860_col, person.Census1860_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1870_col, person.Census1870_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1880_col, person.Census1880_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1890_col, person.Census1890_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1900_col, person.Census1900_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1910_col, person.Census1910_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1920_col, person.Census1920_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1930_col, person.Census1930_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1940_col, person.Census1940_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Census1950_col, person.Census1950_textBox.Text.ToInt());
                U.SetToNewValueIfDifferent(FieldsModified, row, U.ExcludeFromSite_col, person.ExcludeFromSite_checkBox.Checked);
                if (!person.m_bVitalBornFound || row[U.BornSource_col].ToString() == "School Records")
                {
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornDate_col, person.BornDate_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornPlace_col, person.BornPlace_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornHome_col, person.BornHome_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornVerified_col, U.IsChecked(person.BornVerified_checkBox.Checked));
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornSource_col, person.BornSource_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornBook_col, person.BornBook_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornPage_col, person.BornPage_textBox.Text.Trim());
                }
                if (!person.m_bVitalDiedFound)
                {
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedDate_col, person.DiedDate_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedPlace_col, person.DiedPlace_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedHome_col, person.DiedHome_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedVerified_col, U.IsChecked(person.DiedVerified_checkBox.Checked));
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedSource_col, person.DiedSource_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedBook_col, person.DiedBook_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedPage_col, person.DiedPage_textBox.Text.Trim());
                }
                if (!person.m_bVitalBuriedFound)
                {
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BuriedDate_col, person.BuriedDate_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BuriedPlace_col, person.BuriedPlace_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BuriedStone_col, person.BuriedStone_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BuriedVerified_col, U.IsChecked(person.BuriedVerified_checkBox.Checked));
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BuriedSource_col, person.BuriedSource_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BuriedBook_col, person.BuriedBook_textBox.Text.Trim());
                    U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BuriedPage_col, person.BuriedPage_textBox.Text.Trim());
                }
            }
            //****************************************************************************************************************************
            public void UpdatePersonField(string field_col,
                                          int fieldValue)
            {
                DataRow personRow = PersonTable().Rows[0];
                personRow[field_col] = fieldValue;
                SQL.UpdatePersonTableForField(PersonTable(),field_col);
            }
            //****************************************************************************************************************************
            public int SavePerson(int iPersonID,
                                  int iSpouseID)
            {
                try
                {
                    if (iPersonID == 0)
                    {
                        iPersonID = AddNewPersonToDatabase();
                    }
                    else
                    {
                        UpdatePersonInDatabase(iPersonID, iSpouseID);
                    }
                }
                catch (Exception e)
                {
                    string message = "Person Save Error" + UU.LF + e.Message;
                    HistoricJamaicaException ex = new HistoricJamaicaException(message);
                    UU.ShowErrorMessage(ex);
                    throw ex;
                }
                return iPersonID;
            }
            //****************************************************************************************************************************
            private void AddDefaultValuesToBuildingTables()
            {
                foreach (DataRow row in person_ds.Tables[U.BuildingOccupant_Table].Rows)
                {
                    row[U.SpouseLivedWithID_col] = 0;
                    row[U.BuildingValueOrder_col] = 0;
                    row[U.Notes_col] = "";
                    row[U.CensusYears_col] = 0;
                }
            }
            //****************************************************************************************************************************
            private int AddNewPersonToDatabase()
            {
                AddPersonToDataTable();
                AddDefaultValuesToBuildingTables();
                try
                {
                    return SQL.CreatePersonData(person_ds);
                }
                catch (HistoricJamaicaException ex)
                {
                    MessageBox.Show("Unable to create Person Record");
                    throw ex;
                }
            }
            //****************************************************************************************************************************
            private bool TableHasDeletedValues(DataTable tbl)
            {
                DataViewRowState dvrs = DataViewRowState.Added | DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent;
                DataRow[] rows = tbl.Select("", "", dvrs);
                if (rows.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //****************************************************************************************************************************
            private int UpdatePersonInDatabase(int iPersonID,
                                               int iSpouseID)
            {
                ArrayList FieldsModified = new ArrayList();
                UpdatePersonInDataTable(FieldsModified);
                try
                {
                    SQL.UpdatePersonData(iPersonID, iSpouseID, person_ds, FieldsModified);
                }
                catch (HistoricJamaicaException ex)
                {
                    MessageBox.Show("Unable to modify Person Record");
                    throw ex;
                }
                return iPersonID;
            }
            //****************************************************************************************************************************
        }
    }
}
