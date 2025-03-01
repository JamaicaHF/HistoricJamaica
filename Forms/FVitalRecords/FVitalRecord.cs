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
    public partial class FVitalRecord : Form
    {
        private eSearchOption SearchBy = eSearchOption.SO_None;
        private int VitalRecordFormHeight = 450;
        private int VitalRecordFormWidthBirthDeath = 840;
        private int m_iVitalRecordID = 0;
        private int m_iPersonID = 0;
        private CSql m_SQL;
        private EVitalRecordType m_eVitalRecordType;
        private DataTable m_VitalRecord_tbl;
        private string m_FirstName = "";
        private string m_MiddleName = "";
        private string m_LastName = "";
        private string m_Suffix = "";
        private string m_Prefix = "";
        private string m_FatherFirstName = "";
        private string m_FatherMiddleName = "";
        private string m_FatherLastName = "";
        private string m_FatherSuffix = "";
        private string m_FatherPrefix = "";
        private string m_SpouseFirstName = "";
        private string m_SpouseMiddleName = "";
        private string m_SpouseLastName = "";
        private string m_SpouseSuffix = "";
        private string m_SpousePrefix = "";
        private string m_MotherFirstName = "";
        private string m_MotherMiddleName = "";
        private string m_MotherLastName = "";
        private string m_MotherSuffix = "";
        private string m_MotherPrefix = "";
        private char m_sSex = ' ';
        private int m_iSelectedVitalRecordID = 0;
        private string m_sFamilyName = "";
        private bool m_bDidSearch = false;
        private bool bDispositionChanged = false;
        private bool bSexChanged = false;
        private bool m_bAbort = false;
        private bool m_UsePreviousRecordAsDefault = false;
        private bool ExcludeFromSiteCheckedChanged = false;

        //****************************************************************************************************************************
        public FVitalRecord(EVitalRecordType eVitalRecordType,
                             CSql SQL)
        {
            m_SQL = SQL;
            m_eVitalRecordType = eVitalRecordType;
            InitializeVitalRecord();
            InitializeFieldLengths();
            if (eVitalRecordType == EVitalRecordType.eSearch)
            {
                SearchBy = eSearchOption.SO_AllNames;
                SearchForPerson("", "");
                if (m_iVitalRecordID == 0)
                    m_bAbort = true;
            }
            else
            {
                InitializeFields();
                SetVitalRecordDisplay();
            }
        }
        //****************************************************************************************************************************
        public FVitalRecord(EVitalRecordType eVitalRecordType,
                             int               iPersonID,
                             CSql              sql)
        {
            m_SQL = sql;
            m_eVitalRecordType = eVitalRecordType;
            InitializeVitalRecord();

            DataTable tbl = SQL.DefineVitalRecord_Table();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.VitalRecordID_col] };
            SQL.GetAllRecordsForPerson(tbl, iPersonID);
            GetVitalRecordFromGrid(tbl, "");
            if (m_iVitalRecordID == 0)
                m_bAbort = true;
        }
        //****************************************************************************************************************************
        public FVitalRecord(CSql SQL, int iSelectedVitalRecordID)
        {
            m_iVitalRecordID = iSelectedVitalRecordID;
            m_SQL = SQL;
            m_iSelectedVitalRecordID = iSelectedVitalRecordID;
            InitializeVitalRecord();
            DisplayVitalRecord(m_iVitalRecordID);
        }
        //****************************************************************************************************************************
        private void InitializeVitalRecord()
        {
            InitializeComponent();
            Cemetery_comboBox.DataSource = U.CemeteryList();
            Cemetery_comboBox.DisplayMember = "Cemetery";
            Cemetery_comboBox.Text = "";
            UU.LoadSuffixComboBox(Suffix_comboBox);
            UU.LoadPrefixComboBox(Prefix_comboBox);
            UU.LoadSuffixComboBox(FatherSuffix_comboBox);
            UU.LoadPrefixComboBox(FatherPrefix_comboBox);
            UU.LoadSuffixComboBox(MotherSuffix_comboBox);
            UU.LoadPrefixComboBox(MotherPrefix_comboBox);
            UU.LoadSuffixComboBox(SpouseSuffix_comboBox);
            UU.LoadPrefixComboBox(SpousePrefix_comboBox);
            UU.LoadSuffixComboBox(SpouseFatherSuffix_comboBox);
            UU.LoadPrefixComboBox(SpouseFatherPrefix_comboBox);
            UU.LoadSuffixComboBox(SpouseMotherSuffix_comboBox);
            UU.LoadPrefixComboBox(SpouseMotherPrefix_comboBox);
            m_VitalRecord_tbl = SQL.DefineVitalRecord_Table();
            LastMother_button.Visible = false;
            LastSpouse_button.Visible = false;
            LastSpouseMother_button.Visible = false;
        }
        //****************************************************************************************************************************
        public bool AbortVitalRecord()
        {
            return m_bAbort;
        }
        //****************************************************************************************************************************
        private void DisplayCemeteryRecord(int iVitalRecordID)
        {
            m_bDidSearch = false;
            SearchBy = eSearchOption.SO_None;
            SQL.GetVitalRecordWithSpouseRecord(m_VitalRecord_tbl, iVitalRecordID);
            if (m_VitalRecord_tbl.Rows.Count > 0)
            {
                DataRow VitalRecord_row = m_VitalRecord_tbl.Rows[0];
                DataRow SpouseVitalRecord_row = SpouseRow(m_VitalRecord_tbl, VitalRecord_row[U.VitalRecordID_col].ToInt(), 
                                                VitalRecord_row[U.SpouseID_col].ToInt());
                DisplayVitalRecord(VitalRecord_row, SpouseVitalRecord_row);
                SetIntegratedChecked(m_VitalRecord_tbl);
                SetToUnmodified();
                SetAllNameValues();
            }
        }
        //****************************************************************************************************************************
        private void DisplayVitalRecord(int iVitalRecordID)
        {
            m_bDidSearch = false;
            SearchBy = eSearchOption.SO_None;
            SQL.GetVitalRecordWithSpouseRecord(m_VitalRecord_tbl, iVitalRecordID);
            if (m_VitalRecord_tbl.Rows.Count > 0)
            {
                DataRow VitalRecord_row = m_VitalRecord_tbl.Rows[0];
                DataRow SpouseVitalRecord_row = SpouseRow(m_VitalRecord_tbl, VitalRecord_row[U.VitalRecordID_col].ToInt(),
                                                VitalRecord_row[U.SpouseID_col].ToInt());
                DisplayVitalRecord(VitalRecord_row, SpouseVitalRecord_row);
                SetIntegratedChecked(m_VitalRecord_tbl);
                SetToUnmodified();
                SetAllNameValues();
            }
        }
        //****************************************************************************************************************************
        public int SpouseID()
        {
            return m_VitalRecord_tbl.Rows[0][U.SpouseID_col].ToInt();
        }
        //****************************************************************************************************************************
        private void SetSpouseFieldsFalse()
        {
            Spouse_groupBox.Visible = false;
            SpouseFather_groupBox.Visible = false;
            SpouseMother_groupBox.Visible = false;
            BirthDate_groupBox.Visible = false;
            Age_groupBox.Visible = false;
            SpouseBirthDate_groupBox.Visible = false;
            SpouseAge_groupBox.Visible = false;
            ExcludeFromSite_checkBox.Location = new System.Drawing.Point(12, 270);
            Save_button.Location = new System.Drawing.Point(131, 270);
            SearchAll_button.Location = new System.Drawing.Point(306, 270);
            SearchPartial_Button.Location = new System.Drawing.Point(306, 300);
            StartingWith_button.Location = new System.Drawing.Point(306, 365);
            Similar_button.Location = new System.Drawing.Point(306, 395);
            LastMother_button.Location = new System.Drawing.Point(472, 270);
            ClientSize = new System.Drawing.Size(VitalRecordFormWidthBirthDeath, VitalRecordFormHeight);
            SearchRequest_groupBox.Location = new System.Drawing.Point(11, 309);
            Notes_TextBox.Location = new System.Drawing.Point(627, 269);
            Notes_TextBox.Size = new System.Drawing.Size(170, 140);
            Notes_label.Location = new System.Drawing.Point(625, 253);
            ReturnToList_button.Location = new System.Drawing.Point(306, 270);
            SearchAllNamesSpouse_radioButton.Visible = false;
        }
        //****************************************************************************************************************************
        private void SetSpouseFieldsTrue(bool bCivalUnion)
        {
            Spouse_groupBox.Visible = true;
            SpouseFather_groupBox.Visible = true;
            SpouseMother_groupBox.Visible = true;
            BirthDate_groupBox.Visible = true;
            Age_groupBox.Visible = true;
            SpouseBirthDate_groupBox.Visible = true;
            SpouseAge_groupBox.Visible = true;
            Sex_groupBox.Visible = true;
            ExcludeFromSite_checkBox.Location = new System.Drawing.Point(12, 480);
            Save_button.Location = new System.Drawing.Point(131, 480);
            SearchAll_button.Location = new System.Drawing.Point(306, 480);
            SearchPartial_Button.Location = new System.Drawing.Point(306, 510);
            StartingWith_button.Location = new System.Drawing.Point(306, 575);
            Similar_button.Location = new System.Drawing.Point(306, 605);
            LastMother_button.Location = new System.Drawing.Point(472, 480);

            BirthDate_groupBox.Location = new System.Drawing.Point(628, 49);
            Age_groupBox.Location = new System.Drawing.Point(628, 119);
            Age_groupBox.Text = "Age";
            SpouseBirthDate_groupBox.Location = new System.Drawing.Point(628, 264);
            SpouseAge_groupBox.Location = new System.Drawing.Point(628, 334);
            BookPage_groupBox.Location = new System.Drawing.Point(816, 49);
            Date_groupBox.Location = new System.Drawing.Point(816, 119);
            Sex_groupBox.Location = new System.Drawing.Point(816, 189);
            SearchRequest_groupBox.Location = new System.Drawing.Point(12, 519);
            Notes_TextBox.Location = new System.Drawing.Point(815, 269);
            Notes_TextBox.Size = new System.Drawing.Size(170, 191);
            Notes_label.Location = new System.Drawing.Point(813, 253);
            ReturnToList_button.Location = new System.Drawing.Point(306, 480);
            SearchAllNamesSpouse_radioButton.Visible = true;
        }
        //****************************************************************************************************************************
        private void SetBirthDeathRecordDisplay()
        {
            SetSpouseFieldsFalse();
            Prefix_comboBox.Visible = false;
            Burial_groupBox.Visible = false;
            BookPage_groupBox.Location = new System.Drawing.Point(628, 49);
            Date_groupBox.Location = new System.Drawing.Point(628, 119);
            Name_groupBox.Text = "Name";
            Burial_groupBox.Visible = false;
        }
        //****************************************************************************************************************************
        private void SetBirthRecordDisplay()
        {
            SetLabelsVisible(false);
            changeToolStripMenuItem.Visible = true;
            if (m_eVitalRecordType == EVitalRecordType.eBirthMale)
            {
                this.Text = "Birth-Male";
                this.changeToolStripMenuItem.Text = "Change To Birth Female";
            }
            else
            {
                this.Text = "Birth-Female";
                this.changeToolStripMenuItem.Text = "Change To Birth Male";
            }
            Date_groupBox.Text = "Date of Birth (M/D/Y)";
            Prefix_comboBox.Visible = false;
            Age_groupBox.Visible = false;
            SpouseAge_groupBox.Visible = false;
            CalcBornDate_groupBox.Visible = false;
            SetBirthDeathRecordDisplay();
        }
        //****************************************************************************************************************************
        private void SetDeathRecordDisplay()
        {
            SetLabelsVisible(false);
            changeToolStripMenuItem.Visible = true;
            if (m_eVitalRecordType == EVitalRecordType.eDeathMale)
            {
                this.Text = "Death-Male";
                this.changeToolStripMenuItem.Text = "Change To Death Female";
            }
            else
            {
                this.Text = "Death-Female";
                this.changeToolStripMenuItem.Text = "Change To Death Male";
            }
            Date_groupBox.Text = "Date of Death (M/D/Y)";
            CalcBornDate_groupBox.Location = new System.Drawing.Point(Age_groupBox.Location.X, Age_groupBox.Location.Y + 70);
            CalcBornDate_groupBox.Visible = true;
            SetBirthDeathRecordDisplay();
            Notes_label.Location = new System.Drawing.Point(SpouseMother_groupBox.Location.X, Notes_label.Location.Y);
            Notes_TextBox.Location = new System.Drawing.Point(SpouseMother_groupBox.Location.X, SpouseMother_groupBox.Location.Y + 7);
            Notes_TextBox.Size = new System.Drawing.Size(Notes_TextBox.Size.Height, SpouseMother_groupBox.Size.Width);
            Age_groupBox.Visible = true;
            Age_groupBox.Location = new System.Drawing.Point(628, 189);
            Age_groupBox.Text = "Age";
            CalcBornDate_groupBox.Location = new System.Drawing.Point(628, 260);
            CalcBornDate_groupBox.Visible = true;
        }
        //****************************************************************************************************************************
        private void SetBurialRecordDisplay()
        {
            SetLabelsVisible(false);
            changeToolStripMenuItem.Visible = false;
            this.Text = "Burial";
            Father_groupBox.Visible = false;
            Mother_groupBox.Visible = false;
            Burial_groupBox.Visible = true;
            CalcBornDate_groupBox.Visible = true;
            Prefix_comboBox.Visible = true;
            Burial_groupBox.Location = new System.Drawing.Point(288, 49);
            Sex_groupBox.Location = new System.Drawing.Point(288, 251);
            Sex_groupBox.Visible = true;
            BookPage_groupBox.Location = new System.Drawing.Point(458, 49);
            Date_groupBox.Location = new System.Drawing.Point(458, 109);
            Date_groupBox.Text = "Date of Death (M/D/Y)";
            Name_groupBox.Text = "Name";
            Age_groupBox.Location = new System.Drawing.Point(458, 169);
            CalcBornDate_groupBox.Location = new System.Drawing.Point(458, 229);
            CalcBornDate_groupBox.Visible = true;
            SetSpouseFieldsFalse();
            ReturnToList_button.Location = new System.Drawing.Point(306, 315);
            Notes_label.Location = new System.Drawing.Point(459, 293);
            Notes_TextBox.Location = new System.Drawing.Point(455, 309);
            Notes_TextBox.Size = new System.Drawing.Size(170, 100);
            ClientSize = new System.Drawing.Size(660, VitalRecordFormHeight);
        }
        //****************************************************************************************************************************
        private void SetLabelsVisible(bool bVisible)
        {
            SpouseFirstName_label.Visible = bVisible;
            SpouseMiddleName_label.Visible = bVisible;
            SpouseLastName_label.Visible = bVisible;
            SpouseSuffix_label.Visible = bVisible;
            SpousePrefix_label.Visible = bVisible;
        }
        //****************************************************************************************************************************
        private void SetCivilUnionRecordDisplay()
        {
            SetLabelsVisible(true);
            Prefix_comboBox.Visible = true;
            bool bCivalUnion = false;
            if (m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyA)
            {
                changeToolStripMenuItem.Visible = false;
                this.Text = "Marriage-Party A";
                Date_groupBox.Text = "Date of Marriage (M/D/Y)";
                Name_groupBox.Text = "Party A";
                Spouse_groupBox.Text = "Party B";
                bCivalUnion = true;
            }
            else
            if (m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyB)
            {
                changeToolStripMenuItem.Visible = false;
                this.Text = "Marriage-Party B";
                Date_groupBox.Text = "Date of Marriage (M/D/Y)";
                Name_groupBox.Text = "Party B";
                Spouse_groupBox.Text = "Party A";
                bCivalUnion = true;
            }
            else
            if (m_eVitalRecordType == EVitalRecordType.eMarriageGroom)
            {
                this.Text = "Marriage-Groom";
                changeToolStripMenuItem.Visible = true;
                this.changeToolStripMenuItem.Text = "Change To Marriage-Bride";
                Date_groupBox.Text = "Date of Marriage (M/D/Y)";
                Name_groupBox.Text = "Groom";
                Spouse_groupBox.Text = "Bride";
            }
            else
            {
                this.Text = "Marriage-Bride";
                changeToolStripMenuItem.Visible = true;
                this.changeToolStripMenuItem.Text = "Change To Marriage-Groom";
                Date_groupBox.Text = "Date of Marriage (M/D/Y)";
                Name_groupBox.Text = "Bride";
                Spouse_groupBox.Text = "Groom";
            }
            SetSpouseFieldsTrue(bCivalUnion);
            CalcBornDate_groupBox.Visible = false;
            Burial_groupBox.Visible = false;
            ClientSize = new System.Drawing.Size(1020, 638);
        }
        //****************************************************************************************************************************
        private void InitializeFieldLengths()
        {
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
            SpouseFatherFirstName_textBox.MaxLength = U.iMaxNameLength;
            SpouseFatherMiddleName_textBox.MaxLength = U.iMaxNameLength;
            SpouseFatherLastName_textBox.MaxLength = U.iMaxNameLength;
            SpouseFatherPrefix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            SpouseFatherSuffix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            SpouseMotherFirstName_textBox.MaxLength = U.iMaxNameLength;
            SpouseMotherMiddleName_textBox.MaxLength = U.iMaxNameLength;
            SpouseMotherLastName_textBox.MaxLength = U.iMaxNameLength;
            SpouseMotherPrefix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            SpouseMotherSuffix_comboBox.MaxLength = U.iMaxPrefixSuffixLength;
            Book_textBox.MaxLength = U.iMaxBookPageLength;
            Page_textBox.MaxLength = U.iMaxBookPageLength;
            DateMonth_textBox.MaxLength = 2;
            DateDay_textBox.MaxLength = 2;
            DateYear_textBox.MaxLength = 4;
            AgeDays_textBox.MaxLength = 2;
            AgeMonths_textBox.MaxLength = 2;
            AgeYears_textBox.MaxLength = 3;
            SpouseAgeDays_textBox.MaxLength = 2;
            SpouseAgeMonths_textBox.MaxLength = 2;
            SpouseAgeYears_textBox.MaxLength = 3;
            Cemetery_comboBox.MaxLength = U.iMaxValueLength;
            LotNumber_textBox.MaxLength = U.iMaxNameLength;
            Notes_TextBox.MaxLength = U.iMaxDescriptionLength;
        }
        //****************************************************************************************************************************
        private void InitializeFields()
        {
            m_sSex = ' ';
            m_UsePreviousRecordAsDefault = false;
            FirstName_textBox.Text = "";
            MiddleName_textBox.Text = "";
            Suffix_comboBox.Text = "";
            Prefix_comboBox.Text = "";
            ExcludeFromSite_checkBox.Checked = false;
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
            InitializeSpouseBurialDateAndIntegratedCheckboxes(m_eVitalRecordType);
        }
        //****************************************************************************************************************************
        private bool NotBurialToDeathType(EVitalRecordType NewVitalRecordType)
        {
            if ((m_eVitalRecordType.IsDeathRecord() && NewVitalRecordType == EVitalRecordType.eBurial) ||
                (NewVitalRecordType.IsDeathRecord() && m_eVitalRecordType == EVitalRecordType.eBurial))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool bDoNotSavePreviousRecordSettings(EVitalRecordType NewVitalRecordType)
        {
            if (m_iVitalRecordID != 0)
                return true;
            return(NewVitalRecordType == m_eVitalRecordType);
        }
        //****************************************************************************************************************************
        private void InitializeSpouseBurialDateAndIntegratedCheckboxes(EVitalRecordType NewVitalRecordType)
        {
            SpouseLastName_textBox.Text = "";
            SpouseFirstName_textBox.Text = "";
            SpouseMiddleName_textBox.Text = "";
            SpouseSuffix_comboBox.Text = "";
            SpousePrefix_comboBox.Text = "";
            SpouseFatherLastName_textBox.Text = "";
            SpouseFatherFirstName_textBox.Text = "";
            SpouseFatherMiddleName_textBox.Text = "";
            SpouseFatherSuffix_comboBox.Text = "";
            SpouseFatherPrefix_comboBox.Text = "";
            SpouseMotherLastName_textBox.Text = "";
            SpouseMotherFirstName_textBox.Text = "";
            SpouseMotherMiddleName_textBox.Text = "";
            SpouseMotherSuffix_comboBox.Text = "";
            SpouseMotherPrefix_comboBox.Text = "";
            Buried_radioButton.Checked = true;
            Cremated_radioButton.Checked = false;
            Other_radioButton.Checked = false;
            Cemetery_comboBox.Text = "";
            LotNumber_textBox.Text = "";
            if (bDoNotSavePreviousRecordSettings(NewVitalRecordType))
            {
                Book_textBox.Text = "";
                Page_textBox.Text = "";
            }
            if (!m_UsePreviousRecordAsDefault ||
                NotBurialToDeathType(NewVitalRecordType))
            {
                DateYear_textBox.Text = "";
                DateMonth_textBox.Text = "";
                DateDay_textBox.Text = "";
            }
            if (!m_UsePreviousRecordAsDefault || 
                NewVitalRecordType != EVitalRecordType.eBurial)
            {
                Male_radioButton.Checked = false;
                Female_radioButton.Checked = false;
            }
            AgeYears_textBox.Text = "";
            AgeMonths_textBox.Text = "";
            AgeDays_textBox.Text = "";
            SpouseAgeYears_textBox.Text = "";
            SpouseAgeMonths_textBox.Text = "";
            SpouseAgeDays_textBox.Text = "";
            BornDateMonth_textBox.Text = "";
            BornDateDay_textBox.Text = "";
            BornDateYear_textBox.Text = "";
            SpouseBornDateMonth_textBox.Text = "";
            SpouseBornDateDay_textBox.Text = "";
            SpouseBornDateYear_textBox.Text = "";
            CalcDateYear_textBox.Text = "";
            CalcDateMonth_textBox.Text = "";
            CalcDateDay_textBox.Text = "";
            NameIntegrated_checkBox.Checked = false;
            FatherIntegrated_checkBox.Checked = false;
            MotherIntegrated_checkBox.Checked = false;
            SpouseIntegrated_checkBox.Checked = false;
            SpouseFatherIntegrated_checkBox.Checked = false;
            SpouseMotherIntegrated_checkBox.Checked = false;
        }
        //****************************************************************************************************************************
        private void SetVitalRecordDisplay()
        {
            Prefix_comboBox.Visible = true;
            Father_groupBox.Visible = true;
            Mother_groupBox.Visible = true;
            Sex_groupBox.Visible = false;

            Age_groupBox.Visible = true;
            Burial_groupBox.Visible = true;
            CalcBornDate_groupBox.Visible = true;
            setDisplayType();
        }
        //****************************************************************************************************************************
        private void setDisplayType()
        {
            switch (m_eVitalRecordType)
            {
                case EVitalRecordType.eBirthMale:
                case EVitalRecordType.eBirthFemale:
                    SetBirthRecordDisplay();
                    break;
                case EVitalRecordType.eDeathMale:
                case EVitalRecordType.eDeathFemale:
                    SetDeathRecordDisplay();
                    break;
                case EVitalRecordType.eCivilUnionPartyA:
                case EVitalRecordType.eCivilUnionPartyB:
                case EVitalRecordType.eMarriageBride:
                case EVitalRecordType.eMarriageGroom:
                    SetCivilUnionRecordDisplay();
                    break;
                case EVitalRecordType.eBurial:
                    Age_groupBox.Visible = true;
                    SetBurialRecordDisplay();
                    Age_groupBox.Visible = true;
                    break;
                default: break;
            }
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
        private void DisplayVitalRecord(DataRow row,
                                          DataRow Spouse_row)
        {
            m_eVitalRecordType = (EVitalRecordType) row[U.VitalRecordType_col].ToInt();
            InitializeFields();
            SetVitalRecordDisplay();
            LastName_textBox.Text = row[U.LastName_col].ToString();
            FirstName_textBox.Text = row[U.FirstName_col].ToString();
            MiddleName_textBox.Text = row[U.MiddleName_col].ToString();
            Suffix_comboBox.Text = row[U.Suffix_col].ToString();
            Prefix_comboBox.Text = row[U.Prefix_col].ToString();
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
            Book_textBox.Text = row[U.Book_col].ToString();
            Page_textBox.Text = row[U.Page_col].ToString();
            DateYear_textBox.Text = row[U.DateYear_col].ToString();
            DateMonth_textBox.Text = row[U.DateMonth_col].TwoCharNumber();
            DateDay_textBox.Text = row[U.DateDay_col].TwoCharNumber();
            DisplayAge(row, Spouse_row);
            string CalcBornDate = U.BornDateFromDiedDateMinusAge(row[U.DateYear_col].ToInt(),
                                   row[U.DateMonth_col].ToInt(),
                                   row[U.DateDay_col].ToInt(),
                                   row[U.AgeYears_col].ToInt(),
                                   row[U.AgeMonths_col].ToInt(),
                                   row[U.AgeDays_col].ToInt(), "");
            int iyear, imonth, iday;
            U.SplitDate(CalcBornDate, out iyear, out imonth, out iday);
            CalcDateYear_textBox.Text = iyear.ToString();
            CalcDateMonth_textBox.Text = imonth.ToString();
            CalcDateDay_textBox.Text = iday.ToString();
            m_sSex = row[U.Sex_col].ToChar();
            SetSexRadioButtons(m_sSex);
            Buried_radioButton.Checked = false;
            Cremated_radioButton.Checked = false;
            Other_radioButton.Checked = false;
            SetDispositionRadioButtons(row[U.Disposition_col].ToString()[0]);
            Cemetery_comboBox.Text = row[U.CemeteryName_col].ToString();
            LotNumber_textBox.Text = row[U.LotNumber_col].ToString();
            Notes_TextBox.Text = row[U.Notes_col].ToString();
            ExcludeFromSite_checkBox.Checked = row[U.ExcludeFromSite_col].ToBool();
            m_iPersonID = row[U.PersonID_col].ToInt();
            if (Spouse_row != null)
            {
                SpouseLastName_textBox.Text = Spouse_row[U.LastName_col].ToString();
                SpouseFirstName_textBox.Text = Spouse_row[U.FirstName_col].ToString();
                SpouseMiddleName_textBox.Text = Spouse_row[U.MiddleName_col].ToString();
                SpouseSuffix_comboBox.Text = Spouse_row[U.Suffix_col].ToString();
                SpousePrefix_comboBox.Text = Spouse_row[U.Prefix_col].ToString();
                SpouseFatherLastName_textBox.Text = Spouse_row[U.FatherLastName_col].ToString();
                SpouseFatherFirstName_textBox.Text = Spouse_row[U.FatherFirstName_col].ToString();
                SpouseFatherMiddleName_textBox.Text = Spouse_row[U.FatherMiddleName_col].ToString();
                SpouseFatherSuffix_comboBox.Text = Spouse_row[U.FatherSuffix_col].ToString();
                SpouseFatherPrefix_comboBox.Text = Spouse_row[U.FatherPrefix_col].ToString();
                SpouseMotherLastName_textBox.Text = Spouse_row[U.MotherLastName_col].ToString();
                SpouseMotherFirstName_textBox.Text = Spouse_row[U.MotherFirstName_col].ToString();
                SpouseMotherMiddleName_textBox.Text = Spouse_row[U.MotherMiddleName_col].ToString();
                SpouseMotherSuffix_comboBox.Text = Spouse_row[U.MotherSuffix_col].ToString();
                SpouseMotherPrefix_comboBox.Text = Spouse_row[U.MotherPrefix_col].ToString();
            }
        }
        //****************************************************************************************************************************
        private void DisplayAge(DataRow row,
                                DataRow Spouse_row)
        {
            AgeYears_textBox.Text = U.BlankIfZero(row[U.AgeYears_col]);
            AgeMonths_textBox.Text = U.BlankIfZero(row[U.AgeMonths_col]);
            AgeDays_textBox.Text = U.BlankIfZero(row[U.AgeDays_col]);
            int bornYear; int bornMonth; int bornDay;
            U.BornDateFromDiedDateMinusAge(row[U.DateYear_col].ToInt(),
                                           row[U.DateMonth_col].ToInt(),
                                           row[U.DateDay_col].ToInt(),
                                           row[U.AgeYears_col].ToInt(),
                                           row[U.AgeMonths_col].ToInt(),
                                           row[U.AgeDays_col].ToInt(),
                                           out bornYear, out bornMonth, out bornDay);
            if (bornYear == 0)
            {
                BornDateYear_textBox.Text = "";
                BornDateMonth_textBox.Text = "";
                BornDateDay_textBox.Text = "";
            }
            else
            {
                BornDateYear_textBox.Text = bornYear.ToString();
                BornDateMonth_textBox.Text = bornMonth.ToString();
                BornDateDay_textBox.Text = bornDay.ToString();
            }
            if (Spouse_row != null)
            {
                SpouseAgeYears_textBox.Text = U.BlankIfZero(Spouse_row[U.AgeYears_col]);
                SpouseAgeMonths_textBox.Text = U.BlankIfZero(Spouse_row[U.AgeMonths_col]);
                SpouseAgeDays_textBox.Text = U.BlankIfZero(Spouse_row[U.AgeDays_col]);
                U.BornDateFromDiedDateMinusAge(Spouse_row[U.DateYear_col].ToInt(),
                                               Spouse_row[U.DateMonth_col].ToInt(),
                                               Spouse_row[U.DateDay_col].ToInt(),
                                               Spouse_row[U.AgeYears_col].ToInt(),
                                               Spouse_row[U.AgeMonths_col].ToInt(),
                                               Spouse_row[U.AgeDays_col].ToInt(),
                                               out bornYear, out bornMonth, out bornDay);
                if (bornYear == 0)
                {
                    SpouseBornDateYear_textBox.Text = "";
                    SpouseBornDateMonth_textBox.Text = "";
                    SpouseBornDateDay_textBox.Text = "";
                }
                else
                {
                    SpouseBornDateYear_textBox.Text = bornYear.ToString();
                    SpouseBornDateMonth_textBox.Text = bornMonth.ToString();
                    SpouseBornDateDay_textBox.Text = bornDay.ToString();
                }
            }
        }
        //****************************************************************************************************************************
        private bool CheckDate(string dateMonth, string dateDay, string dateYear, bool validIfZero=false)
        {
            bool bSuccess = true;
            int iDay = dateDay.ToIntNoError();
            int iYear = dateYear.ToIntNoError();
            int iMonth = dateMonth.ToIntNoError();
            if (validIfZero && iMonth == 0 && iDay == 0 && iYear == 0)
            {
                return true;
            }
            if (iDay == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Date Day: " + dateDay);
                bSuccess = false;
                iDay = 1;
            }
            else
            if (iDay < 1)
            {
                MessageBox.Show("Invalid value for Date");
                bSuccess = false;
            }
            else
            if (iYear == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Date: " + dateYear);
                bSuccess = false;
                iYear = 2001;
            }
            else
            if (iYear < 1000)
            {
                MessageBox.Show("Value for Year cannot be prior to 1000");
                bSuccess = false;
            }
            else
            if (iYear > 2200)
            {
                MessageBox.Show("Date Cannot be greater than totays date");
                bSuccess = false;
            }
            else
            if (iMonth == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Date: " + dateMonth);
                bSuccess = false;
            }
            else
            if (iMonth > 12 || iMonth < 1)
            {
                MessageBox.Show("Invalid value for Month");
                bSuccess = false;
            }
            else
            if (iDay > U.LastDayOfMonth(iMonth, iYear))
            {
                MessageBox.Show("Day is greater than last day for month");
                bSuccess = false;
            }
            if (bSuccess)
            {
                DateTime dDate = new DateTime(iYear, iMonth, iDay);
                DateTime TodaysDate = System.DateTime.Now;
                if (dDate > TodaysDate)
                {
                    MessageBox.Show("Date Cannot be greater than totays date");
                    bSuccess = false;
                }
            }
            return bSuccess;
        }
        //****************************************************************************************************************************
        private bool CheckAge(int iYears, int iMonths, int iDays)
        {
            bool bSuccess = true;
            if (iYears == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Age Years: " + iYears.ToString());
                bSuccess = false;
            }
            else
            if (iYears < 0 || iYears > 120)
            {
                MessageBox.Show("Invalid Value For Age Years: " + iYears.ToString());
                bSuccess = false;
            }
            if (iMonths == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Age Months: " + iMonths.ToString());
                bSuccess = false;
            }
            else
            if (iMonths < 0 || iMonths > 12)
            {
                MessageBox.Show("Invalid Value For Age Months: " + iMonths.ToString());
                bSuccess = false;
            }
            if (iDays == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Age Days: " + iDays.ToString());
                bSuccess = false;
            }
            else
            if (iDays < 0 || iDays > 365)
            {
                MessageBox.Show("Invalid Value For Age Days: " + iDays.ToString());
                bSuccess = false;
            }
            return bSuccess;
        }
        private bool AbortIfAlreadyExistsAndModifiedWithoutSaving()
        {
            if (m_iVitalRecordID == 0)
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
            SQL.GetVitalRecordWithSpouseRecord(NewTbl, m_iVitalRecordID);
            if (NewTbl.Rows.Count == 0)
            {
                InitializeNewRecord();
            }
        }
        private void InitializeNewRecord()
        {
            m_iVitalRecordID = 0;
            m_VitalRecord_tbl.Clear();
            InitializeFields();
            SetToUnmodified();
        }
        //****************************************************************************************************************************
        private int GetVitalRecordFromGrid(DataTable tbl,
                                            string startingWith)
        {
            if (AbortIfAlreadyExistsAndModifiedWithoutSaving())
                return 0;
            CGridVitalRecord GridVitalRecord = new CGridVitalRecord(m_SQL, ref tbl);
            GridVitalRecord.SetStartingWith(startingWith);
            GridVitalRecord.ShowDialog();
            int iVitalRecordID = GridVitalRecord.SelectedVitalRecordID;
            if (iVitalRecordID != 0)
            {
                m_iVitalRecordID = iVitalRecordID;
                if (iVitalRecordID > 9900000)
                {
                    int cemeteryRecordId = iVitalRecordID - 9900000;
                    DisplayCemeteryRecord(cemeteryRecordId);
                }
                else
                {
                    DisplayVitalRecord(m_iVitalRecordID);
                }
            }
            else
            if (m_iVitalRecordID != 0)
            {
                LookToSeeIfTheRecordWasDeleted();
            }
            return iVitalRecordID;
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
            m_FatherFirstName = FatherFirstName_textBox.Text.TrimString();
            m_FatherMiddleName = FatherMiddleName_textBox.Text.TrimString();
            m_FatherLastName = FatherLastName_textBox.Text.TrimString();
            m_FatherSuffix = FatherSuffix_comboBox.Text.TrimString();
            m_FatherPrefix = FatherPrefix_comboBox.Text.TrimString();
            m_SpouseFirstName = SpouseFirstName_textBox.Text.TrimString();
            m_SpouseMiddleName = SpouseMiddleName_textBox.Text.TrimString();
            m_SpouseLastName = SpouseLastName_textBox.Text.TrimString();
            m_SpouseSuffix = SpouseSuffix_comboBox.Text.TrimString();
            m_SpousePrefix = SpousePrefix_comboBox.Text.TrimString();
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
            SetAllNameValues();
            SearchVitalRecords(startingPerson);
        }
        //****************************************************************************************************************************
        private bool ChangesNameOptionsForSearch()
        {
            if (FirstName_textBox.Modified || LastName_textBox.Modified)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private void SearchForPersonRecords(string sLastName)
        {
            DataTable tbl = SQL.DefinePersonTable();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PersonID_col] };
            sLastName = sLastName.TrimString();
            if (sLastName.Length == 0)
            {
                PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
                SQL.PersonsBasedOnNameOptions(tbl, false, U.Person_Table, U.PersonID_col, SearchBy, personName, "");
            }
            else
            {
                PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
                SQL.PersonsBasedOnNameOptions(tbl, true, U.Person_Table, U.PersonID_col, SearchBy, personName, "");
            }
            CGridPerson GridDataViewPerson;
            if (SearchBy == eSearchOption.SO_AllNames)
                GridDataViewPerson = new CGridPerson(m_SQL, ref tbl, false, 0, false, "Person");
            else
                GridDataViewPerson = new CGridPerson(m_SQL, ref tbl, false, "Person", m_LastName);
            GridDataViewPerson.ShowDialog();
            int iPersonID = GridDataViewPerson.SelectedPersonID;
            if (iPersonID != 0)
            {
                FPerson Person = new FPerson(m_SQL, iPersonID, false);
                Person.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private void SearchVitalRecords(string startingPerson)
        {
            m_bDidSearch = ChangesNameOptionsForSearch();
            DataTable tbl = SQL.DefineVitalRecord_Table();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.VitalRecordID_col] };
            if (SearchBy == eSearchOption.SO_ReturnToLast)
            {
                SearchBy = eSearchOption.SO_StartingWith;
                string lastName = m_LastName[0].ToString();
                PersonName personName = new PersonName("", "", lastName, "", "");
                SearchVitalRecords(tbl, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
            }
            else if (SearchBy == eSearchOption.SO_AllNames)
            {
                PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
                SearchVitalRecords(tbl, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
            }
            else if (SearchAllNames_radioButton.Checked)
            {
                PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
                SearchVitalRecords(tbl, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
                SearchVitalRecords(tbl, U.FirstName_col, U.MiddleName_col, U.FatherLastName_col, personName);
                SearchVitalRecords(tbl, U.FatherFirstName_col, U.FatherMiddleName_col, U.FatherLastName_col, personName);
                SearchVitalRecords(tbl, U.MotherFirstName_col, U.MotherMiddleName_col, U.MotherLastName_col, personName);
                SearchVitalRecords(tbl, U.MotherFirstName_col, U.MotherMiddleName_col, U.FatherLastName_col, personName);
            }
            else if (SearchAllNamesFather_radioButton.Checked)
            {
                PersonName personName = new PersonName(m_FatherFirstName, m_FatherMiddleName, m_FatherLastName, m_FatherSuffix, m_FatherPrefix);
                SearchVitalRecords(tbl, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
                SearchVitalRecords(tbl, U.FatherFirstName_col, U.FatherMiddleName_col, U.FatherLastName_col, personName);
                SearchVitalRecords(tbl, U.MotherFirstName_col, U.MotherMiddleName_col, U.MotherLastName_col, personName);
            }
            else if (SearchAllNamesSpouse_radioButton.Checked)
            {
                PersonName personName = new PersonName(m_SpouseFirstName, m_SpouseMiddleName, m_SpouseLastName, m_SpouseSuffix, m_SpousePrefix);
                SearchVitalRecords(tbl, U.FirstName_col, U.MiddleName_col, U.LastName_col, personName);
                SearchVitalRecords(tbl, U.FatherFirstName_col, U.FatherMiddleName_col, U.FatherLastName_col, personName);
                SearchVitalRecords(tbl, U.MotherFirstName_col, U.MotherMiddleName_col, U.MotherLastName_col, personName);
            }
            else
            {
                PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
                SQL.PersonsBasedOnNameOptions(tbl, false, U.VitalRecord_Table, U.VitalRecordID_col, SearchBy, personName, "");
            }
            int newPersonID = 0;
            if (tbl.Rows.Count == 0)
            {
                MessageBox.Show("No Records Found");
            }
            else
            {
                newPersonID = GetVitalRecordFromGrid(tbl, startingPerson);
            }
            if (newPersonID == 0 && m_bDidSearch)
            {
                string lastName = LastName_textBox.Text;
                string firstName = FirstName_textBox.Text;
                InitializeNewRecord();
                LastName_textBox.Text = lastName;
                FirstName_textBox.Text = firstName;
            }
        }
        //****************************************************************************************************************************
        private void SearchVitalRecords(DataTable tbl,
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
            PersonSearchTableAndColumns personSearchTableAndColumns = new PersonSearchTableAndColumns(U.VitalRecord_Table,
                              firstName_col, middleName_col, lastNameCol);
            SQL.PersonsBasedOnNameOptions(tbl, false, personSearchTableAndColumns, U.VitalRecordID_col, SearchBy, personName);
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
            SearchBy = eSearchOption.SO_StartingWith;
            SearchForPerson("", "");
        }
        //****************************************************************************************************************************
        private void SearchPartial_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson("", "");
        }
        //****************************************************************************************************************************
        private void SearchSimilar_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_Similar;
            SearchForPerson("", "");
        }
        //****************************************************************************************************************************
        private void LastMother_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(MotherLastName_textBox.Text.ToString(), "");
        }
        //****************************************************************************************************************************
        private void LastSpouse_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(SpouseLastName_textBox.Text.ToString(), "");
        }
        //****************************************************************************************************************************
        private void LastSpouseMother_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(SpouseMotherLastName_textBox.Text.ToString(), "");
        }
        //****************************************************************************************************************************
        private void InitializeNewVitalRecord(EVitalRecordType NewVitalRecordType)
        {
            m_VitalRecord_tbl.Clear();
            m_iVitalRecordID = 0;
/*            if (MessageBox.Show("Use Current Record As Default?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                m_UsePreviousRecordAsDefault = true;
                if (NewVitalRecordType == EVitalRecordType.eBurial && m_eVitalRecordType != EVitalRecordType.eBurial)
                {
                    eSex Sex = m_eVitalRecordType.RecordTypeSex();
                    Male_radioButton.Checked = Sex == eSex.eMale;
                    Female_radioButton.Checked = Sex == eSex.eFemale;
                }
                InitializeSpouseBurialDateAndIntegratedCheckboxes(NewVitalRecordType);
            }
            else*/
                InitializeFields();
            m_eVitalRecordType = NewVitalRecordType;
        }
        //****************************************************************************************************************************
        private void ResetScreen(int iVitalRecordID)
        {
            if (iVitalRecordID == 0)
            {
                m_iVitalRecordID = 0;
            }
            else
            {
                m_VitalRecord_tbl.Rows[0][U.VitalRecordType_col] = (int)m_eVitalRecordType;
            }
            SetVitalRecordDisplay();
            SetToUnmodified();
            LastName_textBox.Focus();
        }
        //****************************************************************************************************************************
        private void Changebutton_Click(object sender, System.EventArgs e)
        {
            if (SQL.ChangeToOppositeVitalRecordType(m_iVitalRecordID))
            {
                DisplayVitalRecord(m_iVitalRecordID);
            }
            else
            {
                MessageBox.Show("Unable to Change Vital Record Type");
            }
        }
        //****************************************************************************************************************************
        private void BirthMalebutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eBirthMale);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void BirthFemalebutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eBirthFemale);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void DeathMalebutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eDeathMale);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void DeathFemalebutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eDeathFemale);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void MarriageBridebutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eMarriageBride);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void MarriageGroombutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eMarriageGroom);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void CivilUnionPartyAbutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eCivilUnionPartyA);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void CivilUnionPartyBbutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eCivilUnionPartyB);
                ResetScreen(0);
            }
        }
        //****************************************************************************************************************************
        private void Burialbutton_Click(object sender, System.EventArgs e)
        {
            if (SaveIfDesired())
            {
                InitializeNewVitalRecord(EVitalRecordType.eBurial);
                ResetScreen(0);
            }
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
        private void InitializeRow(DataRow VitalRecordRow,
                                   DataRow Person_row)
        {
            VitalRecordRow[U.LastName_col] = Person_row[U.LastName_col];
            VitalRecordRow[U.FirstName_col] = Person_row[U.FirstName_col];
            VitalRecordRow[U.MiddleName_col] = Person_row[U.MiddleName_col];
            VitalRecordRow[U.Suffix_col] = Person_row[U.Suffix_col];
            VitalRecordRow[U.Prefix_col] = Person_row[U.Prefix_col];
            VitalRecordRow[U.FatherLastName_col] = "";
            VitalRecordRow[U.FatherFirstName_col] = "";
            VitalRecordRow[U.FatherMiddleName_col] = "";
            VitalRecordRow[U.FatherSuffix_col] = "";
            VitalRecordRow[U.FatherPrefix_col] = "";
            VitalRecordRow[U.MotherLastName_col] = "";
            VitalRecordRow[U.MotherFirstName_col] = "";
            VitalRecordRow[U.MotherMiddleName_col] = "";
            VitalRecordRow[U.MotherSuffix_col] = "";
            VitalRecordRow[U.MotherPrefix_col] = "";
            VitalRecordRow[U.SpouseID_col] = 0;
            VitalRecordRow[U.Book_col] = "";
            VitalRecordRow[U.Page_col] = "";
            VitalRecordRow[U.DateYear_col] = 0;
            VitalRecordRow[U.DateMonth_col] = 0;
            VitalRecordRow[U.DateDay_col] = 0;
            VitalRecordRow[U.AgeYears_col] = 0;
            VitalRecordRow[U.AgeMonths_col] = 0;
            VitalRecordRow[U.AgeDays_col] = 0;
            VitalRecordRow[U.Disposition_col] = ' ';
            VitalRecordRow[U.CemeteryName_col] = "";
            VitalRecordRow[U.LotNumber_col] = "";
            VitalRecordRow[U.Notes_col] = "";
            VitalRecordRow[U.PersonID_col] = 0;
            VitalRecordRow[U.FatherID_col] = 0;
            VitalRecordRow[U.MotherID_col] = 0;
            VitalRecordRow[U.ExcludeFromSite_col] = 0;
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
        private DataRow SpouseRow(DataTable VitalRecord_tbl,
                                  int iPersonID,
                                  int iSpouseID)
        {
            if (iSpouseID == 0)
                return null;
            foreach (DataRow row in VitalRecord_tbl.Rows)
            {
                if (row[U.VitalRecordID_col].ToInt() == iSpouseID && row[U.SpouseID_col].ToInt() == iPersonID)
                    return row;
            }
            DataTable tbl = new DataTable();
            SQL.GetVitalRecordsForPerson(tbl, iSpouseID, U.VitalRecordID_col);
            if (tbl.Rows.Count == 0)
                return null;
            else
                return tbl.Rows[0];
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
                PersonIntegrated(FatherIntegrated_checkBox.Checked, FatherLastName_textBox.Text.ToString(), "") &&
                PersonIntegrated(MotherIntegrated_checkBox.Checked, MotherLastName_textBox.Text.ToString(), FatherLastName_textBox.Text.ToString()))
            {
                if (!m_eVitalRecordType.MarriageRecord())
                {
                    return true;
                }
                if (PersonIntegrated(SpouseIntegrated_checkBox.Checked, SpouseLastName_textBox.Text.ToString(), "") &&
                    PersonIntegrated(SpouseFatherIntegrated_checkBox.Checked, SpouseFatherLastName_textBox.Text.ToString(), "") &&
                    PersonIntegrated(SpouseMotherIntegrated_checkBox.Checked, SpouseMotherLastName_textBox.Text.ToString(), SpouseFatherLastName_textBox.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private void Integratebutton_Click(object sender, System.EventArgs e)
        {
            if (VitalRecordChanged())
            {
                if (!SaveVitalRecord())
                {
                    MessageBox.Show("Integration Unsuccesful");
                    return;
                }
            }
            if (m_VitalRecord_tbl.Rows.Count == 0)
                return;
            if (RecordAlreadyIntegrated())
            {
                MessageBox.Show("This Person Has Already Been Integrated");
                return;
            }
            DataRow VitalRecord_row = m_VitalRecord_tbl.Rows[0];
            DataRow SpouseVitalRecord_row = SpouseRow(m_VitalRecord_tbl, VitalRecord_row[U.VitalRecordID_col].ToInt(), VitalRecord_row[U.SpouseID_col].ToInt());
            CIntegrateVitalRecord IntegrateVitalRecord = new CIntegrateVitalRecord(m_SQL, true);
            if (IntegrateVitalRecord.IntegrateRecord(VitalRecord_row, SpouseVitalRecord_row, NameIntegrated_checkBox.Checked, FatherIntegrated_checkBox.Checked, MotherIntegrated_checkBox.Checked,
                                      SpouseIntegrated_checkBox.Checked, SpouseFatherIntegrated_checkBox.Checked, SpouseMotherIntegrated_checkBox.Checked))
            {
                if (!SQL.SaveIntegratedVitalRecords(m_VitalRecord_tbl))
                {
                    MessageBox.Show("Integrate Unsuccesful");
                }
                m_iPersonID = m_VitalRecord_tbl.Rows[0][U.PersonID_col].ToInt();
                if (m_iVitalRecordID > 0)
                {
                    SetIntegratedChecked(m_VitalRecord_tbl);
                }
            }
        }
        //****************************************************************************************************************************
        private void SetIntegratedChecked(DataTable VitalRecords_tbl)
        {
            NameIntegrated_checkBox.Checked   = VitalRecords_tbl.Rows[0][U.PersonID_col].ToInt() != 0;
            FatherIntegrated_checkBox.Checked = VitalRecords_tbl.Rows[0][U.FatherID_col].ToInt() != 0;
            MotherIntegrated_checkBox.Checked = VitalRecords_tbl.Rows[0][U.MotherID_col].ToInt() != 0;
            if (VitalRecords_tbl.Rows.Count > 1)
            {
                SpouseIntegrated_checkBox.Checked = VitalRecords_tbl.Rows[1][U.PersonID_col].ToInt() != 0;
                SpouseFatherIntegrated_checkBox.Checked = VitalRecords_tbl.Rows[1][U.FatherID_col].ToInt() != 0;
                SpouseMotherIntegrated_checkBox.Checked = VitalRecords_tbl.Rows[1][U.MotherID_col].ToInt() != 0;
            }
        }
        //****************************************************************************************************************************
        private void SetToUnmodified()
        {
            m_bDidSearch = false;
            m_sSex = SetSex(m_eVitalRecordType);
            SearchBy = eSearchOption.SO_None;
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
            SpouseFatherFirstName_textBox.Modified = false;
            SpouseFatherMiddleName_textBox.Modified = false;
            SpouseFatherLastName_textBox.Modified = false;
            SpouseMotherFirstName_textBox.Modified = false;
            SpouseMotherMiddleName_textBox.Modified = false;
            SpouseMotherLastName_textBox.Modified = false;
            DateMonth_textBox.Modified = false;
            DateDay_textBox.Modified = false;
            DateYear_textBox.Modified = false;
            AgeYears_textBox.Modified = false;
            AgeMonths_textBox.Modified = false;
            AgeDays_textBox.Modified = false;
            SpouseAgeYears_textBox.Modified = false;
            SpouseAgeMonths_textBox.Modified = false;
            SpouseAgeDays_textBox.Modified = false;
            Book_textBox.Modified = false;
            Page_textBox.Modified = false;
            LotNumber_textBox.Modified = false;
            Notes_TextBox.Modified = false;
            BornDateMonth_textBox.Modified = false;
            BornDateDay_textBox.Modified = false;
            BornDateYear_textBox.Modified = false;
            SpouseBornDateMonth_textBox.Modified = false;
            SpouseBornDateDay_textBox.Modified = false;
            SpouseBornDateYear_textBox.Modified = false;
            ExcludeFromSiteCheckedChanged = false;
        }
        //****************************************************************************************************************************
        private int NewIntegrationID(string sMessageName,
                                     int    iPreviousPersonID)
        {
            string sMessage = sMessageName + " name changed whose similar names do not include the Previously integrated Person. \n" +
                              "Do you wish to change the name and reintegrate?";
            MessageBoxButtons mb = MessageBoxButtons.OKCancel;
            switch (MessageBox.Show(sMessage, "", mb))
            {
                case DialogResult.OK:
                    return 0;
                case DialogResult.Cancel:
                default:
                    return U.Exception;
            }
        }
        //****************************************************************************************************************************
        private void CheckLastNameAgainstRecord(PersonName personName,
                                               string sMessageName)
        {
            if (personName.integratedPersonID == 0)
                return;
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
        }
        //****************************************************************************************************************************
        private bool CheckBookPage(string sBook,
                                   string sPage)
        {
            if (sBook.Length == 0 || sPage.Length == 0)
            {
                MessageBox.Show("You must include a book and page");
                return false;
            }
            else
                return true;
        }
        //****************************************************************************************************************************
        private bool IllegalName(PersonName personName,
                                string sMarriedName,
                                string  sMessageName,
                                bool    bCheckFirstNames)
        {
            if (bCheckFirstNames && personName.firstName.Length == 0 && personName.middleName.Length == 0 &&
                personName.prefix.Length == 0 && personName.suffix.Length == 0)
            {
                return false;
            }
            else if (personName.lastName.Length == 0 && sMarriedName.Length == 0)
            {
                MessageBox.Show("You must have a value for the last name of " + sMessageName);
                return true;
            }
            else
            {
                CheckLastNameAgainstRecord(personName, sMessageName);
                return false;
            }
        }
        //****************************************************************************************************************************
        private bool CheckLastNames(DataTable VitalRecord_tbl,
                                    PersonSpouseWithParents personSpouseWithParents)
        {
            string sRecordTypeTitle = m_eVitalRecordType.RecordTypeTitle();
            if (IllegalName(personSpouseWithParents.PersonName, "", sRecordTypeTitle, false))
            {
                return false;
            }
            if (IllegalName(personSpouseWithParents.FatherName, "", sRecordTypeTitle + "'s Father", true))
            {
                return false;
            }
            if (IllegalName(personSpouseWithParents.MotherName, personSpouseWithParents.FatherName.lastName, sRecordTypeTitle + "'s Mother", true))
            {
                return false;
            }
            if (!m_eVitalRecordType.MarriageRecord() || m_VitalRecord_tbl.Rows.Count < 2) 
                return true;
            sRecordTypeTitle = m_eVitalRecordType.SpouseRecordType().RecordTypeTitle();
            if (IllegalName(personSpouseWithParents.SpouseName, "", sRecordTypeTitle, false))
            {
                return false;
            }
            if (IllegalName(personSpouseWithParents.SpouseFatherName, "", sRecordTypeTitle + "'s Father", true))
            {
                return false;
            }
            if (IllegalName(personSpouseWithParents.SpouseMotherName, personSpouseWithParents.SpouseFatherName.lastName, 
                            sRecordTypeTitle + "'s Mother", true))
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
        private char SetPartnerSex(EVitalRecordType eVitalRecordType)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eMarriageBride: return SetSex(EVitalRecordType.eMarriageGroom);
                case EVitalRecordType.eMarriageGroom: return SetSex(EVitalRecordType.eMarriageBride);
                case EVitalRecordType.eCivilUnionPartyA: return SetSex(EVitalRecordType.eCivilUnionPartyA);
                case EVitalRecordType.eCivilUnionPartyB: return SetSex(EVitalRecordType.eCivilUnionPartyB);
                default: return ' ';
            }
        }
        //****************************************************************************************************************************
        private char SetSex(EVitalRecordType eVitalRecordType)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eCivilUnionPartyA:
                case EVitalRecordType.eCivilUnionPartyB:
                case EVitalRecordType.eBurial:
                {
                    if (Male_radioButton.Checked)
                        return 'M';
                    else
                        return 'F';
                }
                case EVitalRecordType.eBirthMale:
                case EVitalRecordType.eDeathMale:
                case EVitalRecordType.eMarriageGroom: return 'M';
                case EVitalRecordType.eBirthFemale:
                case EVitalRecordType.eDeathFemale:
                case EVitalRecordType.eMarriageBride: return 'F';
                default: return ' ';
            };
        }
        //****************************************************************************************************************************
        private bool CheckSex(PersonSpouseWithParents personSpouseWithParents)
        {
            char personSex = SexCharFromRadioButtons();
            if (m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyA ||
                m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyB ||
                m_eVitalRecordType == EVitalRecordType.eBurial)
            {
                if (personSex == ' ')
                {
                    MessageBox.Show("You must Choose Sex before Saving");
                    return false;
                }
                return true;
            }
            if (CheckSexOfPerson(personSpouseWithParents.PersonWithParents, m_eVitalRecordType.RecordTypeSexChar(' ')))
            {
                if (m_eVitalRecordType.MarriageRecord())
                {
                    EVitalRecordType spouseVitalRecordType = m_eVitalRecordType.OppositeRecordType();
                    return (CheckSexOfPerson(personSpouseWithParents.SpouseWithParents, spouseVitalRecordType.RecordTypeSexChar(' ')));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        //****************************************************************************************************************************
        private bool CheckSexOfPerson(PersonWithParents personWithParents,
                                      char vitalRecordSex)
        {
            char sex = SQL.GetFirstNameSex(personWithParents.PersonName.firstName);
            if (sex != 'B' && sex != ' ' && sex != vitalRecordSex)
            {
                string message = "The sex of the firstname does not match the sex type of this vital record." + UU.crlf() +
                                 "Do you wish to change the vital record type before saving?";
                switch (MessageBox.Show(message, "", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        ChangeToOppositeRecordType(personWithParents);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return false;
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        private void ChangeToOppositeRecordType(PersonWithParents personWithParents)
        {
            m_eVitalRecordType = m_eVitalRecordType.OppositeRecordType();
            personWithParents.VitalRecordType = m_eVitalRecordType;
            if (m_eVitalRecordType.MarriageRecord())
            {
                personWithParents.VitalRecordType = m_eVitalRecordType.OppositeRecordType();
            }
            setDisplayType();
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
        public void SetLastNameToBlankIfNotValidFirstName(EVitalRecordType eVitalRecordType,
                                                          PersonName personName,
                                                          PersonName fatherName)
        {
            if (fatherName.firstName.Length == 0)
            {
                if (FathersLastNameIsMaidenName(eVitalRecordType, personName, fatherName))
                {
                    fatherName.firstName = U.maidenNameConstant;
                }
                else
                {
                    fatherName.lastName = "";
                }
            }
        }
        //****************************************************************************************************************************
        private bool FathersLastNameIsMaidenName(EVitalRecordType eVitalRecordType,
                                                 PersonName personName,
                                                 PersonName fatherName)
        {
            if (NotDeathOrBride(eVitalRecordType))
            {
                return false;
            }
            if (fatherName.lastName == personName.lastName)
            {
                return false;
            }
            if (fatherName.lastName.Length == 0)
            {
                return false;
            }
            if (fatherName.firstName.ToLower() == U.Unknown || fatherName.firstName.Length == 0)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool NotDeathOrBride(EVitalRecordType eVitalRecordType)
        {
            return (eVitalRecordType != EVitalRecordType.eDeathFemale &&
                    eVitalRecordType != EVitalRecordType.eMarriageBride);
        }
        //****************************************************************************************************************************
        private PersonWithParents SetPersonWithParents()
        {
            int iIntegratedPersonID = 0;
            int iIntegratedFatherID = 0;
            int iIntegratedMotherID = 0;
            if (m_VitalRecord_tbl.Rows.Count != 0)
            {
                DataRow PersonRow = m_VitalRecord_tbl.Rows[0];
                iIntegratedPersonID = PersonRow[U.PersonID_col].ToInt();
                iIntegratedFatherID = PersonRow[U.FatherID_col].ToInt();
                iIntegratedMotherID = PersonRow[U.MotherID_col].ToInt();
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
            SetLastNameToBlankIfNotValidFirstName(m_eVitalRecordType, personName, fatherName);
            PersonName motherName = new PersonName(MotherFirstName_textBox.Text.SetNameForDatabase(),
                                                   MotherMiddleName_textBox.Text.SetNameForDatabase(),
                                                   MotherLastName_textBox.Text.SetNameForDatabase(),
                                                   MotherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                                   MotherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedMotherID);
            return new PersonWithParents(m_eVitalRecordType, personName, fatherName, motherName);
        }
        //****************************************************************************************************************************
        private bool TheIsASpouseRecord()
        {
            return (m_VitalRecord_tbl.Rows.Count > 1);
        }
        //****************************************************************************************************************************
        private PersonWithParents SetSpouseWithParents()
        {
            int iIntegratedPersonID = 0;
            int iIntegratedFatherID = 0;
            int iIntegratedMotherID = 0;
            if (TheIsASpouseRecord())
            {
                DataRow PersonRow = m_VitalRecord_tbl.Rows[1];
                iIntegratedPersonID = PersonRow[U.PersonID_col].ToInt();
                iIntegratedFatherID = PersonRow[U.FatherID_col].ToInt();
                iIntegratedMotherID = PersonRow[U.MotherID_col].ToInt();
            }
            PersonName personName = new PersonName(SpouseFirstName_textBox.Text.SetNameForDatabase(),
                                                   SpouseMiddleName_textBox.Text.SetNameForDatabase(),
                                                   SpouseLastName_textBox.Text.SetNameForDatabase(),
                                                   SpouseSuffix_comboBox.Text.SetPrefixForDatabase(),
                                                   SpousePrefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedPersonID);
            PersonName fatherName = new PersonName(SpouseFatherFirstName_textBox.Text.SetNameForDatabase(),
                                                   SpouseFatherMiddleName_textBox.Text.SetNameForDatabase(),
                                                   SpouseFatherLastName_textBox.Text.SetNameForDatabase(),
                                                   SpouseFatherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                                   SpouseFatherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedFatherID);
            SetLastNameToBlankIfNotValidFirstName(U.SpouseRecordType(m_eVitalRecordType), personName, fatherName);
            PersonName motherName = new PersonName(SpouseMotherFirstName_textBox.Text.SetNameForDatabase(),
                                                   SpouseMotherMiddleName_textBox.Text.SetNameForDatabase(),
                                                   SpouseMotherLastName_textBox.Text.SetNameForDatabase(),
                                                   SpouseMotherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                                   SpouseMotherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                                   iIntegratedMotherID);
            return new PersonWithParents(m_eVitalRecordType.SpouseRecordType(), personName, fatherName, motherName);
        }
        //****************************************************************************************************************************
        private PersonSpouseWithParents SetPersonSpouseWithParents()
        {
            PersonWithParents personWithParents = SetPersonWithParents();
            if (!m_eVitalRecordType.MarriageRecord())
            {
                return new PersonSpouseWithParents(personWithParents, null);
            }
            return new PersonSpouseWithParents(personWithParents, SetSpouseWithParents());
        }
        //****************************************************************************************************************************
        private VitalRecordProperties SetVitalRecordProperties()
        {
            VitalRecordProperties vitalRecordProperties = new VitalRecordProperties(
                Book_textBox.Text.ToUpper().TrimString(),
                Page_textBox.Text.TrimString(),
                DateYear_textBox.Text.ToInt(),
                DateMonth_textBox.Text.ToInt(),
                DateDay_textBox.Text.ToInt(),
                SetDisposition(),
                Cemetery_comboBox.Text.TrimString(),
                LotNumber_textBox.Text.TrimString(),
                Notes_TextBox.Text.TrimString(),
                AgeYears_textBox.Text.ToInt(),
                AgeMonths_textBox.Text.ToInt(),
                AgeDays_textBox.Text.ToInt(),
                SetSex(m_eVitalRecordType),
                SpouseAgeYears_textBox.Text.ToInt(),
                SpouseAgeMonths_textBox.Text.ToInt(),
                SpouseAgeDays_textBox.Text.ToInt(),
                SetPartnerSex(m_eVitalRecordType),
                ExcludeFromSite_checkBox.Checked);
            return vitalRecordProperties;
        }
        //****************************************************************************************************************************
        private bool HandleErrorCode(ErrorCodes errorCode)
        {
            switch (errorCode)
            {
                case ErrorCodes.eSuccess:
                    m_iVitalRecordID = m_VitalRecord_tbl.Rows[0][U.VitalRecordID_col].ToInt();
                    SetToUnmodified();
                    SetAllNameValues();
                    SetIntegratedChecked(m_VitalRecord_tbl);
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
                                  VitalRecordProperties vitalRecordProperties)
        {
            if (!VitalRecordChanged())
                return true;
            if (!CheckDate(DateMonth_textBox.Text, DateDay_textBox.Text, DateYear_textBox.Text))
                return true;
            if (!String.IsNullOrEmpty(BornDateYear_textBox.Text) && !CheckDate(BornDateMonth_textBox.Text, BornDateDay_textBox.Text, BornDateYear_textBox.Text))
                return true;
            if (!String.IsNullOrEmpty(SpouseBornDateMonth_textBox.Text) && !CheckDate(SpouseBornDateMonth_textBox.Text, SpouseBornDateDay_textBox.Text, SpouseBornDateYear_textBox.Text))
                return true;
            if (!CheckAge(AgeYears_textBox.Text.ToIntNoError(), AgeMonths_textBox.Text.ToIntNoError(), AgeDays_textBox.Text.ToIntNoError()))
                return true;
            if (m_eVitalRecordType.IsMarriageRecord() &&
                !CheckAge(SpouseAgeYears_textBox.Text.ToIntNoError(), SpouseAgeMonths_textBox.Text.ToIntNoError(), SpouseAgeDays_textBox.Text.ToIntNoError()))
                return true;
            if (!CheckBookPage(Book_textBox.Text.TrimString(), Page_textBox.Text.TrimString()))
                return true;
            if (!CheckSex(personSpouseWithParents))
            {
                return true;
            }
            else
            {
                vitalRecordProperties.Sex = personSpouseWithParents.PersonWithParents.VitalRecordType.RecordTypeSexChar(vitalRecordProperties.Sex);
            }
            //if (!CheckLastNames(m_VitalRecord_tbl, personSpouseWithParents))
            //    return true;
            if (m_iVitalRecordID != 0)
                return false;
            if (ThisRecordAlreadyExits(m_VitalRecord_tbl, personSpouseWithParents, vitalRecordProperties))
            {
                MessageBox.Show("This record has already been entered");
                SetToUnmodified();
                return true;
            }
            PersonSpouseWithParents personSpouseWithParentsNoMiddleSuffixPrefix = new PersonSpouseWithParents(personSpouseWithParents);
            personSpouseWithParentsNoMiddleSuffixPrefix.RemoveMiddleSuffixPrefix();
            if (ThisRecordAlreadyExitsNoMiddleSuffixPrefix(m_VitalRecord_tbl, personSpouseWithParentsNoMiddleSuffixPrefix, vitalRecordProperties))
            {
                if (MessageBox.Show("This record may have already been entered.  Save Anyway","", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool NameChanged(DataRow VitalRecord_row,
                                 PersonName personName)
        {
            if (VitalRecord_row[U.FirstName_col].ToString() != personName.firstName ||
                VitalRecord_row[U.MiddleName_col].ToString() != personName.middleName ||
                VitalRecord_row[U.LastName_col].ToString() != personName.lastName ||
                VitalRecord_row[U.Suffix_col].ToString() != personName.suffix ||
                VitalRecord_row[U.Prefix_col].ToString() != personName.prefix)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool RecordAlreadyExitsNoMiddleSuffixPrefix(EVitalRecordType VitalRecordType,
                                        PersonSpouseWithParents personSpouseWithParents,
                                        VitalRecordProperties vitalRecordProperties)
        {
            DataTable tbl = new DataTable();
            SQL.GetVitalRecordFromNameNoMiddleSuffixPrefixDate(tbl, VitalRecordType, personSpouseWithParents.PersonName, vitalRecordProperties);
            if (tbl.Rows.Count == 0)
                return false;
            if (!VitalRecordType.MarriageRecord())
                return true;
            tbl.Clear();
            SQL.GetVitalRecordFromNameNoMiddleSuffixPrefixDate(tbl, VitalRecordType.SpouseRecordType(),
                                                       personSpouseWithParents.SpouseName, vitalRecordProperties);
            if (tbl.Rows.Count == 0)
                return false;
            else
                return true;
        }
        //****************************************************************************************************************************
        private bool RecordAlreadyExits(EVitalRecordType VitalRecordType,
                                        PersonSpouseWithParents personSpouseWithParents,
                                        VitalRecordProperties vitalRecordProperties)
        {
            DataTable tbl = new DataTable();
            SQL.GetVitalRecordFromNameDate(tbl, VitalRecordType, personSpouseWithParents.PersonName, vitalRecordProperties);
            if (tbl.Rows.Count == 0)
                return false;
            if (!VitalRecordType.MarriageRecord())
                return true;
            tbl.Clear();
            SQL.GetVitalRecordFromNameDate(tbl, VitalRecordType.SpouseRecordType(), 
                                                       personSpouseWithParents.SpouseName, vitalRecordProperties);
            if (tbl.Rows.Count == 0)
                return false;
            else
                return true;
        }
        //****************************************************************************************************************************
        private bool ThisRecordAlreadyExitsNoMiddleSuffixPrefix(DataTable VitalRecord_tbl,
                                            PersonSpouseWithParents personSpouseWithParents,
                                            VitalRecordProperties vitalRecordProperties)
        {
            EVitalRecordType vitalRecordType = personSpouseWithParents.VitalRecordType;
            if (RecordAlreadyExitsNoMiddleSuffixPrefix(vitalRecordType, personSpouseWithParents, vitalRecordProperties))
            {
                return true;
            }
            EVitalRecordType oppositeVitalRecordType = vitalRecordType.OppositeRecordType();
            if (oppositeVitalRecordType == EVitalRecordType.eSearch)
                return false;
            if (RecordAlreadyExitsNoMiddleSuffixPrefix(oppositeVitalRecordType, personSpouseWithParents, vitalRecordProperties))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool ThisRecordAlreadyExits(DataTable VitalRecord_tbl,
                                            PersonSpouseWithParents personSpouseWithParents,
                                            VitalRecordProperties vitalRecordProperties)
        {
            EVitalRecordType vitalRecordType = personSpouseWithParents.VitalRecordType;
            if (RecordAlreadyExits(vitalRecordType, personSpouseWithParents, vitalRecordProperties))
            {
                return true;
            }
            EVitalRecordType oppositeVitalRecordType = vitalRecordType.OppositeRecordType();
            if (oppositeVitalRecordType == EVitalRecordType.eSearch)
                return false;
            if (RecordAlreadyExits(oppositeVitalRecordType, personSpouseWithParents, vitalRecordProperties))
            {
                return true;
            }
            return false;
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
                                                        char vitalRecordSex)
        {
            eSex Sex = VitalRecordSex(m_eVitalRecordType, vitalRecordSex);
            UpdateFirstNameTableForPerson(personSpouseWithParents.PersonName, GetSex(Sex));
            UpdateFirstNameTableForPerson(personSpouseWithParents.FatherName, "M");
            UpdateFirstNameTableForPerson(personSpouseWithParents.MotherName, "F");
            if (m_eVitalRecordType.MarriageRecord())
            {
                eSex SpouseSex = VitalRecordSex(m_eVitalRecordType.SpouseRecordType(), vitalRecordSex);
                UpdateFirstNameTableForPerson(personSpouseWithParents.SpouseName, GetSex(SpouseSex));
                UpdateFirstNameTableForPerson(personSpouseWithParents.SpouseFatherName, "M");
                UpdateFirstNameTableForPerson(personSpouseWithParents.SpouseMotherName, "F");
            }
        }
        private eSex VitalRecordSex(EVitalRecordType vitalRecordType,
                                    char vitalRecordSex)
        {
            eSex Sex = vitalRecordType.RecordTypeSex();
            if (Sex == eSex.eUnknown)
            {
                if (vitalRecordSex == 'F')
                    return eSex.eFemale;
                else
                    return eSex.eMale;
            }
            return Sex;
        }
        //****************************************************************************************************************************
        private ArrayList UpdateVitalRecordInDataTable(PersonSpouseWithParents personSpouseWithParents,
                                                  VitalRecordProperties vitalRecordProperties)
        {
            DataRow person_row = m_VitalRecord_tbl.Rows[0];
            bool isMarriageRecord = TheIsASpouseRecord();
            if (isMarriageRecord)
            {
                vitalRecordProperties.ChangeDateToAge(person_row, BornDateMonth_textBox.Text.ToInt(), BornDateDay_textBox.Text.ToInt(), BornDateYear_textBox.Text.ToInt(), false);
            }
            ArrayList FieldsModified = new ArrayList();
            AddToFieldsModifiedIfChanged(FieldsModified, person_row, personSpouseWithParents.PersonName, U.FirstName_col, 
                                         U.MiddleName_col, U.LastName_col, U.Suffix_col, U.Prefix_col);
            AddToFieldsModifiedIfChanged(FieldsModified, person_row, personSpouseWithParents.FatherName, U.FatherFirstName_col, 
                                         U.FatherMiddleName_col, U.FatherLastName_col, U.FatherSuffix_col, U.FatherPrefix_col);
            AddToFieldsModifiedIfChanged(FieldsModified, person_row, personSpouseWithParents.MotherName, U.MotherFirstName_col,
                                         U.MotherMiddleName_col, U.MotherLastName_col, U.MotherSuffix_col, U.MotherPrefix_col);
            AddToFieldsModifiedIfChanged(FieldsModified, person_row, vitalRecordProperties, false);
            if (isMarriageRecord)
            {
                DataRow spouse_row = m_VitalRecord_tbl.Rows[1];
                vitalRecordProperties.ChangeDateToAge(spouse_row, SpouseBornDateMonth_textBox.Text.ToInt(), SpouseBornDateDay_textBox.Text.ToInt(), SpouseBornDateYear_textBox.Text.ToInt(), true);
                AddToFieldsModifiedIfChanged(FieldsModified, spouse_row, personSpouseWithParents.SpouseName, U.FirstName_col,
                                             U.MiddleName_col, U.LastName_col, U.Suffix_col, U.Prefix_col);
                AddToFieldsModifiedIfChanged(FieldsModified, spouse_row, personSpouseWithParents.SpouseFatherName, U.FatherFirstName_col,
                                             U.FatherMiddleName_col, U.FatherLastName_col, U.FatherSuffix_col, U.FatherPrefix_col);
                AddToFieldsModifiedIfChanged(FieldsModified, spouse_row, personSpouseWithParents.SpouseMotherName, U.MotherFirstName_col,
                                             U.MotherMiddleName_col, U.MotherLastName_col, U.MotherSuffix_col, U.MotherPrefix_col);
                AddToFieldsModifiedIfChanged(FieldsModified, spouse_row, vitalRecordProperties, true);
            }
            return FieldsModified;
        }
        //****************************************************************************************************************************
        private void AddToFieldsModifiedIfChanged(ArrayList FieldsModified,
                                                  DataRow row,
                                                  VitalRecordProperties vitalRecordProperties,
                                                  bool isSpouse)
        {
            DataColumnCollection columns = m_VitalRecord_tbl.Columns;
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Book_col, vitalRecordProperties.Book);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Page_col, vitalRecordProperties.Page);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.DateYear_col, vitalRecordProperties.DateYear);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.DateMonth_col, vitalRecordProperties.DateMonth);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.DateDay_col, vitalRecordProperties.DateDay);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.Disposition_col, vitalRecordProperties.Disposition);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.CemeteryName_col, vitalRecordProperties.CemeteryName);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.LotNumber_col, vitalRecordProperties.LotNumber);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, U.Notes_col, vitalRecordProperties.Notes);
            U.SetToNewValueIfDifferent(FieldsModified, row, U.ExcludeFromSite_col, vitalRecordProperties.ExcludeFromSite);
            if (isSpouse)
            {
                U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeYears_col, vitalRecordProperties.SpouseAgeYears);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeMonths_col, vitalRecordProperties.SpouseAgeMonths);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeDays_col, vitalRecordProperties.SpouseAgeDays);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Sex_col, vitalRecordProperties.SpouseSex);
            }
            else
            {
                U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeYears_col, vitalRecordProperties.AgeYears);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeMonths_col, vitalRecordProperties.AgeMonths);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.AgeDays_col, vitalRecordProperties.AgeDays);
                U.SetToNewValueIfDifferent(FieldsModified, row, U.Sex_col, vitalRecordProperties.Sex);
            }
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
            DataColumnCollection columns = m_VitalRecord_tbl.Columns;
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, firstName_col, personName.firstName);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, middleName_col, personName.middleName);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, lastName_col, personName.lastName);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, suffix_col, personName.suffix);
            U.SetToNewValueIfDifferent(FieldsModified, columns, row, prefix_col, personName.prefix);
        }
        //****************************************************************************************************************************
        private void AddVitalRecordToDataRow(DataTable vitalRecord_tbl,
                                             PersonWithParents personWithParents,
                                             VitalRecordProperties vitalRecordProperties,
                                             bool isSpouse)
        {
            DataRow VitalRecord_row = vitalRecord_tbl.NewRow();
            VitalRecord_row[U.VitalRecordType_col] = (int)personWithParents.VitalRecordType;
            VitalRecord_row[U.FirstName_col] = personWithParents.PersonName.firstName;
            VitalRecord_row[U.MiddleName_col] = personWithParents.PersonName.middleName;
            VitalRecord_row[U.LastName_col] = personWithParents.PersonName.lastName;
            VitalRecord_row[U.Suffix_col] = personWithParents.PersonName.suffix;
            VitalRecord_row[U.Prefix_col] = personWithParents.PersonName.prefix;
            VitalRecord_row[U.FatherFirstName_col] = personWithParents.FatherName.firstName;
            VitalRecord_row[U.FatherMiddleName_col] = personWithParents.FatherName.middleName;
            VitalRecord_row[U.FatherLastName_col] = personWithParents.FatherName.lastName;
            VitalRecord_row[U.FatherSuffix_col] = personWithParents.FatherName.suffix;
            VitalRecord_row[U.FatherPrefix_col] = personWithParents.FatherName.prefix;
            VitalRecord_row[U.MotherFirstName_col] = personWithParents.MotherName.firstName;
            VitalRecord_row[U.MotherMiddleName_col] = personWithParents.MotherName.middleName;
            VitalRecord_row[U.MotherLastName_col] = personWithParents.MotherName.lastName;
            VitalRecord_row[U.MotherSuffix_col] = personWithParents.MotherName.suffix;
            VitalRecord_row[U.MotherPrefix_col] = personWithParents.MotherName.prefix;
            VitalRecord_row[U.SpouseID_col] = 0;
            VitalRecord_row[U.Book_col] = vitalRecordProperties.Book;
            VitalRecord_row[U.Page_col] = vitalRecordProperties.Page;
            VitalRecord_row[U.DateYear_col] = vitalRecordProperties.DateYear;
            VitalRecord_row[U.DateMonth_col] = vitalRecordProperties.DateMonth;
            VitalRecord_row[U.DateDay_col] = vitalRecordProperties.DateDay;
            VitalRecord_row[U.Disposition_col] = vitalRecordProperties.Disposition;
            VitalRecord_row[U.CemeteryName_col] = vitalRecordProperties.CemeteryName;
            VitalRecord_row[U.LotNumber_col] = vitalRecordProperties.LotNumber;
            VitalRecord_row[U.Notes_col] = vitalRecordProperties.Notes;
            VitalRecord_row[U.PersonID_col] = personWithParents.PersonName.integratedPersonID;
            VitalRecord_row[U.FatherID_col] = personWithParents.FatherName.integratedPersonID;
            VitalRecord_row[U.MotherID_col] = personWithParents.MotherName.integratedPersonID;
            VitalRecord_row[U.ExcludeFromSite_col] = vitalRecordProperties.ExcludeFromSite;
            if (isSpouse)
            {
                VitalRecord_row[U.AgeYears_col] = vitalRecordProperties.SpouseAgeYears;
                VitalRecord_row[U.AgeMonths_col] = vitalRecordProperties.SpouseAgeMonths;
                VitalRecord_row[U.AgeDays_col] = vitalRecordProperties.SpouseAgeDays;
                VitalRecord_row[U.Sex_col] = vitalRecordProperties.SpouseSex;
            }
            else
            {
                VitalRecord_row[U.AgeYears_col] = vitalRecordProperties.AgeYears;
                VitalRecord_row[U.AgeMonths_col] = vitalRecordProperties.AgeMonths;
                VitalRecord_row[U.AgeDays_col] = vitalRecordProperties.AgeDays;
                VitalRecord_row[U.Sex_col] = vitalRecordProperties.Sex;
            }
            vitalRecord_tbl.Rows.Add(VitalRecord_row);
        }
        //****************************************************************************************************************************
        private bool SaveVitalRecord()
        {
            PersonSpouseWithParents personSpouseWithParents = SetPersonSpouseWithParents();
            VitalRecordProperties vitalRecordProperties = SetVitalRecordProperties();
            if (NotValidInfo(personSpouseWithParents, vitalRecordProperties))
            {
                return false;
            }
            try
            {
                if (m_iVitalRecordID == 0)
                {
                    CreateNewVitalRecord(personSpouseWithParents, vitalRecordProperties);

                }
                else
                {
                    ArrayList FieldsModified = UpdateVitalRecordInDataTable(personSpouseWithParents, vitalRecordProperties);
                    SQL.UpdateVitalRecord(FieldsModified, m_VitalRecord_tbl, m_iVitalRecordID, personSpouseWithParents);
                }
                DataRow VitalRecord_row = m_VitalRecord_tbl.Rows[0];
                DataRow SpouseVitalRecord_row = SpouseRow(m_VitalRecord_tbl, VitalRecord_row[U.VitalRecordID_col].ToInt(),
                                                VitalRecord_row[U.SpouseID_col].ToInt());
                DisplayAge(VitalRecord_row, SpouseVitalRecord_row);
                UpdateFirstNameTableWithAllPersons(personSpouseWithParents, vitalRecordProperties.Sex);
                SetToUnmodified();
                SetAllNameValues();
                SetIntegratedChecked(m_VitalRecord_tbl);
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
        private void CreateNewVitalRecord(PersonSpouseWithParents personSpouseWithParents,
                                          VitalRecordProperties vitalRecordProperties)
        {
            bool isMarriageRecord = personSpouseWithParents.SpouseVitalRecordType.IsMarriageRecord();
            if (isMarriageRecord)
            {
                vitalRecordProperties.ChangeDateToAge(null, BornDateMonth_textBox.Text.ToInt(), BornDateDay_textBox.Text.ToInt(), BornDateYear_textBox.Text.ToInt(), false);
            }
            AddVitalRecordToDataRow(m_VitalRecord_tbl, personSpouseWithParents.PersonWithParents, vitalRecordProperties, false);
            if (personSpouseWithParents.SpouseVitalRecordType != EVitalRecordType.eSearch)
            {
                vitalRecordProperties.ChangeDateToAge(null, SpouseBornDateMonth_textBox.Text.ToInt(), SpouseBornDateDay_textBox.Text.ToInt(), SpouseBornDateYear_textBox.Text.ToInt(), true);
                changeSexOfPartner(personSpouseWithParents.VitalRecordType, vitalRecordProperties);
                AddVitalRecordToDataRow(m_VitalRecord_tbl, personSpouseWithParents.SpouseWithParents, vitalRecordProperties, true);
            }
            SQL.CreateNewVitalRecord(m_VitalRecord_tbl);
            m_iVitalRecordID = m_VitalRecord_tbl.Rows[0][U.VitalRecordID_col].ToInt();
        }
        //****************************************************************************************************************************
        private void changeSexOfPartner(EVitalRecordType spouseVitalRecordType,
                                        VitalRecordProperties vitalRecordProperties)
        {
            if (spouseVitalRecordType == EVitalRecordType.eMarriageBride ||
                spouseVitalRecordType == EVitalRecordType.eMarriageGroom)
            {
                vitalRecordProperties.ChangeSexToSpouseSex();
            }
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
//                IntegrateSingleRecord(m_VitalRecord_tbl, NameIntegrated_checkBox.Checked,
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
            SearchBy = eSearchOption.SO_ReturnToLast;
            string sStartingPerson = "";
            if (LastName_textBox.Text.Length > 1 && FirstName_textBox.Text.Length != 0)
            {
                sStartingPerson = SQL.BuildNameLastNameFirst(FirstName_textBox.Text.ToString(), MiddleName_textBox.Text.ToString(),
                                                        LastName_textBox.Text.ToString(), Suffix_comboBox.Text.ToString(),
                                                        Prefix_comboBox.Text.ToString(), "", "");
            }
            SearchVitalRecords(sStartingPerson);
        }
        //****************************************************************************************************************************
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SaveVitalRecord() && !RecordAlreadyIntegrated())
            {
                IntegrateYesNo();
            }
        }
        //****************************************************************************************************************************
        private bool VitalRecordSpouseChanged(EVitalRecordType eVitalRecordType)
        {
            if (!eVitalRecordType.MarriageRecord())
                return false;
            if (SpouseFirstName_textBox.Modified ||
                SpouseMiddleName_textBox.Modified ||
                SpouseLastName_textBox.Modified ||
                SpouseFatherFirstName_textBox.Modified ||
                SpouseFatherMiddleName_textBox.Modified ||
                SpouseFatherLastName_textBox.Modified ||
                SpouseMotherFirstName_textBox.Modified ||
                SpouseMotherMiddleName_textBox.Modified ||
                SpouseMotherLastName_textBox.Modified ||
                BornDateMonth_textBox.Modified ||
                BornDateDay_textBox.Modified ||
                BornDateYear_textBox.Modified ||
                SpouseBornDateMonth_textBox.Modified ||
                SpouseBornDateDay_textBox.Modified ||
                SpouseBornDateYear_textBox.Modified)
            {
                return true;
            }
            if (m_VitalRecord_tbl.Rows.Count > 1)
            {
                DataRow Spouse_row = m_VitalRecord_tbl.Rows[1];
                if (Spouse_row[U.Suffix_col].ToString() != SpouseSuffix_comboBox.Text.TrimString() ||
                    Spouse_row[U.Prefix_col].ToString() != SpousePrefix_comboBox.Text.TrimString() ||
                    Spouse_row[U.FatherSuffix_col].ToString() != SpouseFatherSuffix_comboBox.Text.TrimString() ||
                    Spouse_row[U.FatherPrefix_col].ToString() != SpouseFatherPrefix_comboBox.Text.TrimString() ||
                    Spouse_row[U.MotherSuffix_col].ToString() != SpouseMotherSuffix_comboBox.Text.TrimString() ||
                    Spouse_row[U.MotherPrefix_col].ToString() != SpouseMotherPrefix_comboBox.Text.TrimString())
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool VitalRecordChanged()
        {
            if (SexChanged())
            {
                return true;
            }
            if (m_bDidSearch)
                return false;
            if (TextBoxModified())
                return true;
            if (VitalRecordSpouseChanged(m_eVitalRecordType))
                return true;
            if (bDispositionChanged)
                return true;
            if (bSexChanged)
                return true;
            if (ExcludeFromSiteCheckedChanged)
                return true;
            if (U.Modified(Cemetery_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.CemeteryName_col))
                return true;
            if (m_VitalRecord_tbl.Rows.Count != 0 && m_VitalRecord_tbl.Rows[0].RowState != DataRowState.Unchanged)
                return true;
            if (NameChangedOtherThanForSearchOption())
            {
                return true;
            }
            else
            {
//                m_bDidSearch = true;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool SexChanged()
        {
            if (m_VitalRecord_tbl.Rows.Count == 0)
            {
                return false;
            }
            char rowSex = m_VitalRecord_tbl.Rows[0][U.Sex_col].ToChar();
            return (m_sSex != SetSex(m_eVitalRecordType) || m_sSex != rowSex) ? true : false;
        }
        //****************************************************************************************************************************
        private bool ComboBoxModified()
        {
            if (U.Modified(Suffix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.Suffix_col) ||
                U.Modified(Prefix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.Prefix_col) ||
                U.Modified(FatherSuffix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.FatherSuffix_col) ||
                U.Modified(FatherPrefix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.FatherPrefix_col) ||
                U.Modified(MotherSuffix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.MotherSuffix_col) ||
                U.Modified(MotherPrefix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.MotherPrefix_col))
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
            if (FatherFirstName_textBox.Modified ||
                FatherMiddleName_textBox.Modified ||
                FatherLastName_textBox.Modified ||
                MotherFirstName_textBox.Modified ||
                MotherMiddleName_textBox.Modified ||
                MotherLastName_textBox.Modified ||
                DateMonth_textBox.Modified ||
                DateDay_textBox.Modified ||
                DateYear_textBox.Modified ||
                AgeYears_textBox.Modified ||
                AgeMonths_textBox.Modified ||
                AgeDays_textBox.Modified ||
                SpouseAgeYears_textBox.Modified ||
                SpouseAgeMonths_textBox.Modified ||
                SpouseAgeDays_textBox.Modified ||
                Book_textBox.Modified ||
                Page_textBox.Modified ||
                LotNumber_textBox.Modified ||
                Notes_TextBox.Modified)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected bool SaveIfDesired()
        {
            if (!VitalRecordChanged())
                return true;
            string sLastName = LastName_textBox.Text.ToString();
            if (sLastName.Length == 0)
                return true;
            switch (MessageBox.Show("Save Changes?", "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    if (SaveVitalRecord())
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
        private void LastNameTextBox_Click(object sender, EventArgs e)
        {
            if (!m_eVitalRecordType.IsDeathRecord() && m_eVitalRecordType != EVitalRecordType.eBurial && 
                 LastName_textBox.Text.ToString().Length != 0)
                FatherLastName_textBox.Text = LastName_textBox.Text;
        }
        //****************************************************************************************************************************
        private void SpouseLastNameTextBox_Click(object sender, EventArgs e)
        {
            if (!m_eVitalRecordType.IsDeathRecord() && m_eVitalRecordType != EVitalRecordType.eBurial)
                SpouseFatherLastName_textBox.Text = SpouseLastName_textBox.Text;
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
        private void FirstName_textBox_Leave(object sender, System.EventArgs e)
        {
            if (m_eVitalRecordType != EVitalRecordType.eBurial)
            {
                return;
            }
            if (!Male_radioButton.Checked && !Female_radioButton.Checked)
            {
                SetSexRadioButtons(SQL.GetFirstNameSex(FirstName_textBox.Text.ToString()));
            }
        }
        //****************************************************************************************************************************
        private void SetSexRadioButtons(char cSex)
        {
            switch (cSex)
            {
                case 'M': Male_radioButton.Checked = true; break;
                case 'F': Female_radioButton.Checked = true; break;
                default:
                    this.Male_radioButton.Checked = false;
                    this.Female_radioButton.Checked = false;
                    break;
            }
        }
        //****************************************************************************************************************************
    }
}
