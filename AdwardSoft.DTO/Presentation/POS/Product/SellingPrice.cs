using ProtoBuf;
using System;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SellingPriceDatatable))]
    public class SellingPrice
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        public decimal WholesalePrice { get; set; }
        [ProtoMember(4)]
        public decimal RetailPrice { get; set; }
        [ProtoMember(5)]
        public int UnitId { get; set; }
    }
    [ProtoContract]
    public class SellingPriceDatatable : SellingPrice
    {
        [ProtoMember(6)]
        public int Count { get; set; }
        [ProtoMember(7)]
        public string UnitName { get; set; }
    }
}
