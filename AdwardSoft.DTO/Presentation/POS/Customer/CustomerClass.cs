using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CustomerClassDatatable))]
    public class CustomerClass
    {
        [ProtoMember(1)]
        public int CustomerId { get; set; }

        [ProtoMember(2)]
        public int MembershipClassId { get; set; }

        [ProtoMember(3)]
        public DateTime Date { get; set; }

        [ProtoMember(4)]
        public int OldMembershipClassId { get; set; }
    }

    [ProtoContract]
    public class CustomerClassDatatable : CustomerClass
    {
        [ProtoMember(5)]
        public string CustomerName { get; set; }

        [ProtoMember(6)]
        public string CustomerPhone { get; set; }

        [ProtoMember(7)]
        public short CustomerType { get; set; }

        [ProtoMember(8)]
        public string CustomerGroupName { get; set; }

        [ProtoMember(9)]
        public string OldMembershipClassName { get; set; }

        [ProtoMember(10)]
        public string UpdateMembershipClassName { get; set; }

        [ProtoMember(11)]
        public int Count { get; set; }
    }

    public class CustomerClassRedis : CustomerClass
    {
        [AdsKey]
        public new int CustomerId { get; set; }
        [AdsKey]
        public new int MembershipClassId { get; set; }
        [AdsKey]
        public new int OldMembershipClassId { get; set; }
    }
}
