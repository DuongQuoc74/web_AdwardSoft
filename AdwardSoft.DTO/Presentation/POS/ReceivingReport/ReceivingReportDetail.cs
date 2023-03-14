using ProtoBuf;
using System.Collections.Generic;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class ReceivingReportDetail
    {
        [ProtoMember(1)]
        public string ReceivingReportDetailId { get; set; }
        [ProtoMember(2)]
        public string ReceivingReportId { get; set; }
        [ProtoMember(3)]
        public int ProductId { get; set; }
        [ProtoMember(4)]
        public decimal Quantity { get; set; }
        [ProtoMember(5)]
        public int UnitId { get; set; }
        [ProtoMember(6)]
        public decimal Price { get; set; }
        [ProtoMember(7)]
        public decimal Discount { get; set; }
        [ProtoMember(8)]
        public decimal Amount { get; set; }
        [ProtoMember(9)]
        public bool IsPromo { get; set; }
        [ProtoMember(10)]
        public List<Select> Units { get; set; }
        [ProtoMember(11)]
        public string ProductName { get; set; }
        [ProtoMember(12)]
        public string ProductCode { get; set; }
    }
}
