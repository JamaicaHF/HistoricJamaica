using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SQL_Library
{
    public class CPersonRecord
    {
        public string PersonPrefix;
        public string PersonFirstName;
        public string PersonMiddleName;
        public string PersonLastName;
        public string PersonSuffix;
        public string PersonMarriedName;
        public string PersonKnownAs;
        public string PersonDescription;
        public string PersonSource;
        public int PersonFatherID;
        public int PersonMotherID;
        public string PersonSex;
        public string PersonBornDate;
        public string PersonBornPlace;
        public string PersonBornHome;
        public string PersonBornVerified;
        public string PersonBornSource;
        public string PersonBornBook;
        public string PersonBornPage;
        public string PersonDiedDate;
        public string PersonDiedPlace;
        public string PersonDiedHome;
        public string PersonDiedVerified;
        public string PersonDiedSource;
        public string PersonDiedBook;
        public string PersonDiedPage;
        public string PersonBuriedDate;
        public string PersonBuriedPlace;
        public string PersonBuriedStone;
        public string PersonBuriedVerified;
        public string PersonBuriedSource;
        public string PersonBuriedBook;
        public string PersonBuriedPage;
        public int iImportPersonID;
        public string PersonFather;
        public string PersonMother;
        public string PersonSpouse;
        //****************************************************************************************************************************
        public CPersonRecord()
        {
            InitializePerson();
        }
        //****************************************************************************************************************************
        public bool InitializePerson()
        {
            PersonPrefix = "";
            PersonFirstName = "";
            PersonMiddleName = "";
            PersonLastName = "";
            PersonSuffix = "";
            PersonMarriedName = "";
            PersonKnownAs = "";
            PersonDescription = "";
            PersonSource = "";
            PersonFatherID = 0;
            PersonMotherID = 0;
            PersonSex = "";
            PersonBornDate = "";
            PersonBornPlace = "";
            PersonBornHome = "";
            PersonBornVerified = "N";
            PersonBornSource = "";
            PersonBornBook = "";
            PersonBornPage = "";
            PersonDiedDate = "";
            PersonDiedPlace = "";
            PersonDiedHome = "";
            PersonDiedVerified = "N";
            PersonDiedSource = "";
            PersonDiedBook = "";
            PersonDiedPage = "";
            PersonBuriedDate = "";
            PersonBuriedPlace = "";
            PersonBuriedStone = "";
            PersonBuriedVerified = "N";
            PersonBuriedSource = "";
            PersonBuriedBook = "";
            PersonBuriedPage = "";
            iImportPersonID = 0;
            PersonFather = "";
            PersonMother = "";
            PersonSpouse = "";
            return true;
        }
        //****************************************************************************************************************************
    };
}
