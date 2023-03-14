using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EDeliveryType
    {
        [Display(Name = "Đường thủy")]
        [Description("Dường thủy")]
        Waterway = 0,
        [Display(Name = "Đường bộ")]
        [Description("Đường bộ")]
        Road = 1,
        [Display(Name = "Đường hàng không")]
        [Description("Đường hàng không")]
        Airline = 2,
        [Display(Name = "Đường sắt")]
        [Description("Đường sắt")]
        Railway = 3
    }
}
