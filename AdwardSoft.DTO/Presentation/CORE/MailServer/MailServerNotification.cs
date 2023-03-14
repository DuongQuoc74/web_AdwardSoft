using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class MailServerNotification
    {
        [ProtoMember(1)]
        public int MailServerId { get; set; }
        [ProtoMember(2)]
        public int NotificationTemplateId { get; set; }
        [ProtoMember(3)]
        public List<int> NotificationTemplateIds { get; set; }
    }
}
