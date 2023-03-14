using ProtoBuf;

namespace AdwardSoft.DTO.Generic
{
    [ProtoContract]
    public class StringProtobuf
    {
        [ProtoMember(1)]
        public string Data { get; set; }
    }
}
