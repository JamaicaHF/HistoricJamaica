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
        public static DataTable DefinePicturedPerson()
        {
            DataTable tbl = new DataTable(U.PicturedPerson_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PicturedPersonNumber_col, typeof(int));
            tbl.Columns.Add(U.PhotoID_col, typeof(int));
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            return tbl;
        }
        //****************************************************************************************************************************
        public static DataTable DefinePicturedPersonForPhoto()
        {
            DataTable tbl = new DataTable(U.PicturedPerson_Table);
            DataColumn Column = new DataColumn();
            tbl.Columns.Add(U.PicturedPersonNumber_col, typeof(int));
            tbl.Columns.Add(U.PhotoID_col, typeof(int));
            tbl.Columns.Add(U.PersonID_col, typeof(int));
            tbl.Columns.Add(U.FirstName_col, typeof(string));
            tbl.Columns.Add(U.MiddleName_col, typeof(string));
            tbl.Columns.Add(U.LastName_col, typeof(string));
            tbl.Columns.Add(U.Suffix_col, typeof(string));
            tbl.Columns.Add(U.Prefix_col, typeof(string));
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns[U.PicturedPersonNumber_col] };
            return tbl;
        }
        //****************************************************************************************************************************
    }
}
