using ProtoBuf;
using System.ComponentModel.DataAnnotations;


namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(DeliveryPointDatatableViewModel))]
    public class DeliveryPointViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Required(ErrorMessage = "Mã không được để trống")]
        [RegularExpression(@"^[A-Z0-9]*$", ErrorMessage = "Nhập mã in hoa và không có ký tự đặc biệt")]
        [Display(Name = "Mã")]
        public string Code { get; set; }

        [ProtoMember(3)]
        [Required(ErrorMessage = "Điểm giao hàng không được để trống")]
        [Display(Name = "Điểm giao hàng")]
        public string Name { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Hệ số")]
        public int Rate { get; set; }

        [ProtoMember(5)]
        [Display(Name = "Trực thuộc")]
        public int LocationId { get; set; }

        [ProtoMember(6)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 1;
    }
     [ProtoContract]
    public class DeliveryPointDatatableViewModel : DeliveryPointViewModel
    {
        [ProtoMember(101)]
        public int Count { get; set; }
        [ProtoMember(102)]
        [Display(Name = "Trực thuộc")]
        public string LocationName { get; set; }
    }

}
