using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Identity
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SelectLevels))]
    [ProtoInclude(200, typeof(SelectDefault))]
    public class Select
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Text { get; set; }
    }

    [ProtoContract]
    public class SelectLevels : Select
    {
        [ProtoMember(3)]
        public int Level { get; set; }
    }

    [ProtoContract]
    public class SelectDefault : Select
    {
        [ProtoMember(3)]
        public bool IsDefault { get; set; }
    }
}
