using Microsoft.AspNetCore.Mvc;
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
    public class LanguageViewModel
    {
        [ProtoMember(1)]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(2, ErrorMessage = "Không vượt quá 2 ký tự")]
        [DisplayName("Mã ngôn ngữ")]
        [Remote(action: "CheckDuplicateCode", controller: "Language", ErrorMessage = "Mã ngôn ngữ đã tồn tại")]
        public string Code { get; set; }

        [ProtoMember(2)]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(50, ErrorMessage = "Không vượt quá 50 ký tự")]
        [DisplayName("Tên ngôn ngữ")]
        public string Name { get; set; }

        [ProtoMember(3)]
        public bool IsDefault { get; set; }
    }
}
