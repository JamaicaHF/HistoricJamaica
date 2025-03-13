using System;
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
    public partial class FPerson : Form
    {
        private const int m_currentCensusYear = 1950;
        private const int m_pageInCensus = 1;
        private const string censusAgeString = " Census Age ";
        private const string noAgeInCensus = "No Age in Census";
        protected eSearchOption SearchBy = eSearchOption.SO_AllNames;
        protected CSql m_SQL;
        protected string m_FirstName = "";
        protected string m_MiddleName = "";
        protected string m_MarriedName = "";
        protected string m_KnownAs = "";
        protected string m_LastName = "";
        protected string m_Suffix = "";
        protected string m_Prefix = "";
        private PersonData personData;
        private int m_iNumberSpouses = 0;
        private int m_iCurrentSpouseIndex = 0;
        private int m_iSpouseLocationInArray = 1;
        private int m_iPersonID = 0;
        private int m_iSpouseID = 0;
        private int m_iFatherID = 0;
        private int m_iMotherID = 0;
        private bool m_bDidSearch = false;
        private bool m_bCategoryOrBuildingChanged = false;
        private bool m_bSelectPersonForPhoto = false;
        private string m_sPersonWhereStatement = "";
        private int m_iOriginalPersonID = 0;
        private bool m_bVitalBornFound = false;
        private bool m_bVitalDiedFound = false;
        private bool m_bVitalBuriedFound = false;
        private string sDefaultSex = "";
        private int m_yearOfCensus = 0;
        private bool m_buildingsLivedInModified = false;
        private bool ExcludeFromSiteCheckedChanged = false;
        //****************************************************************************************************************************
        public FPerson(CSql SQL) // only called by derived class CPersonFilter
        {
            InitializeComponent();
            AdditionalMarried_button.Visible = false;
        }
        //****************************************************************************************************************************
        public FPerson(CSql SQL,
                       bool bSelectPersonForPhoto)
        {
            m_SQL = SQL;
            m_iPersonID = 0;
            m_iOriginalPersonID = 0;
            m_bSelectPersonForPhoto = bSelectPersonForPhoto;
            InitializeFPerson();
        }
        //****************************************************************************************************************************
        public FPerson(CSql SQL,
                       bool initializeCensusFields,
                       int iPersonID)
        {
            m_SQL = SQL;
            m_iPersonID = iPersonID;
            m_iOriginalPersonID = iPersonID;
            m_bSelectPersonForPhoto = false;
            m_yearOfCensus = m_currentCensusYear;
            InitializeFPerson();
        }
        //****************************************************************************************************************************
        public FPerson(CSql SQL,
                       int iPersonID,
                       bool bSelectPersonForPhoto)
        {
            m_SQL = SQL;
            m_iPersonID = iPersonID;
            m_iOriginalPersonID = iPersonID;
            m_bSelectPersonForPhoto = bSelectPersonForPhoto;
            InitializeFPerson();
        }
        //****************************************************************************************************************************
        public FPerson(CSql SQL,
                       int iPersonID,
                       string sSex,
                       string sLastName,
                       string sMarriedName,
                       int yearOfCensus,
                       bool bSelectPersonForPhoto)
        {
            m_SQL = SQL;
            m_iOriginalPersonID = iPersonID;
            m_bSelectPersonForPhoto = bSelectPersonForPhoto;
            m_iPersonID = iPersonID;
            sDefaultSex = sSex;
            m_MarriedName = sMarriedName;
            m_LastName = sLastName;
            m_yearOfCensus = yearOfCensus;
            InitializeFPerson();
        }
        //****************************************************************************************************************************
        private void InitializeFPerson()
        {
            InitializeComponent();
            Properties_listBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicturedBuildinglistBox_RightClick);
            AdditionalSpouse_button.Visible = false;
            AdditionalMarried_button.Visible = false;
            int iPersonLevel = SQL.GetNextPersonLevel();
            this.Location = new System.Drawing.Point(200 + 30 * iPersonLevel, 30 * iPersonLevel);
            InstantiateContextMenus();
//            Properties_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(BuildinglistBox_Click);
            Properties_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(BuildingOccupantlistBox_Click);
            UU.LoadSuffixComboBox(Suffix_comboBox);
            UU.LoadPrefixComboBox(Prefix_comboBox);
            personData = new PersonData(this);
            if (m_iPersonID == 0)
            {
                SetSexRadioButton(sDefaultSex);
                LastName_textBox.Text = m_LastName;
                MarriedName_textBox.Text = m_MarriedName;
            }
            else
            {
                DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        private void BuildingOccupantlistBox_Click(object sender, EventArgs e)
        {
            CGridGroupValuesModernRoads GridGroupValuesModernRoads = new CGridGroupValuesModernRoads(m_SQL, 0, false, false, true);
            GridGroupValuesModernRoads.ShowDialog();
            int iBuildingID = GridGroupValuesModernRoads.SelectedBuilding;
            DataTable buildingOccupant_tbl = personData.BuildingOccupantTable();
            DataRow buildingOccupant_row = buildingOccupant_tbl.Rows.Find(iBuildingID);
            if (buildingOccupant_row == null)
            {
                m_buildingsLivedInModified = true;
                buildingOccupant_row = buildingOccupant_tbl.NewRow();
                string buildingName = SQL.GetBuildingName(iBuildingID);
                buildingOccupant_row[U.BuildingID_col] = iBuildingID;
                buildingOccupant_row[U.BuildingName_col] = buildingName;
                buildingOccupant_row[U.SpouseLivedWithID_col] = 0;
                buildingOccupant_row[U.Notes_col] = "";
                buildingOccupant_row[U.CensusYears_col] = 0;
                buildingOccupant_tbl.Rows.Add(buildingOccupant_row);
                Properties_listBox.Items.Add(buildingName);
            }
        }
        //****************************************************************************************************************************
        private void RemoveBuilding_click(object sender, EventArgs e)
        {
            int iRowIndex = Properties_listBox.SelectedIndex;
            if (iRowIndex < 0) return;
            string sBuildingName = Properties_listBox.Items[iRowIndex].ToString();
            int indexOf = sBuildingName.IndexOf("(");
            if (indexOf > 0)
            {
                sBuildingName = sBuildingName.Substring(0, indexOf).Trim();
            }
            DataViewRowState dvRowState = DataViewRowState.Added |
                                          DataViewRowState.Unchanged;
            foreach (DataRow row in personData.BuildingOccupantTable().Select("", "", dvRowState))
            {
                if (sBuildingName == row[U.BuildingName_col].ToString().Trim())
                {
                    row.Delete();
                    m_buildingsLivedInModified = true;
                    Properties_listBox.Items.RemoveAt(iRowIndex);
                }
            }
        }
        //****************************************************************************************************************************
        private void ExcludeFromSiteChanged_click(object sender, EventArgs e)
        {
            ExcludeFromSiteCheckedChanged = true;
        }
        //****************************************************************************************************************************
        private void PicturedBuildinglistBox_RightClick(object sender, MouseEventArgs e)
        {
            int indexOfItemUnderMouse = Properties_listBox.IndexFromPoint(e.X, e.Y);
            ContextMenuStrip strip = new ContextMenuStrip();
            ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
            if (indexOfItemUnderMouse < 0)
            {
                toolStripItem1.Text = "Add New Building Lived In";
                toolStripItem1.Click += new EventHandler(BuildingOccupantlistBox_Click);
            }
            else
            {
                toolStripItem1.Text = "Remove Building Lived In";
                toolStripItem1.Click += new EventHandler(RemoveBuilding_click);
            }
            strip.Items.Add(toolStripItem1);
            Properties_listBox.ContextMenuStrip = strip;
        }
        //****************************************************************************************************************************
        protected void SetAllNameValues()
        {
            m_FirstName = FirstName_textBox.Text.TrimString();
            m_MiddleName = MiddleName_textBox.Text.TrimString();
            m_LastName = LastName_textBox.Text.TrimString();
            m_Suffix = Suffix_comboBox.Text.TrimString();
            m_Prefix = Prefix_comboBox.Text.TrimString();
            m_MarriedName = MarriedName_textBox.Text.TrimString();
            m_KnownAs = KnownAs_textBox.Text.TrimString();
        }
        //****************************************************************************************************************************
        private void BornStats(DataRow Person_row)
        {
            string sBook = "";
            string sPage = "";
            string sDate = "";
            string sPlace = "";
            string sHome = "";
            string sSource = "";
            U.SetValuesToPersonInfo(Person_row, EVitalRecordType.eBirthMale, ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
            BornBook_textBox.Text = sBook;
            BornPage_textBox.Text = sPage;
            BornDate_textBox.Text = sDate;
            BornPlace_textBox.Text = sPlace;
            BornHome_textBox.Text = sHome;
            BornSource_textBox.Text = sSource;
            bool indirectBornDate = U.GetPersonVitalStatistics(personData.VitalRecordsTable(), personData.CemeteryRecordsTable(),
                                        Person_row, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                        ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
            if (String.IsNullOrEmpty(sDate))
            {
                indirectBornDate = false;
            }
            if (indirectBornDate)
            {
                BornFrom_textBox.Text = sDate;
                BornFrom_label.Text = sSource;
            }
            BornFrom_textBox.Visible = indirectBornDate;
            BornFrom_label.Visible = indirectBornDate;
            BornDate_label.Visible = indirectBornDate;
        }
        //****************************************************************************************************************************
        private bool VitalRecord(DataRow Person_row,
                                 EVitalRecordType eVitalRecordType1,
                                 EVitalRecordType eVitalRecordType2,
                                 TextBox  Book_textBox,
                                 TextBox  Page_textBox,
                                 TextBox  Date_textBox,
                                 TextBox  Place_textBox,
                                 TextBox  Home_textBox,
                                 CheckBox Verified_checkBox,
                                 TextBox  Source_textBox)
        {
            string sBook = "";
            string sPage = "";
            string sDate = "";
            string sPlace = "";
            string sHome = "";
            string sSource = "";
            Verified_checkBox.Checked = U.GetPersonVitalStatistics(personData.VitalRecordsTable(), personData.CemeteryRecordsTable(),
                                        Person_row, eVitalRecordType1, eVitalRecordType2,
                                        ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
            Book_textBox.Text = sBook;
            Page_textBox.Text = sPage;
            Date_textBox.Text = sDate;
            Place_textBox.Text = sPlace;
            Home_textBox.Text = sHome;
            Source_textBox.Text = sSource;
            return Verified_checkBox.Checked;
        }
        //****************************************************************************************************************************
        private void SetSexRadioButton(string sSex)
        {
            sDefaultSex = sSex;
            if (sSex.Length == 0)
            {
                Male_radioButton.Checked = false;
                Female_radioButton.Checked = false;
            }
            else if (sSex[0] == 'M')
            {
                Male_radioButton.Checked = true;
            }
            else if (sSex[0] == 'F')
            {
                Female_radioButton.Checked = true;
            }
            else
            {
                Male_radioButton.Checked = false;
                Female_radioButton.Checked = false;
            }
        }
        //****************************************************************************************************************************
        private void LoadPersonIntoControls(DataRow Person_row)
        {
            PersonID_textBox.Visible = true;
            PersonID_textBox.Enabled = false;
            PersonID_textBox.Text = m_iPersonID.ToString();
            FirstName_textBox.Text = Person_row[U.FirstName_col].ToString();
            MiddleName_textBox.Text = Person_row[U.MiddleName_col].ToString();
            LastName_textBox.Text = Person_row[U.LastName_col].ToString();
            Suffix_comboBox.Text = Person_row[U.Suffix_col].ToString();
            Prefix_comboBox.Text = Person_row[U.Prefix_col].ToString();
            MarriedName_textBox.Text = Person_row[U.MarriedName_col].ToString();
            KnownAs_textBox.Text = Person_row[U.KnownAs_col].ToString();
            string sSex = Person_row[U.Sex_col].ToString();
            Male_radioButton.Checked = false;
            Female_radioButton.Checked = false;
            ExcludeFromSite_checkBox.Checked = Person_row[U.ExcludeFromSite_col].ToBool();
            if (sSex.Length != 0)
            {
                SetSexRadioButton(sSex);
            }
            BornStats(Person_row);

            m_bVitalDiedFound = VitalRecord(Person_row, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale, DiedBook_textBox, DiedPage_textBox,
                                            DiedDate_textBox, DiedPlace_textBox, DiedHome_textBox, DiedVerified_checkBox, DiedSource_textBox);
            m_bVitalBuriedFound = VitalRecord(Person_row,EVitalRecordType.eBurial, EVitalRecordType.eSearch, BuriedBook_textBox, BuriedPage_textBox,
                                            BuriedDate_textBox, BuriedPlace_textBox, BuriedStone_textBox, BuriedVerified_checkBox, BuriedSource_textBox);
            Description_TextBox.Text = Person_row[U.Notes_col].ToString();
            int GazetteerRoad = Person_row[U.GazetteerRoad_col].ToInt();
            GazetteerRoad_textBox.Text = (GazetteerRoad == 0) ? "" : GazetteerRoad.ToString();
            Beers1869District_textBox.Text = SetDistrictValue(Person_row[U.Beers1869District_col].ToInt());
            McClellan1856District_textBox.Text = SetDistrictValue(Person_row[U.McClellan1856District_col].ToInt());
            Source_textBox.Text = Person_row[U.Source_col].ToString();
            SetCensusYearTextboxs(Person_row);
            m_iFatherID = Person_row[U.FatherID_col].ToInt();
            m_iMotherID = Person_row[U.MotherID_col].ToInt();
            Father_textBox.Text = SQL.GetPersonName(m_iFatherID);
            Mother_textBox.Text = SQL.GetPersonName(m_iMotherID);
            DisplaySpouse();
            SetToUnmodified();
        }
        //****************************************************************************************************************************
        private string SetDistrictValue(int district)
        {
            if (district == 99)
            {
                return "V";
            }
            else
            if (district == 89)
            {
                return "R";
            }
            else
            if (district == 79)
            {
                return "W";
            }
            else
            {
                return ToStringBlankIfZero(district);
            }
        }
        //****************************************************************************************************************************
        private void SetCensusYearTextboxs(DataRow Person_row)
        {
            for (int i = 1790; i <= 1950; i += 10)
            {
                string censusYearCol = U.CensusYearCol(i);
                LoadCensusPage(i, Person_row[censusYearCol].ToInt());
            }
        }
        //****************************************************************************************************************************
        private void LoadCensusPage(int year, int iPage)
        {
            string page = ToStringBlankIfZero(iPage);
            switch (year)
            {
                case 1790: Census1790_textBox.Text = page; break;
                case 1800: Census1800_textBox.Text = page; break;
                case 1810: Census1810_textBox.Text = page; break;
                case 1820: Census1820_textBox.Text = page; break;
                case 1830: Census1830_textBox.Text = page; break;
                case 1840: Census1840_textBox.Text = page; break;
                case 1850: Census1850_textBox.Text = page; break;
                case 1860: Census1860_textBox.Text = page; break;
                case 1870: Census1870_textBox.Text = page; break;
                case 1880: Census1880_textBox.Text = page; break;
                case 1890: Census1890_textBox.Text = page; break;
                case 1900: Census1900_textBox.Text = page; break;
                case 1910: Census1910_textBox.Text = page; break;
                case 1920: Census1920_textBox.Text = page; break;
                case 1930: Census1930_textBox.Text = page; break;
                case 1940: Census1940_textBox.Text = page; break;
                case 1950: Census1950_textBox.Text = page; break;
                default: break;
            }
        }
        //****************************************************************************************************************************
        private string ToStringBlankIfZero(int iCensusPage)
        {
            if (iCensusPage == 0)
                return "";
            else
                return iCensusPage.ToString();
        }
        //****************************************************************************************************************************
        protected void DisplaySpouse()
        {
            if (m_iNumberSpouses == 0)
            {
                m_iSpouseID = 0;
                Spouse_textBox.Text = "";
                return;
            }
            DataViewRowState dvrs = DataViewRowState.Added | DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent;
            DataRow[] rows = personData.MarriageTable().Select("", "", dvrs);
            if (rows.Length == 0)
            {
                Spouse_textBox.Text = "";
                return;
            }
            DataRow row = rows[m_iCurrentSpouseIndex];
            m_iSpouseID = row[m_iSpouseLocationInArray].ToInt();
            bool bModified = Spouse_textBox.Modified;
            Spouse_textBox.Text = SQL.GetPersonName(m_iSpouseID);
            Spouse_textBox.Modified = bModified;
            if (rows.Length > 1)
                AdditionalSpouse_button.Visible = true;
            else
                AdditionalSpouse_button.Visible = false;
        }
        //****************************************************************************************************************************
        public void LoadBuilding_listBox(DataTable BuildingTBL,
                                         ListBoxWithDoubleClick Properties_listBox,
                                         string sTableName,
                                         string sTableId,
                                         int iTableID)
        {
            Properties_listBox.Items.Clear();
            foreach (DataRow row in BuildingTBL.Rows)
            {
                int iBuildingID = row[U.BuildingID_col].ToInt();
                DataTable BuildingTable = SQL.GetBuilding(iBuildingID);
                if (BuildingTable.Rows.Count != 0)
                {
                    DataRow BuildingRow = BuildingTable.Rows[0];
                    string sBuildingName = SQL.AddRoadToGroup(BuildingRow[U.BuildingName_col].ToString(),
                                               BuildingRow[U.BuildingRoadValueID_col].ToInt(),
                                               BuildingRow[U.BuildingGrandListID_col].ToInt());
                    Properties_listBox.Items.Add(sBuildingName);
                }
            }
            DataTable building_tbl = SQL.GetAllBuildingsAsSpouse(m_iPersonID);
        }
        //****************************************************************************************************************************
        protected void GetMarriages()
        {
            DataTable tbl = new DataTable();
            m_iSpouseID = SQL.GetMarriagesID(tbl, ref m_iNumberSpouses, out m_iSpouseLocationInArray, m_iPersonID);
            m_iCurrentSpouseIndex = 0;
            DisplaySpouse();
        }
        //****************************************************************************************************************************
        protected void DisplayPerson(int iPersonID)
        {
            m_bDidSearch = false;
            m_iCurrentSpouseIndex = 0;
            personData.GetPerson(iPersonID);
            if (personData.NoPersonExists())
                return;
            m_iNumberSpouses = personData.NumberSpouses();
            LoadPersonIntoControls(personData.PersonTable().Rows[0]);
            SetAllNameValues();
            if (SQL.IsFullDatabase())
            {
                LoadBuilding_listBox(personData.BuildingOccupantTable(), Properties_listBox,
                                           U.BuildingOccupant_Table, U.Person_Table, m_iPersonID);
            }
            PopulateCensusFields();
        }
        //****************************************************************************************************************************
        private void PopulateCensusFields()
        {
            if (m_yearOfCensus == 0)
                return;
            LoadCensusPage(m_yearOfCensus, m_pageInCensus);
            if (BornSource_textBox.Text.ToString().Length == 0)
            {
                BornSource_textBox.Text = m_yearOfCensus.ToString() + censusAgeString;
            }
        }
        //****************************************************************************************************************************
        public virtual bool AddMode()
        {
            return false;
        }
        //****************************************************************************************************************************
        public string GetOppositeSex()
        {
            if (Male_radioButton.Checked)
                return "F";
            else if (Female_radioButton.Checked)
                return "M";
            else
                return "";
        }
        //****************************************************************************************************************************
        public string SetPersonSex()
        {
            if (Male_radioButton.Checked)
                return "M";
            else if (Female_radioButton.Checked)
                return "F";
            else
                return "";
        }
        //****************************************************************************************************************************
        public char SetPersonSexChar()
        {
            if (Male_radioButton.Checked)
                return 'M';
            else if (Female_radioButton.Checked)
                return 'F';
            else
                return ' ';
        }
        //****************************************************************************************************************************
        private void SetToUnmodified()
        {
            m_bCategoryOrBuildingChanged = false;
            m_bDidSearch = false;
            m_buildingsLivedInModified = false;
            FirstName_textBox.Modified = false;
            MiddleName_textBox.Modified = false;
            LastName_textBox.Modified = false;
            // do not set Modified(TrimString(Suffix_comboBox.Text.ToString()), tbl, "", U.Suffix_col) ||
            //            Modified(TrimString(Prefix_comboBox.Text.ToString()), tbl, "", U.Prefix_col) ||
            MarriedName_textBox.Modified = false;
            KnownAs_textBox.Modified = false;
            Description_TextBox.Modified = false;
            GazetteerRoad_textBox.Modified = false;
            McClellan1856District_textBox.Modified = false;
            Beers1869District_textBox.Modified = false;
            Spouse_textBox.Modified = false;
            Father_textBox.Modified = false;
            Mother_textBox.Modified = false;
            //            Modified(SetPersonSex(), tbl, "", U.Sex_col)) ||
            BornDate_textBox.Modified = false;
            BornPlace_textBox.Modified = false;
            BornHome_textBox.Modified = false;
            //            Modified(IsChecked(BornVerified_checkBox.Checked), tbl,"N", U.BornVerified_col) ||
            BornSource_textBox.Modified = false;
            BornBook_textBox.Modified = false;
            BornPage_textBox.Modified = false;
            BornFrom_textBox.Modified = false;
            DiedDate_textBox.Modified = false;
            DiedPlace_textBox.Modified = false;
            DiedHome_textBox.Modified = false;
            //            Modified(IsChecked(DiedVerified_checkBox.Checked), tbl, "N", U.DiedVerified_col) ||
            DiedSource_textBox.Modified = false;
            DiedBook_textBox.Modified = false;
            DiedPage_textBox.Modified = false;
            BuriedDate_textBox.Modified = false;
            BuriedPlace_textBox.Modified = false;
            BuriedStone_textBox.Modified = false;
            //            Modified(IsChecked(BuriedVerified_checkBox.Checked), tbl, "N", U.BuriedVerified_col) ||
            BuriedSource_textBox.Modified = false;
            BuriedBook_textBox.Modified = false;
            BuriedPage_textBox.Modified = false;
            Census1790_textBox.Modified = false;
            Census1800_textBox.Modified = false;
            Census1810_textBox.Modified = false;
            Census1820_textBox.Modified = false;
            Census1830_textBox.Modified = false;
            Census1840_textBox.Modified = false;
            Census1850_textBox.Modified = false;
            Census1860_textBox.Modified = false;
            Census1870_textBox.Modified = false;
            Census1880_textBox.Modified = false;
            Census1890_textBox.Modified = false;
            Census1900_textBox.Modified = false;
            Census1910_textBox.Modified = false;
            Census1920_textBox.Modified = false;
            Census1930_textBox.Modified = false;
            Census1940_textBox.Modified = false;
            Census1950_textBox.Modified = false;
            ExcludeFromSiteCheckedChanged = false;
        }
        //****************************************************************************************************************************
        private int GetPersonFromGrid(DataTable tbl,
                                      string sSortLastName)
        {
            CGridPerson GridDataViewPerson = new CGridPerson(m_SQL, ref tbl, AddMode(), "Person", sSortLastName);
            GridDataViewPerson.ShowDialog();
            int iPersonID = GridDataViewPerson.SelectedPersonID;
            if (iPersonID != 0 && m_bSelectPersonForPhoto)
            {
                m_FirstName = GridDataViewPerson.SelectedFirstName;
                m_MiddleName = GridDataViewPerson.SelectedMiddleName;
                m_LastName = GridDataViewPerson.SelectedLastName;
                m_Suffix = GridDataViewPerson.SelectedSuffix;
                m_Prefix = GridDataViewPerson.SelectedPrefix;
                m_MarriedName = GridDataViewPerson.SelectedMarriedName;
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        protected void SearchForPerson(string sSortLastName)
        {
            m_bDidSearch = ChangesNameOptionsForSearch();
            SetAllNameValues();
            DataTable tbl = SQL.DefinePersonTable();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PersonID_col] };
            PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
            SQL.PersonsBasedOnNameOptions(tbl, true, U.Person_Table, U.PersonID_col, SearchBy, personName, m_MarriedName);
            int newPersonID = 0;
            if (tbl.Rows.Count == 0)
            {
                MessageBox.Show("No Persons Found");
            }
            else
            {
                newPersonID = GetPersonFromGrid(tbl, sSortLastName);
            }
            if (newPersonID != 0)
            {
                m_iPersonID = newPersonID;
                m_iOriginalPersonID = m_iPersonID;
                if (m_bSelectPersonForPhoto)
                {
                    this.CloseForm();
                    return;
                }
                else
                {
                    DisplayPerson(m_iPersonID);
                }
            }
            else if (m_bDidSearch)
            {
                string lastName = LastName_textBox.Text;
                string firstName = FirstName_textBox.Text;
                string marriedName = MarriedName_textBox.Text;
                m_iPersonID = 0;
                NewPerson();
                LastName_textBox.Text = lastName;
                FirstName_textBox.Text = firstName;
                MarriedName_textBox.Text = marriedName;
            }
        }
        //****************************************************************************************************************************
        private bool ChangesNameOptionsForSearch()
        {
            if (FirstName_textBox.Modified || LastName_textBox.Modified || MarriedName_textBox.Modified)
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected virtual void SearchAll_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_AllNames;
            SearchForPerson("");
        }
        //****************************************************************************************************************************
        protected virtual void SearchStartingWith_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_StartingWith;
            SearchForPerson(LastName_textBox.Text);
        }
        //****************************************************************************************************************************
        protected virtual void SearchPartial_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(LastName_textBox.Text);
        }
        //****************************************************************************************************************************
        protected virtual void VitalRecord_button_Click(object sender, System.EventArgs e)
        {
            if (m_iPersonID == 0)
            {
                return;
            }
            if (SaveBeforeLeaving())
            {
                int iRecordID = DisplayVitalRecords();
                ReturnToPreviousScreen(iRecordID);
            }
        }
        //****************************************************************************************************************************
        private int DisplayVitalRecords()
        {
            DataTable tbl = SQL.DefineVitalRecord_Table();
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.VitalRecordID_col] };
            SQL.GetAllRecordsForPerson(tbl, m_iPersonID, SetPersonSex());
            return GetVitalRecordFromGrid(tbl, "");
        }
        //****************************************************************************************************************************
        private void ReturnToPreviousScreen(int iRecordID)
        {
            if (iRecordID == 0)
            {
                return;
            }
            if (iRecordID > 9900000)
            {
                int cemeteryRecordId = iRecordID - 9900000;
                FCemeteryRecord CemeteryRecord = new FCemeteryRecord(m_SQL, cemeteryRecordId);
                CemeteryRecord.ShowDialog();
            }
            else if (iRecordID > U.SchoolRecordOffset_col)
            {
                int SchoolRecordId = iRecordID - U.SchoolRecordOffset_col;
                DataTable schoolRecord_tbl = SQL.GetSchoolRecord(SchoolRecordId);
                if (schoolRecord_tbl.Rows.Count != 0)
                {
                    DataRow schoolRecord_row = schoolRecord_tbl.Rows[0];
                    int schoolId = schoolRecord_row[U.SchoolID_col].ToInt();
                    int schoolYear = schoolRecord_row[U.Year_col].ToInt();
                    int grade = schoolRecord_row[U.Grade_col].ToInt();
                    CGridStudentRecords gridStudentRecords = new CGridStudentRecords(ref m_SQL, schoolId, schoolYear, grade, false);
                    gridStudentRecords.ShowDialog();
                }
            }
            else
            {
                FVitalRecord VitalRecord = new FVitalRecord(m_SQL, iRecordID);
                VitalRecord.ShowDialog();
            }
        }
        //****************************************************************************************************************************
        private int GetVitalRecordFromGrid(DataTable tbl,
                                            string startingWith)
        {
            //if (AbortIfAlreadyExistsAndModifiedWithoutSaving())
            //    return;
            CGridVitalRecord GridVitalRecord = new CGridVitalRecord(m_SQL, ref tbl);
            GridVitalRecord.SetStartingWith(startingWith);
            GridVitalRecord.ShowDialog();
            return GridVitalRecord.SelectedVitalRecordID;
        }
        //****************************************************************************************************************************
        protected virtual void SearchSimilar_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_Similar;
            SearchForPerson(LastName_textBox.Text);
        }
        //****************************************************************************************************************************
        private void DateTextbox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.ToString().Length != 0)
            {
                string newDate = UU.ConvertToDateYMD(textBox.Text.ToString());
                if (newDate.Length == 0)
                {
                    textBox.Focus();
                }
                else
                {
                    textBox.Text = newDate;
                }
            }
        }
        //****************************************************************************************************************************
        private void ViewPhotographs_Click(object sender, EventArgs e)
        {
            SetAllNameValues();
            if (m_iPersonID == 0)
            {
                return;
            }
            FPhotoViewer PhotoViewer = new FPhotoViewer(ref m_SQL, m_iPersonID, U.PicturedPerson_Table, FHistoricJamaica.RunPhotoSlideShow);
            if (PhotoViewer.PhotoViewerAborted())
            {
                MessageBox.Show("No Photos found");
            }
            else if (SaveBeforeLeaving())
            {
                PhotoViewer.ShowDialog();
            }
            DisplayPerson(m_iPersonID);
        }
        //****************************************************************************************************************************
        public int GetPersonID()
        {
            return m_iPersonID;
        }
        //****************************************************************************************************************************
        public void GetPersonName(ref string sFirstName,
                                  ref string sMiddleName,
                                  ref string sLastName,
                                  ref string sSuffix,
                                  ref string sPrefix)
        {
            sFirstName = m_FirstName;
            sMiddleName = m_MiddleName;
            sLastName = m_LastName;
            sSuffix = m_Suffix;
            sPrefix = m_Prefix;
        }
        //****************************************************************************************************************************
        public void GetPersonName(DataRow row)
        {
            row[U.FirstName_col] = m_FirstName;
            row[U.MiddleName_col] = m_MiddleName;
            row[U.LastName_col] = m_LastName;
            row[U.Suffix_col] = m_Suffix;
            row[U.Prefix_col] = m_Prefix;
        }
        //****************************************************************************************************************************
        public string GetPersonIDandWhereStatement(ref int iPersonID)
        {
            iPersonID = m_iPersonID;
            return m_sPersonWhereStatement;
        }
        //****************************************************************************************************************************
        public void CloseForm()
        {
//            SQL.resetPersonLevel();
            this.Close();
        }
        //****************************************************************************************************************************
        private bool SavePerson()
        {
            if (m_bDidSearch)
            {
                return false;
            }
            if (SourceCensusAgeNoAgeEntered())
            {
                return false;
            }
            if (!ValidSex())
            {
                return false;
            }
            if (CheckLastMarriedName())
            {
                return false;
            }
            string sSuffix = Suffix_comboBox.Text.ToString();
            if (Source_textBox.Text.TrimString().Length == 0)
                Source_textBox.Text = U.ManualEntry;
            try
            {
                m_iPersonID = personData.SavePerson(m_iOriginalPersonID, m_iSpouseID);
            }
            catch (HistoricJamaicaException)
            {
                return false;
            }
            m_iOriginalPersonID = m_iPersonID;
            SetToUnmodified();
            return true;
        }
        //****************************************************************************************************************************
        private bool CheckLastMarriedName()
        {
            if (String.IsNullOrEmpty(MarriedName_textBox.Text))
            {
                return false;
            }
            string message = "";
            if (Male_radioButton.Checked)
            {
                message = "Male has Married Name Entered.  Continue?";
            }
            if (MarriedName_textBox.Text == LastName_textBox.Text)
            {
                message = "Last Name is same as Married name.  Continue?";
            }
            else if (HusbandLastNameSame())
            {
                message = "Last Name same as husband's last name. Continue?";
            }
            if (String.IsNullOrEmpty(message))
            {
                return false;
            }
            switch (MessageBox.Show(message, "", MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                    return false;
                default: return true;
            }
        }
        //****************************************************************************************************************************
        private bool HusbandLastNameSame()
        {
            DataViewRowState dvrs = DataViewRowState.Added | DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent;
            DataRow[] rows = personData.MarriageTable().Select("", "", dvrs);
            if (rows.Length == 0)
            {
                return false;
            }
            foreach (DataRow row in rows)
            {
                int iSpouseID = row[m_iSpouseLocationInArray].ToInt();
                if (LastName_textBox.Text.ToString() == SQL.GetPersonLastName(iSpouseID))
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool SourceCensusAgeNoAgeEntered()
        {
            string bornSource = BornSource_textBox.Text.Trim();
            if (bornSource.EndsWith(censusAgeString.Trim()))
            {
                switch (MessageBox.Show("Census Age Not Entered. Continue", "", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        BornSource_textBox.Text = noAgeInCensus;
                        return false;
                    default: return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool BornModified(DataTable tbl)
        {
            //if (!m_bVitalBornFound && !m_bVitalDiedFound)
            {
                return U.Modified(SetPersonSex(), tbl, sDefaultSex, U.Sex_col) ||
                                BornDate_textBox.Modified ||
                                BornPlace_textBox.Modified ||
                                BornHome_textBox.Modified ||
                                U.ModifiedCheckBox(U.IsChecked(BornVerified_checkBox.Checked), tbl, "N", U.BornVerified_col) ||
                                BornSource_textBox.Modified ||
                                BornBook_textBox.Modified ||
                                BornPage_textBox.Modified;
            }
            //return false;
        }
        //****************************************************************************************************************************
        private bool DiedModified(DataTable tbl)
        {
            if (!m_bVitalDiedFound)
            {
                return DiedDate_textBox.Modified ||
                                DiedPlace_textBox.Modified ||
                                DiedHome_textBox.Modified ||
                                U.ModifiedCheckBox(U.IsChecked(DiedVerified_checkBox.Checked), tbl, "N", U.DiedVerified_col) ||
                                DiedSource_textBox.Modified ||
                                DiedBook_textBox.Modified ||
                                DiedPage_textBox.Modified;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool BuriedModified(DataTable tbl)
        {
            if (!m_bVitalBuriedFound)
            {
                return BuriedDate_textBox.Modified ||
                                  BuriedPlace_textBox.Modified ||
                                  BuriedStone_textBox.Modified ||
                                  U.ModifiedCheckBox(U.IsChecked(BuriedVerified_checkBox.Checked), tbl, "N", U.BuriedVerified_col) ||
                                  BuriedSource_textBox.Modified ||
                                  BuriedBook_textBox.Modified ||
                                  BuriedPage_textBox.Modified;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool PersonChanged()
        {
            SetAllNameValues();
            if (m_bDidSearch)
                return false;
            DataTable person_tbl = personData.PersonTable();
            bool bBornModified = BornModified(person_tbl);
            bool bDiedModified = DiedModified(person_tbl);
            bool bBuriedModified = BuriedModified(person_tbl);
            if (bBornModified || bDiedModified || bBuriedModified ||
                ExcludeFromSiteCheckedChanged ||
                m_bCategoryOrBuildingChanged ||
                FirstName_textBox.Modified ||
                MiddleName_textBox.Modified ||
                LastName_textBox.Modified ||
                CensusModified() ||
                U.Modified(Suffix_comboBox.Text.TrimString(), person_tbl, "", U.Suffix_col) ||
                U.Modified(Prefix_comboBox.Text.TrimString(), person_tbl, "", U.Prefix_col) ||
                MarriedName_textBox.Modified ||
                KnownAs_textBox.Modified ||
                Spouse_textBox.Modified ||
                Father_textBox.Modified ||
                Mother_textBox.Modified ||
                Source_textBox.Modified ||
                m_buildingsLivedInModified ||
                Description_TextBox.Modified ||
                Beers1869District_textBox.Modified ||
                McClellan1856District_textBox.Modified ||
                GazetteerRoad_textBox.Modified)
            {
                return ValidLastName();
            }
            else
                return false;
        }
        //****************************************************************************************************************************
        private bool ValidSex()
        {
            string sSex = SetPersonSex();
            if (sSex != "M" && sSex != "F")
            {
                MessageBox.Show("You must Choose Sex before Saving");
                return false;
            }
            string firstNameSex = SQL.GetFirstNameSex(FirstName_textBox.Text).ToString().Trim();
            if (String.IsNullOrEmpty(firstNameSex))
            {
                SQL.UpdateFirstNameTable(FirstName_textBox.Text.SetNameForDatabase(), sSex);
            }
            else if (firstNameSex != "B" && firstNameSex != sSex)
            {
                if (MessageBox.Show("Sex does not match firstname Table value.  Use Sex Anyway", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
                SQL.UpdateFirstNameTable(FirstName_textBox.Text.SetNameForDatabase(), sSex);
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool CensusModified()
        {
            return (Census1790_textBox.Modified ||
                    Census1800_textBox.Modified ||
                    Census1810_textBox.Modified ||
                    Census1820_textBox.Modified ||
                    Census1830_textBox.Modified ||
                    Census1840_textBox.Modified ||
                    Census1850_textBox.Modified ||
                    Census1860_textBox.Modified ||
                    Census1870_textBox.Modified ||
                    Census1880_textBox.Modified ||
                    Census1890_textBox.Modified ||
                    Census1900_textBox.Modified ||
                    Census1910_textBox.Modified ||
                    Census1920_textBox.Modified ||
                    Census1930_textBox.Modified ||
                    Census1940_textBox.Modified ||
                    Census1950_textBox.Modified);
        }
        //****************************************************************************************************************************
        private bool ValidLastName()
        {
            if (m_LastName.Length != 0 || m_MarriedName.Length != 0)
            {
                return true;
            }
            else
            {
                switch (MessageBox.Show("Must have valid Last Name.  Do you wish to exit without update?", "", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes: return false;
                    default: return true;
                }
            }
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            bool bCancel = !SaveBeforeLeaving();
            if (bCancel)
                e.Cancel = bCancel;
            else
                SQL.resetPersonLevel();
        }
        //****************************************************************************************************************************
        private bool SaveBeforeLeaving()
        {
            if (m_bDidSearch)
            {
                return true;
            }
            if ((m_bSelectPersonForPhoto && m_iPersonID != 0 && m_iPersonID != m_iOriginalPersonID) || !PersonChanged() ||
                (m_LastName.Length == 0 && m_MarriedName.Length == 0))
            {
                return true;
            }
            switch (MessageBox.Show("Save Changes?", "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    if (AcceptSimilarNameAlreadyInDatabase())
                        break;
                    else
                    {
                        if (SavePerson())
                            SetAllNameValues();
                        else
                            return false;
                    }
                    break;
                case DialogResult.No:
                    if (m_iPersonID != 0)
                    {
                        SetToUnmodified();
                        //DisplayPerson(m_iPersonID);
                    }
                    break;
                case DialogResult.Cancel:
                    return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool AcceptSimilarNameAlreadyInDatabase()
        {
            if (m_iPersonID != 0 || !SimilarNames_checkBox.Checked)
                return false;
            DataTable Person_tbl = PersonExists();
            if (Person_tbl.Rows.Count != 0)
            {
                switch (MessageBox.Show("Similiar Names Exists In Database.  Do you want to check?", "", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                    {
                        int iPersonID = GetPersonFromGrid(Person_tbl, m_LastName);
                        if (iPersonID != 0)
                        {
                            SetToUnmodified();
                            m_iPersonID = iPersonID;
                            m_iOriginalPersonID = m_iPersonID;
                            DisplayPerson(m_iPersonID);
                            return true;
                        }
                        else
                        {
                            switch (MessageBox.Show("Save Changes?", "", MessageBoxButtons.YesNo))
                            {
                                case DialogResult.Yes:
                                    return false;
                                default:
                                    return true;
                            }
                        }
                    }
                    default:
                    case DialogResult.No:
                        return false;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private DataTable PersonExists()
        {
            DataTable Person_tbl = SQL.DefinePersonTable();
            PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
            if (personName.lastName.Length != 0)
            {
                SQL.PersonExists(Person_tbl, true, U.Person_Table, U.PersonID_col, personName);
            }
            if (MarriedName_textBox.Text.Length != 0)
            {
                personName.lastName = MarriedName_textBox.Text;
                SQL.PersonExists(Person_tbl, true, U.Person_Table, U.PersonID_col, personName);
            }
            return Person_tbl;
        }
        //****************************************************************************************************************************
        public void Save_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            if (m_LastName.Length == 0 && m_MarriedName.Length == 00)
            {
                MessageBox.Show("You must have a value for the Last or Married Name");
                return;
            }
            if (AcceptSimilarNameAlreadyInDatabase())
                return;
            else
            {
                if (SavePerson())
                {
                    DisplayPerson(m_iPersonID);
                }
            }
        }
        //****************************************************************************************************************************
        private void Family_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            if (m_iPersonID == 0)
            {
                return;
            }
            if (SaveBeforeLeaving())
            {
                FFamily Family = new FFamily(m_SQL, m_iPersonID);
                Family.ShowDialog();
                DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        private void UniquePhotos_button_Click(object sender, EventArgs e)
        {
            CPhotoViewerAddMode PhotoViewer = new CPhotoViewerAddMode(ref m_SQL, null, "");
            if (PhotoViewer.PhotoFound() && SaveBeforeLeaving())
            {
                PhotoViewer.ShowDialog();
                DisplayPerson(m_iPersonID);
            }
        }
        //****************************************************************************************************************************
        private void AddSpouseToMarriageTable(int iSpouseID)
        {
            int iLocation_0_PersonID;
            int iLocation_1_PersonID;
            if (m_iSpouseLocationInArray == 1)
            {
                iLocation_0_PersonID = m_iPersonID;
                iLocation_1_PersonID = m_iSpouseID;
            }
            else
            {
                iLocation_0_PersonID = m_iSpouseID;
                iLocation_1_PersonID = m_iPersonID;
            }
            int iFoundIndex = -1;
            bool bFoundSpouse = false;
            foreach (DataRow row in personData.MarriageTable().Rows)
            {
                iFoundIndex++;
                if (row[m_iSpouseLocationInArray].ToInt() == iSpouseID)
                {
                    bFoundSpouse = true;
                    break;
                }
            }
            if (bFoundSpouse)
                m_iCurrentSpouseIndex = iFoundIndex;
            else
            {
                Spouse_textBox.Modified = true;
                personData.MarriageTable().Rows.Add(iLocation_0_PersonID, iLocation_1_PersonID, "", "M");
                m_iNumberSpouses++;
                m_iCurrentSpouseIndex = m_iNumberSpouses - 1;
                if (Female_radioButton.Checked && MarriedName_textBox.Text.ToString().Length == 0)
                {
                    DataRow Spouse_row = SQL.GetPerson(iSpouseID);
                    MarriedName_textBox.Text = Spouse_row[U.LastName_col].ToString();
                }
            }
        }
        //****************************************************************************************************************************
        private bool DoNotSaveChangesBeforeChoosing(string sChoosing)
        {
            if (PersonChanged() || m_iPersonID == 0)
            {
                switch (MessageBox.Show("Save Changes before choosing " + sChoosing + " ?", "", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        if (!SavePerson())
                            return true;
                        else
                            return false;
                    case DialogResult.No:
                        return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private void NewSpouse_Click(object sender, EventArgs e)
        {
            if (DoNotSaveChangesBeforeChoosing("spouse"))
                return;
            string spouseSex = GetOppositeSex();
            string marriedName = "";
            if (spouseSex == "F")
            {
                marriedName = GetMarriedName();
            }
            int iSpouseID = GetAPerson(m_iSpouseID, spouseSex, "", marriedName);
            if (iSpouseID != 0 && iSpouseID != m_iPersonID && iSpouseID != m_iSpouseID)
            {
                m_iSpouseID = iSpouseID;
                AddSpouseToMarriageTable(iSpouseID);
                Spouse_textBox.Modified = true;
                DisplaySpouse();
            }
        }
        //****************************************************************************************************************************
        private string GetMarriedName()
        {
            if (SetPersonSex() == "M")
            {
                return m_LastName;
            }
            return "";
        }
        //****************************************************************************************************************************
        private int GetAPerson(int iPersonID, 
                               string sSex,
                               string sLastName,
                               string sMarriedName)
        {
            FPerson Person = new FPerson(m_SQL, iPersonID, sSex, sLastName, sMarriedName, m_yearOfCensus, true);
            Person.ShowDialog();
            return Person.GetPersonID();
        }
        //****************************************************************************************************************************
        private void AddMarriageToTable(int iPersonID,
                                        int iSpouseID)
        {
            DataTable tbl = personData.MarriageTable();
            object[] objCriteria = new object[] { iPersonID, iSpouseID };
            DataRow row = tbl.Rows.Find(objCriteria);
            if (row == null)
            {
                object[] objCriteria1 = new object[] { iSpouseID, iPersonID };
                row = tbl.Rows.Find(objCriteria1);
            }
            if (row == null)
            {
                if (SQL.GetMarriage(tbl, iPersonID, iSpouseID))                {
                    return;
                }
                DataRow NewRow = tbl.NewRow();
                NewRow[U.PersonID_col] = iPersonID;
                NewRow[U.SpouseID_col] = iSpouseID;
                NewRow[U.DateMarried_col] = "";
                NewRow[U.Divorced_col] = ' ';
                tbl.Rows.Add(NewRow);
            }
        }
        //****************************************************************************************************************************
        private void Father_Click(object sender, EventArgs e)
        {
            if (DoNotSaveChangesBeforeChoosing("father"))
                return;
            int iFatherID = GetAPerson(m_iFatherID, "M", SQL.GetPersonLastName(m_iPersonID), "");
            if (iFatherID != 0 && iFatherID != m_iPersonID && iFatherID != m_iFatherID && UU.NotCircularReference(m_SQL, m_iPersonID, iFatherID))
            {
                personData.UpdatePersonField(U.FatherID_col, iFatherID);
                m_iFatherID = iFatherID;
                if (m_iMotherID != 0)
                    AddMarriageToTable(m_iFatherID, m_iMotherID);
                Father_textBox.Text = SQL.GetPersonName(m_iFatherID);
                Father_textBox.Modified = false;
            }
        }
        //****************************************************************************************************************************
        private void Mother_Click(object sender, EventArgs e)
        {
            if (DoNotSaveChangesBeforeChoosing("mother"))
                return;
            int iMotherID = GetAPerson(m_iMotherID, "F", "", SQL.GetPersonLastName(m_iPersonID));
            if (iMotherID != 0 && iMotherID != m_iPersonID && iMotherID != m_iMotherID && UU.NotCircularReference(m_SQL, m_iPersonID, iMotherID))
            {
                personData.UpdatePersonField(U.MotherID_col, iMotherID);
                m_iMotherID = iMotherID;
                if (m_iFatherID != 0)
                    AddMarriageToTable(m_iFatherID, m_iMotherID);
                Mother_textBox.Text = SQL.GetPersonName(m_iMotherID);
                Mother_textBox.Modified = false;
            }
        }
        //****************************************************************************************************************************
        private void AdditionalSpouse_Click(object sender, EventArgs e)
        {
            if (m_iNumberSpouses > 1)
            {
                m_iCurrentSpouseIndex++;
                if (m_iCurrentSpouseIndex >= m_iNumberSpouses)
                    m_iCurrentSpouseIndex = 0;
                DisplaySpouse();
            }
        }
        //****************************************************************************************************************************
        private bool RemovePerson(int iPersonID)
        {
            if (iPersonID != 0 && MessageBox.Show("Remove this Person?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;
            else
                return false;
        }
        //****************************************************************************************************************************
        private void RemoveSpouse()
        {
            DataTable tbl = personData.MarriageTable();
            object[] objCriteria = new object[] { m_iPersonID, m_iSpouseID };
            DataRow row = tbl.Rows.Find(objCriteria);
            if (row == null)
            {
                object[] objCriteria1 = new object[] { m_iSpouseID, m_iPersonID };
                row = tbl.Rows.Find(objCriteria1);
            }
            if (row == null)
            {
                Spouse_textBox.Text = "";
            }
            else
            {
                row.Delete();
                m_iNumberSpouses--;
                m_iCurrentSpouseIndex = 0;
                m_iSpouseID = 0;
                DisplaySpouse();
            }
        }
        //****************************************************************************************************************************
        private void SpouseRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iSpouseID))
            {
                RemoveSpouse();
                Spouse_textBox.Modified = true;
            }
        }
        //****************************************************************************************************************************
        private void FatherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iFatherID))
            {
                Father_textBox.Text = "";
                Father_textBox.Text = "";
                m_iFatherID = 0;
            }
        }
        //****************************************************************************************************************************
        private void MotherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iMotherID))
            {
                Mother_textBox.Text = "";
                Mother_textBox.Modified = true;
                m_iMotherID = 0;
            }
        }
        //****************************************************************************************************************************
        private void InitializePerson()
        {
            FirstName_textBox.Text = "";
            MiddleName_textBox.Text = "";
            LastName_textBox.Text = "";
            Suffix_comboBox.Text = "";
            Prefix_comboBox.Text = "";
            MarriedName_textBox.Text = "";
            KnownAs_textBox.Text = "";
            Male_radioButton.Checked = false;
            Female_radioButton.Checked = false;
            BornBook_textBox.Text = "";
            BornPage_textBox.Text = "";
            BornDate_textBox.Text = "";
            BornPlace_textBox.Text = "";
            BornHome_textBox.Text = "";
            BornVerified_checkBox.Checked = false;
            BornSource_textBox.Text = "";
            BornFrom_textBox.Text = "";
            DiedBook_textBox.Text = "";
            DiedPage_textBox.Text = "";
            DiedDate_textBox.Text = "";
            DiedPlace_textBox.Text = "";
            DiedHome_textBox.Text = "";
            DiedVerified_checkBox.Checked = false;
            DiedSource_textBox.Text = "";
            BuriedBook_textBox.Text = "";
            BuriedPage_textBox.Text = "";
            BuriedDate_textBox.Text = "";
            BuriedPlace_textBox.Text = "";
            BuriedStone_textBox.Text = "";
            BuriedVerified_checkBox.Checked = false;
            BuriedSource_textBox.Text = "";
            Description_TextBox.Text = "";
            GazetteerRoad_textBox.Text = "";
            Beers1869District_textBox.Text = "";
            McClellan1856District_textBox.Text = "";
            m_iFatherID = 0;
            m_iMotherID = 0;
            Source_textBox.Text = "";
            Spouse_textBox.Text = "";
            Father_textBox.Text = "";
            Mother_textBox.Text = "";
            m_iNumberSpouses = 0;
            m_iCurrentSpouseIndex = 0;
            m_iSpouseLocationInArray = 1;
            m_FirstName = "";
            m_MiddleName = "";
            m_MarriedName = "";
            m_KnownAs = "";
            m_LastName = "";
            m_Suffix = "";
            m_Prefix = "";
            m_iPersonID = 0;
            m_iSpouseID = 0;
            m_iFatherID = 0;
            m_iMotherID = 0;
            m_iOriginalPersonID = 0;
            sDefaultSex = "";
            for (int i = 1790; i <= 1950; i += 10)
            {
                LoadCensusPage(i, 0);
            }
            Properties_listBox.Items.Clear();
            SetToUnmodified();
        }
        //****************************************************************************************************************************
        private void NewPerson_click(object sender, EventArgs e)
        {
            if (SaveBeforeLeaving())
            {
                NewPerson();
            }
        }
        //****************************************************************************************************************************
        private void NewPerson()
        {
            m_iPersonID = 0;
            InitializePerson();
            personData.ClearPersonInfo();
        }
        //****************************************************************************************************************************
        private void Born_textBox_Leave(object sender, System.EventArgs e)
        {
            int stringLength = censusAgeString.Length;
            if (BornSource_textBox.Text.ToString().Length > (stringLength + 4))
            {
                string censusText = BornSource_textBox.Text.ToString().Substring(4, stringLength);
                if (censusText.ToLower() == censusAgeString.ToLower())
                {
                    int ageInYears = getAgeFromCensus();
                    int yearOfCensus = getYearFromCensus();
                    if (ageInYears > 0)
                    {
                        BornDate_textBox.Text = (yearOfCensus - ageInYears).ToString();
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private int getAgeFromCensus()
        {
            int stringLength = censusAgeString.Length;
            return BornSource_textBox.Text.ToString().Substring(4 + stringLength).ToInt();
        }
        //****************************************************************************************************************************
        private int getYearFromCensus()
        {
            return BornSource_textBox.Text.ToString().Substring(0, 4).ToInt();
        }
        //****************************************************************************************************************************
        private void Census_Checked(object sender, System.EventArgs e)
        {
            if (sender.ToString().IndexOf("System.Windows.Forms.TextBox") >= 0)
            {
                TextBox textBox = (TextBox)sender;
                int page = textBox.Text.ToInt();
                if (page < 0)
                {
                    if (!String.IsNullOrEmpty(textBox.Text))
                    {
                        MessageBox.Show("Invalid character in census field");
                    }
                    textBox.Text = "";
                    return;
                }
                if (page > 99)
                {
                    MessageBox.Show("Invalid page number (" + textBox.Text + ") entered");
                    textBox.Text = "";
                    return;
                }
                string name = textBox.Name;
                if (name.IndexOf("Census") == 0)
                {
                    string year = name.Substring(6, 4);
                    if (BornSource_textBox.Text.Length == 0 && BornDate_textBox.Text.Length == 0)
                    {
                        BornSource_textBox.Text = year + censusAgeString;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void FirstName_textBox_Leave(object sender, System.EventArgs e)
        {
            if (Male_radioButton.Checked || Female_radioButton.Checked)
            {
                return;
            }
            char sex = SQL.GetFirstNameSex(FirstName_textBox.Text.ToString());
            if (sex == 'M')
            {
                this.Male_radioButton.Checked = true;
            }
            else if (sex == 'F')
            {
                this.Female_radioButton.Checked = true;
            }
            else
            {
                this.Male_radioButton.Checked = false;
                this.Female_radioButton.Checked = false;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //****************************************************************************************************************************
    }
}