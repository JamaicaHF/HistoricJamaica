using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static DataTable GetAllBuildingOccupantsForPerson(int personID)
        {
            DataTable building_tbl = DefineBuildingOccupantTable();
            SelectAll(U.BuildingOccupant_Table, building_tbl, new NameValuePair(U.PersonID_col, personID.ToString()));
            return building_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllBuildingsAsSpouse(int SpouseLivedWithID)
        {
            DataTable building_tbl = DefineBuildingOccupantTable();
            SelectAll(U.BuildingOccupant_Table, building_tbl, new NameValuePair(U.SpouseLivedWithID_col, SpouseLivedWithID));
            return building_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetBuildingOccupant(int iPersonID,
                                                    int iBuildingID)
        {
            DataTable tbl = DefineBuildingOccupantTable();
            SelectAll(U.BuildingOccupant_Table, tbl, new NameValuePair(U.PersonID_col, iPersonID),
                                                        new NameValuePair(U.BuildingID_col, iBuildingID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static int GetSpouseLivedWith(int iPersonID,
                                             int iBuildingID)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.BuildingOccupant_Table, tbl, new NameValuePair(U.PersonID_col, iPersonID),
                                                     new NameValuePair(U.BuildingID_col, iBuildingID));
            if (tbl.Rows.Count == 0)
                return 0;
            else
                return tbl.Rows[0][U.SpouseLivedWithID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static bool GetBuildingOccupantRecords(DataTable tbl,
                                                   params int[] buildingIds)
        {
            int iPreviousRecordNumber = tbl.Rows.Count;
            string sOrder = " order by " + U.BuildingID_col;
            NameValuePair[] nameValuePairs = new NameValuePair[buildingIds.Length];
            for (int i = 0; i < buildingIds.Length; i++)
            {
                NameValuePair buildingPair = new NameValuePair(U.BuildingID_col, buildingIds[i]);
                nameValuePairs[i] = buildingPair;
            }
            SelectAllUsingOr(tbl, U.Building_Table, sOrder, nameValuePairs);
            foreach (DataRow row in tbl.Rows)
            {
                row[U.SpouseLivedWithID_col] = 0;
            }
            return (tbl.Rows.Count != iPreviousRecordNumber);
        }
        //****************************************************************************************************************************
        private static string BuildingOccupantSelectCommand(string BuildingOccupantTable,
                                                         string PicturePerson_ID_col)
        {
            return "Select " + U.TableAndColumn(U.Building_Table, U.BuildingID_col) + U.comma +
                                                U.TableAndColumn(U.Building_Table, U.BuildingName_col) + U.comma +
                                                U.TableAndColumn(BuildingOccupantTable, U.SpouseLivedWithID_col) + U.comma +
                                                U.TableAndColumn(BuildingOccupantTable, U.Notes_col) + U.comma +
                                                U.TableAndColumn(BuildingOccupantTable, U.BuildingValueOrder_col) + U.comma +
                                                U.TableAndColumn(BuildingOccupantTable, U.CensusYears_col) +
                  " from [" + BuildingOccupantTable + "],[" + U.Building_Table + "]" +
                  " where (" + U.TableAndColumn(BuildingOccupantTable, ColumnEquals(PicturePerson_ID_col)) +
                  " or " + U.TableAndColumn(BuildingOccupantTable, ColumnEquals(U.SpouseLivedWithID_col, PicturePerson_ID_col)) +
                  ") and " + U.TableAndColumn(BuildingOccupantTable, U.BuildingID_col) + "=" +
                                  U.TableAndColumn(U.Building_Table, U.BuildingID_col) +
                  " order by " + U.TableAndColumn(U.Building_Table, U.BuildingID_col) + ';';
        }
        //****************************************************************************************************************************
    }
}
