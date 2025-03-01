    using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {
        //****************************************************************************************************************************
        public static bool GetPerson(ref DataSet Person_ds,
                                     int iPersonID)
        {
            Person_ds.Clear();
            string sPersonSelect_cmd = SelectAllString(U.Person_Table, U.NoOrderBy, U.PersonID_col);
            string sCategorySelectcmd = CategoryValueSelectCommand(U.PersonCategoryValue_Table, U.PersonID_col, iPersonID);
            string sBuildingSelectcmd = BuildingOccupantSelectCommand(U.BuildingOccupant_Table, U.PersonID_col);
            string sVitalRecordSelectcmd = SelectAllString(U.VitalRecord_Table, U.NoOrderBy, U.PersonID_col);
            string sCemeteryRecordSelectcmd = SelectAllString(U.CemeteryRecord_Table, U.NoOrderBy, U.PersonID_col);
            string sSchoolRecordSelectcmd = SelectAllString(U.SchoolRecord_Table, U.NoOrderBy, U.PersonID_col);
            string sMarriageRecordSelectcmd = MarriageSelectPersonCommand();
            SqlCommand cmd = new SqlCommand(sPersonSelect_cmd + sCategorySelectcmd + sBuildingSelectcmd +
                                            sVitalRecordSelectcmd + sCemeteryRecordSelectcmd + sSchoolRecordSelectcmd + sMarriageRecordSelectcmd, sqlConnection);
            cmd.Parameters.Add(new SqlParameter("@" + U.PersonID_col, iPersonID));
            ExecuteSelectStatement(Person_ds, cmd, U.Person_Table, U.PersonCategoryValue_Table, U.BuildingOccupant_Table,
                                   U.VitalRecord_Table, U.CemeteryRecord_Table, U.SchoolRecord_Table, U.Marriage_Table);
            MakeSureAllMarriagesHavePersonAsPrimary(iPersonID, Person_ds.Tables[U.Marriage_Table]);
            return true;
        }
        //****************************************************************************************************************************
        public static void MakeSureAllMarriagesHavePersonAsPrimary(int iPersonID,
                                                                   DataTable marriage_tbl)
        {
            foreach (DataRow marriage_row in marriage_tbl.Rows)
            {
                if (marriage_row[U.PersonID_col].ToInt() != iPersonID)
                {
                    ReversePersons(marriage_row);
                }
            }
        }
        //****************************************************************************************************************************
        private static void ReversePersons(DataRow marriage_row)
        {
            object tmp = marriage_row[U.SpouseID_col];
            marriage_row[U.SpouseID_col] = marriage_row[U.PersonID_col];
            marriage_row[U.PersonID_col] = tmp;
        }
        //****************************************************************************************************************************
        public static bool GetPerson(DataTable Person_tbl,
                              int iPersonID)
        {
            SelectAll(U.Person_Table, Person_tbl, new NameValuePair(U.PersonID_col, iPersonID));
            return (Person_tbl.Rows.Count != 0);
        }
        //****************************************************************************************************************************
        public static string GetPersonNameWithoutMarriedName(int personID)
        {
            DataTable tbl = new DataTable(U.Person_Table);
            if (GetPersonNameValues(tbl, personID))
            {
                DataRow row = tbl.Rows[0];
                return BuildNameStringWithoutMarriedName(row[U.FirstName_col].ToString(), row[U.MiddleName_col].ToString(),
                                                         row[U.LastName_col].ToString(), row[U.Suffix_col].ToString(),
                                                         row[U.Prefix_col].ToString(), row[U.KnownAs_col].ToString());
            }
            else
                return "";
        }
        //****************************************************************************************************************************
        public static int GetPersonIDFromImportPersonIDFromTable(DataTable Person_tbl,
                                                           int iImportPersonID)
        // Look to see if the ImportPersonID exists in the DataTable.  
        // The ImportPersonID is the PersonID which exist in the From Database
        // Return the PersonId from the main database which corresponds to the ImportPersonID.
        {
            DataRow Person_row = Person_tbl.Rows.Find(iImportPersonID);
            if (Person_row == null)
                return 0;
            else
            {
                return Person_row[U.PersonID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetAllBuildingOccupants()
        {
            DataTable tbl = new DataTable();
            SelectAll(U.BuildingOccupant_Table, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllBuildingOccupants(int iBuildingID)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.BuildingOccupant_Table, tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static bool GetAllBuildingOccupants(DataTable tbl,
                                            int iBuildingID)
        {
            SelectAll(U.BuildingOccupant_Table, OrderBy(U.BuildingValueOrder_col), tbl, new NameValuePair(U.BuildingID_col, iBuildingID));
            return true;
        }
        //****************************************************************************************************************************
    }
}
