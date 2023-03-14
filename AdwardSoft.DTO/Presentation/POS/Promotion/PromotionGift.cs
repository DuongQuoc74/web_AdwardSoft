using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PromotionGiftDatatable))]
    public class PromotionGift
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        public int PurchaseProductId { get; set; }
        [ProtoMember(3)]
        public decimal PurchaseQuantity { get; set; }
        [ProtoMember(4)]
        public int GiftProductId { get; set; }
        [ProtoMember(5)]
        public decimal GiftQuantity { get; set; }
    }

    [ProtoContract]
    public class PromotionGiftDatatable : PromotionGift
    {
        [ProtoMember(6)]
        public string PurchaseProductName { get; set; }
        [ProtoMember(7)]
        public string GiftProductName { get; set; }

    }
}
