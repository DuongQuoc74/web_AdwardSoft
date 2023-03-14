using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EEmployeeStatus
    {
        [Display(Name = "Available")]
        [Description("Hoạt động")]
        Available = 0,
        [Display(Name = "Unavailable")]
        [Description("Không hoạt động")]
        Unavailable = 1
    }
}
