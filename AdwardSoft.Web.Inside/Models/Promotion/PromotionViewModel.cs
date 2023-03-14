using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class PromotionViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Type")]
        public short Type { get; set; }
        public string EffectiveDateStr { get; set; }
        public string ExpiryDateStr { get; set; }
    }
}
