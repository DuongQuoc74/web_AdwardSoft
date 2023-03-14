using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class VehicleTypeViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required(ErrorMessage = "Tên loại không được để trống")]
        [Display(Name = "Tên loại")]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 1;
    }
}
