using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace SQL_Library
{
    //****************************************************************************************************************************
    public static partial class SQL
    {
        //****************************************************************************************************************************
        public static bool IntegrateVitalRecord(DataRow VitalRecord_row,
                                         DataRow SpouseVitalRecord_row,
                                         DataTable Person_tbl,
                                         DataTable Marriage_tbl,
                                         bool getFromGrid)
        {
            SqlTransaction txn = sqlConnection.BeginTransaction();
            try
            {
                bool bSuccess = true;
                EVitalRecordType eVitalRecordType = (EVitalRecordType)VitalRecord_row[U.VitalRecordType_col].ToInt();
                int iPersonID = SavePersonRecordsWithFatherMotherIDs(txn, Person_tbl);
                if (iPersonID == 0)
                    bSuccess = false;
                else
                {
                    VitalRecord_row[U.PersonID_col] = iPersonID;
                    CheckVitalRecordFatherMotherIDs(txn, VitalRecord_row, Person_tbl, Marriage_tbl, U.PersonType, U.PersonFatherType);
                    if (eVitalRecordType.MarriageRecord())
                    {
                        int iSpouseID = CheckVitalRecordFatherMotherIDs(txn, SpouseVitalRecord_row, Person_tbl, Marriage_tbl, U.SpouseType, U.SpouseFatherType);
                        DataRow Marriage_row = GetMarriageRow(Marriage_tbl, iPersonID, iSpouseID);
                        if (Marriage_row == null)
                        {
                            string sDateMarried = U.BuildDate(VitalRecord_row[U.DateYear_col].ToInt(),
                                                              VitalRecord_row[U.DateMonth_col].ToInt(),
                                                              VitalRecord_row[U.DateDay_col].ToInt());
                            AddMarriageToDataTable(Marriage_tbl, iPersonID, iSpouseID, sDateMarried, "N");
                            SpouseVitalRecord_row[U.PersonID_col] = iSpouseID;
                        }
                    }
                    IntegratePersonUpdate(txn, Person_tbl);
                    if (!SaveMarriages(txn, Marriage_tbl))
                        bSuccess = false;
                }
                if (bSuccess && (!getFromGrid || UserDoesNotWantToAbort()))
                {
                    txn.Commit();
                    return true;
                }
                else
                {
                    txn.Rollback();
                    return false;
                }
            }
            catch (HistoricJamaicaException Exception)
            {
                txn.Rollback();
                throw Exception;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw ex;
            }
        }
        //****************************************************************************************************************************
        private static bool UserDoesNotWantToAbort()
        {
            return MessageBox.Show("Was Integrate Successful?", "", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
        //****************************************************************************************************************************
    }
}
