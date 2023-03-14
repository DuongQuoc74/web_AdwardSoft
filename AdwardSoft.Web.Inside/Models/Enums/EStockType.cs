using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EStockType
    {
        [Display(Name = "Theo dõi tồn kho")]
        [Description("Theo dõi tồn kho")]
        InventoryTracking = 0,
        [Display(Name = "Không theo dõi tồn kho")]
        [Description("Không theo dõi tồn kho")]
        NoInventoryTracking = 1
    }
}
