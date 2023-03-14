using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class HandleErrorViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Mã lỗi")]
        [Required(ErrorMessage = "Không được để trống")]
        public int StatusCode { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Ngôn ngữ")]
        public string LanguageCode { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Title { get; set; }
        [ProtoMember(5)]
        [Display(Name = "Thông báo")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Message { get; set; }
    }
}
