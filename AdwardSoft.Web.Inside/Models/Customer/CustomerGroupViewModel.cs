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
    [ProtoInclude(100, typeof(CustomerGroupDatatableViewModel))]
    public class CustomerGroupViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Required(ErrorMessage = "Loại hình không được để trống")]
        [MaxLength(60)]
        [Display(Name = "Loại hình phân phối")]
        public string Name { get; set; }

        [ProtoMember(3)]
        [MaxLength(100)]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        // 0. Wholesale Price(Giá bán sỉ)
        // 1. Retail Price(Giá bán lẻ)
        [Display(Name = "Pricing policy")]
        [ProtoMember(4)]
        public short PricingPolicy { get; set; } = 0;

        // 0. Unavailable(Ngừng hoạt động)
        // 1. Available(Đang hoạt động)
        [ProtoMember(5)]
        [Display(Name = "Trạng thái")]
        [Remote(action: "Check", controller: "CustomerGroup", AdditionalFields = "Id")]
        public short Status { get; set; } = 1;

    }

    [ProtoContract]
    public class CustomerGroupDatatableViewModel : CustomerGroupViewModel
    {

    }
}
