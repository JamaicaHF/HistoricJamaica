drop table if exists buildingoccupant;
CREATE TABLE buildingoccupant
(
  PersonID int(11) NOT NULL,
  SpouseLivedWithID int(11) NOT NULL,
  Notes varchar(1000) NOT NULL,
  BuildingValueOrder int(11) NOT NULL,
  BuildingID int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
