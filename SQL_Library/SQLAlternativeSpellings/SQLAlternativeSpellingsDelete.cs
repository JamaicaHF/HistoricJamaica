using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    public partial class SQL
    {        //****************************************************************************************************************************
        public static void DeleteAlternativeSpelling(string sTable,
                                              string sName1,
                                              string sName2)
        {
            DeleteWithParms(sTable, new NameValuePair(U.NameSpelling1_Col, sName1), new NameValuePair(U.NameSpelling2_Col, sName2));
        }
        //****************************************************************************************************************************
    }
}
