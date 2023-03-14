using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EMenuType
    {
        [Display(Name = "Page")]
        Page = 0,
        [Display(Name = "Post")]
        Post = 1,
        [Display(Name = "Category")]
        Category = 2,
        [Display(Name = "Custom")]
        Custom = 3
    }
}
