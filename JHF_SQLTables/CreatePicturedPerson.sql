drop table if exists picturedperson;
CREATE TABLE picturedperson
(
  PicturedPersonNumber int(11) NOT NULL,
  PhotoID int(11) NOT NULL,
  PersonID int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
