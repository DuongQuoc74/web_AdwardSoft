--/*
--Post-Deployment Script Template							
----------------------------------------------------------------------------------------
-- This file contains SQL statements that will be appended to the build script.		
-- Use SQLCMD syntax to include a file in the post-deployment script.			
-- Example:      :r .\myfile.sql								
-- Use SQLCMD syntax to reference a variable in the post-deployment script.		
-- Example:      :setvar TableName MyTable							
--               SELECT * FROM [$(TableName)]					
----------------------------------------------------------------------------------------
--*/
USE [AdsDL.Core]
GO
SET IDENTITY_INSERT [dbo].[Module] ON 
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (1, N'Tài khoản', N'#', N'fas fa-users', N'#', 1, 1, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (2, N'Quản lý tài khoản', N'User', N'', N'User', 1, 2, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (3, N'Quản lý nhóm tài khoản', N'Role', N'', N'Role', 1, 3, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (4, N'Sản phẩm', N'#', N'fas fa-cube', N'#', 4, 4, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (5, N'Quản lý kho ', N'Stock', N'', N'Stock', 4, 5, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (6, N'Tồn kho đầu kỳ', N'BeginingStock', N'', N'BeginingStock', 4, 8, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (7, N'Quản lý phân loại sản phẩm', N'Category', N'', N'Category', 4, 9, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (8, N'Quản lý sản phẩm', N'Product', N'', N'Product', 4, 10, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (9, N'Quản lý giá bán', N'PriceList', N'', N'PriceList', 4, 6, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (10, N'Nhà phân phối', N'#', N'fas fa-shipping-fast', N'#', 10, 11, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (11, N'Quản lý nhà phân phối', N'Customer', N'', N'Customer', 10, 12, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (12, N'Quản lý loại hình phân phối', N'CustomerGroup', N'', N'CustomerGroup', 10, 13, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (13, N'Quản lý trạng thái hoạt động', N'#', N'', N'#', 10, 14, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (14, N'Quản lý hạn mức tín dụng', N'#', N'', N'#', 10, 15, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (16, N'Điều hành', N'#', N'fas fa-layer-group', N'#', 16, 16, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (17, N'Quản lý điểm giao hàng', N'DeliveryPoint', N'', N'DeliveryPoint', 16, 17, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (19, N'Quản lý phương tiện giao hàng', N'DeliveryVehicle', N'', N'DeliveryVehicle', 16, 18, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (20, N'Quản lý đơn hàng bán', N'Order', N'', N'Order', 16, 19, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (21, N'Quản lý đơn hàng nội bộ', N'Order/Internal', N'', N'Order', 16, 20, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (22, N'Quản lý khuyến mãi, chiết khấu', N'Promotion', N'', N'Promotion', 16, 21, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (23, N'Quản lý thông tin thanh toán', N'CustomerBank', N'', N'CustomerBank', 16, 22, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (24, N'Quản lý nạp tiền', N'CustomerRecharge', N'', N'CustomerRecharge', 16, 23, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (25, N'Giao nhận', N'#', N'fas fa-boxes', N'#', 25, 24, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (26, N'Phiếu xuất kho', N'WarehouseExport', N'', N'WarehouseExport', 25, 25, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (27, N'Phiếu nhập kho', N'WarehouseReceipt', N'', N'WarehouseReceipt', 25, 26, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (28, N'Kiểm tra đơn hàng', N'Order/Trackking', N'', N'Order', 25, 27, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (29, N'Phiếu cân xe', N'WeightSlip', N'', N'WeightSlip', 25, 28, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (30, N'Báo cáo', N'#', N'fas fa-tachometer-alt', N'#', 30, 29, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (31, N'Báo cáo tổng quan', N'Report/Overview', N'', N'Report', 30, 30, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (32, N'Báo cáo tình hình hoạt động', N'Report/Activity', N'', N'Report', 30, 31, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (33, N'Báo cáo nhập xuất tồn', N'Report/StockSummary', N'', N'Report', 30, 32, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (34, N'Thống kê nhà phân phối', N'Report/CustomerSummary', N'', N'Report', 30, 33, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (35, N'Sổ theo dõi công nợ', N'Report/CustomerStatement', N'', N'Report', 30, 34, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (36, N'Hệ thống', N'#', N'fas fa-cogs', N'#', 36, 35, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (37, N'Thông tin hệ thống', N'Setting', N'', N'Setting', 36, 38, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (38, N'Thiết lập Email Server', N'MailServer', N'', N'MailServer', 36, 39, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (39, N'Cài đặt thông báo', N'Notification', N'', N'Notification', 36, 40, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (40, N'Sơ đồ tổ chức', N'OrgChart', N'', N'OrgChart', 36, 44, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (41, N'Chức vụ', N'Position', N'', N'Position', 36, 45, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (42, N'Khu vực', N'Location', N'', N'Location', 36, 46, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (43, N'Tài khoản ngân hàng', N'BankAccount', N'', N'BankBankAccount', 36, 47, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (44, N'Hình thức thanh toán', N'PaymentMethod', N'', N'PaymentMethod', 36, 48, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (45, N'Đơn vị tính', N'Unit', N'', N'Unit', 36, 49, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (46, N'Cài đặt chứng từ', N'ReceiptSetting', N'', N'ReceiptSetting', 36, 41, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (47, N'Sao lưu số liệu', N'DataBackup', N'', N'DataBackup', 36, 42, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (48, N'Lịch sử hoạt động', N'Log', N'', N'Log', 36, 43, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (49, N'Loại phương tiện', N'VehicleType', N'', N'VehicleType', 36, 36, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (50, N'QR Code', N'#', N'', N'Product', 4, 7, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (51, N'Cập nhật thông tin nhà phân phối', N'Distributor', N'', N'Distributor', 36, 37, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (52, N'Đơn hàng', N'#', N'fas fa-file-invoice-dollar', N'', 52, 50, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (53, N'Đặt hàng', N'CustomerOrder', N'', N'CustomerOrder', 52, 51, 0)
GO
INSERT [dbo].[Module] ([Id], [Title], [Link], [ClassName], [ControllerName], [ParentId], [Sort], [IsPublic]) VALUES (54, N'Thanh toán', N'CustomerOrder/PayOrder', N'', N'CustomerOrder', 52, 52, 0)
GO
SET IDENTITY_INSERT [dbo].[Module] OFF
GO

GO
INSERT [dbo].[Language] ([Code], [Name], [IsDefault]) VALUES ('VI', N'Tiếng Việt', '1')
GO
INSERT [dbo].[Language] ([Code], [Name], [IsDefault]) VALUES ('EN', 'English', '0')
GO
