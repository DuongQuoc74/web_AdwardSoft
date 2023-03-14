using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EEmployeeSalaryType
    {
        [Display(Name = "Net")]
        [Description("Net")]
        Net = 0,
        [Display(Name = "Gross")]
        [Description("Gross")]
        Gross = 1
    }
}
