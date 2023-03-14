using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EUserStatus
    {
        [Display(Name = "Available")]
        Available = 0,
        [Display(Name = "Invisible")]
        Invisible = 1,
        [Display(Name = "Lock")]
        Lock = 2,
        [Display(Name = "Remove")]
        Remove = 3
    }
}
