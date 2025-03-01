using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQL_Library
{
    //****************************************************************************************************************************
    public class CemeteryRecordProperties
    {
        public Cemetery cemetery;
        public string NameOnGrave;
        public string SpouseNameOnGrave;
        public string FatherNameOnGrave;
        public string MotherNameOnGrave;
        public string BornDate;
        public string DiedDate;
        public int AgeYears;
        public int AgeMonths;
        public int AgeDays;
        public string LotNumber;
        public char Disposition;
        public char Sex;
        public string Epitaph;
        public string Notes;
        //****************************************************************************************************************************
        public CemeteryRecordProperties(Cemetery cemetery,
                                     string nameOnGrave,
                                     string spouseNameOnGrave,
                                     string fatherNameOnGrave,
                                     string motherNameOnGrave,
                                     string bornDate,
                                     string diedDate,
                                     int ageYears,
                                     int ageMonths,
                                     int ageDays,
                                     string lotNumber,
                                     char disposition,
                                     char sex,
                                     string epitaph,
                                     string notes)
        {
            this.cemetery = cemetery;
            this.NameOnGrave = nameOnGrave;
            this.SpouseNameOnGrave = spouseNameOnGrave;
            this.FatherNameOnGrave = fatherNameOnGrave;
            this.MotherNameOnGrave = motherNameOnGrave;
            this.BornDate = bornDate;
            this.DiedDate = diedDate;
            this.AgeYears = ageYears;
            this.AgeMonths = ageMonths;
            this.AgeDays = ageDays;
            this.LotNumber = lotNumber;
            this.Disposition = disposition;
            this.Sex = sex;
            this.Epitaph = epitaph;
            this.Notes = notes;
        }
    }
}
