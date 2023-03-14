using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(StockDatatable))]
    public class Stock
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int BranchId { get; set; }

        [ProtoMember(4)]
        public string Description { get; set; }

        // 1. Inventory Tracking (Theo dõi tồn kho)
        // 2. No Inventory Tracking (Không theo dõi tồn kho)
        [ProtoMember(5)]
        public short Type { get; set; }

        [ProtoMember(6)]
        public bool IsDefault { get; set; }
        [ProtoMember(7)]
        public short StockIsUsing { get; set; }
    }

    [ProtoContract]
    public class StockDatatable : Stock
    {

    }

    public class StockRedis : Stock
    {
        [AdsKey]
        public new int Id { get; set; }
        [AdsKey]
        public new int BranchId { get; set; }
    }
    public class StockSelect
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
