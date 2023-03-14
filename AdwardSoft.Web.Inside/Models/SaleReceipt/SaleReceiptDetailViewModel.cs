using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SaleReceiptDetailDataTableViewModel))]
    public class SaleReceiptDetailViewModel
    {
        [ProtoMember(1)]
        [Display(Name = "Sale Receipt")]
        public string SaleReceiptId { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }
        [ProtoMember(5)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [ProtoMember(6)]
        [Display(Name = "Promotion")]
        public int PromotionId { get; set; }
        [ProtoMember(7)]
        [Display(Name = "Discount")]
        public decimal Discount { get; set; }
        [ProtoMember(8)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        [ProtoMember(9)]
        [Display(Name = "Promo")]
        public bool IsPromo { get; set; }
        [ProtoMember(10)]
        public decimal RetailPrice { get; set; }
    }

    [ProtoContract]
    public class SaleReceiptDetailDataTableViewModel : SaleReceiptDetailViewModel
    {
        [ProtoMember(11)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [ProtoMember(12)]
        [Display(Name = "Promotion Name")]
        public string PromotionName { get; set; }
        [ProtoMember(13)]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
    }
}
