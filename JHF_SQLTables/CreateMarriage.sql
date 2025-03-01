drop table if exists marriage;
CREATE TABLE marriage
(
  PersonID int(11) NOT NULL,
  SpouseID int(11) NOT NULL,
  DateMarried varchar(20) NOT NULL,
  Divorced char(1) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
