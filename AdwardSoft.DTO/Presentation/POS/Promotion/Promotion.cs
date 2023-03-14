using ProtoBuf;
using System;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class Promotion
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public DateTime EffectiveDate { get; set; }
        [ProtoMember(4)]
        public DateTime ExpiryDate { get; set; }
        [ProtoMember(5)]
        public short Type { get; set; }
    }
}
