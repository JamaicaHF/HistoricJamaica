using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class CPersonName
    {
        public string firstName = "";
        public string middleName = "";
        public string lastName = "";
        public string prefix = "";
        public string suffix = "";
        public string name = "";
        public CPersonName(string nameIn)
        {
            if (String.IsNullOrEmpty(nameIn))
            {
                return;
            }
            name = nameIn.Replace(".", "");
            GetSuffix();
            int lastNameFirstIndex = name.IndexOf(',');
            if (lastNameFirstIndex > 0)
            {
                lastName = name.Substring(0, lastNameFirstIndex);
                name = name.Remove(0, lastNameFirstIndex + 1).Trim();
            }
            GetPrefix();
            firstName = GetName();
            string secondName = GetName();
            string thirdName = GetName();
            string fourthName = GetName();
            string fifthName = GetName();
            SetLastname(ref fifthName);
            SetLastname(ref fourthName);
            SetLastname(ref thirdName);
            SetLastname(ref secondName);
            middleName = secondName + thirdName + fourthName + fifthName;
            middleName = middleName.Trim();
        }
        private void SetLastname(ref string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return;
            }
            if (String.IsNullOrEmpty(lastName))
            {
                lastName = str;
                str = "";
            }
            else
            {
                str = str.Trim();
                str += " ";  // Add space so when concatenated there will be a space between them 
            }
        }
        private string GetName()
        {
            if (String.IsNullOrEmpty(name))
            {
                return "";
            }
            name = name.TrimStart();
            int spaceIndex = name.IndexOf(' ');
            string returnStr = "";
            if (spaceIndex < 0)
            {
                returnStr = name.Trim();
                name = "";
            }
            else
            {
                returnStr = name.Substring(0, spaceIndex);
                name = name.Remove(0, spaceIndex);
            }
            return returnStr.Trim();
        }
        private void GetSuffix()
        {
            DataTable suffix_tbl = UU.GetSuffixList();
            suffix = "";
            foreach (DataRow suffix_row in suffix_tbl.Rows)
            {
                string newSuffix = ContainsString(suffix_row["Suffix"].ToString());
                if (!String.IsNullOrEmpty(newSuffix))
                {
                    suffix = newSuffix;
                }
            }
        }
        private void GetPrefix()
        {
            DataTable prefix_tbl = UU.GetPrefixList();
            prefix = "";
            foreach (DataRow prefix_row in prefix_tbl.Rows)
            {
                string newPrefix = ContainsString(prefix_row["prefix"].ToString());
                if (!String.IsNullOrEmpty(newPrefix))
                {
                    prefix = newPrefix;
                }
            }
        }
        private string ContainsString(string str)
        {
            int indexOf = name.ToLower().IndexOf(str.ToLower());
            if (indexOf >= 0)
            {
                string strWithCaps = name.Substring(indexOf, str.Length);
                if (!String.IsNullOrEmpty(strWithCaps))
                {
                    char firstChar = strWithCaps[0];
                    if (Char.IsUpper(firstChar))
                    {
                        int indexOfStr = name.IndexOf(strWithCaps);
                        if (indexOfStr > 0)
                        {
                            indexOfStr--;
                            if (name[indexOfStr] == ' ')
                            {
                                name = name.Remove(indexOfStr, 1);
                                indexOfStr--;
                            }
                            if (name[indexOfStr] == ',')
                            {
                                name = name.Remove(indexOfStr, 1);
                            }
                        }
                        name = name.Replace(strWithCaps, "").Trim();
                        name = name.Replace("  ", " ");
                        if (str.Length > 3)
                        {
                            str = str.Remove(3);
                        }
                    }
                }
                return str;
            }
            return "";
        }
    }
}
