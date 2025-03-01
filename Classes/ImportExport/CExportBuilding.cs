using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class CExportBuilding
    {
        private CSql m_SQL;
        private StreamWriter m_StreamWriter;
        public CExportBuilding(CSql SQL)
        {
            m_SQL = SQL;
            ExportBuilding();
        }
        //****************************************************************************************************************************
        protected string GetFileNameFromPath(string sFileNameWithPath)
        {
            char[] c = new char[1];
            c[0] = '\\';
            int iIndexOfLastBackslash = sFileNameWithPath.LastIndexOfAny(c);
            return sFileNameWithPath.Substring(iIndexOfLastBackslash + 1);
        }
        //****************************************************************************************************************************
        public bool OpenOutputFile(string sFileNameWithPath)
        {
            sFileNameWithPath = "c:\\JamaicaHF\\" + sFileNameWithPath;
            if (sFileNameWithPath.Length == 0)
            {
                return false;
            }
            string sFileName = GetFileNameFromPath(sFileNameWithPath);
            try
            {
                m_StreamWriter = new StreamWriter(sFileNameWithPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public void CloseOutputFile()
        {
            m_StreamWriter.Close();
        }
        //****************************************************************************************************************************
        private void WriteString(string sText)
        {
            m_StreamWriter.Write(sText);
            m_StreamWriter.Write(UU.crlf());
        }
        //****************************************************************************************************************************
        private void PrintLine(string sNotes)
        {
            sNotes = sNotes.Trim();
            while (sNotes.Length > 0)
            {
                int iStrLen = 55;
                if (sNotes.Length < iStrLen)
                    iStrLen = sNotes.Length;
                else
                {
                    int iLastSpace = sNotes.LastIndexOf(' ');
                    if (iLastSpace > 0)
                    {
                        sNotes = sNotes.Remove(iLastSpace, 1);
                        iStrLen = iLastSpace;
                    }
                }
                string sStr = sNotes.Substring(0, iStrLen);
                WriteString("        " + sStr);
                sNotes = sNotes.Remove(0, iStrLen);
            }
        }
        //****************************************************************************************************************************
        private bool DateBeforeDash(string sNotes,
                                    int iDashLocation)
        {
            if (iDashLocation > 5 || iDashLocation < 3)
                return false;
            string sSubString = sNotes.Substring(0, 4);
            int iDate = sSubString.ToIntNoError();
            if (iDate < 1000 || iDate > 2099)
                return false;
            else
                return true;
        }
        //****************************************************************************************************************************
        private void WriteNotes(string sNotes)
        {
            while (sNotes.Length > 0)
            {
                int iDashLocation = sNotes.IndexOf('-', 0);
                if (iDashLocation >= 0)
                {
                    if (DateBeforeDash(sNotes, iDashLocation))
                    {
                        iDashLocation = sNotes.IndexOf('-', iDashLocation+1);
                        if (iDashLocation < 0)
                        {
                            PrintLine(sNotes);
                            return;
                        }
                    }
                    sNotes = sNotes.Remove(iDashLocation, 1);
                    string sPartialNotes = sNotes.Substring(0, iDashLocation);
                    PrintLine(sPartialNotes);
                    sNotes = sNotes.Remove(0, sPartialNotes.Length);
                }
                else
                {
                    PrintLine(sNotes);
                    return;
                }
            }
        }
        //****************************************************************************************************************************
        private void WriteOccupants(int buildingID,
                                    DataTable Occupant_tbl)
        {
            if (Occupant_tbl.Rows.Count != 0)
            {
                WriteString("");
                WriteString("Building Occupants");
                foreach (DataRow row in Occupant_tbl.Rows)
                {
                    int iPersonID = row[U.PersonID_col].ToInt();
                    int iBuildingID = row[U.BuildingID_col].ToInt();
                    int iSpouseLivedWithID = row[U.SpouseLivedWithID_col].ToInt();
                    string sHome = SQL.PersonHomeName(iPersonID, iBuildingID, iSpouseLivedWithID, false);
                    sHome += (" " + U.GetOccupantString(iPersonID, buildingID));
                    string sNotes = BuildingNotes.AddCensusValues(row[U.Notes_col].ToString(), iPersonID, buildingID);
                    WriteString("    " + sHome + " " + sNotes);
                }
            }
        }
        //****************************************************************************************************************************
        private void WriteBuildingNames(string BuildingMapName,
                                        string BuildingMapNotes,
                                        DataTable BuildingValues_tbl)
        {
            int iNumBuildingNames = BuildingValues_tbl.Rows.Count;
            if (iNumBuildingNames <= 1 && BuildingMapName.Length == 0)
                return;
            WriteString("");
            WriteString("Building Names");
            if (BuildingMapName.Length != 0)
            {
                string sNotes = "";
                if (BuildingMapNotes.Length != 0)
                {
                    sNotes = " - " + BuildingMapNotes;
                }
                WriteString("    " + BuildingMapName + " (1869 Building Name)" + sNotes);
            }
            for (int iBuildingName = 1; iBuildingName < iNumBuildingNames; iBuildingName++)
            {
                DataRow row = BuildingValues_tbl.Rows[iBuildingName];
                int iBuildingValueID = row[U.BuildingValueID_col].ToInt();
                string sBuildingValueValue = row[U.BuildingValueValue_col].ToString();
                string sNotes = row[U.Notes_col].ToString();
                if (sNotes.Length != 0)
                {
                    sNotes = " - " + sNotes;
                }
                WriteString("    " + sBuildingValueValue + sNotes);
//                WriteNotes(sNotes);
            }
        }
        //****************************************************************************************************************************
        private void WriteBuilding(DataRow building_row)
        {
            int iBuildingID = SQL.BuildingIDFromGrandListID(building_row["ID"].ToInt());
            DataTable tbl = new DataTable();
            if (iBuildingID != 0)
                tbl = SQL.GetBuilding(iBuildingID);
            if (tbl.Rows.Count == 0)
                return;

            DataTable BuildingValues_tbl = new DataTable();
            SQL.GetAllBuildingValues(BuildingValues_tbl, iBuildingID);
            if (BuildingValues_tbl.Rows.Count == 0)
                return;

            WriteString("*********************************************************************");
            int iStreetNum = building_row[U.StreetNum_col].ToInt();
            string sRoadName = SQL.GetModernRoadName(building_row[U.BuildingRoadValueID_col].ToInt());
            string sAddress = iStreetNum.ToString() + " " + sRoadName;

            DataRow row = BuildingValues_tbl.Rows[0];
            DataRow BuildingRow = tbl.Rows[0];
            string sNotes = BuildingRow[U.Notes_col].ToString();
            DataTable Occupant_tbl = new DataTable();
            int iBuildingValueID = row[U.BuildingValueID_col].ToInt();
            SQL.GetAllBuildingOccupants(Occupant_tbl, iBuildingID);

            string sBuildingName = row[U.BuildingValueValue_col].ToString().Trim();
            WriteString(sBuildingName + " - " + sAddress);

            WriteString("");
            WriteString("Current Owner");
            string sName1 = building_row[U.Name1_col].ToString();
            string sName2 = building_row[U.Name2_col].ToString();
            WriteString("    " + U.CombineName1AndName2(sName1, sName2));

            WriteBuildingNames(BuildingRow[U.Building1856Name_col].ToString(), BuildingRow[U.Notes1856Name_col].ToString(), BuildingValues_tbl);
            WriteBuildingNames(BuildingRow[U.Building1869Name_col].ToString(), BuildingRow[U.Notes1869Name_col].ToString(), BuildingValues_tbl);
            WriteOccupants(iBuildingID, Occupant_tbl);
        }
        //****************************************************************************************************************************
        private void GetBuildings(int iModernRoadID)
        {
            DataTable Building_tbl = SQL.GetBuildingByModernRoadID(iModernRoadID);
            foreach (DataRow Building_row in Building_tbl.Rows)
            {
                WriteBuilding(Building_row);
            }
            DataTable Road_tbl = SQL.GetBuildingByRoadNoGrandListID(iModernRoadID);
            foreach (DataRow Road_row in Road_tbl.Rows)
            {
            }
        }
        //****************************************************************************************************************************
        private void ExportBuilding()
        {
            if (OpenOutputFile("Buildings.txt"))
            {
                //GetBuildings(92); // Main Street
                //GetBuildings(84); // Depot Street
                //GetBuildings(83); // Water Street
                //GetBuildings(224); // Town Shed Road
                //GetBuildings(1); // Pikes Falls Road Mechanic St
                //GetBuildings(232); // Williams Road
                //GetBuildings(113); // Castle Hill Road
                //GetBuildings(66); // Pikes Falls Road
                GetBuildings(163); // Factory Street
                CloseOutputFile();
                MessageBox.Show("Export Complete");
            }
            else
            {
                MessageBox.Show("Unable to open export file");
            }
        }
    }
}
