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
    [ProtoInclude(100, typeof(StockTakingDatatableViewModel))]
    [ProtoInclude(200, typeof(StockTakingDataViewModel))]
    public class StockTakingViewModel
    {
        [ProtoMember(1)]
        public int StockId { get; set; }
        [ProtoMember(2)]
        public int ProductId { get; set; }
        [ProtoMember(3)]
        public DateTime Date { get; set; }
        [ProtoMember(4)]
        public float Quantity { get; set; }
        [ProtoMember(5)]
        public bool IsLock { get; set; }
        [ProtoMember(6)]
        public bool IsForward { get; set; }
        [ProtoMember(7)]
        public int UnitId { get; set; }
        [ProtoMember(8)]
        public long UserId { get; set; }
    }

    [ProtoContract]
    public class StockTakingDatatableViewModel : StockTakingViewModel
    {
        [ProtoMember(9)]
        public string ProductName { get; set; }
        [ProtoMember(10)]
        public string ProductCode { get; set; }
        [ProtoMember(11)]
        public string StockName { get; set; }
        [ProtoMember(12)]
        public string UnitName { get; set; }
        [ProtoMember(13)]
        public int Count { get; set; }
    }

    [ProtoContract]
    public class StockTakingDataViewModel : StockTakingViewModel
    {
        [ProtoMember(9)]
        public string ProductCode { get; set; }

        [ProtoIgnore]
        public string DateStr { get; set; }
    }

    [ProtoContract]
    public class StockTakingExcelViewModel
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public int StockId { get; set; }

        [Required]
        public string DateStr { get; set; }
    }

}
