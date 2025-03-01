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
        public static DataTable GetAllPicturedPersons()
        {
            DataTable PicturedPerson_tbl = new DataTable();
            SelectAll(U.PicturedPerson_Table, OrderBy(U.PersonID_col), PicturedPerson_tbl);
            return PicturedPerson_tbl;
        }
        //****************************************************************************************************************************
        public static DataTable GetAllPicturedElements(string table,
                                                       string picturedOrderCol)
        {
            DataTable Pictured_tbl = new DataTable();
            SelectAll(table, OrderBy(U.PhotoID_col, picturedOrderCol), Pictured_tbl);
            return Pictured_tbl;
        }
        //****************************************************************************************************************************
    }
}
