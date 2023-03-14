using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CustomerGroupDatatable))]
    public class CustomerGroup
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Note { get; set; }
        [ProtoMember(4)]
        // 0. Wholesale Price(Giá bán sỉ)
        // 1. Retail Price(Giá bán lẻ)
        public short PricingPolicy { get; set; }
        // 0. Unavailable(Ngừng hoạt động)
        // 1. Available(Đang hoạt động)
        [ProtoMember(5)]
        public short Status { get; set; }
    }

    [ProtoContract]
    public class CustomerGroupDatatable : CustomerGroup
    {

    }


    public class CustomerGroupRedis : CustomerGroup
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
