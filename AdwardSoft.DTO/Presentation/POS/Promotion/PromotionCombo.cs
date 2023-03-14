using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PromotionComboDatatable))]
    public class PromotionCombo
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        public int PurchaseProductId { get; set; }
        [ProtoMember(3)]
        public int PromoProductId { get; set; }
        [ProtoMember(4)]
        public short PromoType { get; set; }
        [ProtoMember(5)]
        public decimal PromoAmount { get; set; }
    }

    [ProtoContract]
    public class PromotionComboDatatable : PromotionCombo
    {
        [ProtoMember(6)]
        public string PurchaseProductName { get; set; }
        [ProtoMember(7)]
        public string PromoProductName { get; set; }
    }
}
