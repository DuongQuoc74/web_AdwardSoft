using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class Setting
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string ProjectName { get; set; }
        [ProtoMember(3)]
        public string CompanyName { get; set; }
        [ProtoMember(4)]
        public string Address { get; set; }
        [ProtoMember(5)]
        public string Tel { get; set; }
        [ProtoMember(6)]
        public string Website { get; set; }
        [ProtoMember(7)]
        public decimal MapCoordinateLat { get; set; }
        [ProtoMember(8)]
        public decimal MapCoordinateLong { get; set; } 
        [ProtoMember(9)]
        public decimal ErrorRange { get; set; }
    }
    public class SettingRedis : Setting
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
