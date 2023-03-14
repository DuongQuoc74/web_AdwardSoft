using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ServiceDisplayData))]
    public class ServiceDisplay
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public int SupplierId { get; set; }
        [ProtoMember(4)]
        public DateTime Date { get; set; } = DateTime.Now;
        [ProtoMember(5)]
        public DateTime DateFrom { get; set; } = DateTime.Now;
        [ProtoMember(6)]
        public DateTime DateTo { get; set; } = DateTime.Now;
        [ProtoMember(7)]
        public string Description { get; set; }
        [ProtoMember(8)]
        public decimal Fee { get; set; }
        [ProtoMember(9)]
        public short PaymentPeriod { get; set; }
    }
    [ProtoContract]
    public class ServiceDisplayData : ServiceDisplay
    {
        [ProtoMember(10)]
        public int Count { get; set; }

        [ProtoMember(11)]
        public string Supplier { get; set; }
    }

    public class ServiceDisplayRedis : ServiceDisplay
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
