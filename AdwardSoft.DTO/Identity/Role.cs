using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Identity
{
    [ProtoContract]
    public class Role
    {
        [ProtoMember(1)]
        [AdsKey]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string NormalizedName { get; set; }
        [ProtoMember(4)]
        public string ConcurrencyStamp { get; set; }
        [ProtoMember(5)]
        public bool IsDefault { get; set; }
        [ProtoMember(6)]
        public int UserType { get; set; }
        [ProtoMember(7)]
        public bool IsOTPVerification { get; set; }
    }

    [ProtoContract]
    public class RolePermission
    {
        [ProtoMember(1)]
        public int RoleId { get; set; }
        [ProtoMember(2)]
        public int PermissionId { get; set; }
    }

    [ProtoContract]
    public class RoleConfig
    {
        [ProtoMember(1)]
        public int RoleId { get; set; }
        [ProtoMember(2)]
        public string PwdRegex { get; set; }
        [ProtoMember(3)]
        public short VerificationMethod { get; set; }
        [ProtoMember(4)]
        public bool IsLogging { get; set; }
    }

    [ProtoContract]
    public class RoleConfigDetail
    {
        [ProtoMember(1)]
        public int RoleId { get; set; }
        [ProtoMember(2)]
        public string PwdRegex { get; set; }
        [ProtoMember(3)]
        public short VerificationMethod { get; set; }
        [ProtoMember(4)]
        public bool IsLogging { get; set; }
        [ProtoMember(5)]
        public string RoleName { get; set; }
    }
}
