using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ProductUnitConverterDatatable))]
    public class ProductUnitConverter
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }
        [ProtoMember(2)]
        public int UnitId { get; set; }
        [ProtoMember(3)]
        public decimal QuantityFrom { get; set; }
        [ProtoMember(4)]
        public decimal QuantityTo { get; set; }
    }

    [ProtoContract]
    public class ProductUnitConverterDatatable : ProductUnitConverter
    {
        [ProtoMember(5)]
        public string ProductName { get; set; }
        [ProtoMember(6)]
        public string UnitName { get; set; }
    }

    public class ProductUnitConverterRedis
    {
        [AdsKey]
        public int ProductId { get; set; }
        [AdsKey]
        public int UnitId { get; set; }
        public decimal QuantityFrom { get; set; }
        public decimal QuantityTo { get; set; }
    }
}
