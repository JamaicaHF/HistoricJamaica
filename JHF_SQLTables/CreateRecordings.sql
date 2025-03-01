CREATE TABLE Recordings (
  ID int NOT NULL IDENTITY(1,1) primary key,
  Composer varchar(100) NOT NULL,
  Album varchar(250) NOT NULL,
  Artist varchar(250) NOT NULL,
  Genre varchar(100) NOT NULL,
  Extension varchar(10) NOT NULL,
  IPod int NOT NULL,
  NumTracks int NOT NULL
)
