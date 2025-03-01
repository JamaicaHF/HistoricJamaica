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
    public class TestHistoricJamaicaFVitalRecord
    {
        public TestHistoricJamaicaFVitalRecord(CSql m_SQL,
                                               ArrayList vitalRecordsAdded,
                                               ArrayList personRecordsThatWereAdded)
        {
            FVitalRecord vitalRecord = new FVitalRecord(EVitalRecordType.eBirthMale, m_SQL);
        }
    }
}
