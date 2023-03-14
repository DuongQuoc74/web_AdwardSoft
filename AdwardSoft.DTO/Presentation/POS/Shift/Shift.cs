using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ShiftDatatable))]
    public class Shift
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public bool IsMonday { get; set; }
        [ProtoMember(4)]
        public bool IsTuesday { get; set; }
        [ProtoMember(5)]
        public bool IsWednesday { get; set; }
        [ProtoMember(6)]
        public bool IsThursday { get; set; }
        [ProtoMember(7)]
        public bool IsFriday { get; set; }
        [ProtoMember(8)]
        public bool IsSaturday { get; set; }
        [ProtoMember(9)]
        public bool IsSunday { get; set; }
        [ProtoMember(10)]
        public string StartTime { get; set; }
        [ProtoMember(11)]
        public string EndTime { get; set; }
        [ProtoMember(12)]
        public int BranchId { get; set; }
    }

    [ProtoContract]
    public class ShiftDatatable : Shift
    {

    }

    public class ShiftRedis : Shift
    {
        [AdsKey]
        public new int Id { get; set; }
        [AdsKey]
        public new int BranchId { get; set; }
    }
}
