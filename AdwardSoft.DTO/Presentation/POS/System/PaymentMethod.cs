using AdwardSoft.Utilities.Attributes;
using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class PaymentMethod
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Icon { get; set; }
    }

    [ProtoContract]
    public class PaymentMethodRedis : PaymentMethod
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
