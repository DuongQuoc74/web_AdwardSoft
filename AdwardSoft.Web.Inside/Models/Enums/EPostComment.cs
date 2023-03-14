using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EPostComment
    {
        [Display(Name = "Đang chờ xử lý")]
        Pending = 0,
        [Display(Name = "Đã xác nhận")]
        Published = 1,
        [Display(Name = "Spam")]
        Spam = 2
    }
}
