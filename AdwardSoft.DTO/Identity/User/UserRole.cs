using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Identity
{
    [ProtoContract]
    public class UserRole
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string NormalizedName { get; set; }
        [ProtoMember(4)]
        public string ConcurrencyStamp { get; set; }
        [ProtoMember(5)]
        public long UserId { get; set; }
    }

    [ProtoContract]
    public class UserRoles
    {
        [ProtoMember(1)]
        public Int64 UserId { get; set; }
        [ProtoMember(2)]
        public List<int> Roles { get; set; }
        [ProtoMember(3)]
        public string RolesOfUser { get; set; }
        [ProtoMember(4)]
        public string FullName { get; set; }
    }
}
