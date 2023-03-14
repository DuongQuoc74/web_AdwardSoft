using ProtoBuf;
using System;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class ContactViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Email { get; set; }
        [ProtoMember(5)]
        public string Phone { get; set; }
        [ProtoMember(6)]
        public string Message { get; set; }
        [ProtoMember(7)]
        public short Status { get; set; }
    }
}
