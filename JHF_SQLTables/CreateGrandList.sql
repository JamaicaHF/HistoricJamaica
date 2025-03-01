drop table if exists grandlist;
CREATE TABLE grandlist
(
  ID int(11) NOT NULL,
  TaxMapID varchar(20) NOT NULL,
  StreetName varchar(50) NOT NULL,
  StreetNum int(11) NOT NULL,
  Name1 varchar(50) NOT NULL,
  Name2 varchar(50) NOT NULL,
  BuildingRoadValueID int
) ENGINE=MyISAM DEFAULT CHARSET=utf8;