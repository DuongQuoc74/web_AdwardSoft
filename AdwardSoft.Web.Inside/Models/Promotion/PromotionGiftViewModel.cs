using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PromotionGiftDatatableViewModel))]
    public class PromotionGiftViewModel
    {
        [ProtoMember(1)]
        public int PromotionId { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Purchase Product")]
        [Remote(action: "CheckExist", controller: "PromotionGift"
            , ErrorMessage = "Already exist", AdditionalFields = "PromotionId,GiftProductId,PurchaseProductIdCurrnet,GiftProductIdCurrent")]
        public int PurchaseProductId { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Purchase Quantity")]
        public decimal PurchaseQuantity { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Gift Product")]
        public int GiftProductId { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Gift Quantity")]
        public decimal GiftQuantity { get; set; }
        public int PurchaseProductIdCurrnet { get; set; }
        public int GiftProductIdCurrent { get; set; }
    }

    [ProtoContract]
    public class PromotionGiftDatatableViewModel : PromotionGiftViewModel
    {
        [ProtoMember(6)]
        public string PurchaseProductName { get; set; }
        [ProtoMember(7)]
        public string GiftProductName { get; set; }

    }
}
