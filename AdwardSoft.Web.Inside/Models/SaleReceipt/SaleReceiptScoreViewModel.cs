using ProtoBuf;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class SaleReceiptScoreViewModel
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
