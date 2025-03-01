drop table if exists article;
CREATE TABLE article
(
  ArticleID int(11) NOT NULL,
  Article varchar(5000) NOT NULL,
  ArticleContinueID int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;