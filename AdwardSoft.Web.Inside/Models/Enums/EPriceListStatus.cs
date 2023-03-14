using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EPriceListStatus
    {

        [Display(Name = "Chờ duyệt")]
        [Description("Chờ duyệt")]
        Pending = 2,
        [Display(Name = "Hoạt động")]
        [Description("Hoạt động")]
        Available = 1,
        [Display(Name = "Không khả dụng")]
        [Description("Không khả dụng")]
        Unavailable = 0

    }
}
