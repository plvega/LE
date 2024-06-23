CREATE PROCEDURE [dbo].[spUsers_Authenticate]
@username nvarchar(16),
@password nvarchar(16)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [Id], [UserName], [FirstName], [LastName], [Password]
    FROM dbo.Users
    WHERE UserName = @username
    AND Password = @password;
END