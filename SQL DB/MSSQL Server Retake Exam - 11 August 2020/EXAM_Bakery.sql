CREATE DATABASE Bakery

GO

USE Bakery

GO

--Problem 1

CREATE TABLE Countries (
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(50) UNIQUE
)

CREATE TABLE Customers (
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(25),
LastName NVARCHAR(25),
Gender CHAR(1) CHECK(Gender = 'M' OR GENDER = 'F'),
Age INT,
PhoneNumber CHAR(10),
CountryId INT FOREIGN KEY REFERENCES Countries(Id)
)

CREATE TABLE Products (
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(25) UNIQUE,
[Description] NVARCHAR(250),
Recipe NVARCHAR(MAX),
Price MONEY CHECK(Price >= 0)
)

CREATE TABLE Feedbacks (
Id INT PRIMARY KEY IDENTITY,
[Description] NVARCHAR(255),
Rate DECIMAL(5,2) CHECK(Rate BETWEEN 0 AND 10),
ProductId INT FOREIGN KEY REFERENCES Products(Id),
CustomerId INT FOREIGN KEY REFERENCES Customers(Id)
)

CREATE TABLE Distributors (
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(25) UNIQUE,
AddressText NVARCHAR(30),
Summary NVARCHAR(200),
CountryId INT FOREIGN KEY REFERENCES Countries(Id)
)

CREATE TABLE Ingredients (
Id INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(30),
[Description] NVARCHAR(200),
OriginCountryId INT FOREIGN KEY REFERENCES Countries(Id),
DistributorId INT FOREIGN KEY REFERENCES Distributors(Id)
)

CREATE TABLE ProductsIngredients (
ProductId INT FOREIGN KEY REFERENCES Products(Id) NOT NULL,
IngredientId INT FOREIGN KEY REFERENCES Ingredients(Id) NOT NULL,
PRIMARY KEY(ProductId, IngredientId)
)

--Problem 2

INSERT INTO Distributors (Name, CountryId, AddressText, Summary)
VALUES
 ('Deloitte & Touche',	2,	'6 Arch St #9757',	'Customizable neutral traveling'),
 ('Congress Title',	    13,	'58 Hancock St',	'Customer loyalty'),
 ('Kitchen People',	    1,	'3 E 31st St #77',	'Triple-buffered stable delivery'),
 ('General Color Co Inc',21,	'6185 Bohn St #72',	'Focus group'),
 ('Beck Corporation',	23,	'21 E 64th Ave',	'Quality-focused 4th generation hardware')

INSERT INTO Customers(FirstName, LastName, Age, Gender, PhoneNumber, CountryId)
VALUES
 ('Francoise',   'Rautenstrauch',15,	'M',	'0195698399',	5),
 ('Kendra',	    'Loud',      	22,	'F',	'0063631526',	11),
 ('Lourdes',	    'Bauswell',	    50,	'M',	'0139037043',	8),
 ('Hannah',	    'Edmison',	    18,	'F',	'0043343686',	1),
 ('Tom',	        'Loeza',        31,	'M',	'0144876096',	23),
 ('Queenie',	    'Kramarczyk',	30,	'F',	'0064215793',	29),
 ('Hiu',	        'Portaro',	    25,	'M',	'0068277755',	16),
 ('Josefa',	    'Opitz',	    43,	'F',	'0197887645',	17)

--Problem 3

UPDATE Ingredients
   SET DistributorId = 35
 WHERE [Name] IN ('Bay Leaf', 'Paprika', 'Poppy')

 UPDATE Ingredients
    SET OriginCountryId = 14
  WHERE OriginCountryId = 8 

--Problem 4

DELETE FROM Feedbacks
      WHERE CustomerId = 14 OR ProductId = 5

--Problem 5

SELECT [Name], Price, [Description]
  FROM Products
ORDER BY Price DESC, [Name] 

--Problem 6

SELECT f.ProductId, f.Rate, f.Description, f.CustomerId, c.Age, c.Gender
  FROM Feedbacks AS f
LEFT JOIN Customers AS c ON f.CustomerId = c.Id
 WHERE f.Rate < 5.00
ORDER BY f.ProductId DESC, f.Rate

--Problem 7

SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [CustomerName], c.PhoneNumber, c.Gender
  FROM Customers AS c
LEFT JOIN Feedbacks AS f ON c.Id = f.CustomerId
 WHERE f.Id IS NULL
ORDER BY c.Id

--Problem 8

SELECT FirstName, Age, PhoneNumber
  FROM Customers
 WHERE (Age >= 21 AND FirstName LIKE '%an%')
    OR (PhoneNumber LIKE '________38' AND CountryId <> 31)
ORDER BY FirstName, Age DESC

--Problem 9

SELECT d.Name, i.Name, p.Name, AVG(f.Rate) AS [AvarageRate]
  FROM Distributors AS d            
  JOIN Ingredients AS i ON d.Id = i.DistributorId
  JOIN ProductsIngredients AS pii ON i.Id = pii.IngredientId
  JOIN Products AS p ON pii.ProductId = p.Id
  JOIN Feedbacks AS f ON p.Id = f.ProductId
GROUP BY d.Name, i.Name, p.Name
HAVING AVG(f.Rate) BETWEEN 5 AND 8
ORDER BY d.Name, i.Name, p.Name

--Problem 10

SELECT
    CountryName,
    DistributorName
FROM
(
    SELECT
        c.Name AS CountryName, 
        d.Name AS DistributorName,             
        RANK() OVER (PARTITION BY c.Name ORDER BY COUNT(i.Id) DESC) AS Rank    
    FROM Countries AS c
    LEFT JOIN Distributors AS d ON c.Id = d.CountryId
    LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId    
    GROUP BY c.Name, d.Name
) AS rankedTable
WHERE Rank = 1
ORDER BY CountryName, DistributorName

--Problem 11

GO

CREATE VIEW v_UserWithCountries
AS
SELECT 
    CONCAT(cu.FirstName, ' ', cu.LastName) AS CustomerName,
    cu.Age,
    cu.Gender,
    co.Name AS CountryName
FROM Customers AS cu
JOIN Countries AS co ON cu.CountryId = co.Id

--Problem 12

Go

CREATE TRIGGER tr_DeleteProducts
ON Products 
INSTEAD OF DELETE
AS
BEGIN
DECLARE @productId INT = (SELECT Id FROM deleted)
DELETE FROM Feedbacks WHERE ProductId = @productId
DELETE FROM ProductsIngredients WHERE ProductId = @productId
DELETE FROM Products WHERE Id = @productId
END



	
