using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class SettingViewModel
    {
        [ProtoMember(1)]
        [Required]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Display(Name = "Tên dự án")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Tên dự án không hợp lệ")]
        [Required(ErrorMessage = "Tên dự án không được để trống")]
        [MaxLength(120)]
        public string ProjectName { get; set; }

        [ProtoMember(3)]
        [Display(Name = "Tên công ty")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Tên công ty không hợp lệ")]
        [Required(ErrorMessage = "Tên công ty không được để trống")]
        [MaxLength(120)]
        public string CompanyName { get; set; }

        [ProtoMember(4)]
        [Display(Name = "Địa chỉ")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Địa chỉ không hợp lệ")]
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [MaxLength(120)]
        public string Address { get; set; }

        [ProtoMember(5)]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [MaxLength(120)]
        public string Tel { get; set; }

        [Display(Name = "Website ")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Website không hợp lệ")]
        [Required(ErrorMessage = "Website không được để trống")]
        [MaxLength(120)]
        public string Website { get; set; }

        [ProtoMember(7)]
        [Remote(action: "Kiểm tra vĩ độ bản đồ", controller: "Setting"
            , ErrorMessage = "Tọa độ vĩ độ không chính xác")]
        [Display(Name = "Googlemap Lat")]
        public decimal MapCoordinateLat { get; set; }
        [ProtoMember(8)]
        [Remote(action: "Kiểm tra kinh độ bản đồ", controller: "Setting"
            , ErrorMessage = "Tọa độ kinh độ không chính xác")]
        [Display(Name = "Googlemap Long")]
        public decimal MapCoordinateLong { get; set; }
        [ProtoMember(9)]
        [Required]
        [Display(Name = "Phạm vi lỗi")]
        [Range(0,999, ErrorMessage = "Phạm vi lỗi không đúng !")]
        public decimal ErrorRange { get; set; }
    }
}
