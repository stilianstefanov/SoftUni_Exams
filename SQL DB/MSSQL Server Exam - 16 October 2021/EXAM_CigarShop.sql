CREATE DATABASE CigarShop

GO

USE CigarShop

GO

--Problem 1

CREATE TABLE Sizes (
Id INT PRIMARY KEY IDENTITY,
[Length] INT CHECK([Length] >= 10 AND [Length] <= 25) NOT NULL,
RingRange DECIMAL(2,1) CHECK(RingRange >= 1.5 AND RingRange <= 7.5) NOT NULL
)

CREATE TABLE Tastes (
Id INT PRIMARY KEY IDENTITY,
TasteType VARCHAR(20) NOT NULL,
TasteStrength VARCHAR(15) NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Brands (
Id INT PRIMARY KEY IDENTITY,
BrandName VARCHAR(30) NOT NULL UNIQUE,
BrandDescription VARCHAR(MAX)
)

CREATE TABLE Cigars (
Id INT PRIMARY KEY IDENTITY,
CigarName VARCHAR(80) NOT NULL,
BrandId INT FOREIGN KEY REFERENCES Brands(Id) NOT NULL,
TastId INT FOREIGN KEY REFERENCES Tastes(Id) NOT NULL,
SizeId INT FOREIGN KEY REFERENCES Sizes(Id) NOT NULL,
PriceForSingleCigar MONEY NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Addresses (
Id INT PRIMARY KEY IDENTITY,
Town VARCHAR(30) NOT NULL,
Country NVARCHAR(30) NOT NULL,
Streat NVARCHAR(100) NOT NULL,
ZIP VARCHAR(20) NOT NULL
)

CREATE TABLE Clients (
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
Email NVARCHAR(50) NOT NULL,
AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL
)

CREATE TABLE ClientsCigars (
ClientId INT FOREIGN KEY REFERENCES Clients(Id) NOT NULL,
CigarId INT FOREIGN KEY REFERENCES Cigars(Id) NOT NULL,
PRIMARY KEY(ClientId, CigarId)
)

--Problem 2

INSERT INTO Cigars
VALUES
 ('COHIBA ROBUSTO', 9, 1, 5, 15.50, 'cohiba-robusto-stick_18.jpg'),
 ('COHIBA SIGLO I', 9, 1, 10, 410.00, 'cohiba-siglo-i-stick_12.jpg'),
 ('HOYO DE MONTERREY LE HOYO DU MAIRE', 14, 5, 11, 7.50, 'hoyo-du-maire-stick_17.jpg'),
 ('HOYO DE MONTERREY LE HOYO DE SAN JUAN', 14, 4, 15, 32.00, 'hoyo-de-san-juan-stick_20.jpg'),
 ('TRINIDAD COLONIALES', 2, 3, 8, 85.21, 'trinidad-coloniales-stick_30.jpg')

INSERT INTO Addresses
VALUES 
 ('Sofia', 'Bulgaria', '18 Bul. Vasil levski', '1000'),
 ('Athens', 'Greece', '4342 McDonald Avenue', '10435'),
 ('Zagreb', 'Croatia', '4333 Lauren Drive', '10000')

--Problem 3

UPDATE Cigars
   SET PriceForSingleCigar += PriceForSingleCigar * 0.20
 WHERE TastId = 1

UPDATE Brands
   SET BrandDescription = 'New description'
 WHERE BrandDescription IS NULL

--Problem 4

DELETE
  FROM Clients
 WHERE AddressId IN (SELECT Id FROM Addresses WHERE LEFT(Country, 1) = 'C')

DELETE
  FROM Addresses
 WHERE LEFT(Country, 1) = 'C'

--Problem 5

SELECT CigarName, PriceForSingleCigar, ImageURL
  FROM Cigars
ORDER BY PriceForSingleCigar, CigarName DESC

--Problem 6

SELECT c.Id, CigarName, PriceForSingleCigar, TasteType, TasteStrength
  FROM Cigars AS c
  JOIN Tastes AS t ON c.TastId = t.Id
 WHERE t.TasteType IN ('Earthy', 'Woody')
ORDER BY PriceForSingleCigar DESC

--Problem 7

SELECT c.Id, CONCAT(c.FirstName, ' ', c.LastName) AS [ClientName], c.Email
  FROM Clients AS c
  LEFT JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
 WHERE cc.ClientId IS NULL
ORDER BY ClientName

--Problem 8

SELECT TOP(5) c.CigarName, c.PriceForSingleCigar, c.ImageURL
  FROM Cigars AS c
  JOIN Sizes AS s ON c.SizeId = s.Id
 WHERE s.[Length]  >= 12 
  AND (c.CigarName LIKE '%ci%' OR c.PriceForSingleCigar > 50)
  AND s.RingRange > 2.55
ORDER BY c.CigarName, c.PriceForSingleCigar DESC

--Problem 9

SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [FullName], a.Country, a.ZIP, CONCAT('$', MAX(cg.PriceForSingleCigar))
  FROM Clients AS c
  JOIN Addresses AS a ON c.AddressId = a.Id
  JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
  JOIN Cigars AS cg ON cc.CigarId = cg.Id
 WHERE ISNUMERIC(a.ZIP) = 1
 GROUP BY a.Country, a.ZIP, CONCAT(c.FirstName, ' ', c.LastName)
 ORDER BY CONCAT(c.FirstName, ' ', c.LastName)

--Problem 10

SELECT c.LastName, ROUND(AVG(s.[Length]), 0), CEILING(AVG(s.RingRange))
 FROM Clients AS c
 JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
 JOIN Cigars AS cg ON cc.CigarId = cg.Id
 JOIN Sizes AS s ON cg.SizeId = s.Id
GROUP BY c.LastName
ORDER BY AVG(s.[Length]) DESC

--Problem 11

GO

CREATE FUNCTION udf_ClientWithCigars(@name NVARCHAR(30))
RETURNS INT
BEGIN
     DECLARE @countOfCigars INT = (SELECT COUNT(*) FROM Clients AS c
	                                    JOIN ClientsCigars AS cc ON c.Id = cc.ClientId 
										WHERE c.FirstName = @name)
	 
	 IF (@countOfCigars IS NULL) SET @countOfCigars = 0

RETURN @countOfCigars
END

GO

SELECT dbo.udf_ClientWithCigars('Betty')

GO

--Problem 12

CREATE PROCEDURE usp_SearchByTaste(@taste VARCHAR(20))
AS
BEGIN
     
	 SELECT c.CigarName, CONCAT('$', c.PriceForSingleCigar) AS [Price],
	 t.TasteType, b.BrandName, CONCAT(s.[Length], ' cm') AS [CigarLength],
	 CONCAT(s.RingRange, ' cm') AS [CigarRingRange]
	   FROM Cigars AS c 
	   JOIN Tastes AS t ON c.TastId = t.Id
	   JOIN Brands AS b ON c.BrandId = b.Id
	   JOIN Sizes AS s ON c.SizeId = s.Id
	  WHERE t.TasteType = @taste
	  ORDER BY s.[Length], s.RingRange DESC

END

EXEC usp_SearchByTaste 'Woody' 


