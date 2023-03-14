using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EPromotionAmountType
    {
        [Display(Name = "By value")]
        [Description("By value")]
        Value = 0,
        [Display(Name = "By rate")]
        [Description("By rate")]
        Rate = 1
    }
}
