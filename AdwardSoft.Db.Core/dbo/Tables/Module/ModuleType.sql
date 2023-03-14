CREATE TABLE [dbo].[ModuleType]
(
	[ModuleId] INT NOT NULL , 
    [Type] TINYINT NOT NULL, -- Dựa Theo User → Type 
	CONSTRAINT FK_ModuleType_Module FOREIGN KEY (ModuleId) REFERENCES Module(Id), 
    PRIMARY KEY ([ModuleId], [Type])
)
