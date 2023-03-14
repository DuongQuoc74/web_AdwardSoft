using ProtoBuf;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PriceDetailDatatableViewModel))]
    public class PriceDetailViewModel
    {
        [ProtoMember(1)]
        [Required]
        [Display(Name = "Tên sản phẩm")]
        public int ProductId { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Khu vực")]
        public int LocationId { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Điểm giao hàng")]
        public int DeliveryPointId { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Vận chuyển")]
        public short DeliveryType { get; set; } = 1;
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Giá bán (Đồng/Tấn)")]
        public decimal Price { get; set; }
        [ProtoMember(6)]
        public DateTime Date { get; set; }
    }
    [ProtoContract]
    public class PriceDetailDatatableViewModel : PriceDetailViewModel
    {
        [ProtoIgnore]
        [Display(Name = "STT")]
        public int OrdinalNumber { get; set; }
        [ProtoMember(101)]
        public int Count { get; set; }
        [ProtoMember(102)]
        [Required]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }
        [ProtoMember(103)]
        [Required]
        [Display(Name = "Khu vực")]
        public string LocationName { get; set; }
        [ProtoMember(104)]
        [Required]
        [Display(Name = "Điểm giao hàng")]
        public string DeliveryPointName { get; set; }
        [ProtoMember(105)]
        [Required]
        [Display(Name = "Đơn vị tính")]
        public string UnitName { get; set; }
    }
}
