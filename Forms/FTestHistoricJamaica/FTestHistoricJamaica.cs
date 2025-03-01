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
    public partial class FTestHistoricJamaica : Form
    {
        CSql m_SQL;
        public FTestHistoricJamaica(CSql SQL)
        {
            m_SQL = SQL;
            InitializeComponent();
        }
        //****************************************************************************************************************************
        public void RunTests_button_Click(object sender, System.EventArgs e)
        {
            ArrayList vitalRecordsAdded = new ArrayList();
            ArrayList personRecordsAdded = new ArrayList();
            try
            {
                if (Person_Checkbox.Checked)
                {
                    TestHistoricJamaicaFPerson testHistoricJamaicaFPerson = new TestHistoricJamaicaFPerson(m_SQL, ref personRecordsAdded);
                }
                if (VitalRecord_CheckBox.Checked)
                {
                    TestHistoricJamaicaFVitalRecord testHistoricJamaicaFVitalRecord =
                                                new TestHistoricJamaicaFVitalRecord(m_SQL, vitalRecordsAdded, personRecordsAdded);
                }
                MessageBox.Show("All tests ran successfully");
            }
            catch (HistoricJamaicaException Exception)
            {
                HandleErrorCode(Exception.errorCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error: " + ex.Message);
            }
            TestVitalRecordsDelete(vitalRecordsAdded, personRecordsAdded);
        }
        //****************************************************************************************************************************
        private void TestVitalRecordsDelete(ArrayList vitalRecordsAdded,
                                            ArrayList personRecordsAdded)
        {
            foreach (int PersonID in personRecordsAdded)
            {
                SQL.DeletePersonFromDatabase(PersonID);
            }
            foreach (int vitalRecordID in vitalRecordsAdded)
            {
                SQL.DeleteVitalRecordFromDatabase(vitalRecordID, 0);
            }
        }
        //****************************************************************************************************************************
        private void HandleErrorCode(ErrorCodes errorCode)
        {
            switch (errorCode)
            {
                case ErrorCodes.eSaveUnsuccessful:
                    MessageBox.Show("Test Historic Jamaica Unsuccessfull");
                    break;
                default:
                    MessageBox.Show("Test Historic Jamaica Unsuccesful");
                    break;
            }
        }
        //****************************************************************************************************************************
    }
}
