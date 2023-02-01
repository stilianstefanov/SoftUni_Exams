CREATE DATABASE Airport

GO

USE Airport

GO

--Problem 1

CREATE TABLE Passengers (
Id INT PRIMARY KEY IDENTITY,
FullName VARCHAR(100) NOT NULL UNIQUE,
Email VARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Pilots (
Id INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(30) NOT NULL UNIQUE,
LastName VARCHAR(30) NOT NULL UNIQUE,
Age TINYINT CHECK(Age >= 21 AND Age <= 62) NOT NULL,
Rating FLOAT CHECK(Rating >= 0.0 AND Rating <= 10.0)
)

CREATE TABLE AircraftTypes (
Id INT PRIMARY KEY IDENTITY,
TypeName VARCHAR(30) NOT NULL UNIQUE
)

CREATE TABLE Aircraft (
Id INT PRIMARY KEY IDENTITY,
Manufacturer VARCHAR(25) NOT NULL,
Model VARCHAR(30) NOT NULL,
[Year] INT NOT NULL,
FlightHours INT,
Condition CHAR(1) NOT NULL,
TypeId INT FOREIGN KEY REFERENCES AirCraftTypes(Id) NOT NULL
)

CREATE TABLE PilotsAircraft (
AircraftId INT FOREIGN KEY REFERENCES Aircraft(Id) NOT NULL,
PilotId INT FOREIGN KEY REFERENCES Pilots(Id) NOT NULL,
PRIMARY KEY(AircraftId, PilotId)
)

CREATE TABLE Airports (
Id INT PRIMARY KEY IDENTITY,
AirportName VARCHAR(70) NOT NULL UNIQUE,
Country VARCHAR(100) NOT NULL UNIQUE
)

CREATE TABLE FlightDestinations (
Id INT PRIMARY KEY IDENTITY,
AirportId INT FOREIGN KEY REFERENCES Airports(Id) NOT NULL,
[Start] DATETIME NOT NULL,
AircraftId INT FOREIGN KEY REFERENCES Aircraft(Id) NOT NULL,
PassengerId INT FOREIGN KEY REFERENCES Passengers(Id) NOT NULL,
TicketPrice DECIMAL(18, 2) DEFAULT(15.00) NOT NULL
)

GO

--Problem 2

INSERT INTO Passengers
SELECT CONCAT(FirstName, ' ', LastName), CONCAT(FirstName, LastName, '@gmail.com')
FROM Pilots
WHERE Id BETWEEN 5 AND 15

--Problem 3

UPDATE Aircraft
SET Condition = 'A'
WHERE Condition IN ('C', 'B')
AND (FlightHours IS NULL OR FlightHours <= 100)
AND [Year] >= 2013

--Problem 4

DELETE 
FROM FlightDestinations
WHERE PassengerId IN ( SELECT Id FROM Passengers WHERE LEN(FullName) <= 10)

DELETE
FROM Passengers
WHERE LEN(FullName) <= 10

--Problem 5

SELECT Manufacturer, Model, FlightHours, Condition
  FROM Aircraft
ORDER BY FlightHours DESC

--Problem 6

SELECT p.FirstName, p.LastName, a.Manufacturer, a.Model, a.FlightHours
  FROM Pilots AS p
  JOIN PilotsAircraft AS ap ON p.Id = ap.PilotId
  JOIN Aircraft AS a ON ap.AircraftId = a.Id
 WHERE a.FlightHours IS NOT NULL AND FlightHours <= 304
 ORDER BY a.FlightHours DESC, p.FirstName

--Problem 7

SELECT TOP(20) fd.Id, fd.[Start], p.FullName, a.AirportName, fd.TicketPrice
  FROM FlightDestinations AS fd
  JOIN Passengers AS p ON fd.PassengerId = p.Id
  JOIN Airports AS a ON fd.AirportId = a.Id
 WHERE DAY(fd.[Start]) % 2 = 0
ORDER BY fd.TicketPrice DESC, a.AirportName

--Problem 8

SELECT a.Id, a.Manufacturer, a.FlightHours, COUNT(*), ROUND(AVG(fd.TicketPrice), 2)
  FROM Aircraft AS a
  JOIN FlightDestinations AS fd ON a.Id = fd.AircraftId
  GROUP BY a.Manufacturer, a.FlightHours, a.Id
  HAVING COUNT(*) >= 2
  ORDER BY COUNT(*) DESC, a.Id

--Problem 9

SELECT p.FullName, COUNT(*), SUM(fd.TicketPrice)
  FROM Passengers AS p
  JOIN FlightDestinations AS fd ON p.Id = fd.PassengerId
  JOIN Aircraft AS a ON fd.AircraftId = a.Id
 WHERE p.FullName LIKE '_a%'
 GROUP BY p.FullName
 HAVING COUNT(*) > 1
 ORDER BY p.FullName

 --Problem 10

 SELECT ap.AirportName, fd.[Start], fd.TicketPrice, p.FullName, ac.Manufacturer, ac.Model
   FROM FlightDestinations AS fd
   JOIN Airports AS ap ON fd.AirportId = ap.Id
   JOIN Passengers AS p ON p.Id = fd.PassengerId
   JOIN Aircraft AS ac ON fd.AircraftId = ac.Id
  WHERE (DATEPART(HOUR,fd.[Start]) >= 6 AND DATEPART(HOUR,fd.[Start]) <= 20)
    AND fd.TicketPrice > 2500
ORDER BY ac.Model

--Problem 11
GO

CREATE FUNCTION udf_FlightDestinationsByEmail(@email VARCHAR(60))
RETURNS INT
BEGIN
    DECLARE @count INT = (SELECT COUNT(*) FROM Passengers AS p
	                             JOIN FlightDestinations AS fd ON p.Id = fd.PassengerId
								WHERE p.Email = @email
							 GROUP BY p.Email)

	IF (@count IS NULL) SET @count = 0
 RETURN @count
END

GO

SELECT dbo.udf_FlightDestinationsByEmail('Montacute@gmail.com')

GO

--Problem 12

CREATE PROCEDURE usp_SearchByAirportName (@airportName VARCHAR(70))
AS
BEGIN
     DECLARE @airportId INT = (SELECT Id FROM Airports WHERE AirportName = @airportName)

	 SELECT ap.AirportName, p.FullName,
	   CASE 
	         WHEN fd.TicketPrice <= 400 THEN 'Low'
	         WHEN fd.TicketPrice <= 1500 THEN 'Medium'
	         WHEN fd.TicketPrice >= 1501 THEN 'High'
	          END AS [LevelOfTicketPrice],
	   ac.Manufacturer, ac.Condition, [at].TypeName
	   FROM Airports AS ap
	   JOIN FlightDestinations AS fd ON ap.Id = fd.AirportId
	   JOIN Passengers AS p ON fd.PassengerId = p.Id
	   JOIN Aircraft AS ac ON fd.AircraftId = ac.Id
	   JOIN AircraftTypes AS [at] ON ac.TypeId = [at].Id
	  WHERE ap.Id = @airportId
   ORDER BY ac.Manufacturer, p.FullName
END

GO

EXEC usp_SearchByAirportName 'Sir Seretse Khama International Airport'

