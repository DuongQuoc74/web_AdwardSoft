using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class TimesheetLog
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(1)]
        public long UserId { get; set; }
        [ProtoMember(1)]
        public DateTime Date { get; set; }
        [ProtoMember(1)]
        public int Type { get; set; }
        [ProtoMember(1)]
        public decimal MapCoordinateLong { get; set; }
        [ProtoMember(1)]
        public decimal MapCoordinateLat { get; set; }
    }
}
