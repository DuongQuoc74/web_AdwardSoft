using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EUserType
    {
        [Display(Name = "Nội bộ")]
        Internal = 1,
        [Display(Name = "Khách hàng")]
        Customer = 2
    }
}
