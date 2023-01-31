CREATE DATABASE [Zoo]

GO

USE [Zoo]

GO

--Problem 1

CREATE TABLE Owners (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
PhoneNumber VARCHAR(15) NOT NULL,
[Address] VARCHAR(50)
)

CREATE TABLE AnimalTypes (
Id INT PRIMARY KEY IDENTITY,
AnimalType VARCHAR(30) NOT NULL
)

CREATE TABLE Cages (
Id INT PRIMARY KEY IDENTITY,
AnimalTypeId INT FOREIGN KEY REFERENCES AnimalTypes(id) NOT NULL
)

CREATE TABLE Animals (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(30) NOT NULL,
BirthDate DATE NOT NULL,
OwnerId INT FOREIGN KEY REFERENCES Owners(Id),
AnimalTypeId INT FOREIGN KEY REFERENCES AnimalTypes(id) NOT NULL
)

CREATE TABLE AnimalsCages (
CageId INT FOREIGN KEY REFERENCES Cages(Id) NOT NULL,
AnimalId INT FOREIGN KEY REFERENCES Animals(Id) NOT NULL,
PRIMARY KEY (CageId, AnimalId)
)

CREATE TABLE VolunteersDepartments (
Id INT PRIMARY KEY IDENTITY,
DepartmentName VARCHAR(30) NOT NULL
)

CREATE TABLE Volunteers (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
PhoneNumber VARCHAR(15) NOT NULL,
[Address] VARCHAR(50),
AnimalId INT FOREIGN KEY REFERENCES Animals(Id),
DepartmentId INT FOREIGN KEY REFERENCES VolunteersDepartments(Id) NOT NULL
)


--Problem 2

INSERT INTO Volunteers
VALUES
('Anita Kostova', '0896365412', 'Sofia, 5 Rosa str.', 15, 1),
('Dimitur Stoev', '0877564223', NULL, 42, 4),
('Kalina Evtimova', '0896321112', 'Silistra, 21 Breza str.', 9, 7),
('Stoyan Tomov', '0898564100', 'Montana, 1 Bor str.', 18, 8),
('Boryana Mileva', '0888112233', NULL, 31, 5)

INSERT INTO Animals
VALUES
('Giraffe', '2018-09-21', 21, 1),
('Harpy Eagle', '2015-04-17', 15, 3),
('Hamadryas Baboon', '2017-11-02', NULL, 1),
('Tuatara', '2021-06-30', 2, 4)

--Problem 3

SELECT *
  FROM Owners
 WHERE [Name] = 'Kaloqn Stoqnov'

 UPDATE Animals
    SET OwnerId = 4
  WHERE OwnerId IS NULL

--Problem 4

SELECT * 
  FROM VolunteersDepartments
 WHERE DepartmentName = 'Education program assistant'

 DELETE 
  FROM Volunteers
 WHERE DepartmentId = 2

 DELETE 
  FROM VolunteersDepartments
  WHERE Id = 2

--Problem 5

  SELECT [Name], PhoneNumber, [Address], AnimalId, DepartmentId
    FROM Volunteers
ORDER BY [Name], AnimalId, DepartmentId

--Problem 6

 SELECT a.Name, t.AnimalType, FORMAT(a.BirthDate, 'dd.MM.yyyy')
   FROM Animals AS a
   JOIN AnimalTypes AS t ON a.AnimalTypeId = t.Id
ORDER BY a.Name

--Problem 7

SELECT TOP(5) o.Name, COUNT(*)
  FROM Owners AS o
  JOIN Animals As a ON a.OwnerId = o.Id
  GROUP BY o.Name
  ORDER BY COUNT(*) DESC, o.Name

--Problem 8

SELECT CONCAT(o.Name, '-', a.Name) AS [OwnersAnimals], o.PhoneNumber, c.CageId
  FROM Owners AS o
  JOIN Animals AS a ON o.Id = a.OwnerId
  JOIN AnimalTypes AS t ON a.AnimalTypeId = t.Id
  JOIN AnimalsCages AS c ON c.AnimalId = a.Id
 WHERE t.AnimalType = 'Mammals'
ORDER BY o.Name, a.Name DESC

--Problem 9

SELECT v.Name, v.PhoneNumber, SUBSTRING(v.Address, CHARINDEX(',', v.Address) + 1, LEN(v.Address)) AS [Address]
  FROM Volunteers AS v 
  JOIN VolunteersDepartments AS d ON v.DepartmentId = d.Id
 WHERE d.DepartmentName = 'Education program assistant'
   AND v.Address LIKE '%Sofia%'
ORDER BY v.Name

--Problem 10

SELECT a.Name, YEAR(a.BirthDate), t.AnimalType
  FROM Animals AS a
  JOIN AnimalTypes AS t ON a.AnimalTypeId = t.Id
 WHERE a.OwnerId IS NULL 
   AND DATEDIFF(YEAR, a.BirthDate, '2022-01-01') < 5
   AND t.AnimalType <> 'Birds'
ORDER BY a.Name

--Problem 11
GO

CREATE FUNCTION udf_GetVolunteersCountFromADepartment (@VolunteersDepartment VARCHAR(40))
RETURNS INT
AS
BEGIN
               DECLARE @departmentId INT = ( SELECT Id  FROM VolunteersDepartments
			                                   WHERE DepartmentName = @VolunteersDepartment)

			   DECLARE @countOfVolunteers INT = ( SELECT COUNT(*) FROM Volunteers
			                                           WHERE DepartmentId = @departmentId)

RETURN @countOfVolunteers
END

GO

SELECT dbo.udf_GetVolunteersCountFromADepartment ('Education program assistant')

GO

--Problem 12

CREATE PROCEDURE usp_AnimalsWithOwnersOrNot(@AnimalName VARCHAR(50))
AS
BEGIN
                DECLARE @ownerId INT = (SELECT OwnerId FROM Animals
				                          WHERE [Name] = @AnimalName)

                DECLARE @ownerName VARCHAR(50) = NULL

	            IF (@ownerId IS NOT NULL) SET @ownerName = (SELECT [Name]  FROM Owners
				                                             WHERE Id = @ownerId)

			    IF (@ownerName IS NULL) SET @ownerName = 'For adoption'

 SELECT @AnimalName, @ownerName

END

GO

EXEC usp_AnimalsWithOwnersOrNot 'Pumpkinseed Sunfish' 






          





