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
    public partial class FCemeteryRecord : Form
    {
        private eSearchOption SearchBy = eSearchOption.SO_None;
        private int m_iCemeteryRecordID = 0;
        private int m_iPersonID = 0;
        private CSql m_SQL;
        private DataTable m_CemeteryRecord_tbl;
        private string m_FirstName = "";
        private string m_MiddleName = "";
        private string m_LastName = "";
        private string m_Suffix = "";
        private string m_Prefix = "";
        private string m_SpouseFirstName = "";
        private string m_SpouseMiddleName = "";
        private string m_SpouseLastName = "";
        private string m_SpouseSuffix = "";
        private string m_SpousePrefix = "";
        private string m_FatherFirstName = "";
        private string m_FatherMiddleName = "";
        private string m_FatherLastName = "";
        private string m_FatherSuffix = "";
        private string m_FatherPrefix = "";
        private string m_MotherFirstName = "";
        private string m_MotherMiddleName = "";
        private string m_MotherLastName = "";
        private string m_MotherSuffix = "";
        private string m_MotherPrefix = "";
        private string m_sFamilyName = "";
        private bool m_bDidSearch = false;
        private bool bDispositionChanged = false;
        private bool bSexChanged = false;
        private bool ExcludeFromSiteCheckedChanged = false;
        private bool m_PersonIntegrated = false;
        private bool m_SpouseIntegrated = false;
        private bool m_FatherIntegrated = false;
        private bool m_MotherIntegrated = false;
        private bool m_NewMode = false;
        //****************************************************************************************************************************
        public FCemeteryRecord(CSql SQL)
        {
            m_SQL = SQL;
            InitializeCemeteryRecord();
            InitializeFieldLengths();
            InitializeFields();
            SearchAllNames_radioButton.Checked = true;
            m_NewMode = true;
            ReturnToListbutton.Text = "New Person";
        }
        //****************************************************************************************************************************
        public FCemeteryRecord(int               iPersonID,
                              CSql              sql)
        {
            m_SQL = sql;
            InitializeCemeteryRecord();
            DataTable tbl = SQL.DefineCemeteryRecordTable();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.CemeteryID_col] };
            SQL.GetAllRecordsForPerson(tbl, iPersonID);
            GetCemeteryRecordFromGrid(tbl, "");
        }
        //****************************************************************************************************************************
        public FCemeteryRecord(CSql SQL, int iSelectedCemeteryRecordID)
        {
            m_iCemeteryRecordID = iSelectedCemeteryRecordID;
            m_SQL = SQL;
            InitializeCemeteryRecord();
            DisplayCemeteryRecord(m_iCemeteryRecordID);
        }
        //****************************************************************************************************************************
        private void InitializeCemeteryRecord()
        {
            InitializeComponent();
            Cemetery_comboBox.DataSource = U.CemeteryList();
            Cemetery_comboBox.DisplayMember = "Cemetery";
            UU.LoadSuffixComboBox(Suffix_comboBox);
            UU.LoadPrefixComboBox(Prefix_comboBox);
            UU.LoadSuffixComboBox(FatherSuffix_comboBox);
            UU.LoadPrefixComboBox(FatherPrefix_comboBox);
            UU.LoadSuffixComboBox(MotherSuffix_comboBox);
            UU.LoadPrefixComboBox(MotherPrefix_comboBox);
            UU.LoadSuffixComboBox(SpouseSuffix_comboBox);
            UU.LoadPrefixComboBox(SpousePrefix_comboBox);
            m_CemeteryRecord_tbl = SQL.DefineCemeteryRecordTable();
        }
        //****************************************************************************************************************************
        private void DisplayCemeteryRecord(int iCemeteryRecordID)
        {
            m_bDidSearch = false;
            SearchBy = eSearchOption.SO_None;
            SQL.GetCemeteryRecord(m_CemeteryRecord_tbl, iCemeteryRecordID);
            if (m_CemeteryRecord_tbl.Rows.Count > 0)
            {
                DisplayCemeteryRecord(m_CemeteryRecord_tbl.Rows[0]);
                SetIntegratedChecked(m_CemeteryRecord_tbl);
                SetToUnmodified();
                SetAllNameValues();
            }
        }
        //****************************************************************************************************************************
        public int SpouseID()
        {
            return m_CemeteryRecord_tbl.Rows[0][U.SpouseID_col].ToInt();
        }
        //****************************************************************************************************************************
        private void LoadCemeteryComboBox()
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Cemetery", typeof(string)));
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows.Add(list.NewRow());
            list.Rows[0][0] = "";
            list.Rows[1][0] = "South Hill";
            list.Rows[2][0] = "Sage Hill";
            list.Rows[3][0] = "7th Day Advent";
            list.Rows[4][0] = "Robbins";
            list.Rows[5][0] = "Village";
            list.Rows[6][0] = "Rawsonville";
            list.Rows[7][0] = "South Windham";
            list.Rows[8][0] = "Pikes Falls";
            list.Rows[9][0] = "West Jamaica";
            list.Rows[10][0] = "East Jamaica";
            Cemetery_comboBox.DataSource = list;
            Cemetery_comboBox.DisplayMember = "Cemetery";
        }
        //****************************************************************************************************************************
        private void radioButton_CheckedChanged(Object sender,
                                                EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                if (((RadioButton)sender).Name == Male_radioButton.Name)
                {
                    Female_radioButton.Checked = false;
                    Unknown_radioButton.Checked = false;
                    bSexChanged = true;
                }
                if (((RadioButton)sender).Name == Female_radioButton.Name)
                {
                    Unknown_radioButton.Checked = false;
                    Male_radioButton.Checked = false;
                    bSexChanged = true;
                }
                if (((RadioButton)sender).Name == Unknown_radioButton.Name)
                {
                    Female_radioButton.Checked = false;
                    Male_radioButton.Checked = false;
                    bSexChanged = true;
                }
                if (((RadioButton)sender).Name == Buried_radioButton.Name ||
                    ((RadioButton)sender).Name == Cremated_radioButton.Name ||
                    ((RadioButton)sender).Name == Other_radioButton.Name)
                {
                    bDispositionChanged = true;
                }
            }
        }
        //****************************************************************************************************************************
        private void InitializeFieldLengths()
        {
            Epitaph_TextBox.MaxLength = U.iMaxValueLength;
            PersonNameOnGrave_textBox.MaxLength = U.iMaxValueLength;
            SpouseNameOnGrave_textBox.MaxLength = U.iMaxValueLength;
            FatherNameOnGrave_textBox.MaxLength = U.iMaxValueLength;
            MotherNameOnGrave_textBox.MaxLength = U.iMaxValueLength;
            FirstName_textBox.MaxLength = U.iMaxNameLength;
            MiddleName_textBox.MaxLength = U.iMaxNameLength;
            LastName_textBox.MaxLength = U.iMaxNameLength;
            Prefix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            Suffix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            FatherFirstName_textBox.MaxLength = U.iMaxNameLength;
            FatherMiddleName_textBox.MaxLength = U.iMaxNameLength;
            FatherLastName_textBox.MaxLength = U.iMaxNameLength;
            FatherPrefix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            FatherSuffix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            MotherFirstName_textBox.MaxLength = U.iMaxNameLength;
            MotherMiddleName_textBox.MaxLength = U.iMaxNameLength;
            MotherLastName_textBox.MaxLength = U.iMaxNameLength;
            MotherPrefix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            MotherSuffix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            SpouseFirstName_textBox.MaxLength = U.iMaxNameLength;
            SpouseMiddleName_textBox.MaxLength = U.iMaxNameLength;
            SpouseLastName_textBox.MaxLength = U.iMaxNameLength;
            SpousePrefix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            SpouseSuffix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            DiedDateMonth_textBox.MaxLength = 2;
            DiedDateDay_textBox.MaxLength = 2;
            DiedDateYear_textBox.MaxLength = 4;
            BornDateMonth_textBox.MaxLength = 2;
            BornDateDay_textBox.MaxLength = 2;
            BornDateYear_textBox.MaxLength = 4;
            AgeDays_textBox.MaxLength = 2;
            AgeMonths_textBox.MaxLength = 2;
            AgeYears_textBox.MaxLength = 3;
            Cemetery_comboBox.MaxLength = U.iMaxValueLength;
            LotNumber_textBox.MaxLength = U.iMaxNameLength;
            Notes_TextBox.MaxLength = U.iMaxDescriptionLength;
        }
        //****************************************************************************************************************************
        private void InitializeFields()
        {
            Epitaph_TextBox.Text = "";
            PersonNameOnGrave_textBox.Text = "";
            SpouseNameOnGrave_textBox.Text = "";
            FatherNameOnGrave_textBox.Text = "";
            MotherNameOnGrave_textBox.Text = "";
            FirstName_textBox.Text = "";
            MiddleName_textBox.Text = "";
            Suffix_comboBox.Text = "";
            Prefix_comboBox.Text = "";
            SpouseLastName_textBox.Text = "";
            SpouseFirstName_textBox.Text = "";
            SpouseMiddleName_textBox.Text = "";
            SpouseSuffix_comboBox.Text = "";
            SpousePrefix_comboBox.Text = "";
            FatherLastName_textBox.Text = "";
            FatherFirstName_textBox.Text = "";
            FatherMiddleName_textBox.Text = "";
            FatherSuffix_comboBox.Text = "";
            FatherPrefix_comboBox.Text = "";
            MotherLastName_textBox.Text = "";
            MotherFirstName_textBox.Text = "";
            MotherMiddleName_textBox.Text = "";
            MotherSuffix_comboBox.Text = "";
            MotherPrefix_comboBox.Text = "";
            Notes_TextBox.Text = "";
            Buried_radioButton.Checked = true;
            Buried_radioButton.Checked = true;
            Cremated_radioButton.Checked = false;
            Other_radioButton.Checked = false;
            Cemetery_comboBox.Text = "";
            LotNumber_textBox.Text = "";
            DiedDateYear_textBox.Text = "";
            DiedDateMonth_textBox.Text = "";
            DiedDateDay_textBox.Text = "";
            Male_radioButton.Checked = false;
            Female_radioButton.Checked = false;
            AgeYears_textBox.Text = "";
            AgeMonths_textBox.Text = "";
            AgeDays_textBox.Text = "";
            NameIntegrated_checkBox.Checked = false;
            FatherIntegrated_checkBox.Checked = false;
            MotherIntegrated_checkBox.Checked = false;
            SpouseIntegrated_checkBox.Checked = false;
        }
        //****************************************************************************************************************************
        private void SetDispositionRadioButtons(char cDisposition)
        {
            switch (cDisposition)
            {
                case 'B': Buried_radioButton.Checked = true; break;
                case 'C': Cremated_radioButton.Checked = true; break;
                case 'O': Other_radioButton.Checked = true; break;
                default: break;
            }
        }
        //****************************************************************************************************************************
        private void DisplayCemeteryRecord(DataRow row)
        {
            InitializeFields();
            PersonNameOnGrave_textBox.Text = row[U.NameOnGrave_col].ToString();
            SpouseNameOnGrave_textBox.Text = row[U.SpouseNameOnGrave_col].ToString();
            FatherNameOnGrave_textBox.Text = row[U.FatherNameOnGrave_col].ToString();
            MotherNameOnGrave_textBox.Text = row[U.MotherNameOnGrave_col].ToString();
            LastName_textBox.Text = row[U.LastName_col].ToString();
            LastName_textBox.Text = row[U.LastName_col].ToString();
            FirstName_textBox.Text = row[U.FirstName_col].ToString();
            MiddleName_textBox.Text = row[U.MiddleName_col].ToString();
            Suffix_comboBox.Text = row[U.Suffix_col].ToString();
            Prefix_comboBox.Text = row[U.Prefix_col].ToString();
            SpouseLastName_textBox.Text = row[U.SpouseLastName_col].ToString();
            SpouseFirstName_textBox.Text = row[U.SpouseFirstName_col].ToString();
            SpouseMiddleName_textBox.Text = row[U.SpouseMiddleName_col].ToString();
            SpouseSuffix_comboBox.Text = row[U.SpouseSuffix_col].ToString();
            SpousePrefix_comboBox.Text = row[U.SpousePrefix_col].ToString();
            FatherLastName_textBox.Text = row[U.FatherLastName_col].ToString();
            FatherFirstName_textBox.Text = row[U.FatherFirstName_col].ToString();
            FatherMiddleName_textBox.Text = row[U.FatherMiddleName_col].ToString();
            FatherSuffix_comboBox.Text = row[U.FatherSuffix_col].ToString();
            FatherPrefix_comboBox.Text = row[U.FatherPrefix_col].ToString();
            MotherLastName_textBox.Text = row[U.MotherLastName_col].ToString();
            MotherFirstName_textBox.Text = row[U.MotherFirstName_col].ToString();
            MotherMiddleName_textBox.Text = row[U.MotherMiddleName_col].ToString();
            MotherSuffix_comboBox.Text = row[U.MotherSuffix_col].ToString();
            MotherPrefix_comboBox.Text = row[U.MotherPrefix_col].ToString();
            SetDate(row[U.BornDate_col].ToString(), BornDateYear_textBox, BornDateMonth_textBox, BornDateDay_textBox);
            int iyear, imonth, iday;
            U.SplitDate(row[U.DiedDate_col].ToString(), out iyear, out imonth, out iday);
            string CalcDate = U.BornDateFromDiedDateMinusAge(iyear, imonth, iday, row[U.AgeYears_col].ToInt(), row[U.AgeMonths_col].ToInt(), row[U.AgeDays_col].ToInt(), "");
            SetDate(CalcDate, CalcBornDateYear_textBox, CalcBornDateMonth_textBox, CalcBornDateDay_textBox);
            SetDate(row[U.DiedDate_col].ToString(), DiedDateYear_textBox, DiedDateMonth_textBox, DiedDateDay_textBox);
            AgeYears_textBox.Text = row[U.AgeYears_col].ToString();
            AgeMonths_textBox.Text = row[U.AgeMonths_col].ToString();
            AgeDays_textBox.Text = row[U.AgeDays_col].ToString();
            string sSex = row[U.Sex_col].ToString();
            SetSexRadioButtons(sSex[0]);
            bSexChanged = false;
            Buried_radioButton.Checked = false;
            Cremated_radioButton.Checked = false;
            Other_radioButton.Checked = false;
            SetDispositionRadioButtons(row[U.Disposition_col].ToString()[0]);
            Cemetery cemetery = (Cemetery)row[U.CemeteryID_col].ToInt();
            Cemetery_comboBox.Text = U.CemeteryName(cemetery);
            LotNumber_textBox.Text = row[U.LotNumber_col].ToString();
            Epitaph_TextBox.Text = row[U.Epitaph_col].ToString();
            Notes_TextBox.Text = row[U.Notes_col].ToString();
            m_iPersonID = row[U.PersonID_col].ToInt();
            m_PersonIntegrated = row[U.PersonID_col].ToInt() != 0;
            m_SpouseIntegrated = row[U.SpouseID_col].ToInt() != 0;
            m_FatherIntegrated = row[U.FatherID_col].ToInt() != 0;
            m_MotherIntegrated = row[U.MotherID_col].ToInt() != 0;
            NameIntegrated_checkBox.Checked = m_PersonIntegrated;
            SpouseIntegrated_checkBox.Checked = m_SpouseIntegrated;
            FatherIntegrated_checkBox.Checked = m_FatherIntegrated;
            MotherIntegrated_checkBox.Checked = m_MotherIntegrated;
            m_bDidSearch = false;

        }
        //****************************************************************************************************************************
        private void SetDate(string dateStr,
                             TextBox year,
                             TextBox month,
                             TextBox day)
        {
            if (String.IsNullOrEmpty(dateStr))
            {
                year.Text = "";
                month.Text = "";
                day.Text = "";
                return;
            }
            int iyear, imonth, iday;
            U.SplitDate(dateStr, out iyear, out imonth, out iday);
            year.Text = iyear.ToString("0000");
            month.Text = (imonth == 0) ? "" : imonth.ToString("00");
            day.Text = (iday == 0) ? "" : iday.ToString("00");
        }
        //****************************************************************************************************************************
        private bool CheckDate()
        {
            if (String.IsNullOrEmpty(FirstName_textBox.Text))
            {
                return true;
            }
            bool bSuccess = true;
            int iDay = DiedDateDay_textBox.Text.ToIntNoError();
            int iYear = DiedDateYear_textBox.Text.ToIntNoError();
            int iMonth = DiedDateMonth_textBox.Text.ToIntNoError();
            if (iYear == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Date: " + DiedDateYear_textBox.Text.ToString());
                bSuccess = false;
                iYear = 2001;
            }
            else
            if (!String.IsNullOrEmpty(m_FirstName) && iYear < 1000 && iYear != 0)
            {
                MessageBox.Show("Value for Year cannot be prior to 1000");
                bSuccess = false;
            }
            else
            if (iYear > 2100)
            {
                MessageBox.Show("Date Cannot be greater than totays date");
                bSuccess = false;
            }
            else
            if (iMonth == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Date: " + DiedDateMonth_textBox.Text.ToString());
                bSuccess = false;
            }
            else
            if (iMonth > 12)
            {
                MessageBox.Show("Invalid value for Month");
                bSuccess = false;
            }
            else
            if (iDay == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Date Day: " + DiedDateDay_textBox.Text.ToString());
                bSuccess = false;
                iDay = 1;
            }
            else
            if (iDay > U.LastDayOfMonth(iMonth, iYear))
            {
                MessageBox.Show("Day is greater than last day for month");
                bSuccess = false;
            }
            if (bSuccess)
            {
                DateTime TodaysDate = System.DateTime.Now;
                if (iYear > TodaysDate.Year || (iYear == TodaysDate.Year && iMonth > TodaysDate.Month))
                {
                    MessageBox.Show("Date Cannot be greater than todays date");
                    bSuccess = false;
                }
            }
            return bSuccess;
        }
        //****************************************************************************************************************************
        private bool CheckAge()
        {
            bool bSuccess = true;
            int iYears = AgeYears_textBox.Text.ToIntNoError();
            int iMonths = AgeMonths_textBox.Text.ToIntNoError();
            int iDays = AgeDays_textBox.Text.ToIntNoError();
            if (iYears == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Age Years: " + AgeYears_textBox.Text.ToString());
                bSuccess = false;
            }
            else
            if (iYears < 0 || iYears > 120)
            {
                MessageBox.Show("Invalid Value For Age Years: " + AgeYears_textBox.Text.ToString());
                bSuccess = false;
            }
            if (iMonths == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Age Months: " + AgeMonths_textBox.Text.ToString());
                bSuccess = false;
            }
            else
            if (iMonths < 0 || iMonths > 12)
            {
                MessageBox.Show("Invalid Value For Age Months: " + AgeYears_textBox.Text.ToString());
                bSuccess = false;
            }
            if (iDays == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Age Days: " + AgeDays_textBox.Text.ToString());
                bSuccess = false;
            }
            else
            if (iDays < 0 || iDays > 365)
            {
                MessageBox.Show("Invalid Value For Age Days: " + AgeYears_textBox.Text.ToString());
                bSuccess = false;
            }
            return bSuccess;
        }
        private bool AbortIfAlreadyExistsAndModifiedWithoutSaving()
        {
            if (m_iCemeteryRecordID == 0)
            {
                return false;
            }
            if (SaveIfDesired())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //****************************************************************************************************************************
        private void LookToSeeIfTheRecordWasDeleted()
        {
            DataTable NewTbl = new DataTable();
            SQL.GetCemeteryRecord(NewTbl, m_iCemeteryRecordID);
            if (NewTbl.Rows.Count == 0)
            {
                NewPerson();
            }
            else if (m_bDidSearch)
            {
                SQL.GetCemeteryRecord(m_CemeteryRecord_tbl, m_iCemeteryRecordID);
                if (m_CemeteryRecord_tbl.Rows.Count > 0)
                {
                    DisplayCemeteryRecord(m_CemeteryRecord_tbl.Rows[0]);
                    m_bDidSearch = false;
                    SearchBy = eSearchOption.SO_None;
                }
            }
        }
        //****************************************************************************************************************************
        private void NewPerson()
        {
            m_iCemeteryRecordID = 0;
            m_CemeteryRecord_tbl.Clear();
            InitializeFields();
            SetToUnmodified();
        }
        //****************************************************************************************************************************
        private void GetCemeteryRecordFromGrid(DataTable tbl,
                                            string startingWith)
        {
            CGridCemeteryRecord GridCemeteryRecord = new CGridCemeteryRecord(m_SQL, tbl, Cemetery._Cemetery);
            GridCemeteryRecord.SetStartingWith(startingWith);
            GridCemeteryRecord.ShowDialog();
            int iCemeteryRecordID = GridCemeteryRecord.SelectedCemeteryRecordID;
            if (iCemeteryRecordID != 0)
            {
                m_iCemeteryRecordID = iCemeteryRecordID;
                m_CemeteryRecord_tbl.Rows.Clear();
                SQL.GetCemeteryRecord(m_CemeteryRecord_tbl, m_iCemeteryRecordID);
                if (m_CemeteryRecord_tbl.Rows.Count > 0)
                {
                    DisplayCemeteryRecord(m_CemeteryRecord_tbl.Rows[0]);
                }
            }
            else
                if (m_iCemeteryRecordID != 0)
            {
                LookToSeeIfTheRecordWasDeleted();
            }
        }
        //****************************************************************************************************************************
        private string OrderByStatement()
        {
            return " order by LastName,FirstName,MiddleName,Suffix,Prefix;";
        }
        //****************************************************************************************************************************
        private void SetAllNameValues()
        {
            m_FirstName = FirstName_textBox.Text.TrimString();
            m_MiddleName = MiddleName_textBox.Text.TrimString();
            m_LastName = LastName_textBox.Text.TrimString();
            m_Suffix = Suffix_comboBox.Text.TrimString();
            m_Prefix = Prefix_comboBox.Text.TrimString();
            m_SpouseFirstName = SpouseFirstName_textBox.Text.TrimString();
            m_SpouseMiddleName = SpouseMiddleName_textBox.Text.TrimString();
            m_SpouseLastName = SpouseLastName_textBox.Text.TrimString();
            m_SpouseSuffix = SpouseSuffix_comboBox.Text.TrimString();
            m_SpousePrefix = SpousePrefix_comboBox.Text.TrimString();
            m_FatherFirstName = FatherFirstName_textBox.Text.TrimString();
            m_FatherMiddleName = FatherMiddleName_textBox.Text.TrimString();
            m_FatherLastName = FatherLastName_textBox.Text.TrimString();
            m_FatherSuffix = FatherSuffix_comboBox.Text.TrimString();
            m_FatherPrefix = FatherPrefix_comboBox.Text.TrimString();
            m_MotherFirstName = MotherFirstName_textBox.Text.TrimString();
            m_MotherMiddleName = MotherMiddleName_textBox.Text.TrimString();
            m_MotherLastName = MotherLastName_textBox.Text.TrimString();
            m_MotherSuffix = MotherSuffix_comboBox.Text.TrimString();
            m_MotherPrefix = MotherPrefix_comboBox.Text.TrimString();
        }
        //****************************************************************************************************************************
        private void SearchForPerson(string sLastName, 
                                     string startingPerson)
        {
            if (AbortIfAlreadyExistsAndModifiedWithoutSaving())
            {
                return;
            }
            SetAllNameValues();
            SearchCemeteryRecords(startingPerson);
        }
        //****************************************************************************************************************************
        private void SearchCemeteryRecords(string startingPerson)
        {
            DataTable tbl = SQL.DefineCemeteryRecordTable();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.CemeteryRecordID_col] };
            if (SearchBy == eSearchOption.SO_ReturnToLast)
            {
                Cemetery cemetery = U.GetCemetery(Cemetery_comboBox.Text);
                SQL.SelectAll(U.CemeteryRecord_Table, SQL.OrderBy(U.CemeteryRecordID_col), tbl,
                                              new NameValuePair(U.CemeteryID_col, (int)cemetery));
            }
            else if (SearchAllNames_radioButton.Checked)
            {
                SearchCemeteryRecords(tbl, new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix));
            }
            else if (SearchAllNamesFather_radioButton.Checked)
            {
                SearchCemeteryRecords(tbl, new PersonName(m_FatherFirstName, m_FatherMiddleName, m_FatherLastName, m_FatherSuffix, m_FatherPrefix));
            }
            else if (SearchAllNamesSpouse_radioButton.Checked)
            {
                SearchCemeteryRecords(tbl, new PersonName(m_SpouseFirstName, m_SpouseMiddleName, m_SpouseLastName, m_SpouseSuffix, m_SpousePrefix));
            }
            else
            {
                PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
                SQL.PersonsBasedOnNameOptions(tbl, false, U.CemeteryRecord_Table, U.CemeteryRecordID_col, SearchBy, personName, "");
            }
            if (tbl.Rows.Count == 0)
            {
                MessageBox.Show("No Records Found");
            }
            else
            {
                GetCemeteryRecordFromGrid(tbl, startingPerson);
            }
        }
        //****************************************************************************************************************************
        private void SearchCemeteryRecords(DataTable tbl,
                                           PersonName personName)
        {
            SearchCemeteryRecords(tbl, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
            SearchCemeteryRecords(tbl, U.SpouseFirstName_col, U.SpouseMiddleName_col, U.SpouseLastName_col, personName);
            SearchCemeteryRecords(tbl, U.FatherFirstName_col, U.FatherMiddleName_col, U.FatherLastName_col, personName);
            SearchCemeteryRecords(tbl, U.MotherFirstName_col, U.MotherMiddleName_col, U.MotherLastName_col, personName);
        }
        //****************************************************************************************************************************
        private void SearchCemeteryRecords(DataTable tbl,
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
            PersonSearchTableAndColumns personSearchTableAndColumns = new PersonSearchTableAndColumns(U.CemeteryRecord_Table,
                              firstName_col, middleName_col, lastNameCol);
            SQL.PersonsBasedOnNameOptions(tbl, false, personSearchTableAndColumns, U.CemeteryRecordID_col, SearchBy, personName);
        }
        //****************************************************************************************************************************
        private void SearchAll_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_AllNames;
            SearchForPerson("", "");
        }
        //****************************************************************************************************************************
        private void SearchStartingWith_button_Click(object sender, System.EventArgs e)
        {
            m_bDidSearch = true;
            SearchBy = eSearchOption.SO_StartingWith;
            SearchForPerson("", "");
            m_bDidSearch = false;
        }
        //****************************************************************************************************************************
        private void SearchPartial_button_Click(object sender, System.EventArgs e)
        {
            m_bDidSearch = true;
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson("", "");
            m_bDidSearch = false;
        }
        //****************************************************************************************************************************
        private void SearchSimilar_button_Click(object sender, System.EventArgs e)
        {
            m_bDidSearch = true;
            SearchBy = eSearchOption.SO_Similar;
            SearchForPerson("", "");
            m_bDidSearch = false;
        }
        //****************************************************************************************************************************
        private bool ConvertDate(string  sDate,
                                 ref int iYear,
                                 ref int iMonth,
                                 ref int iDay)
        {
            if (sDate.Length == 4)
            {
                iMonth = 12;
                iDay = 31;
                iYear = sDate.ToIntNoError();
                if (iYear == 0)
                    return false;
                else
                    return true;
            }
            if (sDate.Length == 10)
            {
                iYear = sDate.Substring(0, 4).ToIntNoError();
                iMonth = sDate.Substring(5, 2).ToIntNoError();
                iDay = sDate.Substring(8, 2).ToIntNoError();
                if (iYear == 0 || iMonth == 0 || iDay == 0)
                    return false;
                else
                    return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private void GetName(int    iPersonID,
                             ref string FirstName,
                             ref string MiddleName,
                             ref string LastName,
                             ref string Suffix,
                             ref string Prefix)
        {
            DataTable tbl = new DataTable();
            SQL.SelectAll(U.Person_Table, tbl, new NameValuePair(U.PersonID_col, iPersonID));
            FirstName = tbl.Rows[0][U.FirstName_col].ToString();
            MiddleName = tbl.Rows[0][U.MiddleName_col].ToString();
            LastName = tbl.Rows[0][U.LastName_col].ToString();
            Suffix = tbl.Rows[0][U.Suffix_col].ToString();
            Prefix = tbl.Rows[0][U.Prefix_col].ToString();
        }
        //****************************************************************************************************************************
        private void InitializeRow(DataRow CemeteryRecordRow,
                                   DataRow Person_row)
        {
            CemeteryRecordRow[U.LastName_col] = Person_row[U.LastName_col];
            CemeteryRecordRow[U.FirstName_col] = Person_row[U.FirstName_col];
            CemeteryRecordRow[U.MiddleName_col] = Person_row[U.MiddleName_col];
            CemeteryRecordRow[U.Suffix_col] = Person_row[U.Suffix_col];
            CemeteryRecordRow[U.Prefix_col] = Person_row[U.Prefix_col];
            CemeteryRecordRow[U.FatherLastName_col] = "";
            CemeteryRecordRow[U.FatherFirstName_col] = "";
            CemeteryRecordRow[U.FatherMiddleName_col] = "";
            CemeteryRecordRow[U.FatherSuffix_col] = "";
            CemeteryRecordRow[U.FatherPrefix_col] = "";
            CemeteryRecordRow[U.MotherLastName_col] = "";
            CemeteryRecordRow[U.MotherFirstName_col] = "";
            CemeteryRecordRow[U.MotherMiddleName_col] = "";
            CemeteryRecordRow[U.MotherSuffix_col] = "";
            CemeteryRecordRow[U.MotherPrefix_col] = "";
            CemeteryRecordRow[U.SpouseID_col] = 0;
            CemeteryRecordRow[U.Book_col] = "";
            CemeteryRecordRow[U.Page_col] = "";
            CemeteryRecordRow[U.DateYear_col] = 0;
            CemeteryRecordRow[U.DateMonth_col] = 0;
            CemeteryRecordRow[U.DateDay_col] = 0;
            CemeteryRecordRow[U.AgeYears_col] = 0;
            CemeteryRecordRow[U.AgeMonths_col] = 0;
            CemeteryRecordRow[U.AgeDays_col] = 0;
            CemeteryRecordRow[U.Disposition_col] = ' ';
            CemeteryRecordRow[U.CemeteryName_col] = "";
            CemeteryRecordRow[U.LotNumber_col] = "";
            CemeteryRecordRow[U.Notes_col] = "";
            CemeteryRecordRow[U.PersonID_col] = 0;
            CemeteryRecordRow[U.FatherID_col] = 0;
            CemeteryRecordRow[U.MotherID_col] = 0;
            CemeteryRecordRow[U.ExcludeFromSite_col] = 0;
        }
        //****************************************************************************************************************************
        private void IntegratedPerson_button_Click(object sender, System.EventArgs e)
        {
            if (m_iPersonID == 0)
                MessageBox.Show("Person Has not been integrated");
            else
            {
                FPerson Person = new FPerson(m_SQL, m_iPersonID, false);
                Person.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private string GetSex(eSex Sex)
        {
            switch (Sex)
            {
                case eSex.eMale: return "M";
                case eSex.eFemale: return "F";
                default: return " ";
            }
        }
        //****************************************************************************************************************************
        private bool PersonIntegrated(bool   NameIntegratedChecked,
                                      string sLastName,
                                      string sMaidenName)
        {
            return (NameIntegratedChecked || (sLastName.Length == 0 && sMaidenName.Length == 0));
        }
        //****************************************************************************************************************************
        private bool RecordAlreadyIntegrated()
        {
            if (PersonIntegrated(NameIntegrated_checkBox.Checked, LastName_textBox.Text.ToString(), "") &&
                PersonIntegrated(SpouseIntegrated_checkBox.Checked, SpouseIntegrated_checkBox.Text.ToString(), "") &&
                PersonIntegrated(FatherIntegrated_checkBox.Checked, FatherLastName_textBox.Text.ToString(), "") &&
                PersonIntegrated(MotherIntegrated_checkBox.Checked, MotherLastName_textBox.Text.ToString(), FatherLastName_textBox.Text.ToString()))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private void Integratebutton_Click(object sender, System.EventArgs e)
        {
            bool integrationChanged = false;
            if (CemeteryRecordChanged(ref integrationChanged))
            {
                if (!SaveCemeteryRecord())
                {
                    MessageBox.Show("Integration Unsuccesful");
                    return;
                }
            }
            if (m_CemeteryRecord_tbl.Rows.Count == 0)
                return;
            if (RecordAlreadyIntegrated())
            {
                MessageBox.Show("This Person Has Already Been Integrated");
                return;
            }
            DataRow CemeteryRecord_row = m_CemeteryRecord_tbl.Rows[0];
            CIntegrateCemeteryRecord IntegrateCemeteryRecord = new CIntegrateCemeteryRecord(m_SQL, true);
            if (IntegrateCemeteryRecord.IntegrateRecord(CemeteryRecord_row, NameIntegrated_checkBox.Checked, FatherIntegrated_checkBox.Checked, MotherIntegrated_checkBox.Checked,
                                      SpouseIntegrated_checkBox.Checked))
            {
                if (!SQL.SaveIntegratedCemeteryRecords(m_CemeteryRecord_tbl))
                {
                    MessageBox.Show("Integrate Unsuccesful");
                }
                m_iPersonID = m_CemeteryRecord_tbl.Rows[0][U.PersonID_col].ToInt();
                if (m_iCemeteryRecordID > 0)
                {
                    SetIntegratedChecked(m_CemeteryRecord_tbl);
                }
            }
            else
            {
                CemeteryRecord_row[U.PersonID_col] = 0;
                CemeteryRecord_row[U.SpouseID_col] = 0;
                CemeteryRecord_row[U.FatherID_col] = 0;
                CemeteryRecord_row[U.MotherID_col] = 0;
            }
        }
        //****************************************************************************************************************************
        private void SetIntegratedChecked(DataTable CemeteryRecords_tbl)
        {
            NameIntegrated_checkBox.Checked = CemeteryRecords_tbl.Rows[0][U.PersonID_col].ToInt() != 0;
            SpouseIntegrated_checkBox.Checked = CemeteryRecords_tbl.Rows[0][U.SpouseID_col].ToInt() != 0;
            FatherIntegrated_checkBox.Checked = CemeteryRecords_tbl.Rows[0][U.FatherID_col].ToInt() != 0;
            MotherIntegrated_checkBox.Checked = CemeteryRecords_tbl.Rows[0][U.MotherID_col].ToInt() != 0;
        }
        //****************************************************************************************************************************
        private void SetToUnmodified()
        {
            m_bDidSearch = false;
            bDispositionChanged = false;
            bSexChanged = false;
            FirstName_textBox.Modified = false;
            MiddleName_textBox.Modified = false;
            LastName_textBox.Modified = false;
            FatherFirstName_textBox.Modified = false;
            FatherMiddleName_textBox.Modified = false;
            FatherLastName_textBox.Modified = false;
            MotherFirstName_textBox.Modified = false;
            MotherMiddleName_textBox.Modified = false;
            MotherLastName_textBox.Modified = false;
            SpouseFirstName_textBox.Modified = false;
            SpouseMiddleName_textBox.Modified = false;
            SpouseLastName_textBox.Modified = false;
            DiedDateMonth_textBox.Modified = false;
            DiedDateDay_textBox.Modified = false;
            DiedDateYear_textBox.Modified = false;
            AgeYears_textBox.Modified = false;
            AgeMonths_textBox.Modified = false;
            AgeDays_textBox.Modified = false;
            LotNumber_textBox.Modified = false;
            Notes_TextBox.Modified = false;
            ExcludeFromSiteCheckedChanged = false;
        }
        //****************************************************************************************************************************
        private int NewIntegrationID(string sMessageName,
                                     int    iPreviousPersonID)
        {
            string sMessage = sMessageName + " name changed whose similar names do not include the Previously integrated Person. \n" +
                              "Do you wish to change the name and reintegrate?";
            MessageBoxButtons mb = MessageBoxButtons.YesNo;
            switch (MessageBox.Show(sMessage, "", mb))
            {
                case DialogResult.OK:
                    return 0;
                default:
                    return U.Exception;
            }
        }
        //****************************************************************************************************************************
        private bool CheckLastNameAgainstRecord(PersonName personName,
                                               string sMessageName)
        {
            if (String.IsNullOrEmpty(personName.lastName))
            {
                return true;
            }
            if (String.IsNullOrEmpty(personName.firstName))
            {
                MessageBox.Show("Must have a First Name for " + sMessageName);
                return false;
            }
            if (personName.integratedPersonID == 0)
                return true; ;
            DataTable SimilarPersonTable = SQL.DefinePersonTable();
            SimilarPersonTable.PrimaryKey = new DataColumn[] { SimilarPersonTable.Columns[U.PersonID_col] };
            if (SQL.PersonExists(SimilarPersonTable, true, U.Person_Table, U.PersonID_col, personName))
            {
                DataRow row = SimilarPersonTable.Rows.Find(personName.integratedPersonID);
                if (row == null)
                    personName.integratedPersonID = NewIntegrationID(sMessageName, personName.integratedPersonID);
            }
            else
            {
                personName.integratedPersonID = NewIntegrationID(sMessageName, personName.integratedPersonID);
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool CheckLastNames(PersonSpouseWithParents personSpouseWithParents)
        {
            if (!CheckLastNameAgainstRecord(personSpouseWithParents.PersonName, "Person"))
            {
                return false;
            }
            if (!CheckLastNameAgainstRecord(personSpouseWithParents.FatherName, "Father"))
            {
                return false;
            }
            if (!CheckLastNameAgainstRecord(personSpouseWithParents.MotherName, "Mother"))
            {
                return false;
            }
            if (!CheckLastNameAgainstRecord(personSpouseWithParents.SpouseName, "Spouse"))
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private char SetDisposition()
        {
            if (Buried_radioButton.Checked)
                return 'B';
            else 
            if (Cremated_radioButton.Checked)
                return 'C';
            else 
            if (Other_radioButton.Checked)
                return 'O';
            else
                return ' ';
        }
        //****************************************************************************************************************************
        private char SetSex()
        {
            if (Male_radioButton.Checked)
                return 'M';
            else
            if (Female_radioButton.Checked)
                return 'F';
            else
                return ' ';
        }
        //****************************************************************************************************************************
        private char SexCharFromRadioButtons()
        {
            if (Male_radioButton.Checked)
                return 'M';
            else if (Female_radioButton.Checked)
                return 'F';
            else
                return ' ';
        }
        //****************************************************************************************************************************
        private PersonSpouseWithParents SetPersonWithParents()
        {
            int iIntegratedPersonID = 0;
            int iIntegratedFatherID = 0;
            int iIntegratedMotherID = 0;
            int iIntegratedSpouseID = 0;
            if (m_CemeteryRecord_tbl.Rows.Count != 0)
            {
                DataRow PersonRow = m_CemeteryRecord_tbl.Rows[0];
                iIntegratedPersonID = PersonRow[U.PersonID_col].ToInt();
                iIntegratedFatherID = PersonRow[U.FatherID_col].ToInt();
                iIntegratedMotherID = PersonRow[U.MotherID_col].ToInt();
                iIntegratedSpouseID = PersonRow[U.SpouseID_col].ToInt();
            }
            PersonName personName = new PersonName(FirstName_textBox.Text.SetNameForDatabase(),
                                                   MiddleName_textBox.Text.SetNameForDatabase(),
                                                   LastName_textBox.Text.SetNameForDatabase(),
                                                   Suffix_comboBox.Text.SetPrefixForDatabase(),
                                                   Prefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedPersonID);
            PersonName fatherName = new PersonName(FatherFirstName_textBox.Text.SetNameForDatabase(),
                                                   FatherMiddleName_textBox.Text.SetNameForDatabase(),
                                                   FatherLastName_textBox.Text.SetNameForDatabase(),
                                                   FatherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                                   FatherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedFatherID);
            PersonName motherName = new PersonName(MotherFirstName_textBox.Text.SetNameForDatabase(),
                                                   MotherMiddleName_textBox.Text.SetNameForDatabase(),
                                                   MotherLastName_textBox.Text.SetNameForDatabase(),
                                                   MotherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                                   MotherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedMotherID);
            PersonName spouseName = new PersonName(SpouseFirstName_textBox.Text.SetNameForDatabase(),
                                                   SpouseMiddleName_textBox.Text.SetNameForDatabase(),
                                                   SpouseLastName_textBox.Text.SetNameForDatabase(),
                                                   SpouseSuffix_comboBox.Text.SetPrefixForDatabase(),
                                                   SpousePrefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedSpouseID);
            PersonName emptyName = new PersonName("", "", "", "", "", 0);
            PersonWithParents personWithParents = new PersonWithParents(EVitalRecordType.eSearch, personName, fatherName, motherName);
            PersonWithParents spouseWithParents = new PersonWithParents(EVitalRecordType.eSearch, spouseName, emptyName, emptyName);
            return new PersonSpouseWithParents(personWithParents, spouseWithParents);
        }
        //****************************************************************************************************************************
        private bool ThisIsASpouseRecord()
        {
            return (m_CemeteryRecord_tbl.Rows.Count > 1);
        }
        //****************************************************************************************************************************
        private CemeteryRecordProperties SetCategoryRecordProperties()
        {
            CemeteryRecordProperties cemeteryRecordProperties = new CemeteryRecordProperties(
                U.GetCemetery(Cemetery_comboBox.Text),
                PersonNameOnGrave_textBox.Text.TrimString(),
                SpouseNameOnGrave_textBox.Text.TrimString(),
                MotherNameOnGrave_textBox.Text.TrimString(),
                FatherNameOnGrave_textBox.Text.TrimString(),
                U.BuildDate(BornDateYear_textBox.Text.ToInt(), BornDateMonth_textBox.Text.ToInt(), BornDateDay_textBox.Text.ToInt()),
                U.BuildDate(DiedDateYear_textBox.Text.ToInt(), DiedDateMonth_textBox.Text.ToInt(), DiedDateDay_textBox.Text.ToInt()),
                AgeYears_textBox.Text.ToInt(),
                AgeMonths_textBox.Text.ToInt(),
                AgeDays_textBox.Text.ToInt(),
                LotNumber_textBox.Text.TrimString(),
                SetDisposition(),
                SetSex(),
                Epitaph_TextBox.Text.TrimString(),
                Notes_TextBox.Text.TrimString());
            return cemeteryRecordProperties;
        }
        //****************************************************************************************************************************
        private bool HandleErrorCode(ErrorCodes errorCode)
        {
            switch (errorCode)
            {
                case ErrorCodes.eSuccess:
                    m_iCemeteryRecordID = m_CemeteryRecord_tbl.Rows[0][U.CemeteryRecordID_col].ToInt();
                    SetToUnmodified();
                    SetAllNameValues();
                    SetIntegratedChecked(m_CemeteryRecord_tbl);
                    return true;
                case ErrorCodes.eSpouseRecordDoesNotExist:
                    MessageBox.Show("Spouse Record Not In Database");
                    return false;
                case ErrorCodes.eSaveUnsuccessful:
                default:
                    MessageBox.Show("Save Unsuccesful");
                    return false;
            }
        }
        //****************************************************************************************************************************
        private bool NotValidInfo(PersonSpouseWithParents personSpouseWithParents,
                                  CemeteryRecordProperties cemeteryRecordProperties)
        {
            bool integrationChanged = false;
            if (!CemeteryRecordChanged(ref integrationChanged))
                return true;
            if (!CheckDate())
                return true;
            if (!CheckAge())
                return true;
            if (String.IsNullOrEmpty(Cemetery_comboBox.Text))
            {
                MessageBox.Show("Please Chose a Cemetery");
                return true;
            }
            if (!CheckLastNames(personSpouseWithParents))
                return true;
            if (RecordAlreadyExits(personSpouseWithParents, cemeteryRecordProperties))
            {
                MessageBox.Show("This record has already been entered");
                SetToUnmodified();
                return true;
            }
            PersonSpouseWithParents personSpouseWithParentsNoMiddleSuffixPrefix = new PersonSpouseWithParents(personSpouseWithParents);
            personSpouseWithParentsNoMiddleSuffixPrefix.RemoveMiddleSuffixPrefix();

            if (RecordAlreadyExitsNoMiddleSuffixPrefix(personSpouseWithParents, cemeteryRecordProperties))
            {
                if (MessageBox.Show("This record may have already been entered.  Save Anyway", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool RecordAlreadyExitsNoMiddleSuffixPrefix(PersonSpouseWithParents personSpouseWithParents,
                                                            CemeteryRecordProperties cemeteryRecordProperties)
        {
            DataTable tbl = new DataTable();
            //SQL.GetVitalRecordFromNameNoMiddleSuffixPrefixDate(tbl, VitalRecordType, personSpouseWithParents.PersonName, cemeteryRecordProperties);
            if (tbl.Rows.Count == 0)
                return false;
            else
                return true;
        }
        //****************************************************************************************************************************
        private bool RecordAlreadyExits(PersonSpouseWithParents personSpouseWithParents,
                                        CemeteryRecordProperties cemeteryRecordProperties)
        {
            DataTable tbl = new DataTable();
            //SQL.GetVitalRecordFromNameDate(tbl, VitalRecordType, personSpouseWithParents.PersonName, cemeteryRecordProperties);
            if (tbl.Rows.Count == 0)
                return false;
            else
                return true;
        }
        //****************************************************************************************************************************
        private void UpdateFirstNameTableForPerson(PersonName personName,
                                                   string     sSex)
        {
            if (personName.firstName.Length != 0)
            {
                SQL.UpdateFirstNameTable(personName.firstName, sSex);
            }
        }
        //****************************************************************************************************************************
        private void UpdateFirstNameTableWithAllPersons(PersonSpouseWithParents personSpouseWithParents,
                                                        char sex)
        {
            UpdateFirstNameTableForPerson(personSpouseWithParents.PersonName, sex.ToString());
            UpdateFirstNameTableForPerson(personSpouseWithParents.FatherName, "M");
            UpdateFirstNameTableForPerson(personSpouseWithParents.MotherName, "F");
            sex = (sex != 'F') ? 'F' : 'M'; // reverse sex for spouse
            UpdateFirstNameTableForPerson(personSpouseWithParents.SpouseName, sex.ToString());
        }
        //****************************************************************************************************************************
        private ArrayList UpdateCemeteryRecordInDataTable(PersonSpouseWithParents personSpouseWithParents,
                                                  CemeteryRecordProperties cemeteryRecordProperties)
        {
            ArrayList FieldsModified = new ArrayList();
            DataRow cemetery_row = m_CemeteryRecord_tbl.Rows[0];
            AddToFieldsModifiedIfChanged(FieldsModified, cemetery_row, personSpouseWithParents.PersonName, U.FirstName_col, 
                                         U.MiddleName_col, U.LastName_col, U.Suffix_col, U.Prefix_col);
            AddToFieldsModifiedIfChanged(FieldsModified, cemetery_row, personSpouseWithParents.FatherName, U.FatherFirstName_col, 
                                         U.FatherMiddleName_col, U.FatherLastName_col, U.FatherSuffix_col, U.FatherPrefix_col);
            AddToFieldsModifiedIfChanged(FieldsModified, cemetery_row, personSpouseWithParents.MotherName, U.MotherFirstName_col,
                                         U.MotherMiddleName_col, U.MotherLastName_col, U.MotherSuffix_col, U.MotherPrefix_col);
            AddToFieldsModifiedIfChanged(FieldsModified, cemetery_row, personSpouseWithParents.SpouseName, U.SpouseFirstName_col,
                                             U.SpouseMiddleName_col, U.SpouseLastName_col, U.SpouseSuffix_col, U.SpousePrefix_col);
            AddToFieldsModifiedIfChanged(FieldsModified, cemeteryRecordProperties);
            if (m_PersonIntegrated && !NameIntegrated_checkBox.Checked)
            {
                cemetery_row[U.PersonID_col] = 0;
                FieldsModified.Add(U.PersonID_col);
                m_PersonIntegrated = false;
            }
            if (m_SpouseIntegrated && !SpouseIntegrated_checkBox.Checked)
            {
                cemetery_row[U.SpouseID_col] = 0;
                FieldsModified.Add(U.SpouseID_col);
                m_SpouseIntegrated = false;
            }
            if (m_FatherIntegrated && !FatherIntegrated_checkBox.Checked)
            {
                cemetery_row[U.FatherID_col] = 0;
                FieldsModified.Add(U.FatherID_col);
                m_FatherIntegrated = false;
            }
            if (m_MotherIntegrated && !MotherIntegrated_checkBox.Checked)
            {
                cemetery_row[U.MotherID_col] = 0;
                FieldsModified.Add(U.MotherID_col);
                m_MotherIntegrated = false;
            }
            return FieldsModified;
        }
        //****************************************************************************************************************************
        private void AddToFieldsModifiedIfChanged(ArrayList FieldsModified,
                                                  CemeteryRecordProperties cemeteryRecordProperties)
        {
            DataColumnCollection columns = m_CemeteryRecord_tbl.Columns;
            DataRow row = m_CemeteryRecord_tbl.Rows[0];
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.NameOnGrave_col, cemeteryRecordProperties.NameOnGrave);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.SpouseNameOnGrave_col, cemeteryRecordProperties.SpouseNameOnGrave);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.FatherNameOnGrave_col, cemeteryRecordProperties.FatherNameOnGrave);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.MotherNameOnGrave_col, cemeteryRecordProperties.MotherNameOnGrave);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.BornDate_col, cemeteryRecordProperties.BornDate);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.DiedDate_col, cemeteryRecordProperties.DiedDate);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeYears_col, cemeteryRecordProperties.AgeYears);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeMonths_col, cemeteryRecordProperties.AgeMonths);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeDays_col, cemeteryRecordProperties.AgeDays);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.Disposition_col, cemeteryRecordProperties.Disposition);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.LotNumber_col, cemeteryRecordProperties.LotNumber);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Epitaph_col, cemeteryRecordProperties.Epitaph);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Notes_col, cemeteryRecordProperties.Notes);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.Sex_col, cemeteryRecordProperties.Sex);
        }
        //****************************************************************************************************************************
        private void AddToFieldsModifiedIfChanged(ArrayList FieldsModified,
                                                  DataRow row,
                                                  PersonName personName,
                                                  string firstName_col,
                                                  string middleName_col,
                                                  string lastName_col,
                                                  string suffix_col,
                                                  string prefix_col)
        {
            DataColumnCollection columns = m_CemeteryRecord_tbl.Columns;
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, firstName_col, personName.firstName);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, middleName_col, personName.middleName);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, lastName_col, personName.lastName);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, suffix_col, personName.suffix);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, prefix_col, personName.prefix);
        }
        //****************************************************************************************************************************
        private void AddCemeteryRecordToDataRow(DataTable CemeteryRecord_tbl,
                                             PersonSpouseWithParents personSpouseWithParents,
                                             CemeteryRecordProperties cemeteryRecordProperties,
                                             int iSpouseID)
        {
            DataRow CemeteryRecord_row = CemeteryRecord_tbl.NewRow();
            CemeteryRecord_row[U.FirstName_col] = personSpouseWithParents.PersonWithParents.PersonName.firstName;
            CemeteryRecord_row[U.MiddleName_col] = personSpouseWithParents.PersonWithParents.PersonName.middleName;
            CemeteryRecord_row[U.LastName_col] = personSpouseWithParents.PersonWithParents.PersonName.lastName;
            CemeteryRecord_row[U.Suffix_col] = personSpouseWithParents.PersonWithParents.PersonName.suffix;
            CemeteryRecord_row[U.Prefix_col] = personSpouseWithParents.PersonWithParents.PersonName.prefix;
            CemeteryRecord_row[U.SpouseFirstName_col] = personSpouseWithParents.SpouseWithParents.PersonName.firstName;
            CemeteryRecord_row[U.SpouseMiddleName_col] = personSpouseWithParents.SpouseWithParents.PersonName.middleName;
            CemeteryRecord_row[U.SpouseLastName_col] = personSpouseWithParents.SpouseWithParents.PersonName.lastName;
            CemeteryRecord_row[U.SpouseSuffix_col] = personSpouseWithParents.SpouseWithParents.PersonName.suffix;
            CemeteryRecord_row[U.SpousePrefix_col] = personSpouseWithParents.SpouseWithParents.PersonName.prefix;
            CemeteryRecord_row[U.FatherFirstName_col] = personSpouseWithParents.PersonWithParents.FatherName.firstName;
            CemeteryRecord_row[U.FatherMiddleName_col] = personSpouseWithParents.PersonWithParents.FatherName.middleName;
            CemeteryRecord_row[U.FatherLastName_col] = personSpouseWithParents.PersonWithParents.FatherName.lastName;
            CemeteryRecord_row[U.FatherSuffix_col] = personSpouseWithParents.PersonWithParents.FatherName.suffix;
            CemeteryRecord_row[U.FatherPrefix_col] = personSpouseWithParents.PersonWithParents.FatherName.prefix;
            CemeteryRecord_row[U.MotherFirstName_col] = personSpouseWithParents.PersonWithParents.MotherName.firstName;
            CemeteryRecord_row[U.MotherMiddleName_col] = personSpouseWithParents.PersonWithParents.MotherName.middleName;
            CemeteryRecord_row[U.MotherLastName_col] = personSpouseWithParents.PersonWithParents.MotherName.lastName;
            CemeteryRecord_row[U.MotherSuffix_col] = personSpouseWithParents.PersonWithParents.MotherName.suffix;
            CemeteryRecord_row[U.MotherPrefix_col] = personSpouseWithParents.PersonWithParents.MotherName.prefix;
            CemeteryRecord_row[U.SpouseID_col] = iSpouseID;
            CemeteryRecord_row[U.NameOnGrave_col] = cemeteryRecordProperties.NameOnGrave;
            CemeteryRecord_row[U.SpouseNameOnGrave_col] = cemeteryRecordProperties.SpouseNameOnGrave;
            CemeteryRecord_row[U.FatherNameOnGrave_col] = cemeteryRecordProperties.FatherNameOnGrave;
            CemeteryRecord_row[U.MotherNameOnGrave_col] = cemeteryRecordProperties.MotherNameOnGrave;
            CemeteryRecord_row[U.BornDate_col] = cemeteryRecordProperties.BornDate;
            CemeteryRecord_row[U.DiedDate_col] = cemeteryRecordProperties.DiedDate;
            CemeteryRecord_row[U.AgeYears_col] = cemeteryRecordProperties.AgeYears;
            CemeteryRecord_row[U.AgeMonths_col] = cemeteryRecordProperties.AgeMonths;
            CemeteryRecord_row[U.AgeDays_col] = cemeteryRecordProperties.AgeDays;
            CemeteryRecord_row[U.Sex_col] = cemeteryRecordProperties.Sex;
            CemeteryRecord_row[U.CemeteryID_col] = (int)cemeteryRecordProperties.cemetery;
            CemeteryRecord_row[U.LotNumber_col] = cemeteryRecordProperties.LotNumber;
            CemeteryRecord_row[U.Disposition_col] = cemeteryRecordProperties.Disposition;
            CemeteryRecord_row[U.Epitaph_col] = cemeteryRecordProperties.Epitaph;
            CemeteryRecord_row[U.Notes_col] = cemeteryRecordProperties.Notes;
            CemeteryRecord_row[U.Sex_col] = cemeteryRecordProperties.Sex;
            CemeteryRecord_row[U.PersonID_col] = personSpouseWithParents.PersonName.integratedPersonID;
            CemeteryRecord_row[U.SpouseID_col] = personSpouseWithParents.SpouseName.integratedPersonID;
            CemeteryRecord_row[U.FatherID_col] = personSpouseWithParents.FatherName.integratedPersonID;
            CemeteryRecord_row[U.MotherID_col] = personSpouseWithParents.MotherName.integratedPersonID;
            CemeteryRecord_tbl.Rows.Add(CemeteryRecord_row);
        }
        //****************************************************************************************************************************
        private bool SaveCemeteryRecord()
        {
            PersonSpouseWithParents personSpouseWithParents = SetPersonWithParents();
            CemeteryRecordProperties cemeteryRecordProperties = SetCategoryRecordProperties();
            if (NotValidInfo(personSpouseWithParents, cemeteryRecordProperties))
            {
                return false;
            }
            try
            {
                CheckNamesOnGrave();
                if (m_iCemeteryRecordID == 0)
                {
                    CreateNewCemeteryRecord(personSpouseWithParents, cemeteryRecordProperties);

                }
                else
                {
                    ArrayList FieldsModified = UpdateCemeteryRecordInDataTable(personSpouseWithParents, cemeteryRecordProperties);
                    SQL.UpdateCemeteryRecord(FieldsModified, m_CemeteryRecord_tbl, personSpouseWithParents);
                }
                UpdateFirstNameTableWithAllPersons(personSpouseWithParents, cemeteryRecordProperties.Sex);
                SetToUnmodified();
                SetAllNameValues();
                SetIntegratedChecked(m_CemeteryRecord_tbl);
                return true;
            }
            catch (HistoricJamaicaException e)
            {
                return HandleErrorCode(e.errorCode);
            }
            catch (Exception e)
            {
                HistoricJamaicaException ex = new HistoricJamaicaException(e.Message);
                UU.ShowErrorMessage(ex);
                return false;
            }
        }
        //****************************************************************************************************************************
        private void CheckNamesOnGrave()
        {
            CheckNameOnGrave(PersonNameOnGrave_textBox, LastName_textBox.Text, FirstName_textBox.Text, MiddleName_textBox.Text, Prefix_comboBox.Text, Suffix_comboBox.Text);
            CheckNameOnGrave(SpouseNameOnGrave_textBox, SpouseLastName_textBox.Text, SpouseFirstName_textBox.Text, SpouseMiddleName_textBox.Text, SpousePrefix_comboBox.Text, SpouseSuffix_comboBox.Text);
            CheckNameOnGrave(FatherNameOnGrave_textBox, FatherLastName_textBox.Text, FatherFirstName_textBox.Text, FatherMiddleName_textBox.Text, FatherPrefix_comboBox.Text, FatherSuffix_comboBox.Text);
            CheckNameOnGrave(MotherNameOnGrave_textBox, MotherLastName_textBox.Text, MotherFirstName_textBox.Text, MotherMiddleName_textBox.Text, MotherPrefix_comboBox.Text, MotherSuffix_comboBox.Text);
        }
        //****************************************************************************************************************************
        private void CheckNameOnGrave(TextBox nameOnGrave_textBox,
                                      string lastName,
                                      string firstName,
                                      string middleName,
                                      string prefix,
                                      string suffix)
        {
            if (!string.IsNullOrEmpty(nameOnGrave_textBox.Text))
            {
                return;
            }
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return;
            }
            nameOnGrave_textBox.Text = SQL.BuildNameString(firstName, middleName, lastName, suffix, prefix, "", "");
        }
        //****************************************************************************************************************************
        private void CreateNewCemeteryRecord(PersonSpouseWithParents personSpouseWithParents,
                                          CemeteryRecordProperties cemeteryRecordProperties)
        {
            AddCemeteryRecordToDataRow(m_CemeteryRecord_tbl, personSpouseWithParents, cemeteryRecordProperties, 0);
            SQL.CreateNewCemeteryRecordRecords(m_CemeteryRecord_tbl);
            m_iCemeteryRecordID = m_CemeteryRecord_tbl.Rows[0][U.CemeteryRecordID_col].ToInt();
        }
        //****************************************************************************************************************************
        private void IntegrateYesNo()
        {
            if (RecordAlreadyIntegrated())
            {
//                MessageBox.Show("Save Successful");
            }
//            else
//            if (MessageBox.Show("Do You Wish to Integrate?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
//            {
            //                IntegrateSingleRecord(m_CemeteryRecord_tbl, NameIntegrated_checkBox.Checked,
//                                FatherIntegrated_checkBox.Checked, MotherIntegrated_checkBox.Checked, SpouseIntegrated_checkBox.Checked,
//                               SpouseFatherIntegrated_checkBox.Checked, SpouseMotherIntegrated_checkBox.Checked);
//           }
        }
        //****************************************************************************************************************************
        private void ExcludeFromSiteChanged_click(object sender, EventArgs e)
        {
            ExcludeFromSiteCheckedChanged = true;
        }
        //****************************************************************************************************************************
        private void ReturnToList_Click(object sender, EventArgs e)
        {
            if (m_NewMode)
            {
                if (SaveIfDesired())
                {
                    NewPerson();
                }
            }
            else
            {
                SearchBy = eSearchOption.SO_ReturnToLast;
                SearchForPerson("", m_iCemeteryRecordID.ToString());
            }
        }
        //****************************************************************************************************************************
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SaveCemeteryRecord() && !RecordAlreadyIntegrated())
            {
                IntegrateYesNo();
            }
        }
        //****************************************************************************************************************************
        private void ValidateMonth_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox) sender;
            string name = textBox.Name;
            int iMonth = textBox.Text.ToInt();
            if (iMonth > 12)
            {
                MessageBox.Show("Invalid Month: " + textBox.Text);
                textBox.Text = "";
                this.ActiveControl = textBox;
            }
            else
            {
                textBox.Text = (iMonth == 0) ? "" : String.Format("{0:00}", iMonth);
            }
        }
        //****************************************************************************************************************************
        private void ValidateDay_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string name = textBox.Name;
            bool isDiedDate = name.ToLower().Contains("died");
            int iMonth = (isDiedDate) ? DiedDateMonth_textBox.ToInt() : BornDateMonth_textBox.ToInt();
            int iDay = (isDiedDate) ? DiedDateDay_textBox.ToInt() : BornDateDay_textBox.ToInt();
            if (iMonth > 12)
            {
                MessageBox.Show("Invalid Month: " + textBox.Text);
                textBox.Text = "";
                this.ActiveControl = textBox;
            }
            else
            {
                textBox.Text = (iMonth == 0) ? "" : String.Format("{0:00}", iMonth);
            }
        }
        //****************************************************************************************************************************
        private void FirstNameLeave_Click(object sender, EventArgs e)
        {
            Male_radioButton.Checked = false;
            Male_radioButton.Checked = false;
            Unknown_radioButton.Checked = false;
            if (!String.IsNullOrEmpty(FirstName_textBox.ToString()))
            {
                string firstName = FirstName_textBox.Text.ToString();
                eSex sex = SQL.GetSex(firstName);
                switch (sex)
                {
                    case eSex.eMale: Male_radioButton.Checked = true; break;
                    case eSex.eFemale: Female_radioButton.Checked = true; break;
                    case eSex.eUnknown:
                    default: Unknown_radioButton.Checked = true; break;
                }
            }
        }
        //****************************************************************************************************************************
        private bool CemeteryRecordChanged(ref bool integrationChanged)
        {
            if (m_bDidSearch)
                return false;
            if (TextBoxModified())
                return true;
            if (bDispositionChanged)
                return true;
            if (bSexChanged)
                return true;
            if (ExcludeFromSiteCheckedChanged)
                return true;
            //if (U.Modified(Cemetery_comboBox.Text.TrimString(), m_CemeteryRecord_tbl, "", U.CemeteryName_col))
            //    return true;
            if (m_CemeteryRecord_tbl.Rows.Count != 0 && m_CemeteryRecord_tbl.Rows[0].RowState != DataRowState.Unchanged)
                return true;
            if (NameChangedOtherThanForSearchOption())
            {
                return true;
            }
            else if (SearchBy != eSearchOption.SO_ReturnToLast)
            {
                m_bDidSearch = true;
            }
            if (IntegrationChanged())
            {
                integrationChanged = true;
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool IntegrationChanged()
        {
            if (m_iCemeteryRecordID == 0)
            {
                return false;
            }
            if (m_PersonIntegrated && !NameIntegrated_checkBox.Checked ||
                m_SpouseIntegrated && !SpouseIntegrated_checkBox.Checked ||
                m_FatherIntegrated && !FatherIntegrated_checkBox.Checked ||
                m_MotherIntegrated && !MotherIntegrated_checkBox.Checked)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool ComboBoxModified()
        {
            if (U.Modified(Suffix_comboBox.Text.TrimString(), m_CemeteryRecord_tbl, "", U.Suffix_col) ||
                U.Modified(Prefix_comboBox.Text.TrimString(), m_CemeteryRecord_tbl, "", U.Prefix_col) ||
                U.Modified(FatherSuffix_comboBox.Text.TrimString(), m_CemeteryRecord_tbl, "", U.FatherSuffix_col) ||
                U.Modified(FatherPrefix_comboBox.Text.TrimString(), m_CemeteryRecord_tbl, "", U.FatherPrefix_col) ||
                U.Modified(MotherSuffix_comboBox.Text.TrimString(), m_CemeteryRecord_tbl, "", U.MotherSuffix_col) ||
                U.Modified(MotherPrefix_comboBox.Text.TrimString(), m_CemeteryRecord_tbl, "", U.MotherPrefix_col))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool NameChangedOtherThanForSearchOption()
        {
            if (SearchBy == eSearchOption.SO_PartialNames || SearchBy == eSearchOption.SO_Similar)
            {
                return false;
            }
            if (MiddleName_textBox.Modified || FirstName_textBox.Modified)
            {
                return true;
            }
            if (SearchBy == eSearchOption.SO_StartingWith)
            {
                return false;
            }
            else
            if (LastName_textBox.Modified)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool TextBoxModified()
        {
            if ((FirstName_textBox.Modified || LastName_textBox.Modified) && m_bDidSearch)
            {
                m_bDidSearch = false;
            }
            else if (PersonNameOnGrave_textBox.Modified ||
                SpouseNameOnGrave_textBox.Modified ||
                FatherNameOnGrave_textBox.Modified ||
                FirstName_textBox.Modified || 
                LastName_textBox.Modified ||
                MotherNameOnGrave_textBox.Modified ||
                MiddleName_textBox.Modified ||
                FatherFirstName_textBox.Modified ||
                FatherMiddleName_textBox.Modified ||
                FatherLastName_textBox.Modified ||
                MotherFirstName_textBox.Modified ||
                MotherMiddleName_textBox.Modified ||
                MotherLastName_textBox.Modified ||
                SpouseFirstName_textBox.Modified ||
                SpouseMiddleName_textBox.Modified ||
                SpouseLastName_textBox.Modified ||
                BornDateMonth_textBox.Modified ||
                BornDateDay_textBox.Modified ||
                BornDateYear_textBox.Modified ||
                DiedDateMonth_textBox.Modified ||
                DiedDateDay_textBox.Modified ||
                DiedDateYear_textBox.Modified ||
                AgeYears_textBox.Modified ||
                AgeMonths_textBox.Modified ||
                AgeDays_textBox.Modified ||
                LotNumber_textBox.Modified ||
                Epitaph_TextBox.Modified ||
                Notes_TextBox.Modified)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected bool SaveIfDesired()
        {
            bool integrationChanged = false;
            if (!CemeteryRecordChanged(ref integrationChanged))
                return true;
            string message = (integrationChanged) ? "Remove Integration and Save Changes?" : "Save Changes?";
            switch (MessageBox.Show(message, "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    if (SaveCemeteryRecord())
                        IntegrateYesNo();
                    else
                        return false;
                    break;
                case DialogResult.No:
                    return true;
                case DialogResult.Cancel:
                    return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (m_sFamilyName.Length != 0) // NonIntegrated Family
                return;
            if (!SaveIfDesired())
                e.Cancel = true;
        }
        //****************************************************************************************************************************
        private void SetSexRadioButtons(char cSex)
        {
            switch (cSex)
            {
                case 'M':
                    {
                        Male_radioButton.Checked = true;
                        this.Female_radioButton.Checked = false;
                        this.Unknown_radioButton.Checked = false;
                        break;
                    }
                case 'F':
                    {
                        Female_radioButton.Checked = true;
                        this.Male_radioButton.Checked = false;
                        this.Unknown_radioButton.Checked = false;
                        break;
                    }
                default:
                    {
                        this.Male_radioButton.Checked = false;
                        this.Female_radioButton.Checked = false;
                        this.Unknown_radioButton.Checked = true;
                        break;
                    }
            }
        }
        //****************************************************************************************************************************
    }
}
