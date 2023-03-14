using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class RoleViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required(ErrorMessage = "Trường dữ liệu không được để trống")]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required(ErrorMessage = "Trường dữ liệu không được để trống")]
        public string NormalizedName { get; set; }
        [ProtoMember(4)]
        public string ConcurrencyStamp { get; set; }
        [ProtoMember(5)]
        public bool IsDefault { get; set; }
        [ProtoMember(6)]
        public int UserType { get; set; }
        [ProtoMember(7)]
        public bool IsOTPVerification { get; set; }

        public List<PermissionViewModel> ListPermissions { get; set; }
        public List<int> Permissions { get; set; }
        

        public bool isChecked { get; set; }

    }

    [ProtoContract]
    public class RolePermissionViewModel
    {
        [ProtoMember(1)]
        public int RoleId { get; set; }
        [ProtoMember(2)]
        public int PermissionId { get; set; }
    }


    [ProtoContract]
    public class UserRoleViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string NormalizedName { get; set; }
        [ProtoMember(4)]
        public string ConcurrencyStamp { get; set; }
        [ProtoMember(5)]
        [Required(ErrorMessage = "Người dùng chưa được chọn")]
        public long UserId { get; set; }

        public List<UserInfoViewModel> Users { get; set; }

        public List<int> RolesId { get; set; }

        public List<RoleViewModel> Roles { get; set; }

    }

    [ProtoContract]
    public class UserRolesViewModel
    {
        [ProtoMember(1)]
        public long UserId { get; set; }
        [ProtoMember(2)]
        public List<int> Roles { get; set; }
        [ProtoMember(3)]
        public string RolesOfUser { get; set; }
        [ProtoMember(4)]
        public string FullName { get; set; }
    }

    [ProtoContract]
    public class RoleConfigViewModel
    {
        [ProtoMember(1)]
        public int RoleId { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Quy tắc đặt mật khẩu")]
        [Required(ErrorMessage = "Không được để trống")]
        public string PwdRegex { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Quy định xác thực tài khoản")]
        [Required(ErrorMessage = "Không được để trống")]
        public short VerificationMethod { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Theo dõi lịch sử hoạt động")]
        public bool IsLogging { get; set; }
    }
}
