CREATE PROCEDURE [dbo].[usp_Contact_Read]
	@Status TINYINT
AS
BEGIN
	SELECT *
	FROM [Contact]
	WHERE [Status] = @Status
	ORDER BY [Date] DESC, [Status]
END
