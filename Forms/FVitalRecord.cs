using System;
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
        private eSearchOption SearchBy = eSearchOption.SO_AllNames;
        private int VitalRecordFormHeight = 450;
        private int VitalRecordFormWidthBirthDeath = 840;
        private Button Save_button;
        private RichTextBox Notes_TextBox;
        private Label Notes_label;
        private int m_iVitalRecordID = 0;
        private int m_iPersonID = 0;
        private CSql m_SQL;
        private EVitalRecordType m_eVitalRecordType;
        private Button SearchAll_button;
        private Button SearchPartial_Button;
        private Button StartingWith_button;
        private Button Similar_button;
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
        private string m_MotherFirstName = "";
        private string m_MotherMiddleName = "";
        private string m_MotherLastName = "";
        private string m_MotherSuffix = "";
        private string m_MotherPrefix = "";
        private MenuStrip menuStrip1;
        private int m_iSelectedVitalRecordID = 0;
        private string m_sFamilyName = "";
        private ToolStripMenuItem newRecordToolStripMenuItem;
        private ToolStripMenuItem birthMaleToolStripMenuItem;
        private ToolStripMenuItem birthFemaleToolStripMenuItem;
        private ToolStripMenuItem deathMaleToolStripMenuItem;
        private ToolStripMenuItem deathFemaleToolStripMenuItem;
        private ToolStripMenuItem marriageBrideToolStripMenuItem;
        private ToolStripMenuItem marriageGroomToolStripMenuItem;
        private ToolStripMenuItem CivilUnionPartyAToolStripMenuItem;
        private ToolStripMenuItem CivilUnionPartyBToolStripMenuItem;
        private ToolStripMenuItem burialToolStripMenuItem;
        private CheckBox NameIntegrated_checkBox;
        private CheckBox MotherIntegrated_checkBox;
        private CheckBox FatherIntegrated_checkBox;
        private CheckBox SpouseIntegrated_checkBox;
        private CheckBox SpouseFatherIntegrated_checkBox;
        private CheckBox SpouseMotherIntegrated_checkBox;
        private ToolStripMenuItem integrationToolStripMenuItem;
        private ToolStripMenuItem integrateRecordToolStripMenuItem;
        private ToolStripMenuItem integrateAllRecordsToolStripMenuItem;
        private ToolStripMenuItem integratedPersonToolStripMenuItem;
        private GroupBox Spouse_groupBox;
        private TextBox SpouseLastName_textBox;
        private TextBox SpouseFirstName_textBox;
        private TextBox SpouseMiddleName_textBox;
        private ComboBox SpouseSuffix_comboBox;
        private ComboBox SpousePrefix_comboBox;
        private Label label13;
        private GroupBox Disposition_groupBox;
        private GroupBox SpouseFather_groupBox;
        private TextBox SpouseFatherLastName_textBox;
        private TextBox SpouseFatherFirstName_textBox;
        private TextBox SpouseFatherMiddleName_textBox;
        private ComboBox SpouseFatherSuffix_comboBox;
        private ComboBox SpouseFatherPrefix_comboBox;
        private GroupBox SpouseMother_groupBox;
        private TextBox SpouseMotherLastName_textBox;
        private TextBox SpouseMotherFirstName_textBox;
        private TextBox SpouseMotherMiddleName_textBox;
        private ComboBox SpouseMotherSuffix_comboBox;
        private ComboBox SpouseMotherPrefix_comboBox;
        private RadioButton Buried_radioButton;
        private RadioButton Other_radioButton;
        private RadioButton Cremated_radioButton;
        private Label SpouseLastName_label;
        private Label SpouseFirstName_label;
        private Label SpouseMiddleName_label;
        private Label SpouseSuffix_label;
        private Label SpousePrefix_label;
        private ToolStripMenuItem changeToolStripMenuItem;
        private GroupBox SearchRequest_groupBox;
        private RadioButton Persons_radioButton;
        private RadioButton VitalRecord_radioButton;
        private Button LastMother_button;
        private Button LastSpouse_button;
        private Button LastSpouseMother_button;
        private GroupBox Sex_groupBox;
        private RadioButton Female_radioButton;
        private RadioButton Male_radioButton;
        private bool bDispositionChanged = false;
        private bool bSexChanged = false;
        private bool m_bAbort = false;
        private bool m_bPossibleRecordChangeWithoutSaving = false;
        private RadioButton NonIntegrated_radioButton;
        private bool m_UsePreviousRecordAsDefault = false;
        private RadioButton SearchMother_radioButton;
        private RadioButton SearchFather_radioButton;
        protected bool m_bDidSearch = false;

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
                SearchForPerson("");
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
                             CSql              SQL)
        {
            m_SQL = SQL;
            m_eVitalRecordType = eVitalRecordType;
            InitializeVitalRecord();

            DataTable tbl = Q.t(m_SQL,m_SQL.DefineVitalRecord_Table());
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.VitalRecordID_col] };
            Q.v(m_SQL,m_SQL.GetAllRecordsForPerson(tbl, iPersonID));
            GetVitalRecord(tbl);
            if (m_iVitalRecordID == 0)
                m_bAbort = true;
        }
        //****************************************************************************************************************************
        public FVitalRecord(string sFamilyName,
                            CSql SQL)
        {
            m_SQL = SQL;
            m_sFamilyName = sFamilyName;
            m_eVitalRecordType = EVitalRecordType.eSearch;
            InitializeVitalRecord();
            this.Load += new EventHandler(IntegrateFamily);
            m_bDidSearch = true;
        }
        //****************************************************************************************************************************
        public FVitalRecord(int  iSelectedVitalRecordID,
                            CSql SQL)
        {
            m_SQL = SQL;
            m_eVitalRecordType = EVitalRecordType.eSearch;
            m_iSelectedVitalRecordID = iSelectedVitalRecordID;
            InitializeVitalRecord();
            this.Load += new EventHandler(IntegrateRecord);
            m_bDidSearch = true;
        }
        //****************************************************************************************************************************
        private void IntegrateRecord(System.Object sender, System.EventArgs e)
        {
            DataTable VitalRecord_tbl = new DataTable();
            Q.v(m_SQL, m_SQL.GetVitalRecord(VitalRecord_tbl, m_iSelectedVitalRecordID));
            if (VitalRecord_tbl.Rows.Count == 0)
                return;
            //            this.Show();
            bool bSuccess = true;
            bool bAbort = true;
            DataRow PersonVitalRecord_row = VitalRecord_tbl.Rows[0];
            EVitalRecordType eVitalRecordType = (EVitalRecordType)PersonVitalRecord_row[U.VitalRecordType_col].ToInt();
            DataTable SpouseTbl = Q.t(m_SQL, m_SQL.DefineVitalRecord_Table());
            DataRow SpouseVitalRecord_row = null;
            if (eVitalRecordType.MarriageRecord())
            {
                int iSpouseVitalRecordID = PersonVitalRecord_row[U.SpouseID_col].ToInt();
                Q.v(m_SQL, m_SQL.GetVitalRecord(SpouseTbl, iSpouseVitalRecordID));
                if (SpouseTbl.Rows.Count != 0)
                {
                    SpouseVitalRecord_row = SpouseTbl.Rows[0];
                    if (SpouseVitalRecord_row[U.PersonID_col].ToInt() == 0 ||
                       (eVitalRecordType != EVitalRecordType.eBurial &&
                        SpouseVitalRecord_row[U.FatherFirstName_col].ToString().Length != 0 &&
                        SpouseVitalRecord_row[U.FatherID_col].ToInt() == 0) ||
                       (eVitalRecordType != EVitalRecordType.eBurial &&
                        SpouseVitalRecord_row[U.MotherFirstName_col].ToString().Length != 0 &&
                        SpouseVitalRecord_row[U.MotherID_col].ToInt() == 0))
                    {
                    }
                    else
                        SpouseVitalRecord_row = null;
                }
            }
            bSuccess = IntegrateAllVitalRecords(PersonVitalRecord_row, SpouseVitalRecord_row, ref bAbort);
            SaveIntegratedRecords(VitalRecord_tbl, bAbort, bSuccess);
            this.Close();
        }
        //****************************************************************************************************************************
        private void IntegrateFamily(System.Object sender, System.EventArgs e)
        {
            DataTable VitalRecord_tbl = new DataTable();
            Q.v(m_SQL, m_SQL.SelectAllFamilyRecordsForIntegration(VitalRecord_tbl, m_sFamilyName));
            if (VitalRecord_tbl.Rows.Count == 0)
                return;
//            this.Show();
            bool bSuccess = true;
            bool bAbort = true;
            foreach (DataRow PersonVitalRecord_row in VitalRecord_tbl.Rows)
            {

                EVitalRecordType eVitalRecordType = (EVitalRecordType) PersonVitalRecord_row[U.VitalRecordType_col].ToInt();
                DataTable SpouseTbl = Q.t(m_SQL, m_SQL.DefineVitalRecord_Table());
                DataRow SpouseVitalRecord_row = null;
                if (eVitalRecordType.MarriageRecord())
                {
                    int iSpouseVitalRecordID = PersonVitalRecord_row[U.SpouseID_col].ToInt();
                    Q.v(m_SQL, m_SQL.GetVitalRecord(SpouseTbl, iSpouseVitalRecordID));
                    if (SpouseTbl.Rows.Count != 0)
                    {
                        SpouseVitalRecord_row = SpouseTbl.Rows[0];
                        if (SpouseVitalRecord_row[U.PersonID_col].ToInt() == 0 ||
                           (eVitalRecordType != EVitalRecordType.eBurial &&
                            SpouseVitalRecord_row[U.FatherFirstName_col].ToString().Length != 0 &&
                            SpouseVitalRecord_row[U.FatherID_col].ToInt() == 0) ||
                           (eVitalRecordType != EVitalRecordType.eBurial &&
                            SpouseVitalRecord_row[U.MotherFirstName_col].ToString().Length != 0 &&
                            SpouseVitalRecord_row[U.MotherID_col].ToInt() == 0))
                        {
                        }
                        else
                            SpouseVitalRecord_row = null;
                    }
                }
                bSuccess = IntegrateAllVitalRecords(PersonVitalRecord_row, SpouseVitalRecord_row, ref bAbort);
                if (bAbort || !bSuccess)
                    break;
            }
            SaveIntegratedRecords(VitalRecord_tbl, bAbort, bSuccess);
            this.Close();
        }
        //****************************************************************************************************************************
        private void InitializeVitalRecord()
        {
            InitializeComponent();
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
            m_VitalRecord_tbl = Q.t(m_SQL,m_SQL.DefineVitalRecord_Table());
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
        #region Windows Form Designer generated code
        private System.Windows.Forms.TextBox LastName_textBox;
        private System.Windows.Forms.TextBox FirstName_textBox;
        private System.Windows.Forms.TextBox MiddleName_textBox;
        private System.Windows.Forms.ComboBox Suffix_comboBox;
        private System.Windows.Forms.ComboBox Prefix_comboBox;
        private System.Windows.Forms.TextBox FatherLastName_textBox;
        private System.Windows.Forms.TextBox FatherFirstName_textBox;
        private System.Windows.Forms.TextBox FatherMiddleName_textBox;
        private System.Windows.Forms.ComboBox FatherSuffix_comboBox;
        private System.Windows.Forms.ComboBox FatherPrefix_comboBox;
        private System.Windows.Forms.TextBox MotherLastName_textBox;
        private System.Windows.Forms.TextBox MotherFirstName_textBox;
        private System.Windows.Forms.TextBox MotherMiddleName_textBox;
        private System.Windows.Forms.ComboBox MotherSuffix_comboBox;
        private System.Windows.Forms.ComboBox MotherPrefix_comboBox;
        private System.Windows.Forms.TextBox DateMonth_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DateDay_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DateYear_textBox;
        private System.Windows.Forms.GroupBox Date_groupBox;
        private System.Windows.Forms.GroupBox BookPage_groupBox;
        private System.Windows.Forms.TextBox Page_textBox;
        private System.Windows.Forms.TextBox Book_textBox;
        private System.Windows.Forms.GroupBox Name_groupBox;
        private System.Windows.Forms.GroupBox Mother_groupBox;
        private System.Windows.Forms.GroupBox Father_groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox Age_groupBox;
        private System.Windows.Forms.TextBox AgeDays_textBox;
        private System.Windows.Forms.TextBox AgeMonths_textBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox AgeYears_textBox;
        private System.Windows.Forms.Label label10;
        private GroupBox Burial_groupBox;
        private TextBox LotNumber_textBox;
        private ComboBox Cemetery_comboBox;
        private Label label11;
        private System.Windows.Forms.Label label9;
        //****************************************************************************************************************************
        private void InitializeComponent()
        {
            this.LastName_textBox = new System.Windows.Forms.TextBox();
            this.FirstName_textBox = new System.Windows.Forms.TextBox();
            this.MiddleName_textBox = new System.Windows.Forms.TextBox();
            this.Suffix_comboBox = new System.Windows.Forms.ComboBox();
            this.Prefix_comboBox = new System.Windows.Forms.ComboBox();
            this.FatherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.FatherLastName_textBox = new System.Windows.Forms.TextBox();
            this.FatherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.FatherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.FatherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.MotherLastName_textBox = new System.Windows.Forms.TextBox();
            this.MotherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.MotherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.MotherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.MotherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.DateMonth_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DateDay_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DateYear_textBox = new System.Windows.Forms.TextBox();
            this.Date_groupBox = new System.Windows.Forms.GroupBox();
            this.BookPage_groupBox = new System.Windows.Forms.GroupBox();
            this.Page_textBox = new System.Windows.Forms.TextBox();
            this.Book_textBox = new System.Windows.Forms.TextBox();
            this.Name_groupBox = new System.Windows.Forms.GroupBox();
            this.NameIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.Mother_groupBox = new System.Windows.Forms.GroupBox();
            this.MotherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.Father_groupBox = new System.Windows.Forms.GroupBox();
            this.FatherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Age_groupBox = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.AgeDays_textBox = new System.Windows.Forms.TextBox();
            this.AgeMonths_textBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.AgeYears_textBox = new System.Windows.Forms.TextBox();
            this.Burial_groupBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.Disposition_groupBox = new System.Windows.Forms.GroupBox();
            this.Other_radioButton = new System.Windows.Forms.RadioButton();
            this.Cremated_radioButton = new System.Windows.Forms.RadioButton();
            this.Buried_radioButton = new System.Windows.Forms.RadioButton();
            this.Cemetery_comboBox = new System.Windows.Forms.ComboBox();
            this.LotNumber_textBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Save_button = new System.Windows.Forms.Button();
            this.Notes_TextBox = new System.Windows.Forms.RichTextBox();
            this.Notes_label = new System.Windows.Forms.Label();
            this.SearchAll_button = new System.Windows.Forms.Button();
            this.SearchPartial_Button = new System.Windows.Forms.Button();
            this.StartingWith_button = new System.Windows.Forms.Button();
            this.Similar_button = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.birthMaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.birthFemaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deathMaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deathFemaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marriageBrideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marriageGroomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CivilUnionPartyAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CivilUnionPartyBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.burialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integrateRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integrateAllRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integratedPersonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Spouse_groupBox = new System.Windows.Forms.GroupBox();
            this.SpouseIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.SpouseLastName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFirstName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpousePrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseFather_groupBox = new System.Windows.Forms.GroupBox();
            this.SpouseFatherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.SpouseFatherLastName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFatherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFatherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseFatherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseFatherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseMother_groupBox = new System.Windows.Forms.GroupBox();
            this.SpouseMotherIntegrated_checkBox = new System.Windows.Forms.CheckBox();
            this.SpouseMotherLastName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMotherFirstName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMotherMiddleName_textBox = new System.Windows.Forms.TextBox();
            this.SpouseMotherSuffix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseMotherPrefix_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseLastName_label = new System.Windows.Forms.Label();
            this.SpouseFirstName_label = new System.Windows.Forms.Label();
            this.SpouseMiddleName_label = new System.Windows.Forms.Label();
            this.SpouseSuffix_label = new System.Windows.Forms.Label();
            this.SpousePrefix_label = new System.Windows.Forms.Label();
            this.SearchRequest_groupBox = new System.Windows.Forms.GroupBox();
            this.SearchMother_radioButton = new System.Windows.Forms.RadioButton();
            this.SearchFather_radioButton = new System.Windows.Forms.RadioButton();
            this.NonIntegrated_radioButton = new System.Windows.Forms.RadioButton();
            this.Persons_radioButton = new System.Windows.Forms.RadioButton();
            this.VitalRecord_radioButton = new System.Windows.Forms.RadioButton();
            this.LastMother_button = new System.Windows.Forms.Button();
            this.LastSpouse_button = new System.Windows.Forms.Button();
            this.LastSpouseMother_button = new System.Windows.Forms.Button();
            this.Sex_groupBox = new System.Windows.Forms.GroupBox();
            this.Female_radioButton = new System.Windows.Forms.RadioButton();
            this.Male_radioButton = new System.Windows.Forms.RadioButton();
            this.Date_groupBox.SuspendLayout();
            this.BookPage_groupBox.SuspendLayout();
            this.Name_groupBox.SuspendLayout();
            this.Mother_groupBox.SuspendLayout();
            this.Father_groupBox.SuspendLayout();
            this.Age_groupBox.SuspendLayout();
            this.Burial_groupBox.SuspendLayout();
            this.Disposition_groupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.Spouse_groupBox.SuspendLayout();
            this.SpouseFather_groupBox.SuspendLayout();
            this.SpouseMother_groupBox.SuspendLayout();
            this.SearchRequest_groupBox.SuspendLayout();
            this.Sex_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LastName_textBox
            // 
            this.LastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.LastName_textBox.Name = "LastName_textBox";
            this.LastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.LastName_textBox.TabIndex = 0;
            this.LastName_textBox.Leave += new System.EventHandler(this.LastNameTextBox_Click);
            // 
            // FirstName_textBox
            // 
            this.FirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.FirstName_textBox.Name = "FirstName_textBox";
            this.FirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FirstName_textBox.TabIndex = 2;
            // 
            // MiddleName_textBox
            // 
            this.MiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.MiddleName_textBox.Name = "MiddleName_textBox";
            this.MiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MiddleName_textBox.TabIndex = 3;
            // 
            // Suffix_comboBox
            // 
            this.Suffix_comboBox.FormattingEnabled = true;
            this.Suffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.Suffix_comboBox.Name = "Suffix_comboBox";
            this.Suffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.Suffix_comboBox.TabIndex = 4;
            // 
            // Prefix_comboBox
            // 
            this.Prefix_comboBox.FormattingEnabled = true;
            this.Prefix_comboBox.Location = new System.Drawing.Point(22, 143);
            this.Prefix_comboBox.Name = "Prefix_comboBox";
            this.Prefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.Prefix_comboBox.TabIndex = 5;
            // 
            // FatherPrefix_comboBox
            // 
            this.FatherPrefix_comboBox.FormattingEnabled = true;
            this.FatherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.FatherPrefix_comboBox.Name = "FatherPrefix_comboBox";
            this.FatherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.FatherPrefix_comboBox.TabIndex = 99;
            // 
            // FatherLastName_textBox
            // 
            this.FatherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.FatherLastName_textBox.Name = "FatherLastName_textBox";
            this.FatherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FatherLastName_textBox.TabIndex = 6;
            // 
            // FatherFirstName_textBox
            // 
            this.FatherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.FatherFirstName_textBox.Name = "FatherFirstName_textBox";
            this.FatherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FatherFirstName_textBox.TabIndex = 7;
            // 
            // FatherMiddleName_textBox
            // 
            this.FatherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.FatherMiddleName_textBox.Name = "FatherMiddleName_textBox";
            this.FatherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.FatherMiddleName_textBox.TabIndex = 8;
            // 
            // FatherSuffix_comboBox
            // 
            this.FatherSuffix_comboBox.FormattingEnabled = true;
            this.FatherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.FatherSuffix_comboBox.Name = "FatherSuffix_comboBox";
            this.FatherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.FatherSuffix_comboBox.TabIndex = 9;
            // 
            // MotherLastName_textBox
            // 
            this.MotherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.MotherLastName_textBox.Name = "MotherLastName_textBox";
            this.MotherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MotherLastName_textBox.TabIndex = 11;
            // 
            // MotherFirstName_textBox
            // 
            this.MotherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.MotherFirstName_textBox.Name = "MotherFirstName_textBox";
            this.MotherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MotherFirstName_textBox.TabIndex = 12;
            // 
            // MotherMiddleName_textBox
            // 
            this.MotherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.MotherMiddleName_textBox.Name = "MotherMiddleName_textBox";
            this.MotherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.MotherMiddleName_textBox.TabIndex = 13;
            // 
            // MotherSuffix_comboBox
            // 
            this.MotherSuffix_comboBox.FormattingEnabled = true;
            this.MotherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.MotherSuffix_comboBox.Name = "MotherSuffix_comboBox";
            this.MotherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.MotherSuffix_comboBox.TabIndex = 14;
            // 
            // MotherPrefix_comboBox
            // 
            this.MotherPrefix_comboBox.FormattingEnabled = true;
            this.MotherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.MotherPrefix_comboBox.Name = "MotherPrefix_comboBox";
            this.MotherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.MotherPrefix_comboBox.TabIndex = 15;
            // 
            // DateMonth_textBox
            // 
            this.DateMonth_textBox.Location = new System.Drawing.Point(22, 25);
            this.DateMonth_textBox.Name = "DateMonth_textBox";
            this.DateMonth_textBox.Size = new System.Drawing.Size(22, 20);
            this.DateMonth_textBox.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "/";
            // 
            // DateDay_textBox
            // 
            this.DateDay_textBox.Location = new System.Drawing.Point(65, 25);
            this.DateDay_textBox.Name = "DateDay_textBox";
            this.DateDay_textBox.Size = new System.Drawing.Size(22, 20);
            this.DateDay_textBox.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "/";
            // 
            // DateYear_textBox
            // 
            this.DateYear_textBox.Location = new System.Drawing.Point(108, 25);
            this.DateYear_textBox.Name = "DateYear_textBox";
            this.DateYear_textBox.Size = new System.Drawing.Size(38, 20);
            this.DateYear_textBox.TabIndex = 20;
            // 
            // Date_groupBox
            // 
            this.Date_groupBox.Controls.Add(this.DateMonth_textBox);
            this.Date_groupBox.Controls.Add(this.DateYear_textBox);
            this.Date_groupBox.Controls.Add(this.label4);
            this.Date_groupBox.Controls.Add(this.label5);
            this.Date_groupBox.Controls.Add(this.DateDay_textBox);
            this.Date_groupBox.Location = new System.Drawing.Point(628, 119);
            this.Date_groupBox.Name = "Date_groupBox";
            this.Date_groupBox.Size = new System.Drawing.Size(169, 56);
            this.Date_groupBox.TabIndex = 3;
            this.Date_groupBox.TabStop = false;
            this.Date_groupBox.Text = "Date";
            // 
            // BookPage_groupBox
            // 
            this.BookPage_groupBox.Controls.Add(this.Page_textBox);
            this.BookPage_groupBox.Controls.Add(this.Book_textBox);
            this.BookPage_groupBox.Location = new System.Drawing.Point(628, 49);
            this.BookPage_groupBox.Name = "BookPage_groupBox";
            this.BookPage_groupBox.Size = new System.Drawing.Size(169, 56);
            this.BookPage_groupBox.TabIndex = 2;
            this.BookPage_groupBox.TabStop = false;
            this.BookPage_groupBox.Text = "Book-Page";
            // 
            // Page_textBox
            // 
            this.Page_textBox.Location = new System.Drawing.Point(94, 25);
            this.Page_textBox.Name = "Page_textBox";
            this.Page_textBox.Size = new System.Drawing.Size(52, 20);
            this.Page_textBox.TabIndex = 17;
            // 
            // Book_textBox
            // 
            this.Book_textBox.Location = new System.Drawing.Point(22, 25);
            this.Book_textBox.Name = "Book_textBox";
            this.Book_textBox.Size = new System.Drawing.Size(50, 20);
            this.Book_textBox.TabIndex = 16;
            // 
            // Name_groupBox
            // 
            this.Name_groupBox.Controls.Add(this.NameIntegrated_checkBox);
            this.Name_groupBox.Controls.Add(this.LastName_textBox);
            this.Name_groupBox.Controls.Add(this.FirstName_textBox);
            this.Name_groupBox.Controls.Add(this.MiddleName_textBox);
            this.Name_groupBox.Controls.Add(this.Suffix_comboBox);
            this.Name_groupBox.Controls.Add(this.Prefix_comboBox);
            this.Name_groupBox.Location = new System.Drawing.Point(118, 49);
            this.Name_groupBox.Name = "Name_groupBox";
            this.Name_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Name_groupBox.TabIndex = 0;
            this.Name_groupBox.TabStop = false;
            this.Name_groupBox.Text = "Name";
            // 
            // NameIntegrated_checkBox
            // 
            this.NameIntegrated_checkBox.AutoSize = true;
            this.NameIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NameIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.NameIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.NameIntegrated_checkBox.Name = "NameIntegrated_checkBox";
            this.NameIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.NameIntegrated_checkBox.TabIndex = 82;
            this.NameIntegrated_checkBox.TabStop = false;
            this.NameIntegrated_checkBox.Text = "Integrated";
            this.NameIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // Mother_groupBox
            // 
            this.Mother_groupBox.Controls.Add(this.MotherIntegrated_checkBox);
            this.Mother_groupBox.Controls.Add(this.MotherLastName_textBox);
            this.Mother_groupBox.Controls.Add(this.MotherFirstName_textBox);
            this.Mother_groupBox.Controls.Add(this.MotherMiddleName_textBox);
            this.Mother_groupBox.Controls.Add(this.MotherSuffix_comboBox);
            this.Mother_groupBox.Controls.Add(this.MotherPrefix_comboBox);
            this.Mother_groupBox.Location = new System.Drawing.Point(459, 49);
            this.Mother_groupBox.Name = "Mother_groupBox";
            this.Mother_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Mother_groupBox.TabIndex = 7;
            this.Mother_groupBox.TabStop = false;
            this.Mother_groupBox.Text = "Mother Maiden Name";
            // 
            // MotherIntegrated_checkBox
            // 
            this.MotherIntegrated_checkBox.AutoSize = true;
            this.MotherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MotherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MotherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.MotherIntegrated_checkBox.Name = "MotherIntegrated_checkBox";
            this.MotherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.MotherIntegrated_checkBox.TabIndex = 84;
            this.MotherIntegrated_checkBox.TabStop = false;
            this.MotherIntegrated_checkBox.Text = "Integrated";
            this.MotherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // Father_groupBox
            // 
            this.Father_groupBox.Controls.Add(this.FatherIntegrated_checkBox);
            this.Father_groupBox.Controls.Add(this.FatherLastName_textBox);
            this.Father_groupBox.Controls.Add(this.FatherFirstName_textBox);
            this.Father_groupBox.Controls.Add(this.FatherMiddleName_textBox);
            this.Father_groupBox.Controls.Add(this.FatherSuffix_comboBox);
            this.Father_groupBox.Controls.Add(this.FatherPrefix_comboBox);
            this.Father_groupBox.Location = new System.Drawing.Point(293, 49);
            this.Father_groupBox.Name = "Father_groupBox";
            this.Father_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Father_groupBox.TabIndex = 6;
            this.Father_groupBox.TabStop = false;
            this.Father_groupBox.Text = "Name of Father";
            // 
            // FatherIntegrated_checkBox
            // 
            this.FatherIntegrated_checkBox.AutoSize = true;
            this.FatherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FatherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.FatherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.FatherIntegrated_checkBox.Name = "FatherIntegrated_checkBox";
            this.FatherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.FatherIntegrated_checkBox.TabIndex = 83;
            this.FatherIntegrated_checkBox.TabStop = false;
            this.FatherIntegrated_checkBox.Text = "Integrated";
            this.FatherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 64;
            this.label1.Text = "Middle Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 65;
            this.label3.Text = "Lot No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "First Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 67;
            this.label6.Text = "Suffix";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 68;
            this.label7.Text = "Prefix";
            // 
            // Age_groupBox
            // 
            this.Age_groupBox.Controls.Add(this.label10);
            this.Age_groupBox.Controls.Add(this.label9);
            this.Age_groupBox.Controls.Add(this.AgeDays_textBox);
            this.Age_groupBox.Controls.Add(this.AgeMonths_textBox);
            this.Age_groupBox.Controls.Add(this.label8);
            this.Age_groupBox.Controls.Add(this.AgeYears_textBox);
            this.Age_groupBox.Location = new System.Drawing.Point(628, 189);
            this.Age_groupBox.Name = "Age_groupBox";
            this.Age_groupBox.Size = new System.Drawing.Size(169, 56);
            this.Age_groupBox.TabIndex = 4;
            this.Age_groupBox.TabStop = false;
            this.Age_groupBox.Text = "Age";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(138, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 75;
            this.label10.Text = "Ds";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(96, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 13);
            this.label9.TabIndex = 74;
            this.label9.Text = "Ms";
            // 
            // AgeDays_textBox
            // 
            this.AgeDays_textBox.Location = new System.Drawing.Point(116, 25);
            this.AgeDays_textBox.Name = "AgeDays_textBox";
            this.AgeDays_textBox.Size = new System.Drawing.Size(22, 20);
            this.AgeDays_textBox.TabIndex = 23;
            // 
            // AgeMonths_textBox
            // 
            this.AgeMonths_textBox.Location = new System.Drawing.Point(74, 25);
            this.AgeMonths_textBox.Name = "AgeMonths_textBox";
            this.AgeMonths_textBox.Size = new System.Drawing.Size(22, 20);
            this.AgeMonths_textBox.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 71;
            this.label8.Text = "Ys";
            // 
            // AgeYears_textBox
            // 
            this.AgeYears_textBox.Location = new System.Drawing.Point(22, 25);
            this.AgeYears_textBox.Name = "AgeYears_textBox";
            this.AgeYears_textBox.Size = new System.Drawing.Size(28, 20);
            this.AgeYears_textBox.TabIndex = 21;
            // 
            // Burial_groupBox
            // 
            this.Burial_groupBox.Controls.Add(this.label13);
            this.Burial_groupBox.Controls.Add(this.Disposition_groupBox);
            this.Burial_groupBox.Controls.Add(this.Cemetery_comboBox);
            this.Burial_groupBox.Controls.Add(this.LotNumber_textBox);
            this.Burial_groupBox.Controls.Add(this.label3);
            this.Burial_groupBox.Location = new System.Drawing.Point(820, 49);
            this.Burial_groupBox.Name = "Burial_groupBox";
            this.Burial_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Burial_groupBox.TabIndex = 11;
            this.Burial_groupBox.TabStop = false;
            this.Burial_groupBox.Text = "Burial";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 13);
            this.label13.TabIndex = 67;
            this.label13.Text = "Cemetery";
            // 
            // Disposition_groupBox
            // 
            this.Disposition_groupBox.Controls.Add(this.Other_radioButton);
            this.Disposition_groupBox.Controls.Add(this.Cremated_radioButton);
            this.Disposition_groupBox.Controls.Add(this.Buried_radioButton);
            this.Disposition_groupBox.Location = new System.Drawing.Point(25, 23);
            this.Disposition_groupBox.Name = "Disposition_groupBox";
            this.Disposition_groupBox.Size = new System.Drawing.Size(102, 85);
            this.Disposition_groupBox.TabIndex = 6;
            this.Disposition_groupBox.TabStop = false;
            this.Disposition_groupBox.Text = "Disposition";
            // 
            // Other_radioButton
            // 
            this.Other_radioButton.AutoSize = true;
            this.Other_radioButton.Location = new System.Drawing.Point(6, 59);
            this.Other_radioButton.Name = "Other_radioButton";
            this.Other_radioButton.Size = new System.Drawing.Size(51, 17);
            this.Other_radioButton.TabIndex = 68;
            this.Other_radioButton.Text = "Other";
            this.Other_radioButton.UseVisualStyleBackColor = true;
            // 
            // Cremated_radioButton
            // 
            this.Cremated_radioButton.AutoSize = true;
            this.Cremated_radioButton.Location = new System.Drawing.Point(6, 38);
            this.Cremated_radioButton.Name = "Cremated_radioButton";
            this.Cremated_radioButton.Size = new System.Drawing.Size(70, 17);
            this.Cremated_radioButton.TabIndex = 67;
            this.Cremated_radioButton.Text = "Cremated";
            this.Cremated_radioButton.UseVisualStyleBackColor = true;
            // 
            // Buried_radioButton
            // 
            this.Buried_radioButton.AutoSize = true;
            this.Buried_radioButton.Checked = true;
            this.Buried_radioButton.Location = new System.Drawing.Point(6, 19);
            this.Buried_radioButton.Name = "Buried_radioButton";
            this.Buried_radioButton.Size = new System.Drawing.Size(55, 17);
            this.Buried_radioButton.TabIndex = 66;
            this.Buried_radioButton.TabStop = true;
            this.Buried_radioButton.Text = "Buried";
            this.Buried_radioButton.UseVisualStyleBackColor = true;
            // 
            // Cemetery_comboBox
            // 
            this.Cemetery_comboBox.FormattingEnabled = true;
            this.Cemetery_comboBox.Location = new System.Drawing.Point(25, 137);
            this.Cemetery_comboBox.Name = "Cemetery_comboBox";
            this.Cemetery_comboBox.Size = new System.Drawing.Size(102, 21);
            this.Cemetery_comboBox.TabIndex = 37;
            // 
            // LotNumber_textBox
            // 
            this.LotNumber_textBox.Location = new System.Drawing.Point(83, 164);
            this.LotNumber_textBox.Name = "LotNumber_textBox";
            this.LotNumber_textBox.Size = new System.Drawing.Size(44, 20);
            this.LotNumber_textBox.TabIndex = 38;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 71;
            this.label11.Text = "Last Name";
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(131, 480);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(117, 23);
            this.Save_button.TabIndex = 72;
            this.Save_button.TabStop = false;
            this.Save_button.Text = "Save";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Notes_TextBox
            // 
            this.Notes_TextBox.Location = new System.Drawing.Point(627, 269);
            this.Notes_TextBox.Name = "Notes_TextBox";
            this.Notes_TextBox.Size = new System.Drawing.Size(170, 158);
            this.Notes_TextBox.TabIndex = 36;
            this.Notes_TextBox.Text = "";
            // 
            // Notes_label
            // 
            this.Notes_label.AutoSize = true;
            this.Notes_label.Location = new System.Drawing.Point(625, 253);
            this.Notes_label.Name = "Notes_label";
            this.Notes_label.Size = new System.Drawing.Size(35, 13);
            this.Notes_label.TabIndex = 75;
            this.Notes_label.Text = "Notes";
            // 
            // SearchAll_button
            // 
            this.SearchAll_button.Location = new System.Drawing.Point(306, 480);
            this.SearchAll_button.Name = "SearchAll_button";
            this.SearchAll_button.Size = new System.Drawing.Size(117, 23);
            this.SearchAll_button.TabIndex = 76;
            this.SearchAll_button.TabStop = false;
            this.SearchAll_button.Text = "Search All Records";
            this.SearchAll_button.UseVisualStyleBackColor = true;
            this.SearchAll_button.Click += new System.EventHandler(this.SearchAll_button_Click);
            // 
            // SearchPartial_Button
            // 
            this.SearchPartial_Button.Location = new System.Drawing.Point(306, 510);
            this.SearchPartial_Button.Name = "SearchPartial_Button";
            this.SearchPartial_Button.Size = new System.Drawing.Size(117, 23);
            this.SearchPartial_Button.TabIndex = 77;
            this.SearchPartial_Button.TabStop = false;
            this.SearchPartial_Button.Text = "Partial Name";
            this.SearchPartial_Button.UseVisualStyleBackColor = true;
            this.SearchPartial_Button.Click += new System.EventHandler(this.SearchPartial_button_Click);
            // 
            // StartingWith_button
            // 
            this.StartingWith_button.Location = new System.Drawing.Point(306, 540);
            this.StartingWith_button.Name = "StartingWith_button";
            this.StartingWith_button.Size = new System.Drawing.Size(117, 23);
            this.StartingWith_button.TabIndex = 78;
            this.StartingWith_button.TabStop = false;
            this.StartingWith_button.Text = "Last Starting With";
            this.StartingWith_button.UseVisualStyleBackColor = true;
            this.StartingWith_button.Click += new System.EventHandler(this.SearchStartingWith_button_Click);
            // 
            // Similar_button
            // 
            this.Similar_button.Location = new System.Drawing.Point(306, 570);
            this.Similar_button.Name = "Similar_button";
            this.Similar_button.Size = new System.Drawing.Size(117, 23);
            this.Similar_button.TabIndex = 79;
            this.Similar_button.TabStop = false;
            this.Similar_button.Text = "Similar Names";
            this.Similar_button.UseVisualStyleBackColor = true;
            this.Similar_button.Click += new System.EventHandler(this.SearchSimilar_button_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newRecordToolStripMenuItem,
            this.integrationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1010, 24);
            this.menuStrip1.TabIndex = 80;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newRecordToolStripMenuItem
            // 
            this.newRecordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.birthMaleToolStripMenuItem,
            this.birthFemaleToolStripMenuItem,
            this.deathMaleToolStripMenuItem,
            this.deathFemaleToolStripMenuItem,
            this.marriageBrideToolStripMenuItem,
            this.marriageGroomToolStripMenuItem,
            this.CivilUnionPartyAToolStripMenuItem,
            this.CivilUnionPartyBToolStripMenuItem,
            this.burialToolStripMenuItem,
            this.changeToolStripMenuItem});
            this.newRecordToolStripMenuItem.Name = "newRecordToolStripMenuItem";
            this.newRecordToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.newRecordToolStripMenuItem.Text = "New Record";
            // 
            // birthMaleToolStripMenuItem
            // 
            this.birthMaleToolStripMenuItem.Name = "birthMaleToolStripMenuItem";
            this.birthMaleToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.birthMaleToolStripMenuItem.Text = "Birth-Male";
            this.birthMaleToolStripMenuItem.Click += new System.EventHandler(this.BirthMalebutton_Click);
            // 
            // birthFemaleToolStripMenuItem
            // 
            this.birthFemaleToolStripMenuItem.Name = "birthFemaleToolStripMenuItem";
            this.birthFemaleToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.birthFemaleToolStripMenuItem.Text = "Birth-Female";
            this.birthFemaleToolStripMenuItem.Click += new System.EventHandler(this.BirthFemalebutton_Click);
            // 
            // deathMaleToolStripMenuItem
            // 
            this.deathMaleToolStripMenuItem.Name = "deathMaleToolStripMenuItem";
            this.deathMaleToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.deathMaleToolStripMenuItem.Text = "Death-Male";
            this.deathMaleToolStripMenuItem.Click += new System.EventHandler(this.DeathMalebutton_Click);
            // 
            // deathFemaleToolStripMenuItem
            // 
            this.deathFemaleToolStripMenuItem.Name = "deathFemaleToolStripMenuItem";
            this.deathFemaleToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.deathFemaleToolStripMenuItem.Text = "Death-Female";
            this.deathFemaleToolStripMenuItem.Click += new System.EventHandler(this.DeathFemalebutton_Click);
            // 
            // marriageBrideToolStripMenuItem
            // 
            this.marriageBrideToolStripMenuItem.Name = "marriageBrideToolStripMenuItem";
            this.marriageBrideToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.marriageBrideToolStripMenuItem.Text = "Marriage-Bride";
            this.marriageBrideToolStripMenuItem.Click += new System.EventHandler(this.MarriageBridebutton_Click);
            // 
            // marriageGroomToolStripMenuItem
            // 
            this.marriageGroomToolStripMenuItem.Name = "marriageGroomToolStripMenuItem";
            this.marriageGroomToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.marriageGroomToolStripMenuItem.Text = "Marriage-Groom";
            this.marriageGroomToolStripMenuItem.Click += new System.EventHandler(this.MarriageGroombutton_Click);
            // 
            // CivilUnionPartyAToolStripMenuItem
            // 
            this.CivilUnionPartyAToolStripMenuItem.Name = "CivilUnionPartyAToolStripMenuItem";
            this.CivilUnionPartyAToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.CivilUnionPartyAToolStripMenuItem.Text = "Civil Union-Party A";
            this.CivilUnionPartyAToolStripMenuItem.Click += new System.EventHandler(this.CivilUnionPartyAbutton_Click);
            // 
            // CivilUnionPartyBToolStripMenuItem
            // 
            this.CivilUnionPartyBToolStripMenuItem.Name = "CivilUnionPartyBToolStripMenuItem";
            this.CivilUnionPartyBToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.CivilUnionPartyBToolStripMenuItem.Text = "Civil Union-Party B";
            this.CivilUnionPartyBToolStripMenuItem.Click += new System.EventHandler(this.CivilUnionPartyBbutton_Click);
            // 
            // burialToolStripMenuItem
            // 
            this.burialToolStripMenuItem.Name = "burialToolStripMenuItem";
            this.burialToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.burialToolStripMenuItem.Text = "Burial";
            this.burialToolStripMenuItem.Click += new System.EventHandler(this.Burialbutton_Click);
            // 
            // changeToolStripMenuItem
            // 
            this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
            this.changeToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.changeToolStripMenuItem.Text = "Change";
            this.changeToolStripMenuItem.Click += new System.EventHandler(this.Changebutton_Click);
            // 
            // integrationToolStripMenuItem
            // 
            this.integrationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.integrateRecordToolStripMenuItem,
            this.integrateAllRecordsToolStripMenuItem,
            this.integratedPersonToolStripMenuItem});
            this.integrationToolStripMenuItem.Name = "integrationToolStripMenuItem";
            this.integrationToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.integrationToolStripMenuItem.Text = "Integration";
            // 
            // integrateRecordToolStripMenuItem
            // 
            this.integrateRecordToolStripMenuItem.Name = "integrateRecordToolStripMenuItem";
            this.integrateRecordToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.integrateRecordToolStripMenuItem.Text = "Integrate Record";
            this.integrateRecordToolStripMenuItem.Click += new System.EventHandler(this.Integratebutton_Click);
            // 
            // integrateAllRecordsToolStripMenuItem
            // 
            this.integrateAllRecordsToolStripMenuItem.Name = "integrateAllRecordsToolStripMenuItem";
            this.integrateAllRecordsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.integrateAllRecordsToolStripMenuItem.Text = "Integrate All Records";
            this.integrateAllRecordsToolStripMenuItem.Click += new System.EventHandler(this.IntegrateAllbutton_Click);
            // 
            // integratedPersonToolStripMenuItem
            // 
            this.integratedPersonToolStripMenuItem.Name = "integratedPersonToolStripMenuItem";
            this.integratedPersonToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.integratedPersonToolStripMenuItem.Text = "View Integrated Records";
            this.integratedPersonToolStripMenuItem.Click += new System.EventHandler(this.IntegratedPerson_button_Click);
            // 
            // Spouse_groupBox
            // 
            this.Spouse_groupBox.Controls.Add(this.SpouseIntegrated_checkBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseLastName_textBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseFirstName_textBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseMiddleName_textBox);
            this.Spouse_groupBox.Controls.Add(this.SpouseSuffix_comboBox);
            this.Spouse_groupBox.Controls.Add(this.SpousePrefix_comboBox);
            this.Spouse_groupBox.Location = new System.Drawing.Point(118, 263);
            this.Spouse_groupBox.Name = "Spouse_groupBox";
            this.Spouse_groupBox.Size = new System.Drawing.Size(144, 197);
            this.Spouse_groupBox.TabIndex = 1;
            this.Spouse_groupBox.TabStop = false;
            this.Spouse_groupBox.Text = "Spouse";
            // 
            // SpouseIntegrated_checkBox
            // 
            this.SpouseIntegrated_checkBox.AutoSize = true;
            this.SpouseIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpouseIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SpouseIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.SpouseIntegrated_checkBox.Name = "SpouseIntegrated_checkBox";
            this.SpouseIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.SpouseIntegrated_checkBox.TabIndex = 82;
            this.SpouseIntegrated_checkBox.TabStop = false;
            this.SpouseIntegrated_checkBox.Text = "Integrated";
            this.SpouseIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // SpouseLastName_textBox
            // 
            this.SpouseLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseLastName_textBox.Name = "SpouseLastName_textBox";
            this.SpouseLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseLastName_textBox.TabIndex = 21;
            this.SpouseLastName_textBox.Leave += new System.EventHandler(this.SpouseLastNameTextBox_Click);
            // 
            // SpouseFirstName_textBox
            // 
            this.SpouseFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.SpouseFirstName_textBox.Name = "SpouseFirstName_textBox";
            this.SpouseFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFirstName_textBox.TabIndex = 22;
            // 
            // SpouseMiddleName_textBox
            // 
            this.SpouseMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.SpouseMiddleName_textBox.Name = "SpouseMiddleName_textBox";
            this.SpouseMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMiddleName_textBox.TabIndex = 23;
            // 
            // SpouseSuffix_comboBox
            // 
            this.SpouseSuffix_comboBox.FormattingEnabled = true;
            this.SpouseSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.SpouseSuffix_comboBox.Name = "SpouseSuffix_comboBox";
            this.SpouseSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseSuffix_comboBox.TabIndex = 24;
            // 
            // SpousePrefix_comboBox
            // 
            this.SpousePrefix_comboBox.FormattingEnabled = true;
            this.SpousePrefix_comboBox.Location = new System.Drawing.Point(22, 143);
            this.SpousePrefix_comboBox.Name = "SpousePrefix_comboBox";
            this.SpousePrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpousePrefix_comboBox.TabIndex = 25;
            // 
            // SpouseFather_groupBox
            // 
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherIntegrated_checkBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherLastName_textBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherFirstName_textBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherMiddleName_textBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherSuffix_comboBox);
            this.SpouseFather_groupBox.Controls.Add(this.SpouseFatherPrefix_comboBox);
            this.SpouseFather_groupBox.Location = new System.Drawing.Point(293, 263);
            this.SpouseFather_groupBox.Name = "SpouseFather_groupBox";
            this.SpouseFather_groupBox.Size = new System.Drawing.Size(144, 197);
            this.SpouseFather_groupBox.TabIndex = 8;
            this.SpouseFather_groupBox.TabStop = false;
            this.SpouseFather_groupBox.Text = "Name of Father";
            // 
            // SpouseFatherIntegrated_checkBox
            // 
            this.SpouseFatherIntegrated_checkBox.AutoSize = true;
            this.SpouseFatherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpouseFatherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SpouseFatherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.SpouseFatherIntegrated_checkBox.Name = "SpouseFatherIntegrated_checkBox";
            this.SpouseFatherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.SpouseFatherIntegrated_checkBox.TabIndex = 83;
            this.SpouseFatherIntegrated_checkBox.TabStop = false;
            this.SpouseFatherIntegrated_checkBox.Text = "Integrated";
            this.SpouseFatherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // SpouseFatherLastName_textBox
            // 
            this.SpouseFatherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseFatherLastName_textBox.Name = "SpouseFatherLastName_textBox";
            this.SpouseFatherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFatherLastName_textBox.TabIndex = 26;
            // 
            // SpouseFatherFirstName_textBox
            // 
            this.SpouseFatherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.SpouseFatherFirstName_textBox.Name = "SpouseFatherFirstName_textBox";
            this.SpouseFatherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFatherFirstName_textBox.TabIndex = 27;
            // 
            // SpouseFatherMiddleName_textBox
            // 
            this.SpouseFatherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.SpouseFatherMiddleName_textBox.Name = "SpouseFatherMiddleName_textBox";
            this.SpouseFatherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseFatherMiddleName_textBox.TabIndex = 28;
            // 
            // SpouseFatherSuffix_comboBox
            // 
            this.SpouseFatherSuffix_comboBox.FormattingEnabled = true;
            this.SpouseFatherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.SpouseFatherSuffix_comboBox.Name = "SpouseFatherSuffix_comboBox";
            this.SpouseFatherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseFatherSuffix_comboBox.TabIndex = 29;
            // 
            // SpouseFatherPrefix_comboBox
            // 
            this.SpouseFatherPrefix_comboBox.FormattingEnabled = true;
            this.SpouseFatherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.SpouseFatherPrefix_comboBox.Name = "SpouseFatherPrefix_comboBox";
            this.SpouseFatherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseFatherPrefix_comboBox.TabIndex = 30;
            // 
            // SpouseMother_groupBox
            // 
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherIntegrated_checkBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherLastName_textBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherFirstName_textBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherMiddleName_textBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherSuffix_comboBox);
            this.SpouseMother_groupBox.Controls.Add(this.SpouseMotherPrefix_comboBox);
            this.SpouseMother_groupBox.Location = new System.Drawing.Point(459, 263);
            this.SpouseMother_groupBox.Name = "SpouseMother_groupBox";
            this.SpouseMother_groupBox.Size = new System.Drawing.Size(144, 197);
            this.SpouseMother_groupBox.TabIndex = 9;
            this.SpouseMother_groupBox.TabStop = false;
            this.SpouseMother_groupBox.Text = "Mother Maiden Name";
            // 
            // SpouseMotherIntegrated_checkBox
            // 
            this.SpouseMotherIntegrated_checkBox.AutoSize = true;
            this.SpouseMotherIntegrated_checkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SpouseMotherIntegrated_checkBox.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SpouseMotherIntegrated_checkBox.Location = new System.Drawing.Point(67, 174);
            this.SpouseMotherIntegrated_checkBox.Name = "SpouseMotherIntegrated_checkBox";
            this.SpouseMotherIntegrated_checkBox.Size = new System.Drawing.Size(71, 17);
            this.SpouseMotherIntegrated_checkBox.TabIndex = 84;
            this.SpouseMotherIntegrated_checkBox.TabStop = false;
            this.SpouseMotherIntegrated_checkBox.Text = "Integrated";
            this.SpouseMotherIntegrated_checkBox.UseVisualStyleBackColor = true;
            // 
            // SpouseMotherLastName_textBox
            // 
            this.SpouseMotherLastName_textBox.Location = new System.Drawing.Point(22, 25);
            this.SpouseMotherLastName_textBox.Name = "SpouseMotherLastName_textBox";
            this.SpouseMotherLastName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMotherLastName_textBox.TabIndex = 31;
            // 
            // SpouseMotherFirstName_textBox
            // 
            this.SpouseMotherFirstName_textBox.Location = new System.Drawing.Point(22, 54);
            this.SpouseMotherFirstName_textBox.Name = "SpouseMotherFirstName_textBox";
            this.SpouseMotherFirstName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMotherFirstName_textBox.TabIndex = 32;
            // 
            // SpouseMotherMiddleName_textBox
            // 
            this.SpouseMotherMiddleName_textBox.Location = new System.Drawing.Point(22, 83);
            this.SpouseMotherMiddleName_textBox.Name = "SpouseMotherMiddleName_textBox";
            this.SpouseMotherMiddleName_textBox.Size = new System.Drawing.Size(100, 20);
            this.SpouseMotherMiddleName_textBox.TabIndex = 33;
            // 
            // SpouseMotherSuffix_comboBox
            // 
            this.SpouseMotherSuffix_comboBox.FormattingEnabled = true;
            this.SpouseMotherSuffix_comboBox.Location = new System.Drawing.Point(22, 112);
            this.SpouseMotherSuffix_comboBox.Name = "SpouseMotherSuffix_comboBox";
            this.SpouseMotherSuffix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseMotherSuffix_comboBox.TabIndex = 34;
            // 
            // SpouseMotherPrefix_comboBox
            // 
            this.SpouseMotherPrefix_comboBox.FormattingEnabled = true;
            this.SpouseMotherPrefix_comboBox.Location = new System.Drawing.Point(22, 141);
            this.SpouseMotherPrefix_comboBox.Name = "SpouseMotherPrefix_comboBox";
            this.SpouseMotherPrefix_comboBox.Size = new System.Drawing.Size(60, 21);
            this.SpouseMotherPrefix_comboBox.TabIndex = 35;
            // 
            // SpouseLastName_label
            // 
            this.SpouseLastName_label.AutoSize = true;
            this.SpouseLastName_label.Location = new System.Drawing.Point(31, 290);
            this.SpouseLastName_label.Name = "SpouseLastName_label";
            this.SpouseLastName_label.Size = new System.Drawing.Size(58, 13);
            this.SpouseLastName_label.TabIndex = 86;
            this.SpouseLastName_label.Text = "Last Name";
            // 
            // SpouseFirstName_label
            // 
            this.SpouseFirstName_label.AutoSize = true;
            this.SpouseFirstName_label.Location = new System.Drawing.Point(31, 319);
            this.SpouseFirstName_label.Name = "SpouseFirstName_label";
            this.SpouseFirstName_label.Size = new System.Drawing.Size(57, 13);
            this.SpouseFirstName_label.TabIndex = 87;
            this.SpouseFirstName_label.Text = "First Name";
            // 
            // SpouseMiddleName_label
            // 
            this.SpouseMiddleName_label.AutoSize = true;
            this.SpouseMiddleName_label.Location = new System.Drawing.Point(31, 348);
            this.SpouseMiddleName_label.Name = "SpouseMiddleName_label";
            this.SpouseMiddleName_label.Size = new System.Drawing.Size(69, 13);
            this.SpouseMiddleName_label.TabIndex = 88;
            this.SpouseMiddleName_label.Text = "Middle Name";
            // 
            // SpouseSuffix_label
            // 
            this.SpouseSuffix_label.AutoSize = true;
            this.SpouseSuffix_label.Location = new System.Drawing.Point(31, 377);
            this.SpouseSuffix_label.Name = "SpouseSuffix_label";
            this.SpouseSuffix_label.Size = new System.Drawing.Size(33, 13);
            this.SpouseSuffix_label.TabIndex = 89;
            this.SpouseSuffix_label.Text = "Suffix";
            // 
            // SpousePrefix_label
            // 
            this.SpousePrefix_label.AutoSize = true;
            this.SpousePrefix_label.Location = new System.Drawing.Point(31, 406);
            this.SpousePrefix_label.Name = "SpousePrefix_label";
            this.SpousePrefix_label.Size = new System.Drawing.Size(33, 13);
            this.SpousePrefix_label.TabIndex = 90;
            this.SpousePrefix_label.Text = "Prefix";
            // 
            // SearchRequest_groupBox
            // 
            this.SearchRequest_groupBox.Controls.Add(this.SearchMother_radioButton);
            this.SearchRequest_groupBox.Controls.Add(this.SearchFather_radioButton);
            this.SearchRequest_groupBox.Controls.Add(this.NonIntegrated_radioButton);
            this.SearchRequest_groupBox.Controls.Add(this.Persons_radioButton);
            this.SearchRequest_groupBox.Controls.Add(this.VitalRecord_radioButton);
            this.SearchRequest_groupBox.Location = new System.Drawing.Point(11, 510);
            this.SearchRequest_groupBox.Name = "SearchRequest_groupBox";
            this.SearchRequest_groupBox.Size = new System.Drawing.Size(288, 97);
            this.SearchRequest_groupBox.TabIndex = 91;
            this.SearchRequest_groupBox.TabStop = false;
            this.SearchRequest_groupBox.Text = "Search Options";
            // 
            // SearchMother_radioButton
            // 
            this.SearchMother_radioButton.AutoSize = true;
            this.SearchMother_radioButton.Location = new System.Drawing.Point(6, 63);
            this.SearchMother_radioButton.Name = "SearchMother_radioButton";
            this.SearchMother_radioButton.Size = new System.Drawing.Size(100, 17);
            this.SearchMother_radioButton.TabIndex = 4;
            this.SearchMother_radioButton.Text = "Search Mothers";
            this.SearchMother_radioButton.UseVisualStyleBackColor = true;
            // 
            // SearchFather_radioButton
            // 
            this.SearchFather_radioButton.AutoSize = true;
            this.SearchFather_radioButton.Location = new System.Drawing.Point(6, 42);
            this.SearchFather_radioButton.Name = "SearchFather_radioButton";
            this.SearchFather_radioButton.Size = new System.Drawing.Size(97, 17);
            this.SearchFather_radioButton.TabIndex = 3;
            this.SearchFather_radioButton.Text = "Search Fathers";
            this.SearchFather_radioButton.UseVisualStyleBackColor = true;
            // 
            // NonIntegrated_radioButton
            // 
            this.NonIntegrated_radioButton.AutoSize = true;
            this.NonIntegrated_radioButton.Location = new System.Drawing.Point(137, 37);
            this.NonIntegrated_radioButton.Name = "NonIntegrated_radioButton";
            this.NonIntegrated_radioButton.Size = new System.Drawing.Size(139, 17);
            this.NonIntegrated_radioButton.TabIndex = 2;
            this.NonIntegrated_radioButton.Text = "Non Integrated Records";
            this.NonIntegrated_radioButton.UseVisualStyleBackColor = true;
            // 
            // Persons_radioButton
            // 
            this.Persons_radioButton.AutoSize = true;
            this.Persons_radioButton.Location = new System.Drawing.Point(137, 17);
            this.Persons_radioButton.Name = "Persons_radioButton";
            this.Persons_radioButton.Size = new System.Drawing.Size(100, 17);
            this.Persons_radioButton.TabIndex = 1;
            this.Persons_radioButton.Text = "Search Persons";
            this.Persons_radioButton.UseVisualStyleBackColor = true;
            this.Persons_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // VitalRecord_radioButton
            // 
            this.VitalRecord_radioButton.AutoSize = true;
            this.VitalRecord_radioButton.Checked = true;
            this.VitalRecord_radioButton.Location = new System.Drawing.Point(6, 19);
            this.VitalRecord_radioButton.Name = "VitalRecord_radioButton";
            this.VitalRecord_radioButton.Size = new System.Drawing.Size(125, 17);
            this.VitalRecord_radioButton.TabIndex = 0;
            this.VitalRecord_radioButton.TabStop = true;
            this.VitalRecord_radioButton.Text = "Search Vital Records";
            this.VitalRecord_radioButton.UseVisualStyleBackColor = true;
            this.VitalRecord_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // LastMother_button
            // 
            this.LastMother_button.Location = new System.Drawing.Point(472, 480);
            this.LastMother_button.Name = "LastMother_button";
            this.LastMother_button.Size = new System.Drawing.Size(117, 23);
            this.LastMother_button.TabIndex = 92;
            this.LastMother_button.TabStop = false;
            this.LastMother_button.Text = "Last Mother";
            this.LastMother_button.UseVisualStyleBackColor = true;
            this.LastMother_button.Click += new System.EventHandler(this.LastMother_button_Click);
            // 
            // LastSpouse_button
            // 
            this.LastSpouse_button.Location = new System.Drawing.Point(472, 510);
            this.LastSpouse_button.Name = "LastSpouse_button";
            this.LastSpouse_button.Size = new System.Drawing.Size(117, 23);
            this.LastSpouse_button.TabIndex = 93;
            this.LastSpouse_button.TabStop = false;
            this.LastSpouse_button.Text = "Last Spouse";
            this.LastSpouse_button.UseVisualStyleBackColor = true;
            this.LastSpouse_button.Click += new System.EventHandler(this.LastSpouse_button_Click);
            // 
            // LastSpouseMother_button
            // 
            this.LastSpouseMother_button.Location = new System.Drawing.Point(472, 540);
            this.LastSpouseMother_button.Name = "LastSpouseMother_button";
            this.LastSpouseMother_button.Size = new System.Drawing.Size(117, 23);
            this.LastSpouseMother_button.TabIndex = 94;
            this.LastSpouseMother_button.TabStop = false;
            this.LastSpouseMother_button.Text = "Last Spouse Mother";
            this.LastSpouseMother_button.UseVisualStyleBackColor = true;
            this.LastSpouseMother_button.Click += new System.EventHandler(this.LastSpouseMother_button_Click);
            // 
            // Sex_groupBox
            // 
            this.Sex_groupBox.Controls.Add(this.Female_radioButton);
            this.Sex_groupBox.Controls.Add(this.Male_radioButton);
            this.Sex_groupBox.Location = new System.Drawing.Point(820, 269);
            this.Sex_groupBox.Name = "Sex_groupBox";
            this.Sex_groupBox.Size = new System.Drawing.Size(169, 56);
            this.Sex_groupBox.TabIndex = 10;
            this.Sex_groupBox.TabStop = false;
            this.Sex_groupBox.Text = "Sex";
            // 
            // Female_radioButton
            // 
            this.Female_radioButton.AutoSize = true;
            this.Female_radioButton.Location = new System.Drawing.Point(95, 21);
            this.Female_radioButton.Name = "Female_radioButton";
            this.Female_radioButton.Size = new System.Drawing.Size(59, 17);
            this.Female_radioButton.TabIndex = 1;
            this.Female_radioButton.TabStop = true;
            this.Female_radioButton.Text = "Female";
            this.Female_radioButton.UseVisualStyleBackColor = true;
            this.Female_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // Male_radioButton
            // 
            this.Male_radioButton.AutoSize = true;
            this.Male_radioButton.Location = new System.Drawing.Point(16, 21);
            this.Male_radioButton.Name = "Male_radioButton";
            this.Male_radioButton.Size = new System.Drawing.Size(48, 17);
            this.Male_radioButton.TabIndex = 0;
            this.Male_radioButton.TabStop = true;
            this.Male_radioButton.Text = "Male";
            this.Male_radioButton.UseVisualStyleBackColor = true;
            this.Male_radioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // FVitalRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1010, 654);
            this.Controls.Add(this.Sex_groupBox);
            this.Controls.Add(this.LastSpouseMother_button);
            this.Controls.Add(this.LastSpouse_button);
            this.Controls.Add(this.LastMother_button);
            this.Controls.Add(this.SearchRequest_groupBox);
            this.Controls.Add(this.SpousePrefix_label);
            this.Controls.Add(this.SpouseSuffix_label);
            this.Controls.Add(this.SpouseMiddleName_label);
            this.Controls.Add(this.SpouseFirstName_label);
            this.Controls.Add(this.SpouseLastName_label);
            this.Controls.Add(this.SpouseMother_groupBox);
            this.Controls.Add(this.SpouseFather_groupBox);
            this.Controls.Add(this.Spouse_groupBox);
            this.Controls.Add(this.Similar_button);
            this.Controls.Add(this.StartingWith_button);
            this.Controls.Add(this.SearchPartial_Button);
            this.Controls.Add(this.SearchAll_button);
            this.Controls.Add(this.Notes_label);
            this.Controls.Add(this.Notes_TextBox);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Burial_groupBox);
            this.Controls.Add(this.Age_groupBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Name_groupBox);
            this.Controls.Add(this.Father_groupBox);
            this.Controls.Add(this.Mother_groupBox);
            this.Controls.Add(this.BookPage_groupBox);
            this.Controls.Add(this.Date_groupBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FVitalRecord";
            this.Text = "VitalRecord";
            this.Date_groupBox.ResumeLayout(false);
            this.Date_groupBox.PerformLayout();
            this.BookPage_groupBox.ResumeLayout(false);
            this.BookPage_groupBox.PerformLayout();
            this.Name_groupBox.ResumeLayout(false);
            this.Name_groupBox.PerformLayout();
            this.Mother_groupBox.ResumeLayout(false);
            this.Mother_groupBox.PerformLayout();
            this.Father_groupBox.ResumeLayout(false);
            this.Father_groupBox.PerformLayout();
            this.Age_groupBox.ResumeLayout(false);
            this.Age_groupBox.PerformLayout();
            this.Burial_groupBox.ResumeLayout(false);
            this.Burial_groupBox.PerformLayout();
            this.Disposition_groupBox.ResumeLayout(false);
            this.Disposition_groupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Spouse_groupBox.ResumeLayout(false);
            this.Spouse_groupBox.PerformLayout();
            this.SpouseFather_groupBox.ResumeLayout(false);
            this.SpouseFather_groupBox.PerformLayout();
            this.SpouseMother_groupBox.ResumeLayout(false);
            this.SpouseMother_groupBox.PerformLayout();
            this.SearchRequest_groupBox.ResumeLayout(false);
            this.SearchRequest_groupBox.PerformLayout();
            this.Sex_groupBox.ResumeLayout(false);
            this.Sex_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        //****************************************************************************************************************************
        private System.ComponentModel.IContainer components = null;
        //****************************************************************************************************************************
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //****************************************************************************************************************************
        private void DisplayVitalRecord(int iVitalRecordID)
        {
            m_bDidSearch = false;
            Q.v(m_SQL,m_SQL.GetVitalRecordWithSpouseRecord(m_VitalRecord_tbl, iVitalRecordID));
            if (m_VitalRecord_tbl.Rows.Count > 0)
            {
                DataRow VitalRecord_row = m_VitalRecord_tbl.Rows[0];
                DataRow SpouseVitalRecord_row = SpouseRow(m_VitalRecord_tbl, VitalRecord_row[U.VitalRecordID_col].ToInt(), VitalRecord_row[U.SpouseID_col].ToInt());
                DisplayVitalRecord(VitalRecord_row, SpouseVitalRecord_row);
                SetIntegratedChecked(m_VitalRecord_tbl);
                SetToUnmodified();
                SetAllNameValues();
            }
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
                if (((RadioButton)sender).Name == VitalRecord_radioButton.Name) // This button is now checked
                {
                    SearchAll_button.Text = "Search All Records";
                    LastMother_button.Visible = false;
                    LastSpouse_button.Visible = false;
                    LastSpouseMother_button.Visible = false;
                }
                if (((RadioButton)sender).Name == Persons_radioButton.Name) // This button is now checked
                {
                    SearchAll_button.Text = "Search All Persons";
                    if (m_eVitalRecordType != EVitalRecordType.eBurial)
                    {
                        LastMother_button.Visible = true;
                        if (m_eVitalRecordType.MarriageRecord())
                        {
                            LastSpouse_button.Visible = true;
                            LastSpouseMother_button.Visible = true;
                        }
                    }
                }
                if (((RadioButton)sender).Name == Male_radioButton.Name ||
                    ((RadioButton)sender).Name == Female_radioButton.Name)
                {
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
        private void SetSpouseFieldsFalse()
        {
            Spouse_groupBox.Visible = false;
            SpouseFather_groupBox.Visible = false;
            SpouseMother_groupBox.Visible = false;
            Save_button.Location = new System.Drawing.Point(131, 270);
            SearchAll_button.Location = new System.Drawing.Point(306, 270);
            SearchPartial_Button.Location = new System.Drawing.Point(306, 300);
            StartingWith_button.Location = new System.Drawing.Point(306, 330);
            Similar_button.Location = new System.Drawing.Point(306, 360);
            LastMother_button.Location = new System.Drawing.Point(472, 270);
            ClientSize = new System.Drawing.Size(VitalRecordFormWidthBirthDeath, VitalRecordFormHeight);
            SearchRequest_groupBox.Location = new System.Drawing.Point(11, 309);
            Notes_TextBox.Location = new System.Drawing.Point(627, 269);
            Notes_TextBox.Size = new System.Drawing.Size(170, 140);
            Notes_label.Location = new System.Drawing.Point(625, 253);
        }
        //****************************************************************************************************************************
        private void SetSpouseFieldsTrue(bool bCivalUnion)
        {
            Spouse_groupBox.Visible = true;
            SpouseFather_groupBox.Visible = true;
            SpouseMother_groupBox.Visible = true;
            Sex_groupBox.Visible = bCivalUnion;
            Save_button.Location = new System.Drawing.Point(131, 480);
            SearchAll_button.Location = new System.Drawing.Point(306, 480);
            SearchPartial_Button.Location = new System.Drawing.Point(306, 510);
            StartingWith_button.Location = new System.Drawing.Point(306, 540);
            Similar_button.Location = new System.Drawing.Point(306, 570);
            LastMother_button.Location = new System.Drawing.Point(472, 480);
            BookPage_groupBox.Location = new System.Drawing.Point(628, 49);
            Date_groupBox.Location = new System.Drawing.Point(628, 119);
            Sex_groupBox.Location = new System.Drawing.Point(628, 189);
            SearchRequest_groupBox.Location = new System.Drawing.Point(12, 519);
            Notes_TextBox.Location = new System.Drawing.Point(627, 269);
            Notes_TextBox.Size = new System.Drawing.Size(170, 191);
            Notes_label.Location = new System.Drawing.Point(625, 253);
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
            Age_groupBox.Visible = true;
            SetBirthDeathRecordDisplay();
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
            Age_groupBox.Visible = false;
            Prefix_comboBox.Visible = true;
            Burial_groupBox.Location = new System.Drawing.Point(288, 49);
            BookPage_groupBox.Location = new System.Drawing.Point(458, 49);
            Date_groupBox.Location = new System.Drawing.Point(458, 119);
            Date_groupBox.Text = "Date of Death (M/D/Y)";
            Sex_groupBox.Visible = true;
            Sex_groupBox.Location = new System.Drawing.Point(458, 189);
            LoadCemeteryComboBox();
            Name_groupBox.Text = "Name";
            SetSpouseFieldsFalse();
            Notes_label.Location = new System.Drawing.Point(459,253);
            Notes_TextBox.Location = new System.Drawing.Point(455, 269);
            Notes_TextBox.Size = new System.Drawing.Size(170, 140);
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
            changeToolStripMenuItem.Visible = false;
            Prefix_comboBox.Visible = true;
            bool bCivalUnion = false;
            if (m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyA)
            {
                this.Text = "Civil Union-Party A";
                Date_groupBox.Text = "Date of Civil Union (M/D/Y)";
                Name_groupBox.Text = "Party A";
                Spouse_groupBox.Text = "Party B";
                bCivalUnion = true;
            }
            else
            if (m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyB)
            {
                this.Text = "Civil Union-Party B";
                Date_groupBox.Text = "Date of Civil Union (M/D/Y)";
                Name_groupBox.Text = "Party B";
                Spouse_groupBox.Text = "Party A";
                bCivalUnion = true;
            }
            else
            if (m_eVitalRecordType == EVitalRecordType.eMarriageGroom)
            {
                this.Text = "Marriage-Groom";
                Date_groupBox.Text = "Date of Marriage (M/D/Y)";
                Name_groupBox.Text = "Groom";
                Spouse_groupBox.Text = "Bride";
            }
            else
            {
                this.Text = "Marriage-Bride";
                Date_groupBox.Text = "Date of Marriage (M/D/Y)";
                Name_groupBox.Text = "Bride";
                Spouse_groupBox.Text = "Groom";
            }
            SetSpouseFieldsTrue(bCivalUnion);
            Age_groupBox.Visible = false;
            Age_groupBox.Text = "";
            Burial_groupBox.Visible = false;
            ClientSize = new System.Drawing.Size(980, 638);
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
            Cemetery_comboBox.MaxLength = U.iMaxValueLength;
            LotNumber_textBox.MaxLength = U.iMaxNameLength;
            Notes_TextBox.MaxLength = U.iMaxDescriptionLength;
        }
        //****************************************************************************************************************************
        private void InitializeFields()
        {
            m_UsePreviousRecordAsDefault = false;
//            LastName_textBox.Text = ""; Leave Last Name initialized
            FirstName_textBox.Text = "";
            MiddleName_textBox.Text = "";
            Suffix_comboBox.Text = "";
            Prefix_comboBox.Text = "";
            FatherLastName_textBox.Text = LastName_textBox.Text;
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
            return(!m_bPossibleRecordChangeWithoutSaving ||
                                          NewVitalRecordType == m_eVitalRecordType);
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
            VitalRecord_radioButton.Checked = true;
            Persons_radioButton.Checked = false;
            Prefix_comboBox.Visible = true;
            Father_groupBox.Visible = true;
            Mother_groupBox.Visible = true;
            Sex_groupBox.Visible = false;

            Age_groupBox.Visible = true;
            Burial_groupBox.Visible = true;
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
                    SetBurialRecordDisplay();
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
        private void SetSexRadioButtons(char cSex)
        {
            switch (cSex)
            {
                case 'M': Male_radioButton.Checked = true; break;
                case 'F': Female_radioButton.Checked = true; break;
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
            AgeYears_textBox.Text = row[U.AgeYears_col].ToString();
            AgeMonths_textBox.Text = row[U.AgeMonths_col].ToString();
            AgeDays_textBox.Text = row[U.AgeDays_col].ToString();
            string sSex = row[U.Sex_col].ToString();
            Buried_radioButton.Checked = false;
            Cremated_radioButton.Checked = false;
            Other_radioButton.Checked = false;
            SetDispositionRadioButtons(row[U.Disposition_col].ToString()[0]);
            SetSexRadioButtons(sSex[0]);
            Cemetery_comboBox.Text = row[U.CemeteryName_col].ToString();
            LotNumber_textBox.Text = row[U.LotNumber_col].ToString();
            Notes_TextBox.Text = row[U.Notes_col].ToString();
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
        private bool CheckDate()
        {
            bool bSuccess = true;
            int iDay = DateDay_textBox.Text.ToIntNoError();
            int iYear = DateYear_textBox.Text.ToIntNoError();
            int iMonth = DateMonth_textBox.Text.ToIntNoError();
            if (iDay == U.Exception)
            {
                MessageBox.Show("Invalid Numeric Value For Date Day: " + DateDay_textBox.Text.ToString());
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
                MessageBox.Show("Invalid Numeric Value For Date: " + DateYear_textBox.Text.ToString());
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
                MessageBox.Show("Invalid Numeric Value For Date: " + DateMonth_textBox.Text.ToString());
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
        //****************************************************************************************************************************
        private int GetVitalRecordFromGrid(DataTable tbl)
        {
            CGridVitalRecord GridVitalRecord = new CGridVitalRecord(m_SQL, ref tbl, false);
            GridVitalRecord.ShowDialog();
            int iVitalRecordID = GridVitalRecord.SelectedVitalRecordID;
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
            m_MotherFirstName = MotherFirstName_textBox.Text.TrimString();
            m_MotherMiddleName = MotherMiddleName_textBox.Text.TrimString();
            m_MotherLastName = MotherLastName_textBox.Text.TrimString();
            m_MotherSuffix = MotherSuffix_comboBox.Text.TrimString();
            m_MotherPrefix = MotherPrefix_comboBox.Text.TrimString();
        }
        //****************************************************************************************************************************
        private void SearchForPerson(string sLastName)
        {
//            m_bDidSearch = true;
            SetAllNameValues();
            if (NonIntegrated_radioButton.Checked)
            {
                DataTable tbl = Q.t(m_SQL, m_SQL.DefineVitalRecord_Table());
                tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.VitalRecordID_col] };
                Q.v(m_SQL, m_SQL.PersonsBasedOnNameOptions(tbl, false, U.VitalRecord_Table, U.VitalRecordID_col, SearchBy, m_LastName, m_FirstName,
                                                        m_MiddleName, m_Suffix, m_Prefix, "", ""));
                int iCount = 0;
                foreach (DataRow row in tbl.Rows)
                {
                    EVitalRecordType eVitalRecordType = (EVitalRecordType)row[U.VitalRecordType_col].ToInt();
                    string sLast = row[U.LastName_col].ToString();
                    string sFirst = row[U.FirstName_col].ToString();
                    if (row[U.PersonID_col].ToInt() == 0 ||
                       (eVitalRecordType != EVitalRecordType.eBurial && row[U.FatherFirstName_col].ToString().Length != 0 && row[U.FatherID_col].ToInt() == 0) ||
                       (eVitalRecordType != EVitalRecordType.eBurial && row[U.MotherFirstName_col].ToString().Length != 0 && row[U.MotherID_col].ToInt() == 0))
                    { // include - do nothing
                    }
                    else
                    {
                        row.Delete();
                        iCount++;
                    }

                }
                CGridVitalRecord GridVitalRecord = new CGridVitalRecord(m_SQL, ref tbl, true);
                GridVitalRecord.ShowDialog();
                int iVitalRecordID = GridVitalRecord.SelectedVitalRecordID;
                if (iVitalRecordID > 0)
                {
                    DisplayVitalRecord(iVitalRecordID);
                }
            }
            else
            if (VitalRecord_radioButton.Checked || SearchFather_radioButton.Checked || SearchMother_radioButton.Checked)
            {
                DataTable tbl = Q.t(m_SQL,m_SQL.DefineVitalRecord_Table());
                tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.VitalRecordID_col] };
                if (SearchBy == eSearchOption.SO_AllNames)
                    Q.v(m_SQL, m_SQL.PersonsBasedOnNameOptions(tbl, false, U.VitalRecord_Table, U.VitalRecordID_col, SearchBy, 
                                       m_LastName, m_FirstName, m_MiddleName, m_Suffix, m_Prefix, "", ""));
                else
                if (SearchFather_radioButton.Checked)
                    Q.v(m_SQL, m_SQL.PersonsBasedOnNameOptions(tbl, false, U.VitalRecord_Table, U.VitalRecordID_col, SearchBy, 
                                       m_FatherLastName, m_FatherFirstName, m_FatherMiddleName, m_FatherSuffix, m_FatherPrefix, "", "",
                                       U.FatherLastName_col, U.FatherFirstName_col, U.FatherMiddleName_col));
                else
                if (SearchMother_radioButton.Checked)
                    Q.v(m_SQL, m_SQL.PersonsBasedOnNameOptions(tbl, false, U.VitalRecord_Table, U.VitalRecordID_col, SearchBy, 
                                       m_MotherLastName, m_MotherFirstName, m_MotherMiddleName, m_MotherSuffix, m_MotherPrefix, "", "",
                                       U.MotherLastName_col, U.MotherFirstName_col, U.MotherMiddleName_col));
                else
                    Q.v(m_SQL, m_SQL.PersonsBasedOnNameOptions(tbl, false, U.VitalRecord_Table, U.VitalRecordID_col, SearchBy, 
                                       m_LastName, m_FirstName, m_MiddleName, m_Suffix, m_Prefix, "", ""));
                GetVitalRecord(tbl);
            }
            else
            {
                DataTable tbl = Q.t(m_SQL,m_SQL.DefinePersonTable());
                tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PersonID_col] };
                sLastName = sLastName.TrimString();
                if (sLastName.Length == 0)
                {
                    Q.v(m_SQL,m_SQL.PersonsBasedOnNameOptions(tbl, false, U.Person_Table, U.PersonID_col, SearchBy, m_LastName, m_FirstName,
                                                            m_MiddleName, m_Suffix, m_Prefix, "", ""));
                }
                else
                {
                    Q.v(m_SQL,m_SQL.PersonsBasedOnNameOptions(tbl, true, U.Person_Table, U.PersonID_col, SearchBy, 
                                        sLastName, "", "", "", "", "", ""));
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
        }
        //****************************************************************************************************************************
        private void GetVitalRecord(DataTable tbl)
        {
            int iVitalRecordID = GetVitalRecordFromGrid(tbl);
            if (iVitalRecordID != 0)
            {
                m_iVitalRecordID = iVitalRecordID;
                DisplayVitalRecord(m_iVitalRecordID);
            }
        }
        //****************************************************************************************************************************
        private void SearchAll_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_AllNames;
            SearchForPerson("");
        }
        //****************************************************************************************************************************
        private void SearchStartingWith_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_StartingWith;
            SearchForPerson("");
        }
        //****************************************************************************************************************************
        private void SearchPartial_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson("");
        }
        //****************************************************************************************************************************
        private void SearchSimilar_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_Similar;
            SearchForPerson("");
        }
        //****************************************************************************************************************************
        private void LastMother_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(MotherLastName_textBox.Text.ToString());
        }
        //****************************************************************************************************************************
        private void LastSpouse_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(SpouseLastName_textBox.Text.ToString());
        }
        //****************************************************************************************************************************
        private void LastSpouseMother_button_Click(object sender, System.EventArgs e)
        {
            SearchBy = eSearchOption.SO_PartialNames;
            SearchForPerson(SpouseMotherLastName_textBox.Text.ToString());
        }
        //****************************************************************************************************************************
        private void InitializeNewVitalRecord(EVitalRecordType NewVitalRecordType)
        {
            m_VitalRecord_tbl.Clear();
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
            EVitalRecordType OldVitalRecordType = m_eVitalRecordType;
            switch (m_eVitalRecordType)
            {
                case EVitalRecordType.eBirthMale:
                    m_eVitalRecordType = EVitalRecordType.eBirthFemale;
                    break;
                case EVitalRecordType.eBirthFemale:
                    m_eVitalRecordType = EVitalRecordType.eBirthMale;
                    break;
                case EVitalRecordType.eDeathMale:
                    m_eVitalRecordType = EVitalRecordType.eDeathFemale;
                    break;
                case EVitalRecordType.eDeathFemale:
                    m_eVitalRecordType = EVitalRecordType.eDeathMale;
                    break;
                default: return;
            }
            Q.v(m_SQL, m_SQL.UpdateVitalRecordType(m_iVitalRecordID, OldVitalRecordType, m_eVitalRecordType));
            DisplayVitalRecord(m_iVitalRecordID);
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
            Q.v(m_SQL,m_SQL.selectall(tbl, U.Person_Table, U.NoOrderBy, new NameValuePair(U.PersonID_col, iPersonID)));
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
            Q.v(m_SQL,m_SQL.GetVitalRecordsForPerson(tbl, iSpouseID, U.VitalRecordID_col));
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
        public DataRow GetLastRow(DataTable tbl)
        {
            int iRowIndex = tbl.Rows.Count - 1;
            return tbl.Rows[iRowIndex];
        }
        //****************************************************************************************************************************
        private bool GetPersonIfExists(DataTable Person_tbl,
                                       int iPersonID,
                                       int iVitalRecordPersonType)
        {
            if (iPersonID == 0)
                return false;
            int iCountBefore = Person_tbl.Rows.Count;
            Q.v(m_SQL,m_SQL.GetPerson(Person_tbl, iPersonID));
            if (Person_tbl.Rows.Count == iCountBefore)
                return false;
            else
            {
                int iIndex = Person_tbl.Rows.Count - 1;
                Person_tbl.Rows[iIndex][U.VitalRecordPersonType_col] = iVitalRecordPersonType;
                return true;
            }
        }
        //****************************************************************************************************************************
        public int SimilarPersonExists(DataTable Person_tbl,
                                        string sFormTitle,
                                        eSex Sex,
                                        int iVitalRecordPersonType,
                                        int iPersonID,
                                        bool bNameIntegratedChecked,
                                        string sFirstName,
                                        string sMiddleName,
                                        string sLastName,
                                        string sSuffix,
                                        string sPrefix,
                                        string sMarriedName,
                                        string sNotes)
        {
            if (sLastName.Length == 0 && sFirstName.Length == 0)
                return 0;
            if (sLastName.Length == 0 && sMarriedName.Length == 0)
                return 0;
            if (bNameIntegratedChecked) // be sure there really is an integrated name
            {
                if (GetPersonIfExists(Person_tbl, iPersonID, iVitalRecordPersonType))
                    return iPersonID;
                iPersonID = 0;
                bNameIntegratedChecked = false;
            }
            DataTable SimilarPersonTable = Q.t(m_SQL,m_SQL.DefinePersonTable());
            if (m_SQL.PersonExists(SimilarPersonTable, true, U.Person_Table, U.PersonID_col, sMarriedName, "", sFirstName, sMiddleName, sLastName, sSuffix, sPrefix))
            {
                CGridPerson GridDataViewPerson = new CGridPerson(m_SQL, ref SimilarPersonTable, false, 0, true, sFormTitle);
                GridDataViewPerson.ShowDialog();
                int iNewPersonID = GridDataViewPerson.SelectedPersonID;
                if (iNewPersonID == U.Exception)
                    return U.Exception;
                if (iPersonID != 0 && iPersonID == iNewPersonID)
                {
                    if (GetPersonIfExists(Person_tbl, iPersonID, iVitalRecordPersonType))
                        return iPersonID;
                    else
                        return U.Exception;
                }
                iPersonID = iNewPersonID;
            }
            if (iPersonID == 0)
            {
                DataRow row = Person_tbl.NewRow();
                Q.v(m_SQL,m_SQL.InitializePersonTable(row));
                row[U.PersonID_col] = 0;
                row[U.FirstName_col] = sFirstName;
                row[U.MiddleName_col] = sMiddleName;
                row[U.LastName_col] = sLastName;
                row[U.Suffix_col] = sSuffix;
                row[U.Prefix_col] = sPrefix;
                row[U.MarriedName_col] = sMarriedName;
                row[U.KnownAs_col] = "";
                row[U.Source_col] = U.JamaicaVitalRecords;
                row[U.VitalRecordPersonType_col] = iVitalRecordPersonType;
                row[U.PersonDescription_col] = sNotes;
                row[U.Sex_col] = GetSex(Sex);
                Person_tbl.Rows.Add(row);
            }
            else
            {
                Q.v(m_SQL,m_SQL.GetPerson(Person_tbl, iPersonID));
                if (Person_tbl.Rows.Count == 0)
                    return 0;
                DataRow Person_row = GetLastRow(Person_tbl);
                Person_row[U.VitalRecordPersonType_col] = iVitalRecordPersonType;
                if (Person_row[U.PersonDescription_col].ToString().Length != 0)
                    Person_row[U.PersonDescription_col] += "\n";
                Person_row[U.PersonDescription_col] += sNotes;
                if (Sex != eSex.eUnknown)
                    Person_row[U.Sex_col] = GetSex(Sex);
                if (sMarriedName.Length != 0)
                {
                    string sPersonLastName = Person_row[U.LastName_col].ToString();
                    if (sMarriedName == sPersonLastName)
                    {
                        Person_row[U.MarriedName_col] = Person_row[U.LastName_col];
                        Person_row[U.LastName_col] = sLastName;
                    }
                    else
                        Person_row[U.MarriedName_col] = sMarriedName;
                }
                Person_row[U.FirstName_col] = Q.s(m_SQL, m_SQL.MoveMergeStringToPerson(Person_row[U.FirstName_col].ToString(), sFirstName));
                Person_row[U.MiddleName_col] = Q.s(m_SQL, m_SQL.MoveMergeStringToPerson(Person_row[U.MiddleName_col].ToString(), sMiddleName));
                Person_row[U.LastName_col] = Q.s(m_SQL, m_SQL.MoveMergeStringToPerson(Person_row[U.LastName_col].ToString(), sLastName));
                Person_row[U.Suffix_col] = Q.s(m_SQL, m_SQL.MoveMergeStringToPerson(Person_row[U.Suffix_col].ToString(), sSuffix));
                Person_row[U.Prefix_col] = Q.s(m_SQL, m_SQL.MoveMergeStringToPerson(Person_row[U.Prefix_col].ToString(), sPrefix));
            }
            return iPersonID;
        }
        //****************************************************************************************************************************
        private bool UseSelectedPerson(string sMessageName,
                                       string sPersonName,
                                       string sSelectedName)
        {
            string sMessage = "Person Record " + sMessageName + " is different than the one Just Selected\n\n" +
                              "Person Record " + sMessageName + ":    " + sPersonName + "\n" +
                              "Selected " + sMessageName + ":              " + sSelectedName + "\n\n" +
                              "Use Selected " + sMessageName + "?";
            switch (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo))
            {
               case DialogResult.Yes: return true;
               default: return false;
            }
        }
        //****************************************************************************************************************************
        private bool UseSelectedPerson(string sNameString,
                                       int iPersonID,
                                       int iSelectedID)
        {
            if (iPersonID != 0 && iSelectedID != 0 && iSelectedID != iPersonID)
            {
                if (!UseSelectedPerson(sNameString, Q.s(m_SQL,m_SQL.GetPersonName(iPersonID)), Q.s(m_SQL,m_SQL.GetPersonName(iSelectedID))))
                    return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public bool UseSelectedFatherMother(DataRow Person_row,
                                             int iFatherID,
                                             int iMotherID)
        {
            bool bUseSelectedPerson = true;
            if (!UseSelectedPerson("Father", Person_row[U.FatherID_col].ToInt(), iFatherID))
                bUseSelectedPerson = false;
            if (!UseSelectedPerson("Mother", Person_row[U.MotherID_col].ToInt(), iMotherID))
                bUseSelectedPerson = false;

            Person_row[U.FatherID_col] = iFatherID;
            Person_row[U.MotherID_col] = iMotherID;
            return bUseSelectedPerson;
        }
        //****************************************************************************************************************************
        public bool NotDuplicateOfPpersonID(string sErrorID,
                                             string sPersonSpouse,
                                             int iParentID,
                                             int iPersonID)
        {
            if (iParentID != 0 && iParentID == iPersonID)
            {
                MessageBox.Show(sPersonSpouse + "'s " + sErrorID + " cannot be the same name as the " + sPersonSpouse);
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        public bool IntegratePerson(string sPersonSpouse,
                                    eSex Sex,
                                     DataRow VitalRecord_row,
                                     DataTable Person_tbl,
                                     DataTable Marriage_tbl,
                                     int iVitalRecordPersonType,
                                     bool bPersonIntegrated,
                                     bool bFatherIntegrated,
                                     bool bMotherIntegrated,
                                     string sMarriedName,
                                 out bool bAbort)
        {
            bAbort = false;
            int iPersonID = VitalRecord_row[U.PersonID_col].ToInt();
            string sName = Q.s(m_SQL, m_SQL.BuildNameString(VitalRecord_row[U.FirstName_col].ToString(),
                                            VitalRecord_row[U.MiddleName_col].ToString(),
                                            VitalRecord_row[U.LastName_col].ToString(),
                                            VitalRecord_row[U.Suffix_col].ToString(),
                                            VitalRecord_row[U.Prefix_col].ToString(), 
                                            sMarriedName,""));
            iPersonID = SimilarPersonExists(Person_tbl, sName + " " + sPersonSpouse, Sex, 
                                            iVitalRecordPersonType, iPersonID, bPersonIntegrated,
                                            VitalRecord_row[U.FirstName_col].ToString(),
                                            VitalRecord_row[U.MiddleName_col].ToString(),
                                            VitalRecord_row[U.LastName_col].ToString(),
                                            VitalRecord_row[U.Suffix_col].ToString(),
                                            VitalRecord_row[U.Prefix_col].ToString(), sMarriedName,
                                            VitalRecord_row[U.Notes_col].ToString());
            if (iPersonID == U.Exception)
            {
                bAbort = true;
                return false;
            }
            if (Person_tbl.Rows.Count == 0)
                return false;
            DataRow Person_row = GetLastRow(Person_tbl);

            VitalRecord_row[U.PersonID_col] = iPersonID;
            int iFatherID = VitalRecord_row[U.FatherID_col].ToInt();
            string sFatherLastName = VitalRecord_row[U.FatherLastName_col].ToString();
            EVitalRecordType eVitalRecordType = (EVitalRecordType)VitalRecord_row[U.VitalRecordType_col].ToInt();
            if (eVitalRecordType != EVitalRecordType.eBurial)
            {
                string sGridTitle = eVitalRecordType.GridTitle();
                iFatherID = SimilarPersonExists(Person_tbl, sGridTitle + sName + " Father", eSex.eMale, iVitalRecordPersonType + 1, iFatherID, bFatherIntegrated,
                                 VitalRecord_row[U.FatherFirstName_col].ToString(),
                                 VitalRecord_row[U.FatherMiddleName_col].ToString(), sFatherLastName,
                                 VitalRecord_row[U.FatherSuffix_col].ToString(),
                                 VitalRecord_row[U.FatherPrefix_col].ToString(), "", "");
                if (iFatherID == U.Exception)
                {
                    bAbort = true;
                    return false;
                }
                if (NotDuplicateOfPpersonID("Father", sPersonSpouse, iFatherID, iPersonID))
                    return false;
                VitalRecord_row[U.FatherID_col] = iFatherID;

                int iMotherID = VitalRecord_row[U.MotherID_col].ToInt();
                string sMotherLastName = VitalRecord_row[U.MotherLastName_col].ToString();
                string sMarriedLastName = sFatherLastName;
                iMotherID = SimilarPersonExists(Person_tbl, sGridTitle + sName + " Mother", eSex.eFemale, iVitalRecordPersonType + 2, iMotherID, bMotherIntegrated,
                                 VitalRecord_row[U.MotherFirstName_col].ToString(),
                                 VitalRecord_row[U.MotherMiddleName_col].ToString(), sMotherLastName,
                                 VitalRecord_row[U.MotherSuffix_col].ToString(),
                                 VitalRecord_row[U.MotherPrefix_col].ToString(), sMarriedLastName, "");
                if (iMotherID == U.Exception)
                {
                    bAbort = true;
                    return false;
                }
                if (NotDuplicateOfPpersonID("Mother", sPersonSpouse, iMotherID, iPersonID))
                    return false;
                VitalRecord_row[U.MotherID_col] = iMotherID;
                if (UseSelectedFatherMother(Person_row, iFatherID, iMotherID))
                {
                    Person_row[U.FatherID_col] = iFatherID;
                    Person_row[U.MotherID_col] = iMotherID;
                    if (sFatherLastName.Length != 0 && sMotherLastName.Length != 0 && iFatherID != 0 && iMotherID != 0)
                    {
                        Q.v(m_SQL,m_SQL.GetMarriageRecord(Marriage_tbl, iFatherID, iMotherID,
                                          U.BuildDate(VitalRecord_row[U.DateYear_col].ToInt(),
                                                      VitalRecord_row[U.DateMonth_col].ToInt(),
                                                      VitalRecord_row[U.DateDay_col].ToInt())));
                    }
                    return true;
                }
                else
                    return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public bool CollectCurrentIntegratedRecords(DataRow VitalRecord_row,
                                    DataRow SpouseVitalRecord_row,
                                    DataTable Person_tbl,
                                    DataTable Marriage_tbl,
                                    bool bPersonIntegrated,
                                    bool bFatherIntegrated,
                                    bool bMotherIntegrated,
                                    bool bSpouseIntegrated,
                                    bool bSpouseFatherIntegrated,
                                    bool bSpouseMotherIntegrated,
                                out bool bAbort)
        {
            EVitalRecordType eVitalRecordType = (EVitalRecordType)VitalRecord_row[U.VitalRecordType_col].ToInt();
            string sMarriedName = Q.s(m_SQL,m_SQL.sGetMarriedName(eVitalRecordType, SpouseVitalRecord_row));
            if (!IntegratePerson(eVitalRecordType.RecordTypeTitle(), eVitalRecordType.RecordTypeSex(),
                            VitalRecord_row, Person_tbl, Marriage_tbl, U.PersonType,
                            bPersonIntegrated, bFatherIntegrated, bMotherIntegrated, sMarriedName, out bAbort))
            {
                return false;
            }
            if (eVitalRecordType.MarriageRecord())
            {
                if (SpouseVitalRecord_row == null)
                {
                    MessageBox.Show("This Person Record does not have a spouse Record");
                    return false;
                }
                sMarriedName = Q.s(m_SQL,m_SQL.sGetMarriedName(eVitalRecordType.SpouseRecordType(), VitalRecord_row));
                if (!IntegratePerson(eVitalRecordType.SpouseRecordType().RecordTypeTitle(), eVitalRecordType.SpouseRecordType().RecordTypeSex(),
                                     SpouseVitalRecord_row, Person_tbl, Marriage_tbl,
                                     U.SpouseType, bSpouseIntegrated, bSpouseFatherIntegrated, bSpouseMotherIntegrated, sMarriedName, out bAbort))
                {
                    return false;
                }
                Q.v(m_SQL,m_SQL.GetMarriageRecord(Marriage_tbl, VitalRecord_row[U.PersonID_col].ToInt(), SpouseVitalRecord_row[U.PersonID_col].ToInt(),
                                  U.BuildDate(VitalRecord_row[U.DateYear_col].ToInt(), VitalRecord_row[U.DateMonth_col].ToInt(),
                                  VitalRecord_row[U.DateDay_col].ToInt())));
            }
            else
            if (!bPersonIntegrated && Q.b(m_SQL,m_SQL.RecordAlreadyExists(eVitalRecordType, VitalRecord_row[U.PersonID_col].ToInt())))
            {
                string sVitalRecordType = "";
                switch (eVitalRecordType)
                {
                    case EVitalRecordType.eBirthMale:
                    case EVitalRecordType.eBirthFemale: sVitalRecordType = "Birth"; break;
                    case EVitalRecordType.eDeathMale:
                    case EVitalRecordType.eDeathFemale: sVitalRecordType = "Death"; break;
                    case EVitalRecordType.eBurial: sVitalRecordType = "Burial"; break;
                    default: break;
                }
                MessageBox.Show(sVitalRecordType + " Record Already Exists For This Person");
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool IntegrateRecord(DataRow VitalRecord_row,
                                     DataRow SpouseVitalRecord_row,
                                     bool   bPersonIntegrated,
                                     bool   bFatherIntegrated,
                                     bool   bMotherIntegrated,
                                     bool   bSpouseIntegrated,
                                     bool   bSpouseFatherIntegrated,
                                     bool   bSpouseMotherIntegrated,
                                 ref bool bAbort)
        {
            DataTable Person_tbl = Q.t(m_SQL,m_SQL.DefinePersonTable());
            Person_tbl.Columns.Add(U.VitalRecordPersonType_col, typeof(int));
            DataTable Marriage_tbl = Q.t(m_SQL,m_SQL.DefineMarriageTable());
            if (!CollectCurrentIntegratedRecords(VitalRecord_row, SpouseVitalRecord_row,
                                            Person_tbl, Marriage_tbl, bPersonIntegrated, bFatherIntegrated,
                                            bMotherIntegrated, bSpouseIntegrated, bSpouseFatherIntegrated, bSpouseMotherIntegrated, out bAbort))
            {
                return false;
            }
            else
                return Q.b(m_SQL,m_SQL.IntegrateVitalRecord(VitalRecord_row, SpouseVitalRecord_row, Person_tbl, Marriage_tbl));
        }
        //****************************************************************************************************************************
        private void IntegrateSingleRecord(DataTable VitalRecord_tbl,
                                     bool bPersonIntegrated,
                                     bool bFatherIntegrated,
                                     bool bMotherIntegrated,
                                     bool bSpouseIntegrated,
                                     bool bSpouseFatherIntegrated,
                                     bool bSpouseMotherIntegrated)
        {
            DataRow VitalRecord_row = VitalRecord_tbl.Rows[0];
            DataRow SpouseVitalRecord_row = SpouseRow(VitalRecord_tbl, VitalRecord_row[U.VitalRecordID_col].ToInt(), VitalRecord_row[U.SpouseID_col].ToInt());
            bool bAbort = false;
            if (IntegrateRecord(VitalRecord_row, SpouseVitalRecord_row, bPersonIntegrated, bFatherIntegrated, bMotherIntegrated,
                                      bSpouseIntegrated, bSpouseFatherIntegrated, bSpouseMotherIntegrated, ref bAbort))
            {
                if (m_SQL.SaveIntegratedRecords(VitalRecord_tbl))
                    MessageBox.Show("Integrate Succesful");
                else
                    MessageBox.Show("Integrate Unsuccesful");
                m_iPersonID = m_VitalRecord_tbl.Rows[0][U.PersonID_col].ToInt();
                if (m_iVitalRecordID > 0)
                    SetIntegratedChecked(m_VitalRecord_tbl);
            }
        }
        //****************************************************************************************************************************
        private bool IntegrateAllVitalRecords(DataRow  PersonVitalRecord_row,
                                              DataRow  SpouseVitalRecord_row,
                                              ref bool bAbort)
        {
                m_iVitalRecordID = PersonVitalRecord_row[U.VitalRecordID_col].ToInt();
                EVitalRecordType eVitalRecordType =
                    (EVitalRecordType) PersonVitalRecord_row[U.VitalRecordType_col].ToInt();
                bool bPersonIntegrated = PersonVitalRecord_row[U.PersonID_col].ToInt() != 0;
                bool bFatherIntegrated = true;
                bool bMotherIntegrated = true;
                bool bSpouseIntegrated = true;
                bool bSpouseFatherIntegrated = true;
                bool bSpouseMotherIntegrated = true;
                if (eVitalRecordType != EVitalRecordType.eBurial)
                {
                    bFatherIntegrated = (PersonVitalRecord_row[U.FatherLastName_col].ToString().Length == 0 ||
                                         PersonVitalRecord_row[U.FatherID_col].ToInt() != 0);
                    bMotherIntegrated = (PersonVitalRecord_row[U.MotherLastName_col].ToString().Length == 0 ||
                                         PersonVitalRecord_row[U.MotherID_col].ToInt() != 0);
                    if (eVitalRecordType.MarriageRecord())
                    {
                        bSpouseIntegrated = (SpouseVitalRecord_row == null ||
                                             SpouseVitalRecord_row[U.PersonID_col].ToInt() != 0);
                        bSpouseFatherIntegrated = (SpouseVitalRecord_row == null ||
                                                   SpouseVitalRecord_row[U.FatherLastName_col].ToString().Length == 0 ||
                                                   SpouseVitalRecord_row[U.FatherID_col].ToInt() != 0);
                        bSpouseMotherIntegrated = (SpouseVitalRecord_row == null ||
                                                   SpouseVitalRecord_row[U.MotherLastName_col].ToString().Length == 0 ||
                                                   SpouseVitalRecord_row[U.MotherID_col].ToInt() != 0);
                    }
                }
                if ((!bPersonIntegrated || !bFatherIntegrated || !bMotherIntegrated || !bSpouseIntegrated || !bSpouseFatherIntegrated || !bSpouseMotherIntegrated))
                {
                    DisplayVitalRecord(PersonVitalRecord_row, SpouseVitalRecord_row);
                    if (!IntegrateRecord(PersonVitalRecord_row, SpouseVitalRecord_row, bPersonIntegrated, bFatherIntegrated, bMotherIntegrated,
                                           bSpouseIntegrated, bSpouseFatherIntegrated, bSpouseMotherIntegrated, ref bAbort))
                    {
                        return false;
                    }
                }
                return true;
        }
        //****************************************************************************************************************************
        private void SaveIntegratedRecords(DataTable VitalRecord_tbl,
                                           bool bAbort,
                                           bool bSuccess)
        {
            if (bAbort || bSuccess)
            {
                if (m_SQL.SaveIntegratedRecords(VitalRecord_tbl))
                {
                    if (bAbort)
                        MessageBox.Show("Integrate aborted");
                    else
                        MessageBox.Show("Integrate Successful");
                }
                else
                    MessageBox.Show("Integrate Unsuccessful");
            }
            else
                if (!bSuccess)
                    MessageBox.Show("Integrate Unsuccessful");
        }
        //****************************************************************************************************************************
        private void IntegrateAllVitalRecords()
        {
            DataTable VitalRecord_tbl = Q.t(m_SQL,m_SQL.DefineVitalRecord_Table());
            Q.v(m_SQL,m_SQL.SelectAllRecordsForIntegration(VitalRecord_tbl));
            if (VitalRecord_tbl.Rows.Count == 0)
                return;
            bool bSuccess = true;
            bool bAbort = true;
            foreach (DataRow PersonVitalRecord_row in VitalRecord_tbl.Rows)
            {
                DataRow SpouseVitalRecord_row = SpouseRow(VitalRecord_tbl, 
                                                PersonVitalRecord_row[U.VitalRecordID_col].ToInt(), 
                                                PersonVitalRecord_row[U.SpouseID_col].ToInt());
                bSuccess = IntegrateAllVitalRecords(PersonVitalRecord_row, SpouseVitalRecord_row, ref bAbort);
                if (bAbort || !bSuccess)
                    break;
            }
            SaveIntegratedRecords(VitalRecord_tbl, bAbort, bSuccess);
            if (m_iVitalRecordID > 0)
            {
                Q.v(m_SQL,m_SQL.GetVitalRecordWithSpouseRecord(m_VitalRecord_tbl, m_iVitalRecordID));
                SetIntegratedChecked(m_VitalRecord_tbl);
            }
        }
        //****************************************************************************************************************************
        private void IntegrateAllbutton_Click(object sender, System.EventArgs e)
        {
            IntegrateAllVitalRecords();
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
            if (PersonIntegrated(NameIntegrated_checkBox.Checked, LastName_textBox.Text.ToString(),"") &&
                PersonIntegrated(FatherIntegrated_checkBox.Checked, FatherLastName_textBox.Text.ToString(),"") &&
                PersonIntegrated(MotherIntegrated_checkBox.Checked, MotherLastName_textBox.Text.ToString(), FatherLastName_textBox.Text.ToString()) &&
                PersonIntegrated(SpouseIntegrated_checkBox.Checked, SpouseLastName_textBox.Text.ToString(),"") &&
                PersonIntegrated(SpouseFatherIntegrated_checkBox.Checked, SpouseFatherLastName_textBox.Text.ToString(),"") &&
                PersonIntegrated(SpouseMotherIntegrated_checkBox.Checked, SpouseMotherLastName_textBox.Text.ToString(), SpouseFatherLastName_textBox.ToString()))
            {
                return true;
            }
            else
                return false;
        }
        //****************************************************************************************************************************
        private void Integratebutton_Click(object sender, System.EventArgs e)
        {
            if (VitalRecordChanged())
            {
                if (SaveVitalRecord())
                {
//                    MessageBox.Show("Integration Succesful");
                }
                else
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
            IntegrateSingleRecord(m_VitalRecord_tbl, NameIntegrated_checkBox.Checked,
                            FatherIntegrated_checkBox.Checked, MotherIntegrated_checkBox.Checked,SpouseIntegrated_checkBox.Checked,
                            SpouseFatherIntegrated_checkBox.Checked, SpouseMotherIntegrated_checkBox.Checked);
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
            Book_textBox.Modified = false;
            Page_textBox.Modified = false;
            LotNumber_textBox.Modified = false;
            Notes_TextBox.Modified = false;
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
//                    return iPreviousPersonID;
//                case DialogResult.No:
                    return 0;
                case DialogResult.Cancel:
                default:
                    return U.Exception;
            }
        }
        //****************************************************************************************************************************
        private int CheckLastNameAgainstRecord(int iPersonID,
                                               string sMessageName,
                                               string sNewFirstName,
                                               string sNewMiddleName,
                                               string sNewLasrName,
                                               string sNewSuffix,
                                               string sNewPrefix)
        {
            if (iPersonID == 0)
                return iPersonID;
            DataTable SimilarPersonTable = Q.t(m_SQL,m_SQL.DefinePersonTable());
            SimilarPersonTable.PrimaryKey = new DataColumn[] { SimilarPersonTable.Columns[U.PersonID_col] };
            if (m_SQL.PersonExists(SimilarPersonTable, true, U.Person_Table, U.PersonID_col, "", "", sNewFirstName, sNewMiddleName, sNewLasrName, sNewSuffix, sNewPrefix))
            {
                DataRow row = SimilarPersonTable.Rows.Find(iPersonID);
                if (row == null)
                    iPersonID = NewIntegrationID(sMessageName, iPersonID);
            }
            else
            {
                iPersonID = NewIntegrationID(sMessageName, iPersonID);
            }
            return iPersonID;
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
        private int IllegalName(int     iPersonID,
                                string  sMessageName,
                                bool    bCheckFirstNames,
                                string  sFirstName,
                                string  sMiddleName,
                                string  sLastName,
                                string  sPrefix,
                                string  sSuffix,
                                string  sMarriedName)
        {
            if (bCheckFirstNames && sFirstName.Length == 0 && sMiddleName.Length == 0 && sPrefix.Length == 0 && sSuffix.Length == 0)
                return iPersonID;
            else
            if (sLastName.Length == 0 && sMarriedName.Length == 0)
            {
                MessageBox.Show("You must have a value for the last name of " + sMessageName);
                return U.Exception;
            }
            else
                return CheckLastNameAgainstRecord(iPersonID, sMessageName, sFirstName, sMiddleName, sLastName, sPrefix, sSuffix);
        }
        //****************************************************************************************************************************
        private bool CheckLastNames(DataTable VitalRecord_tbl,
                                    ref int iPersonID,
                                    ref int iFatherID,
                                    ref int iMotherID,
                                    ref int iSpousePersonID,
                                    ref int iSpouseFatherID,
                                    ref int iSpouseMotherID)
        {
            int iOldPersonID = 0;
            int iOldFatherID = 0;
            int iOldMotherID = 0;
            if (m_VitalRecord_tbl.Rows.Count != 0)
            {
                DataRow PersonRow = m_VitalRecord_tbl.Rows[0];
                iOldPersonID = PersonRow[U.PersonID_col].ToInt();
                iOldFatherID = PersonRow[U.FatherID_col].ToInt();
                iOldMotherID = PersonRow[U.MotherID_col].ToInt();
            }
            string sRecordTypeTitle = m_eVitalRecordType.RecordTypeTitle();
            iPersonID = IllegalName(iOldPersonID, sRecordTypeTitle, false,
                                    FirstName_textBox.Text.TrimString(),
                                    MiddleName_textBox.Text.TrimString(),
                                    LastName_textBox.Text.TrimString(),
                                    Suffix_comboBox.Text.TrimString(),
                                    Prefix_comboBox.Text.TrimString(),"");
            if (iPersonID == U.Exception)
            {
                return false;
            }
            iFatherID = IllegalName(iOldFatherID, sRecordTypeTitle + "'s Father", true,
                                      FatherFirstName_textBox.Text.TrimString(),
                                      FatherMiddleName_textBox.Text.TrimString(),
                                      FatherLastName_textBox.Text.TrimString(),
                                      FatherSuffix_comboBox.Text.TrimString(),
                                      FatherPrefix_comboBox.Text.TrimString(),"");
            if (iFatherID == U.Exception)
            {
                return false;
            }
            iMotherID = IllegalName(iOldMotherID, sRecordTypeTitle + "'s Mother", true,
                                      MotherFirstName_textBox.Text.TrimString(),
                                      MotherMiddleName_textBox.Text.TrimString(),
                                      MotherLastName_textBox.Text.TrimString(),
                                      MotherSuffix_comboBox.Text.TrimString(),
                                      MotherPrefix_comboBox.Text.TrimString(),
                                      FatherLastName_textBox.Text.TrimString());
            if (iMotherID == U.Exception)
            {
                return false;
            }
            if (!m_eVitalRecordType.MarriageRecord() || m_VitalRecord_tbl.Rows.Count < 2) 
                return true;
            if (m_VitalRecord_tbl.Rows.Count > 1)
            {
                DataRow SpouseRow = m_VitalRecord_tbl.Rows[1];
                iOldPersonID = SpouseRow[U.PersonID_col].ToInt();
                iOldFatherID = SpouseRow[U.FatherID_col].ToInt();
                iOldMotherID = SpouseRow[U.MotherID_col].ToInt();
            }
            else
            {
                iOldPersonID = 0;
                iOldFatherID = 0;
                iOldMotherID = 0;
            }
            sRecordTypeTitle = m_eVitalRecordType.SpouseRecordType().RecordTypeTitle();
            iSpousePersonID = IllegalName(iOldPersonID, sRecordTypeTitle, false,
                                      SpouseFirstName_textBox.Text.TrimString(),
                                      SpouseMiddleName_textBox.Text.TrimString(),
                                      SpouseLastName_textBox.Text.TrimString(),
                                      SpouseSuffix_comboBox.Text.TrimString(),
                                      SpousePrefix_comboBox.Text.TrimString(),"");
            if (iSpousePersonID == U.Exception)
            {
                return false;
            }
            iSpouseFatherID = IllegalName(iOldFatherID, sRecordTypeTitle + "'s Father", true,
                                     SpouseFatherFirstName_textBox.Text.TrimString(),
                                     SpouseFatherMiddleName_textBox.Text.TrimString(),
                                     SpouseFatherLastName_textBox.Text.TrimString(),
                                     SpouseFatherSuffix_comboBox.Text.TrimString(),
                                     SpouseFatherPrefix_comboBox.Text.TrimString(),"");
            if (iSpouseFatherID == U.Exception)
            {
                return false;
            }
            iSpouseMotherID = IllegalName(iOldMotherID, sRecordTypeTitle + "'s Mother", true,
                                      SpouseMotherFirstName_textBox.Text.TrimString(),
                                      SpouseMotherMiddleName_textBox.Text.TrimString(),
                                      SpouseMotherLastName_textBox.Text.TrimString(),
                                      SpouseMotherSuffix_comboBox.Text.TrimString(),
                                      SpouseMotherPrefix_comboBox.Text.TrimString(),
                                      SpouseFatherLastName_textBox.Text.TrimString());
            if (iSpouseMotherID == U.Exception)
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
            switch (m_eVitalRecordType)
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
        private bool CheckSex()
        {
            if (m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyA ||
                m_eVitalRecordType == EVitalRecordType.eCivilUnionPartyB ||
                m_eVitalRecordType == EVitalRecordType.eBurial)
            {
                if (!Male_radioButton.Checked && !Female_radioButton.Checked)
                {
                    MessageBox.Show("You must Choose Sex before Saving");
                    return false;
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool SaveVitalRecord()
        {
            int iPersonID = 0;
            int iFatherID = 0;
            int iMotherID = 0;
            int iSpousePersonID = 0;
            int iSpouseFatherID = 0;
            int iSpouseMotherID = 0;
            if (VitalRecordChanged() && CheckDate() && CheckAge() && CheckBookPage(Book_textBox.Text.TrimString(), Page_textBox.Text.TrimString()) && CheckSex() &&
                CheckLastNames(m_VitalRecord_tbl,ref iPersonID, ref iFatherID, ref iMotherID, ref iSpousePersonID, ref iSpouseFatherID, ref iSpouseMotherID))
            {
                EVitalRecordType eSpouseRecordsType = m_eVitalRecordType.SpouseRecordType();
                int iVitalRecordID = Q.i(m_SQL,m_SQL.SaveVitalRecord(m_VitalRecord_tbl, m_iVitalRecordID, m_eVitalRecordType,eSpouseRecordsType,
                                 iPersonID, iFatherID, iMotherID, iSpousePersonID, iSpouseFatherID, iSpouseMotherID,
                                 FirstName_textBox.Text.SetNameForDatabase(),
                                 MiddleName_textBox.Text.SetNameForDatabase(),
                                 LastName_textBox.Text.SetNameForDatabase(),
                                 Suffix_comboBox.Text.SetPrefixForDatabase(),
                                 Prefix_comboBox.Text.SetSuffixForDatabase(),
                                 FatherFirstName_textBox.Text.SetNameForDatabase(),
                                 FatherMiddleName_textBox.Text.SetNameForDatabase(),
                                 FatherLastName_textBox.Text.SetNameForDatabase(),
                                 FatherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                 FatherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                 MotherFirstName_textBox.Text.SetNameForDatabase(),
                                 MotherMiddleName_textBox.Text.SetNameForDatabase(),
                                 MotherLastName_textBox.Text.SetNameForDatabase(),
                                 MotherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                 MotherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                 SpouseFirstName_textBox.Text.SetNameForDatabase(),
                                 SpouseMiddleName_textBox.Text.SetNameForDatabase(),
                                 SpouseLastName_textBox.Text.SetNameForDatabase(),
                                 SpouseSuffix_comboBox.Text.SetPrefixForDatabase(),
                                 SpousePrefix_comboBox.Text.SetSuffixForDatabase(),
                                 SpouseFatherFirstName_textBox.Text.SetNameForDatabase(),
                                 SpouseFatherMiddleName_textBox.Text.SetNameForDatabase(),
                                 SpouseFatherLastName_textBox.Text.SetNameForDatabase(),
                                 SpouseFatherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                 SpouseFatherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                 SpouseMotherFirstName_textBox.Text.SetNameForDatabase(),
                                 SpouseMotherMiddleName_textBox.Text.SetNameForDatabase(),
                                 SpouseMotherLastName_textBox.Text.SetNameForDatabase(),
                                 SpouseMotherSuffix_comboBox.Text.SetPrefixForDatabase(),
                                 SpouseMotherPrefix_comboBox.Text.SetSuffixForDatabase(),
                                 DateMonth_textBox.Text.ToInt(), DateDay_textBox.Text.ToInt(), DateYear_textBox.Text.ToInt(),
                                 AgeYears_textBox.Text.ToInt(), AgeMonths_textBox.Text.ToInt(), AgeDays_textBox.Text.ToInt(),
                                 Book_textBox.Text.ToUpper().TrimString(),
                                 Page_textBox.Text.TrimString(),
                                 SetDisposition(),
                                 SetSex(),
                                 Cemetery_comboBox.Text.TrimString(),
                                 LotNumber_textBox.Text.TrimString(),
                                 Notes_TextBox.Text.TrimString()));
                if (iVitalRecordID > 0)
                {
                    m_iVitalRecordID = iVitalRecordID;
                    SetToUnmodified();
                    SetAllNameValues();
                    SetIntegratedChecked(m_VitalRecord_tbl);
                    return true;
                }
                else
                {
                    switch ((SaveVitalRecordErrorCodes)iVitalRecordID)
                    {
                        case SaveVitalRecordErrorCodes.eRecordAlreadyExists:
                            MessageBox.Show("This Record Already Exists");
                            break;
                        case SaveVitalRecordErrorCodes.eUnableToGetPartnerRecord:
                            MessageBox.Show("Unable To Get the Partner Record");
                            break;
                        default:
                            MessageBox.Show("Save Unsuccesful");
                            break;
                    }
                    return false;
                }
            }
            else
                return false;
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
                SpouseMotherLastName_textBox.Modified)
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
            if (m_bDidSearch)
                return false;
            if (FirstName_textBox.Modified ||
                MiddleName_textBox.Modified ||
                LastName_textBox.Modified ||
                U.Modified(Suffix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.Suffix_col) ||
                U.Modified(Prefix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.Prefix_col) ||
                FatherFirstName_textBox.Modified ||
                FatherMiddleName_textBox.Modified ||
                FatherLastName_textBox.Modified ||
                U.Modified(FatherSuffix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.FatherSuffix_col) ||
                U.Modified(FatherPrefix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.FatherPrefix_col) ||
                MotherFirstName_textBox.Modified ||
                MotherMiddleName_textBox.Modified ||
                MotherLastName_textBox.Modified ||
                U.Modified(MotherSuffix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.MotherSuffix_col) ||
                U.Modified(MotherPrefix_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.MotherPrefix_col) ||
                VitalRecordSpouseChanged(m_eVitalRecordType) ||
                DateMonth_textBox.Modified ||
                DateDay_textBox.Modified ||
                DateYear_textBox.Modified ||
                AgeYears_textBox.Modified ||
                AgeMonths_textBox.Modified ||
                AgeDays_textBox.Modified ||
                Book_textBox.Modified ||
                Page_textBox.Modified ||
                bDispositionChanged ||
                bSexChanged ||
                U.Modified(Cemetery_comboBox.Text.TrimString(), m_VitalRecord_tbl, "", U.CemeteryName_col) ||
                LotNumber_textBox.Modified ||
                Notes_TextBox.Modified || 
                (m_VitalRecord_tbl.Rows.Count != 0 && 
                 m_VitalRecord_tbl.Rows[0].RowState != DataRowState.Unchanged))
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        protected bool SaveIfDesired()
        {
            m_bPossibleRecordChangeWithoutSaving = false;
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
                    m_bPossibleRecordChangeWithoutSaving = true;
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
    }
}
