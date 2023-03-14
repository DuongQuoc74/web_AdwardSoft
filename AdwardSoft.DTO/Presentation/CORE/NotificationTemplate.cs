using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class NotificationTemplate
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public short Type { get; set; }
        [ProtoMember(3)]
        public short Action { get; set; }       
        [ProtoMember(4)]
        public string Title { get; set; }
        [ProtoMember(5)]
        public string Content { get; set; }
        [ProtoMember(6)]
        public bool IsActive { get; set; }
        [ProtoMember(7)]
        public int MailServerId { get; set; }
    }
}
