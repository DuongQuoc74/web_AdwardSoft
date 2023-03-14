using AdwardSoft.Utilities.Attributes;
using DevExpress.Xpo;
using ProtoBuf;
using System;

namespace AdwardSoft.DTO.Presentation.POS.RecieptSetting
{
    [ProtoContract]
    public class RecieptSetting
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Sign { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Prefix { get; set; }
        [ProtoMember(5)]
        public string Suffix { get; set; }
        [ProtoMember(6)]
        public int StartNo { get; set; }
        [ProtoMember(7)]
        public int NoC { get; set; }
        [ProtoMember(8)]
        public DateTime AppliedDate { get; set; }
        [ProtoMember(8)]
        public string Note { get; set; }
        public class RecieptSetingRedis : RecieptSetting
        {
            [AdsKey]
            public new int Id { get; set; }
        }

    }

}