using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum ECustomerWalletStatus
    {
        [Display(Name = "Đang xử lý")]
        [Description("Đang xử lý")]
        Processing = 0,

        [Display(Name = "Thực hiện thành công")]
        [Description("Thực hiện thành công")]
        Succeed = 1,

        [Display(Name = "Từ chối")]
        [Description("Từ chối")]
        Denied = 2
    }
}
