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
    public class CImportNicknames : CImport
    {
        private string m_sInputRecord;
        //****************************************************************************************************************************
        public CImportNicknames(CSql Sql, string sDataDirectory)
            : base(Sql, sDataDirectory)
        {
            try
            {
                string sFilter = "CSV Delimited Files (csv)|*.csv";
                if (!OpenInputFile(sFilter))
                {
                    return;
                }
                DataTable alternativeSpellins_tbl = SQL.DefineAlternativeSpellingsTable(U.AlternativeSpellingsFirstName_Table);
                m_sInputRecord = ReadRecord();
                while (m_sInputRecord != null)
                {
                    string[] sInputFields = m_sInputRecord.Split(',');
                    string givenName = Field(sInputFields, 1).ToString();
                    StringBuilder sbGivenName = new StringBuilder(givenName.ToLower());
                    sbGivenName[0] = (Char.ToUpper(sbGivenName[0]));
                    givenName = sbGivenName.ToString();
                    string nickname = Field(sInputFields, 0).ToString();
                    StringBuilder sbNickname = new StringBuilder(nickname.ToLower());
                    sbNickname[0] = (Char.ToUpper(sbNickname[0]));
                    nickname = sbNickname.ToString();
                    DataRow alternativeSpellins_row = alternativeSpellins_tbl.NewRow();
                    alternativeSpellins_row[U.NameSpelling1_Col] = givenName;
                    alternativeSpellins_row[U.NameSpelling2_Col] = nickname;
                    alternativeSpellins_tbl.Rows.Add(alternativeSpellins_row);
                    m_sInputRecord = ReadRecord();
                }
                CloseInputFile();
                SQL.InsertAlternativeSpelling(U.AlternativeSpellingsFirstName_Table, alternativeSpellins_tbl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
