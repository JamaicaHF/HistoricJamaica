drop table if exists picturedbuilding;
CREATE TABLE picturedbuilding
(
  PicturedBuildingNumber int(11) NOT NULL,
  PhotoID int(11) NOT NULL,
  BuildingID int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;