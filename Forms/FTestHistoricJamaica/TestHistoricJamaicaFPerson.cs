using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    public class TestHistoricJamaicaFPerson
    {
        public TestHistoricJamaicaFPerson(CSql m_SQL,
                                          ref ArrayList personRecordsAdded)
        {
            FPerson Person = new FPerson(m_SQL, false);
        }
    }
}
