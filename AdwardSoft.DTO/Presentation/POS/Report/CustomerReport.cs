using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class CustomerReport
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public string CustomerPhone { get; set; }
        [ProtoMember(3)]
        public string CustomerName { get; set; }
        [ProtoMember(4)]
        public decimal TotalAmount { get; set; }
        [ProtoMember(5)]
        public decimal ExchangeAmount { get; set; }
        [ProtoMember(6)]
        public int ExchangePoint { get; set; }
        [ProtoMember(7)]
        public decimal BalanceAmount { get; set; }
        [ProtoMember(8)]
        public int BalancePoint { get; set; }
        [ProtoMember(9)]
        public int Count { get; set; }
        [ProtoMember(10)]
        public int TotalPoint { get; set; }
    }
}
