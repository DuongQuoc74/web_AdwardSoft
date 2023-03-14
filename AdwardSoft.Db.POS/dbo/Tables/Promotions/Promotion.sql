CREATE TABLE [dbo].[Promotion]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(128) NOT NULL, 
    [EffectiveDate] DATE NOT NULL, -- Ngày hiệu lực
    [ExpiryDate] DATE NOT NULL, -- Ngày hết hạn
    [Type] TINYINT NOT NULL, -- 0 : Donation with purchase (Mua hàng tặng hàng), 1 : Discount on total value (Giảm giá trên tổng giá trị đơn hàng), 2 : Disount on product (Giảm giá trực tiếp trên sản phẩm), 3 : Coupon Code (Mã giảm giá), 4 : Discount for combo (Giảm giá mua hàng kết hợp) 
-- Mã giảm giá (chỉ khả dụng với Type = 3)
    [Status] TINYINT NOT NULL -- Trạng thái: 1: Đang hoạt động, 0: Không khả dụng
, 
    [ApplicableObj] TINYINT NOT NULL -- Đối tượng áp dụng: 0: Tất cả nhà phân phối, 1: Theo đối tượng chỉ định
, 
    [Quantity] INT NOT NULL -- Số lượng sử dụng: mặc định = 0 là không giới hạn
    )
