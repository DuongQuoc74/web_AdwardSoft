using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum ENotificationTemplateAction
    {
        [Display(Name = "Đăng nhập")]
        SignIn = 0,
        [Display(Name = "Đăng ký")]
        SignUp = 1,
        [Display(Name = "Change password")]
        ChangePassword = 2,
        [Display(Name = "Cập nhật thông tin tài khoản")]
        UpdateProfile = 3,
        [Display(Name = "Chặn tài khoản")]
        BlockAccount = 4,
        [Display(Name = "Trạng thái xử lý hợp đồng")]
        ContractStatus = 5,
    }

    public enum ENotificationTemplateType
    {
        [Display(Name = "Email")]
        Email = 0,
        [Display(Name = "SMS")]
        SMS = 1,
        [Display(Name = "Mobile")]
        Mobile = 2
    }
}
