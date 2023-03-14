using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(BeginingStockDataTable))]
    [ProtoInclude(200, typeof(BeginingStockData))]
    public class BeginingStock
    {
        [ProtoMember(1)]
        public string Year { get; set; }

        [ProtoMember(2)]
        public int StockId { get; set; }

        [ProtoMember(3)]
        public int ProductId { get; set; }

        [ProtoMember(4)]
        public decimal Quantity { get; set; }

        [ProtoMember(5)]
        public bool IsLock { get; set; } = false;

        [ProtoMember(6)]
        public int UnitId { get; set; }

        [ProtoMember(7)]
        public long UserId { get; set; }
    }

    [ProtoContract]
    public class BeginingStockDataTable : BeginingStock
    {
        [ProtoMember(8)]
        public string ProductName { get; set; }

        [ProtoMember(9)]
        public string ProductCode { get; set; }

        [ProtoMember(10)]
        public string StockName { get; set; }

        [ProtoMember(11)]
        public string UnitName { get; set; }

        [ProtoMember(12)]
        public int Count { get; set; }

        [ProtoMember(13)]
        public string ProductImage { get; set; }
    }

    [ProtoContract]
    public class BeginingStockData : BeginingStock
    {
        [ProtoMember(8)]
        public string ProductCode { get; set; }
    }
}
