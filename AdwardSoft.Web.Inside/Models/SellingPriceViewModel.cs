using ProtoBuf;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SellingPriceDatatableViewModel))]
    public class SellingPriceViewModel
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Wholesale Price")]
        public decimal WholesalePrice { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Retail Price")]
        public decimal RetailPrice { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }
        public bool IsNew { get; set; }
    }
    [ProtoContract]
    public class SellingPriceDatatableViewModel : SellingPriceViewModel
    {
        [ProtoMember(6)]
        public int Count { get; set; }
        [ProtoMember(7)]
        public string UnitName { get; set; }
    }
}
