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
        public static bool SaveOrder(int iBuildingType,
                              int iID,
                              int iBuildingID,
                              int iOrder)
        {
            switch (iBuildingType)
            {
                case U.iBuilding:
                    break;
                case U.i1856BuildingName:
                    {
                        UpdateBuildingValue(iID, U.BuildingValueOrder1856Name_col, iOrder.ToString());
                        break;
                    }
                case U.i1869BuildingName:
                    {
                        UpdateBuildingValue(iID, U.BuildingValueOrder1869Name_col, iOrder.ToString());
                        break;
                    }
                case U.iOccupant:
                    UpdateBuildingOccupant(iID, iBuildingID, U.BuildingValueOrder_col, iOrder.ToString());
                    break;
                default:
                    UpdateBuildingValueField(iBuildingID, U.BuildingValueOrder_col, iOrder.ToString());
                    break;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool SaveNotes(int iBuildingType,
                                int iID,
                                int iBuildingValueID,
                                string sNotes)
        {
            switch (iBuildingType)
            {
                case U.iBuilding:
                    UpdateBuildingValue(iID, U.Notes_col, sNotes);
                    break;
                case U.iOccupant:
                    UpdateBuildingOccupant(iID, iBuildingValueID, U.Notes_col, sNotes);
                    break;
                case U.i1856BuildingName:
                    UpdateBuildingValue(iID, U.Notes1856Name_col, sNotes);
                    break;
                case U.i1869BuildingName:
                    UpdateBuildingValue(iID, U.Notes1869Name_col, sNotes);
                    break;
                case U.iCurrentOwners:
                    {
                        int iBuildingID = GetBuildingIDFromGrandListID(iID);
                        UpdateBuildingValue(iBuildingID, U.NotesCurrentOwner_col, sNotes);
                        break;
                    }
                default:
                    UpdateBuildingValueField(iBuildingValueID, U.Notes_col, sNotes);
                    break;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static void UpdateBuildingValueField(int iBuildingValueID,
                                            string sString_col,
                                            string sStringValue)
        {
            UpdateWithParms(U.BuildingValue_Table, new NameValuePair(U.BuildingValueID_col, iBuildingValueID),
                                                   new NameValuePair(sString_col, sStringValue, false));
        }
        //****************************************************************************************************************************
        public static bool UpdateBuildingValue(DataTable tbl)
        {
            return UpdateWithDA(tbl, U.BuildingValue_Table, U.BuildingValueID_col, ColumnList(U.BuildingValueValue_col));
        }
        //****************************************************************************************************************************
    }
}
