CREATE TABLE [dbo].[PriceDetail]
(
    [ProductId] INT NOT NULL, 
    [LocationId] INT NOT NULL, 
    [DeliveryPointId] INT NOT NULL, 
    [Date] DATE NOT NULL,
    [DeliveryType] TINYINT NOT NULL, [Price] NUMERIC(16, 2) NOT NULL, 
    --0: Đường Thủy, 1: Đường Bộ, 2: Đường hàng không, 3: Đường sắt
    PRIMARY KEY ([DeliveryType], [DeliveryPointId], [LocationId], [ProductId], [Date])
)
