using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class ReportReceiving
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        public string No { get; set; }
        [ProtoMember(4)]
        public string Supplier { get; set; }
        [ProtoMember(5)]
        public string Address { get; set; }
        [ProtoMember(6)]
        public string Phone { get; set; }
        [ProtoMember(7)]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(8)]
        public decimal TotalAmount { get; set; }
        [ProtoMember(9)]
        public short Status { get; set; }
        [ProtoMember(10)]
        public int Count { get; set; }
    }
}
