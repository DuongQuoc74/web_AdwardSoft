using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EPromotionType
    {
        [Display(Name = "Donation with purchase")]
        [Description("Donation with purchase")]
        DonationPurchase = 0,
        [Display(Name = "Discount on total value")]
        [Description("Discount on total value")]
        DiscountValue = 1,
        [Display(Name = "Discount on product")]
        [Description("Discount on product")]
        DiscountProduct = 2,
        [Display(Name = "Discount for combo")]
        [Description("Discount for combo")]
        DiscountCombo = 3
    }
}
