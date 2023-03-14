using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AdwardSoft.DTO.Presentation.POS
{
    public enum ECustomerOrderPaymentStatus
    {
        [Display(Name = "Trạng thái thanh toán")]
        [Description("Chưa thanh toán")]
        PAYMENT_STATUS_UNPAY = 0,

        [Display(Name = "Trạng thái thanh toán")]
        [Description("Đã thanh toán")]
        PAYMENT_STATUS_PAID = 1,

        [Display(Name = "Trạng thái thanh toán")]
        [Description("Từ chối")]
        PAYMENT_STATUS_DECLINE = 2
    }

    public enum ECustomerOrderStatus
    {
        // 0 : Hoàn thành đặt hàng, 1 : Đang xử lý, 2 : Đã giao hàng, 3 : Hoàn thành đơn hàng, 4: Tạm hủy
        [Display(Name = "Trạng thái đơn hàng")]
        [Description("Hoàn thành đặt hàng")]
        STATUS_DONE = 0,

        [Display(Name = "Trạng thái đơn hàng")]
        [Description("Đang xử lý")]
        STATUS_INPROGRESS = 1,

        [Display(Name = "Trạng thái đơn hàng")]
        [Description("Đã giao hàng")]
        STATUS_DELIVERED = 2,

        [Display(Name = "Trạng thái đơn hàng")]
        [Description("Hoàn thành đơn hàng")]
        STATUS_COMPLETE = 3,

        [Display(Name = "Trạng thái đơn hàng")]
        [Description("Tạm hủy")]
        STATUS_CANCELLED = 4,
    }
}
