using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class ProductVoucher
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Code { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public int UnitId { get; set; }
        [ProtoMember(5)]
        public int StockId { get; set; }
        [ProtoMember(6)]
        public List<SellingPrice> SellingPrices { get; set; }
        [ProtoMember(7)]
        public List<Select> Units { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(ProductData))]
    public class Product
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Code { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Image { get; set; }
        [ProtoMember(5)]
        public decimal MinStock { get; set; }
        [ProtoMember(6)]
        public decimal MaxStock { get; set; }
        [ProtoMember(7)]
        public int UnitId { get; set; }
        [ProtoMember(8)]
        public int StockId { get; set; }
        [ProtoMember(9)]
        public int PriceType { get; set; }
        [ProtoMember(10)]
        public bool IsTrade { get; set; }
        [ProtoMember(11)]
        public DateTime ModifyDate { get; set; }
        [ProtoMember(12)]
        public string Description { get; set; }
        [ProtoMember(13)]
        public int Status { get; set; }
        [ProtoMember(14)]
        public decimal Profit { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(ProductDatatable))]
    [ProtoInclude(200, typeof(ProductRedis))]
    public class ProductData : Product
    {
        [ProtoMember(15)]
        public int CategoryId { get; set; }
        [ProtoMember(16)]
        public bool IsDefault { get; set; }
        [ProtoMember(17)]
        public decimal WholesalePrice { get; set; }
        [ProtoMember(18)]
        public decimal RetailPrice { get; set; }
        
    }

    [ProtoContract]
    public class ProductDatatable : ProductData
    {
        [ProtoMember(80)]
        public int Count { get; set; }

        [ProtoMember(81)]
        public string CategoryName { get; set; }
        [ProtoMember(82)]
        public string UnitName { get; set; }
    }


    public class ProductMoible
    {
        public ProductData Product { get; set; }
        public List<SellingPriceUnit> UnitConverters { get; set; }
    }
    public class ProductRedis : ProductData
    {
        [AdsKey]
        public new int Id { get; set; }
        [AdsKey]
        public new int UnitId { get; set; }
        [AdsKey]
        public new int StockId { get; set; }

    }

    public class ProductElastic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    [ProtoContract]
    public class ProductUnitSearch
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Code { get; set; }
        [ProtoMember(4)]
        public List<Select> Units { get; set; }
    }

    [ProtoContract]
    public class ProductPrint
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Code { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public decimal RetailPrice1 { get; set; }
        [ProtoMember(5)]
        public decimal RetailPrice2 { get; set; }
        [ProtoMember(6)]
        public decimal RetailPrice3 { get; set; }
        [ProtoMember(7)]
        public string UnitName1 { get; set; }
        [ProtoMember(8)]
        public string UnitName2 { get; set; }
        [ProtoMember(9)]
        public string UnitName3 { get; set; }
        [ProtoMember(10)]
        public string CategoryName { get; set; }
    }

    [ProtoContract]
    public class SellingPriceUnit
    {
        [ProtoMember(1)]

        public decimal QuantityFrom { get; set; }
        [ProtoMember(2)]

        public decimal QuantityTo { get; set; }
        [ProtoMember(3)]

        public decimal WholesalePrice { get; set; }
        [ProtoMember(4)]

        public decimal RetailPrice { get; set; }
        [ProtoMember(5)]

        public int UnitId { get; set; }
        [ProtoMember(6)]

        public int ProductId { get; set; }
    }
}
