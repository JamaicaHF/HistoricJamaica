using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SQL_Library;

namespace HistoricJamaica
{
    public class CPersonFilter : FPerson
    {
        private string m_sTableName;
        private string m_sTableIDCol;
        private DataTable m_tbl;
        private bool m_bDoCheckMarriedKnownAsNames;
        //****************************************************************************************************************************
        public CPersonFilter(CSql      SQL,
                             DataTable tbl,
                             bool      bDoCheckMarriedKnownAsNames,
                             string    sTableName,
                             string    sTableIDCol): base(SQL) 
        {
            m_SQL = SQL;
            m_tbl = tbl;
            m_bDoCheckMarriedKnownAsNames = bDoCheckMarriedKnownAsNames;
            m_sTableName = sTableName;
            m_sTableIDCol = sTableIDCol;
            HideNonusedObjects();
            SearchAll_button.Location = new System.Drawing.Point(80, 250);
            SearchPartial_Button.Location = new System.Drawing.Point(80, 300);
            StartingWith_button.Location = new System.Drawing.Point(80, 350);
            Similar_button.Location = new System.Drawing.Point(80, 400);
            NewPerson_button.Visible = false;
            SearchBy = eSearchOption.SO_AllNames;
            this.Size = new System.Drawing.Size(300, 500);
        }
        private void HideNonusedObjects()
        {
            Description_TextBox.Visible = false;
            ViewPhotographs_button.Visible = false;
            BornDate_textBox.Visible = false;
            BornPlace_textBox.Visible = false;
            DiedDate_textBox.Visible = false;
            DiedPlace_textBox.Visible = false;
            Father_textBox.Visible = false;
            Mother_textBox.Visible = false;
            Spouse_textBox.Visible = false;
            Male_radioButton.Visible = false;
            BornVerified_checkBox.Visible = false;
            DiedVerified_checkBox.Visible = false;
            Female_radioButton.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            DiedPlace_label.Visible = false;
            Sex_groupBox.Visible = false;
            BornSource_textBox.Visible = false;
            Born_groupBox.Visible = false;
            label25.Visible = false;
            BornHome_textBox.Visible = false;
            BornPage_textBox.Visible = false;
            label15.Visible = false;
            BornBook_textBox.Visible = false;
            label14.Visible = false;
            label12.Visible = false;
            Died_groupBox.Visible = false;
            label26.Visible = false;
            DiedHome_textBox.Visible = false;
            DiedPage_textBox.Visible = false;
            label11.Visible = false;
            DiedBook_textBox.Visible = false;
            DiedPage_label.Visible = false;
            label13.Visible = false;
            DiedSource_textBox.Visible = false;
            label5.Visible = false;
            Save_button.Visible = false;
            SimilarNames_checkBox.Visible = false;
            NameOptions_groupBox.Visible = false;
            Buried_groupBox.Visible = false;
            BuriedVerified_checkBox.Visible = false;
            label29.Visible = false;
            BuriedSource_textBox.Visible = false;
            label28.Visible = false;
            label27.Visible = false;
            BuriedPage_textBox.Visible = false;
            BuriedBook_textBox.Visible = false;
            BuriedDate_textBox.Visible = false;
            BuriedStone_textBox.Visible = false;
            label22.Visible = false;
            BuriedPlace_textBox.Visible = false;
            label17.Visible = false;
            label16.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            label23.Visible = false;
            Family_button.Visible = false;
            UniquePhotos_button.Visible = false;
            AdditionalSpouse_button.Visible = false;
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // empty but must be here to avoid the OnFormClosing in parent class does not execute
        }
        //****************************************************************************************************************************
        protected override void SearchAll_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            SearchBy = eSearchOption.SO_AllNames;
            PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
            SQL.PersonsBasedOnNameOptions(m_tbl, m_bDoCheckMarriedKnownAsNames, m_sTableName, U.PersonID_col, SearchBy, personName, m_MarriedName);
            Close();
        }
        //****************************************************************************************************************************
        protected override void SearchStartingWith_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            SearchBy = eSearchOption.SO_StartingWith;
            PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
            SQL.PersonsBasedOnNameOptions(m_tbl, m_bDoCheckMarriedKnownAsNames, m_sTableName, U.PersonID_col, SearchBy, personName, m_MarriedName);
            Close();
        }
        //****************************************************************************************************************************
        protected override void SearchPartial_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            SearchBy = eSearchOption.SO_PartialNames;
            PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
            SQL.PersonsBasedOnNameOptions(m_tbl, m_bDoCheckMarriedKnownAsNames, m_sTableName, U.PersonID_col, SearchBy, personName, m_MarriedName);
            Close();
        }
        //****************************************************************************************************************************
        protected override void SearchSimilar_button_Click(object sender, System.EventArgs e)
        {
            SetAllNameValues();
            SearchBy = eSearchOption.SO_Similar;
            PersonName personName = new PersonName(m_FirstName, m_MiddleName, m_LastName, m_Suffix, m_Prefix);
            SQL.PersonsBasedOnNameOptions(m_tbl, m_bDoCheckMarriedKnownAsNames, m_sTableName, m_sTableIDCol, SearchBy, personName, m_MarriedName);
            Close();
        }
        //****************************************************************************************************************************
    }
}
