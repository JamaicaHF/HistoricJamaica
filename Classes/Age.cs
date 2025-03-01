using System;
using System.IO;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SQL_Library;
using Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HistoricJamaica
{
    public class CAge
    {
        public int numYears = 0;
        public int numMonths = 0;
        public int numDays = 0;
        public CAge(string ageInOriginal)
        {
            if (string.IsNullOrEmpty(ageInOriginal))
            {
                return;
            }
            string ageIn = ageInOriginal;
            numYears = GetAgeValue(ref ageIn, 'y');
            numMonths = GetAgeValue(ref ageIn, 'm');
            numDays = GetAgeValue(ref ageIn, 'd');
        }
        private int GetAgeValue(ref string ageIn, char valueChar)
        {
            int indexOfValueChar = ageIn.ToLower().IndexOf(valueChar);
            if (indexOfValueChar < 0)
            {
                return 0;
            }
            while (indexOfValueChar > 0 && ageIn[indexOfValueChar - 1] == ' ')
            {
                indexOfValueChar--;
            }
            do
            {
                ageIn = ageIn.Remove(indexOfValueChar, 1);
            }
            while (indexOfValueChar < ageIn.Length && ageIn[indexOfValueChar] != ' ');
            indexOfValueChar--;
            int numDigits = 0;
            while (indexOfValueChar >= 0 && ageIn[indexOfValueChar].IsNumeric())
            {
                indexOfValueChar--;
                numDigits++;
            }
            if (numDigits == 0)
            {
                return 0;
            }
            indexOfValueChar++;
            string num = ageIn.Substring(indexOfValueChar, numDigits);
            ageIn = ageIn.Remove(indexOfValueChar, numDigits).Trim();
            return num.ToInt();
        }
    }
}
