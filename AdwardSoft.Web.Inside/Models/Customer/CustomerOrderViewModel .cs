using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CustomerOrderDatatableViewModel))]
    public class CustomerOrderViewModel
    {
        [ProtoMember(1)]
        [Display(Name = "Mã đơn hàng")]
        public string Id { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Ngày đặt hàng")]
        public DateTime Date { get; set; } = DateTime.Now;
        [ProtoMember(3)]
        [Display(Name = "Nhà phân phối")]
        public int CustomerId { get; set; }
        [ProtoMember(4)]
        public int BranchId { get; set; }
        [ProtoMember(5)]
        public string No { get; set; }
        [ProtoMember(6)]
        public string Description { get; set; }
        [ProtoMember(7)]
        [Required(ErrorMessage = "Thông tin bắt buộc nhập")]
        public DateTime? DeliveryDate { get; set; }
        [ProtoMember(8)]
        [Required(ErrorMessage = "Thông tin bắt buộc nhập")]
        public int DeliveryPointId { get; set; }
        [ProtoMember(9)]
        [Required(ErrorMessage = "Thông tin bắt buộc nhập")]
        public int DeliveryVehicleId { get; set; }
        [ProtoMember(10)]
        [Required(ErrorMessage = "Thông tin bắt buộc nhập")]
        [Display(Name = "Người nhận hàng")]
        public string Receiver { get; set; }
        [ProtoMember(11)]
        [Required(ErrorMessage = "Thông tin bắt buộc nhập")]
        public string ReceiverPhone { get; set; }
        [ProtoMember(12)]
        [Required(ErrorMessage = "Thông tin bắt buộc nhập")]
        public string ReceiverAddress { get; set; }
        [ProtoMember(13)]
        [Required(ErrorMessage = "Thông tin bắt buộc nhập")]
        public short DeliveryType { get; set; }
        [ProtoMember(14)]
        public string VoucherCode { get; set; }
        [ProtoMember(15)]
        [Display(Name = "Trạng thái đơn hàng")]
        public short Status { get; set; } = 0;
        [ProtoMember(16)]
        public bool IsShipping { get; set; }
        [ProtoMember(17)]
        [Display(Name = "Số lượng đăng ký")]
        public decimal TotalQuantityReg { get; set; }
        [ProtoMember(18)]
        [Display(Name = "Số lượng thực giao")]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(19)]
        public decimal SubTotal { get; set; }
        [ProtoMember(20)]
        public decimal ShippingFee { get; set; }
        [ProtoMember(21)]
        public decimal TaxRate { get; set; }
        [ProtoMember(22)]
        public decimal TaxFee { get; set; }
        [ProtoMember(23)]
        public decimal TotalDiscount { get; set; }
        [ProtoMember(24)]
        [Display(Name = "Thành tiền")]
        public decimal TotalAmount { get; set; }
        [ProtoMember(25)]
        public DateTime? PaymentDate { get; set; }
        [ProtoMember(26)]
        public long PaymentUser { get; set; }
        [ProtoMember(27)]
        [Display(Name = "Thanh toán")]
        public short PaymentStatus { get; set; }
        [ProtoMember(28)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [ProtoMember(29)]
        public long CreatedUser { get; set; }
        [ProtoMember(30)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        [ProtoMember(31)]
        public long ModifiedUser { get; set; }
        [ProtoMember(32)]
        public int PaymentMethodId { get; set; }
        [ProtoMember(33)]
        public List<CustomerOrderDetailDatatableViewModel> OrderDetail { get; set; } = new List<CustomerOrderDetailDatatableViewModel>();
    }

    [ProtoContract]
    public class CustomerOrderDatatableViewModel : CustomerOrderViewModel
    {
        [ProtoMember(101)]
        public string CustomerName { get; set; }
    }
}
