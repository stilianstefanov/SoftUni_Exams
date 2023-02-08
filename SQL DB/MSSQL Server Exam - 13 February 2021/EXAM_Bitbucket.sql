CREATE DATABASE Bitbucket

GO

USE Bitbucket

GO

--Problem 1

CREATE TABLE Users (
Id INT PRIMARY KEY IDENTITY,
Username VARCHAR(30) NOT NULL,
[Password] VARCHAR(30) NOT NULL,
Email VARCHAR(50) NOT NULL
)

CREATE TABLE Repositories (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE RepositoriesContributors (
RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
PRIMARY KEY(RepositoryId, ContributorId)
)

CREATE TABLE Issues (
Id INT PRIMARY KEY IDENTITY,
Title VARCHAR(255) NOT NULL,
IssueStatus VARCHAR(6) NOT NULL,
RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
AssigneeId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Commits (
Id INT PRIMARY KEY IDENTITY,
[Message] VARCHAR(255) NOT NULL,
IssueId INT FOREIGN KEY REFERENCES Issues(Id),
RepositoryId INT FOREIGN KEY REFERENCES Repositories(Id) NOT NULL,
ContributorId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL
)

CREATE TABLE Files (
Id INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(100) NOT NULL,
Size DECIMAL(18, 2) NOT NULL,
ParentId INT FOREIGN KEY REFERENCES Files(Id),
CommitId INT FOREIGN KEY REFERENCES Commits(Id) NOT NULL
)

--Problem 2

INSERT INTO Files (Name, Size, ParentId, CommitId)
VALUES
 ('Trade.idk', 2598.00, 1, 1),
 ('menu.net', 9238.31, 2, 2),
 ('Administrate.soshy', 1246.93, 3, 3),
 ('Controller.php', 7353.15, 4, 4),
 ('Find.java', 9957.86, 5, 5),
 ('Controller.json', 14034.87, 3, 6),
 ('Operate.xix', 7662.92, 7, 7)

INSERT INTO Issues (Title, IssueStatus, RepositoryId, AssigneeId)
VALUES
 ('Critical Problem with HomeController.cs file', 'open', 1, 4),
 ('Typo fix in Judge.html', 'open', 4, 3),
 ('Implement documentation for UsersService.cs', 'closed', 8, 2),
 ('Unreachable code in Index.cs', 'open', 9, 8)

 --Problem 3

 UPDATE Issues
    SET IssueStatus = 'closed'
  WHERE AssigneeId = 6

--Problem 4

DELETE
  FROM RepositoriesContributors
 WHERE RepositoryId = (SELECT Id FROM Repositories
                        WHERE [Name] = 'Softuni-Teamwork')

DELETE
  FROM Issues
 WHERE RepositoryId = (SELECT Id FROM Repositories
                        WHERE [Name] = 'Softuni-Teamwork')

--Problem 5

SELECT id, Message, RepositoryId, ContributorId
  FROM Commits
ORDER BY Id, Message, RepositoryId, ContributorId

--Problem 6

SELECT id, Name, Size
  FROM Files
 WHERE Size > 1000 
   AND Name LIKE '%html%'
ORDER BY Size DESC, id, Name

--Problem 7

SELECT i.Id, CONCAT(u.Username, ' : ', i.Title) AS [IssueAssignee]
  FROM Issues AS i
INNER JOIN Users AS u ON i.AssigneeId = u.Id
ORDER BY i.Id DESC, IssueAssignee

--Problem 8

SELECT Id, Name, CONCAT(Size, 'KB') 
  FROM Files
 WHERE Id NOT IN (SELECT ParentId FROM Files WHERE ParentId IS NOT NULL)
ORDER BY Id, Name, Size DESC


--Problem 9

SELECT TOP(5) r.Id, r.Name, COUNT(c.Id) AS [Commits]
  FROM Repositories AS r
LEFT JOIN Commits AS c ON r.Id = c.RepositoryId
LEFT JOIN RepositoriesContributors AS rc ON r.Id = rc.RepositoryId
GROUP BY r.Id, r.Name
ORDER BY Commits DESC, r.Id, r.Name

--Problem 10

SELECT u.Username, AVG(f.Size) AS [Size]
  FROM Users AS u
INNER JOIN Commits AS c ON u.Id = c.ContributorId
INNER JOIN Files AS f ON f.CommitId = c.Id
GROUP BY u.Username
ORDER BY [Size] DESC, u.Username

--Problem 11

GO

CREATE FUNCTION udf_AllUserCommits(@username VARCHAR(30))
RETURNS INT
AS
BEGIN
   DECLARE @commitsCount INT = (SELECT COUNT(c.Id) FROM Users AS u
                                    LEFT JOIN Commits AS c ON u.Id = c.ContributorId
									    WHERE u.Username = @username) 

RETURN @commitsCount
END

--Problem 12

GO

CREATE PROCEDURE usp_SearchForFiles(@fileExtension VARCHAR(20))
AS
BEGIN
  SELECT id, Name, CONCAT(Size, 'KB') AS [Size]
    FROM Files
   WHERE Name LIKE '%' + @fileExtension
ORDER BY Id, Name, Size DESC

END