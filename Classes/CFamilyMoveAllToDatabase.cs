using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    class CFamilyMoveAllToDatabase : FFamily
    {
        private CSql m_SQLMain;
        private CSql m_SQLFrom;
        private Button MoveAll_button = new System.Windows.Forms.Button();
        private Button ShowCemetery_button = new System.Windows.Forms.Button();
        private DataTable m_Cemetery_tbl = new DataTable();
        public CFamilyMoveAllToDatabase(CSql SQLFrom,
                                     CSql SQLMain,
                                     int  iPersonID):base(SQLFrom,iPersonID)
        {
            m_SQLMain = SQLMain;
            m_SQLFrom = SQLFrom;
            InitializeComponent();
            SQL.SelectAll(U.CemeteryRecord_Table, m_Cemetery_tbl);
            if (m_Cemetery_tbl.Rows.Count == 0)
                ShowCemetery_button.Visible = false;
        }
        //****************************************************************************************************************************
        private void InitializeComponent()
        {
            // 
            // MoveAll_button
            // 
            Controls.Add(MoveAll_button);
            MoveAll_button.Location = new System.Drawing.Point(975, 622);
            MoveAll_button.Name = "MoveAll_button";
            MoveAll_button.Size = new System.Drawing.Size(75, 23);
            MoveAll_button.TabIndex = 46;
            MoveAll_button.Text = "Move All";
            MoveAll_button.UseVisualStyleBackColor = true;
            MoveAll_button.Click += new System.EventHandler(MoveAll_button_Click);

            Controls.Add(ShowCemetery_button);
            ShowCemetery_button.Location = new System.Drawing.Point(775, 622);
            ShowCemetery_button.Name = "ShowCemetery_button";
            ShowCemetery_button.Size = new System.Drawing.Size(75, 23);
            ShowCemetery_button.TabIndex = 46;
            ShowCemetery_button.Text = "Cemetery";
            ShowCemetery_button.UseVisualStyleBackColor = true;
            ShowCemetery_button.Click += new System.EventHandler(ShowCemetery_Button_Click);
        }
        //****************************************************************************************************************************
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (SQL.IsFullDatabase())
                return;
            switch (MessageBox.Show("Save Import?", "", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    if (!ImportFamily())
                        e.Cancel = true;
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
        //****************************************************************************************************************************
        private int CheckForSimilarNames(DataRow row)
        // Look to see if the new person already exists in the database.  Look for all similar names incase the names are mispelled
        {
            DataTable MainPerson_tbl = SQL.DefinePersonTable();
            PersonName personName = new PersonName(row[U.FirstName_col].ToString(), row[U.MiddleName_col].ToString(),
                                                   row[U.LastName_col].ToString(), row[U.Suffix_col].ToString(), row[U.Prefix_col].ToString());
            if (SQL.PersonExists(MainPerson_tbl, true, U.Person_Table, U.PersonID_col, personName))
//            if (m_SQLMain.PersonExists(MainPerson_tbl, true, U.Person_Table, U.PersonID_col, row[U.MarriedName_col].ToString(),
//                                       row[U.KnownAs_col].ToString(),row[U.FirstName_col].ToString(), row[U.MiddleName_col].ToString(), 
//                                       row[U.LastName_col].ToString(), row[U.Suffix_col].ToString(), row[U.Prefix_col].ToString()))
            {
                m_iPersonID = row[U.PersonID_col].ToInt();
                DisplayScreen();
                CGridPerson GridPerson = new CGridPerson(m_SQLMain, ref MainPerson_tbl, false, 0, true, "Person");
                GridPerson.ShowDialog();
                return GridPerson.SelectedPersonID;
            }
            return 0;
        }
        //****************************************************************************************************************************
        private bool AddPersonToUpdateTable(DataTable MainUpdatePerson_tbl,
                                            int       iMainPersonID,
                                            int       iNewFatherID,
                                            int       iNewMotherID)
        // Add the new person to the UpdateTable of persons who already exist in the main database.
        // Save the Father and mother IDs in the from table and get the Mother and Father IDS in the main database.
        // Later we will ensure there is no conflict between who the father and mother are within the new and main database.
        {
            DataRow UpdateRow = MainUpdatePerson_tbl.NewRow();
            int iMainFatherID;
            int iMainMotherID;
            SQL.GetFatherMother(iMainPersonID, out iMainFatherID, out iMainMotherID);
            UpdateRow[U.PersonID_col] = iMainPersonID;
            UpdateRow[U.ImportPersonID_col] = iMainPersonID;
            UpdateRow[U.FatherID_col] = iNewFatherID;
            UpdateRow[U.MotherID_col] = iNewMotherID;
            UpdateRow[U.MainFatherID_col] = iMainFatherID;
            UpdateRow[U.MainMotherID_col] = iMainMotherID;
            try
            {
                MainUpdatePerson_tbl.Rows.Add(UpdateRow);
                return true;
            }
            catch (ConstraintException)
            {
                MessageBox.Show("This person has already been selected");
                return false;
            }
        }
        //****************************************************************************************************************************
        private bool AddNewPersonsToEitherTheInsertOrUpdateTable(DataTable FromPerson_tbl, 
                                                                 DataTable MainInsertPerson_tbl, 
                                                                 DataTable MainUpdatePerson_tbl)
        // If the new person already exists in the database, add the new person into the Update Person table, 
        // otherwise add the new person into the Insert Person table
        {
            m_SQLFrom.selectall(FromPerson_tbl, U.Person_Table, U.NoOrderBy);
            //
            // Include only the original rows, not the new rows added below
            DataViewRowState dvRowState = DataViewRowState.OriginalRows;
            foreach (DataRow From_row in FromPerson_tbl.Select("", "", dvRowState))
            {
                From_row[U.ImportPersonID_col] = From_row[U.PersonID_col];
                bool bFoundPerson = false;
                do
                {
                    int iSimilarID = CheckForSimilarNames(From_row);
                    if (iSimilarID == U.Exception) // abort key pressed
                        return false;
                    if (iSimilarID != 0) // New person already in database
                    {
                        if (AddPersonToUpdateTable(MainUpdatePerson_tbl, iSimilarID,
                                                   From_row[U.FatherID_col].ToInt(), From_row[U.MotherID_col].ToInt()))
                        {
                            bFoundPerson = true;
                            From_row[U.PersonID_col] = iSimilarID;
                            FromPerson_tbl.Rows.Add(From_row.ItemArray);  // This statement must be between previous and next
                            From_row[U.ImportPersonID_col] = iSimilarID;
                        }
                        else
                            bFoundPerson = false;
                    }
                    else
                    {
                        MainInsertPerson_tbl.Rows.Add(From_row.ItemArray);
                        bFoundPerson = true;
                    }
                }
                while (!bFoundPerson);
            }
            return true;
        }
        //****************************************************************************************************************************
        private bool MoveAllFromCopyToMain()
        {
            m_SQLFrom.SetAllImportPersonIDsToZero();
            m_SQLMain.SetAllImportPersonIDsToZero();
            //
            // FromPerson_tbl will contain all the person rows from the import file
            DataTable FromPerson_tbl = SQL.DefinePersonTable();
            // 
            // MainInsertPerson_tbl will contain all the new people to be add to the main database
            DataTable MainInsertPerson_tbl = SQL.DefinePersonTable();
            MainInsertPerson_tbl.PrimaryKey = new DataColumn[] { MainInsertPerson_tbl.Columns[U.ImportPersonID_col] };
            //
            // MainUpdatePerson_tbl will contain all the people already within the main database
            DataTable MainUpdatePerson_tbl = SQL.DefinePersonImportIDTable();
            //
            if (!AddNewPersonsToEitherTheInsertOrUpdateTable(FromPerson_tbl, MainInsertPerson_tbl, MainUpdatePerson_tbl))
                return false; // abort key pressed
            //
            try // use a try catch block to ensure that there are no duplicates in the Original table. Do this
            {   // after updating the ImportPersonID with the PersonID for each row in the person table
                FromPerson_tbl.PrimaryKey = new DataColumn[] { FromPerson_tbl.Columns[U.ImportPersonID_col] };
            }
            catch (ArgumentException)
            {
                MessageBox.Show("The same person was selected more than once");
                return false;
            }
            DataTable FromMarriage_tbl = SQL.DefineMarriageTable();
            m_SQLFrom.selectall(FromMarriage_tbl, U.Marriage_Table, U.NoOrderBy);
            if (SQL.ThereAreNoConflictsForFatherOrMotherIDs(MainUpdatePerson_tbl) &&
                SQL.InsertPersonTable(FromPerson_tbl, MainInsertPerson_tbl, FromMarriage_tbl, m_Cemetery_tbl))
            {
                return true;
            }
            else
                return false;
        }
        //****************************************************************************************************************************
        private bool ImportFamily()
        {
            if (MoveAllFromCopyToMain())
            {
                MessageBox.Show("Import Succesful");
                return true;
            }
            else
            {
                MessageBox.Show("Import Unsuccesful");
                return false;
            }
        }
        //****************************************************************************************************************************
        private void MoveAll_button_Click(object sender, EventArgs e)
        {
            if (ImportFamily())
            {
                m_SQL = m_SQLMain;
                m_SQLFrom = m_SQLMain;
                m_iPersonID = SQL.GetPersonIDFromImportPersonIDFromDatabase(m_iPersonID);
                DisplayScreen();
                MoveAll_button.Visible = false;
            }
        }
        //****************************************************************************************************************************
        private void ShowCemetery_Button_Click(object sender, EventArgs e)
        {
            CGridCemetery GridCemetery = new CGridCemetery(m_SQL, m_Cemetery_tbl);
            GridCemetery.ShowDialog();
            m_iPersonID = GridCemetery.SelectedPersonID;
            DisplayScreen();
        }
    }
}
