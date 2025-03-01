drop table if exists building;
CREATE TABLE building
(
  BuildingID int(11) NOT NULL,
  BuildingName varchar(50) NOT NULL,
  BuildingRoadValueID int(11) NOT NULL,
  StreetNum int(11) NOT NULL,
  BuildingGrandListID int(11) NOT NULL,
  Building1856Name varchar(50) NOT NULL,
  Building1869Name varchar(50) NOT NULL,
  Notes varchar(1000) NOT NULL,
  NotesCurrentOwner varchar(1000) NOT NULL,
  Notes1856Name varchar(1000) NOT NULL,
  Notes1869Name varchar(1000) NOT NULL,
  BuildingValueOrder1856Name int(11) NOT NULL,
  BuildingValueOrder1869Name int(11) NOT NULL,
  BuildingArchitectureArticleID int(11) NOT NULL,
  BuildingDescriptionArticleID int(11) NOT NULL,
  BuildingQRCode varchar(10) NOT NULL,
  BuildingThen1 varchar(10) NOT NULL,
  BuildingThen2 varchar(10) NOT NULL,
  BuildingNow1 varchar(10) NOT NULL,
  BuildingNow2 varchar(10) NOT NULL,
  BuildingThen1Title varchar(50) NOT NULL,
  BuildingThen2Title varchar(50) NOT NULL,
  BuildingNow1Title varchar(50) NOT NULL,
  BuildingNow2Title varchar(50) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;