CREATE TABLE [dbo].[Employee]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(70) NOT NULL, 
    [Address] NVARCHAR(128) NOT NULL, 
    [Phone] VARCHAR(20) NOT NULL, 
    [IdentityCode] VARCHAR(20) NOT NULL,  -- Business License (GPKD), Identity Card (CMND), Citizenship Card (CCCD), Passport (Hộ chiếu)
    [Email] VARCHAR(256) NOT NULL, 
    [GenderId] INT NOT NULL, 
    [DepartmentId] INT NOT NULL, 
    [PositionId] INT NOT NULL, 
    [DoB] DATE NOT NULL, 
    [Avatar] VARCHAR(2048) NULL, 
    [Code] VARCHAR(10) NOT NULL, 
    [PlaceOfBirth] NVARCHAR(150) NOT NULL, 
    [Nationality] NVARCHAR(20) NOT NULL, 
    [Religious] TINYINT NOT NULL, -- 0 : Không, 1 : Phật giáo, 2 : Công giáo, 3 : Khác 
    [PermanentAddress] NVARCHAR(150) NOT NULL, 
    [CurrentAddress] NVARCHAR(150) NOT NULL, 
    [LeavingDate] DATE NULL, 
    [StartingDate] DATE NOT NULL, 
    [Status] TINYINT NOT NULL DEFAULT 0, -- 0 : Không, 1 : Nghỉ
    CONSTRAINT [FK_Employee_Gender] FOREIGN KEY ([GenderId]) REFERENCES [Gender]([Id]), 
    CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DepartmentId]) REFERENCES [Department]([Id]), 
    CONSTRAINT [FK_Employee_Position] FOREIGN KEY ([PositionId]) REFERENCES [Position]([Id])
)
