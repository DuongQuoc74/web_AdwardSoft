using AdwardSoft.DTO.Generic;
using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class Module
    {
        [ProtoMember(1)]
        [AdsKey]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(3)]
        public string Link { get; set; }
        [ProtoMember(4)]
        public string ClassName { get; set; }
        [ProtoMember(5)]
        public string ControllerName { get; set; }
        [ProtoMember(6)]
        public int ParentId { get; set; }
        [ProtoMember(7)]
        public int Sort { get; set; }
        [ProtoMember(8)]
        public bool IsPublic { get; set; }
        [ProtoMember(9)]
        public string Types { get; set; }
    }

    [ProtoContract]
    public class ModuleSort : JsonData
    {
    }

    [ProtoContract]
    public class ModuleType
    {
        public int ModuleId { get; set; }
        public short Type { get; set; }
    }
}
