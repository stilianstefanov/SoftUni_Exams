CREATE DATABASE TripService

GO

USE TripService

GO

--Problem 1

CREATE TABLE Cities (
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(20) NOT NULL,
CountryCode CHAR(2) NOT NULL
)

CREATE TABLE Hotels (
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30) NOT NULL,
CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
EmployeeCount INT NOT NULL,
BaseRate DECIMAL(18,2)
)

CREATE TABLE Rooms (
Id INT PRIMARY KEY IDENTITY,
Price DECIMAL(18,2) NOT NULL,
[Type] NVARCHAR(20) NOT NULL,
Beds INT NOT NULL,
HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL
)

CREATE TABLE Trips (
Id INT PRIMARY KEY IDENTITY,
RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL,
BookDate DATE NOT NULL,
ArrivalDate DATE NOT NULL,
ReturnDate DATE NOT NULL,
CancelDate DATE,
CHECK(BookDate < ArrivalDate),
CHECK(ArrivalDate < ReturnDate)
)

CREATE TABLE Accounts (
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(50) NOT NULL,
MiddleName NVARCHAR(20),
LastName NVARCHAR(50) NOT NULL,
CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
BirthDate DATE NOT NULL,
Email VARCHAR(100) NOT NULL UNIQUE
)

CREATE TABLE AccountsTrips (
AccountId INT FOREIGN KEY REFERENCES Accounts(Id) NOT NULL,
TripId INT FOREIGN KEY REFERENCES Trips(Id) NOT NULL,
Luggage INT NOT NULL CHECK(Luggage >= 0),
PRIMARY KEY(AccountId, TripId)
)

--Problem 2

INSERT INTO Accounts (FirstName, MiddleName, LastName, CityId, BirthDate, Email)
VALUES
 ('John',	    'Smith',	'Smith',	34,	'1975-07-21', 'j_smith@gmail.com'),
 ('Gosho',	    NULL,	    'Petrov',	11,	'1978-05-16', 'g_petrov@gmail.com'),
 ('Ivan',	    'Petrovich','Pavlov',	59,	'1849-09-26', 'i_pavlov@softuni.bg'),
 ('Friedrich',	'Wilhelm',	'Nietzsche', 2,	'1844-10-15', 'f_nietzsche@softuni.bg')

INSERT INTO Trips (RoomId, BookDate, ArrivalDate, ReturnDate, CancelDate)
VALUES
 (101,	'2015-04-12',	'2015-04-14',	'2015-04-20',	'2015-02-02'),
 (102,	'2015-07-07',   '2015-07-15',	'2015-07-22',	'2015-04-29'),
 (103,	'2013-07-17',	'2013-07-23',	'2013-07-24',	NULL),
 (104,	'2012-03-17',	'2012-03-31',	'2012-04-01',	'2012-01-10'),
 (109,	'2017-08-07',	'2017-08-28',	'2017-08-29',	NULL)

--Problem 3

UPDATE Rooms
   SET Price += Price * 0.14
 WHERE HotelId IN (5, 7, 9)

--Problem 4

DELETE FROM AccountsTrips
      WHERE AccountId = 47

--Problem 5

SELECT a.FirstName, a.LastName, FORMAT(a.BirthDate, 'MM-dd-yyyy'), c.Name, a.Email
  FROM Accounts AS a
  JOIN Cities AS c ON a.CityId = c.Id
 WHERE Email LIKE 'e%'
ORDER BY c.Name

--Problem 6

SELECT c.Name, COUNT(h.Id) AS [Hotels]
  FROM Cities AS c
  JOIN Hotels AS h ON c.Id = h.CityId
GROUP BY c.Name
ORDER BY Hotels DESC, c.Name

--Problem 7

SELECT a.Id AS [AccountId],
       CONCAT(a.FirstName, ' ', a.LastName) AS [FullName],
	   MAX(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)) AS [LongestTrip],
	   MIN(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)) AS [ShortestTrip]
  FROM Accounts AS a
  JOIN AccountsTrips AS att ON a.Id = att.AccountId
  JOIN Trips AS t ON att.TripId = t.Id
WHERE a.MiddleName IS NULL AND t.CancelDate IS NULL
GROUP BY a.Id, CONCAT(a.FirstName, ' ', a.LastName)
ORDER BY LongestTrip DESC, ShortestTrip ASC

--Problem 8

SELECT TOP(10) c.Id, c.Name, c.CountryCode, COUNT(a.Id) AS [Accounts]
  FROM Cities AS c
  JOIN Accounts AS a ON c.Id = a.CityId
GROUP BY c.Id, c.Name, c.CountryCode
ORDER BY Accounts DESC

--Problem 9

SELECT a.Id, a.Email, ch.Name, COUNT(t.Id) AS [Trips]
  FROM Accounts AS a 
  JOIN AccountsTrips AS att ON a.Id = att.AccountId
  JOIN Trips AS t ON att.TripId = t.Id
  JOIN Rooms AS r ON t.RoomId = r.Id
  JOIN Hotels AS h ON r.HotelId = h.Id
  JOIN Cities AS ch ON h.CityId = ch.Id
 WHERE a.CityId = ch.Id
GROUP BY a.Id, a.Email, ch.Name
ORDER BY Trips DESC, a.Id

--Problem 10

SELECT t.Id,
       CASE
	   WHEN a.MiddleName IS NULL THEN CONCAT(a.FirstName, ' ', a.LastName)
	   WHEN a.MiddleName IS NOT NULL THEN CONCAT(a.FirstName, ' ', a.MiddleName, ' ', a.LastName)
	   END AS [Full Name],
	   c.Name AS [From],
	   ch.Name AS [To],
	   CASE
	   WHEN t.CancelDate IS NOT NULL THEN 'Canceled'
	   WHEN t.CancelDate IS NULL THEN CAST(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate) AS VARCHAR) + ' days'
	   END AS [Duration]
  FROM Trips AS t
  JOIN AccountsTrips AS att ON t.Id = att.TripId
  JOIN Accounts AS a ON att.AccountId = a.Id
  JOIN Cities AS c ON a.CityId = c.Id
  JOIN Rooms AS r ON t.RoomId = r.Id
  JOIN Hotels AS h ON r.HotelId = h.Id
  JOIN Cities AS ch ON h.CityId = ch.Id
ORDER BY [Full Name], t.Id


--Problem 11

GO

CREATE FUNCTION udf_GetAvailableRoom
(@HotelId INT, @Date DATE, @People INT)
RETURNS VARCHAR(255)
AS
BEGIN
DECLARE @result VARCHAR(255) = (
SELECT 'Room ' + CAST(temp.RoomId AS varchar) + ': ' + temp.RoomType + ' (' + CAST(temp.Beds AS varchar) + ' beds) - $' + CAST(temp.TotalPrice AS varchar)
FROM
(
    SELECT TOP 1 
        r.Id AS RoomId, 
        r.[Type] AS RoomType,
        r.Beds AS Beds,
        (r.Price + h.BaseRate) * @People AS TotalPrice
    FROM Rooms AS r
    JOIN Trips AS t ON r.Id = t.RoomId
    JOIN Hotels AS h ON r.HotelId = h.Id
    WHERE         
        HotelId = @HotelId 
        AND Beds >= @People
        AND r.Id NOT IN
        (
            SELECT RoomId FROM Trips
            WHERE @Date BETWEEN ArrivalDate AND ReturnDate AND CancelDate IS NULL
        )
    ORDER BY TotalPrice DESC) AS temp
)

RETURN ISNULL(@result, 'No rooms available')
END

--Problem 12
GO

CREATE PROC usp_SwitchRoom
(@TripId INT, @TargetRoomId INT)
AS
BEGIN
DECLARE @roomHotelId INT = 
(
    SELECT h.Id FROM Hotels AS h
    JOIN Rooms AS r ON h.Id = r.HotelId
    WHERE r.Id = @TargetRoomId
)
DECLARE @tripHotelId INT =
(
    SELECT h.Id FROM Hotels AS h
    JOIN Rooms AS r ON h.Id = r.HotelId
    JOIN Trips AS t ON r.Id = t.RoomId
    WHERE t.Id = @TripId
)
DECLARE @people INT = 
(
    SELECT COUNT(*) FROM Accounts AS a
    JOIN AccountsTrips AS at ON a.Id = at.AccountId
    JOIN Trips AS t ON at.TripId = t.Id
    WHERE t.Id = @TripId
)
IF (@roomHotelId <> @tripHotelId) THROW 50001, 'Target room is in another hotel!', 1
IF ((SELECT Beds FROM Rooms WHERE Id = @TargetRoomId) < @people) THROW 50002, 'Not enough beds in target room!', 1

UPDATE Trips
SET RoomId = @TargetRoomId
WHERE Id = @TripId
END
