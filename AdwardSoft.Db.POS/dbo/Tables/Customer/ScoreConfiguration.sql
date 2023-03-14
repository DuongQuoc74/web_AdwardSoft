CREATE TABLE [dbo].[ScoreConfiguration]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [EffectiveDate] DATE NOT NULL, 
    [FromAmount] NUMERIC(16, 2) NULL, 
    [ToAmount] NUMERIC(16, 2) NULL, 
    [FromPoint] NUMERIC(9) NULL, 
    [ToPoint] NUMERIC(9) NULL
)
