using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Windows.Forms;

namespace SQL_Library
{
    //****************************************************************************************************************************
    public enum ErrorCodes
    {
        eSuccess = 0,
        eException = -1,
        eSaveUnsuccessful = -2,
        eDeleteUnsuccessful = -3,
        eUpdateUnsuccessful = -4,
        eSpouseRecordDoesNotExist = -5
    };
    public enum Cemetery
    {
        _Cemetery = 0,
        SouthHill_Cemetery = 1,
        SageHill_Cemetery = 2,
        SeventhDayAdventist_Cemetery = 3,
        Robbins_Cemetery = 4,
        Village_Cemetery = 5,
        Rawsonville_Cemetery = 6,
        SouthWindham_Cemetery = 7,
        PikesFalls_Cemetery = 8,
        WestJamaica_Cemetery = 9,
        EastJamaica_Cemetery = 10,
        Fessenden_Cemetery = 11,
        MundellBourne_Cemetery = 12,
        RobinsonDalewood_Cemetery = 13,
        Pratt_Cemetery = 14,
        Ramsdell_Cemetery = 15,
        Wilder_Cemetery = 16
    }
    //****************************************************************************************************************************
    public class HistoricJamaicaException : System.Exception
    {
        public ErrorCodes errorCode;
        public string errorString;
        public HistoricJamaicaException(string errorString) : base(errorString)
        {
            this.errorCode = ErrorCodes.eException;
            this.errorString = errorString;
        }
        public HistoricJamaicaException(ErrorCodes errorCode)
        {
            this.errorCode = errorCode;
            this.errorString = "";
        }
    }
    public enum EVitalRecordType
    {
        eBirthMale = 1,
        eBirthFemale = 2,
        eDeathMale = 3,
        eDeathFemale = 4,
        eMarriageGroom = 5,
        eMarriageBride = 6,
        eCivilUnionPartyA = 7,
        eCivilUnionPartyB = 8,
        eBurial = 9,
        eSearch = 10,
        eIntegrateAll = 11,
        eSchool = 12
    };
    public enum eSex
    {
        eMale = 0,
        eFemale = 1,
        eUnknown = 2
    }
    public enum eSearchOption
    {
        SO_AllNames,
        SO_PartialNames,
        SO_StartingWith,
        SO_Similar,
        SO_ReturnToLast,
        SO_None
    }
    public struct NameValuePair
    {
        private string m_sName;
        private object m_sValue;
        public NameValuePair(string sName,
                             object sValue,
                             bool adjustSingleQuote=true)
        {
            m_sName = sName;
            m_sValue = sValue;
            if (m_sValue.GetType() == typeof(string))
            {
                int indexOfSingleQuote = m_sValue.ToString().IndexOf("'");
                if (indexOfSingleQuote >= 0 && adjustSingleQuote)
                {
                    m_sValue = m_sValue.ToString().Replace("'", "''");
                }
            }
        }
        public string Name()
        {
            return m_sName;
        }
        public string Value()
        {
            return m_sValue.ToString();
        }
        public object ObjectValue()
        {
            return m_sValue;
        }
    }
    public static partial class U       // Utilities
    {   // Global Constants
        public const string maidenNameConstant = "MaidenName";
        public const int iBuilding = 0;
        public const int iOccupant = -1;
        public const int i1856BuildingName = -5;
        public const int i1869BuildingName = -2;
        public const int iCurrentOwners = -3;
        public const int iBuildingName = -4;
        public const int iMaxSizeOfGrid = 1400;
        public const int iMaxDateLength = 20;
        public const int iMaxNameLength = 20;
        public const int iMaxStoneLength = 20;
        public const int iMaxPrefixSuffixLength = 3;
        public const int iMaxFileExtensionLength = 4;
        public const int iMaxBookPageLength = 10;
        public const int iMaxPlaceLength = 50;
        public const int iMaxValueLength = 50;
        public const int iMaxFullNameLength = 50;
        public const int iMaxDescriptionLength = 1000;
        public const int iMaxArticleLength = 5000;
        public const int PersonType = 0;
        public const int SpouseType = 1;
        public const int PersonFatherType = 2;
        public const int PersonMotherType = 3;
        public const int SpouseFatherType = 4;
        public const int SpouseMotherType = 5;
        public const int Exception = -999999999;
        public const string NoSQLError = "";
        public const char slash = '/';
        public const char   Tab = '\t';
        public const string NoOrderBy = "";
        public const string Unknown = "Unknown";
        public const string JamaicaVitalRecords = "Jamaica Vital Records";
        public const string JamaicaCemeteryRecords = "Jamaica Cemetery Records";
        public const string ManualEntry = "Manual Entry";
        public const string comma = ",";
        public const string AtSign = "@";
        public const string dot = ".";
        public const string quote = "'";
        public const string sJPGExtension = ".jpg";
        public const string sTIFExtension = ".tif";
        // tables
        public const string AlternativeSpellingsFirstName_Table = "AlternativeSpellingsFirstName";
        public const string AlternativeSpellingsLastName_Table = "AlternativeSpellingsLastName";
        public const string FirstName_Table = "FirstName";
        public const string Building_Table = "Building";
        public const string BuildingValue_Table = "BuildingValue";
        public const string Category_Table = "Category";
        public const string CategoryValue_Table = "CategoryValue";
        public const string Cemetery_Table = "Cemetery";
        public const string CemeteryRecord_Table = "CemeteryRecord";
        public const string PersonCW_Table = "PersonCW";
        public const string Map_Table = "Map";
        public const string Marriage_Table = "Marriage";
        public const string ModernRoadValue_Table = "ModernRoadValue";
        public const string Composers_Table = "Composers";
        public const string Person_Table = "Person";
        public const string PersonCategoryValue_Table = Person_Table + CategoryValue_Table;
        public const string BuildingOccupant_Table = "BuildingOccupant";
        public const string Photo_Table = "Photo";
        public const string SlideShow_Table = "SlideShow";
        public const string GrandList_Table = "GrandList";
        public const string GrandListHistory_Table = "GrandListHistory";
        public const string GrandListAddress_Table = "GrandList";
        public const string WasteDisposalPermits_Table = "WasteDisposalPermit";
        public const string CareTaker_Table = "CareTaker";
        public const string PhotoCategoryValue_Table = Photo_Table + CategoryValue_Table;
        public const string PicturedBuilding_Table = "PicturedBuilding";
        public const string PicturedPerson_Table = "PicturedPerson";
        public const string VitalRecord_Table = "VitalRecord";
        public const string Articles_Table = "Article";
        public const string School_Table = "School";
        public const string SchoolRecord_Table = "SchoolRecord";
        // Columns in Person Table
        public const string AlternativeSpelling_Col = "AlternativeSpelling";
        public const string NameSpelling1_Col = "NameSpelling1";
        public const string NameSpelling2_Col = "NameSpelling2";
        public const string VitalRecordID_col = "VitalRecordID";
        public const string VitalRecordType_col = "VitalRecordType";
        public const string FatherID_col = "FatherID";
        public const string SpouseFirstName_col = "SpouseFirstName";
        public const string SpouseMiddleName_col = "SpouseMiddleName";
        public const string SpouseLastName_col = "SpouseLastName";
        public const string SpouseSuffix_col = "SpouseSuffix";
        public const string SpousePrefix_col = "SpousePrefix";
        public const string FatherFirstName_col = "FatherFirstName";
        public const string FatherMiddleName_col = "FatherMiddleName";
        public const string FatherLastName_col = "FatherLastName";
        public const string FatherSuffix_col = "FatherSuffix";
        public const string FatherPrefix_col = "FatherPrefix";
        public const string MotherID_col = "MotherID";
        public const string MotherFirstName_col = "MotherFirstName";
        public const string MotherMiddleName_col = "MotherMiddleName";
        public const string MotherLastName_col = "MotherLastName";
        public const string MotherSuffix_col = "MotherSuffix";
        public const string MotherPrefix_col = "MotherPrefix";
        public const string Book_col = "Book";
        public const string Page_col = "Page";
        public const string LotNumber_col = "LotNumber";
        public const string Notes_col = "Notes";
        public const string NotesCurrentOwner_col = "NotesCurrentOwner";
        public const string Notes1856Name_col = "Notes1856Name";
        public const string Notes1869Name_col = "Notes1869Name";
        public const string QRCode_col = "QRCode";
        public const string Then1_col = "Then1";
        public const string Then2_col = "Then2";
        public const string Now1_col = "Now1";
        public const string Now2_col = "Now2";
        public const string Then1Title_col = "Then1Title";
        public const string Then2Title_col = "Then2Title";
        public const string Now1Title_col = "Now1Title";
        public const string Now2Title_col = "Now2Title";
        public const string DateYear_col = "DateYear";
        public const string DateMonth_col = "DateMonth";
        public const string DateDay_col = "DateDay";
        public const string AgeYears_col = "AgeYears";
        public const string AgeMonths_col = "AgeMonths";
        public const string AgeDays_col = "AgeDays";
        public const string PersonID_col = "PersonID";
        public const string School_col = "School";
        public const string SchoolID_col = "SchoolID";
        public const string SchoolRecordType_col = "SchoolRecordType";
        public const string SchoolRecordID_col = "SchoolRecordID";
        public const string Year_col = "Year";
        public const string Grade_col = "Grade";
        public const string Person_col = "Person";
        public const string PersonCWID_col = "PersonCWID";
        public const string NameOnGrave_col = "NameOnGrave";
        public const string SpouseNameOnGrave_col = "SpouseNameOnGrave";
        public const string FatherNameOnGrave_col = "FatherNameOnGrave";
        public const string MotherNameOnGrave_col = "MotherNameOnGrave";
        public const string FirstName_col = "FirstName";
        public const string MiddleName_col = "MiddleName";
        public const string LastName_col = "LastName";
        public const string Suffix_col = "Suffix";
        public const string Prefix_col = "Prefix";
        public const string MarriedName_col = "MarriedName";
        public const string MarriedName2_col = "MarriedName2";
        public const string MarriedName3_col = "MarriedName3";
        public const string KnownAs_col = "KnownAs";
        public const string MainFatherID_col = "Main" + FatherID_col;
        public const string MainMotherID_col = "Main" + MotherID_col;
        public const string Source_col = "Source";
        public const string Sex_col = "Sex";
        public const string EnlistmentDate_col = "EnlistmentDate";
        public const string BornDate_col = "BornDate";
        public const string BornPlace_col = "BornPlace";
        public const string BornHome_col = "BornHome";
        public const string BornVerified_col = "BornVerified";
        public const string BornSource_col = "BornSource";
        public const string BornBook_col = "BornBook";
        public const string BornPage_col = "BornPage";
        public const string Date_col = "Date";
        public const string DiedDate_col = "DiedDate";
        public const string DiedPlace_col = "DiedPlace";
        public const string DiedHome_col = "DiedHome";
        public const string DiedVerified_col = "DiedVerified";
        public const string DiedSource_col = "DiedSource";
        public const string DiedBook_col = "DiedBook";
        public const string DiedPage_col = "DiedPage";
        public const string BuriedDate_col = "BuriedDate";
        public const string BuriedPlace_col = "BuriedPlace";
        public const string BuriedStone_col = "BuriedStone";
        public const string BuriedVerified_col = "BuriedVerified";
        public const string BuriedSource_col = "BuriedSource";
        public const string BuriedBook_col = "BuriedBook";
        public const string BuriedPage_col = "BuriedPage";
        public const string ImportPersonID_col = "ImportPersonID";
        public const string CensusYears_col = "CensusYears";
        public const string Census1790_col = "Census1790";
        public const string Census1800_col = "Census1800";
        public const string Census1810_col = "Census1810";
        public const string Census1820_col = "Census1820";
        public const string Census1830_col = "Census1830";
        public const string Census1840_col = "Census1840";
        public const string Census1850_col = "Census1850";
        public const string Census1860_col = "Census1860";
        public const string Census1870_col = "Census1870";
        public const string Census1880_col = "Census1880";
        public const string Census1890_col = "Census1890";
        public const string Census1900_col = "Census1900";
        public const string Census1910_col = "Census1910";
        public const string Census1920_col = "Census1920";
        public const string Census1930_col = "Census1930";
        public const string Census1940_col = "Census1940";
        public const string Census1950_col = "Census1950";
        public const string ExcludeFromSite_col = "ExcludeFromSite";
        public const string GazetteerRoad_col = "GazetteerRoad";
        public const string Beers1869District_col = "Beers1869District";
        public const string McClellan1856District_col = "McClellan1856District";
        // Columns in Marriage Table
        public const string SpouseID_col = "SpouseID";
        public const string DateMarried_col = "DateMarried";
        public const string Divorced_col = "MaritalStatus";
        // Columns In WasteDisposalPermits Table
        public const string PermitID_col = "PermitID";
        public const string CareTakerID_col = "CareTakerID";
        public const string CaretakerName_col = "CaretakerName";
        public const string Town_col = "Town";
        public const string PermitNumber_col = "PermitNumber";
        public const string Apartment_col = "Apartment";
        public const string Phone_col = "Phone";
        public const string EMail_col = "EMail";
        public const string PermitType_col = "PermitType";
        public const string NumberCards_col = "NumberCards";
        public const string NumClients_col = "NumClients";
        public const string Status_col = "Status";
        // Columns In GrandList Table
        public const string GrandListID_col = "GrandListID";
        public const string Building1856Name_col = "Building1856Name";
        public const string Building1869Name_col = "Building1869Name";
        public const string Span_col = "Span";
        public const string TaxMapID_col = "TaxMapID";
        public const string BuildingValue_col = "BuildingValue";
        public const string TotalAcres_col = "TotalAcres";
        public const string StreetName_col = "StreetName";
        public const string StreetNum_col = "StreetNum";
        public const string Name1_col = "Name1";
        public const string Name2_col = "Name2";
        public const string AddressA_col = "AddressA";
        public const string AddressB_col = "AddressB";
        public const string City_col = "City";
        public const string State_col = "State";
        public const string Zip_col = "Zip";
        public const string WhereOwnerLiveID_col = "WhereOwnerLiveID";
        public const string ActiveStatus_col = "ActiveStatus";
        public const string VacantLand_col = "VacantLand";
        // Columns In Picture Table
        public const string PhotoID_col = "PhotoID";
        public const string PhotoSequence_col = "PhotoSequence";
        public const string PhotoExtension_col = "PhotoExtension";
        public const string PhotoNotes_col = "PhotoNotes";
        public const string PhotoYear_col = "PhotoYear";
        public const string PhotoSource_col = "PhotoSource";
        public const string PhotoDrawer_col = "PhotoDrawer";
        public const string PhotoFolder_col = "PhotoFolder";
        public const string PhotoName_col = "PhotoName";
        public const string NumPicturedPersons_col = "NumPicturedPersons";
        public const string NumPicturedBuildings_col = "NumPicturedBuildings";
        public const string PicturedPersonNumber_col = "PicturedPersonNumber";
        public const string PicturedBuildingNumber_col = "PicturedBuildingNumber";
        // Columns In All Category tables
        public const string CategoryID_col = "CategoryID";
        public const string CategoryName_col = "CategoryName";
        public const string CategoryValueID_col = "CategoryValueID";
        public const string CategoryValueValue_col = "CategoryValueValue";
        public const string CategoryValueOrder_col = "CategoryValueOrder";
        // Columns In All Cemetery tables 
        public const string CemeteryID_col = "CemeteryID";
        public const string Disposition_col = "Disposition";
        public const string BattleSiteKilled_col = "BattleSiteKilled";
        public const string DataMilitary_col = "DataMilitary";
        public const string Reference_col = "Reference";
        public const string CemeteryName_col = "CemeteryName";
        public const string CemeteryRecordID_col = "CemeteryRecordID";
        public const string PersonName_col = "PersonName";
        public const string FatherName_col = "FatherName";
        public const string MotherName_col = "MotherName";
        public const string SpouseName_col = "SpouseName";
        public const string PersonAge_col = "PersonAge";
        public const string Epitaph_col = "Epitaph";
        public const string CemeteryNote1_col = "Note1";
        public const string CemeteryNote2_col = "Note2";
        public const string CemeteryNote3_col = "Note3";
        // Columns In All Building tables
        public const string BuildingID_col = "BuildingID";
        public const string BuildingName_col = "BuildingName";
        public const string BuildingValueID_col = "BuildingValueID";
        public const string SpouseLivedWithID_col = "SpouseLivedWithID";
        public const string ModernRoadValueID_col = "ModernRoadValueID";
        public const string ModernRoadValueValue_col = "ModernRoadValueValue";
        public const string ModernRoadValueOrder_col = "ModernRoadValueOrder";
        public const string ModernRoadValueSection_col = "ModernRoadValueSection";
        public const string JRoadName_col = "JRoadName";
        public const string HistoricRoad_col = "HistoricRoad";
        public const string BuildingGrandListID_col = "BuildingGrandListID";
        public const string BuildingValueValue_col = "BuildingValueValue";
        public const string BuildingValueOrder_col = "BuildingValueOrder";
        public const string BuildingValueOrder1856Name_col = "BuildingValueOrder1856Name";
        public const string BuildingValueOrder1869Name_col = "BuildingValueOrder1869Name";
        public const string BuildingArchitectureArticleID_col = "BuildingArchitectureArticleID";
        public const string BuildingDescriptionArticleID_col = "BuildingDescriptionArticleID";
        public const string BuildingRoadValueID_col = "BuildingRoadValueID";
        public const string ArticleID_col = "ArticleID";
        public const string Article_col = "Article";
        public const string ArticleContinueID_col = "ArticleContinueID";
        public const string sOccupantString = "(Occupant)";
        public const string sBuildingNameString = "(Building Name)";
        public const string s1856BuildingNameString = "(1856 Map Name)";
        public const string s1869BuildingNameString = "(1869 Map Name)";
        public const string s1869OccupantString = "(1869 Occupant)";
        public const string s1856OccupantString = "(1856 Occupant)";
        public const string s1884OccupantString = "(1884 Occupant)";
        public const string s1869and1884OccupantString = "(1869 and 1884 Occupant)";
        public const string s1869and1856OccupantString = "(1856 and 1869 Occupant)";
        public const string s1869and1856and1884OccupantString = "(1856,1869 and 1884 Occupant)";
        public const string s1869MapString = "Map 1869";
        public const string s1856MapString = "Map 1856";
        public const string s1884MapString = "Map 1884";
        public const string s1869and1884MapString = "Maps 1869,1884";
        public const string s1869and1856MapString = "Maps 1856,1869";
        public const string s1869and1856and1884MapString = "Maps 1856,1869,1884";
        public const string sCurrentOccupantString = "(Current Owner)";
        public const string sOwnerString = "(Owner)";
        //****************************************************************************************************************************
        public static void ProportionalCoordinates(int iMaxHeight,
                                            int iMaxWidth,
                                            int iPictureHeight,
                                            int iPictureWidth,
                                        ref int iHeight,
                                        ref int iWidth)
        {
            iWidth = (iMaxHeight * iPictureWidth) / iPictureHeight;
            iHeight = iMaxHeight;
            if (iWidth > iMaxWidth)
            {
                iWidth = iMaxWidth;
                iHeight = (iMaxWidth * iPictureHeight) / iPictureWidth;
            }
        }
        //****************************************************************************************************************************
        public static bool DeleteFile(string sFilename)
        {
            try
            {
                sFilename = sFilename.Replace("''", "'");
                System.IO.File.Delete(sFilename);
            }
            catch (IOException exc)
            {
                errors.SQLShowError(exc.Message.ToString());
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool MoveFile(string sNewFilename,
                                    string sOldFilename)
        {
            try
            {
                sNewFilename = sNewFilename.Replace("''", "'");
                System.IO.File.Move(sOldFilename, sNewFilename);
            }
            catch (IOException exc)
            {
                errors.SQLShowError(exc.Message.ToString());
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool CopyFile(string sNewFilename,
                                    string sOldFilename)
        {
            try
            {
                FileInfo TheFile = new FileInfo(sOldFilename);
                if (TheFile.Exists)
                {
                    sNewFilename = sNewFilename.Replace("''", "'");
                    System.IO.File.Copy(sOldFilename, sNewFilename);
                }
            }
            catch (IOException exc)
            {
                errors.SQLShowError(exc.Message.ToString());
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static string RenameFile(string newValue, string oldValue, string oldFilename, string extension)
        {
            var oldValueWithExtension = oldValue + "." + extension;
            var newValueWithExtension = newValue + "." + extension;
            var newFilename = oldFilename.Replace(oldValueWithExtension, newValueWithExtension);
            RenameFile(newFilename, oldFilename);
            return newFilename;
        }
        //****************************************************************************************************************************
        public static bool RenameFile(string sNewFilename,
                                      string sOldFilename)
        {
            try
            {
                FileInfo TheFile = new FileInfo(sOldFilename);
                if (TheFile.Exists)
                {
                    File.Move(sOldFilename, sNewFilename);
                }
            }
            catch (IOException exc)
            {
                errors.SQLShowError(exc.Message.ToString());
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static string ToStringSpacesWhenZero(this object obj)
        {
            if (ToInt(obj) == 0)
                return "";
            else
                return obj.ToString();
        }
        //****************************************************************************************************************************
        public static bool IsNumeric(this char cChar)
        {
            if (cChar >= '0' && cChar <= '9')
                return true;
            else
                return false;
        }
        //****************************************************************************************************************************
        public static int NewHeight(this string sHeight,
                                        int iAdditionalAmount)
        {
            int iStartPosition = sHeight.LastIndexOf("px");
            if (iStartPosition <= 0)
                return 0;
            sHeight = sHeight.Remove(iStartPosition);
            int iNewWidth = ToInt(sHeight.ToInt() + iAdditionalAmount);
            if (iNewWidth < 0)
                iNewWidth = 0;
            return iNewWidth;
        }
        //****************************************************************************************************************************
        public static int NewWidth(this string sWidth,
                                        int iAdditionalAmount)
        {
            int iStartPosition = sWidth.LastIndexOf("px");
            if (iStartPosition <= 0)
                return 0;
            sWidth = sWidth.Remove(iStartPosition);
            int iNewWidth = ToInt(sWidth.ToInt() + iAdditionalAmount);
            if (iNewWidth < 0)
                iNewWidth = 0;
            return iNewWidth;
        }
        //****************************************************************************************************************************
        public static void MergeTables(DataTable personValueTable,
                                        DataTable mergeValueTable,
                                        string ID_col)
        {
            foreach (DataRow mergeRow in mergeValueTable.Rows)
            {
                if (MergeRecordDoesNotExistsInPersonTable(personValueTable, mergeRow, ID_col))
                {
                    DataRow newRow = personValueTable.NewRow();
                    personValueTable.copyRowValues(newRow, mergeRow);
                    personValueTable.Rows.Add(newRow);
                }
            }
        }
        //****************************************************************************************************************************
        private static bool MergeRecordDoesNotExistsInPersonTable(DataTable personValueTable,
                                                                  DataRow mergeRow,
                                                                  string ID_col)
        {
            foreach (DataRow personRow in personValueTable.Rows)
            {
                if (personRow[ID_col].ToInt() == mergeRow[ID_col].ToInt())
                {
                    return false;
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        public static DataTable CheckForNonDuplicateMarriages(DataTable PersonMarriages_tbl,
                                                        DataTable MergePersonMarriages_tbl,
                                                        int iPersonID,
                                                        int iMergePersonID)
        {
            foreach (DataRow MergePerson_row in MergePersonMarriages_tbl.Rows)
            {
                int iMergePersonLocationInArray = 0;
                int iMergeSpouseLocationInArray = PersonSpouseLocationInMarriageRow(MergePerson_row, iMergePersonID, ref iMergePersonLocationInArray);
                bool bFoundDuplicate = false;
                foreach (DataRow Person_row in PersonMarriages_tbl.Rows)
                {
                    if (Person_row.RowState == DataRowState.Unchanged ||
                        Person_row.RowState == DataRowState.Modified)
                    {
                        int iPersonLocationInArray = 0;
                        int iSpouseLocationInArray = PersonSpouseLocationInMarriageRow(Person_row, iPersonID, ref iPersonLocationInArray);
                        if (MergePerson_row[iMergeSpouseLocationInArray].ToInt() == Person_row[iSpouseLocationInArray].ToInt())
                            bFoundDuplicate = true;
                    }
                }
                if (!bFoundDuplicate)
                {
                    DataRow NewRow = PersonMarriages_tbl.NewRow();
                    copyRowValues(PersonMarriages_tbl, NewRow, MergePerson_row);
                    NewRow[PersonID_col] = iPersonID;
                    PersonMarriages_tbl.Rows.Add(NewRow);
                }
            }
            return PersonMarriages_tbl;
        }
        //****************************************************************************************************************************
        private static int PersonSpouseLocationInMarriageRow(DataRow Person_row,
                                                      int iPersonID,
                                                      ref int iSpouseLocationInArray)
        {
            if (Person_row[U.PersonID_col].ToInt() == iPersonID)
            {
                iSpouseLocationInArray = 0;
                return 1;
            }
            else
            {
                iSpouseLocationInArray = 1;
                return 0;
            }
        }
        //****************************************************************************************************************************
        public static void copyRowValues(this DataTable valueTable,
                                          DataRow newRow,
                                          DataRow mergeRow)
        {
            DataColumnCollection columns = valueTable.Columns;
            int numColumns = columns.Count;
            for (int i = 0; i < numColumns; i++)
            {
                string columnName = columns[i].ToString();
                newRow[columnName] = mergeRow[columnName];
            }
        }
        //****************************************************************************************************************************
        public static EVitalRecordType ToVitalRecordType(this Object svalue)
        {
            return (EVitalRecordType)svalue.ToInt();
        }
        //****************************************************************************************************************************
        public static char ToChar(this Object svalue)
        {
            return svalue.ToString()[0];
        }
        //****************************************************************************************************************************
        public static bool ToBool(this Object svalue)
        {
            return svalue.ToInt() != 0;
        }
        //****************************************************************************************************************************
        public static int BornYear(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            if (value.Length < 4)
            {
                return 0;
            }
            if (value.Length == 4)
            {
                return value.ToInt();
            }
            //const char slash = '\\'; 
            if (value[4] != '/')
            {
                return 0;
            }
            return value.Substring(0, 4).ToInt();
        }
        //****************************************************************************************************************************
        public static bool IsNullOrEmpty(this DataTable tbl)
        {
            return (tbl == null || tbl.Rows.Count == 0);
        }
        //****************************************************************************************************************************
        public static int ToInt(this bool value)
        {
            return (value == true) ? 1 : 0;
        }
        //****************************************************************************************************************************
        public static int ToInt(this int ivalue)
        {
            return ivalue;
        }
        //****************************************************************************************************************************
        public static int ToInt(this Object svalue)
        {
            int iInt = ToIntNoError(svalue);
            if (iInt == Exception)
            {
//                MessageBox.Show("Invalid Numeric Value: " + svalue);
                return 0;
            }
            return iInt;
        }
        //****************************************************************************************************************************
        public static Int64 ToInt64(this Object svalue)
        {
            if (svalue == null)
                return 0;
            string sString = svalue.ToString();
            if (sString.Length == 0)
                return 0;
            try
            {
                return Int64.Parse(sString);
            }
            catch
            {
                return Exception;
            }
        }
        //****************************************************************************************************************************
        public static string BlankIfZero(this Object svalue)
        {
            return (svalue.ToInt() == 0) ? "" : svalue.ToString();
        }
        //****************************************************************************************************************************
        public static int ToIntNoError(this Object svalue)
        {
            if (svalue == null)
                return 0;
            string sString = svalue.ToString();
            if (sString.Length == 0)
                return 0;
            try
            {
                return int.Parse(sString);
            }
            catch
            {
                return Exception;
            }
        }
        //****************************************************************************************************************************
        public static string TwoCharNumberBlankIfZero(this object obj)
        {
            string sString = obj.ToString();
            if (sString.ToInt() == 0)
            {
                return "";
            }
            return obj.TwoCharNumber();
        }
        //****************************************************************************************************************************
        public static string TwoCharNumber(this object obj)
        {
            string sString = obj.ToString();
            if (sString.Length == 1)
            {
                sString = "0" + sString;
            }
            return sString;
        }
        //****************************************************************************************************************************
        public static string AbbreviatedDeathInfo(EVitalRecordType vitalRecordType,
                                                  string sCemeteryName,
                                                  string sLotNumber,
                                                  string sBornDate,
                                                  int iAgeYears,
                                                  int iAgeMonths,
                                                  int iAgeDays)
        {
            string returnString = "";
            if (sCemeteryName.Length != 0)
            {
                returnString = sCemeteryName + LotNumber(sLotNumber) + " ";
            }
            if (!vitalRecordType.IsMarriageRecord() && (iAgeYears > 0 || iAgeMonths > 0 || iAgeDays > 0))
            {
                returnString += " Age ";
                if (iAgeMonths == 0 && iAgeDays == 0)
                {
                    returnString += iAgeYears.ToString() + " Years ";
                }
                else
                {
                    if (iAgeYears > 0)
                        returnString += iAgeYears.ToString() + "Y ";
                }
                if (iAgeMonths > 0)
                    returnString += iAgeMonths.ToString() + "M ";
                if (iAgeDays > 0)
                    returnString += iAgeDays.ToString() + "D ";
            }
            else if (!String.IsNullOrEmpty(sBornDate))
            {
                returnString += " Born " + sBornDate;
            }
            return returnString.Trim();
        }
        //****************************************************************************************************************************
        private static string LotNumber(string sLotNumber)
        {
            if (sLotNumber.Length == 0)
            {
                return "";
            }
            if (!sLotNumber.Contains("Section"))
            {
                return ", Lot " + sLotNumber;
            }
            else
            {
                return sLotNumber.Replace("Section.", " Section");
            }
        }
        //****************************************************************************************************************************
        public static string DeathInfo(string sCemeteryName,
                                        string sLotNumber,
                                        int iAgeYears,
                                        int iAgeMonths,
                                        int iAgeDays)
        {
            string sReturnString = "";
            if (sCemeteryName.Length != 0)
            {
                sReturnString = sCemeteryName + " Cemetery";
                if (sLotNumber.Length != 0)
                {
                    sReturnString += ", Lot " + sLotNumber;
                }
            }
            if (iAgeYears > 0 || iAgeMonths > 0 || iAgeDays > 0)
            {
                sReturnString += " Age ";
                if (iAgeYears > 0)
                    sReturnString += iAgeYears.ToString() + " Yrs. ";
                if (iAgeMonths > 0)
                    sReturnString += iAgeMonths.ToString() + " Mos. ";
                if (iAgeDays > 0)
                    sReturnString += iAgeDays.ToString() + " Days ";
            }
            return sReturnString;
        }
        //****************************************************************************************************************************
        public static string AgeString(int iAgeYears,
                                       int iAgeMonths,
                                       int iAgeDays)
        {
            if (iAgeYears > 0 || iAgeMonths > 0 || iAgeDays > 0)
            {
                string sReturnString = " Age ";
                if (iAgeYears > 0)
                    sReturnString += iAgeYears.ToString() + " Yrs. ";
                if (iAgeMonths > 0)
                    sReturnString += iAgeMonths.ToString() + " Mos. ";
                if (iAgeDays > 0)
                    sReturnString += iAgeDays.ToString() + " Days ";
                return sReturnString;
            }
            return "";
        }
        //****************************************************************************************************************************
        public static string BuildDateMDY(int iYear,
                                       int iMonth,
                                       int iDay)
        {
            string sDate = "";
            if (iMonth > 0)
            {
                sDate += String.Format("{0:00}", iMonth);
                if (iDay > 0)
                {
                    sDate += U.slash + String.Format("{0:00}", iDay);
                    sDate += U.slash;
                }
                else
                    sDate += " of ";
            }
            sDate += String.Format("{0:0000}", iYear);
            return sDate;
        }
        //****************************************************************************************************************************
        public static void SplitDate(string dateStr,
                             out int int1,
                             out int int2,
                             out int int3)
        {
            string[] dateValues = dateStr.Split('/');
            int1 = dateValues[0].ToInt();
            if (dateValues.Length == 3)
            {
                int2 = dateValues[1].ToInt();
                int3 = dateValues[2].ToInt();
            }
            else if (dateValues.Length == 2)
            {
                int2 = dateValues[1].ToInt();
                int3 = 0;
            }
            else
            {
                int2 = 0;
                int3 = 0;
            }
        }
        //****************************************************************************************************************************
        public static int GetBornYearFromSchoolRecord(DataRow SchoolRecord_row)
        {
            if ((SchoolRecord_row[U.SchoolRecordType_col].ToSchoolRecordType() == SQL.SchoolRecordType.teacherType))
            {
                return 0;
            }
            string bornDate = SchoolRecord_row[U.BornDate_col].ToString();
            int schoolYear = SchoolRecord_row[U.Year_col].ToInt();
            int grade = SchoolRecord_row[U.Grade_col].ToInt();
            if (String.IsNullOrEmpty(bornDate) || bornDate.Length < 4)
            {
                return schoolYear - grade - 5;
            }
            return bornDate.Substring(0, 4).ToInt();
        }
        //****************************************************************************************************************************
        public static string BuildDate(int iYear,
                                       int iMonth,
                                       int iDay)
        {
            if (iYear < 1000)
            {
                return "";
            }
            string sDate = String.Format("{0:0000}", iYear);
            if (iMonth > 0)
                sDate += U.slash + String.Format("{0:00}", iMonth);
            if (iDay > 0)
                sDate += U.slash + String.Format("{0:00}", iDay);
            return sDate;
        }
        //****************************************************************************************************************************
        public static void GetAge(string bornDate, 
                                  string DiedDate, 
                              out int iAgeYears,
                              out int iAgeMonths,
                              out int iAgeDays)
        {
            int bornYear, bornMonth, bornDay, diedYear, diedMonth, diedDay;
            U.SplitDate(bornDate, out bornYear, out bornMonth, out bornDay);
            U.SplitDate(DiedDate, out diedYear, out diedMonth, out diedDay);
            iAgeYears = diedYear - bornYear;
            if (diedMonth == 0 || bornMonth == 0)
            {
                iAgeMonths = 0;
                iAgeDays = 0;
            }
            else
            {
                iAgeMonths = diedMonth - bornMonth;
                if (iAgeMonths < 0)
                {
                    iAgeMonths += 12;
                    iAgeYears--; 
                }
                if (diedDay == 0 || bornDay == 0)
                {
                    iAgeDays = 0;
                }
                else
                {
                    iAgeDays = diedDay - bornDay;
                    if (iAgeDays < 0)
                    {
                        iAgeMonths--;
                        iAgeDays += 31;
                    }
                }
            }
        }
        //****************************************************************************************************************************
        public static string VitalRecordBornDate(EVitalRecordType eVitalRecordType, DataRow vitalRecord_row, string personBornDate)
        {
            if (eVitalRecordType == EVitalRecordType.eBirthFemale || eVitalRecordType == EVitalRecordType.eBirthMale)
            {
                return U.BuildDate(vitalRecord_row[U.DateYear_col].ToInt(), vitalRecord_row[U.DateMonth_col].ToInt(), vitalRecord_row[U.DateDay_col].ToInt());
            }
            else
            {
                return U.BornDateFromDiedDateMinusAge(vitalRecord_row[U.DateYear_col].ToInt(),
                                                   vitalRecord_row[U.DateMonth_col].ToInt(),
                                                   vitalRecord_row[U.DateDay_col].ToInt(),
                                                   vitalRecord_row[U.AgeYears_col].ToInt(),
                                                   vitalRecord_row[U.AgeMonths_col].ToInt(),
                                                   vitalRecord_row[U.AgeDays_col].ToInt(),
                                                   personBornDate);
            }
        }
        //****************************************************************************************************************************
        public static bool ValidDate(int iYear, int iMonth, int iDay, int greatestPossibleYear = 0)
        {
            if (iYear < 1750 || iYear > greatestPossibleYear)
            {
                return false;
            }
            if (iMonth < 0 || iMonth > 12)
            {
                return false;
            }
            int iLastDayOfMonth = LastDayOfMonth(iMonth, iYear);
            if (iDay < 0 || iDay > iLastDayOfMonth)
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static void BornDateFromDiedDateMinusAge(int iDiedYear,
                                                          int iDiedMonth,
                                                          int iDiedDay,
                                                          int iAgeYears,
                                                          int iAgeMonths,
                                                          int iAgeDays,
                                                      out int bornYear,
                                                      out int bornMonth,
                                                      out int bornDay)
        {
            if (iAgeYears == 0 && iAgeMonths == 0 && iAgeDays == 0)
            {
                bornYear = 0;
                bornMonth = 0;
                bornDay = 0;
                return;
            }
            bornDay = iDiedDay - iAgeDays;
            bornYear = iDiedYear - iAgeYears;
            bornMonth = iDiedMonth - iAgeMonths;
            if (bornMonth <= 0)
            {
                bornYear--;
                bornMonth += 12;
            }
            if (bornDay <= 0)
            {
                bornMonth--;
                if (bornMonth <= 0)
                {
                    bornYear--;
                    bornMonth += 12;
                }
                bornDay = LastDayOfMonth(bornMonth, bornYear) + bornDay;
            }
            if (iAgeDays != 0 && bornDay == iDiedDay)
            {
                bornDay++;
            }
        }
        //****************************************************************************************************************************
        public static string BornDateFromDiedDateMinusAge(int iDiedYear,
                                                          int iDiedMonth,
                                                          int iDiedDay,
                                                          int iAgeYears,
                                                          int iAgeMonths,
                                                          int iAgeDays,
                                                          string personBornDate)
        {
            if (iAgeMonths == 0 && iAgeDays == 0)
            {
                if (personBornDate.Length == 10) // personBornDate would be more accurate 
                {
                    return personBornDate;
                }
                if (iAgeYears == 0)
                {
                    return "";
                }
                return (iDiedYear - iAgeYears).ToString();
            }
            int bornYear; int bornMonth; int bornDay;
            BornDateFromDiedDateMinusAge(iDiedYear, iDiedMonth, iDiedDay, iAgeYears, iAgeMonths, iAgeDays,
                                         out bornYear, out bornMonth, out bornDay);
            return BuildDate(bornYear, bornMonth, bornDay);
        }
        //****************************************************************************************************************************
        public static string BornDateFromDiedDateMinusAgeNoDay(int iDiedYear,
                                                          int iDiedMonth,
                                                          int iAgeYears,
                                                          int iAgeMonths,
                                                          int iAgeDays)
        {
            int iYear = iDiedYear - iAgeYears;
            int iMonth = iDiedMonth - iAgeMonths;
            if (iAgeDays > 14)
            {
                iMonth++;
                if (iMonth > 12)
                {
                    iMonth = 1;
                    iYear--;
                }
            }
            if (iMonth <= 0)
            {
                iYear--;
                iMonth += 12;
            }
            return BuildDate(iYear, iMonth, 0);
        }
        //****************************************************************************************************************************
        public static int LastDayOfMonth(int month, int year)
        {
            switch (month)
            {
                case 2:
                    if (year % 4 == 0)
                    {
                        if (year % 100 == 0 && year % 400 != 0)
                            return 28;
                        else
                            return 29;
                    }
                    else
                        return 28;
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    return 31;
            }
        }
        //****************************************************************************************************************************
        public static EVitalRecordType SpouseRecordType(this EVitalRecordType eVitalRecordType)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eCivilUnionPartyA:
                    return EVitalRecordType.eCivilUnionPartyB;
                case EVitalRecordType.eCivilUnionPartyB:
                    return EVitalRecordType.eCivilUnionPartyA;
                case EVitalRecordType.eMarriageBride:
                    return EVitalRecordType.eMarriageGroom;
                case EVitalRecordType.eMarriageGroom:
                    return EVitalRecordType.eMarriageBride;
                default:
                    return EVitalRecordType.eSearch;
            }
        }
        //****************************************************************************************************************************
        public static EVitalRecordType OppositeRecordType(this EVitalRecordType eVitalRecordType)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eBirthMale:
                    return EVitalRecordType.eBirthFemale;
                case EVitalRecordType.eBirthFemale:
                    return EVitalRecordType.eBirthMale;
                case EVitalRecordType.eDeathMale:
                    return EVitalRecordType.eDeathFemale;
                case EVitalRecordType.eDeathFemale:
                    return EVitalRecordType.eDeathMale;
                case EVitalRecordType.eBurial:
                    return EVitalRecordType.eSearch;
                default:
                    return SpouseRecordType(eVitalRecordType);
            }
        }
        //****************************************************************************************************************************
        public static bool IsBirthOrDeathRecord(this EVitalRecordType eVitalRecordType)
        {
            if (IsBirthRecord(eVitalRecordType))
            {
                return true;
            }
            return IsDeathRecord(eVitalRecordType);
        }
        //****************************************************************************************************************************
        public static bool IsBirthRecord(this EVitalRecordType eVitalRecordType)
        {
            return eVitalRecordType == EVitalRecordType.eBirthFemale ||
                   eVitalRecordType == EVitalRecordType.eBirthMale;
        }
        //****************************************************************************************************************************
        public static bool IsDeathRecord(this EVitalRecordType eVitalRecordType)
        {
            return eVitalRecordType == EVitalRecordType.eDeathFemale ||
                   eVitalRecordType == EVitalRecordType.eDeathMale;
        }
        //****************************************************************************************************************************
        public static bool MarriageRecord(this EVitalRecordType eVitalRecordType)
        {
            return (SpouseRecordType(eVitalRecordType) != EVitalRecordType.eSearch);
        }
        //****************************************************************************************************************************
        public static string TrimString(this object obj)
        {
//            string sString = obj.ToString().Replace("'", "''");
            return obj.ToString().Trim();
        }
        //****************************************************************************************************************************
        public static string CheckStringLength(this string sString,
                                               int iMaxLength)
        {
            if (sString.Length > iMaxLength)
                return sString.Substring(0, iMaxLength);
            else
                return sString;
        }
        //****************************************************************************************************************************
        public static string SetPrefixForDatabase(this object obj)
        {
            return obj.TrimString();
        }
        //****************************************************************************************************************************
        public static string SetSuffixForDatabase(this object obj)
        {
            return obj.TrimString();
        }
        //****************************************************************************************************************************
        public static string Notes(this object obj)
        {
            string sNotes = obj.ToString().Replace(@"""", "'").Trim();
            sNotes = sNotes.Replace("\n", " ~ ").Trim();
            int iLastChar = sNotes.Length - 1;
            if (iLastChar > 0 && sNotes[iLastChar] == '-')
                return sNotes.Remove(iLastChar, 1);
            else
                return sNotes;
        }
        //****************************************************************************************************************************
        public static string AllCharsToUpper(string sString)
        {
            return sString.ToUpper();
        }
        //****************************************************************************************************************************
        public static string SetNameForDatabase(this object obj)
        {
            string sName = obj.TrimString();
            sName = sName.Replace(".", "");
            sName = sName.Replace(",", "");
            if (sName.Length == 0)
                return "";
            char uChar = Char.ToUpper(sName[0]);
            if (sName.Length == 1)
                return uChar.ToString();
            else
            if (uChar != sName[0])
                sName = uChar.ToString() + sName.Substring(1);
            return CheckForApostrophe(sName);
        }
        //****************************************************************************************************************************
        private static string CheckForApostrophe(string name)
        {
            int index = name.IndexOf("'");
            if (index > 0 && index < name.Length)
            {
                char uChar = Char.ToUpper(name[index+1]);
                int startIndex = index + 2;
                return name.Substring(0, index + 1) + uChar + name.Substring(startIndex, name.Length - startIndex);
            }
            return name;
        }
        //****************************************************************************************************************************
        public static string TableAndColumn(string sTable,
                                            string sColumn)
        {
            return sTable + U.dot + sColumn;
        }
        //****************************************************************************************************************************
        public static string IsChecked(bool bChecked)
        {
            if (bChecked)
                return "Y";
            else
                return "N";
        }
        //****************************************************************************************************************************
        public static DataTable CemeteryList()
        {
            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Cemetery", typeof(string)));
            foreach (Cemetery cemetery in Enum.GetValues(typeof(Cemetery)))
            {
                DataRow row = list.NewRow();
                row[0] = (cemetery == Cemetery.SeventhDayAdventist_Cemetery) ? "7th Day Advent" : CemeteryName(cemetery); ;
                list.Rows.Add(row);
            }
            return list;
        }
        //****************************************************************************************************************************
        public static Cemetery GetCemetery(string cemeteryStr)
        {
            cemeteryStr = cemeteryStr.Replace(" ", "") + "_Cemetery";
            if (cemeteryStr.Contains("7th"))
            {
                return Cemetery.SeventhDayAdventist_Cemetery;
            }
            foreach (Cemetery cemetery in Enum.GetValues(typeof(Cemetery)))
            {
                if (cemetery.ToString() == cemeteryStr)
                {
                    return cemetery;
                }
            }
            return Cemetery._Cemetery;
        }
        //****************************************************************************************************************************
        public static string CemeteryName(Cemetery cemetery)
        {
            if (cemetery == Cemetery.SeventhDayAdventist_Cemetery)
            {
                return "7th Day Advent";
            }
            string cemeteryName = cemetery.ToString();
            cemeteryName = cemeteryName.Replace("_Cemetery", "");
            
            return cemeteryName.InsertSpaceBeforeCaps();
        }
        public static string InsertSpaceBeforeCaps(this string str)
        {
            var index = str.Length;
            while (index > 2)
            {
                index--;
                char c = str[index];
                char prevC = str[index - 1];
                if (Char.IsUpper(c) || IsNumericButNotSpecial(c, prevC))
                {
                    str = str.Insert(index, " ");
                }
            }
            return str;
        }
        private static bool IsNumericButNotSpecial(char c, char prevC)
        {
            if (!Char.IsNumber(c))
            {
                return false;
            }
            return true;
        }
        //****************************************************************************************************************************
        public static bool Modified(string sNewValue,
                             DataTable tbl,
                             string sDefaultValue,
                             string sColumnName)
        {
            if (tbl.Rows.Count == 0)
                return (sNewValue != sDefaultValue);
            else
            {
                DataRow row = tbl.Rows[0];
                string sString = row[sColumnName].ToString();
                return (sString.TrimString().ToLower() != sNewValue.TrimString().ToLower());
            }
        }
        //****************************************************************************************************************************
        public static bool ModifiedCheckBox(string sNewValue,
                                     DataTable tbl,
                                     string sDefaultValue,
                                     string sColumnName)
        {
            if (tbl.Rows.Count == 0)
                return (sNewValue != sDefaultValue);
            else
            {
                DataRow row = tbl.Rows[0];
                string sString = row[sColumnName].ToString();
                if (sString.Length == 0 || sString == " ")
                    sString = "N";
                return (TrimString(sString) != TrimString(sNewValue));
            }
        }
        //****************************************************************************************************************************
        public static string VitalRecordTypeToString(this EVitalRecordType eVitalRecordType)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eBirthFemale: return "Birth-Female";
                case EVitalRecordType.eBirthMale: return "Birth-Male";
                case EVitalRecordType.eDeathFemale: return "Death-Female";
                case EVitalRecordType.eDeathMale: return "Death-Male";
                case EVitalRecordType.eBurial: return "Burial";
                case EVitalRecordType.eMarriageBride: return "Marriage-Bride";
                case EVitalRecordType.eMarriageGroom: return "Marriage-Groom";
                case EVitalRecordType.eCivilUnionPartyA: return "Marriage-Party A";
                case EVitalRecordType.eCivilUnionPartyB: return "Marriage-Party B";
                case EVitalRecordType.eSearch: return "Person";
                case EVitalRecordType.eIntegrateAll: return "Cemetery";
                case EVitalRecordType.eSchool: return "School";
                default: return "";
            }
        }
        //****************************************************************************************************************************
        public static string RecordTypeTitle(this EVitalRecordType eVitalRecordType)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eMarriageGroom: return "Groom";
                case EVitalRecordType.eMarriageBride: return "Bride";
                case EVitalRecordType.eCivilUnionPartyA:
                case EVitalRecordType.eCivilUnionPartyB: return "Partner";
                case EVitalRecordType.eBurial:
                case EVitalRecordType.eBirthMale:
                case EVitalRecordType.eBirthFemale:
                case EVitalRecordType.eDeathMale:
                case EVitalRecordType.eDeathFemale:
                default: return "Person";
            }
        }
        //****************************************************************************************************************************
        public static string GridTitle(this EVitalRecordType eVitalRecordType)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eMarriageGroom: return "Groom ";
                case EVitalRecordType.eMarriageBride: return "Bride ";
                case EVitalRecordType.eCivilUnionPartyA: return "PartnerA ";
                case EVitalRecordType.eCivilUnionPartyB: return "PartnerB ";
                case EVitalRecordType.eBurial:
                case EVitalRecordType.eBirthMale:
                case EVitalRecordType.eBirthFemale:
                case EVitalRecordType.eDeathMale:
                case EVitalRecordType.eDeathFemale:
                default: return "";
            }
        }
        //****************************************************************************************************************************
        public static char RecordTypeSexChar(this EVitalRecordType eVitalRecordType,
                                             char oldRecordSex)
        {
            eSex Sex = eVitalRecordType.RecordTypeSex();
            if (Sex == eSex.eMale)
                return 'M';
            else if (Sex == eSex.eFemale)
                return 'F';
            else
                return oldRecordSex;
        }
        //****************************************************************************************************************************
        public static eSex SpouseSex(this eSex sex)
        {
            return (sex == eSex.eMale) ? eSex.eFemale : eSex.eMale;
        }
        //****************************************************************************************************************************
        public static string RecordTypeSex(this EVitalRecordType eVitalRecordType, string defaultChar, string FirstName)
        {
            eSex sex = RecordTypeSex(eVitalRecordType);
            if (sex == eSex.eMale)
                return "M";
            else if (sex == eSex.eFemale)
                return "F";
            else
            {
                if (String.IsNullOrEmpty(defaultChar.Trim()))
                {
                    //return SQL.GetFirstNameSex(FirstName).ToString();
                }
                return defaultChar;
            }
        }
        //****************************************************************************************************************************
        public static eSex RecordTypeSex(this EVitalRecordType eVitalRecordType, char sex = ' ')
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eDeathMale:
                case EVitalRecordType.eBirthMale:
                case EVitalRecordType.eMarriageGroom: return eSex.eMale;
                case EVitalRecordType.eBirthFemale:
                case EVitalRecordType.eDeathFemale:
                case EVitalRecordType.eMarriageBride: return eSex.eFemale; ;
                default:
                case EVitalRecordType.eBurial:
                case EVitalRecordType.eCivilUnionPartyA:
                case EVitalRecordType.eCivilUnionPartyB:
                    {
                        if (sex == 'M')
                            return eSex.eMale;
                        else if (sex == 'F')
                            return eSex.eFemale;
                        else
                            return eSex.eUnknown;
                    }
            }
        }
        //****************************************************************************************************************************
        private static DataRow GetVitalRecordFromTable(DataTable VitalRecord_tbl,
                                                       EVitalRecordType eVitalRecordType)
        {
            foreach (DataRow row in VitalRecord_tbl.Rows)
            {
                EVitalRecordType eRecordVitalRecordType = (EVitalRecordType)row[U.VitalRecordType_col].ToInt();
                if (eVitalRecordType == eRecordVitalRecordType)
                    return row;
            }
            return null;
        }
        //****************************************************************************************************************************
        public static DataRow VitalRecordRow(DataTable        VitalRecord_tbl,
                                              EVitalRecordType eVitalRecordType1,
                                              EVitalRecordType eVitalRecordType2)
        {
            DataRow row = GetVitalRecordFromTable(VitalRecord_tbl, eVitalRecordType1);
            if (row == null && eVitalRecordType1 != EVitalRecordType.eBurial)
                row = GetVitalRecordFromTable(VitalRecord_tbl, eVitalRecordType2);
            return row;
        }
        //****************************************************************************************************************************
        public static bool PersonFemale(object Obj)
        {
            string sSex = Obj.ToString();
            if (sSex.Length == 0)
                return false;
            else
            if (sSex[0] == 'F')
                return true;
            else
                return false;
        }
        //****************************************************************************************************************************
        public static void SetValuesToPersonInfo(DataRow Person_row,
                                           EVitalRecordType eVitalRecordType,
                                           ref string sBook,
                                           ref string sPage,
                                           ref string sDate,
                                           ref string sPlace,
                                           ref string sHome,
                                           ref string sSource)
        {
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eBirthMale:
                case EVitalRecordType.eBirthFemale:
                    sBook = Person_row[BornBook_col].ToStringSpacesWhenZero();
                    sPage = Person_row[BornPage_col].ToStringSpacesWhenZero();
                    sDate = Person_row[BornDate_col].ToString();
                    sPlace = Person_row[BornPlace_col].ToString();
                    sHome = Person_row[BornHome_col].ToString();
                    sSource = Person_row[BornSource_col].ToString();
                    break;
                case EVitalRecordType.eDeathMale:
                case EVitalRecordType.eDeathFemale:
                    sBook = Person_row[DiedBook_col].ToStringSpacesWhenZero();
                    sPage = Person_row[DiedPage_col].ToStringSpacesWhenZero();
                    sDate = Person_row[DiedDate_col].ToString();
                    sPlace = Person_row[DiedPlace_col].ToString();
                    sHome = Person_row[DiedHome_col].ToString();
                    sSource = Person_row[DiedSource_col].ToString();
                    break;
                case EVitalRecordType.eBurial:
                    sBook = Person_row[BuriedBook_col].ToStringSpacesWhenZero();
                    sPage = Person_row[BuriedPage_col].ToStringSpacesWhenZero();
                    sDate = Person_row[BuriedDate_col].ToString();
                    sPlace = Person_row[BuriedPlace_col].ToString();
                    sHome = Person_row[BuriedStone_col].ToString();
                    sSource = Person_row[BuriedSource_col].ToString();
                    break;
                default: break;
            }
        }
        //****************************************************************************************************************************
        public static string DirectoryFromFullFilename(this string sFileName)
        {
            char[] c = new char[1];
            c[0] = '\\';
            int iLocationOfLastSlash = sFileName.LastIndexOfAny(c);
            return sFileName.Substring(0, iLocationOfLastSlash + 1);
        }
        //****************************************************************************************************************************
        public static string FileNameFromFullFilename(this string sFileName)
        {
            char[] c = new char[1];
            c[0] = '\\';
            int iLocationOfLastSlash = sFileName.LastIndexOfAny(c);
            c[0] = '.';
            int iLocationOfLastPoint = sFileName.LastIndexOfAny(c);
            int iLength = iLocationOfLastPoint - iLocationOfLastSlash - 1;
            if (iLength <= 0)
                return "";
            else
            {
                string sReturnString = sFileName.Substring(iLocationOfLastSlash + 1, iLength);
                if (sReturnString.Substring(0, 2) == "HF")
                    return "";
                else
                    return sReturnString;
            }
        }
        //****************************************************************************************************************************
        public static string FileNameWithExtension(string sFileName,
                                                   string sFolder,
                                                   string sExtension)
        {
            string sTableFileName = sFileName + sExtension;
            return sFolder + sTableFileName;
        }
        //****************************************************************************************************************************
        public static bool GetPersonVitalStatistics(DataTable VitalRecord_tbl,
                                 DataTable CemeteryRecord_tbl,
                                 DataRow Person_row,
                                 EVitalRecordType eVitalRecordType1,
                                 EVitalRecordType eVitalRecordType2,
                                 ref string sBook,
                                 ref string sPage,
                                 ref string sDate,
                                 ref string sPlace,
                                 ref string sHome,
                                 ref string sSource,
                                 bool includeMarriageRecords = true)
        {
            DataRow vitalRecord_row = VitalRecordRow(VitalRecord_tbl, eVitalRecordType1, eVitalRecordType2);
            if (vitalRecord_row == null)
            {
                bool fromIndirect = GetVitalInfoIndirectly(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, eVitalRecordType1, ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource, includeMarriageRecords);
                if (String.IsNullOrEmpty(sDate) && Person_row != null)
                {
                    SetValuesToPersonInfo(Person_row, eVitalRecordType1, ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
                    return false;
                }
                return fromIndirect;
            }
            GetFromVitalRecord(vitalRecord_row, ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
            return true;
        }
        //****************************************************************************************************************************
        private static void GetFromVitalRecord(DataRow vitalRecord_row,
                                                 ref string sBook,
                                                 ref string sPage,
                                                 ref string sDate,
                                                 ref string sPlace,
                                                 ref string sHome,
                                                 ref string sSource)
        {
            EVitalRecordType eVitalRecordType = (EVitalRecordType)vitalRecord_row[U.VitalRecordType_col].ToInt();
            sBook = vitalRecord_row[U.Book_col].ToString();
            sPage = vitalRecord_row[U.Page_col].ToString();
            sDate = U.BuildDate(vitalRecord_row[U.DateYear_col].ToInt(), vitalRecord_row[U.DateMonth_col].ToInt(), vitalRecord_row[U.DateDay_col].ToInt());
            if (eVitalRecordType == EVitalRecordType.eBurial)
            {
                sPlace = vitalRecord_row[U.CemeteryName_col].ToString();
                sHome = vitalRecord_row[U.LotNumber_col].ToString();
            }
            else
            {
                sPlace = "Jamaica";
                sHome = "Jamaica";
            }
            switch (eVitalRecordType)
            {
                case EVitalRecordType.eBirthFemale:
                case EVitalRecordType.eBirthMale: sSource = "Birth Record"; break;
                case EVitalRecordType.eDeathFemale:
                case EVitalRecordType.eDeathMale: sSource = "Death Record"; break;
                case EVitalRecordType.eBurial: sSource = "Burial Record"; break;
                default: sSource = "Marriage Record"; break;
            }
        }
        //****************************************************************************************************************************
        private static bool GetVitalInfoIndirectly(DataTable VitalRecord_tbl,
                                                     DataTable CemeteryRecord_tbl,
                                                     DataRow Person_row,
                                                     EVitalRecordType eVitalRecordType,
                                                     ref string sBook,
                                                     ref string sPage,
                                                     ref string sDate,
                                                     ref string sPlace,
                                                     ref string sHome,
                                                     ref string sSource,
                                                     bool includeMarriageRecords)
        {
            if (eVitalRecordType == EVitalRecordType.eBirthFemale || eVitalRecordType == EVitalRecordType.eBirthMale && Person_row != null)
            {
                string CalculatedDate = GetBornDateFromOtherVitalRecords(VitalRecord_tbl, CemeteryRecord_tbl, Person_row, sDate, ref sSource, includeMarriageRecords);
                if (!string.IsNullOrEmpty(CalculatedDate))
                {
                    sDate = CalculatedDate;
                }
                return !string.IsNullOrEmpty(CalculatedDate);
            }
            if (CemeteryRecord_tbl.Rows.Count > 0 && !String.IsNullOrEmpty(CemeteryRecord_tbl.Rows[0][U.DiedDate_col].ToString()))
            {
                DataRow cemeteryRecord_row = CemeteryRecord_tbl.Rows[0];
                sDate = cemeteryRecord_row[U.DiedDate_col].ToString();
                sSource = U.JamaicaCemeteryRecords;
                if (eVitalRecordType == EVitalRecordType.eBurial)
                {
                    sPlace = CemeteryName((Cemetery)cemeteryRecord_row[U.CemeteryID_col].ToInt());
                    sHome = cemeteryRecord_row[U.LotNumber_col].ToString();
                }
                return !string.IsNullOrEmpty(sDate);
            }
            if (eVitalRecordType == EVitalRecordType.eDeathFemale || eVitalRecordType == EVitalRecordType.eDeathMale)
            {
                DataRow vitalRecord_row = VitalRecordRow(VitalRecord_tbl, EVitalRecordType.eBurial, EVitalRecordType.eBurial);
                if (vitalRecord_row != null)
                {
                    GetFromVitalRecord(vitalRecord_row, ref sBook, ref sPage, ref sDate, ref sPlace, ref sHome, ref sSource);
                    sSource = "From Buried Record";
                }
            }
            return !string.IsNullOrEmpty(sDate);
        }
        //****************************************************************************************************************************
        private static string GetBornDateFromMarriageRecord(DataTable VitalRecord_tbl)
        {
            foreach (DataRow row in VitalRecord_tbl.Rows)
            {
                EVitalRecordType eRecordVitalRecordType = (EVitalRecordType)row[U.VitalRecordType_col].ToInt();
                if (eRecordVitalRecordType.IsMarriageRecord())
                {
                    int bornYear = row[U.AgeYears_col].ToInt();
                    int bornMonth = row[U.AgeMonths_col].ToInt();
                    int bornDay = row[U.AgeDays_col].ToInt();
                    if (bornYear != 0)
                    {
                        return BuildDate(bornYear, bornMonth, bornDay);
                    }
                }
            }
            return "";
        }
        //****************************************************************************************************************************
        public static void AgeBetweenTwoDates(int bornYear,
                                              int bornMonth,
                                              int bornDay,
                                              int dateYear,
                                              int dateMonth,
                                              int dateDay,
                                          out int ageYears,
                                          out int ageMonths,
                                          out int ageDays)
        {
            if (!ValidDate(bornYear, bornMonth, bornDay, dateYear))
            {
                string message = "Invalid Date: " + BuildDate(bornYear, bornMonth, bornDay);
                throw new Exception(message);
            }
            ageDays = dateDay - bornDay;
            if (ageDays < 0)
            {
                dateMonth--;
                int ageLastDay = U.NumDaysInMonth(bornYear, bornMonth);
                int dateLastDay = U.NumDaysInMonth(dateYear, dateMonth);
                if (bornDay == ageLastDay)
                {
                    ageDays = dateDay;
                }
                else if (bornDay == dateLastDay)
                {
                    ageDays = dateDay + (ageLastDay - bornDay);
                }
                else
                {
                    if (dateMonth == 0)
                    {
                        dateMonth = 12;
                        dateYear--;
                    }
                    dateDay += ageLastDay;
                    ageDays = dateDay - bornDay;
                }
            }

            ageYears = dateYear - bornYear;
            if (ageYears < 0)
            {
                ageYears = 0;
                ageMonths = 0;
                ageDays = 0;
                MessageBox.Show("Negative Diff");
                return;
            }
            ageMonths = dateMonth - bornMonth;
            if (ageMonths < 0)
            {
                ageYears--;
                ageMonths += 12;
            }
        }
        //****************************************************************************************************************************
        private static string GetBornDateFromOtherVitalRecords(DataTable VitalRecord_tbl,
                                   DataTable CemeteryRecord_tbl,
                                   DataRow personRow,
                                   string sDate,
                               ref string sSource,
                                   bool includeMarriageRecords)
        {
            string personBornDate = sDate;
            if (personRow != null)
            {
                sSource = personRow[U.BornSource_col].ToString();  // default to person source
                personBornDate = personRow[U.BornDate_col].ToString();
            }
            sDate = GetBornDateFromOtherVitalRecords(VitalRecord_tbl, personBornDate, ref sSource, includeMarriageRecords);
            if (String.IsNullOrEmpty(sDate))
            {
                DataRow cemeteryRow = (CemeteryRecord_tbl.Rows.Count == 0) ? null : CemeteryRecord_tbl.Rows[0];
                sDate = GetBornDateFromCemeteryRecord(cemeteryRow, personBornDate, ref sSource);
            }
            if (String.IsNullOrEmpty(sDate))
            {
                if (!String.IsNullOrEmpty(personBornDate))
                {
                    sDate = personBornDate;
                }
                else if (includeMarriageRecords)
                {
                    sDate = ReturnIndirectFromChildRecords(personRow, ref sSource);
                }
            }
            return sDate;
        }
        //****************************************************************************************************************************
        private static string GetBornDateFromOtherVitalRecords(DataTable VitalRecord_tbl,
                                   string personBornDate,
                               ref string sSource,
                                   bool includeMarriageRecords)
        {
            string sDate = "";
            DataRow vitalRecord_row = VitalRecordRow(VitalRecord_tbl, EVitalRecordType.eDeathMale, EVitalRecordType.eDeathFemale);
            if (vitalRecord_row != null)
            {
                sSource = "From Death Record";
                sDate = ReturnIndirectBirthDate(vitalRecord_row, personBornDate);
            }
            else
            {
                vitalRecord_row = VitalRecordRow(VitalRecord_tbl, EVitalRecordType.eBurial, EVitalRecordType.eBurial);
                if (vitalRecord_row != null)
                {
                    sSource = "From Burial Record";
                    sDate = ReturnIndirectBirthDate(vitalRecord_row, personBornDate);
                }
                else if (includeMarriageRecords)
                {
                    vitalRecord_row = VitalRecordRow(VitalRecord_tbl, EVitalRecordType.eMarriageBride, EVitalRecordType.eMarriageGroom);
                    if (vitalRecord_row != null)
                    {
                        sSource = "From Marriage Record";
                        sDate = ReturnIndirectBirthDate(vitalRecord_row, personBornDate, true);
                    }
                    else
                    {
                        vitalRecord_row = VitalRecordRow(VitalRecord_tbl, EVitalRecordType.eCivilUnionPartyA, EVitalRecordType.eCivilUnionPartyB);
                    }
                    if (vitalRecord_row != null)
                    {
                        sSource = "From Marriage Record";
                        sDate = ReturnIndirectBirthDate(vitalRecord_row, personBornDate, true);
                    }
                }
            }
            return sDate;
        }
        //****************************************************************************************************************************
        public static string ReturnIndirectFromChildRecords(DataRow personRow, ref string source)
        {
            int personId = personRow[U.PersonID_col].ToInt();
            char personSex = personRow[U.Sex_col].ToChar();
            DataTable VitalRecord_tbl = new DataTable();
            if (personSex == 'M')
            {
                SQL.GetVitalRecordsForPerson(VitalRecord_tbl, personId, U.FatherID_col);
            }
            else
            {
                SQL.GetVitalRecordsForPerson(VitalRecord_tbl, personId, U.MotherID_col);
            }
            int lowYear = 9999;
            foreach (DataRow vitalRecordRow in VitalRecord_tbl.Rows)
            {
                string vitalRecordDate;
                EVitalRecordType vitalRecordType = (EVitalRecordType) vitalRecordRow[U.VitalRecordType_col].ToInt();
                int offset;
                switch (vitalRecordType)
                {
                    case EVitalRecordType.eBirthMale:
                    case EVitalRecordType.eBirthFemale:
                        {
                            vitalRecordDate = BuildDate(vitalRecordRow[U.DateYear_col].ToInt(), vitalRecordRow[U.DateMonth_col].ToInt(), vitalRecordRow[U.DateDay_col].ToInt());
                            offset = 25;
                            break;
                        }
                    case EVitalRecordType.eDeathMale:
                    case EVitalRecordType.eDeathFemale:
                    case EVitalRecordType.eBurial:
                        {
                            vitalRecordDate = ReturnIndirectBirthDate(vitalRecordRow, "");
                            offset = 25;
                            break;
                        }
                    default:
                        {
                            if (vitalRecordRow[U.AgeYears_col].ToInt() != 0)
                            {
                                vitalRecordDate = ReturnIndirectBirthDate(vitalRecordRow, "");
                                offset = 25;
                            }
                            else
                            {
                                vitalRecordDate = BuildDate(vitalRecordRow[U.DateYear_col].ToInt(), vitalRecordRow[U.DateMonth_col].ToInt(), vitalRecordRow[U.DateDay_col].ToInt());
                                offset = 50;
                            }
                            break;
                        }
                }
                if (vitalRecordDate.Length >= 4)
                {
                    int vitalRecordYear = vitalRecordDate.Substring(0, 4).ToInt() - offset;
                    if (vitalRecordYear < lowYear)
                    {
                        lowYear = vitalRecordYear;
                    }
                }
            }
            return (lowYear == 9999) ? "" : lowYear.ToString() + " about";
        }
        //****************************************************************************************************************************
        public static string ReturnIndirectBirthDate(DataRow vitalRecord_row, string personBornDate, bool fromMarriageRecord=false)
        {
            int ageYears = vitalRecord_row[U.AgeYears_col].ToInt();
            int ageMonths = vitalRecord_row[U.AgeMonths_col].ToInt();
            int ageDays = vitalRecord_row[U.AgeDays_col].ToInt();
            if (ageYears == 0 && ageMonths == 0 && ageDays == 0)
            {
                if (!string.IsNullOrEmpty(personBornDate))
                {
                    return personBornDate;
                }
                if (fromMarriageRecord)
                {
                    return vitalRecord_row[U.DateYear_col].ToInt() - 25 + " about";
                }
                return "";
            }
            return U.BornDateFromDiedDateMinusAge(vitalRecord_row[U.DateYear_col].ToInt(),
                                                   vitalRecord_row[U.DateMonth_col].ToInt(),
                                                   vitalRecord_row[U.DateDay_col].ToInt(),
                                                   ageYears, ageMonths, ageDays, personBornDate);
        }
        //****************************************************************************************************************************
        private static string GetBornDateFromCemeteryRecord(DataRow cemeteryRow, string personBornDate, ref string sSource)
        {
            if (cemeteryRow == null)
            {
                sSource = "";
                return "";
            }
            string sDate = GetCemeteryBornDate(cemeteryRow, personBornDate);
            if (!String.IsNullOrEmpty(sDate) && sDate != personBornDate)
            {
                sSource = "From Cemetery Record";
            }
            return sDate;
        }
        //****************************************************************************************************************************
        public static int GetCemeteryBornYear(DataRow cemeteryRecord_row, string personBornDate)
        {
            string bornDate = GetCemeteryBornDate(cemeteryRecord_row, personBornDate);
            return GetYearFromDate(bornDate);
        }
        //****************************************************************************************************************************
        public static int GetYearFromDate(string date)
        {
            if (date.Length < 4)
            {
                return 0;
            }
            return date.Substring(0, 4).ToInt();
        }
        //****************************************************************************************************************************
        public static string GetCemeteryBornDate(DataRow cemeteryRecord_row, string personBornDate)
        {
            string sDate = "";
            if (!String.IsNullOrEmpty(cemeteryRecord_row[U.BornDate_col].ToString()))
            {
                sDate = cemeteryRecord_row[U.BornDate_col].ToString();
            }
            else if (!String.IsNullOrEmpty(cemeteryRecord_row[U.DiedDate_col].ToString()))
            {
                int ageYears = cemeteryRecord_row[U.AgeYears_col].ToInt();
                int ageMonths = cemeteryRecord_row[U.AgeMonths_col].ToInt();
                int ageDays = cemeteryRecord_row[U.AgeDays_col].ToInt();
                if (ageMonths == 0 && ageDays == 0 && !String.IsNullOrEmpty(personBornDate))
                {
                    sDate = personBornDate;
                }
                else if (ageYears != 0 || ageMonths != 0 || ageDays != 0)
                {
                    int iyear, imonth, iday;
                    U.SplitDate(cemeteryRecord_row[U.DiedDate_col].ToString(), out iyear, out imonth, out iday);
                    sDate = U.BornDateFromDiedDateMinusAge(iyear, imonth, iday, ageYears, ageMonths, ageDays, personBornDate);
                }
            }
            return sDate;
        }
        //****************************************************************************************************************************
        public static int GetPhotoNumber(string sPhotoName,
                                  int iLocationOfNumber)
        {
            return sPhotoName.Substring(iLocationOfNumber).ToIntNoError();
        }
        //****************************************************************************************************************************
        public static FileInfo[] GetAllFilesWithPrefix(string sFolder,
                                                 string sPrefix)
        {
            DirectoryInfo di = new DirectoryInfo(sFolder);
            FileInfo[] rgFiles = di.GetFiles(sPrefix + "*" + U.sJPGExtension);
            Array.Sort(rgFiles, delegate(FileInfo f1, FileInfo f2)
            {
                return f1.Name.CompareTo(f2.Name);
            });
            return rgFiles;
        }
        //****************************************************************************************************************************
        public static string GetNextPhotoNumber(int iPhotoNum)
        {
            if (iPhotoNum <= 0 || iPhotoNum == U.Exception)
                return "";
            string sPhotoName = "";
            bool bFoundNumIfFolder = false;
            do
            {
                iPhotoNum++;
                sPhotoName = "HF" + iPhotoNum.ToString("#000000");
                bFoundNumIfFolder = false; // do not check directory
            } while (bFoundNumIfFolder);
            return sPhotoName;
        }
        //****************************************************************************************************************************
        public static bool CopyOrRenamePhotoFile(ArrayList PhotosToDelete,
                                           ArrayList PhotosCopied,
                                           string sOldFilename,
                                           string sNewFilename)
        {
            if (sOldFilename.Length == 0 || sOldFilename == sNewFilename)
                return true;
            string sOldFolder = sOldFilename.DirectoryFromFullFilename();
            string sNewFolder = sNewFilename.DirectoryFromFullFilename();
            bool bSuccess = false;
            bSuccess = U.CopyFile(sNewFilename, sOldFilename);
            if (bSuccess)
            {
                PhotosToDelete.Add(sOldFilename);
                PhotosCopied.Add(sNewFilename);
            }
            return bSuccess;
        }
        //****************************************************************************************************************************
        public static string AddOneToPhotoName(string sPhotoName)
        {
            return GetNextPhotoNumber(GetPhotoNumber(sPhotoName, 2));
        }
        //****************************************************************************************************************************
        public static string GetGreatestFileInFolder(string sFolder,
                                               string sPrefix)
        {
            FileInfo[] rgFiles = U.GetAllFilesWithPrefix(sFolder, sPrefix);
            Array.Reverse(rgFiles);
            return rgFiles.GetValue(0).ToString();
        }
        //****************************************************************************************************************************
        public static string GetNextFilename(string sNextDBPhotoName,
                                             string sJPGPhotoFolder)
        {
            int iDatabasePhotoNum = U.GetPhotoNumber(sNextDBPhotoName, 2);
            string sFolderPhotoName = U.GetGreatestFileInFolder(sJPGPhotoFolder, "HF");
            int iFolderPhotoNum = U.GetPhotoNumber(sFolderPhotoName.Remove(8), 2);
            if (iFolderPhotoNum >= iDatabasePhotoNum)
            {
                sNextDBPhotoName = U.GetNextPhotoNumber(iFolderPhotoNum);
            }
            return sNextDBPhotoName;
        }
        //****************************************************************************************************************************
        private static string GetShortOrLongMonth(string sDate,
                                string sShortMonth,
                                string sLongMonth,
                                string sOther,
                            ref int iLocation)
        {
            iLocation = sDate.IndexOf(sLongMonth);
            if (iLocation >= 0)
                return sLongMonth;
            if (sOther.Length > 3)
            {
                iLocation = sDate.IndexOf(sOther);
                if (iLocation >= 0)
                    return sOther;
            }
            iLocation = sDate.IndexOf(sShortMonth);
            int iLastChar = iLocation + 3;
            if (iLocation >= 0)
            {
                if (sDate.Length > (iLastChar) && sDate[iLastChar] != ' ' && sDate[iLastChar] != ',')
                {
                    iLocation = -1;
                    return "";
                }
                else
                    return sShortMonth;
            }
            else
                return "";
        }
        //****************************************************************************************************************************
        private static string GetStringMonth(string sDate,
                              int iMonth,
                          ref int iLocation)
        {
            string sValueFound = "";
            switch (iMonth)
            {
                case 01: sValueFound = GetShortOrLongMonth(sDate, "jan", "january", "", ref iLocation); break;
                case 02: sValueFound = GetShortOrLongMonth(sDate, "feb", "february", "", ref iLocation); break;
                case 03: sValueFound = GetShortOrLongMonth(sDate, "mar", "march", "", ref iLocation); break;
                case 04: sValueFound = GetShortOrLongMonth(sDate, "apr", "april", "", ref iLocation); break;
                case 05: sValueFound = GetShortOrLongMonth(sDate, "may", "may", "", ref iLocation); break;
                case 06: sValueFound = GetShortOrLongMonth(sDate, "jun", "june", "", ref iLocation); break;
                case 07: sValueFound = GetShortOrLongMonth(sDate, "jul", "july", "", ref iLocation); break;
                case 08: sValueFound = GetShortOrLongMonth(sDate, "aug", "august", "", ref iLocation); break;
                case 09: sValueFound = GetShortOrLongMonth(sDate, "sep", "september", "sept", ref iLocation); break;
                case 10: sValueFound = GetShortOrLongMonth(sDate, "oct", "october", "", ref iLocation); break;
                case 11: sValueFound = GetShortOrLongMonth(sDate, "nov", "november", "", ref iLocation); break;
                case 12: sValueFound = GetShortOrLongMonth(sDate, "dec", "december", "", ref iLocation); break;
            }
            if (iLocation < 0)
            {
                return "";
            }
            else
            {
                return sValueFound;
            }
        }
        //****************************************************************************************************************************
        private static string LocationOfMonth(string sDate, out int iLocation)
        {
            int iMonth = 0;
            iLocation = -1;
            string sValueFound = "";
            while (iMonth < 12 && iLocation < 0)
            {
                iMonth++;
                sValueFound = GetStringMonth(sDate, iMonth, ref iLocation);
            }
            if (iLocation >= 0)
            {
                sDate = sDate.Replace(sValueFound, iMonth.ToString());
            }
            return sDate;
        }
        //****************************************************************************************************************************
        private static string NonStandardDate(string sOrigDate,
                                              ArrayList numList,
                                              int iArrayNumForCenturyYear)
        {
            if (numList.Count == 1)
                return numList[0].ToString();
            else if (numList.Count == 2)
            {
                if (iArrayNumForCenturyYear == 0)
                    return BuildDateMDY(numList[0].ToInt(), numList[1].ToInt(), 0);
                else
                    return BuildDateMDY(numList[1].ToInt(), numList[0].ToInt(), 0);
            }
            else
                return sOrigDate;
        }
        //****************************************************************************************************************************
        private static string NormalDate(string sOrigDate,
                                         ArrayList numList)
        {
            string sDate;
            if (numList[0].ToInt() > 1000)
            {
                sDate = BuildDateMDY(numList[0].ToInt(), numList[1].ToInt(), numList[2].ToInt());
            }
            else if (numList[2].ToInt() > 1000)
            {
                sDate = BuildDateMDY(numList[2].ToInt(), numList[0].ToInt(), numList[1].ToInt());
            }
            else if (numList[2].ToInt() > 0)
            {
                int iThirdNum = 1900 + numList[2].ToInt();
                sDate = BuildDateMDY(iThirdNum, numList[0].ToInt(), numList[1].ToInt());
            }
            else
            {
                return sOrigDate;
            }
            return sDate;
        }
        //****************************************************************************************************************************
        private static string DateHashDelimiters(string sDate,
                                              char cDelimiter,
                                              int iFirstDelimiterLocation)
        {
            string sOrigDate = sDate;
            ArrayList list = new ArrayList();
            list.Add(sDate.Substring(0, iFirstDelimiterLocation));
            sDate = sDate.Remove(0, iFirstDelimiterLocation + 1).Trim();
            int iSecondDelimiterLocation = sDate.IndexOf(cDelimiter);
            if (iSecondDelimiterLocation >= 0)
            {
                list.Add(sDate.Substring(0, iSecondDelimiterLocation));
                sDate = sDate.Remove(0, iSecondDelimiterLocation + 1).Trim();
            }
            int iAdditionalSpaces = sDate.IndexOf(' ');
            while (iAdditionalSpaces >= 0)
            {
                list.Add(sDate.Substring(0, iAdditionalSpaces));
                sDate = sDate.Remove(0, iAdditionalSpaces + 1).Trim();
                iAdditionalSpaces = sDate.IndexOf(' ');
            }
            list.Add(sDate);
            ArrayList numList = new ArrayList();
            int iArrayNumForCenturyYear = 0;
            bool bFoundNonNumericString = false;
            int i = 0;
            foreach (object obj in list)
            {
                int iNum = obj.ToIntNoError();
                if (iNum <= 0)
                    bFoundNonNumericString = true;
                else if (iNum > 1000 && iNum < 2100)
                {
                    iArrayNumForCenturyYear = i;
                }
                numList.Add(iNum);
                i++;
            }
            if (bFoundNonNumericString)
                return sOrigDate;
            else if (list.Count == 3)
                return NormalDate(sOrigDate, numList);
            else
                return NonStandardDate(sOrigDate, numList, iArrayNumForCenturyYear);
        }
        //****************************************************************************************************************************
        public static EVitalRecordType VitalRecordType(this int vitalRecordType)
        {
            return (EVitalRecordType)vitalRecordType;
        }
        //****************************************************************************************************************************
        public static string toDate(this object obj)
        {
            string sDate = obj.ToString();
            sDate = sDate.Trim().ToLower();
            return sDate;
/*            if (sDate.Length == 0)
                return "";
            string sOrigDate = sDate;
            sDate = sDate.Replace(',', ' ');
            int iMonthLocation;
            sDate = LocationOfMonth(sDate, out iMonthLocation);
            int iFirstDelimiterLocation = sDate.IndexOf('/');
            if (iFirstDelimiterLocation >= 0)
                return DateHashDelimiters(sDate, '/', iFirstDelimiterLocation);
            iFirstDelimiterLocation = sDate.IndexOf(' ');
            if (iFirstDelimiterLocation >= 0)
                return DateHashDelimiters(sDate, ' ', iFirstDelimiterLocation);
            return sDate;*/
        }
        //****************************************************************************************************************************
        public static string OccupantPrintString(int iBuilding1856PersonID,
                                                int iBuilding1869PersonID,
                                                int iBuilding1884PersonID, 
                                                bool mapsOption = false)
        {
            if (iBuilding1856PersonID == 0 && iBuilding1869PersonID == 0 && iBuilding1884PersonID == 0)
                return (mapsOption) ? "" : U.sOccupantString;
            else if (iBuilding1856PersonID == 0 && iBuilding1869PersonID == 0)
                return (mapsOption) ? "" : U.s1884OccupantString;
            else if (iBuilding1856PersonID == 0 && iBuilding1884PersonID == 0)
                return (mapsOption) ? "" : U.s1869OccupantString;
            else if (iBuilding1869PersonID == 0 && iBuilding1884PersonID == 0)
                return (mapsOption) ? U.s1856MapString : U.s1856OccupantString;
            else if (iBuilding1856PersonID == iBuilding1869PersonID && iBuilding1884PersonID == iBuilding1869PersonID)
                return (mapsOption) ? s1869and1856and1884MapString : s1869and1856and1884OccupantString;
            else if (iBuilding1869PersonID == iBuilding1884PersonID)
                return (mapsOption) ? U.s1869and1884MapString : U.s1869and1884OccupantString;
            else if (iBuilding1856PersonID == iBuilding1869PersonID)
                return (mapsOption) ? U.s1869and1856MapString : U.s1869and1856OccupantString;
            else if (iBuilding1856PersonID != 0)
                return (mapsOption) ? U.s1869MapString : U.s1869OccupantString;
            else if (iBuilding1884PersonID != 0)
                return (mapsOption) ? U.s1884MapString : U.s1884OccupantString;
            else
                return (mapsOption) ? s1869MapString : U.s1869OccupantString;
        }
        //****************************************************************************************************************************
        public static string BuildingString(string sInputString,
                                            int iBuildingType,
                                            int personID,
                                            int BuildingID,
                                            int iOccupantYear = 0)
        {
            if (iBuildingType == i1856BuildingName)
                return sInputString + " " + U.s1856BuildingNameString;
            if (iBuildingType == i1869BuildingName)
                return sInputString + " " + U.s1869BuildingNameString;
            if (iBuildingType == iCurrentOwners)
            {
                if (iOccupantYear == 0)
                {
                    return sInputString + " " + U.sCurrentOccupantString;
                }
                else
                {
                    return sInputString + " " + U.sOwnerString.Replace(")", " " + iOccupantYear + ")");
                }
            }
            else if (iBuildingType == iOccupant)
                return sInputString + " " + GetOccupantString(personID, BuildingID);
            else
                return sInputString + " " + U.sBuildingNameString;
        }
        //****************************************************************************************************************************
        public static string GetOccupantString(int personID,
                                            int BuildingID)
        {
            DataTable occupantTbl = SQL.GetBuildingOccupant(personID, BuildingID);
            Census census = new Census();
            if (occupantTbl.Rows.Count != 0)
            {
                DataRow occupantRow = occupantTbl.Rows[0];
                Int64 censusYears = occupantRow[U.CensusYears_col].ToInt64();
                census.LoadCensusData(censusYears);
            }
            return census.OccupantString(personID);
        }
        //****************************************************************************************************************************
        public static string CombineName1AndName2(string sName1,
                                                  string sName2)
        {
            if (sName2.Length == 0)
                return sName1;
            string sReturnString = sName1;
            int iCommaLocation = sName1.IndexOf(',');
            if (iCommaLocation < 0)
            {
                iCommaLocation = sName1.IndexOf(' ');
            }
            if (iCommaLocation > 0)
            {
                string sLastName = sName1.Substring(0, iCommaLocation);
                if (sName2.Length > iCommaLocation && sName2.Substring(0, iCommaLocation) == sLastName)
                {
                    int iCommaSize = 1;
                    if (sName2[iCommaLocation+1] == ' ')
                        iCommaSize++;
                    sName2 = sName2.Remove(0, iCommaLocation + iCommaSize);
                }
            }
            if (sName2.Length != 0)
            {
                if (sName2.Substring(0, 3) != "c/o")
                    sReturnString += " &";
                sReturnString += (" " + sName2);
            }
            return sReturnString;
        }
        //****************************************************************************************************************************
        public static void SetToNewValueIfDifferent(ArrayList FieldsModified,
                                             DataColumnCollection columns,
                                             DataRow row,
                                             string fieldIndex,
                                             string newFieldValue)
        {
            if (row[fieldIndex].Equals(newFieldValue))
            {
                return;
            }
            if (row[fieldIndex].ToString() != newFieldValue)
            {
                row[fieldIndex] = newFieldValue;
                if (FieldIndexDoesNotExist(FieldsModified, fieldIndex))
                {
                    FieldsModified.Add(fieldIndex);
                }
            }
        }
        //****************************************************************************************************************************
        public static void SetToNewValueIfDifferent(ArrayList FieldsModified,
                                             DataRow row,
                                             string fieldIndex,
                                             Int64 newFieldValue)
        {
            if (row[fieldIndex].Equals(newFieldValue))
            {
                return;
            }
            if (FieldIndexDoesNotExist(FieldsModified, fieldIndex))
            {
                FieldsModified.Add(fieldIndex);
            }
            row[fieldIndex] = newFieldValue;
        }
        //****************************************************************************************************************************
        public static void SetToNewValueIfDifferent(ArrayList FieldsModified,
                                             DataRow row,
                                             string fieldIndex,
                                             int newFieldValue)
        {
            if (row[fieldIndex].Equals(newFieldValue))
            {
                return;
            }
            if (row[fieldIndex].ToInt() != newFieldValue)
            {
                row[fieldIndex] = newFieldValue;
            }
            if (FieldIndexDoesNotExist(FieldsModified, fieldIndex))
            {
                FieldsModified.Add(fieldIndex);
            }
        }
        //****************************************************************************************************************************
        public static void GetYMD(string dateWithSlashes, out int year, out int month, out int day)
        {
            year = 0;
            month = 0;
            day = 0;
            if (String.IsNullOrEmpty(dateWithSlashes))
            {
                return;
            }
            int firstSlash = dateWithSlashes.IndexOf('/');
            if (firstSlash < 0)
            {
                if (dateWithSlashes.Length == 4)
                {
                    year = dateWithSlashes.Substring(0, 4).ToIntNoError();
                }
                return;
            }
            int secondSlash = dateWithSlashes.IndexOf('/', firstSlash + 1);
            if (secondSlash < 0)
            {
                if (dateWithSlashes.Length == 7)
                {
                    year = dateWithSlashes.Substring(0, 4).ToIntNoError();
                    month = dateWithSlashes.Substring(5, 2).ToIntNoError();
                }
                return;
            }
            if (secondSlash > firstSlash)
            {
                int len = dateWithSlashes.Length - (secondSlash + 1);
                day = dateWithSlashes.Substring(secondSlash + 1, len).ToIntNoError();
            }
            else
            {
                secondSlash = dateWithSlashes.Length;
            }
            year = dateWithSlashes.Substring(0, firstSlash).ToIntNoError();
            int secondLen = secondSlash - (firstSlash + 1);
            month = dateWithSlashes.Substring(firstSlash + 1, secondLen).ToIntNoError();
        }
        //****************************************************************************************************************************
        public static void SetToNewValueIfDifferent(ArrayList FieldsModified,
                                             DataRow row,
                                             Int64 newFieldValue,  // to avoid similar parameters, the order of fieldIndex and newFields value changed
                                             string fieldIndex)
        {
            if (row[fieldIndex].Equals(newFieldValue))
            {
                return;
            }
            if (FieldIndexDoesNotExist(FieldsModified, fieldIndex))
            {
                FieldsModified.Add(fieldIndex);
                row[fieldIndex] = newFieldValue;
            }
        }
        //****************************************************************************************************************************
        public static void SetToNewValueIfDifferent(ArrayList FieldsModified,
                                             DataRow row,
                                             string fieldIndex,
                                             char newFieldValue)
        {
            if (row[fieldIndex].Equals(newFieldValue))
            {
                return;
            }
            if (FieldIndexDoesNotExist(FieldsModified, fieldIndex))
            {
                FieldsModified.Add(fieldIndex);
                row[fieldIndex] = newFieldValue;
            }
        }
        //****************************************************************************************************************************
        public static void SetToNewValueIfDifferent(ArrayList FieldsModified,
                                             DataRow row,
                                             string fieldIndex,
                                             bool newFieldValue)
        {
            if (row[fieldIndex].Equals(newFieldValue.ToInt()))
            {
                return;
            }
            if (FieldIndexDoesNotExist(FieldsModified, fieldIndex))
            {
                FieldsModified.Add(fieldIndex);
                row[fieldIndex] = newFieldValue;
            }
        }
        //****************************************************************************************************************************
        private static bool FieldIndexDoesNotExist(ArrayList FieldsModified,
                                            string fieldIndex)
        {
            foreach (string fieldValue in FieldsModified)
            {
                if (fieldValue == fieldIndex)
                {
                    return false;
                }
            }
            return true;
        }
        //****************************************************************************************************************************
        public static int NumStateRecordsInTable(this DataTable tbl,
                                                 DataViewRowState state)
        {
            DataViewRowState dvRowState = state;
            DataRow[] rows = tbl.Select("", "", dvRowState);
            return rows.Length;
        }
        //****************************************************************************************************************************
        public static string CensusYearCol(int year)
        {
             return "Census" + year.ToString();
        }
        //****************************************************************************************************************************
        public static void DeleteDirectory(string folder)
        {
            var subFolders = Directory.GetDirectories(folder);
            foreach (var subfolder in subFolders)
            {
                DeleteDirectory(subfolder);
            }
            DeleteFiles(folder);
            try
            {
                Directory.Delete(folder);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to delete folder " + folder + ": " + ex.Message);
            }
        }
        //****************************************************************************************************************************
        private static void DeleteFiles(string folder)
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to delete file " + file + ": " + ex.Message);
                }
            }
        }
        //****************************************************************************************************************************
        public static bool IsMarriageRecord(this EVitalRecordType vitalRecordType)
        {
            return (vitalRecordType == EVitalRecordType.eMarriageBride ||
                    vitalRecordType == EVitalRecordType.eMarriageGroom ||
                    vitalRecordType == EVitalRecordType.eCivilUnionPartyA ||
                    vitalRecordType == EVitalRecordType.eCivilUnionPartyB);
        }
        //****************************************************************************************************************************
        public static int NumDaysInMonth(int year,
                                          int month)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: return 31;
                case 4:
                case 6:
                case 9:
                case 11: return 30;
                case 2: return LeapYearDay(year);
                default: return 31;
            }
        }
        //****************************************************************************************************************************
        public static int LeapYearDay(int year)
        {
            if (year % 4 == 0)
            {
                return 29;
            }
            else
            {
                return 28;
            }
        }
        //****************************************************************************************************************************
    }
}
