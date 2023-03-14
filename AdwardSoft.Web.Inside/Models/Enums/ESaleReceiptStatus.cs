using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum ESaleReceiptStatus
    {
        [Display(Name = "Invoice was created")]
        [Description("Invoice was created")]
        Created = 0,
        [Display(Name = "Invoice is canceled")]
        [Description("Invoice is canceled")]
        Canceled = 1
    }
}
