using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public enum EText
    {
        [Display(Name = "Create")]
        [Description("Thêm mới")]
        Create,
        [Display(Name = "Update")]
        [Description("Cập nhật")]
        Update,
        [Display(Name = "Remove")]
        [Description("Gỡ bỏ")]
        Remove,
        [Display(Name = "Delete")]
        [Description("Xóa")]
        Delete,
        [Display(Name = "Edit")]
        [Description("Chỉnh sửa")]
        Edit,
        [Display(Name = "Cancel")]
        [Description("Hủy")]
        Cancel,
        [Display(Name = "Clear")]
        [Description("Xóa tất cả")]
        Clear,
        [Display(Name = "EmptyTable")]
        [Description("Không có dữ liệu")]
        EmptyTable,
        [Display(Name = "Currency")]
        [Description("VNĐ")]
        Currency,
        [Display(Name = "IsDefault")]
        [Description("<i class='icon-checkmark3'></i>")]
        IsDefault,
        [Display(Name = "TooltipUpdate")]
        [Description("Cập nhật")]
        TooltipUpdate,
        [Display(Name = "TooltipApproval")]
        [Description("Duyệt")]
        TooltipApproval,
        [Display(Name = "TooltipEdit")]
        [Description("Chỉnh sửa")]
        TooltipEdit,
        [Display(Name = "TooltipRemove")]
        [Description("Xóa")]
        TooltipRemove,
        [Display(Name = "TooltipDelete")]
        [Description("Xóa")]
        TooltipDelete,
        [Display(Name = "TooltipCreate")]
        [Description("Thêm mới")]
        TooltipCreate,
        [Display(Name = "TooltipUp")]
        [Description("Up")]
        TooltipUp,
        [Display(Name = "TooltipDown")]
        [Description("Down")]
        TooltipDown,
        [Display(Name = "Processing")]
        [Description("Chờ trong giây lát!!!")]
        Processing,
        [Display(Name = "Sync")]
        [Description("Đồng bộ")]
        Sync,
        [Display(Name = "Print")]
        [Description("In")]
        Print, 
        [Display(Name = "Detail")]
        [Description("Chi tiết")]
        Detail
    }
}
