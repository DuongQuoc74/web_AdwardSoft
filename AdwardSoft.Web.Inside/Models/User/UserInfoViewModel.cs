using ProtoBuf;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class UserInfoViewModel
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string UserName { get; set; }
        [ProtoMember(3)]
        public string Email { get; set; }
        [ProtoMember(4)]
        public string FullName { get; set; }
        [ProtoMember(5)]
        public string Avatar { get; set; }
        [ProtoMember(6)]
        public string Permissions { get; set; }
        [ProtoMember(7)]
        public string PhoneNumber { get; set; }
        [ProtoMember(8)]
        public string LetterAvatar { get; set; }
        [ProtoMember(9)]
        public short Type { get; set; }
        [ProtoMember(10)]
        public short Status { get; set; }
        [ProtoMember(11)]
        public int Gender { get; set; }
    }

    [ProtoContract]
    public class UserSelectViewModel
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string UserName { get; set; }
        [ProtoMember(3)]
        public string Email { get; set; }
        [ProtoMember(4)]
        public string FullName { get; set; }
        public bool isChecked { get; set; }
    }
}
