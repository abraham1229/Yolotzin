use FA24_ksaortizc



--Drop table if exist (7 tables)
DROP TABLE if exists dbo.Classes
DROP TABLE if exists dbo.Users
DROP TABLE if exists dbo.Instructor
DROP TABLE if exists dbo.Levels
DROP TABLE if exists dbo.WeekDays
DROP TABLE if exists dbo.Style
DROP TABLE if exists dbo.AgeRange
GO 





--CREATE TABLES
--Children tables for classes and users

--Create AgeRange Table
--To make sure the age range and price is clear
CREATE TABLE dbo.AgeRange
(
	AgeRangeID int IDENTITY(1,1) NOT NULL CONSTRAINT pkAgeRangeID PRIMARY KEY,
	MinimumAge int NOT NULL, 
	MaximumAge int NOT NULL, 
	RangeName varchar(20) NOT NULL,
	Price decimal(5,2) NOT NULL
)
GO


--Create Style table
--To store all the possible styles
CREATE TABLE dbo.Style
(
	StyleID int IDENTITY(1,1) NOT NULL CONSTRAINT pkStyleID PRIMARY KEY,
	StyleName varchar(30) NOT NULL
)
GO

--Create Days table
--To store the days options for every class
CREATE TABLE dbo.WeekDays
(
	WeekDaysID int IDENTITY(1,1) NOT NULL CONSTRAINT pkWeekDaysID PRIMARY KEY,
	WeekDaysName varchar(30) NOT NULL UNIQUE
)
GO

--Create Levels table
--To store all the possible levels and when this will be
CREATE TABLE dbo.Levels
(
	LevelID int IDENTITY(1,1) NOT NULL CONSTRAINT pkLevelID PRIMARY KEY,
	LevelName varchar(30) NOT NULL,
	StartHour varchar(4) NOT NULL,
	EndHour varchar(4) NOT NUll,
	WeekDaysID int not null constraint fkLevelsToWeekDays Foreign Key REFERENCES dbo.WeekDays(WeekDaysID)
)
GO

--Create Instuctor table
--To store all the possible instructors and the style they teach
CREATE TABLE dbo.Instructor
(
	InstructorID int IDENTITY(1,1) NOT NULL CONSTRAINT pkInstructorID PRIMARY KEY,
	FirstNameInstructor varchar(30) NOT NULL,
	LastNameInstructor varchar(30) NOT NULL,
	EmailAddressInstructor varchar(100) NOT NULL UNIQUE, 
	PhoneNumberInstructor varchar(15) NOT NULL UNIQUE,
	BirthdayInstructor date NOT NULL,
	StyleID int not null constraint fkInstructorToStyle Foreign Key REFERENCES dbo.Style(StyleID)
)
GO


	--Child table for the classes table
--Create users table
--To store all the users and possible guardians
CREATE TABLE dbo.Users
(
	UserID int IDENTITY(1,1) NOT NULL CONSTRAINT pkUserID PRIMARY KEY,
	FirstNameUser varchar(30) NOT NULL,
	LastNameUser varchar(30) NOT NULL,
	EmailAddressUser varchar(100) NOT NULL UNIQUE, 
	PhoneNumberUser varchar(15) NOT NULL UNIQUE,
	BirthdayUser date NOT NULL,
	FirstNameGuardian varchar(30),
    LastNameGuardian varchar(30),
    EmailAddressGuardian varchar(100),
    PhoneNumberGuardian varchar(15),
	BirthdayGuardian date,
	Username varchar(50) NOT NULL UNIQUE,
	UserPasswordHash varbinary(1024),
	UserPasswordSalt varbinary(1024),
	UserCreationDate date NOT NULL DEFAULT GETDATE()
)
GO

--Parent table to store all the users and the class selected
--Create classes table
--To store the class the user has selected
CREATE TABLE dbo.Classes
(
	ClassID int IDENTITY(1,1) NOT NULL CONSTRAINT pkClassID PRIMARY KEY,
	AgeRangeID int not null constraint fkClassesToAgeRange Foreign Key REFERENCES dbo.AgeRange(AgeRangeID),
	LevelID int not null constraint fkClassesToLevel Foreign Key REFERENCES dbo.Levels(LevelID),
	StyleID int not null constraint fkClassesToStyle Foreign Key REFERENCES dbo.Style(StyleID),
	UserID int not null constraint fkClassesToUsers Foreign Key REFERENCES dbo.Users(UserID)
)
GO




--Insert data to table
--Age Range Table
INSERT INTO dbo.AgeRange
(MinimumAge, MaximumAge,RangeName,Price)
VALUES
(4,10,'Children',200),
(11,17,'Youth',180),
(18,120,'Adults',150),
(4,120,'All Ages',150)
GO

--Style Table
INSERT INTO dbo.Style
(StyleName)
VALUES
('Folklore'),('Jazz'),('Ballet'),('Stretching')
GO

--Weekdays options Table
INSERT INTO dbo.WeekDays
(WeekDaysName)
VALUES
('Mon/Wed'),('Thu/Tue'),('Fri/Sun'),('Mon-Fri')
GO

--Levels Table
INSERT INTO dbo.Levels
(LevelName,StartHour,EndHour,WeekDaysID)
VALUES
('Beginner','15','17',1),
('Intermediate','15','17',2),
('Advanced','10','12',3),
('Troupe','17','19',4)
GO

--Instructor table
INSERT INTO dbo.Instructor
(FirstNameInstructor, LastNameInstructor, EmailAddressInstructor, PhoneNumberInstructor, BirthdayInstructor,StyleID)
VALUES
('Luis', 'Mejia', 'luis@gmail.com', '525551234', '1975-03-10',1),
('Juan', 'Lopez', 'juan-lopez@gmail.com', '2223344444', '1990-06-01',2),
('Maria', 'Solano', 'maria-solano@gmail.com', '1234531985', '1998-12-24',3),
('Vero', 'Guzman', 'vero-gusman@gmail.com', '3449090900', '2000-01-04',4)


--User table
INSERT INTO dbo.Users 
(
    FirstNameUser, 
    LastNameUser, 
    EmailAddressUser, 
    PhoneNumberUser, 
    BirthdayUser, 
    FirstNameGuardian, 
    LastNameGuardian, 
    EmailAddressGuardian, 
    PhoneNumberGuardian, 
    BirthdayGuardian, 
    Username, 
    UserPasswordHash, 
    UserPasswordSalt, 
    UserCreationDate
)
--Password is Aoc1229#
VALUES 
(
    'Abraham', 
    'Ortiz', 
    'abrahamortizcastro1229@gmail.com', 
    '2223085976', 
    '2003-12-29', 
    'none', 
    'none', 
    'none', 
    'none', 
    '0001-01-01', 
    'abraham1229', 
	0xB3A9212463037C61F60772F7270B22C1953B2B99882FBFBE324FAE2F0853373D1078B0FC0F7EF4A13D4F6332D6C28E559EB579B46E2ECEBE3D2D3E9696D149BF,
	0x8C946CD1C9A9349F8EBBDB61AFB7B8F5E9A83323DAE365A1A6DC28A97004D261100EC80D57C221D2180D21FFEFF8B579967977165F77E39B195E1255EE8084896BEC53DDD80AC3D3F2E36CB84D8BEBCEE41F9099961EB2DBC07E44AA56C06803EB9ED5BDF9F59B528F736C0DCCD05FEC1FF6F522F01C4AD3516CFB498610F8BC, 
    '2023-11-21'
);


INSERT INTO dbo.Classes 
(AgeRangeID, LevelID, StyleID, UserID)
VALUES 
(3, 4, 1, 1);


--Join to see classes information
SELECT 
	u.FirstNameUser,
	u.Username,
	ag.RangeName as Range,
	l.LevelName as Level,
	s.StyleName as Style,
	l.StartHour as Start,
	l.EndHour as Finish,
	wd.WeekDaysName as WeekDays,
	ag.Price
FROM
	dbo.Classes c
JOIN
	dbo.AgeRange ag ON c.AgeRangeID = ag.AgeRangeID
JOIN
	dbo.Levels l ON c.LevelID = l.LevelID
JOIN
	dbo.Style s ON c.StyleID = s.StyleID
JOIN
	dbo.Users u ON c.UserID = u.UserID
JOIN
	dbo.WeekDays wd ON l.LevelID = wd.WeekDaysID
