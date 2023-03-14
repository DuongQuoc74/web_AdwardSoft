CREATE TABLE [dbo].[ReceiptSetting]
(
	[Code] CHAR(2) NOT NULL PRIMARY KEY,
	[Sign] VARCHAR(60) NOT NULL, -- Ký hiệu
	[Name] NVARCHAR(120) NOT NULL, -- Tên chứng từ
	[Prefix] VARCHAR(10) NOT NULL, --Tiền tố
	[Suffix] VARCHAR(10) NULL, -- Hậu tố
	[StartNo] INT, -- Giá trị bắt đầu
	[NoC] INT, -- Number Of Char: Tổng ký tự phần số
	[AppliedDate] DATE, -- Ngày áp dụng
	[Note] NVARCHAR(300), -- Ghi chú 
    [CurrentIdx] INT NULL

)
