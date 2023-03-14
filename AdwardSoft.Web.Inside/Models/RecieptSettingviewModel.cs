using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public class RecieptSettingviewModel : Controller
    {
        [ProtoContract]
        public class recieptSettingPrintViewModel
        {
            [ProtoMember(1)]
            public int Id { get; set; }

            [ProtoMember(2)]
            public string Sign { get; set; }

            [ProtoMember(3)]
            public string Name { get; set; }
            [ProtoMember(4)]
            public string Prefix { get; set; }
            [ProtoMember(5)]
            public string Suffix { get; set; }
            [ProtoMember(6)]
            public int StartNo { get; set; }
            [ProtoMember(7)]
            public int NoC { get; set; }
            [ProtoMember(8)]
            public int AppliedDate { get; set; }
            [ProtoMember(9)]
            public string Note { get; set; }
        }
        public class RecieptSettingViewModel
        {
            [ProtoMember(1)]
            public int Id { get; set; }

            [ProtoMember(2)]

            [Display(Name = "Mã Chứng Tư")]
            [RegularExpression(@"^[A-Z0-9]*$", ErrorMessage = "Nhập mã in hoa và không có ký tự đặc biệt")]
            [StringLength(maximumLength: 13, MinimumLength = 5, ErrorMessage = "Mã phải có từ 5 đến 13 ký tự.")]
            [Remote(action: "CheckSign", controller: "Reciept", AdditionalFields = "Id")]
            //[Required]
            public string Sign { get; set; }

            [ProtoMember(3)]
            [Display(Name = "Tên Chứng Từ")]
            [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Không được chứa ký tự đặc biệt")]
            [Required(ErrorMessage = "Không được để trống")]
            [MaxLength(120)]
            public string Name { get; set; }

            [ProtoMember(4)]
            [Display(Name = "Tiền Tố")]
            [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Không được bỏ trống tiền tố !")]
            [Required(ErrorMessage = "Không được để trống")]
            [MaxLength(120)]
            public string Prefix { get; set; }

            [ProtoMember(5)]
            [Display(Name = "Hậu Tố")]
            [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Không được bỏ trống hậu tố !")]
            [Required(ErrorMessage = "Không được để trống")]
            [MaxLength(120)]
            public string Suffix { get; set; }

            [ProtoMember(6)]
            [Required]            
            public int StartNo { get; set; }

            [ProtoMember(7)]
            [Required]
            public int NoC { get; set; }

            [ProtoMember(8)]
            [Display(Name = "Ngày áp dụng ")]                   
            public DateTime AppliedDate { get; set; }

            [ProtoMember(9)]
            [StringLength(50)]
            [Display(Name = "Ghi Chú")]
            public string Note { get; set; }

        }
    }
}
