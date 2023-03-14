using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(MembershipClassDatatable))]
    public class MembershipClass
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public decimal LowestValue { get; set; }
        [ProtoMember(4)]
        public decimal HighestValue { get; set; }
        [ProtoMember(5)]
        public short Level { get; set; }
        [ProtoMember(6)]
        public bool IsDefault { get; set; } = false;
    }

    [ProtoContract]
    public class MembershipClassDatatable : MembershipClass
    {
    }

    [ProtoContract]
    public class MembershipClassSort
    {
        [ProtoMember(1)]
        public int SelectId { get; set; }
        [ProtoMember(2)]
        public bool IsUp { get; set; }
    }

    public class MembershipClassRedis : MembershipClass
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
