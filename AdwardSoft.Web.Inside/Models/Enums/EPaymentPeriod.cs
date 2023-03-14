using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EPaymentPeriod
    {
        [Display(Name = "Month")]
        [Description("Month")]
        Month = 0,
        [Display(Name = "Quater")]
        [Description("Quater")]
        Quater = 1,
        [Display(Name = "Year")]
        [Description("Year")]
        Year = 2,
    }
}
