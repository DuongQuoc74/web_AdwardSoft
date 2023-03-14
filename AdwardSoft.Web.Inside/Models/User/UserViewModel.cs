using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    
    public class UserInsertAndInfoViewModel
    {
        public UserInsertViewModel UserInfo { get; set; } = new UserInsertViewModel { };
    }

    [ProtoContract]
    public class UserViewModel
    {
        [ProtoMember(1)]
        public long Id { get; set; }

        [ProtoMember(2)]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [DisplayName("Tên tài khoản")]
        [MaxLength(256, ErrorMessage = "Không được vượt quá 256 ký tự")]
        public string Username { get; set; }

        [ProtoMember(3)]
        [Required]
        [DisplayName("Email")]
        [MaxLength(256, ErrorMessage = "Không được vượt quá 256 ký tự")]
        public string Email { get; set; }

        [ProtoMember(4)]
        [DisplayName("Tên đầy đủ")]
        [MaxLength(128, ErrorMessage = "Không được vượt quá 128 ký tự")]
        public string FullName { get; set; }

        [ProtoMember(5)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DisplayName("Mật khẩu")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        public string Password { get; set; }

        [ProtoMember(6)]
        [DisplayName("Số điện thoại")]
        [MaxLength(12, ErrorMessage = "Không được vượt quá 12 ký tự")]
        public string Phone { get; set; }

        [ProtoMember(7)]
        public int Status { get; set; }

        [ProtoMember(8)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        [DisplayName("Mật khẩu cũ")]
        // [Remote("IsCorrectPassword", "User", HttpMethod= "POST", ErrorMessage = "Mật khẩu cũ không chính xác")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [ProtoMember(9)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [DisplayName("Mật khẩu mới")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [ProtoMember(10)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu xác nhận")]
        [DisplayName("Mật khẩu xác nhận")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string RepeatPassword { get; set; }

        [ProtoMember(11)]
        [Required]
     
        public string Avatar { get; set; }

        [ProtoMember(12)]
        [Required]

        public int Gender { get; set; }
    }

    [ProtoContract]
    public class UserInsertViewModel
    {
        [ProtoMember(1)]
        public virtual long Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [MaxLength(256, ErrorMessage = "Not over 256 character")]
        [DisplayName("User Name")]
        public virtual string UserName { get; set; }
        [ProtoMember(3)]
        [MaxLength(256, ErrorMessage = "Not over 256 character")]
        public virtual string NormalizedUserName { get; set; }
        [ProtoMember(4)]
        [Required]
        [EmailAddress(ErrorMessage = "Email is invalid ")]
        [Remote("IsAlreadySigned", "User", ErrorMessage = "Email is used")]
        [MaxLength(256, ErrorMessage = "Not over 256 character")]
        public virtual string Email { get; set; }
        [ProtoMember(5)]
        [MaxLength(256, ErrorMessage = "Not over 256 character")]
        public string NormalizedEmail { get; set; }
        [ProtoMember(6)]
        public bool EmailConfirmed { get; set; }
        [ProtoMember(7)]
        [Required]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Minimum 8 character")]
        [DisplayName("Password")]
        public virtual string PasswordHash { get; set; }
        [ProtoMember(8)]
        public virtual string SecurityStamp { get; set; }
        [ProtoMember(9)]
        public string ConcurrencyStamp { get; set; }
        [ProtoMember(10)]
        [Required]
        [StringLength(12, ErrorMessage = "Not over 12 character")]
        [DisplayName("Phone Number")]
        //[EmailAddress(ErrorMessage = "Phone is invalid ")]
        //[Remote("IsAlreadySignedName", "User", ErrorMessage = "Phone is used")]
        public virtual string PhoneNumber { get; set; }
        [ProtoMember(11)]
        public bool PhoneNumberConfirmed { get; set; }
        [ProtoMember(12)]
        public bool TwoFactorEnabled { get; set; }
        [ProtoMember(13)]
        public DateTime LockoutEndDateUtc { get; set; }
        [ProtoMember(14)]
        public bool LockoutEnabled { get; set; }
        [ProtoMember(15)]
        public int AccessFailedCount { get; set; }
        [ProtoMember(16)]
        [Required]
        [StringLength(128, ErrorMessage = "Not over 128 character")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [ProtoMember(17)]
        public string Avatar { get; set; }
        [ProtoMember(18)]
        public short Type { get; set; } = 1;
        [ProtoMember(19)]
        public short Status { get; set; } = 0;
        [ProtoMember(20)]
        public int Gender { get; set; }
        [ProtoMember(21)]
        public string IdentityNumber { get; set; }
        [Required]
        [Compare("PasswordHash", ErrorMessage = "Password not correct!")]
        [DisplayName("Password Confirmed")]
        public string PasswordConfirmed { get; set; }
        public IFormFile FileImage { get; set; }

        [Display(Name = "Branch")]
        public string[] Branchs { get; set; }
    }




}
