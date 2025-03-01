using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SQL_Library
{
    public class PersonSearchTableAndColumns
    {
        public string tableName;
        public string firstNameCol;
        public string middleNameCol;
        public string lastNameCol;
        public PersonSearchTableAndColumns(string sTableName,
                                           string sFirstNameCol,
                                           string sMiddleNameCol,
                                           string sLastNameCol)
        {
            this.tableName = sTableName;
            this.firstNameCol = sFirstNameCol;
            this.middleNameCol = sMiddleNameCol;
            this.lastNameCol = sLastNameCol;
        }
        public PersonSearchTableAndColumns(PersonSearchTableAndColumns personSearchTableAndColumns)
        {
            this.tableName = personSearchTableAndColumns.tableName;
            this.firstNameCol = personSearchTableAndColumns.firstNameCol;
            this.middleNameCol = personSearchTableAndColumns.middleNameCol;
            this.lastNameCol = personSearchTableAndColumns.lastNameCol;
        }
    }
    public class PersonName
    {
        public string lastName;
        public string firstName;
        public string middleName;
        public string prefix;
        public string suffix;
        public string marriedName;
        public int integratedPersonID;
        //****************************************************************************************************************************
        public PersonName()
        {
            this.lastName = "";
            this.firstName = "";
            this.middleName = "";
            this.suffix = "";
            this.prefix = "";
            this.marriedName = "";
            this.integratedPersonID = 0;
        }
        //****************************************************************************************************************************
        public PersonName(PersonName personName)  // Copy constructor
        {
            this.firstName = personName.firstName;
            this.middleName = personName.middleName;
            this.lastName = personName.lastName;
            this.prefix = personName.prefix;
            this.suffix = personName.suffix;
            this.marriedName = personName.marriedName;
            this.integratedPersonID = personName.integratedPersonID;
        }
        //****************************************************************************************************************************
        public PersonName(string firstName,
                          string middleName,
                          string lastName,
                          string suffix,
                          string prefix="",
                          int    integratedPersonID=0)
        {
            this.lastName = lastName;
            this.firstName = firstName;
            this.middleName = middleName;
            this.suffix = suffix;
            this.prefix = prefix;
            this.marriedName = "";
            this.integratedPersonID = integratedPersonID;
        }
        //****************************************************************************************************************************
        public PersonName(DataRow personRow)
        {
            this.lastName = personRow[U.LastName_col].ToString();
            this.firstName = personRow[U.FirstName_col].ToString();
            this.middleName = personRow[U.MiddleName_col].ToString();
            this.suffix = personRow[U.Suffix_col].ToString();
            this.prefix = personRow[U.Prefix_col].ToString();
            this.marriedName = personRow[U.MarriedName_col].ToString();
            this.integratedPersonID = 0;
        }
        //****************************************************************************************************************************
        public PersonName(string firstName,
                          string middleName,
                          string lastName,
                          string suffix,
                          string prefix)
        {
            this.lastName = lastName;
            this.firstName = firstName;
            this.middleName = middleName;
            this.suffix = suffix;
            this.prefix = prefix;
            this.marriedName = "";
            this.integratedPersonID = 0;
        }
        //****************************************************************************************************************************
        public PersonName(string lastnameFirstname)
        {

            this.lastName = "";
            this.firstName = "";
            this.middleName = "";
            this.suffix = "";
            this.prefix = "";
            this.marriedName = "";
            this.integratedPersonID = 0;
            int indexOfComma = lastnameFirstname.IndexOf(',');
            if (indexOfComma <= 0)
            {
                return;
            }
            lastName = lastnameFirstname.Substring(0, indexOfComma);
            int indexOfspace = lastName.IndexOf(' ');
            if (indexOfspace > 0)
            {
                suffix = lastName.Substring(indexOfspace).Trim();  // must be before lastname;
                lastName = lastName.Substring(0, indexOfspace).Trim();
            }
            firstName = lastnameFirstname.Substring(indexOfComma + 1).Trim();
            int indexOfJr = firstName.ToLower().IndexOf("jr");
            if (indexOfJr > 0)
            {
                suffix = firstName.Substring(indexOfJr).Trim();  // must be before firstname;
                firstName = firstName.Substring(0, indexOfJr).Trim();
            }
            indexOfspace = firstName.IndexOf(' ');
            if (indexOfspace > 0)
            {
                middleName = firstName.Substring(indexOfspace).Trim();  // must be before firstname;
                firstName = firstName.Substring(0, indexOfspace).Trim();
            }
        }
        public void RemoveMiddleSuffixPrefix()
        {
            this.middleName = "";
            this.suffix = "";
            this.prefix = "";
        }
        //****************************************************************************************************************************
        public string LastNameFirst()
        {
            string name = lastName;
            if (!String.IsNullOrEmpty(suffix))
            {
                name += " " + suffix;
            }
            name += ", " + firstName;
            if (!String.IsNullOrEmpty(middleName))
            {
                name += " " + middleName;
            }
            return name;
        }
        //****************************************************************************************************************************
        public bool IsEmpty(bool getFromGrid)
        {
            if (String.IsNullOrEmpty(firstName))
            {
                return true;
            }
            if (!getFromGrid)
            {
                if (firstName.Length < 3 && middleName.Length < 3)
                {
                    return true;
                }
            }
            if (String.IsNullOrEmpty(lastName))
            {
                return true;
            }
            return false;
        }
    }
    //****************************************************************************************************************************
    public class PersonWithParents
    {
        private PersonName personName;
        private PersonName fatherName;
        private PersonName motherName;
        private EVitalRecordType vitalRecordType;
        public PersonWithParents()
        {
            this.vitalRecordType = EVitalRecordType.eSearch;
            personName = new PersonName();
            fatherName = new PersonName();
            motherName = new PersonName();
        }
        public PersonWithParents(EVitalRecordType vitalRecordType,
                                 PersonName personName,
                                 PersonName fatherName,
                                 PersonName motherName)
        {
            this.vitalRecordType = vitalRecordType;
            this.personName = personName;
            this.fatherName = fatherName;
            this.motherName = motherName;
        }
        public PersonWithParents(PersonWithParents personWithParents)  // Copy constructor
        {
            this.personName = new PersonName(personWithParents.personName);
            this.fatherName = new PersonName(personWithParents.fatherName);
            this.motherName = new PersonName(personWithParents.motherName);
            this.vitalRecordType = personWithParents.vitalRecordType;
        }
        public EVitalRecordType VitalRecordType
        {
            get { return this.vitalRecordType; }
            set { this.vitalRecordType = value; }
        }
        public PersonName PersonName
        {
            get { return this.personName; }
        }
        public PersonName FatherName
        {
            get { return this.fatherName; }
        }
        public PersonName MotherName
        {
            get { return this.motherName; }
        }
    }
    //****************************************************************************************************************************
    public class PersonSpouseWithParents
    {
        PersonWithParents personWithParents;
        PersonWithParents spouseWithParents;
        public PersonSpouseWithParents(PersonSpouseWithParents personSpouseWithParents)  // Copy Constructor
        {
            this.personWithParents = new PersonWithParents(personSpouseWithParents.personWithParents);
            this.spouseWithParents = new PersonWithParents(personSpouseWithParents.spouseWithParents);
        }
        public PersonSpouseWithParents(PersonWithParents personWithParents,
                                       PersonWithParents spouseWithParents)
        {
            this.personWithParents = personWithParents;
            if (spouseWithParents == null)
                this.spouseWithParents = new PersonWithParents();
            else
                this.spouseWithParents = spouseWithParents;
        }
        public PersonWithParents PersonWithParents
        {
            get { return this.personWithParents; }
        }
        public PersonWithParents SpouseWithParents
        {
            get { return this.spouseWithParents; }
        }
        public PersonName PersonName
        {
            get { return this.personWithParents.PersonName; }
        }
        public PersonName FatherName
        {
            get { return this.personWithParents.FatherName; }
        }
        public PersonName MotherName
        {
            get { return this.personWithParents.MotherName; }
        }
        public PersonName SpouseName
        {
            get { return this.spouseWithParents.PersonName; }
        }
        public PersonName SpouseFatherName
        {
            get { return this.spouseWithParents.FatherName; }
        }
        public PersonName SpouseMotherName
        {
            get { return this.spouseWithParents.MotherName; }
        }
        public EVitalRecordType VitalRecordType
        {
            get { return this.personWithParents.VitalRecordType; }
        }
        public EVitalRecordType SpouseVitalRecordType
        {
            get { return this.spouseWithParents.VitalRecordType; }
        }
        public void RemoveMiddleSuffixPrefix()
        {
            this.PersonWithParents.PersonName.RemoveMiddleSuffixPrefix();
            this.spouseWithParents.PersonName.RemoveMiddleSuffixPrefix();
        }
    }
    //****************************************************************************************************************************
}
