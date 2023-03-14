using ProtoBuf;
using System;
using System.Collections.Generic;

namespace AdwardSoft.DTO.Identity
{
    [ProtoContract]
    public class User 
    {
        public User()
        {
            this.Claims = new HashSet<Claim>();
            this.Roles = new HashSet<Role>();
        }

        [ProtoMember(1)]
        public virtual long Id { get; set; }
        [ProtoMember(2)]
        public virtual string UserName { get; set; }
        [ProtoMember(3)]
        public virtual string NormalizedUserName { get; set; }
        [ProtoMember(4)]
        public virtual string Email { get; set; }
        [ProtoMember(5)]
        public string NormalizedEmail { get; set; }
        [ProtoMember(6)]
        public bool EmailConfirmed { get; set; }
        [ProtoMember(7)]
        public virtual String PasswordHash { get; set; }
        [ProtoMember(8)]
        public virtual string SecurityStamp { get; set; }
        [ProtoMember(9)]
        public string ConcurrencyStamp { get; set; }
        [ProtoMember(10)]
        public virtual string PhoneNumber { get; set; }
        [ProtoMember(11)]
        public bool PhoneNumberConfirmed { get; set; }
        [ProtoMember(12)]
        public bool TwoFactorEnabled { get; set; }
        [ProtoMember(13)]
        public DateTime LockoutEndDateUtc { get; set; }
        [ProtoMember(14)]
        public bool LockoutEnabled { get; set; }
        [ProtoMember(15)]
        public int AccessFailedCount { get; set; }
        [ProtoMember(16)]
        public string FullName { get; set; }
        [ProtoMember(17)]
        public string Avatar { get; set; }
        [ProtoMember(18)]
        public short Type { get; set; }
        [ProtoMember(19)]
        public short Status { get; set; }
        [ProtoMember(20)]
        public int Gender { get; set; }
        [ProtoMember(21)]
        public string IdentityNumber { get; set; }
        public virtual ICollection<Claim> Claims { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

    }

    [ProtoContract]
    public class UserChangePassword
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string OldPassword { get; set; }
        [ProtoMember(3)]
        public string NewPassword { get; set; }
    }


    public class UserOTP : User
    {
        public string OTP { get; set; }
        public string ReferralCodeUser { get; set; }
    }
}
