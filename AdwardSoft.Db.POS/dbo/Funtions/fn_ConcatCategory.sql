CREATE FUNCTION [dbo].[fn_ConcatCategory]
(
 @Id SYSNAME
)
RETURNS NVARCHAR(MAX)
WITH SCHEMABINDING
AS
BEGIN
 DECLARE @s NVARCHAR(MAX);

 SELECT @s = COALESCE(@s + N', ', N'') + im.[Name] 
 FROM [dbo].[ProductCategory] i
	INNER JOIN [dbo].[Category] im ON i.[CategoryId] = im.[Id]	     
	WHERE  i.[ProductId] = @Id
	GROUP BY i.[ProductId],im.[Name] 
 RETURN (@s);
END