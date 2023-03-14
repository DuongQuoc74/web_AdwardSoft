using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class PrivacyPolicy
    {
        [ProtoMember(1)]
        [AdsKey]
        public int Id { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        public short Type { get; set; }
        [ProtoMember(4)]
        public bool IsActive { get; set; }
        [ProtoMember(5)]
        [AdsKey]
        public string LanguageCode { get; set; }
        [ProtoMember(6)]
        public string Name { get; set; }
        [ProtoMember(7)]
        public string Content { get; set; }
        [ProtoMember(8)]
        public List<short> UserTypes { get; set; } = new List<short>();
    }

    [ProtoContract]
    public class PrivacyPolicyTrans
    {
        [ProtoMember(1)]
        public int PrivacyPolicyId { get; set; }
        [ProtoMember(2)]
        public string LanguageCode { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Content { get; set; }
    }

    [ProtoContract]
    public class PrivacyPolicyType
    {
        [ProtoMember(1)]
        public int PrivacyPolicyId { get; set; }
        [ProtoMember(2)]
        public short UserType { get; set; }
    }

    public class PrivacyPolicyRedis
    {
        [AdsKey]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [AdsKey]
        public short Type { get; set; }
        public bool IsActive { get; set; }
        [AdsKey]
        public string LanguageCode { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class PrivacyPolicyTypeRedis
    {
        [AdsKey]
        public int PrivacyPolicyId { get; set; }
        [AdsKey]
        public short UserType { get; set; }
    }
}
