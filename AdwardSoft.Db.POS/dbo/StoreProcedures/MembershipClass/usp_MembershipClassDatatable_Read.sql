CREATE PROCEDURE [dbo].[usp_MembershipClassDatatable_Read]
AS
	SELECT *
	FROM [dbo].[MembershipClass]
	ORDER BY [Level] ASC
