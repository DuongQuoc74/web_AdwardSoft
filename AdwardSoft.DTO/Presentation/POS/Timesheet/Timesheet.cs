using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class Timesheet    {
        [ProtoMember(1)]
        public long UserId { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        public short Type { get; set; }
        [ProtoMember(4)]
        public string Reason{ get; set; }
        [ProtoMember(5)]
        public string StartTime { get; set; }
        [ProtoMember(6)]
        public string EndTime { get; set; }
        [ProtoMember(7)]
        public DateTime CreatedDate { get; set; }
    }
}
