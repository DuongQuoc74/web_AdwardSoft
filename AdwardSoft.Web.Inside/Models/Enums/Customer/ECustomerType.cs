﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum ECustomerType
    {
        [Display(Name = "Tổ chức")]
        [Description("Tổ chức")]
        Personal = 0,

        [Display(Name = "Cá nhân")]
        [Description("Cá nhân")]
        Organization = 1
    }
}
