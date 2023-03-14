using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class PositionViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Tên chức vụ")]
        [Required]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}
