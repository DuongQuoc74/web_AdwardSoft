using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class PromotionAmountViewModel
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        public short Idx { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Purchase Amount")]
        public decimal PurchaseAmount { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Promo Type")]
        public short PromoType { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Promo Amount")]
        public decimal PromoAmount { get; set; }
    }
}
