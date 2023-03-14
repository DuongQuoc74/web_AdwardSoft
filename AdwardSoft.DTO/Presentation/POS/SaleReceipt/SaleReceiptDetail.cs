using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SaleReceiptDetailDataTable))]
    public class SaleReceiptDetail
    {
        [ProtoMember(1)]
        public string SaleReceiptId { get; set; }
        [ProtoMember(2)]
        public string SaleReceiptDetailId { get; set; }
        [ProtoMember(3)]
        public int ProductId { get; set; }
        [ProtoMember(4)]
        public decimal Quantity { get; set; }
        [ProtoMember(5)]
        public int UnitId { get; set; }
        [ProtoMember(6)]
        public decimal Price { get; set; }
        [ProtoMember(7)]
        public int PromotionId { get; set; }
        [ProtoMember(8)]
        public decimal Discount { get; set; }
        [ProtoMember(9)]
        public decimal Amount { get; set; }
        [ProtoMember(10)]
        public bool IsPromo { get; set; }
        [ProtoMember(11)]
        public decimal RetailPrice { get; set; }
        [ProtoMember(12)]
        public string CardPinCode { get; set; }
        [ProtoMember(13)]
        public string CardTelco { get; set; }
        [ProtoMember(14)]
        public string CardSerial { get; set; }
        [ProtoMember(15)]
        public decimal CardAmount { get; set; }
        [ProtoMember(16)]
        public string CardTrace { get; set; }
    }

    [ProtoContract]
    public class SaleReceiptDetailDataTable : SaleReceiptDetail
    {
        [ProtoMember(17)]
        public string ProductName { get; set; }
        [ProtoMember(18)]
        public string PromotionName { get; set; }
        [ProtoMember(19)]
        public string ProductCode { get; set; }
    }
    [ProtoContract]
    public class SaleReceiptDetailHistory : SaleReceiptDetail
    {
        [ProtoMember(17)]
        public string ProductName { get; set; }
        [ProtoMember(18)]
        public string PromotionName { get; set; }
        [ProtoMember(19)]
        public string ProductCode { get; set; }
        [ProtoMember(20)]
        public string UnitName { get; set; }
    }
}
