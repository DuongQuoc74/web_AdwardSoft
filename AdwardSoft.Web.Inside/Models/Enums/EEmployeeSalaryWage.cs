using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EEmployeeSalaryWage
    {
        [Display(Name = "Hourly wages")]
        [Description("Hourly wages")]
        Hourly = 0,
        [Display(Name = "Day’s wages")]
        [Description("Day’s wages")]
        Day = 1,
        [Display(Name = "Fixed wages")]
        [Description("Fixed wages")]
        Fixed = 2,
        [Display(Name = "Contractual wages")]
        [Description("Contractual wages")]
        Contractual = 3
    }
}
