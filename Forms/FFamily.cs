using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class FFamily : Form
    {
        protected CSql m_SQL = null;
        private ListBoxWithDoubleClick Person_listBox;
        private ListBoxWithDoubleClick PersonFather_listBox;
        private ListBoxWithDoubleClick PersonMother_listBox;
        private ListBoxWithDoubleClick Spouse_listBox;
        private ListBoxWithDoubleClick SpouseFather_listBox;
        private ListBoxWithDoubleClick SpouseMother_listBox;
        private ListBoxWithDoubleClick Children_listBox;
        private Label label1;
        private Button Marriage_button;
        private Button ExchangePerson_button;
        private Label Personlabel;
        private Label Spouse_label;
        private Button PersonFather_button;
        private Button PersonMother_button;
        private Button SpouseFather_button;
        private Button SpouseMother_button;
        private Label Married_label;
        protected int m_iPersonID = 0;
        private string m_sSex = "";
        private int m_iPersonFatherID = 0;
        private int m_iPersonMotherID = 0;
        private int m_iSpouseFatherID = 0;
        private int m_iSpouseMotherID = 0;
        private int m_iNumChildren = 0;
        private bool m_bDataLoaded = false;
        private DataTable m_tblSpouses = new DataTable(U.Marriage_Table);
        private DataTable m_tblChildren = new DataTable(U.Person_Table);
        private int m_iNumberSpouses = 0;
        private int m_iCurrentSpouseIndex = 0;
        private int m_iSpouseLocationInArray = 1;
        private TextBox DateMarried_textBox;
        private TextBox PersonParentsDateMarried_textBox;
        private TextBox SpouseParentsDateMarried_textBox;
        private Label label3;
        private Label label4;
        private Label label7;
        private ComboBox MariatalStatus_comboBox;
        private ComboBox PersonParentsMariatalStatus_comboBox;
        private ComboBox SpouseParentsMariatalStatus_comboBox;
        private int m_iSpouseID = 0;
        private bool validDateOnExit = true;
        //****************************************************************************************************************************
        public FFamily(CSql SQL)
        {
            m_SQL = SQL;
            InitializeFamily();
        }
        //****************************************************************************************************************************
        public FFamily(CSql SQL,
                       int iPersonID)
        {
            m_iPersonID = iPersonID;
            m_SQL = SQL;
            InitializeFamily();
        }
        //****************************************************************************************************************************
        protected void InitializeFamily()
        {
            InitializeComponent();
            UU.LoadMariatalStatusComboBox(MariatalStatus_comboBox);
            UU.LoadMariatalStatusComboBox(PersonParentsMariatalStatus_comboBox);
            UU.LoadMariatalStatusComboBox(SpouseParentsMariatalStatus_comboBox);
            InstantiateContextMenus();
            Marriage_button.Visible = false;
            DateMarried_textBox.MaxLength = U.iMaxDateLength;
            PersonParentsDateMarried_textBox.MaxLength = U.iMaxDateLength;
            SpouseParentsDateMarried_textBox.MaxLength = U.iMaxDateLength; ;
            DisplayScreen();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void InstantiateContextMenus()
        {
            this.Person_listBox.ContextMenu = new ContextMenu();
            this.PersonFather_listBox.ContextMenu = new ContextMenu();
            this.PersonMother_listBox.ContextMenu = new ContextMenu();
            this.Spouse_listBox.ContextMenu = new ContextMenu();
            this.SpouseFather_listBox.ContextMenu = new ContextMenu();
            this.SpouseMother_listBox.ContextMenu = new ContextMenu();
            this.Children_listBox.ContextMenu = new ContextMenu();
        }
        //****************************************************************************************************************************
        private void SetupPersonContextMenu()
        {
            this.Person_listBox.ContextMenu.MenuItems.Clear();
            Person_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(NewPersonClick);
            Person_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(Person_Click);
            Person_listBox.ContextMenu.MenuItems.Add("New Person", new EventHandler(NewPersonClick));
            if (m_iPersonID == 0)
            {
                DateMarried_textBox.Enabled = false;
                MariatalStatus_comboBox.Enabled = false;
                PersonParentsDateMarried_textBox.Enabled = false;
                PersonParentsMariatalStatus_comboBox.Enabled = false;
                SpouseParentsDateMarried_textBox.Enabled = false;
                SpouseParentsMariatalStatus_comboBox.Enabled = false;
                Person_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(NewPersonClick);
            }
            else
            {
                Person_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(Person_Click));
                if (m_iSpouseID == 0)
                    Person_listBox.ContextMenu.MenuItems.Add("Add Spouse", new EventHandler(Spouse_Click));
                else
                    Person_listBox.ContextMenu.MenuItems.Add("Add Additional Spouse", new EventHandler(NewSpouse_Click));
                Person_listBox.ContextMenu.MenuItems.Add("Add Child", new EventHandler(ChildrenAdd_Click));
                Person_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(PersonDelete_Clicked));
                Person_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Person_Click);
            }
        }
        //****************************************************************************************************************************
        private void SetupSpouseContextMenu()
        {
            Spouse_listBox.ContextMenu.MenuItems.Clear();
            Spouse_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(NewSpouse_Click);
            Spouse_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(Spouse_Click);
            if (m_iPersonID != 0)
            {
                if (m_iSpouseID == 0)
                {
                    DateMarried_textBox.Enabled = false;
                    MariatalStatus_comboBox.Enabled = false;
                    Spouse_listBox.ContextMenu.MenuItems.Add("New Spouse", new EventHandler(NewSpouse_Click));
                    Spouse_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(NewSpouse_Click);
                }
                else
                {
                    DateMarried_textBox.Enabled = true;
                    MariatalStatus_comboBox.Enabled = true;
                    Spouse_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(Spouse_Click));
                    Spouse_listBox.ContextMenu.MenuItems.Add("Add Child", new EventHandler(ChildrenAdd_Click));
                    Spouse_listBox.ContextMenu.MenuItems.Add("Remove Spouse", new EventHandler(SpouseRemove_Clicked));
                    Spouse_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(SpouseDelete_Clicked));
                    Spouse_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Spouse_Click);
                }
            }
        }
        //****************************************************************************************************************************
        private void SetupPersonFatherContextMenu()
        {
            PersonFather_listBox.ContextMenu.MenuItems.Clear();
            PersonFather_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(PersonFather_Click);
            PersonFather_button.Visible = false;
            if (m_iPersonID != 0)
            {
                PersonParentsDateMarried_textBox.Enabled = false;
                PersonParentsMariatalStatus_comboBox.Enabled = false;
                PersonFather_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(PersonFather_Click);
                if (m_iPersonFatherID == 0)
                    PersonFather_listBox.ContextMenu.MenuItems.Add("New Person Father", new EventHandler(PersonFather_Click));
                else
                {
                    PersonFather_button.Visible = true;
                    PersonFather_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(PersonFather_Click));
                    PersonFather_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(PersonFatherRemove_Clicked));
                    PersonFather_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(PersonFatherDelete_Clicked));
                    if (m_iPersonMotherID != 0)
                    {
                        PersonParentsDateMarried_textBox.Enabled = true;
                        PersonParentsMariatalStatus_comboBox.Enabled = true;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void SetupPersonMotherContextMenu()
        {
            PersonMother_listBox.ContextMenu.MenuItems.Clear();
            PersonMother_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(this.PersonMother_Click);
            PersonMother_button.Visible = false;
            if (m_iPersonID != 0)
            {
                PersonParentsDateMarried_textBox.Enabled = false;
                PersonParentsMariatalStatus_comboBox.Enabled = false;
                PersonMother_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PersonMother_Click);
                if (m_iPersonMotherID == 0)
                    PersonMother_listBox.ContextMenu.MenuItems.Add("New Person Mother", new EventHandler(PersonMother_Click));
                else
                {
                    PersonMother_button.Visible = true;
                    PersonMother_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(PersonMother_Click));
                    PersonMother_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(PersonMotherRemove_Clicked));
                    PersonMother_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(PersonMotherDelete_Clicked));
                    if (m_iPersonFatherID != 0)
                    {
                        PersonParentsDateMarried_textBox.Enabled = true;
                        PersonParentsMariatalStatus_comboBox.Enabled = true;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void SetupSpouseFatherContextMenu()
        {
            SpouseFather_listBox.ContextMenu.MenuItems.Clear();
            SpouseFather_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(this.SpouseFather_Click);
            SpouseFather_button.Visible = false;
            if (m_iPersonID != 0)
            {
                SpouseParentsDateMarried_textBox.Enabled = false;
                SpouseParentsMariatalStatus_comboBox.Enabled = false;
                SpouseFather_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SpouseFather_Click);
                if (m_iSpouseFatherID == 0)
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("New Spouse Father", new EventHandler(SpouseFather_Click));
                else
                {
                    SpouseFather_button.Visible = true;
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(SpouseFather_Click));
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(SpouseFatherRemove_Clicked));
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(SpouseFatherDelete_Clicked));
                    if (m_iSpouseMotherID != 0)
                    {
                        SpouseParentsDateMarried_textBox.Enabled = true;
                        SpouseParentsMariatalStatus_comboBox.Enabled = true;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void SetupSpouseMotherContextMenu()
        {
            SpouseMother_listBox.ContextMenu.MenuItems.Clear();
            SpouseMother_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(this.SpouseMother_Click);
            SpouseMother_button.Visible = false;
            if (m_iPersonID != 0)
            {
                SpouseParentsDateMarried_textBox.Enabled = false;
                SpouseParentsMariatalStatus_comboBox.Enabled = false;
                SpouseMother_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SpouseMother_Click);
                if (m_iSpouseMotherID == 0)
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("New Spouse Mother", new EventHandler(SpouseMother_Click));
                else
                {
                    SpouseMother_button.Visible = true;
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(SpouseMother_Click));
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(SpouseMotherRemove_Clicked));
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(SpouseMotherDelete_Clicked));
                    if (m_iSpouseFatherID != 0)
                    {
                        SpouseParentsDateMarried_textBox.Enabled = true;
                        SpouseParentsMariatalStatus_comboBox.Enabled = true;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void SetupChildrenContextMenu()
        {
            Children_listBox.ContextMenu.MenuItems.Clear();
            if (m_iPersonID != 0 && m_iSpouseID != 0)
            {
                Children_listBox.ContextMenu.MenuItems.Add("Add Child", new EventHandler(ChildrenAdd_Click));
                if (m_iNumChildren != 0)
                {
                    Children_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(ChildrenEdit_Click));
                    Children_listBox.ContextMenu.MenuItems.Add("Remove Child", new EventHandler(ChildrenRemove_Click));
                }
            }
        }
        //****************************************************************************************************************************
        private void SetupSpouseContextMenus()
        {
            SetupSpouseContextMenu();
            SetupSpouseFatherContextMenu();
            SetupSpouseMotherContextMenu();
            SetupChildrenContextMenu();
        }
        //****************************************************************************************************************************
        private void SetupContextMenus()
        {
            SetupPersonContextMenu();
            SetupPersonFatherContextMenu();
            SetupPersonMotherContextMenu();

            SetupSpouseContextMenus();
        }
        //****************************************************************************************************************************
        protected void DisplayScreen()
        {
            DisplayPerson();
            if (m_iPersonID != 0)
            {
                GetMarriages();
                DisplaySpouse();
                PopulateChildrenListBox();
            }
        }
        //****************************************************************************************************************************
        protected void SetMariageButtons()
        {
            if (m_iNumberSpouses > 0)
            {
                ExchangePerson_button.Visible = true;
                if (m_iNumberSpouses > 1)
                    Marriage_button.Visible = true;
                else
                    Marriage_button.Visible = false;
            }
            else
            {
                ExchangePerson_button.Visible = false;
                Marriage_button.Visible = false;
            }
        }
        //****************************************************************************************************************************
        protected void GetMarriages()
        {
            m_iSpouseID = SQL.GetMarriagesID(m_tblSpouses, ref m_iNumberSpouses, out m_iSpouseLocationInArray, m_iPersonID);
            SQL.AddSpouseforChildren(m_tblSpouses, m_iPersonID, m_iSpouseLocationInArray);
            m_iNumberSpouses = m_tblSpouses.Rows.Count;
            m_iCurrentSpouseIndex = 0;
            SetMariageButtons();
        }
        //****************************************************************************************************************************
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        //****************************************************************************************************************************
        private void UpdateMarriageDivorced(int iPersonID,
                                           int iSpouseID,
                                           string sDivorced)
        {
            if (iPersonID != 0 && iSpouseID != 0)
            {
                SQL.UpdateMarriageDivorced(iPersonID, iSpouseID, sDivorced);
            }
        }
        //****************************************************************************************************************************
        private void Divorced_textBox_Leave(object sender, System.EventArgs e)
        {
            UpdateMarriageDivorced(m_iPersonID, m_iSpouseID, MariatalStatus_comboBox.Text.TrimString());
            DisplayParentMarriage(DateMarried_textBox, MariatalStatus_comboBox, m_iPersonID, m_iSpouseID);
            UpdateMarraigeTable(m_iPersonID, m_iSpouseID, DateMarried_textBox.Text, MariatalStatus_comboBox.Text);
        }
        //****************************************************************************************************************************
        private void PersonParentsDivorced_textBox_Leave(object sender, EventArgs e)
        {
            UpdateMarriageDivorced(m_iPersonFatherID, m_iPersonMotherID, PersonParentsMariatalStatus_comboBox.Text.TrimString());
            DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsMariatalStatus_comboBox, m_iPersonFatherID, m_iPersonMotherID);
        }
        //****************************************************************************************************************************
        private void SpouseParentsDivorced_textBox_Leave(object sender, EventArgs e)
        {
            UpdateMarriageDivorced(m_iSpouseFatherID, m_iSpouseMotherID, SpouseParentsMariatalStatus_comboBox.Text.TrimString());
            DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsMariatalStatus_comboBox, m_iSpouseFatherID, m_iSpouseMotherID);
        }
        //****************************************************************************************************************************
        private void UpdateMarriageDate(int iPersonID,
                                        int iSpouseID,
                                        string sDateMarried)
        {
            if (iPersonID != 0 && iSpouseID != 0)
            {
                SQL.UpdateMarriageDate(iPersonID, iSpouseID, sDateMarried);
                DateMarried_textBox.Modified = false;
            }
        }
        //****************************************************************************************************************************
        private void DateMarried_textBox_Leave(object sender, System.EventArgs e)
        {
            if (DateHasText(DateMarried_textBox))
            {
                UpdateMarriageDate(m_iPersonID, m_iSpouseID, DateMarried_textBox.Text);
                DisplayParentMarriage(DateMarried_textBox, MariatalStatus_comboBox, m_iPersonID, m_iSpouseID);
                UpdateMarraigeTable(m_iPersonID, m_iSpouseID, DateMarried_textBox.Text, MariatalStatus_comboBox.Text);
            }
        }
        //****************************************************************************************************************************
        private void UpdateMarraigeTable(int personID, 
                                         int spouseID,
                                         string dateMarried,
                                         string mariatalStatus)
        {
            foreach (DataRow row in m_tblSpouses.Rows)
            {
                UpdateRow(row, personID, spouseID, dateMarried, mariatalStatus);
                UpdateRow(row, spouseID, personID, dateMarried, mariatalStatus);
            }
        }
        //****************************************************************************************************************************
        private void UpdateRow(DataRow row,
                               int personID,
                               int spouseID,
                               string dateMarried,
                               string mariatalStatus)
        {
            if (row[U.PersonID_col].ToInt() == personID && row[U.SpouseID_col].ToInt() == spouseID)
            {
                row[U.DateMarried_col] = dateMarried;
                row[U.Divorced_col] = SQL.DivorcedChar(mariatalStatus);
            }
        }
        //****************************************************************************************************************************
        private void PersonParentsDateMarried_textBox_Leave(object sender, System.EventArgs e)
        {
            if (DateHasText(PersonParentsDateMarried_textBox))
            {
                UpdateMarriageDate(m_iPersonFatherID, m_iPersonMotherID, PersonParentsDateMarried_textBox.Text.TrimString());
                DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsMariatalStatus_comboBox, m_iPersonFatherID, m_iPersonMotherID);
            }
        }
        //****************************************************************************************************************************
        private void SpouseParentsDateMarried_textBox_Leave(object sender, System.EventArgs e)
        {
            if (DateHasText(SpouseParentsDateMarried_textBox))
            {
                UpdateMarriageDate(m_iSpouseFatherID, m_iSpouseMotherID, SpouseParentsDateMarried_textBox.Text.TrimString());
                DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsMariatalStatus_comboBox, m_iSpouseFatherID, m_iSpouseMotherID);
            }
        }
        //****************************************************************************************************************************
        private bool DateHasText(TextBox textBox)
        {
            string marriedDate = textBox.Text.ToString().TrimString();
            if (marriedDate.Length == 0)
            {
                if (textBox.Modified)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                string newDate = UU.ConvertToDateYMD(marriedDate);
                if (newDate.Length == 0)
                {
                    textBox.Focus();
                    validDateOnExit = false;
                    return false;
                }
                else
                {
                    textBox.Text = newDate;
                    return true;
                }
            }
        }
        //****************************************************************************************************************************
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.Marriage_button = new System.Windows.Forms.Button();
            this.ExchangePerson_button = new System.Windows.Forms.Button();
            this.Personlabel = new System.Windows.Forms.Label();
            this.Spouse_label = new System.Windows.Forms.Label();
            this.PersonFather_button = new System.Windows.Forms.Button();
            this.PersonMother_button = new System.Windows.Forms.Button();
            this.SpouseFather_button = new System.Windows.Forms.Button();
            this.SpouseMother_button = new System.Windows.Forms.Button();
            this.Married_label = new System.Windows.Forms.Label();
            this.DateMarried_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SpouseParentsDateMarried_textBox = new System.Windows.Forms.TextBox();
            this.PersonParentsDateMarried_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.MariatalStatus_comboBox = new System.Windows.Forms.ComboBox();
            this.PersonParentsMariatalStatus_comboBox = new System.Windows.Forms.ComboBox();
            this.SpouseParentsMariatalStatus_comboBox = new System.Windows.Forms.ComboBox();
            this.Children_listBox = new Utilities.ListBoxWithDoubleClick();
            this.SpouseMother_listBox = new Utilities.ListBoxWithDoubleClick();
            this.SpouseFather_listBox = new Utilities.ListBoxWithDoubleClick();
            this.Spouse_listBox = new Utilities.ListBoxWithDoubleClick();
            this.PersonMother_listBox = new Utilities.ListBoxWithDoubleClick();
            this.PersonFather_listBox = new Utilities.ListBoxWithDoubleClick();
            this.Person_listBox = new Utilities.ListBoxWithDoubleClick();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(672, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Parents";
            // 
            // Marriage_button
            // 
            this.Marriage_button.Location = new System.Drawing.Point(442, 311);
            this.Marriage_button.Name = "Marriage_button";
            this.Marriage_button.Size = new System.Drawing.Size(52, 23);
            this.Marriage_button.TabIndex = 10;
            this.Marriage_button.Text = "Another";
            this.Marriage_button.UseVisualStyleBackColor = true;
            this.Marriage_button.Click += new System.EventHandler(this.Marriage_button_Click);
            // 
            // ExchangePerson_button
            // 
            this.ExchangePerson_button.Location = new System.Drawing.Point(107, 382);
            this.ExchangePerson_button.Name = "ExchangePerson_button";
            this.ExchangePerson_button.Size = new System.Drawing.Size(28, 20);
            this.ExchangePerson_button.TabIndex = 11;
            this.ExchangePerson_button.Text = "^";
            this.ExchangePerson_button.UseVisualStyleBackColor = true;
            this.ExchangePerson_button.Click += new System.EventHandler(this.ExchangePerson_button_Click);
            // 
            // Personlabel
            // 
            this.Personlabel.AutoSize = true;
            this.Personlabel.Location = new System.Drawing.Point(150, 83);
            this.Personlabel.Name = "Personlabel";
            this.Personlabel.Size = new System.Drawing.Size(40, 13);
            this.Personlabel.TabIndex = 12;
            this.Personlabel.Text = "Person";
            // 
            // Spouse_label
            // 
            this.Spouse_label.AutoSize = true;
            this.Spouse_label.Location = new System.Drawing.Point(150, 315);
            this.Spouse_label.Name = "Spouse_label";
            this.Spouse_label.Size = new System.Drawing.Size(43, 13);
            this.Spouse_label.TabIndex = 13;
            this.Spouse_label.Text = "Spouse";
            // 
            // PersonFather_button
            // 
            this.PersonFather_button.Location = new System.Drawing.Point(1100, 73);
            this.PersonFather_button.Name = "PersonFather_button";
            this.PersonFather_button.Size = new System.Drawing.Size(34, 23);
            this.PersonFather_button.TabIndex = 14;
            this.PersonFather_button.Text = ">";
            this.PersonFather_button.UseVisualStyleBackColor = true;
            this.PersonFather_button.Click += new System.EventHandler(this.PersonFather_button_Click);
            // 
            // PersonMother_button
            // 
            this.PersonMother_button.Location = new System.Drawing.Point(1100, 204);
            this.PersonMother_button.Name = "PersonMother_button";
            this.PersonMother_button.Size = new System.Drawing.Size(34, 23);
            this.PersonMother_button.TabIndex = 15;
            this.PersonMother_button.Text = ">";
            this.PersonMother_button.UseVisualStyleBackColor = true;
            this.PersonMother_button.Click += new System.EventHandler(this.PersonMother_button_Click);
            // 
            // SpouseFather_button
            // 
            this.SpouseFather_button.Location = new System.Drawing.Point(1100, 300);
            this.SpouseFather_button.Name = "SpouseFather_button";
            this.SpouseFather_button.Size = new System.Drawing.Size(34, 23);
            this.SpouseFather_button.TabIndex = 16;
            this.SpouseFather_button.Text = ">";
            this.SpouseFather_button.UseVisualStyleBackColor = true;
            this.SpouseFather_button.Click += new System.EventHandler(this.SpouseFather_button_Click);
            // 
            // SpouseMother_button
            // 
            this.SpouseMother_button.Location = new System.Drawing.Point(1100, 431);
            this.SpouseMother_button.Name = "SpouseMother_button";
            this.SpouseMother_button.Size = new System.Drawing.Size(34, 23);
            this.SpouseMother_button.TabIndex = 17;
            this.SpouseMother_button.Text = ">";
            this.SpouseMother_button.UseVisualStyleBackColor = true;
            this.SpouseMother_button.Click += new System.EventHandler(this.SpouseMother_button_Click);
            // 
            // Married_label
            // 
            this.Married_label.AutoSize = true;
            this.Married_label.Location = new System.Drawing.Point(150, 248);
            this.Married_label.Name = "Married_label";
            this.Married_label.Size = new System.Drawing.Size(71, 13);
            this.Married_label.TabIndex = 18;
            this.Married_label.Text = "Date Married:";
            // 
            // DateMarried_textBox
            // 
            this.DateMarried_textBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.DateMarried_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DateMarried_textBox.Location = new System.Drawing.Point(228, 244);
            this.DateMarried_textBox.Name = "DateMarried_textBox";
            this.DateMarried_textBox.Size = new System.Drawing.Size(74, 20);
            this.DateMarried_textBox.TabIndex = 20;
            this.DateMarried_textBox.Leave += new System.EventHandler(this.DateMarried_textBox_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(672, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Date Married:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(672, 367);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Date Married:";
            // 
            // SpouseParentsDateMarried_textBox
            // 
            this.SpouseParentsDateMarried_textBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.SpouseParentsDateMarried_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpouseParentsDateMarried_textBox.Location = new System.Drawing.Point(750, 365);
            this.SpouseParentsDateMarried_textBox.Name = "SpouseParentsDateMarried_textBox";
            this.SpouseParentsDateMarried_textBox.Size = new System.Drawing.Size(74, 20);
            this.SpouseParentsDateMarried_textBox.TabIndex = 39;
            this.SpouseParentsDateMarried_textBox.Leave += new System.EventHandler(this.SpouseParentsDateMarried_textBox_Leave);
            // 
            // PersonParentsDateMarried_textBox
            // 
            this.PersonParentsDateMarried_textBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PersonParentsDateMarried_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PersonParentsDateMarried_textBox.Location = new System.Drawing.Point(750, 135);
            this.PersonParentsDateMarried_textBox.Name = "PersonParentsDateMarried_textBox";
            this.PersonParentsDateMarried_textBox.Size = new System.Drawing.Size(74, 20);
            this.PersonParentsDateMarried_textBox.TabIndex = 40;
            this.PersonParentsDateMarried_textBox.Leave += new System.EventHandler(this.PersonParentsDateMarried_textBox_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(202, 431);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "Children";
            // 
            // MariatalStatus_comboBox
            // 
            this.MariatalStatus_comboBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.MariatalStatus_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MariatalStatus_comboBox.Location = new System.Drawing.Point(378, 244);
            this.MariatalStatus_comboBox.Name = "MariatalStatus_comboBox";
            this.MariatalStatus_comboBox.Size = new System.Drawing.Size(121, 21);
            this.MariatalStatus_comboBox.TabIndex = 46;
            this.MariatalStatus_comboBox.Leave += new System.EventHandler(this.Divorced_textBox_Leave);
            // 
            // PersonParentsMariatalStatus_comboBox
            // 
            this.PersonParentsMariatalStatus_comboBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PersonParentsMariatalStatus_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PersonParentsMariatalStatus_comboBox.Location = new System.Drawing.Point(885, 135);
            this.PersonParentsMariatalStatus_comboBox.Name = "PersonParentsMariatalStatus_comboBox";
            this.PersonParentsMariatalStatus_comboBox.Size = new System.Drawing.Size(121, 21);
            this.PersonParentsMariatalStatus_comboBox.TabIndex = 47;
            this.PersonParentsMariatalStatus_comboBox.Leave += new System.EventHandler(this.PersonParentsDivorced_textBox_Leave);
            // 
            // SpouseParentsMariatalStatus_comboBox
            // 
            this.SpouseParentsMariatalStatus_comboBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.SpouseParentsMariatalStatus_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpouseParentsMariatalStatus_comboBox.Location = new System.Drawing.Point(885, 365);
            this.SpouseParentsMariatalStatus_comboBox.Name = "SpouseParentsMariatalStatus_comboBox";
            this.SpouseParentsMariatalStatus_comboBox.Size = new System.Drawing.Size(121, 21);
            this.SpouseParentsMariatalStatus_comboBox.TabIndex = 48;
            this.SpouseParentsMariatalStatus_comboBox.Leave += new System.EventHandler(this.SpouseParentsDivorced_textBox_Leave);
            // 
            // Children_listBox
            // 
            this.Children_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Children_listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Children_listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.Children_listBox.Location = new System.Drawing.Point(150, 456);
            this.Children_listBox.Name = "Children_listBox";
            this.Children_listBox.Size = new System.Drawing.Size(400, 210);
            this.Children_listBox.TabIndex = 19;
            // 
            // SpouseMother_listBox
            // 
            this.SpouseMother_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.SpouseMother_listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpouseMother_listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.SpouseMother_listBox.Location = new System.Drawing.Point(672, 410);
            this.SpouseMother_listBox.Name = "SpouseMother_listBox";
            this.SpouseMother_listBox.Size = new System.Drawing.Size(400, 54);
            this.SpouseMother_listBox.TabIndex = 7;
            // 
            // SpouseFather_listBox
            // 
            this.SpouseFather_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.SpouseFather_listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpouseFather_listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.SpouseFather_listBox.Location = new System.Drawing.Point(672, 280);
            this.SpouseFather_listBox.Name = "SpouseFather_listBox";
            this.SpouseFather_listBox.Size = new System.Drawing.Size(400, 54);
            this.SpouseFather_listBox.TabIndex = 6;
            // 
            // Spouse_listBox
            // 
            this.Spouse_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Spouse_listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Spouse_listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.Spouse_listBox.Location = new System.Drawing.Point(150, 347);
            this.Spouse_listBox.Name = "Spouse_listBox";
            this.Spouse_listBox.Size = new System.Drawing.Size(400, 54);
            this.Spouse_listBox.TabIndex = 5;
            // 
            // PersonMother_listBox
            // 
            this.PersonMother_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PersonMother_listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PersonMother_listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.PersonMother_listBox.Location = new System.Drawing.Point(672, 180);
            this.PersonMother_listBox.Name = "PersonMother_listBox";
            this.PersonMother_listBox.Size = new System.Drawing.Size(400, 54);
            this.PersonMother_listBox.TabIndex = 4;
            // 
            // PersonFather_listBox
            // 
            this.PersonFather_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PersonFather_listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PersonFather_listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.PersonFather_listBox.Location = new System.Drawing.Point(672, 50);
            this.PersonFather_listBox.Name = "PersonFather_listBox";
            this.PersonFather_listBox.Size = new System.Drawing.Size(400, 54);
            this.PersonFather_listBox.TabIndex = 3;
            // 
            // Person_listBox
            // 
            this.Person_listBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Person_listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Person_listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.Person_listBox.Location = new System.Drawing.Point(150, 117);
            this.Person_listBox.Name = "Person_listBox";
            this.Person_listBox.Size = new System.Drawing.Size(400, 54);
            this.Person_listBox.TabIndex = 2;
            // 
            // FFamily
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1199, 685);
            this.Controls.Add(this.SpouseParentsMariatalStatus_comboBox);
            this.Controls.Add(this.PersonParentsMariatalStatus_comboBox);
            this.Controls.Add(this.MariatalStatus_comboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PersonParentsDateMarried_textBox);
            this.Controls.Add(this.SpouseParentsDateMarried_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DateMarried_textBox);
            this.Controls.Add(this.Children_listBox);
            this.Controls.Add(this.Married_label);
            this.Controls.Add(this.SpouseMother_button);
            this.Controls.Add(this.SpouseFather_button);
            this.Controls.Add(this.PersonMother_button);
            this.Controls.Add(this.PersonFather_button);
            this.Controls.Add(this.Spouse_label);
            this.Controls.Add(this.Personlabel);
            this.Controls.Add(this.ExchangePerson_button);
            this.Controls.Add(this.Marriage_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SpouseMother_listBox);
            this.Controls.Add(this.SpouseFather_listBox);
            this.Controls.Add(this.Spouse_listBox);
            this.Controls.Add(this.PersonMother_listBox);
            this.Controls.Add(this.PersonFather_listBox);
            this.Controls.Add(this.Person_listBox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "FFamily";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.GraphicsObject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        //****************************************************************************************************************************
        private void GraphicsObject_Load(object sender, System.EventArgs e)
        {
        }
        //****************************************************************************************************************************
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            UU.PaintParentLines(G, Person_listBox, PersonFather_listBox, PersonMother_listBox);
            UU.PaintParentLines(G, Spouse_listBox, SpouseFather_listBox, SpouseMother_listBox);
            if (m_bDataLoaded)
            {
                if (m_iPersonID > 0)
                    Person_listBox.SelectedIndex = 0;
                if (m_iPersonFatherID > 0)
                    PersonFather_listBox.SelectedIndex = 0;
                if (m_iPersonMotherID > 0)
                    PersonMother_listBox.SelectedIndex = 0;
                if (m_iSpouseID > 0)
                    Spouse_listBox.SelectedIndex = 0;
                if (m_iSpouseFatherID > 0)
                    SpouseFather_listBox.SelectedIndex = 0;
                if (m_iSpouseMotherID > 0)
                    SpouseMother_listBox.SelectedIndex = 0;
            }
            base.OnPaint(e);
        }
        //****************************************************************************************************************************
        private void DisplayMariageStatistics(TextBox DatetextBox,
                                              ComboBox MariatalcomboBox,
                                              string   sMarriedDate,
                                              string   sDivorced)
        {
            DatetextBox.Text = sMarriedDate;
            MariatalcomboBox.DropDownStyle = ComboBoxStyle.Simple;
            MariatalcomboBox.Text = sDivorced;
            MariatalcomboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        //****************************************************************************************************************************
        private void DisplayParentMarriage(TextBox tbDataMarried_TextBox,
                                           ComboBox tbMariatalStatus_comboBox,
                                           int iFatherId,
                                           int iMotherID)
        {
            DataTable tbl = new DataTable(U.Marriage_Table);
            SQL.GetMarriage(tbl, iFatherId, iMotherID);
            if (tbl.Rows.Count > 0)
            {
                DataRow row = tbl.Rows[0];
                DisplayMariageStatistics(tbDataMarried_TextBox, tbMariatalStatus_comboBox,
                                         row[U.DateMarried_col].ToString(), SQL.GetDivorcedFromRow(row));
            }
            else
            {
                tbDataMarried_TextBox.Text = "";
                tbMariatalStatus_comboBox.Text = "";
            }
        }
        //****************************************************************************************************************************
        private void DisplayPerson()
        {
            DataRow row = PopulateListBox(Person_listBox, m_iPersonID);
            if (row != null)
            {
                m_sSex = row[U.Sex_col].ToString();
            }
            SQL.GetFatherMother(m_iPersonID, out m_iPersonFatherID, out m_iPersonMotherID);
            PopulateListBox(PersonFather_listBox, m_iPersonFatherID);
            PopulateListBox(PersonMother_listBox, m_iPersonMotherID);
            if (m_iPersonFatherID != 0 && m_iPersonMotherID != 0)
            {
                DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsMariatalStatus_comboBox, m_iPersonFatherID, m_iPersonMotherID);
            }
            else
            {
                PersonParentsDateMarried_textBox.Text = "";
                PersonParentsMariatalStatus_comboBox.Text = "";
            }
        }
        //****************************************************************************************************************************
        private void DisplaySpouse()
        {
            if (m_iNumberSpouses == 0)
            {
                m_iSpouseID = 0;
                m_iSpouseFatherID = 0;
                m_iSpouseMotherID = 0;
                DateMarried_textBox.Text = "";
                MariatalStatus_comboBox.Text = "";
                MariatalStatus_comboBox.Visible = false;
                PopulateListBox(Spouse_listBox, m_iSpouseID);
                PopulateListBox(SpouseFather_listBox, m_iSpouseFatherID);
                PopulateListBox(SpouseMother_listBox, m_iSpouseMotherID);
            }
            else
            {
                MariatalStatus_comboBox.Visible = true;
                if (m_iCurrentSpouseIndex == m_tblSpouses.Rows.Count)
                {
                    DisplayMariageStatistics(DateMarried_textBox, MariatalStatus_comboBox, "", "");
                    m_iSpouseID = 0;
                }
                else
                {
                    DataRow row = m_tblSpouses.Rows[m_iCurrentSpouseIndex];
                    m_iSpouseID = row[m_iSpouseLocationInArray].ToInt();
                    DisplayMariageStatistics(DateMarried_textBox, MariatalStatus_comboBox,
                                         row[U.DateMarried_col].ToString(), SQL.GetDivorcedFromRow(row));
                }
                DataRow PersonRow = PopulateListBox(Spouse_listBox, m_iSpouseID);
                if (PersonRow == null)
                {
                    m_iSpouseID = 0;
                    m_iSpouseFatherID = 0;
                    m_iSpouseMotherID = 0;
                }
                else
                {
                    SQL.GetFatherMother(PersonRow, out m_iSpouseFatherID, out m_iSpouseMotherID);
                    PopulateListBox(SpouseFather_listBox, m_iSpouseFatherID);
                    PopulateListBox(SpouseMother_listBox, m_iSpouseMotherID);
                    if (m_iSpouseFatherID != 0 && m_iSpouseMotherID != 0)
                    {
                        DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsMariatalStatus_comboBox, m_iSpouseFatherID, m_iSpouseMotherID);
                    }
                    else
                    {
                        SpouseParentsDateMarried_textBox.Text = "";
                        SpouseParentsMariatalStatus_comboBox.Text = "";
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void SetFont(ListViewItem item)
        {
            float currentSize = item.Font.Size;
            currentSize += 5.0F;
            //item.Font = new Font(item.Font.Name, currentSize);
            item.Font = new Font(this.Font, FontStyle.Bold);
        }
        //****************************************************************************************************************************
        private void EditPerson()
        {
            int iPersonID = GetPersonFromDatabase(m_iPersonID, "", "", "", true);
            if (iPersonID != 0)
            {
                m_iPersonID = iPersonID;
                DisplayScreen();
                ExchangePerson_button.Focus();
                this.Refresh();
                m_bDataLoaded = true;
            }
        }
        //****************************************************************************************************************************
        private void NewPersonClick(object sender, System.EventArgs e)
        {
            int iSavePersonID = m_iPersonID;
            int iSaveSpouseID = m_iSpouseID;
            m_iPersonID = 0;
            m_iSpouseID = 0;
            EditPerson();
            if (m_iPersonID == 0)
            {
                m_iPersonID = iSavePersonID;
                m_iSpouseID = iSaveSpouseID;

            }
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void Person_Click(object sender, System.EventArgs e)
        {
            EditPerson();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void PersonFather_Click(object sender, System.EventArgs e)
        {
            int iPersonFatherID = GetPersonFromDatabase(m_iPersonFatherID, "M", SQL.GetPersonLastName(m_iPersonID), "", true);
            if (iPersonFatherID != 0 && iPersonFatherID != m_iPersonFatherID && UU.NotCircularReference(m_SQL, m_iPersonID, iPersonFatherID))
            {
                SQL.DeleteThisMarriage(m_iPersonFatherID, m_iPersonMotherID);
                m_iPersonFatherID = iPersonFatherID;
                SQL.UpdatePersonFatherOrMother("FatherID", m_iPersonID, m_iPersonFatherID);
                PopulateListBox(PersonFather_listBox, m_iPersonFatherID);
                SQL.InsertMarriageIfItDoesNotExist(m_iPersonFatherID, m_iPersonMotherID);
            }
            DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsMariatalStatus_comboBox, m_iPersonFatherID, m_iPersonMotherID);
            SetupPersonFatherContextMenu();
            PersonParentsDateMarried_textBox.Focus();
            this.Refresh();
        }
        //****************************************************************************************************************************
        private void PersonMother_Click(object sender, System.EventArgs e)
        {
            int iPersonMotherID = GetPersonFromDatabase(m_iPersonMotherID, "F", "",
                                                        SQL.GetPersonLastName(m_iPersonID), true);
            if (iPersonMotherID != 0 && iPersonMotherID != m_iPersonMotherID && UU.NotCircularReference(m_SQL, m_iPersonID, iPersonMotherID))
            {
                SQL.DeleteThisMarriage(m_iPersonFatherID, m_iPersonMotherID);
                m_iPersonMotherID = iPersonMotherID;
                SQL.UpdatePersonFatherOrMother("MotherID", m_iPersonID, m_iPersonMotherID);
                PopulateListBox(PersonMother_listBox, m_iPersonMotherID);
                SQL.InsertMarriageIfItDoesNotExist(m_iPersonFatherID, m_iPersonMotherID);
            }
            DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsMariatalStatus_comboBox, m_iPersonFatherID, m_iPersonMotherID);
            SetupPersonMotherContextMenu();
            PersonParentsDateMarried_textBox.Focus();
            this.Refresh();
        }
        //****************************************************************************************************************************
        private string Divorced(string sDivorced)
        {
            if (sDivorced.Length == 0)
                return "N";
            else if (sDivorced[0] == 'Y' || sDivorced[0] == 'y')
                return "Y";
            else
                return "N";
        }
        //****************************************************************************************************************************
        private void AddSpouse(int iPersonID,
                               int iSpouseID)
        {
            bool bAttachChildren = false;
            if (m_iNumChildren > 0)
            {
                string sMessage = "Do you wish to attach children to this spouse";
                if (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bAttachChildren = true;
                }
            }
            if (iPersonID != 0 && iSpouseID != 0)
            {
                DateMarried_textBox.Text = String.Empty;
                MariatalStatus_comboBox.Text = "Married";
                bool bAlreadySpouse = false;
                foreach (DataRow row in m_tblSpouses.Rows)
                {
                    if (row[U.SpouseID_col].ToInt() == iSpouseID)
                    {
                        bAlreadySpouse = true;
                    }
                }
                if (!bAlreadySpouse)
                {
                    SQL.InsertMarriage(iPersonID, iSpouseID);
                    if (bAttachChildren)
                        AddSpouseToChildrenRows(iPersonID, iSpouseID);
                    else
                        ClearChildren();
                }
                else if (iSpouseID == 0)
                {
                    AddSpouseToChildrenRows(iPersonID, iSpouseID);
                    m_iNumberSpouses--;
                    if (m_iNumberSpouses <= 1)
                    {
                        Marriage_button.Visible = false;
                    }
                }
                else
                {
                    AddSpouseToChildrenRows(iPersonID, iSpouseID);
                }
                SQL.SetSpouseMarriedName(iPersonID, iSpouseID);
            }
        }
        //****************************************************************************************************************************
        private bool SpouseDoesNotExists(int iPersonID,
                                         int iSpouseID)
        {
            DataTable tbl = new DataTable();
            if (SQL.GetMarriage(tbl, iPersonID, iSpouseID))
            {
                DataTable tblChildren = new DataTable();
                SQL.GetAllChildren(tblChildren, iPersonID, m_iSpouseID); // previous spouse
                if (tblChildren.Rows.Count == 0)
                {
                    MessageBox.Show("This Spouse already exists");
                    return false;
                }
                else
                {
                    string sMessage = "This Spouse already exists.  Do you wish to move children to this spouse";
                    if (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        return true;
                    else
                        return false;
                }
            }
            else
                return true;
        }
        //****************************************************************************************************************************
        private string SpouseSex(int iSpouseID)
        {
            if (iSpouseID != 0)
            {
                return "";
            }
            else if (m_sSex == "M")
            {
                return "F";
            }
            else if (m_sSex == "F")
            {
                return "M";
            }
            {
                return "";
            }
        }
        //****************************************************************************************************************************
        private int GetSpouse(int iPreviousSpouse)
        {
            string sex = SpouseSex(iPreviousSpouse);
            string marriedName = "";
            if (sex == "F")
                marriedName = SQL.GetPersonLastName(m_iPersonID);
            return GetPersonFromDatabase(iPreviousSpouse, sex, "", marriedName, true);
        }
        //****************************************************************************************************************************
        private void EditSpouse(int iPreviousSpouse,
                                bool DeletePreviousSpouse)
        {
            int iSpouseID = GetSpouse(iPreviousSpouse);
            if (iSpouseID == 0)
                return;
            if (iSpouseID == m_iSpouseID)
            {
                DisplaySpouse();
                Spouse_listBox.SelectedIndex = 0;
                SetupContextMenus();
            }
            else
            if (SpouseDoesNotExists(m_iPersonID, iSpouseID) && UU.NotCircularReference(m_SQL, m_iPersonID, iSpouseID))
            {
                if (m_iSpouseLocationInArray == 0) // Person is the SpouseID on marrageRecords
                {
                    if (DeletePreviousSpouse)
                    {
                        SQL.DeleteThisMarriage(m_iSpouseID, m_iPersonID);
                        m_tblSpouses.Clear();
                        SQL.GetMarriagesID(m_tblSpouses, ref m_iNumberSpouses, out m_iSpouseLocationInArray, m_iSpouseID);
                    }
                    m_tblSpouses.Rows.Add(new object[] { m_iSpouseID, m_iPersonID });
                }
                else
                {
                    if (DeletePreviousSpouse)
                    {
                        SQL.DeleteThisMarriage(m_iPersonID, m_iSpouseID);
                        m_tblSpouses.Clear();
                        SQL.GetMarriagesID(m_tblSpouses, ref m_iNumberSpouses, out m_iSpouseLocationInArray, m_iPersonID);
                    }
                    m_tblSpouses.Rows.Add(new object[] { m_iPersonID, m_iSpouseID });
                }
                AddSpouse(m_iPersonID, iSpouseID);
                m_iSpouseID = iSpouseID;
                m_iNumberSpouses = m_tblSpouses.Rows.Count;
                m_iCurrentSpouseIndex = m_iNumberSpouses - 1;
                SetCurrentMarriage(m_iSpouseID);
                DisplaySpouse();
                SetMariageButtons();
                SetupContextMenus();
                DateMarried_textBox.Focus();
                this.Refresh();
            }
        }
        //****************************************************************************************************************************
        private void NewSpouse_Click(object sender, System.EventArgs e)
        {
            int iSaveSpouseID = m_iSpouseID;
            EditSpouse(0, false);
            if (m_iSpouseID == 0)
                m_iSpouseID = iSaveSpouseID;
            SetupSpouseContextMenus();
        }
        //****************************************************************************************************************************
        private void Spouse_Click(object sender, System.EventArgs e)
        {
            EditSpouse(m_iSpouseID, true);
            SetupSpouseContextMenus();
        }
        //****************************************************************************************************************************
        public int GetPersonFromDatabase(int iPersonID,
                                         string sSex,
                                         string sLastName,
                                         string sMarriedName,
                                         bool bSelectPersonForPhoto)
        {
            FPerson Person = new FPerson(m_SQL, iPersonID, sSex, sLastName, sMarriedName, 0, bSelectPersonForPhoto);
            Person.ShowDialog();
            return Person.GetPersonID();
        }
        //****************************************************************************************************************************
        private void SpouseFather_Click(object sender, System.EventArgs e)
        {
            if (m_iSpouseID == 0)
                return;
            int iSpouseFatherID = GetPersonFromDatabase(m_iSpouseFatherID, "M", SQL.GetPersonLastName(m_iSpouseID), "", true);
            if (iSpouseFatherID != 0 && iSpouseFatherID != m_iSpouseFatherID && UU.NotCircularReference(m_SQL, m_iSpouseID, iSpouseFatherID))
            {
                SQL.DeleteThisMarriage(m_iSpouseFatherID, m_iSpouseMotherID);
                m_iSpouseFatherID = iSpouseFatherID;
                SQL.UpdatePersonFatherOrMother("FatherID", m_iSpouseID, m_iSpouseFatherID);
                PopulateListBox(SpouseFather_listBox, m_iSpouseFatherID);
                SQL.InsertMarriageIfItDoesNotExist(m_iSpouseFatherID, m_iSpouseMotherID);
            }
            DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsMariatalStatus_comboBox, m_iSpouseFatherID, m_iSpouseMotherID);
            SetupSpouseFatherContextMenu();
            SpouseParentsDateMarried_textBox.Focus();
            this.Refresh();
        }
        //****************************************************************************************************************************
        private void SpouseMother_Click(object sender, System.EventArgs e)
        {
            if (m_iSpouseID == 0)
                return;
            int iSpouseMotherID = GetPersonFromDatabase(m_iSpouseMotherID, "F", "",
                                                        SQL.GetPersonLastName(m_iSpouseID), true);
            if (iSpouseMotherID != 0 && iSpouseMotherID != m_iSpouseMotherID && UU.NotCircularReference(m_SQL, m_iSpouseID, iSpouseMotherID))
            {
                SQL.DeleteThisMarriage(m_iSpouseFatherID, m_iSpouseMotherID);
                m_iSpouseMotherID = iSpouseMotherID;
                SQL.UpdatePersonFatherOrMother("MotherID", m_iSpouseID, m_iSpouseMotherID);
                PopulateListBox(SpouseMother_listBox, m_iSpouseMotherID);
                SQL.InsertMarriageIfItDoesNotExist(m_iSpouseFatherID, m_iSpouseMotherID);
            }
            DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsMariatalStatus_comboBox, m_iSpouseFatherID, m_iSpouseMotherID);
            SetupSpouseMotherContextMenu();
            SpouseParentsDateMarried_textBox.Focus();
            this.Refresh();
        }
        //****************************************************************************************************************************
        private void SetCurrentMarriage(int iNewSpouseID)
        {
            GetMarriages();
            m_iCurrentSpouseIndex = 0;
            while ((m_iCurrentSpouseIndex + 1) < m_tblSpouses.Rows.Count && m_iSpouseID != iNewSpouseID)
            {
                m_iCurrentSpouseIndex++;
                DataRow row = m_tblSpouses.Rows[m_iCurrentSpouseIndex];
                m_iSpouseID = row[m_iSpouseLocationInArray].ToInt();
            }
        }
        //****************************************************************************************************************************
        private void ExchangePerson_button_Click(object sender, System.EventArgs e)
        {
            if (m_iSpouseID != 0)
            {
                int iPerson_WillBecomeSpouse = m_iPersonID;
                m_iPersonID = m_iSpouseID;
                m_iSpouseID = iPerson_WillBecomeSpouse;
                DisplayPerson();
                GetMarriages();
                SetCurrentMarriage(iPerson_WillBecomeSpouse);
                DisplaySpouse();
                SetupContextMenus();
                this.Refresh();
            }
        }
        //****************************************************************************************************************************
        private void Marriage_button_Click(object sender, System.EventArgs e)
        {
            if (m_iNumberSpouses > 1)
            {
                m_iCurrentSpouseIndex++;
                if (m_iCurrentSpouseIndex >= m_iNumberSpouses)
                    m_iCurrentSpouseIndex = 0;
                DisplaySpouse();
                PopulateChildrenListBox();
            }
        }
        //****************************************************************************************************************************
        private void PersonFather_button_Click(object sender, System.EventArgs e)
        {
            m_iPersonID = m_iPersonFatherID;
            DisplayScreen();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void PersonMother_button_Click(object sender, System.EventArgs e)
        {
            m_iPersonID = m_iPersonMotherID;
            DisplayScreen();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void SpouseFather_button_Click(object sender, System.EventArgs e)
        {
            m_iPersonID = m_iSpouseFatherID;
            DisplayScreen();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void SpouseMother_button_Click(object sender, System.EventArgs e)
        {
            m_iPersonID = m_iSpouseMotherID;
            DisplayScreen();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void ClearChildren()
        {
            Children_listBox.Items.Clear();
            m_tblChildren.Clear();
            m_iNumChildren = 0;
        }
        //****************************************************************************************************************************
        private void PopulateChildrenListBox()
        {
            ClearChildren();
            Children_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(SelectChild_Click);
            {
                SQL.GetAllChildren(m_tblChildren, m_iPersonID, m_iSpouseID);
                if (m_tblChildren.Rows.Count == 0)
                    SQL.GetAllChildren(m_tblChildren, m_iSpouseID, m_iPersonID);
                if (m_tblChildren.Rows.Count == 0)
                    return;
                foreach (DataRow row in m_tblChildren.Rows)
                {
                    int iPersonID = row[U.PersonID_col].ToInt();
                    DataTable CemeteryRecord_tbl = SQL.DefineVitalRecord_Table();
                    SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, iPersonID);
                    DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
                    SQL.GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col);

                    AddDatePlaceToPersonRow(row, VitalRecord_tbl, CemeteryRecord_tbl, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                                              U.BornDate_col, U.BornPlace_col, U.BornHome_col);
                    AddDatePlaceToPersonRow(row, VitalRecord_tbl, CemeteryRecord_tbl, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale,
                                                              U.DiedDate_col, U.DiedPlace_col, U.DiedHome_col);
                }
                DataView dv = m_tblChildren.DefaultView;
                dv.Sort = U.BornDate_col;
                m_tblChildren = dv.ToTable();  
                foreach (DataRow row in m_tblChildren.Rows)
                {
                    string sChildBornStat = AddDatePlaceHomeToString("Born", row[U.BornDate_col].ToString(), row[U.BornPlace_col].ToString(), row[U.BornHome_col].ToString(), false);
                    string sChildDiedStat = AddDatePlaceHomeToString("Died", row[U.DiedDate_col].ToString(), row[U.DiedPlace_col].ToString(), row[U.DiedHome_col].ToString(), false);
                    string sChildName = SQL.GetPersonNameFromRow(row);
                    if (sChildBornStat.Length != 0)
                    {
                        sChildName += ":     ";
                        sChildName += sChildBornStat;
                    }
                    if (sChildDiedStat.Length != 0)
                    {
                        if (sChildBornStat.Length == 0)
                        {
                            sChildName += ":    ";
                        }
                        else
                        {
                            sChildName += "    ";
                        }
                        sChildName += sChildDiedStat;
                    }
                    Children_listBox.Items.Add(sChildName);
                    m_iNumChildren++;
                }
                Children_listBox.SelectedIndex = 0;
                Children_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SelectChild_Click);
            }
        }
        //****************************************************************************************************************************
        private void ChildrenEdit_Click(object sender, System.EventArgs e)
        {
            int iChild = Children_listBox.SelectedIndex;
            DataRow row = m_tblChildren.Rows[iChild];
            int iPersonID = GetPersonFromDatabase(row[U.PersonID_col].ToInt(), "", "", "", false);
            if (iPersonID != 0)
            {
                PopulateChildrenListBox();
                SetupChildrenContextMenu();
                Children_listBox.SelectedIndex = iChild;
            }
        }
        //****************************************************************************************************************************
        private void ChildrenRemove_Click(object sender, System.EventArgs e)
        {
            int iChild = Children_listBox.SelectedIndex;
            DataRow row = m_tblChildren.Rows[iChild];
            int iPersonID = row[U.PersonID_col].ToInt();
            SQL.UpdatePersonFatherOrMother("FatherID", iPersonID, 0);
            SQL.UpdatePersonFatherOrMother("MotherID", iPersonID, 0);
            PopulateChildrenListBox();
            SetupChildrenContextMenu();
        }
        //****************************************************************************************************************************
        private bool ChildBelongsToAnotherSpouse(int iChildID,
                                                 int iPersonID,
                                                 int iSpouseID)
        {
            if (iChildID == 0 || iPersonID == 0 || iSpouseID == 0)
                return false;
            DataTable tblChildren = SQL.GetAllChildrenForPerson(iPersonID);
            foreach (DataRow row in tblChildren.Rows)
            {
                int iThisPersonID = row[U.PersonID_col].ToInt();
                int iFatherID = row[U.FatherID_col].ToInt();
                int iMotherID = row[U.MotherID_col].ToInt();
                if (iThisPersonID == iChildID && iSpouseID != iFatherID && iSpouseID != iFatherID)
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private string ChildLastName()
        {
            if (m_sSex == "M")
            {
                return SQL.GetPersonLastName(m_iPersonID);
            }
            else if (m_iSpouseID != 0)
            {
                return SQL.GetPersonLastName(m_iSpouseID);
            }
            else
            {
                return "";
            }
        }
        //****************************************************************************************************************************
        private void ChildrenAdd_Click(object sender, System.EventArgs e)
        {
            int iChildID = GetPersonFromDatabase(0, "", ChildLastName(), "", true);
            if (iChildID != 0)
            {
                DataTable tbl = new DataTable(U.Marriage_Table);
                if (SQL.GetMarriage(tbl, m_iPersonID, iChildID))
                {
                    MessageBox.Show("This Child is a spouse of this person");
                }
                else
                if (ChildBelongsToAnotherSpouse(iChildID, m_iPersonID, m_iSpouseID))
                {
                    string sMessage = "This child belongs to another spouse.  Do you wish to assign this child to this spouse";
                    if (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SQL.UpdatePersonFatherAndMother(iChildID, m_iPersonID, m_iSpouseID);
                        PopulateChildrenListBox();
                        SetupChildrenContextMenu();
                    }
                }
                else
                if (UU.NotCircularReference(m_SQL, m_iPersonID, iChildID))
                {
                    SQL.UpdatePersonFatherAndMother(iChildID, m_iPersonID, m_iSpouseID);
                    PopulateChildrenListBox();
                    SetupChildrenContextMenu();
                }
            }
        }
        //****************************************************************************************************************************
        private void SelectChild_Click(object sender, System.EventArgs e)
        {
            int iChild = Children_listBox.SelectedIndex;
            DataRow row = m_tblChildren.Rows[iChild];
            m_iPersonID = row[U.PersonID_col].ToInt();
            DisplayScreen();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private void AddDatePlaceToPersonRow(DataRow Person_row,
                                             DataTable VitalRecord_tbl,
                                             DataTable CemeteryRecord_tbl,
                                             EVitalRecordType eVitalRecordType1,
                                             EVitalRecordType eVitalRecordType2,
                                             string sDate_col,
                                             string sPlace_col,
                                             string sHome_col)
        {
            string sBook = "";
            string sPage = "";
            string sSource = "";
            string sDate = Person_row[sDate_col].ToString();
            string sPlace = Person_row[sPlace_col].ToString();
            string sHome = Person_row[sHome_col].ToString();
            U.GetPersonVitalStatistics(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, eVitalRecordType1, eVitalRecordType2,
                                                    ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
            Person_row[sDate_col] = sDate;
            Person_row[sPlace_col] = sPlace;
            Person_row[sHome_col] = sHome;
        }
        //****************************************************************************************************************************
        private string AddDatePlaceHomeToString(string sText,
                                                string sDate,
                                                string sPlace,
                                                string sHome,
                                                bool includePlaceHome = true)
        {
            string sString = "";
            if (sDate.Length != 0 || sPlace.Length != 0)
            {
                sString += sText + " ";
                if (sDate.Length != 0)
                    sString += sDate + " ";
                if (includePlaceHome)
                {
                    if (sHome.Length != 0)
                        sString += sHome;
                    else if (sPlace.Length != 0)
                        sString += sPlace;
                }
            }
            return sString;
        }
        //****************************************************************************************************************************
        private DataRow PopulateListBox(ListBoxWithDoubleClick listBox,
                                      int iPersonID)
        {
            listBox.Enabled = true;
            listBox.Items.Clear();
            if (iPersonID != 0)
            {
                DataRow Person_row = SQL.GetPerson(iPersonID);
                if (Person_row != null)
                {
                    listBox.Items.Add(SQL.GetPersonNameFromRow(Person_row));
                    DataTable CemeteryRecord_tbl = SQL.DefineVitalRecord_Table();
                    SQL.GetCemeteryRecordForPerson(CemeteryRecord_tbl, iPersonID);
                    DataTable VitalRecord_tbl = SQL.DefineVitalRecord_Table();
                    SQL.GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col);
                    AddDatePlaceToPersonRow(Person_row, VitalRecord_tbl, CemeteryRecord_tbl, EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                            U.BornDate_col, U.BornPlace_col, U.BornHome_col);
                    string sString = AddDatePlaceHomeToString("Born", Person_row[U.BornDate_col].ToString(), Person_row[U.BornPlace_col].ToString(), Person_row[U.BornHome_col].ToString());
                    listBox.Items.Add(sString);
                    AddDatePlaceToPersonRow(Person_row, VitalRecord_tbl, CemeteryRecord_tbl, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale,
                                            U.DiedDate_col, U.DiedPlace_col, U.DiedHome_col);
                    sString = AddDatePlaceHomeToString("Died", Person_row[U.DiedDate_col].ToString(), Person_row[U.DiedPlace_col].ToString(), Person_row[U.DiedHome_col].ToString());
                    listBox.Items.Add(sString);
                    listBox.SelectedIndex = 0;
                }
                return Person_row;
            }
            return null;
        }
        //****************************************************************************************************************************
        private void AddSpouseToChildrenRows(int iPersonID,
                                             int iSpouseID)
        {
            //            string sMessage = "Is this spouse the parents of these Children?";
            //            if (MessageBox.Show(sMessage, "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (DataRow row in m_tblChildren.Rows)
                {
                    int iFatherID = row[U.FatherID_col].ToInt();
                    int iMotherID = row[U.MotherID_col].ToInt();
                    if (iFatherID == iPersonID)
                        SQL.UpdatePersonFatherOrMother("MotherID", row[U.PersonID_col].ToInt(), iSpouseID);
                    else
                        SQL.UpdatePersonFatherOrMother("FatherID", row[U.PersonID_col].ToInt(), iSpouseID);

                }
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
        private bool DeletePersonFromDatabase(int iPersonID)
        {
            if (iPersonID == 0 || MessageBox.Show("Delete this Person?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                return false;
            else
                return SQL.DeletePersonFromDatabase(iPersonID);
        }
        //****************************************************************************************************************************
        private void PersonDelete_Clicked(object sender, EventArgs e)
        {
            if (DeletePersonFromDatabase(m_iPersonID))
            {
                m_bDataLoaded = false;
                m_iPersonID = 0;
                m_iSpouseID = 0;
                DisplayScreen();
                SetupContextMenus();
                m_bDataLoaded = true;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            validDateOnExit = true;
            Person_listBox.Focus();  // force save 
            if (!validDateOnExit)
            {
                e.Cancel = true;
            }
        }
        private void RemoveSpouseFromChildrenRecords(int iSpouseID)
        {
            foreach (DataRow row in m_tblChildren.Rows)
            {
                int iFatherID = row[U.FatherID_col].ToInt();
                int iMotherID = row[U.MotherID_col].ToInt();
                if (iSpouseID == iFatherID)
                    SQL.UpdatePersonFatherOrMother(U.FatherID_col, row[U.PersonID_col].ToInt(), 0);
                else
                    SQL.UpdatePersonFatherOrMother(U.MotherID_col, row[U.PersonID_col].ToInt(), 0);
            }
        }
        //****************************************************************************************************************************
        private void RemoveSpouse()
        {
            RemoveSpouseFromChildrenRecords(m_iSpouseID);
            m_bDataLoaded = false;
            GetMarriages();
            DisplaySpouse();
            PopulateChildrenListBox();
            SetupContextMenus();
            m_bDataLoaded = true;
        }
        //****************************************************************************************************************************
        private void SpouseRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iSpouseID))
            {
                SQL.DeleteThisMarriage(m_iPersonID, m_iSpouseID);
                RemoveSpouse();
            }
        }
        //****************************************************************************************************************************
        private void SpouseDelete_Clicked(object sender, EventArgs e)
        {
            int iTmpSpouseID = m_iSpouseID;
            m_iSpouseID = 0; // insure that OnPaint does not try to use old ID;
            if (DeletePersonFromDatabase(iTmpSpouseID))
            {
                m_iSpouseID = iTmpSpouseID;
                RemoveSpouse();
            }
            else
                m_iSpouseID = iTmpSpouseID;
        }
        //****************************************************************************************************************************
        private void RemoveParent(ListBoxWithDoubleClick Parent_TextBox,
                                  string sParent,
                                  ref int iParentID,
                                  TextBox Parents_DateMarried,
                                  ComboBox Parents_ParentsDivorced,
                                  int iPersonID,
                                  int iFatherID,
                                  int iMotherID)
        {
            m_bDataLoaded = false;
            DisplayParentMarriage(Parents_DateMarried, Parents_ParentsDivorced, iFatherID, iMotherID);
            iParentID = 0;
            SQL.UpdatePersonFatherOrMother(sParent, iPersonID, iParentID);
            PopulateListBox(Parent_TextBox, iParentID);
            SetupContextMenus();
            m_bDataLoaded = true;
        }
        //****************************************************************************************************************************
        private void PersonFatherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iPersonFatherID))
            {
                RemoveParent(PersonFather_listBox, "FatherID", ref m_iPersonFatherID, PersonParentsDateMarried_textBox,
                             PersonParentsMariatalStatus_comboBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
            }
        }
        //****************************************************************************************************************************
        private void PersonFatherDelete_Clicked(object sender, EventArgs e)
        {
            int iTmpSpouseID = m_iPersonFatherID;
            m_iSpouseID = 0; // insure that OnPaint does not try to use old ID;
            if (DeletePersonFromDatabase(iTmpSpouseID))
            {
                m_iPersonFatherID = iTmpSpouseID;
                RemoveParent(PersonFather_listBox, "FatherID", ref m_iPersonFatherID, PersonParentsDateMarried_textBox,
                             PersonParentsMariatalStatus_comboBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
            }
            else
                m_iPersonFatherID = iTmpSpouseID;
        }
        //****************************************************************************************************************************
        private void PersonMotherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iPersonMotherID))
            {
                RemoveParent(PersonMother_listBox, "MotherID", ref m_iPersonMotherID, PersonParentsDateMarried_textBox,
                             PersonParentsMariatalStatus_comboBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
            }
        }
        //****************************************************************************************************************************
        private void PersonMotherDelete_Clicked(object sender, EventArgs e)
        {
            int iTmpSpouseID = m_iPersonMotherID;
            m_iSpouseID = 0; // insure that OnPaint does not try to use old ID;
            if (DeletePersonFromDatabase(iTmpSpouseID))
            {
                m_iPersonMotherID = iTmpSpouseID;
                RemoveParent(PersonMother_listBox, "MotherID", ref m_iPersonMotherID, PersonParentsDateMarried_textBox,
                             PersonParentsMariatalStatus_comboBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
            }
            else
                m_iPersonMotherID = iTmpSpouseID;
        }
        //****************************************************************************************************************************
        private void SpouseFatherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iSpouseFatherID))
            {
                RemoveParent(SpouseFather_listBox, "FatherID", ref m_iSpouseFatherID, SpouseParentsDateMarried_textBox,
                             SpouseParentsMariatalStatus_comboBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
            }
        }
        //****************************************************************************************************************************
        private void SpouseFatherDelete_Clicked(object sender, EventArgs e)
        {
            int iTmpSpouseID = m_iSpouseFatherID;
            m_iSpouseID = 0; // insure that OnPaint does not try to use old ID;
            if (DeletePersonFromDatabase(iTmpSpouseID))
            {
                m_iSpouseFatherID = iTmpSpouseID;
                RemoveParent(SpouseFather_listBox, "FatherID", ref m_iSpouseFatherID, SpouseParentsDateMarried_textBox,
                             SpouseParentsMariatalStatus_comboBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
            }
            else
                m_iSpouseFatherID = iTmpSpouseID;
        }
        //****************************************************************************************************************************
        private void SpouseMotherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iSpouseMotherID))
            {
                RemoveParent(SpouseMother_listBox, "MotherID", ref m_iSpouseMotherID, SpouseParentsDateMarried_textBox,
                             SpouseParentsMariatalStatus_comboBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
            }
        }
        //****************************************************************************************************************************
        private void SpouseMotherDelete_Clicked(object sender, EventArgs e)
        {
            int iTmpSpouseID = m_iSpouseMotherID;
            m_iSpouseID = 0; // insure that OnPaint does not try to use old ID;
            if (DeletePersonFromDatabase(iTmpSpouseID))
            {
                m_iSpouseMotherID = iTmpSpouseID;
                RemoveParent(SpouseMother_listBox, "MotherID", ref m_iSpouseMotherID, SpouseParentsDateMarried_textBox,
                             SpouseParentsMariatalStatus_comboBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
            }
            else
                m_iSpouseMotherID = iTmpSpouseID;
        }
    }
}