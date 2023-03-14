using AdwardSoft.Utilities.Attributes;
using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class Position
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
    }

    public class PositionRedis : Position
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
