CREATE DATABASE Boardgames

GO

USE Boardgames

GO

--Problem 1

CREATE TABLE Categories (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
)

CREATE TABLE Addresses (
Id INT PRIMARY KEY IDENTITY,
StreetName NVARCHAR(100) NOT NULL,
StreetNumber INT NOT NULL,
Town VARCHAR(30) NOT NULL,
Country VARCHAR(50) NOT NULL,
ZIP INT NOT NULL
)

CREATE TABLE Publishers (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(30) UNIQUE NOT NULL,
AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL,
Website NVARCHAR(40),
Phone NVARCHAR(20),
)

CREATE TABLE PlayersRanges (
Id INT PRIMARY KEY IDENTITY,
PlayersMin INT NOT NULL,
PlayersMax INT NOT NULL
)

CREATE TABLE Boardgames (
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30) NOT NULL,
YearPublished INT NOT NULL,
Rating DECIMAL(18,2) NOT NULL,
CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
PublisherId INT FOREIGN KEY REFERENCES Publishers(Id) NOT NULL,
PlayersRangeId INT FOREIGN KEY REFERENCES PlayersRanges(Id) NOT NULL
)

CREATE TABLE Creators (
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
Email NVARCHAR(30) NOT NULL
)

CREATE TABLE CreatorsBoardgames (
CreatorId INT FOREIGN KEY REFERENCES Creators(Id) NOT NULL,
BoardgameId INT FOREIGN KEY REFERENCES Boardgames(Id) NOT NULL,
PRIMARY KEY(CreatorId, BoardgameId)
)

--Problem 2

INSERT INTO Publishers (Name, AddressId, Website, Phone)
VALUES
 ('Agman Games', 5, 'www.agmangames.com', '+16546135542'),
 ('Amethyst Games', 7, 'www.amethystgames.com', '+15558889992'),
 ('BattleBooks', 13, 'www.battlebooks.com', '+12345678907')

INSERT INTO Boardgames (Name, YearPublished, Rating, CategoryId, PublisherId, PlayersRangeId)
VALUES
 ('Deep Blue', 2019, 5.67, 1, 15, 7),
 ('Paris', 2016, 9.78, 7, 1, 5),
 ('Catan: Starfarers', 2021, 9.87, 7, 13, 6),
 ('Bleeding Kansas', 2020, 3.25, 3, 7, 4),
 ('One Small Step', 2019, 5.75, 5, 9, 2)

--Problem 3

UPDATE PlayersRanges
   SET PlayersMax += 1
 WHERE PlayersMax = 2
   AND PlayersMin = 2

UPDATE Boardgames
   SET Name = CONCAT(Name, 'V2')
 WHERE YearPublished >= 2020

--Problem 4
DELETE
  FROM CreatorsBoardgames
 WHERE BoardgameId IN (SELECT Id FROM Boardgames WHERE PublisherId IN (SELECT Id FROM Publishers WHERE AddressId IN (SELECT Id FROM Addresses WHERE LEFT(Town, 1) = 'L')))

DELETE
  FROM Boardgames
 WHERE PublisherId IN (SELECT Id FROM Publishers WHERE AddressId IN (SELECT Id FROM Addresses WHERE LEFT(Town, 1) = 'L'))

DELETE
  FROM Publishers
 WHERE AddressId IN (SELECT Id FROM Addresses WHERE LEFT(Town, 1) = 'L')

DELETE
  FROM Addresses
 WHERE LEFT(Town, 1) = 'L'

 --Problem 5

 SELECT Name, Rating
 FROM Boardgames
ORDER BY YearPublished ASC, Name DESC

--Problem 6

SELECT b.Id, b.Name, b.YearPublished, c.Name
  FROM Boardgames AS b
  JOIN Categories AS c ON b.CategoryId = c.Id
 WHERE c.Name IN ('Strategy Games', 'Wargames')
ORDER BY b.YearPublished DESC


--Problem 7

SELECT c.Id, CONCAT(c.FirstName, ' ', c.LastName) AS [CreatorName], c.Email
  FROM Creators AS c
LEFT JOIN CreatorsBoardgames AS cb ON c.Id = cb.CreatorId
WHERE cb.CreatorId IS NULL
ORDER BY CreatorName
--Problem 8

SELECT TOP(5) b.Name, b.Rating, c.Name
  FROM Boardgames AS b
  JOIN PlayersRanges AS p ON b.PlayersRangeId = p.Id
  JOIN Categories AS c ON b.CategoryId = c.Id
 WHERE (Rating > 7.00 AND b.Name LIKE '%a%')
    OR (Rating > 7.50 AND p.PlayersMin = 2 AND p.PlayersMax = 5)
ORDER BY b.Name, b.Rating DESC

--Problem 9

SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [FullName], c.Email, MAX(b.Rating) AS [Rating]
  FROM Creators AS c
  JOIN CreatorsBoardgames AS cb ON c.Id = cb.CreatorId
  JOIN Boardgames AS b ON cb.BoardgameId = b.Id
 WHERE c.Email LIKE '%.com'
GROUP BY CONCAT(c.FirstName, ' ', c.LastName), c.Email

--Problem 10

SELECT c.LastName, CEILING(AVG(b.Rating)) AS [AverageRating], p.Name
  FROM Creators AS c
  JOIN CreatorsBoardgames AS cb ON c.Id = cb.CreatorId
  JOIN Boardgames AS b ON cb.BoardgameId = b.Id
  JOIN Publishers AS p ON b.PublisherId = p.Id
 WHERE p.Name = 'Stonemaier Games'
GROUP BY c.LastName, p.Name
ORDER BY AVG(b.Rating) DESC

--Problem 11
GO

CREATE FUNCTION udf_CreatorWithBoardgames(@name NVARCHAR(30))
RETURNS INT
AS 
BEGIN
  DECLARE @count INT = (SELECT COUNT(cb.CreatorId) 
                                FROM Creators AS c 
								LEFT JOIN CreatorsBoardgames AS cb ON c.Id = cb.CreatorId
								WHERE c.FirstName = @name)
 IF (@count IS NULL) RETURN 0

 RETURN @count
END

--Problem 12
GO

CREATE PROCEDURE usp_SearchByCategory(@category VARCHAR(50))
AS
BEGIN
  SELECT b.Name, b.YearPublished, b.Rating, c.Name AS [CategoryName], p.Name AS [PublisherName], 
         CONCAT(pr.PlayersMin, ' people') AS [MinPlayers],
		 CONCAT(pr.PlayersMax, ' people') AS [MaxPlayers]
    FROM Boardgames AS b
	JOIN Categories AS c ON b.CategoryId = c.Id
	JOIN Publishers AS p ON b.PublisherId = p.Id
	JOIN PlayersRanges AS pr ON b.PlayersRangeId = pr.Id
    WHERE c.Name = @category
  ORDER BY p.Name ASC, b.YearPublished DESC

END

