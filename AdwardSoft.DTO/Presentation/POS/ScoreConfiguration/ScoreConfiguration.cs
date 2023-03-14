using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ScoreConfigurationDatatable))]
    public class ScoreConfiguration
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public DateTime EffectiveDate { get; set; }
        [ProtoMember(3)]
        public double FromAmount { get; set; }
        [ProtoMember(4)]
        public double ToAmount { get; set; }
        [ProtoMember(5)]
        public double FromPoint { get; set; }
        [ProtoMember(6)]
        public double ToPoint { get; set; }
    }
    [ProtoContract]
    public class ScoreConfigurationDatatable : ScoreConfiguration
    {
        [ProtoMember(7)]
        public int Count { get; set; }
    }
    public class ScoreConfigurationRedis : ScoreConfiguration
    {
        [AdsKey]
        public new int Id { get; set; }
        public new DateTime EffectiveDate { get; set; }
        public new double FromAmount { get; set; }
        public new double ToAmount { get; set; }
        public new double FromPoint { get; set; }
        public new double ToPoint { get; set; }
    }
}
