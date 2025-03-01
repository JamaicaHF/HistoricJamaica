using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    public class CImportModernRoadValues : CImport
    {
        private string m_sInputRecord;
        //****************************************************************************************************************************
        public CImportModernRoadValues(CSql Sql, string sDataDirectory): base(Sql, sDataDirectory)
        {
            string sFilter = "CSV Delimited Files (csv)|*.csv";
            if (!OpenInputFile(sFilter))
                return;
            SQL.InsertModernRoadName(1, "Pikes Falls-Mechanic St");
            SQL.InsertModernRoadName(2, "Route 30-Rawsonville");
            UpdateGrandListTable(66);
            UpdateGrandListTable(235);
            m_sInputRecord = ReadRecord();
            while (m_sInputRecord != null)
            {
                string[] sInputFields = m_sInputRecord.Split(',');
                int roadValueID = Field(sInputFields, 0).ToInt();
                int section = Field(sInputFields, 1).ToInt();
                AddJName(roadValueID, SQL.GetModernRoadName(roadValueID));
                SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueSection_col, section);
                if (roadValueID == 1)
                {
                    SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueOrder_col, 3);
                }
                if (roadValueID == 2)
                {
                    SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueOrder_col, 7);
                }
                if (roadValueID == 66)
                {
                    SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueOrder_col, 9999);
                }
                if (roadValueID == 92)
                {
                    SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueValue_col, "Main Street");
                }
                if (roadValueID == 235)
                {
                    SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueOrder_col, 9999);
                }
                m_sInputRecord = ReadRecord();
            }
            RemoveUnusedRoads();
            CloseInputFile();
        }
        //****************************************************************************************************************************
        private void AddJName(int roadValueID, string name)
        {
            int jLocation = name.LastIndexOfAny(new char[] {'J'});
            if (jLocation < 0)
            {
                return;
            }
            int num_location = name.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, jLocation+1);
            if (num_location < 0)
            {
                return;
            }
            string jRoadName = name.Substring(jLocation);
            int dashLocation = name.LastIndexOfAny(new char[] { '-' });
            if (dashLocation >= 0)
            {
                jLocation = dashLocation;
            }
            name = name.Remove(jLocation).Trim();
            SQL.UpdateModernRoadValue(roadValueID, U.JRoadName_col, jRoadName);
            SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueValue_col, name);
        }
        private void RemoveUnusedRoads()
        {
            DataTable tbl = SQL.GetAllModernRoadValues();
            foreach (DataRow row in tbl.Rows)
            {
                int roadValueID = row[U.ModernRoadValueID_col].ToInt();
                if (roadValueID == 193)
                {
                }
                DataTable GrandList_tbl = SQL.GetBuildingByModernRoadID(roadValueID);
                if (GrandList_tbl.Rows.Count == 0)
                {
                    DataRow modernRoad_row = SQL.GetModernRoad(roadValueID);
                    if (modernRoad_row[U.ModernRoadValueSection_col].ToInt() != 0)
                    {
                        SQL.UpdateModernRoadValue(roadValueID, U.ModernRoadValueSection_col, 0);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private void UpdateGrandListTable(int roadID)
        {
            DataTable grandList_tbl = SQL.GetBuildingByModernRoadID(roadID);
            foreach (DataRow row in grandList_tbl.Rows)
            {
                if (row[U.BuildingRoadValueID_col].ToInt() == 66 && row[U.StreetNum_col].ToInt() > 0 && row[U.StreetNum_col].ToInt() < 200)
                {
                    row[U.BuildingRoadValueID_col] = 1;
                }
                if (row[U.BuildingRoadValueID_col].ToInt() == 235 && row[U.StreetNum_col].ToInt() > 8550)
                {
                    row[U.BuildingRoadValueID_col] = 2;
                }
            }
            ArrayList fieldList = new ArrayList();
            fieldList.Add(U.BuildingRoadValueID_col);
            SQL.UpdateGrandlist(grandList_tbl, fieldList);
        }
        //****************************************************************************************************************************
        private string Field(string[] sInputFields,
                             int iFieldNum)
        {
            if (sInputFields.Length > iFieldNum)
            {
                string sInputField = sInputFields[iFieldNum];
                const string sQuotesWithSlashBefore = @"""";
                sInputField = sInputField.Replace(sQuotesWithSlashBefore, "");
                return sInputField;
            }
            else
                return "";
        }
        //****************************************************************************************************************************
    }
}
