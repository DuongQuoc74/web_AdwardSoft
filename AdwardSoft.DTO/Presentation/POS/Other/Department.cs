using AdwardSoft.DTO.Generic;
using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class Department
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public int ParentId { get; set; }
        [ProtoMember(5)]
        public int Sort { get; set; }
    }

    [ProtoContract]
    public class DepartmentJson : JsonData
    {

    }

    public class DepartmentRedis : Department
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
