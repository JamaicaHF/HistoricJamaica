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
        private int m_iPersonFatherID = 0;
        private int m_iPersonMotherID = 0;
        private int m_iSpouseID = 0;
        private int m_iSpouseFatherID = 0;
        private int m_iSpouseMotherID = 0;
        private int m_iNumChildren = 0;
        private bool bDataLoaded = false;
        private DataTable m_tblSpouses = new DataTable(U.Marriage_Table);
        private DataTable m_tblChildren = new DataTable(U.Person_Table);
        private int m_iNumberSpouses = 0;
        private int m_iCurrentSpouseIndex = 0;
        private int m_iSpouseLocationInArray = 1;
        private TextBox DateMarried_textBox;
        private TextBox Divorced_textBox;
        private TextBox PersonParentsDateMarried_textBox;
        private TextBox PersonParentsDivorced_textBox;
        private TextBox SpouseParentsDateMarried_textBox;
        private TextBox SpouseParentsDivorced_textBox;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private System.ComponentModel.IContainer components = null;
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
            InstantiateContextMenus();
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
                Divorced_textBox.Enabled = false;
                PersonParentsDateMarried_textBox.Enabled = false;
                PersonParentsDivorced_textBox.Enabled = false;
                SpouseParentsDateMarried_textBox.Enabled = false;
                SpouseParentsDivorced_textBox.Enabled = false;
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
                    Divorced_textBox.Enabled = false;
                    Spouse_listBox.ContextMenu.MenuItems.Add("New Person", new EventHandler(NewSpouse_Click));
                    Spouse_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(NewSpouse_Click);
                }
                else
                {
                    DateMarried_textBox.Enabled = true;
                    Divorced_textBox.Enabled = true;
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
                PersonParentsDivorced_textBox.Enabled = false;
                PersonFather_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(PersonFather_Click);
                if (m_iPersonFatherID == 0)
                    PersonFather_listBox.ContextMenu.MenuItems.Add("New Person", new EventHandler(PersonFather_Click));
                else
                {
                    PersonFather_button.Visible = true;
                    PersonFather_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(PersonFather_Click));
                    PersonFather_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(PersonFatherRemove_Clicked));
                    PersonFather_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(PersonFatherDelete_Clicked));
                    if (m_iPersonMotherID != 0)
                    {
                        PersonParentsDateMarried_textBox.Enabled = true;
                        PersonParentsDivorced_textBox.Enabled = true;
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
                PersonParentsDivorced_textBox.Enabled = false;
                PersonMother_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PersonMother_Click);
                if (m_iPersonMotherID == 0)
                    PersonMother_listBox.ContextMenu.MenuItems.Add("New Person", new EventHandler(PersonMother_Click));
                else
                {
                    PersonMother_button.Visible = true;
                    PersonMother_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(PersonMother_Click));
                    PersonMother_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(PersonMotherRemove_Clicked));
                    PersonMother_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(PersonMotherDelete_Clicked));
                    if (m_iPersonFatherID != 0)
                    {
                        PersonParentsDateMarried_textBox.Enabled = true;
                        PersonParentsDivorced_textBox.Enabled = true;
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
                SpouseParentsDivorced_textBox.Enabled = false;
                SpouseFather_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SpouseFather_Click);
                if (m_iSpouseFatherID == 0)
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("New Person", new EventHandler(SpouseFather_Click));
                else
                {
                    SpouseFather_button.Visible = true;
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(SpouseFather_Click));
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(SpouseFatherRemove_Clicked));
                    SpouseFather_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(SpouseFatherDelete_Clicked));
                    if (m_iSpouseMotherID != 0)
                    {
                        SpouseParentsDateMarried_textBox.Enabled = true;
                        SpouseParentsDivorced_textBox.Enabled = true;
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
                SpouseParentsDivorced_textBox.Enabled = false;
                SpouseMother_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SpouseMother_Click);
                if (m_iSpouseMotherID == 0)
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("New Person", new EventHandler(SpouseMother_Click));
                else
                {
                    SpouseMother_button.Visible = true;
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("Edit Person", new EventHandler(SpouseMother_Click));
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("Remove Parent", new EventHandler(SpouseMotherRemove_Clicked));
                    SpouseMother_listBox.ContextMenu.MenuItems.Add("Delete Person", new EventHandler(SpouseMotherDelete_Clicked));
                    if (m_iSpouseFatherID != 0)
                    {
                        SpouseParentsDateMarried_textBox.Enabled = true;
                        SpouseParentsDivorced_textBox.Enabled = true;
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
                GetMarriages();
            DisplaySpouse();
            PopulateChildrenListBox();
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
            m_iSpouseID = Q.i(m_SQL,m_SQL.GetMarriagesID(m_tblSpouses, ref m_iNumberSpouses, out m_iSpouseLocationInArray, m_iPersonID));
            m_iCurrentSpouseIndex = 0;
            SetMariageButtons();
        }
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
        private void UpdateMarriageDivorced(int   iPersonID,
                                           int    iSpouseID,
                                           string sDivorced)
        {
            if (iPersonID != 0 && iSpouseID != 0)
            {
                Q.i(m_SQL,m_SQL.UpdateMarriageDivorced(iPersonID, iSpouseID, sDivorced));
            }
        }
        //****************************************************************************************************************************
        private void Divorced_textBox_Leave(object sender, System.EventArgs e)
        {
            UpdateMarriageDivorced(m_iPersonID, m_iSpouseID, Divorced_textBox.Text.TrimString());
            DisplayParentMarriage(DateMarried_textBox, Divorced_textBox, m_iPersonID, m_iSpouseID);
        }
        //****************************************************************************************************************************
        private void PersonParentsDivorced_textBox_Leave(object sender, EventArgs e)
        {
            UpdateMarriageDivorced(m_iPersonFatherID, m_iPersonMotherID, PersonParentsDivorced_textBox.Text.TrimString());
            DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsDivorced_textBox, m_iPersonFatherID, m_iPersonMotherID);
        }
        //****************************************************************************************************************************
        private void SpouseParentsDivorced_textBox_Leave(object sender, EventArgs e)
        {
            UpdateMarriageDivorced(m_iSpouseFatherID, m_iSpouseMotherID, SpouseParentsDivorced_textBox.Text.TrimString());
            DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsDivorced_textBox, m_iSpouseFatherID, m_iSpouseMotherID);
        }
        //****************************************************************************************************************************
        private void UpdateMarriageDate(int    iPersonID,
                                        int    iSpouseID,
                                        string sDateMarried)
        {
            if (iPersonID != 0 && iSpouseID != 0)
            {
                Q.i(m_SQL,m_SQL.UpdateMarriageDate(iPersonID, iSpouseID, sDateMarried));
            }
        }
        //****************************************************************************************************************************
        private void DateMarried_textBox_Leave(object sender, System.EventArgs e)
        {
            UpdateMarriageDate(m_iPersonID, m_iSpouseID, DateMarried_textBox.Text.TrimString());
            DisplayParentMarriage(DateMarried_textBox, Divorced_textBox, m_iPersonID, m_iSpouseID);
        }
        //****************************************************************************************************************************
        private void PersonParentsDateMarried_textBox_Leave(object sender, System.EventArgs e)
        {
            UpdateMarriageDate(m_iPersonFatherID, m_iPersonMotherID, PersonParentsDateMarried_textBox.Text.TrimString());
            DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsDivorced_textBox, m_iPersonFatherID, m_iPersonMotherID);
        }
        //****************************************************************************************************************************
        private void SpouseParentsDateMarried_textBox_Leave(object sender, System.EventArgs e)
        {
            UpdateMarriageDate(m_iSpouseFatherID, m_iSpouseMotherID, SpouseParentsDateMarried_textBox.Text.TrimString());
            DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsDivorced_textBox, m_iSpouseFatherID, m_iSpouseMotherID);
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
            this.label2 = new System.Windows.Forms.Label();
            this.Divorced_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SpouseParentsDateMarried_textBox = new System.Windows.Forms.TextBox();
            this.PersonParentsDateMarried_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.PersonParentsDivorced_textBox = new System.Windows.Forms.TextBox();
            this.SpouseParentsDivorced_textBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(333, 248);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Divorced:";
            // 
            // Divorced_textBox
            // 
            this.Divorced_textBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Divorced_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Divorced_textBox.Location = new System.Drawing.Point(412, 246);
            this.Divorced_textBox.Name = "Divorced_textBox";
            this.Divorced_textBox.Size = new System.Drawing.Size(32, 20);
            this.Divorced_textBox.TabIndex = 22;
            this.Divorced_textBox.Leave += new System.EventHandler(this.Divorced_textBox_Leave);
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(850, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Divorced:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(850, 367);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Divorced:";
            // 
            // PersonParentsDivorced_textBox
            // 
            this.PersonParentsDivorced_textBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PersonParentsDivorced_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PersonParentsDivorced_textBox.Location = new System.Drawing.Point(910, 135);
            this.PersonParentsDivorced_textBox.Name = "PersonParentsDivorced_textBox";
            this.PersonParentsDivorced_textBox.Size = new System.Drawing.Size(32, 20);
            this.PersonParentsDivorced_textBox.TabIndex = 43;
            this.PersonParentsDivorced_textBox.Leave += new System.EventHandler(this.PersonParentsDivorced_textBox_Leave);
            // 
            // SpouseParentsDivorced_textBox
            // 
            this.SpouseParentsDivorced_textBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.SpouseParentsDivorced_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SpouseParentsDivorced_textBox.Location = new System.Drawing.Point(910, 365);
            this.SpouseParentsDivorced_textBox.Name = "SpouseParentsDivorced_textBox";
            this.SpouseParentsDivorced_textBox.Size = new System.Drawing.Size(32, 20);
            this.SpouseParentsDivorced_textBox.TabIndex = 44;
            this.SpouseParentsDivorced_textBox.Leave += new System.EventHandler(this.SpouseParentsDivorced_textBox_Leave);
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
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SpouseParentsDivorced_textBox);
            this.Controls.Add(this.PersonParentsDivorced_textBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PersonParentsDateMarried_textBox);
            this.Controls.Add(this.SpouseParentsDateMarried_textBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Divorced_textBox);
            this.Controls.Add(this.label2);
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
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        //****************************************************************************************************************************
        private void GraphicsObject_Load(object sender, System.EventArgs e)
        {
        }
        //****************************************************************************************************************************
        protected void Form_Closing(object sender, CancelEventArgs e)
        {
            if (DateMarried_textBox.Modified)
                UpdateMarriageDate(m_iPersonID, m_iSpouseID, DateMarried_textBox.Text.ToString());
            if (PersonParentsDateMarried_textBox.Modified)
                UpdateMarriageDate(m_iPersonFatherID, m_iPersonMotherID, PersonParentsDateMarried_textBox.Text.ToString());
            if (SpouseParentsDateMarried_textBox.Modified)
                UpdateMarriageDate(m_iSpouseFatherID, m_iSpouseMotherID, SpouseParentsDateMarried_textBox.Text.ToString());
            if (Divorced_textBox.Modified)
                UpdateMarriageDivorced(m_iPersonID, m_iSpouseID, Divorced_textBox.Text.ToString());
            if (PersonParentsDivorced_textBox.Modified)
                UpdateMarriageDivorced(m_iPersonFatherID, m_iPersonMotherID, PersonParentsDivorced_textBox.Text.ToString());
            if (SpouseParentsDivorced_textBox.Modified)
                UpdateMarriageDivorced(m_iSpouseFatherID, m_iSpouseMotherID, SpouseParentsDivorced_textBox.Text.ToString());
        }
        //****************************************************************************************************************************
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics G = e.Graphics;
            UU.PaintParentLines(G, Person_listBox, PersonFather_listBox, PersonMother_listBox);
            UU.PaintParentLines(G, Spouse_listBox, SpouseFather_listBox, SpouseMother_listBox);
            if (bDataLoaded)
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
        private void DisplayMariageStatistics(TextBox DateMarried_textBox,
                                              TextBox Divorced_textBox,
                                              string sMarriedDate,
                                              string sDivorced)
        {
            DateMarried_textBox.Text = sMarriedDate;
            Divorced_textBox.Text = sDivorced;
        }
        //****************************************************************************************************************************
        private void DisplayParentMarriage(TextBox tbDataMarried_TextBox,
                                           TextBox tbDivorced_textBox,
                                           int iFatherId,
                                           int iMotherID)
        {
            DataTable tbl = new DataTable(U.Marriage_Table);
            Q.v(m_SQL,m_SQL.GetMarriage(tbl, iFatherId, iMotherID));
            if (tbl.Rows.Count > 0)
            {
                DataRow row = tbl.Rows[0];
                DisplayMariageStatistics(tbDataMarried_TextBox, tbDivorced_textBox,
                                         Q.s(m_SQL,m_SQL.DateMarriedFromRow(row)), Q.s(m_SQL,m_SQL.GetDivorcedFromRow(row)));
            }
            else
            {
                tbDataMarried_TextBox.Text = "";
                tbDivorced_textBox.Text = "";
            }
        }
        //****************************************************************************************************************************
        private void DisplayPerson() 
        {
            Q.v(m_SQL,m_SQL.GetFatherMother(m_iPersonID, ref m_iPersonFatherID, ref m_iPersonMotherID));
            PopulateListBox(Person_listBox,m_iPersonID);
            PopulateListBox(PersonFather_listBox, m_iPersonFatherID);
            PopulateListBox(PersonMother_listBox, m_iPersonMotherID);
            if (m_iPersonFatherID != 0 && m_iPersonMotherID != 0)
            {
                DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsDivorced_textBox,m_iPersonFatherID, m_iPersonMotherID);
            }
            else
            {
                PersonParentsDateMarried_textBox.Text = "";
                PersonParentsDivorced_textBox.Text = "";
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
                Divorced_textBox.Text = "";
            }
            else
            {
                DataRow row = m_tblSpouses.Rows[m_iCurrentSpouseIndex];
                m_iSpouseID = row[m_iSpouseLocationInArray].ToInt();
                DisplayMariageStatistics(DateMarried_textBox, Divorced_textBox,
                                         Q.s(m_SQL,m_SQL.DateMarriedFromRow(row)), Q.s(m_SQL,m_SQL.GetDivorcedFromRow(row)));
                Q.v(m_SQL,m_SQL.GetFatherMother(m_iSpouseID, ref m_iSpouseFatherID, ref m_iSpouseMotherID));
            }
            PopulateListBox(Spouse_listBox, m_iSpouseID);
            PopulateListBox(SpouseFather_listBox, m_iSpouseFatherID);
            PopulateListBox(SpouseMother_listBox, m_iSpouseMotherID);
            if (m_iSpouseFatherID != 0 && m_iSpouseMotherID != 0)
            {
                DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsDivorced_textBox, m_iSpouseFatherID, m_iSpouseMotherID);
            }
            else
            {
                SpouseParentsDateMarried_textBox.Text = "";
                SpouseParentsDivorced_textBox.Text = "";
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
            int iPersonID = GetPersonFromDatabase(m_iPersonID, false);
            if (iPersonID != 0)
            {
                m_iPersonID = iPersonID;
                DisplayScreen();
                ExchangePerson_button.Focus();
                this.Refresh();
                bDataLoaded = true;
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
            int iPersonFatherID = GetPersonFromDatabase(m_iPersonFatherID, false);
            if (iPersonFatherID != 0 && iPersonFatherID != m_iPersonFatherID)
            {
                m_iPersonFatherID = iPersonFatherID;
                Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("FatherID", m_iPersonID, m_iPersonFatherID));
                PopulateListBox(PersonFather_listBox, m_iPersonFatherID);
                AddSpouse(m_iPersonFatherID, m_iPersonMotherID);
            }
            DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsDivorced_textBox, m_iPersonFatherID, m_iPersonMotherID);
            SetupPersonFatherContextMenu();
            PersonParentsDateMarried_textBox.Focus();
            this.Refresh();
        }
        //****************************************************************************************************************************
        private void PersonMother_Click(object sender, System.EventArgs e)
        {
            int iPersonMotherID = GetPersonFromDatabase(m_iPersonMotherID, false);
            if (iPersonMotherID != 0 && iPersonMotherID != m_iPersonMotherID)
            {
                m_iPersonMotherID = iPersonMotherID;
                Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("MotherID", m_iPersonID, m_iPersonMotherID));
                PopulateListBox(PersonMother_listBox, m_iPersonMotherID);
                AddSpouse(m_iPersonFatherID, m_iPersonMotherID);
            }
            DisplayParentMarriage(PersonParentsDateMarried_textBox, PersonParentsDivorced_textBox, m_iPersonFatherID, m_iPersonMotherID);
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
            if (iPersonID != 0 && iSpouseID != 0)
            {
                DateMarried_textBox.Text = U.Unknown;
                Divorced_textBox.Text = "No ";
                Q.i(m_SQL,m_SQL.InsertMarriage(m_SQL.MarriageValues(iPersonID, iSpouseID, U.Unknown, 'N')));
                Q.b(m_SQL,m_SQL.SetSpouseMarriedName(iPersonID,iSpouseID));
            }
        }
        //****************************************************************************************************************************
        private void EditSpouse()
        {
            int iSpouseID = GetPersonFromDatabase(m_iSpouseID, false);
            if (iSpouseID != 0 && iSpouseID != m_iSpouseID)
            {
                m_iSpouseID = iSpouseID;

                if (m_iSpouseLocationInArray == 0) // Person is the SpouseID on marrageRecords
                    AddSpouse(m_iSpouseID, m_iPersonID);
                else
                    AddSpouse(m_iPersonID, m_iSpouseID);
                MessageBox.Show("D");
                m_tblSpouses.Rows.Add(new object[] { m_iPersonID, iSpouseID });
                m_iNumberSpouses = m_tblSpouses.Rows.Count;
                m_iCurrentSpouseIndex = m_iNumberSpouses - 1;
                SetCurrentMarriage(m_iSpouseID);
                DisplaySpouse();
                SetMariageButtons();
                Children_listBox.Items.Clear();
                SetupContextMenus();
                DateMarried_textBox.Focus();
                this.Refresh();
            }
            else if (iSpouseID != 0)
            {
                Spouse_listBox.SelectedIndex = 0;
                SetupContextMenus();
            }
        }
        //****************************************************************************************************************************
        private void NewSpouse_Click(object sender, System.EventArgs e)
        {
            int iSaveSpouseID = m_iSpouseID;
            m_iSpouseID = 0;
            EditSpouse();
            if (m_iSpouseID == 0)
                m_iSpouseID = iSaveSpouseID;
            SetupSpouseContextMenus();
        }
        //****************************************************************************************************************************
        private void Spouse_Click(object sender, System.EventArgs e)
        {
            EditSpouse();
            SetupSpouseContextMenus();
        }
        //****************************************************************************************************************************
        public int GetPersonFromDatabase(int iPersonID,
                                         bool bSelectPersonForPhoto)
        {
            FPerson Person = new FPerson(m_SQL, iPersonID, bSelectPersonForPhoto);
            Person.ShowDialog();
            return Person.GetPersonID();
        }
        //****************************************************************************************************************************
        private void SpouseFather_Click(object sender, System.EventArgs e)
        {
            if (m_iSpouseID == 0)
                return;
            int iSpouseFatherID = GetPersonFromDatabase(m_iSpouseFatherID, false);
            if (iSpouseFatherID != 0 && iSpouseFatherID != m_iSpouseFatherID)
            {
                m_iSpouseFatherID = iSpouseFatherID;
                Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("FatherID", m_iSpouseID, m_iSpouseFatherID));
                PopulateListBox(SpouseFather_listBox, m_iSpouseFatherID);
                AddSpouse(m_iSpouseFatherID, m_iSpouseMotherID);
            }
            DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsDivorced_textBox, m_iSpouseFatherID, m_iSpouseMotherID);
            SetupSpouseFatherContextMenu();
            SpouseParentsDateMarried_textBox.Focus(); 
            this.Refresh();
        }
        //****************************************************************************************************************************
        private void SpouseMother_Click(object sender, System.EventArgs e)
        {
            if (m_iSpouseID == 0)
                return;
            int iSpouseMotherID = GetPersonFromDatabase(m_iSpouseMotherID, false);
            if (iSpouseMotherID != 0 && iSpouseMotherID != m_iSpouseMotherID)
            {
                m_iSpouseMotherID = iSpouseMotherID;
                Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("MotherID", m_iSpouseID, m_iSpouseMotherID));
                PopulateListBox(SpouseMother_listBox, m_iSpouseMotherID);
                AddSpouse(m_iSpouseFatherID, m_iSpouseMotherID);
            }
            DisplayParentMarriage(SpouseParentsDateMarried_textBox, SpouseParentsDivorced_textBox, m_iSpouseFatherID, m_iSpouseMotherID);
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
                DisplayPerson();
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
        private void PopulateChildrenListBox()
        {
            Children_listBox.Items.Clear();
            m_tblChildren.Clear();
            m_iNumChildren = 0;
            Children_listBox.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(SelectChild_Click);
            {
                Q.v(m_SQL,m_SQL.GetAllChildren(m_tblChildren, m_iPersonID, m_iSpouseID));
                if (m_tblChildren.Rows.Count == 0)
                    Q.v(m_SQL,m_SQL.GetAllChildren(m_tblChildren, m_iSpouseID, m_iPersonID));
                foreach (DataRow row in m_tblChildren.Rows)
                {
                    DataTable VitalRecord_tbl = Q.t(m_SQL,m_SQL.DefineVitalRecord_Table());
                    int iPersonID = row[U.PersonID_col].ToInt();
                    Q.v(m_SQL,m_SQL.GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col));
                    
                    string sChildStat = AddDatePlaceToListBox(row, VitalRecord_tbl, "Born", EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                                              Q.s(m_SQL,m_SQL.PersonBornDate(row)),
                                                              Q.s(m_SQL,m_SQL.PersonBornPlace(row)), Q.s(m_SQL,m_SQL.PersonBornHome(row)));
                    string sChildName = Q.s(m_SQL,m_SQL.GetPersonNameFromRow(row));
                    Children_listBox.Items.Add(sChildName);
                    m_iNumChildren++;
                }
                if (m_iNumChildren != 0)
                {
                    Children_listBox.SelectedIndex = 0;
                    Children_listBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(SelectChild_Click);
                }
            }
        }
        //****************************************************************************************************************************
        private void ChildrenEdit_Click(object sender, System.EventArgs e)
        {
            int iChild = Children_listBox.SelectedIndex;
            DataRow row = m_tblChildren.Rows[iChild];
            int iPersonID = GetPersonFromDatabase(m_SQL.PersonID(row), false);
            if (iPersonID != 0)
            {
                Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("FatherID", iPersonID, m_iPersonID));
                Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("MotherID", iPersonID, m_iSpouseID));
                PopulateChildrenListBox();
                SetupChildrenContextMenu();
            }
        }
        //****************************************************************************************************************************
        private void ChildrenRemove_Click(object sender, System.EventArgs e)
        {
            int iChild = Children_listBox.SelectedIndex;
            DataRow row = m_tblChildren.Rows[iChild];
            int iPersonID = Q.i(m_SQL,m_SQL.PersonID(row));
            Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("FatherID", iPersonID, 0));
            Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother("MotherID", iPersonID, 0));
            PopulateChildrenListBox();
            SetupChildrenContextMenu();
        }
        //****************************************************************************************************************************
        private void ChildrenAdd_Click(object sender, System.EventArgs e)
        {
            int iChildID = GetPersonFromDatabase(0, false);
            if (iChildID != 0)
            {
                Q.v(m_SQL, m_SQL.UpdatePersonFatherAndMother(iChildID, m_iPersonID, m_iSpouseID));
                PopulateChildrenListBox();
                SetupChildrenContextMenu();
            }
        }
        //****************************************************************************************************************************
        private void SelectChild_Click(object sender, System.EventArgs e)
        {
            int iChild = Children_listBox.SelectedIndex;
            DataRow row = m_tblChildren.Rows[iChild];
            m_iPersonID = Q.i(m_SQL,m_SQL.PersonID(row));
            DisplayScreen();
            SetupContextMenus();
        }
        //****************************************************************************************************************************
        private string AddDatePlaceToListBox(DataRow                Person_row,
                                           DataTable              VitalRecord_tbl,
                                           string                 sText,
                                           EVitalRecordType eVitalRecordType1,
                                           EVitalRecordType eVitalRecordType2,
                                           string                 sDate,
                                           string                 sPlace,
                                           string                 sHome)
        {
            string sBook = "";
            string sPage = "";
            string sSource = "";
            eSex Sex = eSex.eUnknown;
            U.GetPersonVitalStatistics(VitalRecord_tbl, Person_row, eVitalRecordType1, eVitalRecordType2,
                                                    ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource, ref Sex);
            string sString = "";
            if (sDate.Length != 0 || sPlace.Length != 0)
            {
                sString += sText + "  ";
                if (sDate.Length != 0)
                    sString += sDate + "   ";
                if (sHome.Length != 0)
                    sString += sHome;
                else
                    if (sPlace.Length != 0)
                        sString += sPlace;
            }
            return sString;
        }
        //****************************************************************************************************************************
        private void PopulateListBox(ListBoxWithDoubleClick listBox,
                                      int iPersonID)
        {
            listBox.Enabled = true;
            listBox.Items.Clear();
            if (iPersonID != 0)
            {
                DataRow Person_row = Q.r(m_SQL,m_SQL.GetPerson(iPersonID));
                if (Person_row != null)
                {
                    listBox.Items.Add(m_SQL.GetPersonNameFromRow(Person_row));
                    DataTable VitalRecord_tbl = Q.t(m_SQL,m_SQL.DefineVitalRecord_Table());
                    Q.v(m_SQL,m_SQL.GetVitalRecordsForPerson(VitalRecord_tbl, iPersonID, U.PersonID_col));
                    string sString = AddDatePlaceToListBox(Person_row, VitalRecord_tbl, "Born", EVitalRecordType.eBirthMale, EVitalRecordType.eBirthFemale,
                                          Q.s(m_SQL,m_SQL.PersonBornDate(Person_row)), 
                                          Q.s(m_SQL,m_SQL.PersonBornPlace(Person_row)), Q.s(m_SQL,m_SQL.PersonBornHome(Person_row)));
                    listBox.Items.Add(sString);
                    sString = AddDatePlaceToListBox(Person_row, VitalRecord_tbl, "Died", EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale,
                                          Q.s(m_SQL,m_SQL.PersonDiedDate(Person_row)), Q.s(m_SQL,m_SQL.PersonDiedPlace(Person_row)),
                                          Q.s(m_SQL,m_SQL.PersonDiedHome(Person_row)));
                    listBox.Items.Add(sString);
                    listBox.SelectedIndex = 0;
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
                return Q.b(m_SQL,m_SQL.DeletePersonFromDatabase(iPersonID));
        }
        //****************************************************************************************************************************
        private void PersonDelete_Clicked(object sender, EventArgs e)
        {
            if (DeletePersonFromDatabase(m_iPersonID))
            {
                bDataLoaded = false;
                m_iPersonID = 0;
                m_iSpouseID = 0;
                DisplayScreen();
                SetupContextMenus();
                bDataLoaded = true;
            }
        }
        //****************************************************************************************************************************
        private void RemoveSpouse()
        {
            bDataLoaded = false;
            GetMarriages();
            DisplaySpouse();
            PopulateChildrenListBox();
            SetupContextMenus();
            bDataLoaded = true;
        }
        //****************************************************************************************************************************
        private void SpouseRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iSpouseID))
            {
                Q.v(m_SQL,m_SQL.DeleteThisMarriage(m_iPersonID, m_iSpouseID));
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
                                  string                 sParent,
                                  ref int                iParentID,
                                  TextBox                Parents_DateMarried,
                                  TextBox                Parents_ParentsDivorced,
                                  int                    iPersonID,
                                  int                    iFatherID,
                                  int                    iMotherID)
        {
            bDataLoaded = false;
            DisplayParentMarriage(Parents_DateMarried, Parents_ParentsDivorced, iFatherID, iMotherID);
            Q.i(m_SQL,m_SQL.UpdatePersonFatherOrMother(sParent, iPersonID, iParentID));
            iParentID = 0;
            PopulateListBox(Parent_TextBox, iParentID);
            SetupContextMenus();
            bDataLoaded = true;
        }
        //****************************************************************************************************************************
        private void PersonFatherRemove_Clicked(object sender, EventArgs e)
        {
            if (RemovePerson(m_iPersonFatherID))
            {
                RemoveParent(PersonFather_listBox, "FatherID", ref m_iPersonFatherID, PersonParentsDateMarried_textBox,
                             PersonParentsDivorced_textBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
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
                             PersonParentsDivorced_textBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
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
                             PersonParentsDivorced_textBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
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
                             PersonParentsDivorced_textBox, m_iPersonID, m_iPersonFatherID, m_iPersonMotherID);
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
                             SpouseParentsDivorced_textBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
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
                             SpouseParentsDivorced_textBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
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
                             SpouseParentsDivorced_textBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
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
                             SpouseParentsDivorced_textBox, m_iSpouseID, m_iSpouseFatherID, m_iSpouseMotherID);
            }
            else
                m_iSpouseMotherID = iTmpSpouseID;
        }
    }
}