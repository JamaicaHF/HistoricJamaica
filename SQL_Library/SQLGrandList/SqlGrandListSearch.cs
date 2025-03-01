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
        public static DataTable GetGrandListFromTaxMapID(string taxMapID)
        {
            DataTable tbl = DefineGrandListTable();
            SelectAll(U.GrandList_Table, tbl, new NameValuePair(U.TaxMapID_col, taxMapID));
            return tbl;
        }
        //****************************************************************************************************************************
        public static int GetGrandListIDFromTaxMapID(string taxMapID)
        {
            DataTable tbl = DefineGrandListTable();
            SelectAll(U.GrandList_Table, tbl, new NameValuePair(U.TaxMapID_col, taxMapID));
            return tbl.Rows.Count == 0 ? 0 : tbl.Rows[0][U.GrandListID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static int GetGrandListIDFromTaxMapIDFromRoadIdAndStreetnum(int buildingRoadValueId, int streetNum)
        {
            DataTable tbl = DefineGrandListTable();
            SelectAll(U.GrandList_Table, tbl, new NameValuePair(U.BuildingRoadValueID_col, buildingRoadValueId),
                                              new NameValuePair(U.StreetNum_col, streetNum));
            return tbl.Rows.Count == 0 ? 0 : tbl.Rows[0][U.GrandListID_col].ToInt();
        }
        //****************************************************************************************************************************
        public static DataTable GetGrandList(int grandListId)
        {
            DataTable tbl = DefineGrandListTable();
            SelectAll(U.GrandList_Table, tbl, new NameValuePair(U.GrandListID_col, grandListId.ToString()));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllModernRoadValues()
        {
            DataTable tbl = new DataTable();
            SelectAll(U.ModernRoadValue_Table, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllGrandList()
        {
            DataTable tbl = new DataTable();
            SelectAll(U.GrandList_Table, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static string GetModernRoadName(int iModernRoadID)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.ModernRoadValue_Table, tbl, new NameValuePair(U.ModernRoadValueID_col, iModernRoadID));
            if (tbl.Rows.Count != 0)
            {
                return tbl.Rows[0][U.ModernRoadValueValue_col].ToString();
            }
            else
            {
                return "";
            }
        }
        //****************************************************************************************************************************
        public static DataRow GetModernRoad(int iModernRoadID)
        {
            DataTable tbl = new DataTable();
            SelectAll(U.ModernRoadValue_Table, tbl, new NameValuePair(U.ModernRoadValueID_col, iModernRoadID));
            if (tbl.Rows.Count != 0)
            {
                return tbl.Rows[0];
            }
            else
            {
                return null;
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetGrandListHistory(int grandListId)
        {
            DataTable tbl = DefineGrandListHistoryTable();
            SelectAll(U.GrandListHistory_Table, tbl, new NameValuePair(U.GrandListID_col, grandListId.ToString()));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllGrandListHistory()
        {
            DataTable tbl = DefineGrandListHistoryTable();
            SelectAll(U.GrandListHistory_Table, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetBuildingByModernRoadID(int ModernRoadID)
        {
            DataTable tbl = DefineGrandListTable();
            string sOrderBy = " order by " + U.StreetNum_col;
            SelectAll(U.GrandList_Table, sOrderBy, tbl, 
                            new NameValuePair(U.BuildingRoadValueID_col, ModernRoadID.ToString()));
            return tbl;
        }
        //****************************************************************************************************************************
        public static int GetModernRoadID(int GrandListID)
        {
            DataTable tbl = DefineGrandListTable();
            SelectAll(U.GrandList_Table, tbl, new NameValuePair(U.GrandListID_col, GrandListID.ToString()));
            if (tbl.Rows.Count != 0)
            {
                DataRow row = tbl.Rows[0];
                return row[U.BuildingRoadValueID_col].ToInt();
            }
            return 0;
        }
        //****************************************************************************************************************************
        public static String GetTaxMapID(int GrandListID)
        {
            DataTable tbl = DefineGrandListTable();
            SelectAll(U.GrandList_Table, tbl, new NameValuePair(U.GrandListID_col, GrandListID.ToString()));
            if (tbl.Rows.Count != 0)
            {
                DataRow row = tbl.Rows[0];
                return row[U.TaxMapID_col].ToString();
            }
            return "";
        }
        //****************************************************************************************************************************
        public static bool GrandListUpperToLower()
        {
            DataTable tbl = GetGrandListPropertys();
            foreach (DataRow row in tbl.Rows)
            {
                int iGrandListID = row["ID"].ToInt();
                string sName1 = LowerCaseNameString(row[U.Name1_col].ToString());
                string sName2 = LowerCaseNameString(row[U.Name2_col].ToString());
                UpdateWithParms(U.GrandList_Table, new NameValuePair(U.GrandListID_col, iGrandListID),
                                     new NameValuePair(U.Name1_col, sName1),
                                     new NameValuePair(U.Name2_col, sName2));
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool GrandListRoads()
        {
            DataTable tbl = GetGrandListPropertys();
            int icount = tbl.Rows.Count;
            foreach (DataRow row in tbl.Rows)
            {
                string sStreetName = row[U.StreetName_col].ToString();
                int iStreetNum = row[U.StreetNum_col].ToInt();
                if (iStreetNum > 0)
                {
                    sStreetName = ChangeRoadName(sStreetName, iStreetNum);
                    DataTable Value_tbl = new DataTable();
                    SelectAllLike(Value_tbl, U.ModernRoadValue_Table, new NameValuePair(U.ModernRoadValueValue_col, sStreetName));
                    if (Value_tbl.Rows.Count != 0)
                    {
                        UpdateGrandListBuildingRoadID(row[U.GrandListID_col].ToInt(),
                            Value_tbl.Rows[0][U.ModernRoadValueID_col].ToInt());
                    }
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        public static void UpdateGrandListBuildingRoadID(int iGrandListID,
                                          int iBuildingRoadID)
        {
            UpdateWithParms(U.GrandList_Table, new NameValuePair(U.GrandListID_col, iGrandListID),
                                                      new NameValuePair(U.BuildingRoadValueID_col, iBuildingRoadID));
//            string UpdateString = "Update " + U.GrandList_Table + " Set " + ColumnEquals(U.BuildingRoadValueID_col) +
//                                 " where " + ColumnEquals(U.GrandListID_col);
//            SqlCommand cmd = new SqlCommand(UpdateString, sqlConnection);
//            cmd.Parameters.Add(new SqlParameter("@" + U.GrandListID_col, iGrandListID));
//            cmd.Parameters.Add(new SqlParameter("@" + U.BuildingRoadValueID_col, iBuildingRoadID));
//            ExecuteUpdateStatement(cmd);
//            return true;
        }
        //****************************************************************************************************************************
        public static string ChangeRoadName(string sGrandListRoadName,
                                      int iStreetNum)
        {
            string sStreetNum = "";// iStreetNum.ToString() + " ";
            sGrandListRoadName = sGrandListRoadName.TrimString();
            if (sGrandListRoadName.Length == 0)
                return "";
            if (sGrandListRoadName == "VT RTE 100 N")
                return sStreetNum + "Route 100 North";
            else if (sGrandListRoadName == "VT RTE 100 S" ||
                     sGrandListRoadName == "VT RTE 100 SOUTH")
                return sStreetNum + "Route 100 South";
            if (sGrandListRoadName == "VT RTE 30")
            {
                if (iStreetNum < 3458)
                    return sStreetNum + "Route 30 South";
                else if (iStreetNum > 3924)
                    return sStreetNum + "Route 30 North";
                else
                    return sStreetNum + "Main Street - Village";
            }
            if (sGrandListRoadName.Substring(0) == "POTTER RD")
            {
                return sStreetNum + "Potter Road";
            }
            sGrandListRoadName = sGrandListRoadName.Replace(" RD", " Road");
            sGrandListRoadName = sGrandListRoadName.Replace("MTN", "Mountain");
            sGrandListRoadName = sGrandListRoadName.Replace("OLDE", "Old");
            sGrandListRoadName = sGrandListRoadName.Replace(" LN", " Lane");
            sGrandListRoadName = sGrandListRoadName.Replace("HALL'S", "Halls");
            sGrandListRoadName = sGrandListRoadName.Replace("CAR0L", "Carol");
            sGrandListRoadName = sGrandListRoadName.Replace("OFF ", "");
            sGrandListRoadName = sGrandListRoadName.Replace(" DR", " Drive");
            sGrandListRoadName = sGrandListRoadName.Replace(" RTE", " Route");
            sGrandListRoadName = sGrandListRoadName.Replace("RUN", "Run");
            sGrandListRoadName = sGrandListRoadName.Replace("'", "");
            return (sStreetNum + CapitalizeString(sGrandListRoadName));
        }
        //****************************************************************************************************************************
        private static string CapitalizeString(string sString)
        {
            int iStrLen = sString.Length;
            if (iStrLen == 0)
                return "";
            StringBuilder sb = new StringBuilder(sString.ToLower());
            sb[0] = (Char.ToUpper(sb[0]));
            for (int i = 0; i < iStrLen - 1; ++i)
            {
                if (sb[i] == ' ')
                {
                    int iNextChar = i + 1;
                    sb[iNextChar] = (Char.ToUpper(sb[iNextChar]));
                }
            }
            return sb.ToString();
        }
        //****************************************************************************************************************************
        private static string LowerCaseNameString(string sName)
        {
            if (sName.Length == 0)
                return sName;
            sName = sName.ToLower();
            sName = CharToUpper(sName, 0);
            sName = ChangeFirstCharInNameToUpper(sName, ';');
            sName = ChangeFirstCharInNameToUpper(sName, '-');
            sName = ChangeFirstCharInNameToUpper(sName, ' ');
            sName = sName.Replace(';', ',');
            return sName;
        }
        //****************************************************************************************************************************
        private static string CharToUpper(string sName, int iLocation)
        {
            if (sName.Length > iLocation + 2)
            {
                if (sName.Substring(iLocation, 3) == "llc")
                    return sName.Replace("llc", "LLC");
                if (sName.Substring(iLocation, 3) == "iii")
                    return sName.Replace("iii", "III");
                if (sName.Substring(iLocation, 3) == "c/o")
                    return sName;
                if (sName.Substring(iLocation, 3) == "and")
                    return sName;
            }
            if (sName.Length > iLocation + 1)
            {
                if (sName.Substring(iLocation, 2) == "ii")
                    return sName.Replace("ii", "II");
                if (sName.Substring(iLocation, 2) == "iv")
                    return sName.Replace("iv", "IV");
                if (sName.Substring(iLocation, 2) == "of")
                    return sName;
            }
            char c = char.ToUpper(sName[iLocation]);
            sName = sName.Remove(iLocation, 1);
            sName = sName.Insert(iLocation, c.ToString());
            return sName;
        }
        //****************************************************************************************************************************
        private static string ChangeFirstCharInNameToUpper(string sName, char cCharToLookFor)
        {
            int iLength = sName.Length;
            for (int i = 0; i < iLength - 1; i++)
            {
                if (sName[i] == cCharToLookFor)
                {
                    sName = CharToUpper(sName, i + 1);
                }
            }
            return sName;
        }
        //****************************************************************************************************************************
        public static DataTable GetModernRoads()
        {
            DataTable tbl = new DataTable();
            SelectAll(U.ModernRoadValue_Table, OrderBy(U.ModernRoadValueOrder_col, U.ModernRoadValueValue_col), tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetGrandListPropertys()
        {
            DataTable tbl = DefineGrandListTable();
            SelectAll(U.GrandList_Table, tbl);
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetGrandListProperties(int iStreetNum,
                           string sStreetName)
        {
            string additionalWhere = AdditionalWhereNotEqual(new NameValuePair(U.StreetNum_col, iStreetNum));
            DataTable tbl = DefineGrandListTable();
            SelectAllLike(tbl, U.GrandList_Table, additionalWhere + OrderBy(U.StreetName_col, U.StreetNum_col),
                                                          new NameValuePair(U.StreetName_col, sStreetName));
            return tbl;
        }
        //****************************************************************************************************************************
        public static int GetModernRoadIDFromValue(string sModernRoadValue)
        {
            DataTable tbl = new DataTable(U.ModernRoadValue_Table);
            SelectAll(U.ModernRoadValue_Table, tbl, new NameValuePair(U.ModernRoadValueValue_col, sModernRoadValue));
            if (tbl.Rows.Count == 0)     // does not exist
                return 0;
            else
            {
                DataRow row = tbl.Rows[0];
                return row[U.ModernRoadValueID_col].ToInt();
            }
        }
        //****************************************************************************************************************************
        public static DataTable GetGrandListPropertiesSortByName(string sName,
                                                          string sStreetName)
        {
            DataTable tbl = new DataTable(U.GrandList_Table);
            SelectAllLike(tbl, U.GrandList_Table, OrderBy(U.Name1_col, U.StreetName_col),
                                                          new NameValuePair(U.Name1_col, sName));
            return tbl;
        }
        //****************************************************************************************************************************
    }
}
