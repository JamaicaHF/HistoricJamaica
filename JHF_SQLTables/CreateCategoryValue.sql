drop table if exists categoryvalue;
CREATE TABLE categoryvalue
(
  CategoryValueID int(11) NOT NULL,
  CategoryID int(11) NOT NULL,
  CategoryValueValue varchar(50) NOT NULL,
  CategoryValueOrder int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;