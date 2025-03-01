using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SQL_Library;
using Utilities;

namespace HistoricJamaica
{
    class QRCodes
    {
        DataTable buildingTbl;
        ArrayList qrCodesWithoutBuilding = new ArrayList();
        public QRCodes()
        {
            buildingTbl = SQL.GetAllBuildings(false);
            AddBuildingToArray("M00", 0, 92, 'a');
            AddBuildingToArray("M01", 3458, 92, ' ');
            AddBuildingToArray("M02", 0, 92, ' ');
            AddBuildingToArray("M03", 3580, 92, ' ');
            AddBuildingToArray("M04", 0, 92, ' ');
            AddBuildingToArray("M05", 3611, 92, ' ');
            AddBuildingToArray("M06", 3606, 0, ' ');
            AddBuildingToArray("M07", 3632, 92, ' ');
            AddBuildingToArray("M08", 3654, 92, ' ');
            AddBuildingToArray("M09", 3686, 92, ' ');
            AddBuildingToArray("M10", 0, 92, ' ');
            AddBuildingToArray("M11", 3708, 92, ' ');
            AddBuildingToArray("M12", 3732, 92, ' ');
            AddBuildingToArray("M13", 3770, 92, ' ');
            AddBuildingToArray("M14", 3782, 92, ' ');
            AddBuildingToArray("M15", 3796, 92, ' ');
            AddBuildingToArray("M16", 3804, 92, ' ');
            AddBuildingToArray("M17", 3816, 92, 'a');
            AddBuildingToArray("M18", 3836, 92, ' ');
            AddBuildingToArray("M19", 3848, 92, ' ');
            AddBuildingToArray("M20", 3868, 92, ' ');
            AddBuildingToArray("M21", 3894, 92, ' ');
            AddBuildingToArray("M22", 3912, 92, ' ');
            AddBuildingToArray("M23", 3924, 92, ' ');
            AddBuildingToArray("M24", 3923, 92, ' ');
            AddBuildingToArray("M25", 3883, 92, ' ');
            AddBuildingToArray("M26", 0, 92, ' ');
            AddBuildingToArray("M27", 3863, 92, ' ');
            AddBuildingToArray("M28", 3849, 92, 'a');
            AddBuildingToArray("M29", 3819, 92, ' ');
            AddBuildingToArray("M30", 3791, 92, ' ');
            AddBuildingToArray("M31", 3779, 92, 'a');
            AddBuildingToArray("M32", 0, 92, ' ');
            AddBuildingToArray("M33", 3767, 92, ' ');
            AddBuildingToArray("M34", 3751, 92, 'a');
            AddBuildingToArray("M35", 3735, 92, ' ');
            AddBuildingToArray("M36", 3719, 92, ' ');
            AddBuildingToArray("D00", 0, 84, ' ');
            AddBuildingToArray("D01", 5, 84, ' ');
            AddBuildingToArray("D02", 17, 84, ' ');
            AddBuildingToArray("D03", 39, 84, ' ');
            AddBuildingToArray("D04", 47, 84, ' ');
            AddBuildingToArray("D05", 67, 84, ' ');
            AddBuildingToArray("D06", 72, 84, ' ');
            AddBuildingToArray("D07", 83, 84, ' ');
            AddBuildingToArray("D08", 101, 84, ' ');
            AddBuildingToArray("D09", 104, 84, ' ');
            AddBuildingToArray("D10", 0, 84, ' ');
            AddBuildingToArray("D11", 153, 84, 'a');
            AddBuildingToArray("D12", 154, 84, ' ');
            AddBuildingToArray("D13", 0, 84, ' ');
            AddBuildingToArray("D14", 192, 84, ' ');
            AddBuildingToArray("D15", 190, 0, ' ');
            AddBuildingToArray("D16", 160, 0, ' ');
            AddBuildingToArray("D17", 210, 84, ' ');
            AddBuildingToArray("D18", 0, 84, ' ');
            AddBuildingToArray("D19", 161, 84, ' ');
            AddBuildingToArray("D20", 205, 84, 'a');
            AddBuildingToArray("D21", 270, 84, ' ');
            AddBuildingToArray("D22", 347, 84, ' ');
            AddBuildingToArray("D23", 0, 84, ' ');
            AddBuildingToArray("D24", 447, 84, ' ');
            AddBuildingToArray("D25", 513, 84, ' ');
            AddBuildingToArray("D26", 536, 84, ' ');
            AddBuildingToArray("D27", 568, 84, ' ');
            AddBuildingToArray("D28", 573, 84, ' ');
            AddBuildingToArray("P00", 0, 1, ' ');
            AddBuildingToArray("P01", 5, 1, ' ');
            AddBuildingToArray("P02", 29, 1, ' ');
            AddBuildingToArray("P03", 39, 1, ' ');
            AddBuildingToArray("P04", 45, 1, ' ');
            AddBuildingToArray("P05", 55, 1, ' ');
            AddBuildingToArray("P06", 129, 1, ' ');
            AddBuildingToArray("P07", 145, 0, ' ');
            AddBuildingToArray("P08", 171, 0, ' ');
            AddBuildingToArray("P09", 191, 0, ' ');
            AddBuildingToArray("P10", 200, 1, ' ');
            AddBuildingToArray("P11", 92, 1, ' ');
            AddBuildingToArray("P12", 84, 1, ' ');
            AddBuildingToArray("P13", 60, 1, ' ');
            AddBuildingToArray("P14", 52, 1, ' ');
            AddBuildingToArray("F01", 23, 163, ' ');
            AddBuildingToArray("F02", 55, 163, ' ');
            AddBuildingToArray("F03", 0, 163, ' ');
            AddBuildingToArray("F04", 0, 163, ' ');
            AddBuildingToArray("F05", 70, 163, ' ');
            AddBuildingToArray("F06", 64, 163, ' ');
            AddBuildingToArray("F07", 30, 163, ' ');
            AddBuildingToArray("W00", 0, 83, ' ');
            AddBuildingToArray("W01", 317, 83, ' ');
            AddBuildingToArray("W02", 309, 83, ' ');
            AddBuildingToArray("W03", 289, 83, ' ');
            AddBuildingToArray("W04", 275, 83, ' ');
            AddBuildingToArray("W05", 257, 83, ' ');
            AddBuildingToArray("W06", 227, 83, ' ');
            AddBuildingToArray("W07", 213, 83, ' ');
            AddBuildingToArray("W08", 205, 83, ' ');
            AddBuildingToArray("W09", 199, 83, ' ');
            AddBuildingToArray("W10", 169, 0, ' ');
            AddBuildingToArray("W11", 110, 83, ' ');
            AddBuildingToArray("W12", 109, 0, ' ');
            AddBuildingToArray("W13", 0, 83, ' ');
            AddBuildingToArray("W14", 0, 83, ' ');
            AddBuildingToArray("W15", 0, 83, ' ');
            AddBuildingToArray("W16", 30, 0, ' ');
            AddBuildingToArray("W17", 9, 83, ' ');
            AddBuildingToArray("L01", 99, 232, ' ');
            AddBuildingToArray("L02", 5, 232, ' ');
            AddBuildingToArray("R00", 0, 235, ' ');
            AddBuildingToArray("R01", 3996, 0, ' ');
            AddBuildingToArray("R02", 4020, 235, ' ');
            AddBuildingToArray("R03", 4017, 235, ' ');
            AddBuildingToArray("C01", 69, 113, ' ');
            AddBuildingToArray("C02", 142, 113, ' ');
            AddBuildingToArray("O01", 28, 242, ' ');
            AddBuildingToArray("T01", 92, 0, ' ');
            AddBuildingToArray("T02", 80, 0, ' ');
            AddBuildingToArray("T03", 20, 224, ' ');
            AddBuildingToArray("T04", 32, 0, ' ');
            SQL.UpdateBuildingValues(buildingTbl, U.QRCode_col);
        }
        private void AddBuildingToArray(string qrCode, int StreetNum, int roadId, char additionalPage)
        {
            if (additionalPage != ' ')
            {
                qrCode += additionalPage.ToString();
            }
            if (StreetNum == 0 || roadId == 0)
            {
                qrCodesWithoutBuilding.Add(qrCode);
                return;
            }
            int grandListId = SQL.GetGrandListIDFromTaxMapIDFromRoadIdAndStreetnum(roadId, StreetNum);
            if (grandListId == 0)
            {
                qrCodesWithoutBuilding.Add(qrCode);
                return;
            }
            string selectStatement = U.BuildingGrandListID_col + "=" + grandListId;
            DataRow[] foundRows = buildingTbl.Select(selectStatement);
            if (foundRows.Length == 0)
            {
                qrCodesWithoutBuilding.Add(qrCode);
                return;
            }
            if (foundRows.Length > 1)
            {
                return;
            }
            foreach (DataRow buildingRow in foundRows)
            {
                string buildingQRCode = buildingRow[U.QRCode_col].ToString();
                if (qrCode != buildingQRCode)
                {
                    buildingRow[U.QRCode_col] = qrCode;
                }
            }
        }
    }
}
