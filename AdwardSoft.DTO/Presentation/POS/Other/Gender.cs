using AdwardSoft.Utilities.Attributes;
using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class Gender
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
    }

    public class GenderRedis : Gender
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
