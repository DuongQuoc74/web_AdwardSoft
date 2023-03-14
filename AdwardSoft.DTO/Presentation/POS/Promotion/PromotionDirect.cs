using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PromotionDirectDatatable))]
    public class PromotionDirect
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        public int PurchaseProductId { get; set; }
        [ProtoMember(3)]
        public short PromoType { get; set; }
        [ProtoMember(4)]
        public decimal PromoAmount { get; set; }
    }

    [ProtoContract]
    public class PromotionDirectDatatable : PromotionDirect
    {
        [ProtoMember(5)]
        public string PurchaseProductName { get; set; }

    }
}
