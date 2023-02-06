CREATE DATABASE [WMS]

GO

USE [WMS]

GO

--Problem 1

CREATE TABLE Clients (
ClientId INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
Phone CHAR(12) NOT NULL
)

CREATE TABLE Mechanics (
MechanicId INT PRIMARY KEY IDENTITY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
[Address] VARCHAR(255) NOT NULL
)

CREATE TABLE Models (
ModelId INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Jobs (
JobId INT PRIMARY KEY IDENTITY,
ModelId INT FOREIGN KEY REFERENCES Models(ModelId) NOT NULL,
[Status] VARCHAR(11) DEFAULT('Pending') CHECK([Status] IN ('Pending', 'In Progress', 'Finished')) NOT NULL,
ClientId INT FOREIGN KEY REFERENCES Clients(ClientId) NOT NULL,
MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId),
IssueDate Date NOT NULL,
FinishDate Date
)

CREATE TABLE Orders (
OrderId INT PRIMARY KEY IDENTITY,
JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
IssueDate Date,
Delivered BIT DEFAULT(0) NOT NULL
)

CREATE TABLE Vendors (
VendorId INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Parts (
PartId INT PRIMARY KEY IDENTITY,
SerialNumber VARCHAR(50) NOT NULL UNIQUE,
[Description] VARCHAR(255),
Price MONEY CHECK(Price > 0) NOT NULL,
VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId) NOT NULL,
StockQty INT DEFAULT(0) CHECK(StockQty >= 0) NOT NULL
)

CREATE TABLE OrderParts (
OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) NOT NULL,
PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
Quantity INT DEFAULT(1) CHECK(Quantity > 0) NOT NULL,
PRIMARY KEY(OrderId, PartId)
)

CREATE TABLE PartsNeeded (
JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
Quantity INT DEFAULT(1) CHECK(Quantity > 0) NOT NULL,
PRIMARY KEY(JobId, PartId)
)

--Problem 2

INSERT INTO Clients (FirstName, LastName, Phone)
VALUES
 ('Teri', 'Ennaco', '570-889-5187'),
 ('Merlyn', 'Lawler', '201-588-7810'),
 ('Georgene', 'Montezuma', '925-615-5185'),
 ('Jettie', 'Mconnell', '908-802-3564'),
 ('Lemuel', 'Latzke', '631-748-6479'),
 ('Melodie', 'Knipp', '805-690-1682'),
 ('Candida', 'Corbley', '908-275-8357')

INSERT INTO Parts (SerialNumber, [Description], Price, VendorId)
VALUES
 ('WP8182119', 'Door Boot Seal', 117.86, 2),
 ('W10780048', 'Suspension Rod', 42.81, 1),
 ('W10841140', 'Silicone Adhesive', 6.77, 4),
 ('WPY055980', 'High Temperature Adhesive', 13.94, 3)

--Problem 3

UPDATE Jobs
   SET MechanicId = 3,
       [Status] = 'In Progress'
 WHERE [Status] = 'Pending'

--Problem 4

DELETE
  FROM OrderParts
 WHERE OrderId = 19

DELETE
  FROM Orders
 WHERE OrderId = 19

--Problem 5


 SELECT CONCAT(m.FirstName , ' ' , m.LastName) AS [Mechanic],
j.Status, j.IssueDate
  FROM Mechanics AS m
  JOIN Jobs AS j ON m.MechanicId = j.MechanicId
ORDER BY m.MechanicId, j.IssueDate, j.JobId

--Problem 6

SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [Client],
DATEDIFF(DAY, j.IssueDate, '2017-04-24') AS [Days going], [Status]
  FROM Clients AS c 
  JOIN Jobs AS j ON c.ClientId = j.ClientId
 WHERE j.Status <> 'Finished'
ORDER BY [Days going] DESC, c.ClientId

--Problem 7

SELECT CONCAT(m.FirstName, ' ', m.LastName) AS [Mechanic], AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS [Average Days]
  FROM Mechanics AS m
  JOIN Jobs AS j ON m.MechanicId = j.MechanicId
 WHERE j.Status = 'Finished'
GROUP BY m.MechanicId, CONCAT(m.FirstName, ' ', m.LastName)
ORDER BY m.MechanicId

--Problem 8

SELECT
    FirstName + ' ' + LastName AS Available
FROM Mechanics
WHERE MechanicId NOT IN
(
    SELECT MechanicId 
    FROM Jobs
    WHERE [Status] = 'In Progress'
)
ORDER BY MechanicId

--Problem 9

SELECT j.JobId, ISNULL(SUM(p.Price * op.Quantity), 0) AS [Total]
  FROM Jobs AS j 
  LEFT JOIN Orders AS o ON j.JobId = o.JobId
  LEFT JOIN OrderParts AS op ON o.OrderId = op.OrderId
  LEFT JOIN Parts AS p ON op.PartId = p.PartId
 WHERE j.Status = 'Finished'
GROUP BY j.JobId
ORDER BY SUM(p.Price * op.Quantity) DESC, j.JobId

--Problem 10

SELECT
    p.PartId,
    p.Description,
    SUM(pn.Quantity) AS Required,
    SUM(p.StockQty) AS InStock,
    ISNULL(SUM(t.Quantity), 0) AS Ordered
FROM Parts AS p
LEFT JOIN PartsNeeded AS pn ON p.PartId = pn.PartId
LEFT JOIN Jobs AS j ON pn.JobId = j.JobId
LEFT JOIN
    (
        SELECT PartId, Quantity        
        FROM Orders AS o
        JOIN OrderParts AS op ON o.OrderId = op.OrderId
        WHERE o.Delivered = 0    
    ) AS t ON p.PartId = t.PartId
WHERE j.Status <> 'Finished'
GROUP BY p.PartId, p.Description
HAVING SUM(pn.Quantity) > SUM(p.StockQty) + ISNULL(SUM(t.Quantity), 0)
ORDER BY p.PartId ASC

--Problem 11

GO

CREATE PROC usp_PlaceOrder
(@jobId INT, @serial VARCHAR(50), @quantity INT)
AS
BEGIN
IF (@jobId IN (SELECT JobId FROM Jobs WHERE Status = 'Finished')) THROW 50011, 'This job is not active!', 1
IF (@quantity <= 0) THROW 50012, 'Part quantity must be more than zero!', 1
IF (@jobId NOT IN (SELECT JobId FROM Jobs)) THROW 50013, 'Job not found!', 1
IF (@serial NOT IN (SELECT SerialNumber FROM Parts)) THROW 50014, 'Part not found!', 1

DECLARE @partId INT = (SELECT TOP(1) PartId FROM Parts WHERE SerialNumber = @serial)
DECLARE @orderId INT

IF (@jobId IN (SELECT JobId FROM Orders WHERE IssueDate IS NULL))
    BEGIN
    SET @OrderId = (SELECT TOP(1) OrderId FROM Orders WHERE JobId = @jobId)
    IF (@partId IN (SELECT PartId FROM OrderParts WHERE OrderId = @OrderId))
        BEGIN
        UPDATE OrderParts
            SET Quantity += @quantity 
            WHERE OrderId = @OrderId AND PartId = @partId
        RETURN
        END
    INSERT INTO OrderParts VALUES (@OrderId, @partId, @quantity)
    RETURN
    END

INSERT INTO Orders VALUES (@jobId, NULL, 0)
SET @orderId = (SELECT TOP(1) OrderId FROM Orders ORDER BY OrderId DESC)
INSERT INTO OrderParts VALUES (@OrderId, @partId, @quantity)
END

--Problem 12
GO

CREATE FUNCTION udf_GetCost (@JobId INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
 
 DECLARE @totalCost DECIMAL(18, 2) = (SELECT SUM(p.Price * op.Quantity) FROM Jobs AS j
                                      LEFT JOIN Orders AS o ON j.JobId = o.JobId
									  LEFT JOIN OrderParts AS op ON o.OrderId = op.OrderId
									  LEFT JOIN Parts AS p ON op.PartId = p.PartId
									 WHERE j.JobId = @JobId)

IF (@totalCost IS NULL) RETURN 0.00

RETURN @totalCost

END

GO

SELECT dbo.udf_GetCost(3) 


