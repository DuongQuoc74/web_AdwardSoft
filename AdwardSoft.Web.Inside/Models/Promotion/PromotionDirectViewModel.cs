using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PromotionDirectDatatableViewModel))]
    public class PromotionDirectViewModel
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        [Required]
        [Remote(action: "CheckExist", controller: "PromotionDirect"
            , ErrorMessage = "Already exist", AdditionalFields = "PromotionId,PurchaseProductIdCurrnet")]
        [Display(Name = "Purchase Product")]
        public int PurchaseProductId { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Promo Type")]
        public short PromoType { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Promo Amount")]
        public decimal PromoAmount { get; set; }

        public int PurchaseProductIdCurrnet { get; set; }
    }

    [ProtoContract]
    public class PromotionDirectDatatableViewModel : PromotionDirectViewModel
    {
        [ProtoMember(5)]
        public string PurchaseProductName { get; set; }

    }
}
