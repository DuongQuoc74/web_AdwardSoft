using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CustomerDatatableViewModel))]
    public class CustomerViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Required(ErrorMessage = "Hình thức thanh toán không được để trống")]
        [Display(Name = "Hình thức thanh toán")]
        public int PaymentMethodId { get; set; }

        [ProtoMember(3)]
        [Required(ErrorMessage = "Loại hình không được để trống")]
        [Display(Name = "Loại hình")]
        public int CustomerGroupId { get; set; }

        [ProtoMember(4)]
        [Required(ErrorMessage = "Tên nhà phân phối không được để trống")]
        [StringLength(120)]
        [Display(Name = "Tên nhà phân phối")]
        public string Name { get; set; }

        [ProtoMember(5)]
       // [Required]
        [Display(Name = "Số GPKD")]
        [StringLength(12)]
        [Remote(action: "CheckIdentity", controller: "Customer", AdditionalFields = "Id")]
        public string IdentityCode { get; set; }

        // 0. Personal – Cá nhân
        // 1. Organization – Tổ chức
        [ProtoMember(6)]
        [Display(Name = "Phân loại")]
        public short Type { get; set; }

        [ProtoMember(7)]
        [Required(ErrorMessage = "Điện thoại không được để trống")]
        [StringLength(12)]
        [Remote(action: "CheckPhone", controller: "Customer", AdditionalFields = "Id")]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [ProtoMember(8)]
        [StringLength(254)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ProtoMember(9)]
        [Required(ErrorMessage ="Địa chỉ không được để trống")]
        [StringLength(128)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [ProtoMember(10)]
        [Display(Name = "Hóa đơn")]
        public bool IsInvoice { get; set; } = false;

        [ProtoMember(11)]
        [StringLength(100)]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        // 0. Unavailable(Ngừng hoạt động)
        // 1. Available(Đang hoạt động))
        [ProtoMember(12)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 1;

        [ProtoMember(13)]
        [Display(Name = "Mặc định")]
        public bool IsDefault { get; set; } = false;

        [ProtoMember(14)]
        [Display(Name = "Giới tính")]
        public int GenderId { get; set; }

        [ProtoMember(15)]
        [Display(Name = "Ngày sinh")]
        public DateTime DoB { get; set; }

        [ProtoIgnore]
        [Display(Name = "Ngày sinh")]
        public string DoBStr { get; set; }
        [ProtoMember(16)]
        public string Tag { get; set; }
        [ProtoMember(17)]
        [Display(Name = "Hạn mức tín dụng")]
        public decimal CreditLimit { get; set; }
        [ProtoMember(18)]
        [Required(ErrorMessage = "Hạn thanh toán không được để trống")]
        [Display(Name = "Hạn thanh toán")]
        public int PaymentTerm { get; set; }
        [ProtoMember(19)]
        [Required(ErrorMessage = "Kho hàng mặc định không được để trống")]
        [Display(Name = "Kho hàng")]
        public int StockId { get; set; }
        [ProtoMember(20)]
        [Display(Name = "Người đại diện")]
        public string Representative { get; set; }
        [ProtoMember(21)]
        [Display(Name = "Số điện thoại")]
        public string RepPhone { get; set; }
        [ProtoMember(22)]
        [Display(Name = "Người điều hành")]
        public string Operator { get; set; }
        [ProtoMember(23)]
        [Display(Name = "Số điện thoại")]
        public string OpePhone { get; set; }
        [ProtoMember(24)]
        [Display(Name = "Bản đồ")]
        public string Map { get; set; }
        [ProtoMember(25)]
        public long UserId { get; set; }

    }

    [ProtoContract]
    public class CustomerDatatableViewModel : CustomerViewModel
    {
        [ProtoMember(101)]
        public int Count { get; set; }

        [ProtoMember(102)]
        [Display(Name = "Loại hình")]
        public string CustomerGroupName { get; set; }

        [ProtoMember(103)]
        [Display(Name = "Giới tính")]
        public string GenderName { get; set; }
        [ProtoMember(104)]
        [Display(Name = "Hình thức thanh toán")]
        public string PaymentMethodName { get; set; }
        [ProtoMember(105)]
        [Display(Name = "Kho hàng")]
        public string StockName { get; set; }
    }

    [ProtoContract]
    public class CustomerElastic
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Phone { get; set; }
        [ProtoMember(4)]
        public string IdentityCode { get; set; }
        [ProtoMember(5)]
        public int PaymentMethodId { get; set; }
        public string Text
        {
            get
            {
                return Name;
            }
        }
    }

    [ProtoContract]
    public class CustomerSearch
    {
        [ProtoMember(1)]
        public long Total { get; set; }
        [ProtoMember(2)]
        public List<CustomerElastic> Items { get; set; }
    }
}
