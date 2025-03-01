drop table if exists modernroadvalue;
CREATE TABLE modernroadvalue
(
  ModernRoadValueID int(11) NOT NULL,
  ModernRoadValueValue varchar(50) NOT NULL,
  ModernRoadValueOrder int(11) NOT NULL,
  ModernRoadValueSection int(11) NOT NULL,
  JRoadName varchar(5) NOT NULL,
  HistoricRoad char(1) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;