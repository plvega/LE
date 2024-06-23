CREATE PROCEDURE [dbo].[spUsers_Register]
@userName nvarchar(16),
@firstName nvarchar(50),
@lastName nvarchar(50),
@password nvarchar(16)

AS
BEGIN

SET NOCOUNT ON;

INSERT INTO dbo.Users
(UserName, FirstName, LastName, Password)
VALUES (@userName, @firstName, @lastName, @password)

END