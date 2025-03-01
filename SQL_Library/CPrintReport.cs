using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace SQL_Library
{
    public class CPrintReport
    {
        private StreamWriter m_StreamWriter;
        private const string TabChar = "\t";
        private string m_sFileNameWithPath;
        public CPrintReport()
        {
        }
        //****************************************************************************************************************************
        public static string GetFileNameFromPath(string sFileNameWithPath)
        {
            char[] c = new char[1];
            c[0] = '\\';
            int iIndexOfLastBackslash = sFileNameWithPath.LastIndexOfAny(c);
            return sFileNameWithPath.Substring(iIndexOfLastBackslash + 1);
        }
        //****************************************************************************************************************************
        public bool OpenOutputFile()
        {
            m_sFileNameWithPath = "c:\\JamaicaHF\\Duplicates.txt";
            if (m_sFileNameWithPath.Length == 0)
            {
                return false;
            }
            string sFileName = GetFileNameFromPath(m_sFileNameWithPath);
            try
            {
                m_StreamWriter = new StreamWriter(m_sFileNameWithPath);
            }
            catch (Exception)
            {
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
        public void WritePrintFile(string Name)
        {
            m_StreamWriter.WriteLine(Name);
        }
        //****************************************************************************************************************************
        public void PrintReport(ArrayList myList)
        {
            if (OpenOutputFile())
            {
                int iNumDuplicates = myList.Count;
                for (int i = 0; i < iNumDuplicates;++i )
                {
                    WritePrintFile(myList[i].ToString());
                }
                CloseOutputFile();
            }
        }
    }
}
