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
    [ProtoInclude(100, typeof(CustomerWalletDatatableViewModel))]
    public class CustomerWalletViewModel
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Ngày thực hiện")]
        public DateTime Date { get; set; }

        [ProtoMember(3)]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(300)]
        [Display(Name = "Nội dung")]
        public string Note { get; set; }

        [ProtoMember(4)]
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Tài khoản ngân hàng")]
        public int BankAccountId { get; set; }

        [ProtoMember(5)]
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Số tiền")]
        public decimal Amount { get; set; }

        [ProtoMember(6)]
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 0;

        [ProtoMember(7)]
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Nhà phân phối")]
        public int CustomerId { get; set; }

        [ProtoMember(8)]
        [Required(ErrorMessage = "Không được để trống")]
        public short Type { get; set; } = 0;
    }

    [ProtoContract]
    public class CustomerWalletDatatableViewModel : CustomerWalletViewModel
    {
        [ProtoMember(101)]
        [Display(Name = "Tài khoản")]
        public string BankNo { get; set; }

        [ProtoMember(102)]
        [Display(Name = "Ngân hàng")]
        public string BankName { get; set; }

        [ProtoMember(103)]
        [Display(Name = "Nhà phân phối")]
        public string CustomerName { get; set; }
    }
}
