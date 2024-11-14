use FA24_ksaortizc



--Drop table if exist (9 tables)
DROP TABLE if exists dbo.ClassUser
DROP TABLE if exists dbo.Classes
DROP TABLE if exists dbo.Users
DROP TABLE if exists dbo.WeekDays
DROP TABLE if exists dbo.Instructor
DROP TABLE if exists dbo.Style
DROP TABLE if exists dbo.Levels
DROP TABLE if exists dbo.AgeRange
GO 





--Create tables
--Children tables for classes and users
--Create AgeRange Table
--To make sure the age range classes is clear
CREATE TABLE dbo.AgeRange
(
	AgeRangeID int IDENTITY(1,1) NOT NULL CONSTRAINT pkAgeRangeID PRIMARY KEY,
	MinimumAge int NOT NULL, 
	MaximumAge int NOT NULL,
	RangeName varchar(20) NOT NULL
)
GO

--Create Levels table
--To store all the possible levels
CREATE TABLE dbo.Levels
(
	LevelID int IDENTITY(1,1) NOT NULL CONSTRAINT pkLevelID PRIMARY KEY,
	LevelName varchar(30) NOT NULL
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

--Create Instuctor table
--To store all the possible instructors
CREATE TABLE dbo.Instructor
(
	InstructorID int IDENTITY(1,1) NOT NULL CONSTRAINT pkInstructorID PRIMARY KEY,
	FirstNameInstructor varchar(30) NOT NULL,
	LastNameInstructor varchar(30) NOT NULL,
	EmailAddressInstructor varchar(100) NOT NULL UNIQUE, 
	PhoneNumberInstructor varchar(15) NOT NULL UNIQUE,
	BirthdayInstructor date NOT NULL
)
GO


--Child table for the many to many classes
--Create users table
--To store all the users
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

--Parent table and children table for many to many groups
--Create classes table
--To store all the classes available in the moment
CREATE TABLE dbo.Classes
(
	ClassID int IDENTITY(1,1) NOT NULL CONSTRAINT pkClassID PRIMARY KEY,
	Price decimal(5,2) NOT NULL, 
	StartHour varchar(4) NOT NULL,
	EndHour varchar(4) NOT NUll,
	AgeRangeID int not null constraint fkClassesToAgeRange Foreign Key REFERENCES dbo.AgeRange(AgeRangeID),
	LevelID int not null constraint fkClassesToLevel Foreign Key REFERENCES dbo.Levels(LevelID),
	StyleID int not null constraint fkClassesToStyle Foreign Key REFERENCES dbo.Style(StyleID),
	WeekDaysID int not null constraint fkClassesToWeekDays Foreign Key REFERENCES dbo.WeekDays(WeekDaysID),
	InstructorID int not null constraint fkClassesToInstructor Foreign Key REFERENCES dbo.Instructor(InstructorID),
	UserID int not null constraint fkClassesToUsers Foreign Key REFERENCES dbo.Users(UserID)


)
GO

--Create table many to many for users and classes
--To store all the users in each class (many to many)
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

--Instructor table
INSERT INTO dbo.Instructor
(FirstNameInstructor, LastNameInstructor, EmailAddressInstructor, PhoneNumberInstructor, BirthdayInstructor)
VALUES
('Luis', 'Mejia', 'luis@gmail.com', '+52555-1234', '1975-03-10')


--Classes Table
--INSERT INTO dbo.Classes
--(Price,StartHour,EndHour,AgeRangeID,LevelID,StyleID,WeekDaysID, InstructorID)
--VALUES 
--(200.00, '15', '17', 1, 1, 1, 1, 1),
--(200.00, '15', '17', 1, 2, 1, 2, 1),
--(200.00, '17', '19', 2, 1, 1, 1, 1),
--(180.00, '17', '19', 2, 2, 1, 2, 1),
--(180.00, '19', '21', 2, 3, 1, 4, 1),
--(150.00, '09', '11', 3, 1, 1, 3, 1),
--(150.00, '11', '13', 3, 2, 1, 3, 1),
--(150.00, '13', '15', 3, 3, 1, 4, 1),
--(150.00, '15', '17', 4, 1, 2, 1, 1),
--(150.00, '15', '17', 4, 2, 2, 1, 1),
--(150.00, '17', '19', 4, 3, 2, 1, 1),
--(150.00, '17', '19', 4, 1, 3, 1, 1),
--(150.00, '19', '21', 4, 2, 3, 1, 1),
--(150.00, '09', '11', 4, 3, 3, 1, 1),
--(150.00, '11', '13', 4, 1, 4, 1, 1),
--(150.00, '13', '15', 4, 2, 4, 1, 1),
--(150.00, '15', '17', 4, 3, 4, 1, 1)
--GO

--Users Table
--INSERT INTO dbo.Users 
--(FirstNameUser, LastNameUser, EmailAddressUser, PhoneNumberUser, BirthdayUser, FirstNameGuardian, LastNameGuardian, EmailAddressGuardian, PhoneNumberGuardian, BirthdayGuardian, Username)
--VALUES 
--('John', 'Doe', 'john.doe@hotmail.com', '555-1234', '2019-01-01','Juan', 'Serino', 'juan.ser@hotmail.com', '555-1234', '2019-01-01', 'johndoe')
--GO

--INSERT INTO dbo.Users 
--(FirstNameUser, LastNameUser, EmailAddressUser, PhoneNumberUser, BirthdayUser, Username)
--VALUES 
--('Jane', 'Smith', 'jane.smith@gmail.com', '555-5678', '2010-02-02', 'janesmith'),
--('Alice', 'Johnson', 'alice.johnson@gmail.com', '555-8765', '1988-03-03', 'alicej')
--GO

--ClassUser Table
--INSERT INTO dbo.ClassUser 
--(ClassID, UserID)
--VALUES 
--(1, 1),
--(5, 2),
--(13, 2),
--(7, 3)
--GO

--SELECT 
--	u.FirstName,
--	u.LastName,
--	u.EmailAddress,
--	ar.RangeName AS AgeRange,
--	s.StyleName AS Style,
--	l.LevelName AS Level,
--	c.StartHour,
--	c.EndHour,
--	t.FirstName
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
--JOIN
--	dbo.Instructor t ON c.InstructorID = t.InstructorID

--GO

--SELECT 
--	i.FirstNameInstructor AS Name,
--	s.StyleName AS Style,
--	l.LevelName AS Level,
--	ag.RangeName AS 'Age Range',
--	wd.WeekDaysName AS 'Days of the week'
--FROM
--	dbo.Classes c
--JOIN
--	dbo.Instructor i ON c.InstructorID = i.InstructorID
--JOIN
--	dbo.Style s ON c.StyleID = s.StyleID
--JOIN 
--	dbo.Levels l ON c.LevelID = l.LevelID
--JOIN
--	dbo.AgeRange ag ON c.AgeRangeID = ag.AgeRangeID
--JOIN
--	dbo.WeekDays wd ON c.WeekDaysID = wd.WeekDaysID