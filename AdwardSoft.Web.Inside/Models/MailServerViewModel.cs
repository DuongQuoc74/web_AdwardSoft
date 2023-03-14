using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class MailServerViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required(ErrorMessage = "Email không được để trống")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Remote(controller: "MailServer",action: "IsExistEmail", ErrorMessage ="Email đã được sử dụng", AdditionalFields = "Id")]
        public string Email { get; set; }
        [ProtoMember(3)]
        [Required(ErrorMessage = "Tên dịch vụ không được để trống")]
        [Display(Name = "Tên dịch vụ")]
        public string Name { get; set; }
        [ProtoMember(4)]
        [Required(ErrorMessage = "SMTP hông được để trống")]
        [Display(Name = "SMTP")]
        public string SMTP { get; set; }
        [ProtoMember(5)]
        [Display(Name = "Tiêu chuẩn SSL")]
        public bool IsSSL { get; set; }
        [ProtoMember(6)]
        [Display(Name = "Chứng chỉ mặc định")]
        public bool IsDefaultCredential { get; set; }

        [ProtoMember(7)]
        [Display(Name = "Port")]
        [Required(ErrorMessage = "Port không được để trống")]
        public int Port { get; set; }

        [ProtoMember(8)]
        [Display(Name = "Mật khẩu dịch vụ")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MaxLength(32, ErrorMessage = "Không được vượt quá 32 ký tự")]
        public string Pwd { get; set; }


        public bool IsNew { get => Id == default(int); }
    }

    [ProtoContract]
    public class MailServerDetailViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Email { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public string SMTP { get; set; }

        [ProtoMember(5)]
        public bool IsSSL { get; set; }

        [ProtoMember(6)]
        public bool IsDefaultCredential { get; set; }

        [ProtoMember(7)]
        public int Port { get; set; }

        [ProtoMember(8)]
        public string Pwd { get; set; }

        [ProtoMember(9)]
        public short Type { get; set; }

        [ProtoMember(10)]
        public short Action { get; set; }

        [ProtoMember(11)]
        public string Title { get; set; }

        [ProtoMember(12)]
        public string Content { get; set; }

        [ProtoMember(13)]
        public bool IsActive { get; set; }
    }

}
