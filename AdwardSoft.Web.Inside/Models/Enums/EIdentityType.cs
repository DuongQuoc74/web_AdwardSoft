using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EIdentityType : Int32
    {
        [Display(Name = "CMND")]
        CMND = 0,
        [Display(Name = "CCCD")]
        CCCD = 1,
        [Display(Name = "Passport")]
        Passport = 2
    }
}
