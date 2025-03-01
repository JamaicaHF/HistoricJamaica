using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    public static class BuildingNotes
    {
        //****************************************************************************************************************************
        public static string GetNotesWithCensusInfo(int iBuildingType,
                                                    int iID,
                                                    int iBuildingOrPersonID)
        {
            string notes = GetNotes(iBuildingType, iID, iBuildingOrPersonID);
            switch (iBuildingType)
            {
                case U.iOccupant:
                    return AddCensusValues(notes, iID, iBuildingOrPersonID);
                default:
                    return notes;
            }
        }
        //****************************************************************************************************************************
        public static string GetCensusYears(int iBuildingType,
                                            int iID,
                                            int iBuildingValueOrPersonID)
        {
            switch (iBuildingType)
            {
                case U.iBuilding:
                    return SQL.GetBuildingValue(U.Notes_col, iID);
                case U.iOccupant:
                    return GetPersonBuildingNotes(iID, iBuildingValueOrPersonID);
                case U.iCurrentOwners:
                    {
                        int iBuildingID = SQL.GetBuildingIDFromGrandListID(iID);
                        return SQL.GetBuildingValue(U.NotesCurrentOwner_col, iBuildingID);
                    }
                case U.i1856BuildingName:
                    return SQL.GetBuildingValue(U.Notes1856Name_col, iID);
                case U.i1869BuildingName:
                    return SQL.GetBuildingValue(U.Notes1869Name_col, iID);
                default:
                    return SQL.GetBuildingValueNotes(iID);
            }
        }
        //****************************************************************************************************************************
        public static string GetNotes(int iBuildingType,
                                      int iID,
                                      int iBuildingValueOrPersonID)
        {
            switch (iBuildingType)
            {
                case U.iBuilding:
                    return SQL.GetBuildingValue(U.Notes_col, iID);
                case U.iOccupant:
                    return GetPersonBuildingNotes(iID, iBuildingValueOrPersonID);
                case U.iCurrentOwners:
                {
                    int iBuildingID = SQL.GetBuildingIDFromGrandListID(iID);
                    return SQL.GetBuildingValue(U.NotesCurrentOwner_col, iBuildingID);
                }
                case U.i1856BuildingName:
                    return SQL.GetBuildingValue(U.Notes1856Name_col, iID);
                case U.i1869BuildingName:
                    return SQL.GetBuildingValue(U.Notes1869Name_col, iID);
                default:
                    return SQL.GetBuildingValueNotes(iID);
            }
        }
        //****************************************************************************************************************************
        public static string AddCensusValues(string notes,
                                              int iPersonID,
                                              int iBuildingID)
        {
            string censusNotes = GetPersonCensusNotes(iPersonID, iBuildingID);
            if (censusNotes.Length != 0 && notes.Length != 0)
            {
                censusNotes += " - ";
            }
            return censusNotes + notes;
        }
        //****************************************************************************************************************************
        public static string GetPersonBuildingNotes(int iPersonID,
                                          int iBuildingID)
        {
            DataTable tbl = new DataTable();
            SQL.SelectAll(U.BuildingOccupant_Table, tbl, new NameValuePair(U.PersonID_col, iPersonID),
                                                        new NameValuePair(U.BuildingID_col, iBuildingID));
            if (tbl.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return tbl.Rows[0][U.Notes_col].ToString();
            }
        }
        //****************************************************************************************************************************
        private static string GetPersonCensusNotes(int iPersonID,
                                                   int iBuildingID)
        {
            DataTable occupantTbl = SQL.GetBuildingOccupant(iPersonID, iBuildingID);
            Census census = new Census();
            if (occupantTbl.Rows.Count != 0)
            {
                DataRow occupantRow = occupantTbl.Rows[0];
                Int64 censusYears = occupantRow[U.CensusYears_col].ToInt64();
                census.LoadCensusData(censusYears);
            }
            return census.StringOfCensusYears();
        }
    }
}
