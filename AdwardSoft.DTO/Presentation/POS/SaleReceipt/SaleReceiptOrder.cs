using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class SaleReceiptOrder
    {
        [ProtoMember(1)]
        public string SaleReceiptId { get; set; }
        [ProtoMember(2)]
        public string CustomerOrderId { get; set; }
    }
}
