use FA24_ksaortizc




--Drop table if exist
DROP TABLE if exists dbo.ClassUser
DROP TABLE if exists dbo.Parent
DROP TABLE if exists dbo.Users
DROP TABLE if exists dbo.Classes
DROP TABLE if exists dbo.WeekDays
DROP TABLE if exists dbo.Style
DROP TABLE if exists dbo.Levels
DROP TABLE if exists dbo.AgeRange
GO 





--Create tables
--Children tables for classes and users
--Create AgeRange Table
CREATE TABLE dbo.AgeRange
(
	AgeRangeID int IDENTITY(1,1) NOT NULL CONSTRAINT pkAgeRangeID PRIMARY KEY,
	MinimumAge int NOT NULL, 
	MaximumAge int NOT NULL,
	RangeName varchar(20) NOT NULL
)
GO

--Create Levels table
CREATE TABLE dbo.Levels
(
	LevelID int IDENTITY(1,1) NOT NULL CONSTRAINT pkLevelID PRIMARY KEY,
	LevelName varchar(30) NOT NULL
)
GO

--Create Style table
CREATE TABLE dbo.Style
(
	StyleID int IDENTITY(1,1) NOT NULL CONSTRAINT pkStyleID PRIMARY KEY,
	StyleName varchar(30) NOT NULL
)
GO

--Create Days table
CREATE TABLE dbo.WeekDays
(
	WeekDaysID int IDENTITY(1,1) NOT NULL CONSTRAINT pkWeekDaysID PRIMARY KEY,
	WeekDaysName varchar(30) NOT NULL UNIQUE
)
GO


--Parent table and children table for many to many groups
--Create classes table
CREATE TABLE dbo.Classes
(
	ClassID int IDENTITY(1,1) NOT NULL CONSTRAINT pkClassID PRIMARY KEY,
	Price decimal(5,2) NOT NULL, 
	ClassHourStart varchar(4) NOT NULL,
	ClassHourFinish varchar(4) NOT NUll,
	AgeRangeID int not null constraint fkClassesToAgeRange Foreign Key REFERENCES dbo.AgeRange(AgeRangeID),
	LevelID int not null constraint fkClassesToLevel Foreign Key REFERENCES dbo.Levels(LevelID),
	StyleID int not null constraint fkClassesToStyle Foreign Key REFERENCES dbo.Style(StyleID),
	WeekDaysID int not null constraint fkClassesToWeekDays Foreign Key REFERENCES dbo.WeekDays(WeekDaysID)
)
GO

--Child table for the many to many classes
--Create users table
CREATE TABLE dbo.Users
(
	UserID int IDENTITY(1,1) NOT NULL CONSTRAINT pkUserID PRIMARY KEY,
	FirstName varchar(30) NOT NULL,
	LastName varchar(30) NOT NULL,
	EmailAddress varchar(100) NOT NULL UNIQUE, 
	PhoneNumber varchar(15) NOT NULL UNIQUE,
	Birthday date NOT NULL,
	Username varchar(50) NOT NULL UNIQUE,
	UserPassword varchar(20) NOT NULL,
	UserCreationDate date NOT NULL DEFAULT GETDATE()
)
GO

--Parent table for users with tutor
CREATE TABLE dbo.Parent
(
    ParentID int IDENTITY(1,1) NOT NULL CONSTRAINT pkParentID PRIMARY KEY,
    FirstName varchar(30) NOT NULL,
    LastName varchar(30) NOT NULL,
    EmailAddress varchar(100) NOT NULL,
    PhoneNumber varchar(15) NOT NULL,
	Birthday date NOT NULL,
    UserID int NOT NULL CONSTRAINT fkParentToUsers FOREIGN KEY REFERENCES dbo.Users(UserID)
)
GO


--Create table many to many for users and classes
CREATE TABLE dbo.ClassUser
(
	ClassUserID int IDENTITY(1,1) NOT NULL CONSTRAINT pkClassUserID PRIMARY KEY,
	EnrollmentDate date NOT NULL DEFAULT GETDATE(),
	ClassID int not null constraint fkClassUserToClasses Foreign Key REFERENCES dbo.Classes(ClassID),
	UserID int not null constraint fkClassUserToUsers Foreign Key REFERENCES dbo.Users(UserID),
	CONSTRAINT uc_UserClass UNIQUE (UserID,ClassId)
)
GO




--Insert data to table
--Age Range Table
INSERT INTO dbo.AgeRange
(MinimumAge, MaximumAge,RangeName)
VALUES
(4,10,'Children'),
(11,17,'Youth'),
(18,120,'Adults'),
(4,120,'All Ages')
GO

--Levels Table
INSERT INTO dbo.Levels
(LevelName)
VALUES
('Beginner'),('Intermediate'),('Advanced'),('Troupe')
GO

--Style Table
INSERT INTO dbo.Style
(StyleName)
VALUES
('Folklore'),('Jazz'),('Ballet'),('Stretching')
GO

--Days Table
INSERT INTO dbo.WeekDays
(WeekDaysName)
VALUES
('Mon/Wed'),('Thu/Tue'),('Fri/Sun'),('Mon-Fri')
GO

--Classes Table
INSERT INTO dbo.Classes
(Price,ClassHourStart,ClassHourFinish,AgeRangeID,LevelID,StyleID,WeekDaysID)
VALUES 
(200.00, '15', '17', 1, 1, 1, 1),
(200.00, '15', '17', 1, 2, 1, 2),
(200.00, '17', '19', 2, 1, 1, 1),
(180.00, '17', '19', 2, 2, 1, 2),
(180.00, '19', '21', 2, 3, 1, 4),
(150.00, '09', '11', 3, 1, 1, 3),
(150.00, '11', '13', 3, 2, 1, 3),
(150.00, '13', '15', 3, 3, 1, 4),
(150.00, '15', '17', 4, 1, 2, 1),
(150.00, '15', '17', 4, 2, 2, 1),
(150.00, '17', '19', 4, 3, 2, 1),
(150.00, '17', '19', 4, 1, 3, 1),
(150.00, '19', '21', 4, 2, 3, 1),
(150.00, '09', '11', 4, 3, 3, 1),
(150.00, '11', '13', 4, 1, 4, 1),
(150.00, '13', '15', 4, 2, 4, 1),
(150.00, '15', '17', 4, 3, 4, 1)
GO

--Users Table
INSERT INTO dbo.Users 
(FirstName, LastName, EmailAddress, PhoneNumber, Birthday, Username, UserPassword)
VALUES 
('John', 'Doe', 'john.doe@example.com', '555-1234', '2019-01-01', 'johndoe', 'password123'),
('Jane', 'Smith', 'jane.smith@example.com', '555-5678', '2010-02-02', 'janesmith', 'password456'),
('Alice', 'Johnson', 'alice.johnson@example.com', '555-8765', '1988-03-03', 'alicej', 'password789')
GO

--Parent table
INSERT INTO dbo.Parent
(FirstName, LastName, EmailAddress, PhoneNumber, Birthday,UserID)
VALUES
('Juan', 'Serino', 'juan.ser@example.com', '555-1234', '2019-01-01',1)

--ClassUser Table
INSERT INTO dbo.ClassUser 
(ClassID, UserID)
VALUES 
(1, 1),
(5, 2),
(13, 2),
(7, 3)
GO

--Select * from Users

--SELECT 
--	u.FirstName,
--	u.LastName,
--	u.EmailAddress,
--	ar.RangeName AS AgeRange,
--	s.StyleName AS Style,
--	l.LevelName AS Level,
--	c.ClassHourStart,
--	c.ClassHourFinish
--FROM 
--    dbo.ClassUser cu
--JOIN 
--    dbo.Users u ON cu.UserID = u.UserID
--JOIN 
--    dbo.Classes c ON cu.ClassID = c.ClassID
--JOIN 
--    dbo.AgeRange ar ON c.AgeRangeID = ar.AgeRangeID
--JOIN 
--    dbo.Style s ON c.StyleID = s.StyleID
--JOIN 
--    dbo.Levels l ON c.LevelID = l.LevelID
--WHERE 
--	u.username = 'johndoe'

--GO
