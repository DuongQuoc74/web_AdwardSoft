using AdwardSoft.DTO.Generic;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(BranchStock))]
    public class Branch
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Address { get; set; }
        [ProtoMember(4)]
        public string Tel { get; set; }
        [ProtoMember(5)]
        public string Email { get; set; }
        [ProtoMember(6)]
        public int ParentId { get; set; }
        [ProtoMember(7)]
        public int Sort { get; set; }
    }

    [ProtoContract]
    public class BranchStock : Branch
    {
        [ProtoMember(8)]
        public int StockId { get; set; }
        [ProtoMember(9)]
        public string StockName { get; set; }
    }

    [ProtoContract]
    public class BranchJson : JsonData
    {

    }
}
