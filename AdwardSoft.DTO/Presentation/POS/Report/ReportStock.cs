using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class ReportStock
    {
        [ProtoMember(1)]
        public string ProductId { get; set; }
        [ProtoMember(2)]
        public string Barcode { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Unit { get; set; }
        [ProtoMember(5)]
        public decimal BeginingStock { get; set; }
        [ProtoMember(6)]
        public decimal ImportStock { get; set; }
        [ProtoMember(7)]
        public decimal ExportStock { get; set; }
        [ProtoMember(8)]
        public int Count { get; set; }
    }
}
