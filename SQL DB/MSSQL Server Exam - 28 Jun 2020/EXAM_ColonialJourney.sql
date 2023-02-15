CREATE DATABASE ColonialJourney

GO

USE ColonialJourney

GO

--Problem 1

CREATE TABLE Planets (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Spaceports (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
PlanetId INT FOREIGN KEY REFERENCES Planets(Id) NOT NULL
)

CREATE TABLE Spaceships (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
Manufacturer VARCHAR(30) NOT NULL,
LightSpeedRate INT DEFAULT(0)
)

CREATE TABLE Colonists (
Id INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(20) NOT NULL,
LastName VARCHAR(20) NOT NULL,
Ucn VARCHAR(10) NOT NULL UNIQUE,
BirthDate DATE NOT NULL
)

CREATE TABLE Journeys (
Id INT PRIMARY KEY IDENTITY,
JourneyStart DATETIME NOT NULL,
JourneyEnd DATETIME NOT NULL,
Purpose VARCHAR(11) CHECK(Purpose IN ('Medical', 'Technical', 'Educational', 'Military')),
DestinationSpaceportId INT FOREIGN KEY REFERENCES Spaceports(Id) NOT NULL,
SpaceshipId INT FOREIGN KEY REFERENCES Spaceships(Id) NOT NULL
)

CREATE TABLE TravelCards (
Id INT PRIMARY KEY IDENTITY,
CardNumber CHAR(10) NOT NULL UNIQUE,
JobDuringJourney VARCHAR(8) CHECK(JobDuringJourney IN ('Pilot', 'Engineer', 'Trooper', 'Cleaner', 'Cook')),
ColonistId INT FOREIGN KEY REFERENCES Colonists(Id) NOT NULL,
JourneyId INT FOREIGN KEY REFERENCES Journeys(Id) NOT NULL
)

--Problem 2

INSERT INTO Planets ([Name])
VALUES
 ('Mars'),
 ('Earth'),
 ('Jupiter'),
 ('Saturn')

INSERT INTO Spaceships ([Name], Manufacturer, LightSpeedRate)
VALUES
 ('Golf', 'VW', 3),
 ('WakaWaka', 'Wakanda', 4),
 ('Falcon9', 'SpaceX', 1),
 ('Bed', 'Vidolov', 6)

--Problem 3

UPDATE Spaceships
   SET LightSpeedRate += 1
 WHERE Id >= 8 AND Id <= 12

--Problem 4

DELETE FROM TravelCards
      WHERE JourneyId IN (1,2,3)

DELETE FROM Journeys
      WHERE Id IN (1,2,3)

--Problem 5

SELECT Id, FORMAT(JourneyStart, 'dd/MM/yyyy') AS [JourneyStart], 
       FORMAT(JourneyEnd, 'dd/MM/yyyy') AS [JourneyEnd]
  FROM Journeys AS j
 WHERE Purpose = 'Military'
ORDER BY j.JourneyStart

--Problem 6

SELECT c.Id, CONCAT(c.FirstName, ' ', c.LastName) AS [full_name]
  FROM Colonists AS c
  JOIN TravelCards AS tc ON c.Id = tc.ColonistId
 WHERE tc.JobDuringJourney = 'Pilot'
ORDER BY c.Id

--Problem 7

SELECT COUNT(c.Id)
  FROM Colonists AS c
  JOIN TravelCards AS tc ON c.Id = tc.ColonistId
  JOIN Journeys AS j ON tc.JourneyId = j.Id
 WHERE j.Purpose = 'Technical'

--Problem 8

SELECT s.Name, s.Manufacturer
  FROM Spaceships AS s
  JOIN Journeys AS j ON s.Id = j.SpaceshipId
  JOIN TravelCards AS tc ON j.Id = tc.JourneyId
  JOIN Colonists AS c ON tc.ColonistId = c.Id
 WHERE tc.JobDuringJourney = 'Pilot'
   AND DATEDIFF(YEAR, c.BirthDate, '2019-01-01') < 30
ORDER BY s.Name

--Problem 9

SELECT p.Name AS [PlanetName], COUNT(j.Id) AS [JourneysCount]
  FROM Planets AS p
  JOIN Spaceports AS s ON p.Id = s.PlanetId
  JOIN Journeys AS j ON s.Id = j.DestinationSpaceportId
GROUP BY p.Name
ORDER BY JourneysCount DESC, p.Name

--Problem 10

SELECT JobDuringJourney,
       FullName,
	   JobRank
  FROM
    (SELECT tc.JobDuringJourney AS [JobDuringJourney],
           CONCAT(c.FirstName, ' ', c.LastName) AS [FullName],
    	   RANK() OVER (PARTITION BY JobDuringJourney ORDER BY Birthdate ASC) AS JobRank
      FROM Colonists AS c
      JOIN TravelCards AS tc ON c.Id = tc.ColonistId)
	  AS ranked
   WHERE JobRank = 2

--Problem 11

GO

CREATE FUNCTION dbo.udf_GetColonistsCount(@PlanetName VARCHAR (30))
RETURNS INT
AS
BEGIN
  
     DECLARE @count INT = (SELECT COUNT(tc.Id)
	                         FROM Planets AS p
					    LEFT JOIN Spaceports AS sp ON p.Id = sp.PlanetId
						LEFT JOIN Journeys AS j ON sp.Id = j.DestinationSpaceportId
						LEFT JOIN TravelCards AS tc ON j.Id = tc.JourneyId
						    WHERE p.Name = @PlanetName)

	IF (@count IS NULL) RETURN 0

RETURN @count
END

--Problem 12

GO

CREATE PROCEDURE usp_ChangeJourneyPurpose(@JourneyId INT, @NewPurpose VARCHAR(11))
AS
BEGIN
     IF (@JourneyId NOT IN (SELECT Id FROM Journeys))
	    THROW 50001, 'The journey does not exist!', 1
     
	 IF (@NewPurpose = (SELECT Purpose FROM Journeys WHERE Id = @JourneyId))
	     THROW 50001, 'You cannot change the purpose!', 1

     UPDATE Journeys
	    SET Purpose = @NewPurpose
	  WHERE Id = @JourneyId

END
	  
 