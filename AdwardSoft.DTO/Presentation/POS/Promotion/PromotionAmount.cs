using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class PromotionAmount
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        public short Idx { get; set; }
        [ProtoMember(3)]
        public decimal PurchaseAmount { get; set; }
        [ProtoMember(4)]
        public short PromoType { get; set; }
        [ProtoMember(5)]
        public decimal PromoAmount { get; set; }
    }
}
