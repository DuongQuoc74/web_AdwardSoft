using ProtoBuf;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class PriceListViewModel
    {
        [ProtoMember(1)]
        [Required]
        [Display(Name = "Ngày áp dụng")]
        public DateTime Date { get; set; }
        [ProtoIgnore]
        [Display(Name = "Ngày áp dụng")]
        public string DateStr { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Tiêu đề")]
        [Required]
        public string Title { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Trạng thái")]
        [Required]
        public short Status { get; set; } = 1;
    }
}
