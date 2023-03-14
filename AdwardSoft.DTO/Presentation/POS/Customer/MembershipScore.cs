using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class MembershipScore
    {
        [ProtoMember(1)]
        [AdsKey]
        public int CustomerId { get; set; }
        [ProtoMember(2)]
        [AdsKey]
        public string Year { get; set; }
        [ProtoMember(3)]
        [AdsKey]
        public string SaleReceiptId { get; set; }
        [ProtoMember(4)]
        public short Type { get; set; }
        [ProtoMember(5)]
        public decimal Amount { get; set; }
        [ProtoMember(6)]
        public decimal Point { get; set; }
    }
}
