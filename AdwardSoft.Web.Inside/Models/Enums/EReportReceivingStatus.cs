using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EReportReceivingStatus
    {
        [Display(Name = "Wating")]
        [Description("Wating")]
        Wating = 0,
        [Display(Name = "Approved")]
        [Description("Approved")]
        Approved = 1,
        [Display(Name = "Trash")]
        [Description("Trash")]
        Trash = 2
    }
}
