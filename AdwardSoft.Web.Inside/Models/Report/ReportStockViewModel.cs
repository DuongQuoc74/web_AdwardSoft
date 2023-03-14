using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class ReportStockViewModel
    {
        [ProtoMember(1)]
        public string ProductId { get; set; }
        [ProtoMember(2)]
        public string Barcode { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Unit { get; set; }
        [ProtoMember(5)]
        [Display(Name = "Begining Stock")]
        public decimal BeginingStock { get; set; }
        [ProtoMember(6)]
        [Display(Name = "Import Stock")]
        public decimal ImportStock { get; set; }
        [ProtoMember(7)]
        [Display(Name = "Export Stock")]
        public decimal ExportStock { get; set; }
        [ProtoMember(8)]
        public int Count { get; set; }
        [Display(Name = "Ending Stock")]
        public decimal EndingStock
        {
            get => BeginingStock + ImportStock - ExportStock;
        }

    }
}
