drop table if exists schoolrecord;
CREATE TABLE schoolrecord
(
  SchoolRecordID int(11) NOT NULL,
  SchoolID int NOT NULL,
  SchoolRecordType int NOT NULL,
  Year int NOT NULL,
  Grade varchar(50) NOT NULL,
  BornDate varchar(20) NOT NULL,
  Person varchar(50) NOT NULL,
  PersonID int NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;