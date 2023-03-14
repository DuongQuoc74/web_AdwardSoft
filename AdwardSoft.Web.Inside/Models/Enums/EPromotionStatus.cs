using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EPromotionStatus
    {
        [Display(Name = "Expired ")]
        [Description("Expired ")]
        Available = 1,
        [Display(Name = "Valid")]
        [Description("Valid")]
        Unavailable = 0
    }
}
