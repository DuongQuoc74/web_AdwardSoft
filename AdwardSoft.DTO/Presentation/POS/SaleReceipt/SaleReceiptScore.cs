using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class SaleReceiptScore
    {
        [ProtoMember(1)]
        public string SaleReceiptId { get; set; }
        [ProtoMember(2)]
        public int ScoreConfigurationId { get; set; }
        [ProtoMember(3)]
        public decimal Point { get; set; }
        [ProtoMember(4)]
        public decimal Amount { get; set; }
    }
}
