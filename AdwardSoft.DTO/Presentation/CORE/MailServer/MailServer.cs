using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class MailServer
    {
        [ProtoMember(1)]
        [AdsKey]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Email { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string SMTP { get; set; }
        [ProtoMember(5)]
        public bool IsSSL { get; set; }
        [ProtoMember(6)]
        public bool IsDefaultCredential { get; set; }
        [ProtoMember(7)]
        public int Port { get; set; }
        [ProtoMember(8)]
        public string Pwd { get; set; }
    }

    [ProtoContract]
    public class MailServerDetail
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Email { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public string SMTP { get; set; }

        [ProtoMember(5)]
        public bool IsSSL { get; set; }

        [ProtoMember(6)]
        public bool IsDefaultCredential { get; set; }

        [ProtoMember(7)]
        public int Port { get; set; }

        [ProtoMember(8)]
        public string Pwd { get; set; }

        [ProtoMember(9)]
        public short Type { get; set; }

        [ProtoMember(10)]
        public short Action { get; set; }

        [ProtoMember(11)]
        public string Title { get; set; }

        [ProtoMember(12)]
        public string Content { get; set; }

        [ProtoMember(13)]
        public bool IsActive { get; set; }
    }
}
