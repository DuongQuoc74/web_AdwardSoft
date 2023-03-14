using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class BranchViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required(ErrorMessage ="Tên đơn vị bắt buộc nhập")]
        [Display(Name = "Tên đơn vị")]
        [MaxLength(120)]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required(ErrorMessage = "Địa chỉ bắt buộc nhập")]
        [Display(Name = "Địa chỉ")]
        [MaxLength(128)]
        public string Address { get; set; }
        [ProtoMember(4)]
        [Required(ErrorMessage = "Số điện thoại bắt buộc nhập")]
        [Display(Name = "Số điện thoại")]
        [MaxLength(12)]
        public string Tel { get; set; }
        [ProtoMember(5)]
        [Required(ErrorMessage = "Email bắt buộc nhập")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không hợp lệ")]
        [EmailAddress]
        [MaxLength(254)]
        public string Email { get; set; }
        [ProtoMember(6)]
        [Required(ErrorMessage = "Đơn vị trực thuộc bắt buộc nhập")]
        [DisplayName("Đơn vị trực thuộc")]
        public int ParentId { get; set; }
        [ProtoMember(7)]
        public int Sort { get; set; }
    }
}
