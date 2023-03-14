using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(StockTakingDatatable))]
    [ProtoInclude(200, typeof(StockTakingData))]
    public class StockTaking
    {
        [ProtoMember(1)]
        public int StockId { get; set; }
        [ProtoMember(2)]
        public int ProductId { get; set; }
        [ProtoMember(3)]
        public DateTime Date { get; set; }
        [ProtoMember(4)]
        public float Quantity { get; set; }
        [ProtoMember(5)]
        public bool IsLock { get; set; } = false;
        [ProtoMember(6)]
        public bool IsForward { get; set; } = false;
        [ProtoMember(7)]
        public int UnitId { get; set; }
        [ProtoMember(8)]
        public long UserId { get; set; }
    }
    [ProtoContract]
    public class StockTakingDatatable : StockTaking
    {
        [ProtoMember(9)]
        public string ProductName { get; set; }
        [ProtoMember(10)]
        public string ProductCode { get; set; }
        [ProtoMember(11)]
        public string StockName { get; set; }
        [ProtoMember(12)]
        public string UnitName { get; set; }
        [ProtoMember(13)]
        public int Count { get; set; }
    }

    [ProtoContract]
    public class StockTakingData : StockTaking
    {
        [ProtoMember(9)]
        public string ProductCode { get; set; }
    }
}
