using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EReligious
    {
        [Display(Name = "No")]
        [Description("No")]
        Personal = 0,
        [Display(Name = "Buddhism")]
        [Description("Buddhism")]
        Buddhism = 1,
        [Display(Name = "Catholic")]
        [Description("Catholic")]
        Catholic = 2,
        [Display(Name = "Other")]
        [Description("Other")]
        Other = 3
    }
}
