using Microsoft.AspNetCore.Http;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(BeginingStockDataTableViewModel))]
    [ProtoInclude(200, typeof(BeginingStockDataViewModel))]
    public class BeginingStockViewModel
    {
        [ProtoMember(1)]
        public string Year { get; set; }

        [ProtoMember(2)]
        public int StockId { get; set; }

        [ProtoMember(3)]
        public int ProductId { get; set; }

        [ProtoMember(4)]
        public decimal Quantity { get; set; }

        [ProtoMember(5)]
        public bool IsLock { get; set; } = false;

        [ProtoMember(6)]
        public int UnitId { get; set; }

        [ProtoMember(7)]
        public long UserId { get; set; }
    }

    [ProtoContract]
    public class BeginingStockDataTableViewModel : BeginingStockViewModel
    {
        [ProtoMember(8)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [ProtoMember(9)]
        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; }

        [ProtoMember(10)]
        [Display(Name = "Kho")]
        public string StockName { get; set; }

        [ProtoMember(11)]
        [Display(Name = "Đơn vị tính")]
        public string UnitName { get; set; }

        [ProtoMember(12)]
        public int Count { get; set; }

        [ProtoMember(13)]
        [Display(Name = "Ảnh sản phẩm")]
        public string ProductImage { get; set; }
    }

    [ProtoContract]
    public class BeginingStockDataViewModel : BeginingStockViewModel
    {
        [ProtoMember(8)]
        public string ProductCode { get; set; }

    }

    [ProtoContract]
    public class BeginingStockExcelViewModel
    {
        [Required]
        [Display(Name = "Tệp")]
        public IFormFile File { get; set; }

        [Required]
        [Display(Name = "Mã kho")]
        public int StockId { get; set; }

        [Required]
        [Display(Name = "Năm")]
        public string Year { get; set; }
    }
}
