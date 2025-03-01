drop table if exists photo;
CREATE TABLE photo
(
  PhotoID int(11) NOT NULL,
  PhotoName varchar(20) NOT NULL,
  PhotoExtension char(4) NOT NULL,
  PhotoNotes varchar(1000) NOT NULL,
  PhotoYear int(11) NOT NULL,
  PhotoSource varchar(20) NOT NULL,
  PhotoDrawer int(11) NOT NULL,
  PhotoFolder int(11) NOT NULL,
  NumPicturedPersons int(11) NOT NULL,
  NumPicturedBuildings int(11) NOT NULL,
  PRIMARY KEY (PhotoID)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
