using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace SQL_Library
{
    public class Census
    {
        public enum CensusYears
        {
            None = 0,
            Year1790 = 1,
            Year1800 = 2,
            Year1810 = 4,
            Year1820 = 8,
            Year1830 = 16,
            Year1840 = 32,
            Year1850 = 64,
            Year1860 = 128,
            Year1870 = 256,
            Year1880 = 512,
            Year1890 = 1024,
            Year1900 = 2048,
            Year1910 = 4096,
            Year1920 = 8192,
            Year1930 = 16384,
            Year1940 = 32768,
            Year1935 = 65536,
            Year1856 = 131072,
            Year1869 = 262144,
            Year1884 = 524288
        }
        protected CensusYears censusYears;
        //****************************************************************************************************************************
        public Census()
        {
        }
        public Census(Int64 yearsInCensus)
        {
            this.censusYears = (CensusYears) yearsInCensus;
        }
        //****************************************************************************************************************************
        public virtual void Clear()
        {
            censusYears = 0;
        }
        //****************************************************************************************************************************
        public virtual void LoadCensusData(Int64 yearsInCensus)
        {
            censusYears = (CensusYears)yearsInCensus;
        }
        //****************************************************************************************************************************
        public Int64 GetCencusYears()
        {
            return (Int64) censusYears;
        }
        //****************************************************************************************************************************
        public string OccupantString(int iPersonID, bool mapsOption=false)
        {
            int iBuilding1856PersonID = 0;
            int iBuilding1869PersonID = 0;
            int iBuilding1884PersonID = 0;
            if (CensusContainsYear(censusYears, 1856))
            {
                iBuilding1856PersonID = iPersonID;
            }
            if (CensusContainsYear(censusYears, 1869))
            {
                iBuilding1869PersonID = iPersonID;
            }
            if (CensusContainsYear(censusYears, 1884))
            {
                iBuilding1884PersonID = iPersonID;
            }
            return U.OccupantPrintString(iBuilding1856PersonID, iBuilding1869PersonID, iBuilding1884PersonID, mapsOption);
        }
        //****************************************************************************************************************************
        public string StringOfCensusYears()
        {
            string censusString = "";
            if (censusYears != 0)
            {
                for (int year = 1790; year <= 1930; year += 10)
                {
                    CensusString(ref censusString, year);
                }
                CensusString(ref censusString, 1935);
                CensusString(ref censusString, 1940);
            }
            return censusString;
        }
        //****************************************************************************************************************************
        private void CensusString(ref string censusString,
                                  int year)
        {
            if (CensusContainsYear(censusYears, year))
            {
                if (censusString.Length == 0)
                {
                    censusString += "Census ";
                }
                else
                {
                    censusString += ",";
                }
                censusString += year.ToString();
            }
        }
        //****************************************************************************************************************************
        protected bool CensusContainsYear(CensusYears censusYears,
                                          int year)
        {
            switch (year)
            {
                case 1790:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1790);
                case 1800:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1800);
                case 1810:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1810);
                case 1820:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1820);
                case 1830:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1830);
                case 1840:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1840);
                case 1850:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1850);
                case 1860:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1860);
                case 1870:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1870);
                case 1880:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1880);
                case 1890:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1890);
                case 1900:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1900);
                case 1910:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1910);
                case 1920:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1920);
                case 1930:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1930);
                case 1935:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1935);
                case 1940:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1940);
                case 1856:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1856);
                case 1869:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1869);
                case 1884:
                    return FlagsHelper.IsSet(censusYears, CensusYears.Year1884);
                default: return false;
            }
        }
        //****************************************************************************************************************************
        public void AddYearsToCensusFor1800sPersons(int iPersonID,
                                                     int i1856PersonID,
                                                     int i1869PersonID,
                                                     int i1884PersonID)
        {
            censusYears = 0;
            if (i1856PersonID == iPersonID)
            {
                AddYearToCensusYears(ref censusYears, 1856);
            }
            if (i1869PersonID == iPersonID)
            {
                AddYearToCensusYears(ref censusYears, 1869);
            }
            if (i1884PersonID == iPersonID)
            {
                AddYearToCensusYears(ref censusYears, 1884);
            }
        }
        //****************************************************************************************************************************
        public void MergeCensusYears(Int64 yearsInCensus)
        {
            CensusYears mergeCensusYears = (CensusYears) yearsInCensus;
            for (int year = 1790; year <= 1940; year += 10)
            {
                MergeYear(mergeCensusYears, year);
            }
            MergeYear(mergeCensusYears, 1856);
            MergeYear(mergeCensusYears, 1869);
            MergeYear(mergeCensusYears, 1884);
            MergeYear(mergeCensusYears, 1935);
        }
        //****************************************************************************************************************************
        protected void MergeYear(CensusYears mergeCensusYears,
                                 int year)
        {
            if (CensusContainsYear(mergeCensusYears, year))
            {
                AddYearToCensusYears(ref censusYears, year);
            }
        }
        //****************************************************************************************************************************
        protected void AddYearToCensusYears(ref CensusYears censusYears,
                                            int year)
        {
            switch (year)
            {
                case 1790:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1790);
                    break;
                case 1800:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1800);
                    break;
                case 1810:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1810);
                    break;
                case 1820:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1820);
                    break;
                case 1830:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1830);
                    break;
                case 1840:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1840);
                    break;
                case 1850:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1850);
                    break;
                case 1860:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1860);
                    break;
                case 1870:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1870);
                    break;
                case 1880:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1880);
                    break;
                case 1890:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1890);
                    break;
                case 1900:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1900);
                    break;
                case 1910:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1910);
                    break;
                case 1920:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1920);
                    break;
                case 1930:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1930);
                    break;
                case 1935:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1935);
                    break;
                case 1940:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1940);
                    break;
                case 1856:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1856);
                    break;
                case 1869:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1869);
                    break;
                case 1884:
                    FlagsHelper.Set(ref censusYears, CensusYears.Year1884);
                    break;
                default: break;
            }
        }
        //****************************************************************************************************************************
        private static class FlagsHelper
        {// http://stackoverflow.com/questions/3261451/using-a-bitmask-in-c-sharp
            public static bool IsSet<T>(T flags, T flag) where T : struct
            {
                int flagsValue = (int)(object)flags;
                int flagValue = (int)(object)flag;

                return (flagsValue & flagValue) != 0;
            }

            public static void Set<T>(ref T flags, T flag) where T : struct
            {
                int flagsValue = (int)(object)flags;
                int flagValue = (int)(object)flag;

                flags = (T)(object)(flagsValue | flagValue);
            }

            public static void Unset<T>(ref T flags, T flag) where T : struct
            {
                int flagsValue = (int)(object)flags;
                int flagValue = (int)(object)flag;

                flags = (T)(object)(flagsValue & (~flagValue));
            }
        }
        //****************************************************************************************************************************
    }
}
