using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EPricingPolicy
    {
        [Display(Name = "Wholesale Price")]
        [Description("Wholesale Price")]
        WholesalePrice = 0,
        [Display(Name = "Retail Price")]
        [Description("Retail Price")]
        RetailPrice = 1,
    }
}
