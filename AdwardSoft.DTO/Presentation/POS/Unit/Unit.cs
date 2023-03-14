using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class Unit
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Code { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        // 0.Unavailable 
        // 1.Available  
        [ProtoMember(4)]
        public short Status { get; set; } 
    }

    public class UnitRedis : Unit
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
