using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EProductStatus
    {
        [Display(Name = "Mới tạo")]
        [Description("Mới khởi tạo")]
        Unavailable = 1,
        [Display(Name = "Hiển thị")]
        [Description("Hiển thị")]
        Display = 2,
        [Display(Name = "Tạm dừng")]
        [Description("Tạm dừng")]
        Pause = 3,
        [Display(Name = "Xóa")]
        [Description("Xóa")]
        Remove = 4

    }
}
