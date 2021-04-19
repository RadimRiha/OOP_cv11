DROP TABLE IF EXISTS Grading
DROP TABLE IF EXISTS Attendance
DROP TABLE IF EXISTS Subjects
DROP TABLE IF EXISTS Students

CREATE TABLE Subjects(Abbreviation char(8) NOT NULL, SubjectName varchar(60),
PRIMARY KEY (Abbreviation))

CREATE TABLE Students(Firstname varchar(50) NOT NULL, Surname varchar(50) NOT NULL, DateOfBirth date, ID int NOT NULL,
PRIMARY KEY (ID))

CREATE TABLE Attendance(StudentID int NOT NULL, SubjectAbbreviation char(8) NOT NULL,
PRIMARY KEY (StudentID, SubjectAbbreviation),
FOREIGN KEY (StudentID) REFERENCES Students(ID) ON DELETE CASCADE,
FOREIGN KEY (SubjectAbbreviation) REFERENCES Subjects(Abbreviation) ON DELETE CASCADE)

CREATE TABLE Grading(StudentID int NOT NULL, SubjectAbbreviation char(8) NOT NULL, GradingDate date, Grade float NOT NULL,
PRIMARY KEY (StudentID, SubjectAbbreviation),
FOREIGN KEY (StudentID, SubjectAbbreviation) REFERENCES Attendance(StudentID, SubjectAbbreviation))
