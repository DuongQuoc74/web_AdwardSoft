using ProtoBuf;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class SaleReceiptOrderViewModel
    {
        [ProtoMember(1)]
        public string SaleReceiptId { get; set; }
        [ProtoMember(2)]
        public string CustomerOrderId { get; set; }
    }
}
