using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using SQL_Library;

namespace HistoricJamaica
{
    //****************************************************************************************************************************
    public class CNemrcExtract
    {
        private enum NemrcColumns
        {
            ParcelId = 1,
            SubParcelId = 2,
            Name1 = 3,
            Name2 = 4,
            Address1 = 5,
            Address2 = 6,
            City = 7,
            State = 8,
            ZipCode = 9,
            LocationA = 10,
            LocationB = 11,
            LocationC = 12,
            StreetNum = 13,
            StreetName = 14,
            TaxMapID = 15,
            PropertyDescription = 16,
            Owner = 17,
            DateHomestead = 18,
            Span = 19,
            VacantLand = 20
        }

        public string parcelId;
        public string SubParcelId;
        public string Name1;
        public string Name2;
        public string Address1;
        public string Address2;
        public string City;
        public string State;
        public string ZipCode;
        public string LocationA;
        public string LocationB;
        public string LocationC;
        public int StreetNum;
        public string StreetName;
        public string Span;
        public string TaxMapID;
        public string PropertyDescription;
        public string Owner;
        public string DateHomestead;
        public char ActiveStatus;
        public char VacantLand;
        public string CurrentAddress;
        private EPPlus epPlus;
        private DataTable modernRoadValueTbl;
        //****************************************************************************************************************************
        public CNemrcExtract(EPPlus epPlus, DataTable modernRoadValueTbl, int rowIndex, char ActiveStatus, bool includeVacant=true)
        {
            this.epPlus = epPlus;
            this.ActiveStatus = ActiveStatus;
            this.modernRoadValueTbl = modernRoadValueTbl;
            parcelId = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.ParcelId).ToString();
            SubParcelId = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.SubParcelId).ToString();
            Name1 = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.Name1).ToString();
            Name2 = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.Name2).ToString();
            Address1 = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.Address1).ToString();
            Address2 = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.Address2).ToString();
            City = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.City).ToString();
            State = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.State).ToString();
            ZipCode = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.ZipCode).ToString();
            LocationA = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.LocationA).ToString();
            LocationB = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.LocationB).ToString();
            LocationC = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.LocationB).ToString();
            StreetNum = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.StreetNum).ToInt();
            StreetName = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.StreetName).ToString();
            TaxMapID = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.TaxMapID).ToString();
            PropertyDescription = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.PropertyDescription).ToString();
            Owner = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.Owner).ToString();
            DateHomestead = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.DateHomestead).ToString();
            Span = epPlus.GetCellValue(rowIndex, (int)NemrcColumns.Span).ToString();
            VacantLand = (includeVacant) ? epPlus.GetCellValue(rowIndex, (int)NemrcColumns.VacantLand).ToChar() : '0';
            if (ZipCode.Length == 4)
            {
                ZipCode = ZipCode.Insert(0, "0");
            }
            CurrentAddress = Address1.Trim() + ", " +
                             City.Trim() + ", " +
                             State.Trim();
        }
        //****************************************************************************************************************************
        public void SetTo2018(DataRow grandListRow, char currentStatus)
        {
            SetNewValueIfDifferent(grandListRow, U.Name1_col, Name1);
            SetNewValueIfDifferent(grandListRow, U.Name2_col, Name2);
            SetNewValueIfDifferent(grandListRow, U.ActiveStatus_col, currentStatus);
            grandListRow["Address"] = CurrentAddress;
        }
        //****************************************************************************************************************************
        public void UpdateExistingGrandListRecord(DataTable grandListHistoryTbl, DataRow grandListRow, 
                                                  int currentYear, ref int previousGrandListId)
        {
            if (grandListRow["GrandListId"].ToString() == "324-101-11492")
            { }
            //string modifiedStreetName = GetModifiedStreetName(StreetName, StreetNum);
            SetNewValueIfDifferent(grandListRow, U.Span_col, Span);
            SetNewValueIfDifferent(grandListRow, U.ActiveStatus_col, ActiveStatus);
            SetNewValueIfDifferent(grandListRow, U.VacantLand_col, VacantLand);
            SetNewValueIfDifferent(grandListRow, U.StreetName_col, StreetName);
            CheckForNewOwner(grandListRow, grandListHistoryTbl, currentYear, ref previousGrandListId);
            SetNewValueIfDifferent(grandListRow, U.Name1_col, Name1);
            SetNewValueIfDifferent(grandListRow, U.Name2_col, Name2);
            if (grandListRow[U.WhereOwnerLiveID_col].ToChar() != Owner[0])
            {
                grandListRow[U.WhereOwnerLiveID_col] = Owner[0];
            }
            if (StreetNum == 0 && grandListRow[U.StreetNum_col].ToInt() != 0)
            {
                StreetNum = grandListRow[U.StreetNum_col].ToInt(); // This Address is for a property with photos
            }
            else if (grandListRow[U.StreetNum_col].ToInt() != StreetNum)
            {
                grandListRow[U.StreetNum_col] = StreetNum;
            }
            CheckId(grandListRow);
        }
        //****************************************************************************************************************************
        private void CheckForNewOwner(DataRow grandListRow, DataTable grandListHistoryTbl, 
                                      int currentYear, ref int previousGrandListId)
        {
            //if (NewAddressOrInJamaica(grandListRow["Address"].ToString()))
            //{
                if (NamesDifferent(grandListRow, ref previousGrandListId))
                {
                    AddNameToHistory(grandListHistoryTbl, grandListRow[U.GrandListID_col].ToInt(), 
                                     grandListRow[U.Name1_col].ToString(), grandListRow[U.Name2_col].ToString(), currentYear);
                }
                grandListRow["Address"] = CurrentAddress;
            //}
        }
        //****************************************************************************************************************************
        private bool NewAddressOrInJamaica(string oldAddress)
        {
            if (oldAddress.ToLower() == CurrentAddress.ToLower())
            {
                if (CurrentAddress.ToLower().Contains("jamaica"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        private void CheckId(DataRow grandListRow)
        {
            int oldId = grandListRow[U.BuildingRoadValueID_col].ToInt();
            int newId = GetModernRoadValue(grandListRow[U.StreetName_col].ToString(), StreetNum);
            if (newId == oldId)
            {
                return;
            }
            if (Span == "324-101-10692")  // Kaneshiro
            {
                newId = oldId;
            }
            else if (oldId == 189) // Hemlock City Lane
            {
                newId = oldId;
            }
            else
            {
                if (newId == 0)
                {
                }
                else if (StreetNum == 0)
                {
                }
                else if (oldId == 0)
                {
                }
                else
                {
                }
                grandListRow[U.BuildingRoadValueID_col] = newId;
            }
        }
        //****************************************************************************************************************************
        private void AddNameToHistory(DataTable grandListHistoryTbl, int grandlistId, string name1, string name2, int historyYear)
        {
            Name2 = CheckLastName(Name2);
            DataRow grandListHistoryRow = grandListHistoryTbl.NewRow();
            grandListHistoryRow[U.GrandListID_col] = grandlistId;
            grandListHistoryRow[U.Year_col] = historyYear;
            grandListHistoryRow[U.Name1_col] = name1;
            grandListHistoryRow[U.Name2_col] = name2;
            grandListHistoryTbl.Rows.Add(grandListHistoryRow);
        }
        //****************************************************************************************************************************
        public string CheckLastName(string name1)
        {
            if (name1.ToLower().Contains("wood"))
            {
            }
            name1 = name1.Replace(",", "");
            name1 = name1.Replace(".", "");
            name1 = name1.Replace(";", "");
            name1 = name1.Replace(":", "");
            if (String.IsNullOrEmpty(name1))
            {
                return "";
            }
            int indexOf = name1.IndexOf(' ');
            if (name1.ToLower().Contains("conservation fund the"))
            {
                name1 = "The Conservation Fund";
            }
            else if (name1.ToLower().Contains("town of jamaica"))
            {
                name1 = "Jamaica Town Of";
            }
            else if (name1.ToLower().Contains("the trevmor group llc"))
            {
                name1 = "Trevmor Group LLC";
            }
            else if (name1.ToLower().Contains("kneipjoseph"))
            {
                name1 = "Kneip Joseph";
            }
            else if (name1.ToLower().Contains("corriveau"))
            {
                name1 = "Corrivea" + " " + name1.Substring(9).Trim();
            }
            else if (name1.ToLower().Contains("buster's"))
            {
                name1 = "Busters" + " " + name1.Substring(8).Trim();
            }
            else if (name1.ToLower().Contains("o kane"))
            {
                name1 = name1.Substring(0, indexOf) + "'" + name1.Substring(indexOf).Trim();
            }
            else if (name1.ToLower().Contains("o hara"))
            {
                name1 = name1.Substring(0, indexOf) + "'" + name1.Substring(indexOf).Trim();
            }
            else if (name1.ToLower().Contains("o neill"))
            {
                name1 = name1.Substring(0, indexOf) + "'" + name1.Substring(indexOf).Trim();
            }
            else if (name1.ToLower().Contains("o brien"))
            {
                name1 = name1.Substring(0, indexOf) + "'" + name1.Substring(indexOf).Trim();
            }
            else if (indexOf > 0)
            {
                string lastname = name1.Substring(0, indexOf + 1).ToLower();
                if (lastname == "de " ||
                    lastname == "mc " ||
                    lastname == "van ")
                {
                    name1 = name1.Substring(0, indexOf).Trim() + name1.Substring(indexOf).Trim();
                }
                lastname = lastname.Trim();
                if (lastname == "basset")
                {
                    name1 = "Bassett " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "frolich")
                {
                    name1 = "Frohlich " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "theile")
                {
                    name1 = "thiele " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "Waria")
                {
                    name1 = "Warias " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "bar`s")
                {
                    name1 = "Bar S " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "lambardo")
                {
                    name1 = "Lambardo " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "ravinelli")
                {
                    name1 = "Rovinelli " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "baldus")
                {
                    name1 = "Baldis " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "fields")
                {
                    name1 = "field " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "coccis")
                {
                    name1 = "coccio " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "eberhard")
                {
                    name1 = "Eberhardt " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "sabev")
                {
                    name1 = "Sabeva " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "obrien")
                {
                    name1 = "O'Brien " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "oneill")
                {
                    name1 = "O'Neill " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "o-kane")
                {
                    name1 = "O'Kane " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "okane")
                {
                    name1 = "O'Kane " + name1.Substring(indexOf).Trim();
                }
                if (lastname == "winick")
                {
                    name1 = "winnick " + name1.Substring(indexOf).Trim();
                }
            }
            name1 = CapitalizeLowerCase(name1.ToLower(), true);
            if (name1.Contains("Gerich Miller"))
            {
                name1 = name1.Replace("Gerich Miller", "Gerich-Miller");
            }
            if (name1.Contains("Stoddardterrance"))
            {
                name1 = name1.Replace("Stoddardterrance", "Stoddard Terrance");
            }
            if (name1.Contains("Wheatleylouise"))
            {
                name1 = name1.Replace("Wheatleylouise", "Wheatley Louise");
            }
            if (name1.Contains("Town Of Winhall"))
            {
                name1 = name1.Replace("Town Of Winhall", "Winhall Town Of");
            }
            if (name1.Contains("Lee Lawlor"))
            {
                name1 = name1.Replace("Lee Lawlor", "Lee-Lawlor");
            }
            if (name1.Contains("Howe Grant"))
            {
                name1 = name1.Replace("Howe Grant", "Howe-Grant");
            }
            if (name1.Contains("Ressler Hochstat"))
            {
                name1 = name1.Replace("Ressler Hochstat", "Ressler-Hochstat");
            }
            if (name1.Contains("Guardiola Diaz"))
            {
                name1 = name1.Replace("Guardiola Diaz", "Guardiola-Diaz");
            }
            name1 = name1.Replace("`", "");
            return name1;
        }
        //****************************************************************************************************************************
        public bool NamesDifferent(DataRow grandListRow, ref int previousGrandListId)
        {
            if (Name1.Contains("`"))
            {
                Name1 = Name1.Replace("`", "'");
            }
            if (Name2.Contains("`"))
            {
                Name2 = Name2.Replace("`", "'");
            }
            if (Name1.Contains("  "))
            {
                Name1 = Name1.Replace("  ", " ");
            }
            if (Name2.Contains("  "))
            {
                Name2 = Name2.Replace("  ", " ");
            }
            string oldName1 = grandListRow[U.Name1_col].ToString();
            string oldName2 = grandListRow[U.Name2_col].ToString();
            if (oldName1.ToUpper() == Name2.ToUpper() && oldName2.ToUpper() == Name1.ToUpper())
            {
                return false;
            }
            if (NameIsReallyDifferent(grandListRow[U.Name1_col].ToString().ToUpper(), Name1.ToUpper(), false))
            {
                previousGrandListId = grandListRow["grandListID"].ToInt();
                return GetMessage(grandListRow, oldName1, oldName2, ref previousGrandListId);

                //return MessageBox.Show(GetMessage(grandListRow, oldName1, oldName2, ref previousGrandListId), "", 
                //                       MessageBoxButtons.YesNo) == DialogResult.Yes;
            }
            //if (NameIsReallyDifferent(grandListRow[U.Name2_col].ToString().ToUpper(), Name2.ToUpper(), true))
            //{
            //    return true;
            //}
            return false;
        }
        //****************************************************************************************************************************
        private bool GetMessage(DataRow grandListRow, string oldName1, string oldName2, ref int previousGrandListId)
        {
            string msg1 = "ID: " + grandListRow["grandListID"].ToString() + " (" + previousGrandListId + ")\r";
            string msg2 = grandListRow["Address"].ToString();
            string msg3 = CurrentAddress;
            if (oldName1.Length > oldName2.Length)
            {
                oldName2 = PadName(oldName2, oldName1.Length);
            }
            else
            {
                oldName1 = PadName(oldName1, oldName2.Length);
            }
            string msg4 = "\r" + oldName1 + " - " + Name1;
            string msg5 = "";
            if (!String.IsNullOrEmpty(oldName2) || !String.IsNullOrEmpty(Name2))
            {
                msg5 = "\r" + oldName2 + " - " + Name2;
            }
            FMsgBox msgBox = new FMsgBox(msg1, msg2, msg3, msg4, msg5);
            msgBox.ShowDialog();
            return msgBox.yesno == FMsgBox.Yesno.yes;
        }
        //****************************************************************************************************************************
        private string PadName(string oldName, int length)
        {
            if (oldName.Length == length)
            {
                return oldName;
            }
            int paddingLength = length - oldName.Length;
            return oldName + new String(' ', paddingLength);
        }
        //****************************************************************************************************************************
        private bool NameIsReallyDifferent(string oldName, string newName, bool isName2)
        {
            if (isName2 && (String.IsNullOrEmpty(oldName) || String.IsNullOrEmpty(newName)))  // Just a new co owner
            {
                return false;
            }
            bool isTheSameTrust;
            if (NowTrust(oldName, newName, out isTheSameTrust))
            {
                return !isTheSameTrust;
            }
            if (DifferentSuffix(oldName, newName))
            {
                return true;
            }
            string lastNameOld, firstNameOld, lastNameNew, firstNameNew;
            GrandListFirstnameLastName(oldName, out lastNameOld, out firstNameOld, true);
            GrandListFirstnameLastName(newName, out lastNameNew, out firstNameNew, true);
            if (lastNameOld != lastNameNew)
            {
                int indexOf = lastNameNew.IndexOf("'");
                if (indexOf > 0)
                {
                    if (lastNameOld[indexOf] == '`')
                    {
                        lastNameOld = lastNameOld.Remove(indexOf, 1);
                    }
                    lastNameOld = lastNameOld.Insert(indexOf, "'");
                    if (lastNameOld == lastNameNew)
                    {
                        return false;
                    }
                }
                return true;
            }
            if (firstNameOld == firstNameNew) // different middle name
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public void GrandListFirstnameLastName(string name, out string lastName, out string firstName, bool removeMiddleName = false)
        {
            if (String.IsNullOrEmpty(name))
            {
                firstName = "";
                lastName = "";
            }
            lastName = "";
            firstName = "";
            int indexOf = name.IndexOf(' ');
            if (indexOf < 0)
            {
                lastName = name;
            }
            else
            {
                lastName = name.Substring(0, indexOf).Trim();
                firstName = name.Substring(indexOf).Trim();
                if (removeMiddleName)
                {
                    indexOf = firstName.IndexOf(' ');
                    if (indexOf > 0)
                    {
                        firstName = firstName.Remove(indexOf);
                    }
                }
            }
        }
        //****************************************************************************************************************************
        private bool NowTrust(string oldName, string newName, out bool isTheSameTrust)
        {
            isTheSameTrust = false;
            bool oldNameIsTrust = oldName.ToLower().Contains("trust");
            bool newNameIsTrust = newName.ToLower().Contains("trust");
            if (oldNameIsTrust == newNameIsTrust)  // either both are trusts or neither is a trust.
            {
                if (!oldNameIsTrust)
                {
                    return false;
                }
                isTheSameTrust = (oldName.ToLower() == newName.ToLower());
                if (!isTheSameTrust)
                {
                }
            }
            return true;  // is the same trust
        }
        //****************************************************************************************************************************
        private bool DifferentSuffix(string oldName, string newName)
        {
            bool oldNameIsIV = NameContainsIV(oldName);
            bool newNameIsIV = NameContainsIV(newName);
            if (oldNameIsIV || newNameIsIV)
            {
                return oldNameIsIV != newNameIsIV;
            }
            bool oldNameIsIII = oldName.ToLower().Contains(" iii");
            bool newNameIsIII = newName.ToLower().Contains(" iii");
            if (oldNameIsIII || newNameIsIII)
            {
                if (oldNameIsIII != newNameIsIII)
                {
                }
                return oldNameIsIII != newNameIsIII;
            }
            bool oldNameIsJr = oldName.ToLower().Contains("jr") || oldName.ToLower().Contains("ii");
            bool newNameIsJr = newName.ToLower().Contains("jr") || newName.ToLower().Contains("ii");
            if (oldNameIsJr || newNameIsJr)
            {
                if (oldNameIsJr != newNameIsJr)
                {
                }
                return oldNameIsJr != newNameIsJr;
            }
            return false;
        }
        //****************************************************************************************************************************
        private bool NameContainsIV(string name)
        {
            int indexOfIV = name.ToLower().IndexOf(" iv");
            if (indexOfIV + 3 == name.Length || name[indexOfIV + 3] == ' ')
            {
                return true;
            }
            return false;
        }
        //****************************************************************************************************************************
        public void CreateNewGrandListRecord(DataRow grandListRow, char activeStatus)
        {
            grandListRow[U.GrandListID_col] = 0;
            grandListRow[U.Span_col] = Span;
            grandListRow[U.TaxMapID_col] = TaxMapID;
            grandListRow[U.Name1_col] = CapitalizeLowerCase(Name1.ToLower());
            grandListRow[U.Name2_col] = CapitalizeLowerCase(Name2.ToLower());
            grandListRow[U.StreetName_col] = CapitalizeLowerCase(StreetName.ToLower());
            grandListRow[U.StreetNum_col] = StreetNum;
            grandListRow[U.WhereOwnerLiveID_col] = Owner[0];
            grandListRow[U.ActiveStatus_col] = activeStatus;
            grandListRow[U.VacantLand_col] = VacantLand;
            grandListRow[U.BuildingRoadValueID_col] = GetModernRoadValue(grandListRow[U.StreetName_col].ToString(), StreetNum);
        }
        //****************************************************************************************************************************
        private int GetModernRoadValue(string streetName, int streetNum)
        {
            if (String.IsNullOrEmpty(streetName))
            {
                return 0;
            }
            streetName = streetName.Replace("Mtn", "Mountain");
            streetName = streetName.Replace("Old Rte 8", "Old Route 8");
            streetName = streetName.Replace("Olde", "Old");
            streetName = streetName.Replace("`", "");
            streetName = streetName.Replace("'", "");
            streetName = ReplaceAbbrevation(streetName, " Rd ", " Road");
            streetName = ReplaceAbbrevation(streetName, " St ", " Street");
            streetName = ReplaceAbbrevation(streetName, " Ln ", " Lane");
            streetName = ReplaceAbbrevation(streetName, " Dr ", " Drive");
            streetName = SpecialRoadValue(streetName, streetNum);  // must be after abreviations
            string selectStatement = U.ModernRoadValueValue_col + " = '" + streetName + "'";
            DataRow[] foundRows = modernRoadValueTbl.Select(selectStatement);
            if (foundRows.Length == 0)
            {
                return 0;
            }
            return foundRows[0][U.ModernRoadValueID_col].ToInt();
        }
        //****************************************************************************************************************************
        private string GetModifiedStreetName(string streetName, int streetNum)
        {
            if (String.IsNullOrEmpty(streetName))
            {
                return "";
            }
            streetName = streetName.Replace("Mtn", "Mountain");
            streetName = streetName.Replace("Old Rte 8", "Old Route 8");
            streetName = streetName.Replace("Olde", "Old");
            streetName = streetName.Replace("`", "");
            streetName = streetName.Replace("'", "");
            //streetName = SpecialRoadValue(ref streetName, streetNum);
            return streetName;
        }
        //****************************************************************************************************************************
        private string SpecialRoadValue(string streetName, int streetNum)
        {
            int indexOf = streetName.IndexOf(" Lane A");
            if (indexOf > 0)
            {
                return streetName.Substring(0, indexOf) + " Lane";
            }
            indexOf = streetName.IndexOf(" Lane B");
            if (indexOf > 0)
            {
                return streetName.Substring(0, indexOf) + " Lane";
            }
            indexOf = streetName.IndexOf(" Lane C");
            if (indexOf > 0)
            {
                return streetName.Substring(0, indexOf) + " Lane";
            }
            if (streetName.Substring(0, 4).ToUpper() == "VT R")
            {
                return SubstituteVtRoute(streetName, streetNum);
            }
            indexOf = streetName.IndexOf(" Ln A");
            if (indexOf > 0)
            {
                return streetName.Substring(0, indexOf) + " Lane";
            }
            indexOf = streetName.IndexOf(" Ln B");
            if (indexOf > 0)
            {
                return streetName.Substring(0, indexOf) + " Lane";
            }
            indexOf = streetName.IndexOf(" Ln C");
            if (indexOf > 0)
            {
                return streetName.Substring(0, indexOf) + " Lane";
            }
            if (streetName.ToUpper() == "PIKES FALLS ROAD" && streetNum < 300)
            {
                return "Pikes Falls-Mechanic St";
            }
            if (streetName.ToUpper().Contains("WEST HILL"))
            {
                return streetName.Replace(" Rd ", " Road ");
            }
            return streetName;
        }
        //****************************************************************************************************************************
        private string SubstituteVtRoute(string streetName, int streetNum)
        {
            int indexOf = streetName.ToUpper().IndexOf("VT ROUTE ");
            if (indexOf >= 0)
            {
                streetName = "Route " + streetName.Remove(0, 9);
            }
            indexOf = streetName.ToUpper().IndexOf("VT RTE ");
            if (indexOf >= 0)
            {
                streetName = "Route " + streetName.Remove(0, 7);
            }
            indexOf = streetName.ToUpper().IndexOf("VT RT ");
            if (indexOf >= 0)
            {
                streetName = "Route " + streetName.Remove(0, 6);
            }
            if (streetName.Contains("30"))
            {
                indexOf = streetName.IndexOf("@");
                if (indexOf > 0)
                {
                    streetName = "Potter Road";
                }
                else if (streetNum < 3458)
                {
                    streetName += " South";
                }
                else if (streetNum > 8550)
                {
                    char lastCharInString = streetName[streetName.Length - 1];
                    if (lastCharInString != '0')
                    {
                        streetName = streetName.Replace("30 A", "30");
                        streetName = streetName.Replace("30 B", "30");
                        streetName = streetName.Replace("30 C", "30");
                        streetName = streetName.Replace("30 D", "30");
                        streetName = streetName.Replace("30 E", "30");
                        streetName = streetName.Replace("30 F", "30");
                        streetName = streetName.Replace("30 G", "30");
                        streetName = streetName.Replace("30 H", "30");
                        streetName = streetName.Replace("30 I", "30");
                    }
                    streetName += "-Rawsonville";
                }
                else if (streetNum > 3924)
                {
                    streetName += " North";
                }
                else
                {
                    streetName = "Main Street";
                }
            }
            else
            {
                char lastCharInString = streetName[streetName.Length - 1];
                if (lastCharInString == 'S' || lastCharInString == 'N')
                {
                    streetName = streetName.Replace("100 S", "100 South");
                    streetName = streetName.Replace("100 N", "100 North");
                }

                else
                {
                    streetName = streetName.Replace("NORTH", "North");
                    streetName = streetName.Replace("SOUTH", "South");
                }
            }
            return streetName;
        }
        //****************************************************************************************************************************
        private string ReplaceAbbrevation(string streetName, string abbrevation, string fullWord)
        {
            streetName = streetName + " ";
            int indexOf = streetName.IndexOf(abbrevation);
            if (indexOf > 0)
            {
                return streetName.Replace(abbrevation, fullWord).Trim();
            }
            return streetName.Trim();
        }
        //****************************************************************************************************************************
        private void SetNewValueIfDifferent(DataRow grandListRow, string col, char newValue)
        {
            if (newValue != grandListRow[col].ToChar())
            {
                grandListRow[col] = newValue;
            }
        }
        //****************************************************************************************************************************
        private void SetNewValueIfDifferent(DataRow grandListRow, string col, string newValue, bool isName = false)
        {
            newValue = newValue.Replace("  ", " ");
            if (!String.IsNullOrEmpty(newValue))
            {
                newValue = CapitalizeLowerCase(newValue.ToLower(), isName);
            }
            if (grandListRow[col].ToString() != newValue)
            {
                grandListRow[col] = newValue;
            }
        }
        //****************************************************************************************************************************
        private string CapitalizeLowerCase(string str, bool isName = false)
        {
            string returnString = "";
            string[] words = str.Split(' ');
            foreach (string word in words)
            {
                if (!String.IsNullOrEmpty(word))
                {
                    returnString += CapitalizeFirstChar(word, isName) + " ";
                }
                isName = false;
            }
            return returnString.Trim();
        }
        //****************************************************************************************************************************
        private string NameWithAffix(string str, bool isName)
        {
            if (str.Length < 2)
            {
                return str;
            }
            int indexOf = str.IndexOf('-');
            if (indexOf > 0 && isName)
            {
                return DoubleCapital(str, indexOf + 1);
            }
            indexOf = str.ToLower().IndexOf("mc", 0, 2);
            if (indexOf == 0)
            {
                return DoubleCapital(str, 2);
            }
            switch (str.ToUpper())
            {
                case "MACDONALD": return DoubleCapital(str, 3);
                case "DEBELLIS": return DoubleCapital(str, 2);
                case "DECASTRO": return DoubleCapital(str, 2);
                case "DECONINCK": return DoubleCapital(str, 2);
                case "DELAPORTE": return DoubleCapital(str, 2);
                case "DELVECCHIO": return DoubleCapital(str, 3);
                case "DESOMMA": return DoubleCapital(str, 2);
                case "DEVITO": return DoubleCapital(str, 2);
                case "DILIELLO": return DoubleCapital(str, 2);
                case "DIRAIMONDO": return DoubleCapital(str, 2);
                case "DISABATO": return DoubleCapital(str, 2);
                case "DUBOSQUE": return DoubleCapital(str, 2);
                case "LAMARCHE": return DoubleCapital(str, 2);
                case "LAMONICA": return DoubleCapital(str, 2);
                case "LAPOINTE": return DoubleCapital(str, 2);
                case "LAVALLE": return DoubleCapital(str, 2);
                case "VANHOUTEN": return DoubleCapital(str, 3);
                case "VANKIRK": return DoubleCapital(str, 3);
                case "VANREUSEL": return DoubleCapital(str, 3);
                default: return str;
            }
        }
        //****************************************************************************************************************************
        private string DoubleCapital(string str, int indexOfSecondCapital)
        {
            char ch = str[0].ToChar();
            str = char.ToUpper(str[0]) + str.Substring(1);
            return str.Substring(0, indexOfSecondCapital) + char.ToUpper(str[indexOfSecondCapital]) + str.Substring(indexOfSecondCapital + 1);
        }
        //****************************************************************************************************************************
        private string CapitalizeFirstChar(string str, bool isName)
        {
            int indexOf = str.IndexOf('`');
            if (indexOf == 1)
            {
                if (str.Length > indexOf)
                {
                    str = str.Replace("`", "");
                }
                else
                {
                    str = str.Replace("`", "'");
                    str = DoubleCapital(str, indexOf + 1);
                }
            }
            indexOf = str.IndexOf("'");
            if (indexOf == 1)
            {
                str = DoubleCapital(str, indexOf + 1);
            }
            if (str == "ii")
            {
                return "II";
            }
            if (str == "iii")
            {
                return "III";
            }
            if (str == "iv")
            {
                return "IV";
            }
            if (str == "po" || str == "p.o.")
            {
                return "PO";
            }
            if (str == "llc")
            {
                return "LLC";
            }
            if (str == "llp")
            {
                return "LLP";
            }
            if (str.Length == 2)
            {
                return CheckForState(str);
            }
            indexOf = str.ToUpper().IndexOf("C/O");
            if (indexOf >= 0)
            {
                str = str.Remove(0, 3);
                if (str.Length > 0)
                {
                    str = char.ToUpper(str[0]) + str.Substring(1);
                }
                return "C/O" + str;
            }
            str = char.ToUpper(str[0]) + str.Substring(1);
            indexOf = str.IndexOf('-');
            if (indexOf > 0 && str.Length > indexOf + 1)
            {
                if (Char.IsLower(str[indexOf + 1]))
                {
                    str = str.Substring(0, indexOf + 1) + char.ToUpper(str[indexOf + 1]) + str.Substring(indexOf + 2);
                }
            }
            return NameWithAffix(str, isName);
        }
        //****************************************************************************************************************************
        private string CheckForState(string str)
        {
            switch (str.ToUpper())
            {
                case "AL": return str.ToUpper();
                case "AK": return str.ToUpper();
                case "AZ": return str.ToUpper();
                case "AR": return str.ToUpper();
                case "CA": return str.ToUpper();
                case "CO": return str.ToUpper();
                case "CT": return str.ToUpper();
                case "DE": return str.ToUpper();
                case "FL": return str.ToUpper();
                case "GA": return str.ToUpper();
                case "HI": return str.ToUpper();
                case "ID": return str.ToUpper();
                case "IL": return str.ToUpper();
                case "IN": return str.ToUpper();
                case "IA": return str.ToUpper();
                case "KS": return str.ToUpper();
                case "KY": return str.ToUpper();
                case "LA": return str.ToUpper();
                case "ME": return str.ToUpper();
                case "MD": return str.ToUpper();
                case "MA": return str.ToUpper();
                case "MI": return str.ToUpper();
                case "MN": return str.ToUpper();
                case "MS": return str.ToUpper();
                case "MO": return str.ToUpper();
                case "MT": return str.ToUpper();
                case "NE": return str.ToUpper();
                case "NV": return str.ToUpper();
                case "NH": return str.ToUpper();
                case "NJ": return str.ToUpper();
                case "NM": return str.ToUpper();
                case "NY": return str.ToUpper();
                case "NC": return str.ToUpper();
                case "ND": return str.ToUpper();
                case "OH": return str.ToUpper();
                case "OK": return str.ToUpper();
                case "OR": return str.ToUpper();
                case "PA": return str.ToUpper();
                case "RI": return str.ToUpper();
                case "SC": return str.ToUpper();
                case "SD": return str.ToUpper();
                case "TN": return str.ToUpper();
                case "TX": return str.ToUpper();
                case "UT": return str.ToUpper();
                case "VT": return str.ToUpper();
                case "VA": return str.ToUpper();
                case "WA": return str.ToUpper();
                case "WV": return str.ToUpper();
                case "WI": return str.ToUpper();
                case "WY": return str.ToUpper();
                case "RD": return "Road";
                case "ST": return "Street";
                case "LN": return "Lane";
                case "DR": return "Drive";
                default: return char.ToUpper(str[0]) + str.Substring(1);
            }
        }
        //****************************************************************************************************************************
    }
}
