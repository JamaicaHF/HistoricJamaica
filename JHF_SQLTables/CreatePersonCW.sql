drop table if exists personcw;
CREATE TABLE personcw
(
  PersonCWID int(11) NOT NULL,
  FirstName varchar(20) NOT NULL,
  MiddleName varchar(20) NOT NULL,
  LastName varchar(20) NOT NULL,
  BornDate varchar(20) NOT NULL,
  DiedDate varchar(20) NOT NULL,
  EnlistmentDate varchar(20) NOT NULL,
  CemeteryName varchar(1000) NOT NULL,
  BattleSiteKilled varchar(1000) NOT NULL,
  DataMilitary varchar(1000) NOT NULL,
  Reference varchar(5000) NOT NULL,
  Notes varchar(1000) NOT NULL,
  PersonID int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;