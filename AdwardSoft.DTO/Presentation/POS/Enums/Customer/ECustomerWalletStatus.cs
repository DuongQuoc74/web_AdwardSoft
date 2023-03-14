using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.DTO.Presentation.POS
{
    public enum ECustomerWalletStatus
    {
        [Display(Name = "Đang xử lý")]
        [Description("Đang xử lý")]
        Processing = 0,

        [Display(Name = "Thực hiện thành công")]
        [Description("Thực hiện thành công")]
        Succeed = 1,

        [Display(Name = "Từ chối")]
        [Description("Từ chối")]
        Denied = 2
    }
}
