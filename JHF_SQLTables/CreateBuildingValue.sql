drop table if exists buildingvalue;
CREATE TABLE buildingvalue
(
  BuildingValueID int(11) NOT NULL,
  BuildingID int(11) NOT NULL,
  BuildingValueValue varchar(50) NOT NULL,
  BuildingValueOrder int(11) NOT NULL,
  Notes varchar(1000) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;