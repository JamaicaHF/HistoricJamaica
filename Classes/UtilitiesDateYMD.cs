using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQL_Library;
using System.Windows.Forms;

namespace Utilities
{
    public static partial class UU       // Utilities
    {
        private static string originalDate;
        //****************************************************************************************************************************
        public static string ConvertToDateYMD(string inDate)
        {
            if (inDate.Length == 0)
                return inDate;
            originalDate = inDate;
            inDate = inDate.Replace("abt ", "");
            inDate = inDate.Replace("-", "/");
            if (DateContainsAlphaChars(inDate.ToUpper()))
            {
                inDate = DateWithAlphaMonth(inDate);
            }
            if (inDate.IndexOf('/') > 0)
            {
                return DateWithSlashes(inDate);
            }
            if (inDate.IndexOf(' ') < 0)
            {
                int year = inDate.ToIntNoError();
                if (ValidYear(year))
                {
                    return year.ToString();
                }
                else
                {
                    MessageBox.Show("Invalid Year: " + originalDate);
                    return "";
                }
            }
            MessageBox.Show("Invalid Date: " + originalDate);
            return "";
        }
        //****************************************************************************************************************************
        private static bool DateContainsAlphaChars(string inDate)
        {
            string inString = inDate.ToUpper();
            foreach (char ch in inString)
            {
                int hexChar = Convert.ToInt32(ch);
                if (hexChar >= Convert.ToInt32('A') && hexChar <= Convert.ToInt32('Z'))
                {
                    return true;
                }
            }
            return false;
        }
        //****************************************************************************************************************************
        private static string DateWithAlphaMonth(string inDate)
        {
            string stringFound;
            int month = StringMonth(inDate, out stringFound);
            if (month > 0)
            {
                string outdate = inDate.ToLower().Replace(stringFound, month.ToString());
                outdate = outdate.Replace(", ", "/");
                outdate = outdate.Replace(",", "/");
                outdate = outdate.Replace(" ", "/");
                return outdate;
            }
            return "";
        }
        //****************************************************************************************************************************
        private static int StringMonth(string inString,
                                   out string stringFound)
        {
            if (IsMonthString(inString, "january", out stringFound) ||
                IsMonthString(inString, "jan", out stringFound))
                return 1;
            else if (IsMonthString(inString, "february", out stringFound) ||
                     IsMonthString(inString, "feb", out stringFound))
                return 2;
            else if (IsMonthString(inString, "march", out stringFound) ||
                     IsMonthString(inString, "mar", out stringFound))
                return 3;
            else if (IsMonthString(inString, "april", out stringFound) ||
                     IsMonthString(inString, "apr", out stringFound))
                return 4;
            else if (IsMonthString(inString, "may", out stringFound))
                return 5;
            else if (IsMonthString(inString, "june", out stringFound) ||
                     IsMonthString(inString, "jun", out stringFound))
                return 6;
            else if (IsMonthString(inString, "july", out stringFound) ||
                     IsMonthString(inString, "jul", out stringFound))
                return 7;
            else if (IsMonthString(inString, "august", out stringFound) ||
                     IsMonthString(inString, "aug", out stringFound))
                return 8;
            else if (IsMonthString(inString, "september", out stringFound) ||
                     IsMonthString(inString, "sept", out stringFound) ||
                     IsMonthString(inString, "sep", out stringFound))
                return 9;
            else if (IsMonthString(inString, "october", out stringFound) ||
                     IsMonthString(inString, "oct", out stringFound))
                return 10;
            else if (IsMonthString(inString, "november", out stringFound) ||
                     IsMonthString(inString, "nov", out stringFound))
                return 11;
            else if (IsMonthString(inString, "december", out stringFound) ||
                     IsMonthString(inString, "dec", out stringFound))
                return 12;
            else
            {
                stringFound = "";
                return 0;
            }
        }
        //****************************************************************************************************************************
        private static bool IsMonthString(string inString,
                                          string strintToSearchFor,
                                      out string stringFound)
        {
            stringFound = strintToSearchFor;
            return !(inString.ToLower().IndexOf(strintToSearchFor) < 0);
        }
        //****************************************************************************************************************************
        public static string DateWithSlashes(string inDate)
        {
            int firstSlash = inDate.IndexOf('/');
            int secondSlash = inDate.IndexOf('/', firstSlash + 1);
            int thirdNum = 0;
            if (secondSlash > firstSlash)
            {
                int len = inDate.Length - (secondSlash + 1);
                thirdNum = inDate.Substring(secondSlash + 1, len).ToIntNoError();
            }
            else
            {
                secondSlash = inDate.Length;
            }
            int firstNum = inDate.Substring(0, firstSlash).ToIntNoError();
            int secondLen = secondSlash - (firstSlash + 1);
            int secondNum = inDate.Substring(firstSlash + 1, secondLen).ToIntNoError();
            if (ValidYear(thirdNum))
            {
                return ReturnDate(thirdNum, firstNum, secondNum);
            }
            else if (ValidYear(firstNum))
            {
                return ReturnDate(firstNum, secondNum, thirdNum);
            }
            else if (ValidYear(secondNum))
            {
                return ReturnDate(secondNum, firstNum, thirdNum);
            }
            MessageBox.Show("Date Contains An Invalid Year: " + originalDate);
            return "";
        }
        //****************************************************************************************************************************
        private static string ReturnDate(int year,
                                         int month,
                                         int day)
        {
            string returnString = year.ToString();
            if (month == 0)
            {
                return returnString;
            }
            else
            if (ValidMonth(month))
            {
                returnString += '/' + month.ToString("00");
                if (day == 0)
                {
                    return returnString;
                }
                else if (ValidDay(year, month, day))
                {
                    returnString += '/' + day.ToString("00");
                    return returnString;
                }
            }
            MessageBox.Show("Invalid Date: " + originalDate);
            return "";
        }
        //****************************************************************************************************************************
        private static bool ValidDay(int year,
                                     int month,
                                     int day)
        {
            if (day < 1)
                return false;
            int numDaysInMonth = U.NumDaysInMonth(year, month);
            return (day <= numDaysInMonth);
        }
        //****************************************************************************************************************************
        private static bool ValidYear(int year)
        {
            if (year < 1000)
            {
                return false;
            }
            if (year > DateTime.Now.Year)
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        private static bool ValidMonth(int month)
        {
            if (month < 0 || month > 12)
            {
                return false;
            }
            return true;
        }
    }
}
