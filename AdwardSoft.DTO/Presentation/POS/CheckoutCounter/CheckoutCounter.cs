using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CheckoutCounterDatatable))]
    public class CheckoutCounter
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public int BranchId { get; set; }

        // 0.Unavailable (Ngừng hoạt động)
        // 1.Available(Đang hoạt động)
        [ProtoMember(4)]
        public short Status { get; set; }
    }

    [ProtoContract]
    public class CheckoutCounterDatatable : CheckoutCounter
    {

    }

    public class CheckoutCounterRedis : CheckoutCounter
    {
        [AdsKey]
        public new int Id { get; set; }
        [AdsKey]
        public new int BranchId { get; set; }
    }
}
