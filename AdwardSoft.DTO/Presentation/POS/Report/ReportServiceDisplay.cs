using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class ReportServiceDisplay
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public DateTime DateFrom { get; set; }
        [ProtoMember(3)]
        public DateTime DateTo { get; set; }
        [ProtoMember(4)]
        public string Supplier { get; set; }
        [ProtoMember(5)]
        public int PaymentPeriod { get; set; }
        [ProtoMember(6)]
        public decimal Fee { get; set; }
        [ProtoMember(7)]
        public string Description { get; set; }
        [ProtoMember(8)]
        public int TotalSupplier { get; set; }
        [ProtoMember(9)]
        public decimal TotalAmount { get; set; }
        [ProtoMember(10)]
        public int Count { get; set; }
    }
}
