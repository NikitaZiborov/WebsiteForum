CREATE DATABASE WebsiteForum
GO

CREATE TABLE Users
(
	ID INT NOT NULL CONSTRAINT User_ID_PK PRIMARY KEY IDENTITY,
	[Name] VARCHAR(20) NOT NULL,
	SurName VARCHAR(20) NOT NULL,
	NickName VARCHAR(20) CONSTRAINT NickNameUnique UNIQUE NOT NULL,
	BirthDate DATE NOT NULL,
	Email VARCHAR(30) NOT NULL,
	[Password] VARCHAR(20) NOT NULL,
	ConfirmationPassword VARCHAR(20) NOT NULL
)
GO

CREATE TABLE Questions
(
	ID INT NOT NULL CONSTRAINT Question_ID_PK PRIMARY KEY IDENTITY,
	Author INT NOT NULL CONSTRAINT UserWhoAsked_ID_FK FOREIGN KEY REFERENCES Users(ID),
	QuestionText VARCHAR(MAX) NOT NULL,
	PostDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	IsOpenStatus BIT NOT NULL DEFAULT 1, 
)
GO

CREATE TABLE Answers
(
	ID INT NOT NULL CONSTRAINT Answer_ID_PK PRIMARY KEY IDENTITY,
	Author INT NOT NULL CONSTRAINT UserWhoAnswered_ID_FK FOREIGN KEY REFERENCES Users(ID),
	ForQuestion INT NOT NULL CONSTRAINT ForQuestion_FK FOREIGN KEY REFERENCES Questions(ID),
	AnswerText VARCHAR(MAX) NOT NULL,
	PostDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	Rating TinyInt CHECK (Rating BETWEEN 0 AND 5) DEFAULT 0 NULL
)
GO
DROP TABLE Answers

TRUNCATE TABLE Questions
TRUNCATE TABLE Answers
--DECLARE @A BIT = NULL; PRINT @A; SET @A = null; PRINT @A;

SELECT *FROM Users
SELECT *FROM Questions
SELECT *FROM Answers

--ADD NEW USERS TO THE USERS TABLE
INSERT INTO Users 
([Name], SurName, NickName, BirthDate, Email, [Password], ConfirmationPassword)
VALUES
('Iryna', 'Voloshkyna', 'Iryna_Voloshkyna', '2000-02-20', 'irynavoloshkyna@ukr.net', 'iryna1', 'iryna1'),
('Stepan','Balytskyy', 'Stepan_Balytskyy', '1995-05-25', 'stepanbalytsky@ukr.net', 'stepan2', 'stepan2'),
('Andrii', 'Ternenko', 'Andrii_Ternenko', '1990-07-17', 'andriiternenko@ukr.net', 'andrii3', 'andrii3'),
('Kateryna', 'Malyar', 'Kateryna_Malyar', '2005-03-30', 'katerynamalyar@ukr.net', 'kateryna4', 'kateryna4')
GO

--ADD NEW QUESTIONS TO THE QUESTIONS TABLE
INSERT INTO Questions
(Author, QuestionText)
VALUES
(1, 'What are the correct version numbers for C#?'),
(2, 'What is the difference between String and string in C#?'),
(3, 'What is the difference between an interface and abstract class?'),
(4, 'Deserialize JSON into C# dynamic object?')
GO

--ADD NEW ANSWERS TO THE ANSWERS TABLE
INSERT INTO Answers
(Author, ForQuestion, AnswerText)
VALUES
(4, 1, 'NET 3.5. The language and framework are versioned independently, however - as is the CLR, which is at version 2.0 for . NET 2.0 through 3.5'),
(3, 2, 'Basically, there is no difference between string and String in C#. "string" is just an alias of System. String and both are compiled in the same manner.'),
(2, 3, 'Interface: Explore the Difference between Abstract Class and Interface in Java. The Abstract class and Interface both are used to have abstraction. An abstract class contains an abstract keyword on the declaration whereas an Interface is a sketch that is used to implement a class.'),
(1, 4, 'Using dynamic With System. Text. Json to Deserialize JSON Into a Dynamic Object')
GO