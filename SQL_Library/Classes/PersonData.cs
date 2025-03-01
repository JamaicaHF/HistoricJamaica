using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;

namespace SQL_Library
{
    public class PersonData
    {
        private DataSet personDS;
        public PersonData()
        {
            personDS = SQL.DefinePerson();
        }
    }
}
