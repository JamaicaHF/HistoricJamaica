drop table if exists grandlisthistory;
CREATE TABLE grandlisthistory
(
  ID int(11) NOT NULL,
  HistoryYear int(11) NOT NULL,
  Name1 varchar(50) NOT NULL,
  Name2 varchar(50) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;