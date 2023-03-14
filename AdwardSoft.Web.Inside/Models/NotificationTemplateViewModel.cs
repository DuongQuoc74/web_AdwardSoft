using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class NotificationTemplateViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Hình thức gửi")]
        public short Type { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Chức năng")]
        public short Action { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Title { get; set; }
        [ProtoMember(5)]
        [Display(Name = "Nội dung")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Content { get; set; }
        [ProtoMember(6)]
        [Display(Name = "Trạng thái hoạt động")]
        public bool IsActive { get; set; }
        [ProtoMember(7)]
        [Display(Name = "Mail server")]
        public int MailServerId { get; set; }

        public bool IsNew { get => Id == default(int); }
    }
}
