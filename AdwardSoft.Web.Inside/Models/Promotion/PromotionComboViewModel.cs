using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PromotionComboDatatableViewModel))]
    public class PromotionComboViewModel
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Purchase Product")]
        [Remote(action: "CheckExist", controller: "PromotionCombo"
            , ErrorMessage = "Already exist Promo Product and Purchase Product", AdditionalFields = "PromotionId,PromoProductId,PurchaseProductIdCurrnet,PromoProductIdCurrent")]
        public int PurchaseProductId { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Promo Product")]
        public int PromoProductId { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Promo Type")]
        public short PromoType { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Promo Amount")]
        public decimal PromoAmount { get; set; }

        public int PurchaseProductIdCurrnet { get; set; }

        public int PromoProductIdCurrent { get; set; }
    }

    [ProtoContract]
    public class PromotionComboDatatableViewModel : PromotionComboViewModel
    {
        [ProtoMember(6)]
        public string PurchaseProductName { get; set; }
        [ProtoMember(7)]
        public string PromoProductName { get; set; }
    }
}
