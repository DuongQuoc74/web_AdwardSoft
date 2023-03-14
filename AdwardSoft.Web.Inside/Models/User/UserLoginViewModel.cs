using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class UserLoginViewModel
    {
        [ProtoMember(1)]
        [Display(Name = "UserName")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserName { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Password")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Remember me")]
        public bool IsRememberMe { get; set; }

        public int BranchId { get; set; }
    }

    [ProtoContract]
    public class UserOTPViewModel
    {
        [ProtoMember(1)]
        public string UserName { get; set; }
        [ProtoMember(2)]
        public string OTP { get; set; }
    }
}
