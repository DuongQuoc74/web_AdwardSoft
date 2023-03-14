using Microsoft.AspNetCore.Http;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(StockDatatableViewModel))]
    public class StockViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Display(Name = "Tên kho")]
        [Required(ErrorMessage = "Tên kho bắt buộc nhập")]
        [MaxLength(60)]
        public string Name { get; set; }

        [ProtoMember(3)]
        [Display(Name = "Đơn vị")]
        [Required(ErrorMessage = "Đơn vị bắt buộc chọn")]
        public int BranchId { get; set; }

        [ProtoMember(4)]
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả bắt buộc nhập")]
        [MaxLength(100)]
        public string Description { get; set; }

        // 0. Inventory Tracking(Theo dõi tồn kho)
        // 1. No Inventory Tracking(Không theo dõi tồn kho)
        [ProtoMember(5)]
        [Display(Name = "Loại")]
        [Required(ErrorMessage = "Loại tồn kho bắt buộc chọn")]
        public short Type { get; set; }

        [ProtoMember(6)]
        [Required]
        [Display(Name = "Mặc định")]
        public bool IsDefault { get; set; }
        [ProtoMember(7)]
        public short StockIsUsing { get; set; }
    }

    [ProtoContract]
    public class StockDatatableViewModel : StockViewModel
    {

    }

    [ProtoContract]
    public class StockSelectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
